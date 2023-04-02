using System;
using System.Linq;

namespace xVM.Helper.Core.Protections
{
    public static class JunkGen
    {
        internal static Random rdm = new Random();
        internal static string RandomString(int length, int type)
        {
            if(type == 0)
                return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length).Select(s => s[rdm.Next(s.Length)]).ToArray());
            else if(type == 1)
                return new string(Enumerable.Repeat("!@#$%^&*()_+-=[]{}|;':,./<>?`~", length).Select(s => s[rdm.Next(s.Length)]).ToArray());

            return string.Empty;
        }
    }
}