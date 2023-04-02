using System;
using System.Security.Cryptography;
using System.Text;

namespace xVM.Helper.Core.Protections.Generator
{
    public class Randomizer
    {
        private static readonly RandomNumberGenerator csp = RandomNumberGenerator.Create();

        private static readonly char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        public static int Next(int maxValue, int minValue = 0)
        {
            if (minValue >= maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
            long diff = (long)maxValue - minValue;
            long upperBound = uint.MaxValue / diff * diff;
            uint ui;
            do { ui = RandomUInt(); } while (ui >= upperBound);
            return (int)(minValue + (ui % diff));
        }

        public static string GenerateRandomString(int size)
        {
            StringBuilder stringy = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var renamer = "_" + Next(100000, 10000);
                stringy.Append(renamer);
            }
            return stringy.ToString();
        }

        private static uint RandomUInt()
        {
            return BitConverter.ToUInt32(RandomBytes(sizeof(uint)), 0);
        }

        private static byte[] RandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            csp.GetNonZeroBytes(buffer);
            return buffer;
        }
    }
}
