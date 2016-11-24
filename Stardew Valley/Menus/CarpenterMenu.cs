using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Locations;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	// Token: 0x020000DB RID: 219
	public class CarpenterMenu : IClickableMenu
	{
		// Token: 0x170000D7 RID: 215
		public BluePrint CurrentBlueprint
		{
			// Token: 0x06000DA6 RID: 3494 RVA: 0x001147F5 File Offset: 0x001129F5
			get
			{
				return this.blueprints[this.currentBlueprintIndex];
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00114808 File Offset: 0x00112A08
		public CarpenterMenu(bool magicalConstruction = false)
		{
			this.magicalConstruction = magicalConstruction;
			Game1.player.forceCanMove();
			this.resetBounds();
			this.blueprints = new List<BluePrint>();
			if (magicalConstruction)
			{
				this.blueprints.Add(new BluePrint("Junimo Hut"));
				this.blueprints.Add(new BluePrint("Earth Obelisk"));
				this.blueprints.Add(new BluePrint("Water Obelisk"));
				this.blueprints.Add(new BluePrint("Gold Clock"));
			}
			else
			{
				this.blueprints.Add(new BluePrint("Coop"));
				this.blueprints.Add(new BluePrint("Barn"));
				this.blueprints.Add(new BluePrint("Well"));
				this.blueprints.Add(new BluePrint("Silo"));
				this.blueprints.Add(new BluePrint("Mill"));
				this.blueprints.Add(new BluePrint("Shed"));
				if (!Game1.getFarm().isBuildingConstructed("Stable"))
				{
					this.blueprints.Add(new BluePrint("Stable"));
				}
				this.blueprints.Add(new BluePrint("Slime Hutch"));
				if (Game1.getFarm().isBuildingConstructed("Coop"))
				{
					this.blueprints.Add(new BluePrint("Big Coop"));
				}
				if (Game1.getFarm().isBuildingConstructed("Big Coop"))
				{
					this.blueprints.Add(new BluePrint("Deluxe Coop"));
				}
				if (Game1.getFarm().isBuildingConstructed("Barn"))
				{
					this.blueprints.Add(new BluePrint("Big Barn"));
				}
				if (Game1.getFarm().isBuildingConstructed("Big Barn"))
				{
					this.blueprints.Add(new BluePrint("Deluxe Barn"));
				}
			}
			this.setNewActiveBlueprint();
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00114A34 File Offset: 0x00112C34
		private void resetBounds()
		{
			this.xPositionOnScreen = Game1.viewport.Width / 2 - this.maxWidthOfBuildingViewer - IClickableMenu.spaceToClearSideBorder;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - this.maxHeightOfBuildingViewer / 2 - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2;
			this.width = this.maxWidthOfBuildingViewer + this.maxWidthOfDescription + IClickableMenu.spaceToClearSideBorder * 2 + Game1.tileSize;
			this.height = this.maxHeightOfBuildingViewer + IClickableMenu.spaceToClearTopBorder;
			base.initialize(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, true);
			this.okButton = new ClickableTextureComponent("OK", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 3 - Game1.pixelZoom * 3, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(366, 373, 16, 16), (float)Game1.pixelZoom, false);
			this.cancelButton = new ClickableTextureComponent("OK", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
			this.backButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.maxWidthOfBuildingViewer - Game1.tileSize * 4 + Game1.tileSize / 4, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.demolishButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\UI:Carpenter_Demolish", new object[0]), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2 - Game1.pixelZoom * 2, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize - Game1.pixelZoom, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(348, 372, 17, 17), (float)Game1.pixelZoom, false);
			this.upgradeIcon = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.maxWidthOfBuildingViewer - Game1.tileSize * 2 + Game1.tileSize / 2, this.yPositionOnScreen + Game1.pixelZoom * 2, 9 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(402, 328, 9, 13), (float)Game1.pixelZoom, false);
			this.moveButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\UI:Carpenter_MoveBuildings", new object[0]), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 4 - Game1.pixelZoom * 5, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(257, 284, 16, 16), (float)Game1.pixelZoom, false);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00114E18 File Offset: 0x00113018
		public void setNewActiveBlueprint()
		{
			if (this.blueprints[this.currentBlueprintIndex].name.Contains("Coop"))
			{
				this.currentBuilding = new Coop(this.blueprints[this.currentBlueprintIndex], Vector2.Zero);
			}
			else if (this.blueprints[this.currentBlueprintIndex].name.Contains("Barn"))
			{
				this.currentBuilding = new Barn(this.blueprints[this.currentBlueprintIndex], Vector2.Zero);
			}
			else if (this.blueprints[this.currentBlueprintIndex].name.Contains("Mill"))
			{
				this.currentBuilding = new Mill(this.blueprints[this.currentBlueprintIndex], Vector2.Zero);
			}
			else if (this.blueprints[this.currentBlueprintIndex].name.Contains("Junimo Hut"))
			{
				this.currentBuilding = new JunimoHut(this.blueprints[this.currentBlueprintIndex], Vector2.Zero);
			}
			else
			{
				this.currentBuilding = new Building(this.blueprints[this.currentBlueprintIndex], Vector2.Zero);
			}
			this.price = this.blueprints[this.currentBlueprintIndex].moneyRequired;
			this.ingredients.Clear();
			foreach (KeyValuePair<int, int> v in this.blueprints[this.currentBlueprintIndex].itemsRequired)
			{
				this.ingredients.Add(new Object(v.Key, v.Value, false, -1, 0));
			}
			this.buildingDescription = this.blueprints[this.currentBlueprintIndex].description;
			this.buildingName = this.blueprints[this.currentBlueprintIndex].name;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00115030 File Offset: 0x00113230
		public override void performHoverAction(int x, int y)
		{
			this.cancelButton.tryHover(x, y, 0.1f);
			base.performHoverAction(x, y);
			if (this.onFarm)
			{
				if ((this.upgrading || this.demolishing || this.moving) && !this.freeze)
				{
					using (List<Building>.Enumerator enumerator = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.color = Color.White;
						}
					}
					Building b = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
					if (b == null)
					{
						b = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY() + Game1.tileSize * 2) / Game1.tileSize)));
						if (b == null)
						{
							b = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY() + Game1.tileSize * 3) / Game1.tileSize)));
						}
					}
					if (this.upgrading)
					{
						if (b != null && this.CurrentBlueprint.nameOfBuildingToUpgrade != null && this.CurrentBlueprint.nameOfBuildingToUpgrade.Equals(b.buildingType))
						{
							b.color = Color.Lime * 0.8f;
							return;
						}
						if (b != null)
						{
							b.color = Color.Red * 0.8f;
							return;
						}
					}
					else if (this.demolishing)
					{
						if (b != null)
						{
							b.color = Color.Red * 0.8f;
							return;
						}
					}
					else if (this.moving && b != null)
					{
						b.color = Color.Lime * 0.8f;
					}
				}
				return;
			}
			this.backButton.tryHover(x, y, 1f);
			this.forwardButton.tryHover(x, y, 1f);
			this.okButton.tryHover(x, y, 0.1f);
			this.demolishButton.tryHover(x, y, 0.1f);
			this.moveButton.tryHover(x, y, 0.1f);
			if (this.CurrentBlueprint.isUpgrade() && this.upgradeIcon.containsPoint(x, y))
			{
				this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_Upgrade", new object[]
				{
					this.CurrentBlueprint.nameOfBuildingToUpgrade
				});
				return;
			}
			if (this.demolishButton.containsPoint(x, y))
			{
				this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_Demolish", new object[0]);
				return;
			}
			if (this.moveButton.containsPoint(x, y))
			{
				this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_MoveBuildings", new object[0]);
				return;
			}
			if (this.okButton.containsPoint(x, y) && this.CurrentBlueprint.doesFarmerHaveEnoughResourcesToBuild())
			{
				this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_Build", new object[0]);
				return;
			}
			this.hoverText = "";
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x001153AC File Offset: 0x001135AC
		public override bool readyToClose()
		{
			return base.readyToClose() && this.buildingToMove == null;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x001153C4 File Offset: 0x001135C4
		public override void receiveKeyPress(Keys key)
		{
			if (this.freeze)
			{
				return;
			}
			if (!this.onFarm)
			{
				base.receiveKeyPress(key);
			}
			if (!Game1.globalFade && this.onFarm)
			{
				if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.returnToCarpentryMenu), 0.02f);
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
				{
					Game1.panScreen(0, 4);
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
				{
					Game1.panScreen(4, 0);
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
				{
					Game1.panScreen(0, -4);
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
				{
					Game1.panScreen(-4, 0);
				}
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x001154B4 File Offset: 0x001136B4
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.onFarm && !Game1.globalFade)
			{
				int mouseX = Game1.getOldMouseX() + Game1.viewport.X;
				int mouseY = Game1.getOldMouseY() + Game1.viewport.Y;
				if (mouseX - Game1.viewport.X < Game1.tileSize)
				{
					Game1.panScreen(-8, 0);
				}
				else if (mouseX - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize * 2)
				{
					Game1.panScreen(8, 0);
				}
				if (mouseY - Game1.viewport.Y < Game1.tileSize)
				{
					Game1.panScreen(0, -8);
				}
				else if (mouseY - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
				{
					Game1.panScreen(0, 8);
				}
				Keys[] pressedKeys = Game1.oldKBState.GetPressedKeys();
				for (int i = 0; i < pressedKeys.Length; i++)
				{
					Keys key = pressedKeys[i];
					this.receiveKeyPress(key);
				}
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x001155B0 File Offset: 0x001137B0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.freeze)
			{
				return;
			}
			if (!this.onFarm)
			{
				base.receiveLeftClick(x, y, playSound);
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				if (!this.onFarm)
				{
					base.exitThisMenu(true);
					Game1.player.forceCanMove();
					Game1.playSound("bigDeSelect");
				}
				else
				{
					if (this.moving && this.buildingToMove != null)
					{
						Game1.playSound("cancel");
						return;
					}
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.returnToCarpentryMenu), 0.02f);
					Game1.playSound("smallSelect");
					return;
				}
			}
			if (!this.onFarm && this.backButton.containsPoint(x, y))
			{
				this.currentBlueprintIndex--;
				if (this.currentBlueprintIndex < 0)
				{
					this.currentBlueprintIndex = this.blueprints.Count - 1;
				}
				this.setNewActiveBlueprint();
				Game1.playSound("shwip");
				this.backButton.scale = this.backButton.baseScale;
			}
			if (!this.onFarm && this.forwardButton.containsPoint(x, y))
			{
				this.currentBlueprintIndex = (this.currentBlueprintIndex + 1) % this.blueprints.Count;
				this.setNewActiveBlueprint();
				this.backButton.scale = this.backButton.baseScale;
				Game1.playSound("shwip");
			}
			if (!this.onFarm && this.demolishButton.containsPoint(x, y))
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForBuildingPlacement), 0.02f);
				Game1.playSound("smallSelect");
				this.onFarm = true;
				this.demolishing = true;
			}
			if (!this.onFarm && this.moveButton.containsPoint(x, y))
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForBuildingPlacement), 0.02f);
				Game1.playSound("smallSelect");
				this.onFarm = true;
				this.moving = true;
			}
			if (this.okButton.containsPoint(x, y) && !this.onFarm && Game1.player.money >= this.price && this.blueprints[this.currentBlueprintIndex].doesFarmerHaveEnoughResourcesToBuild())
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForBuildingPlacement), 0.02f);
				Game1.playSound("smallSelect");
				this.onFarm = true;
			}
			if (this.onFarm && !this.freeze && !Game1.globalFade)
			{
				if (this.demolishing)
				{
					Building destroyed = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
					if (destroyed != null && (destroyed.daysOfConstructionLeft > 0 || destroyed.daysUntilUpgrade > 0))
					{
						Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantDemolish_DuringConstruction", new object[0]), Color.Red, 3500f));
						return;
					}
					if (destroyed != null && destroyed.indoors != null && destroyed.indoors is AnimalHouse && (destroyed.indoors as AnimalHouse).animalsThatLiveHere.Count > 0)
					{
						Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantDemolish_AnimalsHere", new object[0]), Color.Red, 3500f));
						return;
					}
					if (destroyed != null && ((Farm)Game1.getLocationFromName("Farm")).destroyStructure(destroyed))
					{
						int arg_366_0 = destroyed.tileY;
						int arg_36D_0 = destroyed.tilesHigh;
						Game1.flashAlpha = 1f;
						destroyed.showDestroyedAnimation(Game1.getFarm());
						Game1.playSound("explosion");
						Utility.spreadAnimalsAround(destroyed, (Farm)Game1.getLocationFromName("Farm"));
						DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.returnToCarpentryMenu), 1500);
						this.freeze = true;
					}
					return;
				}
				else if (this.upgrading)
				{
					Building toUpgrade = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
					if (toUpgrade != null && this.CurrentBlueprint.name != null && toUpgrade.buildingType.Equals(this.CurrentBlueprint.nameOfBuildingToUpgrade))
					{
						this.CurrentBlueprint.consumeResources();
						toUpgrade.daysUntilUpgrade = 2;
						toUpgrade.showUpgradeAnimation(Game1.getFarm());
						Game1.playSound("axe");
						DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.returnToCarpentryMenuAfterSuccessfulBuild), 1500);
						this.freeze = true;
						return;
					}
					if (toUpgrade != null)
					{
						Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantUpgrade_BuildingType", new object[0]), Color.Red, 3500f));
					}
					return;
				}
				else if (this.moving)
				{
					if (this.buildingToMove == null)
					{
						this.buildingToMove = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getMouseY()) / Game1.tileSize)));
						if (this.buildingToMove != null)
						{
							if (this.buildingToMove.daysOfConstructionLeft > 0)
							{
								this.buildingToMove = null;
								return;
							}
							((Farm)Game1.getLocationFromName("Farm")).buildings.Remove(this.buildingToMove);
							Game1.playSound("axchop");
						}
						return;
					}
					if (((Farm)Game1.getLocationFromName("Farm")).buildStructure(this.buildingToMove, new Vector2((float)((Game1.viewport.X + Game1.getMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getMouseY()) / Game1.tileSize)), false, Game1.player))
					{
						this.buildingToMove = null;
						Game1.playSound("axchop");
						DelayedAction.playSoundAfterDelay("dirtyHit", 50);
						DelayedAction.playSoundAfterDelay("dirtyHit", 150);
						return;
					}
					Game1.playSound("cancel");
					return;
				}
				else
				{
					if (this.tryToBuild())
					{
						this.CurrentBlueprint.consumeResources();
						DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.returnToCarpentryMenuAfterSuccessfulBuild), 2000);
						this.freeze = true;
						return;
					}
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantBuild", new object[0]), Color.Red, 3500f));
				}
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00115C00 File Offset: 0x00113E00
		public bool tryToBuild()
		{
			return ((Farm)Game1.getLocationFromName("Farm")).buildStructure(this.CurrentBlueprint, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, this.magicalConstruction);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00115C68 File Offset: 0x00113E68
		public void returnToCarpentryMenu()
		{
			Game1.currentLocation.cleanupBeforePlayerExit();
			Game1.currentLocation = Game1.getLocationFromName(this.magicalConstruction ? "WizardHouse" : "ScienceHouse");
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.onFarm = false;
			this.resetBounds();
			this.upgrading = false;
			this.moving = false;
			this.freeze = false;
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			Game1.viewport.Location = new Location(5 * Game1.tileSize, 24 * Game1.tileSize);
			this.drawBG = true;
			this.demolishing = false;
			Game1.displayFarmer = true;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00115D14 File Offset: 0x00113F14
		public void returnToCarpentryMenuAfterSuccessfulBuild()
		{
			Game1.currentLocation.cleanupBeforePlayerExit();
			Game1.currentLocation = Game1.getLocationFromName(this.magicalConstruction ? "WizardHouse" : "ScienceHouse");
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(new Game1.afterFadeFunction(this.robinConstructionMessage), 0.02f);
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			Game1.viewport.Location = new Location(5 * Game1.tileSize, 24 * Game1.tileSize);
			this.freeze = true;
			Game1.displayFarmer = true;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00115DA0 File Offset: 0x00113FA0
		public void robinConstructionMessage()
		{
			base.exitThisMenu(true);
			Game1.player.forceCanMove();
			if (!this.magicalConstruction)
			{
				string dialoguePath = "Data\\ExtraDialogue:Robin_" + (this.upgrading ? "Upgrade" : "New") + "Construction";
				if (Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
				{
					dialoguePath += "_Festival";
				}
				Game1.drawDialogue(Game1.getCharacterFromName("Robin", false), Game1.content.LoadString(dialoguePath, new object[]
				{
					this.CurrentBlueprint.name.ToLower(),
					this.CurrentBlueprint.name.ToLower().Split(new char[]
					{
						' '
					}).Last<string>()
				}));
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00115E68 File Offset: 0x00114068
		public void setUpForBuildingPlacement()
		{
			Game1.currentLocation.cleanupBeforePlayerExit();
			this.hoverText = "";
			Game1.currentLocation = Game1.getLocationFromName("Farm");
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.onFarm = true;
			this.cancelButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
			this.cancelButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
			Game1.displayHUD = false;
			Game1.viewportFreeze = true;
			Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
			Game1.panScreen(0, 0);
			this.drawBG = false;
			this.freeze = false;
			Game1.displayFarmer = false;
			if (!this.demolishing && this.CurrentBlueprint.nameOfBuildingToUpgrade != null && this.CurrentBlueprint.nameOfBuildingToUpgrade.Length > 0 && !this.moving)
			{
				this.upgrading = true;
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00115F75 File Offset: 0x00114175
		public override void gameWindowSizeChanged(Microsoft.Xna.Framework.Rectangle oldBounds, Microsoft.Xna.Framework.Rectangle newBounds)
		{
			this.resetBounds();
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00115F80 File Offset: 0x00114180
		public override void draw(SpriteBatch b)
		{
			if (this.drawBG)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.5f);
			}
			if (Game1.globalFade || this.freeze)
			{
				return;
			}
			if (!this.onFarm)
			{
				base.draw(b);
				IClickableMenu.drawTextureBox(b, this.xPositionOnScreen - Game1.tileSize * 3 / 2, this.yPositionOnScreen - Game1.tileSize / 4, this.maxWidthOfBuildingViewer + Game1.tileSize, this.maxHeightOfBuildingViewer + Game1.tileSize, this.magicalConstruction ? Color.RoyalBlue : Color.White);
				this.currentBuilding.drawInMenu(b, this.xPositionOnScreen + this.maxWidthOfBuildingViewer / 2 - this.currentBuilding.tilesWide * Game1.tileSize / 2 - Game1.tileSize, this.yPositionOnScreen + this.maxHeightOfBuildingViewer / 2 - this.currentBuilding.getSourceRectForMenu().Height * Game1.pixelZoom / 2);
				if (this.CurrentBlueprint.isUpgrade())
				{
					this.upgradeIcon.draw(b);
				}
				SpriteText.drawStringWithScrollBackground(b, this.buildingName, this.xPositionOnScreen + this.maxWidthOfBuildingViewer - IClickableMenu.spaceToClearSideBorder - Game1.tileSize / 4 + Game1.tileSize + ((this.width - (this.maxWidthOfBuildingViewer + Game1.tileSize * 2)) / 2 - SpriteText.getWidthOfString("Deluxe Barn") / 2), this.yPositionOnScreen, "Deluxe Barn", 1f, -1);
				IClickableMenu.drawTextureBox(b, this.xPositionOnScreen + this.maxWidthOfBuildingViewer - Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4, this.maxWidthOfDescription + Game1.tileSize, this.maxWidthOfDescription + Game1.tileSize * 3 / 2, this.magicalConstruction ? Color.RoyalBlue : Color.White);
				if (this.magicalConstruction)
				{
					Utility.drawTextWithShadow(b, Game1.parseText(this.buildingDescription, Game1.dialogueFont, this.maxWidthOfDescription + Game1.tileSize / 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize - Game1.pixelZoom), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4 + Game1.pixelZoom)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, 0f, 3);
					Utility.drawTextWithShadow(b, Game1.parseText(this.buildingDescription, Game1.dialogueFont, this.maxWidthOfDescription + Game1.tileSize / 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize - 1), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4 + Game1.pixelZoom)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, 0f, 3);
				}
				Utility.drawTextWithShadow(b, Game1.parseText(this.buildingDescription, Game1.dialogueFont, this.maxWidthOfDescription + Game1.tileSize / 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4)), this.magicalConstruction ? Color.PaleGoldenrod : Game1.textColor, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
				Vector2 ingredientsPosition = new Vector2((float)(this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize / 4 + Game1.tileSize), (float)(this.yPositionOnScreen + Game1.tileSize * 4 + Game1.tileSize / 2));
				SpriteText.drawString(b, "$", (int)ingredientsPosition.X, (int)ingredientsPosition.Y, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				if (this.magicalConstruction)
				{
					Utility.drawTextWithShadow(b, this.price + "g", Game1.dialogueFont, new Vector2(ingredientsPosition.X + (float)Game1.tileSize, ingredientsPosition.Y + (float)(Game1.pixelZoom * 2)), Game1.textColor * 0.5f, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
					Utility.drawTextWithShadow(b, this.price + "g", Game1.dialogueFont, new Vector2(ingredientsPosition.X + (float)Game1.tileSize + (float)Game1.pixelZoom - 1f, ingredientsPosition.Y + (float)(Game1.pixelZoom * 2)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
				}
				Utility.drawTextWithShadow(b, this.price + "g", Game1.dialogueFont, new Vector2(ingredientsPosition.X + (float)Game1.tileSize + (float)Game1.pixelZoom, ingredientsPosition.Y + (float)Game1.pixelZoom), (Game1.player.money >= this.price) ? (this.magicalConstruction ? Color.PaleGoldenrod : Game1.textColor) : Color.Red, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
				ingredientsPosition.X -= (float)(Game1.tileSize / 4);
				ingredientsPosition.Y -= (float)(Game1.tileSize / 3);
				foreach (Item i in this.ingredients)
				{
					ingredientsPosition.Y += (float)(Game1.tileSize + Game1.pixelZoom);
					i.drawInMenu(b, ingredientsPosition, 1f);
					bool hasItem = !(i is Object) || Game1.player.hasItemInInventory((i as Object).parentSheetIndex, i.Stack, 0);
					if (this.magicalConstruction)
					{
						Utility.drawTextWithShadow(b, i.Name, Game1.dialogueFont, new Vector2(ingredientsPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 3), ingredientsPosition.Y + (float)(Game1.pixelZoom * 6)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
						Utility.drawTextWithShadow(b, i.Name, Game1.dialogueFont, new Vector2(ingredientsPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 4) - 1f, ingredientsPosition.Y + (float)(Game1.pixelZoom * 6)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
					}
					Utility.drawTextWithShadow(b, i.Name, Game1.dialogueFont, new Vector2(ingredientsPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 4), ingredientsPosition.Y + (float)(Game1.pixelZoom * 5)), hasItem ? (this.magicalConstruction ? Color.PaleGoldenrod : Game1.textColor) : Color.Red, 1f, -1f, -1, -1, this.magicalConstruction ? 0f : 0.25f, 3);
				}
				this.backButton.draw(b);
				this.forwardButton.draw(b);
				this.okButton.draw(b, this.blueprints[this.currentBlueprintIndex].doesFarmerHaveEnoughResourcesToBuild() ? Color.White : (Color.Gray * 0.8f), 0.88f);
				this.demolishButton.draw(b);
				this.moveButton.draw(b);
			}
			else
			{
				string message = this.upgrading ? Game1.content.LoadString("Strings\\UI:Carpenter_SelectBuilding_Upgrade", new object[]
				{
					this.CurrentBlueprint.nameOfBuildingToUpgrade
				}) : (this.demolishing ? Game1.content.LoadString("Strings\\UI:Carpenter_SelectBuilding_Demolish", new object[0]) : Game1.content.LoadString("Strings\\UI:Carpenter_ChooseLocation", new object[0]));
				SpriteText.drawStringWithScrollBackground(b, message, Game1.viewport.Width / 2 - SpriteText.getWidthOfString(message) / 2, Game1.tileSize / 4, "", 1f, -1);
				if (!this.upgrading && !this.demolishing && !this.moving)
				{
					Vector2 mousePositionTile = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
					for (int y = 0; y < this.CurrentBlueprint.tilesHeight; y++)
					{
						for (int x = 0; x < this.CurrentBlueprint.tilesWidth; x++)
						{
							int sheetIndex = this.CurrentBlueprint.getTileSheetIndexForStructurePlacementTile(x, y);
							Vector2 currentGlobalTilePosition = new Vector2(mousePositionTile.X + (float)x, mousePositionTile.Y + (float)y);
							if (!(Game1.currentLocation as BuildableGameLocation).isBuildable(currentGlobalTilePosition))
							{
								sheetIndex++;
							}
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, currentGlobalTilePosition * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(194 + sheetIndex * 16, 388, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.999f);
						}
					}
				}
				else if (this.moving && this.buildingToMove != null)
				{
					Vector2 mousePositionTile2 = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
					for (int y2 = 0; y2 < this.buildingToMove.tilesHigh; y2++)
					{
						for (int x2 = 0; x2 < this.buildingToMove.tilesWide; x2++)
						{
							int sheetIndex2 = this.buildingToMove.getTileSheetIndexForStructurePlacementTile(x2, y2);
							Vector2 currentGlobalTilePosition2 = new Vector2(mousePositionTile2.X + (float)x2, mousePositionTile2.Y + (float)y2);
							if (!(Game1.currentLocation as BuildableGameLocation).isBuildable(currentGlobalTilePosition2))
							{
								sheetIndex2++;
							}
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, currentGlobalTilePosition2 * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(194 + sheetIndex2 * 16, 388, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.999f);
						}
					}
				}
			}
			this.cancelButton.draw(b);
			base.drawMouse(b);
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04000E58 RID: 3672
		public int maxWidthOfBuildingViewer = 7 * Game1.tileSize;

		// Token: 0x04000E59 RID: 3673
		public int maxHeightOfBuildingViewer = 8 * Game1.tileSize;

		// Token: 0x04000E5A RID: 3674
		public int maxWidthOfDescription = 6 * Game1.tileSize;

		// Token: 0x04000E5B RID: 3675
		private List<BluePrint> blueprints;

		// Token: 0x04000E5C RID: 3676
		private int currentBlueprintIndex;

		// Token: 0x04000E5D RID: 3677
		private ClickableTextureComponent okButton;

		// Token: 0x04000E5E RID: 3678
		private ClickableTextureComponent cancelButton;

		// Token: 0x04000E5F RID: 3679
		private ClickableTextureComponent backButton;

		// Token: 0x04000E60 RID: 3680
		private ClickableTextureComponent forwardButton;

		// Token: 0x04000E61 RID: 3681
		private ClickableTextureComponent upgradeIcon;

		// Token: 0x04000E62 RID: 3682
		private ClickableTextureComponent demolishButton;

		// Token: 0x04000E63 RID: 3683
		private ClickableTextureComponent moveButton;

		// Token: 0x04000E64 RID: 3684
		private Building currentBuilding;

		// Token: 0x04000E65 RID: 3685
		private Building buildingToMove;

		// Token: 0x04000E66 RID: 3686
		private string buildingDescription;

		// Token: 0x04000E67 RID: 3687
		private string buildingName;

		// Token: 0x04000E68 RID: 3688
		private List<Item> ingredients = new List<Item>();

		// Token: 0x04000E69 RID: 3689
		private int price;

		// Token: 0x04000E6A RID: 3690
		private bool onFarm;

		// Token: 0x04000E6B RID: 3691
		private bool drawBG = true;

		// Token: 0x04000E6C RID: 3692
		private bool freeze;

		// Token: 0x04000E6D RID: 3693
		private bool upgrading;

		// Token: 0x04000E6E RID: 3694
		private bool demolishing;

		// Token: 0x04000E6F RID: 3695
		private bool moving;

		// Token: 0x04000E70 RID: 3696
		private bool magicalConstruction;

		// Token: 0x04000E71 RID: 3697
		private string hoverText = "";
	}
}
