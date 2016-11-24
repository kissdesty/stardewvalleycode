using System;

namespace StardewValley.Tools
{
	// Token: 0x02000069 RID: 105
	public class ToolFactory
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x000C8744 File Offset: 0x000C6944
		public static ToolDescription getIndexFromTool(Tool t)
		{
			if (t is Axe)
			{
				return new ToolDescription(0, (byte)t.upgradeLevel);
			}
			if (t is Hoe)
			{
				return new ToolDescription(1, (byte)t.upgradeLevel);
			}
			if (t is FishingRod)
			{
				return new ToolDescription(2, (byte)t.upgradeLevel);
			}
			if (t is Pickaxe)
			{
				return new ToolDescription(3, (byte)t.upgradeLevel);
			}
			if (t is WateringCan)
			{
				return new ToolDescription(4, (byte)t.upgradeLevel);
			}
			if (t is MeleeWeapon)
			{
				return new ToolDescription(5, (byte)t.upgradeLevel);
			}
			if (t is Slingshot)
			{
				return new ToolDescription(6, (byte)t.upgradeLevel);
			}
			return new ToolDescription(0, 0);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x000C87F4 File Offset: 0x000C69F4
		public static Tool getToolFromDescription(byte index, int upgradeLevel)
		{
			Tool t = null;
			switch (index)
			{
			case 0:
				t = new Axe();
				break;
			case 1:
				t = new Hoe();
				break;
			case 2:
				t = new FishingRod();
				break;
			case 3:
				t = new Pickaxe();
				break;
			case 4:
				t = new WateringCan();
				break;
			case 5:
				t = new MeleeWeapon(0, upgradeLevel);
				break;
			case 6:
				t = new Slingshot();
				break;
			}
			t.UpgradeLevel = upgradeLevel;
			return t;
		}

		// Token: 0x04000937 RID: 2359
		public const byte axe = 0;

		// Token: 0x04000938 RID: 2360
		public const byte hoe = 1;

		// Token: 0x04000939 RID: 2361
		public const byte fishingRod = 2;

		// Token: 0x0400093A RID: 2362
		public const byte pickAxe = 3;

		// Token: 0x0400093B RID: 2363
		public const byte wateringCan = 4;

		// Token: 0x0400093C RID: 2364
		public const byte meleeWeapon = 5;

		// Token: 0x0400093D RID: 2365
		public const byte slingshot = 6;
	}
}
