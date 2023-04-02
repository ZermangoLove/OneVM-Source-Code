using System;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.RT.Mutation
{
    internal static class RuntimePatcher
    {
        public static void Patch(VMRuntime runtime)
        {
            PatchDispatcher(runtime.RTModule);
        }

        private static void PatchDispatcher(ModuleDef runtime)
        {
            var dispatcher = runtime.Find(RTMap.VMDispatcher, true);
            var dispatcherRun = dispatcher.FindMethod(RTMap.VMEntry_Invoke);
            foreach(var eh in dispatcherRun.Body.ExceptionHandlers)
                if(eh.HandlerType == ExceptionHandlerType.Catch)
                    eh.CatchType = runtime.CorLibTypes.Object.ToTypeDefOrRef();
            PatchDoThrow(dispatcher.FindMethod(RTMap.VMDispatcher_DoThrow).Body);
            dispatcher.Methods.Remove(dispatcher.FindMethod(RTMap.VMDispatcher_Throw));
        }

        private static void PatchDoThrow(CilBody body)
        {
            for(var i = 0; i < body.Instructions.Count; i++)
            {
                var method = body.Instructions[i].Operand as IMethod;
                if (method != null && method.Name == RTMap.VMDispatcher_Throw)
                    body.Instructions.RemoveAt(i);
            }
        }
    }
}