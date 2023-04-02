using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Ckfinite : IVCall
    {
        public byte Code => Constants.OPCODELIST[188];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp--];

            var fl = ctx.Registers[Constants.OPCODELIST[20]].U1;
            if((fl & Constants.OPCODELIST[38]) != 0)
            {
                var v = valueSlot.R4;
                if(float.IsNaN(v) || float.IsInfinity(v))
                    throw new ArithmeticException();
            }
            else
            {
                var v = valueSlot.R8;
                if(double.IsNaN(v) || double.IsInfinity(v))
                    throw new ArithmeticException();
            }

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}