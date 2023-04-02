using System;

internal class Mutation
{
    public static readonly int IntKey0 = 0;
    public static readonly int IntKey1 = 1;
    public static readonly int IntKey2 = 2;
    public static readonly int IntKey3 = 3;
    public static readonly int IntKey4 = 4;
    public static readonly int IntKey5 = 5;
    public static readonly int IntKey6 = 6;
    public static readonly int IntKey7 = 7;
    public static readonly int IntKey8 = 8;
    public static readonly int IntKey9 = 9;
    public static readonly int IntKey10 = 10;
    public static readonly int IntKey11 = 11;
    public static readonly int IntKey12 = 12;
    public static readonly int IntKey13 = 13;
    public static readonly int IntKey14 = 14;
    public static readonly int IntKey15 = 15;
    public static readonly int IntKey16 = 16;
    public static readonly int IntKey17 = 17;
    public static readonly int IntKey18 = 18;
    public static readonly int IntKey19 = 19;
    public static readonly int IntKey20 = 20;

    public static readonly long LongKey0 = 0;
	public static readonly long LongKey1 = 1;
	public static readonly long LongKey2 = 2;
	public static readonly long LongKey3 = 3;
	public static readonly long LongKey4 = 4;
	public static readonly long LongKey5 = 5;
	public static readonly long LongKey6 = 6;
	public static readonly long LongKey7 = 7;
	public static readonly long LongKey8 = 8;
	public static readonly long LongKey9 = 9;
	public static readonly long LongKey10 = 10;
	public static readonly long LongKey11 = 11;
	public static readonly long LongKey12 = 12;
	public static readonly long LongKey13 = 13;
	public static readonly long LongKey14 = 14;
	public static readonly long LongKey15 = 15;
    public static readonly long LongKey16 = 16;
    public static readonly long LongKey17 = 17;
    public static readonly long LongKey18 = 18;
    public static readonly long LongKey19 = 19;
    public static readonly long LongKey20 = 20;

    public static readonly string LdstrKey0 = Convert.ToString(0);
    public static readonly string LdstrKey1 = Convert.ToString(1);
    public static readonly string LdstrKey2 = Convert.ToString(2);
    public static readonly string LdstrKey3 = Convert.ToString(3);
    public static readonly string LdstrKey4 = Convert.ToString(4);
    public static readonly string LdstrKey5 = Convert.ToString(5);
    public static readonly string LdstrKey6 = Convert.ToString(6);
    public static readonly string LdstrKey7 = Convert.ToString(7);
    public static readonly string LdstrKey8 = Convert.ToString(8);
    public static readonly string LdstrKey9 = Convert.ToString(9);
    public static readonly string LdstrKey10 = Convert.ToString(10);

    public static T Placeholder<T>(T val) {
		return val;
	}

	public static T Value<T>() {
		return default(T);
	}

	public static T Value<T, Arg0>(Arg0 arg0) {
		return default(T);
	}

	public static void Crypt(uint[] data, uint[] key) { }
}