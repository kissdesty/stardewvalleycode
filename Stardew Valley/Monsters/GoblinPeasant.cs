using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley.Monsters
{
	// Token: 0x020000B3 RID: 179
	public class GoblinPeasant : Monster
	{
		// Token: 0x06000BCB RID: 3019 RVA: 0x000EB6E5 File Offset: 0x000E98E5
		public GoblinPeasant()
		{
			this.pickColors();
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000EB724 File Offset: 0x000E9924
		public GoblinPeasant(Vector2 position) : this(position, 2)
		{
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000EB730 File Offset: 0x000E9930
		public GoblinPeasant(Vector2 position, int facingDir, Rectangle hallway) : base("Goblin Warrior", position, facingDir)
		{
			this.facingDirection = facingDir;
			this.faceDirection(facingDir);
			this.sprite.faceDirection(facingDir);
			this.hallway = new Rectangle(hallway.X * Game1.tileSize, hallway.Y * Game1.tileSize, hallway.Width * Game1.tileSize, hallway.Height * Game1.tileSize);
			this.hallway.Inflate(Game1.tileSize / 2, Game1.tileSize / 2);
			this.moveTowardPlayerThreshold = 8;
			base.IsWalkingTowardPlayer = false;
			this.pickColors();
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000EB7FD File Offset: 0x000E99FD
		public GoblinPeasant(Vector2 position, int facingDirection) : this(position, facingDirection, false)
		{
			this.pickColors();
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000EB810 File Offset: 0x000E9A10
		public GoblinPeasant(Vector2 position, int facingDir, bool actionGoblin) : base("Goblin Warrior", position, facingDir)
		{
			this.facingDirection = facingDir;
			base.IsWalkingTowardPlayer = false;
			this.faceDirection(facingDir);
			this.sprite.faceDirection(facingDir);
			this.moveTowardPlayerThreshold = 8;
			this.actionGoblin = actionGoblin;
			this.pickColors();
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000EB890 File Offset: 0x000E9A90
		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = Game1.tileSize * 3 / 2;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000EB8B8 File Offset: 0x000E9AB8
		private void pickColors()
		{
			this.clothesColor = new Color(Game1.random.Next(80, 256), Game1.random.Next(80, 256), Game1.random.Next(80, 256));
			this.gogglesColor = new Color(Game1.random.Next(50, 256), Game1.random.Next(50, 256), Game1.random.Next(50, 256));
			this.skinColor = new Color(Game1.random.Next(100, (Game1.random.NextDouble() < 0.08) ? 256 : 150), Game1.random.Next(100, (Game1.random.NextDouble() < 0.08) ? 256 : 190), Game1.random.Next(100, (Game1.random.NextDouble() < 0.08) ? 256 : 190));
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000EB9D4 File Offset: 0x000E9BD4
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if (!this.spottedPlayer)
				{
					this.controller = null;
					this.spottedPlayer = true;
					this.Halt();
					base.facePlayer(Game1.player);
					base.doEmote(16, false);
					Game1.playSound("goblinSpot");
					base.IsWalkingTowardPlayer = true;
					actualDamage *= 3;
					Game1.addHUDMessage(new HUDMessage("Sneak Attack!", Color.Yellow, 3500f));
				}
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					this.deathAnimation();
					Game1.playSound("goblinDie");
					if (this.weapon.indexOfMenuItemView == 22 || Game1.random.NextDouble() < 0.18)
					{
						Game1.currentLocation.debris.Add(new Debris(this.weapon, new Vector2((float)this.GetBoundingBox().Center.X, (float)this.GetBoundingBox().Center.Y)));
					}
				}
				else
				{
					Game1.playSound("goblinHurt");
				}
			}
			return actualDamage;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000EBB18 File Offset: 0x000E9D18
		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, Game1.tileSize * 3 / 2 * 5, Game1.tileSize, Game1.tileSize * 3 / 2), (float)Game1.random.Next(150, 200), 4, 0, this.position, false, false));
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000EBB84 File Offset: 0x000E9D84
		public void meetUpWithNeighborEndFunction(Character c, GameLocation location)
		{
			List<Vector2> arg_0D_0 = Utility.getAdjacentTileLocations(base.getTileLocation());
			bool neighbor = false;
			foreach (Vector2 v in arg_0D_0)
			{
				foreach (Character charact in location.characters)
				{
					if (charact.getTileLocation().Equals(v))
					{
						charact.faceGeneralDirection(base.getTileLocation(), 0);
						base.faceGeneralDirection(v, 0);
						neighbor = true;
					}
				}
			}
			if (neighbor)
			{
				base.doEmote((Game1.random.NextDouble() < 0.5) ? 20 : ((Game1.random.NextDouble() < 0.5) ? 32 : 8), true);
			}
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000EBC7C File Offset: 0x000E9E7C
		private void tryToMeetUpWithNeighbor()
		{
			Character c = Game1.currentLocation.characters[Game1.random.Next(Game1.currentLocation.characters.Count)];
			if (!this.Equals(c) && c.controller != null && c is GoblinPeasant && !((GoblinPeasant)c).hallway.Equals(Rectangle.Empty))
			{
				Vector2 v = Utility.getRandomAdjacentOpenTile(c.getTileLocation());
				if (!v.Equals(Vector2.Zero))
				{
					this.controller = new PathFindController(this, Game1.currentLocation, new Point((int)v.X, (int)v.Y), Game1.random.Next(4), new PathFindController.endBehavior(this.meetUpWithNeighborEndFunction));
				}
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000EBD38 File Offset: 0x000E9F38
		public override void draw(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))), (float)this.GetBoundingBox().Center.Y / 10000f, 0, 0, this.skinColor, false, (float)Game1.pixelZoom, 0f, false);
			if (this.attacking)
			{
				this.weapon.drawDuringUse((200 - this.actionCountdown) / 100, base.FacingDirection, b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))) + new Vector2((float)((base.FacingDirection == 1) ? -16 : ((base.FacingDirection == 3) ? 16 : 0)), (float)(Game1.tileSize + ((base.FacingDirection == 1 || base.FacingDirection == 3) ? (Game1.tileSize / 2) : 0))), Game1.player);
			}
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))), (float)this.GetBoundingBox().Center.Y / 10000f + 1E-05f, 0, 144, this.clothesColor, false, (float)Game1.pixelZoom, 0f, false);
			if (this.clothesColor.R % 2 == 0 || this.clothesColor.G % 2 == 0 || this.clothesColor.B % 2 == 0)
			{
				this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 4 / 5))), (float)this.GetBoundingBox().Center.Y / 10000f + 1E-06f, 0, 264, this.gogglesColor, false, (float)Game1.pixelZoom, 0f, false);
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000EBF59 File Offset: 0x000EA159
		public override void update(GameTime time, GameLocation location)
		{
			if (!this.attacking)
			{
				base.update(time, location);
				return;
			}
			this.behaviorAtGameTick(time);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000EBF74 File Offset: 0x000EA174
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (!this.spottedPlayer && this.controller == null)
			{
				if (this.actionGoblin)
				{
					this.actionCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.actionCountdown <= 0)
					{
						if (this.sprite.CurrentFrame >= 16)
						{
							this.actionCountdown = Game1.random.Next(500, 2000);
							this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 16) * 4;
							this.sprite.UpdateSourceRect();
						}
						else
						{
							this.sprite.CurrentFrame = 16 + ((this.facingDirection % 2 == 0) ? ((this.facingDirection + 2) % 4) : this.facingDirection);
							this.sprite.UpdateSourceRect();
							this.actionCountdown = Game1.random.Next(100, 500);
							if (Utility.isOnScreen(this.position, Game1.tileSize * 4))
							{
								Game1.playSound("hammer");
								Game1.createRadialDebris(Game1.currentLocation, 14, (int)base.getTileLocation().X + ((this.facingDirection == 1) ? 1 : ((this.facingDirection == 3) ? -1 : 0)), (int)base.getTileLocation().Y + ((this.facingDirection == 0) ? -1 : ((this.facingDirection == 2) ? 1 : 0)), Game1.random.Next(4, 6), false, -1, false, -1);
							}
						}
					}
				}
				else if (!this.hallway.Equals(Rectangle.Empty))
				{
					if (this.hallway.Contains(this.GetBoundingBox().Center))
					{
						if (this.hallway.Height > this.hallway.Width && (this.facingDirection == 1 || this.facingDirection == 3))
						{
							this.facingDirection = ((Game1.random.NextDouble() < 0.5) ? 0 : 2);
						}
						else if (this.hallway.Width > this.hallway.Height && (this.facingDirection == 0 || this.facingDirection == 2))
						{
							this.facingDirection = ((Game1.random.NextDouble() < 0.5) ? 1 : 3);
						}
						base.setMovingInFacingDirection();
					}
					else
					{
						this.controller = new PathFindController(this, Game1.currentLocation, new Point(this.hallway.Center.X / Game1.tileSize, this.hallway.Center.Y / Game1.tileSize), Game1.random.Next(4));
					}
					if (Game1.currentLocation.isCollidingPosition(this.nextPosition(this.facingDirection), Game1.viewport, false, 0, false, this))
					{
						this.faceDirection((this.facingDirection + 2) % 4);
						base.setMovingInFacingDirection();
						this.MovePosition(time, Game1.viewport, Game1.currentLocation);
						this.MovePosition(time, Game1.viewport, Game1.currentLocation);
					}
				}
				else if (Game1.random.NextDouble() < 0.0025)
				{
					this.tryToMeetUpWithNeighbor();
				}
				else
				{
					Game1.random.NextDouble();
				}
			}
			if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision(this) && Utility.doesPointHaveLineOfSightInMine(base.getTileLocation(), Game1.player.getTileLocation(), 8))
			{
				this.controller = null;
				this.spottedPlayer = true;
				this.Halt();
				base.facePlayer(Game1.player);
				base.doEmote(16, false);
				Game1.playSound("goblinSpot");
				base.IsWalkingTowardPlayer = true;
				this.actionGoblin = false;
				return;
			}
			if ((this.spottedPlayer && base.withinPlayerThreshold(13)) || this.attacking)
			{
				if (base.withinPlayerThreshold(2) || this.attacking)
				{
					if (!this.attacking && Game1.random.NextDouble() < 0.04)
					{
						this.actionCountdown = 200;
						this.attacking = true;
						Game1.playSound("daggerswipe");
						Vector2 tileLocation = Vector2.Zero;
						Vector2 tileLocation2 = Vector2.Zero;
						Rectangle areaOfEffect = this.weapon.getAreaOfEffect((int)this.GetToolLocation(false).X, (int)this.GetToolLocation(false).Y, base.getFacingDirection(), ref tileLocation, ref tileLocation2, this.GetBoundingBox(), 0);
						if (this.facingDirection == 1 || this.facingDirection == 3)
						{
							areaOfEffect.Inflate(-12, -12);
						}
						if (areaOfEffect.Intersects(Game1.player.GetBoundingBox()))
						{
							int farmerHealth = Game1.player.health;
							Game1.farmerTakeDamage(Game1.random.Next(this.weapon.minDamage, this.weapon.maxDamage + 1), false, this);
							if (Game1.player.health == farmerHealth)
							{
								base.setTrajectory(Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox()) / 2f);
								return;
							}
						}
					}
					else if (this.attacking)
					{
						this.Halt();
						int startingFrame = 16 + ((this.facingDirection == 0) ? 2 : ((this.facingDirection == 2) ? 0 : this.facingDirection));
						this.sprite.CurrentFrame = startingFrame;
						this.actionCountdown -= time.ElapsedGameTime.Milliseconds;
						if (this.actionCountdown <= 0)
						{
							this.attacking = false;
							this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 16) * 4;
							return;
						}
					}
				}
			}
			else if (this.spottedPlayer)
			{
				base.IsWalkingTowardPlayer = false;
				this.spottedPlayer = false;
				this.controllerCountdown = 0;
				this.controller = null;
				this.Halt();
				this.tryToMeetUpWithNeighbor();
				this.addedSpeed = 0;
				base.doEmote(8, false);
			}
		}

		// Token: 0x04000B99 RID: 2969
		public const int durationOfDaggerThrust = 200;

		// Token: 0x04000B9A RID: 2970
		public const int huntForPlayerUpdateTicks = 2000;

		// Token: 0x04000B9B RID: 2971
		public const int distanceToStopChasing = 13;

		// Token: 0x04000B9C RID: 2972
		public const int visionDistance = 8;

		// Token: 0x04000B9D RID: 2973
		public Rectangle hallway = Rectangle.Empty;

		// Token: 0x04000B9E RID: 2974
		private bool spottedPlayer;

		// Token: 0x04000B9F RID: 2975
		private bool actionGoblin;

		// Token: 0x04000BA0 RID: 2976
		private bool attacking;

		// Token: 0x04000BA1 RID: 2977
		private int controllerCountdown;

		// Token: 0x04000BA2 RID: 2978
		private int actionCountdown;

		// Token: 0x04000BA3 RID: 2979
		private MeleeWeapon weapon = new MeleeWeapon((Game1.random.NextDouble() < 0.01) ? 22 : 16);

		// Token: 0x04000BA4 RID: 2980
		private Color clothesColor;

		// Token: 0x04000BA5 RID: 2981
		private Color gogglesColor;

		// Token: 0x04000BA6 RID: 2982
		private Color skinColor;
	}
}
