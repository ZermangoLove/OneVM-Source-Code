using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Ckoverflow : IVCall
    {
        public byte Code => Constants.OPCODELIST[190];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var fSlot = ctx.Stack[sp--];

            if(fSlot.U4 != 0)
                throw new OverflowException();

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}