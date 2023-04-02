using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections
{
    public class HideMethod
    {
        public static void Execute(MethodDef method)
        {
            method.Body.Instructions.Insert(1, new Instruction(OpCodes.Br_S, method.Body.Instructions[1]));
            method.Body.Instructions.Insert(2, new Instruction(OpCodes.Unaligned, (byte)0));
        }
    }
}
