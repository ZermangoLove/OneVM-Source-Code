using System;
using System.IO;
using System.Text;

using xVM.Runtime.Protection;

using static Lzma;

namespace xVM.Runtime
{
    public class IDAttribute : Attribute
    {
        internal ulong ID { get; set; }

        [VMProtect.BeginMutation]
        public IDAttribute(string value, string key)
        {
            try
            {
                if (AntiDumpV2.AntiDumpIsRunning == true && Utils.AntiTamperChecker == null)
                {
                    int NumberChars = value.Length;
                    byte[] input = new byte[NumberChars / 2];
                    for (int i = 0; i < NumberChars; i += 2)
                        input[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);

                    byte[] Out = new byte[input.Length - 1];
                    int x = input[input.Length - 1];
                    for (int i = 0; i <= Out.Length - 1; i++)
                        Out[i] = (byte)(input[i] ^ (key[i % key.Length] + x) & 255);

                    ID = ulong.Parse(Encoding.Default.GetString(Out));
                }
            }
            catch (Exception except)
            {
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }
    }
}
