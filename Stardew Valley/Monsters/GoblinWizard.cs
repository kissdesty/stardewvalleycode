using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.Projectiles;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	// Token: 0x020000B4 RID: 180
	public class GoblinWizard : Monster
	{
		// Token: 0x06000BD9 RID: 3033 RVA: 0x000EC51D File Offset: 0x000EA71D
		public GoblinWizard()
		{
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000EC530 File Offset: 0x000EA730
		public GoblinWizard(Vector2 position) : this(position, 2)
		{
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000EC53C File Offset: 0x000EA73C
		public GoblinWizard(Vector2 position, int facingDirection) : base("Goblin Wizard", position, facingDirection)
		{
			this.facingDirection = facingDirection;
			this.faceDirection(facingDirection);
			this.sprite.faceDirection(facingDirection);
			this.moveTowardPlayerThreshold = 8;
			base.IsWalkingTowardPlayer = false;
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000EB890 File Offset: 0x000E9A90
		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = Game1.tileSize * 3 / 2;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x000EC58C File Offset: 0x000EA78C
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - addedPrecision * this.missChance || this.teleporting)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					this.deathAnimation();
					Game1.playSound("goblinDie");
				}
				else
				{
					if (!this.spottedPlayer)
					{
						this.controller = null;
						this.spottedPlayer = true;
						this.Halt();
						base.facePlayer(Game1.player);
						Game1.playSound("goblinSpot");
						actualDamage *= 3;
						Game1.addHUDMessage(new HUDMessage("Sneak Attack!", Color.Yellow, 3500f));
					}
					Game1.playSound("goblinHurt");
					if (this.casting)
					{
						this.coolDown += 200;
					}
					if (Game1.random.NextDouble() < 0.25)
					{
						this.castTeleport();
					}
				}
			}
			return actualDamage;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x000EC698 File Offset: 0x000EA898
		public void castTeleport()
		{
			int tries = 0;
			Vector2 possiblePoint = new Vector2(base.getTileLocation().X + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)), base.getTileLocation().Y + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)));
			while (tries < 6 && (!Game1.currentLocation.isTileOnMap(possiblePoint) || !Game1.currentLocation.isTileLocationOpen(new Location((int)possiblePoint.X, (int)possiblePoint.Y)) || Game1.currentLocation.isTileOccupiedForPlacement(possiblePoint, null)))
			{
				possiblePoint = new Vector2(base.getTileLocation().X + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)), base.getTileLocation().Y + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-12, -6) : Game1.random.Next(6, 12)));
				tries++;
			}
			if (tries < 6)
			{
				this.teleporting = true;
				this.teleportationPath = Utility.GetPointsOnLine((int)base.getTileLocation().X, (int)base.getTileLocation().Y, (int)possiblePoint.X, (int)possiblePoint.Y, true).GetEnumerator();
				this.coolDown = 200;
				Game1.playSound("leafrustle");
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000EC858 File Offset: 0x000EAA58
		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, Game1.tileSize * 3 / 2 * 5, Game1.tileSize, Game1.tileSize * 3 / 2), (float)Game1.random.Next(150, 200), 4, 0, this.position, false, false));
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x000EC8C4 File Offset: 0x000EAAC4
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.teleporting)
			{
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					if (this.teleportationPath.MoveNext())
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, this.sprite.SourceRect, this.position, false, 0.05f, Color.Violet));
						this.position = new Vector2((float)(this.teleportationPath.Current.X * Game1.tileSize + 4), (float)(this.teleportationPath.Current.Y * Game1.tileSize - Game1.tileSize / 2 - 4));
					}
					else
					{
						this.teleporting = false;
						this.coolDown = 500;
					}
				}
			}
			else if (!this.spottedPlayer && this.controller == null && Game1.random.NextDouble() < 0.005)
			{
				this.faceDirection(Game1.random.Next(4));
			}
			if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision(this) && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = null;
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				Game1.playSound("goblinSpot");
				return;
			}
			if (this.casting)
			{
				this.scale = 1f + (float)(1500 - this.coolDown) / 8000f;
				this.coolDown -= time.ElapsedGameTime.Milliseconds;
				if (this.coolDown <= 0)
				{
					this.scale = 1f;
					Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(this.GetBoundingBox().Center, 15f, Game1.player);
					if (Game1.player.addedSpeed >= 0 && Game1.random.NextDouble() < 0.6)
					{
						Game1.currentLocation.projectiles.Add(new DebuffingProjectile(new Buff(12), 0, 2, 2, 0.09817477f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y), null));
					}
					else
					{
						Game1.playSound("fireball");
						Game1.currentLocation.projectiles.Add(new BasicProjectile(8, 1, 0, 5, 0f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float)this.GetBoundingBox().X, (float)this.GetBoundingBox().Y)));
					}
					this.casting = false;
					this.coolDown = 1500;
					return;
				}
			}
			else
			{
				if (this.spottedPlayer && base.withinPlayerThreshold(8))
				{
					if (this.coolDown <= 0 && Game1.random.NextDouble() < 0.02)
					{
						this.casting = true;
						this.Halt();
						this.coolDown = 500;
						if (this.sprite.CurrentFrame < 16)
						{
							this.sprite.CurrentFrame = 16 + this.sprite.CurrentFrame / 4;
						}
					}
					if (this.health < 8)
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
					this.coolDown -= time.ElapsedGameTime.Milliseconds;
					return;
				}
				if (this.spottedPlayer)
				{
					base.IsWalkingTowardPlayer = false;
					this.spottedPlayer = false;
					this.controller = null;
					this.addedSpeed = 0;
				}
			}
		}

		// Token: 0x04000BA7 RID: 2983
		public const int visionDistance = 8;

		// Token: 0x04000BA8 RID: 2984
		public const int spellCooldown = 1500;

		// Token: 0x04000BA9 RID: 2985
		private bool spottedPlayer;

		// Token: 0x04000BAA RID: 2986
		private bool casting;

		// Token: 0x04000BAB RID: 2987
		private bool teleporting;

		// Token: 0x04000BAC RID: 2988
		private int coolDown = 1500;

		// Token: 0x04000BAD RID: 2989
		private IEnumerator<Point> teleportationPath;
	}
}
