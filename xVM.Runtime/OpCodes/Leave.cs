using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class Leave : IOpCode
    {
        public byte Code => Constants.OPCODELIST[178];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var handler = ctx.Stack[sp--].U8;

            var frameIndex = ctx.EHStack.Count - 1;
            var frame = ctx.EHStack[frameIndex];

            if(frame.HandlerAddr != handler)
                throw new InvalidProgramException();
            ctx.EHStack.RemoveAt(frameIndex);

            if(frame.EHType == Constants.OPCODELIST[232])
            {
                ctx.Stack[++sp] = ctx.Registers[Constants.OPCODELIST[18]];
                ctx.Registers[Constants.OPCODELIST[22]].U1 = 0;
                ctx.Registers[Constants.OPCODELIST[18]].U8 = frame.HandlerAddr;
            }

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            state = ExecutionState.Next;
        }
    }
}