using dnlib.DotNet.Emit;
using xVM.Helper.Core.AST.ILAST;
using xVM.Helper.Core.AST.IR;

namespace xVM.Helper.Core.VMIR
{
    public interface ITranslationHandler
    {
        Code ILCode
        {
            get;
        }

        IIROperand Translate(ILASTExpression expr, IRTranslator tr);
    }
}