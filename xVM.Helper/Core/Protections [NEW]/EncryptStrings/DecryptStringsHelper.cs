using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    internal static class DecryptStringsHelper
    {
        public static string DecryptString(string encodedString, int key)
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (char c in encodedString)
                {
                    stringBuilder.Append((char)((int)c ^ key));
                }
                return stringBuilder.ToString();
            }
            return null;
        }
    }
}
