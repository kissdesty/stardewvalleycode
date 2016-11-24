using System;

namespace StardewValley.Objects
{
	// Token: 0x02000097 RID: 151
	public struct ItemDescription
	{
		// Token: 0x06000B03 RID: 2819 RVA: 0x000E2441 File Offset: 0x000E0641
		public ItemDescription(byte type, int index, int stack)
		{
			this.type = type;
			this.index = index;
			this.stack = stack;
		}

		// Token: 0x04000B2F RID: 2863
		public byte type;

		// Token: 0x04000B30 RID: 2864
		public int index;

		// Token: 0x04000B31 RID: 2865
		public int stack;
	}
}
