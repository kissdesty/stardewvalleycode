using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Tools
{
	// Token: 0x0200006A RID: 106
	public class Wand : Tool
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x000C8867 File Offset: 0x000C6A67
		public Wand() : base("Return Scepter", 0, 2, 2, "The golden handle quivers with raw potential. Hold this scepter to the sky and return home at will.", false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000C8898 File Offset: 0x000C6A98
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			this.indexOfMenuItemView = 2;
			base.CurrentParentTileIndex = 2;
			if (who.IsMainPlayer)
			{
				for (int i = 0; i < 12; i++)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)who.position.X - Game1.tileSize * 4, (int)who.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)who.position.Y - Game1.tileSize * 4, (int)who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
				}
				Game1.playSound("wand");
				Game1.displayFarmer = false;
				Game1.player.Halt();
				Game1.player.faceDirection(2);
				Game1.player.freezePause = 1000;
				Game1.flashAlpha = 1f;
				DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.wandWarpForReal), 1000);
				Rectangle r = new Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize);
				r.Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
				int j = 0;
				for (int xTile = who.getTileX() + 8; xTile >= who.getTileX() - 8; xTile--)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)xTile, (float)who.getTileY()) * (float)Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
					{
						layerDepth = 1f,
						delayBeforeAnimationStart = j * 25,
						motion = new Vector2(-0.25f, 0f)
					});
					j++;
				}
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000C8ABA File Offset: 0x000C6CBA
		public override bool actionWhenPurchased()
		{
			Game1.player.mailReceived.Add("ReturnScepter");
			return base.actionWhenPurchased();
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x000C8AD8 File Offset: 0x000C6CD8
		private void wandWarpForReal()
		{
			Game1.warpFarmer("Farm", 64, 15, false);
			if (!Game1.isStartingToGetDarkOut())
			{
				Game1.playMorningSong();
			}
			else
			{
				Game1.changeMusicTrack("none");
			}
			Game1.fadeToBlackAlpha = 0.99f;
			Game1.screenGlow = false;
			Game1.player.temporarilyInvincible = false;
			Game1.player.temporaryInvincibilityTimer = 0;
			Game1.displayFarmer = true;
		}

		// Token: 0x0400093E RID: 2366
		public bool charged;
	}
}
