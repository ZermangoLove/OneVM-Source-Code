using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using xVM.Helper.Core.Helpers;

using xVM.Helper.Core.Protections.Generator.Context;

namespace xVM.Helper.Core.Protections
{
    public static class NullControlFlow
    {
        private static xVM.Helper.Core.Protections.Generator.Context.Generator generator = new xVM.Helper.Core.Protections.Generator.Context.Generator();

        public static void Execute(MethodDef method)
        {
            method.Body.SimplifyMacros(method.Parameters);
            var blocks = NullBlockParser.Block(method);
            blocks = Randomize(blocks);
            method.Body.Instructions.Clear();
            var local = new Local(method.Module.CorLibTypes.IntPtr);
            method.Body.Variables.Add(local);
            var target = Instruction.Create(OpCodes.Nop);
            var instr = Instruction.Create(OpCodes.Br, target);
            foreach (var instruction in Calc(0))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
            method.Body.Instructions.Add(target);
            foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                foreach (var instruction in Calc(block.Number))
                    method.Body.Instructions.Add(instruction);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                Instruction instruction4 = Instruction.Create(OpCodes.Nop);
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));

                foreach (var instruction in block.Instructions)
                    method.Body.Instructions.Add(instruction);

                foreach (var instruction in Calc(block.Number + 1))
                    method.Body.Instructions.Add(instruction);

                method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                method.Body.Instructions.Add(instruction4);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
            foreach (var instruction in Calc(blocks.Count - 1))
                method.Body.Instructions.Add(instruction);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
            method.Body.Instructions.Add(instr);

            foreach (var lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                method.Body.Instructions.Add(lastBlock);

            method.Body = ControlFlowGraph.Construct(method.Body).Body;
        }

        private static List<NullBlock> Randomize(List<NullBlock> input)
        {
            var ret = new List<NullBlock>();
            foreach (var group in input)
                ret.Insert(new Random().Next(0, ret.Count), group);
            return ret;
        }

        private static List<Instruction> Calc(int value)
        {
            var instructions = new List<Instruction> { Instruction.Create(OpCodes.Ldc_I4, value) };
            return instructions;
        }

        private static void AddJump(IList<Instruction> instrs, Instruction target)
        {
            instrs.Add(Instruction.Create(OpCodes.Br, target));
        }
    }
}