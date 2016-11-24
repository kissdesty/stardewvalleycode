using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewValley.Tools
{
	// Token: 0x0200005D RID: 93
	public class MagnifyingGlass : Tool
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x000C0B18 File Offset: 0x000BED18
		public MagnifyingGlass() : base("Magnifying Glass", -1, 5, 5, "Use this on your animals to see how they're doing.", false, 0)
		{
			this.instantUse = true;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x000C0B38 File Offset: 0x000BED38
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			who.Halt();
			who.canMove = true;
			who.usingTool = false;
			this.DoFunction(location, Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 0, who);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x000C0B88 File Offset: 0x000BED88
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			this.currentParentTileIndex = 5;
			this.indexOfMenuItemView = 5;
			Rectangle tileRect = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (location is Farm)
			{
				using (Dictionary<long, FarmAnimal>.Enumerator enumerator = (location as Farm).animals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, FarmAnimal> a = enumerator.Current;
						if (a.Value.GetBoundingBox().Intersects(tileRect))
						{
							Game1.activeClickableMenu = new AnimalQueryMenu(a.Value);
							break;
						}
					}
					return;
				}
			}
			if (location is AnimalHouse)
			{
				foreach (KeyValuePair<long, FarmAnimal> a2 in (location as AnimalHouse).animals)
				{
					if (a2.Value.GetBoundingBox().Intersects(tileRect))
					{
						Game1.activeClickableMenu = new AnimalQueryMenu(a2.Value);
						break;
					}
				}
			}
		}
	}
}
