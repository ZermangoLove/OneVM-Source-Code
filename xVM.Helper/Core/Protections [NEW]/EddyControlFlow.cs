using System;
using System.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.RT.Renamer;
using xVM.Helper.Core.Helpers.System;

using OpCode = dnlib.DotNet.Emit.OpCode;
using ReflOpCode = System.Reflection.Emit.OpCode;
using OpCodes = dnlib.DotNet.Emit.OpCodes;
using ReflOpCodes = System.Reflection.Emit.OpCodes;

namespace xVM.Helper.Core.Protections
{
    public class EddyControlFlow
    {
        private static Dictionary<MethodDef, Tuple<int[], int[]>> obfMethods;

        public static void Execute(MethodDef method)
        {
            obfMethods = CreateMethods(method.Module);

            if (method.HasBody && method.Body.Instructions.Count > 0)
            {
                method.Body.SimplifyBranches();
                MethodDef methodd = method as MethodDef;
                var body = method.Body;
                body.SimplifyBranches();
                body.MaxStack++;
                List<Instruction> instructions = body.Instructions.ToList();
                List<HBlock> Hblocks = new List<HBlock>();
                List<HBlock> obfHBlocks = new List<HBlock>();
                //if (body.HasExceptionHandlers)
                //{
                //    foreach (ExceptionHandler eh in body.ExceptionHandlers)
                //    {
                //        ExceptionHandlerType HType = eh.HandlerType;
                //        List<Instruction> HInstr = new List<Instruction>();
                //        int HTStart = Array.IndexOf(instructions.ToArray(), eh.TryStart);
                //        int HTEnd = Array.IndexOf(instructions.ToArray(), eh.TryEnd);
                //        if (eh.TryEnd == null) HTEnd = instructions.Count - 1;
                //        for (int i = HTStart; i < HTEnd; i++)
                //            HInstr.Add(instructions[i]);
                //        Hblocks.Add(new HBlock() { instructions = HInstr });
                //        HInstr.Clear();
                //        int HCStart = Array.IndexOf(instructions.ToArray(), eh.HandlerStart);
                //        int HCEnd = Array.IndexOf(instructions.ToArray(), eh.HandlerEnd);
                //        if (eh.HandlerEnd == null) HCEnd = instructions.Count - 1;
                //        for (int i = HCStart; i < HCEnd; i++)
                //            HInstr.Add(instructions[i]);
                //        Hblocks.Add(new HBlock() { instructions = HInstr });
                //    }
                //    foreach (HBlock Hblock in Hblocks)
                //        obfHBlocks.Add(ObfuscateHBlock(Hblock, true));
                //}
                //else
                obfHBlocks.Add(ObfuscateHBlock(method.Module, new HBlock() { instructions = instructions }, false));
                body.Instructions.Clear();
                foreach (HBlock hBlock in obfHBlocks)
                {
                    foreach (Instruction instr in hBlock.instructions)
                        body.Instructions.Add(instr);
                }
                body.UpdateInstructionOffsets();
                body.SimplifyBranches();

                foreach (var m in obfMethods)
                {
                    var mm = m.Key;
                    //new MutationProtection().StartMutate(mm,mm.Module,0);
                    //new MutationProtection().Mutate3(mm);
                    //new MutationProtection().Mutate1(mm);
                }
            }
        }

