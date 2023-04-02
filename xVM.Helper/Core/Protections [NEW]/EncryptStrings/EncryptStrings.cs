using dnlib.DotNet;
using dnlib.DotNet.Emit;
using xVM.Helper.Core.Helpers.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    public class EncryptStrings
    {
		public void Execute(ModuleDefMD moduleDefMd)
		{
			ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(DecryptStringsHelper).Module);
			TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(typeof(DecryptStringsHelper).MetadataToken));
			MethodDef method = InjectHelper.Inject(typeDef, moduleDefMd.GlobalType, moduleDefMd).SingleOrDefault<IDnlibDef>() as MethodDef;
			Random random = new Random();
			foreach (TypeDef typeDef2 in from x in moduleDefMd.GetTypes() where x.HasMethods select x)
			{
				foreach (MethodDef methodDef in typeDef2.Methods.Where((MethodDef x) => x.HasBody))
				{
					IList<Instruction> instructions = methodDef.Body.Instructions;
					for (int i = 0; i < instructions.Count; i++)
					{
						if (instructions[i].OpCode == OpCodes.Ldstr && !string.IsNullOrEmpty(instructions[i].Operand.ToString()))
						{
							int num = random.Next(10, 30);
							string operand = EncryptString(new Tuple<string, int>(instructions[i].Operand.ToString(), num));
							instructions[i].OpCode = OpCodes.Ldstr;
							instructions[i].Operand = operand;
							instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(num));
							instructions.Insert(i + 2, OpCodes.Call.ToInstruction(method));
							i += 2;
						}
					}
					methodDef.Body.SimplifyMacros(methodDef.Parameters);
				}
			}
		}

		private static string EncryptString(Tuple<string, int> values)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int item = values.Item2;
			foreach (char c in values.Item1)
			{
				stringBuilder.Append((char)((int)c ^ item));
			}
			return stringBuilder.ToString();
		}
	}
}
