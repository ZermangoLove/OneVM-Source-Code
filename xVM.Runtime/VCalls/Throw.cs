using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Throw : IVCall
    {
        public byte Code => Constants.OPCODELIST[202];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var type = ctx.Stack[sp--].U4;
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            if(type == 1)
                state = ExecutionState.Rethrow;
            else
                state = ExecutionState.Throw;
        }
    }
}