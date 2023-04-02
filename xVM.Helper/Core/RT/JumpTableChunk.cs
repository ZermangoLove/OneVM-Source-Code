using System;
using xVM.Helper.Core.AST.IL;

namespace xVM.Helper.Core.RT
{
    internal class JumpTableChunk : IVMChunk
    {
        internal VMRuntime runtime;

        public JumpTableChunk(ILJumpTable table)
        {
            Table = table;
            if(table.Targets.Length > ushort.MaxValue)
                throw new NotSupportedException("Jump table too large.");
        }

        public ILJumpTable Table
        {
            get;
        }

        public uint Offset
        {
            get;
            private set;
        }

        ulong IVMChunk.Length => unchecked((ulong)Table.Targets.Length * 4 + 2);

        void IVMChunk.OnOffsetComputed(uint offset)
        {
            Offset = offset + 2;
        }

        byte[] IVMChunk.GetData()
        {
            var data = new byte[Table.Targets.Length * 4 + 2];
            var ptr = 0;
            data[ptr++] = (byte) Table.Targets.Length;
            data[ptr++] = (byte) (Table.Targets.Length >> 8);

            var relBase = Table.RelativeBase.Offset;
            relBase += runtime.Serializer.ComputeLength(Table.RelativeBase);
            for(var i = 0; i < Table.Targets.Length; i++)
            {
                var offset = ((ILBlock) Table.Targets[i]).Content[0].Offset;
                offset -= relBase;
                data[ptr++] = (byte) (offset >> 0);
                data[ptr++] = (byte) (offset >> 8);
                data[ptr++] = (byte) (offset >> 16);
                data[ptr++] = (byte) (offset >> 24);
            }
            return data;
        }
    }
}