using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using dnlib.DotNet;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.VM;
using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;

using OpCode = dnlib.DotNet.Emit.OpCode;
using ReflOpCode = System.Reflection.Emit.OpCode;
using OpCodes = dnlib.DotNet.Emit.OpCodes;
using ReflOpCodes = System.Reflection.Emit.OpCodes;
using Code = dnlib.DotNet.Emit.Code;

namespace xVM.Helper.Core
{
    public static class Utils
    {
        private static Random Random = new Random(32);

        public static readonly char[] hexCharset = "0123456789ABCDEF".ToCharArray();
        public static readonly char[] unicodeCharset = new char[] { }
.Concat(Enumerable.Range(0x200b, 5).Select(ord => (char)ord))
.Concat(Enumerable.Range(0x2029, 6).Select(ord => (char)ord))
.Concat(Enumerable.Range(0x206a, 6).Select(ord => (char)ord))
.Except(new[] { '\u2029' })
.ToArray();

        public static ModuleWriterOptions ExecuteModuleWriterOptions;

        public static int CompressedUInt_Key0 = 0;
        public static int CompressedUInt_Key1 = 0;

        public static void AddListEntry<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> self, TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            HashSet<TValue> list;
            if (!self.TryGetValue(key, out list))
                list = self[key] = new HashSet<TValue>();
            list.Add(value);
        }

        public static void AddListEntry<TKey, TValue>(this IDictionary<TKey, List<TValue>> self, TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            List<TValue> list;
            if (!self.TryGetValue(key, out list))
                list = self[key] = new List<TValue>();
            list.Add(value);
        }

        public static StrongNameKey LoadSNKey(string path, string pass)
        {
            if (path == null) return null;

            try
            {
                if (pass != null) //pfx
                {
                    // http://stackoverflow.com/a/12196742/462805
                    var cert = new X509Certificate2();
                    cert.Import(path, pass, X509KeyStorageFlags.Exportable);

                    var rsa = cert.PrivateKey as RSACryptoServiceProvider;
                    if (rsa == null)
                        throw new ArgumentException("RSA key does not present in the certificate.", "path");

                    return new StrongNameKey(rsa.ExportCspBlob(true));
                }
                return new StrongNameKey(path);
            }
            catch (Exception ex)
            {
                NativeMethods.MessageBox("Cannot load the Strong Name Key located at: \"" + path + "\" LOG[" + ex + "]", "ERROR");
                throw new ProtectionException(ex);
            }
        }

