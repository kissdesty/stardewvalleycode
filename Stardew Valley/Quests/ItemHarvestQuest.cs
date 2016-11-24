using System;

namespace StardewValley.Quests
{
	// Token: 0x0200007E RID: 126
	public class ItemHarvestQuest : Quest
	{
		// Token: 0x06000A0A RID: 2570 RVA: 0x000D4730 File Offset: 0x000D2930
		public ItemHarvestQuest()
		{
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000D499E File Offset: 0x000D2B9E
		public ItemHarvestQuest(int index, int number = 1)
		{
			this.itemIndex = index;
			this.number = number;
			this.questType = 9;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000D49BC File Offset: 0x000D2BBC
		public override bool checkIfComplete(NPC n = null, int itemIndex = -1, int numberHarvested = 1, Item item = null, string str = null)
		{
			if (!this.completed && itemIndex != -1 && itemIndex == this.itemIndex)
			{
				this.number -= numberHarvested;
				if (this.number <= 0)
				{
					base.questComplete();
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A61 RID: 2657
		public int itemIndex;

		// Token: 0x04000A62 RID: 2658
		public int number;
	}
}
