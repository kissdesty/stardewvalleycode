using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	// Token: 0x02000129 RID: 297
	public class MineShaft : GameLocation
	{
		// Token: 0x060010E8 RID: 4328 RVA: 0x00159CB0 File Offset: 0x00157EB0
		public MineShaft()
		{
			this.name = "UndergroundMine";
			this.permanentMineChanges = new SerializableDictionary<int, MineInfo>();
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00159D20 File Offset: 0x00157F20
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			foreach (ResourceClump r in this.resourceClumps)
			{
				r.tickUpdate(time, r.tile);
			}
			if (Game1.currentSong != null && (!Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("Ambient")) && Game1.random.NextDouble() < 0.00195)
			{
				Game1.playSound("cavedrip");
			}
			if (this.timeUntilElevatorLightUp > 0)
			{
				this.timeUntilElevatorLightUp -= time.ElapsedGameTime.Milliseconds;
				if (this.timeUntilElevatorLightUp <= 0)
				{
					Game1.playSound("crystal");
					base.setMapTileIndex(this.ElevatorLightSpot.X, this.ElevatorLightSpot.Y, 48, "Buildings", 0);
					Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)this.ElevatorLightSpot.X, (float)this.ElevatorLightSpot.Y) * (float)Game1.tileSize, 2f, Color.Black, this.ElevatorLightSpot.X + this.ElevatorLightSpot.Y * 1000));
				}
			}
			if (this.fogTime > 0 && Game1.shouldTimePass())
			{
				if (Game1.soundBank != null && (this.bugLevelLoop == null || this.bugLevelLoop.IsStopped))
				{
					this.bugLevelLoop = Game1.soundBank.GetCue("bugLevelLoop");
					this.bugLevelLoop.Play();
				}
				if (this.fogAlpha < 1f)
				{
					this.fogAlpha += 0.01f;
					if (this.bugLevelLoop != null && Game1.soundBank != null)
					{
						this.bugLevelLoop.SetVariable("Volume", this.fogAlpha * 100f);
						this.bugLevelLoop.SetVariable("Frequency", this.fogAlpha * 25f);
					}
				}
				else if (this.bugLevelLoop != null && Game1.soundBank != null)
				{
					float f = (float)Math.Max(0.0, Math.Min(100.0, Math.Sin((double)((float)this.fogTime / 10000f) % 628.31853071795865)));
					this.bugLevelLoop.SetVariable("Frequency", Math.Max(0f, Math.Min(100f, this.fogAlpha * 25f + f * 10f)));
				}
				int oldTime = this.fogTime;
				this.fogTime -= (int)time.ElapsedGameTime.TotalMilliseconds;
				if (this.fogTime > 5000 && oldTime % 4000 < this.fogTime % 4000)
				{
					this.spawnFlyingMonsterOffScreen();
				}
				Vector2 currentViewport = new Vector2((float)Game1.viewport.X, (float)Game1.viewport.Y);
				this.fogPos = Game1.updateFloatingObjectPositionForMovement(this.fogPos, currentViewport, Game1.previousViewportPosition, -1f);
				this.fogPos.X = (this.fogPos.X + 0.5f) % (float)(64 * Game1.pixelZoom);
				this.fogPos.Y = (this.fogPos.Y + 0.5f) % (float)(64 * Game1.pixelZoom);
			}
			else if (this.fogAlpha > 0f)
			{
				this.fogAlpha -= 0.01f;
				if (this.bugLevelLoop != null)
				{
					this.bugLevelLoop.SetVariable("Volume", this.fogAlpha * 100f);
					this.bugLevelLoop.SetVariable("Frequency", Math.Max(0f, this.bugLevelLoop.GetVariable("Frequency") - 0.01f));
					if (this.fogAlpha <= 0f)
					{
						this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
						this.bugLevelLoop = null;
					}
				}
			}
			else if (this.ambientFog)
			{
				Vector2 currentViewport2 = new Vector2((float)Game1.viewport.X, (float)Game1.viewport.Y);
				this.fogPos = Game1.updateFloatingObjectPositionForMovement(this.fogPos, currentViewport2, Game1.previousViewportPosition, -1f);
				this.fogPos.X = (this.fogPos.X + 0.5f) % (float)(64 * Game1.pixelZoom);
				this.fogPos.Y = (this.fogPos.Y + 0.5f) % (float)(64 * Game1.pixelZoom);
			}
			base.UpdateWhenCurrentLocation(time);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0015A1E4 File Offset: 0x001583E4
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (this.bugLevelLoop != null)
			{
				this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
				this.bugLevelLoop = null;
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0015A207 File Offset: 0x00158407
		public void setNextLevel(int level)
		{
			this.nextLevel = level;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0015A210 File Offset: 0x00158410
		public override int getExtraMillisecondsPerInGameMinuteForThisLocation()
		{
			if (this.getMineArea(-1) != 121)
			{
				return 0;
			}
			return 2000;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0015A224 File Offset: 0x00158424
		public Vector2 enterMine(Farmer who, int mineLevel, bool ridingElevator)
		{
			this.mineRandom = new Random();
			this.ladderHasSpawned = false;
			this.loadLevel(this.nextLevel);
			this.chooseLevelType();
			this.findLadder();
			this.populateLevel();
			if (!who.ridingMineElevator || this.tileBeneathElevator.Equals(Vector2.Zero))
			{
				return this.tileBeneathLadder;
			}
			return this.tileBeneathElevator;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0015A288 File Offset: 0x00158488
		public void chooseLevelType()
		{
			this.fogTime = 0;
			if (this.bugLevelLoop != null)
			{
				this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
				this.bugLevelLoop = null;
			}
			this.ambientFog = false;
			this.rainbowLights = false;
			this.isLightingDark = false;
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)this.mineLevel + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			this.lighting = new Color(80, 80, 40);
			if (this.getMineArea(-1) == 80)
			{
				this.lighting = new Color(100, 100, 50);
			}
			if (r.NextDouble() < 0.3 && this.mineLevel > 2)
			{
				this.isLightingDark = true;
				this.lighting = new Color(120, 120, 60);
				if (r.NextDouble() < 0.3)
				{
					this.lighting = new Color(150, 150, 120);
				}
			}
			if (r.NextDouble() < 0.15 && this.mineLevel > 5 && this.mineLevel != 120)
			{
				this.isLightingDark = true;
				int mineArea = this.getMineArea(-1);
				if (mineArea <= 10)
				{
					if (mineArea == 0 || mineArea == 10)
					{
						this.lighting = new Color(110, 110, 70);
					}
				}
				else if (mineArea != 40)
				{
					if (mineArea == 80)
					{
						this.lighting = new Color(90, 130, 70);
					}
				}
				else
				{
					this.lighting = Color.Black;
				}
			}
			if (r.NextDouble() < 0.035 && this.getMineArea(-1) == 80 && this.mineLevel % 5 != 0)
			{
				this.rainbowLights = true;
			}
			if (this.isDarkArea() && this.mineLevel < 120)
			{
				this.isLightingDark = true;
				this.lighting = ((this.getMineArea(-1) == 80) ? new Color(70, 100, 100) : new Color(150, 150, 120));
				if (this.getMineArea(-1) == 0)
				{
					this.ambientFog = true;
				}
			}
			if (this.mineLevel == 100)
			{
				this.lighting = new Color(140, 140, 80);
			}
			if (this.getMineArea(-1) == 121)
			{
				this.lighting = new Color(110, 110, 40);
				if (r.NextDouble() < 0.05)
				{
					this.lighting = ((r.NextDouble() < 0.5) ? new Color(30, 30, 0) : new Color(150, 150, 50));
				}
			}
			if (this.mineLevel > 1 && (this.mineLevel == 2 || (this.mineLevel % 5 != 0 && this.timeSinceLastMusic > 150000 && Game1.random.NextDouble() < 0.5)))
			{
				this.playMineSong();
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0015A540 File Offset: 0x00158740
		private bool canAdd(int typeOfFeature, int numberSoFar)
		{
			if (this.permanentMineChanges.ContainsKey(this.mineLevel))
			{
				switch (typeOfFeature)
				{
				case 0:
					return this.permanentMineChanges[this.mineLevel].platformContainersLeft > numberSoFar;
				case 1:
					return this.permanentMineChanges[this.mineLevel].chestsLeft > numberSoFar;
				case 2:
					return this.permanentMineChanges[this.mineLevel].coalCartsLeft > numberSoFar;
				case 3:
					return this.permanentMineChanges[this.mineLevel].elevator == 0;
				}
			}
			return true;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0015A5E4 File Offset: 0x001587E4
		public void updateMineLevelData(int feature, int amount = 1)
		{
			if (!this.permanentMineChanges.ContainsKey(this.mineLevel))
			{
				this.permanentMineChanges.Add(this.mineLevel, new MineInfo());
			}
			switch (feature)
			{
			case 0:
				this.permanentMineChanges[this.mineLevel].platformContainersLeft += amount;
				return;
			case 1:
				this.permanentMineChanges[this.mineLevel].chestsLeft += amount;
				return;
			case 2:
				this.permanentMineChanges[this.mineLevel].coalCartsLeft += amount;
				return;
			case 3:
				this.permanentMineChanges[this.mineLevel].elevator += amount;
				return;
			default:
				return;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0015A6AC File Offset: 0x001588AC
		public bool isLevelSlimeArea()
		{
			return this.isSlimeArea;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0015A6B4 File Offset: 0x001588B4
		public void checkForMapAlterations(int x, int y)
		{
			Tile buildingsTile = this.map.GetLayer("Buildings").Tiles[x, y];
			if (buildingsTile != null)
			{
				int tileIndex = buildingsTile.TileIndex;
				if (tileIndex == 194 && !this.canAdd(2, 0))
				{
					base.setMapTileIndex(x, y, 195, "Buildings", 0);
					base.setMapTileIndex(x, y - 1, 179, "Front", 0);
				}
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0015A724 File Offset: 0x00158924
		public void findLadder()
		{
			int found = 0;
			this.tileBeneathElevator = Vector2.Zero;
			bool lookForWater = this.mineLevel % 20 == 0;
			if (lookForWater)
			{
				this.waterTiles = new bool[this.map.Layers[0].LayerWidth, this.map.Layers[0].LayerHeight];
				this.waterColor = ((this.getMineArea(-1) == 80) ? (Color.Red * 0.8f) : (new Color(50, 100, 200) * 0.5f));
			}
			bool foundAnyWater = false;
			this.lightGlows.Clear();
			for (int y = 0; y < this.map.GetLayer("Buildings").LayerHeight; y++)
			{
				for (int x = 0; x < this.map.GetLayer("Buildings").LayerWidth; x++)
				{
					if (this.map.GetLayer("Buildings").Tiles[x, y] != null)
					{
						int currentTileIndex = this.map.GetLayer("Buildings").Tiles[x, y].TileIndex;
						if (currentTileIndex == 115)
						{
							this.tileBeneathLadder = new Vector2((float)x, (float)(y + 1));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)x, (float)(y - 2)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 0.25f, new Color(0, 20, 50), x + y * 999));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)x, (float)(y - 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 0.5f, new Color(0, 20, 50), x + y * 998));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 0.75f, new Color(0, 20, 50), x + y * 997));
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)x, (float)(y + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 1f, new Color(0, 20, 50), x + y * 1000));
							found++;
						}
						else if (currentTileIndex == 112)
						{
							this.tileBeneathElevator = new Vector2((float)x, (float)(y + 1));
							found++;
						}
						if (this.lighting.Equals(Color.White) && found == 2 && !lookForWater)
						{
							return;
						}
						if (!this.lighting.Equals(Color.White) && (currentTileIndex == 97 || currentTileIndex == 113 || currentTileIndex == 65 || currentTileIndex == 66 || currentTileIndex == 81 || currentTileIndex == 82 || currentTileIndex == 48))
						{
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)x, (float)y) * (float)Game1.tileSize, 2.5f, new Color(0, 50, 100), x + y * 1000));
							if (currentTileIndex == 66)
							{
								this.lightGlows.Add(new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2(0f, (float)Game1.tileSize));
							}
							else if (currentTileIndex == 97 || currentTileIndex == 113)
							{
								this.lightGlows.Add(new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
							}
						}
					}
					if (base.doesTileHaveProperty(x, y, "Water", "Back") != null)
					{
						foundAnyWater = true;
						this.waterTiles[x, y] = true;
						if (this.getMineArea(-1) == 80 && Game1.random.NextDouble() < 0.1)
						{
							Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)x, (float)y) * (float)Game1.tileSize, 2f, new Color(0, 220, 220), x + y * 1000));
						}
					}
				}
			}
			if (!foundAnyWater)
			{
				this.waterTiles = null;
			}
			if (this.isFallingDownShaft)
			{
				Vector2 p = default(Vector2);
				while (!this.isTileClearForMineObjects(p))
				{
					p.X = (float)Game1.random.Next(1, this.map.Layers[0].LayerWidth);
					p.Y = (float)Game1.random.Next(1, this.map.Layers[0].LayerHeight);
				}
				this.tileBeneathLadder = p;
				Game1.player.showFrame(5, false);
			}
			this.isFallingDownShaft = false;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0015AC48 File Offset: 0x00158E48
		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (this.mustKillAllMonstersToAdvance() && this.characters.Count == 0)
			{
				Vector2 p = new Vector2((float)((int)this.tileBeneathLadder.X), (float)((int)this.tileBeneathLadder.Y));
				if (base.getTileIndexAt(Utility.Vector2ToPoint(p), "Buildings") == -1)
				{
					this.createLadderAt(p, "newArtifact");
					if (this.mustKillAllMonstersToAdvance())
					{
						Game1.showGlobalMessage("A way down has appeared.");
					}
				}
			}
			while (this.map != null && this.Equals(Game1.currentLocation) && this.mineLevel % 5 != 0 && Game1.random.NextDouble() < 0.1 && !Game1.player.hasBuff(23))
			{
				if (this.mineLevel > 10 && !this.mustKillAllMonstersToAdvance() && Game1.random.NextDouble() < 0.1)
				{
					this.fogTime = 35000 + Game1.random.Next(-5, 6) * 1000;
					Game1.changeMusicTrack("none");
					int mineArea = this.getMineArea(-1);
					if (mineArea <= 10)
					{
						if (mineArea == 0 || mineArea == 10)
						{
							this.fogColor = (this.isDarkArea() ? Color.Khaki : (Color.Green * 0.75f));
						}
					}
					else if (mineArea != 40)
					{
						if (mineArea != 80)
						{
							if (mineArea == 121)
							{
								this.fogColor = Color.BlueViolet * 1f;
							}
						}
						else
						{
							this.fogColor = Color.Red * 0.5f;
						}
					}
					else
					{
						this.fogColor = Color.Blue * 0.75f;
					}
				}
				else
				{
					this.spawnFlyingMonsterOffScreen();
				}
			}
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0015AE10 File Offset: 0x00159010
		public void spawnFlyingMonsterOffScreen()
		{
			Vector2 spawnLocation = Vector2.Zero;
			switch (Game1.random.Next(4))
			{
			case 0:
				spawnLocation.X = (float)Game1.random.Next(this.map.Layers[0].LayerWidth);
				break;
			case 1:
				spawnLocation.X = (float)(this.map.Layers[0].LayerWidth - 1);
				spawnLocation.Y = (float)Game1.random.Next(this.map.Layers[0].LayerHeight);
				break;
			case 2:
				spawnLocation.Y = (float)(this.map.Layers[0].LayerHeight - 1);
				spawnLocation.X = (float)Game1.random.Next(this.map.Layers[0].LayerWidth);
				break;
			case 3:
				spawnLocation.Y = (float)Game1.random.Next(this.map.Layers[0].LayerHeight);
				break;
			}
			if (Utility.isOnScreen(spawnLocation * (float)Game1.tileSize, Game1.tileSize))
			{
				spawnLocation.X -= (float)(Game1.viewport.Width / Game1.tileSize);
			}
			int mineArea = this.getMineArea(-1);
			if (mineArea <= 10)
			{
				if (mineArea != 0)
				{
					if (mineArea != 10)
					{
						return;
					}
					this.characters.Add(new Fly(spawnLocation * (float)Game1.tileSize, false)
					{
						focusedOnFarmers = true
					});
					return;
				}
				else if (this.mineLevel > 10 && this.isDarkArea())
				{
					this.characters.Add(new Bat(spawnLocation * (float)Game1.tileSize, this.mineLevel)
					{
						focusedOnFarmers = true
					});
					Game1.playSound("batScreech");
					return;
				}
			}
			else
			{
				if (mineArea == 40)
				{
					this.characters.Add(new Bat(spawnLocation * (float)Game1.tileSize, this.mineLevel)
					{
						focusedOnFarmers = true
					});
					Game1.playSound("batScreech");
					return;
				}
				if (mineArea == 80)
				{
					this.characters.Add(new Bat(spawnLocation * (float)Game1.tileSize, this.mineLevel)
					{
						focusedOnFarmers = true
					});
					Game1.playSound("batScreech");
					return;
				}
				if (mineArea != 121)
				{
					return;
				}
				this.characters.Add(new Serpent(spawnLocation * (float)Game1.tileSize)
				{
					focusedOnFarmers = true
				});
				Game1.playSound("serpentDie");
			}
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0015B0A0 File Offset: 0x001592A0
		public override void drawLightGlows(SpriteBatch b)
		{
			int mineArea = this.getMineArea(-1);
			Color c;
			if (mineArea <= 40)
			{
				if (mineArea == 0)
				{
					c = (this.isDarkArea() ? (Color.PaleGoldenrod * 0.5f) : (Color.PaleGoldenrod * 0.33f));
					goto IL_B0;
				}
				if (mineArea == 40)
				{
					c = Color.White * 0.65f;
					goto IL_B0;
				}
			}
			else
			{
				if (mineArea == 80)
				{
					c = (this.isDarkArea() ? (Color.Pink * 0.4f) : (Color.Red * 0.33f));
					goto IL_B0;
				}
				if (mineArea == 121)
				{
					c = Color.White * 0.8f;
					goto IL_B0;
				}
			}
			c = Color.PaleGoldenrod * 0.33f;
			IL_B0:
			foreach (Vector2 v in this.lightGlows)
			{
				if (this.rainbowLights)
				{
					switch ((int)(v.X / (float)Game1.tileSize + v.Y / (float)Game1.tileSize) % 4)
					{
					case 0:
						c = Color.Red * 0.5f;
						break;
					case 1:
						c = Color.Yellow * 0.5f;
						break;
					case 2:
						c = Color.Cyan * 0.33f;
						break;
					case 3:
						c = Color.Lime * 0.45f;
						break;
					}
				}
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, v), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 30, 30)), c, 0f, new Vector2(15f, 15f), (float)(Game1.pixelZoom * 2) + (float)((double)(Game1.tileSize * 3 / 2) * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(v.X * 777f) + (double)(v.Y * 9746f)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0015B2E0 File Offset: 0x001594E0
		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			int fish = -1;
			double chanceMultiplier = 1.0;
			chanceMultiplier += 0.4 * (double)who.FishingLevel;
			chanceMultiplier += (double)waterDepth * 0.1;
			int mineArea = this.getMineArea(-1);
			if (mineArea <= 10)
			{
				if (mineArea == 0 || mineArea == 10)
				{
					chanceMultiplier += (double)((bait == 689) ? 3 : 0);
					if (Game1.random.NextDouble() < 0.02 + 0.01 * chanceMultiplier)
					{
						fish = 158;
					}
				}
			}
			else if (mineArea != 40)
			{
				if (mineArea == 80)
				{
					chanceMultiplier += (double)((bait == 684) ? 3 : 0);
					if (Game1.random.NextDouble() < 0.01 + 0.008 * chanceMultiplier)
					{
						fish = 162;
					}
				}
			}
			else
			{
				chanceMultiplier += (double)((bait == 682) ? 3 : 0);
				if (Game1.random.NextDouble() < 0.015 + 0.009 * chanceMultiplier)
				{
					fish = 161;
				}
			}
			int quality = 0;
			if (Game1.random.NextDouble() < (double)((float)who.FishingLevel / 10f))
			{
				quality = 1;
			}
			if (Game1.random.NextDouble() < (double)((float)who.FishingLevel / 50f + (float)who.LuckLevel / 100f))
			{
				quality = 2;
			}
			if (fish != -1)
			{
				return new Object(fish, 1, false, -1, quality);
			}
			if (this.getMineArea(-1) == 80)
			{
				return new Object(Game1.random.Next(167, 173), 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0015B480 File Offset: 0x00159680
		private void adjustLevelChances(ref double stoneChance, ref double monsterChance, ref double itemChance, ref double gemStoneChance)
		{
			if (this.mineLevel == 1)
			{
				monsterChance = 0.0;
				itemChance = 0.0;
				gemStoneChance = 0.0;
			}
			else if (this.mineLevel % 5 == 0 && this.getMineArea(-1) != 121)
			{
				itemChance = 0.0;
				gemStoneChance = 0.0;
				if (this.mineLevel % 10 == 0)
				{
					monsterChance = 0.0;
				}
			}
			if (this.mustKillAllMonstersToAdvance())
			{
				monsterChance = 0.025;
				itemChance = 0.001;
				stoneChance = 0.0;
				gemStoneChance = 0.0;
			}
			if (Game1.player.hasBuff(23) && this.getMineArea(-1) != 121)
			{
				monsterChance = 0.0;
			}
			gemStoneChance /= 2.0;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0015B568 File Offset: 0x00159768
		private void populateLevel()
		{
			this.objects.Clear();
			this.terrainFeatures.Clear();
			this.resourceClumps.Clear();
			this.debris.Clear();
			this.characters.Clear();
			this.ghostAdded = false;
			this.stonesLeftOnThisLevel = 0;
			double stoneChance = (double)this.mineRandom.Next(10, 30) / 100.0;
			double monsterChance = 0.002 + (double)this.mineRandom.Next(200) / 10000.0;
			double itemChance = 0.0025;
			double gemStoneChance = 0.003;
			this.adjustLevelChances(ref stoneChance, ref monsterChance, ref itemChance, ref gemStoneChance);
			int barrelsAdded = 0;
			bool firstTime = !this.permanentMineChanges.ContainsKey(this.mineLevel);
			if (this.mineLevel > 1 && this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.5 && !this.mustKillAllMonstersToAdvance())
			{
				int numBarrels = this.mineRandom.Next(5) + (int)(Game1.dailyLuck * 20.0);
				for (int i = 0; i < numBarrels; i++)
				{
					Point p;
					Point motion;
					if (this.mineRandom.NextDouble() < 0.33)
					{
						p = new Point(this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), 0);
						motion = new Point(0, 1);
					}
					else if (this.mineRandom.NextDouble() < 0.5)
					{
						p = new Point(0, this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
						motion = new Point(1, 0);
					}
					else
					{
						p = new Point(this.map.GetLayer("Back").LayerWidth - 1, this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
						motion = new Point(-1, 0);
					}
					while (base.isTileOnMap(p.X, p.Y))
					{
						p.X += motion.X;
						p.Y += motion.Y;
						if (this.isTileClearForMineObjects(p.X, p.Y))
						{
							Vector2 objectPos = new Vector2((float)p.X, (float)p.Y);
							this.objects.Add(objectPos, new BreakableContainer(objectPos, 118));
							break;
						}
					}
				}
			}
			this.addLevelUnique(firstTime);
			if (this.mineLevel % 10 != 0 || this.getMineArea(-1) == 121)
			{
				for (int j = 0; j < this.map.GetLayer("Back").LayerWidth; j++)
				{
					for (int k = 0; k < this.map.GetLayer("Back").LayerHeight; k++)
					{
						this.checkForMapAlterations(j, k);
						if (this.isTileClearForMineObjects(j, k))
						{
							if (this.mineRandom.NextDouble() <= stoneChance)
							{
								Vector2 objectPos2 = new Vector2((float)j, (float)k);
								if (!base.Objects.ContainsKey(objectPos2))
								{
									if (this.getMineArea(-1) == 40 && this.mineRandom.NextDouble() < 0.15)
									{
										base.Objects.Add(objectPos2, new Object(objectPos2, this.mineRandom.Next(319, 322), "Weeds", true, false, false, false)
										{
											fragility = 2,
											canBeGrabbed = true
										});
									}
									else if (this.rainbowLights && this.mineRandom.NextDouble() < 0.55)
									{
										if (this.mineRandom.NextDouble() < 0.25)
										{
											int which = 404;
											switch (this.mineRandom.Next(5))
											{
											case 0:
												which = 422;
												break;
											case 1:
												which = 420;
												break;
											case 2:
												which = 420;
												break;
											case 3:
												which = 420;
												break;
											case 4:
												which = 420;
												break;
											}
											base.Objects.Add(objectPos2, new Object(which, 1, false, -1, 0)
											{
												isSpawnedObject = true
											});
										}
									}
									else
									{
										base.Objects.Add(objectPos2, this.chooseStoneType(0.001, 5E-05, gemStoneChance, objectPos2));
										this.stonesLeftOnThisLevel++;
									}
								}
							}
							else if (this.mineRandom.NextDouble() <= monsterChance && Utility.distance(this.tileBeneathLadder.X, (float)j, this.tileBeneathLadder.Y, (float)k) > 5f)
							{
								Monster monsterToAdd = this.getMonsterForThisLevel(this.mineLevel, j, k);
								if (monsterToAdd is Grub)
								{
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j - 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j + 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j, k - 1);
									}
									if (this.mineRandom.NextDouble() < 0.4)
									{
										this.tryToAddMonster(new Grub(Vector2.Zero, false), j, k + 1);
									}
								}
								else if (monsterToAdd is DustSpirit)
								{
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j - 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j + 1, k);
									}
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j, k - 1);
									}
									if (this.mineRandom.NextDouble() < 0.6)
									{
										this.tryToAddMonster(new DustSpirit(Vector2.Zero), j, k + 1);
									}
								}
								if (this.mineRandom.NextDouble() < 0.00175)
								{
									monsterToAdd.hasSpecialItem = true;
								}
								if (monsterToAdd.GetBoundingBox().Width <= Game1.tileSize || this.isTileClearForMineObjects(j + 1, k))
								{
									this.characters.Add(monsterToAdd);
								}
							}
							else if (this.mineRandom.NextDouble() <= itemChance)
							{
								Vector2 objectPos3 = new Vector2((float)j, (float)k);
								base.Objects.Add(objectPos3, this.getRandomItemForThisLevel(this.mineLevel));
							}
							else if (this.mineRandom.NextDouble() <= 0.005 && !this.isDarkArea() && !this.mustKillAllMonstersToAdvance() && this.isTileClearForMineObjects(j + 1, k) && this.isTileClearForMineObjects(j, k + 1) && this.isTileClearForMineObjects(j + 1, k + 1))
							{
								Vector2 objectPos4 = new Vector2((float)j, (float)k);
								int whichClump = (this.mineRandom.NextDouble() < 0.5) ? 752 : 754;
								int mineArea = this.getMineArea(-1);
								if (mineArea == 40)
								{
									whichClump = ((this.mineRandom.NextDouble() < 0.5) ? 756 : 758);
								}
								this.resourceClumps.Add(new ResourceClump(whichClump, 2, 2, objectPos4));
							}
						}
						else if (this.isContainerPlatform(j, k) && base.isTileLocationTotallyClearAndPlaceable(j, k) && this.mineRandom.NextDouble() < 0.4 && (firstTime || this.canAdd(0, barrelsAdded)))
						{
							Vector2 objectPos5 = new Vector2((float)j, (float)k);
							this.objects.Add(objectPos5, new BreakableContainer(objectPos5, 118));
							barrelsAdded++;
							if (firstTime)
							{
								this.updateMineLevelData(0, 1);
							}
						}
						else if (this.mineRandom.NextDouble() <= monsterChance && base.isTileLocationTotallyClearAndPlaceable(j, k) && this.isTileOnClearAndSolidGround(j, k) && (!Game1.player.hasBuff(23) || this.getMineArea(-1) == 121))
						{
							Monster monsterToAdd2 = this.getMonsterForThisLevel(this.mineLevel, j, k);
							if (this.mineRandom.NextDouble() < 0.01)
							{
								monsterToAdd2.hasSpecialItem = true;
							}
							this.characters.Add(monsterToAdd2);
						}
					}
				}
				if (this.stonesLeftOnThisLevel > 35)
				{
					int tries = this.stonesLeftOnThisLevel / 35;
					for (int l = 0; l < tries; l++)
					{
						Vector2 stone = this.objects.Keys.ElementAt(this.mineRandom.Next(this.objects.Count));
						if (this.objects[stone].name.Equals("Stone"))
						{
							int radius = this.mineRandom.Next(3, 8);
							bool monsterSpot = this.mineRandom.NextDouble() < 0.1;
							int x = (int)stone.X - radius / 2;
							while ((float)x < stone.X + (float)(radius / 2))
							{
								int y = (int)stone.Y - radius / 2;
								while ((float)y < stone.Y + (float)(radius / 2))
								{
									Vector2 tile = new Vector2((float)x, (float)y);
									if (this.objects.ContainsKey(tile) && this.objects[tile].name.Equals("Stone"))
									{
										this.objects.Remove(tile);
										this.stonesLeftOnThisLevel--;
										if (monsterSpot && this.mineRandom.NextDouble() < 0.12)
										{
											this.characters.Add(this.getMonsterForThisLevel(this.mineLevel, x, y));
										}
									}
									y++;
								}
								x++;
							}
						}
					}
				}
				this.tryToAddAreaUniques();
				if (this.mineRandom.NextDouble() < 0.95 && !this.mustKillAllMonstersToAdvance() && this.mineLevel > 1 && this.mineLevel % 5 != 0)
				{
					Vector2 possibleSpot = new Vector2((float)this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), (float)this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
					if (this.isTileClearForMineObjects(possibleSpot))
					{
						this.createLadderDown((int)possibleSpot.X, (int)possibleSpot.Y);
					}
				}
				if (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1)
				{
					this.characters.Add(new Bat(this.tileBeneathLadder * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 4), (float)(Game1.tileSize * 4))));
				}
			}
			if (!this.mustKillAllMonstersToAdvance() && this.mineLevel % 5 != 0 && this.mineLevel > 2)
			{
				this.tryToAddOreClumps();
				if (this.isLightingDark)
				{
					this.tryToAddOldMinerPath();
				}
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0015C106 File Offset: 0x0015A306
		public void placeAppropriateOreAt(Vector2 tile)
		{
			if (this.isTileLocationTotallyClearAndPlaceable(tile))
			{
				this.objects.Add(tile, this.getAppropriateOre(tile));
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0015C124 File Offset: 0x0015A324
		public Object getAppropriateOre(Vector2 tile)
		{
			Object ore = new Object(tile, 751, "Stone", true, false, false, false)
			{
				minutesUntilReady = 3
			};
			int mineArea = this.getMineArea(-1);
			if (mineArea != 40)
			{
				if (mineArea != 80)
				{
					if (mineArea == 121)
					{
						ore = new Object(tile, 764, "Stone", true, false, false, false)
						{
							minutesUntilReady = 8
						};
						if (this.mineRandom.NextDouble() < 0.02)
						{
							return new Object(tile, 765, "Stone", true, false, false, false)
							{
								minutesUntilReady = 16
							};
						}
					}
				}
				else if (this.mineRandom.NextDouble() < 0.8)
				{
					ore = new Object(tile, 764, "Stone", true, false, false, false)
					{
						minutesUntilReady = 8
					};
				}
			}
			else if (this.mineRandom.NextDouble() < 0.8)
			{
				ore = new Object(tile, 290, "Stone", true, false, false, false)
				{
					minutesUntilReady = 4
				};
			}
			if (this.mineRandom.NextDouble() < 0.25 && this.getMineArea(-1) != 40)
			{
				ore = new Object(tile, (this.mineRandom.NextDouble() < 0.5) ? 668 : 670, "Stone", true, false, false, false)
				{
					minutesUntilReady = 2
				};
			}
			return ore;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0015C284 File Offset: 0x0015A484
		public void tryToAddOreClumps()
		{
			if (this.mineRandom.NextDouble() < 0.55 + Game1.dailyLuck)
			{
				Vector2 endPoint = base.getRandomTile();
				int tries = 0;
				while (tries < 1 || this.mineRandom.NextDouble() < 0.25 + Game1.dailyLuck)
				{
					if (this.isTileLocationTotallyClearAndPlaceable(endPoint) && this.isTileOnClearAndSolidGround(endPoint) && base.doesTileHaveProperty((int)endPoint.X, (int)endPoint.Y, "Diggable", "Back") == null)
					{
						Object ore = this.getAppropriateOre(endPoint);
						if (ore.parentSheetIndex == 670)
						{
							ore.parentSheetIndex = 668;
						}
						Utility.recursiveObjectPlacement(ore, (int)endPoint.X, (int)endPoint.Y, 0.949999988079071, 0.30000001192092896, this, "Dirt", (ore.parentSheetIndex == 668) ? 1 : 0, 0.05000000074505806, (ore.parentSheetIndex == 668) ? 2 : 1);
					}
					endPoint = base.getRandomTile();
					tries++;
				}
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0015C3A0 File Offset: 0x0015A5A0
		public void tryToAddOldMinerPath()
		{
			Vector2 endPoint = base.getRandomTile();
			int tries = 0;
			while (!this.isTileOnClearAndSolidGround(endPoint) && tries < 8)
			{
				endPoint = base.getRandomTile();
				tries++;
			}
			if (this.isTileOnClearAndSolidGround(endPoint))
			{
				Stack<Point> path = PathFindController.findPath(Utility.Vector2ToPoint(this.tileBeneathLadder), Utility.Vector2ToPoint(endPoint), new PathFindController.isAtEnd(PathFindController.isAtEndPoint), this, Game1.player, 500);
				if (path != null)
				{
					while (path.Count > 0)
					{
						Point p = path.Pop();
						this.removeEverythingExceptCharactersFromThisTile(p.X, p.Y);
						if (path.Count > 0 && this.mineRandom.NextDouble() < 0.2)
						{
							Vector2 torchPosition = Vector2.Zero;
							if (path.Peek().X == p.X)
							{
								torchPosition = new Vector2((float)(p.X + ((this.mineRandom.NextDouble() < 0.5) ? -1 : 1)), (float)p.Y);
							}
							else
							{
								torchPosition = new Vector2((float)p.X, (float)(p.Y + ((this.mineRandom.NextDouble() < 0.5) ? -1 : 1)));
							}
							if (!torchPosition.Equals(Vector2.Zero) && this.isTileLocationTotallyClearAndPlaceable(torchPosition) && this.isTileOnClearAndSolidGround(torchPosition))
							{
								if (this.mineRandom.NextDouble() < 0.5)
								{
									new Torch(torchPosition, 1).placementAction(this, (int)torchPosition.X * Game1.tileSize, (int)torchPosition.Y * Game1.tileSize, null);
								}
								else
								{
									this.placeAppropriateOreAt(torchPosition);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0015C54C File Offset: 0x0015A74C
		public void tryToAddAreaUniques()
		{
			if ((this.getMineArea(-1) == 10 || this.getMineArea(-1) == 80 || (this.getMineArea(-1) == 40 && this.mineRandom.NextDouble() < 0.1)) && !this.isDarkArea() && !this.mustKillAllMonstersToAdvance())
			{
				int tries = this.mineRandom.Next(7, 24);
				int baseWeedIndex = (this.getMineArea(-1) == 80) ? 316 : ((this.getMineArea(-1) == 40) ? 319 : 313);
				for (int i = 0; i < tries; i++)
				{
					Vector2 tile = new Vector2((float)this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), (float)this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
					Utility.recursiveObjectPlacement(new Object(tile, baseWeedIndex, "Weeds", true, false, false, false)
					{
						fragility = 2,
						canBeGrabbed = true
					}, (int)tile.X, (int)tile.Y, 1.0, (double)((float)this.mineRandom.Next(10, 40) / 100f), this, "Dirt", 2, 0.29, 1);
				}
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0015C6A2 File Offset: 0x0015A8A2
		public void tryToAddMonster(Monster m, int tileX, int tileY)
		{
			if (this.isTileClearForMineObjects(tileX, tileY) && !this.isTileOccupied(new Vector2((float)tileX, (float)tileY), ""))
			{
				m.setTilePosition(tileX, tileY);
				this.characters.Add(m);
			}
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0015C6D8 File Offset: 0x0015A8D8
		public bool isContainerPlatform(int x, int y)
		{
			return this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Back").Tiles[x, y].TileIndex == 257;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0015C72E File Offset: 0x0015A92E
		public bool mustKillAllMonstersToAdvance()
		{
			return this.isSlimeArea || this.isMonsterArea;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0015C740 File Offset: 0x0015A940
		public void createLadderAt(Vector2 p, string sound = "hoeHit")
		{
			base.setMapTileIndex((int)p.X, (int)p.Y, 173, "Buildings", 0);
			Game1.playSound(sound);
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize, Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				interval = 80f
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize - new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 150,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 300,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize - new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 450,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float)Game1.tileSize - new Vector2((float)(-(float)Game1.tileSize / 4), (float)(Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 600,
				interval = 80f,
				scale = 0.75f,
				startSound = "sandyStep"
			});
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle((int)p.X * Game1.tileSize, (int)p.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0015C9F8 File Offset: 0x0015ABF8
		public bool recursiveTryToCreateLadderDown(Vector2 centerTile, string sound = "hoeHit", int maxIterations = 16)
		{
			List<Vector2> directions = Utility.getDirectionsTileVectors();
			int iterations = 0;
			Queue<Vector2> positionsToCheck = new Queue<Vector2>();
			positionsToCheck.Enqueue(centerTile);
			List<Vector2> closedList = new List<Vector2>();
			while (iterations < maxIterations && positionsToCheck.Count > 0)
			{
				Vector2 currentPoint = positionsToCheck.Dequeue();
				closedList.Add(currentPoint);
				if (!this.isTileOccupied(currentPoint, "ignoreMe") && this.isTileOnClearAndSolidGround(currentPoint) && base.isTileOccupiedByFarmer(currentPoint) == null && base.doesTileHaveProperty((int)currentPoint.X, (int)currentPoint.Y, "Type", "Back") != null && base.doesTileHaveProperty((int)currentPoint.X, (int)currentPoint.Y, "Type", "Back").Equals("Stone"))
				{
					this.createLadderAt(currentPoint, "hoeHit");
					return true;
				}
				foreach (Vector2 v in directions)
				{
					if (!closedList.Contains(currentPoint + v))
					{
						positionsToCheck.Enqueue(currentPoint + v);
					}
				}
				iterations++;
			}
			return false;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0015CB28 File Offset: 0x0015AD28
		public override void monsterDrop(Monster monster, int x, int y)
		{
			if (monster.hasSpecialItem)
			{
				Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(this.mineLevel, x / Game1.tileSize, y / Game1.tileSize), monster.position, Game1.random.Next(4), null);
			}
			else
			{
				base.monsterDrop(monster, x, y);
			}
			if ((!this.mustKillAllMonstersToAdvance() && Game1.random.NextDouble() < 0.15) || (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1))
			{
				Vector2 p = new Vector2((float)x, (float)y) / (float)Game1.tileSize;
				p.X = (float)((int)p.X);
				p.Y = (float)((int)p.Y);
				monster.name = "ignoreMe";
				Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle((int)p.X * Game1.tileSize, (int)p.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
				if (!this.isTileOccupied(p, "ignoreMe") && this.isTileOnClearAndSolidGround(p) && !Game1.player.GetBoundingBox().Intersects(tileRect) && base.doesTileHaveProperty((int)p.X, (int)p.Y, "Type", "Back") != null && base.doesTileHaveProperty((int)p.X, (int)p.Y, "Type", "Back").Equals("Stone"))
				{
					this.createLadderAt(p, "hoeHit");
					return;
				}
				if (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1)
				{
					p = new Vector2((float)((int)this.tileBeneathLadder.X), (float)((int)this.tileBeneathLadder.Y));
					this.createLadderAt(p, "newArtifact");
					if (this.mustKillAllMonstersToAdvance())
					{
						Game1.showGlobalMessage("A way down has appeared.");
					}
				}
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0015CCF4 File Offset: 0x0015AEF4
		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			for (int i = this.resourceClumps.Count - 1; i >= 0; i--)
			{
				if (this.resourceClumps[i] != null && this.resourceClumps[i].getBoundingBox(this.resourceClumps[i].tile).Contains(tileX * Game1.tileSize, tileY * Game1.tileSize))
				{
					if (this.resourceClumps[i].performToolAction(t, 1, this.resourceClumps[i].tile, null))
					{
						this.resourceClumps.RemoveAt(i);
					}
					return true;
				}
			}
			return base.performToolAction(t, tileX, tileY);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0015CDA8 File Offset: 0x0015AFA8
		private void addLevelUnique(bool firstTime)
		{
			List<Item> chestItem = new List<Item>();
			Vector2 chestSpot = new Vector2(9f, 9f);
			Color tint = Color.White;
			if (this.mineLevel % 20 == 0 && this.mineLevel % 40 != 0)
			{
				chestSpot.Y += 4f;
			}
			int num = this.mineLevel;
			if (num <= 60)
			{
				if (num <= 20)
				{
					if (num != 5)
					{
						if (num != 10)
						{
							if (num == 20)
							{
								chestItem.Add(new MeleeWeapon(11));
							}
						}
						else
						{
							chestItem.Add(new Boots(506));
						}
					}
					else
					{
						Game1.player.completeQuest(14);
						if (!Game1.player.hasOrWillReceiveMail("guildQuest"))
						{
							Game1.addMailForTomorrow("guildQuest", false, false);
						}
					}
				}
				else if (num != 40)
				{
					if (num != 50)
					{
						if (num == 60)
						{
							chestItem.Add(new MeleeWeapon(21));
						}
					}
					else
					{
						chestItem.Add(new Boots(509));
					}
				}
				else
				{
					Game1.player.completeQuest(17);
					chestItem.Add(new Slingshot());
				}
			}
			else if (num <= 90)
			{
				if (num != 70)
				{
					if (num != 80)
					{
						if (num == 90)
						{
							chestItem.Add(new MeleeWeapon(8));
						}
					}
					else
					{
						chestItem.Add(new Boots(512));
					}
				}
				else
				{
					chestItem.Add(new Slingshot(33));
				}
			}
			else if (num != 100)
			{
				if (num != 110)
				{
					if (num == 120)
					{
						Game1.player.completeQuest(18);
						Game1.getSteamAchievement("Achievement_TheBottom");
						if (!Game1.player.hasSkullKey)
						{
							chestItem.Add(new SpecialItem(1, 4, ""));
						}
						tint = Color.Pink;
					}
				}
				else
				{
					chestItem.Add(new Boots(514));
				}
			}
			else
			{
				chestItem.Add(new Object(434, 1, false, -1, 0));
			}
			if (chestItem.Count > 0 && this.canAdd(1, 0))
			{
				this.objects.Add(chestSpot, new Chest(0, chestItem, chestSpot, false)
				{
					tint = tint
				});
				if (firstTime)
				{
					this.updateMineLevelData(1, 1);
				}
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0015CFE8 File Offset: 0x0015B1E8
		public static Item getSpecialItemForThisMineLevel(int level, int x, int y)
		{
			Random r = new Random(level + (int)Game1.stats.DaysPlayed + x + y * 10000);
			if (level < 20)
			{
				switch (r.Next(6))
				{
				case 0:
					return new MeleeWeapon(16);
				case 1:
					return new MeleeWeapon(24);
				case 2:
					return new Boots(504);
				case 3:
					return new Boots(505);
				case 4:
					return new Ring(516);
				case 5:
					return new Ring(518);
				}
			}
			else if (level < 40)
			{
				switch (r.Next(7))
				{
				case 0:
					return new MeleeWeapon(22);
				case 1:
					return new MeleeWeapon(24);
				case 2:
					return new Boots(504);
				case 3:
					return new Boots(505);
				case 4:
					return new Ring(516);
				case 5:
					return new Ring(518);
				case 6:
					return new MeleeWeapon(15);
				}
			}
			else if (level < 60)
			{
				switch (r.Next(7))
				{
				case 0:
					return new MeleeWeapon(6);
				case 1:
					return new MeleeWeapon(26);
				case 2:
					return new MeleeWeapon(15);
				case 3:
					return new Boots(510);
				case 4:
					return new Ring(517);
				case 5:
					return new Ring(519);
				case 6:
					return new MeleeWeapon(27);
				}
			}
			else if (level < 160)
			{
				switch (r.Next(7))
				{
				case 0:
					return new MeleeWeapon(26);
				case 1:
					return new MeleeWeapon(26);
				case 2:
					return new Boots(508);
				case 3:
					return new Boots(510);
				case 4:
					return new Ring(517);
				case 5:
					return new Ring(519);
				case 6:
					return new MeleeWeapon(26);
				}
			}
			else if (level < 100)
			{
				switch (r.Next(7))
				{
				case 0:
					return new MeleeWeapon(48);
				case 1:
					return new MeleeWeapon(48);
				case 2:
					return new Boots(511);
				case 3:
					return new Boots(513);
				case 4:
					return new MeleeWeapon(18);
				case 5:
					return new MeleeWeapon(28);
				case 6:
					return new MeleeWeapon(52);
				}
			}
			else if (level < 120)
			{
				switch (r.Next(6))
				{
				case 0:
					return new MeleeWeapon(19);
				case 1:
					return new MeleeWeapon(50);
				case 2:
					return new Boots(511);
				case 3:
					return new Boots(513);
				case 4:
					return new MeleeWeapon(18);
				case 5:
					return new MeleeWeapon(46);
				}
			}
			else
			{
				switch (r.Next(8))
				{
				case 0:
					return new MeleeWeapon(45);
				case 1:
					return new MeleeWeapon(50);
				case 2:
					return new Boots(511);
				case 3:
					return new Boots(513);
				case 4:
					return new MeleeWeapon(18);
				case 5:
					return new MeleeWeapon(28);
				case 6:
					return new MeleeWeapon(52);
				case 7:
					return new Object(787, 1, false, -1, 0);
				}
			}
			return new Object(78, 1, false, -1, 0);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0015D344 File Offset: 0x0015B544
		public override bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
		{
			using (List<ResourceClump>.Enumerator enumerator = this.resourceClumps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.occupiesTile((int)tileLocation.X, (int)tileLocation.Y))
					{
						return true;
					}
				}
			}
			return this.tileBeneathLadder.Equals(tileLocation) || base.isTileOccupied(tileLocation, characterToIgnore);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0015D3C4 File Offset: 0x0015B5C4
		public bool isDarkArea()
		{
			return (this.loadedDarkArea || this.mineLevel % 40 > 30) && this.getMineArea(-1) != 40;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0015D3EC File Offset: 0x0015B5EC
		public bool isTileClearForMineObjects(Vector2 v)
		{
			if (this.tileBeneathLadder.Equals(v) || this.tileBeneathElevator.Equals(v))
			{
				return false;
			}
			if (!this.isTileLocationTotallyClearAndPlaceable(v))
			{
				return false;
			}
			string s = base.doesTileHaveProperty((int)v.X, (int)v.Y, "Type", "Back");
			return s != null && s.Equals("Stone") && this.isTileOnClearAndSolidGround(v) && !this.objects.ContainsKey(v);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0015D470 File Offset: 0x0015B670
		public bool isTileOnClearAndSolidGround(Vector2 v)
		{
			return this.map.GetLayer("Back").Tiles[(int)v.X, (int)v.Y] != null && this.map.GetLayer("Front").Tiles[(int)v.X, (int)v.Y] == null && this.map.GetLayer("Buildings").Tiles[(int)v.X, (int)v.Y] == null && base.getTileIndexAt((int)v.X, (int)v.Y, "Back") != 77;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0015D520 File Offset: 0x0015B720
		public bool isTileOnClearAndSolidGround(int x, int y)
		{
			return this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Front").Tiles[x, y] == null && base.getTileIndexAt(x, y, "Back") != 77;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0015D581 File Offset: 0x0015B781
		public bool isTileClearForMineObjects(int x, int y)
		{
			return this.isTileClearForMineObjects(new Vector2((float)x, (float)y));
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0015D594 File Offset: 0x0015B794
		public void loadLevel(int level)
		{
			this.isMonsterArea = false;
			this.isSlimeArea = false;
			this.loadedDarkArea = false;
			this.mineLoader.Unload();
			this.mineLoader.Dispose();
			this.mineLoader = Game1.content.CreateTemporary();
			int mapNumberToLoad = (level % 40 % 20 == 0 && level % 40 != 0) ? 20 : ((level % 10 == 0) ? 10 : level);
			mapNumberToLoad %= 40;
			if (level == 120)
			{
				mapNumberToLoad = 120;
			}
			if (this.getMineArea(level) == 121)
			{
				mapNumberToLoad = this.mineRandom.Next(40);
				while (mapNumberToLoad % 5 == 0)
				{
					mapNumberToLoad = this.mineRandom.Next(40);
				}
			}
			this.map = this.mineLoader.Load<Map>("Maps\\Mines\\" + mapNumberToLoad);
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)level + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			if ((!Game1.player.hasBuff(23) || this.getMineArea(-1) == 121) && r.NextDouble() < 0.05 && mapNumberToLoad % 5 != 0 && mapNumberToLoad % 40 > 5 && mapNumberToLoad % 40 < 30 && mapNumberToLoad % 40 != 19)
			{
				if (r.NextDouble() < 0.5)
				{
					this.isMonsterArea = true;
				}
				else
				{
					this.isSlimeArea = true;
				}
				Game1.showGlobalMessage(Game1.content.LoadString((r.NextDouble() < 0.5) ? "Strings\\Locations:Mines_Infested" : "Strings\\Locations:Mines_Overrun", new object[0]));
			}
			if (this.getMineArea(this.nextLevel) != this.getMineArea(this.mineLevel) || this.mineLevel == 120)
			{
				Game1.changeMusicTrack("none");
			}
			if (this.isSlimeArea)
			{
				this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_slime";
				this.map.LoadTileSheets(Game1.mapDisplayDevice);
			}
			else if (this.getMineArea(-1) == 0 || this.getMineArea(-1) == 10 || (this.getMineArea(this.nextLevel) != 0 && this.getMineArea(this.nextLevel) != 10))
			{
				if (this.getMineArea(this.nextLevel) == 40)
				{
					this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_frost";
					if (this.nextLevel >= 70)
					{
						TileSheet expr_251 = this.map.TileSheets[0];
						expr_251.ImageSource += "_dark";
						this.loadedDarkArea = true;
					}
					this.map.LoadTileSheets(Game1.mapDisplayDevice);
				}
				else if (this.getMineArea(this.nextLevel) == 80)
				{
					this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_lava";
					if (this.nextLevel >= 110 && this.nextLevel != 120)
					{
						TileSheet expr_2D2 = this.map.TileSheets[0];
						expr_2D2.ImageSource += "_dark";
						this.loadedDarkArea = true;
					}
					this.map.LoadTileSheets(Game1.mapDisplayDevice);
				}
				else if (this.getMineArea(this.nextLevel) == 121)
				{
					this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_desert";
					if (mapNumberToLoad % 40 >= 30)
					{
						TileSheet expr_34A = this.map.TileSheets[0];
						expr_34A.ImageSource += "_dark";
						this.loadedDarkArea = true;
					}
					this.map.LoadTileSheets(Game1.mapDisplayDevice);
					if (this.nextLevel >= 145 && Game1.player.hasQuest(20) && !Game1.player.hasOrWillReceiveMail("QiChallengeComplete"))
					{
						Game1.player.completeQuest(20);
						Game1.addMailForTomorrow("QiChallengeComplete", false, false);
					}
				}
			}
			if (!this.map.TileSheets[0].TileIndexProperties[165].ContainsKey("Diggable"))
			{
				this.map.TileSheets[0].TileIndexProperties[165].Add("Diggable", new PropertyValue("true"));
			}
			if (!this.map.TileSheets[0].TileIndexProperties[181].ContainsKey("Diggable"))
			{
				this.map.TileSheets[0].TileIndexProperties[181].Add("Diggable", new PropertyValue("true"));
			}
			if (!this.map.TileSheets[0].TileIndexProperties[183].ContainsKey("Diggable"))
			{
				this.map.TileSheets[0].TileIndexProperties[183].Add("Diggable", new PropertyValue("true"));
			}
			this.mineLevel = this.nextLevel;
			if (this.nextLevel > this.lowestLevelReached)
			{
				this.lowestLevelReached = this.nextLevel;
				Game1.player.deepestMineLevel = this.nextLevel;
			}
			if (this.mineLevel % 5 == 0 && this.getMineArea(-1) != 121)
			{
				this.prepareElevator();
			}
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0015DACC File Offset: 0x0015BCCC
		private void prepareElevator()
		{
			Point elevatorSpot = Utility.findTile(this, 80, "Buildings");
			this.ElevatorLightSpot = elevatorSpot;
			if (elevatorSpot.X >= 0)
			{
				if (this.canAdd(3, 0))
				{
					this.timeUntilElevatorLightUp = 1500;
					this.updateMineLevelData(3, 1);
					return;
				}
				base.setMapTileIndex(elevatorSpot.X, elevatorSpot.Y, 48, "Buildings", 0);
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0015DB30 File Offset: 0x0015BD30
		public void enterMineShaft()
		{
			DelayedAction.playSoundAfterDelay("fallDown", 1200);
			DelayedAction.playSoundAfterDelay("clubSmash", 2200);
			int levelsDown = this.mineRandom.Next(3, 9);
			if (this.mineRandom.NextDouble() < 0.1)
			{
				levelsDown = levelsDown * 2 - 1;
			}
			this.lastLevelsDownFallen = levelsDown;
			Game1.player.health = Math.Max(1, Game1.player.health - levelsDown * 3);
			this.isFallingDownShaft = true;
			Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFall), 0.025f);
			Game1.player.CanMove = false;
			Game1.player.jump();
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0015DBE0 File Offset: 0x0015BDE0
		private void afterFall()
		{
			Game1.drawObjectDialogue(string.Concat(new object[]
			{
				"You fell ",
				this.lastLevelsDownFallen,
				" levels.",
				(this.lastLevelsDownFallen > 7) ? "Ouch!" : ""
			}));
			Game1.drawObjectDialogue(Game1.content.LoadString((this.lastLevelsDownFallen > 7) ? "Strings\\Locations:Mines_FallenFar" : "Strings\\Locations:Mines_Fallen", new object[]
			{
				this.lastLevelsDownFallen
			}));
			this.setNextLevel(this.mineLevel + this.lastLevelsDownFallen);
			Game1.messagePause = true;
			Game1.warpFarmer("UndergroundMine", 0, 0, 2);
			Game1.player.faceDirection(2);
			Game1.player.showFrame(5, false);
			Game1.globalFadeToClear(null, 0.01f);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0015DCB4 File Offset: 0x0015BEB4
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null && who.IsMainPlayer)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex <= 115)
				{
					if (tileIndex != 112)
					{
						if (tileIndex == 115)
						{
							Response[] options = new Response[]
							{
								new Response("Leave", Game1.content.LoadString("Strings\\Locations:Mines_LeaveMine", new object[0])),
								new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing", new object[0]))
							};
							base.createQuestionDialogue(" ", options, "ExitMine");
						}
					}
					else
					{
						Game1.activeClickableMenu = new MineElevatorMenu();
					}
				}
				else if (tileIndex != 173)
				{
					if (tileIndex != 174)
					{
						if (tileIndex == 194)
						{
							Game1.playSound("openBox");
							Game1.playSound("Ship");
							Tile expr_1B9 = this.map.GetLayer("Buildings").Tiles[tileLocation];
							tileIndex = expr_1B9.TileIndex;
							expr_1B9.TileIndex = tileIndex + 1;
							Tile expr_1F0 = this.map.GetLayer("Front").Tiles[tileLocation.X, tileLocation.Y - 1];
							tileIndex = expr_1F0.TileIndex;
							expr_1F0.TileIndex = tileIndex + 1;
							Game1.createRadialDebris(this, 382, tileLocation.X, tileLocation.Y, 6, false, -1, true, -1);
							this.updateMineLevelData(2, -1);
						}
					}
					else
					{
						Response[] options2 = new Response[]
						{
							new Response("Jump", Game1.content.LoadString("Strings\\Locations:Mines_ShaftJumpIn", new object[0])),
							new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing", new object[0]))
						};
						base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Mines_Shaft", new object[0]), options2, "Shaft");
					}
				}
				else
				{
					Game1.enterMine(false, this.mineLevel + 1, null);
					Game1.playSound("stairsdown");
				}
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0015DEF0 File Offset: 0x0015C0F0
		public override string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly)
		{
			if (Game1.random.NextDouble() < 0.15)
			{
				int objectIndex = 330;
				if (Game1.random.NextDouble() < 0.07)
				{
					if (Game1.random.NextDouble() < 0.75)
					{
						switch (Game1.random.Next(5))
						{
						case 0:
							objectIndex = 96;
							break;
						case 1:
							objectIndex = (Game1.player.archaeologyFound.ContainsKey(102) ? ((Game1.player.archaeologyFound[102][0] < 21) ? 102 : 770) : 770);
							break;
						case 2:
							objectIndex = 110;
							break;
						case 3:
							objectIndex = 112;
							break;
						case 4:
							objectIndex = 585;
							break;
						}
					}
					else if (Game1.random.NextDouble() < 0.75)
					{
						int mineArea = this.getMineArea(-1);
						if (mineArea != 0)
						{
							if (mineArea != 40)
							{
								if (mineArea == 80)
								{
									objectIndex = 99;
								}
							}
							else
							{
								objectIndex = ((Game1.random.NextDouble() < 0.5) ? 122 : 336);
							}
						}
						else
						{
							objectIndex = ((Game1.random.NextDouble() < 0.5) ? 121 : 97);
						}
					}
					else
					{
						objectIndex = ((Game1.random.NextDouble() < 0.5) ? 126 : 127);
					}
				}
				else if (Game1.random.NextDouble() < 0.19)
				{
					objectIndex = ((Game1.random.NextDouble() < 0.5) ? 390 : this.getOreIndexForLevel(this.mineLevel, Game1.random));
				}
				else
				{
					if (Game1.random.NextDouble() < 0.08)
					{
						Game1.createRadialDebris(this, 8, xLocation, yLocation, Game1.random.Next(1, 5), true, -1, false, -1);
						return "";
					}
					if (Game1.random.NextDouble() < 0.45)
					{
						objectIndex = 330;
					}
					else if (Game1.random.NextDouble() < 0.12)
					{
						if (Game1.random.NextDouble() < 0.25)
						{
							objectIndex = 749;
						}
						else
						{
							int mineArea = this.getMineArea(-1);
							if (mineArea != 0)
							{
								if (mineArea != 40)
								{
									if (mineArea == 80)
									{
										objectIndex = 537;
									}
								}
								else
								{
									objectIndex = 536;
								}
							}
							else
							{
								objectIndex = 535;
							}
						}
					}
					else
					{
						objectIndex = 78;
					}
				}
				Game1.createObjectDebris(objectIndex, xLocation, yLocation, Game1.player.uniqueMultiplayerID, this);
				return "";
			}
			return "";
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0015E194 File Offset: 0x0015C394
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			foreach (ResourceClump r in this.resourceClumps)
			{
				if (!glider && r.getBoundingBox(r.tile).Intersects(position))
				{
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0015E210 File Offset: 0x0015C410
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			foreach (NPC i in this.characters)
			{
				if (i is Monster)
				{
					(i as Monster).drawAboveAllLayers(b);
				}
			}
			if (this.fogAlpha > 0f || this.ambientFog)
			{
				Vector2 v = default(Vector2);
				for (float x = (float)(-64 * Game1.pixelZoom + (int)(this.fogPos.X % (float)(64 * Game1.pixelZoom))); x < (float)Game1.graphics.GraphicsDevice.Viewport.Width; x += (float)(64 * Game1.pixelZoom))
				{
					for (float y = (float)(-64 * Game1.pixelZoom + (int)(this.fogPos.Y % (float)(64 * Game1.pixelZoom))); y < (float)Game1.graphics.GraphicsDevice.Viewport.Height; y += (float)(64 * Game1.pixelZoom))
					{
						v.X = (float)((int)x);
						v.Y = (float)((int)y);
						b.Draw(Game1.mouseCursors, v, new Microsoft.Xna.Framework.Rectangle?(this.fogSource), (this.fogAlpha > 0f) ? (this.fogColor * this.fogAlpha) : (Color.Black * 0.95f), 0f, Vector2.Zero, (float)Game1.pixelZoom + 0.001f, SpriteEffects.None, 1f);
					}
				}
			}
			if (this.isMonsterArea)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(193, 324, 7, 10)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 25f, SpriteEffects.None, 1f);
				return;
			}
			SpriteText.drawString(b, string.Concat(this.mineLevel + ((this.getMineArea(-1) == 121) ? -120 : 0)), Game1.tileSize / 4, Game1.tileSize / 4, 999999, -1, 999999, 1f, 1f, false, 2, "", (this.getMineArea(-1) == 0 || this.isDarkArea()) ? 4 : ((this.getMineArea(-1) == 10) ? 6 : ((this.getMineArea(-1) == 40) ? 7 : ((this.getMineArea(-1) == 80) ? 2 : 3))));
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0015E4AC File Offset: 0x0015C6AC
		public override void checkForMusic(GameTime time)
		{
			if (Game1.player.freezePause > 0 || this.fogTime > 0)
			{
				return;
			}
			if (this.mineLevel == 120)
			{
				return;
			}
			if (Game1.currentSong == null || !Game1.currentSong.IsPlaying)
			{
				string trackName = "";
				int mineArea = this.getMineArea(-1);
				if (mineArea <= 10)
				{
					if (mineArea != 0 && mineArea != 10)
					{
						goto IL_77;
					}
				}
				else
				{
					if (mineArea == 40)
					{
						trackName = "Frost";
						goto IL_77;
					}
					if (mineArea == 80)
					{
						trackName = "Lava";
						goto IL_77;
					}
					if (mineArea != 121)
					{
						goto IL_77;
					}
				}
				trackName = "Upper";
				IL_77:
				trackName += "_Ambient";
				Game1.changeMusicTrack(trackName);
			}
			this.timeSinceLastMusic = Math.Min(335000, this.timeSinceLastMusic + time.ElapsedGameTime.Milliseconds);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0015E568 File Offset: 0x0015C768
		public void playMineSong()
		{
			if ((Game1.currentSong == null || !Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("Ambient")) && !this.isDarkArea())
			{
				this.timeSinceLastMusic = 0;
				if (Game1.player.isWearingRing(528))
				{
					Game1.changeMusicTrack(Utility.getRandomNonLoopingSong());
					return;
				}
				if (this.mineLevel < 40)
				{
					Game1.changeMusicTrack("EarthMine");
					return;
				}
				if (this.mineLevel < 80)
				{
					Game1.changeMusicTrack("FrostMine");
					return;
				}
				Game1.changeMusicTrack("LavaMine");
			}
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0015E5FD File Offset: 0x0015C7FD
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.forceViewportPlayerFollow = true;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0015E60C File Offset: 0x0015C80C
		public void createLadderDown(int x, int y)
		{
			if (this.getMineArea(-1) == 121 && !this.mustKillAllMonstersToAdvance() && this.mineRandom.NextDouble() < 0.2)
			{
				this.map.GetLayer("Buildings").Tiles[x, y] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 174);
			}
			else
			{
				this.ladderHasSpawned = true;
				this.map.GetLayer("Buildings").Tiles[x, y] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 173);
			}
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0015E704 File Offset: 0x0015C904
		public void checkStoneForItems(int tileIndexOfStone, int x, int y, Farmer who)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			double chanceModifier = Game1.dailyLuck / 2.0 + (double)who.MiningLevel * 0.005 + (double)who.LuckLevel * 0.001;
			Random r = new Random(x * 1000 + y + this.mineLevel + (int)Game1.uniqueIDForThisGame / 2);
			r.NextDouble();
			double oreModifier = (tileIndexOfStone == 40 || tileIndexOfStone == 42) ? 1.2 : 0.8;
			if (tileIndexOfStone == 34 || tileIndexOfStone == 36 || tileIndexOfStone != 50)
			{
			}
			this.stonesLeftOnThisLevel--;
			double chanceForLadderDown = 0.02 + 1.0 / (double)Math.Max(1, this.stonesLeftOnThisLevel) + (double)who.LuckLevel / 100.0 + Game1.dailyLuck / 5.0;
			if (this.characters.Count == 0)
			{
				chanceForLadderDown += 0.04;
			}
			if (!this.ladderHasSpawned && (this.stonesLeftOnThisLevel == 0 || r.NextDouble() < chanceForLadderDown))
			{
				this.createLadderDown(x, y);
			}
			if (base.breakStone(tileIndexOfStone, x, y, who, r))
			{
				return;
			}
			if (tileIndexOfStone == 44)
			{
				int whichGem = r.Next(59, 70);
				whichGem += whichGem % 2;
				if (who.timesReachedMineBottom == 0)
				{
					if (this.mineLevel < 40 && whichGem != 66 && whichGem != 68)
					{
						whichGem = ((r.NextDouble() < 0.5) ? 66 : 68);
					}
					else if (this.mineLevel < 80 && (whichGem == 64 || whichGem == 60))
					{
						whichGem = ((r.NextDouble() < 0.5) ? ((r.NextDouble() < 0.5) ? 66 : 70) : ((r.NextDouble() < 0.5) ? 68 : 62));
					}
				}
				Game1.createObjectDebris(whichGem, x, y, who.uniqueMultiplayerID, this);
				Stats expr_1FF = Game1.stats;
				uint otherPreciousGemsFound = expr_1FF.OtherPreciousGemsFound;
				expr_1FF.OtherPreciousGemsFound = otherPreciousGemsFound + 1u;
				return;
			}
			if (r.NextDouble() < 0.022 * (1.0 + chanceModifier) * (double)(who.professions.Contains(22) ? 2 : 1))
			{
				int index = 535 + ((this.getMineArea(-1) == 40) ? 1 : ((this.getMineArea(-1) == 80) ? 2 : 0));
				if (this.getMineArea(-1) == 121)
				{
					index = 749;
				}
				if (who.professions.Contains(19) && r.NextDouble() < 0.5)
				{
					Game1.createObjectDebris(index, x, y, who.uniqueMultiplayerID, this);
				}
				Game1.createObjectDebris(index, x, y, who.uniqueMultiplayerID, this);
				who.gainExperience(5, 20 * this.getMineArea(-1));
			}
			if (this.mineLevel > 20 && r.NextDouble() < 0.005 * (1.0 + chanceModifier) * (double)(who.professions.Contains(22) ? 2 : 1))
			{
				if (who.professions.Contains(19) && r.NextDouble() < 0.5)
				{
					Game1.createObjectDebris(749, x, y, who.uniqueMultiplayerID, this);
				}
				Game1.createObjectDebris(749, x, y, who.uniqueMultiplayerID, this);
				who.gainExperience(5, 40 * this.getMineArea(-1));
			}
			if (r.NextDouble() < 0.05 * (1.0 + chanceModifier) * oreModifier)
			{
				r.Next(1, 3);
				r.NextDouble();
				double arg_3B4_0 = 0.1 * (1.0 + chanceModifier);
				if (r.NextDouble() < 0.25)
				{
					Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, this);
					this.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)(Game1.tileSize * x), (float)(Game1.tileSize * y)), Color.White, 8, Game1.random.NextDouble() < 0.5, 80f, 0, -1, -1f, Game1.tileSize * 2, 0));
				}
				else
				{
					Game1.createObjectDebris(this.getOreIndexForLevel(this.mineLevel, r), x, y, who.uniqueMultiplayerID, this);
				}
				who.gainExperience(3, 5);
				return;
			}
			if (r.NextDouble() < 0.5)
			{
				Game1.createDebris(14, x, y, 1, this);
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0015EB88 File Offset: 0x0015CD88
		public int getOreIndexForLevel(int mineLevel, Random r)
		{
			if (mineLevel < 40)
			{
				if (mineLevel >= 20 && r.NextDouble() < 0.1)
				{
					return 380;
				}
				return 378;
			}
			else if (mineLevel < 80)
			{
				if (mineLevel >= 60 && r.NextDouble() < 0.1)
				{
					return 384;
				}
				if (r.NextDouble() >= 0.75)
				{
					return 378;
				}
				return 380;
			}
			else if (mineLevel < 120)
			{
				if (r.NextDouble() < 0.75)
				{
					return 384;
				}
				if (r.NextDouble() >= 0.75)
				{
					return 378;
				}
				return 380;
			}
			else
			{
				if (r.NextDouble() < 0.01 + (double)((float)(mineLevel - 120) / 2000f))
				{
					return 386;
				}
				if (r.NextDouble() < 0.75)
				{
					return 384;
				}
				if (r.NextDouble() >= 0.75)
				{
					return 378;
				}
				return 380;
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0015EC8A File Offset: 0x0015CE8A
		public int getMineArea(int level = -1)
		{
			if (level == -1)
			{
				level = this.mineLevel;
			}
			if (level >= 80 && level <= 120)
			{
				return 80;
			}
			if (level > 120)
			{
				return 121;
			}
			if (level >= 40)
			{
				return 40;
			}
			if (level > 10 && this.mineLevel < 30)
			{
				return 10;
			}
			return 0;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0015ECC8 File Offset: 0x0015CEC8
		public byte getWallAt(int x, int y)
		{
			return 255;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0015ECCF File Offset: 0x0015CECF
		public Color getLightingColor(GameTime time)
		{
			return this.lighting;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0015ECD8 File Offset: 0x0015CED8
		public Object getRandomItemForThisLevel(int level)
		{
			int index = 0;
			if (this.mineRandom.NextDouble() < 0.05 && level > 80)
			{
				index = 422;
			}
			else if (this.mineRandom.NextDouble() < 0.1 && level > 20 && this.getMineArea(-1) != 40)
			{
				index = 420;
			}
			else if (this.mineRandom.NextDouble() < 0.25)
			{
				int mineArea = this.getMineArea(-1);
				if (mineArea <= 10)
				{
					if (mineArea == 0 || mineArea == 10)
					{
						index = 86;
					}
				}
				else if (mineArea != 40)
				{
					if (mineArea != 80)
					{
						if (mineArea == 121)
						{
							index = ((this.mineRandom.NextDouble() < 0.3) ? 86 : ((this.mineRandom.NextDouble() < 0.3) ? 84 : 82));
						}
					}
					else
					{
						index = 82;
					}
				}
				else
				{
					index = 84;
				}
			}
			else
			{
				index = 80;
			}
			return new Object(index, 1, false, -1, 0)
			{
				isSpawnedObject = true
			};
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0015EDD8 File Offset: 0x0015CFD8
		public int getRandomGemRichStoneForThisLevel(int level)
		{
			int whichGem = this.mineRandom.Next(59, 70);
			whichGem += whichGem % 2;
			if (Game1.player.timesReachedMineBottom == 0)
			{
				if (level < 40 && whichGem != 66 && whichGem != 68)
				{
					whichGem = ((this.mineRandom.NextDouble() < 0.5) ? 66 : 68);
				}
				else if (level < 80 && (whichGem == 64 || whichGem == 60))
				{
					whichGem = ((this.mineRandom.NextDouble() < 0.5) ? ((this.mineRandom.NextDouble() < 0.5) ? 66 : 70) : ((this.mineRandom.NextDouble() < 0.5) ? 68 : 62));
				}
			}
			switch (whichGem)
			{
			case 60:
				return 12;
			case 62:
				return 14;
			case 64:
				return 4;
			case 66:
				return 8;
			case 68:
				return 10;
			case 70:
				return 6;
			}
			return 40;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0015EEE0 File Offset: 0x0015D0E0
		public Monster getMonsterForThisLevel(int level, int xTile, int yTile)
		{
			Vector2 position = new Vector2((float)xTile, (float)yTile) * (float)Game1.tileSize;
			float distanceFromLadder = Utility.distance((float)xTile, this.tileBeneathLadder.X, (float)yTile, this.tileBeneathLadder.Y);
			if (!this.isSlimeArea)
			{
				if (level < 40)
				{
					if (this.mineRandom.NextDouble() < 0.25 && !this.mustKillAllMonstersToAdvance())
					{
						return new Bug(position, this.mineRandom.Next(4));
					}
					if (level < 15)
					{
						if (base.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null)
						{
							return new Duggy(position);
						}
						if (this.mineRandom.NextDouble() < 0.15)
						{
							return new RockCrab(position);
						}
						return new GreenSlime(position, level);
					}
					else if (level <= 30)
					{
						if (base.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null)
						{
							return new Duggy(position);
						}
						if (this.mineRandom.NextDouble() < 0.15)
						{
							return new RockCrab(position);
						}
						if (this.mineRandom.NextDouble() < 0.05 && distanceFromLadder > 10f)
						{
							return new Fly(position, false);
						}
						if (this.mineRandom.NextDouble() < 0.45)
						{
							return new GreenSlime(position, level);
						}
						return new Grub(position, false);
					}
					else if (level <= 40)
					{
						if (this.mineRandom.NextDouble() < 0.1 && distanceFromLadder > 10f)
						{
							return new Bat(position, level);
						}
						return new RockGolem(position);
					}
				}
				else if (this.getMineArea(-1) == 40)
				{
					if (this.mineLevel >= 70 && this.mineRandom.NextDouble() < 0.75)
					{
						return new Skeleton(position);
					}
					if (this.mineRandom.NextDouble() < 0.3)
					{
						return new DustSpirit(position, this.mineRandom.NextDouble() < 0.8);
					}
					if (this.mineRandom.NextDouble() < 0.3 && distanceFromLadder > 10f)
					{
						return new Bat(position, this.mineLevel);
					}
					if (!this.ghostAdded && this.mineLevel > 50 && this.mineRandom.NextDouble() < 0.3 && distanceFromLadder > 10f)
					{
						this.ghostAdded = true;
						return new Ghost(position);
					}
				}
				else if (this.getMineArea(-1) == 80)
				{
					if (this.isDarkArea() && this.mineRandom.NextDouble() < 0.25)
					{
						return new Bat(position, this.mineLevel);
					}
					if (this.mineRandom.NextDouble() < 0.15)
					{
						return new GreenSlime(position, this.getMineArea(-1));
					}
					if (this.mineRandom.NextDouble() < 0.15)
					{
						return new MetalHead(position, this.getMineArea(-1));
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new ShadowBrute(position);
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new ShadowShaman(position);
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new RockCrab(position, "Lava Crab");
					}
					if (this.mineRandom.NextDouble() < 0.2 && distanceFromLadder > 8f && this.mineLevel >= 90)
					{
						return new SquidKid(position);
					}
				}
				else if (this.getMineArea(-1) == 121)
				{
					if (this.loadedDarkArea)
					{
						return new Mummy(position);
					}
					if (this.mineLevel % 20 == 0 && distanceFromLadder > 10f)
					{
						return new Bat(position, this.mineLevel);
					}
					if (this.mineLevel % 16 == 0 && !this.mustKillAllMonstersToAdvance())
					{
						return new Bug(position, this.mineRandom.Next(4));
					}
					if (this.mineRandom.NextDouble() < 0.33 && distanceFromLadder > 10f)
					{
						return new Serpent(position);
					}
					if (this.mineRandom.NextDouble() < 0.33 && !this.mustKillAllMonstersToAdvance())
					{
						return new Bug(position, this.mineRandom.Next(4));
					}
					if (this.mineRandom.NextDouble() < 0.25)
					{
						return new GreenSlime(position, level);
					}
					return new BigSlime(position);
				}
				return new GreenSlime(position, level);
			}
			if (this.mineRandom.NextDouble() < 0.2)
			{
				return new BigSlime(position, this.getMineArea(-1));
			}
			return new GreenSlime(position, this.mineLevel);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0015F374 File Offset: 0x0015D574
		public Color getCrystalColorForThisLevel()
		{
			Random levelRandom = new Random(this.mineLevel + Game1.player.timesReachedMineBottom);
			if (levelRandom.NextDouble() < 0.04 && this.mineLevel < 80)
			{
				Color c = new Color(this.mineRandom.Next(256), this.mineRandom.Next(256), this.mineRandom.Next(256));
				while ((int)(c.R + c.G + c.B) < 500)
				{
					c.R = (byte)Math.Min(255, (int)(c.R + 10));
					c.G = (byte)Math.Min(255, (int)(c.G + 10));
					c.B = (byte)Math.Min(255, (int)(c.B + 10));
				}
				return c;
			}
			if (levelRandom.NextDouble() < 0.07)
			{
				return new Color(255 - this.mineRandom.Next(20), 255 - this.mineRandom.Next(20), 255 - this.mineRandom.Next(20));
			}
			if (this.mineLevel < 40)
			{
				int num = this.mineRandom.Next(2);
				if (num == 0)
				{
					return new Color(58, 145, 72);
				}
				if (num == 1)
				{
					return new Color(255, 255, 255);
				}
			}
			else if (this.mineLevel < 80)
			{
				switch (this.mineRandom.Next(4))
				{
				case 0:
					return new Color(120, 0, 210);
				case 1:
					return new Color(0, 100, 170);
				case 2:
					return new Color(0, 220, 255);
				case 3:
					return new Color(0, 255, 220);
				}
			}
			else
			{
				int num = this.mineRandom.Next(2);
				if (num == 0)
				{
					return new Color(200, 100, 0);
				}
				if (num == 1)
				{
					return new Color(220, 60, 0);
				}
			}
			return Color.White;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0015F5A4 File Offset: 0x0015D7A4
		private Object chooseStoneType(double chanceForPurpleStone, double chanceForMysticStone, double gemStoneChance, Vector2 tile)
		{
			int stoneHealth = 1;
			int whichStone;
			if (this.mineLevel < 40)
			{
				whichStone = this.mineRandom.Next(31, 42);
				if (this.mineLevel % 40 < 30 && whichStone >= 33 && whichStone < 38)
				{
					whichStone = ((this.mineRandom.NextDouble() < 0.5) ? 32 : 38);
				}
				else if (this.mineLevel % 40 >= 30)
				{
					whichStone = ((this.mineRandom.NextDouble() < 0.5) ? 34 : 36);
				}
				if (this.mineLevel != 1 && this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
				{
					return new Object(tile, 751, "Stone", true, false, false, false)
					{
						minutesUntilReady = 3
					};
				}
			}
			else if (this.mineLevel < 80)
			{
				whichStone = this.mineRandom.Next(47, 54);
				stoneHealth = 3;
				if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
				{
					return new Object(tile, 290, "Stone", true, false, false, false)
					{
						minutesUntilReady = 4
					};
				}
			}
			else if (this.mineLevel < 120)
			{
				stoneHealth = 4;
				if (this.mineRandom.NextDouble() < 0.3 && !this.isDarkArea())
				{
					if (this.mineRandom.NextDouble() < 0.5)
					{
						whichStone = 38;
					}
					else
					{
						whichStone = 32;
					}
				}
				else if (this.mineRandom.NextDouble() < 0.3)
				{
					whichStone = this.mineRandom.Next(55, 58);
				}
				else if (this.mineRandom.NextDouble() < 0.5)
				{
					whichStone = 760;
				}
				else
				{
					whichStone = 762;
				}
				if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
				{
					return new Object(tile, 764, "Stone", true, false, false, false)
					{
						minutesUntilReady = 8
					};
				}
			}
			else
			{
				stoneHealth = 5;
				if (this.mineRandom.NextDouble() < 0.5)
				{
					if (this.mineRandom.NextDouble() < 0.5)
					{
						whichStone = 38;
					}
					else
					{
						whichStone = 32;
					}
				}
				else if (this.mineRandom.NextDouble() < 0.5)
				{
					whichStone = 40;
				}
				else
				{
					whichStone = 42;
				}
				double chanceForOre = 0.02 + (double)(this.mineLevel - 120) * 0.0005;
				if (this.mineLevel >= 130)
				{
					chanceForOre += 0.01 * (double)((float)(this.mineLevel - 120 - 10) / 10f);
				}
				double iridiumBoost = 0.0;
				if (this.mineLevel >= 130)
				{
					iridiumBoost += 0.001 * (double)((float)(this.mineLevel - 120 - 10) / 10f);
				}
				iridiumBoost = Math.Min(iridiumBoost, 0.004);
				if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < chanceForOre)
				{
					double chanceForIridium = (double)(this.mineLevel - 120) * (0.0003 + iridiumBoost);
					double chanceForGold = 0.01 + (double)(this.mineLevel - 120) * 0.0005;
					double chanceForIron = Math.Min(0.5, 0.1 + (double)(this.mineLevel - 120) * 0.005);
					if (this.mineRandom.NextDouble() < chanceForIridium)
					{
						return new Object(tile, 765, "Stone", true, false, false, false)
						{
							minutesUntilReady = 16
						};
					}
					if (this.mineRandom.NextDouble() < chanceForGold)
					{
						return new Object(tile, 764, "Stone", true, false, false, false)
						{
							minutesUntilReady = 8
						};
					}
					if (this.mineRandom.NextDouble() < chanceForIron)
					{
						return new Object(tile, 290, "Stone", true, false, false, false)
						{
							minutesUntilReady = 4
						};
					}
					return new Object(tile, 751, "Stone", true, false, false, false)
					{
						minutesUntilReady = 2
					};
				}
			}
			double chanceModifier = Game1.dailyLuck / 2.0 + (double)Game1.player.MiningLevel * 0.005;
			if (this.mineLevel > 50 && this.mineRandom.NextDouble() < 0.00025 + (double)this.mineLevel / 120000.0 + 0.0005 * chanceModifier / 2.0)
			{
				whichStone = 2;
				stoneHealth = 10;
			}
			else if (gemStoneChance != 0.0 && this.mineRandom.NextDouble() < gemStoneChance + gemStoneChance * chanceModifier + (double)this.mineLevel / 24000.0)
			{
				return new Object(tile, this.getRandomGemRichStoneForThisLevel(this.mineLevel), "Stone", true, false, false, false)
				{
					minutesUntilReady = 5
				};
			}
			if (this.mineRandom.NextDouble() < chanceForPurpleStone / 2.0 + chanceForPurpleStone * (double)Game1.player.MiningLevel * 0.008 + chanceForPurpleStone * (Game1.dailyLuck / 2.0))
			{
				whichStone = 44;
			}
			if (this.mineLevel > 100 && this.mineRandom.NextDouble() < chanceForMysticStone + chanceForMysticStone * (double)Game1.player.MiningLevel * 0.008 + chanceForMysticStone * (Game1.dailyLuck / 2.0))
			{
				whichStone = 46;
			}
			whichStone += whichStone % 2;
			if (this.mineRandom.NextDouble() < 0.1 && this.getMineArea(-1) != 40)
			{
				return new Object(tile, (this.mineRandom.NextDouble() < 0.5) ? 668 : 670, "Stone", true, false, false, false)
				{
					minutesUntilReady = 2,
					flipped = (this.mineRandom.NextDouble() < 0.5)
				};
			}
			return new Object(tile, whichStone, "Stone", true, false, false, false)
			{
				minutesUntilReady = stoneHealth
			};
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0015FBBC File Offset: 0x0015DDBC
		public override void draw(SpriteBatch b)
		{
			foreach (ResourceClump r in this.resourceClumps)
			{
				r.draw(b, r.tile);
			}
			base.draw(b);
		}

		// Token: 0x0400120F RID: 4623
		public const int mineFrostLevel = 40;

		// Token: 0x04001210 RID: 4624
		public const int mineLavaLevel = 80;

		// Token: 0x04001211 RID: 4625
		public const int upperArea = 0;

		// Token: 0x04001212 RID: 4626
		public const int jungleArea = 10;

		// Token: 0x04001213 RID: 4627
		public const int frostArea = 40;

		// Token: 0x04001214 RID: 4628
		public const int lavaArea = 80;

		// Token: 0x04001215 RID: 4629
		public const int desertArea = 121;

		// Token: 0x04001216 RID: 4630
		public const int bottomOfMineLevel = 120;

		// Token: 0x04001217 RID: 4631
		public const int numberOfLevelsPerArea = 40;

		// Token: 0x04001218 RID: 4632
		public const int mineFeature_barrels = 0;

		// Token: 0x04001219 RID: 4633
		public const int mineFeature_chests = 1;

		// Token: 0x0400121A RID: 4634
		public const int mineFeature_coalCart = 2;

		// Token: 0x0400121B RID: 4635
		public const int mineFeature_elevator = 3;

		// Token: 0x0400121C RID: 4636
		public const double chanceForColoredGemstone = 0.008;

		// Token: 0x0400121D RID: 4637
		public const double chanceForDiamond = 0.0005;

		// Token: 0x0400121E RID: 4638
		public const double chanceForPrismaticShard = 0.0005;

		// Token: 0x0400121F RID: 4639
		public const int monsterLimit = 30;

		// Token: 0x04001220 RID: 4640
		public SerializableDictionary<int, MineInfo> permanentMineChanges;

		// Token: 0x04001221 RID: 4641
		public List<ResourceClump> resourceClumps = new List<ResourceClump>();

		// Token: 0x04001222 RID: 4642
		private Random mineRandom;

		// Token: 0x04001223 RID: 4643
		public int mineLevel;

		// Token: 0x04001224 RID: 4644
		public int nextLevel;

		// Token: 0x04001225 RID: 4645
		public int lowestLevelReached;

		// Token: 0x04001226 RID: 4646
		private LocalizedContentManager mineLoader = Game1.content.CreateTemporary();

		// Token: 0x04001227 RID: 4647
		private Vector2 tileBeneathLadder;

		// Token: 0x04001228 RID: 4648
		private Vector2 tileBeneathElevator;

		// Token: 0x04001229 RID: 4649
		private int stonesLeftOnThisLevel;

		// Token: 0x0400122A RID: 4650
		private int timeSinceLastMusic = 200000;

		// Token: 0x0400122B RID: 4651
		private int timeUntilElevatorLightUp;

		// Token: 0x0400122C RID: 4652
		private int fogTime;

		// Token: 0x0400122D RID: 4653
		private Point ElevatorLightSpot;

		// Token: 0x0400122E RID: 4654
		private bool ladderHasSpawned;

		// Token: 0x0400122F RID: 4655
		private bool ghostAdded;

		// Token: 0x04001230 RID: 4656
		private bool loadedDarkArea;

		// Token: 0x04001231 RID: 4657
		private bool isSlimeArea;

		// Token: 0x04001232 RID: 4658
		private bool isMonsterArea;

		// Token: 0x04001233 RID: 4659
		private bool isFallingDownShaft;

		// Token: 0x04001234 RID: 4660
		private bool ambientFog;

		// Token: 0x04001235 RID: 4661
		private Vector2 fogPos;

		// Token: 0x04001236 RID: 4662
		private Color lighting = Color.White;

		// Token: 0x04001237 RID: 4663
		private Color fogColor;

		// Token: 0x04001238 RID: 4664
		private Point mostRecentLadder;

		// Token: 0x04001239 RID: 4665
		private float fogAlpha;

		// Token: 0x0400123A RID: 4666
		[XmlIgnore]
		public Cue bugLevelLoop;

		// Token: 0x0400123B RID: 4667
		private bool rainbowLights;

		// Token: 0x0400123C RID: 4668
		private bool isLightingDark;

		// Token: 0x0400123D RID: 4669
		private int lastLevelsDownFallen;

		// Token: 0x0400123E RID: 4670
		private Microsoft.Xna.Framework.Rectangle fogSource = new Microsoft.Xna.Framework.Rectangle(640, 0, 64, 64);
	}
}
