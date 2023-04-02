using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class ShlDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[158];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.U4 = op1Slot.U4 << (int) op2Slot.U4;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36]);
            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class ShlQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[160];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.U8 = op1Slot.U8 << (int) op2Slot.U4;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36]);
            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }
}