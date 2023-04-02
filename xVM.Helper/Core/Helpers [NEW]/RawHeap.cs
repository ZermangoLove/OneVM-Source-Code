using System.IO;
using dnlib.DotNet.Writer;

namespace xVM.Helper.Core.Helpers
{
    internal class RawHeap : HeapBase
    {
        private readonly byte[] content;

        public RawHeap(string name, byte[] content)
        {
            Name = name;
            this.content = content;
        }

        public override string Name
        {
            get;
        }

        public override uint GetRawLength()
        {
            return (uint)content.Length;
        }

        protected override void WriteToImpl(DataWriter writer)
        {
            writer.WriteBytes(content);
        }
    }
}
