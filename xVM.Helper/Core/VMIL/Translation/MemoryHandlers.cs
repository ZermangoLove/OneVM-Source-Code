using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core.VMIL.Translation
{
    public class PushHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.PUSH;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.PushOperand(instr.Operand1);
        }
    }

    public class PopHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.POP;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.PopOperand(instr.Operand1);
        }
    }

    public class MovHandler : ITranslationHandler
    {
        public IROpCode IRCode => IROpCode.MOV;

        public void Translate(IRInstruction instr, ILTranslator tr)
        {
            tr.PushOperand(instr.Operand2);
            tr.PopOperand(instr.Operand1);
        }
    }
}