using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000026 RID: 38
	public class Farm : BuildableGameLocation
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0002B5DC File Offset: 0x000297DC
		public Farm()
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0002B664 File Offset: 0x00029864
		public Farm(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0002B6F0 File Offset: 0x000298F0
		public static string getMapNameFromTypeInt(int type)
		{
			switch (type)
			{
			case 0:
				return "Farm";
			case 1:
				return "Farm_Fishing";
			case 2:
				return "Farm_Foraging";
			case 3:
				return "Farm_Mining";
			case 4:
				return "Farm_Combat";
			default:
				return "Farm";
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0002B73C File Offset: 0x0002993C
		public void setMap(int which)
		{
			this.map = Game1.content.Load<Map>("Maps\\" + Farm.getMapNameFromTypeInt(which));
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0002B760 File Offset: 0x00029960
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			if (Game1.whichFarm == 4 && !Game1.player.mailReceived.Contains("henchmanGone"))
			{
				Game1.spawnMonstersAtNight = true;
			}
			this.lastItemShipped = null;
			for (int i = this.animals.Count - 1; i >= 0; i--)
			{
				this.animals.ElementAt(i).Value.dayUpdate(this);
			}
			for (int j = this.characters.Count - 1; j >= 0; j--)
			{
				if (this.characters[j] is JunimoHarvester)
				{
					this.characters.RemoveAt(j);
				}
			}
			for (int k = this.characters.Count - 1; k >= 0; k--)
			{
				if (this.characters[k] is Monster && (this.characters[k] as Monster).wildernessFarmMonster)
				{
					this.characters.RemoveAt(k);
				}
			}
			if (this.characters.Count > 5)
			{
				int slimesEscaped = 0;
				for (int l = this.characters.Count - 1; l >= 0; l--)
				{
					if (this.characters[l] is GreenSlime && Game1.random.NextDouble() < 0.035)
					{
						this.characters.RemoveAt(l);
						slimesEscaped++;
					}
				}
				if (slimesEscaped > 0)
				{
					Game1.showGlobalMessage(Game1.content.LoadString((slimesEscaped == 1) ? "Strings\\Locations:Farm_1SlimeEscaped" : "Strings\\Locations:Farm_NSlimesEscaped", new object[]
					{
						slimesEscaped
					}));
				}
			}
			if (Game1.whichFarm == 2)
			{
				for (int x = 0; x < 20; x++)
				{
					for (int y = 0; y < this.map.Layers[0].LayerHeight; y++)
					{
						if (this.map.GetLayer("Paths").Tiles[x, y] != null && this.map.GetLayer("Paths").Tiles[x, y].TileIndex == 21 && base.isTileLocationTotallyClearAndPlaceable(x, y) && base.isTileLocationTotallyClearAndPlaceable(x + 1, y) && base.isTileLocationTotallyClearAndPlaceable(x + 1, y + 1) && base.isTileLocationTotallyClearAndPlaceable(x, y + 1))
						{
							this.resourceClumps.Add(new ResourceClump(600, 2, 2, new Vector2((float)x, (float)y)));
						}
					}
				}
				if (!Game1.IsWinter)
				{
					while (Game1.random.NextDouble() < 0.75)
					{
						Vector2 v = new Vector2((float)Game1.random.Next(18), (float)Game1.random.Next(this.map.Layers[0].LayerHeight));
						if (Game1.random.NextDouble() < 0.5)
						{
							v = base.getRandomTile();
						}
						if (this.isTileLocationTotallyClearAndPlaceable(v) && base.getTileIndexAt((int)v.X, (int)v.Y, "AlwaysFront") == -1 && (v.X < 18f || base.doesTileHavePropertyNoNull((int)v.X, (int)v.Y, "Type", "Back").Equals("Grass")))
						{
							int whichItem = 792;
							string currentSeason = Game1.currentSeason;
							if (!(currentSeason == "spring"))
							{
								if (!(currentSeason == "summer"))
								{
									if (currentSeason == "fall")
									{
										switch (Game1.random.Next(4))
										{
										case 0:
											whichItem = 281;
											break;
										case 1:
											whichItem = 420;
											break;
										case 2:
											whichItem = 422;
											break;
										case 3:
											whichItem = 404;
											break;
										}
									}
								}
								else
								{
									switch (Game1.random.Next(4))
									{
									case 0:
										whichItem = 402;
										break;
									case 1:
										whichItem = 396;
										break;
									case 2:
										whichItem = 398;
										break;
									case 3:
										whichItem = 404;
										break;
									}
								}
							}
							else
							{
								switch (Game1.random.Next(4))
								{
								case 0:
									whichItem = 16;
									break;
								case 1:
									whichItem = 22;
									break;
								case 2:
									whichItem = 20;
									break;
								case 3:
									whichItem = 257;
									break;
								}
							}
							this.dropObject(new Object(v, whichItem, null, false, true, false, true), v * (float)Game1.tileSize, Game1.viewport, true, null);
						}
					}
					if (this.objects.Count > 0)
					{
						for (int m = 0; m < 6; m++)
						{
							Object o = this.objects.ElementAt(Game1.random.Next(this.objects.Count)).Value;
							if (o.name.Equals("Weeds"))
							{
								o.parentSheetIndex = 792 + Utility.getSeasonNumber(Game1.currentSeason);
							}
						}
					}
				}
			}
			if (Game1.whichFarm == 3)
			{
				this.doDailyMountainFarmUpdate();
			}
			Dictionary<Vector2, TerrainFeature>.KeyCollection objKeys = this.terrainFeatures.Keys;
			for (int n = objKeys.Count - 1; n >= 0; n--)
			{
				if (this.terrainFeatures[objKeys.ElementAt(n)] is HoeDirt && (this.terrainFeatures[objKeys.ElementAt(n)] as HoeDirt).crop == null && Game1.random.NextDouble() <= 0.1)
				{
					this.terrainFeatures.Remove(objKeys.ElementAt(n));
				}
			}
			if (this.terrainFeatures.Count > 0 && Game1.currentSeason.Equals("fall") && Game1.dayOfMonth > 1 && Game1.random.NextDouble() < 0.05)
			{
				for (int tries = 0; tries < 10; tries++)
				{
					TerrainFeature t = this.terrainFeatures.ElementAt(Game1.random.Next(this.terrainFeatures.Count)).Value;
					if (t is Tree && (t as Tree).growthStage >= 5 && !(t as Tree).tapped)
					{
						(t as Tree).treeType = 7;
						(t as Tree).loadSprite();
						break;
					}
				}
			}
			this.addCrows();
			if (!Game1.currentSeason.Equals("winter"))
			{
				base.spawnWeedsAndStones(Game1.currentSeason.Equals("summer") ? 30 : 20, false, true);
			}
			base.spawnWeeds(false);
			if (dayOfMonth == 1)
			{
				for (int i2 = this.terrainFeatures.Count - 1; i2 >= 0; i2--)
				{
					if (this.terrainFeatures.ElementAt(i2).Value is HoeDirt && (this.terrainFeatures.ElementAt(i2).Value as HoeDirt).crop == null && Game1.random.NextDouble() < 0.8)
					{
						this.terrainFeatures.Remove(this.terrainFeatures.ElementAt(i2).Key);
					}
				}
				base.spawnWeedsAndStones(20, false, false);
				if (Game1.currentSeason.Equals("spring") && Game1.stats.DaysPlayed > 1u)
				{
					base.spawnWeedsAndStones(40, false, false);
					base.spawnWeedsAndStones(40, true, false);
					for (int i3 = 0; i3 < 15; i3++)
					{
						int xCoord = Game1.random.Next(this.map.DisplayWidth / Game1.tileSize);
						int yCoord = Game1.random.Next(this.map.DisplayHeight / Game1.tileSize);
						Vector2 location = new Vector2((float)xCoord, (float)yCoord);
						Object o2;
						this.objects.TryGetValue(location, out o2);
						if (o2 == null && base.doesTileHaveProperty(xCoord, yCoord, "Diggable", "Back") != null && base.isTileLocationOpen(new Location(xCoord * Game1.tileSize, yCoord * Game1.tileSize)) && !this.isTileOccupied(location, "") && base.doesTileHaveProperty(xCoord, yCoord, "Water", "Back") == null)
						{
							this.terrainFeatures.Add(location, new Grass(1, 4));
						}
					}
					base.growWeedGrass(40);
				}
			}
			base.growWeedGrass(1);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		public void doDailyMountainFarmUpdate()
		{
			double chance = 1.0;
			while (Game1.random.NextDouble() < chance)
			{
				Vector2 v = Utility.getRandomPositionInThisRectangle(new Microsoft.Xna.Framework.Rectangle(5, 37, 22, 8), Game1.random);
				if (base.doesTileHavePropertyNoNull((int)v.X, (int)v.Y, "Type", "Back").Equals("Dirt") && this.isTileLocationTotallyClearAndPlaceable(v))
				{
					int whichStone = 668;
					int health = 2;
					if (Game1.random.NextDouble() < 0.15)
					{
						this.objects.Add(v, new Object(v, 590, 1));
						continue;
					}
					if (Game1.random.NextDouble() < 0.5)
					{
						whichStone = 670;
					}
					if (Game1.random.NextDouble() < 0.1)
					{
						if (Game1.player.MiningLevel >= 8 && Game1.random.NextDouble() < 0.33)
						{
							whichStone = 77;
							health = 7;
						}
						else if (Game1.player.MiningLevel >= 5 && Game1.random.NextDouble() < 0.5)
						{
							whichStone = 76;
							health = 5;
						}
						else
						{
							whichStone = 75;
							health = 3;
						}
					}
					if (Game1.random.NextDouble() < 0.21)
					{
						whichStone = 751;
						health = 3;
					}
					if (Game1.player.MiningLevel >= 4 && Game1.random.NextDouble() < 0.15)
					{
						whichStone = 290;
						health = 4;
					}
					if (Game1.player.MiningLevel >= 7 && Game1.random.NextDouble() < 0.1)
					{
						whichStone = 764;
						health = 8;
					}
					if (Game1.player.MiningLevel >= 10 && Game1.random.NextDouble() < 0.01)
					{
						whichStone = 765;
						health = 16;
					}
					this.objects.Add(v, new Object(v, whichStone, 10)
					{
						minutesUntilReady = health
					});
				}
				chance *= 0.66;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0002C1F4 File Offset: 0x0002A3F4
		public void addCrows()
		{
			int numCrops = 0;
			foreach (KeyValuePair<Vector2, TerrainFeature> v in this.terrainFeatures)
			{
				if (v.Value is HoeDirt && (v.Value as HoeDirt).crop != null)
				{
					numCrops++;
				}
			}
			List<Vector2> scarecrowPositions = new List<Vector2>();
			foreach (KeyValuePair<Vector2, Object> v2 in this.objects)
			{
				if (v2.Value.bigCraftable && v2.Value.Name.Contains("arecrow"))
				{
					scarecrowPositions.Add(v2.Key);
				}
			}
			int potentialCrows = Math.Min(4, numCrops / 16);
			for (int i = 0; i < potentialCrows; i++)
			{
				if (Game1.random.NextDouble() < 0.3)
				{
					int attempts = 0;
					while (attempts < 10)
					{
						Vector2 v3 = this.terrainFeatures.ElementAt(Game1.random.Next(this.terrainFeatures.Count)).Key;
						if (this.terrainFeatures[v3] is HoeDirt && (this.terrainFeatures[v3] as HoeDirt).crop != null && (this.terrainFeatures[v3] as HoeDirt).crop.currentPhase > 1)
						{
							bool scarecrow = false;
							foreach (Vector2 s in scarecrowPositions)
							{
								if (Vector2.Distance(s, v3) < 9f)
								{
									scarecrow = true;
									this.objects[s].specialVariable++;
									break;
								}
							}
							if (!scarecrow)
							{
								(this.terrainFeatures[v3] as HoeDirt).crop = null;
								this.critters.Add(new StardewValley.BellsAndWhistles.Crow((int)v3.X, (int)v3.Y));
								break;
							}
							break;
						}
						else
						{
							attempts++;
						}
					}
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0002C458 File Offset: 0x0002A658
		public static Point getFrontDoorPositionForFarmer(Farmer who)
		{
			int farmerNumber = Utility.getFarmerNumberFromFarmer(who);
			if (farmerNumber == 1)
			{
				return new Point(64, 14);
			}
			throw new Exception();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0002C480 File Offset: 0x0002A680
		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (timeOfDay == 1300)
			{
				foreach (NPC i in this.characters)
				{
					if (i.isMarried())
					{
						i.returnHomeFromFarmPosition(this);
					}
				}
			}
			foreach (NPC c in this.characters)
			{
				if (c.isMarried())
				{
					c.checkForMarriageDialogue(timeOfDay, this);
				}
				if (c is Child)
				{
					(c as Child).tenMinuteUpdate();
				}
			}
			if (Game1.spawnMonstersAtNight && Game1.farmEvent == null && Game1.timeOfDay >= 1900 && Game1.random.NextDouble() < 0.25 - Game1.dailyLuck / 2.0)
			{
				if (Game1.random.NextDouble() < 0.25)
				{
					if (this.Equals(Game1.currentLocation))
					{
						this.spawnFlyingMonstersOffScreen();
						return;
					}
				}
				else
				{
					this.spawnGroundMonsterOffScreen();
				}
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0002C5B8 File Offset: 0x0002A7B8
		public void spawnGroundMonsterOffScreen()
		{
			for (int i = 0; i < 15; i++)
			{
				Vector2 spawnLocation = Vector2.Zero;
				spawnLocation = base.getRandomTile();
				if (Utility.isOnScreen(Utility.Vector2ToPoint(spawnLocation), Game1.tileSize, this))
				{
					spawnLocation.X -= (float)(Game1.viewport.Width / Game1.tileSize);
				}
				if (this.isTileLocationTotallyClearAndPlaceable(spawnLocation))
				{
					bool success;
					if (Game1.player.CombatLevel >= 8 && Game1.random.NextDouble() < 0.15)
					{
						this.characters.Add(new ShadowBrute(spawnLocation * (float)Game1.tileSize)
						{
							focusedOnFarmers = true,
							wildernessFarmMonster = true
						});
						success = true;
					}
					else if (Game1.random.NextDouble() < 0.65 && this.isTileLocationTotallyClearAndPlaceable(spawnLocation))
					{
						this.characters.Add(new RockGolem(spawnLocation * (float)Game1.tileSize, Game1.player.CombatLevel)
						{
							wildernessFarmMonster = true
						});
						success = true;
					}
					else
					{
						int virtualMineLevel = 1;
						if (Game1.player.CombatLevel >= 10)
						{
							virtualMineLevel = 140;
						}
						else if (Game1.player.CombatLevel >= 8)
						{
							virtualMineLevel = 100;
						}
						else if (Game1.player.CombatLevel >= 4)
						{
							virtualMineLevel = 41;
						}
						this.characters.Add(new GreenSlime(spawnLocation * (float)Game1.tileSize, virtualMineLevel)
						{
							wildernessFarmMonster = true
						});
						success = true;
					}
					if (success && Game1.currentLocation.Equals(this))
					{
						foreach (KeyValuePair<Vector2, Object> v in this.objects)
						{
							if (v.Value != null && v.Value.bigCraftable && v.Value.parentSheetIndex == 83)
							{
								v.Value.shakeTimer = 1000;
								v.Value.showNextIndex = true;
								Game1.currentLightSources.Add(new LightSource(4, v.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 1f, Color.Cyan * 0.75f, (int)(v.Key.X * 797f + v.Key.Y * 13f + 666f)));
							}
						}
					}
					return;
				}
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0002C84C File Offset: 0x0002AA4C
		public void spawnFlyingMonstersOffScreen()
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
				spawnLocation.X -= (float)Game1.viewport.Width;
			}
			bool success;
			if (Game1.player.CombatLevel >= 10 && Game1.random.NextDouble() < 0.25)
			{
				this.characters.Add(new Serpent(spawnLocation * (float)Game1.tileSize)
				{
					focusedOnFarmers = true,
					wildernessFarmMonster = true
				});
				success = true;
			}
			else if (Game1.player.CombatLevel >= 8 && Game1.random.NextDouble() < 0.5)
			{
				this.characters.Add(new Bat(spawnLocation * (float)Game1.tileSize, 81)
				{
					focusedOnFarmers = true,
					wildernessFarmMonster = true
				});
				success = true;
			}
			else if (Game1.player.CombatLevel >= 5 && Game1.random.NextDouble() < 0.5)
			{
				this.characters.Add(new Bat(spawnLocation * (float)Game1.tileSize, 41)
				{
					focusedOnFarmers = true,
					wildernessFarmMonster = true
				});
				success = true;
			}
			else
			{
				this.characters.Add(new Bat(spawnLocation * (float)Game1.tileSize, 1)
				{
					focusedOnFarmers = true,
					wildernessFarmMonster = true
				});
				success = true;
			}
			if (success && Game1.currentLocation.Equals(this))
			{
				foreach (KeyValuePair<Vector2, Object> v in this.objects)
				{
					if (v.Value != null && v.Value.bigCraftable && v.Value.parentSheetIndex == 83)
					{
						v.Value.shakeTimer = 1000;
						v.Value.showNextIndex = true;
						Game1.currentLightSources.Add(new LightSource(4, v.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), 1f, Color.Cyan * 0.75f, (int)(v.Key.X * 797f + v.Key.Y * 13f + 666f)));
					}
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0002CBEC File Offset: 0x0002ADEC
		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			Point p = new Point(tileX * Game1.tileSize + Game1.tileSize / 2, tileY * Game1.tileSize + Game1.tileSize / 2);
			for (int i = this.resourceClumps.Count - 1; i >= 0; i--)
			{
				if (this.resourceClumps[i].getBoundingBox(this.resourceClumps[i].tile).Contains(p) && this.resourceClumps[i].performToolAction(t, 1, this.resourceClumps[i].tile, null))
				{
					this.resourceClumps.RemoveAt(i);
				}
			}
			if (t is MeleeWeapon)
			{
				foreach (FarmAnimal a in this.animals.Values)
				{
					if (a.GetBoundingBox().Intersects((t as MeleeWeapon).mostRecentArea))
					{
						a.hitWithWeapon(t as MeleeWeapon);
					}
				}
			}
			if (t is WateringCan && (t as WateringCan).WaterLeft > 0 && base.getTileIndexAt(tileX, tileY, "Buildings") == 1938)
			{
				base.setMapTileIndex(tileX, tileY, 1939, "Buildings", 0);
			}
			return false;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0002CD4C File Offset: 0x0002AF4C
		public override void timeUpdate(int timeElapsed)
		{
			base.timeUpdate(timeElapsed);
			using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator = this.animals.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.updatePerTenMinutes(Game1.timeOfDay, this);
				}
			}
			foreach (Building b in this.buildings)
			{
				if (b.indoors != null)
				{
					b.indoors.performTenMinuteUpdate(timeElapsed);
				}
				b.performTenMinuteAction(timeElapsed);
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0002CE04 File Offset: 0x0002B004
		public bool placeAnimal(BluePrint blueprint, Vector2 tileLocation, bool serverCommand, long ownerID)
		{
			for (int y = 0; y < blueprint.tilesHeight; y++)
			{
				for (int x = 0; x < blueprint.tilesWidth; x++)
				{
					Vector2 currentGlobalTilePosition = new Vector2(tileLocation.X + (float)x, tileLocation.Y + (float)y);
					if (Game1.player.getTileLocation().Equals(currentGlobalTilePosition) || this.isTileOccupied(currentGlobalTilePosition, "") || !base.isTilePassable(new Location((int)currentGlobalTilePosition.X, (int)currentGlobalTilePosition.Y), Game1.viewport))
					{
						return false;
					}
				}
			}
			long id = 0L;
			if (Game1.IsMultiplayer)
			{
				if (Game1.IsServer)
				{
					MultiplayerUtility.recentMultiplayerEntityID = MultiplayerUtility.getNewID();
					id = MultiplayerUtility.recentMultiplayerEntityID;
				}
				MultiplayerUtility.broadcastBuildingChange(0, tileLocation, blueprint.name, this.name, Game1.player.uniqueMultiplayerID);
				if (Game1.IsClient && !serverCommand)
				{
					return false;
				}
				if (Game1.IsClient & serverCommand)
				{
					id = MultiplayerUtility.recentMultiplayerEntityID;
				}
			}
			else
			{
				id = MultiplayerUtility.getNewID();
			}
			FarmAnimal animal = new FarmAnimal(blueprint.name, id, ownerID);
			animal.position = new Vector2(tileLocation.X * (float)Game1.tileSize + 4f, tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize - (float)animal.sprite.getHeight() - 4f);
			this.animals.Add(id, animal);
			if (animal.sound != null && !animal.sound.Equals(""))
			{
				Game1.playSound(animal.sound);
			}
			return true;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0002CF84 File Offset: 0x0002B184
		public int tryToAddHay(int num)
		{
			int piecesToAdd = Math.Min(Utility.numSilos() * 240 - this.piecesOfHay, num);
			this.piecesOfHay += piecesToAdd;
			return num - piecesToAdd;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0002CFC0 File Offset: 0x0002B1C0
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			if (!glider)
			{
				foreach (ResourceClump expr_1C in this.resourceClumps)
				{
					if (expr_1C.getBoundingBox(expr_1C.tile).Intersects(position))
					{
						bool result = true;
						return result;
					}
				}
				foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
				{
					if (character != null && !character.Equals(kvp.Value) && !(character is FarmAnimal) && position.Intersects(kvp.Value.GetBoundingBox()))
					{
						if (isFarmer && (character as Farmer).temporaryImpassableTile.Intersects(position))
						{
							break;
						}
						kvp.Value.farmerPushing();
						bool result = true;
						return result;
					}
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0002D0E0 File Offset: 0x0002B2E0
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (!this.objects.ContainsKey(new Vector2((float)tileLocation.X, (float)tileLocation.Y)))
			{
				foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
				{
					if (kvp.Value.GetBoundingBox().Intersects(tileRect) && !kvp.Value.wasPet)
					{
						kvp.Value.pet(who);
						bool result = true;
						return result;
					}
				}
				foreach (KeyValuePair<long, FarmAnimal> kvp2 in this.animals)
				{
					if (kvp2.Value.GetBoundingBox().Intersects(tileRect))
					{
						kvp2.Value.pet(who);
						bool result = true;
						return result;
					}
				}
			}
			foreach (ResourceClump stump in this.resourceClumps)
			{
				if (stump.getBoundingBox(stump.tile).Intersects(tileRect))
				{
					stump.performUseAction(new Vector2((float)tileLocation.X, (float)tileLocation.Y));
					bool result = true;
					return result;
				}
			}
			if (tileLocation.X >= 71 && tileLocation.X <= 72 && tileLocation.Y >= 13 && tileLocation.Y <= 14)
			{
				ItemGrabMenu expr_1E1 = new ItemGrabMenu(null, true, false, new InventoryMenu.highlightThisItem(Utility.highlightShippableObjects), new ItemGrabMenu.behaviorOnItemSelect(this.shipItem), "", null, true, true, false, true, false, 0, null, -1, null);
				expr_1E1.initializeUpperRightCloseButton();
				expr_1E1.setBackgroundTransparency(false);
				expr_1E1.setDestroyItemOnClick(true);
				expr_1E1.initializeShippingBin();
				Game1.activeClickableMenu = expr_1E1;
				Game1.playSound("shwip");
				if (Game1.player.facingDirection == 1)
				{
					Game1.player.Halt();
				}
				Game1.player.showCarrying();
				return true;
			}
			switch ((this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1)
			{
			case 1956:
			case 1957:
			case 1958:
				if (!this.hasSeenGrandpaNote)
				{
					this.hasSeenGrandpaNote = true;
					Game1.activeClickableMenu = new LetterViewerMenu(Game1.content.LoadString("Strings\\Locations:Farm_GrandpaNote", new object[]
					{
						Game1.player.name
					}).Replace('\n', '^'));
					return true;
				}
				if (Game1.year >= 3 && this.grandpaScore > 0 && this.grandpaScore < 4)
				{
					if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 72 && this.grandpaScore < 4)
					{
						who.reduceActiveItemByOne();
						this.grandpaScore = 0;
						Game1.soundBank.PlayCue("stoneStep");
						Game1.soundBank.PlayCue("fireball");
						DelayedAction.playSoundAfterDelay("yoba", 800);
						who.eventsSeen.Remove(558292);
						who.eventsSeen.Add(321777);
						who.freezePause = 1200;
						base.removeTemporarySpritesWithID(6666);
						DelayedAction.showDialogueAfterDelay(Game1.content.LoadString("Strings\\Locations:Farm_GrandpaShrine_PlaceDiamond", new object[0]), 1200);
						return true;
					}
					if (who.ActiveObject == null || who.ActiveObject.parentSheetIndex != 72)
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Farm_GrandpaShrine_DiamondSlot", new object[0]));
						return true;
					}
				}
				else
				{
					if (this.grandpaScore >= 4 && !Utility.doesItemWithThisIndexExistAnywhere(160, true))
					{
						who.addItemByMenuIfNecessaryElseHoldUp(new Object(Vector2.Zero, 160, false), new ItemGrabMenu.behaviorOnItemSelect(this.grandpaStatueCallback));
						return true;
					}
					if (this.grandpaScore == 0 && Game1.year >= 3)
					{
						Game1.player.eventsSeen.Remove(558292);
						if (!Game1.player.eventsSeen.Contains(321777))
						{
							Game1.player.eventsSeen.Add(321777);
						}
					}
				}
				break;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0002D590 File Offset: 0x0002B790
		public void grandpaStatueCallback(Item item, Farmer who)
		{
			if (item != null && item is Object && (item as Object).bigCraftable && (item as Object).parentSheetIndex == 160 && who != null)
			{
				who.mailReceived.Add("grandpaPerfect");
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		private void shipItem(Item i, Farmer who)
		{
			if (i != null)
			{
				this.shippingBin.Add(i);
				if (i is Object)
				{
					this.showShipment(i as Object, false);
				}
				this.lastItemShipped = i;
				who.removeItemFromInventory(i);
				if (Game1.player.ActiveObject == null)
				{
					Game1.player.showNotCarrying();
					Game1.player.Halt();
				}
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0002D62F File Offset: 0x0002B82F
		public void shipItem(Item i)
		{
			this.shippingBin.Add(i);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0002D640 File Offset: 0x0002B840
		public override bool leftClick(int x, int y, Farmer who)
		{
			if (who.ActiveObject != null && x / Game1.tileSize >= 71 && x / Game1.tileSize <= 72 && y / Game1.tileSize >= 13 && y / Game1.tileSize <= 14 && who.ActiveObject.canBeShipped() && Vector2.Distance(who.getTileLocation(), new Vector2(71.5f, 14f)) <= 2f)
			{
				this.shippingBin.Add(who.ActiveObject);
				this.lastItemShipped = who.ActiveObject;
				who.showNotCarrying();
				this.showShipment(who.ActiveObject, true);
				who.ActiveObject = null;
				return true;
			}
			return base.leftClick(x, y, who);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0002D6F8 File Offset: 0x0002B8F8
		public void showShipment(Object o, bool playThrowSound = true)
		{
			if (playThrowSound)
			{
				Game1.playSound("backpackIN");
			}
			DelayedAction.playSoundAfterDelay("Ship", playThrowSound ? 250 : 0);
			int temp = Game1.random.Next();
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(524, 218, 34, 22), new Vector2(71f, 13f) * (float)Game1.tileSize + new Vector2(0f, 5f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 100f,
				totalNumberOfLoops = 1,
				animationLength = 3,
				pingPong = true,
				scale = (float)Game1.pixelZoom,
				layerDepth = (float)(15 * Game1.tileSize) / 10000f + 1E-05f,
				id = (float)temp,
				extraInfoForEndBehavior = temp,
				endFunction = new TemporaryAnimatedSprite.endBehavior(base.removeTemporarySpritesWithID)
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(524, 230, 34, 10), new Vector2(71f, 13f) * (float)Game1.tileSize + new Vector2(0f, 17f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 100f,
				totalNumberOfLoops = 1,
				animationLength = 3,
				pingPong = true,
				scale = (float)Game1.pixelZoom,
				layerDepth = (float)(15 * Game1.tileSize) / 10000f + 0.0003f,
				id = (float)temp,
				extraInfoForEndBehavior = temp
			});
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, o.parentSheetIndex, 16, 16), new Vector2(71f, 13f) * (float)Game1.tileSize + new Vector2((float)(8 + Game1.random.Next(6)), 2f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 9999f,
				scale = (float)Game1.pixelZoom,
				alphaFade = 0.045f,
				layerDepth = (float)(15 * Game1.tileSize) / 10000f + 0.000225f,
				motion = new Vector2(0f, 0.3f),
				acceleration = new Vector2(0f, 0.2f),
				scaleChange = -0.05f
			});
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0002D9B0 File Offset: 0x0002BBB0
		public override int getFishingLocation(Vector2 tile)
		{
			switch (Game1.whichFarm)
			{
			case 1:
			case 2:
				return 1;
			case 3:
				return 0;
			default:
				return -1;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0002D9E0 File Offset: 0x0002BBE0
		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.whichFarm != 1)
			{
				if (Game1.whichFarm == 3)
				{
					if (Game1.random.NextDouble() < 0.5)
					{
						return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Forest");
					}
				}
				else if (Game1.whichFarm == 2)
				{
					if (Game1.random.NextDouble() < 0.05 + Game1.dailyLuck)
					{
						return new Object(734, 1, false, -1, 0);
					}
					if (Game1.random.NextDouble() < 0.45)
					{
						return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Forest");
					}
				}
				else if (Game1.whichFarm == 4 && Game1.random.NextDouble() <= 0.35)
				{
					return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Mountain");
				}
				return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
			}
			if (Game1.random.NextDouble() < 0.3)
			{
				return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Forest");
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Town");
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0002DAFC File Offset: 0x0002BCFC
		public List<FarmAnimal> getAllFarmAnimals()
		{
			List<FarmAnimal> farmAnimals = this.animals.Values.ToList<FarmAnimal>();
			foreach (Building b in this.buildings)
			{
				if (b.indoors != null && b.indoors.GetType() == typeof(AnimalHouse))
				{
					farmAnimals.AddRange(((AnimalHouse)b.indoors).animals.Values.ToList<FarmAnimal>());
				}
			}
			return farmAnimals;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0002DBA0 File Offset: 0x0002BDA0
		public override bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
		{
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (kvp.Value.getTileLocation().Equals(tileLocation))
				{
					bool result = true;
					return result;
				}
			}
			Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			foreach (ResourceClump expr_8F in this.resourceClumps)
			{
				if (expr_8F.getBoundingBox(expr_8F.tile).Intersects(r))
				{
					bool result = true;
					return result;
				}
			}
			return base.isTileOccupied(tileLocation, "");
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0002DC9C File Offset: 0x0002BE9C
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (!Game1.player.mailReceived.Contains("button_tut_2"))
			{
				Game1.player.mailReceived.Add("button_tut_2");
				Game1.onScreenMenus.Add(new ButtonTutorialMenu(1));
			}
			this.houseSource = new Microsoft.Xna.Framework.Rectangle(0, 144 * ((Game1.player.houseUpgradeLevel == 3) ? 2 : Game1.player.houseUpgradeLevel), 160, 144);
			this.greenhouseSource = new Microsoft.Xna.Framework.Rectangle(160, 160 * (Game1.player.mailReceived.Contains("ccPantry") ? 1 : 0), 112, 160);
			if (Game1.player.isMarried())
			{
				this.addSpouseOutdoorArea(Game1.player.spouse);
			}
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i] is Child)
				{
					(this.characters[i] as Child).resetForPlayerEntry(this);
				}
				if (this.characters[i].isVillager() && this.characters[i].name.Equals(Game1.player.spouse))
				{
					base.setMapTileIndex(54, 7, 1939, "Buildings", 0);
				}
				if (this.characters[i] is Pet && (base.getTileIndexAt(this.characters[i].getTileLocationPoint(), "Buildings") != -1 || base.getTileIndexAt(this.characters[i].getTileX() + 1, this.characters[i].getTileY(), "Buildings") != -1 || !this.isTileLocationTotallyClearAndPlaceable(this.characters[i].getTileLocation()) || !this.isTileLocationTotallyClearAndPlaceable(new Vector2((float)(this.characters[i].getTileX() + 1), (float)this.characters[i].getTileY()))))
				{
					this.characters[i].setTilePosition(new Point(54, 8));
					NPC expr_231_cp_0_cp_0 = this.characters[i];
					expr_231_cp_0_cp_0.position.X = expr_231_cp_0_cp_0.position.X - (float)Game1.tileSize;
				}
				if (Game1.timeOfDay >= 1300 && this.characters[i].isMarried() && this.characters[i].controller == null)
				{
					this.characters[i].drawOffset = Vector2.Zero;
					this.characters[i].sprite.StopAnimation();
					FarmHouse farmHouse = (FarmHouse)Game1.getLocationFromName("FarmHouse");
					Game1.warpCharacter(this.characters[i], "FarmHouse", farmHouse.getEntryLocation(), false, true);
					break;
				}
			}
			if (Game1.timeOfDay >= 1830)
			{
				for (int j = this.animals.Count - 1; j >= 0; j--)
				{
					this.animals.ElementAt(j).Value.warpHome(this, this.animals.ElementAt(j).Value);
				}
			}
			this.shippingBinLid = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(134, 226, 30, 25), new Vector2(71f, 13f) * (float)Game1.tileSize + new Vector2(2f, -7f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				holdLastFrame = true,
				destroyable = false,
				interval = 20f,
				animationLength = 13,
				paused = true,
				scale = (float)Game1.pixelZoom,
				layerDepth = (float)(15 * Game1.tileSize) / 10000f + 0.0001f,
				pingPong = true,
				pingPongMotion = 0
			};
			if (base.isThereABuildingUnderConstruction() && base.getBuildingUnderConstruction().daysOfConstructionLeft > 0 && Game1.getCharacterFromName("Robin", false).currentLocation.Equals(this))
			{
				Building b = base.getBuildingUnderConstruction();
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(399, 262, (b.daysOfConstructionLeft == 1) ? 29 : 9, 43), new Vector2((float)(b.tileX + b.tilesWide / 2), (float)(b.tileY + b.tilesHigh / 2)) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize * 2 - Game1.tileSize / 4)), false, 0f, Color.White)
				{
					scale = (float)Game1.pixelZoom,
					interval = 999999f,
					animationLength = 1,
					totalNumberOfLoops = 99999,
					layerDepth = (float)((b.tileY + b.tilesHigh / 2) * Game1.tileSize + Game1.tileSize / 2) / 10000f
				});
			}
			this.addGrandpaCandles();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0002E1CC File Offset: 0x0002C3CC
		public void addSpouseOutdoorArea(string spouseName)
		{
			base.removeTile(70, 9, "Buildings");
			base.removeTile(71, 9, "Buildings");
			base.removeTile(72, 9, "Buildings");
			base.removeTile(69, 9, "Buildings");
			base.removeTile(70, 8, "Buildings");
			base.removeTile(71, 8, "Buildings");
			base.removeTile(72, 8, "Buildings");
			base.removeTile(69, 8, "Buildings");
			base.removeTile(70, 7, "Front");
			base.removeTile(71, 7, "Front");
			base.removeTile(72, 7, "Front");
			base.removeTile(69, 7, "Front");
			base.removeTile(70, 6, "AlwaysFront");
			base.removeTile(71, 6, "AlwaysFront");
			base.removeTile(72, 6, "AlwaysFront");
			base.removeTile(69, 6, "AlwaysFront");
			uint num = <PrivateImplementationDetails>.ComputeStringHash(spouseName);
			if (num <= 1866496948u)
			{
				if (num <= 1067922812u)
				{
					if (num != 161540545u)
					{
						if (num != 587846041u)
						{
							if (num != 1067922812u)
							{
								return;
							}
							if (!(spouseName == "Sam"))
							{
								return;
							}
							base.setMapTileIndex(69, 8, 1173, "Buildings", 1);
							base.setMapTileIndex(72, 8, 1174, "Buildings", 1);
							base.setMapTileIndex(70, 8, 1198, "Buildings", 1);
							base.setMapTileIndex(71, 8, 1199, "Buildings", 1);
							base.setMapTileIndex(69, 7, 1148, "Front", 1);
							base.setMapTileIndex(72, 7, 1149, "Front", 1);
							return;
						}
						else
						{
							if (!(spouseName == "Penny"))
							{
								return;
							}
							base.setMapTileIndex(69, 8, 1098, "Buildings", 1);
							base.setMapTileIndex(70, 8, 1123, "Buildings", 1);
							base.setMapTileIndex(72, 8, 1098, "Buildings", 1);
							return;
						}
					}
					else
					{
						if (!(spouseName == "Sebastian"))
						{
							return;
						}
						base.setMapTileIndex(70, 8, 1927, "Buildings", 1);
						base.setMapTileIndex(71, 8, 1928, "Buildings", 1);
						base.setMapTileIndex(72, 8, 1929, "Buildings", 1);
						base.setMapTileIndex(70, 7, 1902, "Front", 1);
						base.setMapTileIndex(71, 7, 1903, "Front", 1);
						return;
					}
				}
				else if (num != 1281010426u)
				{
					if (num != 1708213605u)
					{
						if (num != 1866496948u)
						{
							return;
						}
						if (!(spouseName == "Shane"))
						{
							return;
						}
						base.setMapTileIndex(70, 9, 1940, "Buildings", 1);
						base.setMapTileIndex(71, 9, 1941, "Buildings", 1);
						base.setMapTileIndex(72, 9, 1942, "Buildings", 1);
						base.setMapTileIndex(70, 8, 1915, "Buildings", 1);
						base.setMapTileIndex(71, 8, 1916, "Buildings", 1);
						base.setMapTileIndex(72, 8, 1917, "Buildings", 1);
						base.setMapTileIndex(70, 7, 1772, "Front", 1);
						base.setMapTileIndex(71, 7, 1773, "Front", 1);
						base.setMapTileIndex(72, 7, 1774, "Front", 1);
						base.setMapTileIndex(70, 6, 1747, "AlwaysFront", 1);
						base.setMapTileIndex(71, 6, 1748, "AlwaysFront", 1);
						base.setMapTileIndex(72, 6, 1749, "AlwaysFront", 1);
						return;
					}
					else
					{
						if (!(spouseName == "Alex"))
						{
							return;
						}
						base.setMapTileIndex(69, 8, 1099, "Buildings", 1);
						return;
					}
				}
				else
				{
					if (!(spouseName == "Maru"))
					{
						return;
					}
					base.setMapTileIndex(71, 8, 1124, "Buildings", 1);
					return;
				}
			}
			else if (num <= 2571828641u)
			{
				if (num != 2010304804u)
				{
					if (num != 2434294092u)
					{
						if (num != 2571828641u)
						{
							return;
						}
						if (!(spouseName == "Emily"))
						{
							return;
						}
						base.setMapTileIndex(69, 8, 1867, "Buildings", 1);
						base.setMapTileIndex(72, 8, 1867, "Buildings", 1);
						base.setMapTileIndex(69, 7, 1842, "Front", 1);
						base.setMapTileIndex(72, 7, 1842, "Front", 1);
						base.setMapTileIndex(69, 9, 1866, "Buildings", 1);
						base.setMapTileIndex(71, 8, 1866, "Buildings", 1);
						base.setMapTileIndex(72, 9, 1967, "Buildings", 1);
						base.setMapTileIndex(70, 8, 1967, "Buildings", 1);
						return;
					}
					else
					{
						if (!(spouseName == "Haley"))
						{
							return;
						}
						base.setMapTileIndex(69, 8, 1074, "Buildings", 1);
						base.setMapTileIndex(69, 7, 1049, "Front", 1);
						base.setMapTileIndex(69, 6, 1024, "AlwaysFront", 1);
						base.setMapTileIndex(72, 8, 1074, "Buildings", 1);
						base.setMapTileIndex(72, 7, 1049, "Front", 1);
						base.setMapTileIndex(72, 6, 1024, "AlwaysFront", 1);
						return;
					}
				}
				else
				{
					if (!(spouseName == "Harvey"))
					{
						return;
					}
					base.setMapTileIndex(69, 8, 1098, "Buildings", 1);
					base.setMapTileIndex(70, 8, 1123, "Buildings", 1);
					base.setMapTileIndex(72, 8, 1098, "Buildings", 1);
					return;
				}
			}
			else if (num != 2732913340u)
			{
				if (num != 2826247323u)
				{
					if (num != 3066176300u)
					{
						return;
					}
					if (!(spouseName == "Elliott"))
					{
						return;
					}
					base.setMapTileIndex(69, 8, 1098, "Buildings", 1);
					base.setMapTileIndex(70, 8, 1123, "Buildings", 1);
					base.setMapTileIndex(72, 8, 1098, "Buildings", 1);
					return;
				}
				else
				{
					if (!(spouseName == "Leah"))
					{
						return;
					}
					base.setMapTileIndex(70, 8, 1122, "Buildings", 1);
					base.setMapTileIndex(70, 7, 1097, "Front", 1);
					return;
				}
			}
			else
			{
				if (!(spouseName == "Abigail"))
				{
					return;
				}
				base.setMapTileIndex(69, 8, 1098, "Buildings", 1);
				base.setMapTileIndex(70, 8, 1123, "Buildings", 1);
				base.setMapTileIndex(72, 8, 1098, "Buildings", 1);
				return;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0002E85C File Offset: 0x0002CA5C
		public void addGrandpaCandles()
		{
			if (this.grandpaScore > 0)
			{
				Microsoft.Xna.Framework.Rectangle candleSource = new Microsoft.Xna.Framework.Rectangle(577, 1985, 2, 5);
				base.removeTemporarySpritesWithID(6666);
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, candleSource, 99999f, 1, 9999, new Vector2((float)(7 * Game1.tileSize + 5 * Game1.pixelZoom), (float)(6 * Game1.tileSize + 5 * Game1.pixelZoom)), false, false, (float)(6 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(7 * Game1.tileSize + 3 * Game1.pixelZoom), (float)(6 * Game1.tileSize - Game1.pixelZoom)), false, 0f, Color.White)
				{
					interval = 50f,
					totalNumberOfLoops = 99999,
					animationLength = 7,
					light = true,
					id = 6666f,
					lightRadius = 1f,
					scale = (float)(Game1.pixelZoom * 3) / 4f,
					layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
					delayBeforeAnimationStart = 0
				});
				if (this.grandpaScore > 1)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, candleSource, 99999f, 1, 9999, new Vector2((float)(7 * Game1.tileSize + 10 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float)(6 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(7 * Game1.tileSize + 9 * Game1.pixelZoom), (float)(5 * Game1.tileSize)), false, 0f, Color.White)
					{
						interval = 50f,
						totalNumberOfLoops = 99999,
						animationLength = 7,
						light = true,
						id = 6666f,
						lightRadius = 1f,
						scale = (float)(Game1.pixelZoom * 3) / 4f,
						layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
						delayBeforeAnimationStart = 50
					});
				}
				if (this.grandpaScore > 2)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, candleSource, 99999f, 1, 9999, new Vector2((float)(9 * Game1.tileSize + 5 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float)(6 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(9 * Game1.tileSize + 4 * Game1.pixelZoom), (float)(5 * Game1.tileSize)), false, 0f, Color.White)
					{
						interval = 50f,
						totalNumberOfLoops = 99999,
						animationLength = 7,
						light = true,
						id = 6666f,
						lightRadius = 1f,
						scale = (float)(Game1.pixelZoom * 3) / 4f,
						layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
						delayBeforeAnimationStart = 100
					});
				}
				if (this.grandpaScore > 3)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, candleSource, 99999f, 1, 9999, new Vector2((float)(9 * Game1.tileSize + 10 * Game1.pixelZoom), (float)(6 * Game1.tileSize + 5 * Game1.pixelZoom)), false, false, (float)(6 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(9 * Game1.tileSize + 9 * Game1.pixelZoom), (float)(6 * Game1.tileSize - Game1.pixelZoom)), false, 0f, Color.White)
					{
						interval = 50f,
						totalNumberOfLoops = 99999,
						animationLength = 7,
						light = true,
						id = 6666f,
						lightRadius = 1f,
						scale = (float)(Game1.pixelZoom * 3) / 4f,
						layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
						delayBeforeAnimationStart = 150
					});
				}
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0002ED9D File Offset: 0x0002CF9D
		private void openShippingBinLid()
		{
			if (this.shippingBinLid != null)
			{
				if (this.shippingBinLid.pingPongMotion != 1)
				{
					Game1.playSound("doorCreak");
				}
				this.shippingBinLid.pingPongMotion = 1;
				this.shippingBinLid.paused = false;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0002EDD8 File Offset: 0x0002CFD8
		private void closeShippingBinLid()
		{
			if (this.shippingBinLid != null && this.shippingBinLid.currentParentTileIndex > 0)
			{
				if (this.shippingBinLid.pingPongMotion != -1)
				{
					Game1.playSound("doorCreakReverse");
				}
				this.shippingBinLid.pingPongMotion = -1;
				this.shippingBinLid.paused = false;
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0002EE2C File Offset: 0x0002D02C
		private void updateShippingBinLid(GameTime time)
		{
			if (this.isShippingBinLidOpen(true) && this.shippingBinLid.pingPongMotion == 1)
			{
				this.shippingBinLid.paused = true;
			}
			else if (this.shippingBinLid.currentParentTileIndex == 0 && this.shippingBinLid.pingPongMotion == -1)
			{
				if (!this.shippingBinLid.paused)
				{
					Game1.playSound("woodyStep");
				}
				this.shippingBinLid.paused = true;
			}
			this.shippingBinLid.update(time);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0002EEA9 File Offset: 0x0002D0A9
		private bool isShippingBinLidOpen(bool requiredToBeFullyOpen = false)
		{
			return this.shippingBinLid != null && this.shippingBinLid.currentParentTileIndex >= (requiredToBeFullyOpen ? (this.shippingBinLid.animationLength - 1) : 1);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0002EED6 File Offset: 0x0002D0D6
		public override bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
		{
			return base.doesTileHavePropertyNoNull((int)p.X, (int)p.Y, "Type", "Back").Length > 0 || base.shouldShadowBeDrawnAboveBuildingsLayer(p);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0002EF08 File Offset: 0x0002D108
		public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
		{
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (kvp.Value.getTileLocation().Equals(tileLocation))
				{
					bool result = true;
					return result;
				}
			}
			using (List<ResourceClump>.Enumerator enumerator2 = this.resourceClumps.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.occupiesTile((int)tileLocation.X, (int)tileLocation.Y))
					{
						bool result = true;
						return result;
					}
				}
			}
			return base.isTileOccupiedForPlacement(tileLocation, toPlace);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0002EFD0 File Offset: 0x0002D1D0
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			foreach (ResourceClump stump in this.resourceClumps)
			{
				stump.draw(b, stump.tile);
			}
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				kvp.Value.draw(b);
			}
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(3776f, 1088f)), new Microsoft.Xna.Framework.Rectangle?(Building.leftShadow), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			for (int x = 1; x < 8; x++)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(3776 + x * 64), 1088f)), new Microsoft.Xna.Framework.Rectangle?(Building.middleShadow), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			}
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(4288f, 1088f)), new Microsoft.Xna.Framework.Rectangle?(Building.rightShadow), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			b.Draw(this.houseTextures, Game1.GlobalToLocal(Game1.viewport, new Vector2(3712f, 520f)), new Microsoft.Xna.Framework.Rectangle?(this.houseSource), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.075f);
			b.Draw(this.houseTextures, Game1.GlobalToLocal(Game1.viewport, new Vector2(1600f, 384f)), new Microsoft.Xna.Framework.Rectangle?(this.greenhouseSource), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.0704f);
			if (Game1.mailbox.Count > 0)
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(68 * Game1.tileSize), (float)(16 * Game1.tileSize - Game1.tileSize * 3 / 2 - 48) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(17 * Game1.tileSize) / 10000f + 1E-06f + 0.0068f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(68 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float)(16 * Game1.tileSize - Game1.tileSize - 24 - Game1.tileSize / 8) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(189, 423, 15, 13)), Color.White, 0f, new Vector2(7f, 6f), 4f, SpriteEffects.None, (float)(17 * Game1.tileSize) / 10000f + 1E-05f + 0.0068f);
			}
			if (this.shippingBinLid != null)
			{
				this.shippingBinLid.draw(b, false, 0, 0);
			}
			if (!this.hasSeenGrandpaNote)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(9 * Game1.tileSize), (float)(7 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(575, 1972, 11, 8)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(7 * Game1.tileSize) / 10000f + 1E-06f);
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0002F3FC File Offset: 0x0002D5FC
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			if (Game1.spawnMonstersAtNight)
			{
				foreach (Character c in this.characters)
				{
					if (c is Monster)
					{
						(c as Monster).drawAboveAllLayers(b);
					}
				}
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0002F46C File Offset: 0x0002D66C
		public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			if (!Game1.currentLocation.Equals(this))
			{
				for (int i = this.animals.Count - 1; i >= 0; i--)
				{
					this.animals.ElementAt(i).Value.updateWhenNotCurrentLocation(null, time, this);
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0002F4C4 File Offset: 0x0002D6C4
		public bool isTileOpenBesidesTerrainFeatures(Vector2 tile)
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = new Microsoft.Xna.Framework.Rectangle((int)tile.X * Game1.tileSize, (int)tile.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.intersects(boundingBox))
					{
						bool result = false;
						return result;
					}
				}
			}
			foreach (ResourceClump expr_7D in this.resourceClumps)
			{
				if (expr_7D.getBoundingBox(expr_7D.tile).Intersects(boundingBox))
				{
					bool result = false;
					return result;
				}
			}
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (kvp.Value.getTileLocation().Equals(tile))
				{
					bool result = true;
					return result;
				}
			}
			return !this.objects.ContainsKey(tile) && base.isTilePassable(new Location((int)tile.X, (int)tile.Y), Game1.viewport);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0002F62C File Offset: 0x0002D82C
		public void addResourceClumpAndRemoveUnderlyingTerrain(int resourceClumpIndex, int width, int height, Vector2 tile)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					this.removeEverythingExceptCharactersFromThisTile((int)tile.X + x, (int)tile.Y + y);
				}
			}
			this.resourceClumps.Add(new ResourceClump(resourceClumpIndex, width, height, tile));
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0002F680 File Offset: 0x0002D880
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated && Game1.gameMode != 0)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			if (Utility.getHomeOfFarmer(Game1.player).fireplaceOn)
			{
				this.chimneyTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.chimneyTimer <= 0)
				{
					Point p = Utility.getHomeOfFarmer(Game1.player).getPorchStandingSpot();
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2((float)(p.X * Game1.tileSize + Game1.pixelZoom * ((Utility.getHomeOfFarmer(Game1.player).upgradeLevel >= 2) ? 9 : -5)), (float)(p.Y * Game1.tileSize - Game1.pixelZoom * 105)), false, 0.002f, Color.Gray)
					{
						alpha = 0.75f,
						motion = new Vector2(0f, -0.5f),
						acceleration = new Vector2(0.002f, 0f),
						interval = 99999f,
						layerDepth = 1f,
						scale = (float)(Game1.pixelZoom / 2),
						scaleChange = 0.02f,
						rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f
					});
					this.chimneyTimer = 500;
				}
			}
			foreach (ResourceClump stump in this.resourceClumps)
			{
				stump.tickUpdate(time, stump.tile);
			}
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (kvp.Value.updateWhenCurrentLocation(time, this))
				{
					this.animalsToRemove.Add(kvp.Key);
				}
			}
			for (int i = 0; i < this.animalsToRemove.Count; i++)
			{
				this.animals.Remove(this.animalsToRemove[i]);
			}
			this.animalsToRemove.Clear();
			if (this.shippingBinLid != null)
			{
				bool opening = false;
				using (List<Farmer>.Enumerator enumerator3 = base.getFarmers().GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.GetBoundingBox().Intersects(this.shippingBinLidOpenArea))
						{
							this.openShippingBinLid();
							opening = true;
						}
					}
				}
				if (!opening)
				{
					this.closeShippingBinLid();
				}
				this.updateShippingBinLid(time);
			}
		}

		// Token: 0x0400021E RID: 542
		public const int default_layout = 0;

		// Token: 0x0400021F RID: 543
		public const int riverlands_layout = 1;

		// Token: 0x04000220 RID: 544
		public const int forest_layout = 2;

		// Token: 0x04000221 RID: 545
		public const int mountains_layout = 3;

		// Token: 0x04000222 RID: 546
		public const int combat_layout = 4;

		// Token: 0x04000223 RID: 547
		public SerializableDictionary<long, FarmAnimal> animals = new SerializableDictionary<long, FarmAnimal>();

		// Token: 0x04000224 RID: 548
		[XmlIgnore]
		public Texture2D houseTextures = Game1.content.Load<Texture2D>("Buildings\\houses");

		// Token: 0x04000225 RID: 549
		public List<ResourceClump> resourceClumps = new List<ResourceClump>();

		// Token: 0x04000226 RID: 550
		public int piecesOfHay;

		// Token: 0x04000227 RID: 551
		public int grandpaScore;

		// Token: 0x04000228 RID: 552
		private TemporaryAnimatedSprite shippingBinLid;

		// Token: 0x04000229 RID: 553
		private Microsoft.Xna.Framework.Rectangle shippingBinLidOpenArea = new Microsoft.Xna.Framework.Rectangle(70 * Game1.tileSize, 13 * Game1.tileSize, Game1.tileSize * 4, Game1.tileSize * 3);

		// Token: 0x0400022A RID: 554
		[XmlIgnore]
		public List<Item> shippingBin = new List<Item>();

		// Token: 0x0400022B RID: 555
		[XmlIgnore]
		public Item lastItemShipped;

		// Token: 0x0400022C RID: 556
		public bool hasSeenGrandpaNote;

		// Token: 0x0400022D RID: 557
		private int chimneyTimer = 500;

		// Token: 0x0400022E RID: 558
		public const int numCropsForCrow = 16;

		// Token: 0x0400022F RID: 559
		private Microsoft.Xna.Framework.Rectangle houseSource;

		// Token: 0x04000230 RID: 560
		private Microsoft.Xna.Framework.Rectangle greenhouseSource;

		// Token: 0x04000231 RID: 561
		private List<long> animalsToRemove = new List<long>();
	}
}
