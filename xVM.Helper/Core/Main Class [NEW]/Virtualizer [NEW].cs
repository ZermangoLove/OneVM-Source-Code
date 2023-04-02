using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.RT;
using xVM.Helper.Core.VMIL;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.RT.Renamer;
using xVM.Helper.Core.Protections;
using xVM.Helper.Core.RT.Mutation;
using xVM.Helper.Core.AntiTamperEXEC;
using xVM.Helper.Core.Protections.Mutation;
using xVM.Helper.Core.Protections.SugarControlFlow;

namespace xVM.Helper.Core
{
    internal class Virtualizer : IVMSettings
    {
        private readonly HashSet<MethodDef> DoInstantiation = new HashSet<MethodDef>();
        private readonly GenericInstantiation Instantiation = new GenericInstantiation();
        private readonly Dictionary<MethodDef, bool> MethodList = new Dictionary<MethodDef, bool>();
        private readonly HashSet<ModuleDef> Processed = new HashSet<ModuleDef>();
        public IList<string> Custom_Method_List = new List<string>();
        private MethodVirtualizer MD_VR;
        internal EventHandler2<ModuleWriterEventArgs> CommitListener;

        internal VMRuntime Runtime = null;

        public Virtualizer()
        {
            XMLUtils.Methods_FullName = new List<string>();
            Custom_Method_List = new List<string>();

            Instantiation.ShouldInstantiate += spec => DoInstantiation.Contains(spec.Method.ResolveMethodDefThrow());
        }

        bool IVMSettings.IsExported(MethodDef method)
        {
            bool ret;
            if(!MethodList.TryGetValue(method, out ret))
                return false;
            return ret;
        }

        bool IVMSettings.IsVirtualized(MethodDef method)
        {
            return MethodList.ContainsKey(method);
        }

