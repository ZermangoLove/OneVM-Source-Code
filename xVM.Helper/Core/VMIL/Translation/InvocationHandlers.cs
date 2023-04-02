using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core.VMIL.Translation
{
    public class CallHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.CALL;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.PushOperand(instr.Operand1);
            tr.Instructions.Add(new ILInstruction(ILOpCode.CALL) {Annotation = instr.Annotation});
        }
    }

    public class RetHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.RET;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.Instructions.Add(new ILInstruction(ILOpCode.RET));
        }
    }
}