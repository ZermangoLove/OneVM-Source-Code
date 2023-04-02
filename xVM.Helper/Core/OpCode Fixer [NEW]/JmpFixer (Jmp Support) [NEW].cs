using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core
{
    internal static class JmpFixer
    {
        internal static void JmpFixerA(MethodDef method)
        {
            try
            {
                var instr = method.Body.Instructions;
                for (var i = 0; i < instr.Count; i++)
                {
                    if (instr[i].OpCode == OpCodes.Jmp)
                    {
                        instr[i].OpCode = OpCodes.Call;
                    }
                }
            }
            catch
            {
                System.Console.WriteLine("JmpFixerA Failed!");
            }
        }
    }
}
