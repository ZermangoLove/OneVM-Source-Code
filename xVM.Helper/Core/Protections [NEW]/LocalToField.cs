using dnlib.DotNet;
using dnlib.DotNet.Emit;
using xVM.Helper.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace xVM.Helper.Core.Protections
{
    public class LocalToField
    {
        public static void Execute(ModuleDef module)
        {
            var cctor = module.GlobalType.FindOrCreateStaticConstructor();
            var body = cctor.Body.Instructions;
            foreach (var type in module.GetTypes())
            {
                if (type.IsGlobalModuleType)
                    continue;

                foreach (var method in type.Methods)
                {
                    if (!method.HasBody || !method.Body.HasInstructions)
                        continue;

                    var instrs = method.Body.Instructions;

                    if (!instrs.Any(x => x.IsLdcI4()))
                        continue;

                    var first = instrs.First(x => x.IsLdcI4());

                    var value = first.GetLdcI4Value();

                    var field = CreateField(new FieldSig(module.CorLibTypes.Int32));
                    module.GlobalType.Fields.Add(field);

                    body.Insert(0, OpCodes.Ldc_I4.ToInstruction(value));
                    body.Insert(1, OpCodes.Stsfld.ToInstruction(field));

                    first.OpCode = OpCodes.Ldsfld;
                    first.Operand = field;
                }
            }
        }

        public static FieldDefUser CreateField(FieldSig sig)
        {
            return new FieldDefUser(new RandomGenerator().NextHexString(true), sig, FieldAttributes.Public | FieldAttributes.Static);
        }
    }
}
