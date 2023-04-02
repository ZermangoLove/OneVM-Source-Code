using System;
using System.Runtime.InteropServices;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.Execution
{
    internal unsafe class TypedRef : IReference
    {
        private TypedRefPtr? _ptr;
        private readonly PseudoTypedRef _typedRef;

        public TypedRef(TypedRefPtr ptr)
        {
            _ptr = ptr;
        }

        public TypedRef(TypedReference typedRef)
        {
            _ptr = null;
            _typedRef = *(PseudoTypedRef*) &typedRef;
        }

        public VMSlot GetValue(VMContext ctx, PointerType type)
        {
            TypedReference typedRef;
            if(_ptr != null)
                *&typedRef = *(TypedReference*) _ptr.Value;
            else
                *(PseudoTypedRef*) &typedRef = _typedRef;
            return ctx.Registers[Constants.OPCODELIST[222]].FromObject(TypedReference.ToObject(typedRef), __reftype(typedRef));
        }

        public void SetValue(VMContext ctx, VMSlot slot, PointerType type)
        {
            TypedReference typedRef;
            if(_ptr != null)
                *&typedRef = *(TypedReference*) _ptr.Value;
            else
                *(PseudoTypedRef*) &typedRef = _typedRef;

            var refType = __reftype(typedRef);
            var value = slot.ToObject(refType);
            TypedReferenceHelpers.SetTypedRef(value, &typedRef);
        }

        public IReference Add(uint value)
        {
            return this;
        }

        public IReference Add(ulong value)
        {
            return this;
        }

        public void ToTypedReference(VMContext ctx, TypedRefPtr typedRef, Type type)
        {
            if(_ptr != null)
                *(TypedReference*) typedRef = *(TypedReference*) _ptr.Value;
            else
                *(PseudoTypedRef*) typedRef = _typedRef;
        }

        // TODO: compat with mono?
        [StructLayout(LayoutKind.Sequential)]
        private struct PseudoTypedRef
        {
            public readonly IntPtr Type;
            public readonly IntPtr Value;
        }
    }
}