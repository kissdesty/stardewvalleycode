using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000155 RID: 341
	public class Rabbit : Critter
	{
		// Token: 0x0600130B RID: 4875 RVA: 0x0017FF98 File Offset: 0x0017E198
		public Rabbit(Vector2 position, bool flip)
		{
			this.position = position * (float)Game1.tileSize;
			position.Y += (float)(Game1.tileSize * 3 / 4);
			this.flip = flip;
			this.baseFrame = (Game1.currentSeason.Equals("winter") ? 74 : 54);
			this.sprite = new AnimatedSprite(Critter.critterTexture, Game1.currentSeason.Equals("winter") ? 69 : 68, 32, 32);
			this.sprite.loop = true;
			this.startingPosition = position;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x00180040 File Offset: 0x0017E240
		public override bool update(GameTime time, GameLocation environment)
		{
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer <= 0 && !this.running)
			{
				if (Utility.isOnScreen(this.position, -Game1.tileSize / 2))
				{
					this.running = true;
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame, 40),
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, 40),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, 40),
						new FarmerSprite.AnimationFrame(this.baseFrame + 3, 100),
						new FarmerSprite.AnimationFrame(this.baseFrame + 5, 70),
						new FarmerSprite.AnimationFrame(this.baseFrame + 5, 40)
					});
					this.sprite.loop = true;
				}
				this.characterCheckTimer = 200;
			}
			if (this.running)
			{
				this.position.X = this.position.X + (float)(this.flip ? -6 : 6);
			}
			if (this.running && this.characterCheckTimer <= 0)
			{
				this.characterCheckTimer = 200;
				if (environment.largeTerrainFeatures != null)
				{
					Rectangle tileRect = new Rectangle((int)this.position.X + Game1.tileSize / 2, (int)this.position.Y - Game1.tileSize / 2, Game1.pixelZoom, Game1.tileSize * 3);
					foreach (LargeTerrainFeature f in environment.largeTerrainFeatures)
					{
						if (f is Bush && f.getBoundingBox().Intersects(tileRect))
						{
							(f as Bush).performUseAction(f.tilePosition);
							return true;
						}
					}
				}
			}
			return base.update(time, environment);
		}

		// Token: 0x04001386 RID: 4998
		private int characterCheckTimer = 200;

		// Token: 0x04001387 RID: 4999
		private bool running;
	}
}
