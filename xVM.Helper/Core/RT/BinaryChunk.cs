using System;

namespace xVM.Helper.Core.RT
{
    internal class BinaryChunk : IVMChunk
    {
        public EventHandler<OffsetComputeEventArgs> OffsetComputed;

        public BinaryChunk(byte[] data)
        {
            Data = data;
        }

        public byte[] Data
        {
            get;
        }

        public uint Offset
        {
            get;
            private set;
        }

        ulong IVMChunk.Length => unchecked((ulong)Data.Length);

        void IVMChunk.OnOffsetComputed(uint offset)
        {
            if(OffsetComputed != null)
                OffsetComputed(this, new OffsetComputeEventArgs(offset));
            Offset = offset;
        }

        byte[] IVMChunk.GetData()
        {
            return Data;
        }
    }

    public class OffsetComputeEventArgs : EventArgs
    {
        internal OffsetComputeEventArgs(ulong offset)
        {
            Offset = offset;
        }

        public ulong Offset
        {
            get;
        }
    }
}