        public static byte[] ReadAllBytes(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static void Increment<T>(this Dictionary<T, int> self, T key)
        {
            int count;
            if(!self.TryGetValue(key, out count))
                count = 0;
            self[key] = ++count;
        }

        public static IList<T> RemoveWhere<T>(this IList<T> self, System.Predicate<T> match)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                if (match(self[i]))
                    self.RemoveAt(i);
            }
            return self;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defValue = default(TValue))
        {
            TValue ret;
            if (dictionary.TryGetValue(key, out ret))
                return ret;
            return defValue;
        }

        public static TValue GetValueOrDefaultLazy<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> defValueFactory)
        {
            TValue ret;
            if (dictionary.TryGetValue(key, out ret))
                return ret;
            return defValueFactory(key);
        }

        public static byte[] Checker_HashUpdate(byte[] FileBytes)
        {
            var stream = new MemoryStream(FileBytes);
            var reader = new BinaryReader(stream);
            var noHash = reader.ReadBytes((int)stream.Length);
            var newHash = MD5.Create().ComputeHash(noHash);

            var a = BitConverter.ToString(reader.ReadBytes(16));
            var b = BitConverter.ToString(newHash);
            if (a == b)
            {
                reader.ReadBytes((int)stream.Length - 16);
            }

            var outstr = new MemoryStream();
            outstr.Write(noHash, 0, noHash.Length);
            outstr.Write(newHash, 0, newHash.Length);
            stream.Flush();
            outstr.Flush();

            var outBytes = outstr.ToArray();
            stream.Close();
            outstr.Close();

            return outBytes;
        }

        public static string ToHexString(this byte[] buff)
        {
            var ret = new char[buff.Length * 2];
            int i = 0;
            foreach (byte val in buff)
            {
                ret[i++] = hexCharset[val >> 4];
                ret[i++] = hexCharset[val & 0xf];
            }
            return new string(ret);
        }

        public static string ToHexString(this string str)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public static bool Contains(this byte[] toSearch, byte[] toFind)
        {
            for (var i = 0; i + toFind.Length < toSearch.Length; i++)
            {
                var allSame = true;
                for (var j = 0; j < toFind.Length; j++)
                {
                    if (toSearch[i + j] != toFind[j])
                    {
                        allSame = false;
                        break;
                    }
                }
                if (allSame)
                {
                    return true;
                }
            }
            return false;
        }


        public static string EncodeString(byte[] buff, char[] charset)
        {
            int current = buff[0];
            var ret = new StringBuilder();
            for (int i = 1; i < buff.Length; i++)
            {
                current = (current << 8) + buff[i];
                while (current >= charset.Length)
                {
                    current = Math.DivRem(current, charset.Length, out int remainder);
                    ret.Append(charset[remainder]);
                }
            }
            if (current != 0)
                ret.Append(charset[current % charset.Length]);
            return ret.ToString();
        }

        public static byte[] Xor(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1.Length != buffer2.Length)
                throw new ArgumentException("Length mismatched.");
            var ret = new byte[buffer1.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = (byte)(buffer1[i] ^ buffer2[i]);
            return ret;
        }

        public static byte[] SHA1(byte[] buffer)
        {
            var sha = new SHA1Managed();
            return sha.ComputeHash(buffer);
        }

        public static void CopyTo(this Stream scr, Stream dest)
        {
            int size = (scr.CanSeek) ? Math.Min((int)(scr.Length - scr.Position), 0x2000) : 0x2000;
            byte[] buffer = new byte[size];
            int n;
            do
            {
                n = scr.Read(buffer, 0, buffer.Length);
                dest.Write(buffer, 0, n);
            } while (n != 0);
        }

        public static List<Type> CreateTypeList(int size)
        {
            List<Type> retList = new List<Type>();
            while (size != 0)
            {
                if (16 <= size)
                {
                    size -= 16;
                    retList.Add(GetType(16));
                }
                else if (12 <= size)
                {
                    size -= 12;
                    retList.Add(GetType(12));
                }
                else if (8 <= size)
                {
                    size -= 8;
                    retList.Add(GetType(8));
                }
                else if (4 <= size)
                {
                    size -= 4;
                    retList.Add(GetType(4));
                }
                else if (2 <= size)
                {
                    size -= 2;
                    retList.Add(GetType(2));
                }
                else if (1 <= size)
                {
                    size -= 1;
                    retList.Add(GetType(1));
                }
            }
            return retList;
        }

        public static Type GetType(int operand)
        {
            switch (operand)
            {
                case 1:
                    switch (Random.Next(0, 3))
                    {
                        case 0: return typeof(Boolean);
                        case 1: return typeof(SByte);
                        case 2: return typeof(Byte);
                    }
                    break;
                case 2:
                    switch (Random.Next(0, 3))
                    {
                        case 0: return typeof(Int16);
                        case 1: return typeof(UInt16);
                        case 2: return typeof(Char);
                    }
                    break;
                case 4:
                    switch (Random.Next(0, 3))
                    {
                        case 0: return typeof(Int32);
                        case 1: return typeof(Single);
                        case 2: return typeof(UInt32);
                    }
                    break;
                case 8:
                    switch (Random.Next(0, 5))
                    {
                        case 0: return typeof(DateTime);
                        case 1: return typeof(TimeSpan);
                        case 2: return typeof(Int64);
                        case 3: return typeof(Double);
                        case 4: return typeof(UInt64);
                    }
                    break;

                case 12: return typeof(ConsoleKeyInfo);

                case 16:
                    switch (Random.Next(0, 2))
                    {
                        case 0: return typeof(Guid);
                        case 1: return typeof(Decimal);
                    }
                    break;
            }
            return null;
        }

        public static ReflOpCode ToReflectionOp(this OpCode op)
        {
            switch (op.Code)
            {
                case Code.Add: return ReflOpCodes.Add;
                case Code.Mul: return ReflOpCodes.Mul;
                case Code.Sub: return ReflOpCodes.Sub;
                case Code.And: return ReflOpCodes.And;
                case Code.Xor: return ReflOpCodes.Xor;
                case Code.Or: return ReflOpCodes.Or;
                case Code.Ldc_I4: return ReflOpCodes.Ldc_I4;
                case Code.Ldarg_0: return ReflOpCodes.Ldarg_0;
                case Code.Ret: return ReflOpCodes.Ret;
                default: throw new NotImplementedException();
            }
        }

        public static void Replace<T>(this List<T> list, int index, IEnumerable<T> newItems)
        {
            list.RemoveAt(index);
            list.InsertRange(index, newItems);
        }

        public static void Replace(this List<IRInstruction> list, int index, IEnumerable<IRInstruction> newItems)
        {
            var instr = list[index];
            list.RemoveAt(index);
            foreach(var i in newItems)
                i.ILAST = instr.ILAST;
            list.InsertRange(index, newItems);
        }

        public static bool IsGPR(this VMRegisters reg)
        {
            if(reg >= VMRegisters.R0 && reg <= VMRegisters.R7_Removed)
                return true;
            return false;
        }

        public static ulong GetCompressedUIntLength(ulong value)
        {
            ulong val = (value ^ ((ulong)CompressedUInt_Key0 + ((ulong)CompressedUInt_Key1 << 32)));
            uint len = 0;
            do
            {
                val >>= 7;
                len++;
            } while(val != 0);
            return len;
        }

        public unsafe static void WriteCompressedUInt(this BinaryWriter writer, ulong value)
        {
            ulong val = (value ^ ((ulong)CompressedUInt_Key0 + ((ulong)CompressedUInt_Key1 << 32)));
            do
            {
                var b = (byte)(val & 0x7f);
                val >>= 7;
                if (val != 0)
                    b |= 0x80;
                writer.Write(b);
            } while (val != 0);
        }

        internal static string GetAssemblyNameString(UTF8String name, Version version, UTF8String culture, PublicKeyBase publicKey, AssemblyAttributes attributes)
        {
            var sb = new StringBuilder();

            foreach (var c in UTF8String.ToSystemStringOrEmpty(name))
            {
                if (c == ',' || c == '=')
                    sb.Append('\\');
                sb.Append(c);
            }

            if (version != null)
            {
                sb.Append(", Version=");
                sb.Append(CreateVersionWithNoUndefinedValues(version).ToString());
            }

            if ((object)culture != null)
            {
                sb.Append(", Culture=");
                sb.Append(UTF8String.IsNullOrEmpty(culture) ? "neutral" : culture.String);
            }

            sb.Append(", ");
            sb.Append(publicKey == null || publicKey is PublicKeyToken ? "PublicKeyToken=" : "PublicKey=");
            sb.Append(publicKey == null ? "null" : publicKey.ToString());

            if ((attributes & AssemblyAttributes.Retargetable) != 0)
                sb.Append(", Retargetable=Yes");

            if ((attributes & AssemblyAttributes.ContentType_Mask) == AssemblyAttributes.ContentType_WindowsRuntime)
                sb.Append(", ContentType=WindowsRuntime");

            return sb.ToString();
        }

        internal static Version CreateVersionWithNoUndefinedValues(Version a)
        {
            if (a == null)
                return new Version(0, 0, 0, 0);
            return new Version(a.Major, a.Minor, GetDefaultVersionValue(a.Build), GetDefaultVersionValue(a.Revision));
        }

        static int GetDefaultVersionValue(int val)
        {
            return val == -1 ? 0 : val;
        }

        internal static TypeSig ResolveType(this GenericArguments genericArgs, TypeSig typeSig)
        {
            switch(typeSig.ElementType)
            {
                case ElementType.Ptr:
                    return new PtrSig(genericArgs.ResolveType(typeSig.Next));

                case ElementType.ByRef:
                    return new ByRefSig(genericArgs.ResolveType(typeSig.Next));

                case ElementType.SZArray:
                    return new SZArraySig(genericArgs.ResolveType(typeSig.Next));

                case ElementType.Array:
                    var arraySig = (ArraySig) typeSig;
                    return new ArraySig(genericArgs.ResolveType(typeSig.Next), arraySig.Rank, arraySig.Sizes, arraySig.LowerBounds);

                case ElementType.Pinned:
                    return new PinnedSig(genericArgs.ResolveType(typeSig.Next));

                case ElementType.Var:
                case ElementType.MVar:
                    return genericArgs.Resolve(typeSig);

                case ElementType.GenericInst:
                    var genInst = (GenericInstSig) typeSig;
                    var typeArgs = new List<TypeSig>();
                    foreach(var arg in genInst.GenericArguments)
                        typeArgs.Add(genericArgs.ResolveType(arg));
                    return new GenericInstSig(genInst.GenericType, typeArgs);

                case ElementType.CModReqd:
                    return new CModReqdSig(((CModReqdSig) typeSig).Modifier, genericArgs.ResolveType(typeSig.Next));

                case ElementType.CModOpt:
                    return new CModOptSig(((CModOptSig) typeSig).Modifier, genericArgs.ResolveType(typeSig.Next));

                case ElementType.ValueArray:
                    return new ValueArraySig(genericArgs.ResolveType(typeSig.Next), ((ValueArraySig) typeSig).Size);

                case ElementType.Module:
                    return new ModuleSig(((ModuleSig) typeSig).Index, genericArgs.ResolveType(typeSig.Next));
            }
            if(typeSig.IsTypeDefOrRef)
            {
                var s = (TypeDefOrRefSig) typeSig;
                if(s.TypeDefOrRef is TypeSpec)
                    throw new NotSupportedException(); // TODO: ?
            }
            return typeSig;
        }
    }
}