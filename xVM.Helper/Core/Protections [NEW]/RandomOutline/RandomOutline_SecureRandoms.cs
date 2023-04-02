using System;
using System.Security.Cryptography;
using System.Text;

namespace xVM.Helper.Core.Protections
{
	public class SecureRandoms
	{
		private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();

		internal static readonly char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

		public static int Next(int minValue, int maxExclusiveValue)
		{
			if (minValue >= maxExclusiveValue)
			{
				throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");
			}
			long num = (long)maxExclusiveValue - (long)minValue;
			long num2 = 4294967295L / num * num;
			uint randomUInt;
			do
			{
				randomUInt = GetRandomUInt();
			}
			while (randomUInt >= num2);
			return (int)(minValue + (long)randomUInt % num);
		}

		public static uint GetRandomUInt()
		{
			byte[] value = GenerateRandomBytes(4);
			return BitConverter.ToUInt32(value, 0);
		}

		public static byte[] GenerateRandomBytes(int bytesNumber)
		{
			byte[] array = new byte[bytesNumber];
			csp.GetBytes(array);
			return array;
		}

		public static string GenerateRandomString(int size)
		{
			byte[] array = new byte[4 * size];
			RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			rNGCryptoServiceProvider.GetBytes(array);
			StringBuilder stringBuilder = new StringBuilder(size);
			for (int i = 0; i < size; i++)
			{
				uint num = BitConverter.ToUInt32(array, i * 4);
				long num2 = (long)num % (long)chars.Length;
				stringBuilder.Append(chars[num2]);
			}
			return stringBuilder.ToString();
		}
	}
}
