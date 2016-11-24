using System;

namespace StardewValley
{
	// Token: 0x02000032 RID: 50
	internal struct ShippedItem
	{
		// Token: 0x0600029A RID: 666 RVA: 0x00036273 File Offset: 0x00034473
		public ShippedItem(int index, int price, string name)
		{
			this.index = index;
			this.price = price;
			this.name = name;
		}

		// Token: 0x040002BA RID: 698
		public int index;

		// Token: 0x040002BB RID: 699
		public int price;

		// Token: 0x040002BC RID: 700
		public string name;
	}
}
