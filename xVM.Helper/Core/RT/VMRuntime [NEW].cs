using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.VM;
using xVM.Helper.Core.AST;
using xVM.Helper.Core.CFG;
using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.Constants;
using xVM.Helper.Core.RT.Renamer;
using xVM.Helper.Core.RT.Mutation;
using xVM.Helper.Core.Protections;
using xVM.Helper.Core.Helpers.System;
using xVM.Helper.Core.Protections.SugarControlFlow;

namespace xVM.Helper.Core.RT
{
    public class VMRuntime
    {
        private List<Tuple<MethodDef, ILBlock>> BasicBlocks;

        private List<IVMChunk> ExtraChunks;
        private List<IVMChunk> FinalChunks;
        internal Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>> MD_MAP;
        internal BasicBlockSerializer Serializer;

        internal CompressionService CompressionService;

        internal RuntimeMutator RTMutator;
        internal RuntimeSearch RTSearch;
        internal static RuntimeSearch Static_RTSearch;
        internal NameService RNMService;
        internal MethodPatcher Internal_MDPatcher;
        internal static MethodPatcher Static_MDPatcher;

        internal ILVDynamicDeriver __ILVDATA_Deriver;
        internal static LdstrDynamicDeriver LdstrDeriver;

        internal ModuleWriterOptions RTModuleWriterOptions;

        public VMRuntime(IVMSettings settings, ModuleDef RTM)
        {
            RTModule = (ModuleDefMD)RTM;
            Descriptor = new VMDescriptor(settings);
            RTLibStream = new MemoryStream();

            BasicBlocks = new List<Tuple<MethodDef, ILBlock>>();

            CompressionService = new CompressionService();

            ExtraChunks = new List<IVMChunk>();
            FinalChunks = new List<IVMChunk>();
            MD_MAP = new Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>>();
            Serializer = new BasicBlockSerializer(this);

            RTMutator = new RuntimeMutator(this);
            RTSearch = new RuntimeSearch(this).Search();
            Static_RTSearch = new RuntimeSearch(this).Search();
            RNMService = new NameService();
            Internal_MDPatcher = new MethodPatcher();
            Static_MDPatcher = new MethodPatcher();

            __ILVDATA_Deriver = new ILVDynamicDeriver();
            LdstrDeriver = new LdstrDynamicDeriver();

            #region Normal ModuleWriterOptions
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            RTModuleWriterOptions = new ModuleWriterOptions(RTModule)
            {
                Logger = DummyLogger.NoThrowInstance,
            };
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region VMData->.ctor String Mutation->Crypt Write
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Descriptor.Data.Seed = BitConverter.ToInt32(Encoding.Default.GetBytes(RTSearch.Murmur2.Name), 0); // Generate Seed

            RTSearch.VMData_Ctor.Body.SimplifyMacros(RTSearch.VMData_Ctor.Parameters);
            List<Instruction> instrs = RTSearch.VMData_Ctor.Body.Instructions.ToList();
            for (int i = 0; i < instrs.Count; i++)
            {
                Instruction instr = instrs[i];
                var method = instr.Operand as IMethod;
                if (instr.OpCode == OpCodes.Call)
                {
                    if (method.DeclaringType.Name == RTMap.Mutation &&
                        method.Name == RTMap.MutationCore_Crypt)
                    {
                        Instruction ldBlock = instrs[i - 2];
                        Instruction ldKey = instrs[i - 1];
                        instrs.RemoveAt(i);
                        instrs.RemoveAt(i - 1);
                        instrs.RemoveAt(i - 2);
                        instrs.InsertRange(i - 2, LdstrDeriver.EmitDecrypt(RTSearch.VMData_Ctor, this, (Local)ldBlock.Operand, (Local)ldKey.Operand));
                    }
                }
            }
            RTSearch.VMData_Ctor.Body.Instructions.Clear();
            foreach (Instruction instr in instrs)
                RTSearch.VMData_Ctor.Body.Instructions.Add(instr);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion
        }

