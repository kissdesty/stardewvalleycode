using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000A8 RID: 168
	public class Serpent : Monster
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public Serpent()
		{
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x000E59F0 File Offset: 0x000E3BF0
		public Serpent(Vector2 position) : base("Serpent", position)
		{
			this.slipperiness = 24 + Game1.random.Next(10);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.sprite.spriteWidth = 32;
			this.sprite.spriteHeight = 32;
			this.scale = 0.75f;
			this.hideShadow = true;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000E5A58 File Offset: 0x000E3C58
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Serpent"));
			this.sprite.spriteWidth = 32;
			this.sprite.spriteHeight = 32;
			this.scale = 0.75f;
			this.hideShadow = true;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000E5AAC File Offset: 0x000E3CAC
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				this.wasHitCounter = 500;
				Game1.playSound("serpentHit");
				if (this.health <= 0)
				{
					Rectangle bb = this.GetBoundingBox();
					bb.Inflate(-bb.Width / 2 + 1, -bb.Height / 2 + 1);
					Vector2 traj = Utility.getVelocityTowardPlayer(bb.Center, 4f, Game1.player);
					this.deathAnimation(-(int)traj.X, -(int)traj.Y);
				}
			}
			this.addedSpeed = Game1.random.Next(-1, 1);
			return actualDamage;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000E5B88 File Offset: 0x000E3D88
		public void deathAnimation(int xTrajectory, int yTrajectory)
		{
			Game1.playSound("serpentDie");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 64, 32, 32), 200f, 4, 0, this.position, false, false, 0.9f, 0.001f, Color.White, (float)Game1.pixelZoom * this.scale, 0.01f, this.rotation + 3.14159274f, (float)((double)Game1.random.Next(3, 5) * 3.1415926535897931 / 64.0), false)
			{
				motion = new Vector2((float)xTrajectory, (float)yTrajectory),
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), Color.LightGreen * 0.9f, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 50,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory),
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2((float)(Game1.tileSize / 2), 0f), Color.LightGreen * 0.8f, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 100,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.8f,
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), Color.LightGreen * 0.7f, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 150,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.6f,
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center), Color.LightGreen * 0.6f, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 200,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.4f,
				layerDepth = 1f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2(0f, (float)(Game1.tileSize / 2)), Color.LightGreen * 0.5f, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 250,
				startSound = "cowboy_monsterhit",
				motion = new Vector2((float)xTrajectory, (float)yTrajectory) * 0.2f,
				layerDepth = 1f
			});
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000E5F1C File Offset: 0x000E411C
		public override void drawAboveAllLayers(SpriteBatch b)
		{
			if (Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)Game1.tileSize, (float)this.GetBoundingBox().Height), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)Game1.tileSize, (float)(this.GetBoundingBox().Height / 2)), new Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f)));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)Game1.tileSize, (float)(this.GetBoundingBox().Height / 2)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f + 0.0001f)));
				}
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000E6154 File Offset: 0x000E4354
		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize * 2 * 3 / 4);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000E61A8 File Offset: 0x000E43A8
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.wasHitCounter >= 0)
			{
				this.wasHitCounter -= time.ElapsedGameTime.Milliseconds;
			}
			if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
			{
				this.health = -500;
			}
			if (this.position.X <= -640f || this.position.Y <= -640f || this.position.X >= (float)(Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || this.position.Y >= (float)(Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640))
			{
				this.health = -500;
			}
			this.sprite.Animate(time, 0, 9, 40f);
			if (this.withinPlayerThreshold() && this.invincibleCountdown <= 0)
			{
				this.faceDirection(2);
				float xSlope = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
				float ySlope = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
				float t = Math.Max(1f, Math.Abs(xSlope) + Math.Abs(ySlope));
				if (t < (float)Game1.tileSize)
				{
					this.xVelocity = Math.Max(-7f, Math.Min(7f, this.xVelocity * 1.1f));
					this.yVelocity = Math.Max(-7f, Math.Min(7f, this.yVelocity * 1.1f));
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
					this.wasHitCounter = 5 + Game1.random.Next(-1, 2);
				}
				float maxAccel = Math.Min(7f, Math.Max(2f, 7f - t / (float)Game1.tileSize / 2f));
				xSlope = (float)Math.Cos((double)this.rotation + 1.5707963267948966);
				ySlope = -(float)Math.Sin((double)this.rotation + 1.5707963267948966);
				this.xVelocity += -xSlope * maxAccel / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				this.yVelocity += -ySlope * maxAccel / 6f + (float)Game1.random.Next(-10, 10) / 100f;
				if (Math.Abs(this.xVelocity) > Math.Abs(-xSlope * 7f))
				{
					this.xVelocity -= -xSlope * maxAccel / 6f;
				}
				if (Math.Abs(this.yVelocity) > Math.Abs(-ySlope * 7f))
				{
					this.yVelocity -= -ySlope * maxAccel / 6f;
				}
			}
		}

		// Token: 0x04000B5F RID: 2911
		public const float rotationIncrement = 0.0490873866f;

		// Token: 0x04000B60 RID: 2912
		private int wasHitCounter;

		// Token: 0x04000B61 RID: 2913
		private float targetRotation;

		// Token: 0x04000B62 RID: 2914
		private bool turningRight;
	}
}
