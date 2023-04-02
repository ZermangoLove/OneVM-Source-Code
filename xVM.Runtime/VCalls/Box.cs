using System;
using System.Diagnostics;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Box : IVCall
    {
        public byte Code => Constants.OPCODELIST[208];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var typeSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp];

            var valType = (Type) ctx.Instance.Data.LookupReference(typeSlot.U4);
            if(Type.GetTypeCode(valType) == TypeCode.String && valSlot.O == null)
            {
                valSlot.O = ctx.Instance.Data.LookupString(valSlot.U4);
            }
            else
            {
                Debug.Assert(valType.IsValueType);
                valSlot.O = valSlot.ToObject(valType);
            }
            ctx.Stack[sp] = valSlot;

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}