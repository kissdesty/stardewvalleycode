using System;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000079 RID: 121
	public struct TerrainFeatureDescription
	{
		// Token: 0x060009F0 RID: 2544 RVA: 0x000D2117 File Offset: 0x000D0317
		public TerrainFeatureDescription(byte index, int extraInfo)
		{
			this.index = index;
			this.extraInfo = extraInfo;
		}

		// Token: 0x04000A18 RID: 2584
		public byte index;

		// Token: 0x04000A19 RID: 2585
		public int extraInfo;
	}
}
