using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000B1 RID: 177
	public class Fly : Monster
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x000EA55E File Offset: 0x000E875E
		public Fly()
		{
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000EA574 File Offset: 0x000E8774
		public Fly(Vector2 position, bool hard = false) : base("Fly", position)
		{
			this.slipperiness = 24 + Game1.random.Next(-10, 10);
			this.Halt();
			base.IsWalkingTowardPlayer = false;
			this.hard = hard;
			if (hard)
			{
				this.damageToFarmer *= 2;
				this.maxHealth *= 3;
				this.health = this.maxHealth;
			}
			this.hideShadow = true;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000EA5F5 File Offset: 0x000E87F5
		public void setHard()
		{
			this.hard = true;
			if (this.hard)
			{
				this.damageToFarmer = 12;
				this.maxHealth = 66;
				this.health = this.maxHealth;
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000EA622 File Offset: 0x000E8822
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Fly"));
			if (Game1.soundBank != null)
			{
				Fly.buzz = Game1.soundBank.GetCue("flybuzzing");
			}
			this.hideShadow = true;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000EA660 File Offset: 0x000E8860
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
				Game1.playSound("hitEnemy");
				if (this.health <= 0)
				{
					Game1.playSound("monsterdead");
					Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.HotPink, 10, false, 100f, 0, -1, -1f, -1, 0)
					{
						interval = 70f
					}, Game1.currentLocation, 4, 64, 64);
					if (Game1.soundBank != null && Fly.buzz != null)
					{
						Fly.buzz.Stop(AudioStopOptions.AsAuthored);
					}
				}
			}
			this.addedSpeed = Game1.random.Next(-1, 1);
			return actualDamage;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000EA74C File Offset: 0x000E894C
		public override void drawAboveAllLayers(SpriteBatch b)
		{
			if (Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), this.hard ? Color.Lime : Color.White, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f)));
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 - Game1.tileSize / 2)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000EA9A6 File Offset: 0x000E8BA6
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (Game1.currentLocation.treatAsOutdoors)
			{
				this.drawAboveAllLayers(b);
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000EA9BC File Offset: 0x000E8BBC
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (Game1.soundBank != null && (Fly.buzz == null || !Fly.buzz.IsPlaying))
			{
				Fly.buzz = Game1.soundBank.GetCue("flybuzzing");
				Fly.buzz.SetVariable("Volume", 0f);
				Fly.buzz.Play();
			}
			if ((double)Game1.fadeToBlackAlpha > 0.8 && Game1.fadeIn && Fly.buzz != null)
			{
				Fly.buzz.Stop(AudioStopOptions.AsAuthored);
			}
			else if (Fly.buzz != null)
			{
				Fly.buzz.SetVariable("Volume", Math.Max(0f, Fly.buzz.GetVariable("Volume") - 1f));
				float volume = Math.Max(0f, 100f - Vector2.Distance(this.position, Game1.player.position) / (float)Game1.tileSize / 16f * 100f);
				if (volume > Fly.buzz.GetVariable("Volume"))
				{
					Fly.buzz.SetVariable("Volume", volume);
				}
			}
			if (this.wasHitCounter >= 0)
			{
				this.wasHitCounter -= time.ElapsedGameTime.Milliseconds;
			}
			if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
			{
				this.health = -500;
			}
			this.sprite.Animate(time, (this.facingDirection == 0) ? 8 : ((this.facingDirection == 2) ? 0 : (this.facingDirection * 4)), 4, 75f);
			if (this.position.X <= -640f || this.position.Y <= -640f || this.position.X >= (float)(Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || this.position.Y >= (float)(Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640))
			{
				this.health = -500;
			}
			if (this.spawningCounter >= 0)
			{
				this.spawningCounter -= time.ElapsedGameTime.Milliseconds;
				this.scale = 1f - (float)this.spawningCounter / 1000f;
				return;
			}
			if ((this.withinPlayerThreshold() || Utility.isOnScreen(this.position, Game1.tileSize * 4)) && this.invincibleCountdown <= 0)
			{
				this.faceDirection(0);
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

		// Token: 0x04000B8B RID: 2955
		public const float rotationIncrement = 0.0490873866f;

		// Token: 0x04000B8C RID: 2956
		public const int volumeTileRange = 16;

		// Token: 0x04000B8D RID: 2957
		public const int spawnTime = 1000;

		// Token: 0x04000B8E RID: 2958
		private int spawningCounter = 1000;

		// Token: 0x04000B8F RID: 2959
		private int wasHitCounter;

		// Token: 0x04000B90 RID: 2960
		private float targetRotation;

		// Token: 0x04000B91 RID: 2961
		public static Cue buzz;

		// Token: 0x04000B92 RID: 2962
		private bool turningRight;

		// Token: 0x04000B93 RID: 2963
		public bool hard;
	}
}
