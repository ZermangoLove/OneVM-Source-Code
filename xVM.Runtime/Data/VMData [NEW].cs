using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.IO.Compression;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

using xVM.Runtime.Services;
using xVM.Runtime.Execution;
using xVM.Runtime.Protection;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.Data
{
    internal unsafe class VMData
    {
        private static readonly Dictionary<Module, VMData> ModuleVMDATA = new Dictionary<Module, VMData>();
        private static readonly Dictionary<ulong, RefInfo> References = new Dictionary<ulong, RefInfo>();
        private static readonly Dictionary<ulong, byte[]> Strings = new Dictionary<ulong, byte[]>();
        private static readonly Dictionary<ulong, VMExportInfo> Exports = new Dictionary<ulong, VMExportInfo>();

        [VMProtect.BeginMutation]
        private VMData()
        {
            try
            {
                if (Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) && AntiDumpV2.AntiDumpIsRunning == true && Utils.AntiTamperChecker == null)
                {
                    var ptr = Interpreter.__ILVDATA + SizeOfHelper.SizeOf(typeof(VMDATA_HEADER));

                    #region Burası VMDATA'dan Referansları Okuyor
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    for (var x = 0; x < ((VMDATA_HEADER*)Interpreter.__ILVDATA)->MD_COUNT; x++)
                    {
                        var id = Utils.ReadCompressedULong(ref ptr);
                        var token = (int)Utils.FromCodedToken(Utils.ReadCompressedULong(ref ptr));
                        References[id] = new RefInfo
                        {
                            Token = token
                        };
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Burası VMDATA'dan Stringleri Okuyor
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    for (var x = 0; x < ((VMDATA_HEADER*)Interpreter.__ILVDATA)->STR_COUNT; x++)
                    {
                        var id = Utils.ReadCompressedULong(ref ptr);
                        var strlength = Utils.ReadCompressedULong(ref ptr);

                        #region XOR
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        var result = new List<char>();
                        var xorKey = BitConverter.GetBytes(int.Parse(Mutation.LdstrKey1));
                        for (int g = 0; g < (int)strlength; g++)
                        {
                            result.Add((char)((uint)new string((char*)ptr, 0, (int)strlength)[g] ^ (uint)xorKey[g % xorKey.Length]));
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Phoenix Encoding
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        var dat = new byte[result.ToArray().Length];
                        for (var i = 0; i < dat.Length; i++)
                            dat[i] = (byte)((byte)(result[i] >> 8 ^ i) << 8 | (byte)(result[i] ^ dat.Length - i));
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Decompress and Decrypt
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        uint[] q = new uint[dat.Length / 4];
                        Buffer.BlockCopy(dat, 0, q, 0, dat.Length);

                        byte[] _MAGIC_KEY = BitConverter.GetBytes(((VMDATA_HEADER*)Interpreter.__ILVDATA)->MAGIC);
                        var state = Murmur2.Hash(_MAGIC_KEY, (ulong)_MAGIC_KEY.LongLength, CRC32.CheckSum(_MAGIC_KEY) ^ (ulong)int.Parse(Mutation.LdstrKey0));
                        var key = new uint[0x10];
                        for (int i = 0; i < 0x10; i++)
                        {
                            state ^= state >> 13;
                            state ^= state << 25;
                            state ^= state >> 27;
                            key[i] = (uint)state;
                        }

                        for (int i = 0; i < dat.Length; i++)
                        {
                            dat[i] ^= (byte)state;
                            if ((i & 0xff) == 0)
                                state = (state * state) % 0x8a5cb7;
                        }

                        int s = 0, d = 0;
                        var l = (uint)q.Length;
                        var w = new uint[0x10];
                        var o = new byte[l * 4];
                        while (s < l)
                        {
                            for (int j = 0; j < 0x10; j++)
                                w[j] = q[s + j];
                            Mutation.Crypt(w, key);
                            for (int j = 0; j < 0x10; j++)
                            {
                                uint e = w[j];
                                o[d++] = (byte)e;
                                o[d++] = (byte)(e >> 8);
                                o[d++] = (byte)(e >> 16);
                                o[d++] = (byte)(e >> 24);
                                key[j] ^= e;
                            }
                            s += 0x10;
                        }

                        #region RC4 Decrypt
                        ////////////////////////////////////////////////////////////////////////////////
                        var f = new byte[256];
                        var k = new byte[256];
                        byte temp;
                        int p, u;

                        for (p = 0; p < 256; p++)
                        {
                            f[p] = (byte)p;
                            k[p] = _MAGIC_KEY[p % _MAGIC_KEY.GetLength(0)];
                        }

                        u = 0;
                        for (p = 0; p < 256; p++)
                        {
                            u = (u + f[p] + k[p]) % 256;
                            temp = f[p];
                            f[p] = f[u];
                            f[u] = temp;
                        }

                        p = u = 0;
                        for (int c = 0; c < o.GetLength(0); c++)
                        {
                            p = (p + 1) % 256;
                            u = (u + f[p]) % 256;
                            temp = f[p];
                            f[p] = f[u];
                            f[u] = temp;
                            int t = (f[p] + f[u]) % 256;
                            o[c] ^= f[t];
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        var decompressedMs = new MemoryStream();
                        using (var gzs = new BufferedStream(new GZipStream(new MemoryStream(o), CompressionMode.Decompress)))
                        {
                            int bufSize = 1024, count;
                            Strings[id] = new byte[bufSize];
                            count = gzs.Read(Strings[id], 0, bufSize);
                            while (count > 0)
                            {
                                decompressedMs.Write(Strings[id], 0, count);
                                count = gzs.Read(Strings[id], 0, bufSize);
                            }
                        }
                        Strings[id] = decompressedMs.ToArray();
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        ptr += strlength << 1;
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    for (var x = 0; x < ((VMDATA_HEADER*)Interpreter.__ILVDATA)->EXP_COUNT; x++)
                    {
                        Exports[Utils.ReadCompressedULong(ref ptr)] = new VMExportInfo(ref ptr);
                    }

                    ModuleVMDATA[VMInstance.__ExecuteModule] = this;
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        [VMProtect.BeginMutation]
        public static VMData Instance()
        {
            VMData data;
            lock (ModuleVMDATA)
            {
                if (!ModuleVMDATA.TryGetValue(VMInstance.__ExecuteModule, out data))
                {
                    if (!BitConverter.IsLittleEndian)
                    {
                        throw new PlatformNotSupportedException();
                    }
                    else
                    {
                        data = ModuleVMDATA[VMInstance.__ExecuteModule] = new VMData();
                    }
                }
            }
            return data;
        }

        public MemberInfo LookupReference(ulong id)
        {
            return References[id].Member();
        }

        [VMProtect.BeginMutation]
        public string LookupString(ulong id)
        {
            if (Utils.AntiTamperChecker == null)
            {
                if (id == 0)
                    return null;
                else
                    using (var reader = new StreamReader(new MemoryStream(Strings[id]), true))
                    {
                        reader.Peek(); // you need this!
                        return reader.CurrentEncoding.GetString(Strings[id]);
                    }
            }
            return string.Empty;
        }

        public VMExportInfo LookupExport(ulong id)
        {
            return Exports[unchecked(id)];
        }

        public VMExportInfo LookupExport(VMSlot slot)
        {
            return Exports[unchecked(slot.U8)];
        }
    }
}