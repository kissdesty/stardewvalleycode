using System;
using Microsoft.Xna.Framework;
using StardewValley.Tools;

namespace StardewValley.Monsters
{
	// Token: 0x020000BD RID: 189
	public class RockCrab : Monster
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x000EF1F9 File Offset: 0x000ED3F9
		public RockCrab()
		{
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000EF208 File Offset: 0x000ED408
		public RockCrab(Vector2 position) : base("Rock Crab", position)
		{
			this.waiter = (Game1.random.NextDouble() < 0.4);
			bool arg_34_0 = this.waiter;
			this.moveTowardPlayerThreshold = 3;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000EF246 File Offset: 0x000ED446
		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000EF259 File Offset: 0x000ED459
		public RockCrab(Vector2 position, string name) : base(name, position)
		{
			this.waiter = (Game1.random.NextDouble() < 0.4);
			this.moveTowardPlayerThreshold = 3;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000EF28C File Offset: 0x000ED48C
		public override bool hitWithTool(Tool t)
		{
			base.hitWithTool(t);
			if (t is Pickaxe)
			{
				Game1.playSound("hammer");
				this.shellHealth--;
				base.shake(500);
				this.waiter = false;
				this.moveTowardPlayerThreshold = 3;
				base.setTrajectory(Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox(), t.getLastFarmerToUse()));
				if (this.shellHealth <= 0)
				{
					this.shellGone = true;
					base.moveTowardPlayer(-1);
					Game1.playSound("stoneCrack");
					Game1.createRadialDebris(Game1.currentLocation, 14, base.getTileX(), base.getTileY(), Game1.random.Next(2, 7), false, -1, false, -1);
					Game1.createRadialDebris(Game1.currentLocation, 14, base.getTileX(), base.getTileY(), Game1.random.Next(2, 7), false, -1, false, -1);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x000EF368 File Offset: 0x000ED568
		public override void shedChunks(int number)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 120, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, 1f * (float)Game1.pixelZoom * this.scale);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000EF3E4 File Offset: 0x000ED5E4
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (isBomb)
			{
				this.shellGone = true;
				this.waiter = false;
				base.moveTowardPlayer(-1);
			}
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else if (this.sprite.CurrentFrame % 4 == 0 && !this.shellGone)
			{
				actualDamage = 0;
				Game1.playSound("crafting");
			}
			else
			{
				this.health -= actualDamage;
				this.slipperiness = 3;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("hitEnemy");
				this.glowingColor = Color.Cyan;
				if (this.health <= 0)
				{
					Game1.playSound("monsterdead");
					this.deathAnimation();
					Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.Red, 10, false, 100f, 0, -1, -1f, -1, 0)
					{
						holdLastFrame = true,
						alphaFade = 0.01f
					}, Game1.currentLocation, 4, 64, 64);
				}
			}
			return actualDamage;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000EF4F4 File Offset: 0x000ED6F4
		public override void update(GameTime time, GameLocation location)
		{
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			if (!this.shellGone && !Game1.player.isRafting)
			{
				base.update(time, location);
				return;
			}
			if (!Game1.player.isRafting)
			{
				this.behaviorAtGameTick(time);
			}
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000EF534 File Offset: 0x000ED734
		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.waiter && this.shellHealth > 4)
			{
				this.moveTowardPlayerThreshold = 0;
				return;
			}
			base.behaviorAtGameTick(time);
			if (this.isMoving() && this.sprite.CurrentFrame % 4 == 0)
			{
				AnimatedSprite expr_3D = this.sprite;
				int currentFrame = expr_3D.CurrentFrame;
				expr_3D.CurrentFrame = currentFrame + 1;
				this.sprite.UpdateSourceRect();
			}
			if (!this.withinPlayerThreshold() && !this.shellGone)
			{
				this.Halt();
				return;
			}
			if (this.shellGone)
			{
				base.updateGlow();
				if (this.invincibleCountdown > 0)
				{
					this.glowingColor = Color.Cyan;
					this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.invincibleCountdown <= 0)
					{
						base.stopGlowing();
					}
				}
				if (Math.Abs(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y) > Game1.tileSize * 3)
				{
					if (Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X > 0)
					{
						this.SetMovingLeft(true);
					}
					else
					{
						this.SetMovingRight(true);
					}
				}
				else if (Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y > 0)
				{
					this.SetMovingUp(true);
				}
				else
				{
					this.SetMovingDown(true);
				}
				this.MovePosition(time, Game1.viewport, Game1.currentLocation);
				this.sprite.CurrentFrame = 16 + this.sprite.CurrentFrame % 4;
			}
		}

		// Token: 0x04000BC5 RID: 3013
		private bool leftDrift;

		// Token: 0x04000BC6 RID: 3014
		private bool shellGone;

		// Token: 0x04000BC7 RID: 3015
		private bool waiter;

		// Token: 0x04000BC8 RID: 3016
		private int shellHealth = 5;
	}
}
