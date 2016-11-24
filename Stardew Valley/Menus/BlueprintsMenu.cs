using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	// Token: 0x020000D7 RID: 215
	public class BlueprintsMenu : IClickableMenu
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x001108E8 File Offset: 0x0010EAE8
		public BlueprintsMenu(int x, int y) : base(x, y, Game1.viewport.Width / 2 + Game1.tileSize * 3 / 2, 0, false)
		{
			BlueprintsMenu.tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
			BlueprintsMenu.blueprintButtonMargin = Game1.tileSize / 2;
			BlueprintsMenu.heightOfDescriptionBox = Game1.tileSize * 6;
			for (int i = 0; i < 5; i++)
			{
				this.blueprintButtons.Add(new Dictionary<ClickableComponent, BluePrint>());
			}
			this.xPositionOnScreen = x;
			this.yPositionOnScreen = y;
			int[] rowWidthTally = new int[5];
			for (int j = 0; j < Game1.player.blueprints.Count; j++)
			{
				BluePrint print = new BluePrint(Game1.player.blueprints[j]);
				int tabNumber = this.getTabNumberFromName(print.blueprintType);
				if (print.blueprintType != null)
				{
					int printWidth = (int)((float)Math.Max(print.tilesWidth, 4) / 4f * (float)Game1.tileSize) + BlueprintsMenu.blueprintButtonMargin;
					if (rowWidthTally[tabNumber] % (this.width - IClickableMenu.borderWidth * 2) + printWidth > this.width - IClickableMenu.borderWidth * 2)
					{
						rowWidthTally[tabNumber] += this.width - IClickableMenu.borderWidth * 2 - rowWidthTally[tabNumber] % (this.width - IClickableMenu.borderWidth * 2);
					}
					this.blueprintButtons[Math.Min(4, tabNumber)].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + rowWidthTally[tabNumber] % (this.width - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + rowWidthTally[tabNumber] / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, printWidth, Game1.tileSize * 2), print.name), print);
					rowWidthTally[tabNumber] += printWidth;
				}
			}
			this.blueprintButtons[4].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + rowWidthTally[4] % (this.width - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + rowWidthTally[4] / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, Game1.tileSize + BlueprintsMenu.blueprintButtonMargin, Game1.tileSize * 2), "Info Tool"), new BluePrint("Info Tool"));
			int tallestTab = 0;
			for (int k = 0; k < rowWidthTally.Length; k++)
			{
				if (rowWidthTally[k] > tallestTab)
				{
					tallestTab = rowWidthTally[k];
				}
			}
			this.height = Game1.tileSize * 2 + tallestTab / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + IClickableMenu.borderWidth * 4 + BlueprintsMenu.heightOfDescriptionBox;
			this.buildingPlacementTiles = Game1.content.Load<Texture2D>("LooseSprites\\buildingPlacementTiles");
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Buildings"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize + 4, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Upgrades"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 2, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Decorations"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 3, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Demolish"));
			this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 4, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Animals"));
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00110D38 File Offset: 0x0010EF38
		public int getTabNumberFromName(string name)
		{
			int whichTab = -1;
			if (!(name == "Buildings"))
			{
				if (!(name == "Upgrades"))
				{
					if (!(name == "Decorations"))
					{
						if (!(name == "Demolish"))
						{
							if (name == "Animals")
							{
								whichTab = 4;
							}
						}
						else
						{
							whichTab = 3;
						}
					}
					else
					{
						whichTab = 2;
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

		// Token: 0x06000D82 RID: 3458 RVA: 0x00110DA0 File Offset: 0x0010EFA0
		public void changePosition(int x, int y)
		{
			int translateX = this.xPositionOnScreen - x;
			int translateY = this.yPositionOnScreen - y;
			this.xPositionOnScreen = x;
			this.yPositionOnScreen = y;
			using (List<Dictionary<ClickableComponent, BluePrint>>.Enumerator enumerator = this.blueprintButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (ClickableComponent expr_49 in enumerator.Current.Keys)
					{
						expr_49.bounds.X = expr_49.bounds.X + translateX;
						expr_49.bounds.Y = expr_49.bounds.Y - translateY;
					}
				}
			}
			foreach (ClickableComponent expr_B0 in this.tabs)
			{
				expr_B0.bounds.X = expr_B0.bounds.X + translateX;
				expr_B0.bounds.Y = expr_B0.bounds.Y - translateY;
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00110EC0 File Offset: 0x0010F0C0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.currentAnimal != null)
			{
				this.currentAnimal = null;
				this.placingStructure = true;
				this.queryingAnimals = true;
			}
			if (!this.placingStructure)
			{
				Microsoft.Xna.Framework.Rectangle menuBounds = new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
				foreach (ClickableComponent c in this.blueprintButtons[this.currentTab].Keys)
				{
					if (c.containsPoint(x, y))
					{
						if (c.name.Equals("Info Tool"))
						{
							this.placingStructure = true;
							this.queryingAnimals = true;
							Game1.playSound("smallSelect");
							return;
						}
						if (this.blueprintButtons[this.currentTab][c].doesFarmerHaveEnoughResourcesToBuild())
						{
							this.structureForPlacement = this.blueprintButtons[this.currentTab][c];
							this.placingStructure = true;
							if (this.currentTab == 1)
							{
								this.upgrading = true;
							}
							Game1.playSound("smallSelect");
							return;
						}
						Game1.addHUDMessage(new HUDMessage("Not Enough Resources", Color.Red, 3500f));
						return;
					}
				}
				foreach (ClickableComponent c2 in this.tabs)
				{
					if (c2.containsPoint(x, y))
					{
						this.currentTab = this.getTabNumberFromName(c2.name);
						Game1.playSound("smallSelect");
						if (this.currentTab == 3)
						{
							this.placingStructure = true;
							this.demolishing = true;
						}
						return;
					}
				}
				if (!menuBounds.Contains(x, y))
				{
					Game1.exitActiveMenu();
					return;
				}
			}
			else if (this.demolishing)
			{
				Building destroyed = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (destroyed != null && ((Farm)Game1.getLocationFromName("Farm")).destroyStructure(destroyed))
				{
					int groundYTile = destroyed.tileY + destroyed.tilesHigh;
					for (int i = 0; i < destroyed.texture.Bounds.Height / Game1.tileSize; i++)
					{
						Game1.createRadialDebris(Game1.currentLocation, destroyed.texture, new Microsoft.Xna.Framework.Rectangle(destroyed.texture.Bounds.Center.X, destroyed.texture.Bounds.Center.Y, Game1.tileSize / 16, Game1.tileSize / 16), destroyed.tileX + Game1.random.Next(destroyed.tilesWide), destroyed.tileY + destroyed.tilesHigh - i, Game1.random.Next(20, 45), groundYTile);
					}
					Game1.playSound("explosion");
					Utility.spreadAnimalsAround(destroyed, (Farm)Game1.getLocationFromName("Farm"));
					return;
				}
				Game1.exitActiveMenu();
				return;
			}
			else if (this.upgrading && Game1.currentLocation.GetType() == typeof(Farm))
			{
				Building toUpgrade = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (toUpgrade != null && this.structureForPlacement.name != null && toUpgrade.buildingType.Equals(this.structureForPlacement.nameOfBuildingToUpgrade))
				{
					toUpgrade.indoors.map = Game1.content.Load<Map>("Maps\\" + this.structureForPlacement.mapToWarpTo);
					toUpgrade.indoors.name = this.structureForPlacement.mapToWarpTo;
					toUpgrade.buildingType = this.structureForPlacement.name;
					toUpgrade.texture = this.structureForPlacement.texture;
					if (toUpgrade.indoors.GetType() == typeof(AnimalHouse))
					{
						((AnimalHouse)toUpgrade.indoors).resetPositionsOfAllAnimals();
					}
					Game1.playSound("axe");
					this.structureForPlacement.consumeResources();
					toUpgrade.color = Color.White;
					Game1.exitActiveMenu();
					return;
				}
				if (toUpgrade != null)
				{
					Game1.addHUDMessage(new HUDMessage("Incorrect Building Type", Color.Red, 3500f));
					return;
				}
				Game1.exitActiveMenu();
				return;
			}
			else
			{
				if (this.queryingAnimals)
				{
					if (!(Game1.currentLocation.GetType() == typeof(Farm)) && !(Game1.currentLocation.GetType() == typeof(AnimalHouse)))
					{
						return;
					}
					using (List<FarmAnimal>.Enumerator enumerator3 = ((Game1.currentLocation.GetType() == typeof(Farm)) ? ((Farm)Game1.currentLocation).animals.Values.ToList<FarmAnimal>() : ((AnimalHouse)Game1.currentLocation).animals.Values.ToList<FarmAnimal>()).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							FarmAnimal animal = enumerator3.Current;
							if (new Microsoft.Xna.Framework.Rectangle((int)animal.position.X, (int)animal.position.Y, animal.sprite.SourceRect.Width, animal.sprite.SourceRect.Height).Contains(Game1.viewport.X + Game1.getOldMouseX(), Game1.viewport.Y + Game1.getOldMouseY()))
							{
								this.positionOfAnimalWhenClicked = Game1.GlobalToLocal(Game1.viewport, animal.position);
								this.currentAnimal = animal;
								this.queryingAnimals = false;
								this.placingStructure = false;
								if (animal.sound != null && !animal.sound.Equals(""))
								{
									Game1.playSound(animal.sound);
								}
								break;
							}
						}
						return;
					}
				}
				if (Game1.currentLocation.GetType() != typeof(Farm))
				{
					Game1.addHUDMessage(new HUDMessage("Can Only Place Outside On Farm", Color.Red, 3500f));
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
				else
				{
					Game1.addHUDMessage(new HUDMessage("Can't Build There", Color.Red, 3500f));
				}
			}
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x001115F0 File Offset: 0x0010F7F0
		public bool tryToBuild()
		{
			if (this.structureForPlacement.blueprintType.Equals("Animals"))
			{
				return ((Farm)Game1.getLocationFromName("Farm")).placeAnimal(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player.uniqueMultiplayerID);
			}
			return ((Farm)Game1.getLocationFromName("Farm")).buildStructure(this.structureForPlacement, new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, false);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x001116C4 File Offset: 0x0010F8C4
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.currentAnimal != null)
			{
				this.currentAnimal = null;
				this.queryingAnimals = true;
				this.placingStructure = true;
				return;
			}
			if (this.placingStructure)
			{
				this.placingStructure = false;
				this.queryingAnimals = false;
				this.upgrading = false;
				this.demolishing = false;
				return;
			}
			Game1.exitActiveMenu();
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0011171C File Offset: 0x0010F91C
		public override void performHoverAction(int x, int y)
		{
			if (this.demolishing)
			{
				using (List<Building>.Enumerator enumerator = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.color = Color.White;
					}
				}
				Building b = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (b != null)
				{
					b.color = Color.Red * 0.8f;
					return;
				}
			}
			else if (this.upgrading)
			{
				using (List<Building>.Enumerator enumerator = ((Farm)Game1.getLocationFromName("Farm")).buildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.color = Color.White;
					}
				}
				Building b2 = ((Farm)Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
				if (b2 != null && this.structureForPlacement.nameOfBuildingToUpgrade != null && this.structureForPlacement.nameOfBuildingToUpgrade.Equals(b2.buildingType))
				{
					b2.color = Color.Green * 0.8f;
					return;
				}
				if (b2 != null)
				{
					b2.color = Color.Red * 0.8f;
					return;
				}
			}
			else if (!this.placingStructure)
			{
				foreach (ClickableComponent c in this.tabs)
				{
					if (c.containsPoint(x, y))
					{
						this.hoverText = c.name;
						return;
					}
				}
				this.hoverText = "";
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
				if (!overAnyButton)
				{
					this.hoveredItem = null;
				}
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00111A20 File Offset: 0x0010FC20
		public int getTileSheetIndexForStructurePlacementTile(int x, int y)
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

		// Token: 0x06000D88 RID: 3464 RVA: 0x00111A80 File Offset: 0x0010FC80
		public override void draw(SpriteBatch b)
		{
			if (this.currentAnimal != null)
			{
				int x = (int)Math.Max(0f, Math.Min(this.positionOfAnimalWhenClicked.X - (float)(Game1.tileSize * 4) + (float)(Game1.tileSize / 2), (float)(Game1.viewport.Width - Game1.tileSize * 8)));
				int y = (int)Math.Max(0f, Math.Min((float)(Game1.viewport.Height - Game1.tileSize * 4 - this.currentAnimal.frontBackSourceRect.Height), this.positionOfAnimalWhenClicked.Y - (float)(Game1.tileSize * 4) - (float)this.currentAnimal.frontBackSourceRect.Height));
				Game1.drawDialogueBox(x, y, Game1.tileSize * 8, Game1.tileSize * 5 + Game1.tileSize / 2, false, true, null, false);
				b.Draw(this.currentAnimal.sprite.Texture, new Vector2((float)(x + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 - this.currentAnimal.frontBackSourceRect.Width / 2), (float)(y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, this.currentAnimal.frontBackSourceRect.Width, this.currentAnimal.frontBackSourceRect.Height)), Color.White);
				float fullness = (float)this.currentAnimal.fullness / 255f;
				float happiness = (float)this.currentAnimal.happiness / 255f;
				b.DrawString(Game1.dialogueFont, this.currentAnimal.name, new Vector2((float)(x + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2) - Game1.dialogueFont.MeasureString(this.currentAnimal.name).X / 2f, (float)(y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + this.currentAnimal.frontBackSourceRect.Height + Game1.tileSize / 8)), Game1.textColor);
				b.DrawString(Game1.dialogueFont, "Fullness:", new Vector2((float)(x + IClickableMenu.borderWidth + Game1.tileSize * 3), (float)(y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2)), Game1.textColor);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString("Fullness:").Y + Game1.tileSize / 8, Game1.tileSize * 3, Game1.tileSize / 4), Color.Gray);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString("Fullness:").Y + Game1.tileSize / 8, (int)((float)(Game1.tileSize * 3) * fullness), Game1.tileSize / 4), ((double)fullness > 0.33) ? (((double)fullness > 0.66) ? Color.Green : Color.Goldenrod) : Color.Red);
				b.DrawString(Game1.dialogueFont, "Happy:", new Vector2((float)(x + IClickableMenu.borderWidth + Game1.tileSize * 3), (float)(y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2) + Game1.dialogueFont.MeasureString("Fullness:").Y + (float)(Game1.tileSize / 2)), Game1.textColor);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString("Fullness:").Y + (int)Game1.dialogueFont.MeasureString("Happy:").Y + Game1.tileSize / 2, Game1.tileSize * 3, Game1.tileSize / 4), Color.Gray);
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int)Game1.dialogueFont.MeasureString("Fullness:").Y + (int)Game1.dialogueFont.MeasureString("Happy:").Y + Game1.tileSize / 2, (int)((float)(Game1.tileSize * 3) * happiness), Game1.tileSize / 4), ((double)happiness > 0.33) ? (((double)happiness > 0.66) ? Color.Green : Color.Goldenrod) : Color.Red);
			}
			else if (!this.placingStructure)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - BlueprintsMenu.heightOfDescriptionBox, false, true, null, false);
				foreach (ClickableComponent c in this.tabs)
				{
					int sheetIndex = 0;
					string name = c.name;
					if (!(name == "Buildings"))
					{
						if (!(name == "Upgrades"))
						{
							if (!(name == "Decorations"))
							{
								if (!(name == "Demolish"))
								{
									if (name == "Animals")
									{
										sheetIndex = 8;
									}
								}
								else
								{
									sheetIndex = 6;
								}
							}
							else
							{
								sheetIndex = 7;
							}
						}
						else
						{
							sheetIndex = 5;
						}
					}
					else
					{
						sheetIndex = 4;
					}
					b.Draw(Game1.mouseCursors, new Vector2((float)c.bounds.X, (float)(c.bounds.Y + ((this.currentTab == this.getTabNumberFromName(c.name)) ? 8 : 0))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, sheetIndex, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);
				}
				foreach (ClickableComponent c2 in this.blueprintButtons[this.currentTab].Keys)
				{
					Texture2D structureTexture = this.blueprintButtons[this.currentTab][c2].texture;
					Vector2 origin = c2.name.Equals("Info Tool") ? new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) : new Vector2((float)this.blueprintButtons[this.currentTab][c2].sourceRectForMenuView.Center.X, (float)this.blueprintButtons[this.currentTab][c2].sourceRectForMenuView.Center.Y);
					b.Draw(structureTexture, new Vector2((float)c2.bounds.Center.X, (float)c2.bounds.Center.Y), new Microsoft.Xna.Framework.Rectangle?(this.blueprintButtons[this.currentTab][c2].sourceRectForMenuView), Color.White, 0f, origin, 0.25f * c2.scale + ((this.currentTab == 4) ? 0.75f : 0f), SpriteEffects.None, 0.9f);
				}
				Game1.drawWithBorder(this.hoverText, Color.Black, Color.White, new Vector2((float)(Game1.getOldMouseX() + Game1.tileSize), (float)(Game1.getOldMouseY() + Game1.tileSize)), 0f, 1f, 1f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen + (this.height - BlueprintsMenu.heightOfDescriptionBox) - IClickableMenu.borderWidth * 2, this.width, BlueprintsMenu.heightOfDescriptionBox, false, true, null, false);
				if (this.hoveredItem != null)
				{
				}
			}
			else if (!this.demolishing && !this.upgrading && !this.queryingAnimals)
			{
				Vector2 mousePositionTile = new Vector2((float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
				for (int y2 = 0; y2 < this.structureForPlacement.tilesHeight; y2++)
				{
					for (int x2 = 0; x2 < this.structureForPlacement.tilesWidth; x2++)
					{
						int sheetIndex2 = this.getTileSheetIndexForStructurePlacementTile(x2, y2);
						Vector2 currentGlobalTilePosition = new Vector2(mousePositionTile.X + (float)x2, mousePositionTile.Y + (float)y2);
						if (Game1.player.getTileLocation().Equals(currentGlobalTilePosition) || Game1.currentLocation.isTileOccupied(currentGlobalTilePosition, "") || !Game1.currentLocation.isTilePassable(new Location((int)currentGlobalTilePosition.X, (int)currentGlobalTilePosition.Y), Game1.viewport))
						{
							sheetIndex2++;
						}
						b.Draw(this.buildingPlacementTiles, Game1.GlobalToLocal(Game1.viewport, currentGlobalTilePosition * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(this.buildingPlacementTiles, sheetIndex2, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
					}
				}
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, (this.queryingAnimals || this.currentAnimal != null) ? 9 : 0, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
		}

		// Token: 0x04000DFA RID: 3578
		public static int heightOfDescriptionBox = Game1.tileSize * 6;

		// Token: 0x04000DFB RID: 3579
		public static int blueprintButtonMargin = Game1.tileSize / 2;

		// Token: 0x04000DFC RID: 3580
		public new static int tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;

		// Token: 0x04000DFD RID: 3581
		public const int buildingsTab = 0;

		// Token: 0x04000DFE RID: 3582
		public const int upgradesTab = 1;

		// Token: 0x04000DFF RID: 3583
		public const int decorationsTab = 2;

		// Token: 0x04000E00 RID: 3584
		public const int demolishTab = 3;

		// Token: 0x04000E01 RID: 3585
		public const int animalsTab = 4;

		// Token: 0x04000E02 RID: 3586
		public const int numberOfTabs = 5;

		// Token: 0x04000E03 RID: 3587
		private bool placingStructure;

		// Token: 0x04000E04 RID: 3588
		private bool demolishing;

		// Token: 0x04000E05 RID: 3589
		private bool upgrading;

		// Token: 0x04000E06 RID: 3590
		private bool queryingAnimals;

		// Token: 0x04000E07 RID: 3591
		private int currentTab;

		// Token: 0x04000E08 RID: 3592
		private Vector2 positionOfAnimalWhenClicked;

		// Token: 0x04000E09 RID: 3593
		private string hoverText = "";

		// Token: 0x04000E0A RID: 3594
		private List<Dictionary<ClickableComponent, BluePrint>> blueprintButtons = new List<Dictionary<ClickableComponent, BluePrint>>();

		// Token: 0x04000E0B RID: 3595
		private List<ClickableComponent> tabs = new List<ClickableComponent>();

		// Token: 0x04000E0C RID: 3596
		private BluePrint hoveredItem;

		// Token: 0x04000E0D RID: 3597
		private BluePrint structureForPlacement;

		// Token: 0x04000E0E RID: 3598
		private FarmAnimal currentAnimal;

		// Token: 0x04000E0F RID: 3599
		private Texture2D buildingPlacementTiles;
	}
}
