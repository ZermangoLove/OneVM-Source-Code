using xVM.Helper.Core.RT;

namespace xVM.Helper.Core.AST.IL
{
    internal class ILDataTarget : IILOperand, IHasOffset
    {
        public ILDataTarget(BinaryChunk target)
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

        public uint Offset => Target.Offset;

        public override string ToString()
        {
            return Name;
        }
    }
}