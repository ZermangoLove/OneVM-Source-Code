using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.RT.Renamer;

namespace xVM.Helper.Core.RT.Mutation
{
    internal class RuntimeMutator
    {
        internal RTConstants Constants;
        private RuntimeHelpers Helpers;
        private readonly VMRuntime VMRT;
        public dnlib.DotNet.Writer.Metadata rtMD;
        public string HeapName;

        public RuntimeMutator(VMRuntime rt)
        {
            this.VMRT = rt;

            Constants = new RTConstants();
            Helpers = new RuntimeHelpers(Constants, rt);
            Constants.ReadConstants(rt.Descriptor, Helpers);
            Helpers.AddHelpers();
        }

        private void EXECWriterEvent(object sender, ModuleWriterEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;

            rtMD = writer.Metadata;
            if (e.Event == ModuleWriterEvent.MDEndCreateTables)
            {
                MutateMetadata();
                VMRT.OnKoiRequested();
                VMRT.ResetData();

                #region Write Impl '[MethodImpl(MethodImplOptions.NoInlining)]'
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    foreach (var Def in writer.Module.GetTypes())
                    {
                        foreach (var Custom_MD in Scanner.MethodsDF)
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
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Generate New #GUID
                ///////////////////////////////////////////////////////////////////////////
                writer.Metadata.GuidHeap.SetRawData(0, Guid.NewGuid().ToByteArray());
                ///////////////////////////////////////////////////////////////////////////
                #endregion

                Scanner.MethodsDF.Clear();
                Scanner.MethodsDF = new HashSet<MethodDef>();

                #region Merge RT
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (XMLUtils.Runtime_Normal_Mode == false && XMLUtils.Runtime_Normal_Merge_Mode == true)
                {
                    var compressed_RT = VMRT.CompressionService.GZIP_Compress(SaveVMPRuntimeLib.Runtime_VMP_Protected_Merge_Mode.ToArray());

                    var stream = new MemoryStream();
                    stream.Write(compressed_RT, 0, compressed_RT.Length);

                    writer.TheOptions.MetadataOptions.CustomHeaps.Add(new RawHeap(HeapName.Substring(0, 8), stream.ToArray()));
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
        }

        public void EndRuntimeListener(object sender, ModuleWriterEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;

            #region Set Platform To AnyCpu (For RT)
            /////////////////////////////////////////////////////////////////////////////////////
            writer.TheOptions.Cor20HeaderOptions.Flags = dnlib.DotNet.MD.ComImageFlags.ILOnly;
            /////////////////////////////////////////////////////////////////////////////////////
            #endregion

            if (e.Event == ModuleWriterEvent.MDEndCreateTables)
            {
                #region Generate New #GUID (For RT)
                ///////////////////////////////////////////////////////////////////////////
                writer.Metadata.GuidHeap.SetRawData(0, Guid.NewGuid().ToByteArray());
                ///////////////////////////////////////////////////////////////////////////
                #endregion
            }
        }

        public void MutateRuntime()
        {
            try
            {
                #region Patch And Repace RT
                ////////////////////////////////////////////
                RuntimePatcher.Patch(VMRT);
                ////////////////////////////////////////////
                #endregion

                #region Read And Write RT OpCodes (Constants)
                ////////////////////////////////////////////////////
                Constants.ReadConstants(VMRT.Descriptor, Helpers);
                ////////////////////////////////////////////////////
                #endregion

                #region Renamer for RT
                ////////////////////////////////////////////
                VMRT.RNMService.Process(VMRT);
                ////////////////////////////////////////////
                #endregion

                #region "Anti ILDasm" Protection for RT
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                TypeRef SuppressIldasmAttribute = VMRT.RTModule.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "SuppressIldasmAttribute");
                MemberRefUser SuppressIldasmAttribute_ctor = new MemberRefUser(VMRT.RTModule, ".ctor", MethodSig.CreateInstance(VMRT.RTModule.CorLibTypes.Void), SuppressIldasmAttribute);
                CustomAttribute SuppressIldasmAttribute_item = new CustomAttribute(SuppressIldasmAttribute_ctor);
                VMRT.RTModule.Assembly.CustomAttributes.Add(SuppressIldasmAttribute_item);
                VMRT.RTModule.CustomAttributes.Add(SuppressIldasmAttribute_item);


                TypeRef SuppressUnmanagedCodeSecurity = VMRT.RTModule.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "SuppressUnmanagedCodeSecurity");
                MemberRefUser SuppressUnmanagedCodeSecurity_ctor = new MemberRefUser(VMRT.RTModule, ".ctor", MethodSig.CreateInstance(VMRT.RTModule.CorLibTypes.Void), SuppressUnmanagedCodeSecurity);
                CustomAttribute SuppressUnmanagedCodeSecurity_item = new CustomAttribute(SuppressUnmanagedCodeSecurity_ctor);
                VMRT.RTModule.Assembly.CustomAttributes.Add(SuppressUnmanagedCodeSecurity_item);
                VMRT.RTModule.CustomAttributes.Add(SuppressUnmanagedCodeSecurity_item);


                TypeRef UnsafeValueTypeAttribute = VMRT.RTModule.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "UnsafeValueTypeAttribute");
                MemberRefUser UnsafeValueTypeAttribute_ctor = new MemberRefUser(VMRT.RTModule, ".ctor", MethodSig.CreateInstance(VMRT.RTModule.CorLibTypes.Void), UnsafeValueTypeAttribute);
                CustomAttribute UnsafeValueTypeAttribute_item = new CustomAttribute(UnsafeValueTypeAttribute_ctor);
                VMRT.RTModule.Assembly.CustomAttributes.Add(UnsafeValueTypeAttribute_item);
                VMRT.RTModule.CustomAttributes.Add(UnsafeValueTypeAttribute_item);


                TypeRef RuntimeWrappedException = VMRT.RTModule.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "RuntimeWrappedException");
                MemberRefUser RuntimeWrappedException_ctor = new MemberRefUser(VMRT.RTModule, ".ctor", MethodSig.CreateInstance(VMRT.RTModule.CorLibTypes.Void), RuntimeWrappedException);
                CustomAttribute RuntimeWrappedException_item = new CustomAttribute(RuntimeWrappedException_ctor);
                VMRT.RTModule.Assembly.CustomAttributes.Add(RuntimeWrappedException_item);
                VMRT.RTModule.CustomAttributes.Add(RuntimeWrappedException_item);
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Anti Reflector (For my dearest Reflector devs, this is my Christmas present)
                //////////////////////////////////////////////
                RickRoller.CommenceRickroll(VMRT.RTModule);
                //////////////////////////////////////////////
                #endregion
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        public void InitHelpers()
        {
            Helpers = new RuntimeHelpers(Constants, VMRT);
            Helpers.AddHelpers();
        }

        public EventHandler2<ModuleWriterEventArgs> CommitModule(ModuleDef module)
        {
            ImportReferences(module);
            return EXECWriterEvent;
        }

        private void ImportReferences(ModuleDef module)
        {
            var refCopy = VMRT.Descriptor.Data.refMap.ToList();
            VMRT.Descriptor.Data.refMap.Clear();
            foreach (var mdRef in refCopy)
            {
                object item;
                if (mdRef.Key is ITypeDefOrRef)
                    item = module.Import((ITypeDefOrRef)mdRef.Key);
                else if (mdRef.Key is MemberRef)
                    item = module.Import((MemberRef)mdRef.Key);
                else if (mdRef.Key is MethodDef)
                    item = module.Import((MethodDef)mdRef.Key);
                else if (mdRef.Key is MethodSpec)
                    item = module.Import((MethodSpec)mdRef.Key);
                else if (mdRef.Key is FieldDef)
                    item = module.Import((FieldDef)mdRef.Key);
                else
                    item = mdRef.Key;
                VMRT.Descriptor.Data.refMap.Add((IMemberRef)item, mdRef.Value);
            }
            foreach (var sig in VMRT.Descriptor.Data.sigs)
            {
                var methodSig = sig.Signature;
                var funcSig = sig.FuncSig;

                if (methodSig.HasThis)
                    funcSig.Flags |= VMRT.Descriptor.Runtime.RTFlags.INSTANCE;

                var paramTypes = new List<ITypeDefOrRef>();
                if (methodSig.HasThis && !methodSig.ExplicitThis)
                {
                    IType thisType;
                    if (sig.DeclaringType.IsValueType)
                        thisType = module.Import(new ByRefSig(sig.DeclaringType.ToTypeSig()).ToTypeDefOrRef());
                    else
                        thisType = module.Import(sig.DeclaringType);
                    paramTypes.Add((ITypeDefOrRef)thisType);
                }
                foreach (var param in methodSig.Params)
                {
                    var paramType = (ITypeDefOrRef)module.Import(param.ToTypeDefOrRef());
                    paramTypes.Add(paramType);
                }
                funcSig.ParamSigs = paramTypes.ToArray();

                var retType = (ITypeDefOrRef)module.Import(methodSig.RetType.ToTypeDefOrRef());
                funcSig.RetType = retType;
            }
        }

        public void MutateMetadata()
        {
            foreach (var mdRef in VMRT.Descriptor.Data.refMap)
                mdRef.Key.Rid = rtMD.GetToken(mdRef.Key).Rid;

            foreach (var sig in VMRT.Descriptor.Data.sigs)
            {
                var funcSig = sig.FuncSig;

                foreach (var paramType in funcSig.ParamSigs)
                    paramType.Rid = rtMD.GetToken(paramType).Rid;

                funcSig.RetType.Rid = rtMD.GetToken(funcSig.RetType).Rid;
            }
        }
    }
}