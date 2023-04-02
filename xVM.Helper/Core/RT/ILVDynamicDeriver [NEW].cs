using System;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.Services;

using xVM.Helper.DynCipher;
using xVM.Helper.DynCipher.AST;
using xVM.Helper.DynCipher.Generation;
using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core.RT {
    internal class ILVDynamicDeriver
    {
        public int Seed = 0;
        public int MurmurSeed = 0;

        StatementBlock derivation;
        VMRuntime VMRT;
        Action<uint[], uint[]> encryptFunc;

        public void Initialize(VMRuntime runtime)
        {
            Seed = BitConverter.ToInt32(Encoding.Default.GetBytes(runtime.RTSearch.Interpreter_GetInternalVData.Name), 0);
            MurmurSeed = BitConverter.ToInt32(Encoding.Default.GetBytes(runtime.RTSearch.Interpreter_Set_Constants.Name), 0);

            StatementBlock dummy;
            new DynCipherService().GenerateCipherPair(runtime.Descriptor.RandomGenerator, out derivation, out dummy);

            var dmCodeGen = new DMCodeGen(typeof(void), new[] {
                Tuple.Create("{BUFFER}", typeof(uint[])),
                Tuple.Create("{KEY}", typeof(uint[]))
            });
            dmCodeGen.GenerateCIL(derivation);
            VMRT = runtime;
            encryptFunc = dmCodeGen.Compile<Action<uint[], uint[]>>();
        }

        public byte[] Encrypt(byte[] data, int offset)
        {
            var dst = new uint[0x10];
            var src = new uint[0x10];
            var arraySeed = BitConverter.GetBytes(Seed);
            var state = Murmur2.Hash(arraySeed, (ulong)arraySeed.LongLength, CRC32.CheckSum(arraySeed) ^ (ulong)MurmurSeed);
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

            var key = new uint[src.Length];
            Buffer.BlockCopy(dst, offset * sizeof(uint), key, 0, src.Length * sizeof(uint));
            encryptFunc(key, src);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= (byte)state;
                if ((i & 0xff) == 0)
                    state = (state * state) % 0x8a5cb7;
            }

            #region PolyDex Encrypt (byte[] data, byte[] arraySeed)
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            byte[] expandedKey;
            if (arraySeed.Length >= data.Length)
                expandedKey = arraySeed;
            else
            {
                byte[] rconst = BitConverter.GetBytes(Math.Round((double)3.1415926535897931, 3));
                byte[] result = new byte[data.Length];
                Buffer.BlockCopy(arraySeed, 0, result, 0, arraySeed.Length);
                for (int j = arraySeed.Length; j < data.Length; j++)
                    result[j] = (byte)((arraySeed[(j - arraySeed.Length) % arraySeed.Length] ^ result[j - 1]) % 0x100);
                for (int k = 0; k < 5; k++)
                {
                    result[0] = (byte)(result[0] ^ rconst[k]);
                    for (int m = 1; m < result.Length; m++)
                    {
                        result[m] = (byte)(((result[m] ^ ((byte)(rconst[k] << (m % 3)))) ^ result[m - 1]) % 0x100);
                    }
                }
                expandedKey = result;
            }
            byte magic = (byte)new RandomGenerator().NextInt32(0xff);
            Array.Resize(ref data, data.Length + 1);
            data[data.Length - 1] = magic;
            for (int i = 0; i < (data.Length - 1); i++)
                data[i] = (byte)((data[i] ^ expandedKey[i]) ^ magic);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region LZMA Compress
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            data = VMRT.CompressionService.LZMA_Compress(data);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            Array.Resize(ref data, (data.Length + 3) & ~3);

            var encryptedData = new byte[data.Length];
            int keyIndex = 0;
            for (int i = 0; i < data.Length; i += 4)
            {
                var datum = (uint)(data[i + 0] | (data[i + 1] << 8) | (data[i + 2] << 16) | (data[i + 3] << 24));
                uint encrypted = datum ^ key[keyIndex & 0xf];
                key[keyIndex & 0xf] = (key[keyIndex & 0xf] ^ datum) + 0x3ddb2819;
                encryptedData[i + 0] = (byte)(encrypted >> 0);
                encryptedData[i + 1] = (byte)(encrypted >> 8);
                encryptedData[i + 2] = (byte)(encrypted >> 16);
                encryptedData[i + 3] = (byte)(encrypted >> 24);
                keyIndex++;
            }
            return encryptedData;
        }

        public IEnumerable<Instruction> EmitDecrypt(MethodDef method, Local dst, Local src)
        {
            var ret = new List<Instruction>();
            var codeGen = new CodeGen(dst, src, method, ret);
            codeGen.GenerateCIL(derivation);
            codeGen.Commit(method.Body);
            return ret;
        }

        class CodeGen : CILCodeGen
        {
            readonly Local block;
            readonly Local key;

            public CodeGen(Local block, Local key, MethodDef method, IList<Instruction> instrs)
             : base(method, instrs)
            {
                this.block = block;
                this.key = key;
            }

            protected override Local Var(Variable var)
            {
                if (var.Name == "{BUFFER}")
                    return block;
                if (var.Name == "{KEY}")
                    return key;
                return base.Var(var);
            }
        }
    }
}