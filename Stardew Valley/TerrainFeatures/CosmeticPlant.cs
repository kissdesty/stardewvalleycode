using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200006D RID: 109
	public class CosmeticPlant : Grass
	{
		// Token: 0x0600095C RID: 2396 RVA: 0x000CA154 File Offset: 0x000C8354
		public CosmeticPlant()
		{
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x000CA168 File Offset: 0x000C8368
		public CosmeticPlant(int which) : base(which, 1)
		{
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.scale = 1f - ((Game1.random.NextDouble() < 0.5) ? ((float)Game1.random.Next((which == 0) ? 10 : 51) / 100f) : 0f);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000CA1E8 File Offset: 0x000C83E8
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 4)), (int)((tileLocation.Y + 1f) * (float)Game1.tileSize - (float)(Game1.tileSize / 8) - 4f), Game1.tileSize / 8, Game1.tileSize / 16 + 4);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000CA248 File Offset: 0x000C8448
		public override void loadSprite()
		{
			try
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\upperCavePlants");
			}
			catch (Exception)
			{
			}
			this.xOffset = Game1.random.Next(-2, 3) * 4;
			this.yOffset = Game1.random.Next(-2, 1) * 4;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000CA2AC File Offset: 0x000C84AC
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if ((t != null && t is MeleeWeapon && ((MeleeWeapon)t).type != 2) || explosion > 0)
			{
				base.shake(0.2945243f, 0.07853982f, Game1.random.NextDouble() < 0.5);
				int numberOfWeedsToDestroy;
				if (explosion > 0)
				{
					numberOfWeedsToDestroy = Math.Max(1, explosion + 2 - Game1.random.Next(2));
				}
				else
				{
					numberOfWeedsToDestroy = ((t.upgradeLevel == 3) ? 3 : (t.upgradeLevel + 1));
				}
				Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(28 + (int)this.grassType * Game1.tileSize, 24, 28, 24), (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(6, 14));
				this.numberOfWeeds -= numberOfWeedsToDestroy;
				if (this.numberOfWeeds <= 0)
				{
					Random grassRandom = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)Game1.mine.mineLevel + (float)Game1.player.timesReachedMineBottom));
					if (grassRandom.NextDouble() < 0.005)
					{
						Game1.createObjectDebris(114, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
					}
					else if (grassRandom.NextDouble() < 0.01)
					{
						Game1.createDebris((grassRandom.NextDouble() < 0.5) ? 4 : 8, (int)tileLocation.X, (int)tileLocation.Y, grassRandom.Next(1, 2), null);
					}
					else if (grassRandom.NextDouble() < 0.02)
					{
						Game1.createDebris(92, (int)tileLocation.X, (int)tileLocation.Y, grassRandom.Next(2, 4), null);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000CA478 File Offset: 0x000C8678
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize) + new Vector2((float)(Game1.tileSize / 2 + this.xOffset), (float)(Game1.tileSize - 4 + this.yOffset))), new Rectangle?(new Rectangle((int)this.grassType * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2)), Color.White, this.shakeRotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2 - 4)), this.scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, ((float)(this.getBoundingBox(tileLocation).Y - 4) + tileLocation.X / 900f + this.scale / 100f) / 10000f);
		}

		// Token: 0x04000957 RID: 2391
		public bool flipped;

		// Token: 0x04000958 RID: 2392
		public float scale = 1f;

		// Token: 0x04000959 RID: 2393
		private int xOffset;

		// Token: 0x0400095A RID: 2394
		private int yOffset;
	}
}
