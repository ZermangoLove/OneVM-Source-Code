using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.Protections
{
    public class MutateRT
    {
        private static Random rnd = new Random();

        public static void IntControlFlow(MethodDef method)
        {
            Local local = new Local(method.Module.ImportAsTypeSig(typeof(int)));
            method.Body.Variables.Add(local);
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                if (method.Body.Instructions[i].IsLdcI4())
                {
                    int numorig = rnd.Next();
                    int div = rnd.Next();
                    int num = numorig ^ div;

                    Instruction nop = OpCodes.Nop.ToInstruction();
                    method.Body.Instructions.Insert(i + 1, OpCodes.Stloc_S.ToInstruction(local));
                    method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, method.Body.Instructions[i].GetLdcI4Value() - sizeof(float)));
                    method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, num));
                    method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, div));
                    method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
                    method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, numorig));
                    method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, nop));
                    method.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
                    method.Body.Instructions.Insert(i + 9, OpCodes.Stloc_S.ToInstruction(local));
                    method.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Sizeof, method.Module.Import(typeof(float))));
                    method.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
                    method.Body.Instructions.Insert(i + 12, nop);
                    i += 12;
                }
            }
        }

        public static void IntControlFlow_NoSizeOf(MethodDef method)
        {
            Local local = new Local(method.Module.ImportAsTypeSig(typeof(int)));
            method.Body.Variables.Add(local);
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                if (method.Body.Instructions[i].IsLdcI4())
                {
                    int numorig = rnd.Next();
                    int div = rnd.Next();
                    int num = numorig ^ div;

                    Instruction nop = OpCodes.Nop.ToInstruction();
                    method.Body.Instructions.Insert(i + 1, OpCodes.Stloc_S.ToInstruction(local));
                    method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, method.Body.Instructions[i].GetLdcI4Value() - sizeof(float)));
                    method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, num));
                    method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, div));
                    method.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
                    method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, numorig));
                    method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, nop));
                    method.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
                    method.Body.Instructions.Insert(i + 9, OpCodes.Stloc_S.ToInstruction(local));
                    method.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Ldc_I4, sizeof(float)));
                    method.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
                    method.Body.Instructions.Insert(i + 12, nop);
                    i += 12;
                }
            }
        }

        public static void BasicEncodeInt(MethodDef method)
        {
            if (method.HasBody)
            {
                if (method.Body.HasInstructions)
                {
                    int x = 0;
                    while (x < method.Body.Instructions.Count)
                    {
                        if (method.Body.Instructions[x].IsLdcI4())
                        {
                            uint num = (uint)((int)method.Body.Instructions[x].GetLdcI4Value());
                            uint num2 = (uint)(rnd.Next());
                            uint value = num2 ^ num;

                            method.Body.Instructions[x].OpCode = OpCodes.Ldc_I4;
                            method.Body.Instructions[x].Operand = (int)value;
                            method.Body.Instructions.Insert(x + 1, Instruction.Create(OpCodes.Ldc_I4, (int)num2));
                            method.Body.Instructions.Insert(x + 2, Instruction.Create(OpCodes.Xor));
                            x += 3;
                        }
                        else
                            x++;
                    }
                }
            }
        }

        public static void Array_Mutation(MethodDef a)
        {
            a.Body.Instructions.SimplifyBranches();
            a.Body.Instructions.SimplifyMacros(a.Body.Variables, a.Parameters);
            List<Instruction> list = new List<Instruction>();
            Local local = new Local(new SZArraySig(a.Module.CorLibTypes.Object));
            Local local2 = new Local(a.Module.CorLibTypes.Object);
            Local local3 = new Local(new SZArraySig(a.Module.CorLibTypes.Int32));
            Local local4 = new Local(a.Module.CorLibTypes.Int32);
            list.Add(OpCodes.Ldc_I4.ToInstruction(a.Body.Variables.Count));
            list.Add(OpCodes.Newarr.ToInstruction(a.Module.CorLibTypes.Int32));
            list.Add(OpCodes.Stloc.ToInstruction(local3));
            for (int i = 0; i < a.Body.Variables.Count; i++)
            {
                list.Add(OpCodes.Ldloc.ToInstruction(local3));
                list.Add(OpCodes.Ldc_I4.ToInstruction(i));
                list.Add(OpCodes.Ldc_I4_M1.ToInstruction());
                list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Int32));
            }
            list.Add(OpCodes.Ldc_I4_0.ToInstruction());
            list.Add(OpCodes.Stloc.ToInstruction(local4));
            int j = 0;
            while (j < a.Body.Instructions.Count)
            {
                Instruction instruction = a.Body.Instructions[j];
                list.Add(instruction);
                if (instruction.OpCode.Code == Code.Ldloc)
                {
                    goto IL_4AA;
                }
                if (instruction.OpCode.Code == Code.Ldloca)
                {
                    goto IL_4AA;
                }
                if (instruction.OpCode.Code == Code.Stloc)
                {
                    Local local5 = (Local)instruction.Operand;
                    int index = local5.Index;
                    if (local5.Type.IsValueType)
                    {
                        list.Add(OpCodes.Box.ToInstruction(local5.Type.ToTypeDefOrRef()));
                    }
                    else
                    {
                        list.Add(OpCodes.Castclass.ToInstruction(a.Module.CorLibTypes.Object));
                    }
                    Instruction instruction2 = OpCodes.Nop.ToInstruction();
                    list.Add(OpCodes.Ldloc.ToInstruction(local3));
                    list.Add(OpCodes.Ldc_I4.ToInstruction(index));
                    list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
                    list.Add(OpCodes.Ldc_I4_M1.ToInstruction());
                    list.Add(OpCodes.Ceq.ToInstruction());
                    list.Add(OpCodes.Brtrue.ToInstruction(instruction2));
                    list.Add(OpCodes.Ldloc.ToInstruction(local));
                    list.Add(OpCodes.Ldloc.ToInstruction(local3));
                    list.Add(OpCodes.Ldc_I4.ToInstruction(index));
                    list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
                    list.Add(OpCodes.Ldnull.ToInstruction());
                    list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Object));
                    list.Add(instruction2);
                    list.Add(OpCodes.Ldloc.ToInstruction(local4));
                    list.Add(OpCodes.Ldc_I4_1.ToInstruction());
                    list.Add(OpCodes.Add.ToInstruction());
                    list.Add(OpCodes.Stloc.ToInstruction(local4));
                    list.Add(OpCodes.Ldloc.ToInstruction(local3));
                    list.Add(OpCodes.Ldc_I4.ToInstruction(index));
                    list.Add(OpCodes.Ldloc.ToInstruction(local4));
                    list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Int32));
                    list.Add(OpCodes.Stloc.ToInstruction(local2));
                    list.Add(OpCodes.Ldloc.ToInstruction(local));
                    list.Add(OpCodes.Ldloc.ToInstruction(local3));
                    list.Add(OpCodes.Ldc_I4.ToInstruction(index));
                    list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
                    list.Add(OpCodes.Ldloc.ToInstruction(local2));
                    list.Add(OpCodes.Stelem_Ref.ToInstruction());
                    list.Add(OpCodes.Ldnull.ToInstruction());
                    list.Add(OpCodes.Stloc.ToInstruction(local2));
                    instruction.Operand = null;
                    instruction.OpCode = OpCodes.Nop;
                }
            IL_795:
                j++;
                continue;
            IL_4AA:
                Local local6 = (Local)instruction.Operand;
                int index2 = local6.Index;
                list.Add(OpCodes.Ldloc.ToInstruction(local));
                list.Add(OpCodes.Ldloc.ToInstruction(local3));
                list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
                list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
                list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Object));
                list.Add(OpCodes.Dup.ToInstruction());
                list.Add(OpCodes.Ldloc.ToInstruction(local));
                list.Add(OpCodes.Ldloc.ToInstruction(local3));
                list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
                list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
                list.Add(OpCodes.Ldnull.ToInstruction());
                list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Object));
                list.Add(OpCodes.Ldloc.ToInstruction(local4));
                list.Add(OpCodes.Ldc_I4_1.ToInstruction());
                list.Add(OpCodes.Add.ToInstruction());
                list.Add(OpCodes.Stloc.ToInstruction(local4));
                list.Add(OpCodes.Ldloc.ToInstruction(local3));
                list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
                list.Add(OpCodes.Ldloc.ToInstruction(local4));
                list.Add(OpCodes.Stelem.ToInstruction(a.Module.CorLibTypes.Int32));
                list.Add(OpCodes.Stloc.ToInstruction(local2));
                list.Add(OpCodes.Ldloc.ToInstruction(local));
                list.Add(OpCodes.Ldloc.ToInstruction(local3));
                list.Add(OpCodes.Ldc_I4.ToInstruction(index2));
                list.Add(OpCodes.Ldelem.ToInstruction(a.Module.CorLibTypes.Int32));
                list.Add(OpCodes.Ldloc.ToInstruction(local2));
                list.Add(OpCodes.Stelem_Ref.ToInstruction());
                list.Add(OpCodes.Ldnull.ToInstruction());
                list.Add(OpCodes.Stloc.ToInstruction(local2));
                if (local6.Type.IsValueType)
                {
                    if (instruction.OpCode.Code == Code.Ldloc)
                    {
                        list.Add(OpCodes.Unbox_Any.ToInstruction(local6.Type.ToTypeDefOrRef()));
                    }
                    else
                    {
                        list.Add(OpCodes.Unbox.ToInstruction(local6.Type.ToTypeDefOrRef()));
                    }
                }
                else
                {
                    list.Add(OpCodes.Castclass.ToInstruction(local6.Type.ToTypeDefOrRef()));
                }
                instruction.Operand = null;
                instruction.OpCode = OpCodes.Nop;
                goto IL_795;
            }
            a.Body.Variables.Clear();
            a.Body.Variables.Add(local);
            a.Body.Variables.Add(local2);
            a.Body.Variables.Add(local3);
            a.Body.Variables.Add(local4);
            list.Insert(0, OpCodes.Ldc_I4.ToInstruction(new Random().Next(10000, 20000)));
            list.Insert(1, OpCodes.Newarr.ToInstruction(a.Module.CorLibTypes.Object));
            list.Insert(2, OpCodes.Stloc.ToInstruction(local));
            list.OptimizeBranches();
            list.OptimizeMacros();
            a.Body.Instructions.Clear();
            foreach (Instruction item in list)
            {
                a.Body.Instructions.Add(item);
            }
        }

        public static void EncodeIntSizeOf(MethodDef method)
        {
            Random R = new Random();
            int num = 0;
            ITypeDefOrRef type = null;
            method.Body.SimplifyBranches();
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                Instruction instruction = method.Body.Instructions[i];
                if (instruction.IsLdcI4())
                {
                    switch (R.Next(1, 16))
                    {
                        case 1:
                            type = method.Module.Import(typeof(int));
                            num = 4;
                            break;
                        case 2:
                            type = method.Module.Import(typeof(sbyte));
                            num = 1;
                            break;
                        case 3:
                            type = method.Module.Import(typeof(byte));
                            num = 1;
                            break;
                        case 4:
                            type = method.Module.Import(typeof(bool));
                            num = 1;
                            break;
                        case 5:
                            type = method.Module.Import(typeof(decimal));
                            num = 16;
                            break;
                        case 6:
                            type = method.Module.Import(typeof(short));
                            num = 2;
                            break;
                        case 7:
                            type = method.Module.Import(typeof(long));
                            num = 8;
                            break;
                        case 8:
                            type = method.Module.Import(typeof(uint));
                            num = 4;
                            break;
                        case 9:
                            type = method.Module.Import(typeof(float));
                            num = 4;
                            break;
                        case 10:
                            type = method.Module.Import(typeof(char));
                            num = 2;
                            break;
                        case 11:
                            type = method.Module.Import(typeof(ushort));
                            num = 2;
                            break;
                        case 12:
                            type = method.Module.Import(typeof(double));
                            num = 8;
                            break;
                        case 13:
                            type = method.Module.Import(typeof(DateTime));
                            num = 8;
                            break;
                        case 14:
                            type = method.Module.Import(typeof(ConsoleKeyInfo));
                            num = 12;
                            break;
                        case 15:
                            type = method.Module.Import(typeof(Guid));
                            num = 16;
                            break;
                    }
                    int num2 = R.Next(1, 1000);
                    bool flag = Convert.ToBoolean(R.Next(0, 2));
                    switch ((num != 0) ? ((Convert.ToInt32(instruction.Operand) % num == 0) ? R.Next(1, 5) : R.Next(1, 4)) : R.Next(1, 4))
                    {
                        case 1:
                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
                            method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
                            instruction.Operand = Convert.ToInt32(instruction.Operand) - num + (flag ? (-num2) : num2);
                            method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
                            method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
                            i += 4;
                            break;
                        case 2:
                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
                            method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sub));
                            instruction.Operand = Convert.ToInt32(instruction.Operand) + num + (flag ? (-num2) : num2);
                            method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
                            method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
                            i += 4;
                            break;
                        case 3:
                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
                            method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
                            instruction.Operand = Convert.ToInt32(instruction.Operand) - num + (flag ? (-num2) : num2);
                            method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
                            method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
                            i += 4;
                            break;
                        case 4:
                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, type));
                            method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Mul));
                            instruction.Operand = Convert.ToInt32(instruction.Operand) / num;
                            i += 2;
                            break;
                        default:
                            method.Body.Instructions.Insert(i + 3, Instruction.CreateLdcI4(num2));
                            method.Body.Instructions.Insert(i + 4, Instruction.Create(flag ? OpCodes.Add : OpCodes.Sub));
                            i += 4;
                            break;
                    }
                }
            }
        }

        private static CilBody body;

        private static double RandomDouble(double min, double max)
        {
            return new Random().NextDouble() * (max - min) + min;
        }

        public static void SizeOfMutate(MethodDef methodDef)
        {
            if (methodDef.HasBody)
            {
                for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
                {
                    if (methodDef.Body.Instructions[i].IsLdcI4())
                    {
                        // EmptyType

                        //int operand = methodDef.Body.Instructions[i].GetLdcI4Value();
                        //methodDef.Body.Instructions[i].Operand = operand - Type.EmptyTypes.Length;
                        //methodDef.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                        //methodDef.Body.Instructions.Insert(i + 1, OpCodes.Ldsfld.ToInstruction(methodDef.Module.Import(typeof(Type).GetField("EmptyTypes"))));
                        //methodDef.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldlen));
                        //methodDef.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Add));

                        body = methodDef.Body;
                        int ldcI4Value = body.Instructions[i].GetLdcI4Value();
                        int num = rnd.Next(1, 4);
                        int num2 = ldcI4Value - num;
                        body.Instructions[i].Operand = num2;
                        Mutate(i, num, num2, methodDef.Module);

                        // Double Parse

                        //int operand3 = methodDef.Body.Instructions[i].GetLdcI4Value();
                        //double n = RandomDouble(1.0, 1000.0);
                        //string converter = Convert.ToString(n);
                        //double nEw = double.Parse(converter);
                        //int conta = operand3 - (int)nEw;
                        //methodDef.Body.Instructions[i].Operand = conta;
                        //methodDef.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                        //methodDef.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldstr, converter));
                        //methodDef.Body.Instructions.Insert(i + 2, OpCodes.Call.ToInstruction(methodDef.Module.Import(typeof(double).GetMethod("Parse", new Type[] { typeof(string) }))));
                        //methodDef.Body.Instructions.Insert(i + 3, OpCodes.Conv_I4.ToInstruction());
                        //methodDef.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Add));

                        // Calc

                        //int op = methodDef.Body.Instructions[i].GetLdcI4Value();
                        //int newvalue = rnd.Next(-100, 10000);
                        //switch (rnd.Next(1, 4))
                        //{
                        //    case 1:
                        //        methodDef.Body.Instructions[i].Operand = op - newvalue;
                        //        methodDef.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(newvalue));
                        //        methodDef.Body.Instructions.Insert(i + 2, OpCodes.Add.ToInstruction());
                        //        i += 2;
                        //        break;
                        //    case 2:
                        //        methodDef.Body.Instructions[i].Operand = op + newvalue;
                        //        methodDef.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(newvalue));
                        //        methodDef.Body.Instructions.Insert(i + 2, OpCodes.Sub.ToInstruction());
                        //        i += 2;
                        //        break;
                        //    case 3:
                        //        methodDef.Body.Instructions[i].Operand = op ^ newvalue;
                        //        methodDef.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(newvalue));
                        //        methodDef.Body.Instructions.Insert(i + 2, OpCodes.Xor.ToInstruction());
                        //        i += 2;
                        //        break;
                        //    case 4:
                        //        int operand2 = methodDef.Body.Instructions[i].GetLdcI4Value();
                        //        methodDef.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                        //        methodDef.Body.Instructions[i].Operand = operand2 - 1;
                        //        int valor = rnd.Next(100, 500);
                        //        int valor2 = rnd.Next(1000, 5000);
                        //        methodDef.Body.Instructions.Insert(i + 1, Instruction.CreateLdcI4(valor));
                        //        methodDef.Body.Instructions.Insert(i + 2, Instruction.CreateLdcI4(valor2));
                        //        methodDef.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Clt));
                        //        methodDef.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Conv_I4));
                        //        methodDef.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Add));
                        //        i += 5;
                        //        break;
                        //}
                    }
                }
            }
        }

        private static void Mutate(int i, int sub, int num2, ModuleDef module)
        {
            switch (sub)
            {
                case 1:
                    body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
                    body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Add));
                    return;
                case 2:
                    body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
                    body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
                    body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Add));
                    body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Add));
                    return;
                case 3:
                    body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(int))));
                    body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(byte))));
                    body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Sub));
                    body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Add));
                    return;
                case 4:
                    body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(decimal))));
                    body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(GCCollectionMode))));
                    body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Sub));
                    body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Sizeof, module.Import(typeof(int))));
                    body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Sub));
                    body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Add));
                    return;
                default:
                    return;
            }
        }
    }
}