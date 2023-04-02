namespace xVM.Runtime.Execution
{
    internal struct EHFrame
    {
        public byte EHType;
        public ulong FilterAddr;
        public ulong HandlerAddr;
        public System.Type CatchType;

        public VMSlot BP;
        public VMSlot SP;
    }
}