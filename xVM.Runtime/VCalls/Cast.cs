using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Cast : IVCall
    {
        public byte Code => Constants.OPCODELIST[186];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var typeSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp];

            var castclass = (typeSlot.U4 & 0x80000000) != 0;
            var castType = (Type) ctx.Instance.Data.LookupReference(typeSlot.U4 & ~0x80000000);
            if(Type.GetTypeCode(castType) == TypeCode.String && valSlot.O == null)
            {
                valSlot.O = ctx.Instance.Data.LookupString(valSlot.U4);
            }
            else if(valSlot.O == null)
            {
                valSlot.O = null;
            }
            else if(!castType.IsInstanceOfType(valSlot.O))
            {
                valSlot.O = null;
                if(castclass)
                    throw new InvalidCastException();
            }
            ctx.Stack[sp] = valSlot;

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}