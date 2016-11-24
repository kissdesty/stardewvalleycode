using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using xTile;

namespace StardewValley.Buildings
{
	// Token: 0x02000149 RID: 329
	[XmlInclude(typeof(Coop)), XmlInclude(typeof(Barn)), XmlInclude(typeof(Stable)), XmlInclude(typeof(Mill)), XmlInclude(typeof(JunimoHut))]
	public class Building
	{
		// Token: 0x06001299 RID: 4761 RVA: 0x00178D0C File Offset: 0x00176F0C
		public Building()
		{
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00178D20 File Offset: 0x00176F20
		public Building(string buildingType, string nameOfIndoors, int tileX, int tileY, int tilesWide, int tilesTall, Point humanDoor, Point animalDoor, GameLocation indoors, Texture2D texture, bool magical, long owner)
		{
			this.tileX = tileX;
			this.tileY = tileY;
			this.tilesWide = tilesWide;
			this.tilesHigh = tilesTall;
			this.buildingType = buildingType;
			this.nameOfIndoors = nameOfIndoors + (tileX * 2000 + tileY);
			this.texture = texture;
			this.indoors = indoors;
			this.baseNameOfIndoors = indoors.name;
			this.nameOfIndoorsWithoutUnique = this.baseNameOfIndoors;
			this.humanDoor = humanDoor;
			this.animalDoor = animalDoor;
			this.daysOfConstructionLeft = 2;
			this.magical = magical;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00178DC7 File Offset: 0x00176FC7
		public int getTileSheetIndexForStructurePlacementTile(int x, int y)
		{
			if (x == this.humanDoor.X && y == this.humanDoor.Y)
			{
				return 2;
			}
			if (x == this.animalDoor.X && y == this.animalDoor.Y)
			{
				return 4;
			}
			return 0;
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void performTenMinuteAction(int timeElapsed)
		{
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00178E06 File Offset: 0x00177006
		public virtual void performActionOnPlayerLocationEntry()
		{
			this.color = Color.White;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00178E14 File Offset: 0x00177014
		public virtual bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (who.IsMainPlayer && tileLocation.X >= (float)this.tileX && tileLocation.X < (float)(this.tileX + this.tilesWide) && tileLocation.Y >= (float)this.tileY && tileLocation.Y < (float)(this.tileY + this.tilesHigh) && this.daysOfConstructionLeft > 0)
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:UnderConstruction", new object[0]));
			}
			else if (who.IsMainPlayer && tileLocation.X == (float)(this.humanDoor.X + this.tileX) && tileLocation.Y == (float)(this.humanDoor.Y + this.tileY) && this.indoors != null)
			{
				if (who.getMount() != null)
				{
					Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:DismountBeforeEntering", new object[0]));
					return false;
				}
				this.indoors.isStructure = true;
				this.indoors.uniqueName = this.baseNameOfIndoors + (this.tileX * 2000 + this.tileY);
				Game1.warpFarmer(this.indoors, this.indoors.warps[0].X, this.indoors.warps[0].Y - 1, Game1.player.facingDirection, true);
				Game1.playSound("doorClose");
				return true;
			}
			else if (who.IsMainPlayer && this.buildingType.Equals("Silo") && !this.isTilePassable(tileLocation))
			{
				if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 178)
				{
					if (who.ActiveObject.Stack == 0)
					{
						who.ActiveObject.stack = 1;
					}
					int old = who.ActiveObject.Stack;
					int leftOver = (Game1.getLocationFromName("Farm") as Farm).tryToAddHay(who.ActiveObject.Stack);
					who.ActiveObject.stack = leftOver;
					if (who.ActiveObject.stack < old)
					{
						Game1.playSound("Ship");
						DelayedAction.playSoundAfterDelay("grassyStep", 100);
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:AddedHay", new object[]
						{
							old - who.ActiveObject.Stack
						}));
					}
					if (who.ActiveObject.Stack <= 0)
					{
						who.removeItemFromInventory(who.ActiveObject);
					}
				}
				else
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:PiecesOfHay", new object[]
					{
						(Game1.getLocationFromName("Farm") as Farm).piecesOfHay,
						Utility.numSilos() * 240
					}));
				}
			}
			else if (who.IsMainPlayer && this.buildingType.Contains("Obelisk") && !this.isTilePassable(tileLocation))
			{
				for (int i = 0; i < 12; i++)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)who.position.X - Game1.tileSize * 4, (int)who.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)who.position.Y - Game1.tileSize * 4, (int)who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
				}
				Game1.playSound("wand");
				Game1.displayFarmer = false;
				Game1.player.freezePause = 1000;
				Game1.flashAlpha = 1f;
				DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.obeliskWarpForReal), 1000);
				Rectangle r = new Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize);
				r.Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
				int j = 0;
				for (int x = who.getTileX() + 8; x >= who.getTileX() - 8; x--)
				{
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)x, (float)who.getTileY()) * (float)Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
					{
						layerDepth = 1f,
						delayBeforeAnimationStart = j * 25,
						motion = new Vector2(-0.25f, 0f)
					});
					j++;
				}
			}
			return false;
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00179300 File Offset: 0x00177500
		private void obeliskWarpForReal()
		{
			string a = this.buildingType;
			if (!(a == "Earth Obelisk"))
			{
				if (a == "Water Obelisk")
				{
					Game1.warpFarmer("Beach", 20, 4, false);
				}
			}
			else
			{
				Game1.warpFarmer("Mountain", 31, 20, false);
			}
			Game1.fadeToBlackAlpha = 0.99f;
			Game1.screenGlow = false;
			Game1.player.temporarilyInvincible = false;
			Game1.player.temporaryInvincibilityTimer = 0;
			Game1.displayFarmer = true;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0017937C File Offset: 0x0017757C
		public virtual bool isActionableTile(int xTile, int yTile, Farmer who)
		{
			return (this.humanDoor.X >= 0 && xTile == this.tileX + this.humanDoor.X && yTile == this.tileY + this.humanDoor.Y) || (this.animalDoor.X >= 0 && xTile == this.tileX + this.animalDoor.X && yTile == this.tileY + this.animalDoor.Y);
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00179400 File Offset: 0x00177600
		public virtual void performActionOnConstruction(GameLocation location)
		{
			this.daysOfConstructionLeft = 2;
			this.newConstructionTimer = (this.magical ? 2000 : 1000);
			if (!this.magical)
			{
				Game1.playSound("axchop");
				for (int x = this.tileX; x < this.tileX + this.tilesWide; x++)
				{
					for (int y = this.tileY; y < this.tileY + this.tilesHigh; y++)
					{
						for (int i = 0; i < 5; i++)
						{
							location.temporarySprites.Add(new TemporaryAnimatedSprite((Game1.random.NextDouble() < 0.5) ? 46 : 12, new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2)), Color.White, 10, Game1.random.NextDouble() < 0.5, 100f, 0, -1, -1f, -1, 0)
							{
								delayBeforeAnimationStart = Math.Max(0, Game1.random.Next(-200, 400)),
								motion = new Vector2(0f, -1f),
								interval = (float)Game1.random.Next(50, 80)
							});
						}
						location.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2)), Color.White, 10, Game1.random.NextDouble() < 0.5, 100f, 0, -1, -1f, -1, 0));
					}
				}
				for (int j = 0; j < 8; j++)
				{
					DelayedAction.playSoundAfterDelay("dirtyHit", 250 + j * 150);
				}
				return;
			}
			for (int k = 0; k < 8; k++)
			{
				DelayedAction.playSoundAfterDelay("dirtyHit", 100 + k * 210);
			}
			Game1.flashAlpha = 2f;
			Game1.soundBank.PlayCue("wand");
			for (int x2 = 0; x2 < this.getSourceRectForMenu().Width / 16 * 2; x2++)
			{
				for (int y2 = this.texture.Bounds.Height / 16 * 2; y2 >= 0; y2--)
				{
					location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(666, 1851, 8, 8), 40f, 4, 2, new Vector2((float)this.tileX, (float)this.tileY) * (float)Game1.tileSize + new Vector2((float)(x2 * Game1.tileSize / 2), (float)(y2 * Game1.tileSize / 2 - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, false)
					{
						layerDepth = (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + (float)x2 / 10000f,
						pingPong = true,
						delayBeforeAnimationStart = (this.texture.Bounds.Height / 16 * 2 - y2) * 100,
						scale = (float)Game1.pixelZoom,
						alphaFade = 0.01f,
						color = Color.AliceBlue
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(666, 1851, 8, 8), 40f, 4, 2, new Vector2((float)this.tileX, (float)this.tileY) * (float)Game1.tileSize + new Vector2((float)(x2 * Game1.tileSize / 2), (float)(y2 * Game1.tileSize / 2 - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, false)
					{
						layerDepth = (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + (float)x2 / 10000f + 0.0001f,
						pingPong = true,
						delayBeforeAnimationStart = (this.texture.Bounds.Height / 16 * 2 - y2) * 100,
						scale = (float)Game1.pixelZoom,
						alphaFade = 0.01f,
						color = Color.AliceBlue
					});
				}
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void performActionOnDemolition(GameLocation location)
		{
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void performActionOnUpgrade(GameLocation location)
		{
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x00035CBD File Offset: 0x00033EBD
		public virtual string isThereAnythingtoPreventConstruction(GameLocation location)
		{
			return null;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00179957 File Offset: 0x00177B57
		public virtual void updateWhenFarmNotCurrentLocation(GameTime time)
		{
			if (this.indoors != null)
			{
				this.indoors.updateEvenIfFarmerIsntHere(time, false);
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00179970 File Offset: 0x00177B70
		public virtual void Update(GameTime time)
		{
			if (this.newConstructionTimer > 0)
			{
				this.newConstructionTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.newConstructionTimer <= 0 && this.magical)
				{
					this.daysOfConstructionLeft = 0;
				}
			}
			this.alpha = Math.Min(1f, this.alpha + 0.05f);
			if (Game1.player.GetBoundingBox().Intersects(new Rectangle(Game1.tileSize * this.tileX, Game1.tileSize * (this.tileY + (-(this.getSourceRectForMenu().Height / 16) + this.tilesHigh)), this.tilesWide * Game1.tileSize, (this.getSourceRectForMenu().Height / 16 - this.tilesHigh) * Game1.tileSize + Game1.tileSize / 2)))
			{
				this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00179A68 File Offset: 0x00177C68
		public void showUpgradeAnimation(GameLocation location)
		{
			this.color = Color.White;
			location.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.getUpgradeSignLocation() + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), Color.Beige, 10, Game1.random.NextDouble() < 0.5, 75f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -0.5f),
				acceleration = new Vector2(-0.02f, 0.01f),
				delayBeforeAnimationStart = Game1.random.Next(100),
				layerDepth = 0.89f
			});
			location.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.getUpgradeSignLocation() + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), Color.Beige, 10, Game1.random.NextDouble() < 0.5, 75f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -0.5f),
				acceleration = new Vector2(-0.02f, 0.01f),
				delayBeforeAnimationStart = Game1.random.Next(40),
				layerDepth = 0.89f
			});
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00179C0E File Offset: 0x00177E0E
		public virtual Vector2 getUpgradeSignLocation()
		{
			return new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize / 2), (float)(this.tileY * Game1.tileSize - Game1.tileSize / 2));
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00179C40 File Offset: 0x00177E40
		public string getNameOfNextUpgrade()
		{
			string a = this.buildingType.ToLower();
			if (a == "coop")
			{
				return "Big Coop";
			}
			if (a == "big coop")
			{
				return "Deluxe Coop";
			}
			if (a == "barn")
			{
				return "Big Barn";
			}
			if (!(a == "big barn"))
			{
				return "well";
			}
			return "Deluxe Barn";
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00179CAC File Offset: 0x00177EAC
		public void showDestroyedAnimation(GameLocation location)
		{
			for (int x = this.tileX; x < this.tileX + this.tilesWide; x++)
			{
				for (int y = this.tileY; y < this.tileY + this.tilesHigh; y++)
				{
					location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5)
					{
						delayBeforeAnimationStart = Game1.random.Next(300)
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5)
					{
						delayBeforeAnimationStart = 250 + Game1.random.Next(300)
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, 0f, Color.White)
					{
						interval = 30f,
						totalNumberOfLoops = 99999,
						animationLength = 4,
						scale = (float)Game1.pixelZoom,
						alphaFade = 0.01f
					});
				}
			}
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00179F2C File Offset: 0x0017812C
		public virtual void dayUpdate(int dayOfMonth)
		{
			if (this.daysOfConstructionLeft > 0 && !Utility.isFestivalDay(dayOfMonth, Game1.currentSeason))
			{
				this.daysOfConstructionLeft--;
				if (this.daysOfConstructionLeft <= 0)
				{
					Game1.player.checkForQuestComplete(null, -1, -1, null, this.buildingType, 8, -1);
					if (this.buildingType.Equals("Slime Hutch") && this.indoors != null)
					{
						this.indoors.objects.Add(new Vector2(1f, 4f), new Object(new Vector2(1f, 4f), 156, false)
						{
							fragility = 2
						});
						if (!Game1.player.mailReceived.Contains("slimeHutchBuilt"))
						{
							Game1.player.mailReceived.Add("slimeHutchBuilt");
						}
					}
				}
				return;
			}
			if (this.daysUntilUpgrade > 0 && !Utility.isFestivalDay(dayOfMonth, Game1.currentSeason))
			{
				this.daysUntilUpgrade--;
				if (this.daysUntilUpgrade <= 0)
				{
					Game1.player.checkForQuestComplete(null, -1, -1, null, this.getNameOfNextUpgrade(), 8, -1);
					BluePrint CurrentBlueprint = new BluePrint(this.getNameOfNextUpgrade());
					this.indoors.map = Game1.content.Load<Map>("Maps\\" + CurrentBlueprint.mapToWarpTo);
					this.indoors.name = CurrentBlueprint.mapToWarpTo;
					this.buildingType = CurrentBlueprint.name;
					this.texture = CurrentBlueprint.texture;
					if (this.indoors.GetType() == typeof(AnimalHouse))
					{
						((AnimalHouse)this.indoors).resetPositionsOfAllAnimals();
						((AnimalHouse)this.indoors).animalLimit += 4;
						((AnimalHouse)this.indoors).loadLights();
					}
					this.upgrade();
				}
			}
			if (this.indoors != null)
			{
				this.indoors.DayUpdate(dayOfMonth);
			}
			if (this.buildingType.Contains("Deluxe"))
			{
				(this.indoors as AnimalHouse).feedAllAnimals();
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void upgrade()
		{
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0017A142 File Offset: 0x00178342
		public virtual Rectangle getSourceRectForMenu()
		{
			return this.texture.Bounds;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0017A150 File Offset: 0x00178350
		public Building(BluePrint blueprint, Vector2 tileLocation)
		{
			this.tileX = (int)tileLocation.X;
			this.tileY = (int)tileLocation.Y;
			this.tilesWide = blueprint.tilesWidth;
			this.tilesHigh = blueprint.tilesHeight;
			this.buildingType = blueprint.name;
			this.texture = blueprint.texture;
			this.humanDoor = blueprint.humanDoor;
			this.animalDoor = blueprint.animalDoor;
			this.nameOfIndoors = blueprint.mapToWarpTo;
			this.baseNameOfIndoors = this.nameOfIndoors;
			this.nameOfIndoorsWithoutUnique = this.baseNameOfIndoors;
			this.indoors = this.getIndoors();
			this.nameOfIndoors += this.tileX * 2000 + this.tileY;
			this.maxOccupants = blueprint.maxOccupants;
			this.daysOfConstructionLeft = 2;
			this.magical = blueprint.magical;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0017A248 File Offset: 0x00178448
		protected virtual GameLocation getIndoors()
		{
			if (this.buildingType.Equals("Slime Hutch"))
			{
				if (this.indoors != null)
				{
					this.nameOfIndoorsWithoutUnique = this.indoors.name;
				}
				string a2 = this.nameOfIndoorsWithoutUnique;
				if (a2 == "Slime Hutch")
				{
					this.nameOfIndoorsWithoutUnique = "SlimeHutch";
				}
				GameLocation lcl_indoors = new SlimeHutch(Game1.content.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
				lcl_indoors.IsFarm = true;
				lcl_indoors.isStructure = true;
				foreach (Warp expr_96 in lcl_indoors.warps)
				{
					expr_96.TargetX = this.humanDoor.X + this.tileX;
					expr_96.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				return lcl_indoors;
			}
			if (this.buildingType.Equals("Shed"))
			{
				if (this.indoors != null)
				{
					this.nameOfIndoorsWithoutUnique = this.indoors.name;
				}
				string a2 = this.nameOfIndoorsWithoutUnique;
				if (a2 == "Shed")
				{
					this.nameOfIndoorsWithoutUnique = "Shed";
				}
				GameLocation lcl_indoors2 = new Shed(Game1.content.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
				lcl_indoors2.IsFarm = true;
				lcl_indoors2.isStructure = true;
				foreach (Warp expr_178 in lcl_indoors2.warps)
				{
					expr_178.TargetX = this.humanDoor.X + this.tileX;
					expr_178.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				return lcl_indoors2;
			}
			if (this.nameOfIndoorsWithoutUnique != null && this.nameOfIndoorsWithoutUnique.Length > 0 && !this.nameOfIndoorsWithoutUnique.Equals("null"))
			{
				GameLocation lcl_indoors3 = new GameLocation(Game1.content.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
				lcl_indoors3.IsFarm = true;
				lcl_indoors3.isStructure = true;
				if (lcl_indoors3.name.Equals("Greenhouse"))
				{
					lcl_indoors3.terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
				}
				foreach (Warp expr_261 in lcl_indoors3.warps)
				{
					expr_261.TargetX = this.humanDoor.X + this.tileX;
					expr_261.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				if (lcl_indoors3 is AnimalHouse)
				{
					AnimalHouse a = lcl_indoors3 as AnimalHouse;
					string a2 = this.buildingType.Split(new char[]
					{
						' '
					})[0];
					if (!(a2 == "Big"))
					{
						if (!(a2 == "Deluxe"))
						{
							a.animalLimit = 4;
						}
						else
						{
							a.animalLimit = 12;
						}
					}
					else
					{
						a.animalLimit = 8;
					}
				}
				return lcl_indoors3;
			}
			return null;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0017A590 File Offset: 0x00178790
		public virtual Rectangle getRectForAnimalDoor()
		{
			return new Rectangle((this.animalDoor.X + this.tileX) * Game1.tileSize, (this.tileY + this.animalDoor.Y) * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0017A5DC File Offset: 0x001787DC
		public virtual void load()
		{
			this.texture = Game1.content.Load<Texture2D>("Buildings\\" + this.buildingType);
			GameLocation baseLocation = this.getIndoors();
			if (baseLocation != null)
			{
				baseLocation.characters = this.indoors.characters;
				baseLocation.objects = this.indoors.objects;
				baseLocation.terrainFeatures = this.indoors.terrainFeatures;
				baseLocation.IsFarm = true;
				baseLocation.IsOutdoors = false;
				baseLocation.isStructure = true;
				baseLocation.uniqueName = baseLocation.name + (this.tileX * 2000 + this.tileY);
				baseLocation.numberOfSpawnedObjectsOnMap = this.indoors.numberOfSpawnedObjectsOnMap;
				if (this.indoors.GetType() == typeof(AnimalHouse))
				{
					((AnimalHouse)baseLocation).animals = ((AnimalHouse)this.indoors).animals;
					((AnimalHouse)baseLocation).animalsThatLiveHere = ((AnimalHouse)this.indoors).animalsThatLiveHere;
					foreach (KeyValuePair<long, FarmAnimal> kvp in ((AnimalHouse)baseLocation).animals)
					{
						kvp.Value.reload();
					}
				}
				if (this.indoors is Shed)
				{
					((Shed)baseLocation).furniture = ((Shed)this.indoors).furniture;
					using (List<Furniture>.Enumerator enumerator2 = ((Shed)baseLocation).furniture.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.updateDrawPosition();
						}
					}
					((Shed)baseLocation).wallPaper = ((Shed)this.indoors).wallPaper;
					((Shed)baseLocation).floor = ((Shed)this.indoors).floor;
				}
				this.indoors = baseLocation;
				baseLocation = null;
				foreach (Warp expr_1FE in this.indoors.warps)
				{
					expr_1FE.TargetX = this.humanDoor.X + this.tileX;
					expr_1FE.TargetY = this.humanDoor.Y + this.tileY + 1;
				}
				if (this.indoors.IsFarm && this.indoors.terrainFeatures == null)
				{
					this.indoors.terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
				}
				using (List<NPC>.Enumerator enumerator4 = this.indoors.characters.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						enumerator4.Current.reloadSprite();
					}
				}
				using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator5 = this.indoors.terrainFeatures.Values.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						enumerator5.Current.loadSprite();
					}
				}
				foreach (KeyValuePair<Vector2, Object> v in this.indoors.objects)
				{
					v.Value.initializeLightSource(v.Key);
					v.Value.reloadSprite();
				}
				if (this.indoors is AnimalHouse)
				{
					AnimalHouse a = this.indoors as AnimalHouse;
					string a2 = this.buildingType.Split(new char[]
					{
						' '
					})[0];
					if (a2 == "Big")
					{
						a.animalLimit = 8;
						return;
					}
					if (a2 == "Deluxe")
					{
						a.animalLimit = 12;
						return;
					}
					a.animalLimit = 4;
				}
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0017A9E0 File Offset: 0x00178BE0
		public bool isUnderConstruction()
		{
			return this.daysOfConstructionLeft > 0;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0017A9EC File Offset: 0x00178BEC
		public virtual bool isTilePassable(Vector2 tile)
		{
			return tile.X < (float)this.tileX || tile.X >= (float)(this.tileX + this.tilesWide) || tile.Y < (float)this.tileY || tile.Y >= (float)(this.tileY + this.tilesHigh);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0017AA4C File Offset: 0x00178C4C
		public virtual bool intersects(Rectangle boundingBox)
		{
			return new Rectangle(this.tileX * Game1.tileSize, this.tileY * Game1.tileSize, this.tilesWide * Game1.tileSize, this.tilesHigh * Game1.tileSize).Intersects(boundingBox);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0017AA98 File Offset: 0x00178C98
		public virtual void drawInMenu(SpriteBatch b, int x, int y)
		{
			if (this.tilesWide <= 8)
			{
				this.drawShadow(b, x, y);
				b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(this.texture.Bounds), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
				return;
			}
			int xOffset = Game1.tileSize + 11 * Game1.pixelZoom;
			int yOffset = Game1.tileSize / 2 - Game1.pixelZoom;
			b.Draw(this.texture, new Vector2((float)(x + xOffset), (float)(y + yOffset)), new Rectangle?(new Rectangle(this.texture.Bounds.Center.X - 64, this.texture.Bounds.Bottom - 136 - 2, 122, 138)), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0017ABA8 File Offset: 0x00178DA8
		public virtual void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				this.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.texture.Bounds), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			if (this.magical && this.buildingType.Equals("Gold Clock"))
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.hourHandSource), Color.White * this.alpha, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1200) / 1200f) + (double)((float)Game1.gameTimeInterval / 7000f / 23f)), new Vector2(2.5f, 8f), (float)(Game1.pixelZoom * 3) / 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.minuteHandSource), Color.White * this.alpha, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1000 % 100 % 60) / 60f) + (double)((float)Game1.gameTimeInterval / 7000f * 1.02f)), new Vector2(2.5f, 12f), (float)(Game1.pixelZoom * 3) / 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.00011f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.clockNub), Color.White * this.alpha, 0f, new Vector2(2f, 2f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.00012f);
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0017AED4 File Offset: 0x001790D4
		public virtual void drawShadow(SpriteBatch b, int localX = -1, int localY = -1)
		{
			Vector2 basePosition = (localX == -1) ? Game1.GlobalToLocal(new Vector2((float)(this.tileX * Game1.tileSize), (float)((this.tileY + this.tilesHigh) * Game1.tileSize))) : new Vector2((float)localX, (float)(localY + this.getSourceRectForMenu().Height * Game1.pixelZoom));
			b.Draw(Game1.mouseCursors, basePosition, new Rectangle?(Building.leftShadow), Color.White * ((localX == -1) ? this.alpha : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			for (int x = 1; x < this.tilesWide - 1; x++)
			{
				b.Draw(Game1.mouseCursors, basePosition + new Vector2((float)(x * Game1.tileSize), 0f), new Rectangle?(Building.middleShadow), Color.White * ((localX == -1) ? this.alpha : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
			}
			b.Draw(Game1.mouseCursors, basePosition + new Vector2((float)((this.tilesWide - 1) * Game1.tileSize), 0f), new Rectangle?(Building.rightShadow), Color.White * ((localX == -1) ? this.alpha : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0017B04C File Offset: 0x0017924C
		public void drawInConstruction(SpriteBatch b)
		{
			int drawPercentage = Math.Min(16, Math.Max(0, (int)(16f - (float)this.newConstructionTimer / 1000f * 16f)));
			float drawPercentageReal = (float)(2000 - this.newConstructionTimer) / 2000f;
			if (this.magical)
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize) + (float)(this.texture.Bounds.Height * Game1.pixelZoom) * (1f - drawPercentageReal))), new Rectangle?(new Rectangle(0, (int)((float)this.texture.Bounds.Bottom - drawPercentageReal * (float)this.texture.Bounds.Height), this.getSourceRectForMenu().Width, (int)((float)this.texture.Bounds.Height * drawPercentageReal))), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
				for (int i = 0; i < this.tilesWide * 4; i++)
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + i * (Game1.tileSize / 4)), (float)(this.tileY * Game1.tileSize - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize) + (float)(this.texture.Bounds.Height * Game1.pixelZoom) * (1f - drawPercentageReal))) + new Vector2((float)Game1.random.Next(-1, 2), (float)(Game1.random.Next(-1, 2) - ((i % 2 == 0) ? (Game1.pixelZoom * 8) : (Game1.pixelZoom * 2)))), new Rectangle?(new Rectangle(536 + (this.newConstructionTimer + i * 4) % 56 / 8 * 8, 1945, 8, 8)), (i % 2 == 1) ? (Color.Pink * this.alpha) : (Color.LightPink * this.alpha), 0f, new Vector2(0f, 0f), 4f + (float)Game1.random.Next(100) / 100f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
					if (i % 2 == 0)
					{
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + i * (Game1.tileSize / 4)), (float)(this.tileY * Game1.tileSize - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize) + (float)(this.texture.Bounds.Height * Game1.pixelZoom) * (1f - drawPercentageReal))) + new Vector2((float)Game1.random.Next(-1, 2), (float)(Game1.random.Next(-1, 2) + ((i % 2 == 0) ? (Game1.pixelZoom * 8) : (Game1.pixelZoom * 2)))), new Rectangle?(new Rectangle(536 + (this.newConstructionTimer + i * 4) % 56 / 8 * 8, 1945, 8, 8)), Color.White * this.alpha, 0f, new Vector2(0f, 0f), 4f + (float)Game1.random.Next(100) / 100f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
					}
				}
				return;
			}
			bool drawFloor = this.daysOfConstructionLeft == 1;
			for (int x = this.tileX; x < this.tileX + this.tilesWide; x++)
			{
				for (int y = this.tileY; y < this.tileY + this.tilesHigh; y++)
				{
					if (x == this.tileX + this.tilesWide / 2 && y == this.tileY + this.tilesHigh - 1)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4 - Game1.pixelZoom)), new Rectangle?(new Rectangle(367, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 309, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (x == this.tileX && y == this.tileY)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 293, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (x == this.tileX + this.tilesWide - 1 && y == this.tileY)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 293, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (x == this.tileX + this.tilesWide - 1 && y == this.tileY + this.tilesHigh - 1)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 325, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize) / 10000f);
					}
					else if (x == this.tileX && y == this.tileY + this.tilesHigh - 1)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 325, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize) / 10000f);
					}
					else if (x == this.tileX + this.tilesWide - 1)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 309, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize) / 10000f);
					}
					else if (y == this.tileY + this.tilesHigh - 1)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 277, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 325, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize) / 10000f);
					}
					else if (x == this.tileX)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 309, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize) / 10000f);
					}
					else if (y == this.tileY)
					{
						if (drawFloor)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
						}
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom)) + ((this.newConstructionTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 293, 16, drawPercentage)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize - 1) / 10000f);
					}
					else if (drawFloor)
					{
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize) + new Vector2(0f, (float)(Game1.tileSize - drawPercentage * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 261, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-05f);
					}
				}
			}
		}

		// Token: 0x04001322 RID: 4898
		public GameLocation indoors;

		// Token: 0x04001323 RID: 4899
		[XmlIgnore]
		public Texture2D texture;

		// Token: 0x04001324 RID: 4900
		public int tileX;

		// Token: 0x04001325 RID: 4901
		public int tileY;

		// Token: 0x04001326 RID: 4902
		public int tilesWide;

		// Token: 0x04001327 RID: 4903
		public int tilesHigh;

		// Token: 0x04001328 RID: 4904
		public int maxOccupants;

		// Token: 0x04001329 RID: 4905
		public int currentOccupants;

		// Token: 0x0400132A RID: 4906
		public int daysOfConstructionLeft;

		// Token: 0x0400132B RID: 4907
		public int daysUntilUpgrade;

		// Token: 0x0400132C RID: 4908
		public string buildingType;

		// Token: 0x0400132D RID: 4909
		public string nameOfIndoors;

		// Token: 0x0400132E RID: 4910
		public string baseNameOfIndoors;

		// Token: 0x0400132F RID: 4911
		public string nameOfIndoorsWithoutUnique;

		// Token: 0x04001330 RID: 4912
		public Point humanDoor;

		// Token: 0x04001331 RID: 4913
		public Point animalDoor;

		// Token: 0x04001332 RID: 4914
		public Color color = Color.White;

		// Token: 0x04001333 RID: 4915
		public bool animalDoorOpen;

		// Token: 0x04001334 RID: 4916
		public bool magical;

		// Token: 0x04001335 RID: 4917
		public long owner;

		// Token: 0x04001336 RID: 4918
		private int newConstructionTimer;

		// Token: 0x04001337 RID: 4919
		protected float alpha;

		// Token: 0x04001338 RID: 4920
		public static Rectangle leftShadow = new Rectangle(656, 394, 16, 16);

		// Token: 0x04001339 RID: 4921
		public static Rectangle middleShadow = new Rectangle(672, 394, 16, 16);

		// Token: 0x0400133A RID: 4922
		public static Rectangle rightShadow = new Rectangle(688, 394, 16, 16);
	}
}
