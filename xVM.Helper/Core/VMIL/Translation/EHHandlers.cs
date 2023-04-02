using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core.VMIL.Translation
{
    public class TryHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.TRY;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            if(instr.Operand2 != null)
                tr.PushOperand(instr.Operand2);
            tr.PushOperand(instr.Operand1);
            tr.Instructions.Add(new ILInstruction(ILOpCode.TRY) {Annotation = instr.Annotation});
        }
    }

    public class LeaveHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.LEAVE;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.PushOperand(instr.Operand1);
            tr.Instructions.Add(new ILInstruction(ILOpCode.LEAVE) {Annotation = instr.Annotation});
        }
    }
}