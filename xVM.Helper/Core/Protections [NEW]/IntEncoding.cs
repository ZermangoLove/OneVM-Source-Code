using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Security.Cryptography;
using xVM.Helper.Core.Protections.Generator;

namespace xVM.Helper.Core.Protections
{
    public class IntEncoding
    {
        private static int Amount { get; set; }

        public static void Execute(ModuleDef moduleDef)
        {
            IMethod absMethod = moduleDef.Import(typeof(Math).GetMethod("Abs", new Type[] { typeof(int) }));
            IMethod minMethod = moduleDef.Import(typeof(Math).GetMethod("Min", new Type[] { typeof(int), typeof(int) }));

            foreach (TypeDef type in moduleDef.Types)
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                        if (method.Body.Instructions[i] != null && method.Body.Instructions[i].IsLdcI4())
                        {
                            int operand = method.Body.Instructions[i].GetLdcI4Value();
                            if (operand <= 0) // Prevents errors.
                                continue;

                            // The Absolute method.
                            method.Body.Instructions.Insert(i + 1, OpCodes.Call.ToInstruction(absMethod));

                            int neg = Randomizer.Next(StringLength(), 8);
                            if (neg % 2 != 0)
                                neg += 1;

                            for (var j = 0; j < neg; j++)
                                method.Body.Instructions.Insert(i + j + 1, Instruction.Create(OpCodes.Neg));

                            if (operand < int.MaxValue)
                            {
                                method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(int.MaxValue));
                                method.Body.Instructions.Insert(i + 2, OpCodes.Call.ToInstruction(minMethod));
                            }
                            ++Amount;
                        }
                }
        }

        private static readonly RandomNumberGenerator csp = RandomNumberGenerator.Create();

        public static int Next(int maxValue, int minValue = 0)
        {
            if (minValue >= maxValue)
                throw new ArgumentOutOfRangeException(nameof(minValue));

            long diff = (long)maxValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;
            uint ui;
            do { ui = RandomUInt(); } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        public static int Next()
        {
            return BitConverter.ToInt32(RandomBytes(sizeof(int)), 0);
        }

        private static uint RandomUInt()
        {
            return BitConverter.ToUInt32(RandomBytes(sizeof(uint)), 0);
        }

        private static byte[] RandomBytes(int bytes)
        {
            byte[] buffer = new byte[bytes];
            csp.GetBytes(buffer);
            return buffer;
        }

        public static int StringLength()
        {
            return Next(120, 30);
        }
    }
}
