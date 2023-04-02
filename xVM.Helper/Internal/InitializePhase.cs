using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using dnlib.PE;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core;
using xVM.Helper.Core.RT;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.RT.Renamer;

namespace xVM.Helper.Internal
{
    public class InitializePhase
    {
        public ModuleDefMD ModuleDFMD { get; internal set; } = null;
        public MemoryStream XMLSettings { get; internal set; } = new MemoryStream();

        public static bool isEncryptAllStrings = false;

        public void Run()
        {
            var vr = new Virtualizer();
            var refRepl = new Dictionary<IMemberRef, IMemberRef>();
            var nameService = new NameService();

            var oldType = ModuleDFMD.GlobalType;
            var newType = new TypeDefUser(oldType.Name);
            oldType.Name = nameService.NewName(oldType.Name);
            oldType.BaseType = ModuleDFMD.CorLibTypes.Object.ToTypeDefOrRef();
            ModuleDFMD.Types.Insert(0, newType);

            var old_cctor = oldType.FindOrCreateStaticConstructor();
            var cctor = newType.FindOrCreateStaticConstructor();
            old_cctor.Name = nameService.NewName(old_cctor.Name);
            old_cctor.IsRuntimeSpecialName = false;
            old_cctor.IsSpecialName = false;
            old_cctor.Access = MethodAttributes.PrivateScope;

            cctor.Body = new CilBody(true, new List<Instruction>
            {
                Instruction.Create(OpCodes.Jmp, old_cctor),
                Instruction.Create(OpCodes.Ret)
            }, new List<ExceptionHandler>(), new List<Local>());

            #region Remove <Module>.cctor
            ///////////////////////////////////////////////////////////////
            if (XMLUtils.Methods_FullName.Contains(cctor.FullName))
                XMLUtils.Methods_FullName.Remove(cctor.FullName);
            ///////////////////////////////////////////////////////////////
            #endregion

            #region Virtualize New Ctor
            /////////////////////////////////////////////
            vr.Custom_Method_List.Add(old_cctor.FullName);
            /////////////////////////////////////////////
            #endregion

            #region Virtualize Decrypt String Method
            if (isEncryptAllStrings == true)
            {
                vr.Custom_Method_List.Add(oldType.FindMethod("DecryptString").FullName);
            }    
            #endregion

            for (var i = 0; i < oldType.Methods.Count; i++)
            {
                var nativeMethod = oldType.Methods[i];

                if (nativeMethod.IsNative)
                {
                    var methodStub = new MethodDefUser(nativeMethod.Name, nativeMethod.MethodSig.Clone())
                    {
                        Attributes = MethodAttributes.Assembly | MethodAttributes.Static,
                        Body = new CilBody()
                    };
                    methodStub.Body.Instructions.Add(new Instruction(OpCodes.Jmp, nativeMethod));
                    methodStub.Body.Instructions.Add(new Instruction(OpCodes.Ret));

                    oldType.Methods[i] = methodStub;
                    newType.Methods.Add(nativeMethod);
                    refRepl[nativeMethod] = methodStub;
                }
            }

            vr.Initialize(ModuleDFMD, oldType, XMLSettings);

            vr.ConfigureModuleAndMerge(ModuleDFMD);
            vr.WaterMark(ModuleDFMD); // Discord WaterMark

            var ToProcess = new Dictionary<ModuleDef, HashSet<MethodDef>>();
            foreach (var entry in new Scanner(ModuleDFMD, vr.Runtime.CompressionService).Scan())
            {
                vr.AddMethod(entry.Item1, ModuleDFMD, entry.Item2, true);
                ToProcess.AddListEntry(ModuleDFMD, entry.Item1);
            }

            Utils.ExecuteModuleWriterOptions = new ModuleWriterOptions(ModuleDFMD)
            {
                Logger = DummyLogger.NoThrowInstance,
                WritePdb = true,
            };

            vr.AntiTamper(ModuleDFMD, Utils.ExecuteModuleWriterOptions);

            Utils.ExecuteModuleWriterOptions.WriterEvent += delegate (object sender, ModuleWriterEventArgs e)
            {
                if (vr.CommitListener != null)
                {
                    vr.CommitListener((ModuleWriterBase)sender, e);
                }

                if (e.Event == ModuleWriterEvent.MDBeginWriteMethodBodies && ToProcess.ContainsKey(ModuleDFMD))
                {
                    vr.ProcessMethods(ModuleDFMD);

                    foreach (var repl in refRepl)
                    {
                        vr.Runtime.Descriptor.Data.ReplaceReference(repl.Key, repl.Value);
                    }

                    vr.CommitListener = vr.CommitModule(ModuleDFMD);
                }
            };
        }
    }
}