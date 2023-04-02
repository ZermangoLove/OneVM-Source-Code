using xVM.Runtime.Data;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class Vcall : IOpCode
    {
        public byte Code => Constants.OPCODELIST[174];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var slot = ctx.Stack[sp];
            ctx.Stack.SetTopPosition(--sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var vCall = VCallMap.Lookup(slot.U1);
            vCall.Run(ctx, out state);
        }
    }
}