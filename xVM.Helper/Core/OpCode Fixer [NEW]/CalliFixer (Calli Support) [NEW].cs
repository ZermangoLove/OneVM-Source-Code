using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core
{
    internal static class CalliFixer
    {
        #region CalliFixerA
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal static void CalliFixerA(MethodDef method)
        {
            try
            {
                RemoveNops(method);
                IList<Instruction> instr = method.Body.Instructions;
                for (var i = 0; i < instr.Count; i++)
                {
                    if (instr[i].OpCode != OpCodes.Ldftn || instr[i + 1].OpCode != OpCodes.Calli) continue;

                    instr[i + 1].OpCode = OpCodes.Nop;
                    instr[i].OpCode = OpCodes.Call;
                }
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("CalliFixeA Failed!");
            }
        }

        internal static void RemoveNops(MethodDef method)
        {
            try
            {
                IList<Instruction> instr = method.Body.Instructions;
                for (var i = 0; i < instr.Count; i++)
                {
                    if (instr[i].OpCode != OpCodes.Nop || IsNopBranchTarget(method, instr[i]) ||
                        IsNopSwitchTarget(method, instr[i]) ||
                        IsNopExceptionHandlerTarget(method, instr[i])) continue;

                    instr.RemoveAt(i);
                    i--;
                }
            }
            catch
            {
                System.Console.WriteLine("RemoveNops Failed!");
            }
        }

        private static bool IsNopBranchTarget(MethodDef method, Instruction nopInstr)
        {
            return (from instr in method.Body.Instructions
                    where instr.OpCode.OperandType == OperandType.InlineBrTarget ||
                          instr.OpCode.OperandType == OperandType.ShortInlineBrTarget && instr.Operand != null
                    select (Instruction)instr.Operand).Any(instruction2 => instruction2 == nopInstr);
        }

        private static bool IsNopSwitchTarget(MethodDef method, Instruction nopInstr)
        {
            return (from t in method.Body.Instructions
                    where t.OpCode.OperandType == OperandType.InlineSwitch && t.Operand != null
                    select (Instruction[])t.Operand).Any(source => source.Contains(nopInstr));
        }

        private static bool IsNopExceptionHandlerTarget(MethodDef method, Instruction nopInstr)
        {
            if (!method.Body.HasExceptionHandlers) return false;
            return method.Body.ExceptionHandlers.Any(exceptionHandler => exceptionHandler.FilterStart == nopInstr ||
                                                                         exceptionHandler.HandlerEnd == nopInstr ||
                                                                         exceptionHandler.HandlerStart == nopInstr ||
                                                                         exceptionHandler.TryEnd == nopInstr ||
                                                                         exceptionHandler.TryStart == nopInstr);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        internal static void CalliFixerB(MethodDef method)
        {
            try
            {
                for (int i = 2; i < method.Body.Instructions.Count; i++)
                {
                    if (method.Body.Instructions[i].OpCode.Code == Code.Ldftn && method.Body.Instructions[i + 1].OpCode.Code == Code.Calli)
                    {
                        method.Body.Instructions[i].OpCode = OpCodes.Call;
                        method.Body.Instructions[i + 1].OpCode = OpCodes.Nop;
                    }
                }
            }
            catch
            {
                System.Console.WriteLine("CalliFixerB Failed!");
            }
        }

        internal static void CalliFixerC(MethodDef method)
        {
            try
            {
                var instr = method.Body.Instructions;
                for (var i = 0; i < instr.Count; i++)
                {
                    if (instr[i].OpCode == OpCodes.Calli && instr[i - 1].OpCode == OpCodes.Ldftn)
                    {
                        instr[i].OpCode = OpCodes.Nop;
                        instr[i - 1].OpCode = OpCodes.Call;
                    }
                }
            }
            catch
            {
                System.Console.WriteLine("CalliFixerC Failed!");
            }
        }

        internal static void CalliFixerD(MethodDef method)
        {
            try
            {
                for (int x = 0; x < method.Body.Instructions.Count(); x++)
                {
                    Instruction inst = method.Body.Instructions[x];
                    if (inst.OpCode.Equals(OpCodes.Ldftn) && method.Body.Instructions[x + 1].OpCode.Equals(OpCodes.Calli))
                    {
                        inst.OpCode = OpCodes.Call;
                        method.Body.Instructions.RemoveAt(x + 1);
                    }
                }
            }
            catch
            {
                System.Console.WriteLine("CalliFixerD Failed!");
            }
        }

        internal static void CalliFixerE(MethodDef method) //typeof(<Module>).Module.ResolveMethod(A_0).MethodHandle.GetFunctionPointer();
        {
            try
            {
                var instr = method.Body.Instructions;
                for (var i = 0; i < instr.Count; i++)
                {
                    if (instr[i].OpCode == OpCodes.Calli && instr[i - 1].OpCode == OpCodes.Call)
                    {
                        var mdtoken = uint.Parse(instr[i - 2].Operand.ToString());
                        var calli_md = method.Module.ResolveToken(mdtoken) as IMethod;

                        instr[i].OpCode = OpCodes.Nop;
                        instr[i - 1].Operand = method.Module.Import(calli_md);
                        instr[i - 2].OpCode = OpCodes.Nop;
                    }
                }
            }
            catch
            {
                System.Console.WriteLine("CalliFixerE Failed!");
            }
        }
    }
}
