using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using xVM.Runtime.Data;

namespace xVM.Runtime.Execution.Internal
{
    internal class VMTrampoline
    {
        private static readonly Hashtable trampolines = new Hashtable();

        private delegate RuntimeMethodHandle GetMethodDescriptor(DynamicMethod dymethod);

        private static readonly GetMethodDescriptor getDesc;
        private static readonly MethodInfo EntryStub;
        public static ulong GetFunctionPointer;
        public static bool? Trigger;

        static VMTrampoline()
        {
            VMTrampoline trampoline = new VMTrampoline();
            foreach (var method in __reftype(__makeref(trampoline)).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                EntryStub = method;
            }

            getDesc = (GetMethodDescriptor)Delegate.CreateDelegate
            (
                typeof(GetMethodDescriptor),
                typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic)
            );
        }

        public unsafe static object CreateTrampoline(ulong codeAddr, ulong key, ulong sigId, char*[] typedRefs = null)
        {
            var ins = new VMInstance(VMInstance.__ExecuteFrame);
            var sig = ins.Data.LookupExport(unchecked(sigId)).Signature;

            if (Trigger == null)
            {
                var dm = trampolines[codeAddr];
                dm = (DynamicMethod)trampolines[codeAddr];
                if (dm != null)
                {
                    GetFunctionPointer = (ulong)getDesc((DynamicMethod)dm).GetFunctionPointer();
                }

                var dynamicMethod = new DynamicMethod(string.Empty, sig.RetType, sig.ParamTypes, VMInstance.__ExecuteModule, true);
                var gen = dynamicMethod.GetILGenerator();
                lock (trampolines)
                {
                    #region VMEntry'daki InvokeInternal Bölümünü çalıştırmak için gereken IL kodu.
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I8, unchecked((long)codeAddr));
                    gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I8, unchecked((long)key));
                    gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I8, unchecked((long)sigId));

                    gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, sig.ParamTypes.Length);
                    gen.Emit(System.Reflection.Emit.OpCodes.Newarr, typeof(char*));

                    for (var i = 0; i < sig.ParamTypes.Length; i++)
                    {
                        gen.Emit(System.Reflection.Emit.OpCodes.Dup);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, i);
                        if (sig.ParamTypes[i].IsByRef)
                        {
                            gen.Emit(System.Reflection.Emit.OpCodes.Ldarg, i);
                            gen.Emit(System.Reflection.Emit.OpCodes.Mkrefany, sig.ParamTypes[i].GetElementType());
                        }
                        else
                        {
                            gen.Emit(System.Reflection.Emit.OpCodes.Ldarga, i);
                            gen.Emit(System.Reflection.Emit.OpCodes.Mkrefany, sig.ParamTypes[i]);
                        }
                        var local = gen.DeclareLocal(typeof(TypedReference));
                        gen.Emit(System.Reflection.Emit.OpCodes.Stloc, local);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldloca, local);
                        gen.Emit(System.Reflection.Emit.OpCodes.Conv_I);
                        gen.Emit(System.Reflection.Emit.OpCodes.Stelem_I);
                    }

                    if (sig.RetType == typeof(void))
                    {
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldftn, EntryStub);
                        gen.EmitCalli(System.Reflection.Emit.OpCodes.Calli, EntryStub.CallingConvention, typeof(object), new Type[] { typeof(ulong), typeof(ulong), typeof(ulong), typeof(char*[]) }, null);
                        gen.Emit(System.Reflection.Emit.OpCodes.Pop);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    }
                    else if (sig.RetType.IsValueType)
                    {
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldftn, EntryStub);
                        gen.EmitCalli(System.Reflection.Emit.OpCodes.Calli, EntryStub.CallingConvention, typeof(object), new Type[] { typeof(ulong), typeof(ulong), typeof(ulong), typeof(char*[]) }, null);
                        gen.Emit(System.Reflection.Emit.OpCodes.Unbox_Any, sig.RetType);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    }
                    else if (sig.RetType.IsArray)
                    {
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldftn, EntryStub);
                        gen.EmitCalli(System.Reflection.Emit.OpCodes.Calli, EntryStub.CallingConvention, typeof(object), new Type[] { typeof(ulong), typeof(ulong), typeof(ulong), typeof(char*[]) }, null);
                        gen.Emit(System.Reflection.Emit.OpCodes.Castclass, typeof(Array));
                        gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    }
                    else if (sig.RetType.IsPointer)
                    {
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldftn, EntryStub);
                        gen.EmitCalli(System.Reflection.Emit.OpCodes.Calli, EntryStub.CallingConvention, typeof(object), new Type[] { typeof(ulong), typeof(ulong), typeof(ulong), typeof(char*[]) }, null);

                        var Unbox = typeof(Pointer).GetMethod("Unbox", BindingFlags.Public | BindingFlags.Static);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldftn, Unbox);
                        gen.EmitCalli(System.Reflection.Emit.OpCodes.Calli, Unbox.CallingConvention, typeof(void*), new Type[] { typeof(object) }, null);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    }
                    else
                    {
                        gen.Emit(System.Reflection.Emit.OpCodes.Ldftn, EntryStub);
                        gen.EmitCalli(System.Reflection.Emit.OpCodes.Calli, EntryStub.CallingConvention, typeof(object), new Type[] { typeof(ulong), typeof(ulong), typeof(ulong), typeof(char*[]) }, null);
                        gen.Emit(System.Reflection.Emit.OpCodes.Castclass, sig.RetType);
                        gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion
                }
                dm = dynamicMethod;
                trampolines[codeAddr] = dm;
                GetFunctionPointer = (ulong)getDesc((DynamicMethod)dm).GetFunctionPointer();
                return null;
            }
            return ins.Invoke(null, unchecked(codeAddr), unchecked(key), sigId, typedRefs, true);
        }
    }
}