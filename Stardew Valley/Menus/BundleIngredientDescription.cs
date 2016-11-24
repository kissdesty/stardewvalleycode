using System;

namespace StardewValley.Menus
{
	// Token: 0x020000F9 RID: 249
	public struct BundleIngredientDescription
	{
		// Token: 0x06000F04 RID: 3844 RVA: 0x001333E0 File Offset: 0x001315E0
		public BundleIngredientDescription(int index, int stack, int quality, bool completed)
		{
			this.completed = completed;
			this.index = index;
			this.stack = stack;
			this.quality = quality;
		}

		// Token: 0x0400101A RID: 4122
		public int index;

		// Token: 0x0400101B RID: 4123
		public int stack;

		// Token: 0x0400101C RID: 4124
		public int quality;

		// Token: 0x0400101D RID: 4125
		public bool completed;
	}
}
