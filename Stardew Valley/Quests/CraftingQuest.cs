using System;

namespace StardewValley.Quests
{
	// Token: 0x02000080 RID: 128
	public class CraftingQuest : Quest
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x000D4730 File Offset: 0x000D2930
		public CraftingQuest()
		{
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000D4A34 File Offset: 0x000D2C34
		public CraftingQuest(int indexToCraft, bool bigCraftable)
		{
			this.indexToCraft = indexToCraft;
			this.isBigCraftable = bigCraftable;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000D4A4A File Offset: 0x000D2C4A
		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (item != null && item is Object && (item as Object).bigCraftable == this.isBigCraftable && (item as Object).parentSheetIndex == this.indexToCraft)
			{
				base.questComplete();
				return true;
			}
			return false;
		}

		// Token: 0x04000A64 RID: 2660
		public bool isBigCraftable;

		// Token: 0x04000A65 RID: 2661
		public int indexToCraft;
	}
}
