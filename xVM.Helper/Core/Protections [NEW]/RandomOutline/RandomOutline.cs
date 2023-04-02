using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    public class RandomOutline
    {
        public static int thelength = 15; // Default

        public int count = 5;

        Random random = new Random();

        public static string RandomOutlineMode = "Ascii"; // Default
        
        public void Execute(ModuleDef module)
        {
            foreach (TypeDef typeDef in module.Types)
            {
                foreach (MethodDef methodDef in typeDef.Methods.ToArray<MethodDef>())
                {
                    if (RandomOutlineMode.Contains("Ascii"))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            MethodDef methodDef2 = CreateReturnMethodDef(GenerateRandomString(thelength), methodDef);
                            MethodDef methodDef3 = CreateReturnMethodDef(SecureRandoms.Next(10, 20), methodDef);
                            typeDef.Methods.Add(methodDef2);
                            typeDef.Methods.Add(methodDef3);
                        }
                    }
                    if (RandomOutlineMode.Contains("Numbers"))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            MethodDef methodDef4 = CreateReturnMethodDef(RandomNum(thelength), methodDef);
                            MethodDef methodDef5 = CreateReturnMethodDef(SecureRandoms.Next(5, 20), methodDef);
                            typeDef.Methods.Add(methodDef4);
                            typeDef.Methods.Add(methodDef5);
                        }
                    }
                    if (RandomOutlineMode.Contains("Symbols"))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            MethodDef methodDef6 = CreateReturnMethodDef(RandomSymbols(thelength), methodDef);
                            MethodDef methodDef7 = CreateReturnMethodDef(SecureRandoms.Next(5, 20), methodDef);
                            typeDef.Methods.Add(methodDef6);
                            typeDef.Methods.Add(methodDef7);
                        }
                    }
                    if (RandomOutlineMode.Contains("Hex"))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            MethodDef methodDef8 = CreateReturnMethodDef(RandomHex(thelength), methodDef);
                            MethodDef methodDef9 = CreateReturnMethodDef(SecureRandoms.Next(5, 20), methodDef);
                            typeDef.Methods.Add(methodDef8);
                            typeDef.Methods.Add(methodDef9);
                        }
                    }
                }    
            }    
        }

        public MethodDef CreateReturnMethodDef(object value, MethodDef source_method)
        {
            CorLibTypeSig corLibTypeSig = null;
            if (value is int)
            {
                corLibTypeSig = source_method.Module.CorLibTypes.Int32;
            }
            else if (value is string)
            {
                corLibTypeSig = source_method.Module.CorLibTypes.String;
            }
            MethodDef methodDef = new MethodDefUser(GenerateRandomString(thelength), MethodSig.CreateStatic(corLibTypeSig), MethodImplAttributes.IL, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.HideBySig)
            {
                Body = new CilBody()
            };
            if (value is int)
            {
                methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, (int)value));
            }
            else if (value is string)
            {
                methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, (string)value));
            }
            methodDef.Body.Instructions.Add(new Instruction(OpCodes.Ret));
            return methodDef;
        }

        public string GenerateRandomString(int size)
        {
            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] array = text.ToCharArray();
            byte[] array2 = new byte[1];
            RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
            rngcryptoServiceProvider.GetNonZeroBytes(array2);
            array2 = new byte[size];
            rngcryptoServiceProvider.GetNonZeroBytes(array2);
            StringBuilder stringBuilder = new StringBuilder(size);
            foreach (byte b in array2)
            {
                stringBuilder.Append(array[(int)b % array.Length]);
            }
            return stringBuilder.ToString();
        }

        public string RandomNum(int length)
        {
            return new string((from s in Enumerable.Repeat<string>("0123456789", length) select s[random.Next(s.Length)]).ToArray<char>());
        }

        public string RandomSymbols(int length)
        {
            return new string((from s in Enumerable.Repeat<string>("!@#$%^&*()_+-=[]{}|;':,./<>?`~", length) select s[random.Next(s.Length)]).ToArray<char>());
        }
        
        public string RandomHex(int length)
        {
            string res = "";
            for (int i = 0; i < length; i++)
            {
                if (random.Next(4) == 4)
                {
                    // Text + Num
                    res = res + RandomUpperString4Hex(1);
                    res = res + RandomNum4Hex(10);
                }
                else if (random.Next(4) == 3)
                {
                    // Num + Text
                    res = res + RandomNum4Hex(10);
                    res = res + RandomUpperString4Hex(1);
                }
                else if (random.Next(4) == 2)
                {
                    // Num + Num
                    res = res + RandomNum4Hex(10);
                    res = res + RandomNum4Hex(10);
                }
                else
                {
                    // Text + Text
                    res = res + RandomUpperString4Hex(1);
                    res = res + RandomUpperString4Hex(1);
                }
                if (length != 1)
                {
                    res = res + " ";
                }
            }
            return res.TrimEnd();
        }
        
        public string RandomNum4Hex(int max) // Return int < max | For example if max = 10, return int < 10 (Random)
        {
            return random.Next(max).ToString();
        }

        public string RandomUpperString4Hex(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
