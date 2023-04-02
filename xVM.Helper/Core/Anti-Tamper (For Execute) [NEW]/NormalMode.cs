using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.RT;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.AntiTamperEXEC
{
    internal class NormalMode
    {
        AntiTamperEXEContext context;
        private ModuleDef Module;

        public NormalMode(AntiTamperEXEContext ctx, ModuleDef mod)
        {
            this.context = ctx;
            this.Module = mod;
        }

        private MethodDef initMethod;

        private DynamicDeriver deriver;
        private RandomGenerator random;
        private uint name1, name2, name3, name4;
        private uint z;
        private uint x;
        private uint c;
        private uint v;
        private uint EndKey;

        public void HandleRun(VMRuntime runtime, ModuleWriterOptions options)
        {
            if (context.Targets.Count != 0)
            {
                random = new RandomGenerator(32);

                z = random.NextUInt32();
                x = random.NextUInt32();
                c = random.NextUInt32();
                v = random.NextUInt32();

                name1 = random.NextUInt32() & 0x7f7f7f7f;
                name2 = random.NextUInt32() & 0x7f7f7f7f;
                name3 = random.NextUInt32() & 0x7f7f7f7f;
                name4 = random.NextUInt32() & 0x7f7f7f7f;

                var Keys = new uint[] { (name1 * name2), z, x, c, v };
                var arraySeed = new byte[Keys.Length * 4];
                int buffIndex = 0;
                foreach (uint dat in Keys) //make bytes from uint.
                {
                    arraySeed[buffIndex++] = (byte)((dat >> 0) & 0xff);
                    arraySeed[buffIndex++] = (byte)((dat >> 8) & 0xff);
                    arraySeed[buffIndex++] = (byte)((dat >> 16) & 0xff);
                    arraySeed[buffIndex++] = (byte)((dat >> 24) & 0xff);
                }
                Debug.Assert(buffIndex == arraySeed.Length);

                EndKey = BitConverter.ToUInt32(MD5.Create().ComputeHash(arraySeed), 0);

                deriver = new DynamicDeriver();
                deriver.Init(random);

                #region Rename Parameters
                //////////////////////////////////////////////////////////////
                foreach (var method in context.Targets)
                    foreach (var param in method.Parameters)
                        param.Name = runtime.RNMService.NewName(param.Name);
                //////////////////////////////////////////////////////////////
                #endregion

                context.Targets.Remove(Module.GlobalType.FindOrCreateStaticConstructor()); // Remove Module Ctor

                initMethod = runtime.RTSearch.AntiTamperEXEC_Initialize;
                initMethod.Body.SimplifyMacros(initMethod.Parameters);
                List<Instruction> instrs = initMethod.Body.Instructions.ToList();
                for (int i = 0; i < instrs.Count; i++)
                {
                    Instruction instr = instrs[i];
                    if (instr.OpCode == OpCodes.Ldtoken)
                    {
                        instr.Operand = Module.GlobalType;
                    }
                    else if (instr.OpCode == OpCodes.Call)
                    {
                        var method = (IMethod)instr.Operand;
                        if (method.DeclaringType.Name == RTMap.Mutation &&
                            method.Name == RTMap.Mutation_Crypt)
                        {
                            Instruction ldDst = instrs[i - 2];
                            Instruction ldSrc = instrs[i - 1];
                            Debug.Assert(ldDst.OpCode == OpCodes.Ldloc && ldSrc.OpCode == OpCodes.Ldloc);
                            instrs.RemoveAt(i);
                            instrs.RemoveAt(i - 1);
                            instrs.RemoveAt(i - 2);
                            instrs.InsertRange(i - 2, deriver.EmitDerivation(initMethod, (Local)ldDst.Operand, (Local)ldSrc.Operand));
                        }
                    }
                }
                initMethod.Body.Instructions.Clear();
                foreach (Instruction instr in instrs)
                    initMethod.Body.Instructions.Add(instr);

                //MutationHelper.InjectKeys_Int(initMethod,
                //              new[] { 0, 1, 2, 3, 4 },
                //              new[] { (int)(name1 * name2), (int)z, (int)x, (int)c, (int)v });

                MutationHelper.InjectKeys_String(initMethod,
                              new[] { 0, 1, 2, 3, 4 },
                              new[] { (name1 * name2).ToString(), z.ToString(), x.ToString(), c.ToString(), v.ToString() });

                #region HandleMD
                ///////////////////////////////////////////////
                options.WriterEvent += WriterEvent;
                ///////////////////////////////////////////////
                #endregion
            }
        }

        private void WriterEvent(object sender, ModuleWriterEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;
            if (e.Event == ModuleWriterEvent.MDEndCreateTables)
            {
                CreateSections(writer);

                #region Write Impl '[MethodImpl(MethodImplOptions.NoInlining)]'
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                foreach (var Def in writer.Module.GetTypes())
                {
                    foreach (var Custom_MD in context.Targets)
                    {
                        if (Def.Methods.Contains(Custom_MD))
                        {
                            MDToken Token = writer.Metadata.GetToken(Custom_MD);
                            RawMethodRow methodRow = writer.Metadata.TablesHeap.MethodTable[Token.Rid];
                            writer.Metadata.TablesHeap.MethodTable[Token.Rid] = new RawMethodRow(
                                methodRow.RVA,
                                (ushort)(methodRow.ImplFlags | (ushort)MethodImplAttributes.NoInlining),
                                methodRow.Flags,
                                methodRow.Name,
                                methodRow.Signature,
                                methodRow.ParamList);
                        }
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
            else if (e.Event == ModuleWriterEvent.BeginStrongNameSign)
            {
                EncryptSection(writer);
            }
            else if (e.Event == ModuleWriterEvent.End)
            {
                WriteTamperHash((ModuleWriterBase)sender);
            }
        }

        private void CreateSections(ModuleWriterBase writer)
        {
            var nameBufferA = new byte[8];
            nameBufferA[0] = (byte)(name1 >> 0);
            nameBufferA[1] = (byte)(name1 >> 8);
            nameBufferA[2] = (byte)(name1 >> 16);
            nameBufferA[3] = (byte)(name1 >> 24);
            nameBufferA[4] = (byte)(name2 >> 0);
            nameBufferA[5] = (byte)(name2 >> 8);
            nameBufferA[6] = (byte)(name2 >> 16);
            nameBufferA[7] = (byte)(name2 >> 24);

            var newSection = new PESection(Encoding.ASCII.GetString(nameBufferA), 0xE0000040);
            writer.Sections.Insert(0, newSection); // insert first to ensure proper RVA

            uint alignment;

            alignment = writer.TextSection.Remove(writer.Metadata).Value;
            writer.TextSection.Add(writer.Metadata, alignment);

            alignment = writer.TextSection.Remove(writer.NetResources).Value;
            writer.TextSection.Add(writer.NetResources, alignment);

            alignment = writer.TextSection.Remove(writer.Constants).Value;
            newSection.Add(writer.Constants, alignment);

            /* New ".rsrc" Name*/
            var nameBufferB = new byte[8];
            nameBufferB[0] = (byte)(name3 >> 0);
            nameBufferB[1] = (byte)(name3 >> 8);
            nameBufferB[2] = (byte)(name3 >> 16);
            nameBufferB[3] = (byte)(name3 >> 24);
            nameBufferB[4] = (byte)(name4 >> 0);
            nameBufferB[5] = (byte)(name4 >> 8);
            nameBufferB[6] = (byte)(name4 >> 16);
            nameBufferB[7] = (byte)(name4 >> 24);
            writer.RsrcSection.Name = Encoding.ASCII.GetString(nameBufferB);
            /////////////////////////////////////////////////////////////////////////

            // move some PE parts to separate section to prevent it from being hashed
            var peSection = new PESection(string.Empty, 0x60000020);
            bool moved = false;
            if (writer.StrongNameSignature != null)
            {
                alignment = writer.TextSection.Remove(writer.StrongNameSignature).Value;
                peSection.Add(writer.StrongNameSignature, alignment);
                moved = true;
            }
            var managedWriter = writer as ModuleWriter;
            if (managedWriter != null)
            {
                if (managedWriter.ImportAddressTable != null)
                {
                    alignment = writer.TextSection.Remove(managedWriter.ImportAddressTable).Value;
                    peSection.Add(managedWriter.ImportAddressTable, alignment);
                    moved = true;
                }
                if (managedWriter.StartupStub != null)
                {
                    alignment = writer.TextSection.Remove(managedWriter.StartupStub).Value;
                    peSection.Add(managedWriter.StartupStub, alignment);
                    moved = true;
                }
            }
            if (moved)
                writer.Sections.AddBeforeReloc(peSection);

            // move encrypted methods
            var encryptedChunk = new MethodBodyChunks(writer.TheOptions.ShareMethodBodies);
            newSection.Add(encryptedChunk, 4);
            foreach (var method in context.Targets)
            {
                var body = writer.Metadata.GetMethodBody(method);
                writer.MethodBodies.Remove(body);
                encryptedChunk.Add(body);
            }

            // padding to prevent bad size due to shift division
            newSection.Add(new ByteArrayChunk(new byte[4]), 4);
        }

        private void EncryptSection(ModuleWriterBase writer)
        {
            Stream stream = writer.DestinationStream;
            var reader = new BinaryReader(writer.DestinationStream);
            stream.Position = 0x3C;
            stream.Position = reader.ReadUInt32();

            stream.Position += 6;
            ushort sections = reader.ReadUInt16();
            stream.Position += 0xc;
            ushort optSize = reader.ReadUInt16();
            stream.Position += 2 + optSize;

            uint encLoc = 0, encSize = 0;
            int origSects = -1;
            if (writer is NativeModuleWriter && writer.Module is ModuleDefMD)
                origSects = ((ModuleDefMD)writer.Module).Metadata.PEImage.ImageSectionHeaders.Count;
            for (int i = 0; i < sections; i++)
            {
                uint nameHash;
                if (origSects > 0)
                {
                    origSects--;
                    stream.Write(new byte[8], 0, 8);
                    nameHash = 0;
                }
                else
                    nameHash = reader.ReadUInt32() * reader.ReadUInt32();
                stream.Position += 8;
                if (nameHash == name1 * name2)
                {
                    encSize = reader.ReadUInt32();
                    encLoc = reader.ReadUInt32();
                }
                else if (nameHash != 0)
                {
                    uint sectSize = reader.ReadUInt32();
                    uint sectLoc = reader.ReadUInt32();
                    Hash(stream, reader, sectLoc, sectSize);
                }
                else
                    stream.Position += 8;
                stream.Position += 16;
            }

            uint[] key = DeriveKey();
            encSize >>= 2;
            stream.Position = encLoc;
            var result = new uint[encSize];
            for (uint i = 0; i < encSize; i++)
            {
                uint data = reader.ReadUInt32();
                result[i] = data ^ key[i & 0xf];
                key[i & 0xf] = (key[i & 0xf] ^ data) + EndKey;
            }
            var byteResult = new byte[encSize << 2];
            Buffer.BlockCopy(result, 0, byteResult, 0, byteResult.Length);
            stream.Position = encLoc;
            stream.Write(byteResult, 0, byteResult.Length);
        }

        private void WriteTamperHash(ModuleWriterBase writer)
        {
            var st = new StreamReader(writer.DestinationStream);
            var a = new BinaryReader(st.BaseStream);
            a.BaseStream.Position = 0;
            var enc = MD5.Create().ComputeHash(a.ReadBytes((int)(st.BaseStream.Length)));

            writer.DestinationStream.Write(enc, 0, enc.Length);
        }

        private void Hash(Stream stream, BinaryReader reader, uint offset, uint size)
        {
            long original = stream.Position;
            stream.Position = offset;
            size >>= 2;
            for (uint i = 0; i < size; i++)
            {
                uint data = reader.ReadUInt32();
                uint tmp = (z ^ data) + x + c * v;
                z = x;
                x = c;
                x = v;
                v = tmp;
            }
            stream.Position = original;
        }

        private uint[] DeriveKey()
        {
            uint[] dst = new uint[0x10], src = new uint[0x10];
            for (int i = 0; i < 0x10; i++)
            {
                dst[i] = v;
                src[i] = x;
                z = (x >> 5) | (x << 27);
                x = (c >> 3) | (c << 29);
                c = (v >> 7) | (v << 25);
                v = (z >> 11) | (z << 21);
            }
            return deriver.DeriveKey(dst, src);
        }
    }
}
