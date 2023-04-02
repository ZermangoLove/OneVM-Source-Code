namespace xVM.Runtime.VCalls
{
    internal interface IVCall
    {
        byte Code
        {
            get;
        }

        void Run(Execution.VMContext ctx, out Execution.ExecutionState state);
    }
}