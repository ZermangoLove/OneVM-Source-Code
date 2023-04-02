using System;
using System.Reflection;

namespace xVM.Runtime.Data
{
    internal class VMFuncSig
    {
        private readonly int[] paramToks;
        private readonly int retTok;

        public byte Flags;

        private Type[] paramTypes;
        private Type retType;

        public unsafe VMFuncSig(ref byte* ptr)
        {
            Flags = *ptr++;
            paramToks = new int[Utils.ReadCompressedULong(ref ptr)];
            for(var i = 0; i < paramToks.Length; i++) paramToks[i] = (int) Utils.FromCodedToken(Utils.ReadCompressedULong(ref ptr));
            retTok = (int) Utils.FromCodedToken(Utils.ReadCompressedULong(ref ptr));
        }

        public Type[] ParamTypes
        {
            get
            {
                if(paramTypes != null)
                    return paramTypes;

                var p = new Type[paramToks.Length];
                for(var i = 0; i < p.Length; i++) p[i] = VMInstance.__ExecuteModule.ResolveType(paramToks[i]);
                paramTypes = p;
                return p;
            }
        }

        public Type RetType => retType ?? (retType = VMInstance.__ExecuteModule.ResolveType(retTok));
    }
}