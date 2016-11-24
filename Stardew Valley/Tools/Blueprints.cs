using System;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewValley.Tools
{
	// Token: 0x02000059 RID: 89
	public class Blueprints : Tool
	{
		// Token: 0x060008BF RID: 2239 RVA: 0x000BAF68 File Offset: 0x000B9168
		public Blueprints() : base("Farmer's Catalogue", 0, 75, 75, "Use this to purchase buildings, animals, and more!", false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x000BAF9C File Offset: 0x000B919C
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			if (Game1.activeClickableMenu == null)
			{
				Game1.activeClickableMenu = new BlueprintsMenu(Game1.viewport.Width / 2 - (Game1.viewport.Width / 2 + Game1.tileSize * 3 / 2) / 2, Game1.viewport.Height / 4);
				((BlueprintsMenu)Game1.activeClickableMenu).changePosition(((BlueprintsMenu)Game1.activeClickableMenu).xPositionOnScreen, Game1.viewport.Height / 2 - ((BlueprintsMenu)Game1.activeClickableMenu).height / 2);
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00002834 File Offset: 0x00000A34
		public override void tickUpdate(GameTime time, Farmer who)
		{
		}
	}
}
