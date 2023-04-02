using System;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.DynCipher;
using xVM.Helper.DynCipher.AST;
using xVM.Helper.DynCipher.Generation;

using xVM.Helper.Core.Services;
using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core.AntiTamperEXEC
{
	internal class DynamicDeriver{
		StatementBlock derivation;
		Action<uint[], uint[]> encryptFunc;

		public void Init(RandomGenerator random) {
			StatementBlock dummy;
			new DynCipherService().GenerateCipherPair(random, out derivation, out dummy);

			var dmCodeGen = new DMCodeGen(typeof(void), new[] {
				Tuple.Create("{BUFFER}", typeof(uint[])),
				Tuple.Create("{KEY}", typeof(uint[]))
			});
			dmCodeGen.GenerateCIL(derivation);
			encryptFunc = dmCodeGen.Compile<Action<uint[], uint[]>>();
		}

		public uint[] DeriveKey(uint[] a, uint[] b) {
			var ret = new uint[0x10];
			Buffer.BlockCopy(a, 0, ret, 0, a.Length * sizeof(uint));
			encryptFunc(ret, b);
			return ret;
		}

		public IEnumerable<Instruction> EmitDerivation(MethodDef method, Local dst, Local src) {
			var ret = new List<Instruction>();
			var codeGen = new CodeGen(dst, src, method, ret);
			codeGen.GenerateCIL(derivation);
			codeGen.Commit(method.Body);
			return ret;
		}

		class CodeGen : CILCodeGen {
			readonly Local block;
			readonly Local key;

			public CodeGen(Local block, Local key, MethodDef method, IList<Instruction> instrs)
				: base(method, instrs) {
				this.block = block;
				this.key = key;
			}

			protected override Local Var(Variable var) {
				if (var.Name == "{BUFFER}")
					return block;
				if (var.Name == "{KEY}")
					return key;
				return base.Var(var);
			}
		}
	}
}