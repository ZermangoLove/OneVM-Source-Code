using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal class Exit : IVCall
    {
        public byte Code => Constants.OPCODELIST[180];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            state = ExecutionState.Exit;
        }
    }
}