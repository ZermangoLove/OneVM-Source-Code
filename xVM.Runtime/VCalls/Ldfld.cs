using System;
using System.Reflection;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.VCalls
{
    internal unsafe class Ldfld : IVCall
    {
        public byte Code => Constants.OPCODELIST[196];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var fieldSlot = ctx.Stack[sp--];
            var objSlot = ctx.Stack[sp];

            var addr = (fieldSlot.U4 & 0x80000000) != 0;
            var field = (FieldInfo) ctx.Instance.Data.LookupReference(fieldSlot.U4 & 0x7fffffff);
            if(!field.IsStatic && objSlot.O == null)
                throw new NullReferenceException();

            if(addr)
            {
                ctx.Stack[sp] = new VMSlot {O = new FieldRef(objSlot.O, field)};
            }
            else
            {
                object instance;
                if(field.DeclaringType.IsValueType && objSlot.O is IReference)
                    instance = ((IReference) objSlot.O).GetValue(ctx, PointerType.OBJECT).ToObject(field.DeclaringType);
                else
                    instance = objSlot.ToObject(field.DeclaringType);
                ctx.Stack[sp] = ctx.Registers[Constants.OPCODELIST[222]].FromObject(field.GetValue(instance), field.FieldType);
            }

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}