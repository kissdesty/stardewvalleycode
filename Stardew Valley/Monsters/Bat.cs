using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000B0 RID: 176
	public class Bat : Monster
	{
		// Token: 0x06000BB2 RID: 2994 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public Bat()
		{
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000E9C34 File Offset: 0x000E7E34
		public Bat(Vector2 position) : base("Bat", position)
		{
			this.slipperiness = 24 + Game1.random.Next(-10, 11);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.hideShadow = true;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000E9C70 File Offset: 0x000E7E70
		public Bat(Vector2 position, int mineLevel) : base("Bat", position)
		{
			if (mineLevel >= 40 && mineLevel < 80)
			{
				this.name = "Frost Bat";
				base.parseMonsterInfo("Frost Bat");
				this.reloadSprite();
			}
			else if (mineLevel >= 80)
			{
				this.name = "Lava Bat";
				base.parseMonsterInfo("Lava Bat");
				this.reloadSprite();
			}
			this.slipperiness = 20 + Game1.random.Next(-5, 6);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.hideShadow = true;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000E9CFC File Offset: 0x000E7EFC
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + this.name));
			this.hideShadow = true;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000E9D2C File Offset: 0x000E7F2C
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			this.seenPlayer = true;
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				this.wasHitCounter = 500;
				Game1.playSound("hitEnemy");
				if (this.health <= 0)
				{
					this.deathAnimation();
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, Color.DarkMagenta, 10, false, 100f, 0, -1, -1f, -1, 0));
					Game1.playSound("batScreech");
				}
			}
			this.addedSpeed = Game1.random.Next(-1, 1);
			return actualDamage;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000E9DFC File Offset: 0x000E7FFC
		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 384, 64, 64), 32, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, scale);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000E9E68 File Offset: 0x000E8068
		public override void drawAboveAllLayers(SpriteBatch b)
		{
			if (Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), Color.White, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.92f);
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, this.wildernessFarmMonster ? 0.0001f : ((float)(base.getStandingY() - 1) / 10000f));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x000EA088 File Offset: 0x000E8288
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.wasHitCounter >= 0)
			{
				this.wasHitCounter -= time.ElapsedGameTime.Milliseconds;
			}
			if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity) || this.position.X < -2000f || this.position.Y < -2000f)
			{
				this.health = -500;
			}
			if (this.position.X <= -640f || this.position.Y <= -640f || this.position.X >= (float)(Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || this.position.Y >= (float)(Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640))
			{
				this.health = -500;
			}
			if (this.focusedOnFarmers || base.withinPlayerThreshold(6) || this.seenPlayer)
			{
				this.seenPlayer = true;
				this.sprite.Animate(time, 0, 4, 80f);
				if (this.sprite.CurrentFrame % 3 == 0 && Utility.isOnScreen(this.position, Game1.tileSize * 8) && (this.batFlap == null || !this.batFlap.IsPlaying) && Game1.soundBank != null)
				{
					this.batFlap = Game1.soundBank.GetCue("batFlap");
					this.batFlap.Play();
				}
				if (this.invincibleCountdown > 0)
				{
					if (this.name.Equals("Lava Bat"))
					{
						this.glowingColor = Color.Cyan;
					}
					return;
				}
				float xSlope = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
				float ySlope = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
				float t = Math.Max(1f, Math.Abs(xSlope) + Math.Abs(ySlope));
				if (t < (float)Game1.tileSize)
				{
					this.xVelocity = Math.Max(-5f, Math.Min(5f, this.xVelocity * 1.05f));
					this.yVelocity = Math.Max(-5f, Math.Min(5f, this.yVelocity * 1.05f));
				}
				xSlope /= t;
				ySlope /= t;
				if (this.wasHitCounter <= 0)
				{
					this.targetRotation = (float)Math.Atan2((double)(-(double)ySlope), (double)xSlope) - 1.57079637f;
					if ((double)(Math.Abs(this.targetRotation) - Math.Abs(this.rotation)) > 2.748893571891069 && Game1.random.NextDouble() < 0.5)
					{
						this.turningRight = true;
					}
					else if ((double)(Math.Abs(this.targetRotation) - Math.Abs(this.rotation)) < 0.39269908169872414)
					{
						this.turningRight = false;
					}
					if (this.turningRight)
					{
						this.rotation -= (float)Math.Sign(this.targetRotation - this.rotation) * 0.0490873866f;
					}
					else
					{
						this.rotation += (float)Math.Sign(this.targetRotation - this.rotation) * 0.0490873866f;
					}
					this.rotation %= 6.28318548f;
					this.wasHitCounter = 0;
				}
				float maxAccel = Math.Min(5f, Math.Max(1f, 5f - t / (float)Game1.tileSize / 2f));
				xSlope = (float)Math.Cos((double)this.rotation + 1.5707963267948966);
				ySlope = -(float)Math.Sin((double)this.rotation + 1.5707963267948966);
				this.xVelocity += -xSlope * maxAccel / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				this.yVelocity += -ySlope * maxAccel / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				if (Math.Abs(this.xVelocity) > Math.Abs(-xSlope * 5f))
				{
					this.xVelocity -= -xSlope * maxAccel / 6f;
				}
				if (Math.Abs(this.yVelocity) > Math.Abs(-ySlope * 5f))
				{
					this.yVelocity -= -ySlope * maxAccel / 6f;
					return;
				}
			}
			else
			{
				this.sprite.CurrentFrame = 4;
				this.Halt();
			}
		}

		// Token: 0x04000B85 RID: 2949
		public const float rotationIncrement = 0.0490873866f;

		// Token: 0x04000B86 RID: 2950
		private int wasHitCounter;

		// Token: 0x04000B87 RID: 2951
		private float targetRotation;

		// Token: 0x04000B88 RID: 2952
		private bool turningRight;

		// Token: 0x04000B89 RID: 2953
		private bool seenPlayer;

		// Token: 0x04000B8A RID: 2954
		private Cue batFlap;
	}
}
