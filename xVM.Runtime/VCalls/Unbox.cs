using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Unbox : IVCall
    {
        public byte Code => Constants.OPCODELIST[210];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var typeSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp];

            var unboxPtr = (typeSlot.U4 & 0x80000000) != 0;
            var valType = (Type) ctx.Instance.Data.LookupReference(typeSlot.U4 & ~0x80000000);
            if(unboxPtr)
            {
                unsafe
                {
                    TypedReference typedRef;
                    TypedReferenceHelpers.UnboxTypedRef(valSlot.O, &typedRef);
                    var reference = new TypedRef(typedRef);
                    valSlot = ctx.Registers[Constants.OPCODELIST[222]].FromObject(valSlot.O, valType);
                    ctx.Stack[sp] = valSlot;
                }
            }
            else
            {
                if(valType == typeof(object) && valSlot.O != null)
                    valType = valSlot.O.GetType();
                valSlot = ctx.Registers[Constants.OPCODELIST[222]].FromObject(valSlot.O, valType);
                ctx.Stack[sp] = valSlot;
            }

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}