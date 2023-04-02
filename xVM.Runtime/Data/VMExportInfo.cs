namespace xVM.Runtime.Data
{
    internal struct VMExportInfo
    {
        public unsafe VMExportInfo(ref byte* ptr)
        {
            CodeOffset = (ulong)*(uint*) ptr;
            ptr += 4;
            if(CodeOffset != 0)
            {
                EntryKey = (ulong)*(uint*) ptr;
                ptr += 4;
            }
            else
            {
                EntryKey = 0;
            }
            Signature = new VMFuncSig(ref ptr);
        }

        public readonly ulong CodeOffset;
        public readonly ulong EntryKey;
        public readonly VMFuncSig Signature;
    }
}