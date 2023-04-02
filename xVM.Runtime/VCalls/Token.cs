using System;
using System.Reflection;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Token : IVCall
    {
        public byte Code => Constants.OPCODELIST[200];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var typeSlot = ctx.Stack[sp];

            var reference = ctx.Instance.Data.LookupReference(typeSlot.U4);
            if(reference is Type)
                typeSlot.O = ValueTypeBox.Box(((Type) reference).TypeHandle, typeof(RuntimeTypeHandle));
            else if(reference is MethodBase)
                typeSlot.O = ValueTypeBox.Box(((MethodBase) reference).MethodHandle, typeof(RuntimeMethodHandle));
            else if(reference is FieldInfo)
                typeSlot.O = ValueTypeBox.Box(((FieldInfo) reference).FieldHandle, typeof(RuntimeFieldHandle));
            ctx.Stack[sp] = typeSlot;

            state = ExecutionState.Next;
        }
    }
}