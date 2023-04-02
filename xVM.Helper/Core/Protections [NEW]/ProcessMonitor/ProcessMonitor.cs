using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xVM.Helper.Core.RT.Renamer;

namespace xVM.Helper.Core.Protections
{
    public static class ProcessMonitor
    {
        public static IList<MethodDef> Execute(ModuleDef module)
        {
            var typeDef = ModuleDefMD.Load(typeof(ProcessMonitor_Runtime).Module).ResolveTypeDef(MDToken.ToRID(typeof(ProcessMonitor_Runtime).MetadataToken));
            var members = Helpers.Injection.InjectHelper.Inject(typeDef, module.GlobalType, module);
            var init = members.OfType<MethodDef>().Single(method => method.Name == "StartPM");

            var methods = new HashSet<MethodDef>();
            methods.Add(init);
            methods.Add(members.OfType<MethodDef>().Single(method => method.Name == "PMScan"));

            var Module_ctor = module.GlobalType.FindOrCreateStaticConstructor();
            Module_ctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldftn, init));
            Module_ctor.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Calli, init.MethodSig));

            #region Rename Merged Methods
            ///////////////////////////////////////////////////////////////////////
            foreach (IDnlibDef def in members)
            {
                IMemberDef memberDef = def as IMemberDef;

                if ((memberDef as MethodDef) != null)
                    memberDef.Name = new NameService().NewName(memberDef.Name);
                else if ((memberDef as FieldDef) != null)
                    memberDef.Name = new NameService().NewName(memberDef.Name);
            }
            ///////////////////////////////////////////////////////////////////////
            #endregion

            return methods.ToList();
        }
    }
}
