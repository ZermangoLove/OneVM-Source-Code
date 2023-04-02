namespace xVM.Helper.Core.ILAST
{
    public interface ITransformationHandler
    {
        void Initialize(ILASTTransformer tr);
        void Transform(ILASTTransformer tr);
    }
}