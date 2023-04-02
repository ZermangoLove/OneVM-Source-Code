using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class Try : IOpCode
    {
        public byte Code => Constants.OPCODELIST[176];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var type = ctx.Stack[sp--].U1;

            var frame = new EHFrame();
            frame.EHType = type;
            if(type == Constants.OPCODELIST[226]) frame.CatchType = (Type) ctx.Instance.Data.LookupReference(ctx.Stack[sp--].U4);
            else if(type == Constants.OPCODELIST[228]) frame.FilterAddr = ctx.Stack[sp--].U8;
            frame.HandlerAddr = ctx.Stack[sp--].U8;

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            frame.BP = ctx.Registers[Constants.OPCODELIST[14]];
            frame.SP = ctx.Registers[Constants.OPCODELIST[16]];
            ctx.EHStack.Add(frame);

            state = ExecutionState.Next;
        }
    }
}