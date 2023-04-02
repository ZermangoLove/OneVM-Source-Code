using System.Runtime.InteropServices;

namespace xVM.Runtime.Data
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct VMDATA_HEADER
    {
        public uint MAGIC;
        public uint MD_COUNT;
        public uint STR_COUNT;
        public uint EXP_COUNT;
    }
}
