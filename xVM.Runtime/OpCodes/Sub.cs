using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class SubR32 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[126];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.R4 = op1Slot.R4 - op2Slot.R4;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~mask);
            if(slot.R4 == 0)
                fl |= Constants.OPCODELIST[34];
            else if(slot.R4 < 0)
                fl |= Constants.OPCODELIST[36];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class SubR64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[128];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.R8 = op1Slot.R8 - op2Slot.R8;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~mask);
            if(slot.R8 == 0)
                fl |= Constants.OPCODELIST[34];
            else if(slot.R8 < 0)
                fl |= Constants.OPCODELIST[36];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }
}