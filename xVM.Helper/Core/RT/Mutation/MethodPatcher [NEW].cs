using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Protections;

namespace xVM.Helper.Core.RT.Mutation
{
    internal class MethodPatcher
    {
        internal void Patch(MethodDef method, ModuleDef module, VMRuntime runtime /*, ulong ID */)
        {
            method.Body = new CilBody();
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region Object Array
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, 1)); // array içinde kaç tane işlem ekliyecen onu belirleme 1 tane varsa 1 2 tane varsa 2 yaz
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Newarr, method.Module.CorLibTypes.Object.ToTypeDefOrRef())); // buda arrayı object olarak belirlemek için.
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Dup));


            ////////// Array[0]
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, 0)); // Array[0]
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I8, unchecked((long)ID)));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            ////////// Eğer Array'a bir işlem daha ekliyeceksen kodlarının aralarına bu 2 kodu ekle:
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Dup));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region ID Pointer (TypedReference)
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Local localA = new Local(method.Module.CorLibTypes.UInt64);
            //Local localB = new Local(method.Module.CorLibTypes.TypedReference);

            //method.Body.Variables.Add(localA);
            //method.Body.Variables.Add(localB);

            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I8, unchecked((long)ID)));
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, localA));
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloca, localA));

            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, localA.Type.ToTypeDefOrRef()));
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, localB));
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloca, localB));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            // new VMEntry();
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, method.Module.Import(runtime.RTSearch.VMEntry_Ctor)));

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (method.Parameters.Count == 0)
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldnull));
            }
            else
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Newarr, new PtrSig(method.Module.CorLibTypes.Char).ToTypeDefOrRef()));

                foreach (var param in method.Parameters)
                {
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Dup));
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, param.Index));
                    if (param.Type.IsByRef)
                    {
                        method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, param));
                        method.Body.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, param.Type.Next.ToTypeDefOrRef()));
                    }
                    else
                    {
                        method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarga, param));
                        method.Body.Instructions.Add(Instruction.Create(OpCodes.Mkrefany, param.Type.ToTypeDefOrRef()));
                    }
                    var locals = new Local(method.Module.CorLibTypes.TypedReference);
                    method.Body.Variables.Add(locals);
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, locals));
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloca, locals));
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Conv_I));
                    method.Body.Instructions.Add(Instruction.Create(OpCodes.Stelem_I));
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (method.ReturnType.ElementType == ElementType.Void)
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, method.Module.Import(runtime.RTSearch.VMEntry_Invoke)));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Calli, method.Module.Import(runtime.RTSearch.VMEntry_Invoke).MethodSig));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Pop));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            }
            else if (method.ReturnType.IsValueType)
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, method.Module.Import(runtime.RTSearch.VMEntry_Invoke)));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Calli, method.Module.Import(runtime.RTSearch.VMEntry_Invoke).MethodSig));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Unbox_Any, method.ReturnType.ToTypeDefOrRef()));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            }
            else if (method.ReturnType.IsSZArray)
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, method.Module.Import(runtime.RTSearch.VMEntry_Invoke)));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Calli, method.Module.Import(runtime.RTSearch.VMEntry_Invoke).MethodSig));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Castclass, module.Import(typeof(System.Array))));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            }
            else if (method.ReturnType.IsPointer)
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, method.Module.Import(runtime.RTSearch.VMEntry_Invoke)));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Calli, method.Module.Import(runtime.RTSearch.VMEntry_Invoke).MethodSig));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, module.Import(typeof(System.Reflection.Pointer).GetMethod("Unbox", BindingFlags.Public | BindingFlags.Static))));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Calli, module.Import(typeof(System.Reflection.Pointer).GetMethod("Unbox", BindingFlags.Public | BindingFlags.Static)).MethodSig));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            }
            else
            {
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, method.Module.Import(runtime.RTSearch.VMEntry_Invoke)));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Calli, method.Module.Import(runtime.RTSearch.VMEntry_Invoke).MethodSig));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Castclass, method.ReturnType.ToTypeDefOrRef()));
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            method.Body.OptimizeMacros();
            method.Body.OptimizeBranches();

            // Hide Methods Protection
            HideMethod.Execute(method);
            /////////////////////////////
        }
    }
}