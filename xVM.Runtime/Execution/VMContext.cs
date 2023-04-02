using System.Collections.Generic;
using xVM.Runtime.Dynamic;

namespace xVM.Runtime.Execution
{
    internal class VMContext
    {
        public readonly List<EHFrame> EHStack = new List<EHFrame>();
        public readonly List<EHState> EHStates = new List<EHState>();
        public readonly VMInstance Instance;

        public readonly VMSlot[] Registers = new VMSlot[16];
        public readonly VMStack Stack = new VMStack();

        public VMContext(VMInstance inst)
        {
            Instance = inst;
        }

        public unsafe byte ReadByte()
        {
            var key = Registers[Constants.OPCODELIST[22]].U4;
            var ip = (byte*) Registers[Constants.OPCODELIST[18]].U8++;
            var b = (byte) (*ip ^ key);
            key = key * 7 + b;
            Registers[Constants.OPCODELIST[22]].U4 = key;
            return b;
        }
    }
}