        public List<byte> __ILVDATA = new List<byte>();

        public ModuleDefMD RTModule
        {
            get;
            set;
        }

        public VMDescriptor Descriptor
        {
            get;
            private set;
        }

        public static MemoryStream RTLibStream
        {
            get;
            set;
        } = new MemoryStream();

        public static string Watermark_Class_Name { get; set; } = "VirtualizatedWithAttribute";
        public static string Watermark { get; set; } = "Test";

        public void AddMethod(MethodDef method, ScopeBlock rootScope)
        {
            ILBlock entry = null;
            foreach (ILBlock block in rootScope.GetBasicBlocks())
            {
                if (block.Id == 0)
                    entry = block;
                BasicBlocks.Add(Tuple.Create(method, block));
            }
            Debug.Assert(entry != null);
            MD_MAP[method] = Tuple.Create(rootScope, entry);
        }

        #region Add Method Attribute (Virt. ID)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddMethodID(MethodDef method)
        {
            if (Scanner.MethodsDF.Count > 0)
            {
                var byte_0 = Encoding.Default.GetBytes(Descriptor.Data.GetExportStringID(method));
                var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(method.FullName));
                Buffer.BlockCopy(hash, 0, hash, 0, 16);

                var Key = new Guid(hash).ToString();
                var Encrypted_ID = new byte[byte_0.Length + 1];
                int num = new Random().Next(1, 255);
                for (int i = 0; i <= byte_0.Length - 1; i++)
                {
                    Encrypted_ID[i] = (byte)((int)byte_0[i] ^ ((int)Key[i % Key.Length] + num & 255));
                }
                Encrypted_ID[byte_0.Length] = (byte)num;

                var attr = new CustomAttribute((ICustomAttributeType)method.Module.Import(RTSearch.IDAttribute_Ctor), new[]
                {
                    new CAArgument(method.Module.CorLibTypes.GetTypeRef("System", "String").ToTypeSig(), BitConverter.ToString(Encrypted_ID).Replace("-", string.Empty)),
                    new CAArgument(method.Module.CorLibTypes.GetTypeRef("System", "String").ToTypeSig(), Key)
                });
                method.CustomAttributes.Add(attr);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        internal void AddHelper(MethodDef method, ScopeBlock rootScope, ILBlock entry)
        {
            MD_MAP[method] = Tuple.Create(rootScope, entry);
        }

        internal void AddBlock(MethodDef method, ILBlock block)
        {
            BasicBlocks.Add(Tuple.Create(method, block));
        }

        public ScopeBlock LookupMethod(MethodDef method)
        {
            var m = MD_MAP[method];
            return m.Item1;
        }

        internal ScopeBlock LookupMethod(MethodDef method, out ILBlock entry)
        {
            var m = MD_MAP[method];
            entry = m.Item2;
            return m.Item1;
        }

        internal void AddChunk(IVMChunk chunk)
        {
            ExtraChunks.Add(chunk);
        }

        public void ExportMethod(MethodDef method, ModuleDef module)
        {
            Internal_MDPatcher.Patch(method, module, this /*, this.Descriptor.Data.GetExportId(method)*/);
        }

        public void OnKoiRequested()
        {
            try
            {
                var header = new HeaderChunk(this);

                foreach (var block in BasicBlocks)
                {
                    FinalChunks.Add(block.Item2.CreateChunk(this, block.Item1));
                }
                FinalChunks.AddRange(ExtraChunks);
                Descriptor.RandomGenerator.Shuffle(FinalChunks);
                FinalChunks.Insert(0, header);

                ComputeOffsets();
                FixupReferences();
                header.WriteData(this);

                #region ***********| Create ILVData |***********
                ///////////////////////////////////////////////////////
                List<byte> Data = new List<byte>();
                foreach (var chunk in FinalChunks)
                {
                    Data.AddRange(chunk.GetData());
                }
                Data.Reverse();
                ///////////////////////////////////////////////////////
                #endregion

                #region ***********| Write Encrypted And Compressed DATA To __ILVDATA LIST |***********
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                __ILVDATA_Deriver.Initialize(this);

                var Encrypted_ILVDATA = __ILVDATA_Deriver.Encrypt(Data.ToArray(), 0);

                for (var l = 0; l < Encrypted_ILVDATA.Length; l++)
                {
                    __ILVDATA.Add(Encrypted_ILVDATA[l]);
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region DATA Write To RT Module
                ///////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    GetWriteVData();
                }
                ///////////////////////////////////////
                #endregion

                #region Write Key0 And Key1 (Utils->ReadCompressedULong Keys)
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    MutationHelper.InjectKey_String(RTSearch.Utils_ReadCompressedULong, 0, Utils.CompressedUInt_Key0.ToString()); // Key0
                    MutationHelper.InjectKey_String(RTSearch.Utils_ReadCompressedULong, 1, Utils.CompressedUInt_Key1.ToString()); // Key1
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Write Seed And MurmurSeed (VMData->.ctor String Enc. Keys)
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    MutationHelper.InjectKey_String(RTSearch.VMData_Ctor, 1, Convert.ToString(Descriptor.Data.Seed)); // Seed
                    MutationHelper.InjectKey_String(RTSearch.VMData_Ctor, 0, Convert.ToString(LdstrDeriver.MurmurSeed)); // MurmurSeed
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Write Murmur2 Key
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                MutationHelper.InjectKey_Int(RTSearch.Murmur2_Hash, 0, (int)Murmur2.Key);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Replace, Protect And Write RT Stream
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////           
                #region RT Listener
                /////////////////////////////////////////////////////////////////////////////////////////
                RTModuleWriterOptions.WriterEvent += RTMutator.EndRuntimeListener;
                /////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Before Optimize RT
                /////////////////////////////////////////////////////////////////////////////////////////
                Optimize.OptimizeCode.Remove_const_Value(RTModule);
                Optimize.OptimizeCode.ArmDot_Optimize(RTModule);
                Optimize.OptimizeCode.Method_Optimize_A(RTModule);
                Optimize.OptimizeCode.Reduce_MetaData_Confusion(RTModule);
                /////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Custom Mutation
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #region VMInstance Methods
                /////////////////////////////////////////////////////////
                foreach (var md in RTSearch.VMInstance.Methods)
                    Protections.Mutation.MutationConfusion.Execute(md);
                /////////////////////////////////////////////////////////
                #endregion

                #region Interpreter->GetInternalVData Method
                ////////////////////////////////////////////////////////////////////////////////////////////////
                Protections.Mutation.MutationConfusion.Execute(RTSearch.Interpreter_GetInternalVData);
                ////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Constants (Protection)->Get Method
                //////////////////////////////////////////////////////////////////////////////
                Protections.Mutation.MutationConfusion.Execute(RTSearch.ConstantsProt_Get);
                //////////////////////////////////////////////////////////////////////////////
                #endregion

                #region VMTrampoline All Methods
                ///////////////////////////////////////////////////////////
                foreach (var md in RTSearch.VMTrampoline.Methods)
                    Protections.Mutation.MutationConfusion.Execute(md);
                ///////////////////////////////////////////////////////////
                #endregion
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region End Protection (For RT)
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #region All String Encryption (ConfuserEX Constants Protection (Modded)
                /////////////////////////////////////////////////////////////////
                new ConstantsProtection().Execute(RTModule, this);
                /////////////////////////////////////////////////////////////////
                #endregion

                #region Int Control Flow For All RT Methods (Without Murmur2, Interpreter, VMData Class)
                /////////////////////////////////////////////////////////////////////////////////////////////
                foreach (var def in RTModule.Types)
                    if (def != RTSearch.Murmur2 && def != RTSearch.Interpreter && def != RTSearch.VMData)
                        foreach (var RTMD in def.Methods)
                        {
                            if (!RTMD.HasBody) continue;
                            if (RTMD != RTSearch.ConstantsProt_Initialize)
                            {
                                MutateRT.IntControlFlow_NoSizeOf(RTMD);
                            }
                        }
                MutateRT.IntControlFlow(RTSearch.Interpreter_GetInternalVData);
                /////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region [By Sugar, Null Control Flow Protection] For Constants (Protection)->Get Method
                ///////////////////////////////////////////////////////////////
                SugarControlFlow.Execute(RTSearch.ConstantsProt_Get);
                NullControlFlow.Execute(RTSearch.ConstantsProt_Get);
                ///////////////////////////////////////////////////////////////
                #endregion

                #region [By Sugar, Null Control Flow Protection] For Interpreter->GetInternalVData Method
                //////////////////////////////////////////////////////////////////
                NullControlFlow.Execute(RTSearch.Interpreter_GetInternalVData);
                SugarControlFlow.Execute(RTSearch.Interpreter_GetInternalVData);
                //////////////////////////////////////////////////////////////////
                #endregion

                #region [By Null, Sugar, Eddy Control Flow Protection] For All VMInstance Methods
                /////////////////////////////////////////////////////
                foreach (var ins in RTSearch.VMInstance.Methods)
                {
                    NullControlFlow.Execute(ins);
                    SugarControlFlow.Execute(ins);
                    EddyControlFlow.Execute(ins);
                }
                /////////////////////////////////////////////////////
                #endregion

                #region [By ConfuserEX Control Flow And Array Mutation Protection] For All VMTrampoline Methods
                //////////////////////////////////////////////////////////////
                foreach (var tramboline in RTSearch.VMTrampoline.Methods)
                {
                    CEXControlFlow.Execute(tramboline, 2);
                    MutateRT.Array_Mutation(tramboline);
                }
                //////////////////////////////////////////////////////////////
                #endregion
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Hide Methods
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #region VMInsance Methods
                /////////////////////////////////////////////////
                foreach (var md in RTSearch.VMInstance.Methods)
                    HideMethod.Execute(md);
                /////////////////////////////////////////////////
                #endregion

                #region Interpreter->GetInternalVData Method
                ///////////////////////////////////////////////////////////
                HideMethod.Execute(RTSearch.Interpreter_GetInternalVData);
                ///////////////////////////////////////////////////////////
                #endregion

                #region Constants (Protection)->Get Method
                ///////////////////////////////////////////////////////////
                HideMethod.Execute(RTSearch.ConstantsProt_Get);
                ///////////////////////////////////////////////////////////
                #endregion

                #region VMTrampoline All Methods
                ///////////////////////////////////////////////////////////
                foreach (var md in RTSearch.VMTrampoline.Methods)
                    HideMethod.Execute(md);
                ///////////////////////////////////////////////////////////
                #endregion
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Anti De4dot
                //////////////////////////////////////////
                Anti_De4dot.Execute(RTModule, Watermark);
                //////////////////////////////////////////
                #endregion

                #region End Optimize RT
                /////////////////////////////////////////////////////////////////////////////////////////
                Optimize.OptimizeCode.Remove_const_Value(RTModule);
                Optimize.OptimizeCode.ArmDot_Optimize(RTModule);
                Optimize.OptimizeCode.Method_Optimize_A(RTModule);
                Optimize.OptimizeCode.Reduce_MetaData_Confusion(RTModule);
                /////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                RTModule.Write(RTLibStream, RTModuleWriterOptions);
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Save RT
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    if (XMLUtils.Runtime_Normal_Mode == true && XMLUtils.Runtime_Normal_Merge_Mode == false)
                        SaveVMPRuntimeLib.Save(this, false);
                    else if (XMLUtils.Runtime_Normal_Mode == false && XMLUtils.Runtime_Normal_Merge_Mode == true)
                        SaveVMPRuntimeLib.Save(this, true);
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        private void GetWriteVData()
        {
            try
            {
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var DataType = new TypeDefUser(RNMService.NewName(Descriptor.RandomGenerator.NextString()), RTModule.CorLibTypes.GetTypeRef("System", "ValueType"))
                {
                    Layout = dnlib.DotNet.TypeAttributes.ExplicitLayout,
                    Visibility = dnlib.DotNet.TypeAttributes.Sealed,
                    IsSealed = true,
                    ClassLayout = new ClassLayoutUser(0, (uint)__ILVDATA.Count)
                };
                RTModule.Types.Add(DataType);
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var FieldWithRVA = new FieldDefUser(RNMService.NewName(Descriptor.RandomGenerator.NextString()), new FieldSig(DataType.ToTypeSig()), dnlib.DotNet.FieldAttributes.Private | dnlib.DotNet.FieldAttributes.Static | dnlib.DotNet.FieldAttributes.HasFieldRVA)
                {
                    HasFieldRVA = true,
                    InitialValue = __ILVDATA.ToArray()
                };
                RTSearch.Interpreter.Fields.Add(FieldWithRVA);
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                MutationHelper.InjectKey_String(RTSearch.Interpreter_GetInternalVData, 0, Convert.ToString((__ILVDATA.Count / 4)));
                MutationHelper.InjectKey_String(RTSearch.Interpreter_GetInternalVData, 1, FieldWithRVA.Name);

                #region Write Mutation.Crypt (For Encrypted And Compressed Data)
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                MutationHelper.InjectKey_String(RTSearch.Interpreter_GetInternalVData, 2, Convert.ToString(__ILVDATA_Deriver.Seed)); // Write Seed
                MutationHelper.InjectKey_String(RTSearch.Interpreter_GetInternalVData, 3, Convert.ToString(__ILVDATA_Deriver.MurmurSeed)); // Write Murmur Seed

                RTSearch.Interpreter_GetInternalVData.Body.SimplifyMacros(RTSearch.Interpreter_GetInternalVData.Parameters);
                List<Instruction> instrs = RTSearch.Interpreter_GetInternalVData.Body.Instructions.ToList();
                for (int i = 0; i < instrs.Count; i++)
                {
                    Instruction instr = instrs[i];
                    var method = instr.Operand as IMethod;
                    if (instr.OpCode == OpCodes.Call)
                    {
                        if (method.DeclaringType.Name == RTMap.Mutation &&
                            method.Name == RTMap.Mutation_Crypt)
                        {
                            Instruction ldBlock = instrs[i - 2];
                            Instruction ldKey = instrs[i - 1];
                            instrs.RemoveAt(i);
                            instrs.RemoveAt(i - 1);
                            instrs.RemoveAt(i - 2);
                            instrs.InsertRange(i - 2, __ILVDATA_Deriver.EmitDecrypt(RTSearch.Interpreter_GetInternalVData, (Local)ldBlock.Operand, (Local)ldKey.Operand));
                        }
                    }
                }

                RTSearch.Interpreter_GetInternalVData.Body.Instructions.Clear();
                foreach (Instruction instr in instrs)
                    RTSearch.Interpreter_GetInternalVData.Body.Instructions.Add(instr);
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        private void ComputeOffsets()
        {
            ulong offset = 0;
            foreach (var chunk in FinalChunks)
            {
                chunk.OnOffsetComputed((uint)offset);
                offset += chunk.Length;
            }
        }

        private void FixupReferences()
        {
            foreach (var block in BasicBlocks)
            {
                foreach (var instr in block.Item2.Content)
                {
                    if (instr.Operand is ILRelReference)
                    {
                        var reference = (ILRelReference)instr.Operand;
                        instr.Operand = ILImmediate.Create(reference.Resolve(this), ASTType.I4);
                    }
                }
            }
        }

        public void ResetData()
        {
            MD_MAP = new Dictionary<MethodDef, Tuple<ScopeBlock, ILBlock>>();
            BasicBlocks = new List<Tuple<MethodDef, ILBlock>>();

            ExtraChunks = new List<IVMChunk>();
            FinalChunks = new List<IVMChunk>();
            Descriptor.ResetData();

            RTMutator.InitHelpers();
        }
    }
}