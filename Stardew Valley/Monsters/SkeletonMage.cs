using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	// Token: 0x020000AB RID: 171
	public class SkeletonMage : Monster
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x000E6D49 File Offset: 0x000E4F49
		public SkeletonMage()
		{
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000E6D5C File Offset: 0x000E4F5C
		public SkeletonMage(Vector2 position) : base("Skeleton Mage", position)
		{
			if (Game1.player.friendships.ContainsKey("???") && Game1.player.friendships["???"][0] >= 1250)
			{
				this.damageToFarmer = 0;
			}
			this.sprite.spriteHeight = 32;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000E6DC7 File Offset: 0x000E4FC7
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Skeleton Mage"));
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000E6DFC File Offset: 0x000E4FFC
		public override void draw(SpriteBatch b)
		{
			if (!this.casting)
			{
				base.draw(b);
				return;
			}
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 + Game1.random.Next(-8, 9)), (float)(Game1.tileSize + Game1.random.Next(-8, 9))), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White * 0.5f, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 + Game1.random.Next(-8, 9)), (float)(Game1.tileSize + Game1.random.Next(-8, 9))), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White * 0.5f, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 1) / 10000f)));
			for (int i = 0; i < 8; i++)
			{
				b.Draw(Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, base.getStandingPosition()), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(24, 92, 8, 8)), Color.White * 0.7f, this.rotationTimer + (float)i * 3.14159274f / 4f, new Vector2(32f, (float)(Game1.tileSize * 4)), 1.5f, SpriteEffects.None, 0.95f);
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000E7034 File Offset: 0x000E5234
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if (Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				this.health -= actualDamage;
				if (this.casting && Game1.random.NextDouble() < 0.5)
				{
					this.coolDown += 200;
				}
				else if (Game1.random.NextDouble() < 0.4 + 1.0 / (double)this.health && !Game1.currentLocation.IsFarm)
				{
					this.castTeleport();
					if (this.health <= 10)
					{
						this.speed = Math.Min(3, this.speed + 1);
					}
				}
				else
				{
					base.setTrajectory(xTrajectory, yTrajectory);
					Game1.playSound("skeletonHit");
				}
				if (this.health <= 0)
				{
					Game1.playSound("skeletonDie");
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.White, 10, false, 70f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
					{
						delayBeforeAnimationStart = 100
					});
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(Game1.tileSize / 4), 0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
					{
						delayBeforeAnimationStart = 200
					});
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000E728C File Offset: 0x000E548C
		public override void shedChunks(int number)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, 512, 64, 64), 32, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, 0.75f);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000E72FC File Offset: 0x000E54FC
		public override void deathAnimation()
		{
			Game1.playSound("skeletonDie");
			int numberOfDebris = Game1.random.Next(5, 13);
			for (int i = 0; i < numberOfDebris; i++)
			{
				Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize + Game1.random.Next(3) * Game1.tileSize, 512, 64, 64), 64, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, 1, (int)base.getTileLocation().Y, Color.White, 1f);
			}
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(12, (Game1.random.NextDouble() < 0.5) ? 11 : 139, 44, 41), 44, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, 1, (int)base.getTileLocation().Y, Color.White, 1f);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x000E7428 File Offset: 0x000E5628
		public void castTeleport()
		{
			int tries = 0;
			Vector2 possiblePoint = new Vector2(base.getTileLocation().X + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-5, -1) : Game1.random.Next(2, 6)), base.getTileLocation().Y + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-5, -1) : Game1.random.Next(2, 6)));
			while (tries < 6 && (!Game1.currentLocation.isTileOnMap(possiblePoint) || !Game1.currentLocation.isTileLocationOpen(new Location((int)possiblePoint.X, (int)possiblePoint.Y)) || Game1.currentLocation.isTileOccupiedForPlacement(possiblePoint, null)))
			{
				possiblePoint = new Vector2(base.getTileLocation().X + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-5, -1) : Game1.random.Next(2, 6)), base.getTileLocation().Y + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-5, -1) : Game1.random.Next(2, 6)));
				tries++;
			}
			if (tries < 6)
			{
				this.teleporting = true;
				this.teleportationPath = Utility.GetPointsOnLine((int)base.getTileLocation().X, (int)base.getTileLocation().Y, (int)possiblePoint.X, (int)possiblePoint.Y, true).GetEnumerator();
				this.coolDown = 20;
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x000E75D4 File Offset: 0x000E57D4
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.timeBeforeAIMovementAgain <= 0f)
			{
				this.isInvisible = false;
			}
			if (this.teleporting)
			{
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					if (this.teleportationPath.MoveNext())
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, this.sprite.SourceRect, this.position, false, 0.04f, Color.White));
						this.position = new Vector2((float)(this.teleportationPath.Current.X * Game1.tileSize + 4), (float)(this.teleportationPath.Current.Y * Game1.tileSize - Game1.tileSize / 2 - 4));
						this.coolDown = 20;
						return;
					}
					this.teleporting = false;
					this.coolDown = 500;
					return;
				}
			}
			else if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision(this) && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = null;
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				if (Game1.random.NextDouble() < 0.3)
				{
					Game1.playSound("skeletonStep");
					return;
				}
			}
			else if (this.casting)
			{
				this.Halt();
				base.IsWalkingTowardPlayer = false;
				this.rotationTimer = (float)((double)((float)time.TotalGameTime.Milliseconds * 0.0245436933f / 24f) % 3216.9908772759482);
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					this.scale = 1f;
					Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(this.GetBoundingBox().Center, 15f, Game1.player);
					if (Game1.player.speed >= 0 && Game1.random.NextDouble() < 0.5)
					{
						Game1.currentLocation.projectiles.Add(new DebuffingProjectile(new Buff(19), 9, 4, 1, 0.196349546f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y), null));
					}
					else
					{
						Game1.playSound("coldSpell");
						Game1.currentLocation.projectiles.Add(new BasicProjectile(8, 8, 0, 6, 0f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y)));
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

		// Token: 0x04000B64 RID: 2916
		public const int visionDistance = 8;

		// Token: 0x04000B65 RID: 2917
		public const int spellCooldown = 1500;

		// Token: 0x04000B66 RID: 2918
		private bool spottedPlayer;

		// Token: 0x04000B67 RID: 2919
		private bool casting;

		// Token: 0x04000B68 RID: 2920
		private bool teleporting;

		// Token: 0x04000B69 RID: 2921
		private int coolDown = 1500;

		// Token: 0x04000B6A RID: 2922
		private IEnumerator<Point> teleportationPath;

		// Token: 0x04000B6B RID: 2923
		private float rotationTimer;
	}
}
