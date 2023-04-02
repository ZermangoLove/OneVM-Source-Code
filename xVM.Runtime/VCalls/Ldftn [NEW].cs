using System;
using System.Collections.Generic;
using System.Reflection;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;
using xVM.Runtime.Execution.Internal;

namespace xVM.Runtime.VCalls
{
    internal class Ldftn : IVCall
    {
        public byte Code => Constants.OPCODELIST[198];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var methodSlot = ctx.Stack[sp--];
            var objectSlot = ctx.Stack[sp];

            if(objectSlot.O != null)
            {
                var method = (MethodInfo) ctx.Instance.Data.LookupReference(methodSlot.U4);
                var type = objectSlot.O.GetType();

                var baseTypes = new List<Type>();
                do
                {
                    baseTypes.Add(type);
                    type = type.BaseType;
                } while(type != null && type != method.DeclaringType);
                baseTypes.Reverse();

                var found = method;
                const BindingFlags fl = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                foreach(var baseType in baseTypes)
                foreach(var m in baseType.GetMethods(fl))
                    if(m.GetBaseDefinition() == found)
                    {
                        found = m;
                        break;
                    }

                ctx.Stack[sp] = new VMSlot {U8 = (ulong) found.MethodHandle.GetFunctionPointer()};
            }
            if(objectSlot.U8 != 0)
            {
                VMTrampoline.Trigger = null;

                VMTrampoline.CreateTrampoline(methodSlot.U8, ctx.Stack[--sp].U4, objectSlot.U4);
                ctx.Stack[sp] = new VMSlot {U8 = VMTrampoline.GetFunctionPointer};

                VMTrampoline.Trigger = new bool?(false);
            }
            else
            {
                var method = (MethodBase) ctx.Instance.Data.LookupReference(methodSlot.U4);
                ctx.Stack[sp] = new VMSlot {U8 = (ulong) method.MethodHandle.GetFunctionPointer()};
            }

            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }
}