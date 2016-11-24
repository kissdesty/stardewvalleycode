using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000B2 RID: 178
	public class Grub : Monster
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x000EAF60 File Offset: 0x000E9160
		public Grub()
		{
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000EAF74 File Offset: 0x000E9174
		public Grub(Vector2 position, bool hard = false) : base("Grub", position)
		{
			if (Game1.random.NextDouble() < 0.5)
			{
				this.leftDrift = true;
			}
			this.facingDirection = Game1.random.Next(4);
			this.rotation = (float)Game1.random.Next(4) / 3.14159274f;
			this.hard = hard;
			if (hard)
			{
				this.damageToFarmer *= 3;
				this.health *= 5;
				this.maxHealth = this.health;
				this.experienceGained *= 3;
				if (Game1.random.NextDouble() < 0.1)
				{
					this.objectsToDrop.Add(456);
				}
			}
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000EB042 File Offset: 0x000E9242
		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = 24;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000EB064 File Offset: 0x000E9264
		public void setHard()
		{
			this.hard = true;
			if (this.hard)
			{
				this.damageToFarmer = 12;
				this.health = 100;
				this.maxHealth = this.health;
				this.experienceGained = 10;
				if (Game1.random.NextDouble() < 0.1)
				{
					this.objectsToDrop.Add(456);
				}
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000EB0CC File Offset: 0x000E92CC
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				Game1.playSound("slimeHit");
				if (this.pupating)
				{
					Game1.playSound("crafting");
					base.setTrajectory(xTrajectory / 2, yTrajectory / 2);
					return 0;
				}
				this.slipperiness = 4;
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					Game1.playSound("slimedead");
					Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.Orange, 10, false, 100f, 0, -1, -1f, -1, 0)
					{
						holdLastFrame = true,
						alphaFade = 0.01f,
						interval = 50f
					}, Game1.currentLocation, 4, 64, 64);
				}
			}
			return actualDamage;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000EB1B8 File Offset: 0x000E93B8
		public override void defaultMovementBehavior(GameTime time)
		{
			this.scale = 1f + (float)((double)((float)Game1.tileSize / 512f) * Math.Sin(time.TotalGameTime.TotalMilliseconds / (double)(500f + this.position.X / 100f)));
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000EB20C File Offset: 0x000E940C
		public override void update(GameTime time, GameLocation location)
		{
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			if ((this.health > 8 || (this.hard && this.health >= this.maxHealth)) && !this.pupating)
			{
				base.update(time, location);
				return;
			}
			this.behaviorAtGameTick(time);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000EB260 File Offset: 0x000E9460
		public override void draw(SpriteBatch b)
		{
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(base.Sprite.SourceRect), this.hard ? Color.Lime : Color.White, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)this.sprite.spriteHeight * 3f / 4f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, (this.flip || (this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000EB3B8 File Offset: 0x000E95B8
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.invincibleCountdown > 0)
			{
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
			if (this.pupating)
			{
				this.scale = 1f + (float)Math.Sin((double)((float)time.TotalGameTime.Milliseconds * 0.3926991f)) / 12f;
				this.metamorphCounter -= time.ElapsedGameTime.Milliseconds;
				if (this.metamorphCounter <= 0)
				{
					this.health = -500;
					Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(208, 424, 32, 40), 4, base.getStandingX(), base.getStandingY(), 25, (int)base.getTileLocation().Y);
					Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(208, 424, 32, 40), 8, base.getStandingX(), base.getStandingY(), 15, (int)base.getTileLocation().Y);
					Game1.currentLocation.characters.Add(new Fly(this.position, this.hard));
					return;
				}
			}
			else if (this.health <= 8 || (this.hard && this.health < this.maxHealth))
			{
				this.metamorphCounter -= time.ElapsedGameTime.Milliseconds;
				if (this.metamorphCounter <= 0)
				{
					this.sprite.Animate(time, 16, 4, 125f);
					if (this.sprite.currentFrame == 19)
					{
						this.pupating = true;
						this.metamorphCounter = 4500;
					}
					return;
				}
				if (Math.Abs(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y) > Game1.tileSize * 2)
				{
					if (Game1.player.GetBoundingBox().Center.X > this.GetBoundingBox().Center.X)
					{
						this.SetMovingLeft(true);
					}
					else
					{
						this.SetMovingRight(true);
					}
				}
				else if (Math.Abs(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X) > Game1.tileSize * 2)
				{
					if (Game1.player.GetBoundingBox().Center.Y > this.GetBoundingBox().Center.Y)
					{
						this.SetMovingUp(true);
					}
					else
					{
						this.SetMovingDown(true);
					}
				}
				this.MovePosition(time, Game1.viewport, Game1.currentLocation);
				return;
			}
			else
			{
				if (this.withinPlayerThreshold())
				{
					this.scale = 1f;
					this.rotation = 0f;
					return;
				}
				if (this.isMoving())
				{
					this.Halt();
					this.faceDirection(Game1.random.Next(4));
					this.rotation = (float)Game1.random.Next(4) / 3.14159274f;
				}
			}
		}

		// Token: 0x04000B94 RID: 2964
		public const int healthToRunAway = 8;

		// Token: 0x04000B95 RID: 2965
		private bool leftDrift;

		// Token: 0x04000B96 RID: 2966
		private bool pupating;

		// Token: 0x04000B97 RID: 2967
		public bool hard;

		// Token: 0x04000B98 RID: 2968
		private int metamorphCounter = 2000;
	}
}
