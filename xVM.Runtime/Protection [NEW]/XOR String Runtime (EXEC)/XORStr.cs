using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using xVM.Runtime.Services;

namespace xVM.Runtime.Protection
{
    public static class XORStr
    {
        [VMProtect.BeginUltra]
		public static string Decrypt(string text, int key)
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