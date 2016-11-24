using System;

namespace StardewValley
{
	// Token: 0x0200002A RID: 42
	internal static class NoiseGenerator
	{
		// Token: 0x1700001F RID: 31
		public static int Seed
		{
			// Token: 0x06000239 RID: 569 RVA: 0x00032DA8 File Offset: 0x00030FA8
			get;
			// Token: 0x0600023A RID: 570 RVA: 0x00032DAF File Offset: 0x00030FAF
			set;
		}

		// Token: 0x17000020 RID: 32
		public static int Octaves
		{
			// Token: 0x0600023B RID: 571 RVA: 0x00032DB7 File Offset: 0x00030FB7
			get;
			// Token: 0x0600023C RID: 572 RVA: 0x00032DBE File Offset: 0x00030FBE
			set;
		}

		// Token: 0x17000021 RID: 33
		public static double Amplitude
		{
			// Token: 0x0600023D RID: 573 RVA: 0x00032DC6 File Offset: 0x00030FC6
			get;
			// Token: 0x0600023E RID: 574 RVA: 0x00032DCD File Offset: 0x00030FCD
			set;
		}

		// Token: 0x17000022 RID: 34
		public static double Persistence
		{
			// Token: 0x0600023F RID: 575 RVA: 0x00032DD5 File Offset: 0x00030FD5
			get;
			// Token: 0x06000240 RID: 576 RVA: 0x00032DDC File Offset: 0x00030FDC
			set;
		}

		// Token: 0x17000023 RID: 35
		public static double Frequency
		{
			// Token: 0x06000241 RID: 577 RVA: 0x00032DE4 File Offset: 0x00030FE4
			get;
			// Token: 0x06000242 RID: 578 RVA: 0x00032DEB File Offset: 0x00030FEB
			set;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00032DF4 File Offset: 0x00030FF4
		static NoiseGenerator()
		{
			NoiseGenerator.Seed = new Random().Next(2147483647);
			NoiseGenerator.Octaves = 8;
			NoiseGenerator.Amplitude = 1.0;
			NoiseGenerator.Frequency = 0.015;
			NoiseGenerator.Persistence = 0.65;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00032E48 File Offset: 0x00031048
		public static double Noise(int x, int y)
		{
			double total = 0.0;
			double freq = NoiseGenerator.Frequency;
			double amp = NoiseGenerator.Amplitude;
			for (int i = 0; i < NoiseGenerator.Octaves; i++)
			{
				total += NoiseGenerator.Smooth((double)x * freq, (double)y * freq) * amp;
				freq *= 2.0;
				amp *= NoiseGenerator.Persistence;
			}
			if (total < -2.4)
			{
				total = -2.4;
			}
			else if (total > 2.4)
			{
				total = 2.4;
			}
			return total / 2.4;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00032EDC File Offset: 0x000310DC
		public static double NoiseGeneration(int x, int y)
		{
			int i = x + y * 57;
			i = (i << 13 ^ i);
			return 1.0 - (double)(i * (i * i * 15731 + 789221) + NoiseGenerator.Seed & 2147483647) / 1073741824.0;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00032F2C File Offset: 0x0003112C
		private static double Interpolate(double x, double y, double a)
		{
			double value = (1.0 - Math.Cos(a * 3.1415926535897931)) * 0.5;
			return x * (1.0 - value) + y * value;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00032F70 File Offset: 0x00031170
		private static double Smooth(double x, double y)
		{
			double arg_35_0 = NoiseGenerator.NoiseGeneration((int)x, (int)y);
			double n2 = NoiseGenerator.NoiseGeneration((int)x + 1, (int)y);
			double n3 = NoiseGenerator.NoiseGeneration((int)x, (int)y + 1);
			double n4 = NoiseGenerator.NoiseGeneration((int)x + 1, (int)y + 1);
			double arg_4D_0 = NoiseGenerator.Interpolate(arg_35_0, n2, x - (double)((int)x));
			double i2 = NoiseGenerator.Interpolate(n3, n4, x - (double)((int)x));
			return NoiseGenerator.Interpolate(arg_4D_0, i2, y - (double)((int)y));
		}
	}
}
