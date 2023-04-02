using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core.VMIL
{
    public interface ITranslationHandler
    {
        IROpCode IRCode
        {
            get;
        }

        void Translate(IRInstruction instr, ILTranslator tr);
    }
}