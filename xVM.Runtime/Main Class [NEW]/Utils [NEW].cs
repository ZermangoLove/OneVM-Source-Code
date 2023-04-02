using System;
using System.IO;
using System.Text;
using System.Reflection;

using xVM.Runtime.Dynamic;

namespace xVM.Runtime
{
    internal static unsafe class Utils
    {
        public static bool? AntiTamperChecker
        {
            get;
            set;
        } = true;

        [VMProtect.BeginMutation]
        public static ulong ReadCompressedULong(ref byte* ptr)
        {
            try
            {
                if (Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) && Utils.AntiTamperChecker == null)
                {
                    ulong value = 0;
                    ulong shift = 0;
                    do
                    {
                        value |= (ulong)(*ptr & 0x7f) << (int)shift;
                        shift += 7;
                    } while ((*ptr++ & 0x80) != 0);

                    var val = (value ^ ((ulong)int.Parse(Mutation.LdstrKey0) + ((ulong)int.Parse(Mutation.LdstrKey1) << 32)));
                    return val;
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return 0;
            }
            return 0;
        }

        [VMProtect.BeginMutation]
        public static ulong FromCodedToken(ulong codedToken)
        {
            try
            {
                if (Equals(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly()) && Utils.AntiTamperChecker == null)
                {
                    var rid = codedToken >> 3;
                    switch (codedToken & 7)
                    {
                        case 1:
                            return rid | 0x02000000;
                        case 2:
                            return rid | 0x01000000;
                        case 3:
                            return rid | 0x1b000000;
                        case 4:
                            return rid | 0x0a000000;
                        case 5:
                            return rid | 0x06000000;
                        case 6:
                            return rid | 0x04000000;
                        case 7:
                            return rid | 0x2b000000;
                    }
                    return rid;
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return 0;
            }
            return 0;
        }

        [VMProtect.BeginMutation]
        public static void UpdateFL(ulong op1, ulong op2, ulong flResult, ulong result, ref byte fl, byte mask)
        {
            try
            {
                const ulong SignMask = 1U << 63;
                byte flag = 0;
                if (result == 0)
                    flag |= Constants.OPCODELIST[34];
                if ((result & SignMask) != 0)
                    flag |= Constants.OPCODELIST[36];
                if (((op1 ^ flResult) & (op2 ^ flResult) & SignMask) != 0)
                    flag |= Constants.OPCODELIST[30];
                if (((op1 ^ ((op1 ^ op2) & (op2 ^ flResult))) & SignMask) != 0)
                    flag |= Constants.OPCODELIST[32];
                fl = (byte)((fl & ~mask) | (flag & mask));
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }            
        }
    }
}