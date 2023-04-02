using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.Services;
using xVM.Helper.Core.RT.Renamer;

namespace xVM.Helper.Core.Protections
{
    public static class MergeNET_Inject
    {
        public static IList<MethodDef> Execute(ModuleDef module, IList<string> DLLS)
        {
            var compression = new CompressionService();

            var typeDef = ModuleDefMD.Load(typeof(MergeNET_Runtime).Module).ResolveTypeDef(MDToken.ToRID(typeof(MergeNET_Runtime).MetadataToken));
            var members = Helpers.Injection.InjectHelper.Inject(typeDef, module.GlobalType, module);
            var init = members.OfType<MethodDef>().Single(method => method.Name == "Initialize");

            var methods = new HashSet<MethodDef>();
            methods.Add(init);
            methods.Add(members.OfType<MethodDef>().Single(method => method.Name == "ReadDLList"));
            methods.Add(members.OfType<MethodDef>().Single(method => method.Name == "Decompress"));

            var Module_ctor = module.GlobalType.FindOrCreateStaticConstructor();
            Module_ctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldftn, init));
            Module_ctor.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Calli, init.MethodSig));

            var sb_DLLS = new StringBuilder();

            foreach (var location in DLLS.Distinct())
            {
                var dlName = Path.GetFileNameWithoutExtension(location) + ".xMerge";

                sb_DLLS.Append(dlName);

                module.Resources.Add(new EmbeddedResource(dlName, compression.GZIP_Compress(File.ReadAllBytes(location))));
            }

            module.Resources.Add(new EmbeddedResource("__xMerge__List.resources", compression.GZIP_Compress(Encoding.Default.GetBytes(sb_DLLS.ToString()))));

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
