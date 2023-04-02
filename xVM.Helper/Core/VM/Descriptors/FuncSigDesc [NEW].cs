using System;
using dnlib.DotNet;

namespace xVM.Helper.Core.VM
{
    internal class FuncSigDesc
    {
        public readonly ITypeDefOrRef DeclaringType;
        public readonly FuncSig FuncSig;
        public readonly ulong Id;
        public readonly MethodDef Method;
        public readonly MethodSig Signature;

        public FuncSigDesc(ulong id, MethodDef method)
        {
            Id = id;
            Method = method;
            DeclaringType = method.DeclaringType;
            Signature = method.MethodSig;
            FuncSig = new FuncSig();
        }

        public FuncSigDesc(ulong id, ITypeDefOrRef declType, MethodSig sig)
        {
            Id = id;
            Method = null;
            DeclaringType = declType;
            Signature = sig;
            FuncSig = new FuncSig();
        }
    }
}
