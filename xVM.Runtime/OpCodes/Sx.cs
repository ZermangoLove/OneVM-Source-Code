using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class SxDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[90];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var operand = ctx.Stack[sp];
            if((operand.U4 & 0x80000000) != 0)
                operand.U8 = 0xffffffff00000000 | operand.U4;
            ctx.Stack[sp] = operand;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class SxWord : IOpCode
    {
        public byte Code => Constants.OPCODELIST[88];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var operand = ctx.Stack[sp];
            if((operand.U2 & 0x8000) != 0)
                operand.U4 = operand.U2 | 0xffff0000;
            ctx.Stack[sp] = operand;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class SxByte : IOpCode
    {
        public byte Code => Constants.OPCODELIST[86];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var operand = ctx.Stack[sp];
            if((operand.U1 & 0x80) != 0)
                operand.U4 = operand.U1 | 0xffffff00;
            ctx.Stack[sp] = operand;

            state = ExecutionState.Next;
        }
    }
}