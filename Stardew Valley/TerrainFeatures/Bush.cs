using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200006C RID: 108
	public class Bush : LargeTerrainFeature
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x000C915C File Offset: 0x000C735C
		public Bush()
		{
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000C9178 File Offset: 0x000C7378
		public Bush(Vector2 tileLocation, int size, GameLocation location)
		{
			this.tilePosition = tileLocation;
			this.size = size;
			if (size == 0)
			{
				this.tileSheetOffset = Game1.random.Next(2);
			}
			if (location is Town && tileLocation.X % 5f != 0f)
			{
				this.townBush = true;
			}
			if (location.map.GetLayer("Front").Tiles[(int)tileLocation.X, (int)tileLocation.Y] != null)
			{
				this.drawShadow = false;
			}
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000C9234 File Offset: 0x000C7434
		public void setUpSourceRect()
		{
			int seasonNumber = Utility.getSeasonNumber(Game1.currentSeason);
			if (this.size == 0)
			{
				this.sourceRect = new Rectangle(seasonNumber * 16 * 2 + this.tileSheetOffset * 16, 224, 16, 32);
				return;
			}
			if (this.size != 1)
			{
				if (this.size == 2)
				{
					if (this.townBush && (seasonNumber == 0 || seasonNumber == 1))
					{
						this.sourceRect = new Rectangle(48, 176, 48, 48);
						return;
					}
					switch (seasonNumber)
					{
					case 0:
					case 1:
						this.sourceRect = new Rectangle(0, 128, 48, 48);
						return;
					case 2:
						this.sourceRect = new Rectangle(48, 128, 48, 48);
						return;
					case 3:
						this.sourceRect = new Rectangle(0, 176, 48, 48);
						break;
					default:
						return;
					}
				}
				return;
			}
			if (this.townBush)
			{
				this.sourceRect = new Rectangle(seasonNumber * 16 * 2, 96, 32, 32);
				return;
			}
			this.sourceRect = new Rectangle((seasonNumber * 16 * 4 + this.tileSheetOffset * 16 * 2) % Bush.texture.Bounds.Width, (seasonNumber * 16 * 4 + this.tileSheetOffset * 16 * 2) / Bush.texture.Bounds.Width * 3 * 16, 32, 48);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000C9383 File Offset: 0x000C7583
		public bool inBloom(string season, int dayOfMonth)
		{
			if (season.Equals("spring"))
			{
				if (dayOfMonth > 14 && dayOfMonth < 19)
				{
					return true;
				}
			}
			else if (season.Equals("fall") && dayOfMonth > 7 && dayOfMonth < 12)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool isActionable()
		{
			return true;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000C93B8 File Offset: 0x000C75B8
		public override void loadSprite()
		{
			if (Bush.texture == null)
			{
				try
				{
					Bush.texture = Game1.content.Load<Texture2D>("TileSheets\\bushes");
				}
				catch (Exception)
				{
				}
			}
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame) + (uint)((int)this.tilePosition.X) + (uint)((int)this.tilePosition.Y * 777)));
			if (this.size == 1 && this.tileSheetOffset == 0 && r.NextDouble() < 0.5 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 1;
			}
			else if (!Game1.currentSeason.Equals("summer") && !this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 0;
			}
			this.setUpSourceRect();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000C949C File Offset: 0x000C769C
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			switch (this.size)
			{
			case 0:
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			case 1:
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
			case 2:
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize);
			default:
				return Rectangle.Empty;
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x000C954E File Offset: 0x000C774E
		public override bool performUseAction(Vector2 tileLocation)
		{
			if (this.maxShake == 0f)
			{
				Game1.playSound("leafrustle");
			}
			this.shake(tileLocation, false);
			return true;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000C9570 File Offset: 0x000C7770
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
			if (this.maxShake > 0f)
			{
				if (this.shakeLeft)
				{
					this.shakeRotation -= 0.0157079641f;
					if (this.shakeRotation <= -this.maxShake)
					{
						this.shakeLeft = false;
					}
				}
				else
				{
					this.shakeRotation += 0.0157079641f;
					if (this.shakeRotation >= this.maxShake)
					{
						this.shakeLeft = true;
					}
				}
			}
			if (this.maxShake > 0f)
			{
				this.maxShake = Math.Max(0f, this.maxShake - 0.00306796166f);
			}
			return false;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x000C9654 File Offset: 0x000C7854
		private void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
		{
			if (this.maxShake == 0f | doEvenIfStillShaking)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = 0.0245436933f;
				if (!this.townBush && this.tileSheetOffset == 1 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
				{
					int shakeOff = -1;
					string currentSeason = Game1.currentSeason;
					if (!(currentSeason == "spring"))
					{
						if (currentSeason == "fall")
						{
							shakeOff = 410;
						}
					}
					else
					{
						shakeOff = 296;
					}
					if (shakeOff != -1)
					{
						this.tileSheetOffset = 0;
						this.setUpSourceRect();
						int number = new Random((int)tileLocation.X + (int)tileLocation.Y * 5000 + (int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed).Next(1, 2) + Game1.player.ForagingLevel / 4;
						for (int i = 0; i < number; i++)
						{
							Game1.createItemDebris(new Object(shakeOff, 1, false, -1, Game1.player.professions.Contains(16) ? 4 : 0), Utility.PointToVector2(base.getBoundingBox().Center), Game1.random.Next(1, 4), null);
						}
						DelayedAction.playSoundAfterDelay("leafrustle", 100);
						return;
					}
				}
				else if (tileLocation.X == 20f && tileLocation.Y == 8f && Game1.dayOfMonth == 28 && Game1.timeOfDay == 1200 && !Game1.player.mailReceived.Contains("junimoPlush"))
				{
					Game1.player.addItemByMenuIfNecessaryElseHoldUp(new Furniture(1733, Vector2.Zero), new ItemGrabMenu.behaviorOnItemSelect(this.junimoPlushCallback));
				}
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000C984C File Offset: 0x000C7A4C
		public void junimoPlushCallback(Item item, Farmer who)
		{
			if (item != null && item is Furniture && (item as Furniture).parentSheetIndex == 1733 && who != null)
			{
				who.mailReceived.Add("junimoPlush");
			}
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPassable(Character c = null)
		{
			return false;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x000C9880 File Offset: 0x000C7A80
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
			if (this.size == 1 && this.tileSheetOffset == 0 && Game1.random.NextDouble() < 0.2 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 1;
				this.setUpSourceRect();
			}
			else if (!Game1.currentSeason.Equals("summer") && !this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
			{
				this.tileSheetOffset = 0;
				this.setUpSourceRect();
			}
			this.health = 0f;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000C9910 File Offset: 0x000C7B10
		public override bool seasonUpdate(bool onLoad)
		{
			if (this.size == 1 && Game1.currentSeason.Equals("summer") && Game1.random.NextDouble() < 0.5)
			{
				this.tileSheetOffset = 1;
			}
			else
			{
				this.tileSheetOffset = 0;
			}
			this.loadSprite();
			return false;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000C9964 File Offset: 0x000C7B64
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if (explosion > 0)
			{
				this.shake(tileLocation, true);
				return false;
			}
			if (t != null && t is Axe && this.isDestroyable(location, tileLocation))
			{
				Game1.soundBank.PlayCue("leafrustle");
				this.shake(tileLocation, true);
				if ((t as Axe).upgradeLevel >= 1)
				{
					this.health -= (float)(t as Axe).upgradeLevel / 5f;
					if (this.health <= -1f)
					{
						Game1.soundBank.PlayCue("treethud");
						DelayedAction.playSoundAfterDelay("leafrustle", 100);
						Color c = Color.Green;
						string currentSeason = Game1.currentSeason;
						if (!(currentSeason == "spring"))
						{
							if (!(currentSeason == "summer"))
							{
								if (!(currentSeason == "fall"))
								{
									if (currentSeason == "winter")
									{
										c = Color.Cyan;
									}
								}
								else
								{
									c = Color.IndianRed;
								}
							}
							else
							{
								c = Color.ForestGreen;
							}
						}
						else
						{
							c = Color.Green;
						}
						for (int i = 0; i <= this.size; i++)
						{
							for (int j = 0; j < 12; j++)
							{
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(355, 1200 + (Game1.IsFall ? 16 : (Game1.IsWinter ? -16 : 0)), 16, 16), Utility.getRandomPositionInThisRectangle(base.getBoundingBox(), Game1.random) - new Vector2(0f, (float)Game1.random.Next(Game1.tileSize)), false, 0.01f, Game1.IsWinter ? Color.Cyan : Color.White)
								{
									motion = new Vector2((float)Game1.random.Next(-10, 11) / 10f, (float)(-(float)Game1.random.Next(5, 7))),
									acceleration = new Vector2(0f, (float)Game1.random.Next(13, 17) / 100f),
									accelerationChange = new Vector2(0f, -0.001f),
									scale = (float)Game1.pixelZoom,
									layerDepth = tileLocation.Y * (float)Game1.tileSize / 10000f,
									animationLength = 11,
									totalNumberOfLoops = 99,
									interval = (float)Game1.random.Next(20, 90),
									delayBeforeAnimationStart = (i + 1) * j * 20
								});
								if (j % 6 == 0)
								{
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(50, Utility.getRandomPositionInThisRectangle(base.getBoundingBox(), Game1.random) - new Vector2((float)(Game1.tileSize / 2), (float)Game1.random.Next(Game1.tileSize / 2, Game1.tileSize)), c, 8, false, 100f, 0, -1, -1f, -1, 0));
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(12, Utility.getRandomPositionInThisRectangle(base.getBoundingBox(), Game1.random) - new Vector2((float)(Game1.tileSize / 2), (float)Game1.random.Next(Game1.tileSize / 2, Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
								}
							}
						}
						return true;
					}
					Game1.soundBank.PlayCue("axchop");
				}
			}
			return false;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000C9CC8 File Offset: 0x000C7EC8
		public bool isDestroyable(GameLocation location, Vector2 tile)
		{
			if (location != null && location is Farm)
			{
				switch (Game1.whichFarm)
				{
				case 1:
					return new Rectangle(32, 11, 11, 25).Contains((int)tile.X, (int)tile.Y);
				case 2:
					return (tile.X == 13f && tile.Y == 35f) || (tile.X == 37f && tile.Y == 9f) || new Rectangle(43, 11, 34, 50).Contains((int)tile.X, (int)tile.Y);
				case 3:
					return new Rectangle(24, 56, 10, 8).Contains((int)tile.X, (int)tile.Y);
				}
			}
			return false;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000C9DAC File Offset: 0x000C7FAC
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			layerDepth += positionOnScreen.X / 100000f;
			spriteBatch.Draw(Bush.texture, positionOnScreen + new Vector2(0f, (float)(-(float)Game1.tileSize) * scale), new Rectangle?(new Rectangle(32, 96, 16, 32)), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale - 1f) / 20000f);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000C9E40 File Offset: 0x000C8040
		public override void performPlayerEntryAction(Vector2 tileLocation)
		{
			base.performPlayerEntryAction(tileLocation);
			if (!Game1.currentSeason.Equals("winter") && !Game1.isRaining && Game1.isDarkOut() && Game1.random.NextDouble() < (Game1.currentSeason.Equals("summer") ? 0.08 : 0.04))
			{
				AmbientLocationSounds.addSound(tileLocation, 3);
				Game1.debugOutput = Game1.debugOutput + "  added cricket at " + tileLocation.ToString();
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000C9ECC File Offset: 0x000C80CC
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.drawShadow)
			{
				if (this.size > 0)
				{
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((tileLocation.X + ((this.size == 1) ? 0.5f : 1f)) * (float)Game1.tileSize - (float)(Game1.tileSize * 4 / 5), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 4))), new Rectangle?(Bush.shadowSourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
				}
				else
				{
					spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize - (float)Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 1E-06f);
				}
			}
			spriteBatch.Draw(Bush.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)((this.size + 1) * Game1.tileSize / 2), (tileLocation.Y + 1f) * (float)Game1.tileSize - (float)((this.size > 0 && (!this.townBush || this.size != 1)) ? Game1.tileSize : 0))), new Rectangle?(this.sourceRect), Color.White * this.alpha, this.shakeRotation, new Vector2((float)((this.size + 1) * 16 / 2), 32f), (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.getBoundingBox(tileLocation).Center.Y + 48) / 10000f - tileLocation.X / 1000000f);
		}

		// Token: 0x04000941 RID: 2369
		public const float shakeRate = 0.0157079641f;

		// Token: 0x04000942 RID: 2370
		public const float shakeDecayRate = 0.00306796166f;

		// Token: 0x04000943 RID: 2371
		public const int smallBush = 0;

		// Token: 0x04000944 RID: 2372
		public const int mediumBush = 1;

		// Token: 0x04000945 RID: 2373
		public const int largeBush = 2;

		// Token: 0x04000946 RID: 2374
		public static Texture2D texture;

		// Token: 0x04000947 RID: 2375
		public int size;

		// Token: 0x04000948 RID: 2376
		public int tileSheetOffset;

		// Token: 0x04000949 RID: 2377
		public float health;

		// Token: 0x0400094A RID: 2378
		public bool flipped;

		// Token: 0x0400094B RID: 2379
		public bool townBush;

		// Token: 0x0400094C RID: 2380
		public bool drawShadow = true;

		// Token: 0x0400094D RID: 2381
		private bool shakeLeft;

		// Token: 0x0400094E RID: 2382
		private float shakeRotation;

		// Token: 0x0400094F RID: 2383
		private float maxShake;

		// Token: 0x04000950 RID: 2384
		private float alpha = 1f;

		// Token: 0x04000951 RID: 2385
		private long lastPlayerToHit;

		// Token: 0x04000952 RID: 2386
		private float shakeTimer;

		// Token: 0x04000953 RID: 2387
		private Rectangle sourceRect;

		// Token: 0x04000954 RID: 2388
		public static Rectangle treeTopSourceRect = new Rectangle(0, 0, 48, 96);

		// Token: 0x04000955 RID: 2389
		public static Rectangle stumpSourceRect = new Rectangle(32, 96, 16, 32);

		// Token: 0x04000956 RID: 2390
		public static Rectangle shadowSourceRect = new Rectangle(663, 1011, 41, 30);
	}
}
