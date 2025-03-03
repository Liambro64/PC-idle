using System;
using System.Collections.Generic;
using Unity.Mathematics;

public class suffix
{
	public static string[] f12 = {"", "k",
									   "m", "b", "t", "q", "Q", "s", "S", "o", "n", "d"};
	public static string[] b10 = { "u", "d", "t", "q", "Q", "s", "S", "o", "n", "d" };
	public static string[] b1010 = { "d", "D", "t", "q", "Q", "s", "S", "o", "n", "d" };
	public static string getSuffix(long base1000)
	{
		if (base1000 < 12)
			return f12[base1000];
		base1000 -= 2;
		long logBase1000 = (long)math.log10(base1000);
		string s = b10[base1000 % 10];
		for (long j = 0; j < logBase1000 - 1; j++)
			s += b1010[base1000 / (long)Math.Pow(10, j) % 10];
		return s;
	}
}