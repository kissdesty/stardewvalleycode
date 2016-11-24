using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000A7 RID: 167
	public class Mummy : Monster
	{
		// Token: 0x06000B6C RID: 2924 RVA: 0x000E5650 File Offset: 0x000E3850
		public Mummy()
		{
			this.sprite.spriteHeight = 32;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x000E5665 File Offset: 0x000E3865
		public Mummy(Vector2 position) : base("Mummy", position)
		{
			this.sprite.spriteHeight = 32;
			this.sprite.ignoreStopAnimation = true;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000E5697 File Offset: 0x000E3897
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Mummy"));
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
			this.sprite.ignoreStopAnimation = true;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x000E56D8 File Offset: 0x000E38D8
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (this.reviveTimer <= 0)
			{
				if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
				{
					actualDamage = -1;
				}
				else
				{
					this.slipperiness = 2;
					this.health -= actualDamage;
					base.setTrajectory(xTrajectory, yTrajectory);
					Game1.playSound("shadowHit");
					Game1.playSound("skeletonStep");
					base.IsWalkingTowardPlayer = true;
					if (this.health <= 0)
					{
						this.health = this.maxHealth;
						this.deathAnimation();
					}
				}
				return actualDamage;
			}
			if (isBomb)
			{
				this.health = 0;
				Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.BlueViolet, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					holdLastFrame = true,
					alphaFade = 0.01f,
					interval = 70f
				}, Game1.currentLocation, 4, 64, 64);
				Game1.playSound("ghost");
				return 999;
			}
			return -1;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000E57E0 File Offset: 0x000E39E0
		public override void deathAnimation()
		{
			Game1.playSound("monsterdead");
			this.reviveTimer = 10000;
			this.sprite.setCurrentAnimation(this.getCrumbleAnimation(false));
			this.Halt();
			this.collidesWithOtherCharacters = false;
			base.IsWalkingTowardPlayer = false;
			this.moveTowardPlayerThreshold = -1;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x000E5830 File Offset: 0x000E3A30
		private List<FarmerSprite.AnimationFrame> getCrumbleAnimation(bool reverse = false)
		{
			List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
			if (!reverse)
			{
				animation.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, null, false, 0));
			}
			else
			{
				animation.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.behaviorAfterRevival), true, 0));
			}
			animation.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, null, false, 0));
			animation.Add(new FarmerSprite.AnimationFrame(18, 100, 0, false, false, null, false, 0));
			if (!reverse)
			{
				animation.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.behaviorAfterCrumble), false, 0));
			}
			else
			{
				animation.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, null, false, 0));
			}
			if (reverse)
			{
				animation.Reverse();
			}
			return animation;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x000E58EC File Offset: 0x000E3AEC
		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.sprite.currentAnimation != null && base.Sprite.animateOnce(time))
			{
				this.sprite.currentAnimation = null;
			}
			if (this.reviveTimer > 0)
			{
				this.reviveTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.reviveTimer < 2000)
				{
					base.shake(this.reviveTimer);
				}
				if (this.reviveTimer <= 0)
				{
					this.sprite.setCurrentAnimation(this.getCrumbleAnimation(true));
					Game1.playSound("skeletonDie");
					base.IsWalkingTowardPlayer = true;
				}
			}
			if (this.withinPlayerThreshold())
			{
				base.IsWalkingTowardPlayer = true;
			}
			base.behaviorAtGameTick(time);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x000E599F File Offset: 0x000E3B9F
		private void behaviorAfterCrumble(Farmer who)
		{
			this.Halt();
			this.sprite.CurrentFrame = 19;
			this.sprite.currentAnimation = null;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x000E59C0 File Offset: 0x000E3BC0
		private void behaviorAfterRevival(Farmer who)
		{
			base.IsWalkingTowardPlayer = true;
			this.collidesWithOtherCharacters = true;
			this.sprite.CurrentFrame = 0;
			this.moveTowardPlayerThreshold = 8;
			this.sprite.currentAnimation = null;
		}

		// Token: 0x04000B5D RID: 2909
		private int reviveTimer;

		// Token: 0x04000B5E RID: 2910
		public const int revivalTime = 10000;
	}
}
