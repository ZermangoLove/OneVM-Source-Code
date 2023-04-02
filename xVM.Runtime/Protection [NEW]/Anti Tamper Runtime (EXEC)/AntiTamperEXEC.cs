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
    internal class AntiTamperEXEC
    {
        [VMProtect.BeginMutation]
        internal unsafe void Initialize()
        {
            try
            {
                var stream = new StreamReader(VMInstance.__ExecuteAssembly.Location).BaseStream;
                var reader = new BinaryReader(stream);
                var newMd5 = BitConverter.ToString(MD5.Create().ComputeHash(reader.ReadBytes((int)stream.Length - 16)));
                stream.Seek(-16, SeekOrigin.End);
                var realMd5 = BitConverter.ToString(reader.ReadBytes(16));

                if ((newMd5 == realMd5) && (Utils.AntiTamperChecker == true) && !(Utils.AntiTamperChecker == null) && !(Utils.AntiTamperChecker == false))
                {
                    NativeMethods.SwitchToThread();
                    string n = VMInstance.__ExecuteModule.FullyQualifiedName;
                    bool f = n.Length > 0 && n[0] == 0x3C;
                    var b = (byte*)Marshal.GetHINSTANCE(VMInstance.__ExecuteModule);
                    byte* p = b + *(uint*)(b + 0x3c);
                    ushort s = *(ushort*)(p + 0x6);
                    ushort o = *(ushort*)(p + 0x14);

                    uint* encLoc = null;
                    uint l = 0;
                    var r = (uint*)(p + 0x18 + o);

                    var Keys = new uint[5]
                    {
                        uint.Parse(VMProtect.SDK.DecryptString(Mutation.LdstrKey0)), uint.Parse(VMProtect.SDK.DecryptString(Mutation.LdstrKey1)),
                        uint.Parse(VMProtect.SDK.DecryptString(Mutation.LdstrKey2)), uint.Parse(VMProtect.SDK.DecryptString(Mutation.LdstrKey3)),
                        uint.Parse(VMProtect.SDK.DecryptString(Mutation.LdstrKey4))
                    };

                    var arraySeed = new byte[Keys.Length * 4];
                    int buffIndex = 0;
                    foreach (uint dat in Keys) //make bytes from uint.
                    {
                        arraySeed[buffIndex++] = (byte)((dat >> 0) & 0xff);
                        arraySeed[buffIndex++] = (byte)((dat >> 8) & 0xff);
                        arraySeed[buffIndex++] = (byte)((dat >> 16) & 0xff);
                        arraySeed[buffIndex++] = (byte)((dat >> 24) & 0xff);
                    }
                    Debug.Assert(buffIndex == arraySeed.Length);

                    uint EndKey = BitConverter.ToUInt32(MD5.Create().ComputeHash(arraySeed), 0);

                    for (int i = 0; i < s; i++)
                    {
                        uint g = (*r++) * (*r++);
                        if (g == Keys[0] && newMd5 == realMd5)
                        {
                            encLoc = (uint*)(b + (f ? *(r + 3) : *(r + 1)));
                            l = (f ? *(r + 2) : *(r + 0)) >> 2;
                        }
                        else if (g != 0)
                        {
                            if (newMd5 == realMd5)
                            {
                                var q = (uint*)(b + (f ? *(r + 3) : *(r + 1)));
                                uint j = *(r + 2) >> 2;
                                for (uint k = 0; k < j; k++)
                                {
                                    uint t = (Keys[1] ^ (*q++)) + Keys[2] + Keys[3] * Keys[4];
                                    Keys[1] = Keys[2];
                                    Keys[2] = Keys[3];
                                    Keys[2] = Keys[4];
                                    Keys[4] = t;
                                }
                            }
                        }
                        r += 8;
                    }

                    uint[] y = new uint[0x10], d = new uint[0x10];
                    for (int i = 0; i < 0x10; i++)
                    {
                        y[i] = Keys[4];
                        d[i] = Keys[2];
                        Keys[1] = (Keys[2] >> 5) | (Keys[2] << 27);
                        Keys[2] = (Keys[3] >> 3) | (Keys[3] << 29);
                        Keys[3] = (Keys[4] >> 7) | (Keys[4] << 25);
                        Keys[4] = (Keys[1] >> 11) | (Keys[1] << 21);
                    }
                    Mutation.Crypt(y, d);

                    uint w = 0x40;
                    NativeMethods.VirtualProtect((void*)encLoc, l << 2, w, out w);

                    if (w == 0x40)
                        return;

                    uint h = 0;
                    for (uint i = 0; i < l; i++)
                    {
                        *encLoc ^= y[h & 0xf];
                        y[h & 0xf] = (y[h & 0xf] ^ (*encLoc++)) + EndKey;
                        h++;
                    }

                    if (newMd5 == realMd5)
                    {
                        Utils.AntiTamperChecker = null;
                    }
                }
                else
                {
                    Utils.AntiTamperChecker = false;
                    throw new BadImageFormatException();
                }
            }
            catch (Exception except)
            {
                Utils.AntiTamperChecker = false;
                NativeMethods.MessageBox(except.ToString(), except.GetType().Name, NativeMethods.MessageBoxButtons.OK, NativeMethods.MessageBoxIcon.Error);
                return;
            }
        }
    }
}