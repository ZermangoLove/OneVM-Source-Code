using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using dnlib.DotNet;

using xVM.Helper.Core;
using System.Security.Cryptography;

namespace xVM.Helper.Core.RT
{
    internal static class SaveVMPRuntimeLib
    {
        public static string WriteDirectory
        {
            get;
            set;
        }

        public static MemoryStream Runtime_VMP_Protected_Merge_Mode;

        public static void Save(VMRuntime runtime, bool MergeMode = false)
        {
            var wow64Value = IntPtr.Zero;
            NativeMethods.Wow64DisableWow64FsRedirection(ref wow64Value);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           
            var outDllName = runtime.RTModule.Assembly.Name + ".dll";
            var Runtime_Output_Dll_Location = Path.Combine(Path.GetTempPath(), outDllName);
            var TempDir = System.Environment.GetEnvironmentVariable("TEMP");

            var one = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(runtime.RTSearch.VMData.Name));
            var two = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(runtime.RTSearch.VMTrampoline.Name));
            Buffer.BlockCopy(one, 0, one, 0, 16);
            Buffer.BlockCopy(two, 0, two, 0, 16);
            var TempFile = Path.Combine(Path.Combine(Path.GetTempPath(), new Guid(one).ToString().Replace("-", string.Empty)), new Guid(two).ToString().Substring(0, 7));

            var VMPEXEName = "22c36ed9";
            var VMPPROJName = "6c63ba12";

            var ThreadA = new Thread(() =>
            {
                if (!Directory.Exists(TempDir))
                    Directory.CreateDirectory(TempDir);

                if (!Directory.Exists(Path.GetDirectoryName(TempFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(TempFile));

                if (File.Exists(Path.Combine(Path.GetDirectoryName(TempFile), VMPPROJName)))
                    File.Delete(Path.Combine(Path.GetDirectoryName(TempFile), VMPPROJName));

                if (File.Exists(Path.Combine(Path.GetDirectoryName(TempFile), VMPEXEName)))
                    File.Delete(Path.Combine(Path.GetDirectoryName(TempFile), VMPEXEName));

                if (File.Exists(Runtime_Output_Dll_Location))
                    File.Delete(Runtime_Output_Dll_Location);

                File.WriteAllBytes(TempFile, VMRuntime.RTLibStream.ToArray());

                Thread.Sleep(300);
            });
            ThreadA.Start();
            ThreadA.Join();

            var ThreadB = new Thread(() =>
            {
                var PROJ = new StringBuilder();
                PROJ.Append(Encoding.Default.GetString(Properties.Resources.SeKLxcb6qX0KIM8JWy3GiA));
                PROJ.Replace("dlldir", TempFile);
                PROJ.Replace("dlloutdir", Runtime_Output_Dll_Location);

                File.WriteAllText(Path.Combine(Path.GetDirectoryName(TempFile), VMPPROJName), PROJ.ToString());
                File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(TempFile), VMPEXEName), Properties.Resources._80DMUlAjbQM3mEyuNvXTog);

                Thread.Sleep(300);
            });
            ThreadB.Start();
            ThreadB.Join();

            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "\"" + Path.Combine(Path.GetDirectoryName(TempFile), VMPEXEName) + "\"",
                Arguments = "\"" + Path.Combine(Path.GetDirectoryName(TempFile), VMPPROJName) + "\""
            };
            process.Start();
            process.WaitForExit();

            Thread.Sleep(300);

            if (MergeMode == true)
            {
                var stream = new MemoryStream();
                var MergeThread = new Thread(() =>
                {
                    var fileBytes = File.ReadAllBytes(Runtime_Output_Dll_Location);
                    var Hash = MD5.Create().ComputeHash(fileBytes);

                    stream.Write(fileBytes, 0, fileBytes.Length);

                    var streamOut = stream.ToArray();

                    stream.Flush();

                    Runtime_VMP_Protected_Merge_Mode = new MemoryStream(streamOut);
                });
                MergeThread.Start();
                MergeThread.Join();

                Thread.Sleep(500);

                Runtime_VMP_Protected_Merge_Mode.Flush();
                Runtime_VMP_Protected_Merge_Mode.Close();

                if (Directory.Exists(Path.GetDirectoryName(TempFile)))
                    Directory.Delete(Path.GetDirectoryName(TempFile), true);

                if (File.Exists(Runtime_Output_Dll_Location))
                    File.Delete(Runtime_Output_Dll_Location);
            }
            else
            {
                var stream = new MemoryStream();
                var NormalThread = new Thread(() =>
                {
                    var fileBytes = File.ReadAllBytes(Runtime_Output_Dll_Location);
                    var Hash = MD5.Create().ComputeHash(fileBytes);

                    stream.Write(fileBytes, 0, fileBytes.Length);

                    stream.Flush();

                    File.WriteAllBytes(Path.Combine(WriteDirectory, outDllName), stream.ToArray());
                });
                NormalThread.Start();
                NormalThread.Join();

                stream.Close();

                Thread.Sleep(500);

                if (Directory.Exists(Path.GetDirectoryName(TempFile)))
                    Directory.Delete(Path.GetDirectoryName(TempFile), true);

                if (File.Exists(Runtime_Output_Dll_Location))
                    File.Delete(Runtime_Output_Dll_Location);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            NativeMethods.Wow64RevertWow64FsRedirection(wow64Value);
        }
    }
}