using System;

namespace StardewValley
{
	// Token: 0x02000029 RID: 41
	public class Noise
	{
		// Token: 0x06000230 RID: 560 RVA: 0x00032A4C File Offset: 0x00030C4C
		public static float Generate(float x)
		{
			int i0 = Noise.FastFloor(x);
			int i = i0 + 1;
			float x2 = x - (float)i0;
			float x3 = x2 - 1f;
			float expr_21 = 1f - x2 * x2;
			float expr_23 = expr_21 * expr_21;
			float n0 = expr_23 * expr_23 * Noise.grad((int)Noise.perm[i0 & 255], x2);
			float expr_44 = 1f - x3 * x3;
			float expr_46 = expr_44 * expr_44;
			float n = expr_46 * expr_46 * Noise.grad((int)Noise.perm[i & 255], x3);
			return 0.395f * (n0 + n);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00032AC4 File Offset: 0x00030CC4
		public static float Generate(float x, float y)
		{
			float s = (x + y) * 0.3660254f;
			float arg_12_0 = x + s;
			float ys = y + s;
			int arg_20_0 = Noise.FastFloor(arg_12_0);
			int i = Noise.FastFloor(ys);
			float t = (float)(arg_20_0 + i) * 0.211324871f;
			float X0 = (float)arg_20_0 - t;
			float Y0 = (float)i - t;
			float x2 = x - X0;
			float y2 = y - Y0;
			int i2;
			int j;
			if (x2 > y2)
			{
				i2 = 1;
				j = 0;
			}
			else
			{
				i2 = 0;
				j = 1;
			}
			float x3 = x2 - (float)i2 + 0.211324871f;
			float y3 = y2 - (float)j + 0.211324871f;
			float x4 = x2 - 1f + 0.422649741f;
			float y4 = y2 - 1f + 0.422649741f;
			int ii = arg_20_0 % 256;
			int jj = i % 256;
			float t2 = 0.5f - x2 * x2 - y2 * y2;
			float n0;
			if (t2 < 0f)
			{
				n0 = 0f;
			}
			else
			{
				t2 *= t2;
				n0 = t2 * t2 * Noise.grad((int)Noise.perm[ii + (int)Noise.perm[jj]], x2, y2);
			}
			float t3 = 0.5f - x3 * x3 - y3 * y3;
			float n;
			if (t3 < 0f)
			{
				n = 0f;
			}
			else
			{
				t3 *= t3;
				n = t3 * t3 * Noise.grad((int)Noise.perm[ii + i2 + (int)Noise.perm[jj + j]], x3, y3);
			}
			float t4 = 0.5f - x4 * x4 - y4 * y4;
			float n2;
			if (t4 < 0f)
			{
				n2 = 0f;
			}
			else
			{
				t4 *= t4;
				n2 = t4 * t4 * Noise.grad((int)Noise.perm[ii + 1 + (int)Noise.perm[jj + 1]], x4, y4);
			}
			return 40f * (n0 + n + n2);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00032C74 File Offset: 0x00030E74
		private static int FastFloor(float x)
		{
			if (x <= 0f)
			{
				return (int)x - 1;
			}
			return (int)x;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00032C88 File Offset: 0x00030E88
		private static float grad(int hash, float x)
		{
			int h = hash & 15;
			float grad = 1f + (float)(h & 7);
			if ((h & 8) != 0)
			{
				grad = -grad;
			}
			return grad * x;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00032CB0 File Offset: 0x00030EB0
		private static float grad(int hash, float x, float y)
		{
			int h = hash & 7;
			float u = (h < 4) ? x : y;
			float v = (h < 4) ? y : x;
			return (((h & 1) != 0) ? (-u) : u) + (((h & 2) != 0) ? (-2f * v) : (2f * v));
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00032CF4 File Offset: 0x00030EF4
		private static float grad(int hash, float x, float y, float z)
		{
			int h = hash & 15;
			float u = (h < 8) ? x : y;
			float v = (h < 4) ? y : ((h == 12 || h == 14) ? x : z);
			return (((h & 1) != 0) ? (-u) : u) + (((h & 2) != 0) ? (-v) : v);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00032D3C File Offset: 0x00030F3C
		private static float grad(int hash, float x, float y, float z, float t)
		{
			int h = hash & 31;
			float u = (h < 24) ? x : y;
			float v = (h < 16) ? y : z;
			float w = (h < 8) ? z : t;
			return (((h & 1) != 0) ? (-u) : u) + (((h & 2) != 0) ? (-v) : v) + (((h & 4) != 0) ? (-w) : w);
		}

		// Token: 0x04000271 RID: 625
		private static byte[] perm = new byte[]
		{
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			255,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180,
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			255,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180
		};
	}
}
