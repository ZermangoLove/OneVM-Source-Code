using xVM.Helper.Core.CFG;
using xVM.Helper.Core.RT;

namespace xVM.Helper.Core.AST.IL
{
    public class ILJumpTable : IILOperand, IHasOffset
    {
        public ILJumpTable(IBasicBlock[] targets)
        {
            Targets = targets;
            Chunk = new JumpTableChunk(this);
        }

        internal JumpTableChunk Chunk
        {
            get;
        }

        public ILInstruction RelativeBase
        {
            get;
            set;
        }

        public IBasicBlock[] Targets
        {
            get;
            set;
        }

        public uint Offset => Chunk.Offset;

        public override string ToString()
        {
            return string.Format("[..{0}..]", Targets.Length);
        }
    }
}