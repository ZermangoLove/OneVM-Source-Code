using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class CmpDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[102];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var result = op1Slot.U4 - op2Slot.U4;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            Utils.UpdateFL(result, op2Slot.U4, op1Slot.U4, result, ref fl, mask);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class CmpQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[104];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var result = op1Slot.U8 - op2Slot.U8;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            Utils.UpdateFL(result, op2Slot.U8, op1Slot.U8, result, ref fl, mask);
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class CmpR32 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[106];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var result = op1Slot.R4 - op2Slot.R4;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~mask);
            if(result == 0)
                fl |= Constants.OPCODELIST[34];
            else if(result < 0)
                fl |= Constants.OPCODELIST[36];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class CmpR64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[108];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var result = op1Slot.R8 - op2Slot.R8;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~mask);
            if(result == 0)
                fl |= Constants.OPCODELIST[34];
            else if(result < 0)
                fl |= Constants.OPCODELIST[36];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class Cmp : IOpCode
    {
        public byte Code => Constants.OPCODELIST[100];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var op1Slot = ctx.Stack[sp - 1];
            var op2Slot = ctx.Stack[sp];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            int result;
            if(ReferenceEquals(op1Slot.O, op2Slot.O))
                result = 0;
            else
                result = -1;

            var mask = (byte) (Constants.OPCODELIST[34] | Constants.OPCODELIST[36] | Constants.OPCODELIST[30] | Constants.OPCODELIST[32]);
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~mask);
            if(result == 0)
                fl |= Constants.OPCODELIST[34];
            else if(result < 0)
                fl |= Constants.OPCODELIST[36];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            state = ExecutionState.Next;
        }
    }
}