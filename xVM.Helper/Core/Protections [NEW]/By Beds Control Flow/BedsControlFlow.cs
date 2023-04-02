using System;
using System.Diagnostics;

using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace xVM.Helper.Core.Protections
{
	public class BedsControlFlow
    {
        public static void Execute(MethodDef method) {

            CFHelper cFHelper = new CFHelper();
            if (method.HasBody && method.Body.Instructions.Count > 0)
            {
                if (Simplify(method))
                {
                    CTFLWBlockss blocks = cFHelper.GetBlocks(method);
                    if (blocks.blocks.Count != 1)
                    {
                        Run(cFHelper, method, blocks);
                    }
                    Optimize(method);
                }
            }
        }

        public static void Execute_NoOptimize_NoBlock(MethodDef method)
        {
            CFHelper cFHelper = new CFHelper();
            if (method.HasBody && method.Body.Instructions.Count > 0)
            {
                CTFLWBlockss blocks = cFHelper.GetBlocks(method);
                if (blocks.blocks.Count != 1)
                {
                    Run(cFHelper, method, blocks);
                }
            }
        }

        private static bool Simplify(MethodDef methodDef)
        {
            if (methodDef.Parameters == null)
                return false;
            methodDef.Body.SimplifyMacros(methodDef.Parameters);
            return true;
        }

        private static bool Optimize(MethodDef methodDef)
        {
            if (methodDef.Body == null)
                return false;
            methodDef.Body.OptimizeMacros();
            methodDef.Body.OptimizeBranches();
            return true;
        }

        private static void Run(CFHelper cFHelper, MethodDef method, CTFLWBlockss blocks)
        {
            blocks.Scramble(out blocks);
            method.Body.Instructions.Clear();
            Local local = new Local(method.Module.CorLibTypes.Int32);
            method.Body.Variables.Add(local);
            Instruction target = Instruction.Create(OpCodes.Nop);
            Instruction instr = Instruction.Create(OpCodes.Br, target);
            foreach (Instruction instruction in cFHelper.Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.blocks)
            {
                if (block != blocks.getBlock((blocks.blocks.Count - 1)))
                {
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                    foreach (Instruction instruction in cFHelper.Calc(block.ID))
                        method.Body.Instructions.Add(instruction);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                    Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));
                    foreach (Instruction instruction in block.instructions)
                        method.Body.Instructions.Add(instruction);
                    foreach (Instruction instruction in cFHelper.Calc(block.nextBlock))
                        method.Body.Instructions.Add(instruction);

                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                    method.Body.Instructions.Add(instruction4);
                }
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (Instruction instruction in cFHelper.Calc(blocks.blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.getBlock((blocks.blocks.Count - 1)).instructions[0]));
            method.Body.Instructions.Add(instr);
            foreach (Instruction lastBlock in blocks.getBlock((blocks.blocks.Count - 1)).instructions)
                method.Body.Instructions.Add(lastBlock);

        }

    }
}