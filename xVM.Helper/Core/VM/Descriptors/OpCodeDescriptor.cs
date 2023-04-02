using System.Linq;

using xVM.Helper.Core.VMIL;
using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.VM
{
    public class OpCodeDescriptor
    {
        private readonly byte[] opCodeOrder = Enumerable.Range(0, 256).Select(x => (byte) x).ToArray();

        internal OpCodeDescriptor(RandomGenerator randomGenerator)
        {
            randomGenerator.Shuffle(opCodeOrder);
        }

        public byte this[ILOpCode opCode] => opCodeOrder[(int) opCode];
    }
}