namespace xVM.Helper.Core.RT
{
    public interface IVMChunk
    {
        ulong Length
        {
            get;
        }

        void OnOffsetComputed(uint offset);

        byte[] GetData();
    }
}