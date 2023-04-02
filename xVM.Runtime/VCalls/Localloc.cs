using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Localloc : IVCall
    {
        public byte Code => Constants.OPCODELIST[212];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var bp = ctx.Registers[Constants.OPCODELIST[14]].U4;
            var size = ctx.Stack[sp].U4;
            ctx.Stack[sp] = new VMSlot
            {
                U8 = (ulong) ctx.Stack.Localloc(bp, size)
            };

            state = ExecutionState.Next;
        }
    }
}