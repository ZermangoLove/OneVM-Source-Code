using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class AddDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[118];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            if(op1Slot.O is IReference)
                slot.O = ((IReference) op1Slot.O).Add(op2Slot.U4);
            else if(op2Slot.O is IReference)
                slot.O = ((IReference) op2Slot.O).Add(op1Slot.U4);
            else
                slot.U4 = op2Slot.U4 + op1Slot.U4;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class AddQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[120];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            if(op1Slot.O is IReference)
                slot.O = ((IReference) op1Slot.O).Add(op2Slot.U8);
            else if(op2Slot.O is IReference)
                slot.O = ((IReference) op2Slot.O).Add(op1Slot.U8);
            else
                slot.U8 = op2Slot.U8 + op1Slot.U8;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            Utils.UpdateFL(op1Slot.U8, op2Slot.U8, slot.U8, slot.U8, ref fl, mask);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class AddR32 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[122];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.R4 = op2Slot.R4 + op1Slot.R4;
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

    internal unsafe class AddR64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[124];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.R8 = op2Slot.R8 + op1Slot.R8;
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