using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	// Token: 0x020000C2 RID: 194
	public class Monster : NPC
	{
		// Token: 0x06000C40 RID: 3136 RVA: 0x000F3DF2 File Offset: 0x000F1FF2
		public Monster()
		{
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000F3E17 File Offset: 0x000F2017
		public Monster(string name, Vector2 position) : this(name, position, 2)
		{
			this.breather = false;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000F3E29 File Offset: 0x000F2029
		public virtual List<Item> getExtraDropItems()
		{
			return new List<Item>();
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000F3E30 File Offset: 0x000F2030
		public override bool withinPlayerThreshold()
		{
			return this.focusedOnFarmers || base.withinPlayerThreshold(this.moveTowardPlayerThreshold);
		}

		// Token: 0x170000D5 RID: 213
		public override bool IsMonster
		{
			// Token: 0x06000C44 RID: 3140 RVA: 0x0000846E File Offset: 0x0000666E
			get
			{
				return true;
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000F3E48 File Offset: 0x000F2048
		public Monster(string name, Vector2 position, int facingDir) : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + name)), position, facingDir, name, null)
		{
			this.parseMonsterInfo(name);
			this.breather = false;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void drawAboveAllLayers(SpriteBatch b)
		{
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x000F3EA4 File Offset: 0x000F20A4
		public override void draw(SpriteBatch b)
		{
			if (!this.isGlider)
			{
				base.draw(b);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000F3EB5 File Offset: 0x000F20B5
		public bool isInvincible()
		{
			return this.invincibleCountdown > 0;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000F3EC0 File Offset: 0x000F20C0
		public void setInvincibleCountdown(int time)
		{
			this.invincibleCountdown = time;
			base.startGlowing(new Color(255, 0, 0), false, 0.25f);
			this.glowingTransparency = 1f;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000F3EEC File Offset: 0x000F20EC
		protected void parseMonsterInfo(string name)
		{
			string[] monsterInfo = Game1.content.Load<Dictionary<string, string>>("Data\\Monsters")[name].Split(new char[]
			{
				'/'
			});
			this.health = Convert.ToInt32(monsterInfo[0]);
			this.maxHealth = this.health;
			this.damageToFarmer = Convert.ToInt32(monsterInfo[1]);
			this.coinsToDrop = Game1.random.Next(Convert.ToInt32(monsterInfo[2]), Convert.ToInt32(monsterInfo[3]) + 1);
			this.isGlider = Convert.ToBoolean(monsterInfo[4]);
			this.durationOfRandomMovements = Convert.ToInt32(monsterInfo[5]);
			string[] objectsSplit = monsterInfo[6].Split(new char[]
			{
				' '
			});
			this.objectsToDrop.Clear();
			for (int i = 0; i < objectsSplit.Length; i += 2)
			{
				if (Game1.random.NextDouble() < Convert.ToDouble(objectsSplit[i + 1]))
				{
					this.objectsToDrop.Add(Convert.ToInt32(objectsSplit[i]));
				}
			}
			this.resilience = Convert.ToInt32(monsterInfo[7]);
			this.jitteriness = Convert.ToDouble(monsterInfo[8]);
			this.willDestroyObjectsUnderfoot = false;
			base.moveTowardPlayer(Convert.ToInt32(monsterInfo[9]));
			this.speed = Convert.ToInt32(monsterInfo[10]);
			this.missChance = Convert.ToDouble(monsterInfo[11]);
			this.mineMonster = Convert.ToBoolean(monsterInfo[12]);
			if (Game1.player.timesReachedMineBottom >= 1 && this.mineMonster)
			{
				this.resilience *= 2;
				if (Game1.random.NextDouble() < 0.1)
				{
					this.addedSpeed = 1;
				}
				this.missChance *= 2.0;
				this.health += Game1.random.Next(0, this.health);
				this.damageToFarmer += Game1.random.Next(0, this.damageToFarmer);
				this.coinsToDrop += Game1.random.Next(0, this.coinsToDrop + 1);
				if (Game1.random.NextDouble() < 0.008)
				{
					this.objectsToDrop.Add((Game1.random.NextDouble() < 0.5) ? 72 : 74);
				}
			}
			try
			{
				this.experienceGained = Convert.ToInt32(monsterInfo[13]);
			}
			catch (Exception)
			{
				this.experienceGained = 1;
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x000F4158 File Offset: 0x000F2358
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + this.name), 0, 16, 16);
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000F4184 File Offset: 0x000F2384
		public virtual void shedChunks(int number)
		{
			this.shedChunks(number, 0.75f);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000F4194 File Offset: 0x000F2394
		public virtual void shedChunks(int number, float scale)
		{
			if (this.sprite.Texture.Height > this.sprite.getHeight() * 4)
			{
				Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, this.sprite.getHeight() * 4 + 16, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, 1f * (float)Game1.pixelZoom * scale);
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000F4236 File Offset: 0x000F2436
		public virtual void deathAnimation()
		{
			this.shedChunks(Game1.random.Next(4, 9), 0.75f);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x000F4250 File Offset: 0x000F2450
		public virtual int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			return this.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision, "hitEnemy");
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x000F4264 File Offset: 0x000F2464
		public int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, string hitSound)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			this.slideAnimationTimer = 0;
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				Game1.playSound(hitSound);
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000F42D8 File Offset: 0x000F24D8
		public virtual void behaviorAtGameTick(GameTime time)
		{
			if (this.timeBeforeAIMovementAgain > 0f)
			{
				this.timeBeforeAIMovementAgain -= (float)time.ElapsedGameTime.Milliseconds;
			}
			if (Game1.player.isRafting && base.withinPlayerThreshold(4))
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
				this.MovePosition(time, Game1.viewport, Game1.currentLocation);
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool passThroughCharacters()
		{
			return false;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000F43F4 File Offset: 0x000F25F4
		public override void update(GameTime time, GameLocation location)
		{
			if (this.invincibleCountdown > 0)
			{
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			if (!Game1.player.isRafting || !base.withinPlayerThreshold(4))
			{
				base.update(time, location);
			}
			this.behaviorAtGameTick(time);
			if (this.controller != null && base.withinPlayerThreshold(3))
			{
				this.controller = null;
			}
			if (!this.isGlider && (this.position.X < 0f || this.position.X > (float)(location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || this.position.Y < 0f || this.position.Y > (float)(location.map.GetLayer("Back").LayerHeight * Game1.tileSize)))
			{
				location.characters.Remove(this);
				return;
			}
			if (this.isGlider && this.position.X < -2000f)
			{
				this.health = -500;
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000F4534 File Offset: 0x000F2734
		private bool doHorizontalMovement(GameLocation location)
		{
			bool wasAbleToMoveHorizontally = false;
			if (this.position.X > Game1.player.position.X + (float)(Game1.pixelZoom * 2) || (this.skipHorizontal > 0 && Game1.player.getStandingX() < base.getStandingX() - Game1.pixelZoom * 2))
			{
				base.SetMovingOnlyLeft();
				if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					wasAbleToMoveHorizontally = true;
				}
				else
				{
					this.faceDirection(3);
					if (this.durationOfRandomMovements > 0 && Game1.random.NextDouble() < this.jitteriness)
					{
						if (Game1.random.NextDouble() < 0.5)
						{
							base.tryToMoveInDirection(2, false, this.damageToFarmer, this.isGlider);
						}
						else
						{
							base.tryToMoveInDirection(0, false, this.damageToFarmer, this.isGlider);
						}
						this.timeBeforeAIMovementAgain = (float)this.durationOfRandomMovements;
					}
				}
			}
			else if (this.position.X < Game1.player.position.X - (float)(Game1.pixelZoom * 2))
			{
				base.SetMovingOnlyRight();
				if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					wasAbleToMoveHorizontally = true;
				}
				else
				{
					this.faceDirection(1);
					if (this.durationOfRandomMovements > 0 && Game1.random.NextDouble() < this.jitteriness)
					{
						if (Game1.random.NextDouble() < 0.5)
						{
							base.tryToMoveInDirection(2, false, this.damageToFarmer, this.isGlider);
						}
						else
						{
							base.tryToMoveInDirection(0, false, this.damageToFarmer, this.isGlider);
						}
						this.timeBeforeAIMovementAgain = (float)this.durationOfRandomMovements;
					}
				}
			}
			else
			{
				base.faceGeneralDirection(Game1.player.getStandingPosition(), 0);
				base.setMovingInFacingDirection();
				this.skipHorizontal = 500;
			}
			return wasAbleToMoveHorizontally;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000F4748 File Offset: 0x000F2948
		private void checkHorizontalMovement(ref bool success, ref bool setMoving, ref bool scootSuccess, Farmer who, GameLocation location)
		{
			if (who.position.X > this.position.X + (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyRight();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
				}
				else
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					if (!this.position.Equals(this.lastPosition))
					{
						scootSuccess = true;
					}
				}
			}
			if (!success && who.position.X < this.position.X - (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyLeft();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
					return;
				}
				this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
				if (!this.position.Equals(this.lastPosition))
				{
					scootSuccess = true;
				}
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000F4850 File Offset: 0x000F2A50
		private void checkVerticalMovement(ref bool success, ref bool setMoving, ref bool scootSuccess, Farmer who, GameLocation location)
		{
			if (!success && who.position.Y < this.position.Y - (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyUp();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
				}
				else
				{
					this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
					if (!this.position.Equals(this.lastPosition))
					{
						scootSuccess = true;
					}
				}
			}
			if (!success && who.position.Y > this.position.Y + (float)(Game1.tileSize / 4))
			{
				base.SetMovingOnlyDown();
				setMoving = true;
				if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					success = true;
					return;
				}
				this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
				if (!this.position.Equals(this.lastPosition))
				{
					scootSuccess = true;
				}
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000F495C File Offset: 0x000F2B5C
		public override void updateMovement(GameLocation location, GameTime time)
		{
			if (base.IsWalkingTowardPlayer)
			{
				if ((this.moveTowardPlayerThreshold == -1 || this.withinPlayerThreshold()) && this.timeBeforeAIMovementAgain <= 0f && this.IsMonster && !this.isGlider && !location.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].Properties.ContainsKey("NPCBarrier"))
				{
					if (this.skipHorizontal <= 0)
					{
						Farmer who = Utility.getNearestFarmerInCurrentLocation(base.getTileLocation());
						if (this.lastPosition.Equals(this.position) && Game1.random.NextDouble() < 0.001)
						{
							switch (this.facingDirection)
							{
							case 0:
							case 2:
								if (Game1.random.NextDouble() < 0.5)
								{
									base.SetMovingOnlyRight();
								}
								else
								{
									base.SetMovingOnlyLeft();
								}
								break;
							case 1:
							case 3:
								if (Game1.random.NextDouble() < 0.5)
								{
									base.SetMovingOnlyUp();
								}
								else
								{
									base.SetMovingOnlyDown();
								}
								break;
							}
							this.skipHorizontal = 700;
							return;
						}
						bool success = false;
						bool setMoving = false;
						bool scootSuccess = false;
						if (this.lastPosition.X == this.position.X)
						{
							this.checkHorizontalMovement(ref success, ref setMoving, ref scootSuccess, who, location);
							this.checkVerticalMovement(ref success, ref setMoving, ref scootSuccess, who, location);
						}
						else
						{
							this.checkVerticalMovement(ref success, ref setMoving, ref scootSuccess, who, location);
							this.checkHorizontalMovement(ref success, ref setMoving, ref scootSuccess, who, location);
						}
						if (!success && !setMoving)
						{
							this.Halt();
							base.faceGeneralDirection(who.getStandingPosition(), 0);
						}
						if (success)
						{
							this.skipHorizontal = 500;
						}
						if (scootSuccess)
						{
							return;
						}
					}
					else
					{
						this.skipHorizontal -= time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else
			{
				this.defaultMovementBehavior(time);
			}
			this.MovePosition(time, Game1.viewport, location);
			if (this.position.Equals(this.lastPosition) && base.IsWalkingTowardPlayer && this.withinPlayerThreshold())
			{
				this.noMovementProgressNearPlayerBehavior();
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000F4B92 File Offset: 0x000F2D92
		public virtual void noMovementProgressNearPlayerBehavior()
		{
			this.Halt();
			base.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(base.getTileLocation()).getStandingPosition(), 0);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x000F4BB4 File Offset: 0x000F2DB4
		public virtual void defaultMovementBehavior(GameTime time)
		{
			if (Game1.random.NextDouble() < this.jitteriness * 1.8 && this.skipHorizontal <= 0)
			{
				switch (Game1.random.Next(6))
				{
				case 0:
					base.SetMovingOnlyUp();
					return;
				case 1:
					base.SetMovingOnlyRight();
					return;
				case 2:
					base.SetMovingOnlyDown();
					return;
				case 3:
					base.SetMovingOnlyLeft();
					return;
				default:
					this.Halt();
					break;
				}
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x000F4C2C File Offset: 0x000F2E2C
		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			this.lastPosition = this.position;
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
				{
					this.xVelocity = 0f;
					this.yVelocity = 0f;
				}
				Microsoft.Xna.Framework.Rectangle nextPosition = this.GetBoundingBox();
				nextPosition.X += (int)this.xVelocity;
				nextPosition.Y -= (int)this.yVelocity;
				if (!currentLocation.isCollidingPosition(nextPosition, viewport, false, this.damageToFarmer, this.isGlider, this))
				{
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
					if (this.slipperiness < 1000)
					{
						this.xVelocity -= this.xVelocity / (float)this.slipperiness;
						this.yVelocity -= this.yVelocity / (float)this.slipperiness;
						if (Math.Abs(this.xVelocity) <= 0.05f)
						{
							this.xVelocity = 0f;
						}
						if (Math.Abs(this.yVelocity) <= 0.05f)
						{
							this.yVelocity = 0f;
						}
					}
					if (!this.isGlider && this.invincibleCountdown > 0)
					{
						this.slideAnimationTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.slideAnimationTimer < 0 && (Math.Abs(this.xVelocity) >= 3f || Math.Abs(this.yVelocity) >= 3f))
						{
							this.slideAnimationTimer = 100 - (int)(Math.Abs(this.xVelocity) * 2f + Math.Abs(this.yVelocity) * 2f);
							currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, base.getStandingPosition() + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)), Color.White * 0.75f, 8, Game1.random.NextDouble() < 0.5, 20f, 0, -1, -1f, -1, 0)
							{
								scale = 0.75f
							});
						}
					}
				}
				else if (this.isGlider || this.slipperiness >= 8)
				{
					bool[] expr_26C = Utility.horizontalOrVerticalCollisionDirections(nextPosition, this, false);
					if (expr_26C[0])
					{
						this.xVelocity = -this.xVelocity;
						this.position.X = this.position.X + (float)Math.Sign(this.xVelocity);
						this.rotation += (float)(3.1415926535897931 + (double)Game1.random.Next(-10, 11) * 3.1415926535897931 / 500.0);
					}
					if (expr_26C[1])
					{
						this.yVelocity = -this.yVelocity;
						this.position.Y = this.position.Y - (float)Math.Sign(this.yVelocity);
						this.rotation += (float)(3.1415926535897931 + (double)Game1.random.Next(-10, 11) * 3.1415926535897931 / 500.0);
					}
					if (this.slipperiness < 1000)
					{
						this.xVelocity -= this.xVelocity / (float)this.slipperiness / 4f;
						this.yVelocity -= this.yVelocity / (float)this.slipperiness / 4f;
						if (Math.Abs(this.xVelocity) <= 0.05f)
						{
							this.xVelocity = 0f;
						}
						if (Math.Abs(this.yVelocity) <= 0.051f)
						{
							this.yVelocity = 0f;
						}
					}
				}
				else
				{
					this.xVelocity -= this.xVelocity / (float)this.slipperiness;
					this.yVelocity -= this.yVelocity / (float)this.slipperiness;
					if (Math.Abs(this.xVelocity) <= 0.05f)
					{
						this.xVelocity = 0f;
					}
					if (Math.Abs(this.yVelocity) <= 0.05f)
					{
						this.yVelocity = 0f;
					}
				}
				if (this.isGlider)
				{
					return;
				}
			}
			if (this.moveUp)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateUp(time, 0, "");
					}
					this.facingDirection = 0;
					this.faceDirection(0);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle tmp = this.nextPosition(0);
					tmp.Width /= 4;
					bool leftCorner = currentLocation.isCollidingPosition(tmp, viewport, false, this.damageToFarmer, this.isGlider, this);
					tmp.X += tmp.Width * 3;
					bool rightCorner = currentLocation.isCollidingPosition(tmp, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (leftCorner && !rightCorner && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (rightCorner && !leftCorner && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize), (float)(base.getStandingY() / Game1.tileSize - 1));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(0), true))
						{
							Game1.playSound("stoneCrack");
							this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (this.moveRight)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateRight(time, 0, "");
					}
					this.facingDirection = 1;
					this.faceDirection(1);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle tmp2 = this.nextPosition(1);
					tmp2.Height /= 4;
					bool topCorner = currentLocation.isCollidingPosition(tmp2, viewport, false, this.damageToFarmer, this.isGlider, this);
					tmp2.Y += tmp2.Height * 3;
					bool bottomCorner = currentLocation.isCollidingPosition(tmp2, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (topCorner && !bottomCorner && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (bottomCorner && !topCorner && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize + 1), (float)(base.getStandingY() / Game1.tileSize));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(1), true))
						{
							Game1.playSound("stoneCrack");
							this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (this.moveDown)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateDown(time, 0, "");
					}
					this.facingDirection = 2;
					this.faceDirection(2);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle tmp3 = this.nextPosition(2);
					tmp3.Width /= 4;
					bool leftCorner2 = currentLocation.isCollidingPosition(tmp3, viewport, false, this.damageToFarmer, this.isGlider, this);
					tmp3.X += tmp3.Width * 3;
					bool rightCorner2 = currentLocation.isCollidingPosition(tmp3, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (leftCorner2 && !rightCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (rightCorner2 && !leftCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize), (float)(base.getStandingY() / Game1.tileSize + 1));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(2), true))
						{
							Game1.playSound("stoneCrack");
							this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (this.moveLeft)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, this) || this.isCharging)
				{
					this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					this.facingDirection = 3;
					if (!this.ignoreMovementAnimations)
					{
						this.sprite.AnimateLeft(time, 0, "");
					}
					this.faceDirection(3);
				}
				else
				{
					Microsoft.Xna.Framework.Rectangle tmp4 = this.nextPosition(3);
					tmp4.Height /= 4;
					bool topCorner2 = currentLocation.isCollidingPosition(tmp4, viewport, false, this.damageToFarmer, this.isGlider, this);
					tmp4.Y += tmp4.Height * 3;
					bool bottomCorner2 = currentLocation.isCollidingPosition(tmp4, viewport, false, this.damageToFarmer, this.isGlider, this);
					if (topCorner2 && !bottomCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					else if (bottomCorner2 && !topCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, this))
					{
						this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
					}
					if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
					{
						this.Halt();
					}
					else if (this.willDestroyObjectsUnderfoot)
					{
						new Vector2((float)(base.getStandingX() / Game1.tileSize - 1), (float)(base.getStandingY() / Game1.tileSize));
						if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(3), true))
						{
							Game1.playSound("stoneCrack");
							this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
						}
						else
						{
							this.blockedInterval += time.ElapsedGameTime.Milliseconds;
						}
					}
					if (this.onCollision != null)
					{
						this.onCollision(currentLocation);
					}
				}
			}
			else if (!this.ignoreMovementAnimations)
			{
				if (this.moveUp)
				{
					this.sprite.AnimateUp(time, 0, "");
				}
				else if (this.moveRight)
				{
					this.sprite.AnimateRight(time, 0, "");
				}
				else if (this.moveDown)
				{
					this.sprite.AnimateDown(time, 0, "");
				}
				else if (this.moveLeft)
				{
					this.sprite.AnimateLeft(time, 0, "");
				}
			}
			if (!this.ignoreMovementAnimations)
			{
				this.sprite.interval = (float)this.defaultAnimationInterval - (float)(this.speed + this.addedSpeed - 2) * 20f;
			}
			if ((this.blockedInterval < 3000 || (float)this.blockedInterval > 3750f) && this.blockedInterval >= 5000)
			{
				this.speed = 4;
				this.isCharging = true;
				this.blockedInterval = 0;
			}
			if (this.damageToFarmer > 0 && Game1.random.NextDouble() < 0.00033333333333333332)
			{
				if (this.name.Equals("Shadow Guy") && Game1.random.NextDouble() < 0.3)
				{
					if (Game1.random.NextDouble() < 0.5)
					{
						Game1.playSound("grunt");
						return;
					}
					Game1.playSound("shadowpeep");
					return;
				}
				else if (!this.name.Equals("Shadow Girl"))
				{
					if (this.name.Equals("Ghost"))
					{
						Game1.playSound("ghost");
						return;
					}
					if (!this.name.Contains("Slime"))
					{
						this.name.Contains("Jelly");
					}
				}
			}
		}

		// Token: 0x04000BF2 RID: 3058
		public const int defaultInvincibleCountdown = 450;

		// Token: 0x04000BF3 RID: 3059
		public const int seekPlayerIterationLimit = 80;

		// Token: 0x04000BF4 RID: 3060
		public int damageToFarmer;

		// Token: 0x04000BF5 RID: 3061
		public int health;

		// Token: 0x04000BF6 RID: 3062
		public int maxHealth;

		// Token: 0x04000BF7 RID: 3063
		public int coinsToDrop;

		// Token: 0x04000BF8 RID: 3064
		public int durationOfRandomMovements;

		// Token: 0x04000BF9 RID: 3065
		public int resilience;

		// Token: 0x04000BFA RID: 3066
		public int slipperiness = 2;

		// Token: 0x04000BFB RID: 3067
		public int experienceGained;

		// Token: 0x04000BFC RID: 3068
		public double jitteriness;

		// Token: 0x04000BFD RID: 3069
		public double missChance;

		// Token: 0x04000BFE RID: 3070
		public bool isGlider;

		// Token: 0x04000BFF RID: 3071
		public bool mineMonster;

		// Token: 0x04000C00 RID: 3072
		public bool hasSpecialItem;

		// Token: 0x04000C01 RID: 3073
		public List<int> objectsToDrop = new List<int>();

		// Token: 0x04000C02 RID: 3074
		protected int skipHorizontal;

		// Token: 0x04000C03 RID: 3075
		protected int invincibleCountdown;

		// Token: 0x04000C04 RID: 3076
		private bool skipHorizontalUp;

		// Token: 0x04000C05 RID: 3077
		protected int defaultAnimationInterval = 175;

		// Token: 0x04000C06 RID: 3078
		[XmlIgnore]
		public bool focusedOnFarmers;

		// Token: 0x04000C07 RID: 3079
		[XmlIgnore]
		public bool wildernessFarmMonster;

		// Token: 0x04000C08 RID: 3080
		protected Monster.collisionBehavior onCollision;

		// Token: 0x04000C09 RID: 3081
		private int slideAnimationTimer;

		// Token: 0x02000176 RID: 374
		// Token: 0x0600139D RID: 5021
		protected delegate void collisionBehavior(GameLocation location);
	}
}
