using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Tools
{
	// Token: 0x02000062 RID: 98
	public class Raft : Tool
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x000C646C File Offset: 0x000C466C
		public Raft() : base("Raft", 0, 1, 1, "Not suitable for ocean use.", false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000C64A0 File Offset: 0x000C46A0
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			if (!who.isRafting && location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "Water", "Back") != null)
			{
				who.isRafting = true;
				Rectangle collidingBox = new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize);
				if (location.isCollidingPosition(collidingBox, Game1.viewport, true))
				{
					who.isRafting = false;
					return;
				}
				who.xVelocity = ((who.facingDirection == 1) ? 3f : ((who.facingDirection == 3) ? -3f : 0f));
				who.yVelocity = ((who.facingDirection == 2) ? 3f : ((who.facingDirection == 0) ? -3f : 0f));
				who.position = new Vector2((float)(x - Game1.tileSize / 2), (float)(y - Game1.tileSize / 2 - Game1.tileSize / 2 - ((y < who.getStandingY()) ? Game1.tileSize : 0)));
				Game1.playSound("dropItemInWater");
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}
	}
}
