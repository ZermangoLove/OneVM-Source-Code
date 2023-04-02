using System;

namespace xVM.Helper.Core.CFG
{
    [Flags]
    public enum BlockFlags
    {
        Normal = 0,
        ExitEHLeave = 1,
        ExitEHReturn = 2
    }
}