using System.Collections.Generic;

using xVM.Helper.Core.CFG;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.VM
{
    public class VMMethodInfo
    {
        internal readonly Dictionary<IBasicBlock, VMBlockKey> BlockKeys;
        internal readonly HashSet<VMRegisters> UsedRegister;
        public byte EntryKey;
        public byte ExitKey;

        public ScopeBlock RootScope;

        public VMMethodInfo()
        {
            BlockKeys = new Dictionary<IBasicBlock, VMBlockKey>();
            UsedRegister = new HashSet<VMRegisters>();
        }
    }

    public struct VMBlockKey
    {
        public byte EntryKey;
        public byte ExitKey;
    }
}