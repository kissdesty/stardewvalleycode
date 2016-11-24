using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
	// Token: 0x020000C3 RID: 195
	internal class AbigailGame : IMinigame
	{
		// Token: 0x170000D6 RID: 214
		public static int TileSize
		{
			// Token: 0x06000C5C RID: 3164 RVA: 0x000F5AF5 File Offset: 0x000F3CF5
			get
			{
				return 48;
			}
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000F5AFC File Offset: 0x000F3CFC
		public AbigailGame(bool playingWithAbby = false)
		{
			this.reset(playingWithAbby);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000F5BFC File Offset: 0x000F3DFC
		public AbigailGame(int coins, int ammoLevel, int bulletDamage, int fireSpeedLevel, int runSpeedLevel, int lives, bool spreadPistol, int whichRound)
		{
			this.reset(false);
			this.coins = coins;
			this.ammoLevel = ammoLevel;
			this.bulletDamage = bulletDamage;
			this.fireSpeedLevel = fireSpeedLevel;
			this.runSpeedLevel = runSpeedLevel;
			this.lives = lives;
			this.spreadPistol = spreadPistol;
			this.whichRound = whichRound;
			this.monsterChances[0] = new Vector2(0.014f + (float)whichRound * 0.005f, 0.41f + (float)whichRound * 0.05f);
			this.monsterChances[4] = new Vector2(0.002f, 0.1f);
			AbigailGame.onStartMenu = false;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000F5D88 File Offset: 0x000F3F88
		public void reset(bool playingWithAbby)
		{
			this.died = false;
			AbigailGame.topLeftScreenCoordinate = new Vector2((float)(Game1.viewport.Width / 2 - 384), (float)(Game1.viewport.Height / 2 - 384));
			AbigailGame.enemyBullets.Clear();
			AbigailGame.holdItemTimer = 0;
			AbigailGame.itemToHold = -1;
			AbigailGame.merchantArriving = false;
			AbigailGame.merchantLeaving = false;
			AbigailGame.merchantShopOpen = false;
			AbigailGame.monsterConfusionTimer = 0;
			AbigailGame.monsters.Clear();
			AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
			AbigailGame.scrollingMap = false;
			AbigailGame.shopping = false;
			AbigailGame.store = false;
			AbigailGame.temporarySprites.Clear();
			AbigailGame.waitingForPlayerToMoveDownAMap = false;
			AbigailGame.waveTimer = 80000;
			AbigailGame.whichWave = 0;
			AbigailGame.zombieModeTimer = 0;
			this.bulletDamage = 1;
			AbigailGame.deathTimer = 0f;
			AbigailGame.shootoutLevel = false;
			AbigailGame.betweenWaveTimer = 5000;
			AbigailGame.gopherRunning = false;
			AbigailGame.hasGopherAppeared = false;
			AbigailGame.playerMovementDirections.Clear();
			AbigailGame.outlawSong = null;
			AbigailGame.overworldSong = null;
			AbigailGame.endCutscene = false;
			AbigailGame.endCutscenePhase = 0;
			AbigailGame.endCutsceneTimer = 0;
			AbigailGame.gameOver = false;
			AbigailGame.deathTimer = 0f;
			AbigailGame.playerInvincibleTimer = 0;
			AbigailGame.playingWithAbigail = playingWithAbby;
			AbigailGame.beatLevelWithAbigail = false;
			AbigailGame.onStartMenu = true;
			AbigailGame.startTimer = 0;
			AbigailGame.powerups.Clear();
			AbigailGame.world = 0;
			Game1.changeMusicTrack("none");
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if ((i == 0 || i == 15 || j == 0 || j == 15) && (i <= 6 || i >= 10) && (j <= 6 || j >= 10))
					{
						AbigailGame.map[i, j] = 5;
					}
					else if (i == 0 || i == 15 || j == 0 || j == 15)
					{
						AbigailGame.map[i, j] = ((Game1.random.NextDouble() < 0.15) ? 1 : 0);
					}
					else if (i == 1 || i == 14 || j == 1 || j == 14)
					{
						AbigailGame.map[i, j] = 2;
					}
					else
					{
						AbigailGame.map[i, j] = ((Game1.random.NextDouble() < 0.1) ? 4 : 3);
					}
				}
			}
			this.playerPosition = new Vector2(384f, 384f);
			this.playerBoundingBox.X = (int)this.playerPosition.X + AbigailGame.TileSize / 4;
			this.playerBoundingBox.Y = (int)this.playerPosition.Y + AbigailGame.TileSize / 4;
			this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
			this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
			if (AbigailGame.playingWithAbigail)
			{
				AbigailGame.onStartMenu = false;
				AbigailGame.player2Position = new Vector2(432f, 384f);
				this.player2BoundingBox = new Rectangle(9 * AbigailGame.TileSize, 8 * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize);
				AbigailGame.betweenWaveTimer += 1500;
			}
			for (int k = 0; k < 4; k++)
			{
				this.spawnQueue[k] = new List<Point>();
			}
			this.noPickUpBox = new Rectangle(0, 0, AbigailGame.TileSize, AbigailGame.TileSize);
			this.merchantBox = new Rectangle(8 * AbigailGame.TileSize, 0, AbigailGame.TileSize, AbigailGame.TileSize);
			AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000F60E0 File Offset: 0x000F42E0
		public float getMovementSpeed(float speed, int directions)
		{
			float movementSpeed = speed;
			if (directions > 1)
			{
				movementSpeed = (float)Math.Max(1, (int)Math.Sqrt((double)(2f * (movementSpeed * movementSpeed))) / 2);
			}
			return movementSpeed;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000F6110 File Offset: 0x000F4310
		public bool getPowerUp(AbigailGame.CowboyPowerup c)
		{
			int which = c.which;
			switch (which)
			{
			case -3:
				this.usePowerup(-3);
				break;
			case -2:
				this.usePowerup(-2);
				break;
			case -1:
				this.usePowerup(-1);
				break;
			case 0:
				this.coins++;
				Game1.playSound("Pickup_Coin15");
				break;
			case 1:
				this.coins += 5;
				Game1.playSound("Pickup_Coin15");
				break;
			default:
				if (which != 8)
				{
					if (this.heldItem != null)
					{
						AbigailGame.CowboyPowerup tmp = this.heldItem;
						this.heldItem = c;
						this.noPickUpBox.Location = c.position;
						tmp.position = c.position;
						AbigailGame.powerups.Add(tmp);
						Game1.playSound("cowboy_powerup");
						return true;
					}
					this.heldItem = c;
					Game1.playSound("cowboy_powerup");
				}
				else
				{
					this.lives++;
					Game1.playSound("cowboy_powerup");
				}
				break;
			}
			return true;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x000F621C File Offset: 0x000F441C
		public void usePowerup(int which)
		{
			if (this.activePowerups.ContainsKey(which))
			{
				this.activePowerups[which] = this.powerupDuration + 2000;
				return;
			}
			switch (which)
			{
			case -3:
				AbigailGame.itemToHold = 13;
				AbigailGame.holdItemTimer = 4000;
				Game1.playSound("Cowboy_Secret");
				AbigailGame.endCutscene = true;
				AbigailGame.endCutsceneTimer = 4000;
				AbigailGame.world = 0;
				if (!Game1.player.hasOrWillReceiveMail("Beat_PK"))
				{
					Game1.addMailForTomorrow("Beat_PK", false, false);
					goto IL_846;
				}
				goto IL_846;
			case -2:
			case -1:
				AbigailGame.itemToHold = ((which == -1) ? 12 : 11);
				AbigailGame.holdItemTimer = 2000;
				Game1.playSound("Cowboy_Secret");
				AbigailGame.gopherTrain = true;
				AbigailGame.gopherTrainPosition = -AbigailGame.TileSize * 2;
				goto IL_846;
			case 0:
				this.coins++;
				Game1.playSound("Pickup_Coin15");
				goto IL_846;
			case 1:
				this.coins += 5;
				Game1.playSound("Pickup_Coin15");
				Game1.playSound("Pickup_Coin15");
				goto IL_846;
			case 2:
			case 3:
			case 7:
				this.shotTimer = 0;
				Game1.playSound("cowboy_gunload");
				this.activePowerups.Add(which, this.powerupDuration + 2000);
				goto IL_846;
			case 4:
				Game1.playSound("cowboy_explosion");
				if (!AbigailGame.shootoutLevel)
				{
					foreach (AbigailGame.CowboyMonster c in AbigailGame.monsters)
					{
						AbigailGame.addGuts(c.position.Location, c.type);
					}
					AbigailGame.monsters.Clear();
				}
				else
				{
					foreach (AbigailGame.CowboyMonster c2 in AbigailGame.monsters)
					{
						c2.takeDamage(30);
						this.bullets.Add(new AbigailGame.CowboyBullet(c2.position.Center, 2, 1));
					}
				}
				for (int i = 0; i < 30; i++)
				{
					AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)Game1.random.Next(1, 16), (float)Game1.random.Next(1, 16)) * (float)AbigailGame.TileSize + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
					{
						delayBeforeAnimationStart = Game1.random.Next(800)
					});
				}
				goto IL_846;
			case 5:
				if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
				{
					AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
				}
				Game1.playSound("Cowboy_undead");
				this.motionPause = 1800;
				AbigailGame.zombieModeTimer = 10000;
				goto IL_846;
			case 8:
				this.lives++;
				Game1.playSound("cowboy_powerup");
				goto IL_846;
			case 9:
			{
				Point teleportSpot = Point.Zero;
				while (Math.Abs((float)teleportSpot.X - this.playerPosition.X) < 8f || Math.Abs((float)teleportSpot.Y - this.playerPosition.Y) < 8f || AbigailGame.isCollidingWithMap(teleportSpot) || AbigailGame.isCollidingWithMonster(new Rectangle(teleportSpot.X, teleportSpot.Y, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					teleportSpot = new Point(Game1.random.Next(AbigailGame.TileSize, 16 * AbigailGame.TileSize - AbigailGame.TileSize), Game1.random.Next(AbigailGame.TileSize, 16 * AbigailGame.TileSize - AbigailGame.TileSize));
				}
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)teleportSpot.X, (float)teleportSpot.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)(teleportSpot.X - AbigailGame.TileSize / 2), (float)teleportSpot.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 200
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)(teleportSpot.X + AbigailGame.TileSize / 2), (float)teleportSpot.Y) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 400
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)teleportSpot.X, (float)(teleportSpot.Y - AbigailGame.TileSize / 2)) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 600
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 120f, 5, 0, new Vector2((float)teleportSpot.X, (float)(teleportSpot.Y + AbigailGame.TileSize / 2)) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 800
				});
				this.playerPosition = new Vector2((float)teleportSpot.X, (float)teleportSpot.Y);
				AbigailGame.monsterConfusionTimer = 4000;
				AbigailGame.playerInvincibleTimer = 4000;
				Game1.playSound("cowboy_powerup");
				goto IL_846;
			}
			case 10:
				this.usePowerup(7);
				this.usePowerup(3);
				this.usePowerup(6);
				for (int j = 0; j < this.activePowerups.Count; j++)
				{
					Dictionary<int, int> dictionary = this.activePowerups;
					int key = this.activePowerups.ElementAt(j).Key;
					dictionary[key] *= 2;
				}
				goto IL_846;
			}
			this.activePowerups.Add(which, this.powerupDuration);
			Game1.playSound("cowboy_powerup");
			IL_846:
			if (this.whichRound > 0 && this.activePowerups.ContainsKey(which))
			{
				Dictionary<int, int> dictionary = this.activePowerups;
				dictionary[which] /= 2;
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000F6ABC File Offset: 0x000F4CBC
		public static void addGuts(Point position, int whichGuts)
		{
			switch (whichGuts)
			{
			case 0:
			case 2:
			case 5:
			case 6:
			case 7:
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(512, 1696, 16, 16), 80f, 6, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 1696, 16, 16), 10000f, 1, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = 480
				});
				return;
			case 1:
			case 4:
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(544, 1728, 16, 16), 80f, 4, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				return;
			case 3:
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)position.X, (float)position.Y), false, Game1.random.NextDouble() < 0.5, 0.001f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
				return;
			default:
				return;
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000F6D14 File Offset: 0x000F4F14
		public void endOfGopherAnimationBehavior2(int extraInfo)
		{
			Game1.playSound("cowboy_gopher");
			if (Math.Abs(AbigailGame.gopherBox.X - 8 * AbigailGame.TileSize) > Math.Abs(AbigailGame.gopherBox.Y - 8 * AbigailGame.TileSize))
			{
				if (AbigailGame.gopherBox.X > 8 * AbigailGame.TileSize)
				{
					this.gopherMotion = new Point(-2, 0);
				}
				else
				{
					this.gopherMotion = new Point(2, 0);
				}
			}
			else if (AbigailGame.gopherBox.Y > 8 * AbigailGame.TileSize)
			{
				this.gopherMotion = new Point(0, -2);
			}
			else
			{
				this.gopherMotion = new Point(0, 2);
			}
			AbigailGame.gopherRunning = true;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x000F6DC4 File Offset: 0x000F4FC4
		public void endOfGopherAnimationBehavior(int extrainfo)
		{
			AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(384, 1792, 16, 16), 120f, 4, 2, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.gopherBox.X + AbigailGame.TileSize / 2), (float)(AbigailGame.gopherBox.Y + AbigailGame.TileSize / 2)), false, false, (float)AbigailGame.gopherBox.Y / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
			{
				endFunction = new TemporaryAnimatedSprite.endBehavior(this.endOfGopherAnimationBehavior2)
			});
			Game1.playSound("cowboy_gopher");
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x000F6E84 File Offset: 0x000F5084
		public static void killOutlaw()
		{
			AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup((AbigailGame.world == 0) ? -1 : -2, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
			if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
			{
				AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
			}
			AbigailGame.map[8, 8] = 10;
			AbigailGame.screenFlash = 200;
			Game1.playSound("Cowboy_monsterDie");
			for (int i = 0; i < 15; i++)
			{
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)(AbigailGame.monsters[0].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[0].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = i * 75
				});
			}
			AbigailGame.monsters.Clear();
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x000F7004 File Offset: 0x000F5204
		public void updateBullets(GameTime time)
		{
			for (int i = this.bullets.Count - 1; i >= 0; i--)
			{
				AbigailGame.CowboyBullet expr_29_cp_0_cp_0 = this.bullets[i];
				expr_29_cp_0_cp_0.position.X = expr_29_cp_0_cp_0.position.X + this.bullets[i].motion.X;
				AbigailGame.CowboyBullet expr_59_cp_0_cp_0 = this.bullets[i];
				expr_59_cp_0_cp_0.position.Y = expr_59_cp_0_cp_0.position.Y + this.bullets[i].motion.Y;
				if (this.bullets[i].position.X <= 0 || this.bullets[i].position.Y <= 0 || this.bullets[i].position.X >= 768 || this.bullets[i].position.Y >= 768)
				{
					this.bullets.RemoveAt(i);
				}
				else if (AbigailGame.map[this.bullets[i].position.X / 16 / 3, this.bullets[i].position.Y / 16 / 3] == 7)
				{
					this.bullets.RemoveAt(i);
				}
				else
				{
					int j = AbigailGame.monsters.Count - 1;
					while (j >= 0)
					{
						if (AbigailGame.monsters[j].position.Intersects(new Rectangle(this.bullets[i].position.X, this.bullets[i].position.Y, 12, 12)))
						{
							int monsterhealth = AbigailGame.monsters[j].health;
							int monsterAfterDamageHealth;
							if (AbigailGame.monsters[j].takeDamage(this.bullets[i].damage))
							{
								monsterAfterDamageHealth = AbigailGame.monsters[j].health;
								AbigailGame.addGuts(AbigailGame.monsters[j].position.Location, AbigailGame.monsters[j].type);
								int loot = AbigailGame.monsters[j].getLootDrop();
								if (this.whichRound == 1 && Game1.random.NextDouble() < 0.5)
								{
									loot = -1;
								}
								if (this.whichRound > 0 && (loot == 5 || loot == 8) && Game1.random.NextDouble() < 0.4)
								{
									loot = -1;
								}
								if (loot != -1 && AbigailGame.whichWave != 12)
								{
									AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(loot, AbigailGame.monsters[j].position.Location, this.lootDuration));
								}
								if (AbigailGame.shootoutLevel)
								{
									if (AbigailGame.whichWave == 12 && AbigailGame.monsters[j].type == -2)
									{
										Game1.playSound("cowboy_explosion");
										AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup(-3, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
										this.noPickUpBox = new Rectangle(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize);
										if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
										{
											AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
										}
										AbigailGame.screenFlash = 200;
										for (int k = 0; k < 30; k++)
										{
											AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(512, 1696, 16, 16), 70f, 6, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
											{
												delayBeforeAnimationStart = k * 75
											});
											if (k % 4 == 0)
											{
												AbigailGame.addGuts(new Point(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize), AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), 7);
											}
											if (k % 4 == 0)
											{
												AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
												{
													delayBeforeAnimationStart = k * 75
												});
											}
											if (k % 3 == 0)
											{
												AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(544, 1728, 16, 16), 100f, 4, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
												{
													delayBeforeAnimationStart = k * 75
												});
											}
										}
									}
									else if (AbigailGame.whichWave != 12)
									{
										AbigailGame.powerups.Add(new AbigailGame.CowboyPowerup((AbigailGame.world == 0) ? -1 : -2, new Point(8 * AbigailGame.TileSize, 10 * AbigailGame.TileSize), 9999999));
										if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
										{
											AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
										}
										AbigailGame.map[8, 8] = 10;
										AbigailGame.screenFlash = 200;
										for (int l = 0; l < 15; l++)
										{
											AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, new Vector2((float)(AbigailGame.monsters[j].position.X + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize)), (float)(AbigailGame.monsters[j].position.Y + Game1.random.Next(-AbigailGame.TileSize, AbigailGame.TileSize))) + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
											{
												delayBeforeAnimationStart = l * 75
											});
										}
									}
								}
								AbigailGame.monsters.RemoveAt(j);
								Game1.playSound("Cowboy_monsterDie");
							}
							else
							{
								monsterAfterDamageHealth = AbigailGame.monsters[j].health;
							}
							this.bullets[i].damage -= monsterhealth - monsterAfterDamageHealth;
							if (this.bullets[i].damage <= 0)
							{
								this.bullets.RemoveAt(i);
								break;
							}
							break;
						}
						else
						{
							j--;
						}
					}
				}
			}
			for (int m = AbigailGame.enemyBullets.Count - 1; m >= 0; m--)
			{
				AbigailGame.CowboyBullet expr_8A4_cp_0_cp_0 = AbigailGame.enemyBullets[m];
				expr_8A4_cp_0_cp_0.position.X = expr_8A4_cp_0_cp_0.position.X + AbigailGame.enemyBullets[m].motion.X;
				AbigailGame.CowboyBullet expr_8D4_cp_0_cp_0 = AbigailGame.enemyBullets[m];
				expr_8D4_cp_0_cp_0.position.Y = expr_8D4_cp_0_cp_0.position.Y + AbigailGame.enemyBullets[m].motion.Y;
				if (AbigailGame.enemyBullets[m].position.X <= 0 || AbigailGame.enemyBullets[m].position.Y <= 0 || AbigailGame.enemyBullets[m].position.X >= 762 || AbigailGame.enemyBullets[m].position.Y >= 762)
				{
					AbigailGame.enemyBullets.RemoveAt(m);
				}
				else if (AbigailGame.map[(AbigailGame.enemyBullets[m].position.X + 6) / 16 / 3, (AbigailGame.enemyBullets[m].position.Y + 6) / 16 / 3] == 7)
				{
					AbigailGame.enemyBullets.RemoveAt(m);
				}
				else if (AbigailGame.playerInvincibleTimer <= 0 && AbigailGame.deathTimer <= 0f && this.playerBoundingBox.Intersects(new Rectangle(AbigailGame.enemyBullets[m].position.X, AbigailGame.enemyBullets[m].position.Y, 15, 15)))
				{
					this.playerDie();
					return;
				}
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000F7A3C File Offset: 0x000F5C3C
		public void playerDie()
		{
			AbigailGame.gopherRunning = false;
			AbigailGame.hasGopherAppeared = false;
			this.spawnQueue = new List<Point>[4];
			for (int i = 0; i < 4; i++)
			{
				this.spawnQueue[i] = new List<Point>();
			}
			AbigailGame.enemyBullets.Clear();
			if (!AbigailGame.shootoutLevel)
			{
				AbigailGame.powerups.Clear();
				AbigailGame.monsters.Clear();
			}
			this.died = true;
			this.activePowerups.Clear();
			AbigailGame.deathTimer = 3000f;
			if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
			{
				AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
			}
			AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 120f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate, false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
			AbigailGame.waveTimer = Math.Min(80000, AbigailGame.waveTimer + 10000);
			AbigailGame.betweenWaveTimer = 4000;
			this.lives--;
			AbigailGame.playerInvincibleTimer = 5000;
			if (AbigailGame.shootoutLevel)
			{
				this.playerPosition = new Vector2((float)(8 * AbigailGame.TileSize), (float)(3 * AbigailGame.TileSize));
				Game1.playSound("Cowboy_monsterDie");
			}
			else
			{
				this.playerPosition = new Vector2((float)(8 * AbigailGame.TileSize - AbigailGame.TileSize), (float)(8 * AbigailGame.TileSize));
				this.playerBoundingBox.X = (int)this.playerPosition.X;
				this.playerBoundingBox.Y = (int)this.playerPosition.Y;
				if (this.playerBoundingBox.Intersects(this.player2BoundingBox))
				{
					this.playerPosition.X = this.playerPosition.X - (float)(AbigailGame.TileSize * 3 / 2);
					this.player2deathtimer = (int)AbigailGame.deathTimer;
				}
				Game1.playSound("cowboy_dead");
			}
			if (this.lives < 0)
			{
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 550f, 5, 0, this.playerPosition + AbigailGame.topLeftScreenCoordinate, false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					alpha = 0.001f,
					endFunction = new TemporaryAnimatedSprite.endBehavior(this.afterPlayerDeathFunction)
				});
				AbigailGame.deathTimer *= 3f;
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x000F7CD8 File Offset: 0x000F5ED8
		public void afterPlayerDeathFunction(int extra)
		{
			if (this.lives < 0)
			{
				AbigailGame.gameOver = true;
				if (AbigailGame.overworldSong != null && !AbigailGame.overworldSong.IsPlaying)
				{
					AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
				}
				if (AbigailGame.outlawSong != null && !AbigailGame.outlawSong.IsPlaying)
				{
					AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
				}
				AbigailGame.monsters.Clear();
				AbigailGame.powerups.Clear();
				this.died = false;
				Game1.playSound("Cowboy_monsterDie");
				if (AbigailGame.playingWithAbigail && Game1.currentLocation.currentEvent != null)
				{
					this.unload();
					Game1.currentMinigame = null;
					Event expr_9C = Game1.currentLocation.currentEvent;
					int currentCommand = expr_9C.CurrentCommand;
					expr_9C.CurrentCommand = currentCommand + 1;
				}
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000F7D90 File Offset: 0x000F5F90
		public void startAbigailPortrait(int whichExpression, string sayWhat)
		{
			if (this.abigailPortraitTimer <= 0)
			{
				this.abigailPortraitTimer = 6000;
				this.AbigailDialogue = sayWhat;
				this.abigailPortraitExpression = whichExpression;
				this.abigailPortraitYposition = Game1.graphics.GraphicsDevice.Viewport.Height;
				Game1.playSound("dwop");
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000F7DE6 File Offset: 0x000F5FE6
		public void startNewRound()
		{
			this.gamerestartTimer = 2000;
			Game1.playSound("Cowboy_monsterDie");
			this.whichRound++;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000F7E0C File Offset: 0x000F600C
		public bool tick(GameTime time)
		{
			if (this.quit)
			{
				if (Game1.currentLocation != null && Game1.currentLocation.name.Equals("Saloon") && Game1.timeOfDay >= 1700)
				{
					Game1.changeMusicTrack("Saloon1");
				}
				return true;
			}
			if (AbigailGame.gameOver || AbigailGame.onStartMenu)
			{
				if (AbigailGame.startTimer > 0)
				{
					AbigailGame.startTimer -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.startTimer <= 0)
					{
						AbigailGame.onStartMenu = false;
					}
				}
				return false;
			}
			if (this.gamerestartTimer > 0)
			{
				this.gamerestartTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gamerestartTimer <= 0)
				{
					this.unload();
					if (this.whichRound == 0 || !AbigailGame.endCutscene)
					{
						Game1.currentMinigame = new AbigailGame(false);
					}
					else
					{
						Game1.currentMinigame = new AbigailGame(this.coins, this.ammoLevel, this.bulletDamage, this.fireSpeedLevel, this.runSpeedLevel, this.lives, this.spreadPistol, this.whichRound);
					}
				}
			}
			if (this.fadethenQuitTimer > 0 && (float)this.abigailPortraitTimer <= 0f)
			{
				this.fadethenQuitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.fadethenQuitTimer <= 0)
				{
					if (Game1.currentLocation.currentEvent != null)
					{
						Event expr_159 = Game1.currentLocation.currentEvent;
						int num = expr_159.CurrentCommand;
						expr_159.CurrentCommand = num + 1;
						if (AbigailGame.beatLevelWithAbigail)
						{
							Game1.currentLocation.currentEvent.specialEventVariable1 = true;
						}
					}
					return true;
				}
			}
			if (this.abigailPortraitTimer > 0)
			{
				this.abigailPortraitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.abigailPortraitTimer > 1000 && this.abigailPortraitYposition > Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 4)
				{
					this.abigailPortraitYposition -= 16;
				}
				else if (this.abigailPortraitTimer <= 1000)
				{
					this.abigailPortraitYposition += 16;
				}
			}
			if (AbigailGame.endCutscene)
			{
				AbigailGame.endCutsceneTimer -= time.ElapsedGameTime.Milliseconds;
				if (AbigailGame.endCutsceneTimer < 0)
				{
					AbigailGame.endCutscenePhase++;
					if (AbigailGame.endCutscenePhase > 5)
					{
						AbigailGame.endCutscenePhase = 5;
					}
					switch (AbigailGame.endCutscenePhase)
					{
					case 1:
						Game1.getSteamAchievement("Achievement_PrairieKing");
						if (!this.died)
						{
							Game1.getSteamAchievement("Achievement_FectorsChallenge");
						}
						AbigailGame.endCutsceneTimer = 15500;
						Game1.playSound("Cowboy_singing");
						AbigailGame.map = this.getMap(-1);
						break;
					case 2:
						this.playerPosition = new Vector2(0f, (float)(8 * AbigailGame.TileSize));
						AbigailGame.endCutsceneTimer = 12000;
						break;
					case 3:
						AbigailGame.endCutsceneTimer = 5000;
						break;
					case 4:
						AbigailGame.endCutsceneTimer = 1000;
						break;
					case 5:
						if (Game1.oldKBState.GetPressedKeys().Length == 0)
						{
							GamePadState arg_304_0 = Game1.oldPadState;
							if (Game1.oldPadState.Buttons.X != ButtonState.Pressed && Game1.oldPadState.Buttons.Start != ButtonState.Pressed && Game1.oldPadState.Buttons.A != ButtonState.Pressed)
							{
								break;
							}
						}
						if (this.gamerestartTimer <= 0)
						{
							this.startNewRound();
						}
						break;
					}
				}
				if (AbigailGame.endCutscenePhase == 2 && this.playerPosition.X < (float)(9 * AbigailGame.TileSize))
				{
					this.playerPosition.X = this.playerPosition.X + 1f;
					this.playerMotionAnimationTimer += (float)time.ElapsedGameTime.Milliseconds;
					this.playerMotionAnimationTimer %= 400f;
				}
				return false;
			}
			if (this.motionPause > 0)
			{
				this.motionPause -= time.ElapsedGameTime.Milliseconds;
				if (this.motionPause <= 0 && this.behaviorAfterPause != null)
				{
					this.behaviorAfterPause();
					this.behaviorAfterPause = null;
				}
			}
			else if (AbigailGame.monsterConfusionTimer > 0)
			{
				AbigailGame.monsterConfusionTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.zombieModeTimer > 0)
			{
				AbigailGame.zombieModeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.holdItemTimer > 0)
			{
				AbigailGame.holdItemTimer -= time.ElapsedGameTime.Milliseconds;
				return false;
			}
			if (AbigailGame.screenFlash > 0)
			{
				AbigailGame.screenFlash -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.gopherTrain)
			{
				AbigailGame.gopherTrainPosition += 3;
				if (AbigailGame.gopherTrainPosition % 30 == 0)
				{
					Game1.playSound("Cowboy_Footstep");
				}
				if (AbigailGame.playerJumped)
				{
					this.playerPosition.Y = this.playerPosition.Y + 3f;
				}
				if (Math.Abs(this.playerPosition.Y - (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize)) <= 16f)
				{
					AbigailGame.playerJumped = true;
					this.playerPosition.Y = (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize);
				}
				if (AbigailGame.gopherTrainPosition > 16 * AbigailGame.TileSize + AbigailGame.TileSize)
				{
					AbigailGame.gopherTrain = false;
					AbigailGame.playerJumped = false;
					AbigailGame.whichWave++;
					AbigailGame.map = this.getMap(AbigailGame.whichWave);
					this.playerPosition = new Vector2((float)(8 * AbigailGame.TileSize), (float)(8 * AbigailGame.TileSize));
					AbigailGame.world = ((AbigailGame.world == 0) ? 2 : 1);
					AbigailGame.waveTimer = 80000;
					AbigailGame.betweenWaveTimer = 5000;
					AbigailGame.waitingForPlayerToMoveDownAMap = false;
					AbigailGame.shootoutLevel = false;
				}
			}
			if ((AbigailGame.shopping || AbigailGame.merchantArriving || AbigailGame.merchantLeaving || AbigailGame.waitingForPlayerToMoveDownAMap) && AbigailGame.holdItemTimer <= 0)
			{
				int oldTimer = AbigailGame.shoppingTimer;
				AbigailGame.shoppingTimer += time.ElapsedGameTime.Milliseconds;
				AbigailGame.shoppingTimer %= 500;
				if (!AbigailGame.merchantShopOpen && AbigailGame.shopping && ((oldTimer < 250 && AbigailGame.shoppingTimer >= 250) || oldTimer > AbigailGame.shoppingTimer))
				{
					Game1.playSound("Cowboy_Footstep");
				}
			}
			if (AbigailGame.playerInvincibleTimer > 0)
			{
				AbigailGame.playerInvincibleTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (AbigailGame.scrollingMap)
			{
				AbigailGame.newMapPosition -= AbigailGame.TileSize / 8;
				this.playerPosition.Y = this.playerPosition.Y - (float)(AbigailGame.TileSize / 8);
				this.playerPosition.Y = this.playerPosition.Y + 3f;
				this.playerBoundingBox.X = (int)this.playerPosition.X + AbigailGame.TileSize / 4;
				this.playerBoundingBox.Y = (int)this.playerPosition.Y + AbigailGame.TileSize / 4;
				this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
				this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
				AbigailGame.playerMovementDirections = new List<int>
				{
					2
				};
				this.playerMotionAnimationTimer += (float)time.ElapsedGameTime.Milliseconds;
				this.playerMotionAnimationTimer %= 400f;
				if (AbigailGame.newMapPosition <= 0)
				{
					AbigailGame.scrollingMap = false;
					AbigailGame.map = AbigailGame.nextMap;
					AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
					AbigailGame.shopping = false;
					AbigailGame.betweenWaveTimer = 5000;
					AbigailGame.waitingForPlayerToMoveDownAMap = false;
					AbigailGame.playerMovementDirections.Clear();
					if (AbigailGame.whichWave == 12)
					{
						AbigailGame.shootoutLevel = true;
						AbigailGame.monsters.Add(new AbigailGame.Dracula());
						if (this.whichRound > 0)
						{
							AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health *= 2;
						}
					}
					else if (AbigailGame.whichWave % 4 == 0)
					{
						AbigailGame.shootoutLevel = true;
						AbigailGame.monsters.Add(new AbigailGame.Outlaw(new Point(8 * AbigailGame.TileSize, 13 * AbigailGame.TileSize), (AbigailGame.world == 0) ? 50 : 100));
						if (Game1.soundBank != null)
						{
							AbigailGame.outlawSong = Game1.soundBank.GetCue("cowboy_outlawsong");
							AbigailGame.outlawSong.Play();
						}
					}
				}
			}
			if (AbigailGame.gopherRunning)
			{
				AbigailGame.gopherBox.X = AbigailGame.gopherBox.X + this.gopherMotion.X;
				AbigailGame.gopherBox.Y = AbigailGame.gopherBox.Y + this.gopherMotion.Y;
				for (int i = AbigailGame.monsters.Count - 1; i >= 0; i--)
				{
					if (AbigailGame.gopherBox.Intersects(AbigailGame.monsters[i].position))
					{
						AbigailGame.addGuts(AbigailGame.monsters[i].position.Location, AbigailGame.monsters[i].type);
						AbigailGame.monsters.RemoveAt(i);
						Game1.playSound("Cowboy_monsterDie");
					}
				}
				if (AbigailGame.gopherBox.X < 0 || AbigailGame.gopherBox.Y < 0 || AbigailGame.gopherBox.X > 16 * AbigailGame.TileSize || AbigailGame.gopherBox.Y > 16 * AbigailGame.TileSize)
				{
					AbigailGame.gopherRunning = false;
				}
			}
			for (int j = AbigailGame.temporarySprites.Count - 1; j >= 0; j--)
			{
				if (AbigailGame.temporarySprites[j].update(time))
				{
					AbigailGame.temporarySprites.RemoveAt(j);
				}
			}
			if (this.motionPause <= 0)
			{
				for (int k = AbigailGame.powerups.Count - 1; k >= 0; k--)
				{
					if (Utility.distance((float)this.playerBoundingBox.Center.X, (float)(AbigailGame.powerups[k].position.X + AbigailGame.TileSize / 2), (float)this.playerBoundingBox.Center.Y, (float)(AbigailGame.powerups[k].position.Y + AbigailGame.TileSize / 2)) <= (float)(AbigailGame.TileSize + 3) && (AbigailGame.powerups[k].position.X < AbigailGame.TileSize || AbigailGame.powerups[k].position.X >= 16 * AbigailGame.TileSize - AbigailGame.TileSize || AbigailGame.powerups[k].position.Y < AbigailGame.TileSize || AbigailGame.powerups[k].position.Y >= 16 * AbigailGame.TileSize - AbigailGame.TileSize))
					{
						if (AbigailGame.powerups[k].position.X + AbigailGame.TileSize / 2 < this.playerBoundingBox.Center.X)
						{
							AbigailGame.CowboyPowerup expr_AAA_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_AAA_cp_0_cp_0.position.X = expr_AAA_cp_0_cp_0.position.X + 1;
						}
						if (AbigailGame.powerups[k].position.X + AbigailGame.TileSize / 2 > this.playerBoundingBox.Center.X)
						{
							AbigailGame.CowboyPowerup expr_AF5_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_AF5_cp_0_cp_0.position.X = expr_AF5_cp_0_cp_0.position.X - 1;
						}
						if (AbigailGame.powerups[k].position.Y + AbigailGame.TileSize / 2 < this.playerBoundingBox.Center.Y)
						{
							AbigailGame.CowboyPowerup expr_B40_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_B40_cp_0_cp_0.position.Y = expr_B40_cp_0_cp_0.position.Y + 1;
						}
						if (AbigailGame.powerups[k].position.Y + AbigailGame.TileSize / 2 > this.playerBoundingBox.Center.Y)
						{
							AbigailGame.CowboyPowerup expr_B8B_cp_0_cp_0 = AbigailGame.powerups[k];
							expr_B8B_cp_0_cp_0.position.Y = expr_B8B_cp_0_cp_0.position.Y - 1;
						}
					}
					AbigailGame.powerups[k].duration -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.powerups[k].duration <= 0)
					{
						AbigailGame.powerups.RemoveAt(k);
					}
				}
				for (int l = this.activePowerups.Count - 1; l >= 0; l--)
				{
					Dictionary<int, int> dictionary = this.activePowerups;
					int num = this.activePowerups.ElementAt(l).Key;
					dictionary[num] -= time.ElapsedGameTime.Milliseconds;
					if (this.activePowerups[this.activePowerups.ElementAt(l).Key] <= 0)
					{
						this.activePowerups.Remove(this.activePowerups.ElementAt(l).Key);
					}
				}
				if (AbigailGame.deathTimer <= 0f && AbigailGame.playerMovementDirections.Count > 0 && !AbigailGame.scrollingMap)
				{
					int effectiveDirections = AbigailGame.playerMovementDirections.Count;
					if (effectiveDirections >= 2 && AbigailGame.playerMovementDirections.Last<int>() == (AbigailGame.playerMovementDirections.ElementAt(AbigailGame.playerMovementDirections.Count - 2) + 2) % 4)
					{
						effectiveDirections = 1;
					}
					float speed = this.getMovementSpeed(3f, effectiveDirections);
					if (this.activePowerups.Keys.Contains(6))
					{
						speed *= 1.5f;
					}
					if (AbigailGame.zombieModeTimer > 0)
					{
						speed *= 1.5f;
					}
					for (int m = 0; m < this.runSpeedLevel; m++)
					{
						speed *= 1.25f;
					}
					for (int n = Math.Max(0, AbigailGame.playerMovementDirections.Count - 2); n < AbigailGame.playerMovementDirections.Count; n++)
					{
						if (n != 0 || AbigailGame.playerMovementDirections.Count < 2 || AbigailGame.playerMovementDirections.Last<int>() != (AbigailGame.playerMovementDirections.ElementAt(AbigailGame.playerMovementDirections.Count - 2) + 2) % 4)
						{
							Vector2 newPlayerPosition = this.playerPosition;
							switch (AbigailGame.playerMovementDirections.ElementAt(n))
							{
							case 0:
								newPlayerPosition.Y -= speed;
								break;
							case 1:
								newPlayerPosition.X += speed;
								break;
							case 2:
								newPlayerPosition.Y += speed;
								break;
							case 3:
								newPlayerPosition.X -= speed;
								break;
							}
							Rectangle newPlayerBox = new Rectangle((int)newPlayerPosition.X + AbigailGame.TileSize / 4, (int)newPlayerPosition.Y + AbigailGame.TileSize / 4, AbigailGame.TileSize / 2, AbigailGame.TileSize / 2);
							if (!AbigailGame.isCollidingWithMap(newPlayerBox) && (!this.merchantBox.Intersects(newPlayerBox) || this.merchantBox.Intersects(this.playerBoundingBox)) && (!AbigailGame.playingWithAbigail || !newPlayerBox.Intersects(this.player2BoundingBox)))
							{
								this.playerPosition = newPlayerPosition;
							}
						}
					}
					this.playerBoundingBox.X = (int)this.playerPosition.X + AbigailGame.TileSize / 4;
					this.playerBoundingBox.Y = (int)this.playerPosition.Y + AbigailGame.TileSize / 4;
					this.playerBoundingBox.Width = AbigailGame.TileSize / 2;
					this.playerBoundingBox.Height = AbigailGame.TileSize / 2;
					this.playerMotionAnimationTimer += (float)time.ElapsedGameTime.Milliseconds;
					this.playerMotionAnimationTimer %= 400f;
					this.playerFootstepSoundTimer -= (float)time.ElapsedGameTime.Milliseconds;
					if (this.playerFootstepSoundTimer <= 0f)
					{
						Game1.playSound("Cowboy_Footstep");
						this.playerFootstepSoundTimer = 200f;
					}
					for (int i2 = AbigailGame.powerups.Count - 1; i2 >= 0; i2--)
					{
						if (this.playerBoundingBox.Intersects(new Rectangle(AbigailGame.powerups[i2].position.X, AbigailGame.powerups[i2].position.Y, AbigailGame.TileSize, AbigailGame.TileSize)) && !this.playerBoundingBox.Intersects(this.noPickUpBox))
						{
							if (this.heldItem != null)
							{
								this.usePowerup(AbigailGame.powerups[i2].which);
								AbigailGame.powerups.RemoveAt(i2);
							}
							else if (this.getPowerUp(AbigailGame.powerups[i2]))
							{
								AbigailGame.powerups.RemoveAt(i2);
							}
						}
					}
					if (!this.playerBoundingBox.Intersects(this.noPickUpBox))
					{
						this.noPickUpBox.Location = new Point(0, 0);
					}
					if (AbigailGame.waitingForPlayerToMoveDownAMap && this.playerBoundingBox.Bottom >= 16 * AbigailGame.TileSize - AbigailGame.TileSize / 2)
					{
						AbigailGame.shopping = false;
						AbigailGame.merchantArriving = false;
						AbigailGame.merchantLeaving = false;
						AbigailGame.merchantShopOpen = false;
						this.merchantBox.Y = -AbigailGame.TileSize;
						AbigailGame.scrollingMap = true;
						AbigailGame.nextMap = this.getMap(AbigailGame.whichWave);
						AbigailGame.newMapPosition = 16 * AbigailGame.TileSize;
						AbigailGame.temporarySprites.Clear();
						AbigailGame.powerups.Clear();
					}
					if (!this.shoppingCarpetNoPickup.Intersects(this.playerBoundingBox))
					{
						this.shoppingCarpetNoPickup.X = -1000;
					}
				}
				if (AbigailGame.shopping)
				{
					if (this.merchantBox.Y < 8 * AbigailGame.TileSize - AbigailGame.TileSize * 3 && AbigailGame.merchantArriving)
					{
						this.merchantBox.Y = this.merchantBox.Y + 2;
						if (this.merchantBox.Y >= 8 * AbigailGame.TileSize - AbigailGame.TileSize * 3)
						{
							AbigailGame.merchantShopOpen = true;
							Game1.playSound("cowboy_monsterhit");
							AbigailGame.map[8, 15] = 3;
							AbigailGame.map[7, 15] = 3;
							AbigailGame.map[7, 15] = 3;
							AbigailGame.map[8, 14] = 3;
							AbigailGame.map[7, 14] = 3;
							AbigailGame.map[7, 14] = 3;
							this.shoppingCarpetNoPickup = new Rectangle(this.merchantBox.X - AbigailGame.TileSize, this.merchantBox.Y + AbigailGame.TileSize, AbigailGame.TileSize * 3, AbigailGame.TileSize * 2);
						}
					}
					else if (AbigailGame.merchantLeaving)
					{
						this.merchantBox.Y = this.merchantBox.Y - 2;
						if (this.merchantBox.Y <= -AbigailGame.TileSize)
						{
							AbigailGame.shopping = false;
							AbigailGame.merchantLeaving = false;
							AbigailGame.merchantArriving = true;
						}
					}
					else if (AbigailGame.merchantShopOpen)
					{
						for (int i3 = this.storeItems.Count - 1; i3 >= 0; i3--)
						{
							if (!this.playerBoundingBox.Intersects(this.shoppingCarpetNoPickup) && this.playerBoundingBox.Intersects(this.storeItems.ElementAt(i3).Key) && this.coins >= this.getPriceForItem(this.storeItems.ElementAt(i3).Value))
							{
								Game1.playSound("Cowboy_Secret");
								AbigailGame.holdItemTimer = 2500;
								this.motionPause = 2500;
								AbigailGame.itemToHold = this.storeItems.ElementAt(i3).Value;
								this.storeItems.Remove(this.storeItems.ElementAt(i3).Key);
								AbigailGame.merchantLeaving = true;
								AbigailGame.merchantArriving = false;
								AbigailGame.merchantShopOpen = false;
								this.coins -= this.getPriceForItem(AbigailGame.itemToHold);
								switch (AbigailGame.itemToHold)
								{
								case 0:
								case 1:
								case 2:
									this.fireSpeedLevel++;
									break;
								case 3:
								case 4:
									this.runSpeedLevel++;
									break;
								case 5:
									this.lives++;
									break;
								case 6:
								case 7:
								case 8:
									this.ammoLevel++;
									this.bulletDamage++;
									break;
								case 9:
									this.spreadPistol = true;
									break;
								case 10:
									this.heldItem = new AbigailGame.CowboyPowerup(10, Point.Zero, 9999);
									break;
								}
							}
						}
					}
				}
				this.cactusDanceTimer += (float)time.ElapsedGameTime.Milliseconds;
				this.cactusDanceTimer %= 1600f;
				if (this.shotTimer > 0)
				{
					this.shotTimer -= time.ElapsedGameTime.Milliseconds;
				}
				if (AbigailGame.deathTimer <= 0f && AbigailGame.playerShootingDirections.Count > 0 && this.shotTimer <= 0)
				{
					if (this.activePowerups.ContainsKey(2))
					{
						this.spawnBullets(new int[1], this.playerPosition);
						this.spawnBullets(new int[]
						{
							1
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							2
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							3
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							0,
							1
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							1,
							2
						}, this.playerPosition);
						this.spawnBullets(new int[]
						{
							2,
							3
						}, this.playerPosition);
						int[] expr_1536 = new int[2];
						expr_1536[0] = 3;
						this.spawnBullets(expr_1536, this.playerPosition);
					}
					else if (AbigailGame.playerShootingDirections.Count == 1 || AbigailGame.playerShootingDirections.Last<int>() == (AbigailGame.playerShootingDirections.ElementAt(AbigailGame.playerShootingDirections.Count - 2) + 2) % 4)
					{
						this.spawnBullets(new int[]
						{
							(AbigailGame.playerShootingDirections.Count == 2 && AbigailGame.playerShootingDirections.Last<int>() == (AbigailGame.playerShootingDirections.ElementAt(AbigailGame.playerShootingDirections.Count - 2) + 2) % 4) ? AbigailGame.playerShootingDirections.ElementAt(1) : AbigailGame.playerShootingDirections.ElementAt(0)
						}, this.playerPosition);
					}
					else
					{
						this.spawnBullets(AbigailGame.playerShootingDirections.ToArray(), this.playerPosition);
					}
					Game1.playSound("Cowboy_gunshot");
					this.shotTimer = this.shootingDelay;
					if (this.activePowerups.ContainsKey(3))
					{
						this.shotTimer /= 4;
					}
					for (int i4 = 0; i4 < this.fireSpeedLevel; i4++)
					{
						this.shotTimer = this.shotTimer * 3 / 4;
					}
					if (this.activePowerups.ContainsKey(7))
					{
						this.shotTimer = this.shotTimer * 3 / 2;
					}
					this.shotTimer = Math.Max(this.shotTimer, 20);
				}
				this.updateBullets(time);
				if (AbigailGame.waveTimer > 0 && AbigailGame.betweenWaveTimer <= 0 && AbigailGame.zombieModeTimer <= 0 && !AbigailGame.shootoutLevel && (AbigailGame.overworldSong == null || !AbigailGame.overworldSong.IsPlaying) && Game1.soundBank != null)
				{
					AbigailGame.overworldSong = Game1.soundBank.GetCue("Cowboy_OVERWORLD");
					AbigailGame.overworldSong.Play();
					Game1.musicPlayerVolume = Game1.options.musicVolumeLevel;
					Game1.musicCategory.SetVolume(Game1.musicPlayerVolume);
				}
				if (AbigailGame.deathTimer > 0f)
				{
					AbigailGame.deathTimer -= (float)time.ElapsedGameTime.Milliseconds;
				}
				if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.monsters.Count == 0 && this.isSpawnQueueEmpty() && !AbigailGame.shopping && !AbigailGame.waitingForPlayerToMoveDownAMap)
				{
					AbigailGame.betweenWaveTimer -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.betweenWaveTimer <= 0 && AbigailGame.playingWithAbigail)
					{
						this.startAbigailPortrait(7, "Here they come!");
					}
				}
				else if (AbigailGame.deathTimer <= 0f && !AbigailGame.waitingForPlayerToMoveDownAMap && !AbigailGame.shopping && !AbigailGame.shootoutLevel)
				{
					if (AbigailGame.waveTimer > 0)
					{
						int oldWaveTimer = AbigailGame.waveTimer;
						AbigailGame.waveTimer -= time.ElapsedGameTime.Milliseconds;
						if (AbigailGame.playingWithAbigail && oldWaveTimer > 40000 && AbigailGame.waveTimer <= 40000)
						{
							this.startAbigailPortrait(0, "Half-way done!");
						}
						int u = 0;
						foreach (Vector2 v in this.monsterChances)
						{
							if (Game1.random.NextDouble() < (double)(v.X * (float)((AbigailGame.monsters.Count == 0) ? 2 : 1)))
							{
								int numMonsters = 1;
								while (Game1.random.NextDouble() < (double)v.Y && numMonsters < 15)
								{
									numMonsters++;
								}
								this.spawnQueue[(AbigailGame.whichWave == 11) ? (Game1.random.Next(1, 3) * 2 - 1) : Game1.random.Next(4)].Add(new Point(u, numMonsters));
							}
							u++;
						}
						if (!AbigailGame.hasGopherAppeared && AbigailGame.monsters.Count > 6 && Game1.random.NextDouble() < 0.0004 && AbigailGame.waveTimer > 7000 && AbigailGame.waveTimer < 50000)
						{
							AbigailGame.hasGopherAppeared = true;
							AbigailGame.gopherBox = new Rectangle(Game1.random.Next(16 * AbigailGame.TileSize), Game1.random.Next(16 * AbigailGame.TileSize), AbigailGame.TileSize, AbigailGame.TileSize);
							while (AbigailGame.isCollidingWithMap(AbigailGame.gopherBox) || AbigailGame.isCollidingWithMonster(AbigailGame.gopherBox, null) || Math.Abs((float)AbigailGame.gopherBox.X - this.playerPosition.X) < (float)(AbigailGame.TileSize * 6) || Math.Abs((float)AbigailGame.gopherBox.Y - this.playerPosition.Y) < (float)(AbigailGame.TileSize * 6) || Math.Abs(AbigailGame.gopherBox.X - 8 * AbigailGame.TileSize) < AbigailGame.TileSize * 4 || Math.Abs(AbigailGame.gopherBox.Y - 8 * AbigailGame.TileSize) < AbigailGame.TileSize * 4)
							{
								AbigailGame.gopherBox.X = Game1.random.Next(16 * AbigailGame.TileSize);
								AbigailGame.gopherBox.Y = Game1.random.Next(16 * AbigailGame.TileSize);
							}
							AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(256, 1664, 16, 32), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.gopherBox.X + AbigailGame.TileSize / 2), (float)(AbigailGame.gopherBox.Y - AbigailGame.TileSize + AbigailGame.TileSize / 2)), false, false, (float)AbigailGame.gopherBox.Y / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
							{
								endFunction = new TemporaryAnimatedSprite.endBehavior(this.endOfGopherAnimationBehavior)
							});
						}
					}
					for (int p = 0; p < 4; p++)
					{
						if (this.spawnQueue[p].Count > 0)
						{
							if (this.spawnQueue[p][0].X == 1 || this.spawnQueue[p][0].X == 4)
							{
								List<Vector2> border = Utility.getBorderOfThisRectangle(new Rectangle(0, 0, 16, 16));
								Vector2 tile = border.ElementAt(Game1.random.Next(border.Count));
								while (AbigailGame.isCollidingWithMonster(new Rectangle((int)tile.X * AbigailGame.TileSize, (int)tile.Y * AbigailGame.TileSize, AbigailGame.TileSize, AbigailGame.TileSize), null))
								{
									tile = border.ElementAt(Game1.random.Next(border.Count));
								}
								AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[p][0].X, new Point((int)tile.X * AbigailGame.TileSize, (int)tile.Y * AbigailGame.TileSize)));
								if (this.whichRound > 0)
								{
									AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
								}
								this.spawnQueue[p][0] = new Point(this.spawnQueue[p][0].X, this.spawnQueue[p][0].Y - 1);
								if (this.spawnQueue[p][0].Y <= 0)
								{
									this.spawnQueue[p].RemoveAt(0);
								}
							}
							else
							{
								switch (p)
								{
								case 0:
								{
									int x = 7;
									while (x < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(x * 16 * 3, 0, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[p].First<Point>().X, new Point(x * AbigailGame.TileSize, 0)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[p][0] = new Point(this.spawnQueue[p][0].X, this.spawnQueue[p][0].Y - 1);
											if (this.spawnQueue[p][0].Y <= 0)
											{
												this.spawnQueue[p].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											x++;
										}
									}
									break;
								}
								case 1:
								{
									int y = 7;
									while (y < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(720, y * AbigailGame.TileSize, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[p].First<Point>().X, new Point(15 * AbigailGame.TileSize, y * AbigailGame.TileSize)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[p][0] = new Point(this.spawnQueue[p][0].X, this.spawnQueue[p][0].Y - 1);
											if (this.spawnQueue[p][0].Y <= 0)
											{
												this.spawnQueue[p].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											y++;
										}
									}
									break;
								}
								case 2:
								{
									int x2 = 7;
									while (x2 < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(x2 * 16 * 3, 15 * AbigailGame.TileSize, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[p].First<Point>().X, new Point(x2 * AbigailGame.TileSize, 15 * AbigailGame.TileSize)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[p][0] = new Point(this.spawnQueue[p][0].X, this.spawnQueue[p][0].Y - 1);
											if (this.spawnQueue[p][0].Y <= 0)
											{
												this.spawnQueue[p].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											x2++;
										}
									}
									break;
								}
								case 3:
								{
									int y2 = 7;
									while (y2 < 10)
									{
										if (Game1.random.NextDouble() < 0.5 && !AbigailGame.isCollidingWithMonster(new Rectangle(0, y2 * AbigailGame.TileSize, 48, 48), null))
										{
											AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(this.spawnQueue[p].First<Point>().X, new Point(0, y2 * AbigailGame.TileSize)));
											if (this.whichRound > 0)
											{
												AbigailGame.monsters.Last<AbigailGame.CowboyMonster>().health += this.whichRound * 2;
											}
											this.spawnQueue[p][0] = new Point(this.spawnQueue[p][0].X, this.spawnQueue[p][0].Y - 1);
											if (this.spawnQueue[p][0].Y <= 0)
											{
												this.spawnQueue[p].RemoveAt(0);
												break;
											}
											break;
										}
										else
										{
											y2++;
										}
									}
									break;
								}
								}
							}
						}
					}
					if (AbigailGame.waveTimer <= 0 && AbigailGame.monsters.Count > 0 && this.isSpawnQueueEmpty())
					{
						bool onlySpikeys = true;
						using (List<AbigailGame.CowboyMonster>.Enumerator enumerator2 = AbigailGame.monsters.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.type != 6)
								{
									onlySpikeys = false;
									break;
								}
							}
						}
						if (onlySpikeys)
						{
							using (List<AbigailGame.CowboyMonster>.Enumerator enumerator2 = AbigailGame.monsters.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									enumerator2.Current.health = 1;
								}
							}
						}
					}
					if (AbigailGame.waveTimer <= 0 && AbigailGame.monsters.Count == 0 && this.isSpawnQueueEmpty())
					{
						AbigailGame.hasGopherAppeared = false;
						if (AbigailGame.playingWithAbigail)
						{
							this.startAbigailPortrait(1, "Hey! We did it!");
						}
						AbigailGame.waveTimer = 80000;
						AbigailGame.betweenWaveTimer = 3333;
						AbigailGame.whichWave++;
						if (AbigailGame.playingWithAbigail)
						{
							AbigailGame.beatLevelWithAbigail = true;
							this.fadethenQuitTimer = 2000;
						}
						switch (AbigailGame.whichWave)
						{
						case 1:
						case 2:
						case 3:
							this.monsterChances[0] = new Vector2(this.monsterChances[0].X + 0.001f, this.monsterChances[0].Y + 0.02f);
							if (AbigailGame.whichWave > 1)
							{
								this.monsterChances[2] = new Vector2(this.monsterChances[2].X + 0.001f, this.monsterChances[2].Y + 0.01f);
							}
							this.monsterChances[6] = new Vector2(this.monsterChances[6].X + 0.001f, this.monsterChances[6].Y + 0.01f);
							if (this.whichRound > 0)
							{
								this.monsterChances[4] = new Vector2(0.002f, 0.1f);
							}
							break;
						case 4:
						case 5:
						case 6:
						case 7:
							if (this.monsterChances[5].Equals(Vector2.Zero))
							{
								this.monsterChances[5] = new Vector2(0.01f, 0.15f);
								if (this.whichRound > 0)
								{
									this.monsterChances[5] = new Vector2(0.01f + (float)this.whichRound * 0.004f, 0.15f + (float)this.whichRound * 0.04f);
								}
							}
							this.monsterChances[0] = Vector2.Zero;
							this.monsterChances[6] = Vector2.Zero;
							this.monsterChances[2] = new Vector2(this.monsterChances[2].X + 0.002f, this.monsterChances[2].Y + 0.02f);
							this.monsterChances[5] = new Vector2(this.monsterChances[5].X + 0.001f, this.monsterChances[5].Y + 0.02f);
							this.monsterChances[1] = new Vector2(this.monsterChances[1].X + 0.0018f, this.monsterChances[1].Y + 0.08f);
							if (this.whichRound > 0)
							{
								this.monsterChances[4] = new Vector2(0.001f, 0.1f);
							}
							break;
						case 8:
						case 9:
						case 10:
						case 11:
							this.monsterChances[5] = Vector2.Zero;
							this.monsterChances[1] = Vector2.Zero;
							this.monsterChances[2] = Vector2.Zero;
							if (this.monsterChances[3].Equals(Vector2.Zero))
							{
								this.monsterChances[3] = new Vector2(0.012f, 0.4f);
								if (this.whichRound > 0)
								{
									this.monsterChances[3] = new Vector2(0.012f + (float)this.whichRound * 0.005f, 0.4f + (float)this.whichRound * 0.075f);
								}
							}
							if (this.monsterChances[4].Equals(Vector2.Zero))
							{
								this.monsterChances[4] = new Vector2(0.003f, 0.1f);
							}
							this.monsterChances[3] = new Vector2(this.monsterChances[3].X + 0.002f, this.monsterChances[3].Y + 0.05f);
							this.monsterChances[4] = new Vector2(this.monsterChances[4].X + 0.0015f, this.monsterChances[4].Y + 0.04f);
							if (AbigailGame.whichWave == 11)
							{
								this.monsterChances[4] = new Vector2(this.monsterChances[4].X + 0.01f, this.monsterChances[4].Y + 0.04f);
								this.monsterChances[3] = new Vector2(this.monsterChances[3].X - 0.01f, this.monsterChances[3].Y + 0.04f);
							}
							break;
						}
						if (this.whichRound > 0)
						{
							for (int i5 = 0; i5 < this.monsterChances.Count<Vector2>(); i5++)
							{
								Vector2 arg_26D5_0 = this.monsterChances[i5];
								List<Vector2> list = this.monsterChances;
								int num = i5;
								list[num] *= 1.1f;
							}
						}
						if (AbigailGame.whichWave > 0 && AbigailGame.whichWave % 2 == 0)
						{
							this.startShoppingLevel();
						}
						else if (AbigailGame.whichWave > 0)
						{
							AbigailGame.waitingForPlayerToMoveDownAMap = true;
							if (!AbigailGame.playingWithAbigail)
							{
								AbigailGame.map[8, 15] = 3;
								AbigailGame.map[7, 15] = 3;
								AbigailGame.map[9, 15] = 3;
							}
						}
					}
				}
				if (AbigailGame.playingWithAbigail)
				{
					this.updateAbigail(time);
				}
				for (int i6 = AbigailGame.monsters.Count - 1; i6 >= 0; i6--)
				{
					AbigailGame.monsters[i6].move(this.playerPosition, time);
					if (i6 < AbigailGame.monsters.Count && AbigailGame.monsters[i6].position.Intersects(this.playerBoundingBox) && AbigailGame.playerInvincibleTimer <= 0)
					{
						if (AbigailGame.zombieModeTimer <= 0)
						{
							this.playerDie();
							break;
						}
						AbigailGame.addGuts(AbigailGame.monsters[i6].position.Location, AbigailGame.monsters[i6].type);
						AbigailGame.monsters.RemoveAt(i6);
						Game1.playSound("Cowboy_monsterDie");
					}
					if (AbigailGame.playingWithAbigail && i6 < AbigailGame.monsters.Count && AbigailGame.monsters[i6].position.Intersects(this.player2BoundingBox) && this.player2invincibletimer <= 0)
					{
						Game1.playSound("Cowboy_monsterDie");
						this.player2deathtimer = 3000;
						AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1808, 16, 16), 120f, 5, 0, AbigailGame.player2Position + AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true));
						this.player2invincibletimer = 4000;
						AbigailGame.player2Position = new Vector2(8f, 8f) * (float)AbigailGame.TileSize;
						this.startAbigailPortrait(5, (Game1.random.NextDouble() < 0.5) ? "Urghh..." : "Nooo!");
					}
				}
			}
			return false;
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x000FA7AC File Offset: 0x000F89AC
		public void updateAbigail(GameTime time)
		{
			this.player2TargetUpdateTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.player2deathtimer > 0)
			{
				this.player2deathtimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.player2invincibletimer > 0)
			{
				this.player2invincibletimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.player2deathtimer <= 0)
			{
				if (this.player2TargetUpdateTimer < 0)
				{
					this.player2TargetUpdateTimer = 500;
					AbigailGame.CowboyMonster closest = null;
					double closestDistance = 99999.0;
					foreach (AbigailGame.CowboyMonster i in AbigailGame.monsters)
					{
						double distance = Math.Sqrt(Math.Pow((double)((float)i.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)i.position.Y - AbigailGame.player2Position.Y), 2.0));
						if (closest == null || distance < closestDistance)
						{
							closest = i;
							closestDistance = Math.Sqrt(Math.Pow((double)((float)closest.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)closest.position.Y - AbigailGame.player2Position.Y), 2.0));
						}
					}
					this.targetMonster = closest;
				}
				this.player2ShootingDirections.Clear();
				this.player2MovementDirections.Clear();
				if (this.targetMonster != null)
				{
					if (Math.Sqrt(Math.Pow((double)((float)this.targetMonster.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)this.targetMonster.position.Y - AbigailGame.player2Position.Y), 2.0)) < (double)(AbigailGame.TileSize * 3))
					{
						if ((float)this.targetMonster.position.X > AbigailGame.player2Position.X)
						{
							this.addPlayer2MovementDirection(3);
						}
						else if ((float)this.targetMonster.position.X < AbigailGame.player2Position.X)
						{
							this.addPlayer2MovementDirection(1);
						}
						if ((float)this.targetMonster.position.Y > AbigailGame.player2Position.Y)
						{
							this.addPlayer2MovementDirection(0);
						}
						else if ((float)this.targetMonster.position.Y < AbigailGame.player2Position.Y)
						{
							this.addPlayer2MovementDirection(2);
						}
						using (List<int>.Enumerator enumerator2 = this.player2MovementDirections.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								int j = enumerator2.Current;
								this.player2ShootingDirections.Add((j + 2) % 4);
							}
							goto IL_4DC;
						}
					}
					if (Math.Abs((float)this.targetMonster.position.X - AbigailGame.player2Position.X) > Math.Abs((float)this.targetMonster.position.Y - AbigailGame.player2Position.Y) && Math.Abs((float)this.targetMonster.position.Y - AbigailGame.player2Position.Y) > 4f)
					{
						if ((float)this.targetMonster.position.Y > AbigailGame.player2Position.Y + 3f)
						{
							this.addPlayer2MovementDirection(2);
						}
						else if ((float)this.targetMonster.position.Y < AbigailGame.player2Position.Y - 3f)
						{
							this.addPlayer2MovementDirection(0);
						}
					}
					else if (Math.Abs((float)this.targetMonster.position.X - AbigailGame.player2Position.X) > 4f)
					{
						if ((float)this.targetMonster.position.X > AbigailGame.player2Position.X + 3f)
						{
							this.addPlayer2MovementDirection(1);
						}
						else if ((float)this.targetMonster.position.X < AbigailGame.player2Position.X - 3f)
						{
							this.addPlayer2MovementDirection(3);
						}
					}
					if ((float)this.targetMonster.position.X > AbigailGame.player2Position.X + 3f)
					{
						this.addPlayer2ShootingDirection(1);
					}
					else if ((float)this.targetMonster.position.X < AbigailGame.player2Position.X - 3f)
					{
						this.addPlayer2ShootingDirection(3);
					}
					if ((float)this.targetMonster.position.Y > AbigailGame.player2Position.Y + 3f)
					{
						this.addPlayer2ShootingDirection(2);
					}
					else if ((float)this.targetMonster.position.Y < AbigailGame.player2Position.Y - 3f)
					{
						this.addPlayer2ShootingDirection(0);
					}
				}
				IL_4DC:
				if (this.player2MovementDirections.Count > 0)
				{
					float speed = this.getMovementSpeed(3f, this.player2MovementDirections.Count);
					for (int k = 0; k < this.player2MovementDirections.Count; k++)
					{
						Vector2 newPlayerPosition = AbigailGame.player2Position;
						switch (this.player2MovementDirections[k])
						{
						case 0:
							newPlayerPosition.Y -= speed;
							break;
						case 1:
							newPlayerPosition.X += speed;
							break;
						case 2:
							newPlayerPosition.Y += speed;
							break;
						case 3:
							newPlayerPosition.X -= speed;
							break;
						}
						Rectangle newPlayerBox = new Rectangle((int)newPlayerPosition.X + AbigailGame.TileSize / 4, (int)newPlayerPosition.Y + AbigailGame.TileSize / 4, AbigailGame.TileSize / 2, AbigailGame.TileSize / 2);
						if (!AbigailGame.isCollidingWithMap(newPlayerBox) && (!this.merchantBox.Intersects(newPlayerBox) || this.merchantBox.Intersects(this.player2BoundingBox)) && !newPlayerBox.Intersects(this.playerBoundingBox))
						{
							AbigailGame.player2Position = newPlayerPosition;
						}
					}
					this.player2BoundingBox.X = (int)AbigailGame.player2Position.X + AbigailGame.TileSize / 4;
					this.player2BoundingBox.Y = (int)AbigailGame.player2Position.Y + AbigailGame.TileSize / 4;
					this.player2BoundingBox.Width = AbigailGame.TileSize / 2;
					this.player2BoundingBox.Height = AbigailGame.TileSize / 2;
					this.player2AnimationTimer += time.ElapsedGameTime.Milliseconds;
					this.player2AnimationTimer %= 400;
					this.player2FootstepSoundTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.player2FootstepSoundTimer <= 0)
					{
						Game1.playSound("Cowboy_Footstep");
						this.player2FootstepSoundTimer = 200;
					}
					for (int l = AbigailGame.powerups.Count - 1; l >= 0; l--)
					{
						if (this.player2BoundingBox.Intersects(new Rectangle(AbigailGame.powerups[l].position.X, AbigailGame.powerups[l].position.Y, AbigailGame.TileSize, AbigailGame.TileSize)) && !this.player2BoundingBox.Intersects(this.noPickUpBox))
						{
							AbigailGame.powerups.RemoveAt(l);
						}
					}
				}
				this.player2shotTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.player2ShootingDirections.Count > 0 && this.player2shotTimer <= 0)
				{
					if (this.player2ShootingDirections.Count == 1)
					{
						this.spawnBullets(new int[]
						{
							this.player2ShootingDirections[0]
						}, AbigailGame.player2Position);
					}
					else
					{
						this.spawnBullets(this.player2ShootingDirections.ToArray(), AbigailGame.player2Position);
					}
					Game1.playSound("Cowboy_gunshot");
					this.player2shotTimer = this.shootingDelay;
				}
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x000FAFB0 File Offset: 0x000F91B0
		public int[,] getMap(int wave)
		{
			int[,] newMap = new int[16, 16];
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if ((i == 0 || i == 15 || j == 0 || j == 15) && (i <= 6 || i >= 10) && (j <= 6 || j >= 10))
					{
						newMap[i, j] = 5;
					}
					else if (i == 0 || i == 15 || j == 0 || j == 15)
					{
						newMap[i, j] = ((Game1.random.NextDouble() < 0.15) ? 1 : 0);
					}
					else if (i == 1 || i == 14 || j == 1 || j == 14)
					{
						newMap[i, j] = 2;
					}
					else
					{
						newMap[i, j] = ((Game1.random.NextDouble() < 0.1) ? 4 : 3);
					}
				}
			}
			switch (wave)
			{
			case -1:
				for (int k = 0; k < 16; k++)
				{
					for (int l = 0; l < 16; l++)
					{
						if (newMap[k, l] == 0 || newMap[k, l] == 1 || newMap[k, l] == 2 || newMap[k, l] == 5)
						{
							newMap[k, l] = 3;
						}
					}
				}
				newMap[3, 1] = 5;
				newMap[8, 2] = 5;
				newMap[13, 1] = 5;
				newMap[5, 0] = 0;
				newMap[10, 2] = 2;
				newMap[15, 2] = 1;
				newMap[14, 12] = 5;
				newMap[10, 6] = 7;
				newMap[11, 6] = 7;
				newMap[12, 6] = 7;
				newMap[13, 6] = 7;
				newMap[14, 6] = 7;
				newMap[14, 7] = 7;
				newMap[14, 8] = 7;
				newMap[14, 9] = 7;
				newMap[14, 10] = 7;
				newMap[14, 11] = 7;
				newMap[14, 12] = 7;
				newMap[14, 13] = 7;
				for (int m = 0; m < 16; m++)
				{
					newMap[m, 3] = ((m % 2 == 0) ? 9 : 8);
				}
				newMap[3, 3] = 10;
				newMap[7, 8] = 2;
				newMap[8, 8] = 2;
				newMap[4, 11] = 2;
				newMap[11, 12] = 2;
				newMap[9, 11] = 2;
				newMap[3, 9] = 2;
				newMap[2, 12] = 5;
				newMap[8, 13] = 5;
				newMap[12, 11] = 5;
				newMap[7, 14] = 0;
				newMap[6, 14] = 2;
				newMap[8, 14] = 2;
				newMap[7, 13] = 2;
				newMap[7, 15] = 2;
				return newMap;
			case 1:
				newMap[4, 4] = 7;
				newMap[4, 5] = 7;
				newMap[5, 4] = 7;
				newMap[12, 4] = 7;
				newMap[11, 4] = 7;
				newMap[12, 5] = 7;
				newMap[4, 12] = 7;
				newMap[5, 12] = 7;
				newMap[4, 11] = 7;
				newMap[12, 12] = 7;
				newMap[11, 12] = 7;
				newMap[12, 11] = 7;
				return newMap;
			case 2:
				newMap[8, 4] = 7;
				newMap[12, 8] = 7;
				newMap[8, 12] = 7;
				newMap[4, 8] = 7;
				newMap[1, 1] = 5;
				newMap[14, 1] = 5;
				newMap[14, 14] = 5;
				newMap[1, 14] = 5;
				newMap[2, 1] = 5;
				newMap[13, 1] = 5;
				newMap[13, 14] = 5;
				newMap[2, 14] = 5;
				newMap[1, 2] = 5;
				newMap[14, 2] = 5;
				newMap[14, 13] = 5;
				newMap[1, 13] = 5;
				return newMap;
			case 3:
				newMap[5, 5] = 7;
				newMap[6, 5] = 7;
				newMap[7, 5] = 7;
				newMap[9, 5] = 7;
				newMap[10, 5] = 7;
				newMap[11, 5] = 7;
				newMap[5, 11] = 7;
				newMap[6, 11] = 7;
				newMap[7, 11] = 7;
				newMap[9, 11] = 7;
				newMap[10, 11] = 7;
				newMap[11, 11] = 7;
				newMap[5, 6] = 7;
				newMap[5, 7] = 7;
				newMap[5, 9] = 7;
				newMap[5, 10] = 7;
				newMap[11, 6] = 7;
				newMap[11, 7] = 7;
				newMap[11, 9] = 7;
				newMap[11, 10] = 7;
				return newMap;
			case 4:
			case 8:
				for (int n = 0; n < 16; n++)
				{
					for (int j2 = 0; j2 < 16; j2++)
					{
						if (newMap[n, j2] == 5)
						{
							newMap[n, j2] = ((Game1.random.NextDouble() < 0.5) ? 0 : 1);
						}
					}
				}
				for (int i2 = 0; i2 < 16; i2++)
				{
					newMap[i2, 8] = ((Game1.random.NextDouble() < 0.5) ? 8 : 9);
				}
				newMap[8, 4] = 7;
				newMap[8, 12] = 7;
				newMap[9, 12] = 7;
				newMap[7, 12] = 7;
				newMap[5, 6] = 5;
				newMap[10, 6] = 5;
				return newMap;
			case 5:
				newMap[1, 1] = 5;
				newMap[14, 1] = 5;
				newMap[14, 14] = 5;
				newMap[1, 14] = 5;
				newMap[2, 1] = 5;
				newMap[13, 1] = 5;
				newMap[13, 14] = 5;
				newMap[2, 14] = 5;
				newMap[1, 2] = 5;
				newMap[14, 2] = 5;
				newMap[14, 13] = 5;
				newMap[1, 13] = 5;
				newMap[3, 1] = 5;
				newMap[13, 1] = 5;
				newMap[13, 13] = 5;
				newMap[1, 13] = 5;
				newMap[1, 3] = 5;
				newMap[13, 3] = 5;
				newMap[12, 13] = 5;
				newMap[3, 14] = 5;
				newMap[3, 3] = 5;
				newMap[13, 12] = 5;
				newMap[13, 12] = 5;
				newMap[3, 12] = 5;
				return newMap;
			case 6:
				newMap[4, 5] = 2;
				newMap[12, 10] = 5;
				newMap[10, 9] = 5;
				newMap[5, 12] = 2;
				newMap[5, 9] = 5;
				newMap[12, 12] = 5;
				newMap[3, 4] = 5;
				newMap[2, 3] = 5;
				newMap[11, 3] = 5;
				newMap[10, 6] = 5;
				newMap[5, 9] = 7;
				newMap[10, 12] = 7;
				newMap[3, 12] = 7;
				newMap[10, 8] = 7;
				return newMap;
			case 7:
				for (int i3 = 0; i3 < 16; i3++)
				{
					newMap[i3, 5] = ((i3 % 2 == 0) ? 9 : 8);
					newMap[i3, 10] = ((i3 % 2 == 0) ? 9 : 8);
				}
				newMap[4, 5] = 10;
				newMap[8, 5] = 10;
				newMap[12, 5] = 10;
				newMap[4, 10] = 10;
				newMap[8, 10] = 10;
				newMap[12, 10] = 10;
				return newMap;
			case 9:
				newMap[4, 4] = 5;
				newMap[5, 4] = 5;
				newMap[10, 4] = 5;
				newMap[12, 4] = 5;
				newMap[4, 5] = 5;
				newMap[5, 5] = 5;
				newMap[10, 5] = 5;
				newMap[12, 5] = 5;
				newMap[4, 10] = 5;
				newMap[5, 10] = 5;
				newMap[10, 10] = 5;
				newMap[12, 10] = 5;
				newMap[4, 12] = 5;
				newMap[5, 12] = 5;
				newMap[10, 12] = 5;
				newMap[12, 12] = 5;
				return newMap;
			case 10:
				for (int i4 = 0; i4 < 16; i4++)
				{
					newMap[i4, 1] = ((i4 % 2 == 0) ? 9 : 8);
					newMap[i4, 14] = ((i4 % 2 == 0) ? 9 : 8);
				}
				newMap[8, 1] = 10;
				newMap[7, 1] = 10;
				newMap[9, 1] = 10;
				newMap[8, 14] = 10;
				newMap[7, 14] = 10;
				newMap[9, 14] = 10;
				newMap[6, 8] = 5;
				newMap[10, 8] = 5;
				newMap[8, 6] = 5;
				newMap[8, 9] = 5;
				return newMap;
			case 11:
				for (int i5 = 0; i5 < 16; i5++)
				{
					newMap[i5, 0] = 7;
					newMap[i5, 15] = 7;
					if (i5 % 2 == 0)
					{
						newMap[i5, 1] = 5;
						newMap[i5, 14] = 5;
					}
				}
				return newMap;
			case 12:
			{
				for (int i6 = 0; i6 < 16; i6++)
				{
					for (int j3 = 0; j3 < 16; j3++)
					{
						if (newMap[i6, j3] == 0 || newMap[i6, j3] == 1)
						{
							newMap[i6, j3] = 5;
						}
					}
				}
				for (int i7 = 0; i7 < 16; i7++)
				{
					newMap[i7, 0] = ((i7 % 2 == 0) ? 9 : 8);
					newMap[i7, 15] = ((i7 % 2 == 0) ? 9 : 8);
				}
				Rectangle r = new Rectangle(1, 1, 14, 14);
				foreach (Vector2 v in Utility.getBorderOfThisRectangle(r))
				{
					newMap[(int)v.X, (int)v.Y] = 10;
				}
				r.Inflate(-1, -1);
				using (List<Vector2>.Enumerator enumerator = Utility.getBorderOfThisRectangle(r).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Vector2 v2 = enumerator.Current;
						newMap[(int)v2.X, (int)v2.Y] = 2;
					}
					return newMap;
				}
				break;
			}
			}
			newMap[4, 4] = 5;
			newMap[12, 4] = 5;
			newMap[4, 12] = 5;
			newMap[12, 12] = 5;
			return newMap;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000FBA6C File Offset: 0x000F9C6C
		public void spawnBullets(int[] directions, Vector2 spawn)
		{
			Point bulletSpawn = new Point((int)spawn.X + 24, (int)spawn.Y + 24 - 6);
			int speed = (int)this.getMovementSpeed(8f, 2);
			if (directions.Length == 1)
			{
				int playerShootingDirection = directions[0];
				switch (playerShootingDirection)
				{
				case 0:
					bulletSpawn.Y -= 22;
					break;
				case 1:
					bulletSpawn.X += 16;
					bulletSpawn.Y -= 6;
					break;
				case 2:
					bulletSpawn.Y += 10;
					break;
				case 3:
					bulletSpawn.X -= 16;
					bulletSpawn.Y -= 6;
					break;
				}
				this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, playerShootingDirection, this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					switch (playerShootingDirection)
					{
					case 0:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(-2, -8), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(2, -8), this.bulletDamage));
						return;
					case 1:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(8, -2), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(8, 2), this.bulletDamage));
						return;
					case 2:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(-2, 8), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(2, 8), this.bulletDamage));
						return;
					case 3:
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(-8, -2), this.bulletDamage));
						this.bullets.Add(new AbigailGame.CowboyBullet(new Point(bulletSpawn.X, bulletSpawn.Y), new Point(-8, 2), this.bulletDamage));
						return;
					default:
						return;
					}
				}
			}
			else if (directions.Contains(0) && directions.Contains(1))
			{
				bulletSpawn.X += AbigailGame.TileSize / 2;
				bulletSpawn.Y -= AbigailGame.TileSize / 2;
				this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(speed, -speed), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int modifier = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(speed + modifier, -speed + modifier), this.bulletDamage));
					modifier = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(speed + modifier, -speed + modifier), this.bulletDamage));
					return;
				}
			}
			else if (directions.Contains(0) && directions.Contains(3))
			{
				bulletSpawn.X -= AbigailGame.TileSize / 2;
				bulletSpawn.Y -= AbigailGame.TileSize / 2;
				this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(-speed, -speed), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int modifier2 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(-speed - modifier2, -speed + modifier2), this.bulletDamage));
					modifier2 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(-speed - modifier2, -speed + modifier2), this.bulletDamage));
					return;
				}
			}
			else if (directions.Contains(2) && directions.Contains(1))
			{
				bulletSpawn.X += AbigailGame.TileSize / 2;
				bulletSpawn.Y += AbigailGame.TileSize / 4;
				this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(speed, speed), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int modifier3 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(speed - modifier3, speed + modifier3), this.bulletDamage));
					modifier3 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(speed - modifier3, speed + modifier3), this.bulletDamage));
					return;
				}
			}
			else if (directions.Contains(2) && directions.Contains(3))
			{
				bulletSpawn.X -= AbigailGame.TileSize / 2;
				bulletSpawn.Y += AbigailGame.TileSize / 4;
				this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(-speed, speed), this.bulletDamage));
				if (this.activePowerups.ContainsKey(7) || this.spreadPistol)
				{
					int modifier4 = -2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(-speed + modifier4, speed + modifier4), this.bulletDamage));
					modifier4 = 2;
					this.bullets.Add(new AbigailGame.CowboyBullet(bulletSpawn, new Point(-speed + modifier4, speed + modifier4), this.bulletDamage));
				}
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000FBFF0 File Offset: 0x000FA1F0
		public bool isSpawnQueueEmpty()
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.spawnQueue[i].Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000FC01C File Offset: 0x000FA21C
		public static bool isMapTilePassable(int tileType)
		{
			switch (tileType)
			{
			case 0:
			case 1:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return false;
			}
			return true;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000FC051 File Offset: 0x000FA251
		public static bool isMapTilePassableForMonsters(int tileType)
		{
			switch (tileType)
			{
			case 5:
			case 7:
			case 8:
			case 9:
				return false;
			}
			return true;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x000FC074 File Offset: 0x000FA274
		public static bool isCollidingWithMonster(Rectangle r, AbigailGame.CowboyMonster subject)
		{
			foreach (AbigailGame.CowboyMonster c in AbigailGame.monsters)
			{
				if ((subject == null || !subject.Equals(c)) && Math.Abs(c.position.X - r.X) < 48 && Math.Abs(c.position.Y - r.Y) < 48 && r.Intersects(new Rectangle(c.position.X, c.position.Y, 48, 48)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000FC130 File Offset: 0x000FA330
		public static bool isCollidingWithMapForMonsters(Rectangle positionToCheck)
		{
			foreach (Vector2 p in Utility.getCornersOfThisRectangle(positionToCheck))
			{
				if (p.X < 0f || p.Y < 0f || p.X >= 768f || p.Y >= 768f || !AbigailGame.isMapTilePassableForMonsters(AbigailGame.map[(int)p.X / 16 / 3, (int)p.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x000FC1E0 File Offset: 0x000FA3E0
		public static bool isCollidingWithMap(Rectangle positionToCheck)
		{
			foreach (Vector2 p in Utility.getCornersOfThisRectangle(positionToCheck))
			{
				if (p.X < 0f || p.Y < 0f || p.X >= 768f || p.Y >= 768f || !AbigailGame.isMapTilePassable(AbigailGame.map[(int)p.X / 16 / 3, (int)p.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000FC290 File Offset: 0x000FA490
		public static bool isCollidingWithMap(Point position)
		{
			foreach (Vector2 p in Utility.getCornersOfThisRectangle(new Rectangle(position.X, position.Y, 48, 48)))
			{
				if (p.X < 0f || p.Y < 0f || p.X >= 768f || p.Y >= 768f || !AbigailGame.isMapTilePassable(AbigailGame.map[(int)p.X / 16 / 3, (int)p.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000FC354 File Offset: 0x000FA554
		public static bool isCollidingWithMap(Vector2 position)
		{
			foreach (Vector2 p in Utility.getCornersOfThisRectangle(new Rectangle((int)position.X, (int)position.Y, 48, 48)))
			{
				if (p.X < 0f || p.Y < 0f || p.X >= 768f || p.Y >= 768f || !AbigailGame.isMapTilePassable(AbigailGame.map[(int)p.X / 16 / 3, (int)p.Y / 16 / 3]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000FC41C File Offset: 0x000FA61C
		private void addPlayer2MovementDirection(int direction)
		{
			if (!this.player2MovementDirections.Contains(direction))
			{
				if (this.player2MovementDirections.Count == 1 && direction == (this.player2MovementDirections[0] + 2) % 4)
				{
					this.player2MovementDirections.Clear();
				}
				this.player2MovementDirections.Add(direction);
				if (this.player2MovementDirections.Count > 2)
				{
					this.player2MovementDirections.RemoveAt(0);
				}
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000FC489 File Offset: 0x000FA689
		private void addPlayerMovementDirection(int direction)
		{
			if (!AbigailGame.playerMovementDirections.Contains(direction))
			{
				if (AbigailGame.playerMovementDirections.Count == 1)
				{
					int arg_2A_0 = (AbigailGame.playerMovementDirections.ElementAt(0) + 2) % 4;
				}
				AbigailGame.playerMovementDirections.Add(direction);
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x000FC4C4 File Offset: 0x000FA6C4
		private void addPlayer2ShootingDirection(int direction)
		{
			if (!this.player2ShootingDirections.Contains(direction))
			{
				if (this.player2ShootingDirections.Count == 1 && direction == (this.player2ShootingDirections[0] + 2) % 4)
				{
					this.player2ShootingDirections.Clear();
				}
				this.player2ShootingDirections.Add(direction);
				if (this.player2ShootingDirections.Count > 2)
				{
					this.player2ShootingDirections.RemoveAt(0);
				}
			}
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x000FC531 File Offset: 0x000FA731
		private void addPlayerShootingDirection(int direction)
		{
			if (!AbigailGame.playerShootingDirections.Contains(direction))
			{
				AbigailGame.playerShootingDirections.Add(direction);
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x000FC54C File Offset: 0x000FA74C
		public void startShoppingLevel()
		{
			this.merchantBox.Y = -AbigailGame.TileSize;
			AbigailGame.shopping = true;
			AbigailGame.merchantArriving = true;
			AbigailGame.merchantLeaving = false;
			AbigailGame.merchantShopOpen = false;
			if (AbigailGame.overworldSong != null)
			{
				AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
			}
			AbigailGame.monsters.Clear();
			AbigailGame.waitingForPlayerToMoveDownAMap = true;
			this.storeItems.Clear();
			int num = AbigailGame.whichWave;
			if (num == 2)
			{
				this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 3);
				this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 0);
				this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), 6);
			}
			else
			{
				this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.runSpeedLevel >= 2) ? 5 : (3 + this.runSpeedLevel));
				this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.fireSpeedLevel >= 3) ? ((this.ammoLevel >= 3 && !this.spreadPistol) ? 9 : 10) : this.fireSpeedLevel);
				this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.ammoLevel < 3) ? (6 + this.ammoLevel) : 10);
			}
			if (this.whichRound > 0)
			{
				this.storeItems.Clear();
				this.storeItems.Add(new Rectangle(7 * AbigailGame.TileSize + 12, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.runSpeedLevel >= 2) ? 5 : (3 + this.runSpeedLevel));
				this.storeItems.Add(new Rectangle(8 * AbigailGame.TileSize + 24, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.fireSpeedLevel >= 3) ? ((this.ammoLevel >= 3 && !this.spreadPistol) ? 9 : 10) : this.fireSpeedLevel);
				this.storeItems.Add(new Rectangle(9 * AbigailGame.TileSize + 36, 8 * AbigailGame.TileSize - AbigailGame.TileSize * 2, AbigailGame.TileSize, AbigailGame.TileSize), (this.ammoLevel < 3) ? (6 + this.ammoLevel) : 10);
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000FC84C File Offset: 0x000FAA4C
		public void receiveKeyPress(Keys k)
		{
			Vector2 arg_06_0 = this.playerPosition;
			if (Game1.options.gamepadControls && (Game1.isAnyGamePadButtonBeingPressed() || Game1.isGamePadThumbstickInMotion()))
			{
				Game1.thumbstickMotionMargin = 0;
				if (Game1.isGamePadThumbstickInMotion() && (Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up) || Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right) || Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down) || Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left)))
				{
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					k = Keys.W;
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
				{
					k = Keys.D;
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					k = Keys.S;
				}
				else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
				{
					k = Keys.A;
				}
				else if (k != Keys.None && Game1.options.doesInputListContain(Game1.options.actionButton, k))
				{
					k = Keys.Down;
				}
				else if (k != Keys.None && Game1.options.doesInputListContain(Game1.options.useToolButton, k))
				{
					k = Keys.Left;
				}
				else if (Game1.options.doesInputListContain(Game1.options.toolSwapButton, k) || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) && !Game1.oldPadState.IsButtonDown(Buttons.Start)) || (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger)))
				{
					k = Keys.Space;
				}
				else if (Game1.options.doesInputListContain(Game1.options.journalButton, k))
				{
					k = Keys.Escape;
				}
				else if (Game1.options.doesInputListContain(Game1.options.menuButton, k))
				{
					k = ((GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y) && !Game1.oldPadState.IsButtonDown(Buttons.Y)) ? Keys.Up : ((GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) && !Game1.oldPadState.IsButtonDown(Buttons.B)) ? Keys.Right : k));
				}
			}
			else
			{
				bool pressed = false;
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k) && !Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up))
				{
					this.addPlayerMovementDirection(0);
					if (AbigailGame.gameOver)
					{
						this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
						Game1.playSound("Cowboy_gunshot");
					}
					pressed = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k) && !Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))
				{
					this.addPlayerMovementDirection(3);
					pressed = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k) && !Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down))
				{
					this.addPlayerMovementDirection(2);
					if (AbigailGame.gameOver)
					{
						this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
						Game1.playSound("Cowboy_gunshot");
					}
					pressed = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k) && !Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right))
				{
					this.addPlayerMovementDirection(1);
					pressed = true;
				}
				if (pressed)
				{
					return;
				}
			}
			if (k <= Keys.Down)
			{
				if (k != Keys.Enter)
				{
					if (k != Keys.Escape)
					{
						switch (k)
						{
						case Keys.Space:
							break;
						case Keys.PageUp:
						case Keys.PageDown:
						case Keys.End:
						case Keys.Home:
							return;
						case Keys.Left:
							this.addPlayerShootingDirection(3);
							return;
						case Keys.Up:
							this.addPlayerShootingDirection(0);
							if (AbigailGame.gameOver)
							{
								this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
								Game1.playSound("Cowboy_gunshot");
								return;
							}
							return;
						case Keys.Right:
							this.addPlayerShootingDirection(1);
							return;
						case Keys.Down:
							this.addPlayerShootingDirection(2);
							if (AbigailGame.gameOver)
							{
								this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
								Game1.playSound("Cowboy_gunshot");
								return;
							}
							return;
						default:
							return;
						}
					}
					else
					{
						if (AbigailGame.playingWithAbigail)
						{
							return;
						}
						Game1.currentMinigame = null;
						if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
						{
							AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
						}
						if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
						{
							AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
						}
						if (Game1.currentLocation != null && Game1.currentLocation.name.Equals("Saloon") && Game1.timeOfDay >= 1700)
						{
							Game1.changeMusicTrack("Saloon1");
							return;
						}
						return;
					}
				}
				if (AbigailGame.gameOver)
				{
					if (this.gameOverOption == 1)
					{
						this.quit = true;
						return;
					}
					this.gamerestartTimer = 1500;
					AbigailGame.gameOver = false;
					this.gameOverOption = 0;
					Game1.playSound("Pickup_Coin15");
					return;
				}
				else
				{
					if (AbigailGame.onStartMenu)
					{
						AbigailGame.startTimer = 1500;
						Game1.playSound("Pickup_Coin15");
						return;
					}
					if (this.heldItem != null && AbigailGame.deathTimer <= 0f && AbigailGame.zombieModeTimer <= 0)
					{
						this.usePowerup(this.heldItem.which);
						this.heldItem = null;
						return;
					}
				}
			}
			else if (k <= Keys.D)
			{
				if (k == Keys.A)
				{
					this.addPlayerMovementDirection(3);
					return;
				}
				if (k != Keys.D)
				{
					return;
				}
				this.addPlayerMovementDirection(1);
				return;
			}
			else if (k != Keys.S)
			{
				if (k == Keys.W)
				{
					this.addPlayerMovementDirection(0);
					if (AbigailGame.gameOver)
					{
						this.gameOverOption = Math.Max(0, this.gameOverOption - 1);
						Game1.playSound("Cowboy_gunshot");
						return;
					}
				}
			}
			else
			{
				this.addPlayerMovementDirection(2);
				if (AbigailGame.gameOver)
				{
					this.gameOverOption = Math.Min(1, this.gameOverOption + 1);
					Game1.playSound("Cowboy_gunshot");
					return;
				}
			}
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000FCE34 File Offset: 0x000FB034
		public void receiveKeyRelease(Keys k)
		{
			if (Game1.options.gamepadControls && Utility.getPressedButtons(Game1.oldPadState, GamePad.GetState(PlayerIndex.One)).Count > 0)
			{
				if (k != Keys.None)
				{
					if (Game1.isGamePadThumbstickInMotion() && (Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up) || Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right) || Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down) || Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left)))
					{
						return;
					}
					if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
					{
						k = Keys.W;
					}
					else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
					{
						k = Keys.D;
					}
					else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
					{
						k = Keys.S;
					}
					else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
					{
						k = Keys.A;
					}
					else if (Game1.options.doesInputListContain(Game1.options.actionButton, k))
					{
						k = Keys.Down;
					}
					else if (Game1.options.doesInputListContain(Game1.options.useToolButton, k))
					{
						k = Keys.Left;
					}
					else if (Game1.options.doesInputListContain(Game1.options.toolSwapButton, k))
					{
						k = Keys.Space;
					}
					else if (Game1.options.doesInputListContain(Game1.options.menuButton, k))
					{
						k = ((Game1.oldPadState.IsButtonDown(Buttons.Y) && !GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y)) ? Keys.Up : ((Game1.oldPadState.IsButtonDown(Buttons.B) && !GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B)) ? Keys.Right : k));
					}
				}
			}
			else
			{
				bool pressed = false;
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k) && !Game1.options.doesInputListContain(Game1.options.moveUpButton, Keys.Up))
				{
					if (AbigailGame.playerMovementDirections.Contains(0))
					{
						AbigailGame.playerMovementDirections.Remove(0);
					}
					pressed = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k) && !Game1.options.doesInputListContain(Game1.options.moveLeftButton, Keys.Left))
				{
					if (AbigailGame.playerMovementDirections.Contains(3))
					{
						AbigailGame.playerMovementDirections.Remove(3);
					}
					pressed = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k) && !Game1.options.doesInputListContain(Game1.options.moveDownButton, Keys.Down))
				{
					if (AbigailGame.playerMovementDirections.Contains(2))
					{
						AbigailGame.playerMovementDirections.Remove(2);
					}
					pressed = true;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k) && !Game1.options.doesInputListContain(Game1.options.moveRightButton, Keys.Right))
				{
					if (AbigailGame.playerMovementDirections.Contains(1))
					{
						AbigailGame.playerMovementDirections.Remove(1);
					}
					pressed = true;
				}
				if (pressed)
				{
					return;
				}
			}
			if (k <= Keys.A)
			{
				switch (k)
				{
				case Keys.Left:
					if (AbigailGame.playerShootingDirections.Contains(3))
					{
						AbigailGame.playerShootingDirections.Remove(3);
					}
					break;
				case Keys.Up:
					if (AbigailGame.playerShootingDirections.Contains(0))
					{
						AbigailGame.playerShootingDirections.Remove(0);
						return;
					}
					break;
				case Keys.Right:
					if (AbigailGame.playerShootingDirections.Contains(1))
					{
						AbigailGame.playerShootingDirections.Remove(1);
						return;
					}
					break;
				case Keys.Down:
					if (AbigailGame.playerShootingDirections.Contains(2))
					{
						AbigailGame.playerShootingDirections.Remove(2);
						return;
					}
					break;
				default:
					if (k != Keys.A)
					{
						return;
					}
					if (AbigailGame.playerMovementDirections.Contains(3))
					{
						AbigailGame.playerMovementDirections.Remove(3);
						return;
					}
					break;
				}
			}
			else if (k != Keys.D)
			{
				if (k != Keys.S)
				{
					if (k == Keys.W && AbigailGame.playerMovementDirections.Contains(0))
					{
						AbigailGame.playerMovementDirections.Remove(0);
						return;
					}
				}
				else if (AbigailGame.playerMovementDirections.Contains(2))
				{
					AbigailGame.playerMovementDirections.Remove(2);
					return;
				}
			}
			else if (AbigailGame.playerMovementDirections.Contains(1))
			{
				AbigailGame.playerMovementDirections.Remove(1);
				return;
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000FD26C File Offset: 0x000FB46C
		public int getPriceForItem(int whichItem)
		{
			switch (whichItem)
			{
			case 0:
				return 10;
			case 1:
				return 20;
			case 2:
				return 30;
			case 3:
				return 8;
			case 4:
				return 20;
			case 5:
				return 10;
			case 6:
				return 15;
			case 7:
				return 30;
			case 8:
				return 45;
			case 9:
				return 99;
			case 10:
				return 10;
			default:
				return 5;
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x000FD2D0 File Offset: 0x000FB4D0
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (AbigailGame.onStartMenu)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.97f);
				b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(6 * AbigailGame.TileSize), (float)(5 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(128, 1744, 96, 72 - ((AbigailGame.startTimer > 0) ? 16 : 0))), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
			}
			else if ((AbigailGame.gameOver || this.gamerestartTimer > 0) && !AbigailGame.endCutscene)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
				b.DrawString(Game1.dialogueFont, "Game Over", AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float)AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				b.DrawString(Game1.dialogueFont, "Game Over", AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float)AbigailGame.TileSize + new Vector2(-1f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				b.DrawString(Game1.dialogueFont, "Game Over", AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 7f) * (float)AbigailGame.TileSize + new Vector2(1f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				string option = "Restart";
				if (this.gameOverOption == 0)
				{
					option = "> " + option;
				}
				string option2 = "Quit";
				if (this.gameOverOption == 1)
				{
					option2 = "> " + option2;
				}
				if (this.gamerestartTimer <= 0 || this.gamerestartTimer / 500 % 2 == 0)
				{
					b.DrawString(Game1.smallFont, option, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * (float)AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				b.DrawString(Game1.smallFont, option2, AbigailGame.topLeftScreenCoordinate + new Vector2(6f, 9f) * (float)AbigailGame.TileSize + new Vector2(0f, (float)(AbigailGame.TileSize * 2 / 3)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
			}
			else if (AbigailGame.endCutscene)
			{
				switch (AbigailGame.endCutscenePhase)
				{
				case 0:
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0001f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White * ((AbigailGame.endCutsceneTimer < 2000) ? (1f * ((float)AbigailGame.endCutsceneTimer / 2000f)) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3)) + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White * ((AbigailGame.endCutsceneTimer < 2000) ? (1f * ((float)AbigailGame.endCutsceneTimer / 2000f)) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f);
					break;
				case 1:
				case 2:
				case 3:
					for (int i = 0; i < 16; i++)
					{
						for (int j = 0; j < 16; j++)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)i, (float)j) * 16f * 3f + new Vector2(0f, (float)(AbigailGame.newMapPosition - 16 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.map[i, j] + ((AbigailGame.map[i, j] == 5 && this.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
						}
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(6 * AbigailGame.TileSize), (float)(3 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(288, 1697, 64, 80)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
					if (AbigailGame.endCutscenePhase == 3)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(9 * AbigailGame.TileSize), (float)(7 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(544, 1792, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.05f);
						if (AbigailGame.endCutsceneTimer < 3000)
						{
							b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * (1f - (float)AbigailGame.endCutsceneTimer / 3000f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
						}
					}
					else
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(10 * AbigailGame.TileSize), (float)(8 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(272 - AbigailGame.endCutsceneTimer / 300 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.02f);
						if (AbigailGame.endCutscenePhase == 2)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(484, 1760 + (int)(this.playerMotionAnimationTimer / 100f) * 3, 8, 3)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f + 0.001f);
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition, new Rectangle?(new Rectangle(384, 1760, 16, 13)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f + 0.001f);
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3 - AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.005f);
						}
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * ((AbigailGame.endCutscenePhase == 1 && AbigailGame.endCutsceneTimer > 12500) ? ((float)((AbigailGame.endCutsceneTimer - 12500) / 3000)) : 0f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
					}
					break;
				case 4:
				case 5:
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.97f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(6 * AbigailGame.TileSize), (float)(3 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(224, 1744, 64, 48)), Color.White * ((AbigailGame.endCutsceneTimer > 0) ? (1f - ((float)AbigailGame.endCutsceneTimer - 2000f) / 2000f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					if (AbigailGame.endCutscenePhase == 5 && this.gamerestartTimer <= 0)
					{
						b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_PK_NewGame+", new object[0]), AbigailGame.topLeftScreenCoordinate + new Vector2(3f, 10f) * (float)AbigailGame.TileSize, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
					}
					break;
				}
			}
			else
			{
				if (AbigailGame.zombieModeTimer > 8200)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition, new Rectangle?(new Rectangle(384 + ((AbigailGame.zombieModeTimer / 200 % 2 == 0) ? 16 : 0), 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					for (int y = (int)(this.playerPosition.Y - (float)AbigailGame.TileSize); y > -AbigailGame.TileSize; y -= AbigailGame.TileSize)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X, (float)y), new Rectangle?(new Rectangle(368 + ((y / AbigailGame.TileSize % 3 == 0) ? 16 : 0), 1744, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					}
					b.End();
					return;
				}
				for (int k = 0; k < 16; k++)
				{
					for (int l = 0; l < 16; l++)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)k, (float)l) * 16f * 3f + new Vector2(0f, (float)(AbigailGame.newMapPosition - 16 * AbigailGame.TileSize)), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.map[k, l] + ((AbigailGame.map[k, l] == 5 && this.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
					}
				}
				if (AbigailGame.scrollingMap)
				{
					for (int m = 0; m < 16; m++)
					{
						for (int n = 0; n < 16; n++)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)m, (float)n) * 16f * 3f + new Vector2(0f, (float)AbigailGame.newMapPosition), new Rectangle?(new Rectangle(464 + 16 * AbigailGame.nextMap[m, n] + ((AbigailGame.nextMap[m, n] == 5 && this.cactusDanceTimer > 800f) ? 16 : 0), 1680 - AbigailGame.world * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
						}
					}
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, -1, 16 * AbigailGame.TileSize, (int)AbigailGame.topLeftScreenCoordinate.Y), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize, (int)AbigailGame.topLeftScreenCoordinate.Y + 2), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
				if (AbigailGame.deathTimer <= 0f && (AbigailGame.playerInvincibleTimer <= 0 || AbigailGame.playerInvincibleTimer / 100 % 2 == 0))
				{
					if (AbigailGame.holdItemTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3)) + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f);
					}
					else if (AbigailGame.zombieModeTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(352 + ((AbigailGame.zombieModeTimer / 50 % 2 == 0) ? 16 : 0), 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					}
					else if (AbigailGame.playerMovementDirections.Count == 0 && AbigailGame.playerShootingDirections.Count == 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(496, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					}
					else
					{
						int facingDirection = (AbigailGame.playerShootingDirections.Count == 0) ? AbigailGame.playerMovementDirections.ElementAt(0) : AbigailGame.playerShootingDirections.Last<int>();
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)) + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(483, 1760 + (int)(this.playerMotionAnimationTimer / 100f) * 3, 10, 3)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(3f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(464 + facingDirection * 16, 1744, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.002f + 0.001f);
					}
				}
				if (AbigailGame.playingWithAbigail && this.player2deathtimer <= 0 && (this.player2invincibletimer <= 0 || this.player2invincibletimer / 100 % 2 == 0))
				{
					if (this.player2MovementDirections.Count == 0 && this.player2ShootingDirections.Count == 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(256, 1728, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, this.playerPosition.Y / 10000f + 0.001f);
					}
					else
					{
						int facingDirection2 = (this.player2ShootingDirections.Count == 0) ? this.player2MovementDirections[0] : this.player2ShootingDirections[0];
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)) + new Vector2(4f, 13f) * 3f, new Rectangle?(new Rectangle(243, 1728 + this.player2AnimationTimer / 100 * 3, 10, 3)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.001f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + AbigailGame.player2Position + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(224 + facingDirection2 * 16, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, AbigailGame.player2Position.Y / 10000f + 0.002f + 0.001f);
					}
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = AbigailGame.temporarySprites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
				using (List<AbigailGame.CowboyPowerup>.Enumerator enumerator2 = AbigailGame.powerups.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b);
					}
				}
				foreach (AbigailGame.CowboyBullet p in this.bullets)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)p.position.X, (float)p.position.Y), new Rectangle?(new Rectangle(518, 1760 + (this.bulletDamage - 1) * 4, 4, 4)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
				}
				foreach (AbigailGame.CowboyBullet p2 in AbigailGame.enemyBullets)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)p2.position.X, (float)p2.position.Y), new Rectangle?(new Rectangle(523, 1760, 5, 5)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.9f);
				}
				if (AbigailGame.shopping)
				{
					if ((AbigailGame.merchantArriving || AbigailGame.merchantLeaving) && !AbigailGame.merchantShopOpen)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.merchantBox.Location.X, (float)this.merchantBox.Location.Y), new Rectangle?(new Rectangle(464 + ((AbigailGame.shoppingTimer / 100 % 2 == 0) ? 16 : 0), 1728, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.merchantBox.Y / 10000f + 0.001f);
					}
					else
					{
						int whichFrame = (this.playerBoundingBox.X - this.merchantBox.X > AbigailGame.TileSize) ? 2 : ((this.merchantBox.X - this.playerBoundingBox.X > AbigailGame.TileSize) ? 1 : 0);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.merchantBox.Location.X, (float)this.merchantBox.Location.Y), new Rectangle?(new Rectangle(496 + whichFrame * 16, 1728, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.merchantBox.Y / 10000f + 0.001f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.merchantBox.Location.X - AbigailGame.TileSize), (float)(this.merchantBox.Location.Y + AbigailGame.TileSize)), new Rectangle?(new Rectangle(529, 1744, 63, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.merchantBox.Y / 10000f + 0.001f);
						foreach (KeyValuePair<Rectangle, int> v in this.storeItems)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)v.Key.Location.X, (float)v.Key.Location.Y), new Rectangle?(new Rectangle(320 + v.Value * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)v.Key.Location.Y / 10000f);
							b.DrawString(Game1.smallFont, string.Concat(this.getPriceForItem(v.Value)), AbigailGame.topLeftScreenCoordinate + new Vector2((float)(v.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(string.Concat(this.getPriceForItem(v.Value))).X / 2f, (float)(v.Key.Location.Y + AbigailGame.TileSize + 3)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)v.Key.Location.Y / 10000f + 0.002f);
							b.DrawString(Game1.smallFont, string.Concat(this.getPriceForItem(v.Value)), AbigailGame.topLeftScreenCoordinate + new Vector2((float)(v.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(string.Concat(this.getPriceForItem(v.Value))).X / 2f - 1f, (float)(v.Key.Location.Y + AbigailGame.TileSize + 3)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)v.Key.Location.Y / 10000f + 0.002f);
							b.DrawString(Game1.smallFont, string.Concat(this.getPriceForItem(v.Value)), AbigailGame.topLeftScreenCoordinate + new Vector2((float)(v.Key.Location.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString(string.Concat(this.getPriceForItem(v.Value))).X / 2f + 1f, (float)(v.Key.Location.Y + AbigailGame.TileSize + 3)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)v.Key.Location.Y / 10000f + 0.002f);
						}
					}
				}
				if (AbigailGame.waitingForPlayerToMoveDownAMap && (AbigailGame.merchantShopOpen || AbigailGame.merchantLeaving || !AbigailGame.shopping) && AbigailGame.shoppingTimer < 250)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(7f, 15f) * (float)AbigailGame.TileSize, new Rectangle?(new Rectangle(355, 1750, 8, 8)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.001f);
				}
				using (List<AbigailGame.CowboyMonster>.Enumerator enumerator5 = AbigailGame.monsters.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						enumerator5.Current.draw(b);
					}
				}
				if (AbigailGame.gopherRunning)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)AbigailGame.gopherBox.X, (float)AbigailGame.gopherBox.Y), new Rectangle?(new Rectangle(320 + AbigailGame.waveTimer / 100 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)AbigailGame.gopherBox.Y / 10000f + 0.001f);
				}
				if (AbigailGame.gopherTrain && AbigailGame.gopherTrainPosition > -AbigailGame.TileSize)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X - (float)(AbigailGame.TileSize / 2), (float)AbigailGame.gopherTrainPosition), new Rectangle?(new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X + (float)(AbigailGame.TileSize / 2), (float)AbigailGame.gopherTrainPosition), new Rectangle?(new Rectangle(384 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X, (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize * 3)), new Rectangle?(new Rectangle(320 + AbigailGame.gopherTrainPosition / 30 % 4 * 16, 1792, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.96f);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2(this.playerPosition.X - (float)(AbigailGame.TileSize / 2), (float)(AbigailGame.gopherTrainPosition - AbigailGame.TileSize)), new Rectangle?(new Rectangle(400, 1728, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.97f);
					if (AbigailGame.holdItemTimer > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(384, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize * 2 / 3)) + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(320 + AbigailGame.itemToHold * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
					}
					else
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + this.playerPosition + new Vector2(0f, (float)(-(float)AbigailGame.TileSize / 4)), new Rectangle?(new Rectangle(464, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.98f);
					}
				}
				else
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize + 27), 0f), new Rectangle?(new Rectangle(294, 1782, 22, 22)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.25f);
					if (this.heldItem != null)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize + 18), -9f), new Rectangle?(new Rectangle(272 + this.heldItem.which * 16, 1808, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize * 2), (float)(-(float)AbigailGame.TileSize - 18)), new Rectangle?(new Rectangle(400, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					b.DrawString(Game1.smallFont, "x" + this.lives, AbigailGame.topLeftScreenCoordinate - new Vector2((float)AbigailGame.TileSize, (float)(-(float)AbigailGame.TileSize - AbigailGame.TileSize / 4 - 18)), Color.White);
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate - new Vector2((float)(AbigailGame.TileSize * 2), (float)(-(float)AbigailGame.TileSize * 2 - 18)), new Rectangle?(new Rectangle(272, 1808, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					b.DrawString(Game1.smallFont, "x" + this.coins, AbigailGame.topLeftScreenCoordinate - new Vector2((float)AbigailGame.TileSize, (float)(-(float)AbigailGame.TileSize * 2 - AbigailGame.TileSize / 4 - 18)), Color.White);
					for (int i2 = 0; i2 < AbigailGame.whichWave + this.whichRound * 12; i2++)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(AbigailGame.TileSize * 16 + 3), (float)(i2 * 3 * 6)), new Rectangle?(new Rectangle(512, 1760, 5, 5)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					b.Draw(Game1.mouseCursors, new Vector2((float)((int)AbigailGame.topLeftScreenCoordinate.X), (float)((int)AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 - 12)), new Rectangle?(new Rectangle(595, 1748, 9, 11)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					if (!AbigailGame.shootoutLevel)
					{
						b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X + 30, (int)AbigailGame.topLeftScreenCoordinate.Y - AbigailGame.TileSize / 2 + 3, (int)((float)(16 * AbigailGame.TileSize - 30) * ((float)AbigailGame.waveTimer / 80000f)), AbigailGame.TileSize / 4), (AbigailGame.waveTimer < 8000) ? new Color(188, 51, 74) : new Color(147, 177, 38));
					}
					if (AbigailGame.betweenWaveTimer > 0 && AbigailGame.whichWave == 0 && !AbigailGame.scrollingMap)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 120), (float)(Game1.graphics.GraphicsDevice.Viewport.Height - 144 - 3)), new Rectangle?(new Rectangle(352, 1648, 80, 48)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.99f);
					}
					if (this.bulletDamage > 1)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize)), new Rectangle?(new Rectangle(416 + (this.ammoLevel - 1) * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (this.fireSpeedLevel > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(320 + (this.fireSpeedLevel - 1) * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (this.runSpeedLevel > 0)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize * 3)), new Rectangle?(new Rectangle(368 + (this.runSpeedLevel - 1) * 16, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
					if (this.spreadPistol)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(-(float)AbigailGame.TileSize - 3), (float)(16 * AbigailGame.TileSize - AbigailGame.TileSize * 4)), new Rectangle?(new Rectangle(464, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.5f);
					}
				}
				if (AbigailGame.screenFlash > 0)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y, 16 * AbigailGame.TileSize, 16 * AbigailGame.TileSize), new Rectangle?(Game1.staminaRect.Bounds), new Color(255, 214, 168), 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
			}
			if (this.fadethenQuitTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * (1f - (float)this.fadethenQuitTimer / 2000f), 0f, Vector2.Zero, SpriteEffects.None, 1f);
			}
			if (this.abigailPortraitTimer > 0)
			{
				b.Draw(Game1.getCharacterFromName("Abigail", false).Portrait, new Vector2(AbigailGame.topLeftScreenCoordinate.X + (float)(16 * AbigailGame.TileSize), (float)this.abigailPortraitYposition), new Rectangle?(new Rectangle(64 * (this.abigailPortraitExpression % 2), 64 * (this.abigailPortraitExpression / 2), 64, 64)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				if (this.abigailPortraitTimer < 5500 && this.abigailPortraitTimer > 500)
				{
					Game1.drawWithBorder(this.AbigailDialogue, Color.Black, Color.Violet, new Vector2(AbigailGame.topLeftScreenCoordinate.X + (float)(16 * AbigailGame.TileSize) + (float)(64 * Game1.pixelZoom / 2) - Game1.dialogueFont.MeasureString(this.AbigailDialogue).X / 2f, (float)(this.abigailPortraitYposition - Game1.tileSize / 2)));
				}
			}
			b.End();
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000FFC8C File Offset: 0x000FDE8C
		public void changeScreenSize()
		{
			AbigailGame.topLeftScreenCoordinate = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 384), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 384));
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000FFCE4 File Offset: 0x000FDEE4
		public void unload()
		{
			if (AbigailGame.overworldSong != null && AbigailGame.overworldSong.IsPlaying)
			{
				AbigailGame.overworldSong.Stop(AudioStopOptions.Immediate);
			}
			if (AbigailGame.outlawSong != null && AbigailGame.outlawSong.IsPlaying)
			{
				AbigailGame.outlawSong.Stop(AudioStopOptions.Immediate);
			}
			this.lives = 3;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000C0A RID: 3082
		public const int mapWidth = 16;

		// Token: 0x04000C0B RID: 3083
		public const int mapHeight = 16;

		// Token: 0x04000C0C RID: 3084
		public const int pixelZoom = 3;

		// Token: 0x04000C0D RID: 3085
		public const int bulletSpeed = 8;

		// Token: 0x04000C0E RID: 3086
		public const double lootChance = 0.05;

		// Token: 0x04000C0F RID: 3087
		public const double coinChance = 0.05;

		// Token: 0x04000C10 RID: 3088
		public int lootDuration = 7500;

		// Token: 0x04000C11 RID: 3089
		public int powerupDuration = 10000;

		// Token: 0x04000C12 RID: 3090
		private const int abigailPortraitDuration = 6000;

		// Token: 0x04000C13 RID: 3091
		public const float playerSpeed = 3f;

		// Token: 0x04000C14 RID: 3092
		public const int baseTileSize = 16;

		// Token: 0x04000C15 RID: 3093
		public const int orcSpeed = 2;

		// Token: 0x04000C16 RID: 3094
		public const int ogreSpeed = 1;

		// Token: 0x04000C17 RID: 3095
		public const int ghostSpeed = 3;

		// Token: 0x04000C18 RID: 3096
		public const int spikeySpeed = 3;

		// Token: 0x04000C19 RID: 3097
		public const int orcHealth = 1;

		// Token: 0x04000C1A RID: 3098
		public const int ghostHealth = 1;

		// Token: 0x04000C1B RID: 3099
		public const int ogreHealth = 3;

		// Token: 0x04000C1C RID: 3100
		public const int spikeyHealth = 2;

		// Token: 0x04000C1D RID: 3101
		public const int cactusDanceDelay = 800;

		// Token: 0x04000C1E RID: 3102
		public const int playerMotionDelay = 100;

		// Token: 0x04000C1F RID: 3103
		public const int playerFootStepDelay = 200;

		// Token: 0x04000C20 RID: 3104
		public const int deathDelay = 3000;

		// Token: 0x04000C21 RID: 3105
		public const int MAP_BARRIER1 = 0;

		// Token: 0x04000C22 RID: 3106
		public const int MAP_BARRIER2 = 1;

		// Token: 0x04000C23 RID: 3107
		public const int MAP_ROCKY1 = 2;

		// Token: 0x04000C24 RID: 3108
		public const int MAP_DESERT = 3;

		// Token: 0x04000C25 RID: 3109
		public const int MAP_GRASSY = 4;

		// Token: 0x04000C26 RID: 3110
		public const int MAP_CACTUS = 5;

		// Token: 0x04000C27 RID: 3111
		public const int MAP_FENCE = 7;

		// Token: 0x04000C28 RID: 3112
		public const int MAP_TRENCH1 = 8;

		// Token: 0x04000C29 RID: 3113
		public const int MAP_TRENCH2 = 9;

		// Token: 0x04000C2A RID: 3114
		public const int MAP_BRIDGE = 10;

		// Token: 0x04000C2B RID: 3115
		public const int orc = 0;

		// Token: 0x04000C2C RID: 3116
		public const int ghost = 1;

		// Token: 0x04000C2D RID: 3117
		public const int ogre = 2;

		// Token: 0x04000C2E RID: 3118
		public const int mummy = 3;

		// Token: 0x04000C2F RID: 3119
		public const int devil = 4;

		// Token: 0x04000C30 RID: 3120
		public const int mushroom = 5;

		// Token: 0x04000C31 RID: 3121
		public const int spikey = 6;

		// Token: 0x04000C32 RID: 3122
		public const int dracula = 7;

		// Token: 0x04000C33 RID: 3123
		public const int desert = 0;

		// Token: 0x04000C34 RID: 3124
		public const int woods = 2;

		// Token: 0x04000C35 RID: 3125
		public const int graveyard = 1;

		// Token: 0x04000C36 RID: 3126
		public const int POWERUP_LOG = -1;

		// Token: 0x04000C37 RID: 3127
		public const int POWERUP_SKULL = -2;

		// Token: 0x04000C38 RID: 3128
		public const int coin1 = 0;

		// Token: 0x04000C39 RID: 3129
		public const int coin5 = 1;

		// Token: 0x04000C3A RID: 3130
		public const int POWERUP_SPREAD = 2;

		// Token: 0x04000C3B RID: 3131
		public const int POWERUP_RAPIDFIRE = 3;

		// Token: 0x04000C3C RID: 3132
		public const int POWERUP_NUKE = 4;

		// Token: 0x04000C3D RID: 3133
		public const int POWERUP_ZOMBIE = 5;

		// Token: 0x04000C3E RID: 3134
		public const int POWERUP_SPEED = 6;

		// Token: 0x04000C3F RID: 3135
		public const int POWERUP_SHOTGUN = 7;

		// Token: 0x04000C40 RID: 3136
		public const int POWERUP_LIFE = 8;

		// Token: 0x04000C41 RID: 3137
		public const int POWERUP_TELEPORT = 9;

		// Token: 0x04000C42 RID: 3138
		public const int POWERUP_SHERRIFF = 10;

		// Token: 0x04000C43 RID: 3139
		public const int POWERUP_HEART = -3;

		// Token: 0x04000C44 RID: 3140
		public const int ITEM_FIRESPEED1 = 0;

		// Token: 0x04000C45 RID: 3141
		public const int ITEM_FIRESPEED2 = 1;

		// Token: 0x04000C46 RID: 3142
		public const int ITEM_FIRESPEED3 = 2;

		// Token: 0x04000C47 RID: 3143
		public const int ITEM_RUNSPEED1 = 3;

		// Token: 0x04000C48 RID: 3144
		public const int ITEM_RUNSPEED2 = 4;

		// Token: 0x04000C49 RID: 3145
		public const int ITEM_LIFE = 5;

		// Token: 0x04000C4A RID: 3146
		public const int ITEM_AMMO1 = 6;

		// Token: 0x04000C4B RID: 3147
		public const int ITEM_AMMO2 = 7;

		// Token: 0x04000C4C RID: 3148
		public const int ITEM_AMMO3 = 8;

		// Token: 0x04000C4D RID: 3149
		public const int ITEM_SPREADPISTOL = 9;

		// Token: 0x04000C4E RID: 3150
		public const int ITEM_STAR = 10;

		// Token: 0x04000C4F RID: 3151
		public const int ITEM_SKULL = 11;

		// Token: 0x04000C50 RID: 3152
		public const int ITEM_LOG = 12;

		// Token: 0x04000C51 RID: 3153
		public const int option_retry = 0;

		// Token: 0x04000C52 RID: 3154
		public const int option_quit = 1;

		// Token: 0x04000C53 RID: 3155
		public int runSpeedLevel;

		// Token: 0x04000C54 RID: 3156
		public int fireSpeedLevel;

		// Token: 0x04000C55 RID: 3157
		public int ammoLevel;

		// Token: 0x04000C56 RID: 3158
		public int whichRound;

		// Token: 0x04000C57 RID: 3159
		public bool spreadPistol;

		// Token: 0x04000C58 RID: 3160
		public const int waveDuration = 80000;

		// Token: 0x04000C59 RID: 3161
		public const int betweenWaveDuration = 5000;

		// Token: 0x04000C5A RID: 3162
		public static List<AbigailGame.CowboyMonster> monsters = new List<AbigailGame.CowboyMonster>();

		// Token: 0x04000C5B RID: 3163
		public Vector2 playerPosition;

		// Token: 0x04000C5C RID: 3164
		public static Vector2 player2Position = default(Vector2);

		// Token: 0x04000C5D RID: 3165
		public Rectangle playerBoundingBox;

		// Token: 0x04000C5E RID: 3166
		public Rectangle merchantBox;

		// Token: 0x04000C5F RID: 3167
		public Rectangle player2BoundingBox;

		// Token: 0x04000C60 RID: 3168
		public Rectangle noPickUpBox;

		// Token: 0x04000C61 RID: 3169
		public static List<int> playerMovementDirections = new List<int>();

		// Token: 0x04000C62 RID: 3170
		public static List<int> playerShootingDirections = new List<int>();

		// Token: 0x04000C63 RID: 3171
		public List<int> player2MovementDirections = new List<int>();

		// Token: 0x04000C64 RID: 3172
		public List<int> player2ShootingDirections = new List<int>();

		// Token: 0x04000C65 RID: 3173
		public int shootingDelay = 300;

		// Token: 0x04000C66 RID: 3174
		public int shotTimer;

		// Token: 0x04000C67 RID: 3175
		public int motionPause;

		// Token: 0x04000C68 RID: 3176
		public int bulletDamage;

		// Token: 0x04000C69 RID: 3177
		public int speedBonus;

		// Token: 0x04000C6A RID: 3178
		public int fireRateBonus;

		// Token: 0x04000C6B RID: 3179
		public int lives = 3;

		// Token: 0x04000C6C RID: 3180
		public int coins;

		// Token: 0x04000C6D RID: 3181
		public int score;

		// Token: 0x04000C6E RID: 3182
		public int player2deathtimer;

		// Token: 0x04000C6F RID: 3183
		public int player2invincibletimer;

		// Token: 0x04000C70 RID: 3184
		public List<AbigailGame.CowboyBullet> bullets = new List<AbigailGame.CowboyBullet>();

		// Token: 0x04000C71 RID: 3185
		public static List<AbigailGame.CowboyBullet> enemyBullets = new List<AbigailGame.CowboyBullet>();

		// Token: 0x04000C72 RID: 3186
		public static int[,] map = new int[16, 16];

		// Token: 0x04000C73 RID: 3187
		public static int[,] nextMap = new int[16, 16];

		// Token: 0x04000C74 RID: 3188
		public List<Point>[] spawnQueue = new List<Point>[4];

		// Token: 0x04000C75 RID: 3189
		public static Vector2 topLeftScreenCoordinate;

		// Token: 0x04000C76 RID: 3190
		public float cactusDanceTimer;

		// Token: 0x04000C77 RID: 3191
		public float playerMotionAnimationTimer;

		// Token: 0x04000C78 RID: 3192
		public float playerFootstepSoundTimer = 200f;

		// Token: 0x04000C79 RID: 3193
		public AbigailGame.behaviorAfterMotionPause behaviorAfterPause;

		// Token: 0x04000C7A RID: 3194
		public List<Vector2> monsterChances = new List<Vector2>
		{
			new Vector2(0.014f, 0.4f),
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero,
			Vector2.Zero
		};

		// Token: 0x04000C7B RID: 3195
		public Rectangle shoppingCarpetNoPickup;

		// Token: 0x04000C7C RID: 3196
		public Dictionary<int, int> activePowerups = new Dictionary<int, int>();

		// Token: 0x04000C7D RID: 3197
		public static List<AbigailGame.CowboyPowerup> powerups = new List<AbigailGame.CowboyPowerup>();

		// Token: 0x04000C7E RID: 3198
		public string AbigailDialogue = "";

		// Token: 0x04000C7F RID: 3199
		public static List<TemporaryAnimatedSprite> temporarySprites = new List<TemporaryAnimatedSprite>();

		// Token: 0x04000C80 RID: 3200
		public AbigailGame.CowboyPowerup heldItem;

		// Token: 0x04000C81 RID: 3201
		public static int world = 0;

		// Token: 0x04000C82 RID: 3202
		public int gameOverOption;

		// Token: 0x04000C83 RID: 3203
		public int gamerestartTimer;

		// Token: 0x04000C84 RID: 3204
		public int player2TargetUpdateTimer;

		// Token: 0x04000C85 RID: 3205
		public int player2shotTimer;

		// Token: 0x04000C86 RID: 3206
		public int player2AnimationTimer;

		// Token: 0x04000C87 RID: 3207
		public int fadethenQuitTimer;

		// Token: 0x04000C88 RID: 3208
		public int abigailPortraitYposition;

		// Token: 0x04000C89 RID: 3209
		public int abigailPortraitTimer;

		// Token: 0x04000C8A RID: 3210
		public int abigailPortraitExpression;

		// Token: 0x04000C8B RID: 3211
		public static int waveTimer = 80000;

		// Token: 0x04000C8C RID: 3212
		public static int betweenWaveTimer = 5000;

		// Token: 0x04000C8D RID: 3213
		public static int whichWave;

		// Token: 0x04000C8E RID: 3214
		public static int monsterConfusionTimer;

		// Token: 0x04000C8F RID: 3215
		public static int zombieModeTimer;

		// Token: 0x04000C90 RID: 3216
		public static int shoppingTimer;

		// Token: 0x04000C91 RID: 3217
		public static int holdItemTimer;

		// Token: 0x04000C92 RID: 3218
		public static int itemToHold;

		// Token: 0x04000C93 RID: 3219
		public static int newMapPosition;

		// Token: 0x04000C94 RID: 3220
		public static int playerInvincibleTimer;

		// Token: 0x04000C95 RID: 3221
		public static int screenFlash;

		// Token: 0x04000C96 RID: 3222
		public static int gopherTrainPosition;

		// Token: 0x04000C97 RID: 3223
		public static int endCutsceneTimer;

		// Token: 0x04000C98 RID: 3224
		public static int endCutscenePhase;

		// Token: 0x04000C99 RID: 3225
		public static int startTimer;

		// Token: 0x04000C9A RID: 3226
		public static float deathTimer;

		// Token: 0x04000C9B RID: 3227
		public static bool onStartMenu;

		// Token: 0x04000C9C RID: 3228
		public static bool shopping;

		// Token: 0x04000C9D RID: 3229
		public static bool gopherRunning;

		// Token: 0x04000C9E RID: 3230
		public static bool store;

		// Token: 0x04000C9F RID: 3231
		public static bool merchantLeaving;

		// Token: 0x04000CA0 RID: 3232
		public static bool merchantArriving;

		// Token: 0x04000CA1 RID: 3233
		public static bool merchantShopOpen;

		// Token: 0x04000CA2 RID: 3234
		public static bool waitingForPlayerToMoveDownAMap;

		// Token: 0x04000CA3 RID: 3235
		public static bool scrollingMap;

		// Token: 0x04000CA4 RID: 3236
		public static bool hasGopherAppeared;

		// Token: 0x04000CA5 RID: 3237
		public static bool shootoutLevel;

		// Token: 0x04000CA6 RID: 3238
		public static bool gopherTrain;

		// Token: 0x04000CA7 RID: 3239
		public static bool playerJumped;

		// Token: 0x04000CA8 RID: 3240
		public static bool endCutscene;

		// Token: 0x04000CA9 RID: 3241
		public static bool gameOver;

		// Token: 0x04000CAA RID: 3242
		public static bool playingWithAbigail;

		// Token: 0x04000CAB RID: 3243
		public static bool beatLevelWithAbigail;

		// Token: 0x04000CAC RID: 3244
		private Dictionary<Rectangle, int> storeItems = new Dictionary<Rectangle, int>();

		// Token: 0x04000CAD RID: 3245
		private bool quit;

		// Token: 0x04000CAE RID: 3246
		private bool died;

		// Token: 0x04000CAF RID: 3247
		public static Rectangle gopherBox;

		// Token: 0x04000CB0 RID: 3248
		public Point gopherMotion;

		// Token: 0x04000CB1 RID: 3249
		private static Cue overworldSong;

		// Token: 0x04000CB2 RID: 3250
		private static Cue outlawSong;

		// Token: 0x04000CB3 RID: 3251
		private int player2FootstepSoundTimer;

		// Token: 0x04000CB4 RID: 3252
		private AbigailGame.CowboyMonster targetMonster;

		// Token: 0x02000177 RID: 375
		// Token: 0x060013A1 RID: 5025
		public delegate void behaviorAfterMotionPause();

		// Token: 0x02000178 RID: 376
		public class CowboyPowerup
		{
			// Token: 0x060013A4 RID: 5028 RVA: 0x00186701 File Offset: 0x00184901
			public CowboyPowerup(int which, Point position, int duration)
			{
				this.which = which;
				this.position = position;
				this.duration = duration;
			}

			// Token: 0x060013A5 RID: 5029 RVA: 0x00186720 File Offset: 0x00184920
			public void draw(SpriteBatch b)
			{
				if (this.duration > 2000 || this.duration / 200 % 2 == 0)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y + this.yOffset), new Rectangle?(new Rectangle(272 + this.which * 16, 1808, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
				}
			}

			// Token: 0x04001475 RID: 5237
			public int which;

			// Token: 0x04001476 RID: 5238
			public Point position;

			// Token: 0x04001477 RID: 5239
			public int duration;

			// Token: 0x04001478 RID: 5240
			public float yOffset;
		}

		// Token: 0x02000179 RID: 377
		public class CowboyBullet
		{
			// Token: 0x060013A6 RID: 5030 RVA: 0x001867D5 File Offset: 0x001849D5
			public CowboyBullet(Point position, Point motion, int damage)
			{
				this.position = position;
				this.motion = motion;
				this.damage = damage;
			}

			// Token: 0x060013A7 RID: 5031 RVA: 0x001867F4 File Offset: 0x001849F4
			public CowboyBullet(Point position, int direction, int damage)
			{
				this.position = position;
				switch (direction)
				{
				case 0:
					this.motion = new Point(0, -8);
					break;
				case 1:
					this.motion = new Point(8, 0);
					break;
				case 2:
					this.motion = new Point(0, 8);
					break;
				case 3:
					this.motion = new Point(-8, 0);
					break;
				}
				this.damage = damage;
			}

			// Token: 0x04001479 RID: 5241
			public Point position;

			// Token: 0x0400147A RID: 5242
			public Point motion;

			// Token: 0x0400147B RID: 5243
			public int damage;
		}

		// Token: 0x0200017A RID: 378
		public class CowboyMonster
		{
			// Token: 0x060013A8 RID: 5032 RVA: 0x0018686C File Offset: 0x00184A6C
			public CowboyMonster(int which, int health, int speed, Point position)
			{
				this.health = health;
				this.type = which;
				this.speed = speed;
				this.position = new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize);
				this.uninterested = (Game1.random.NextDouble() < 0.25);
			}

			// Token: 0x060013A9 RID: 5033 RVA: 0x001868E8 File Offset: 0x00184AE8
			public CowboyMonster(int which, Point position)
			{
				this.type = which;
				this.position = new Rectangle(position.X, position.Y, AbigailGame.TileSize, AbigailGame.TileSize);
				switch (this.type)
				{
				case 0:
					this.speed = 2;
					this.health = 1;
					this.uninterested = (Game1.random.NextDouble() < 0.25);
					if (this.uninterested)
					{
						this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
					}
					break;
				case 1:
					this.speed = 2;
					this.health = 1;
					this.flyer = true;
					break;
				case 2:
					this.speed = 1;
					this.health = 3;
					break;
				case 3:
					this.health = 6;
					this.speed = 1;
					this.uninterested = (Game1.random.NextDouble() < 0.25);
					if (this.uninterested)
					{
						this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
					}
					break;
				case 4:
					this.health = 3;
					this.speed = 3;
					this.flyer = true;
					break;
				case 5:
					this.speed = 3;
					this.health = 2;
					break;
				case 6:
					this.speed = 3;
					this.health = 2;
					do
					{
						this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
					}
					while (AbigailGame.isCollidingWithMap(this.targetPosition));
					break;
				}
				this.oppositeMotionGuy = (Game1.random.NextDouble() < 0.5);
			}

			// Token: 0x060013AA RID: 5034 RVA: 0x00186AF8 File Offset: 0x00184CF8
			public virtual void draw(SpriteBatch b)
			{
				if (this.type != 6 || !this.special)
				{
					if (!this.invisible)
					{
						if (this.flashColorTimer > 0f)
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(352 + this.type * 16, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						}
						else
						{
							b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(352 + (this.type * 2 + ((this.movementAnimationTimer < 250f) ? 1 : 0)) * 16, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						}
						if (AbigailGame.monsterConfusionTimer > 0)
						{
							b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f, (float)(this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.position.Y / 10000f);
							b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f + 1f, (float)(this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.position.Y / 10000f);
							b.DrawString(Game1.smallFont, "?", AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X + AbigailGame.TileSize / 2) - Game1.smallFont.MeasureString("?").X / 2f - 1f, (float)(this.position.Y - AbigailGame.TileSize / 2)), new Color(88, 29, 43), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.position.Y / 10000f);
						}
					}
					return;
				}
				if (this.flashColorTimer > 0f)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(480, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
					return;
				}
				b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(576, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
			}

			// Token: 0x060013AB RID: 5035 RVA: 0x00186F14 File Offset: 0x00185114
			public virtual bool takeDamage(int damage)
			{
				this.health -= damage;
				this.health = Math.Max(0, this.health);
				if (this.health <= 0)
				{
					return true;
				}
				Game1.playSound("cowboy_monsterhit");
				this.flashColor = Color.Red;
				this.flashColorTimer = 100f;
				return false;
			}

			// Token: 0x060013AC RID: 5036 RVA: 0x00186F70 File Offset: 0x00185170
			public virtual int getLootDrop()
			{
				if (this.type == 6 && this.special)
				{
					return -1;
				}
				if (Game1.random.NextDouble() < 0.05)
				{
					if (this.type != 0 && Game1.random.NextDouble() < 0.1)
					{
						return 1;
					}
					if (Game1.random.NextDouble() < 0.01)
					{
						return 1;
					}
					return 0;
				}
				else
				{
					if (Game1.random.NextDouble() >= 0.05)
					{
						return -1;
					}
					if (Game1.random.NextDouble() < 0.15)
					{
						return Game1.random.Next(6, 8);
					}
					if (Game1.random.NextDouble() < 0.07)
					{
						return 10;
					}
					int loot = Game1.random.Next(2, 10);
					if (loot == 5 && Game1.random.NextDouble() < 0.4)
					{
						loot = Game1.random.Next(2, 10);
					}
					return loot;
				}
			}

			// Token: 0x060013AD RID: 5037 RVA: 0x00187064 File Offset: 0x00185264
			public virtual bool move(Vector2 playerPosition, GameTime time)
			{
				this.movementAnimationTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.movementAnimationTimer <= 0f)
				{
					this.movementAnimationTimer = (float)Math.Max(100, 500 - this.speed * 50);
				}
				if (this.flashColorTimer > 0f)
				{
					this.flashColorTimer -= (float)time.ElapsedGameTime.Milliseconds;
					return false;
				}
				if (AbigailGame.monsterConfusionTimer > 0)
				{
					return false;
				}
				if (AbigailGame.shopping)
				{
					AbigailGame.shoppingTimer -= time.ElapsedGameTime.Milliseconds;
					if (AbigailGame.shoppingTimer <= 0)
					{
						AbigailGame.shoppingTimer = 100;
					}
				}
				this.ticksSinceLastMovement++;
				switch (this.type)
				{
				case 0:
				case 2:
				case 3:
				case 5:
				case 6:
				{
					if (this.type == 6)
					{
						if (this.special || this.invisible)
						{
							break;
						}
						if (this.ticksSinceLastMovement > 20)
						{
							int tries = 0;
							do
							{
								tries++;
								this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
								if (!AbigailGame.isCollidingWithMap(this.targetPosition))
								{
									break;
								}
							}
							while (tries < 5);
						}
					}
					else if (this.ticksSinceLastMovement > 20)
					{
						int tries2 = 0;
						do
						{
							this.oppositeMotionGuy = !this.oppositeMotionGuy;
							tries2++;
							this.targetPosition = new Point(Game1.random.Next(this.position.X - AbigailGame.TileSize * 2, this.position.X + AbigailGame.TileSize * 2), Game1.random.Next(this.position.Y - AbigailGame.TileSize * 2, this.position.Y + AbigailGame.TileSize * 2));
						}
						while (AbigailGame.isCollidingWithMap(this.targetPosition) && tries2 < 5);
					}
					Point arg_211_0 = this.targetPosition;
					Vector2 target = (!this.targetPosition.Equals(Point.Zero)) ? new Vector2((float)this.targetPosition.X, (float)this.targetPosition.Y) : playerPosition;
					if (AbigailGame.playingWithAbigail && target.Equals(playerPosition))
					{
						double distanceToPlayer = Math.Sqrt(Math.Pow((double)((float)this.position.X - target.X), 2.0) - Math.Pow((double)((float)this.position.Y - target.Y), 2.0));
						if (Math.Sqrt(Math.Pow((double)((float)this.position.X - AbigailGame.player2Position.X), 2.0) - Math.Pow((double)((float)this.position.Y - AbigailGame.player2Position.Y), 2.0)) < distanceToPlayer)
						{
							target = AbigailGame.player2Position;
						}
					}
					if (AbigailGame.gopherRunning)
					{
						target = new Vector2((float)AbigailGame.gopherBox.X, (float)AbigailGame.gopherBox.Y);
					}
					if (Game1.random.NextDouble() < 0.001)
					{
						this.oppositeMotionGuy = !this.oppositeMotionGuy;
					}
					if ((this.type == 6 && !this.oppositeMotionGuy) || Math.Abs(target.X - (float)this.position.X) > Math.Abs(target.Y - (float)this.position.Y))
					{
						if (target.X + (float)this.speed < (float)this.position.X && (this.movedLastTurn || this.movementDirection != 3))
						{
							this.movementDirection = 3;
						}
						else if (target.X > (float)(this.position.X + this.speed) && (this.movedLastTurn || this.movementDirection != 1))
						{
							this.movementDirection = 1;
						}
						else if (target.Y > (float)(this.position.Y + this.speed) && (this.movedLastTurn || this.movementDirection != 2))
						{
							this.movementDirection = 2;
						}
						else if (target.Y + (float)this.speed < (float)this.position.Y && (this.movedLastTurn || this.movementDirection != 0))
						{
							this.movementDirection = 0;
						}
					}
					else if (target.Y > (float)(this.position.Y + this.speed) && (this.movedLastTurn || this.movementDirection != 2))
					{
						this.movementDirection = 2;
					}
					else if (target.Y + (float)this.speed < (float)this.position.Y && (this.movedLastTurn || this.movementDirection != 0))
					{
						this.movementDirection = 0;
					}
					else if (target.X + (float)this.speed < (float)this.position.X && (this.movedLastTurn || this.movementDirection != 3))
					{
						this.movementDirection = 3;
					}
					else if (target.X > (float)(this.position.X + this.speed) && (this.movedLastTurn || this.movementDirection != 1))
					{
						this.movementDirection = 1;
					}
					this.movedLastTurn = false;
					Rectangle attemptedPosition = this.position;
					switch (this.movementDirection)
					{
					case 0:
						attemptedPosition.Y -= this.speed;
						break;
					case 1:
						attemptedPosition.X += this.speed;
						break;
					case 2:
						attemptedPosition.Y += this.speed;
						break;
					case 3:
						attemptedPosition.X -= this.speed;
						break;
					}
					if (AbigailGame.zombieModeTimer > 0)
					{
						attemptedPosition.X = this.position.X - (attemptedPosition.X - this.position.X);
						attemptedPosition.Y = this.position.Y - (attemptedPosition.Y - this.position.Y);
					}
					if (this.type == 2)
					{
						for (int i = AbigailGame.monsters.Count - 1; i >= 0; i--)
						{
							if (AbigailGame.monsters[i].type == 6 && AbigailGame.monsters[i].special && AbigailGame.monsters[i].position.Intersects(attemptedPosition))
							{
								AbigailGame.addGuts(AbigailGame.monsters[i].position.Location, AbigailGame.monsters[i].type);
								Game1.playSound("Cowboy_monsterDie");
								AbigailGame.monsters.RemoveAt(i);
							}
						}
					}
					if (!AbigailGame.isCollidingWithMapForMonsters(attemptedPosition) && !AbigailGame.isCollidingWithMonster(attemptedPosition, this) && AbigailGame.deathTimer <= 0f)
					{
						this.ticksSinceLastMovement = 0;
						this.position = attemptedPosition;
						this.movedLastTurn = true;
						if (this.position.Contains((int)target.X + AbigailGame.TileSize / 2, (int)target.Y + AbigailGame.TileSize / 2))
						{
							this.targetPosition = Point.Zero;
							if ((this.type == 0 || this.type == 3) && this.uninterested)
							{
								this.targetPosition = new Point(Game1.random.Next(2, 14) * AbigailGame.TileSize, Game1.random.Next(2, 14) * AbigailGame.TileSize);
								if (Game1.random.NextDouble() < 0.5)
								{
									this.uninterested = false;
									this.targetPosition = Point.Zero;
								}
							}
							if (this.type == 6 && !this.invisible)
							{
								AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(352, 1728, 16, 16), 60f, 3, 0, new Vector2((float)this.position.X, (float)this.position.Y) + AbigailGame.topLeftScreenCoordinate, false, false, (float)this.position.Y / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
								{
									endFunction = new TemporaryAnimatedSprite.endBehavior(this.spikeyEndBehavior)
								});
								this.invisible = true;
							}
						}
					}
					break;
				}
				case 1:
				case 4:
				{
					if (this.ticksSinceLastMovement > 20)
					{
						int tries3 = 0;
						do
						{
							this.oppositeMotionGuy = !this.oppositeMotionGuy;
							tries3++;
							this.targetPosition = new Point(Game1.random.Next(this.position.X - AbigailGame.TileSize * 2, this.position.X + AbigailGame.TileSize * 2), Game1.random.Next(this.position.Y - AbigailGame.TileSize * 2, this.position.Y + AbigailGame.TileSize * 2));
						}
						while (AbigailGame.isCollidingWithMap(this.targetPosition) && tries3 < 5);
					}
					Point arg_914_0 = this.targetPosition;
					Vector2 target = (!this.targetPosition.Equals(Point.Zero)) ? new Vector2((float)this.targetPosition.X, (float)this.targetPosition.Y) : playerPosition;
					Vector2 targetToFly = Utility.getVelocityTowardPoint(this.position.Location, target + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), (float)this.speed);
					float accelerationMultiplyer = (targetToFly.X != 0f && targetToFly.Y != 0f) ? 1.5f : 1f;
					if (targetToFly.X > this.acceleration.X)
					{
						this.acceleration.X = this.acceleration.X + 0.1f * accelerationMultiplyer;
					}
					if (targetToFly.X < this.acceleration.X)
					{
						this.acceleration.X = this.acceleration.X - 0.1f * accelerationMultiplyer;
					}
					if (targetToFly.Y > this.acceleration.Y)
					{
						this.acceleration.Y = this.acceleration.Y + 0.1f * accelerationMultiplyer;
					}
					if (targetToFly.Y < this.acceleration.Y)
					{
						this.acceleration.Y = this.acceleration.Y - 0.1f * accelerationMultiplyer;
					}
					if (!AbigailGame.isCollidingWithMonster(new Rectangle(this.position.X + (int)Math.Ceiling((double)this.acceleration.X), this.position.Y + (int)Math.Ceiling((double)this.acceleration.Y), AbigailGame.TileSize, AbigailGame.TileSize), this) && AbigailGame.deathTimer <= 0f)
					{
						this.ticksSinceLastMovement = 0;
						this.position.X = this.position.X + (int)Math.Ceiling((double)this.acceleration.X);
						this.position.Y = this.position.Y + (int)Math.Ceiling((double)this.acceleration.Y);
						if (this.position.Contains((int)target.X + AbigailGame.TileSize / 2, (int)target.Y + AbigailGame.TileSize / 2))
						{
							this.targetPosition = Point.Zero;
						}
					}
					break;
				}
				}
				return false;
			}

			// Token: 0x060013AE RID: 5038 RVA: 0x00187BA5 File Offset: 0x00185DA5
			public void spikeyEndBehavior(int extraInfo)
			{
				this.invisible = false;
				this.health += 5;
				this.special = true;
			}

			// Token: 0x0400147C RID: 5244
			public const int MonsterAnimationDelay = 500;

			// Token: 0x0400147D RID: 5245
			public int health;

			// Token: 0x0400147E RID: 5246
			public int type;

			// Token: 0x0400147F RID: 5247
			public int speed;

			// Token: 0x04001480 RID: 5248
			public float movementAnimationTimer;

			// Token: 0x04001481 RID: 5249
			public Rectangle position;

			// Token: 0x04001482 RID: 5250
			private int movementDirection;

			// Token: 0x04001483 RID: 5251
			private bool movedLastTurn;

			// Token: 0x04001484 RID: 5252
			private bool oppositeMotionGuy;

			// Token: 0x04001485 RID: 5253
			private bool invisible;

			// Token: 0x04001486 RID: 5254
			private bool special;

			// Token: 0x04001487 RID: 5255
			private bool uninterested;

			// Token: 0x04001488 RID: 5256
			private bool flyer;

			// Token: 0x04001489 RID: 5257
			private Color tint = Color.White;

			// Token: 0x0400148A RID: 5258
			private Color flashColor = Color.Red;

			// Token: 0x0400148B RID: 5259
			public float flashColorTimer;

			// Token: 0x0400148C RID: 5260
			public int ticksSinceLastMovement;

			// Token: 0x0400148D RID: 5261
			public Vector2 acceleration;

			// Token: 0x0400148E RID: 5262
			private Point targetPosition;
		}

		// Token: 0x0200017B RID: 379
		public class Dracula : AbigailGame.CowboyMonster
		{
			// Token: 0x060013AF RID: 5039 RVA: 0x00187BC4 File Offset: 0x00185DC4
			public Dracula() : base(-2, new Point(8 * AbigailGame.TileSize, 8 * AbigailGame.TileSize))
			{
				this.homePosition = this.position.Location;
				this.position.Y = this.position.Y + AbigailGame.TileSize * 4;
				this.health = 350;
				this.fullHealth = this.health;
				this.phase = -1;
				this.phaseInternalTimer = 4000;
				this.speed = 2;
			}

			// Token: 0x060013B0 RID: 5040 RVA: 0x00187C4C File Offset: 0x00185E4C
			public override void draw(SpriteBatch b)
			{
				if (this.phase != -1)
				{
					b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize + 3, (int)((float)(16 * AbigailGame.TileSize) * ((float)this.health / (float)this.fullHealth)), AbigailGame.TileSize / 3), new Color(188, 51, 74));
				}
				if (this.flashColorTimer > 0f)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(464, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
					return;
				}
				switch (this.phase)
				{
				case -1:
				case 1:
				case 2:
				case 3:
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(592 + this.phaseInternalTimer / 100 % 3 * 16, 1760, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
					if (this.phase == -1)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)(this.position.Y + AbigailGame.TileSize) + (float)Math.Sin((double)((float)this.phaseInternalTimer / 1000f)) * 3f), new Rectangle?(new Rectangle(528, 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X - AbigailGame.TileSize / 2), (float)(this.position.Y - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(608, 1728, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
						return;
					}
					return;
				}
				b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(592 + this.phaseInternalTimer / 100 % 2 * 16, 1712, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f);
			}

			// Token: 0x060013B1 RID: 5041 RVA: 0x00035C5D File Offset: 0x00033E5D
			public override int getLootDrop()
			{
				return -1;
			}

			// Token: 0x060013B2 RID: 5042 RVA: 0x00187F89 File Offset: 0x00186189
			public override bool takeDamage(int damage)
			{
				if (this.phase == -1)
				{
					return false;
				}
				this.health -= damage;
				if (this.health < 0)
				{
					return true;
				}
				this.flashColorTimer = 100f;
				Game1.playSound("cowboy_monsterhit");
				return false;
			}

			// Token: 0x060013B3 RID: 5043 RVA: 0x00187FC8 File Offset: 0x001861C8
			public override bool move(Vector2 playerPosition, GameTime time)
			{
				if (this.flashColorTimer > 0f)
				{
					this.flashColorTimer -= (float)time.ElapsedGameTime.Milliseconds;
				}
				this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
				switch (this.phase)
				{
				case -1:
					if (this.phaseInternalTimer <= 0)
					{
						this.phaseInternalCounter = 0;
						if (Game1.soundBank != null)
						{
							AbigailGame.outlawSong = Game1.soundBank.GetCue("cowboy_boss");
							AbigailGame.outlawSong.Play();
						}
						this.phase = 0;
					}
					break;
				case 0:
					if (this.phaseInternalCounter == 0)
					{
						this.phaseInternalCounter++;
						this.phaseInternalTimer = Game1.random.Next(3000, 7000);
					}
					if (this.phaseInternalTimer < 0)
					{
						this.phaseInternalCounter = 0;
						this.phase = Game1.random.Next(1, 4);
						this.phaseInternalTimer = 9999;
					}
					if (AbigailGame.deathTimer <= 0f)
					{
						int movementDirection = -1;
						if (Math.Abs(playerPosition.X - (float)this.position.X) > Math.Abs(playerPosition.Y - (float)this.position.Y))
						{
							if (playerPosition.X + (float)this.speed < (float)this.position.X)
							{
								movementDirection = 3;
							}
							else if (playerPosition.X > (float)(this.position.X + this.speed))
							{
								movementDirection = 1;
							}
							else if (playerPosition.Y > (float)(this.position.Y + this.speed))
							{
								movementDirection = 2;
							}
							else if (playerPosition.Y + (float)this.speed < (float)this.position.Y)
							{
								movementDirection = 0;
							}
						}
						else if (playerPosition.Y > (float)(this.position.Y + this.speed))
						{
							movementDirection = 2;
						}
						else if (playerPosition.Y + (float)this.speed < (float)this.position.Y)
						{
							movementDirection = 0;
						}
						else if (playerPosition.X + (float)this.speed < (float)this.position.X)
						{
							movementDirection = 3;
						}
						else if (playerPosition.X > (float)(this.position.X + this.speed))
						{
							movementDirection = 1;
						}
						Rectangle attemptedPosition = this.position;
						switch (movementDirection)
						{
						case 0:
							attemptedPosition.Y -= this.speed;
							break;
						case 1:
							attemptedPosition.X += this.speed;
							break;
						case 2:
							attemptedPosition.Y += this.speed;
							break;
						case 3:
							attemptedPosition.X -= this.speed;
							break;
						}
						attemptedPosition.X = this.position.X - (attemptedPosition.X - this.position.X);
						attemptedPosition.Y = this.position.Y - (attemptedPosition.Y - this.position.Y);
						if (!AbigailGame.isCollidingWithMapForMonsters(attemptedPosition) && !AbigailGame.isCollidingWithMonster(attemptedPosition, this))
						{
							this.position = attemptedPosition;
						}
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							Vector2 trajectory = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							if (AbigailGame.playerMovementDirections.Count > 0)
							{
								trajectory = Utility.getTranslatedVector2(trajectory, AbigailGame.playerMovementDirections.Last<int>(), 3f);
							}
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int)trajectory.X, (int)trajectory.Y), 1));
							this.shootTimer = 250;
							Game1.playSound("Cowboy_gunshot");
						}
					}
					break;
				case 1:
					if (this.phaseInternalCounter == 0)
					{
						Point oldPosition = this.position.Location;
						if (this.position.X > this.homePosition.X + 6)
						{
							this.position.X = this.position.X - 6;
						}
						else if (this.position.X < this.homePosition.X - 6)
						{
							this.position.X = this.position.X + 6;
						}
						if (this.position.Y > this.homePosition.Y + 6)
						{
							this.position.Y = this.position.Y - 6;
						}
						else if (this.position.Y < this.homePosition.Y - 6)
						{
							this.position.Y = this.position.Y + 6;
						}
						if (this.position.Location.Equals(oldPosition))
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 1500;
						}
					}
					else if (this.phaseInternalCounter == 1)
					{
						if (this.phaseInternalTimer < 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 2000;
							this.shootTimer = 200;
							this.fireSpread(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), 0.0);
						}
					}
					else if (this.phaseInternalCounter == 2)
					{
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							this.fireSpread(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), 0.0);
							this.shootTimer = 200;
						}
						if (this.phaseInternalTimer < 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 500;
						}
					}
					else if (this.phaseInternalCounter == 3)
					{
						if (this.phaseInternalTimer < 0)
						{
							this.phaseInternalTimer = 2000;
							this.shootTimer = 200;
							this.phaseInternalCounter++;
							Vector2 trajectory2 = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int)trajectory2.X, (int)trajectory2.Y), 1));
							Game1.playSound("Cowboy_gunshot");
						}
					}
					else if (this.phaseInternalCounter == 4)
					{
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							Vector2 trajectory3 = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							trajectory3.X += (float)Game1.random.Next(-1, 2);
							trajectory3.Y += (float)Game1.random.Next(-1, 2);
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), new Point((int)trajectory3.X, (int)trajectory3.Y), 1));
							Game1.playSound("Cowboy_gunshot");
							this.shootTimer = 200;
						}
						if (this.phaseInternalTimer < 0)
						{
							if (Game1.random.NextDouble() < 0.4)
							{
								this.phase = 0;
								this.phaseInternalCounter = 0;
							}
							else
							{
								this.phaseInternalTimer = 500;
								this.phaseInternalCounter = 1;
							}
						}
					}
					break;
				case 2:
				case 3:
					if (this.phaseInternalCounter == 0)
					{
						Point oldPosition2 = this.position.Location;
						if (this.position.X > this.homePosition.X + 6)
						{
							this.position.X = this.position.X - 6;
						}
						else if (this.position.X < this.homePosition.X - 6)
						{
							this.position.X = this.position.X + 6;
						}
						if (this.position.Y > this.homePosition.Y + 6)
						{
							this.position.Y = this.position.Y - 6;
						}
						else if (this.position.Y < this.homePosition.Y - 6)
						{
							this.position.Y = this.position.Y + 6;
						}
						if (this.position.Location.Equals(oldPosition2))
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = 1500;
						}
					}
					else if (this.phaseInternalCounter == 1 && this.phaseInternalTimer < 0)
					{
						this.summonEnemies(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y + AbigailGame.TileSize / 2), Game1.random.Next(0, 5));
						if (Game1.random.NextDouble() < 0.4)
						{
							this.phase = 0;
							this.phaseInternalCounter = 0;
						}
						else
						{
							this.phaseInternalTimer = 2000;
						}
					}
					break;
				}
				return false;
			}

			// Token: 0x060013B4 RID: 5044 RVA: 0x001889D8 File Offset: 0x00186BD8
			public void fireSpread(Point origin, double offsetAngle)
			{
				Vector2[] surroundingTileLocationsArray = Utility.getSurroundingTileLocationsArray(new Vector2((float)origin.X, (float)origin.Y));
				for (int i = 0; i < surroundingTileLocationsArray.Length; i++)
				{
					Vector2 p = surroundingTileLocationsArray[i];
					Vector2 trajectory = Utility.getVelocityTowardPoint(origin, p, 6f);
					if (offsetAngle > 0.0)
					{
						offsetAngle /= 2.0;
						trajectory.X = (float)(Math.Cos(offsetAngle) * (double)(p.X - (float)origin.X) - Math.Sin(offsetAngle) * (double)(p.Y - (float)origin.Y) + (double)origin.X);
						trajectory.Y = (float)(Math.Sin(offsetAngle) * (double)(p.X - (float)origin.X) + Math.Cos(offsetAngle) * (double)(p.Y - (float)origin.Y) + (double)origin.Y);
						trajectory = Utility.getVelocityTowardPoint(origin, trajectory, 8f);
					}
					AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(origin, new Point((int)trajectory.X, (int)trajectory.Y), 1));
				}
				Game1.playSound("Cowboy_gunshot");
			}

			// Token: 0x060013B5 RID: 5045 RVA: 0x00188AF8 File Offset: 0x00186CF8
			public void summonEnemies(Point origin, int which)
			{
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2, origin.Y, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2, origin.Y)));
				}
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2, origin.Y, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2, origin.Y)));
				}
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X, origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X, origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2)));
				}
				if (!AbigailGame.isCollidingWithMonster(new Rectangle(origin.X, origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4, AbigailGame.TileSize, AbigailGame.TileSize), null))
				{
					AbigailGame.monsters.Add(new AbigailGame.CowboyMonster(which, new Point(origin.X, origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4)));
				}
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(origin.X - AbigailGame.TileSize - AbigailGame.TileSize / 2), (float)origin.Y), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(origin.X + AbigailGame.TileSize + AbigailGame.TileSize / 2), (float)origin.Y), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)origin.X, (float)(origin.Y - AbigailGame.TileSize - AbigailGame.TileSize * 3 / 4)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				AbigailGame.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(464, 1792, 16, 16), 80f, 5, 0, AbigailGame.topLeftScreenCoordinate + new Vector2((float)origin.X, (float)(origin.Y + AbigailGame.TileSize + AbigailGame.TileSize / 2)), false, false, 1f, 0f, Color.White, 3f, 0f, 0f, 0f, true)
				{
					delayBeforeAnimationStart = Game1.random.Next(800)
				});
				Game1.playSound("Cowboy_monsterDie");
			}

			// Token: 0x0400148F RID: 5263
			private const int gloatingPhase = -1;

			// Token: 0x04001490 RID: 5264
			private const int walkRandomlyAndShootPhase = 0;

			// Token: 0x04001491 RID: 5265
			private const int spreadShotPhase = 1;

			// Token: 0x04001492 RID: 5266
			private const int summonDemonPhase = 2;

			// Token: 0x04001493 RID: 5267
			private const int summonMummyPhase = 3;

			// Token: 0x04001494 RID: 5268
			private int phase = -1;

			// Token: 0x04001495 RID: 5269
			private int phaseInternalTimer;

			// Token: 0x04001496 RID: 5270
			private int phaseInternalCounter;

			// Token: 0x04001497 RID: 5271
			private int shootTimer;

			// Token: 0x04001498 RID: 5272
			private int fullHealth;

			// Token: 0x04001499 RID: 5273
			private Point homePosition;
		}

		// Token: 0x0200017C RID: 380
		public class Outlaw : AbigailGame.CowboyMonster
		{
			// Token: 0x060013B6 RID: 5046 RVA: 0x00188EE5 File Offset: 0x001870E5
			public Outlaw(Point position, int health) : base(-1, position)
			{
				this.homePosition = position;
				this.health = health;
				this.fullHealth = health;
				this.phaseCountdown = 4000;
				this.phase = -1;
			}

			// Token: 0x060013B7 RID: 5047 RVA: 0x00188F18 File Offset: 0x00187118
			public override void draw(SpriteBatch b)
			{
				b.Draw(Game1.staminaRect, new Rectangle((int)AbigailGame.topLeftScreenCoordinate.X, (int)AbigailGame.topLeftScreenCoordinate.Y + 16 * AbigailGame.TileSize + 3, (int)((float)(16 * AbigailGame.TileSize) * ((float)this.health / (float)this.fullHealth)), AbigailGame.TileSize / 3), new Color(188, 51, 74));
				if (this.flashColorTimer > 0f)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(496, 1696, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
					return;
				}
				int num = this.phase;
				if (num == -1 || num == 0)
				{
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(560 + ((this.phaseCountdown / 250 % 2 == 0) ? 16 : 0), 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
					if (this.phase == -1 && this.phaseCountdown > 1000)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)(this.position.X - AbigailGame.TileSize / 2), (float)(this.position.Y - AbigailGame.TileSize * 2)), new Rectangle?(new Rectangle(576 + ((AbigailGame.whichWave > 5) ? 32 : 0), 1792, 32, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						return;
					}
				}
				else
				{
					if (this.phase == 3 && this.phaseInternalCounter == 2)
					{
						b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(560 + ((this.phaseCountdown / 250 % 2 == 0) ? 16 : 0), 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
						return;
					}
					b.Draw(Game1.mouseCursors, AbigailGame.topLeftScreenCoordinate + new Vector2((float)this.position.X, (float)this.position.Y), new Rectangle?(new Rectangle(592 + ((this.phaseCountdown / 80 % 2 == 0) ? 16 : 0), 1776, 16, 16)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, (float)this.position.Y / 10000f + 0.001f);
				}
			}

			// Token: 0x060013B8 RID: 5048 RVA: 0x0018928C File Offset: 0x0018748C
			public override bool move(Vector2 playerPosition, GameTime time)
			{
				if (this.flashColorTimer > 0f)
				{
					this.flashColorTimer -= (float)time.ElapsedGameTime.Milliseconds;
				}
				this.phaseCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.position.X > 17 * AbigailGame.TileSize || this.position.X < -AbigailGame.TileSize)
				{
					this.position.X = 16 * AbigailGame.TileSize / 2;
				}
				switch (this.phase)
				{
				case -1:
				case 0:
					if (this.phaseCountdown < 0)
					{
						this.phase = Game1.random.Next(1, 5);
						this.dartLeft = (playerPosition.X < (float)this.position.X);
						if (playerPosition.X > (float)(7 * AbigailGame.TileSize) && playerPosition.X < (float)(9 * AbigailGame.TileSize))
						{
							if (Game1.random.NextDouble() < 0.66 || this.phase == 2)
							{
								this.phase = 4;
							}
						}
						else if (this.phase == 4)
						{
							this.phase = 3;
						}
						this.phaseInternalCounter = 0;
						this.phaseInternalTimer = 0;
					}
					break;
				case 1:
				{
					int motion = this.dartLeft ? -3 : 3;
					if (Math.Abs(this.position.Location.X - this.homePosition.X + AbigailGame.TileSize / 2) < AbigailGame.TileSize * 2 + 12 && this.phaseInternalCounter == 0)
					{
						this.position.X = this.position.X + motion;
						if (this.position.X > 256)
						{
							this.phaseInternalCounter = 2;
						}
					}
					else if (this.phaseInternalCounter == 2)
					{
						this.position.X = this.position.X - motion;
						if (Math.Abs(this.position.X - this.homePosition.X) < 4)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
					}
					else
					{
						if (this.phaseInternalCounter == 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = Game1.random.Next(1000, 2000);
						}
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-2, 3), -8), 1));
							this.shootTimer = 150;
							Game1.playSound("Cowboy_gunshot");
						}
						if (this.phaseInternalTimer <= 0)
						{
							this.phaseInternalCounter++;
						}
					}
					break;
				}
				case 2:
					if (this.phaseInternalCounter == 2)
					{
						if (this.position.X < this.homePosition.X)
						{
							this.position.X = this.position.X + 4;
						}
						else
						{
							this.position.X = this.position.X - 4;
						}
						if (Math.Abs(this.position.X - this.homePosition.X) < 5)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
						return false;
					}
					if (this.phaseInternalCounter == 0)
					{
						this.phaseInternalCounter++;
						this.phaseInternalTimer = Game1.random.Next(4000, 7000);
					}
					this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
					if ((float)this.position.X > playerPosition.X && (float)this.position.X - playerPosition.X > 3f)
					{
						this.position.X = this.position.X - 2;
					}
					else if ((float)this.position.X < playerPosition.X && playerPosition.X - (float)this.position.X > 3f)
					{
						this.position.X = this.position.X + 2;
					}
					this.shootTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.shootTimer < 0)
					{
						AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-1, 2), -8), 1));
						this.shootTimer = 250;
						if (this.fullHealth > 50)
						{
							this.shootTimer -= 50;
						}
						if (Game1.random.NextDouble() < 0.2)
						{
							this.shootTimer = 150;
						}
						Game1.playSound("Cowboy_gunshot");
					}
					if (this.phaseInternalTimer <= 0)
					{
						this.phaseInternalCounter++;
					}
					break;
				case 3:
					if (this.phaseInternalCounter == 0)
					{
						this.phaseInternalCounter++;
						this.phaseInternalTimer = Game1.random.Next(3000, 6500);
					}
					else if (this.phaseInternalCounter == 2)
					{
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.phaseInternalTimer <= 0)
						{
							this.phaseInternalCounter++;
						}
					}
					else if (this.phaseInternalCounter == 3)
					{
						if (this.position.X < this.homePosition.X)
						{
							this.position.X = this.position.X + 4;
						}
						else
						{
							this.position.X = this.position.X - 4;
						}
						if (Math.Abs(this.position.X - this.homePosition.X) < 5)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
					}
					else
					{
						int motion = this.dartLeft ? -3 : 3;
						this.position.X = this.position.X + motion;
						if (this.position.X < AbigailGame.TileSize || this.position.X > 15 * AbigailGame.TileSize)
						{
							this.dartLeft = !this.dartLeft;
						}
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point(Game1.random.Next(-1, 2), -8), 1));
							this.shootTimer = 250;
							if (this.fullHealth > 50)
							{
								this.shootTimer -= 50;
							}
							if (Game1.random.NextDouble() < 0.2)
							{
								this.shootTimer = 150;
							}
							Game1.playSound("Cowboy_gunshot");
						}
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.phaseInternalTimer <= 0)
						{
							if (this.phase == 2)
							{
								this.phaseInternalCounter = 3;
							}
							else
							{
								this.phaseInternalTimer = 3000;
								this.phaseInternalCounter++;
							}
						}
					}
					break;
				case 4:
				{
					int motion = this.dartLeft ? -3 : 3;
					if (this.phaseInternalCounter == 0 && (playerPosition.X <= (float)(7 * AbigailGame.TileSize) || playerPosition.X >= (float)(9 * AbigailGame.TileSize)))
					{
						this.phaseInternalCounter = 1;
						this.phaseInternalTimer = Game1.random.Next(500, 1500);
					}
					else if (Math.Abs(this.position.Location.X - this.homePosition.X + AbigailGame.TileSize / 2) < AbigailGame.TileSize * 7 + 12 && this.phaseInternalCounter == 0)
					{
						this.position.X = this.position.X + motion;
					}
					else if (this.phaseInternalCounter == 2)
					{
						motion = (this.dartLeft ? -4 : 4);
						this.position.X = this.position.X - motion;
						if (Math.Abs(this.position.X - this.homePosition.X) < 4)
						{
							this.position.X = this.homePosition.X;
							this.phase = 0;
							this.phaseCountdown = Game1.random.Next(1000, 2000);
						}
					}
					else
					{
						if (this.phaseInternalCounter == 0)
						{
							this.phaseInternalCounter++;
							this.phaseInternalTimer = Game1.random.Next(1000, 2000);
						}
						this.phaseInternalTimer -= time.ElapsedGameTime.Milliseconds;
						this.shootTimer -= time.ElapsedGameTime.Milliseconds;
						if (this.shootTimer < 0)
						{
							Vector2 trajectory = Utility.getVelocityTowardPoint(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y), playerPosition + new Vector2((float)(AbigailGame.TileSize / 2), (float)(AbigailGame.TileSize / 2)), 8f);
							AbigailGame.enemyBullets.Add(new AbigailGame.CowboyBullet(new Point(this.position.X + AbigailGame.TileSize / 2, this.position.Y - AbigailGame.TileSize / 2), new Point((int)trajectory.X, (int)trajectory.Y), 1));
							this.shootTimer = 120;
							Game1.playSound("Cowboy_gunshot");
						}
						if (this.phaseInternalTimer <= 0)
						{
							this.phaseInternalCounter++;
						}
					}
					break;
				}
				}
				if (this.position.X <= 16 * AbigailGame.TileSize)
				{
					int arg_A90_0 = this.position.X;
				}
				return false;
			}

			// Token: 0x060013B9 RID: 5049 RVA: 0x0003309A File Offset: 0x0003129A
			public override int getLootDrop()
			{
				return 8;
			}

			// Token: 0x060013BA RID: 5050 RVA: 0x00189D2C File Offset: 0x00187F2C
			public override bool takeDamage(int damage)
			{
				if (Math.Abs(this.position.X - this.homePosition.X) < 5)
				{
					return false;
				}
				this.health -= damage;
				if (this.health < 0)
				{
					return true;
				}
				this.flashColorTimer = 150f;
				Game1.playSound("cowboy_monsterhit");
				return false;
			}

			// Token: 0x0400149A RID: 5274
			private const int talkingPhase = -1;

			// Token: 0x0400149B RID: 5275
			private const int hidingPhase = 0;

			// Token: 0x0400149C RID: 5276
			private const int dartOutAndShootPhase = 1;

			// Token: 0x0400149D RID: 5277
			private const int runAndGunPhase = 2;

			// Token: 0x0400149E RID: 5278
			private const int runGunAndPantPhase = 3;

			// Token: 0x0400149F RID: 5279
			private const int shootAtPlayerPhase = 4;

			// Token: 0x040014A0 RID: 5280
			private int phase;

			// Token: 0x040014A1 RID: 5281
			private int phaseCountdown;

			// Token: 0x040014A2 RID: 5282
			private int shootTimer;

			// Token: 0x040014A3 RID: 5283
			private int phaseInternalTimer;

			// Token: 0x040014A4 RID: 5284
			private int phaseInternalCounter;

			// Token: 0x040014A5 RID: 5285
			public bool dartLeft;

			// Token: 0x040014A6 RID: 5286
			private int fullHealth;

			// Token: 0x040014A7 RID: 5287
			private Point homePosition;
		}
	}
}