        private static HBlock ObfuscateHBlock(ModuleDef module, HBlock HB, bool isHBlock)
        {
            List<BBlock> bBlocks = new List<BBlock>();
            List<Instruction> instructions = HB.instructions;
            Instruction firstBr = Instruction.Create(OpCodes.Br, instructions[0]);
            BBlock mainBlock = new BBlock() { instructions = new List<Instruction>(), fakeBranches = new List<Instruction>(), branchOrRet = new List<Instruction>() };
            int stack = 0;
            int push, pop;
            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction instr = instructions[i];
                instr.CalculateStackUsage(out push, out pop);
                stack += (push - pop);
                if (instr.OpCode == OpCodes.Ret)
                {
                    mainBlock.branchOrRet.Add(instr);
                    bBlocks.Add((BBlock)mainBlock.Clone());
                    mainBlock.Clear();
                }
                else
                    if (stack == 0 && instr.OpCode.OpCodeType != dnlib.DotNet.Emit.OpCodeType.Prefix)
                {
                    MethodDef obfMethod = obfMethods.Keys.ToArray()[new Random().Next(0, 4)];
                    mainBlock.instructions.Add(instr);
                    if (new Random().Next(0, 2) == 0)
                    {
                        mainBlock.branchOrRet.Add(Instruction.CreateLdcI4(obfMethods[obfMethod].Item2[new Random().Next(0, 4)]));
                        mainBlock.branchOrRet.Add(Instruction.Create(OpCodes.Call, module.Import(obfMethod)));
                        mainBlock.branchOrRet.Add(Instruction.Create(OpCodes.Brfalse, instructions[i + 1]));
                    }
                    else
                    {
                        mainBlock.branchOrRet.Add(Instruction.CreateLdcI4(obfMethods[obfMethod].Item1[new Random().Next(0, 4)]));
                        mainBlock.branchOrRet.Add(Instruction.Create(OpCodes.Call, module.Import(obfMethod)));
                        mainBlock.branchOrRet.Add(Instruction.Create(OpCodes.Brtrue, instructions[i + 1]));
                    }
                    bBlocks.Add((BBlock)mainBlock.Clone());
                    mainBlock.Clear();
                }
                else
                    mainBlock.instructions.Add(instr);
            }
            /*if (instructions.Count != bBlocks.Sum(a => a.instructions.Count) + 1)
            {
                throw new Exception("Did you delete any instruction?");
            }*/
            int[] position;
            bBlocks = Shuffle<BBlock>(bBlocks, out position);
            int index = Array.IndexOf(position, position.Length - 1);
            BBlock lastB = bBlocks[position.Length - 1];
            BBlock tempB;
            tempB = bBlocks[index];
            bBlocks[index] = lastB;
            bBlocks[position.Length - 1] = tempB;
            if (isHBlock)
            {
                int index2 = Array.IndexOf(position, 0);
                BBlock firstB = bBlocks[0];
                BBlock tempB2;
                tempB2 = bBlocks[index2];
                bBlocks[index2] = firstB;
                bBlocks[0] = tempB2;
            }
            foreach (BBlock block in bBlocks)
            {
                if (block.branchOrRet[0].OpCode != OpCodes.Ret)
                {
                    MethodDef obfMethod = obfMethods.Keys.ToArray()[new Random().Next(0, 4)];
                    int rr = new Random().Next(0, bBlocks.Count);
                    while (bBlocks[rr].instructions.Count == 0)
                        rr = new Random().Next(0, bBlocks.Count);
                    if (new Random().Next(0, 2) == 0)
                    {
                        block.fakeBranches.Add(Instruction.CreateLdcI4(obfMethods[obfMethod].Item1[new Random().Next(0, 4)]));
                        block.fakeBranches.Add(Instruction.Create(OpCodes.Call, module.Import(obfMethod)));
                        block.fakeBranches.Add(Instruction.Create(OpCodes.Brfalse, bBlocks[rr].instructions[0]));
                    }
                    else
                    {
                        block.fakeBranches.Add(Instruction.CreateLdcI4(obfMethods[obfMethod].Item2[new Random().Next(0, 4)]));
                        block.fakeBranches.Add(Instruction.Create(OpCodes.Call, module.Import(obfMethod)));
                        block.fakeBranches.Add(Instruction.Create(OpCodes.Brtrue, bBlocks[rr].instructions[0]));
                    }
                }
            }
            List<Instruction> bInstrs = new List<Instruction>();
            foreach (BBlock B in bBlocks)
            {
                bInstrs.AddRange(B.instructions);
                if (new Random().Next(0, 2) == 0)
                {
                    if (B.branchOrRet.Count != 0) bInstrs.AddRange(B.branchOrRet);
                    if (B.fakeBranches.Count != 0) bInstrs.AddRange(B.fakeBranches);
                }
                else
                {
                    if (B.fakeBranches.Count != 0) bInstrs.AddRange(B.fakeBranches);
                    if (B.branchOrRet.Count != 0) bInstrs.AddRange(B.branchOrRet);
                }
                if (B.afterInstr != null)
                    bInstrs.Add(B.afterInstr);

            }
            if (!isHBlock)
                bInstrs.Insert(0, firstBr);
            return new HBlock() { instructions = bInstrs };
        }

