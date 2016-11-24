using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Monsters
{
	// Token: 0x020000B6 RID: 182
	public class Cat : Monster
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x000ED1F9 File Offset: 0x000EB3F9
		public Cat()
		{
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000ED208 File Offset: 0x000EB408
		public Cat(Vector2 position, string myName) : base("Cat", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.sprite.spriteWidth = Game1.tileSize;
			this.sprite.spriteHeight = Game1.tileSize;
			this.sprite.UpdateSourceRect();
			this.kittyName = myName;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000ED264 File Offset: 0x000EB464
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			this.health -= actualDamage;
			base.doEmote(12, true);
			return actualDamage;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x000ED298 File Offset: 0x000EB498
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.isEmoting && this.currentEmote == 20)
			{
				this.Halt();
				this.sprite.CurrentFrame = 22;
				this.sprite.UpdateSourceRect();
				this.currentBehavior = -1;
				return;
			}
			if (this.currentBehavior == -1 && Game1.random.NextDouble() < 0.0005)
			{
				Game1.playSound("catHurt");
				this.currentBehavior = -2;
			}
			if (Game1.timeOfDay == 600)
			{
				this.currentBehavior = 2;
			}
			if (this.currentBehavior == 0)
			{
				int spriteFrame = this.sprite.currentFrame;
				this.sprite.Animate(time, 20, 2, 400f);
				if (this.sprite.currentFrame != spriteFrame)
				{
					this.timesOnCurrentAnimation++;
					if (this.timesOnCurrentAnimation > 4 && Game1.random.NextDouble() < 0.2)
					{
						this.timesOnCurrentAnimation = 0;
						this.Halt();
						this.currentBehavior = -1;
					}
				}
			}
			else if (this.currentBehavior == 1)
			{
				int spriteFrame2 = this.sprite.currentFrame;
				this.sprite.Animate(time, 16, 2, 200f);
				if (this.sprite.currentFrame != spriteFrame2)
				{
					this.timesOnCurrentAnimation++;
					if (this.timesOnCurrentAnimation > 4 && Game1.random.NextDouble() < 0.2)
					{
						this.timesOnCurrentAnimation = 0;
						base.setTrajectory(0, -Game1.random.Next(16, 24));
						this.Halt();
						this.currentBehavior = -1;
					}
				}
			}
			else if (this.currentBehavior == 2)
			{
				int spriteFrame3 = this.sprite.currentFrame;
				this.sprite.Animate(time, 18, 2, 1000f);
				if ((this.sprite.currentFrame != spriteFrame3 && Game1.random.NextDouble() < 0.02) || this.xVelocity != 0f || this.yVelocity != 0f)
				{
					this.Halt();
					this.currentBehavior = -1;
				}
				else if (this.sprite.CurrentFrame != spriteFrame3 && this.sprite.CurrentFrame == 18 && Game1.debrisSoundInterval <= 0f && Game1.random.NextDouble() < 0.4 + (this.madeSnoozeSoundLastFrame ? 0.4 : -0.1))
				{
					Game1.playSound("breathin");
					Game1.debrisSoundInterval = 999f - this.sprite.timer;
					this.madeSnoozeSoundLastFrame = true;
				}
				else if (this.sprite.CurrentFrame != spriteFrame3 && this.sprite.CurrentFrame == 19 && Game1.debrisSoundInterval <= 0f && Game1.random.NextDouble() < 0.4 + (this.madeSnoozeSoundLastFrame ? 0.4 : -0.1))
				{
					Game1.playSound("breathout");
					this.madeSnoozeSoundLastFrame = true;
					Game1.debrisSoundInterval = 999f - this.sprite.timer;
				}
				else
				{
					this.madeSnoozeSoundLastFrame = false;
				}
			}
			else if (Game1.random.NextDouble() < 0.01)
			{
				switch (Game1.random.Next(12))
				{
				case 0:
					base.SetMovingOnlyUp();
					break;
				case 1:
					base.SetMovingOnlyRight();
					break;
				case 2:
					base.SetMovingOnlyDown();
					break;
				case 3:
					base.SetMovingOnlyLeft();
					break;
				case 4:
					this.currentBehavior = 0;
					break;
				case 5:
					this.currentBehavior = 1;
					break;
				case 6:
					this.currentBehavior = 2;
					break;
				default:
					this.Halt();
					break;
				}
			}
			if (Game1.player.ActiveObject != null && (Game1.player.ActiveObject.Category == -6 || (Game1.player.ActiveObject.ParentSheetIndex >= 129 && Game1.player.ActiveObject.ParentSheetIndex <= 147)))
			{
				base.IsWalkingTowardPlayer = true;
			}
			else
			{
				base.IsWalkingTowardPlayer = false;
			}
			this.MovePosition(time, Game1.viewport, Game1.currentLocation);
		}

		// Token: 0x04000BAF RID: 2991
		public const int lick = 0;

		// Token: 0x04000BB0 RID: 2992
		public const int pounce = 1;

		// Token: 0x04000BB1 RID: 2993
		public const int nap = 2;

		// Token: 0x04000BB2 RID: 2994
		public int currentBehavior = -1;

		// Token: 0x04000BB3 RID: 2995
		public int timesOnCurrentAnimation;

		// Token: 0x04000BB4 RID: 2996
		public string kittyName;

		// Token: 0x04000BB5 RID: 2997
		public bool wasPet;

		// Token: 0x04000BB6 RID: 2998
		public bool madeSnoozeSoundLastFrame;

		// Token: 0x04000BB7 RID: 2999
		public int loveForMaster;
	}
}
