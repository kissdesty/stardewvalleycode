using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley.Locations;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	// Token: 0x020000DC RID: 220
	public class CataloguePage : IClickableMenu
	{
		// Token: 0x06000DB7 RID: 3511 RVA: 0x00116AF0 File Offset: 0x00114CF0
		public CataloguePage(int x, int y, int width, int height, GameMenu parent) : base(x, y, width, height, false)
		{
			this.parent = parent;
			this.buildingPlacementTiles = Game1.content.Load<Texture2D>("LooseSprites\\buildingPlacementTiles");
			CataloguePage.widthToMoveActiveTab = Game1.tileSize / 8;
			CataloguePage.blueprintButtonMargin = Game1.tileSize / 2;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, false, null, null, -1, 3, 0, 0, true);
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4 + CataloguePage.widthToMoveActiveTab, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), "", "Buildings", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 4, -1, -1), 1f, false));
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), "", "Building Upgrades", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 5, -1, -1), 1f, false));
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 4, Game1.tileSize, Game1.tileSize), "", "Animals", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 8, -1, -1), 1f, false));
			this.sideTabs.Add(new ClickableTextureComponent("", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 5, Game1.tileSize, Game1.tileSize), "", "Demolish Buildings", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 6, -1, -1), 1f, false));
			for (int i = 0; i < 4; i++)
			{
				this.blueprintButtons.Add(new Dictionary<ClickableComponent, BluePrint>());
			}
			int widthOfBlueprintSpace = Game1.tileSize * 8;
			int[] rowWidthTally = new int[4];
			for (int j = 0; j < Game1.player.blueprints.Count; j++)
			{
				BluePrint print = new BluePrint(Game1.player.blueprints[j]);
				if (CataloguePage.canPlaceThisBuildingOnTheCurrentMap(print, Game1.currentLocation))
				{
					print.canBuildOnCurrentMap = true;
				}
				int tabNumber = this.getTabNumberFromName(print.blueprintType);
				if (print.blueprintType != null)
				{
					int printWidth = (int)((float)Math.Max(print.tilesWidth, 4) / 4f * (float)Game1.tileSize) + CataloguePage.blueprintButtonMargin;
					if (rowWidthTally[tabNumber] % (widthOfBlueprintSpace - IClickableMenu.borderWidth * 2) + printWidth > widthOfBlueprintSpace - IClickableMenu.borderWidth * 2)
					{
						rowWidthTally[tabNumber] += widthOfBlueprintSpace - IClickableMenu.borderWidth * 2 - rowWidthTally[tabNumber] % (widthOfBlueprintSpace - IClickableMenu.borderWidth * 2);
					}
					this.blueprintButtons[Math.Min(3, tabNumber)].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + rowWidthTally[tabNumber] % (widthOfBlueprintSpace - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + rowWidthTally[tabNumber] / (widthOfBlueprintSpace - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, printWidth, Game1.tileSize * 2), print.name), print);
					rowWidthTally[tabNumber] += printWidth;
				}
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00116EBC File Offset: 0x001150BC
		public int getTabNumberFromName(string name)
		{
			int whichTab = -1;
			if (!(name == "Buildings"))
			{
				if (!(name == "Upgrades"))
				{
					if (!(name == "Demolish"))
					{
						if (name == "Animals")
						{
							whichTab = 2;
						}
					}
					else
					{
						whichTab = 3;
					}
				}
				else
				{
					whichTab = 1;
				}
			}
			else
			{
				whichTab = 0;
			}
			return whichTab;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00116F10 File Offset: 0x00115110
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.placingStructure)
			{
				this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
				for (int i = 0; i < this.sideTabs.Count; i++)
				{
					if (this.sideTabs[i].containsPoint(x, y) && this.currentTab != i)
					{
						Game1.playSound("smallSelect");
						if (i == 3)
						{
							this.placingStructure = true;
							this.demolishing = true;
							this.parent.invisible = true;
						}
						else
						{
							ClickableTextureComponent expr_8F_cp_0_cp_0 = this.sideTabs[this.currentTab];
							expr_8F_cp_0_cp_0.bounds.X = expr_8F_cp_0_cp_0.bounds.X - CataloguePage.widthToMoveActiveTab;
							this.currentTab = i;
							ClickableTextureComponent expr_B5_cp_0_cp_0 = this.sideTabs[i];
							expr_B5_cp_0_cp_0.bounds.X = expr_B5_cp_0_cp_0.bounds.X + CataloguePage.widthToMoveActiveTab;
						}
					}
				}
				using (Dictionary<ClickableComponent, BluePrint>.KeyCollection.Enumerator enumerator = this.blueprintButtons[this.currentTab].Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableComponent c = enumerator.Current;
						if (c.containsPoint(x, y))
						{
							if (this.blueprintButtons[this.currentTab][c].doesFarmerHaveEnoughResourcesToBuild())
							{
								this.structureForPlacement = this.blueprintButtons[this.currentTab][c];
								this.placingStructure = true;
								this.parent.invisible = true;
								if (this.currentTab == 1)
								{
									this.upgrading = true;
								}
								Game1.playSound("smallSelect");
								break;
							}
							Game1.addHUDMessage(new HUDMessage("Not Enough Resources", Color.Red, 3500f));
							break;
						}
					}
					return;
				}
			}
			if (this.demolishing)
			{
				if (!(Game1.currentLocation is Farm))
				{
					return;
				}
				if (Game1.IsClient)
				{
					Game1.addHUDMessage(new HUDMessage("Only the farm owner can demolish.", Color.Red, 3500f));
					return;
				}
				Vector2 tileLocation = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
				Building destroyed = ((Farm)Game1.currentLocation).getBuildingAt(tileLocation);
				if (Game1.IsMultiplayer && destroyed != null && destroyed.indoors.farmers.Count > 0)
				{
					Game1.addHUDMessage(new HUDMessage("Someone's in there!", Color.Red, 3500f));
					return;
				}
				if (destroyed == null || !((Farm)Game1.currentLocation).destroyStructure(destroyed))
				{
					this.parent.invisible = false;
					this.placingStructure = false;
					this.demolishing = false;
					return;
				}
				int groundYTile = destroyed.tileY + destroyed.tilesHigh;
				for (int j = 0; j < destroyed.texture.Bounds.Height / Game1.tileSize; j++)
				{
					Game1.createRadialDebris(Game1.currentLocation, destroyed.texture, new Microsoft.Xna.Framework.Rectangle(destroyed.texture.Bounds.Center.X, destroyed.texture.Bounds.Center.Y, Game1.tileSize / 16, Game1.tileSize / 16), destroyed.tileX + Game1.random.Next(destroyed.tilesWide), destroyed.tileY + destroyed.tilesHigh - j, Game1.random.Next(20, 45), groundYTile);
				}
				Game1.playSound("explosion");
				Utility.spreadAnimalsAround(destroyed, (Farm)Game1.currentLocation);
				if (Game1.IsServer)
				{
					MultiplayerUtility.broadcastBuildingChange(1, tileLocation, "", Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
					return;
				}
			}
			else
			{
				if (this.upgrading && Game1.currentLocation.GetType() == typeof(Farm))
				{
					(Game1.currentLocation as Farm).tryToUpgrade(((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize))), this.structureForPlacement);
					return;
				}
				if (!CataloguePage.canPlaceThisBuildingOnTheCurrentMap(this.structureForPlacement, Game1.currentLocation))
				{
					Game1.addHUDMessage(new HUDMessage("You can't build that in this location.", Color.Red, 3500f));
					return;
				}
				if (!this.structureForPlacement.doesFarmerHaveEnoughResourcesToBuild())
				{
					Game1.addHUDMessage(new HUDMessage("Not Enough Resources", Color.Red, 3500f));
					return;
				}
				if (this.tryToBuild())
				{
					this.structureForPlacement.consumeResources();
					if (!this.structureForPlacement.blueprintType.Equals("Animals"))
					{
						Game1.playSound("axe");
						return;
					}
				}
				else if (!Game1.IsClient)
				{
					Game1.addHUDMessage(new HUDMessage("Can't Build There", Color.Red, 3500f));
				}
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00117414 File Offset: 0x00115614
		public static bool canPlaceThisBuildingOnTheCurrentMap(BluePrint structureToPlace, GameLocation map)
		{
			return true;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00117424 File Offset: 0x00115624
		private bool tryToBuild()
		{
			if (this.structureForPlacement.blueprintType.Equals("Animals"))
			{
				return ((Farm)Game1.getLocationFromName("Farm")).placeAnimal(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player.uniqueMultiplayerID);
			}
			return (Game1.currentLocation as BuildableGameLocation).buildStructure(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, false);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x001174F0 File Offset: 0x001156F0
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.placingStructure)
			{
				this.placingStructure = false;
				this.upgrading = false;
				this.demolishing = false;
				this.parent.invisible = false;
				return;
			}
			this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00117541 File Offset: 0x00115741
		public override bool readyToClose()
		{
			return this.heldItem == null && !this.placingStructure;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00117558 File Offset: 0x00115758
		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			foreach (ClickableTextureComponent c in this.sideTabs)
			{
				if (c.containsPoint(x, y))
				{
					this.hoverText = c.hoverText;
					return;
				}
			}
			bool overAnyButton = false;
			foreach (ClickableComponent c2 in this.blueprintButtons[this.currentTab].Keys)
			{
				if (c2.containsPoint(x, y))
				{
					c2.scale = Math.Min(c2.scale + 0.01f, 1.1f);
					this.hoveredItem = this.blueprintButtons[this.currentTab][c2];
					overAnyButton = true;
				}
				else
				{
					c2.scale = Math.Max(c2.scale - 0.01f, 1f);
				}
			}
			if (this.demolishing)
			{
				using (List<Building>.Enumerator enumerator3 = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.color = Color.White;
					}
				}
				Building b = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (b != null)
				{
					b.color = Color.Red * 0.8f;
				}
			}
			else if (this.upgrading)
			{
				using (List<Building>.Enumerator enumerator3 = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.color = Color.White;
					}
				}
				Building b2 = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (b2 != null && this.structureForPlacement.nameOfBuildingToUpgrade != null && this.structureForPlacement.nameOfBuildingToUpgrade.Equals(b2.buildingType))
				{
					b2.color = Color.Green * 0.8f;
				}
				else if (b2 != null)
				{
					b2.color = Color.Red * 0.8f;
				}
			}
			if (!overAnyButton)
			{
				this.hoveredItem = null;
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00117864 File Offset: 0x00115A64
		private int getTileSheetIndexForStructurePlacementTile(int x, int y)
		{
			if (x == this.structureForPlacement.humanDoor.X && y == this.structureForPlacement.humanDoor.Y)
			{
				return 2;
			}
			if (x == this.structureForPlacement.animalDoor.X && y == this.structureForPlacement.animalDoor.Y)
			{
				return 4;
			}
			return 0;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x001178C4 File Offset: 0x00115AC4
		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.placingStructure)
			{
				this.placingStructure = false;
				this.upgrading = false;
				this.demolishing = false;
				this.parent.invisible = false;
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00117914 File Offset: 0x00115B14
		public override void draw(SpriteBatch b)
		{
			if (!this.placingStructure)
			{
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.sideTabs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
				base.drawVerticalUpperIntersectingPartition(b, this.xPositionOnScreen + Game1.tileSize * 9, 5 * Game1.tileSize + Game1.tileSize / 8);
				this.inventory.draw(b);
				foreach (ClickableComponent c in this.blueprintButtons[this.currentTab].Keys)
				{
					Texture2D structureTexture = this.blueprintButtons[this.currentTab][c].texture;
					Vector2 origin = new Vector2((float)this.blueprintButtons[this.currentTab][c].sourceRectForMenuView.Center.X, (float)this.blueprintButtons[this.currentTab][c].sourceRectForMenuView.Center.Y);
					b.Draw(structureTexture, new Vector2((float)c.bounds.Center.X, (float)c.bounds.Center.Y), new Microsoft.Xna.Framework.Rectangle?(this.blueprintButtons[this.currentTab][c].sourceRectForMenuView), this.blueprintButtons[this.currentTab][c].canBuildOnCurrentMap ? Color.White : (Color.Gray * 0.8f), 0f, origin, 1f * c.scale + ((this.currentTab == 2) ? 0.75f : 0f), SpriteEffects.None, 0.9f);
				}
				if (this.hoveredItem != null)
				{
					this.hoveredItem.drawDescription(b, this.xPositionOnScreen + Game1.tileSize * 9 + Game1.tileSize * 2 / 3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 2, Game1.tileSize * 3 + Game1.tileSize / 2);
				}
				if (this.heldItem != null)
				{
					this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
				}
				if (!this.hoverText.Equals(""))
				{
					IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
					return;
				}
			}
			else if (!this.demolishing && !this.upgrading)
			{
				Vector2 mousePositionTile = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
				for (int y = 0; y < this.structureForPlacement.tilesHeight; y++)
				{
					for (int x = 0; x < this.structureForPlacement.tilesWidth; x++)
					{
						int sheetIndex = this.getTileSheetIndexForStructurePlacementTile(x, y);
						Vector2 currentGlobalTilePosition = new Vector2(mousePositionTile.X + (float)x, mousePositionTile.Y + (float)y);
						if (Game1.player.getTileLocation().Equals(currentGlobalTilePosition) || Game1.currentLocation.isTileOccupied(currentGlobalTilePosition, "") || !Game1.currentLocation.isTilePassable(new Location((int)currentGlobalTilePosition.X, (int)currentGlobalTilePosition.Y), Game1.viewport))
						{
							sheetIndex++;
						}
						b.Draw(this.buildingPlacementTiles, Game1.GlobalToLocal(Game1.viewport, currentGlobalTilePosition * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(this.buildingPlacementTiles, sheetIndex, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
					}
				}
			}
		}

		// Token: 0x04000E72 RID: 3698
		public static int widthToMoveActiveTab = Game1.tileSize / 8;

		// Token: 0x04000E73 RID: 3699
		public static int blueprintButtonMargin = Game1.tileSize / 2;

		// Token: 0x04000E74 RID: 3700
		public const int buildingsTab = 0;

		// Token: 0x04000E75 RID: 3701
		public const int upgradesTab = 1;

		// Token: 0x04000E76 RID: 3702
		public const int animalsTab = 2;

		// Token: 0x04000E77 RID: 3703
		public const int demolishTab = 3;

		// Token: 0x04000E78 RID: 3704
		public const int numberOfTabs = 4;

		// Token: 0x04000E79 RID: 3705
		private string descriptionText = "";

		// Token: 0x04000E7A RID: 3706
		private string hoverText = "";

		// Token: 0x04000E7B RID: 3707
		private InventoryMenu inventory;

		// Token: 0x04000E7C RID: 3708
		private Item heldItem;

		// Token: 0x04000E7D RID: 3709
		private int currentTab;

		// Token: 0x04000E7E RID: 3710
		private BluePrint hoveredItem;

		// Token: 0x04000E7F RID: 3711
		private List<ClickableTextureComponent> sideTabs = new List<ClickableTextureComponent>();

		// Token: 0x04000E80 RID: 3712
		private List<Dictionary<ClickableComponent, BluePrint>> blueprintButtons = new List<Dictionary<ClickableComponent, BluePrint>>();

		// Token: 0x04000E81 RID: 3713
		private bool demolishing;

		// Token: 0x04000E82 RID: 3714
		private bool upgrading;

		// Token: 0x04000E83 RID: 3715
		private bool placingStructure;

		// Token: 0x04000E84 RID: 3716
		private BluePrint structureForPlacement;

		// Token: 0x04000E85 RID: 3717
		private GameMenu parent;

		// Token: 0x04000E86 RID: 3718
		private Texture2D buildingPlacementTiles;
	}
}
