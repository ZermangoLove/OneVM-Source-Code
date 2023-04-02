namespace xVM.Helper.Core.AST.IR
{
    public class IRPointer : IIROperand
    {
        public IRRegister Register
        {
            get;
            set;
        }

        public int Offset
        {
            get;
            set;
        }

        public IRVariable SourceVariable
        {
            get;
            set;
        }

        public ASTType Type
        {
            get;
            set;
        }

        public override string ToString()
        {
            var prefix = Type.ToString();
            var offsetStr = string.Empty;
            if(Offset > 0)
                offsetStr = string.Format(" + {0:x}h", Offset);
            else if(Offset < 0)
                offsetStr = string.Format(" - {0:x}h", -Offset);
            return string.Format("{0}:[{1}{2}]", prefix, Register, offsetStr);
        }
    }
}