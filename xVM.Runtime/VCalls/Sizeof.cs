using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Sizeof : IVCall
    {
        public byte Code => Constants.OPCODELIST[204];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var bp = ctx.Registers[Constants.OPCODELIST[14]].U4;
            var type = (Type) ctx.Instance.Data.LookupReference(ctx.Stack[sp].U4);
            ctx.Stack[sp] = new VMSlot
            {
                U4 = (uint) SizeOfHelper.SizeOf(type)
            };

            state = ExecutionState.Next;
        }
    }
}