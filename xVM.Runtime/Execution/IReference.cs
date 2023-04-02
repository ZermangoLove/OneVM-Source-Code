﻿namespace xVM.Runtime.Execution
{
    internal interface IReference
    {
        VMSlot GetValue(VMContext ctx, PointerType type);
        void SetValue(VMContext ctx, VMSlot slot, PointerType type);
        IReference Add(uint value);
        IReference Add(ulong value);

        void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, System.Type type);
    }
}