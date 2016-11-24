using System;

namespace StardewValley.Quests
{
	// Token: 0x0200007F RID: 127
	public class GoSomewhereQuest : Quest
	{
		// Token: 0x06000A0D RID: 2573 RVA: 0x000D4730 File Offset: 0x000D2930
		public GoSomewhereQuest()
		{
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000D49F3 File Offset: 0x000D2BF3
		public GoSomewhereQuest(string where)
		{
			this.whereToGo = where;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x000D4A02 File Offset: 0x000D2C02
		public override void adjustGameLocation(GameLocation location)
		{
			this.checkIfComplete(null, -1, -2, null, location.name);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x000D4A16 File Offset: 0x000D2C16
		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (str != null && str.Equals(this.whereToGo))
			{
				base.questComplete();
				return true;
			}
			return false;
		}

		// Token: 0x04000A63 RID: 2659
		public string whereToGo;
	}
}
