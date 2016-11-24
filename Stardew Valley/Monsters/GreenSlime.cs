using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000C1 RID: 193
	public class GreenSlime : Monster
	{
		// Token: 0x06000C2E RID: 3118 RVA: 0x000F1623 File Offset: 0x000EF823
		public GreenSlime()
		{
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x000F1644 File Offset: 0x000EF844
		public GreenSlime(Vector2 position) : base("Green Slime", position)
		{
			if (Game1.random.NextDouble() < 0.5)
			{
				this.leftDrift = true;
			}
			this.slipperiness = 4;
			this.readyToMate = Game1.random.Next(1000, 120000);
			int green = Game1.random.Next(200, 256);
			this.color = new Color(green / Game1.random.Next(2, 10), Game1.random.Next(180, 256), (Game1.random.NextDouble() < 0.1) ? 255 : (255 - green));
			this.firstGeneration = true;
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.cute = (Game1.random.NextDouble() < 0.49);
			this.hideShadow = true;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000F1760 File Offset: 0x000EF960
		public GreenSlime(Vector2 position, int mineLevel) : base("Green Slime", position)
		{
			this.cute = (Game1.random.NextDouble() < 0.49);
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.specialNumber = Game1.random.Next(100);
			if (mineLevel < 40)
			{
				base.parseMonsterInfo("Green Slime");
				int green = Game1.random.Next(200, 256);
				this.color = new Color(green / Game1.random.Next(2, 10), green, (Game1.random.NextDouble() < 0.01) ? 255 : (255 - green));
				if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
				{
					this.color = new Color(205, 255, 0) * 0.7f;
					this.hasSpecialItem = true;
					this.health *= 3;
					this.damageToFarmer *= 2;
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(680);
				}
			}
			else if (mineLevel < 80)
			{
				this.name = "Frost Jelly";
				base.parseMonsterInfo("Frost Jelly");
				int blue = Game1.random.Next(200, 256);
				this.color = new Color((Game1.random.NextDouble() < 0.01) ? 180 : (blue / Game1.random.Next(2, 10)), (Game1.random.NextDouble() < 0.1) ? 255 : (255 - blue / 3), blue);
				if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
				{
					this.color = new Color(0, 0, 0) * 0.7f;
					this.hasSpecialItem = true;
					this.health *= 3;
					this.damageToFarmer *= 2;
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(413);
				}
			}
			else if (mineLevel > 120)
			{
				this.name = "Sludge";
				base.parseMonsterInfo("Sludge");
				this.color = Color.BlueViolet;
				this.health *= 2;
				int r = (int)this.color.R;
				int g = (int)this.color.G;
				int b = (int)this.color.B;
				r += Game1.random.Next(-20, 21);
				g += Game1.random.Next(-20, 21);
				b += Game1.random.Next(-20, 21);
				this.color.R = (byte)Math.Max(Math.Min(255, r), 0);
				this.color.G = (byte)Math.Max(Math.Min(255, g), 0);
				this.color.B = (byte)Math.Max(Math.Min(255, b), 0);
				while (Game1.random.NextDouble() < 0.08)
				{
					this.objectsToDrop.Add(386);
				}
				if (Game1.random.NextDouble() < 0.009)
				{
					this.objectsToDrop.Add(337);
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(439);
				}
			}
			else
			{
				this.name = "Sludge";
				base.parseMonsterInfo("Sludge");
				int green2 = Game1.random.Next(200, 256);
				this.color = new Color(green2, (Game1.random.NextDouble() < 0.01) ? 255 : (255 - green2), green2 / Game1.random.Next(2, 10));
				if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
				{
					this.color = new Color(50, 10, 50) * 0.7f;
					this.hasSpecialItem = true;
					this.health *= 3;
					this.damageToFarmer *= 2;
				}
				if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
				{
					this.objectsToDrop.Add(437);
				}
			}
			if (this.cute)
			{
				this.health += this.health / 4;
				this.damageToFarmer++;
			}
			if (Game1.random.NextDouble() < 0.5)
			{
				this.leftDrift = true;
			}
			this.slipperiness = 3;
			this.readyToMate = Game1.random.Next(1000, 120000);
			if (Game1.random.NextDouble() < 0.001)
			{
				this.color = new Color(255, 255, 50);
				this.coinsToDrop = 10;
			}
			this.firstGeneration = true;
			this.hideShadow = true;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000F1D44 File Offset: 0x000EFF44
		public GreenSlime(Vector2 position, Color color) : base("Green Slime", position)
		{
			this.color = color;
			this.firstGeneration = true;
			this.hideShadow = true;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000F1D80 File Offset: 0x000EFF80
		public override void reloadSprite()
		{
			this.hideShadow = true;
			string tmp = this.name;
			this.name = "Green Slime";
			base.reloadSprite();
			this.name = tmp;
			this.sprite.spriteHeight = 24;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000F1DCC File Offset: 0x000EFFCC
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if (Game1.random.NextDouble() < 0.025 && this.cute)
				{
					if (!this.focusedOnFarmers)
					{
						this.damageToFarmer += this.damageToFarmer / 2;
						base.shake(1000);
					}
					this.focusedOnFarmers = true;
				}
				this.slipperiness = 3;
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("slimeHit");
				this.readyToJump = -1;
				base.IsWalkingTowardPlayer = true;
				if (this.health <= 0)
				{
					Game1.playSound("slimedead");
					Stats expr_CE = Game1.stats;
					uint slimesKilled = expr_CE.SlimesKilled;
					expr_CE.SlimesKilled = slimesKilled + 1u;
					if (this.mateToPursue != null)
					{
						this.mateToPursue.mateToAvoid = null;
					}
					if (this.mateToAvoid != null)
					{
						this.mateToAvoid.mateToPursue = null;
					}
					if (Game1.gameMode == 3 && this.scale > 1.8f)
					{
						this.health = 10;
						int toCreate = (this.scale > 1.8f) ? Game1.random.Next(3, 5) : 1;
						this.scale *= 0.6666667f;
						for (int i = 0; i < toCreate; i++)
						{
							Game1.currentLocation.characters.Add(new GreenSlime(this.position + new Vector2((float)(i * this.GetBoundingBox().Width), 0f), Game1.mine.mineLevel));
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].setTrajectory(xTrajectory + Game1.random.Next(-20, 20), yTrajectory + Game1.random.Next(-20, 20));
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].willDestroyObjectsUnderfoot = false;
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].moveTowardPlayer(4);
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].scale = 0.75f + (float)Game1.random.Next(-5, 10) / 100f;
						}
					}
					else
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							delayBeforeAnimationStart = 0,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2(0f, (float)(Game1.tileSize / 4)), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							delayBeforeAnimationStart = 100,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)(Game1.tileSize / 4), 0f), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
						{
							interval = 70f,
							delayBeforeAnimationStart = 200,
							holdLastFrame = true,
							alphaFade = 0.01f
						});
					}
				}
			}
			return actualDamage;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x000F2224 File Offset: 0x000F0424
		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 120, 16, 16), 8, this.GetBoundingBox().Center.X + Game1.tileSize / 2, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, this.color, 4f * scale);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000F229C File Offset: 0x000F049C
		public override void collisionWithFarmerBehavior()
		{
			if (Game1.random.NextDouble() < 0.3 && !Game1.player.temporarilyInvincible && !Game1.player.isWearingRing(520) && Game1.buffsDisplay.addOtherBuff(new Buff(13)))
			{
				Game1.playSound("slime");
			}
			this.farmerPassesThrough = Game1.player.isWearingRing(520);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000F2310 File Offset: 0x000F0510
		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yOffset)), new Rectangle?(base.Sprite.SourceRect), this.color, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (this.ageUntilFullGrown <= 0)
				{
					if (this.cute || this.hasSpecialItem)
					{
						int xDongleSource = (this.isMoving() || this.wagTimer > 0) ? (16 * Math.Min(7, Math.Abs(((this.wagTimer > 0) ? (992 - this.wagTimer) : (Game1.currentGameTime.TotalGameTime.Milliseconds % 992)) - 496) / 62) % 64) : 48;
						int yDongleSource = (this.isMoving() || this.wagTimer > 0) ? (24 * Math.Min(1, Math.Max(1, Math.Abs(((this.wagTimer > 0) ? (992 - this.wagTimer) : (Game1.currentGameTime.TotalGameTime.Milliseconds % 992)) - 496) / 62) / 4)) : 24;
						if (this.hasSpecialItem)
						{
							yDongleSource += 48;
						}
						b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height - Game1.pixelZoom * 4 + ((this.readyToJump <= 0) ? (Game1.pixelZoom * (-2 + Math.Abs(this.sprite.currentFrame % 4 - 2))) : (Game1.pixelZoom + Game1.pixelZoom * (this.sprite.currentFrame % 4 % 3))) + this.yOffset)) * this.scale, new Rectangle?(new Rectangle(xDongleSource, 168 + yDongleSource, 16, 24)), this.hasSpecialItem ? Color.White : this.color, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.0001f)));
					}
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + (new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + ((this.readyToJump <= 0) ? (Game1.pixelZoom * (-2 + Math.Abs(this.sprite.currentFrame % 4 - 2))) : (Game1.pixelZoom - Game1.pixelZoom * (this.sprite.currentFrame % 4 % 3))) + this.yOffset)) + this.facePosition) * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), new Rectangle?(new Rectangle(32 + ((this.readyToJump > 0 || this.focusedOnFarmers) ? 16 : 0), 120 + ((this.readyToJump < 0 && (this.focusedOnFarmers || this.invincibleCountdown > 0)) ? 24 : 0), 16, 24)), Color.White * ((this.facingDirection == 0) ? 0.5f : 1f), 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale - 0.4f * ((float)this.ageUntilFullGrown / 120000f)), SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.0001f)));
				}
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yOffset)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale), SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
				if (this.mateToPursue != null)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2 + this.yOffset)), new Rectangle?(new Rectangle(16, 120, 8, 8)), Color.White, 0f, new Vector2(3f, 3f), (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				}
				else if (this.mateToAvoid != null)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2 + this.yOffset)), new Rectangle?(new Rectangle(24, 120, 8, 8)), Color.White, 0f, new Vector2(4f, 4f), (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				}
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 * 7) / 4f + (float)this.yOffset + (float)(Game1.pixelZoom * 2) * this.scale - (float)((this.ageUntilFullGrown > 0) ? (Game1.pixelZoom * 2) : 0)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + this.scale - (float)this.ageUntilFullGrown / 120000f - ((base.Sprite.CurrentFrame % 4 % 3 != 0) ? 1f : 0f) + (float)this.yOffset / 30f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000F2AD0 File Offset: 0x000F0CD0
		public void moveTowardOtherSlime(GreenSlime other, bool moveAway, GameTime time)
		{
			int xToGo = Math.Abs(other.getStandingX() - base.getStandingX());
			int yToGo = Math.Abs(other.getStandingY() - base.getStandingY());
			if (xToGo > 4 || yToGo > 4)
			{
				int dx = (other.getStandingX() > base.getStandingX()) ? 1 : -1;
				int dy = (other.getStandingY() > base.getStandingY()) ? 1 : -1;
				if (moveAway)
				{
					dx = -dx;
					dy = -dy;
				}
				double chanceForX = (double)xToGo / (double)(xToGo + yToGo);
				if (Game1.random.NextDouble() < chanceForX)
				{
					base.tryToMoveInDirection((dx > 0) ? 1 : 3, false, this.damageToFarmer, false);
				}
				else
				{
					base.tryToMoveInDirection((dy > 0) ? 2 : 0, false, this.damageToFarmer, false);
				}
			}
			this.sprite.AnimateDown(time, 0, "");
			if (this.invincibleCountdown > 0)
			{
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x000F2BC5 File Offset: 0x000F0DC5
		public void doneMating()
		{
			this.readyToMate = 120000;
			this.matingCountdown = 2000;
			this.mateToPursue = null;
			this.mateToAvoid = null;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x000F2BEB File Offset: 0x000F0DEB
		public override void update(GameTime time, GameLocation location)
		{
			if (this.mateToPursue == null && this.mateToAvoid == null)
			{
				base.update(time, location);
				return;
			}
			if (this.currentLocation == null)
			{
				this.currentLocation = location;
			}
			this.behaviorAtGameTick(time);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x000F2C1C File Offset: 0x000F0E1C
		public override void noMovementProgressNearPlayerBehavior()
		{
			base.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(base.getTileLocation()).getStandingPosition(), 0);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000F2C38 File Offset: 0x000F0E38
		public void mateWith(GreenSlime mateToPursue, GameLocation location)
		{
			if (location.canSlimeMateHere())
			{
				GreenSlime baby = new GreenSlime(Vector2.Zero);
				Utility.recursiveFindPositionForCharacter(baby, location, base.getTileLocation(), 30);
				Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 10) + (uint)((int)(this.scale * 100f)) + (uint)((int)(mateToPursue.scale * 100f))));
				switch (r.Next(4))
				{
				case 0:
					baby.color = new Color(Math.Min(255, Math.Max(0, (int)this.color.R + r.Next((int)((float)(-(float)this.color.R) * 0.25f), (int)((float)this.color.R * 0.25f)))), Math.Min(255, Math.Max(0, (int)this.color.G + r.Next((int)((float)(-(float)this.color.G) * 0.25f), (int)((float)this.color.G * 0.25f)))), Math.Min(255, Math.Max(0, (int)this.color.B + r.Next((int)((float)(-(float)this.color.B) * 0.25f), (int)((float)this.color.B * 0.25f)))));
					break;
				case 1:
				case 2:
					baby.color = Utility.getBlendedColor(this.color, mateToPursue.color);
					break;
				case 3:
					baby.color = new Color(Math.Min(255, Math.Max(0, (int)mateToPursue.color.R + r.Next((int)((float)(-(float)mateToPursue.color.R) * 0.25f), (int)((float)mateToPursue.color.R * 0.25f)))), Math.Min(255, Math.Max(0, (int)mateToPursue.color.G + r.Next((int)((float)(-(float)mateToPursue.color.G) * 0.25f), (int)((float)mateToPursue.color.G * 0.25f)))), Math.Min(255, Math.Max(0, (int)mateToPursue.color.B + r.Next((int)((float)(-(float)mateToPursue.color.B) * 0.25f), (int)((float)mateToPursue.color.B * 0.25f)))));
					break;
				}
				int red = (int)baby.color.R;
				int green = (int)baby.color.G;
				int blue = (int)baby.color.B;
				if (red > 100 && blue > 100 && green < 50)
				{
					baby.parseMonsterInfo("Sludge");
					while (r.NextDouble() < 0.1)
					{
						baby.objectsToDrop.Add(386);
					}
					if (r.NextDouble() < 0.01)
					{
						baby.objectsToDrop.Add(337);
					}
				}
				else if (red >= 200 && green < 75)
				{
					baby.parseMonsterInfo("Sludge");
				}
				else if (blue >= 200 && red < 100)
				{
					baby.parseMonsterInfo("Frost Jelly");
				}
				baby.health = ((r.NextDouble() < 0.5) ? this.health : mateToPursue.health);
				baby.health = Math.Max(1, this.health + r.Next(-4, 5));
				baby.damageToFarmer = ((r.NextDouble() < 0.5) ? this.damageToFarmer : mateToPursue.damageToFarmer);
				baby.damageToFarmer = Math.Max(0, this.damageToFarmer + r.Next(-1, 2));
				baby.resilience = ((r.NextDouble() < 0.5) ? this.resilience : mateToPursue.resilience);
				baby.resilience = Math.Max(0, this.resilience + r.Next(-1, 2));
				baby.missChance = ((r.NextDouble() < 0.5) ? this.missChance : mateToPursue.missChance);
				baby.missChance = Math.Max(0.0, this.missChance + (double)((float)r.Next(-1, 2) / 100f));
				baby.scale = ((r.NextDouble() < 0.5) ? this.scale : mateToPursue.scale);
				baby.scale = Math.Max(0.6f, Math.Min(1.5f, this.scale + (float)r.Next(-2, 3) / 100f));
				baby.slipperiness = 8;
				this.speed = ((r.NextDouble() < 0.5) ? this.speed : mateToPursue.speed);
				if (r.NextDouble() < 0.015)
				{
					this.speed = Math.Max(1, Math.Min(6, this.speed + r.Next(-1, 2)));
				}
				baby.setTrajectory(Utility.getAwayFromPositionTrajectory(baby.GetBoundingBox(), base.getStandingPosition()) / 2f);
				baby.ageUntilFullGrown = 120000;
				baby.Halt();
				baby.firstGeneration = false;
				if (Utility.isOnScreen(this.position, 128))
				{
					Game1.playSound("slime");
				}
			}
			mateToPursue.doneMating();
			this.doneMating();
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x000F3194 File Offset: 0x000F1394
		public override List<Item> getExtraDropItems()
		{
			List<Item> extra = new List<Item>();
			if (this.color.R < 80 && this.color.G < 80 && this.color.B < 80)
			{
				extra.Add(new Object(382, 1, false, -1, 0));
				Random expr_82 = new Random((int)this.position.X * 777 + (int)this.position.Y * 77 + (int)Game1.stats.DaysPlayed);
				if (expr_82.NextDouble() < 0.05)
				{
					extra.Add(new Object(553, 1, false, -1, 0));
				}
				if (expr_82.NextDouble() < 0.05)
				{
					extra.Add(new Object(539, 1, false, -1, 0));
				}
			}
			else if (this.color.R > 200 && this.color.G > 180 && this.color.B < 50)
			{
				extra.Add(new Object(384, 2, false, -1, 0));
			}
			else if (this.color.R > 220 && this.color.G > 90 && this.color.G < 150 && this.color.B < 50)
			{
				extra.Add(new Object(378, 2, false, -1, 0));
			}
			else if (this.color.R > 230 && this.color.G > 230 && this.color.B > 230)
			{
				extra.Add(new Object(380, 1, false, -1, 0));
				if (this.color.R % 2 == 0 && this.color.G % 2 == 0 && this.color.B % 2 == 0)
				{
					extra.Add(new Object(72, 1, false, -1, 0));
				}
			}
			else if (this.color.R > 150 && this.color.G > 150 && this.color.B > 150)
			{
				extra.Add(new Object(390, 2, false, -1, 0));
			}
			else if (this.color.R > 150 && this.color.B > 180 && this.color.G < 50 && this.specialNumber % 4 == 0)
			{
				extra.Add(new Object(386, 2, false, -1, 0));
			}
			if (Game1.player.mailReceived.Contains("slimeHutchBuilt") && this.specialNumber == 1)
			{
				string name = this.name;
				if (!(name == "Green Slime"))
				{
					if (name == "Frost Jelly")
					{
						extra.Add(new Object(413, 1, false, -1, 0));
					}
				}
				else
				{
					extra.Add(new Object(680, 1, false, -1, 0));
				}
			}
			return extra;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x000F34BE File Offset: 0x000F16BE
		public override void dayUpdate(int dayOfMonth)
		{
			if (this.ageUntilFullGrown > 0)
			{
				this.ageUntilFullGrown /= 2;
			}
			if (this.readyToMate > 0)
			{
				this.readyToMate /= 2;
			}
			base.dayUpdate(dayOfMonth);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x000F34F8 File Offset: 0x000F16F8
		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.wagTimer > 0)
			{
				this.wagTimer -= (int)time.ElapsedGameTime.TotalMilliseconds;
			}
			switch (base.FacingDirection)
			{
			case 0:
				if (this.facePosition.X > 0f)
				{
					this.facePosition.X = this.facePosition.X - 2f;
				}
				else if (this.facePosition.X < 0f)
				{
					this.facePosition.X = this.facePosition.X + 2f;
				}
				if (this.facePosition.Y > (float)(-(float)Game1.pixelZoom * 2))
				{
					this.facePosition.Y = this.facePosition.Y - 2f;
				}
				break;
			case 1:
				if (this.facePosition.X < (float)(Game1.pixelZoom * 2))
				{
					this.facePosition.X = this.facePosition.X + 2f;
				}
				if (this.facePosition.Y < 0f)
				{
					this.facePosition.Y = this.facePosition.Y + 2f;
				}
				break;
			case 2:
				if (this.facePosition.X > 0f)
				{
					this.facePosition.X = this.facePosition.X - 2f;
				}
				else if (this.facePosition.X < 0f)
				{
					this.facePosition.X = this.facePosition.X + 2f;
				}
				if (this.facePosition.Y < 0f)
				{
					this.facePosition.Y = this.facePosition.Y + 2f;
				}
				break;
			case 3:
				if (this.facePosition.X > (float)(-(float)Game1.pixelZoom * 2))
				{
					this.facePosition.X = this.facePosition.X - 2f;
				}
				if (this.facePosition.Y < 0f)
				{
					this.facePosition.Y = this.facePosition.Y + 2f;
				}
				break;
			}
			if (this.ageUntilFullGrown <= 0)
			{
				this.readyToMate -= time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.ageUntilFullGrown -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.mateToPursue != null)
			{
				if (this.readyToMate <= -35000)
				{
					this.mateToPursue.doneMating();
					this.doneMating();
					return;
				}
				this.moveTowardOtherSlime(this.mateToPursue, false, time);
				if (this.mateToPursue.mateToAvoid == null && this.mateToPursue.mateToPursue != null && !this.mateToPursue.mateToPursue.Equals(this))
				{
					this.doneMating();
					return;
				}
				if (Vector2.Distance(base.getStandingPosition(), this.mateToPursue.getStandingPosition()) < (float)(this.GetBoundingBox().Width + 4))
				{
					if (this.mateToPursue.mateToAvoid != null && this.mateToPursue.mateToAvoid.Equals(this))
					{
						this.mateToPursue.mateToAvoid = null;
						this.mateToPursue.matingCountdown = 2000;
						this.mateToPursue.mateToPursue = this;
					}
					this.matingCountdown -= time.ElapsedGameTime.Milliseconds;
					if (this.currentLocation != null && this.matingCountdown <= 0 && this.mateToPursue != null && (!this.currentLocation.isOutdoors || Utility.getNumberOfCharactersInRadius(this.currentLocation, Utility.Vector2ToPoint(this.position), 1) <= 4))
					{
						this.mateWith(this.mateToPursue, this.currentLocation);
						return;
					}
				}
				else if (Vector2.Distance(base.getStandingPosition(), this.mateToPursue.getStandingPosition()) > (float)(GreenSlime.matingRange * 2))
				{
					this.mateToPursue.mateToAvoid = null;
					this.mateToPursue = null;
					return;
				}
			}
			else
			{
				if (this.mateToAvoid != null)
				{
					this.moveTowardOtherSlime(this.mateToAvoid, true, time);
					return;
				}
				if (this.readyToMate < 0 && this.cute)
				{
					this.readyToMate = -1;
					if (Game1.random.NextDouble() < 0.001)
					{
						GreenSlime mate = (GreenSlime)Utility.checkForCharacterWithinArea(base.GetType(), this.position, Game1.currentLocation, new Rectangle(base.getStandingX() - GreenSlime.matingRange, base.getStandingY() - GreenSlime.matingRange, GreenSlime.matingRange * 2, GreenSlime.matingRange * 2));
						if (mate != null && mate.readyToMate <= 0 && !mate.cute)
						{
							this.matingCountdown = 2000;
							this.mateToPursue = mate;
							mate.mateToAvoid = this;
							this.addedSpeed = 1;
							this.mateToPursue.addedSpeed = 1;
							return;
						}
					}
				}
				else if (!this.isGlowing)
				{
					this.addedSpeed = 0;
				}
				this.yOffset = Math.Max(this.yOffset - (int)Math.Abs(this.xVelocity + this.yVelocity) / 2, -Game1.tileSize);
				base.behaviorAtGameTick(time);
				if (this.yOffset < 0)
				{
					this.yOffset = Math.Min(0, this.yOffset + 4 + (int)((this.yOffset <= -Game1.tileSize) ? ((float)(-(float)this.yOffset) / 8f) : ((float)(-(float)this.yOffset) / 16f)));
				}
				this.timeSinceLastJump += time.ElapsedGameTime.Milliseconds;
				if (this.readyToJump != -1)
				{
					this.Halt();
					base.IsWalkingTowardPlayer = false;
					this.readyToJump -= time.ElapsedGameTime.Milliseconds;
					this.sprite.CurrentFrame = 16 + (800 - this.readyToJump) / 200;
					if (this.readyToJump <= 0)
					{
						this.timeSinceLastJump = this.timeSinceLastJump;
						this.slipperiness = 10;
						base.IsWalkingTowardPlayer = true;
						this.readyToJump = -1;
						if (Utility.isOnScreen(this.position, 128))
						{
							Game1.playSound("slime");
						}
						Vector2 trajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
						trajectory.X = -trajectory.X / 2f;
						trajectory.Y = -trajectory.Y / 2f;
						base.setTrajectory((int)trajectory.X, (int)trajectory.Y);
						this.sprite.CurrentFrame = 1;
						this.invincibleCountdown = 0;
					}
				}
				else if (Game1.random.NextDouble() < 0.1 && !this.focusedOnFarmers)
				{
					if (this.facingDirection == 0 || this.facingDirection == 2)
					{
						if (this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, 1, false, this))
						{
							this.position.X = this.position.X - (float)this.speed;
						}
						else if (!this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, 1, false, this))
						{
							this.position.X = this.position.X + (float)this.speed;
						}
					}
					else if (this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, 1, false, this))
					{
						this.position.Y = this.position.Y - (float)this.speed;
					}
					else if (!this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, 1, false, this))
					{
						this.position.Y = this.position.Y + (float)this.speed;
					}
					if (Game1.random.NextDouble() < 0.08)
					{
						this.leftDrift = !this.leftDrift;
					}
				}
				else if (this.withinPlayerThreshold() && this.timeSinceLastJump > (this.focusedOnFarmers ? 1000 : 4000) && Game1.random.NextDouble() < 0.01)
				{
					if (this.name.Equals("Frost Jelly") && Game1.random.NextDouble() < 0.25)
					{
						this.addedSpeed = 2;
						base.startGlowing(Color.Cyan, false, 0.15f);
					}
					else
					{
						this.addedSpeed = 0;
						base.stopGlowing();
						this.readyToJump = 800;
					}
				}
				if (Game1.random.NextDouble() < 0.01 && this.wagTimer <= 0)
				{
					this.wagTimer = 992;
				}
				if (Math.Abs(this.xVelocity) >= 0.5f || Math.Abs(this.yVelocity) >= 0.5f)
				{
					this.sprite.AnimateDown(time, 0, "");
				}
				else if (!this.position.Equals(this.lastPosition))
				{
					this.animateTimer = 500;
				}
				if (this.animateTimer > 0 && this.readyToJump <= 0)
				{
					this.animateTimer -= time.ElapsedGameTime.Milliseconds;
					this.sprite.AnimateDown(time, 0, "");
				}
			}
		}

		// Token: 0x04000BDB RID: 3035
		public const float mutationFactor = 0.25f;

		// Token: 0x04000BDC RID: 3036
		public const int matingInterval = 120000;

		// Token: 0x04000BDD RID: 3037
		public const int childhoodLength = 120000;

		// Token: 0x04000BDE RID: 3038
		public const int durationOfMating = 2000;

		// Token: 0x04000BDF RID: 3039
		public const double chanceToMate = 0.001;

		// Token: 0x04000BE0 RID: 3040
		public static int matingRange = Game1.tileSize * 3;

		// Token: 0x04000BE1 RID: 3041
		public bool leftDrift;

		// Token: 0x04000BE2 RID: 3042
		public bool cute = true;

		// Token: 0x04000BE3 RID: 3043
		private int readyToJump = -1;

		// Token: 0x04000BE4 RID: 3044
		private int matingCountdown;

		// Token: 0x04000BE5 RID: 3045
		private new int yOffset;

		// Token: 0x04000BE6 RID: 3046
		private int wagTimer;

		// Token: 0x04000BE7 RID: 3047
		public int readyToMate = 120000;

		// Token: 0x04000BE8 RID: 3048
		public int ageUntilFullGrown;

		// Token: 0x04000BE9 RID: 3049
		public int animateTimer;

		// Token: 0x04000BEA RID: 3050
		public int timeSinceLastJump;

		// Token: 0x04000BEB RID: 3051
		public int specialNumber;

		// Token: 0x04000BEC RID: 3052
		public bool firstGeneration;

		// Token: 0x04000BED RID: 3053
		public Color color;

		// Token: 0x04000BEE RID: 3054
		private GreenSlime mateToPursue;

		// Token: 0x04000BEF RID: 3055
		private GreenSlime mateToAvoid;

		// Token: 0x04000BF0 RID: 3056
		private Vector2 facePosition;

		// Token: 0x04000BF1 RID: 3057
		private Vector2 faceTargetPosition;
	}
}
