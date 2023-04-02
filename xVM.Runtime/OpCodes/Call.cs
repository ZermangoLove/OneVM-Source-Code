using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class Call : IOpCode
    {
        public byte Code => Constants.OPCODELIST[92];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var slot = ctx.Stack[sp];
            ctx.Stack[sp] = ctx.Registers[Constants.OPCODELIST[18]];
            ctx.Registers[Constants.OPCODELIST[18]].U8 = slot.U8;
            state = ExecutionState.Next;
        }
    }
}