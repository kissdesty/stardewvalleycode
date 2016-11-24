using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000156 RID: 342
	public class Frog : Critter
	{
		// Token: 0x0600130D RID: 4877 RVA: 0x0018024C File Offset: 0x0017E44C
		public Frog(Vector2 position, bool waterLeaper = false, bool forceFlip = false)
		{
			this.waterLeaper = waterLeaper;
			this.position = position * (float)Game1.tileSize;
			this.sprite = new AnimatedSprite(Critter.critterTexture, waterLeaper ? 300 : 280, 16, 16);
			this.sprite.loop = true;
			if (!this.flip & forceFlip)
			{
				this.flip = true;
			}
			if (waterLeaper)
			{
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(300, 600),
					new FarmerSprite.AnimationFrame(304, 100),
					new FarmerSprite.AnimationFrame(305, 100),
					new FarmerSprite.AnimationFrame(306, 300),
					new FarmerSprite.AnimationFrame(305, 100),
					new FarmerSprite.AnimationFrame(304, 100)
				});
			}
			else
			{
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(280, 60),
					new FarmerSprite.AnimationFrame(281, 70),
					new FarmerSprite.AnimationFrame(282, 140),
					new FarmerSprite.AnimationFrame(283, 90)
				});
				this.beforeFadeTimer = 1000;
				this.flip = (this.position.X + (float)Game1.pixelZoom < Game1.player.position.X);
			}
			this.startingPosition = position;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x001803F4 File Offset: 0x0017E5F4
		public void startSplash(Farmer who)
		{
			this.splash = true;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00180400 File Offset: 0x0017E600
		public override bool update(GameTime time, GameLocation environment)
		{
			if (this.waterLeaper)
			{
				if (!this.leapingIntoWater)
				{
					this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.characterCheckTimer <= 0)
					{
						if (Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 6, environment) != null)
						{
							this.leapingIntoWater = true;
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(300, 100),
								new FarmerSprite.AnimationFrame(301, 100),
								new FarmerSprite.AnimationFrame(302, 100),
								new FarmerSprite.AnimationFrame(303, 1500, false, false, new AnimatedSprite.endOfAnimationBehavior(this.startSplash), true)
							});
							this.sprite.loop = false;
							this.sprite.oldFrame = 303;
							this.gravityAffectedDY = -6f;
						}
						else if (Game1.random.NextDouble() < 0.01)
						{
							Game1.playSound("croak");
						}
						this.characterCheckTimer = 200;
					}
				}
				else
				{
					this.position.X = this.position.X + (float)(this.flip ? -4 : 4);
				}
			}
			else
			{
				this.position.X = this.position.X + (float)(this.flip ? -3 : 3);
				this.beforeFadeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.beforeFadeTimer <= 0)
				{
					this.alpha -= 0.001f * (float)time.ElapsedGameTime.Milliseconds;
					if (this.alpha <= 0f)
					{
						return true;
					}
				}
				if (environment.doesTileHaveProperty((int)this.position.X / Game1.tileSize, (int)this.position.Y / Game1.tileSize, "Water", "Back") != null)
				{
					this.splash = true;
				}
			}
			if (this.splash)
			{
				environment.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 50f, 2, 1, this.position, false, false));
				Game1.playSound("dropItemInWater");
				return true;
			}
			return base.update(time, environment);
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00180640 File Offset: 0x0017E840
		public override void draw(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.pixelZoom * 5))), (this.position.Y + (float)Game1.tileSize) / 10000f, 0, 0, Color.White * this.alpha, this.flip, 4f, 0f, false);
			b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.pixelZoom * 2))), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + Math.Max(-3f, (this.yJumpOffset + this.yOffset) / 16f), SpriteEffects.None, (this.position.Y - 1f) / 10000f);
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00002834 File Offset: 0x00000A34
		public override void drawAboveFrontLayer(SpriteBatch b)
		{
		}

		// Token: 0x04001388 RID: 5000
		private bool waterLeaper;

		// Token: 0x04001389 RID: 5001
		private bool leapingIntoWater;

		// Token: 0x0400138A RID: 5002
		private bool splash;

		// Token: 0x0400138B RID: 5003
		private int characterCheckTimer = 200;

		// Token: 0x0400138C RID: 5004
		private int beforeFadeTimer;

		// Token: 0x0400138D RID: 5005
		private float alpha = 1f;
	}
}
