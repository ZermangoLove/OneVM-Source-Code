using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Rangechk : IVCall
    {
        public byte Code => Constants.OPCODELIST[192];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp--];
            var maxSlot = ctx.Stack[sp--];
            var minSlot = ctx.Stack[sp];

            valueSlot.U8 = (long) valueSlot.U8 > (long) maxSlot.U8 || (long) valueSlot.U8 < (long) minSlot.U8 ? 1u : 0;

            ctx.Stack[sp] = valueSlot;

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}