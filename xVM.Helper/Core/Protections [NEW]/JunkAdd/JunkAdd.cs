using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    public class JunkAdd
    {
        public static int junksMethods = 5; // Can add more

        public static int Junks = 1000; // Default

        public void Execute(ModuleDef mod)
        {
            foreach (TypeDef type in mod.GetTypes())
            {
                for (int i = 0; i < junksMethods * 4; i++) // junksMethods x 4
                {
                    var meth1 = new MethodDefUser(JunkGen.RandomString(JunkGen.rdm.Next(10, 30), 1), MethodSig.CreateStatic(mod.CorLibTypes.Void), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                    type.Methods.Add(meth1);
                    var memberRef = new MemberRefUser(mod, ".ctor", MethodSig.CreateInstance(mod.CorLibTypes.Void, mod.CorLibTypes.String), mod.CorLibTypes.GetTypeRef("System", "Exception"));
                    meth1.Body = new CilBody()
                    {
                        Instructions =
                        {
                            Instruction.Create(OpCodes.Ldstr, "Error, xVM Runtime library not loadded!"),
                            Instruction.Create(OpCodes.Newobj, memberRef),
                            Instruction.Create(OpCodes.Throw),
                        }
                    };
                }
            }
            for (int i = 0; i < Junks; i++)
            {
                var junk2 = new TypeDefUser(JunkGen.RandomString(JunkGen.rdm.Next(10, 30), 1), JunkGen.RandomString(JunkGen.rdm.Next(10, 30), 1), mod.CorLibTypes.Object.TypeDefOrRef);
                mod.Types.Add(junk2);
                for (int ii = 0; ii < junksMethods; ii++)
                {
                    var meth1 = new MethodDefUser(JunkGen.RandomString(JunkGen.rdm.Next(10, 30), 1), MethodSig.CreateStatic(mod.CorLibTypes.Object), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                    junk2.Methods.Add(meth1);

                    meth1.Body = new CilBody()
                    {
                        Variables =
                        {
                            new Local(mod.CorLibTypes.Object)
                        },
                        Instructions =
                        {
                            Instruction.Create(OpCodes.Nop),
                            Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)13),
                            Instruction.Create(OpCodes.Newarr, mod.CorLibTypes.Object),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_0),
                            Instruction.Create(OpCodes.Ldstr, "Fuck You"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_1),
                            Instruction.Create(OpCodes.Ldstr, "Get Out Of Here Now!"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_2),
                            Instruction.Create(OpCodes.Ldstr, "-"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_3),
                            Instruction.Create(OpCodes.Ldstr, "This File Was Protected"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_4),
                            Instruction.Create(OpCodes.Ldstr, "With"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_5),
                            Instruction.Create(OpCodes.Ldstr, "xVM"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_6),
                            Instruction.Create(OpCodes.Ldstr, " "),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_7),
                            Instruction.Create(OpCodes.Ldstr, "Fuck You"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_8),
                            Instruction.Create(OpCodes.Ldstr, "Get Out Of Here Now!"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)9),
                            Instruction.Create(OpCodes.Ldstr, "-"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)10),
                            Instruction.Create(OpCodes.Ldstr, "This File Was Protected"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)11),
                            Instruction.Create(OpCodes.Ldstr, "With"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Dup),
                            Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)12),
                            Instruction.Create(OpCodes.Ldstr, "xVM"),
                            Instruction.Create(OpCodes.Stelem_Ref),
                            Instruction.Create(OpCodes.Stloc_0),
                            Instruction.Create(OpCodes.Nop), // 56 => 57
                            Instruction.Create(OpCodes.Ldloc_0),
                            Instruction.Create(OpCodes.Ret)
                        }
                    };
                    meth1.Body.Instructions[56].OpCode = OpCodes.Br_S;
                    meth1.Body.Instructions[56].Operand = meth1.Body.Instructions[57];
                }
            }
        }
    }
}
