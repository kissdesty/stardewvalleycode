using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	// Token: 0x02000135 RID: 309
	public class Woods : GameLocation
	{
		// Token: 0x060011AF RID: 4527 RVA: 0x0016A973 File Offset: 0x00168B73
		public Woods()
		{
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0016A986 File Offset: 0x00168B86
		public Woods(Map map, string name) : base(map, name)
		{
			this.isOutdoors = true;
			this.ignoreDebrisWeather = true;
			this.ignoreOutdoorLighting = true;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0016A9B0 File Offset: 0x00168BB0
		public override void checkForMusic(GameTime time)
		{
			if ((Game1.currentSong == null || !Game1.currentSong.IsPlaying) && (Game1.nextMusicTrack == null || Game1.nextMusicTrack.Length == 0))
			{
				if (Game1.isRaining)
				{
					Game1.changeMusicTrack("rain");
					return;
				}
				Game1.changeMusicTrack(Game1.currentSeason + "_day_ambient");
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0016AA0C File Offset: 0x00168C0C
		public void statueAnimation(Farmer who)
		{
			this.hasUnlockedStatue = true;
			who.reduceActiveItemByOne();
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 7f) * (float)Game1.tileSize, Color.White, 9, false, 50f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 7f) * (float)Game1.tileSize, Color.Orange, 9, false, 70f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 6f) * (float)Game1.tileSize, Color.White, 9, false, 60f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 6f) * (float)Game1.tileSize, Color.OrangeRed, 9, false, 120f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 5f) * (float)Game1.tileSize, Color.Red, 9, false, 100f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 5f) * (float)Game1.tileSize, Color.White, 9, false, 170f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.Orange, 9, false, 40f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(9 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.White, 9, false, 90f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize + Game1.tileSize / 4)), Color.OrangeRed, 9, false, 190f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(9 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize + Game1.tileSize / 4)), Color.White, 9, false, 80f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(5 * Game1.tileSize + Game1.tileSize / 4)), Color.Red, 9, false, 69f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(9 * Game1.tileSize + Game1.tileSize / 2), (float)(5 * Game1.tileSize + Game1.tileSize / 4)), Color.OrangeRed, 9, false, 130f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float)(7 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.Orange, 9, false, 40f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(10 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), Color.White, 9, false, 90f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float)(7 * Game1.tileSize + Game1.tileSize / 2), (float)(7 * Game1.tileSize + Game1.tileSize / 4)), Color.Red, 9, false, 30f, 0, -1, -1f, -1, 0));
			this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float)(10 * Game1.tileSize + Game1.tileSize / 2), (float)(6 * Game1.tileSize - Game1.tileSize / 4)), Color.White, 9, false, 180f, 0, -1, -1f, -1, 0));
			Game1.playSound("secret1");
			this.map.GetLayer("Front").Tiles[8, 6].TileIndex += 2;
			this.map.GetLayer("Front").Tiles[9, 6].TileIndex += 2;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0016AF18 File Offset: 0x00169118
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null && who.IsMainPlayer)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex == 1140 || tileIndex == 1141)
				{
					if (!this.hasUnlockedStatue)
					{
						if (who.ActiveObject != null && who.ActiveObject.ParentSheetIndex == 417)
						{
							this.statueTimer = 1000;
							who.freezePause = 1000;
							who.FarmerSprite.ignoreDefaultActionThisTime = true;
							Game1.changeMusicTrack("none");
							Game1.playSound("newArtifact");
						}
						else
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Woods_Statue", new object[0]).Replace('\n', '^'));
						}
					}
					if (this.hasUnlockedStatue && !this.hasFoundStardrop && who.freeSpotsInInventory() > 0)
					{
						who.addItemByMenuIfNecessaryElseHoldUp(new Object(434, 1, false, -1, 0), null);
						this.hasFoundStardrop = true;
						if (!Game1.player.mailReceived.Contains("CF_Statue"))
						{
							Game1.player.mailReceived.Add("CF_Statue");
						}
					}
					return true;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0016B070 File Offset: 0x00169270
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			foreach (ResourceClump expr_15 in this.stumps)
			{
				if (expr_15.getBoundingBox(expr_15.tile).Intersects(position))
				{
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0016B0E8 File Offset: 0x001692E8
		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (t is Axe)
			{
				Point p = new Point(tileX * Game1.tileSize + Game1.tileSize / 2, tileY * Game1.tileSize + Game1.tileSize / 2);
				for (int i = this.stumps.Count - 1; i >= 0; i--)
				{
					if (this.stumps[i].getBoundingBox(this.stumps[i].tile).Contains(p))
					{
						if (this.stumps[i].performToolAction(t, 1, this.stumps[i].tile, null))
						{
							this.stumps.RemoveAt(i);
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0016B1A0 File Offset: 0x001693A0
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.characters.Clear();
			this.addedSlimesToday = false;
			PropertyValue rawStumpData;
			this.map.Properties.TryGetValue("Stumps", out rawStumpData);
			if (rawStumpData != null)
			{
				string[] stumpData = rawStumpData.ToString().Split(new char[]
				{
					' '
				});
				for (int i = 0; i < stumpData.Length; i += 3)
				{
					int x = Convert.ToInt32(stumpData[i]);
					int y = Convert.ToInt32(stumpData[i + 1]);
					Vector2 tile = new Vector2((float)x, (float)y);
					bool foundStump = false;
					using (List<ResourceClump>.Enumerator enumerator = this.stumps.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.tile.Equals(tile))
							{
								foundStump = true;
								break;
							}
						}
					}
					if (!foundStump)
					{
						this.stumps.Add(new ResourceClump(600, 2, 2, tile));
						base.removeObject(tile, false);
						base.removeObject(tile + new Vector2(1f, 0f), false);
						base.removeObject(tile + new Vector2(1f, 1f), false);
						base.removeObject(tile + new Vector2(0f, 1f), false);
					}
				}
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0016B308 File Offset: 0x00169508
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
			if (this.baubles != null)
			{
				this.baubles.Clear();
			}
			if (this.weatherDebris != null)
			{
				this.weatherDebris.Clear();
			}
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0016B340 File Offset: 0x00169540
		public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
		{
			using (List<ResourceClump>.Enumerator enumerator = this.stumps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.occupiesTile((int)v.X, (int)v.Y))
					{
						return false;
					}
				}
			}
			return base.isTileLocationTotallyClearAndPlaceable(v);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0016B3B0 File Offset: 0x001695B0
		public override void resetForPlayerEntry()
		{
			if (!Game1.player.mailReceived.Contains("beenToWoods"))
			{
				Game1.player.mailReceived.Add("beenToWoods");
			}
			if (!this.addedSlimesToday)
			{
				this.addedSlimesToday = true;
				Random rand = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame) + 12u));
				for (int tries = 50; tries > 0; tries--)
				{
					Vector2 tile = base.getRandomTile();
					if (rand.NextDouble() < 0.25 && this.isTileLocationTotallyClearAndPlaceable(tile))
					{
						string currentSeason = Game1.currentSeason;
						if (!(currentSeason == "spring"))
						{
							if (!(currentSeason == "summer"))
							{
								if (!(currentSeason == "fall"))
								{
									if (currentSeason == "winter")
									{
										this.characters.Add(new GreenSlime(tile * (float)Game1.tileSize, 40));
									}
								}
								else
								{
									this.characters.Add(new GreenSlime(tile * (float)Game1.tileSize, (rand.NextDouble() < 0.5) ? 0 : 40));
								}
							}
							else
							{
								this.characters.Add(new GreenSlime(tile * (float)Game1.tileSize, 0));
							}
						}
						else
						{
							this.characters.Add(new GreenSlime(tile * (float)Game1.tileSize, 0));
						}
					}
				}
			}
			if (Game1.timeOfDay > 1600)
			{
				this.ignoreOutdoorLighting = false;
				this.ignoreLights = true;
			}
			else
			{
				this.ignoreOutdoorLighting = true;
				this.ignoreLights = false;
			}
			base.resetForPlayerEntry();
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			int numberOfBaubles = 25 + r.Next(0, 75);
			if (!Game1.isRaining)
			{
				this.baubles = new List<Vector2>();
				for (int i = 0; i < numberOfBaubles; i++)
				{
					this.baubles.Add(new Vector2((float)Game1.random.Next(0, this.map.DisplayWidth), (float)Game1.random.Next(0, this.map.DisplayHeight)));
				}
				if (!Game1.currentSeason.Equals("winter"))
				{
					this.weatherDebris = new List<WeatherDebris>();
					int spacing = Game1.tileSize * 3;
					for (int j = 0; j < numberOfBaubles; j++)
					{
						this.weatherDebris.Add(new WeatherDebris(new Vector2((float)(j * spacing % Game1.graphics.GraphicsDevice.Viewport.Width + Game1.random.Next(spacing)), (float)(j * spacing / Game1.graphics.GraphicsDevice.Viewport.Width * spacing + Game1.random.Next(spacing))), 1, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
					}
				}
			}
			if (Game1.timeOfDay < 1800)
			{
				Game1.changeMusicTrack("woodsTheme");
			}
			if (this.hasUnlockedStatue && !this.hasFoundStardrop)
			{
				this.map.GetLayer("Front").Tiles[8, 6].TileIndex += 2;
				this.map.GetLayer("Front").Tiles[9, 6].TileIndex += 2;
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0016B744 File Offset: 0x00169944
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.statueTimer > 0)
			{
				this.statueTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.statueTimer <= 0)
				{
					this.statueAnimation(Game1.player);
				}
			}
			if (this.baubles != null)
			{
				for (int i = 0; i < this.baubles.Count; i++)
				{
					Vector2 v = default(Vector2);
					v.X = this.baubles[i].X - Math.Max(0.4f, Math.Min(1f, (float)i * 0.01f)) - (float)((double)((float)i * 0.01f) * Math.Sin(6.2831853071795862 * (double)time.TotalGameTime.Milliseconds / 8000.0));
					v.Y = this.baubles[i].Y + Math.Max(0.5f, Math.Min(1.2f, (float)i * 0.02f));
					if (v.Y > (float)this.map.DisplayHeight || v.X < 0f)
					{
						v.X = (float)Game1.random.Next(0, this.map.DisplayWidth);
						v.Y = (float)(-(float)Game1.tileSize);
					}
					this.baubles[i] = v;
				}
			}
			if (this.weatherDebris != null)
			{
				using (List<WeatherDebris>.Enumerator enumerator = this.weatherDebris.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.update();
					}
				}
				Game1.updateDebrisWeatherForMovement(this.weatherDebris);
			}
			foreach (ResourceClump stump in this.stumps)
			{
				stump.tickUpdate(time, stump.tile);
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0016B95C File Offset: 0x00169B5C
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (!Game1.eventUp || (this.currentEvent != null && this.currentEvent.showGroundObjects))
			{
				foreach (ResourceClump stump in this.stumps)
				{
					stump.draw(b, stump.tile);
				}
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0016B9D8 File Offset: 0x00169BD8
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			if (this.baubles != null)
			{
				for (int i = 0; i < this.baubles.Count; i++)
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.baubles[i]), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(346 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(i * 25)) % 600.0) / 150 * 5, 1971, 5, 5)), Color.White, (float)i * 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
			}
			if (this.weatherDebris != null && this.currentEvent == null)
			{
				using (List<WeatherDebris>.Enumerator enumerator = this.weatherDebris.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
			}
		}

		// Token: 0x0400128A RID: 4746
		public const int numBaubles = 25;

		// Token: 0x0400128B RID: 4747
		private List<Vector2> baubles;

		// Token: 0x0400128C RID: 4748
		private List<WeatherDebris> weatherDebris;

		// Token: 0x0400128D RID: 4749
		public List<ResourceClump> stumps = new List<ResourceClump>();

		// Token: 0x0400128E RID: 4750
		public bool hasUnlockedStatue;

		// Token: 0x0400128F RID: 4751
		public bool hasFoundStardrop;

		// Token: 0x04001290 RID: 4752
		private bool addedSlimesToday;

		// Token: 0x04001291 RID: 4753
		private int statueTimer;
	}
}
