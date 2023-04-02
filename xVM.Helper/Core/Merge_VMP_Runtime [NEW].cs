using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace xVM.Helper
{
	internal static unsafe class Merge_VMP_Runtime
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

		[DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

		private static string y;

		static void _ctor()
		{
			Start();
			if (File.Exists(y))
			{
				Thread.Sleep(100);
			}
		}

		private static void Start()
		{
			var wow64Value = IntPtr.Zero;
			Wow64DisableWow64FsRedirection(ref wow64Value);
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			var module = typeof(Merge_VMP_Runtime).Module;
			if (Equals((Assembly)typeof(Assembly).GetMethod("GetExecutingAssembly").Invoke(null, null), Assembly.GetExecutingAssembly()))
			{
				var sb = new StringBuilder();
				var bytes = Encoding.Unicode.GetBytes(Encoding.BigEndianUnicode.GetString(SHA1.Create().ComputeHash(BitConverter.GetBytes(MutationCore.IntKey0))));
				foreach (var t in bytes)
				{
					sb.Append(t.ToString("X2"));
				}

				ulong len = 0;
				void* ptrdata = null;
				if (module.FullyQualifiedName.Length > 0 && module.FullyQualifiedName[0] == '<')
					ptrdata = GetKoiStreamFlat(out len, sb.ToString().Substring(0, 8));
				else
					ptrdata = GetKoiStreamMapped(out len, sb.ToString().Substring(0, 8));

				y = Path.Combine(Path.GetDirectoryName(module.Assembly.Location), MutationCore.LdstrKey0);
				using (var ustream = new UnmanagedMemoryStream((byte*)ptrdata, (long)len, (long)len, FileAccess.Read))
				using (var reader = new BinaryReader(ustream))
				{
					byte[] buff;
					var decompressedMs = new MemoryStream();
					using (var gzs = new GZipStream(new MemoryStream(reader.ReadBytes((int)ustream.Length)), CompressionMode.Decompress))
					{
						int bufSize = 1024, count;
						buff = new byte[bufSize];
						count = gzs.Read(buff, 0, bufSize);
						while (count > 0)
						{
							decompressedMs.Write(buff, 0, count);
							count = gzs.Read(buff, 0, bufSize);
						}
					}
					buff = decompressedMs.ToArray();

					if (!File.Exists(y))
						File.WriteAllBytes(y, buff);
					ustream.Flush();
					ustream.Close();
				}

				File.SetAttributes(y, FileAttributes.Hidden | FileAttributes.System);

                #region Terminate Detection
                ////////////////////////////////////////////////////////////////////////////////////
                Console.CancelKeyPress += ConsoleExitHandler;

				var process = Process.GetCurrentProcess();
				process.EnableRaisingEvents = true;
				process.Exited += new EventHandler(ExitHandler);

				AppDomain.CurrentDomain.ProcessExit += ProcessExit;
				////////////////////////////////////////////////////////////////////////////////////
				#endregion
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			Wow64RevertWow64FsRedirection(wow64Value);
		}

		private static bool closed_this = false;

		private static void ProcessExit(object A_0, EventArgs A_1)
		{
			try
			{
				IntPtr wow64Value = IntPtr.Zero;
				Wow64RevertWow64FsRedirection(wow64Value);

				if (!closed_this)
                {
					closed_this = true;

					ShellExecute(IntPtr.Zero, null, "cmd.exe", "/C choice /C Y /N /D Y /T 3 & Del \"" + y + "\" /A:H", null, 0);
				}
			}
			catch { }
		}

		private static void ExitHandler(object o, EventArgs args)
		{
			try
			{
				IntPtr wow64Value = IntPtr.Zero;
				Wow64RevertWow64FsRedirection(wow64Value);

				if (!closed_this)
				{
					closed_this = true;

					ShellExecute(IntPtr.Zero, null, "cmd.exe", "/C choice /C Y /N /D Y /T 3 & Del \"" + y + "\" /A:H", null, 0);
				}
			}
			catch { }
		}

		private static void ConsoleExitHandler(object sender, ConsoleCancelEventArgs e)
		{
			try
			{
				IntPtr wow64Value = IntPtr.Zero;
				Wow64RevertWow64FsRedirection(wow64Value);

				if (!closed_this)
				{
					closed_this = true;

					ShellExecute(IntPtr.Zero, null, "cmd.exe", "/C choice /C Y /N /D Y /T 3 & Del \"" + y + "\" /A:H", null, 0);
				}
			}
			catch { }
		}

		private static void* GetKoiStreamMapped(out ulong len, string storageName)
		{
			byte* moduleBase = (byte*)Marshal.GetHINSTANCE(typeof(Merge_VMP_Runtime).Module);
			byte* ptr = moduleBase + 0x3c;
			byte* ptr2;
			ptr = ptr2 = moduleBase + *(uint*)ptr;
			ptr += 0x6;
			ushort sectNum = *(ushort*)ptr;
			ptr += 14;
			ushort optSize = *(ushort*)ptr;
			ptr = ptr2 = ptr + 0x4 + optSize;

			byte* mdDir = moduleBase + *(uint*)(ptr - 16);
			byte* mdHdr = moduleBase + *(uint*)(mdDir + 8);
			mdHdr += 12;
			mdHdr += *(uint*)mdHdr;
			mdHdr = (byte*)(((ulong)mdHdr + 7) & ~3UL);
			mdHdr += 2;
			ushort numOfStream = *mdHdr;
			mdHdr += 2;
			var streamName = new System.Text.StringBuilder();
			for (int i = 0; i < numOfStream; i++)
			{
				uint offset = *(uint*)mdHdr;
				len = *(uint*)(mdHdr + 4);
				mdHdr += 8;
				streamName.Length = 0;
				for (int ii = 0; ii < 8; ii++)
				{
					streamName.Append((char)*mdHdr++);
					if (*mdHdr == 0)
					{
						mdHdr += 3;
						break;
					}
					streamName.Append((char)*mdHdr++);
					if (*mdHdr == 0)
					{
						mdHdr += 2;
						break;
					}
					streamName.Append((char)*mdHdr++);
					if (*mdHdr == 0)
					{
						mdHdr += 1;
						break;
					}
					streamName.Append((char)*mdHdr++);
					if (*mdHdr == 0)
					{
						break;
					}
				}
				if (streamName.ToString() == storageName)
				{
					var koi = (void*)Marshal.AllocHGlobal((int)len);
					CopyMemory(koi, moduleBase + *(uint*)(mdDir + 8) + offset, len);
					return koi;
				}
			}
			len = 0;
			return null;
		}

		private static void* GetKoiStreamFlat(out ulong len, string storageName)
		{
			byte* moduleBase = (byte*)Marshal.GetHINSTANCE(typeof(Merge_VMP_Runtime).Module);
			byte* ptr = moduleBase + 0x3c;
			byte* ptr2;
			ptr = ptr2 = moduleBase + *(uint*)ptr;
			ptr += 0x6;
			ushort sectNum = *(ushort*)ptr;
			ptr += 14;
			ushort optSize = *(ushort*)ptr;
			ptr = ptr2 = ptr + 0x4 + optSize;

			uint mdDir = *(uint*)(ptr - 16);

			var vAdrs = new uint[sectNum];
			var vSizes = new uint[sectNum];
			var rAdrs = new uint[sectNum];
			for (int i = 0; i < sectNum; i++)
			{
				vAdrs[i] = *(uint*)(ptr + 12);
				vSizes[i] = *(uint*)(ptr + 8);
				rAdrs[i] = *(uint*)(ptr + 20);
				ptr += 0x28;
			}

			for (int i = 0; i < sectNum; i++)
				if (vAdrs[i] <= mdDir && mdDir < vAdrs[i] + vSizes[i])
				{
					mdDir = mdDir - vAdrs[i] + rAdrs[i];
					break;
				}
			byte* mdDirPtr = moduleBase + mdDir;
			uint mdHdr = *(uint*)(mdDirPtr + 8);
			for (int i = 0; i < sectNum; i++)
				if (vAdrs[i] <= mdHdr && mdHdr < vAdrs[i] + vSizes[i])
				{
					mdHdr = mdHdr - vAdrs[i] + rAdrs[i];
					break;
				}


			byte* mdHdrPtr = moduleBase + mdHdr;
			mdHdrPtr += 12;
			mdHdrPtr += *(uint*)mdHdrPtr;
			mdHdrPtr = (byte*)(((ulong)mdHdrPtr + 7) & ~3UL);
			mdHdrPtr += 2;
			ushort numOfStream = *mdHdrPtr;
			mdHdrPtr += 2;
			var streamName = new System.Text.StringBuilder();
			for (int i = 0; i < numOfStream; i++)
			{
				uint offset = *(uint*)mdHdrPtr;
				len = *(uint*)(mdHdrPtr + 4);
				streamName.Length = 0;
				mdHdrPtr += 8;
				for (int ii = 0; ii < 8; ii++)
				{
					streamName.Append((char)*mdHdrPtr++);
					if (*mdHdrPtr == 0)
					{
						mdHdrPtr += 3;
						break;
					}
					streamName.Append((char)*mdHdrPtr++);
					if (*mdHdrPtr == 0)
					{
						mdHdrPtr += 2;
						break;
					}
					streamName.Append((char)*mdHdrPtr++);
					if (*mdHdrPtr == 0)
					{
						mdHdrPtr += 1;
						break;
					}
					streamName.Append((char)*mdHdrPtr++);
					if (*mdHdrPtr == 0)
					{
						break;
					}
				}
				if (streamName.ToString() == storageName)
				{
					var koi = (void*)Marshal.AllocHGlobal((int)len);
					CopyMemory(koi, moduleBase + mdHdr + offset, len);
					return koi;
				}
			}
			len = 0;
			return null;
		}

		private static void CopyMemory(void* dest, void* src, ulong count)
		{
			ulong block;

			block = count >> 3;

			long* pDest = (long*)dest;
			long* pSrc = (long*)src;

			for (ulong i = 0; i < block; i++)
			{
				*pDest = *pSrc; pDest++; pSrc++;
			}
			dest = pDest;
			src = pSrc;
			count = count - (block << 3);

			if (count > 0)
			{
				byte* pDestB = (byte*)dest;
				byte* pSrcB = (byte*)src;
				for (ulong i = 0; i < count; i++)
				{
					*pDestB = *pSrcB; pDestB++; pSrcB++;
				}
			}
		}
	}
}