        public void Initialize(ModuleDef EXECModule, TypeDef NewModule, MemoryStream xmlFile)
        {
            byte[] Decrypted_RT = Properties.Resources.c60f5823495b4242;
            byte[] Password = Properties.Resources.f52b6acb1e894178;

            #region Decrypt RT
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            byte[] saltBytes = new byte[] { 115, 21, 58, 64, 101, 144, 255, 15 };
            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(Password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(Decrypted_RT, 0, Decrypted_RT.Length);
                        cs.Close();
                    }
                    Decrypted_RT = ms.ToArray();
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            var rtModule = ModuleDefMD.Load(Decrypted_RT);
            try
            {
                #region Configure VM (Read VM Settings)
                ///////////////////////////////////////
                XMLUtils.Read(this, xmlFile);
                ///////////////////////////////////////
                #endregion

                #region Fix Virtualize New Module Methods
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var Fix_Thread = new Thread(() =>
                {
                    var org_module_type = EXECModule.GlobalType;
                    for (var i = 0; i < XMLUtils.Methods_FullName.Count; i++)
                    {
                        foreach (var mdn in NewModule.Methods)
                        {
                            var md = XMLUtils.Methods_FullName[i];
                            if (md.Contains(org_module_type.Name))
                            {

                                if (md.Replace(org_module_type.Name, NewModule.Name) == mdn.FullName)
                                    if (!mdn.IsPinvokeImpl && !mdn.IsUnmanagedExport)
                                    {
                                        XMLUtils.Methods_FullName.Remove(md);
                                        XMLUtils.Methods_FullName.Add(mdn.FullName);
                                    }
                                    else if (mdn.IsPinvokeImpl || mdn.IsUnmanagedExport)
                                        XMLUtils.Methods_FullName.Remove(md);


                                if (md.Replace(org_module_type.FullName, NewModule.FullName) == mdn.FullName)
                                    if (!mdn.IsPinvokeImpl && !mdn.IsUnmanagedExport)
                                    {
                                        XMLUtils.Methods_FullName.Remove(md);
                                        XMLUtils.Methods_FullName.Add(mdn.FullName);
                                    }
                                    else if (mdn.IsPinvokeImpl || mdn.IsUnmanagedExport)
                                        XMLUtils.Methods_FullName.Remove(md);
                            }
                        }
                    }
                });
                Fix_Thread.Start();
                Fix_Thread.Join();
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region AssemblyReferences Adder
                //////////////////////////////////////////////////////////////////////////////////////////
                var AsmResolver = new AssemblyResolver { EnableTypeDefCache = true };
                var ModCtx = new ModuleContext(AsmResolver);

                AsmResolver.DefaultModuleContext = ModCtx;
                EXECModule.Context = ModCtx;

                foreach (var AsmRef in EXECModule.GetAssemblyRefs())
                    try
                    {
                        if (AsmRef == null) continue;
                        var resolver = AsmResolver.Resolve(AsmRef.FullName, EXECModule);
                    }
                    catch { /* throw null */ }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Reset MutationHelper
                //////////////////////////////////////////////////////////////////////////////////////////////
                MutationHelper.Field2IntIndex = MutationHelper.Original_Field2IntIndex;
                MutationHelper.Field2LongIndex = MutationHelper.Original_Field2LongIndex;
                MutationHelper.Field2LdstrIndex = MutationHelper.Original_Field2LdstrIndex;

                MutationHelperCore.Field2IntIndex = MutationHelperCore.Original_Field2IntIndex;
                MutationHelperCore.Field2LongIndex = MutationHelperCore.Original_Field2LongIndex;
                MutationHelperCore.Field2LdstrIndex = MutationHelperCore.Original_Field2LdstrIndex;

                RTMap.Mutation = "Mutation";
                RTMap.Mutation_Placeholder = "Placeholder";
                RTMap.Mutation_Value_T = "Value";
                RTMap.Mutation_Value_T_Arg0 = "Value";
                RTMap.Mutation_Crypt = "Crypt";

                RTMap.MutationCore = "MutationCore";
                RTMap.MutationCore_Placeholder = "Placeholder";
                RTMap.MutationCore_Value_T = "Value";
                RTMap.MutationCore_Value_T_Arg0 = "Value";
                RTMap.MutationCore_Crypt = "Crypt";
                //////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Load SNK Settings
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                if (File.Exists(XMLUtils.SNKFile_Location))
                {
                    var signatureKey = Utils.LoadSNKey(XMLUtils.SNKFile_Location, XMLUtils.SNK_Password);
                    Utils.ExecuteModuleWriterOptions.InitializeStrongNameSigning(EXECModule, signatureKey);
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Configure VMSettings
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Runtime = new VMRuntime(this, rtModule); // Start and Configure VMRuntime
                MD_VR = new MethodVirtualizer(Runtime); // Configure Method Virtualization Settings

                Runtime.RTSearch = new RuntimeSearch(Runtime).Search(); // Search RTMap Methods "Public Version"
                VMRuntime.Static_RTSearch = new RuntimeSearch(Runtime).Search(); // Search RTMap Methods "Static Version"

                // "Utils.ReadCompressedULong" Key0
                Utils.CompressedUInt_Key0 = BitConverter.ToInt32(Encoding.Default.GetBytes(Runtime.RTSearch.Utils_ReadCompressedULong.Name), 0);

                // "Utils.ReadCompressedULong" Key1
                Utils.CompressedUInt_Key1 = BitConverter.ToInt32(Encoding.Default.GetBytes(Runtime.RTSearch.Utils_FromCodedToken.Name), 0);

                Runtime.RTMutator.MutateRuntime(); // First DLL Protections
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Rename DLL RT
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (XMLUtils.Runtime_Normal_Mode == false && XMLUtils.Runtime_Normal_Merge_Mode == true)
                {
                    var nameservice = new NameService();
                    rtModule.Assembly.Name = nameservice.NewName(rtModule.Assembly.Name);
                    rtModule.Name = null;
                }
                else
                {
                    rtModule.Assembly.Name = XMLUtils.RuntimeName;
                    rtModule.Name = null;
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

            }
            finally { /* Null */ }
        }

        public void ConfigureModuleAndMerge(ModuleDef module)
        {
            if (XMLUtils.Runtime_Normal_Mode == false && XMLUtils.Runtime_Normal_Merge_Mode == true)
            {
                TypeDef typeDef = ModuleDefMD.Load(typeof(Merge_VMP_Runtime).Module).ResolveTypeDef(MDToken.ToRID(typeof(Merge_VMP_Runtime).MetadataToken));
                IEnumerable<IDnlibDef> members = Helpers.Injection.InjectHelper.Inject(typeDef, module.GlobalType, module);

                var Module_ctor = module.GlobalType.FindOrCreateStaticConstructor();
                var init = (MethodDef)members.Single(method => method.Name == "_ctor");
                var Start = members.OfType<MethodDef>().Single(method => method.Name == "Start");
                var ProcessExit = members.OfType<MethodDef>().Single(method => method.Name == "ProcessExit");
                var ExitHandler = members.OfType<MethodDef>().Single(method => method.Name == "ExitHandler");
                var ConsoleExitHandler = members.OfType<MethodDef>().Single(method => method.Name == "ConsoleExitHandler");

                // Inject Runtime Name
                MutationHelperCore.InjectKey_String(Start, 0, Runtime.RTModule.Assembly.Name + ".dll");
                ////////////////////////////////////////////////////////////////////////////////////////

                // Inject Heap Name Key
                int resID = Runtime.Descriptor.RandomGenerator.NextInt32();
                string heapName = Encoding.BigEndianUnicode.GetString(SHA1.Create().ComputeHash(BitConverter.GetBytes(resID))).ToHexString();
                Runtime.RTMutator.HeapName = heapName;

                MutationHelperCore.InjectKey_Int(Start, 0, resID);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                #region ConfigureModule (Merge VMP Version)
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var Module_NewCtor = new MethodDefUser(Runtime.Descriptor.RandomGenerator.NextHexString(true), MethodSig.CreateStatic(module.CorLibTypes.Void));
                Module_NewCtor.IsRuntimeSpecialName = false;
                Module_NewCtor.IsSpecialName = false;
                Module_NewCtor.IsStatic = true;
                Module_NewCtor.Access = MethodAttributes.PrivateScope;
                Module_NewCtor.Body = Module_ctor.Body;
                Module_ctor.Body = new CilBody(); // Reset Module .ctor

                // Call VMEntry->ConfigureRT Method -> Module..ctor
                Module_NewCtor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Newobj, module.Import(Runtime.RTSearch.VMEntry_Ctor)));
                Module_NewCtor.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldftn, module.Import(Runtime.RTSearch.VMEntry_ConfigureRT)));
                Module_NewCtor.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Calli, module.Import(Runtime.RTSearch.VMEntry_ConfigureRT).MethodSig));
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                module.GlobalType.Methods.Add(Module_NewCtor); // Add New Module .ctor Method;

                // Call Merge_VMP_Runtime->_ctor Method -> Module..ctor
                Module_ctor.Body = new CilBody(true, new List<Instruction>
                {
                    Instruction.Create(OpCodes.Jmp, init),
                    Instruction.Create(OpCodes.Ret)
                }, new List<ExceptionHandler>(), new List<Local>());
                //////////////////////////////////////////////////////////////////////////////////

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region _ctor Instructions Copy To Module .ctor Instructions
                ///////////////////////////////////////////////////////////////
                var instructions = Module_ctor.Body.Instructions;
                for (var i = instructions.Count - 1; i >= 0; i--)
                {
                    if (!(instructions[i].Operand == init)) continue;

                    Module_ctor.Body.MergeCall(instructions[i]);
                    init.DeclaringType.Methods.Remove(init);
                }
                ///////////////////////////////////////////////////////////////
                #endregion

                #region ConfigureModule (Merge VMP Version) Add New Module .ctor Method
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Module_ctor.Body.Instructions.Insert(Module_ctor.Body.Instructions.Count - 2, Instruction.Create(OpCodes.Jmp, Module_NewCtor));
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Protect Module.ctor, Start, ProcessExit Method (Sugar Control Flow and Mutation)
                //////////////////////////////////////////////////
                MutationConfusion.Execute(Module_NewCtor);
                MutationConfusion.Execute(Start);
                MutationConfusion.Execute(ProcessExit);
                MutationConfusion.Execute(ExitHandler);
                MutationConfusion.Execute(ConsoleExitHandler);

                SugarControlFlow.Execute(Module_ctor);
                SugarControlFlow.Execute(Module_NewCtor);
                SugarControlFlow.Execute(Start);
                SugarControlFlow.Execute(ProcessExit);
                SugarControlFlow.Execute(ExitHandler);
                SugarControlFlow.Execute(ConsoleExitHandler);
                //////////////////////////////////////////////////
                #endregion

                #region Rename Merged Methods
                ///////////////////////////////////////////////////////////////////////
                foreach (IDnlibDef def in members)
                {
                    IMemberDef memberDef = def as IMemberDef;

                    if ((memberDef as TypeDef) != null)
                        memberDef.Name = Runtime.RNMService.NewName(memberDef.Name);
                    else if ((memberDef as MethodDef) != null)
                        memberDef.Name = Runtime.RNMService.NewName(memberDef.Name);
                    else if ((memberDef as FieldDef) != null)
                        memberDef.Name = Runtime.RNMService.NewName(memberDef.Name);
                }
                ///////////////////////////////////////////////////////////////////////
                #endregion

                #region Hide Methods Merged Methods
                //////////////////////////////////////////
                HideMethod.Execute(Module_ctor);
                HideMethod.Execute(Module_NewCtor);
                HideMethod.Execute(Start);
                HideMethod.Execute(ProcessExit);
                HideMethod.Execute(ExitHandler);
                HideMethod.Execute(ConsoleExitHandler);
                //////////////////////////////////////////
                #endregion
            }
            else if (XMLUtils.Runtime_Normal_Mode == true && XMLUtils.Runtime_Normal_Merge_Mode == false)
            {
                var Module_ctor = module.GlobalType.FindOrCreateStaticConstructor();

                #region ConfigureModule (Merge Normal And VMP Version)
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var Module_NewCtor = new MethodDefUser(Runtime.Descriptor.RandomGenerator.NextHexString(true), MethodSig.CreateStatic(module.CorLibTypes.Void));
                Module_NewCtor.IsRuntimeSpecialName = false;
                Module_NewCtor.IsSpecialName = false;
                Module_NewCtor.IsStatic = true;
                Module_NewCtor.Access = MethodAttributes.PrivateScope;
                Module_NewCtor.Body = Module_ctor.Body;
                Module_ctor.Body = new CilBody(); // Reset Module .ctor

                // Call VMEntry->ConfigureRT Method -> Module..ctor
                Module_NewCtor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Newobj, module.Import(Runtime.RTSearch.VMEntry_Ctor)));
                Module_NewCtor.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldftn, module.Import(Runtime.RTSearch.VMEntry_ConfigureRT)));
                Module_NewCtor.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Calli, module.Import(Runtime.RTSearch.VMEntry_ConfigureRT).MethodSig));
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                module.GlobalType.Methods.Add(Module_NewCtor); // Add New Module .ctor Method;

                // Call Merge_VMP_Runtime->_ctor Method -> Module..ctor
                Module_ctor.Body = new CilBody(true, new List<Instruction>
                {
                    Instruction.Create(OpCodes.Jmp, Module_NewCtor),
                    Instruction.Create(OpCodes.Ret)
                }, new List<ExceptionHandler>(), new List<Local>());
                ///////////////////////////////////////////////////////////////////////////////////////////////////////

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Protect "Module_ctor" And "Module_NewCtor" Method (ConfuserEX Control Flow)
                //////////////////////////////////////////////////
                MutationConfusion.Execute(Module_NewCtor);
                SugarControlFlow.Execute(Module_ctor);
                SugarControlFlow.Execute(Module_NewCtor);
                //////////////////////////////////////////////////
                #endregion

                #region Hide Methods "Module_ctor" And "Module_NewCtor" Method
                ////////////////////////////////////
                HideMethod.Execute(Module_ctor);
                HideMethod.Execute(Module_NewCtor);
                ////////////////////////////////////
                #endregion
            }
        }

        public void AddMethod(MethodDef method, ModuleDef module, bool isExport, bool Optimize_Code)
        {
            try
            {
                //for (int x = 0; x < method.Body.Instructions.Count; x++)
                //{
                //    if
                //    (
                //       method.Body.Instructions[x].OpCode.Code == Code.Jmp ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Calli ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Cpobj ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Cpblk ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Initblk ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Refanyval ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Mkrefany ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Refanytype ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Arglist ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Unaligned ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Volatile ||
                //       method.Body.Instructions[x].OpCode.Code == Code.Tailcall
                //    )
                //    {
                //        return;
                //    }
                //}

                #region Optimize Code (BETA)
                ///////////////////////////////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    if (Optimize_Code)
                    {
                        Optimize.OptimizeCode.Remove_const_Value(module);
                        Optimize.OptimizeCode.ArmDot_Optimize(module);
                        Optimize.OptimizeCode.Method_Optimize_A(module);
                        Optimize.OptimizeCode.Reduce_MetaData_Confusion(module);
                    }
                }
                ///////////////////////////////////////////////////////////////
                #endregion

                if (!method.OVMAnalyzer()) return;

                #region Calli OpCode Support (BETA)
                ///////////////////////////////////////////////////////////////
                CalliFixer.CalliFixerA(method);
                CalliFixer.CalliFixerB(method);
                CalliFixer.CalliFixerC(method);
                CalliFixer.CalliFixerD(method);
                CalliFixer.CalliFixerE(method);
                ///////////////////////////////////////////////////////////////
                #endregion

                #region Switch OpCode Support (BETA)
                ///////////////////////////////////////////////////////////////
                SwitchFixer.SwitchFixerA(method);
                ///////////////////////////////////////////////////////////////
                #endregion

                #region Jmp OpCode Support (BETA)
                ///////////////////////////////////////////////////////////////
                JmpFixer.JmpFixerA(method);
                ///////////////////////////////////////////////////////////////
                #endregion

                #region Rename Parameters
                //////////////////////////////////////////////////////////////
                if (Scanner.MethodsDF.Count > 0)
                {
                    foreach (var param in method.Parameters)
                        param.Name = Runtime.RNMService.NewName(param.Name);
                }
                //////////////////////////////////////////////////////////////
                #endregion

                #region Add Method Attribute (Virt. ID)
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Runtime.AddMethodID(method);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                MethodList.Add(method, isExport);

                if (!isExport)
                {
                    var thisParam = method.HasThis ? method.Parameters[0].Type : null;

                    var declType = method.DeclaringType;
                    declType.Methods.Remove(method);
                    if (method.SemanticsAttributes != 0)
                    {
                        foreach (var prop in declType.Properties)
                        {
                            if (prop.GetMethod == method)
                                prop.GetMethod = null;
                            if (prop.SetMethod == method)
                                prop.SetMethod = null;
                        }
                        foreach (var evt in declType.Events)
                        {
                            if (evt.AddMethod == method)
                                evt.AddMethod = null;
                            if (evt.RemoveMethod == method)
                                evt.RemoveMethod = null;
                            if (evt.InvokeMethod == method)
                                evt.InvokeMethod = null;
                        }
                    }
                    method.DeclaringType2 = declType;

                    if (thisParam != null)
                        method.Parameters[0].Type = thisParam;
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        public void AntiTamper(ModuleDef module, ModuleWriterOptions options)
        {
            try
            {
                var ctx = new AntiTamperEXEContext();
                var targets = new HashSet<MethodDef>();

                #region Search Virtualized Methods
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                foreach (var method in FullNameToMethod_Class.FullNamesToMethods(module, XMLUtils.Methods_FullName))
                {
                    if (method.HasBody)
                        targets.Add(method);
                }
                targets.Remove(module.GlobalType.FindOrCreateStaticConstructor());
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                ctx.Targets = targets;

                var antitamper = new NormalMode(ctx, module);
                antitamper.HandleRun(Runtime, options);
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        public void ProcessMethods(ModuleDef module, Action<int, int> progress = null)
        {
            if(Processed.Contains(module))
                throw new InvalidOperationException("Module already processed.");

            if(progress == null)
                progress = (num, total) => { };

            var targets = MethodList.Keys.Where(method => method.Module == module).ToList();
            for (var i = 0; i < targets.Count; i++)
            {
                var method = targets[i];
                Instantiation.EnsureInstantiation(method, (spec, instantation) =>
                {
                    if(instantation.Module == module || Processed.Contains(instantation.Module))
                        targets.Add(instantation);
                    MethodList[instantation] = false;
                });
                try
                {
                    MD_VR.Run(method, module, MethodList[method]);
                }
                catch
                {
                    NativeMethods.MessageBox("[Failed]: " + method.FullName, "ERROR!");
                }
                progress(i, targets.Count);
            }
            progress(targets.Count, targets.Count);
            Processed.Add(module);
        }

        public EventHandler2<ModuleWriterEventArgs> CommitModule(ModuleDefMD module, Action<int, int> progress = null)
        {
            if(progress == null)
                progress = (num, total) => { };

            var methods = MethodList.Keys.Where(method => method.Module == module).ToArray();
            for(var i = 0; i < methods.Length; i++)
            {
                var scope = Runtime.LookupMethod(methods[i]);
                var ilTransformer = new ILPostTransformer(methods[i], scope, Runtime);

                ilTransformer.Transform();
                progress(i, MethodList.Count);
            }
            progress(methods.Length, methods.Length);

            return Runtime.RTMutator.CommitModule(module);
        }

        public void WaterMark(ModuleDef module)
        {
            TypeRef attrRef = module.CorLibTypes.GetTypeRef("System", "Attribute");
            var attrType = new TypeDefUser(string.Empty, VMRuntime.Watermark_Class_Name, attrRef);
            module.Types.Add(attrType);

            var ctor = new MethodDefUser(
                ".ctor",
                MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.String),
                MethodImplAttributes.Managed,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            ctor.Body = new CilBody();
            ctor.Body.MaxStack = 1;
            ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            ctor.Body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), attrRef)));
            ctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            attrType.Methods.Add(ctor);

            var attr = new CustomAttribute(ctor);
            attr.ConstructorArguments.Add(new CAArgument(module.CorLibTypes.String, VMRuntime.Watermark));

            module.CustomAttributes.Add(attr);
        }
    }
}