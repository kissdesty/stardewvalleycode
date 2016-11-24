using System;
using Microsoft.Xna.Framework;
using StardewValley.Tools;

namespace StardewValley.Objects
{
	// Token: 0x02000098 RID: 152
	public class ObjectFactory
	{
		// Token: 0x06000B04 RID: 2820 RVA: 0x000E2458 File Offset: 0x000E0658
		public static ItemDescription getDescriptionFromItem(Item i)
		{
			if (i is Object && (i as Object).bigCraftable)
			{
				return new ItemDescription(1, (i as Object).ParentSheetIndex, i.Stack);
			}
			if (i is Object)
			{
				return new ItemDescription(0, (i as Object).ParentSheetIndex, i.Stack);
			}
			if (i is MeleeWeapon)
			{
				return new ItemDescription(2, (i as MeleeWeapon).currentParentTileIndex, i.Stack);
			}
			throw new Exception("ItemFactory trying to create item description from unknown item");
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000E24DC File Offset: 0x000E06DC
		public static Item getItemFromDescription(byte type, int index, int stack)
		{
			switch (type)
			{
			case 0:
				return new Object(Vector2.Zero, index, stack);
			case 1:
				return new Object(Vector2.Zero, index, false);
			case 2:
				return new MeleeWeapon(index);
			case 4:
				return new Object(index, stack, true, -1, 0);
			case 5:
				return new Object(Vector2.Zero, index, true);
			}
			throw new Exception("ItemFactory trying to create item from unknown description");
		}

		// Token: 0x04000B32 RID: 2866
		public const byte regularObject = 0;

		// Token: 0x04000B33 RID: 2867
		public const byte bigCraftable = 1;

		// Token: 0x04000B34 RID: 2868
		public const byte weapon = 2;

		// Token: 0x04000B35 RID: 2869
		public const byte specialItem = 3;

		// Token: 0x04000B36 RID: 2870
		public const byte regularObjectRecipe = 4;

		// Token: 0x04000B37 RID: 2871
		public const byte bigCraftableRecipe = 5;
	}
}
