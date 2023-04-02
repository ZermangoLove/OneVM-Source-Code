using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.IO.Compression;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using xVM.Runtime.Services;
using xVM.Runtime.Protection;
using xVM.Runtime.Execution.Internal;

using static Lzma;

namespace xVM.Runtime
{
    internal unsafe sealed class Interpreter
    {
        [VMProtect.BeginUltra]
        static Interpreter()
        {
            #region Anti Dump Protection For RT
            ////////////////////////////////////////////
            AntiDumpV2.Initialize();
            ////////////////////////////////////////////
            #endregion

            #region Run Active ILVData ("ByteArray")
            //////////////////////////////////////////////////////////
            if (ILVData_Array == null && Utils.AntiTamperChecker == null)
                GetInternalVData();
            //////////////////////////////////////////////////////////
            #endregion

            #region Run Active ILVMData Data "Set_VMDATA"
            //////////////////////////////////////////////////////////
            if (__ILVDATA == null && Utils.AntiTamperChecker == null)
                Set_VMDATA();
            //////////////////////////////////////////////////////////
            #endregion

            #region Run Active Constants Data "Set_Constants"
            //////////////////////////////////////////////////////////
            if (__CONSTDATA == null && Utils.AntiTamperChecker == null)
                Set_Constants();
            //////////////////////////////////////////////////////////
            #endregion
        }

        private static byte[] ILVData_Array = null;

        public static byte* __ILVDATA
        {
            get;
            set;
        } = null;

        public static byte[] __CONSTDATA
        {
            get;
            set;
        } = null;

