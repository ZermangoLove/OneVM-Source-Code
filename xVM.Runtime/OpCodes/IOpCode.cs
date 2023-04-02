using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal interface IOpCode
    {
        byte Code
        {
            get;
        }

        void Run(VMContext ctx, out ExecutionState state);
    }
}