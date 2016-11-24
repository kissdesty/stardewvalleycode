using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Tools;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000072 RID: 114
	public class HoeDirt : TerrainFeature
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x000CDDBC File Offset: 0x000CBFBC
		public HoeDirt()
		{
			this.loadSprite();
			if (HoeDirt.drawGuide == null)
			{
				HoeDirt.populateDrawGuide();
			}
			if (Game1.currentLocation is MineShaft && (Game1.currentLocation as MineShaft).getMineArea(-1) == 80)
			{
				this.c = Color.MediumPurple * 0.4f;
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x000CDE21 File Offset: 0x000CC021
		public HoeDirt(int startingState) : this()
		{
			this.state = startingState;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x000CDE30 File Offset: 0x000CC030
		public HoeDirt(int startingState, Crop crop) : this()
		{
			this.state = startingState;
			this.crop = crop;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x000CA8DF File Offset: 0x000C8ADF
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.Y * (float)Game1.tileSize), Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x000CDE48 File Offset: 0x000CC048
		public override void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
		{
			if (this.crop != null && this.crop.currentPhase != 0 && speedOfCollision > 0 && this.maxShake == 0f && positionOfCollider.Intersects(this.getBoundingBox(tileLocation)) && Utility.isOnScreen(Utility.Vector2ToPoint(tileLocation), Game1.tileSize, location))
			{
				if (Game1.soundBank != null && (who == null || who.GetType() != typeof(FarmAnimal)) && !Grass.grassSound.IsPlaying)
				{
					Grass.grassSound = Game1.soundBank.GetCue("grassyStep");
					Grass.grassSound.Play();
				}
				this.shake(0.3926991f / (float)((5 + Game1.player.addedSpeed) / speedOfCollision) - ((speedOfCollision > 2) ? ((float)this.crop.currentPhase * 3.14159274f / 64f) : 0f), 0.03926991f / (float)((5 + Game1.player.addedSpeed) / speedOfCollision), (float)positionOfCollider.Center.X > tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2));
			}
			if (this.crop != null && this.crop.currentPhase != 0 && who is Farmer && (who as Farmer).running)
			{
				(who as Farmer).temporarySpeedBuff = -1f;
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000CDFB4 File Offset: 0x000CC1B4
		private void shake(float shake, float rate, bool left)
		{
			if (this.crop != null)
			{
				this.maxShake = shake * (this.crop.raisedSeeds ? 0.6f : 1.5f);
				this.shakeRate = rate * 0.5f;
				this.shakeRotation = 0f;
				this.shakeLeft = left;
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000CE009 File Offset: 0x000CC209
		public bool needsWatering()
		{
			return this.crop != null && (!this.readyForHarvest() || this.crop.regrowAfterHarvest != -1);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000CE030 File Offset: 0x000CC230
		public static void populateDrawGuide()
		{
			HoeDirt.drawGuide = new Dictionary<int, int>();
			HoeDirt.drawGuide.Add(0, 0);
			HoeDirt.drawGuide.Add(10, 15);
			HoeDirt.drawGuide.Add(100, 13);
			HoeDirt.drawGuide.Add(1000, 12);
			HoeDirt.drawGuide.Add(500, 4);
			HoeDirt.drawGuide.Add(1010, 11);
			HoeDirt.drawGuide.Add(1100, 9);
			HoeDirt.drawGuide.Add(1500, 8);
			HoeDirt.drawGuide.Add(600, 1);
			HoeDirt.drawGuide.Add(510, 3);
			HoeDirt.drawGuide.Add(110, 14);
			HoeDirt.drawGuide.Add(1600, 5);
			HoeDirt.drawGuide.Add(1610, 6);
			HoeDirt.drawGuide.Add(1510, 7);
			HoeDirt.drawGuide.Add(1110, 10);
			HoeDirt.drawGuide.Add(610, 2);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000CE144 File Offset: 0x000CC344
		public override void loadSprite()
		{
			if (HoeDirt.lightTexture == null)
			{
				try
				{
					HoeDirt.lightTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\hoeDirt");
				}
				catch (Exception)
				{
				}
			}
			if (HoeDirt.darkTexture == null)
			{
				try
				{
					HoeDirt.darkTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\hoeDirtDark");
				}
				catch (Exception)
				{
				}
			}
			if (HoeDirt.snowTexture == null)
			{
				try
				{
					HoeDirt.snowTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\hoeDirtSnow");
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000CE1DC File Offset: 0x000CC3DC
		public override bool isPassable(Character c)
		{
			return this.crop == null || !this.crop.raisedSeeds || c is JunimoHarvester;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000CE200 File Offset: 0x000CC400
		public bool readyForHarvest()
		{
			return this.crop != null && (!this.crop.fullyGrown || this.crop.dayOfCurrentPhase <= 0) && this.crop.currentPhase >= this.crop.phaseDays.Count - 1 && !this.crop.dead;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x000CE260 File Offset: 0x000CC460
		public override bool performUseAction(Vector2 tileLocation)
		{
			if (this.crop == null)
			{
				return false;
			}
			bool harvestable = this.crop.currentPhase >= this.crop.phaseDays.Count - 1 && (!this.crop.fullyGrown || this.crop.dayOfCurrentPhase <= 0);
			if (this.crop.harvestMethod == 0 && this.crop.harvest((int)tileLocation.X, (int)tileLocation.Y, this, null))
			{
				this.destroyCrop(tileLocation, false);
				return true;
			}
			if (this.crop.harvestMethod == 1 && this.readyForHarvest())
			{
				if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && (Game1.player.CurrentTool as MeleeWeapon).initialParentTileIndex == 47)
				{
					Game1.player.CanMove = false;
					Game1.player.UsingTool = true;
					Game1.player.canReleaseTool = true;
					Game1.player.Halt();
					try
					{
						Game1.player.CurrentTool.beginUsing(Game1.currentLocation, (int)Game1.player.lastClick.X, (int)Game1.player.lastClick.Y, Game1.player);
					}
					catch (Exception)
					{
					}
					((MeleeWeapon)Game1.player.CurrentTool).setFarmerAnimating(Game1.player);
				}
				else
				{
					Game1.showRedMessage("Requires Scythe");
				}
			}
			return harvestable;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x000CE3EC File Offset: 0x000CC5EC
		public bool plant(int index, int tileX, int tileY, Farmer who, bool isFertilizer = false)
		{
			if (isFertilizer)
			{
				if (this.crop != null && (index == 368 || index == 369))
				{
					Game1.showRedMessage("Must be added before planting");
					return false;
				}
				if (this.fertilizer == 0)
				{
					this.fertilizer = index;
					Game1.playSound("dirtyHit");
				}
				return true;
			}
			else
			{
				Crop c = new Crop(index, tileX, tileY);
				if (c.seasonsToGrowIn.Count == 0)
				{
					return false;
				}
				if (this.fertilizer == 465 || this.fertilizer == 466 || who.professions.Contains(5))
				{
					int totalDaysOfCropGrowth = 0;
					for (int i = 0; i < c.phaseDays.Count - 1; i++)
					{
						totalDaysOfCropGrowth += c.phaseDays[i];
					}
					float speedIncrease = (this.fertilizer == 465) ? 0.1f : ((this.fertilizer == 466) ? 0.25f : 0f);
					if (who.professions.Contains(5))
					{
						speedIncrease += 0.1f;
					}
					int daysToRemove = (int)Math.Ceiling((double)((float)totalDaysOfCropGrowth * speedIncrease));
					int tries = 0;
					while (daysToRemove > 0 && tries < 3)
					{
						for (int j = 0; j < c.phaseDays.Count; j++)
						{
							if (j > 0 || c.phaseDays[j] > 1)
							{
								List<int> arg_12F_0 = c.phaseDays;
								int index2 = j;
								int num = arg_12F_0[index2];
								arg_12F_0[index2] = num - 1;
								daysToRemove--;
							}
							if (daysToRemove <= 0)
							{
								break;
							}
						}
						tries++;
					}
				}
				if (!who.currentLocation.isFarm && !who.currentLocation.name.Equals("Greenhouse"))
				{
					Game1.showRedMessage("Must be planted on farm.");
					return false;
				}
				if (who.currentLocation.name.Equals("Greenhouse") || c.seasonsToGrowIn.Contains(Game1.currentSeason))
				{
					this.crop = c;
					if (c.raisedSeeds)
					{
						Game1.playSound("stoneStep");
					}
					Game1.playSound("dirtyHit");
					Stats expr_1F4 = Game1.stats;
					uint seedsSown = expr_1F4.SeedsSown;
					expr_1F4.SeedsSown = seedsSown + 1u;
					return true;
				}
				if (c.seasonsToGrowIn.Count > 0 && !c.seasonsToGrowIn.Contains(Game1.currentSeason))
				{
					Game1.showRedMessage("Out of season.");
				}
				else
				{
					Game1.showRedMessage("Can't plant here.");
				}
				return false;
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x000CE638 File Offset: 0x000CC838
		public void destroyCrop(Vector2 tileLocation, bool showAnimation = true)
		{
			if (this.crop != null & showAnimation)
			{
				if (this.crop.currentPhase < 1 && !this.crop.dead)
				{
					Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					Game1.playSound("dirtyHit");
				}
				else
				{
					Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(50, tileLocation * (float)Game1.tileSize, this.crop.dead ? new Color(207, 193, 43) : Color.ForestGreen, 8, false, 100f, 0, -1, -1f, -1, 0));
				}
			}
			this.crop = null;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000CE71C File Offset: 0x000CC91C
		public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			if (t != null)
			{
				if (t.GetType() == typeof(Pickaxe) && this.crop == null)
				{
					return true;
				}
				if (t.GetType() == typeof(WateringCan))
				{
					this.state = 1;
				}
				else if (t is MeleeWeapon && (t as MeleeWeapon).name.Equals("Scythe"))
				{
					if (this.crop != null && this.crop.harvestMethod == 1 && this.crop.harvest((int)tileLocation.X, (int)tileLocation.Y, this, null))
					{
						this.destroyCrop(tileLocation, true);
					}
					if (this.crop != null && this.crop.dead)
					{
						this.destroyCrop(tileLocation, true);
					}
				}
				else if (t.isHeavyHitter() && t.GetType() != typeof(Hoe) && !(t is MeleeWeapon) && this.crop != null)
				{
					this.destroyCrop(tileLocation, true);
				}
				this.shake(0.09817477f, 0.07853982f, tileLocation.X * (float)Game1.tileSize < Game1.player.position.X);
			}
			else if (damage > 0 && this.crop != null)
			{
				if (damage == 50)
				{
					this.crop.dead = true;
				}
				else
				{
					this.destroyCrop(tileLocation, true);
				}
			}
			return false;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000CE87C File Offset: 0x000CCA7C
		public bool canPlantThisSeedHere(int objectIndex, int tileX, int tileY, bool isFertilizer = false)
		{
			if (isFertilizer)
			{
				if (this.fertilizer == 0)
				{
					return true;
				}
			}
			else if (this.crop == null)
			{
				Crop c = new Crop(objectIndex, tileX, tileY);
				if (c.seasonsToGrowIn.Count == 0)
				{
					return false;
				}
				if (Game1.currentLocation.name.Equals("Greenhouse") || c.seasonsToGrowIn.Contains(Game1.currentSeason))
				{
					return !c.raisedSeeds || !Utility.doesRectangleIntersectTile(Game1.player.GetBoundingBox(), tileX, tileY);
				}
				if (objectIndex == 309 || objectIndex == 310 || objectIndex == 311)
				{
					return true;
				}
				if (Game1.didPlayerJustClickAtAll())
				{
					Game1.playSound("cancel");
				}
			}
			return false;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000CE930 File Offset: 0x000CCB30
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.maxShake > 0f)
			{
				if (this.shakeLeft)
				{
					this.shakeRotation -= this.shakeRate;
					if (Math.Abs(this.shakeRotation) >= this.maxShake)
					{
						this.shakeLeft = false;
					}
				}
				else
				{
					this.shakeRotation += this.shakeRate;
					if (this.shakeRotation >= this.maxShake)
					{
						this.shakeLeft = true;
						this.shakeRotation -= this.shakeRate;
					}
				}
				this.maxShake = Math.Max(0f, this.maxShake - 0.0104719754f);
			}
			else
			{
				this.shakeRotation /= 2f;
			}
			return this.state == 2 && this.crop == null;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000CEA04 File Offset: 0x000CCC04
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
			if (this.crop != null)
			{
				this.crop.newDay(this.state, this.fertilizer, (int)tileLocation.X, (int)tileLocation.Y, environment);
				if (!environment.name.Equals("Greenhouse") && Game1.currentSeason.Equals("winter") && this.crop != null && !this.crop.isWildSeedCrop())
				{
					this.destroyCrop(tileLocation, false);
				}
			}
			if ((this.fertilizer != 370 || Game1.random.NextDouble() >= 0.33) && (this.fertilizer != 371 || Game1.random.NextDouble() >= 0.66))
			{
				this.state = 0;
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x000CEACA File Offset: 0x000CCCCA
		public override bool seasonUpdate(bool onLoad)
		{
			if (!onLoad && (this.crop == null || this.crop.dead || !this.crop.seasonsToGrowIn.Contains(Game1.currentSeason)))
			{
				this.fertilizer = 0;
			}
			return false;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000CEB04 File Offset: 0x000CCD04
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			int drawSum = 0;
			Vector2 surroundingLocations = tileLocation;
			surroundingLocations.X += 1f;
			GameLocation farm = Game1.getLocationFromName("Farm");
			if (farm.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
			{
				drawSum += 100;
			}
			surroundingLocations.X -= 2f;
			if (farm.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
			{
				drawSum += 10;
			}
			surroundingLocations.X += 1f;
			surroundingLocations.Y += 1f;
			if (Game1.currentLocation.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
			{
				drawSum += 500;
			}
			surroundingLocations.Y -= 2f;
			if (farm.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
			{
				drawSum += 1000;
			}
			int sourceRectPosition = HoeDirt.drawGuide[drawSum];
			spriteBatch.Draw(HoeDirt.lightTexture, positionOnScreen, new Rectangle?(new Rectangle(sourceRectPosition % 4 * Game1.tileSize, sourceRectPosition / 4 * Game1.tileSize, Game1.tileSize, Game1.tileSize)), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth + positionOnScreen.Y / 20000f);
			if (this.crop != null)
			{
				this.crop.drawInMenu(spriteBatch, positionOnScreen + new Vector2((float)Game1.tileSize * scale, (float)Game1.tileSize * scale), Color.White, 0f, scale, layerDepth + (positionOnScreen.Y + (float)Game1.tileSize * scale) / 20000f);
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000CED08 File Offset: 0x000CCF08
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.state != 2)
			{
				int drawSum = 0;
				int wateredSum = 0;
				Vector2 surroundingLocations = tileLocation;
				surroundingLocations.X += 1f;
				if (Game1.currentLocation.terrainFeatures.ContainsKey(surroundingLocations) && Game1.currentLocation.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
				{
					drawSum += 100;
					if (((HoeDirt)Game1.currentLocation.terrainFeatures[surroundingLocations]).state == this.state)
					{
						wateredSum += 100;
					}
				}
				surroundingLocations.X -= 2f;
				if (Game1.currentLocation.terrainFeatures.ContainsKey(surroundingLocations) && Game1.currentLocation.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
				{
					drawSum += 10;
					if (((HoeDirt)Game1.currentLocation.terrainFeatures[surroundingLocations]).state == this.state)
					{
						wateredSum += 10;
					}
				}
				surroundingLocations.X += 1f;
				surroundingLocations.Y += 1f;
				if (Game1.currentLocation.terrainFeatures.ContainsKey(surroundingLocations) && Game1.currentLocation.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
				{
					drawSum += 500;
					if (((HoeDirt)Game1.currentLocation.terrainFeatures[surroundingLocations]).state == this.state)
					{
						wateredSum += 500;
					}
				}
				surroundingLocations.Y -= 2f;
				if (Game1.currentLocation.terrainFeatures.ContainsKey(surroundingLocations) && Game1.currentLocation.terrainFeatures[surroundingLocations].GetType() == typeof(HoeDirt))
				{
					drawSum += 1000;
					if (((HoeDirt)Game1.currentLocation.terrainFeatures[surroundingLocations]).state == this.state)
					{
						wateredSum += 1000;
					}
				}
				int sourceRectPosition = HoeDirt.drawGuide[drawSum];
				int wateredRectPosition = HoeDirt.drawGuide[wateredSum];
				Texture2D texture = (Game1.currentLocation.Name.Equals("Mountain") || Game1.currentLocation.Name.Equals("Mine") || (Game1.currentLocation is MineShaft && Game1.mine.getMineArea(-1) != 121)) ? HoeDirt.darkTexture : HoeDirt.lightTexture;
				if ((Game1.currentSeason.Equals("winter") && !(Game1.currentLocation is Desert) && !Game1.currentLocation.Name.Equals("Greenhouse") && !(Game1.currentLocation is MineShaft)) || (Game1.currentLocation is MineShaft && Game1.mine.getMineArea(-1) == 40 && !Game1.mine.isLevelSlimeArea()))
				{
					texture = HoeDirt.snowTexture;
				}
				spriteBatch.Draw(texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(sourceRectPosition % 4 * 16, sourceRectPosition / 4 * 16, 16, 16)), this.c, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-08f);
				if (this.state == 1)
				{
					spriteBatch.Draw(texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(wateredRectPosition % 4 * 16 + 64, wateredRectPosition / 4 * 16, 16, 16)), this.c, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1.2E-08f);
				}
				if (this.fertilizer != 0)
				{
					int fertilizerIndex = 0;
					int num = this.fertilizer;
					switch (num)
					{
					case 369:
						fertilizerIndex = 1;
						break;
					case 370:
						fertilizerIndex = 2;
						break;
					case 371:
						fertilizerIndex = 3;
						break;
					default:
						if (num != 465)
						{
							if (num == 466)
							{
								fertilizerIndex = 5;
							}
						}
						else
						{
							fertilizerIndex = 4;
						}
						break;
					}
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(173 + fertilizerIndex / 2 * 16, 466 + fertilizerIndex % 2 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1.9E-08f);
				}
			}
			if (this.crop != null)
			{
				this.crop.draw(spriteBatch, tileLocation, (this.state == 1 && this.crop.currentPhase == 0 && !this.crop.raisedSeeds) ? (new Color(180, 100, 200) * 1f) : Color.White, this.shakeRotation);
			}
		}

		// Token: 0x0400099F RID: 2463
		public const float defaultShakeRate = 0.03926991f;

		// Token: 0x040009A0 RID: 2464
		public const float maximumShake = 0.3926991f;

		// Token: 0x040009A1 RID: 2465
		public const float shakeDecayRate = 0.0104719754f;

		// Token: 0x040009A2 RID: 2466
		public const int N = 1000;

		// Token: 0x040009A3 RID: 2467
		public const int E = 100;

		// Token: 0x040009A4 RID: 2468
		public const int S = 500;

		// Token: 0x040009A5 RID: 2469
		public const int W = 10;

		// Token: 0x040009A6 RID: 2470
		public const int dry = 0;

		// Token: 0x040009A7 RID: 2471
		public const int watered = 1;

		// Token: 0x040009A8 RID: 2472
		public const int invisible = 2;

		// Token: 0x040009A9 RID: 2473
		public const int noFertilizer = 0;

		// Token: 0x040009AA RID: 2474
		public const int fertilizerLowQuality = 368;

		// Token: 0x040009AB RID: 2475
		public const int fertilizerHighQuality = 369;

		// Token: 0x040009AC RID: 2476
		public const int waterRetentionSoil = 370;

		// Token: 0x040009AD RID: 2477
		public const int waterRetentionSoilQUality = 371;

		// Token: 0x040009AE RID: 2478
		public const int speedGro = 465;

		// Token: 0x040009AF RID: 2479
		public const int superSpeedGro = 466;

		// Token: 0x040009B0 RID: 2480
		public static Texture2D lightTexture;

		// Token: 0x040009B1 RID: 2481
		public static Texture2D darkTexture;

		// Token: 0x040009B2 RID: 2482
		public static Texture2D snowTexture;

		// Token: 0x040009B3 RID: 2483
		public Crop crop;

		// Token: 0x040009B4 RID: 2484
		public static Dictionary<int, int> drawGuide;

		// Token: 0x040009B5 RID: 2485
		public int state;

		// Token: 0x040009B6 RID: 2486
		public int fertilizer;

		// Token: 0x040009B7 RID: 2487
		private bool shakeLeft;

		// Token: 0x040009B8 RID: 2488
		private float shakeRotation;

		// Token: 0x040009B9 RID: 2489
		private float maxShake;

		// Token: 0x040009BA RID: 2490
		private float shakeRate;

		// Token: 0x040009BB RID: 2491
		private Color c = Color.White;
	}
}
