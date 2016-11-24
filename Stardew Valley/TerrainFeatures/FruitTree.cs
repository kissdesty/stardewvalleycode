using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000070 RID: 112
	public class FruitTree : TerrainFeature
	{
		// Token: 0x0600097C RID: 2428 RVA: 0x000CB3A2 File Offset: 0x000C95A2
		public FruitTree()
		{
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000CB3C8 File Offset: 0x000C95C8
		public FruitTree(int saplingIndex, int growthStage)
		{
			this.growthStage = growthStage;
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.health = 10f;
			this.loadData(saplingIndex);
			this.daysUntilMature = 28;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x000CB43C File Offset: 0x000C963C
		public FruitTree(int saplingIndex)
		{
			this.loadSprite();
			this.flipped = (Game1.random.NextDouble() < 0.5);
			this.health = 10f;
			this.loadData(saplingIndex);
			this.daysUntilMature = 28;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x000CB4A8 File Offset: 0x000C96A8
		private void loadData(int saplingIndex)
		{
			Dictionary<int, string> data = Game1.content.Load<Dictionary<int, string>>("Data\\fruitTrees");
			if (data.ContainsKey(saplingIndex))
			{
				string[] rawData = data[saplingIndex].Split(new char[]
				{
					'/'
				});
				this.treeType = Convert.ToInt32(rawData[0]);
				this.fruitSeason = rawData[1];
				this.indexOfFruit = Convert.ToInt32(rawData[2]);
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x000CB50C File Offset: 0x000C970C
		public override void loadSprite()
		{
			try
			{
				if (FruitTree.texture == null)
				{
					FruitTree.texture = Game1.content.Load<Texture2D>("TileSheets\\fruitTrees");
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool isActionable()
		{
			return true;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000CB54C File Offset: 0x000C974C
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			switch (this.growthStage)
			{
			case 0:
			case 1:
			case 2:
				return new Rectangle((int)tileLocation.X * Game1.tileSize + Game1.tileSize / 5, (int)tileLocation.Y * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 3 / 5, Game1.tileSize * 3 / 4);
			}
			return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x000CB5E4 File Offset: 0x000C97E4
		public override bool performUseAction(Vector2 tileLocation)
		{
			if (this.maxShake == 0f && !this.stump && this.growthStage >= 3 && (!Game1.currentSeason.Equals("winter") || Game1.currentLocation.name.ToLower().Contains("greenhouse")))
			{
				Game1.playSound("leafrustle");
			}
			this.shake(tileLocation, false);
			return true;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000CB650 File Offset: 0x000C9850
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.destroy)
			{
				return true;
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			if (this.growthStage >= 4 && !this.falling && !this.stump && Game1.player.GetBoundingBox().Intersects(new Rectangle(Game1.tileSize * ((int)tileLocation.X - 1), Game1.tileSize * ((int)tileLocation.Y - 4), 3 * Game1.tileSize, 3 * Game1.tileSize + Game1.tileSize / 2)))
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
						this.shakeRotation -= ((this.growthStage >= 4) ? 0.005235988f : 0.0157079641f);
						if (this.shakeRotation <= -this.maxShake)
						{
							this.shakeLeft = false;
						}
					}
					else
					{
						this.shakeRotation += ((this.growthStage >= 4) ? 0.005235988f : 0.0157079641f);
						if (this.shakeRotation >= this.maxShake)
						{
							this.shakeLeft = true;
						}
					}
				}
				if (this.maxShake > 0f)
				{
					this.maxShake = Math.Max(0f, this.maxShake - ((this.growthStage >= 4) ? 0.00102265389f : 0.00306796166f));
				}
				if (this.struckByLightningCountdown > 0 && Game1.random.NextDouble() < 0.01)
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(372, 1956, 10, 10), new Vector2(tileLocation.X * (float)Game1.tileSize + (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize * 3 / 2), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3) + (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize * 2)), false, 0.002f, Color.Gray)
					{
						alpha = 0.75f,
						motion = new Vector2(0f, -0.5f),
						interval = 99999f,
						layerDepth = 1f,
						scale = (float)(Game1.pixelZoom / 2),
						scaleChange = 0.01f
					});
				}
			}
			else
			{
				this.shakeRotation += (this.shakeLeft ? (-(this.maxShake * this.maxShake)) : (this.maxShake * this.maxShake));
				this.maxShake += 0.00153398083f;
				if (Game1.random.NextDouble() < 0.01 && !Game1.currentSeason.Equals("winter"))
				{
					Game1.playSound("leafrustle");
				}
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966)
				{
					this.falling = false;
					this.maxShake = 0f;
					Game1.playSound("treethud");
					int leavesToAdd = Game1.random.Next(90, 120);
					for (int i = 0; i < leavesToAdd; i++)
					{
						this.leaves.Add(new Leaf(new Vector2((float)(Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3))) + (this.shakeLeft ? (-Game1.tileSize * 5) : (Game1.tileSize * 4))), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(10, 40) / 10f));
					}
					Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 12, true, -1, false, -1);
					Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 12, false, -1, false, -1);
					if (Game1.IsMultiplayer)
					{
						Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
						Random arg_4D4_0 = Game1.recentMultiplayerRandom;
					}
					else
					{
						new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
					}
					if (Game1.IsMultiplayer)
					{
						Game1.createMultipleObjectDebris(92, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 10, this.lastPlayerToHit);
					}
					else
					{
						Game1.createMultipleObjectDebris(92, (int)tileLocation.X + (this.shakeLeft ? -4 : 4), (int)tileLocation.Y, 10);
					}
					if (this.health <= 0f)
					{
						this.health = -100f;
					}
				}
			}
			for (int j = this.leaves.Count - 1; j >= 0; j--)
			{
				Leaf expr_59F_cp_0_cp_0 = this.leaves.ElementAt(j);
				expr_59F_cp_0_cp_0.position.Y = expr_59F_cp_0_cp_0.position.Y - (this.leaves.ElementAt(j).yVelocity - 3f);
				this.leaves.ElementAt(j).yVelocity = Math.Max(0f, this.leaves.ElementAt(j).yVelocity - 0.01f);
				this.leaves.ElementAt(j).rotation += this.leaves.ElementAt(j).rotationRate;
				if (this.leaves.ElementAt(j).position.Y >= tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)
				{
					this.leaves.RemoveAt(j);
				}
			}
			return false;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000CBCC0 File Offset: 0x000C9EC0
		public void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
		{
			if ((this.maxShake == 0f | doEvenIfStillShaking) && this.growthStage >= 3 && !this.stump)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = (float)((this.growthStage >= 4) ? 0.024543692606170259 : 0.049087385212340517);
				if (this.growthStage >= 4)
				{
					if (Game1.random.NextDouble() < 0.66)
					{
						int numberOfLeaves = Game1.random.Next(1, 6);
						for (int i = 0; i < numberOfLeaves; i++)
						{
							this.leaves.Add(new Leaf(new Vector2((float)Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize - (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 2))), (float)Game1.random.Next((int)(tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 4)), (int)(tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3)))), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(5) / 10f));
						}
					}
					int fruitquality = 0;
					if (this.daysUntilMature <= -112)
					{
						fruitquality = 1;
					}
					if (this.daysUntilMature <= -224)
					{
						fruitquality = 2;
					}
					if (this.daysUntilMature <= -336)
					{
						fruitquality = 4;
					}
					if (this.struckByLightningCountdown > 0)
					{
						fruitquality = 0;
					}
					if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) && Game1.currentLocation.terrainFeatures[tileLocation].Equals(this))
					{
						for (int j = 0; j < this.fruitsOnTree; j++)
						{
							Vector2 offset = new Vector2(0f, 0f);
							switch (j)
							{
							case 0:
								offset.X = (float)(-(float)Game1.tileSize);
								break;
							case 1:
								offset.X = (float)Game1.tileSize;
								offset.Y = (float)(-(float)Game1.tileSize / 2);
								break;
							case 2:
								offset.Y = (float)(Game1.tileSize / 2);
								break;
							}
							Debris d = new Debris((this.struckByLightningCountdown > 0) ? 382 : this.indexOfFruit, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), (tileLocation.Y - 3f) * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) + offset, new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()))
							{
								itemQuality = fruitquality
							};
							d.Chunks[0].xVelocity += (float)Game1.random.Next(-10, 11) / 10f;
							d.chunkFinalYLevel = (int)(tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize);
							Game1.currentLocation.debris.Add(d);
						}
						this.fruitsOnTree = 0;
						return;
					}
				}
				else if (Game1.random.NextDouble() < 0.66)
				{
					int numberOfLeaves2 = Game1.random.Next(1, 3);
					for (int k = 0; k < numberOfLeaves2; k++)
					{
						this.leaves.Add(new Leaf(new Vector2((float)Game1.random.Next((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3 / 2)), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(30) / 10f));
					}
					return;
				}
			}
			else if (this.stump)
			{
				this.shakeTimer = 100f;
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x000CC10A File Offset: 0x000CA30A
		public override bool isPassable(Character c = null)
		{
			return this.health <= -99f;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000CC11C File Offset: 0x000CA31C
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
			if (this.health <= -99f)
			{
				this.destroy = true;
			}
			if (this.struckByLightningCountdown > 0)
			{
				this.struckByLightningCountdown--;
				if (this.struckByLightningCountdown <= 0)
				{
					this.fruitsOnTree = 0;
				}
			}
			bool foundSomething = false;
			Vector2[] surroundingTileLocationsArray = Utility.getSurroundingTileLocationsArray(tileLocation);
			for (int i = 0; i < surroundingTileLocationsArray.Length; i++)
			{
				Vector2 v = surroundingTileLocationsArray[i];
				bool isClearHoeDirt = environment.terrainFeatures.ContainsKey(v) && environment.terrainFeatures[v] is HoeDirt && (environment.terrainFeatures[v] as HoeDirt).crop == null;
				if (environment.isTileOccupied(v, "") && !isClearHoeDirt)
				{
					foundSomething = true;
					break;
				}
			}
			if (!foundSomething)
			{
				if (this.daysUntilMature > 28)
				{
					this.daysUntilMature = 28;
				}
				this.daysUntilMature--;
				if (this.daysUntilMature <= 0)
				{
					this.growthStage = 4;
				}
				else if (this.daysUntilMature <= 7)
				{
					this.growthStage = 3;
				}
				else if (this.daysUntilMature <= 14)
				{
					this.growthStage = 2;
				}
				else if (this.daysUntilMature <= 21)
				{
					this.growthStage = 1;
				}
				else
				{
					this.growthStage = 0;
				}
			}
			if (!this.stump && this.growthStage == 4 && ((this.struckByLightningCountdown > 0 && !Game1.IsWinter) || Game1.currentSeason.Equals(this.fruitSeason) || environment.name.ToLower().Contains("greenhouse")))
			{
				this.fruitsOnTree = Math.Min(3, this.fruitsOnTree + 1);
				if (environment.name.ToLower().Contains("greenhouse"))
				{
					this.greenHouseTree = true;
				}
			}
			if (this.stump)
			{
				this.fruitsOnTree = 0;
			}
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000CC2D7 File Offset: 0x000CA4D7
		public override bool seasonUpdate(bool onLoad)
		{
			if (!Game1.currentSeason.Equals(this.fruitSeason) && !onLoad && !this.greenHouseTree)
			{
				this.fruitsOnTree = 0;
			}
			return false;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x000CC300 File Offset: 0x000CA500
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			Console.WriteLine(string.Concat(new object[]
			{
				"FRUIT TREE: IsClient:",
				Game1.IsClient.ToString(),
				" randomOutput: ",
				Game1.recentMultiplayerRandom.Next(9999)
			}));
			if (this.health <= -99f)
			{
				return false;
			}
			if (t != null && t is MeleeWeapon)
			{
				return false;
			}
			if (this.growthStage >= 4)
			{
				if (t != null && t is Axe)
				{
					Game1.playSound("axchop");
					Game1.currentLocation.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f), t.getLastFarmerToUse().position, 0, -1));
					this.lastPlayerToHit = t.getLastFarmerToUse().uniqueMultiplayerID;
					int fruitquality = 0;
					if (this.daysUntilMature <= -112)
					{
						fruitquality = 1;
					}
					if (this.daysUntilMature <= -224)
					{
						fruitquality = 2;
					}
					if (this.daysUntilMature <= -336)
					{
						fruitquality = 4;
					}
					if (this.struckByLightningCountdown > 0)
					{
						fruitquality = 0;
					}
					if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) && Game1.currentLocation.terrainFeatures[tileLocation].Equals(this))
					{
						for (int i = 0; i < this.fruitsOnTree; i++)
						{
							Vector2 offset = new Vector2(0f, 0f);
							switch (i)
							{
							case 0:
								offset.X = (float)(-(float)Game1.tileSize);
								break;
							case 1:
								offset.X = (float)Game1.tileSize;
								offset.Y = (float)(-(float)Game1.tileSize / 2);
								break;
							case 2:
								offset.Y = (float)(Game1.tileSize / 2);
								break;
							}
							Debris d = new Debris((this.struckByLightningCountdown > 0) ? 382 : this.indexOfFruit, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), (tileLocation.Y - 3f) * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) + offset, new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()))
							{
								itemQuality = fruitquality
							};
							d.Chunks[0].xVelocity += (float)Game1.random.Next(-10, 11) / 10f;
							d.chunkFinalYLevel = (int)(tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize);
							Game1.currentLocation.debris.Add(d);
						}
						this.fruitsOnTree = 0;
					}
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
						Game1.playSound("treecrack");
						this.stump = true;
						this.health = 5f;
						this.falling = true;
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
						Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(30, 40), false, -1, false, -1);
						int whatToDrop = 92;
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 2000 + (int)tileLocation.Y);
							Random arg_447_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (t == null || t.getLastFarmerToUse() == null)
						{
							Game1.createMultipleObjectDebris(92, (int)tileLocation.X, (int)tileLocation.Y, 2);
						}
						else if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(whatToDrop, (int)tileLocation.X, (int)tileLocation.Y, 1, this.lastPlayerToHit);
							Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, 4, true, -1, false, -1);
						}
						else
						{
							Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, 5, true, -1, false, -1);
							Game1.createMultipleObjectDebris(whatToDrop, (int)tileLocation.X, (int)tileLocation.Y, 1);
						}
					}
				}
			}
			else if (this.growthStage >= 3)
			{
				if (t != null && t.name.Contains("Ax"))
				{
					Game1.playSound("axchop");
					Game1.playSound("leafrustle");
					Game1.currentLocation.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f), new Vector2((float)t.getLastFarmerToUse().GetBoundingBox().Center.X, (float)t.getLastFarmerToUse().GetBoundingBox().Center.Y), 0, -1));
				}
				else if (explosion <= 0)
				{
					return false;
				}
				this.shake(tileLocation, true);
				float damage2 = 1f;
				Random debrisRandom;
				if (Game1.IsMultiplayer)
				{
					debrisRandom = Game1.recentMultiplayerRandom;
				}
				else
				{
					debrisRandom = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + Game1.stats.DaysPlayed + this.health));
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
				int debris = 0;
				while (t != null && debrisRandom.NextDouble() < (double)damage2 * 0.08 + (double)((float)t.getLastFarmerToUse().ForagingLevel / 200f))
				{
					debris++;
				}
				this.health -= damage2;
				if (debris > 0)
				{
					Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, debris, null);
				}
				if (this.health <= 0f)
				{
					Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(20, 30), false, -1, false, -1);
					return true;
				}
			}
			else if (this.growthStage >= 1)
			{
				if (explosion > 0)
				{
					return true;
				}
				if (t != null && t.name.Contains("Axe"))
				{
					Game1.playSound("axchop");
					Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
				}
				if (t is Axe || t is Pickaxe || t is Hoe || t is MeleeWeapon)
				{
					Game1.createRadialDebris(Game1.currentLocation, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
					if (t.name.Contains("Axe") && Game1.recentMultiplayerRandom.NextDouble() < (double)((float)t.getLastFarmerToUse().ForagingLevel / 10f))
					{
						Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, 1, null);
					}
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
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
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x000CCC0C File Offset: 0x000CAE0C
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			layerDepth += positionOnScreen.X / 100000f;
			if (this.growthStage < 4)
			{
				Rectangle sourceRect = Rectangle.Empty;
				switch (this.growthStage)
				{
				case 0:
					sourceRect = new Rectangle(2 * Game1.tileSize, 8 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					break;
				case 1:
					sourceRect = new Rectangle(0, 8 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					break;
				case 2:
					sourceRect = new Rectangle(Game1.tileSize, 8 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					break;
				default:
					sourceRect = new Rectangle(0, 6 * Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
					break;
				}
				spriteBatch.Draw(FruitTree.texture, positionOnScreen - new Vector2(0f, (float)sourceRect.Height * scale), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)sourceRect.Height * scale) / 20000f);
				return;
			}
			if (!this.falling)
			{
				spriteBatch.Draw(FruitTree.texture, positionOnScreen + new Vector2(0f, (float)(-(float)Game1.tileSize) * scale), new Rectangle?(new Rectangle(2 * Game1.tileSize, 6 * Game1.tileSize, Game1.tileSize, Game1.tileSize * 2)), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale - 1f) / 20000f);
			}
			if (!this.stump || this.falling)
			{
				spriteBatch.Draw(FruitTree.texture, positionOnScreen + new Vector2((float)(-(float)Game1.tileSize) * scale, (float)(-5 * Game1.tileSize) * scale), new Rectangle?(new Rectangle(0, 0, 3 * Game1.tileSize, 6 * Game1.tileSize)), Color.White, this.shakeRotation, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)(7 * Game1.tileSize) * scale) / 20000f);
			}
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000CCE54 File Offset: 0x000CB054
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.greenHouseTileTree)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(669, 1957, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
			}
			if (this.growthStage < 4)
			{
				Vector2 positionOffset = new Vector2((float)Math.Max(-8.0, Math.Min((double)Game1.tileSize, Math.Sin((double)(tileLocation.X * 200f) / 6.2831853071795862) * -16.0)), (float)Math.Max(-8.0, Math.Min((double)Game1.tileSize, Math.Sin((double)(tileLocation.X * 200f) / 6.2831853071795862) * -16.0)));
				Rectangle sourceRect = Rectangle.Empty;
				switch (this.growthStage)
				{
				case 0:
					sourceRect = new Rectangle(0, this.treeType * 5 * 16, 48, 80);
					break;
				case 1:
					sourceRect = new Rectangle(48, this.treeType * 5 * 16, 48, 80);
					break;
				case 2:
					sourceRect = new Rectangle(96, this.treeType * 5 * 16, 48, 80);
					break;
				default:
					sourceRect = new Rectangle(144, this.treeType * 5 * 16, 48, 80);
					break;
				}
				spriteBatch.Draw(FruitTree.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + positionOffset.X, tileLocation.Y * (float)Game1.tileSize - (float)sourceRect.Height + (float)(Game1.tileSize * 2) + positionOffset.Y)), new Rectangle?(sourceRect), Color.White, this.shakeRotation, new Vector2(24f, 80f), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f - tileLocation.X / 1000000f);
			}
			else
			{
				if (!this.stump || this.falling)
				{
					if (!this.falling)
					{
						spriteBatch.Draw(FruitTree.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)), new Rectangle?(new Rectangle((12 + (this.greenHouseTree ? 1 : Utility.getSeasonNumber(Game1.currentSeason)) * 3) * 16, this.treeType * 5 * 16 + 64, 48, 16)), (this.struckByLightningCountdown > 0) ? (Color.Gray * this.alpha) : (Color.White * this.alpha), 0f, new Vector2(24f, 16f), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-07f);
					}
					spriteBatch.Draw(FruitTree.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)), new Rectangle?(new Rectangle((12 + (this.greenHouseTree ? 1 : Utility.getSeasonNumber(Game1.currentSeason)) * 3) * 16, this.treeType * 5 * 16, 48, 64)), (this.struckByLightningCountdown > 0) ? (Color.Gray * this.alpha) : (Color.White * this.alpha), this.shakeRotation, new Vector2(24f, 80f), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.001f - tileLocation.X / 1000000f);
				}
				if (this.health >= 1f || (!this.falling && this.health > -99f))
				{
					spriteBatch.Draw(FruitTree.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) + ((this.shakeTimer > 0f) ? ((float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 2f) : 0f), tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize)), new Rectangle?(new Rectangle(384, this.treeType * 5 * 16 + 48, 48, 32)), (this.struckByLightningCountdown > 0) ? (Color.Gray * this.alpha) : (Color.White * this.alpha), 0f, new Vector2(24f, 32f), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.stump && !this.falling) ? ((float)this.getBoundingBox(tileLocation).Bottom / 10000f) : ((float)this.getBoundingBox(tileLocation).Bottom / 10000f - 0.001f - tileLocation.X / 1000000f));
				}
				for (int i = 0; i < this.fruitsOnTree; i++)
				{
					switch (i)
					{
					case 0:
						spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize - (float)Game1.tileSize + tileLocation.X * 200f % (float)Game1.tileSize / 2f, tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3) - tileLocation.X % (float)Game1.tileSize / 3f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (this.struckByLightningCountdown > 0) ? 382 : this.indexOfFruit, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.002f - tileLocation.X / 1000000f);
						break;
					case 1:
						spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 4) + tileLocation.X * 232f % (float)Game1.tileSize / 3f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (this.struckByLightningCountdown > 0) ? 382 : this.indexOfFruit, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.002f - tileLocation.X / 1000000f);
						break;
					case 2:
						spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + tileLocation.X * 200f % (float)Game1.tileSize / 3f, tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize * 2.5f + tileLocation.X * 200f % (float)Game1.tileSize / 3f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (this.struckByLightningCountdown > 0) ? 382 : this.indexOfFruit, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * 1f, SpriteEffects.FlipHorizontally, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.002f - tileLocation.X / 1000000f);
						break;
					}
				}
			}
			foreach (Leaf j in this.leaves)
			{
				spriteBatch.Draw(FruitTree.texture, Game1.GlobalToLocal(Game1.viewport, j.position), new Rectangle?(new Rectangle((24 + Utility.getSeasonNumber(Game1.currentSeason)) * 16, this.treeType * 5 * 16, Game1.tileSize / 8, Game1.tileSize / 8)), Color.White, j.rotation, Vector2.Zero, 4f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 0.01f);
			}
		}

		// Token: 0x04000977 RID: 2423
		public const float shakeRate = 0.0157079641f;

		// Token: 0x04000978 RID: 2424
		public const float shakeDecayRate = 0.00306796166f;

		// Token: 0x04000979 RID: 2425
		public const int minWoodDebrisForFallenTree = 12;

		// Token: 0x0400097A RID: 2426
		public const int minWoodDebrisForStump = 5;

		// Token: 0x0400097B RID: 2427
		public const int startingHealth = 10;

		// Token: 0x0400097C RID: 2428
		public const int leafFallRate = 3;

		// Token: 0x0400097D RID: 2429
		public const int DaysUntilMaturity = 28;

		// Token: 0x0400097E RID: 2430
		public const int maxFruitsOnTrees = 3;

		// Token: 0x0400097F RID: 2431
		public const int seedStage = 0;

		// Token: 0x04000980 RID: 2432
		public const int sproutStage = 1;

		// Token: 0x04000981 RID: 2433
		public const int saplingStage = 2;

		// Token: 0x04000982 RID: 2434
		public const int bushStage = 3;

		// Token: 0x04000983 RID: 2435
		public const int treeStage = 4;

		// Token: 0x04000984 RID: 2436
		public static Texture2D texture;

		// Token: 0x04000985 RID: 2437
		public int growthStage;

		// Token: 0x04000986 RID: 2438
		public int treeType = -1;

		// Token: 0x04000987 RID: 2439
		public int indexOfFruit;

		// Token: 0x04000988 RID: 2440
		public int daysUntilMature;

		// Token: 0x04000989 RID: 2441
		public int fruitsOnTree;

		// Token: 0x0400098A RID: 2442
		public int struckByLightningCountdown;

		// Token: 0x0400098B RID: 2443
		public float health;

		// Token: 0x0400098C RID: 2444
		public bool flipped;

		// Token: 0x0400098D RID: 2445
		public bool stump;

		// Token: 0x0400098E RID: 2446
		public bool greenHouseTree;

		// Token: 0x0400098F RID: 2447
		public bool greenHouseTileTree;

		// Token: 0x04000990 RID: 2448
		private bool shakeLeft;

		// Token: 0x04000991 RID: 2449
		private bool falling;

		// Token: 0x04000992 RID: 2450
		private bool destroy;

		// Token: 0x04000993 RID: 2451
		private float shakeRotation;

		// Token: 0x04000994 RID: 2452
		private float maxShake;

		// Token: 0x04000995 RID: 2453
		private float alpha = 1f;

		// Token: 0x04000996 RID: 2454
		private List<Leaf> leaves = new List<Leaf>();

		// Token: 0x04000997 RID: 2455
		private long lastPlayerToHit;

		// Token: 0x04000998 RID: 2456
		public string fruitSeason;

		// Token: 0x04000999 RID: 2457
		private float shakeTimer;
	}
}
