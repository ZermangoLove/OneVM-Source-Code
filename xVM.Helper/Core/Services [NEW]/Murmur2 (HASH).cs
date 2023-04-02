using System;
using System.Text;
using System.Collections.Generic;

namespace xVM.Helper.Core.Services
{
    internal class Murmur2
    {
        public readonly static Int32 Key = new RandomGenerator().NextByte() * new RandomGenerator().NextByte();

        public static UInt64 Hash(Byte[] data, UInt64 length, UInt64 seed)
        {
            UInt64 m = 0xc6a4a7935bd1e995L;
            UInt64 r = 47;
            UInt64 h = ((UInt64)Key & seed & 0xffffffffL) ^ (length * m);
            UInt64 length8 = length / 8;

            for (UInt32 i = 0; i < length8; i++)
            {
                UInt32 i8 = i * 8;
                UInt64 k = ((UInt64)data[i8 + 0] & 0xff) + (((UInt64)data[i8 + 1] & 0xff) << 8)
                    + (((UInt64)data[i8 + 2] & 0xff) << 16) + (((UInt64)data[i8 + 3] & 0xff) << 24)
                    + (((UInt64)data[i8 + 4] & 0xff) << 32) + (((UInt64)data[i8 + 5] & 0xff) << 40)
                    + (((UInt64)data[i8 + 6] & 0xff) << 48) + (((UInt64)data[i8 + 7] & 0xff) << 56);

                k *= m;
                k ^= k >> (Int16)r;
                k *= m;

                h ^= k;
                h *= m;
            }

            switch (length % 8)
            {
                case 7:
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 6] & 0xff) << 48;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 5] & 0xff) << 40;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 4] & 0xff) << 32;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 3] & 0xff) << 24;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 2] & 0xff) << 16;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 1] & 0xff) << 8;
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
                case 6:
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 5] & 0xff) << 40;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 4] & 0xff) << 32;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 3] & 0xff) << 24;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 2] & 0xff) << 16;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 1] & 0xff) << 8;
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
                case 5:
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 4] & 0xff) << 32;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 3] & 0xff) << 24;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 2] & 0xff) << 16;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 1] & 0xff) << 8;
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
                case 4:
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 3] & 0xff) << 24;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 2] & 0xff) << 16;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 1] & 0xff) << 8;
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
                case 3:
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 2] & 0xff) << 16;
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 1] & 0xff) << 8;
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
                case 2:
                    h ^= (UInt64)(data[(length & ~(UInt64)7) + 1] & 0xff) << 8;
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
                case 1:
                    h ^= (UInt64)(data[length & ~(UInt64)7] & 0xff);
                    h *= m;
                    break;
            };

            h ^= h >> (Int16)r;
            h *= m;
            h ^= h >> (Int16)r;

            return h;
        }
    }
}

