using System;
using System.IO;
using System.Diagnostics;

using dnlib.DotNet;
using dnlib.DotNet.MD;

using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.RT
{
    internal class HeaderChunk : IVMChunk
    {
        private byte[] data;

        public HeaderChunk(VMRuntime rt)
        {
            Length = ComputeLength(rt);
        }

        public ulong Length
        {
            get;
            set;
        }

        public void OnOffsetComputed(uint offset) { /* NULL */ }

        public byte[] GetData()
        {
            return data;
        }

        private ulong GetCodedLen(MDToken token)
        {
            switch (token.Table)
            {
                case Table.TypeDef:
                case Table.TypeRef:
                case Table.TypeSpec:
                case Table.MemberRef:
                case Table.Method:
                case Table.Field:
                case Table.MethodSpec:
                    return Utils.GetCompressedUIntLength(unchecked(token.Rid << 3));
                default:
                    throw new NotSupportedException();
            }
        }

        private uint GetCodedToken(MDToken token)
        {
            switch (token.Table)
            {
                case Table.TypeDef:
                    return (token.Rid << 3) | 1;
                case Table.TypeRef:
                    return (token.Rid << 3) | 2;
                case Table.TypeSpec:
                    return (token.Rid << 3) | 3;
                case Table.MemberRef:
                    return (token.Rid << 3) | 4;
                case Table.Method:
                    return (token.Rid << 3) | 5;
                case Table.Field:
                    return (token.Rid << 3) | 6;
                case Table.MethodSpec:
                    return (token.Rid << 3) | 7;
                default:
                    throw new NotSupportedException();
            }
        }

        private ulong ComputeLength(VMRuntime rt)
        {
            ulong len = 16;
            foreach (var reference in rt.Descriptor.Data.refMap)
            {
                len += Utils.GetCompressedUIntLength(unchecked(reference.Value)) + GetCodedLen(reference.Key.MDToken);
            }
            foreach (var str in rt.Descriptor.Data.strMap)
            {
                len += Utils.GetCompressedUIntLength(unchecked(str.Value));
                len += Utils.GetCompressedUIntLength(unchecked((ulong)str.Key.Length));
                len += (uint)str.Key.Length * 2;
            }
            foreach (var sig in rt.Descriptor.Data.sigs)
            {
                len += Utils.GetCompressedUIntLength(unchecked(sig.Id));
                len += 4;
                if (sig.Method != null)
                    len += 4;
                var paramCount = (uint)sig.FuncSig.ParamSigs.Length;
                len += 1 + Utils.GetCompressedUIntLength(unchecked(paramCount));

                foreach (var param in sig.FuncSig.ParamSigs)
                    len += GetCodedLen(param.MDToken);
                len += GetCodedLen(sig.FuncSig.RetType.MDToken);
            }
            return len;
        }

        internal void WriteData(VMRuntime rt)
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.Write(rt.Descriptor.Data.constantsMap);
            writer.Write(rt.Descriptor.Data.refMap.Count);
            writer.Write(rt.Descriptor.Data.strMap.Count);
            writer.Write(rt.Descriptor.Data.sigs.Count);

            foreach (var refer in rt.Descriptor.Data.refMap)
            {
                writer.WriteCompressedUInt(unchecked(refer.Value));
                writer.WriteCompressedUInt(unchecked(GetCodedToken(refer.Key.MDToken)));
            }

            foreach (var str in rt.Descriptor.Data.strMap)
            {
                writer.WriteCompressedUInt(unchecked(str.Value));
                writer.WriteCompressedUInt(unchecked((ulong)str.Key.Length));
                foreach (var chr in str.Key)
                    writer.Write((ushort)chr);
            }

            foreach (var sig in rt.Descriptor.Data.sigs)
            {
                writer.WriteCompressedUInt(unchecked(sig.Id));
                if (sig.Method != null)
                {
                    var entry = rt.MD_MAP[sig.Method].Item2;
                    var entryOffset = entry.Content[0].Offset;
                    Debug.Assert(entryOffset != 0);
                    writer.Write(entryOffset);

                    var key = rt.Descriptor.RandomGenerator.NextUInt32();
                    key = (key << 8) | rt.Descriptor.Data.LookupInfo(sig.Method).EntryKey;
                    writer.Write(key);
                }
                else
                {
                    writer.Write(0u);
                }

                writer.Write(sig.FuncSig.Flags);
                writer.WriteCompressedUInt(unchecked((ulong)sig.FuncSig.ParamSigs.Length));
                foreach (var paramType in sig.FuncSig.ParamSigs) writer.WriteCompressedUInt(unchecked(GetCodedToken(paramType.MDToken)));
                writer.WriteCompressedUInt(unchecked(GetCodedToken(sig.FuncSig.RetType.MDToken)));
            }

            data = stream.ToArray();
            Debug.Assert((ulong)data.Length == Length);
        }
    }
}