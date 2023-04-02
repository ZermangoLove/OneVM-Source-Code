using System;
using System.Reflection;
using System.Diagnostics;

using xVM.Runtime.Protection;

namespace xVM.Runtime
{
    public unsafe class VMEntry
    {
        [VMProtect.BeginMutation]
        public object Invoke(char*[] typedRefs = null)
        {
            if (AntiDumpV2.AntiDumpIsRunning == true)
            {
                VMInstance.__ExecuteMethod = new StackFrame(1, true).GetMethod();
                return new VMInstance().Invoke(typedRefs);
            }
            return null;
        }

        [VMProtect.BeginMutation]
        public void ConfigureRT()
        {
            #region Anti Dump Protection For RT
            /////////////////////////////
            AntiDumpV2.Initialize();
            /////////////////////////////
            #endregion

            if (AntiDumpV2.AntiDumpIsRunning == true)
            {
                VMInstance.__ExecuteFrame = new StackFrame(1, true);
                VMInstance.__ExecuteAssembly = VMInstance.__ExecuteFrame.GetMethod().Module.Assembly;
                VMInstance.__ExecuteModule = VMInstance.__ExecuteFrame.GetMethod().Module;

                new AntiTamperEXEC().Initialize();
            }
        }
    }
}