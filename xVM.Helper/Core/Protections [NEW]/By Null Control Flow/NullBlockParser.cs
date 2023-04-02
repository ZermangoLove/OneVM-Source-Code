using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections
{
	internal static class NullBlockParser
    {
        public static List<NullBlock> Block(MethodDef method)
		{
			var list = new List<NullBlock>();
			new List<Instruction>(method.Body.Instructions);
            var block = new NullBlock();
			int num = 0;
			int num2 = 0;
			block.Number = num;
			block.Instructions.Add(Instruction.Create(OpCodes.Nop));
			list.Add(block);
			block = new NullBlock();
			var stack = new Stack<ExceptionHandler>();
			foreach (Instruction instruction in method.Body.Instructions)
			{
				foreach (ExceptionHandler exceptionHandler in method.Body.ExceptionHandlers)
				{
					if (exceptionHandler.HandlerStart == instruction || exceptionHandler.TryStart == instruction || exceptionHandler.FilterStart == instruction)
					{
						stack.Push(exceptionHandler);
					}
				}
				foreach (ExceptionHandler exceptionHandler2 in method.Body.ExceptionHandlers)
				{
					if (exceptionHandler2.HandlerEnd == instruction || exceptionHandler2.TryEnd == instruction)
					{
						stack.Pop();
					}
				}
				int num3;
				int num4;
				instruction.CalculateStackUsage(out num3, out num4);
				block.Instructions.Add(instruction);
				num2 += num3 - num4;
				if (num3 == 0 && instruction.OpCode != OpCodes.Nop && (num2 == 0 || instruction.OpCode == OpCodes.Ret) && stack.Count == 0)
				{
					num = (block.Number = num + 1);
					list.Add(block);
					block = new NullBlock();
				}
			}
			return list;
		}
	}
}