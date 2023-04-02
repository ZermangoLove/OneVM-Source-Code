using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.VM
{
    public class VMDescriptor
    {
        public VMDescriptor(IVMSettings settings)
        {
            RandomGenerator = new RandomGenerator(32);
            Settings = settings;
            Architecture = new ArchDescriptor(RandomGenerator);
            Runtime = new RuntimeDescriptor(RandomGenerator);
            Data = new DataDescriptor(RandomGenerator);
        }

        internal RandomGenerator RandomGenerator
        {
            get;
        }

        public IVMSettings Settings
        {
            get;
        }

        public ArchDescriptor Architecture
        {
            get;
        }

        public RuntimeDescriptor Runtime
        {
            get;
        }

        public DataDescriptor Data
        {
            get;
            private set;
        }

        public void ResetData()
        {
            Data = new DataDescriptor(RandomGenerator);
        }
    }
}