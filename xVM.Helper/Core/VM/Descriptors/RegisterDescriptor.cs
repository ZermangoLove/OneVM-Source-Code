using System.Linq;

using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.VM
{
    public class RegisterDescriptor
    {
        private readonly byte[] regOrder = Enumerable.Range(0, (int) VMRegisters.Max).Select(x => (byte) x).ToArray();

        internal RegisterDescriptor(RandomGenerator randomGenerator)
        {
            randomGenerator.Shuffle(regOrder);
        }

        public byte this[VMRegisters reg] => regOrder[(int) reg];
    }
}