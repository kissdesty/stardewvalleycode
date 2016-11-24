using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Monsters
{
	// Token: 0x020000BC RID: 188
	public class LavaCrab : Monster
	{
		// Token: 0x06000C0B RID: 3083 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public LavaCrab()
		{
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000EF0CE File Offset: 0x000ED2CE
		public LavaCrab(Vector2 position) : base("Lava Crab", position)
		{
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x000EF0DC File Offset: 0x000ED2DC
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else if (this.sprite.CurrentFrame % 4 == 0)
			{
				actualDamage = 0;
				Game1.playSound("crafting");
			}
			else
			{
				this.health -= actualDamage;
				Game1.playSound("hitEnemy");
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, Color.Purple, 10, false, 100f, 0, -1, -1f, -1, 0));
					Game1.playSound("monsterdead");
				}
			}
			return actualDamage;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000EF1A0 File Offset: 0x000ED3A0
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.isMoving() && this.sprite.CurrentFrame % 4 == 0)
			{
				AnimatedSprite expr_24 = this.sprite;
				int currentFrame = expr_24.CurrentFrame;
				expr_24.CurrentFrame = currentFrame + 1;
				this.sprite.UpdateSourceRect();
			}
			if (!this.withinPlayerThreshold())
			{
				this.Halt();
			}
		}

		// Token: 0x04000BC4 RID: 3012
		private bool leftDrift;
	}
}
