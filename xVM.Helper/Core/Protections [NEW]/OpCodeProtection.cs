using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    public class OpCodeProtection
    {
        private static Random random = new Random();

        public static void Execute(MethodDef method)
        {
            LdfldProtection(method);
            CallvirtProtection(method);
            CtorCallProtection(method);
        }

        private static void CtorCallProtection(MethodDef method)
        {
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Call)
                {
                    if (instr[i].Operand.ToString().ToLower().Contains("void"))
                    {
                        if ((i - 1) > 0)
                            if (instr[i - 1].IsLdarg())
                            {
                                Local new_local = new Local(method.Module.CorLibTypes.Int32);
                                method.Body.Variables.Add(new_local);

                                instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                                instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
                                instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
                                instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                                //---------------------------------------------------bne.un.s +3
                                instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
                                //---------------------------------------------------br.s +4
                                instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
                                //---------------------------------------------------br.s +1
                                instr.Insert(i + 6, OpCodes.Nop.ToInstruction());

                                instr.Insert(i + 3, new Instruction(OpCodes.Bne_Un_S, instr[i + 4]));
                                instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
                                instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
                            }
                    }
                }
            }
        }

        private static void LdfldProtection(MethodDef method)
        {
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Ldfld)
                {
                    //if (instr[i].Operand.ToString().ToLower().Contains("class"))
                    //{
                    if ((i - 1) > 0)
                        if (instr[i - 1].IsLdarg())
                        {
                            Local new_local = new Local(method.Module.CorLibTypes.Int32);
                            method.Body.Variables.Add(new_local);

                            instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                            instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
                            instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
                            instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                            //---------------------------------------------------bne.un.s +3
                            instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
                            //---------------------------------------------------br.s +4
                            instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
                            //---------------------------------------------------br.s +1
                            instr.Insert(i + 6, OpCodes.Nop.ToInstruction());

                            instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
                            instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
                            instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
                        }
                    //}
                }
            }
        }

        private static void CallvirtProtection(MethodDef method)
        {
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Callvirt)
                {
                    if (instr[i].Operand.ToString().ToLower().Contains("int32"))
                    {
                        if ((i - 1) > 0)
                            if (instr[i - 1].IsLdloc())
                            {
                                Local new_local = new Local(method.Module.CorLibTypes.Int32);
                                method.Body.Variables.Add(new_local);

                                instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                                instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
                                instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
                                instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                                //---------------------------------------------------bne.un.s +3
                                instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
                                //---------------------------------------------------br.s +4
                                instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
                                //---------------------------------------------------br.s +1
                                instr.Insert(i + 6, OpCodes.Nop.ToInstruction());

                                instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
                                instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
                                instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
                            }
                    }
                }
            }
        }
    }
}
