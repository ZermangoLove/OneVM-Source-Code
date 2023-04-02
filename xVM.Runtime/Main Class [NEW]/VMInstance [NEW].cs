using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using xVM.Runtime.Data;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;
using xVM.Runtime.Protection;
using System.Security.Cryptography;

namespace xVM.Runtime
{
    internal unsafe class VMInstance
    {
        internal static StackFrame __ExecuteFrame;
        internal static Assembly __ExecuteAssembly;
        internal static Module __ExecuteModule;
        internal static MethodBase __ExecuteMethod;

        public VMInstance(StackFrame frame = null)
        {
            if (frame != null)
            {
                __ExecuteFrame = frame;
                __ExecuteAssembly = frame.GetMethod().Module.Assembly;
                __ExecuteModule = frame.GetMethod().Module;
                __ExecuteMethod = frame.GetMethod();
            }
            Initialize();
        }

        public VMData Data
        {
            get
            {
                return VMData.Instance();
            }
        }

        private void Initialize()
        {
            TypedReference typedReference = __makeref(Constants.OPCODELIST[222]);
            Invoke((void*)(&typedReference), 0, 0, 0, null);
        }

        internal object Invoke(char*[] typedRefs)
        {
            if (Utils.AntiTamperChecker == null)
            {
                object[] attrs = __ExecuteMethod.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    IDAttribute IDAttr = attr as IDAttribute;
                    if (IDAttr != null)
                    {
                        ulong ID = IDAttr.ID;
                        TypedReference typedReference = __makeref(ID);
                        return Invoke((void*)(&typedReference), 0, 0, 0, typedRefs);
                    }
                }
            }
            return null;
        }

        internal object Invoke(void* typeId, ulong codeAddr, ulong key, ulong sigId, char*[] typedRefs, bool Internal_Mode = false)
        {
            try
            {
                if (Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) && Utils.AntiTamperChecker == null)
                {
                    Stack<VMContext> ctxStack = new Stack<VMContext>();
                    VMContext ctx = new VMContext(this);
                    VMExportInfo export = new VMExportInfo();

                    if (ctx != null)
                        ctxStack.Push(ctx);

                    try
                    {
                        if (!Internal_Mode)
                        {
                            var typeIDRef = *(TypedReference*)typeId;
                            export = Data.LookupExport(ctx.Registers[Constants.OPCODELIST[222]].FromObject(TypedReference.ToObject(typeIDRef), __reftype(typeIDRef)));
                            codeAddr = (ulong)(Interpreter.__ILVDATA + export.CodeOffset);
                            key = export.EntryKey;
                        }
                        else
                        {
                            export = Data.LookupExport(sigId);
                        }

                        if (typedRefs == null)
                            typedRefs = new char*[0];

                        Debug.Assert(export.Signature.ParamTypes.Length == typedRefs.Length);
                        ctx.Stack.SetTopPosition(unchecked((uint)typedRefs.Length + 1));
                        for (uint i = 0; i < typedRefs.Length; i++)
                        {
                            var paramType = export.Signature.ParamTypes[i];
                            if (paramType.IsByRef)
                            {
                                ctx.Stack[i + 1] = new VMSlot { O = new TypedRef(typedRefs[i]) };
                            }
                            else
                            {
                                var typedRef = *(TypedReference*)typedRefs[i];
                                ctx.Stack[i + 1] = ctx.Registers[Constants.OPCODELIST[222]].FromObject(TypedReference.ToObject(typedRef), __reftype(typedRef));
                            }
                        }
                        ctx.Stack[unchecked((uint)typedRefs.Length + 1)] = new VMSlot { U8 = 1 };

                        ctx.Registers[Constants.OPCODELIST[22]] = new VMSlot { U8 = key };
                        ctx.Registers[Constants.OPCODELIST[14]] = new VMSlot { U8 = 0 };
                        ctx.Registers[Constants.OPCODELIST[16]] = new VMSlot { U8 = unchecked((ulong)typedRefs.Length) + 1 };
                        ctx.Registers[Constants.OPCODELIST[18]] = new VMSlot { U8 = codeAddr };
                        VMDispatcher.Invoke(ctx);
                        Debug.Assert(ctx.EHStack.Count == 0);

                        object retVal = null;
                        if (export.Signature.RetType != typeof(void))
                        {
                            var retSlot = ctx.Registers[Constants.OPCODELIST[0]];
                            if (Type.GetTypeCode(export.Signature.RetType) == TypeCode.String && retSlot.O == null)
                                retVal = Data.LookupString(retSlot.U4);
                            else
                                retVal = retSlot.ToObject(export.Signature.RetType);
                        }
                        return retVal;
                    }
                    finally
                    {
                        ctx.Stack.FreeAllLocalloc();

                        if (ctxStack.Count > 0)
                            ctx = ctxStack.Pop();
                    }
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
            }
            return null;
        }
    }
}