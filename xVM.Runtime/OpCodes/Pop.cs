using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class Pop : IOpCode
    {
        public byte Code => Constants.OPCODELIST[70];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var slot = ctx.Stack[sp];
            ctx.Stack.SetTopPosition(--sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var regId = ctx.ReadByte();
            if((regId == Constants.OPCODELIST[16] || regId == Constants.OPCODELIST[14]) && slot.O is StackRef)
                ctx.Registers[regId] = new VMSlot {U4 = ((StackRef) slot.O).StackPos};
            else
                ctx.Registers[regId] = slot;
            state = ExecutionState.Next;
        }
    }
}