using System;

namespace StardewValley.Tools
{
	// Token: 0x02000066 RID: 102
	public abstract class Stackable : Tool
	{
		// Token: 0x170000BF RID: 191
		public int NumberInStack
		{
			// Token: 0x06000930 RID: 2352 RVA: 0x000C7D94 File Offset: 0x000C5F94
			get
			{
				return this.numberInStack;
			}
			// Token: 0x06000931 RID: 2353 RVA: 0x000C7D9C File Offset: 0x000C5F9C
			set
			{
				this.numberInStack = value;
			}
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000C7DA5 File Offset: 0x000C5FA5
		public Stackable()
		{
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000C7DAD File Offset: 0x000C5FAD
		public Stackable(string name, int upgradeLevel, int initialParentTileIndex, int indexOfMenuItemView, string description, bool stackable) : base(name, upgradeLevel, initialParentTileIndex, indexOfMenuItemView, description, stackable, 0)
		{
		}

		// Token: 0x04000932 RID: 2354
		private int numberInStack;
	}
}
