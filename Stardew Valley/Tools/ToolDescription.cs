using System;

namespace StardewValley.Tools
{
	// Token: 0x02000068 RID: 104
	public struct ToolDescription
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x000C8733 File Offset: 0x000C6933
		public ToolDescription(byte index, byte upgradeLevel)
		{
			this.index = index;
			this.upgradeLevel = upgradeLevel;
		}

		// Token: 0x04000935 RID: 2357
		public byte index;

		// Token: 0x04000936 RID: 2358
		public byte upgradeLevel;
	}
}
