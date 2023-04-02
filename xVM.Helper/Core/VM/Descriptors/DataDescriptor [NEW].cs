using System;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;

using dnlib.DotNet;

using xVM.Helper.Core.RT;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.Helpers;
using System.Security.Cryptography;

namespace xVM.Helper.Core.VM
{
    public class DataDescriptor
    {
        private readonly Dictionary<MethodDef, ulong> exportMap = new Dictionary<MethodDef, ulong>();
        private readonly Dictionary<MethodDef, VMMethodInfo> methodInfos = new Dictionary<MethodDef, VMMethodInfo>();

        private ulong ID = 0;
        private readonly RandomGenerator random;

        internal int Seed = 0;

        internal byte[] constantsMap = new byte[0];
        internal Dictionary<IMemberRef, ulong> refMap = new Dictionary<IMemberRef, ulong>();
        private readonly Dictionary<MethodSig, ulong> sigMap = new Dictionary<MethodSig, ulong>(SignatureEqualityComparer.Instance);
        internal List<FuncSigDesc> sigs = new List<FuncSigDesc>();
        internal Dictionary<string, ulong> strMap = new Dictionary<string, ulong>(StringComparer.Ordinal);

        internal DataDescriptor(RandomGenerator randomGenerator)
        {
            // 0 = null, 1 = ""
            strMap[string.Empty] = 1;
            ID = 2;

            this.random = randomGenerator;
        }

        #region Burası Referansları DATA'ya yüklüyor
        ////////////////////////////////////////////////////
        public ulong GetId(IMemberRef memberRef)
        {
            ulong ret;
            if (!refMap.TryGetValue(memberRef, out ret))
                refMap[memberRef] = ret = ID++;
            return ret;
        }
        ////////////////////////////////////////////////////
        #endregion

        #region Burası Stringleri DATA'ya yüklüyor
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ulong GetId(byte[] strdat)
        {
            byte[] _MAGIC_KEY = new byte[4];
            Array.Copy(constantsMap, 230, _MAGIC_KEY, 0, 4);

            var encryptedBuffer = VMRuntime.LdstrDeriver.Encrypt(strdat, _MAGIC_KEY);

            #region Phoenix Encoding
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var array = new byte[encryptedBuffer.Length];
            for (var i = 0; i < array.Length; i++)
                array[i] = (byte)((byte)(encryptedBuffer[i] >> 8 ^ i) << 8 | (byte)(encryptedBuffer[i] ^ encryptedBuffer.Length - i));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region XOR
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var result = new StringBuilder();
            var xorKey = BitConverter.GetBytes(Seed);
            for (int s = 0; s < array.Length; s++)
            {
                result.Append((char)((uint)array[s] ^ (uint)xorKey[s % xorKey.Length]));
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            ulong ret;
            if (!strMap.TryGetValue(result.ToString(), out ret))
                strMap[result.ToString()] = ret = ID++;
            return ret;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        public ulong GetId(ITypeDefOrRef declType, MethodSig methodSig)
        {
            ulong ret;
            if (!sigMap.TryGetValue(methodSig, out ret))
            {
                ulong id = ID++;
                sigMap[methodSig] = ret = id;
                sigs.Add(new FuncSigDesc(id, declType, methodSig));
            }
            return ret;
        }

        public ulong GetExportUlongID(MethodDef method)
        {
            ulong ret;
            if (!exportMap.TryGetValue(method, out ret))
            {
                ulong id = ID++;
                exportMap[method] = ret = id;
                sigs.Add(new FuncSigDesc(id, method));
            }
            return ret;
        }

        public string GetExportStringID(MethodDef method)
        {
            ulong ret;
            if (!exportMap.TryGetValue(method, out ret))
            {
                ulong id = ID++;
                exportMap[method] = ret = id;
                sigs.Add(new FuncSigDesc(id, method));
            }
            return Convert.ToString(ret);
        }

        public void ReplaceReference(IMemberRef old, IMemberRef @new)
        {
            ulong id;
            if (!refMap.TryGetValue(old, out id))
                return;
            refMap.Remove(old);
            refMap[@new] = id;
        }

        public VMMethodInfo LookupInfo(MethodDef method)
        {
            VMMethodInfo ret;
            if (!methodInfos.TryGetValue(method, out ret))
            {
                var k = random.NextByte();
                ret = new VMMethodInfo
                {
                    EntryKey = k,
                    ExitKey = (byte)(k >> 8)
                };
                methodInfos[method] = ret;
            }
            return ret;
        }

        public void SetInfo(MethodDef method, VMMethodInfo info)
        {
            methodInfos[method] = info;
        }
    }
}