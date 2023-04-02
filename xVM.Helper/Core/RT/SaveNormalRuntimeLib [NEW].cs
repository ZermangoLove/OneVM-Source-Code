using System;
using System.IO;
using System.Security.Cryptography;

using dnlib.DotNet;

using xVM.Helper.Core;

namespace xVM.Helper.Core.RT
{
    internal static class SaveNormalRuntimeLib
    {
        public static string WriteDirectory
        {
            get;
            set;
        }

        public static void Save(VMRuntime runtime)
        {
            var outDllName = runtime.RTModule.Assembly.Name + ".dll";
            var Runtime_Output_Dll_Location = Path.Combine(WriteDirectory, outDllName);

            if (!Directory.Exists(Path.GetDirectoryName(Runtime_Output_Dll_Location)))
                Directory.CreateDirectory(Path.GetDirectoryName(Runtime_Output_Dll_Location));

            if (File.Exists(Runtime_Output_Dll_Location))
                File.Delete(Runtime_Output_Dll_Location);

            File.WriteAllBytes(Runtime_Output_Dll_Location, VMRuntime.RTLibStream.ToArray());
        }
    }
}