        private static void GetInternalVData()
        {
            try
            {
                if (Equals((Assembly)typeof(Assembly).GetMethod("GetExecutingAssembly").Invoke(null, null),
                    Assembly.GetExecutingAssembly()) && Equals(Assembly.GetCallingAssembly(),
                    Assembly.GetExecutingAssembly()) && AntiDumpV2.AntiDumpIsRunning == true &&
                    Utils.AntiTamperChecker == null)
                {
                    uint[] The_Data = new uint[int.Parse(Mutation.LdstrKey0)];
                    RuntimeHelpers.InitializeArray(The_Data, typeof(Interpreter).GetField(Mutation.LdstrKey1, BindingFlags.NonPublic | BindingFlags.Static).FieldHandle);

                    #region DeCrypt And DeCompress DATA
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var dst = new uint[0x10];
                    var src = new uint[0x10];
                    var arraySeed = BitConverter.GetBytes(int.Parse(Mutation.LdstrKey2));
                    var state = Murmur2.Hash(arraySeed, (ulong)arraySeed.LongLength, CRC32.CheckSum(arraySeed) ^ (ulong)int.Parse(Mutation.LdstrKey3));
                    for (int i = 0; i < 0x10; i++)
                    {
                        state = (state * state) % 0x143fc089;
                        src[i] = (uint)state;
                        dst[i] = (uint)((state * state) % 0x444d56fb);
                    }

                    for (int i = 0; i < 0x10; i++)
                    {
                        state ^= state >> 13;
                        state ^= state << 25;
                        state ^= state >> 27;
                        src[i] = (uint)state;
                    }

                    Mutation.Crypt(dst, src);
                    Array.Clear(src, 0, 0x10);

                    var b = new byte[The_Data.Length << 2];
                    uint q = 0;
                    for (int i = 0; i < The_Data.Length; i++)
                    {
                        if (Utils.AntiTamperChecker == null)
                        {
                            uint d = The_Data[i] ^ dst[i & 0xf];
                            dst[i & 0xf] = (dst[i & 0xf] ^ d) + 0x3ddb2819;
                            b[q + 0] = (byte)(d >> 0);
                            b[q + 1] = (byte)(d >> 8);
                            b[q + 2] = (byte)(d >> 16);
                            b[q + 3] = (byte)(d >> 24);
                            q += 4;
                        }
                    }
                    Array.Clear(dst, 0, 0x10);

                    #region LZMA Decompress
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var m = new MemoryStream(b);
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
                    ILVData_Array = new byte[outSize];
                    var l = new MemoryStream(ILVData_Array, true);
                    long compressedSize = m.Length - 5 - SizeOfHelper.SizeOf(typeof(int));
                    decoder.Code(m, l, compressedSize, outSize);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    Array.Clear(b, 0, b.Length);

                    #region PolyDex Decrypt (byte[] ILVData, byte[] arraySeed)
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    byte[] expandedKey;
                    if (arraySeed.Length >= ILVData_Array.Length)
                        expandedKey = arraySeed;
                    else
                    {
                        byte[] rconst = BitConverter.GetBytes(Math.Round((double)3.1415926535897931, 3));
                        byte[] result = new byte[ILVData_Array.Length];
                        Buffer.BlockCopy(arraySeed, 0, result, 0, arraySeed.Length);
                        for (int i = arraySeed.Length; i < ILVData_Array.Length; i++)
                            result[i] = (byte)((arraySeed[(i - arraySeed.Length) % arraySeed.Length] ^ (result[i - 1])) % 256);
                        for (int round = 0; round < 5; round++)
                        {
                            result[0] = (byte)(result[0] ^ rconst[round]);
                            for (int i = 1; i < result.Length; i++)
                                result[i] = (byte)(((result[i] ^ (byte)(rconst[round] << (i % 3))) ^ result[i - 1]) % 256);
                        }
                        expandedKey = result;
                    }

                    var magic = ILVData_Array[ILVData_Array.Length - 1];
                    Array.Resize(ref ILVData_Array, ILVData_Array.Length - 1);
                    for (int i = 0; i < ILVData_Array.Length; i++)
                        ILVData_Array[i] = (byte)(ILVData_Array[i] ^ magic ^ expandedKey[i]);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    for (int i = 0; i < ILVData_Array.Length; i++)
                    {
                        ILVData_Array[i] ^= (byte)state;
                        if ((i & 0xff) == 0)
                            state = (state * state) % 0x8a5cb7;
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        [VMProtect.BeginMutation]
        private static void Set_VMDATA()
        {
            try
            {
                if (Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) &&
                    AntiDumpV2.AntiDumpIsRunning == true && Utils.AntiTamperChecker == null)
                {
                    byte[] Data = (byte[])ILVData_Array.Clone();
                    void* __PTRDATA = NativeMethods.malloc((ulong)Data.LongLength);

                    #region CopyMemory and Reverse Data
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    fixed (byte* ptr = Data)
                    {
                        PreventEdit<long> const_length = int.Parse(Mutation.LdstrKey0) - 4;
                        PreventEdit<long> count = Data.LongLength - const_length;
                        PreventEdit<long> block = count >> 3;

                        long* pDest = (long*)__PTRDATA;
                        long* pSrc = (long*)&ptr[const_length]; // "&ptr[const_length]" kısım yani "pSrc" Constantsı atlıyor.

                        #region Reverse Data
                        //////////////////////////////////////////////////////////////////
                        int ci = Data.GetLowerBound(0);
                        int za = Data.GetLowerBound(0) + Data.Length - 1;
                        object[] array2 = (Array)Data as object[];
                        if (array2 != null)
                        {
                            while (ci < za)
                            {
                                PreventEdit<byte> obj = (byte)array2[ci];
                                array2[ci] = array2[za];
                                array2[za] = obj;
                                ci++;
                                za--;
                            }
                            return;
                        }
                        while (ci < za)
                        {
                            object value = Data.GetValue(ci);
                            Data.SetValue(Data.GetValue(za), ci);
                            Data.SetValue(value, za);
                            ci++;
                            za--;
                        }
                        //////////////////////////////////////////////////////////////////
                        #endregion

                        for (int i = 0; i < block; i++)
                        {
                            *pDest = *pSrc; pDest++; pSrc++;
                        }

                        count = count - (block << 3);

                        if (count > 0)
                        {
                            byte* pDestB = (byte*)pDest;
                            byte* pSrcB = (byte*)pSrc;
                            for (var i = 0; i < count; i++)
                            {
                                *pDestB = *pSrcB; pDestB++; pSrcB++;
                            }
                        }

                        __ILVDATA = (byte*)__PTRDATA;
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        [VMProtect.BeginMutation]
        private static void Set_Constants()
        {
            try
            {
                if (Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) &&
                    AntiDumpV2.AntiDumpIsRunning == true && Utils.AntiTamperChecker == null)
                {
                    __CONSTDATA = (byte[])ILVData_Array.Clone();

                    #region Reverse Data
                    //////////////////////////////////////////////////////////////////
                    int ci = __CONSTDATA.GetLowerBound(0);
                    int za = __CONSTDATA.GetLowerBound(0) + __CONSTDATA.Length - 1;
                    object[] array2 = (Array)__CONSTDATA as object[];
                    if (array2 != null)
                    {
                        while (ci < za)
                        {
                            PreventEdit<byte> obj = (byte)array2[ci];
                            array2[ci] = array2[za];
                            array2[za] = obj;
                            ci++;
                            za--;
                        }
                    }
                    while (ci < za)
                    {
                        object value = __CONSTDATA.GetValue(ci);
                        __CONSTDATA.SetValue(__CONSTDATA.GetValue(za), ci);
                        __CONSTDATA.SetValue(value, za);
                        ci++;
                        za--;
                    }
                    //////////////////////////////////////////////////////////////////
                    #endregion

                    Buffer.BlockCopy(__CONSTDATA, 0, __CONSTDATA, 0, int.Parse(Mutation.LdstrKey0));

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    #region "VeryFastDecryption" Algorithm
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var hash = new System.Text.StringBuilder();
                    var arrayKey = BitConverter.GetBytes(int.Parse(Mutation.LdstrKey1));
                    var state = Murmur2.Hash(arrayKey, (ulong)arrayKey.LongLength, CRC32.CheckSum(arrayKey) ^ (ulong)int.Parse(Mutation.LdstrKey2));
                    byte[] crypto = new SHA256Managed().ComputeHash(BitConverter.GetBytes(state));
                    foreach (byte theByte in crypto)
                    {
                        hash.Append(theByte.ToString("x2"));
                    }
                    byte[] hashedPasswordBytes = System.Text.Encoding.ASCII.GetBytes(hash.ToString());
                    int passwordShiftIndex = 0;
                    bool shiftFlag = false;
                    for (int i = 0; i < __CONSTDATA.Length; i++)
                    {
                        int shift = hashedPasswordBytes[passwordShiftIndex];
                        __CONSTDATA[i] = shift <= 128
                            ? (byte)(__CONSTDATA[i] - (shiftFlag
                                ? (byte)(((shift << 2)) % 255)
                                : (byte)(((shift << 4)) % 255)))
                            : (byte)(__CONSTDATA[i] + (shiftFlag
                                ? (byte)(((shift << 4)) % 255)
                                : (byte)(((shift << 2)) % 255)));
                        passwordShiftIndex = (passwordShiftIndex + 1) % 64;
                        shiftFlag = !shiftFlag;
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }
    }
}
 