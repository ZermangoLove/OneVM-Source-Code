using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace xVM.Helper.Core.Protections
{
	public static class XORStr_Runtime
	{
		public static string EncryptOrDecrypt(string text, int key)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				stringBuilder.Append((char)((ulong)text[i] ^ ((ulong)i % (ulong)((long)key))));
			}
			return stringBuilder.ToString();
		}
	}
}
