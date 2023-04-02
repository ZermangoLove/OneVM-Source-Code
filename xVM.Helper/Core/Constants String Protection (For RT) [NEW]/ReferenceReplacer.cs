using System;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core.Constants
{
	internal class ReferenceReplacer
	{
		public static void ReplaceReference(CEContext ctx)
		{
			foreach (var entry in ctx.ReferenceRepl)
			{
				ReplaceNormal(entry.Key, entry.Value);
			}
		}

		private static void ReplaceNormal(MethodDef method, List<Tuple<Instruction, uint, IMethod>> instrs)
		{
			foreach (var instr in instrs)
			{
                int i = method.Body.Instructions.IndexOf(instr.Item1);
				instr.Item1.OpCode = OpCodes.Ldc_I4;
				instr.Item1.Operand = (int)instr.Item2;
				method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Call, instr.Item3));
			}
		}
    }
}
