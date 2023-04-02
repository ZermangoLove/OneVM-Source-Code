using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class FConvR32 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[166];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp];

            valueSlot.R4 = (long) valueSlot.U8;

            ctx.Stack[sp] = valueSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class FConvR64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[168];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp];

            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            if((fl & Constants.OPCODELIST[38]) != 0) valueSlot.R8 = valueSlot.U8;
            else valueSlot.R8 = (long) valueSlot.U8;
            ctx.Registers[Constants.OPCODELIST[20]].U1 = (byte) (fl & ~Constants.OPCODELIST[38]);

            ctx.Stack[sp] = valueSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class FConvR32R64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[162];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp];
            valueSlot.R8 = valueSlot.R4;
            ctx.Stack[sp] = valueSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class FConvR64R32 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[164];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp];
            valueSlot.R4 = (float) valueSlot.R8;
            ctx.Stack[sp] = valueSlot;

            state = ExecutionState.Next;
        }
    }
}