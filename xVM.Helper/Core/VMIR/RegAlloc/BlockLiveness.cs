using System.Collections.Generic;

using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.VMIR.RegAlloc
{
    public class BlockLiveness
    {
        private BlockLiveness(HashSet<IRVariable> inLive, HashSet<IRVariable> outLive)
        {
            InLive = inLive;
            OutLive = outLive;
        }

        internal HashSet<IRVariable> InLive
        {
            get;
        }

        internal HashSet<IRVariable> OutLive
        {
            get;
        }

        internal static BlockLiveness Empty()
        {
            return new BlockLiveness(new HashSet<IRVariable>(), new HashSet<IRVariable>());
        }

        internal BlockLiveness Clone()
        {
            return new BlockLiveness(new HashSet<IRVariable>(InLive), new HashSet<IRVariable>(OutLive));
        }
    }
}