using System;

namespace StardewValley
{
	// Token: 0x0200000C RID: 12
	public class BloomSettings
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00006514 File Offset: 0x00004714
		public BloomSettings(string name, float bloomThreshold, float blurAmount, float bloomIntensity, float baseIntensity, float bloomSaturation, float baseSaturation, bool brightWhiteOnly = false)
		{
			this.Name = name;
			this.BloomThreshold = bloomThreshold;
			this.BlurAmount = blurAmount;
			this.BloomIntensity = bloomIntensity;
			this.BaseIntensity = baseIntensity;
			this.BloomSaturation = bloomSaturation;
			this.BaseSaturation = baseSaturation;
			this.brightWhiteOnly = brightWhiteOnly;
		}

		// Token: 0x0400006A RID: 106
		public const int nightTimeLights = 7;

		// Token: 0x0400006B RID: 107
		public string Name;

		// Token: 0x0400006C RID: 108
		public float BloomThreshold;

		// Token: 0x0400006D RID: 109
		public float BlurAmount;

		// Token: 0x0400006E RID: 110
		public float BloomIntensity;

		// Token: 0x0400006F RID: 111
		public float BaseIntensity;

		// Token: 0x04000070 RID: 112
		public float BloomSaturation;

		// Token: 0x04000071 RID: 113
		public float BaseSaturation;

		// Token: 0x04000072 RID: 114
		public bool brightWhiteOnly;

		// Token: 0x04000073 RID: 115
		public static BloomSettings[] PresetSettings = new BloomSettings[]
		{
			new BloomSettings("Default", 0.25f, 4f, 1.25f, 1f, 1f, 1f, false),
			new BloomSettings("Soft", 0f, 3f, 1f, 1f, 1f, 1f, false),
			new BloomSettings("Desaturated", 0.5f, 8f, 2f, 1f, 0f, 1f, false),
			new BloomSettings("Saturated", 0.25f, 4f, 2f, 1f, 2f, 0f, false),
			new BloomSettings("RainyDay", 0f, 2f, 0.7f, 1f, 0.5f, 0.5f, false),
			new BloomSettings("SunnyDay", 0.6f, 4f, 0.7f, 1f, 1f, 1f, false),
			new BloomSettings("B&W", 0f, 0f, 0f, 1f, 0f, 0f, false),
			new BloomSettings("NightTimeLights", 0f, 3f, 7f, 1f, 4f, 1.2f, true)
		};
	}
}
