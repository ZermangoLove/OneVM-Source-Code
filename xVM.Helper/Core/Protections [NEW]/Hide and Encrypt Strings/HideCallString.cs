using System;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.Services;
using xVM.Helper.Core.RT.Renamer;
using System.Linq;

namespace xVM.Helper.Core.Protections
{
    public class HideCallString
    {
        public ModuleDef Module { get; set; }
        public Dictionary<string, FieldDef> Numbers { get; set; }

        private MethodDef EncryptOrDecrypt;

        public HideCallString(ModuleDef module)
        {
            this.Numbers = new Dictionary<string, FieldDef>();
            this.Module = module;
        }

        public IList<MethodDef> Inject_Runtime()
        {
            var typeDef = ModuleDefMD.Load(typeof(XORStr_Runtime).Module).ResolveTypeDef(MDToken.ToRID(typeof(XORStr_Runtime).MetadataToken));
            var members = Helpers.Injection.InjectHelper.Inject(typeDef, this.Module.GlobalType, this.Module);
            EncryptOrDecrypt = members.OfType<MethodDef>().Single(methodx => methodx.Name == "EncryptOrDecrypt");

            var methods = new HashSet<MethodDef>();
            methods.Add(EncryptOrDecrypt);

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

        public void Execute(MethodDef method)
        {
            if (method.HasBody && method.Body.HasInstructions)
            {
                HideAllStr(method, this.Module);
            }
        }

        private FieldDef Add(string value, ModuleDef module)
        {
            var rand = new RandomGenerator();
            var field = new FieldDefUser(new NameService().NewName(rand.NextString()), new FieldSig(module.CorLibTypes.String), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
            field.DeclaringType = null;

            Module.GlobalType.Fields.Add(field);

            var cctor = Module.GlobalType.FindOrCreateStaticConstructor();

            var key = rand.NextInt32();
            var encrypted = XORStr_Runtime.EncryptOrDecrypt(value, key);

            cctor.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldstr, encrypted));
            cctor.Body.Instructions.Insert(1, new Instruction(OpCodes.Ldc_I4, key));
            cctor.Body.Instructions.Insert(2, new Instruction(OpCodes.Call, EncryptOrDecrypt));
            cctor.Body.Instructions.Insert(3, new Instruction(OpCodes.Stsfld, field));

            return field;
        }

        private void HideAllStr(MethodDef method, ModuleDef module)
        {
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                var inst = method.Body.Instructions[i];
                if (inst.OpCode == OpCodes.Ldstr && this.Module.GlobalType.Fields.Count < 65000)
                {
                    try
                    {
                        string value = inst.Operand.ToString();

                        FieldDef field;
                        if (!Numbers.TryGetValue(value, out field))
                        {
                            field = Add(value, module);
                            Numbers.Add(value, field);
                        }

                        inst.OpCode = OpCodes.Ldsfld;
                        inst.Operand = field;
                    }
                    catch { }
                }
            }
        }
    }
}
