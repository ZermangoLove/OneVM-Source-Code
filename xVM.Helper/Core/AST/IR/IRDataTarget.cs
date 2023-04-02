using xVM.Helper.Core.RT;

namespace xVM.Helper.Core.AST.IR
{
    internal class IRDataTarget : IIROperand
    {
        public IRDataTarget(BinaryChunk target)
        {
            Target = target;
        }

        public BinaryChunk Target
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ASTType Type => ASTType.Ptr;

        public override string ToString()
        {
            return Name;
        }
    }
}