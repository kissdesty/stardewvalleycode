using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;

namespace StardewValley.Monsters
{
	// Token: 0x020000AC RID: 172
	public class ShadowShaman : Monster
	{
		// Token: 0x06000B92 RID: 2962 RVA: 0x000E7AC3 File Offset: 0x000E5CC3
		public ShadowShaman()
		{
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x000E7AD8 File Offset: 0x000E5CD8
		public ShadowShaman(Vector2 position) : base("Shadow Shaman", position)
		{
			if (Game1.player.friendships.ContainsKey("???") && Game1.player.friendships["???"][0] >= 1250)
			{
				this.damageToFarmer = 0;
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000E7B36 File Offset: 0x000E5D36
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Shaman"));
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000E7B54 File Offset: 0x000E5D54
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.casting)
			{
				for (int i = 0; i < 8; i++)
				{
					b.Draw(Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, base.getStandingPosition()), new Rectangle?(new Rectangle(119, 6, 3, 3)), Color.White * 0.7f, this.rotationTimer + (float)i * 3.14159274f / 4f, new Vector2(8f, 48f), 1.5f * (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
				}
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x000E7BEC File Offset: 0x000E5DEC
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name != null && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				this.health -= actualDamage;
				if (this.casting && Game1.random.NextDouble() < 0.5)
				{
					this.coolDown += 200;
				}
				else
				{
					base.setTrajectory(xTrajectory, yTrajectory);
					Game1.playSound("shadowHit");
				}
				if (this.health <= 0)
				{
					Game1.playSound("shadowDie");
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000E7D30 File Offset: 0x000E5F30
		public override void deathAnimation()
		{
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0), Game1.currentLocation, 4, 64, 64);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, 16, 5), 16, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + 2, this.sprite.SourceRect.Y + 5, 16, 5), 10, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 10, 16, 5), 16, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White);
			for (int i = 1; i < 3; i++)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, 1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, -1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, 1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, -1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x000E8050 File Offset: 0x000E6250
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.timeBeforeAIMovementAgain <= 0f)
			{
				this.isInvisible = false;
			}
			if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision(this) && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = null;
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				if (Game1.random.NextDouble() < 0.3)
				{
					Game1.playSound("shadowpeep");
					return;
				}
			}
			else if (this.casting)
			{
				base.IsWalkingTowardPlayer = false;
				this.sprite.Animate(time, 16, 4, 200f);
				this.rotationTimer = (float)((double)((float)time.TotalGameTime.Milliseconds * 0.0245436933f / 24f) % 3216.9908772759482);
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					this.scale = 1f;
					Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(this.GetBoundingBox().Center, 15f, Game1.player);
					if (Game1.player.attack >= 0 && Game1.random.NextDouble() < 0.6)
					{
						Game1.currentLocation.projectiles.Add(new DebuffingProjectile(new Buff(14), 7, 4, 4, 0.196349546f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y), this));
					}
					else
					{
						List<Monster> monstersNearPlayer = new List<Monster>();
						foreach (NPC i in Game1.currentLocation.characters)
						{
							if (i is Monster && (i as Monster).withinPlayerThreshold(6))
							{
								monstersNearPlayer.Add((Monster)i);
							}
						}
						Monster lowestHealthMonster = null;
						double lowestHealth = 1.0;
						foreach (Monster j in monstersNearPlayer)
						{
							if ((double)j.health / (double)j.maxHealth <= lowestHealth)
							{
								lowestHealthMonster = j;
								lowestHealth = (double)j.health / (double)j.maxHealth;
							}
						}
						if (lowestHealthMonster != null)
						{
							int amountToHeal = 60;
							lowestHealthMonster.health = Math.Min(lowestHealthMonster.maxHealth, lowestHealthMonster.health + amountToHeal);
							Game1.playSound("healSound");
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 256, 64, 64), 40f, 8, 0, lowestHealthMonster.position + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), false, false));
							Game1.currentLocation.debris.Add(new Debris(amountToHeal, new Vector2((float)lowestHealthMonster.GetBoundingBox().Center.X, (float)lowestHealthMonster.GetBoundingBox().Center.Y), Color.Green, 1f, lowestHealthMonster));
						}
					}
					this.casting = false;
					this.coolDown = 1500;
					base.IsWalkingTowardPlayer = true;
					return;
				}
			}
			else
			{
				if (this.spottedPlayer && base.withinPlayerThreshold(8))
				{
					if (this.health < 30)
					{
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
					}
					else if (this.controller == null && !Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
					{
						this.controller = new PathFindController(this, Game1.currentLocation, new Point((int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y), -1, null, 300);
						if (this.controller == null || this.controller.pathToEndPoint == null || this.controller.pathToEndPoint.Count == 0)
						{
							this.spottedPlayer = false;
							this.Halt();
							this.controller = null;
							this.addedSpeed = 0;
						}
					}
					else if (this.coolDown <= 0 && Game1.random.NextDouble() < 0.02)
					{
						this.casting = true;
						this.Halt();
						this.coolDown = 500;
					}
					this.coolDown -= time.ElapsedGameTime.Milliseconds;
					return;
				}
				if (this.spottedPlayer)
				{
					base.IsWalkingTowardPlayer = false;
					this.spottedPlayer = false;
					this.controller = null;
					this.addedSpeed = 0;
					return;
				}
				this.defaultMovementBehavior(time);
			}
		}

		// Token: 0x04000B6C RID: 2924
		public const int visionDistance = 8;

		// Token: 0x04000B6D RID: 2925
		public const int spellCooldown = 1500;

		// Token: 0x04000B6E RID: 2926
		private bool spottedPlayer;

		// Token: 0x04000B6F RID: 2927
		private bool casting;

		// Token: 0x04000B70 RID: 2928
		private int coolDown = 1500;

		// Token: 0x04000B71 RID: 2929
		private float rotationTimer;
	}
}