        private static List<T> Shuffle<T>(List<T> array, out int[] position)
        {
            var rand = new Random();
            List<KeyValuePair<int, T>> list = new List<KeyValuePair<int, T>>();
            foreach (T s in array)
                list.Add(new KeyValuePair<int, T>(rand.Next(), s));
            var sorted = from item in list
                         orderby item.Key
                         select item;
            T[] result = new T[array.Count];
            int index = 0;
            foreach (KeyValuePair<int, T> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            List<int> positions = new List<int>();
            for (int i = 0; i < array.Count; i++)
                positions.Add(Array.IndexOf(array.ToArray(), result[i]));
            position = positions.ToArray();
            return result.ToList();
        }

        private static Dictionary<MethodDef, Tuple<int[], int[]>> CreateMethods(ModuleDef loadedMod)
        {
            DynamicCode code = new DynamicCode(3);
            int[] modules = new int[4];
            for (int i = 0; i < modules.Length; i++)
                modules[i] = new Random().Next(2, 25);
            Instruction[,] methods = new Instruction[4, 10];
            for (int i = 0; i < 4; i++)
            {
                Instruction[] methodBody = code.Create();
                for (int y = 0; y < methodBody.Length; y++)
                    methods[i, y] = methodBody[y];
            }

            List<Tuple<Instruction[], Tuple<int, Tuple<int[], int[]>>>> InstrToInt =
                           new List<Tuple<Instruction[], Tuple<int, Tuple<int[], int[]>>>>();

            for (int i = 0; i < 4; i++)
            {
                List<Instruction> instr = new List<Instruction>();
                int[] numbersTrue = new int[5];
                int[] numbersFalse = new int[5];
                for (int y = 0; y < 10; y++)
                    instr.Add(methods[i, y]);
                for (int y = 0; y < 5; y++)
                    numbersTrue[y] = code.RandomNumberInModule(instr.ToArray(), modules[i], true);
                for (int y = 0; y < 5; y++)
                    numbersFalse[y] = code.RandomNumberInModule(instr.ToArray(), modules[i], false);
                InstrToInt.Add(Tuple.Create(instr.ToArray(), Tuple.Create(modules[i], Tuple.Create(numbersTrue, numbersFalse))));
            }
            Dictionary<MethodDef, Tuple<int[], int[]>> final = new Dictionary<MethodDef, Tuple<int[], int[]>>();
            MethodAttributes methFlags = MethodAttributes.Public | MethodAttributes.Static
                | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
            MethodImplAttributes methImplFlags = MethodImplAttributes.IL | MethodImplAttributes.Managed;
            for (int i = 0; i < 4; i++)
            {
                MethodDef methodDefs1 = new MethodDefUser(
                                     string.Empty,
                                     MethodSig.CreateStatic(loadedMod.CorLibTypes.Boolean, loadedMod.CorLibTypes.Int32),
                                     methImplFlags, methFlags);
                methodDefs1.Name = new NameService().NewName(new RandomGenerator().NextString());
                methodDefs1.Body = new CilBody();
                methodDefs1.ParamDefs.Add(new ParamDefUser("lol", 0));
                List<Instruction> preInstr = new List<Instruction>(InstrToInt[i].Item1);
                int module = InstrToInt[i].Item2.Item1;
                //preInstr.RemoveAt(preInstr.Count - 1);
                preInstr.Insert(preInstr.Count - 1, Instruction.CreateLdcI4(module));
                preInstr.Insert(preInstr.Count - 1, OpCodes.Rem.ToInstruction());
                preInstr.Insert(preInstr.Count - 1, Instruction.CreateLdcI4(0));
                preInstr.Insert(preInstr.Count - 1, Instruction.Create(OpCodes.Ceq));
                //preInstr.Insert(preInstr.Count - 1, Instruction.Create(OpCodes.Ret));
                foreach (var item in preInstr)
                    methodDefs1.Body.Instructions.Add(item);
                final.Add(methodDefs1, InstrToInt[i].Item2.Item2);
            }

            TypeDef type1 = new TypeDefUser(string.Empty, string.Empty, loadedMod.CorLibTypes.Object.TypeDefOrRef);
            type1.Name = new NameService().NewName(new RandomGenerator().NextString());
            type1.Attributes = dnlib.DotNet.TypeAttributes.Public | dnlib.DotNet.TypeAttributes.AutoLayout |
            dnlib.DotNet.TypeAttributes.Class | dnlib.DotNet.TypeAttributes.AnsiClass;
            loadedMod.Types.Add(type1);
            foreach (var item in final)
                type1.Methods.Add(item.Key);
            return final;
        }


        private class HBlock : ICloneable
        {
            public List<Instruction> instructions;

            public void Clear()
            {
                instructions = new List<Instruction>();
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        private class BBlock : ICloneable
        {
            public List<Instruction> instructions;
            public List<Instruction> branchOrRet;
            public Instruction afterInstr;
            public List<Instruction> fakeBranches;

            public void Clear()
            {
                instructions = new List<Instruction>();
                branchOrRet = new List<Instruction>();
                afterInstr = null;
                fakeBranches = new List<Instruction>();
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        private class DynamicCode
        {
            int intensity;
            delegate int Result();
            Random r;

            public DynamicCode(int intensity)
            {
                this.intensity = intensity;
                r = new Random();
            }

            public Instruction[] Create()
            {
                int positionValue = r.Next(0, intensity);
                List<Instruction> instructions = new List<Instruction>();
                instructions.Add(OpCodes.Ldc_I4.ToInstruction(r.Next()));
                instructions.Add(OpCodes.Ldc_I4.ToInstruction(r.Next()));
                for (int i = 0; i < intensity; i++)
                {
                    instructions.Add(getRandomOperation().ToInstruction());
                    if (positionValue == i)
                        instructions.Add(OpCodes.Ldarg_0.ToInstruction());
                    else
                        instructions.Add(OpCodes.Ldc_I4.ToInstruction(r.Next()));
                }
                instructions.Add(getRandomOperation().ToInstruction());
                instructions.Add(OpCodes.Ret.ToInstruction());
                return instructions.ToArray();
            }

            public int RandomNumberInModule(Instruction[] instructions, int module, bool divisible)
            {
                int Rnum = module * r.Next(1, 12);
                Rnum = divisible ? Rnum : Rnum + 1;
                int x = 0;
                List<Instruction> instsx = new List<Instruction>();
                while (instructions[x].OpCode != OpCodes.Ldarg_0)
                {
                    instsx.Add(instructions[x]);
                    x++;
                }
                instsx.Add(OpCodes.Ret.ToInstruction());
                int valuesx = DynamicCode.Emulate(instsx.ToArray(), 0);
                List<Instruction> instdx = new List<Instruction>();
                instdx.Add(OpCodes.Ldc_I4.ToInstruction(Rnum));
                for (int i = instructions.Length - 2; i > x + 2; i -= 2)
                {
                    Instruction operation = ReverseOperation(instructions[i].OpCode).ToInstruction();
                    Instruction value = instructions[i - 1];
                    instdx.Add(value);
                    instdx.Add(operation);
                }
                instdx.Add(Instruction.Create(OpCodes.Ret));
                int valuedx = DynamicCode.Emulate(instdx.ToArray(), 0);
                Instruction ope = ReverseOperation(instructions[x + 1].OpCode).ToInstruction();
                List<Instruction> final = new List<Instruction>();
                final.Add(OpCodes.Ldc_I4.ToInstruction(valuedx));
                final.Add(OpCodes.Ldc_I4.ToInstruction(valuesx));
                final.Add(ope.OpCode == OpCodes.Add ? OpCodes.Sub.ToInstruction() : ope);
                final.Add(OpCodes.Ret.ToInstruction());
                int finalValue = DynamicCode.Emulate(final.ToArray(), 0);
                return ope.OpCode == OpCodes.Add ? (finalValue * -1) : finalValue;
            }

            public static int Emulate(Instruction[] code, int value)
            {
                DynamicMethod emulatore = new DynamicMethod("MER ? BUULHE", typeof(int), null);
                ILGenerator il = emulatore.GetILGenerator();
                foreach (Instruction instr in code)
                {
                    if (instr.OpCode == OpCodes.Ldarg_0)
                        il.Emit(ReflOpCodes.Ldc_I4, value);
                    else if (instr.Operand != null)
                        il.Emit(instr.OpCode.ToReflectionOp(), Convert.ToInt32(instr.Operand));
                    else
                        il.Emit(instr.OpCode.ToReflectionOp());
                }
                Result ris = (Result)emulatore.CreateDelegate(typeof(Result));
                return ris.Invoke();
            }

            private OpCode getRandomOperation()
            {
                OpCode operation = null;
                switch (r.Next(0, 3))
                {
                    case 0: operation = OpCodes.Add; break;
                    case 1: operation = OpCodes.Sub; break;
                    //case 2: operation = OpCodes.Not; break;
                    case 2: operation = OpCodes.Xor; break;
                }
                return operation;
            }

            private OpCode ReverseOperation(OpCode operation)
            {
                switch (operation.Code)
                {
                    case Code.Add: return OpCodes.Sub;
                    case Code.Sub: return OpCodes.Add;
                    case Code.Xor: return OpCodes.Xor;
                    default: throw new NotImplementedException();
                }
            }
        }
    }
}
