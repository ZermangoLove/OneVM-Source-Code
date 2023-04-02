using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class MulDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[130];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;

            var slot = new VMSlot();
            ulong result = op1Slot.U4 * op2Slot.U4;
            slot.U4 = (uint) result;
            ctx.Stack[sp] = slot;

            var mask1 = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[38]);
            var mask2 = (byte) (Constants.OPCODELIST[32] | Constants.OPCODELIST[30]);
            byte ovF = 0;
            if((fl & Constants.OPCODELIST[38]) != 0)
            {
                if((result & (0xffffffff << 32)) != 0)
                    ovF = mask2;
            }
            else
            {
                result = (ulong) ((int) op1Slot.U4 * (int) op2Slot.U4);
                if(result >> 63 != slot.U4 >> 31) ovF = mask2;
            }
            fl = (byte) ((fl & ~mask2) | ovF);
            Utils.UpdateFL(op1Slot.U4, op2Slot.U4, slot.U4, slot.U4, ref fl, mask1);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class MulQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[132];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;

            var slot = new VMSlot();
            var result = op1Slot.U8 * op2Slot.U8;
            slot.U8 = result;
            ctx.Stack[sp] = slot;

            var mask1 = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[38]);
            var mask2 = (byte) (Constants.OPCODELIST[32] | Constants.OPCODELIST[30]);
            byte ovF = 0;
            if((fl & Constants.OPCODELIST[38]) != 0)
            {
                if(Carry(op1Slot.U8, op2Slot.U8) != 0)
                    ovF = mask2;
            }
            else
            {
                if(result >> 63 != (op1Slot.U8 ^ op2Slot.U8) >> 63)
                    ovF = mask2;
            }
            fl = (byte) ((fl & ~mask2) | ovF);
            Utils.UpdateFL(op1Slot.U4, op2Slot.U8, slot.U8, slot.U8, ref fl, mask1);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }

        private static ulong Carry(ulong a, ulong b)
        {
            // https://stackoverflow.com/questions/1815367/multiplication-of-large-numbers-how-to-catch-overflow
            ulong lo1 = a & 0xffffffff, hi1 = a >> 32;
            ulong lo2 = b & 0xffffffff, hi2 = b >> 32;

            ulong s0, s1, s2;
            var x = lo1 * lo2;
            s0 = x & 0xffffffff;

            x = hi1 * lo2 + (x >> 32);
            s1 = x & 0xffffffff;
            s2 = x >> 32;

            x = s1 + lo1 * hi2;
            s1 = x & 0xffffffff;

            x = s2 + hi1 * hi2 + (x >> 32);
            return x;
        }
    }

    internal unsafe class MulR32 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[134];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.R4 = op2Slot.R4 * op1Slot.R4;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[38]);
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~mask);
            if(slot.R4 == 0)
                fl |= Constants.OPCODELIST[34];
            else if(slot.R4 < 0)
                fl |= Constants.OPCODELIST[36];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class MulR64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[136];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 1;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var slot = new VMSlot();
            slot.R8 = op2Slot.R8 * op1Slot.R8;
            ctx.Stack[sp] = slot;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[38]);
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