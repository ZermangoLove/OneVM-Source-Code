using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal class Nop : IOpCode
    {
        public byte Code => Constants.OPCODELIST[44];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            state = ExecutionState.Next;
        }
    }
}