using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections.Mutation
{
    public class Int32Local
    {
        public Local Local;
        public int Value;
        public Int32Local(Local local, int value)
        {
            Local = local;
            Value = value;
        }
    }
}
