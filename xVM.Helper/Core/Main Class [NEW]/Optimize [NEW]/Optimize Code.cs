using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Optimize
{
    internal static class OptimizeCode
    {
        internal static void Remove_const_Value(ModuleDef module)
        {
            try
            {
                foreach (TypeDef type in module.Types)
                {
                    for (int x = 0; x < type.Fields.Count; x++)
                    {
                        FieldDef field = type.Fields[x];

                        if (field.HasConstant && field.ElementType.Equals(ElementType.Object))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.Array))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.String))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.Boolean))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.Char))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.Ptr))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.SZArray))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.Class))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.I))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.I1))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.I2))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.I4))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.I8))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.R))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.R4))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.R8))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.U))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.U1))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.U4))
                        {
                            type.Fields.RemoveAt(x);
                        }

                        if (field.HasConstant && field.ElementType.Equals(ElementType.U8))
                        {
                            type.Fields.RemoveAt(x);
                        }
                    }
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        public static void ArmDot_Optimize(ModuleDef module)
        {
            foreach (TypeDef type in module.Types)
                foreach (MethodDef _method in type.Methods)
                    if (_method.HasBody)
                    {
                        using (new AutoMethodBodySimplifyOptimize(_method, false))
                        {
                            _method.Body.MaxStack = 65535;
                            var methodReference = _method.Module.Import(typeof(IntPtr).GetConstructor(new Type[] { typeof(int) }));
                            var methodReference2 = _method.Module.Import(typeof(IntPtr).GetConstructor(new Type[] { typeof(long) }));
                            var methodReference3 = _method.Module.Import(typeof(IntPtr).GetMethod("ToInt32", Type.EmptyTypes));
                            var methodReference4 = _method.Module.Import(typeof(IntPtr).GetMethod("ToInt64", Type.EmptyTypes));
                            var methodReference5 = _method.Module.Import(typeof(UIntPtr).GetConstructor(new Type[] { typeof(uint) }));
                            var methodReference6 = _method.Module.Import(typeof(UIntPtr).GetConstructor(new Type[] { typeof(ulong) }));
                            var methodReference7 = _method.Module.Import(typeof(UIntPtr).GetMethod("ToUInt32", Type.EmptyTypes));
                            var methodReference8 = _method.Module.Import(typeof(UIntPtr).GetMethod("ToUInt64", Type.EmptyTypes));
                            var list = new List<Instruction>();
                            var dictionary = new Dictionary<Instruction, Instruction>();

                            Instruction instruction = null;
                            foreach (var inst in _method.Body.Instructions)
                            {
                                Instruction instA = null;
                                Instruction instB = null;
                                if (inst.OpCode == OpCodes.Newobj)
                                {
                                    var methodReference9 = (IMethod)inst.Operand;
                                    if (MethodReferenceEqualityComparer.Singleton.Equals(methodReference9, methodReference) || MethodReferenceEqualityComparer.Singleton.Equals(methodReference9, methodReference2))
                                        instA = new Instruction(OpCodes.Conv_I);
                                    else if (MethodReferenceEqualityComparer.Singleton.Equals(methodReference9, methodReference5) || MethodReferenceEqualityComparer.Singleton.Equals(methodReference9, methodReference6))
                                        instA = new Instruction(OpCodes.Conv_U);
                                }
                                else if (inst.OpCode == OpCodes.Call)
                                {
                                    var mRef = (IMethod)inst.Operand;
                                    if (MethodReferenceEqualityComparer.Singleton.Equals(mRef, methodReference4))
                                    {
                                        instB = instruction.CreateLoadInstructionInsteadOfLoadAddress(inst);
                                        if (instB != null)
                                            instA = new Instruction(OpCodes.Conv_I8);
                                    }
                                    else if (MethodReferenceEqualityComparer.Singleton.Equals(mRef, methodReference3))
                                    {
                                        instB = instruction.CreateLoadInstructionInsteadOfLoadAddress(inst);
                                        if (instB != null)
                                            instA = new Instruction(OpCodes.Conv_I4);
                                    }
                                    else if (MethodReferenceEqualityComparer.Singleton.Equals(mRef, methodReference8))
                                    {
                                        instB = instruction.CreateLoadInstructionInsteadOfLoadAddress(inst);
                                        if (instB != null)
                                            instA = new Instruction(OpCodes.Conv_U8);
                                    }
                                    else if (MethodReferenceEqualityComparer.Singleton.Equals(mRef, methodReference7))
                                    {
                                        instB = instruction.CreateLoadInstructionInsteadOfLoadAddress(inst);
                                        if (instB != null)
                                            instA = new Instruction(OpCodes.Conv_U4);
                                    }
                                }

                                if (instA == null)
                                    instA = inst;

                                list.Add(instA);
                                dictionary.Add(inst, instA);

                                if (instB != null)
                                {
                                    list.Insert(list.IndexOf(instruction), instB);
                                    list.Remove(instruction);
                                    dictionary.Remove(instruction);
                                    dictionary.Add(instruction, instB);
                                }
                                instruction = inst;
                            }
                            _method.Body.SetNewInstructions(list, dictionary);
                        }
                    }
        }

        internal static void Reduce_MetaData_Confusion(object module)
        {
            try
            {
                Func<ModuleDef, IList<IDnlibDef>> Targets = (ModuleDef md) => md.FindDefinitions().ToList<IDnlibDef>();
                IMemberDef memberDef = Targets((ModuleDef)module) as IMemberDef;

                TypeDef typeDef = (memberDef as TypeDef);
                if (typeDef != null && !typeDef.IsTypePublic())
                {
                    if (typeDef.IsEnum)
                    {
                        int num = 0;
                        while (typeDef.Fields.Count != 1)
                        {
                            if (typeDef.Fields[num].Name != "value__")
                            {
                                typeDef.Fields.RemoveAt(num);
                            }
                            else
                            {
                                num++;
                            }
                        }
                        return;
                    }
                }
                else if (memberDef is EventDef)
                {
                    if (memberDef.DeclaringType != null)
                    {
                        memberDef.DeclaringType.Events.Remove(memberDef as EventDef);
                        return;
                    }
                }
                else if (memberDef is PropertyDef && memberDef.DeclaringType != null)
                {
                    memberDef.DeclaringType.Properties.Remove(memberDef as PropertyDef);
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }

        internal static void Method_Optimize_A(object module)
        {
            try
            {
                var moduleDefinition = (ModuleDef)module;
                foreach (var typeDefinition in moduleDefinition.GetTypes())
                {
                    foreach (var methodDefinition in typeDefinition.Methods)
                    {
                        if (methodDefinition.HasBody)
                        {
                            methodDefinition.Body.OptimizeMacros();
                            methodDefinition.Body.OptimizeBranches();
                        }
                    }
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), "ERROR! - " + except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }
    }
}
