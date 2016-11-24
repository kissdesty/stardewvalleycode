using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace StardewValley
{
	// Token: 0x02000040 RID: 64
	public class Crop
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0003C994 File Offset: 0x0003AB94
		public Crop()
		{
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0003C9BC File Offset: 0x0003ABBC
		public Crop(bool forageCrop, int which, int tileX, int tileY)
		{
			this.forageCrop = forageCrop;
			this.whichForageCrop = which;
			this.fullyGrown = true;
			this.currentPhase = 5;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0003CA08 File Offset: 0x0003AC08
		public Crop(int seedIndex, int tileX, int tileY)
		{
			Dictionary<int, string> cropData = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
			if (seedIndex == 770)
			{
				seedIndex = Crop.getRandomLowGradeCropForThisSeason(Game1.currentSeason);
				if (seedIndex == 473)
				{
					seedIndex--;
				}
			}
			if (cropData.ContainsKey(seedIndex))
			{
				string[] split = cropData[seedIndex].Split(new char[]
				{
					'/'
				});
				string[] phaseSplit = split[0].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < phaseSplit.Length; i++)
				{
					this.phaseDays.Add(Convert.ToInt32(phaseSplit[i]));
				}
				this.phaseDays.Add(99999);
				string[] seasonSplit = split[1].Split(new char[]
				{
					' '
				});
				for (int j = 0; j < seasonSplit.Length; j++)
				{
					this.seasonsToGrowIn.Add(seasonSplit[j]);
				}
				this.rowInSpriteSheet = Convert.ToInt32(split[2]);
				this.indexOfHarvest = Convert.ToInt32(split[3]);
				this.regrowAfterHarvest = Convert.ToInt32(split[4]);
				this.harvestMethod = Convert.ToInt32(split[5]);
				string[] cropYieldSplit = split[6].Split(new char[]
				{
					' '
				});
				if (cropYieldSplit.Length != 0 && cropYieldSplit[0].Equals("true"))
				{
					this.minHarvest = Convert.ToInt32(cropYieldSplit[1]);
					this.maxHarvest = Convert.ToInt32(cropYieldSplit[2]);
					this.maxHarvestIncreasePerFarmingLevel = Convert.ToInt32(cropYieldSplit[3]);
					this.chanceForExtraCrops = Convert.ToDouble(cropYieldSplit[4]);
				}
				this.raisedSeeds = Convert.ToBoolean(split[7]);
				string[] programColors = split[8].Split(new char[]
				{
					' '
				});
				if (programColors.Length != 0 && programColors[0].Equals("true"))
				{
					List<Color> colors = new List<Color>();
					for (int k = 1; k < programColors.Length; k += 3)
					{
						colors.Add(new Color((int)Convert.ToByte(programColors[k]), (int)Convert.ToByte(programColors[k + 1]), (int)Convert.ToByte(programColors[k + 2])));
					}
					Random r = new Random(tileX * 1000 + tileY + Game1.dayOfMonth);
					this.tintColor = colors[r.Next(colors.Count)];
					this.programColored = true;
				}
				this.flip = (Game1.random.NextDouble() < 0.5);
			}
			if (this.rowInSpriteSheet == 23)
			{
				this.whichForageCrop = seedIndex;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0003CC94 File Offset: 0x0003AE94
		public static int getRandomLowGradeCropForThisSeason(string season)
		{
			if (season.Equals("winter"))
			{
				season = ((Game1.random.NextDouble() < 0.33) ? "spring" : ((Game1.random.NextDouble() < 0.5) ? "summer" : "fall"));
			}
			if (!(season == "spring"))
			{
				if (!(season == "summer"))
				{
					if (season == "fall")
					{
						return Game1.random.Next(487, 491);
					}
				}
				else
				{
					switch (Game1.random.Next(4))
					{
					case 0:
						return 487;
					case 1:
						return 483;
					case 2:
						return 482;
					case 3:
						return 484;
					}
				}
				return -1;
			}
			return Game1.random.Next(472, 476);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0003CD7D File Offset: 0x0003AF7D
		public void growCompletely()
		{
			this.currentPhase = this.phaseDays.Count - 1;
			this.dayOfCurrentPhase = 0;
			if (this.regrowAfterHarvest != -1)
			{
				this.fullyGrown = true;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0003CDAC File Offset: 0x0003AFAC
		public bool harvest(int xTile, int yTile, HoeDirt soil, JunimoHarvester junimoHarvester = null)
		{
			if (this.dead)
			{
				return junimoHarvester != null;
			}
			if (this.forageCrop)
			{
				Object o = null;
				int experience = 3;
				int fertilizer = this.whichForageCrop;
				if (fertilizer == 1)
				{
					o = new Object(399, 1, false, -1, 0);
				}
				if (Game1.player.professions.Contains(16))
				{
					o.quality = 4;
				}
				else if (Game1.random.NextDouble() < (double)((float)Game1.player.ForagingLevel / 30f))
				{
					o.quality = 2;
				}
				else if (Game1.random.NextDouble() < (double)((float)Game1.player.ForagingLevel / 15f))
				{
					o.quality = 1;
				}
				if (junimoHarvester != null)
				{
					junimoHarvester.tryToAddItemToHut(o);
					return true;
				}
				if (Game1.player.addItemToInventoryBool(o, false))
				{
					Vector2 initialTile = new Vector2((float)xTile, (float)yTile);
					Game1.player.animateOnce(279 + Game1.player.facingDirection);
					Game1.player.canMove = false;
					Game1.playSound("harvest");
					DelayedAction.playSoundAfterDelay("coin", 260);
					if (this.regrowAfterHarvest == -1)
					{
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(initialTile.X * (float)Game1.tileSize, initialTile.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(initialTile.X * (float)Game1.tileSize, initialTile.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
					}
					Game1.player.gainExperience(2, experience);
					return true;
				}
				Game1.showRedMessage("Inventory Full");
			}
			else if (this.currentPhase >= this.phaseDays.Count - 1 && (!this.fullyGrown || this.dayOfCurrentPhase <= 0))
			{
				int numToHarvest = 1;
				int cropQuality = 0;
				int fertilizerQualityLevel = 0;
				if (this.indexOfHarvest == 0)
				{
					return true;
				}
				Random r = new Random(xTile * 7 + yTile * 11 + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
				int fertilizer = soil.fertilizer;
				if (fertilizer != 368)
				{
					if (fertilizer == 369)
					{
						fertilizerQualityLevel = 2;
					}
				}
				else
				{
					fertilizerQualityLevel = 1;
				}
				double chanceForGoldQuality = 0.2 * (double)(Game1.player.FarmingLevel / 10) + 0.2 * (double)fertilizerQualityLevel * (double)((float)(Game1.player.FarmingLevel + 2) / 12f) + 0.01;
				double chanceForSilverQuality = Math.Min(0.75, chanceForGoldQuality * 2.0);
				if (r.NextDouble() < chanceForGoldQuality)
				{
					cropQuality = 2;
				}
				else if (r.NextDouble() < chanceForSilverQuality)
				{
					cropQuality = 1;
				}
				if (this.minHarvest > 1 || this.maxHarvest > 1)
				{
					numToHarvest = r.Next(this.minHarvest, Math.Min(this.minHarvest + 1, this.maxHarvest + 1 + Game1.player.FarmingLevel / this.maxHarvestIncreasePerFarmingLevel));
				}
				if (this.chanceForExtraCrops > 0.0)
				{
					while (r.NextDouble() < Math.Min(0.9, this.chanceForExtraCrops))
					{
						numToHarvest++;
					}
				}
				if (this.harvestMethod == 1)
				{
					if (junimoHarvester == null)
					{
						DelayedAction.playSoundAfterDelay("daggerswipe", 150);
					}
					if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
					{
						Game1.playSound("harvest");
					}
					if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
					{
						DelayedAction.playSoundAfterDelay("coin", 260);
					}
					for (int i = 0; i < numToHarvest; i++)
					{
						if (junimoHarvester != null)
						{
							junimoHarvester.tryToAddItemToHut(new Object(this.indexOfHarvest, 1, false, -1, cropQuality));
						}
						else
						{
							Game1.createObjectDebris(this.indexOfHarvest, xTile, yTile, -1, cropQuality, 1f, null);
						}
					}
					if (this.regrowAfterHarvest == -1)
					{
						return true;
					}
					this.dayOfCurrentPhase = this.regrowAfterHarvest;
					this.fullyGrown = true;
				}
				else
				{
					if (junimoHarvester == null)
					{
						Farmer arg_487_0 = Game1.player;
						Object arg_487_1;
						if (!this.programColored)
						{
							arg_487_1 = new Object(this.indexOfHarvest, 1, false, -1, cropQuality);
						}
						else
						{
							(arg_487_1 = new ColoredObject(this.indexOfHarvest, 1, this.tintColor)).quality = cropQuality;
						}
						if (!arg_487_0.addItemToInventoryBool(arg_487_1, false))
						{
							Game1.showRedMessage("Inventory Full");
							return false;
						}
					}
					Vector2 initialTile2 = new Vector2((float)xTile, (float)yTile);
					if (junimoHarvester == null)
					{
						Game1.player.animateOnce(279 + Game1.player.facingDirection);
						Game1.player.canMove = false;
					}
					else
					{
						Object arg_4FD_1;
						if (!this.programColored)
						{
							arg_4FD_1 = new Object(this.indexOfHarvest, 1, false, -1, cropQuality);
						}
						else
						{
							(arg_4FD_1 = new ColoredObject(this.indexOfHarvest, 1, this.tintColor)).quality = cropQuality;
						}
						junimoHarvester.tryToAddItemToHut(arg_4FD_1);
					}
					if (r.NextDouble() < (double)((float)Game1.player.LuckLevel / 1500f) + Game1.dailyLuck / 1200.0 + 9.9999997473787516E-05)
					{
						numToHarvest *= 2;
						if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
						{
							Game1.playSound("dwoop");
						}
					}
					else if (this.harvestMethod == 0)
					{
						if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
						{
							Game1.playSound("harvest");
						}
						if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
						{
							DelayedAction.playSoundAfterDelay("coin", 260);
						}
						if (this.regrowAfterHarvest == -1 && (junimoHarvester == null || junimoHarvester.currentLocation.Equals(Game1.currentLocation)))
						{
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(initialTile2.X * (float)Game1.tileSize, initialTile2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(initialTile2.X * (float)Game1.tileSize, initialTile2.Y * (float)Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
						}
					}
					if (this.indexOfHarvest == 421)
					{
						this.indexOfHarvest = 431;
						numToHarvest = r.Next(1, 4);
					}
					for (int j = 0; j < numToHarvest - 1; j++)
					{
						if (junimoHarvester == null)
						{
							Game1.createObjectDebris(this.indexOfHarvest, xTile, yTile, -1, 0, 1f, null);
						}
						else
						{
							junimoHarvester.tryToAddItemToHut(new Object(this.indexOfHarvest, 1, false, -1, 0));
						}
					}
					int price = Convert.ToInt32(Game1.objectInformation[this.indexOfHarvest].Split(new char[]
					{
						'/'
					})[1]);
					float experience2 = (float)(16.0 * Math.Log(0.018 * (double)price + 1.0, 2.7182818284590451));
					if (junimoHarvester == null)
					{
						Game1.player.gainExperience(0, (int)Math.Round((double)experience2));
					}
					if (this.regrowAfterHarvest == -1)
					{
						return true;
					}
					this.dayOfCurrentPhase = this.regrowAfterHarvest;
					this.fullyGrown = true;
				}
			}
			return false;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0003D574 File Offset: 0x0003B774
		public int getRandomWildCropForSeason(string season)
		{
			if (season == "spring")
			{
				return 16 + Game1.random.Next(4) * 2;
			}
			if (!(season == "summer"))
			{
				if (season == "fall")
				{
					return 404 + Game1.random.Next(4) * 2;
				}
				if (!(season == "winter"))
				{
					return 22;
				}
				return 412 + Game1.random.Next(4) * 2;
			}
			else
			{
				if (Game1.random.NextDouble() < 0.33)
				{
					return 396;
				}
				if (Game1.random.NextDouble() >= 0.5)
				{
					return 402;
				}
				return 398;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0003D630 File Offset: 0x0003B830
		private Rectangle getSourceRect(int number)
		{
			if (this.dead)
			{
				return new Rectangle(192 + number % 4 * 16, 384, 16, 32);
			}
			return new Rectangle(Math.Min(240, (this.fullyGrown ? ((this.dayOfCurrentPhase <= 0) ? 6 : 7) : (((this.phaseToShow != -1) ? this.phaseToShow : this.currentPhase) + ((((this.phaseToShow != -1) ? this.phaseToShow : this.currentPhase) == 0 && number % 2 == 0) ? -1 : 0) + 1)) * 16 + ((this.rowInSpriteSheet % 2 != 0) ? 128 : 0)), this.rowInSpriteSheet / 2 * 16 * 2, 16, 32);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0003D6EC File Offset: 0x0003B8EC
		public void newDay(int state, int fertilizer, int xTile, int yTile, GameLocation environment)
		{
			if (!environment.name.Equals("Greenhouse") && (this.dead || !this.seasonsToGrowIn.Contains(Game1.currentSeason)))
			{
				this.dead = true;
				return;
			}
			if (state == 1)
			{
				if (!this.fullyGrown)
				{
					this.dayOfCurrentPhase = Math.Min(this.dayOfCurrentPhase + 1, (this.phaseDays.Count > 0) ? this.phaseDays[Math.Min(this.phaseDays.Count - 1, this.currentPhase)] : 0);
				}
				else
				{
					this.dayOfCurrentPhase--;
				}
				if (this.dayOfCurrentPhase >= ((this.phaseDays.Count > 0) ? this.phaseDays[Math.Min(this.phaseDays.Count - 1, this.currentPhase)] : 0) && this.currentPhase < this.phaseDays.Count - 1)
				{
					this.currentPhase++;
					this.dayOfCurrentPhase = 0;
				}
				while (this.currentPhase < this.phaseDays.Count - 1 && this.phaseDays.Count > 0 && this.phaseDays[this.currentPhase] <= 0)
				{
					this.currentPhase++;
				}
				if (this.rowInSpriteSheet == 23 && this.phaseToShow == -1 && this.currentPhase > 0)
				{
					this.phaseToShow = Game1.random.Next(1, 7);
				}
				if (environment is Farm && this.currentPhase == this.phaseDays.Count - 1 && (this.indexOfHarvest == 276 || this.indexOfHarvest == 190 || this.indexOfHarvest == 254) && new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + xTile * 2000 + yTile).NextDouble() < 0.01)
				{
					for (int x = xTile - 1; x <= xTile + 1; x++)
					{
						for (int y = yTile - 1; y <= yTile + 1; y++)
						{
							Vector2 v = new Vector2((float)x, (float)y);
							if (!environment.terrainFeatures.ContainsKey(v) || !(environment.terrainFeatures[v] is HoeDirt) || (environment.terrainFeatures[v] as HoeDirt).crop == null || (environment.terrainFeatures[v] as HoeDirt).crop.indexOfHarvest != this.indexOfHarvest)
							{
								return;
							}
						}
					}
					for (int x2 = xTile - 1; x2 <= xTile + 1; x2++)
					{
						for (int y2 = yTile - 1; y2 <= yTile + 1; y2++)
						{
							Vector2 v2 = new Vector2((float)x2, (float)y2);
							(environment.terrainFeatures[v2] as HoeDirt).crop = null;
						}
					}
					(environment as Farm).resourceClumps.Add(new GiantCrop(this.indexOfHarvest, new Vector2((float)(xTile - 1), (float)(yTile - 1))));
				}
			}
			if ((!this.fullyGrown || this.dayOfCurrentPhase <= 0) && this.currentPhase >= this.phaseDays.Count - 1 && this.rowInSpriteSheet == 23)
			{
				Vector2 v3 = new Vector2((float)xTile, (float)yTile);
				environment.objects.Remove(v3);
				string season = Game1.currentSeason;
				switch (this.whichForageCrop)
				{
				case 495:
					season = "spring";
					break;
				case 496:
					season = "summer";
					break;
				case 497:
					season = "fall";
					break;
				case 498:
					season = "winter";
					break;
				}
				environment.objects.Add(v3, new Object(v3, this.getRandomWildCropForSeason(season), 1)
				{
					isSpawnedObject = true,
					canBeGrabbed = true
				});
				if (environment.terrainFeatures[v3] != null && environment.terrainFeatures[v3] is HoeDirt)
				{
					(environment.terrainFeatures[v3] as HoeDirt).crop = null;
				}
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0003DB08 File Offset: 0x0003BD08
		public bool isWildSeedCrop()
		{
			return this.rowInSpriteSheet == 23;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0003DB14 File Offset: 0x0003BD14
		public void draw(SpriteBatch b, Vector2 tileLocation, Color toTint, float rotation)
		{
			if (this.forageCrop)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((tileLocation.X * 11f + tileLocation.Y * 7f) % 10f - 5f) + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((int)(tileLocation.X * 51f + tileLocation.Y * 77f) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) / 10000f);
				return;
			}
			b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.X * 11f + tileLocation.Y * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2))), new Rectangle?(this.getSourceRect((int)tileLocation.X * 7 + (int)tileLocation.Y * 11)), toTint, rotation, new Vector2(8f, 24f), (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f))) / 10000f / ((this.currentPhase == 0 && !this.raisedSeeds) ? 2f : 1f));
			if (!this.tintColor.Equals(Color.White) && this.currentPhase == this.phaseDays.Count - 1 && !this.dead)
			{
				b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.X * 11f + tileLocation.Y * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + ((this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1) ? 0f : ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) + (float)(Game1.tileSize / 2))), new Rectangle?(new Rectangle((this.fullyGrown ? ((this.dayOfCurrentPhase <= 0) ? 6 : 7) : (this.currentPhase + 1 + 1)) * 16 + ((this.rowInSpriteSheet % 2 != 0) ? 128 : 0), this.rowInSpriteSheet / 2 * 16 * 2, 16, 32)), this.tintColor, rotation, new Vector2(8f, 24f), (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((tileLocation.Y * 11f + tileLocation.X * 7f) % 10f - 5f)) / 10000f / (float)((this.currentPhase == 0 && !this.raisedSeeds) ? 2 : 1));
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0003DFE4 File Offset: 0x0003C1E4
		public void drawInMenu(SpriteBatch b, Vector2 screenPosition, Color toTint, float rotation, float scale, float layerDepth)
		{
			b.Draw(Game1.cropSpriteSheet, screenPosition, new Rectangle?(this.getSourceRect(0)), toTint, rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + Game1.tileSize / 2)), scale, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}

		// Token: 0x04000362 RID: 866
		public const int mixedSeedIndex = 770;

		// Token: 0x04000363 RID: 867
		public const int seedPhase = 0;

		// Token: 0x04000364 RID: 868
		public const int grabHarvest = 0;

		// Token: 0x04000365 RID: 869
		public const int sickleHarvest = 1;

		// Token: 0x04000366 RID: 870
		public const int rowOfWildSeeds = 23;

		// Token: 0x04000367 RID: 871
		public const int finalPhaseLength = 99999;

		// Token: 0x04000368 RID: 872
		public const int forageCrop_springOnion = 1;

		// Token: 0x04000369 RID: 873
		public List<int> phaseDays = new List<int>();

		// Token: 0x0400036A RID: 874
		public int rowInSpriteSheet;

		// Token: 0x0400036B RID: 875
		public int phaseToShow = -1;

		// Token: 0x0400036C RID: 876
		public int currentPhase;

		// Token: 0x0400036D RID: 877
		public int harvestMethod;

		// Token: 0x0400036E RID: 878
		public int indexOfHarvest;

		// Token: 0x0400036F RID: 879
		public int regrowAfterHarvest;

		// Token: 0x04000370 RID: 880
		public int dayOfCurrentPhase;

		// Token: 0x04000371 RID: 881
		public int minHarvest;

		// Token: 0x04000372 RID: 882
		public int maxHarvest;

		// Token: 0x04000373 RID: 883
		public int maxHarvestIncreasePerFarmingLevel;

		// Token: 0x04000374 RID: 884
		public int daysOfUnclutteredGrowth;

		// Token: 0x04000375 RID: 885
		public int whichForageCrop;

		// Token: 0x04000376 RID: 886
		public List<string> seasonsToGrowIn = new List<string>();

		// Token: 0x04000377 RID: 887
		public Color tintColor;

		// Token: 0x04000378 RID: 888
		public bool flip;

		// Token: 0x04000379 RID: 889
		public bool fullyGrown;

		// Token: 0x0400037A RID: 890
		public bool raisedSeeds;

		// Token: 0x0400037B RID: 891
		public bool programColored;

		// Token: 0x0400037C RID: 892
		public bool dead;

		// Token: 0x0400037D RID: 893
		public bool forageCrop;

		// Token: 0x0400037E RID: 894
		public double chanceForExtraCrops;
	}
}
