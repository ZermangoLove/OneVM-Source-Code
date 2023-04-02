using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core.VMIL.Translation
{
    public class VcallHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.VCALL;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            if(instr.Operand2 != null)
                tr.PushOperand(instr.Operand2);
            tr.PushOperand(instr.Operand1);
            tr.Instructions.Add(new ILInstruction(ILOpCode.VCALL));
        }
    }

    public class NopHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.NOP;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.Instructions.Add(new ILInstruction(ILOpCode.NOP));
        }
    }
}