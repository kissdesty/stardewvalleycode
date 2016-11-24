using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;
using xTile.Dimensions;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200007C RID: 124
	public class Tree : TerrainFeature
	{
		// Token: 0x060009F5 RID: 2549 RVA: 0x000D24AA File Offset: 0x000D06AA
		public Tree()
		{
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000D24C8 File Offset: 0x000D06C8
		public Tree(int which, int growthStage)
		{
			this.growthStage = growthStage;
			this.treeType = which;
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.health = 10f;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000D252C File Offset: 0x000D072C
		public Tree(int which)
		{
			this.treeType = which;
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.health = 10f;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x000D2588 File Offset: 0x000D0788
		public override void loadSprite()
		{
			try
			{
				if (this.treeType == 7)
				{
					this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\mushroom_tree");
				}
				else if (this.treeType == 6)
				{
					this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\tree_palm");
				}
				else
				{
					if (this.treeType == 4)
					{
						this.treeType = 1;
					}
					if (this.treeType == 5)
					{
						this.treeType = 2;
					}
					string season = Game1.currentSeason;
					if (this.treeType == 3 && season.Equals("summer"))
					{
						season = "spring";
					}
					this.texture = Game1.content.Load<Texture2D>(string.Concat(new object[]
					{
						"TerrainFeatures\\tree",
						Math.Max(1, this.treeType),
						"_",
						season
					}));
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000D2678 File Offset: 0x000D0878
		public override Microsoft.Xna.Framework.Rectangle getBoundingBox(Vector2 tileLocation)
		{
			switch (this.growthStage)
			{
			case 0:
			case 1:
			case 2:
				return new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize + Game1.tileSize / 5, (int)tileLocation.Y * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 3 / 5, Game1.tileSize * 3 / 4);
			}
			return new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x000D2710 File Offset: 0x000D0910
		public override bool performUseAction(Vector2 tileLocation)
		{
			if (!this.tapped)
			{
				if (this.maxShake == 0f && !this.stump && this.growthStage >= 3 && (!Game1.currentSeason.Equals("winter") || this.treeType == 3))
				{
					Game1.playSound("leafrustle");
				}
				this.shake(tileLocation, false);
			}
			return false;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x000D2770 File Offset: 0x000D0970
		private int extraWoodCalculator(Vector2 tileLocation)
		{
			Random arg_2D_0 = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
			int extraWood = 0;
			if (arg_2D_0.NextDouble() < Game1.dailyLuck)
			{
				extraWood++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.ForagingLevel / 12.5)
			{
				extraWood++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.ForagingLevel / 12.5)
			{
				extraWood++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.LuckLevel / 25.0)
			{
				extraWood++;
			}
			return extraWood;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x000D2820 File Offset: 0x000D0A20
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			if (this.destroy)
			{
				return true;
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
			if (this.growthStage >= 5 && !this.falling && !this.stump && Game1.player.GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(Game1.tileSize * ((int)tileLocation.X - 1), Game1.tileSize * ((int)tileLocation.Y - 5), 3 * Game1.tileSize, 4 * Game1.tileSize + Game1.tileSize / 2)))
			{
				this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
			}
			if (!this.falling)
			{
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966 && this.leaves.Count <= 0 && this.health <= 0f)
				{
					return true;
				}
				if (this.maxShake > 0f)
				{
					if (this.shakeLeft)
					{
						this.shakeRotation -= ((this.growthStage >= 5) ? 0.005235988f : 0.0157079641f);
						if (this.shakeRotation <= -this.maxShake)
						{
							this.shakeLeft = false;
						}
					}
					else
					{
						this.shakeRotation += ((this.growthStage >= 5) ? 0.005235988f : 0.0157079641f);
						if (this.shakeRotation >= this.maxShake)
						{
							this.shakeLeft = true;
						}
					}
				}
				if (this.maxShake > 0f)
				{
					this.maxShake = Math.Max(0f, this.maxShake - ((this.growthStage >= 5) ? 0.00102265389f : 0.00306796166f));
				}
			}
			else
			{
				this.shakeRotation += (this.shakeLeft ? (-(this.maxShake * this.maxShake)) : (this.maxShake * this.maxShake));
				this.maxShake += 0.00153398083f;
				if (Game1.random.NextDouble() < 0.01 && this.treeType != 7)
				{
					Game1.playSound("leafrustle");
				}
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966)
				{
					this.falling = false;
					this.maxShake = 0f;
					Game1.playSound("treethud");
					int leavesToAdd = Game1.random.Next(90, 120);
					if (Game1.currentLocation.Objects.ContainsKey(tileLocation))
					{
						Game1.currentLocation.Objects.Remove(tileLocation);
					}
					for (int i = 0; i < leavesToAdd; i++)
					{
						this.leaves.Add(new Leaf(new Vector2((float)(Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3))) + (this.shakeLeft ? (-Game1.tileSize * 5) : (Game1.tileSize * 4))), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(10, 40) / 10f));
					}
					if (this.treeType != 7)
					{
						Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 12 + this.extraWoodCalculator(tileLocation), true, -1, false, -1);
						Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 12 + this.extraWoodCalculator(tileLocation), false, -1, false, -1);
						Random r;
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
							r = Game1.recentMultiplayerRandom;
						}
						else
						{
							r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(92, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 5, this.lastPlayerToHit);
							int numHardwood = 0;
							if (Game1.getFarmer(this.lastPlayerToHit) != null)
							{
								while (Game1.getFarmer(this.lastPlayerToHit).professions.Contains(14) && r.NextDouble() < 0.4)
								{
									numHardwood++;
								}
							}
							if (numHardwood > 0)
							{
								Game1.createMultipleObjectDebris(709, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, numHardwood, this.lastPlayerToHit);
							}
							if (Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1 && r.NextDouble() < 0.75)
							{
								Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, r.Next(1, 3), this.lastPlayerToHit);
							}
						}
						else
						{
							Game1.createMultipleObjectDebris(92, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 5);
							int numHardwood2 = 0;
							if (Game1.getFarmer(this.lastPlayerToHit) != null)
							{
								while (Game1.getFarmer(this.lastPlayerToHit).professions.Contains(14) && r.NextDouble() < 0.4)
								{
									numHardwood2++;
								}
							}
							if (numHardwood2 > 0)
							{
								Game1.createMultipleObjectDebris(709, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, numHardwood2);
							}
							if (this.lastPlayerToHit != 0L && Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1 && r.NextDouble() < 0.75 && this.treeType < 4)
							{
								Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, r.Next(1, 3));
							}
						}
					}
					else if (!Game1.IsMultiplayer)
					{
						Game1.createMultipleObjectDebris(420, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 5);
					}
					if (this.health == -100f)
					{
						return true;
					}
					if (this.health <= 0f)
					{
						this.health = -100f;
					}
				}
			}
			for (int j = this.leaves.Count - 1; j >= 0; j--)
			{
				Leaf expr_6D9_cp_0_cp_0 = this.leaves.ElementAt(j);
				expr_6D9_cp_0_cp_0.position.Y = expr_6D9_cp_0_cp_0.position.Y - (this.leaves.ElementAt(j).yVelocity - 3f);
				this.leaves.ElementAt(j).yVelocity = Math.Max(0f, this.leaves.ElementAt(j).yVelocity - 0.01f);
				this.leaves.ElementAt(j).rotation += this.leaves.ElementAt(j).rotationRate;
				if (this.leaves.ElementAt(j).position.Y >= tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)
				{
					this.leaves.RemoveAt(j);
				}
			}
			return false;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x000D2FCC File Offset: 0x000D11CC
		private void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
		{
			if ((this.maxShake == 0f | doEvenIfStillShaking) && this.growthStage >= 3 && !this.stump)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = (float)((this.growthStage >= 5) ? 0.024543692606170259 : 0.049087385212340517);
				if (this.growthStage >= 5)
				{
					if (Game1.random.NextDouble() < 0.66)
					{
						int numberOfLeaves = Game1.random.Next(1, 6);
						for (int i = 0; i < numberOfLeaves; i++)
						{
							this.leaves.Add(new Leaf(new Vector2((float)Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize - (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 2))), (float)Game1.random.Next((int)(tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 4)), (int)(tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3)))), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(5) / 10f));
						}
					}
					if (Game1.random.NextDouble() < 0.01)
					{
						if (!Game1.currentSeason.Equals("spring"))
						{
							if (!Game1.currentSeason.Equals("summer"))
							{
								goto IL_229;
							}
						}
						while (Game1.random.NextDouble() < 0.8)
						{
							Game1.currentLocation.addCritter(new Butterfly(new Vector2(tileLocation.X + (float)Game1.random.Next(1, 3), tileLocation.Y - 2f + (float)Game1.random.Next(-1, 2))));
						}
					}
					IL_229:
					if (this.hasSeed && (Game1.IsMultiplayer || Game1.player.ForagingLevel >= 1))
					{
						int seedIndex = -1;
						switch (this.treeType)
						{
						case 1:
							seedIndex = 309;
							break;
						case 2:
							seedIndex = 310;
							break;
						case 3:
							seedIndex = 311;
							break;
						case 6:
							seedIndex = 88;
							break;
						}
						if (Game1.currentSeason.Equals("fall") && this.treeType == 2 && Game1.dayOfMonth >= 14)
						{
							seedIndex = 408;
						}
						if (seedIndex != -1)
						{
							Game1.createObjectDebris(seedIndex, (int)tileLocation.X, (int)tileLocation.Y - 3, ((int)tileLocation.Y + 1) * Game1.tileSize, 0, 1f, null);
						}
						this.hasSeed = false;
						return;
					}
				}
				else if (Game1.random.NextDouble() < 0.66)
				{
					int numberOfLeaves2 = Game1.random.Next(1, 3);
					for (int j = 0; j < numberOfLeaves2; j++)
					{
						this.leaves.Add(new Leaf(new Vector2((float)Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2)), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(30) / 10f));
					}
					return;
				}
			}
			else if (this.stump)
			{
				this.shakeTimer = 100f;
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x000D33AC File Offset: 0x000D15AC
		public override bool isPassable(Character c = null)
		{
			return this.health <= -99f || this.growthStage == 0;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x000D33C8 File Offset: 0x000D15C8
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
			Microsoft.Xna.Framework.Rectangle growthRect = new Microsoft.Xna.Framework.Rectangle((int)((tileLocation.X - 1f) * (float)Game1.tileSize), (int)((tileLocation.Y - 1f) * (float)Game1.tileSize), Game1.tileSize * 3, Game1.tileSize * 3);
			if (this.health <= -100f)
			{
				this.destroy = true;
			}
			if (!Game1.currentSeason.Equals("winter") || this.treeType == 6 || environment.Name.ToLower().Contains("greenhouse"))
			{
				string s = environment.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "NoSpawn", "Back");
				if (s != null && (s.Equals("All") || s.Equals("Tree") || s.Equals("True")))
				{
					return;
				}
				if (this.growthStage == 4)
				{
					using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = environment.terrainFeatures.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Vector2, TerrainFeature> t = enumerator.Current;
							if (t.Value is Tree && !t.Value.Equals(this) && ((Tree)t.Value).growthStage >= 5 && t.Value.getBoundingBox(t.Key).Intersects(growthRect))
							{
								return;
							}
						}
						goto IL_176;
					}
				}
				if (this.growthStage == 0 && environment.objects.ContainsKey(tileLocation))
				{
					return;
				}
				IL_176:
				if (Game1.random.NextDouble() < 0.2)
				{
					this.growthStage++;
				}
			}
			if (Game1.currentSeason.Equals("winter") && this.treeType == 7)
			{
				this.stump = true;
			}
			else if (this.treeType == 7 && Game1.dayOfMonth <= 1 && Game1.currentSeason.Equals("spring"))
			{
				this.stump = false;
				this.health = 10f;
			}
			if (this.growthStage >= 5 && environment is Farm && Game1.random.NextDouble() < 0.15)
			{
				int xCoord = Game1.random.Next(-3, 4) + (int)tileLocation.X;
				int yCoord = Game1.random.Next(-3, 4) + (int)tileLocation.Y;
				Vector2 location = new Vector2((float)xCoord, (float)yCoord);
				string noSpawn = environment.doesTileHaveProperty(xCoord, yCoord, "NoSpawn", "Back");
				if ((noSpawn == null || (!noSpawn.Equals("Tree") && !noSpawn.Equals("All") && !noSpawn.Equals("True"))) && environment.isTileLocationOpen(new Location(xCoord * Game1.tileSize, yCoord * Game1.tileSize)) && !environment.isTileOccupied(location, "") && environment.doesTileHaveProperty(xCoord, yCoord, "Water", "Back") == null && environment.isTileOnMap(location))
				{
					environment.terrainFeatures.Add(location, new Tree(this.treeType, 0));
				}
			}
			this.hasSeed = false;
			if (this.growthStage >= 5 && Game1.random.NextDouble() < 0.05000000074505806)
			{
				this.hasSeed = true;
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x000D3718 File Offset: 0x000D1918
		public override bool seasonUpdate(bool onLoad)
		{
			this.loadSprite();
			return false;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x000D3721 File Offset: 0x000D1921
		public override bool isActionable()
		{
			return !this.tapped && this.growthStage >= 3;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000D373C File Offset: 0x000D193C
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if (explosion > 0)
			{
				this.tapped = false;
			}
			if (this.tapped)
			{
				return false;
			}
			Console.WriteLine(string.Concat(new object[]
			{
				"TREE: IsClient:",
				Game1.IsClient.ToString(),
				" randomOutput: ",
				Game1.recentMultiplayerRandom.Next(9999)
			}));
			if (this.health <= -99f)
			{
				return false;
			}
			if (this.growthStage >= 5)
			{
				if (t != null && t is Axe)
				{
					Game1.playSound("axchop");
					location.debris.Add(new Debris(12, Game1.random.Next(1, 3), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f), t.getLastFarmerToUse().position, 0, -1));
					this.lastPlayerToHit = t.getLastFarmerToUse().uniqueMultiplayerID;
				}
				else if (explosion <= 0)
				{
					return false;
				}
				this.shake(tileLocation, true);
				float damage = 1f;
				if (explosion > 0)
				{
					damage = (float)explosion;
				}
				else
				{
					if (t == null)
					{
						return false;
					}
					switch (t.upgradeLevel)
					{
					case 0:
						damage = 1f;
						break;
					case 1:
						damage = 1.25f;
						break;
					case 2:
						damage = 1.67f;
						break;
					case 3:
						damage = 2.5f;
						break;
					case 4:
						damage = 5f;
						break;
					}
				}
				this.health -= damage;
				if (this.health <= 0f)
				{
					if (!this.stump)
					{
						if ((t != null || explosion > 0) && location.Equals(Game1.currentLocation))
						{
							Game1.playSound("treecrack");
						}
						this.stump = true;
						this.health = 5f;
						this.falling = true;
						if (t != null)
						{
							t.getLastFarmerToUse().gainExperience(2, 12);
						}
						if (t == null || t.getLastFarmerToUse() == null)
						{
							this.shakeLeft = true;
						}
						else
						{
							this.shakeLeft = (t.getLastFarmerToUse().getTileLocation().X > tileLocation.X || (t.getLastFarmerToUse().getTileLocation().Y < tileLocation.Y && tileLocation.X % 2f == 0f));
						}
					}
					else
					{
						this.health = -100f;
						Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(30, 40), false, -1, false, -1);
						int whatToDrop = (this.treeType == 7 && tileLocation.X % 7f == 0f) ? 422 : ((this.treeType == 7) ? 420 : 92);
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 2000 + (int)tileLocation.Y);
							Random arg_2D6_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (t == null || t.getLastFarmerToUse() == null)
						{
							if (location.Equals(Game1.currentLocation))
							{
								Game1.createMultipleObjectDebris(92, (int)tileLocation.X, (int)tileLocation.Y, 2);
							}
							else
							{
								Game1.createItemDebris(new Object(92, 1, false, -1, 0), tileLocation * (float)Game1.tileSize, 2, location);
								Game1.createItemDebris(new Object(92, 1, false, -1, 0), tileLocation * (float)Game1.tileSize, 2, location);
							}
						}
						else if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(whatToDrop, (int)tileLocation.X, (int)tileLocation.Y, 1, this.lastPlayerToHit);
							if (this.treeType != 7)
							{
								Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, 4, true, -1, false, -1);
							}
						}
						else
						{
							if (this.treeType != 7)
							{
								Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, 5 + this.extraWoodCalculator(tileLocation), true, -1, false, -1);
							}
							Game1.createMultipleObjectDebris(whatToDrop, (int)tileLocation.X, (int)tileLocation.Y, 1);
						}
						if (location.Equals(Game1.currentLocation))
						{
							Game1.playSound("treethud");
						}
						if (!this.falling)
						{
							return true;
						}
					}
				}
			}
			else if (this.growthStage >= 3)
			{
				if (t != null && t.name.Contains("Ax"))
				{
					Game1.playSound("axchop");
					if (this.treeType != 7)
					{
						Game1.playSound("leafrustle");
					}
					location.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f), new Vector2((float)t.getLastFarmerToUse().GetBoundingBox().Center.X, (float)t.getLastFarmerToUse().GetBoundingBox().Center.Y), 0, -1));
				}
				else if (explosion <= 0)
				{
					return false;
				}
				this.shake(tileLocation, true);
				float damage2 = 1f;
				if (Game1.IsMultiplayer)
				{
					Random arg_51E_0 = Game1.recentMultiplayerRandom;
				}
				else
				{
					new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + Game1.stats.DaysPlayed + this.health));
				}
				if (explosion > 0)
				{
					damage2 = (float)explosion;
				}
				else
				{
					switch (t.upgradeLevel)
					{
					case 0:
						damage2 = 2f;
						break;
					case 1:
						damage2 = 2.5f;
						break;
					case 2:
						damage2 = 3.34f;
						break;
					case 3:
						damage2 = 5f;
						break;
					case 4:
						damage2 = 10f;
						break;
					}
				}
				this.health -= damage2;
				if (this.health <= 0f)
				{
					Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, 4, null);
					Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(20, 30), false, -1, false, -1);
					return true;
				}
			}
			else if (this.growthStage >= 1)
			{
				if (explosion > 0)
				{
					return true;
				}
				if (location.Equals(Game1.currentLocation))
				{
					Game1.playSound("cut");
				}
				if (t != null && t.name.Contains("Axe"))
				{
					Game1.playSound("axchop");
					Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
				}
				if (t is Axe || t is Pickaxe || t is Hoe || t is MeleeWeapon)
				{
					Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
					if (t.name.Contains("Axe") && Game1.recentMultiplayerRandom.NextDouble() < (double)((float)t.getLastFarmerToUse().ForagingLevel / 10f))
					{
						Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, 1, null);
					}
					location.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					return true;
				}
			}
			else
			{
				if (explosion > 0)
				{
					return true;
				}
				if (t.name.Contains("Axe") || t.name.Contains("Pick") || t.name.Contains("Hoe"))
				{
					Game1.playSound("woodyHit");
					Game1.playSound("axchop");
					location.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					if (this.lastPlayerToHit != 0L && Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1)
					{
						Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X, (int)tileLocation.Y, 1, t.getLastFarmerToUse().uniqueMultiplayerID, location);
					}
					else if (!Game1.IsMultiplayer && Game1.player.getEffectiveSkillLevel(2) >= 1)
					{
						Game1.createMultipleObjectDebris(308 + this.treeType, (int)tileLocation.X, (int)tileLocation.Y, 1, t.getLastFarmerToUse().uniqueMultiplayerID, location);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000D3FB4 File Offset: 0x000D21B4
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			layerDepth += positionOnScreen.X / 100000f;
			if (this.growthStage < 5)
			{
				Microsoft.Xna.Framework.Rectangle sourceRect = Microsoft.Xna.Framework.Rectangle.Empty;
				switch (this.growthStage)
				{
				case 0:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16);
					break;
				case 1:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16);
					break;
				case 2:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16);
					break;
				default:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32);
					break;
				}
				spriteBatch.Draw(this.texture, positionOnScreen - new Vector2(0f, (float)sourceRect.Height * scale), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)sourceRect.Height * scale) / 20000f);
				return;
			}
			if (!this.falling)
			{
				spriteBatch.Draw(this.texture, positionOnScreen + new Vector2(0f, (float)(-(float)Game1.tileSize) * scale), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32)), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale - 1f) / 20000f);
			}
			if (!this.stump || this.falling)
			{
				spriteBatch.Draw(this.texture, positionOnScreen + new Vector2((float)(-(float)Game1.tileSize) * scale, (float)(-5 * Game1.tileSize) * scale), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96)), Color.White, this.shakeRotation, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale) / 20000f);
			}
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000D41B0 File Offset: 0x000D23B0
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.growthStage < 5)
			{
				Microsoft.Xna.Framework.Rectangle sourceRect = Microsoft.Xna.Framework.Rectangle.Empty;
				switch (this.growthStage)
				{
				case 0:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16);
					break;
				case 1:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16);
					break;
				case 2:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16);
					break;
				default:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32);
					break;
				}
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)(sourceRect.Height * Game1.pixelZoom - Game1.tileSize) + (float)((this.growthStage >= 3) ? (Game1.tileSize * 2) : Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White, this.shakeRotation, new Vector2(8f, (float)((this.growthStage >= 3) ? 32 : 16)), (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.growthStage == 0) ? 0.0001f : ((float)this.getBoundingBox(tileLocation).Bottom / 10000f));
			}
			else
			{
				if (!this.stump || this.falling)
				{
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize - (float)(Game1.tileSize * 4 / 5), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 4))), new Microsoft.Xna.Framework.Rectangle?(Tree.shadowSourceRect), Color.White * (1.57079637f - Math.Abs(this.shakeRotation)), 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Tree.treeTopSourceRect), Color.White * this.alpha, this.shakeRotation, new Vector2(24f, 96f), (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.getBoundingBox(tileLocation).Bottom + 2) / 10000f - tileLocation.X / 1000000f);
				}
				if (this.health >= 1f || (!this.falling && this.health > -99f))
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.shakeTimer > 0f) ? ((float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Tree.stumpSourceRect), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f);
				}
				if (this.stump && this.health < 4f && this.health > -99f)
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + ((this.shakeTimer > 0f) ? ((float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Math.Min(2, (int)(3f - this.health)) * 16, 144, 16, 16)), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.getBoundingBox(tileLocation).Bottom + 1) / 10000f);
				}
			}
			foreach (Leaf i in this.leaves)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, i.position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(16 + i.type % 2 * 8, 112 + i.type / 2 * 8, 8, 8)), Color.White, i.rotation, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.01f);
			}
		}

		// Token: 0x04000A34 RID: 2612
		public const float chanceForDailySeed = 0.05f;

		// Token: 0x04000A35 RID: 2613
		public const float shakeRate = 0.0157079641f;

		// Token: 0x04000A36 RID: 2614
		public const float shakeDecayRate = 0.00306796166f;

		// Token: 0x04000A37 RID: 2615
		public const int minWoodDebrisForFallenTree = 12;

		// Token: 0x04000A38 RID: 2616
		public const int minWoodDebrisForStump = 5;

		// Token: 0x04000A39 RID: 2617
		public const int startingHealth = 10;

		// Token: 0x04000A3A RID: 2618
		public const int leafFallRate = 3;

		// Token: 0x04000A3B RID: 2619
		public const int bushyTree = 1;

		// Token: 0x04000A3C RID: 2620
		public const int leafyTree = 2;

		// Token: 0x04000A3D RID: 2621
		public const int pineTree = 3;

		// Token: 0x04000A3E RID: 2622
		public const int winterTree1 = 4;

		// Token: 0x04000A3F RID: 2623
		public const int winterTree2 = 5;

		// Token: 0x04000A40 RID: 2624
		public const int palmTree = 6;

		// Token: 0x04000A41 RID: 2625
		public const int mushroomTree = 7;

		// Token: 0x04000A42 RID: 2626
		public const int seedStage = 0;

		// Token: 0x04000A43 RID: 2627
		public const int sproutStage = 1;

		// Token: 0x04000A44 RID: 2628
		public const int saplingStage = 2;

		// Token: 0x04000A45 RID: 2629
		public const int bushStage = 3;

		// Token: 0x04000A46 RID: 2630
		public const int treeStage = 5;

		// Token: 0x04000A47 RID: 2631
		private Texture2D texture;

		// Token: 0x04000A48 RID: 2632
		public int growthStage;

		// Token: 0x04000A49 RID: 2633
		public int treeType;

		// Token: 0x04000A4A RID: 2634
		public float health;

		// Token: 0x04000A4B RID: 2635
		public bool flipped;

		// Token: 0x04000A4C RID: 2636
		public bool stump;

		// Token: 0x04000A4D RID: 2637
		public bool tapped;

		// Token: 0x04000A4E RID: 2638
		public bool hasSeed;

		// Token: 0x04000A4F RID: 2639
		private bool shakeLeft;

		// Token: 0x04000A50 RID: 2640
		private bool falling;

		// Token: 0x04000A51 RID: 2641
		private bool destroy;

		// Token: 0x04000A52 RID: 2642
		private float shakeRotation;

		// Token: 0x04000A53 RID: 2643
		private float maxShake;

		// Token: 0x04000A54 RID: 2644
		private float alpha = 1f;

		// Token: 0x04000A55 RID: 2645
		private List<Leaf> leaves = new List<Leaf>();

		// Token: 0x04000A56 RID: 2646
		private long lastPlayerToHit;

		// Token: 0x04000A57 RID: 2647
		private float shakeTimer;

		// Token: 0x04000A58 RID: 2648
		public static Microsoft.Xna.Framework.Rectangle treeTopSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96);

		// Token: 0x04000A59 RID: 2649
		public static Microsoft.Xna.Framework.Rectangle stumpSourceRect = new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32);

		// Token: 0x04000A5A RID: 2650
		public static Microsoft.Xna.Framework.Rectangle shadowSourceRect = new Microsoft.Xna.Framework.Rectangle(663, 1011, 41, 30);
	}
}
