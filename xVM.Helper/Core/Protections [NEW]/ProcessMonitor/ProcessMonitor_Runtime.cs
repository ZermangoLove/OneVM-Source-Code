using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace xVM.Helper.Core.Protections
{
    internal static class ProcessMonitor_Runtime
    {
		static void StartPM()
		{
			var thread = new Thread(PMScan);
			thread.IsBackground = true;
			thread.Start(null);
		}

		static void PMScan(object thread)
		{
			var th = thread as Thread;
			if (th == null)
			{
				th = new Thread(PMScan);
				th.IsBackground = true;
				th.Start(Thread.CurrentThread);
				Thread.Sleep(500);
			}
			try
			{
				// Process Monitor
				string[] array = new string[]
				{
					"solarwinds", "paessler", "cpacket", "wireshark", "Ethereal", "sectools", "riverbed", "tcpdump", "Kismet", "EtherApe",
					"Fiddler", "telerik", "glasswire", "HTTPDebuggerSvc", "HTTPDebuggerUI", "charles", "intercepter", "snpa", "dumcap", "comview",
					"netcheat", "cheat", "winpcap", "megadumper", "MegaDumper", "dnspy", "ilspy", "reflector", "megadumper", "mega dumper",
					"extreme dumper", "extremedumper", "dnspyex", "process hacker", "processhacker", "de4dot", "fiddler", "charles", "wireshark",
					"burp", "cawkvm", "solarwinds", "paessler", "cpacket", "Ethereal", "telerik", "codecracker", "httpanalyzer", "httpdebug", "simpleassembly",
					"x64dbg", "x32dbg", "ida -", "netcheat", "intercepter", "hacker", "debugger", "dotpeek", "dbg", "ida", "dumper", "dump"
				};
				if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Length > 2)
				{
					Process.GetCurrentProcess().Kill();
				}
				for (; ; )
				{
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						if (Process.GetProcessesByName(array2[i]).Length != 0)
						{
							Environment.Exit(0);
						}
					}
					foreach (Process process in Process.GetProcesses())
					{
						foreach (string text in array)
						{
							bool flag = false;
							if (process.MainWindowTitle.ToLower().Contains(text) || process.ProcessName.ToLower().Contains(text) || process.MainWindowTitle.ToLower().Contains(text) || process.ProcessName.ToLower().Contains(text))
							{
								flag = true;
							}
							if (flag)
							{
								try
								{
									process.Kill();
									// Automatically delete files
									//string FileName = Assembly.GetExecutingAssembly().Location;
									//if (FileName == "" || FileName == null)
									//{
									//	FileName = Assembly.GetEntryAssembly().Location;
									//}
									//Process.Start(new ProcessStartInfo
									//{
									//	FileName = "cmd.exe",
									//	Arguments = "/C ping 1.1.1.1 -n 1 -w 4000 > Nul & Del \"" + FileName + "\"",
									//	CreateNoWindow = true,
									//	WindowStyle = ProcessWindowStyle.Hidden
									//});
									//Environment.Exit(0);
								}
								catch
								{
									//Environment.Exit(0);
								}
							}
						}
					}
					//Thread.Sleep(3000);
				}
			}
            catch
            {

            }
		}
    }
}
