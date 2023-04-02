using System;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.DynCipher;
using xVM.Helper.Core.Services;
using xVM.Helper.DynCipher.AST;
using xVM.Helper.Core.Helpers.System;
using xVM.Helper.DynCipher.Generation;

namespace xVM.Helper.Core.RT
{
    internal class LdstrDynamicDeriver
    {
        public int MurmurSeed = 0;
        Action<uint[], uint[]> encryptFunc;
        VMRuntime VMRT;

        public IEnumerable<Instruction> EmitDecrypt(MethodDef init, VMRuntime runtime, Local block, Local key)
        {
            MurmurSeed = BitConverter.ToInt32(Encoding.Default.GetBytes(init.Name), 0);
            VMRT = runtime;

            StatementBlock encrypt, decrypt;
            new DynCipherService().GenerateCipherPair(runtime.Descriptor.RandomGenerator, out encrypt, out decrypt);
            var ret = new List<Instruction>();

            var codeGen = new CodeGen(block, key, init, ret);
            codeGen.GenerateCIL(decrypt);
            codeGen.Commit(init.Body);

            var dmCodeGen = new DMCodeGen(typeof(void), new[] {
                Tuple.Create("{BUFFER}", typeof(uint[])),
                Tuple.Create("{KEY}", typeof(uint[]))
            });
            dmCodeGen.GenerateCIL(encrypt);
            encryptFunc = dmCodeGen.Compile<Action<uint[], uint[]>>();

            return ret;
        }

        public byte[] Encrypt(byte[] strdat, byte[] _MAGIC_KEY)
        {
            var moduleBuff = Compress(strdat);

            #region RC4 Encrypt
            ////////////////////////////////////////////////////////////////////////////////
            int num;
            byte temp;
            var buffA = new byte[0x100];
            var buffB = new byte[0x100];
            for (num = 0; num < 0x100; num++)
            {
                buffA[num] = (byte)num;
                buffB[num] = _MAGIC_KEY[num % _MAGIC_KEY.GetLength(0)];
            }
            int index = 0;
            for (num = 0; num < 0x100; num++)
            {
                index = ((index + buffA[num]) + buffB[num]) % 0x100;
                temp = buffA[num];
                buffA[num] = buffA[index];
                buffA[index] = temp;
            }
            num = index = 0;
            for (int i = 0; i < moduleBuff.GetLength(0); i++)
            {
                num = (num + 1) % 0x100;
                index = (index + buffA[num]) % 0x100;
                temp = buffA[num];
                buffA[num] = buffA[index];
                buffA[index] = temp;
                int num5 = (buffA[num] + buffA[index]) % 0x100;
                moduleBuff[i] = (byte)(moduleBuff[i] ^ buffA[num5]);
            }
            ////////////////////////////////////////////////////////////////////////////////
            #endregion

            uint compressedLen = (uint)(moduleBuff.Length + 3) / 4;
            compressedLen = (compressedLen + 0xfu) & ~0xfu;
            var compressedBuff = new uint[compressedLen];
            Buffer.BlockCopy(moduleBuff, 0, compressedBuff, 0, moduleBuff.Length);

            var state = Murmur2.Hash(_MAGIC_KEY, (ulong)_MAGIC_KEY.LongLength, CRC32.CheckSum(_MAGIC_KEY) ^ (ulong)MurmurSeed);

            var key = new uint[0x10];
            for (int i = 0; i < 0x10; i++)
            {
                state ^= state >> 13;
                state ^= state << 25;
                state ^= state >> 27;
                key[i] = (uint)state;
            }

            var z = (uint)(state % 0x8a5cb7);
            for (int i = 0; i < moduleBuff.Length; i++)
            {
                moduleBuff[i] ^= (byte)state;
                if ((i & 0xff) == 0)
                    state = (state * state) % 0x8a5cb7;
            }

            var encryptedBuffer = new byte[compressedBuff.Length * 4];
            int buffIndex = 0;
            while (buffIndex < compressedBuff.Length)
            {
                uint[] enc = DeriveKey(compressedBuff, buffIndex, key);
                for (int j = 0; j < 0x10; j++)
                    key[j] ^= compressedBuff[buffIndex + j];
                Buffer.BlockCopy(enc, 0, encryptedBuffer, buffIndex * 4, 0x40);
                buffIndex += 0x10;
            }
            return encryptedBuffer;
        }

        private byte[] Compress(byte[] inputData)
        {
            using (var compressIntoMs = new MemoryStream())
            {
                using (var gzs = new BufferedStream(new GZipStream(compressIntoMs, CompressionMode.Compress)))
                {
                    gzs.Write(inputData, 0, inputData.Length);
                }
                return compressIntoMs.ToArray();
            }
        }

        private uint[] DeriveKey(uint[] data, int offset, uint[] key)
        {
            var ret = new uint[key.Length];
            Buffer.BlockCopy(data, offset * sizeof(uint), ret, 0, key.Length * sizeof(uint));
            encryptFunc(ret, key);
            return ret;
        }

        class CodeGen : CILCodeGen
        {
            readonly Local block;
            readonly Local key;

            public CodeGen(Local block, Local key, MethodDef init, IList<Instruction> instrs)
             : base(init, instrs)
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
