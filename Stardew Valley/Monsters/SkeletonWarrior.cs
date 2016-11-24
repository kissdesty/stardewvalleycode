using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000B9 RID: 185
	public class SkeletonWarrior : Monster
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public SkeletonWarrior()
		{
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000EDEA8 File Offset: 0x000EC0A8
		public SkeletonWarrior(Vector2 position) : base("Skeleton Warrior", position)
		{
			this.slipperiness = 1;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000EDEBD File Offset: 0x000EC0BD
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Skeleton Warrior"));
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000EDEDC File Offset: 0x000EC0DC
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if ((this.sprite.CurrentFrame == 16 && Game1.player.position.Y > this.position.Y + (float)(Game1.tileSize / 2)) || (this.sprite.CurrentFrame == 17 && Game1.player.position.X > this.position.X + (float)(Game1.tileSize / 2)) || (this.sprite.CurrentFrame == 18 && Game1.player.position.Y < this.position.Y - (float)Game1.tileSize) || (this.sprite.CurrentFrame == 19 && Game1.player.position.X < this.position.X - (float)(Game1.tileSize / 2)))
				{
					actualDamage = 0;
					Game1.playSound("crafting");
				}
				if (Game1.random.NextDouble() < 0.25)
				{
					this.Halt();
					actualDamage = 0;
					Game1.playSound("crafting");
					switch (this.facingDirection)
					{
					case 0:
						this.sprite.CurrentFrame = 18;
						break;
					case 1:
						this.sprite.CurrentFrame = 17;
						break;
					case 2:
						this.sprite.CurrentFrame = 16;
						break;
					case 3:
						this.sprite.CurrentFrame = 19;
						break;
					}
					this.timeBeforeAIMovementAgain = 400f;
				}
				this.health -= actualDamage;
				if (actualDamage > 0 && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				if (actualDamage > 0)
				{
					base.setTrajectory(xTrajectory, yTrajectory);
				}
			}
			if (this.health <= 0)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.White, 10, false, 70f, 0, -1, -1f, -1, 0));
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = 100
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(Game1.tileSize / 4), 0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = 200
				});
			}
			return actualDamage;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x000EE200 File Offset: 0x000EC400
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.withinPlayerThreshold())
			{
				if (Game1.random.NextDouble() < 0.005)
				{
					this.willDestroyObjectsUnderfoot = true;
				}
				if (Game1.random.NextDouble() < 0.01)
				{
					this.willDestroyObjectsUnderfoot = false;
				}
				if (base.withinPlayerThreshold(2) && Game1.random.NextDouble() < 0.01)
				{
					Game1.playSound("swordswipe");
					switch (this.facingDirection)
					{
					case 0:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float)(Game1.tileSize / 2), this.position.Y - (float)(Game1.tileSize / 4)), false, false, true, -1.57079637f));
						break;
					case 1:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float)Game1.tileSize, this.position.Y + (float)(Game1.tileSize * 3 / 4)), false, false));
						break;
					case 2:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float)(Game1.tileSize / 2), this.position.Y + (float)(Game1.tileSize * 3 / 2)), false, false, true, 1.57079637f));
						break;
					case 3:
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X - (float)(Game1.tileSize / 4), this.position.Y + (float)(Game1.tileSize * 3 / 4)), false, true));
						break;
					}
					int x = (int)this.GetToolLocation(false).X;
					int y = (int)this.GetToolLocation(false).Y;
					Rectangle areaOfEffect = Rectangle.Empty;
					Rectangle playerBoundingBox = this.GetBoundingBox();
					switch (this.facingDirection)
					{
					case 0:
						areaOfEffect = new Rectangle(x - Game1.tileSize, playerBoundingBox.Y - Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
						break;
					case 1:
						areaOfEffect = new Rectangle(playerBoundingBox.Right, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
						break;
					case 2:
						areaOfEffect = new Rectangle(x - Game1.tileSize, playerBoundingBox.Bottom, Game1.tileSize * 2, Game1.tileSize);
						break;
					case 3:
						areaOfEffect = new Rectangle(playerBoundingBox.Left - Game1.tileSize, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
						break;
					}
					Game1.currentLocation.isCollidingPosition(areaOfEffect, Game1.viewport, false, Game1.random.Next(2, 10), true);
				}
			}
		}
	}
}
