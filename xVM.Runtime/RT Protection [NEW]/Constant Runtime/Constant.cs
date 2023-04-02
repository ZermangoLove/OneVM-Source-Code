using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

using xVM.Runtime.Services;
using xVM.Runtime.Execution.Internal;

using static Lzma;

namespace xVM.Runtime.Protection
{
    internal static unsafe class Constant
    {
        private static byte[] b = null;

        [VMProtect.BeginUltra]
        static Constant()
        {
            if (b == null && Utils.AntiTamperChecker == null)
                Initialize();
        }

        [VMProtect.BeginMutation]
        public static void Initialize()
        {
            if (Equals((Assembly)typeof(Assembly).GetMethod("GetExecutingAssembly").Invoke(null, null), Assembly.GetExecutingAssembly()) &&
                Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) && Utils.AntiTamperChecker == null)
            {
                var l = (uint)Mutation.IntKey0;

                var sb = new StringBuilder();
                var bytes = Encoding.Unicode.GetBytes(Encoding.BigEndianUnicode.GetString(SHA1.Create().ComputeHash(BitConverter.GetBytes(Mutation.IntKey1))));
                foreach (var t in bytes)
                {
                    sb.Append(t.ToString("X2"));
                }

                var str = MethodBase.GetCurrentMethod().Module.Assembly.GetManifestResourceStream(sb.ToString());
                var a = new byte[str.Length];
                str.Read(a, 0, a.Length);

                uint[] q = new uint[l];
                Buffer.BlockCopy(a, 0, q, 0, a.Length);

                var arraySeed = BitConverter.GetBytes(int.Parse(VMProtect.SDK.DecryptString(Mutation.LdstrKey0)));
                var n = Murmur2.Hash(arraySeed, (ulong)arraySeed.LongLength, (ulong)Mutation.LongKey0);

                var k = new uint[0x10];
                for (int i = 0; i < 0x10; i++)
                {
                    n ^= n >> 12;
                    n ^= n << 25;
                    n ^= n >> 27;
                    k[i] = (uint)n;
                }

                int s = 0, d = 0;
                var w = new uint[0x10];
                var o = new byte[l * 4];
                while (s < l)
                {
                    for (int j = 0; j < 0x10; j++)
                        w[j] = q[s + j];
                    Mutation.Crypt(w, k);
                    for (int j = 0; j < 0x10; j++)
                    {
                        uint e = w[j];
                        o[d++] = (byte)e;
                        o[d++] = (byte)(e >> 8);
                        o[d++] = (byte)(e >> 16);
                        o[d++] = (byte)(e >> 24);
                        k[j] ^= e;
                    }
                    s += 0x10;
                }

                var m = new MemoryStream(o);
                var decoder = new LzmaDecoder();
                var prop = new byte[5];
                var readCnt = 0;
                while (readCnt < 5)
                {
                    readCnt += m.Read(prop, readCnt, 5 - readCnt);
                }
                decoder.SetDecoderProperties(prop);

                readCnt = 0;
                while (readCnt < SizeOfHelper.SizeOf(typeof(int)))
                {
                    readCnt += m.Read(prop, readCnt, SizeOfHelper.SizeOf(typeof(int)) - readCnt);
                }

                if (!BitConverter.IsLittleEndian)
                {
                    var i = prop.GetLowerBound(0);
                    var j = prop.GetLowerBound(0) + SizeOfHelper.SizeOf(typeof(int)) - 1;
                    var objArray = (Array)prop as object[];
                    if (objArray != null)
                    {
                        while (i < j)
                        {
                            PreventEdit<byte> temp = (byte)objArray[i];
                            objArray[i] = objArray[j];
                            objArray[j] = temp;
                            i++;
                            j--;
                        }
                    }
                    else
                    {
                        while (i < j)
                        {
                            var temp = prop.GetValue(i);
                            prop.SetValue(prop.GetValue(j), i);
                            prop.SetValue(temp, j);
                            i++;
                            j--;
                        }
                    }
                }

                var outSize = BitConverter.ToInt32(prop, 0);
                var h = new byte[outSize];
                var z = new MemoryStream(h, true);
                long compressedSize = m.Length - 5 - SizeOfHelper.SizeOf(typeof(int));
                decoder.Code(m, z, compressedSize, outSize);

                b = h;
            }
        }

        public static T Get<T>(uint id)
        {
            if (Equals((Assembly)typeof(Assembly).GetMethod("GetExecutingAssembly").Invoke(null, null), Assembly.GetExecutingAssembly()) &&
                Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) && Utils.AntiTamperChecker == null)
            {
                id = (uint)Mutation.Placeholder((int)id);
                uint t = id >> 30;

                T ret = default(T);
                id &= 0x3fffffff;
                id <<= 2;

                if (t == Mutation.IntKey0)
                {
                    int l = b[id++] | (b[id++] << 8) | (b[id++] << 16) | (b[id++] << 24);
                    ret = (T)(object)string.Intern(Encoding.UTF8.GetString(b, (int)id, l));
                }
                // NOTE: Assume little-endian
                else if (t == Mutation.IntKey1)
                {
                    var v = new T[1];
                    Buffer.BlockCopy(b, (int)id, v, 0, Mutation.Value<int>());
                    ret = v[0];
                }
                else if (t == Mutation.IntKey2)
                {
                    int s = b[id++] | (b[id++] << 8) | (b[id++] << 16) | (b[id++] << 24);
                    int l = b[id++] | (b[id++] << 8) | (b[id++] << 16) | (b[id++] << 24);
                    Array v = Array.CreateInstance(typeof(T).GetElementType(), l);
                    Buffer.BlockCopy(b, (int)id, v, 0, s - 4);
                    ret = (T)(object)v;
                }
                return ret;
            }
            return (T)(object)null;
        }
    }
}