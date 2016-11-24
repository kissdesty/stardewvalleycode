using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;

namespace StardewValley.Menus
{
	// Token: 0x020000DD RID: 221
	public class CharacterCustomization : IClickableMenu
	{
		// Token: 0x06000DC3 RID: 3523 RVA: 0x00117D84 File Offset: 0x00115F84
		public CharacterCustomization(List<int> shirtOptions, List<int> hairStyleOptions, List<int> accessoryOptions, bool wizardSource = false) : base(Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize, 632 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize, false)
		{
			this.shirtOptions = shirtOptions;
			this.hairStyleOptions = hairStyleOptions;
			this.accessoryOptions = accessoryOptions;
			this.wizardSource = wizardSource;
			this.setUpPositions();
			Game1.player.faceDirection(2);
			Game1.player.FarmerSprite.StopAnimation();
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00117E78 File Offset: 0x00116078
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
			this.setUpPositions();
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00117EE0 File Offset: 0x001160E0
		private void setUpPositions()
		{
			this.labels.Clear();
			this.petButtons.Clear();
			this.genderButtons.Clear();
			this.leftSelectionButtons.Clear();
			this.rightSelectionButtons.Clear();
			this.farmTypeButtons.Clear();
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.nameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4,
				Text = Game1.player.name
			};
			this.labels.Add(this.nameLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Name", new object[0])));
			this.farmnameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize,
				Text = Game1.player.farmName
			};
			this.labels.Add(this.farmLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Farm", new object[0])));
			this.favThingBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize * 2,
				Text = Game1.player.favoriteThing
			};
			this.labels.Add(this.favoriteLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize * 2, 1, 1), Game1.content.LoadString("Strings\\UI:Character_FavoriteThing", new object[0])));
			this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 12, this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 14, Game1.pixelZoom * 10, Game1.pixelZoom * 10), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
			int yOffset = Game1.tileSize * 2;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Direction", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Direction", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false));
			if (!this.wizardSource)
			{
				this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 3, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Animal", new object[0])));
			}
			this.petButtons.Add(new ClickableTextureComponent("Cat", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 6 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, "Cat", Game1.mouseCursors, new Rectangle(160, 192, 16, 16), (float)Game1.pixelZoom, false));
			this.petButtons.Add(new ClickableTextureComponent("Dog", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 7 - Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, "Dog", Game1.mouseCursors, new Rectangle(176, 192, 16, 16), (float)Game1.pixelZoom, false));
			this.genderButtons.Add(new ClickableTextureComponent("Male", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 2 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), null, "Male", Game1.mouseCursors, new Rectangle(128, 192, 16, 16), (float)Game1.pixelZoom, false));
			this.genderButtons.Add(new ClickableTextureComponent("Female", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 2 + Game1.tileSize + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), null, "Female", Game1.mouseCursors, new Rectangle(144, 192, 16, 16), (float)Game1.pixelZoom, false));
			yOffset = Game1.tileSize * 4 + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Skin", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false));
			this.labels.Add(this.skinLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 4 + Game1.tileSize + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Skin", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Skin", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false));
			if (!this.wizardSource)
			{
				Point baseFarmButton = new Point(this.xPositionOnScreen + this.width + Game1.pixelZoom + Game1.tileSize / 8, this.yPositionOnScreen + IClickableMenu.borderWidth * 2);
				this.farmTypeButtons.Add(new ClickableTextureComponent("Standard", new Rectangle(baseFarmButton.X, baseFarmButton.Y + 22 * Game1.pixelZoom, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmStandard", new object[0]), Game1.mouseCursors, new Rectangle(0, 324, 22, 20), (float)Game1.pixelZoom, false));
				this.farmTypeButtons.Add(new ClickableTextureComponent("Riverland", new Rectangle(baseFarmButton.X, baseFarmButton.Y + 22 * Game1.pixelZoom * 2, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmFishing", new object[0]), Game1.mouseCursors, new Rectangle(22, 324, 22, 20), (float)Game1.pixelZoom, false));
				this.farmTypeButtons.Add(new ClickableTextureComponent("Forest", new Rectangle(baseFarmButton.X, baseFarmButton.Y + 22 * Game1.pixelZoom * 3, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmForaging", new object[0]), Game1.mouseCursors, new Rectangle(44, 324, 22, 20), (float)Game1.pixelZoom, false));
				this.farmTypeButtons.Add(new ClickableTextureComponent("Hills", new Rectangle(baseFarmButton.X, baseFarmButton.Y + 22 * Game1.pixelZoom * 4, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmMining", new object[0]), Game1.mouseCursors, new Rectangle(66, 324, 22, 20), (float)Game1.pixelZoom, false));
				this.farmTypeButtons.Add(new ClickableTextureComponent("Wilderness", new Rectangle(baseFarmButton.X, baseFarmButton.Y + 22 * Game1.pixelZoom * 5, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmCombat", new object[0]), Game1.mouseCursors, new Rectangle(88, 324, 22, 20), (float)Game1.pixelZoom, false));
			}
			this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_EyeColor", new object[0])));
			this.eyeColorPicker = new ColorPicker(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 + Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset);
			this.eyeColorPicker.setColor(Game1.player.newEyeColor);
			yOffset += Game1.tileSize + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Hair", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false));
			this.labels.Add(this.hairLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Hair", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Hair", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false));
			this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_HairColor", new object[0])));
			this.hairColorPicker = new ColorPicker(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 + Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset);
			this.hairColorPicker.setColor(Game1.player.hairstyleColor);
			yOffset += Game1.tileSize + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Shirt", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false));
			this.labels.Add(this.shirtLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Shirt", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Shirt", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false));
			this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_PantsColor", new object[0])));
			this.pantsColorPicker = new ColorPicker(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 + Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset);
			this.pantsColorPicker.setColor(Game1.player.pantsColor);
			this.skipIntroButton = new ClickableTextureComponent("Skip Intro", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 - Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + Game1.tileSize * 5 / 4, Game1.pixelZoom * 9, Game1.pixelZoom * 9), null, Game1.content.LoadString("Strings\\UI:Character_SkipIntro", new object[0]), Game1.mouseCursors, new Rectangle(227, 425, 9, 9), (float)Game1.pixelZoom, false);
			yOffset += Game1.tileSize + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Acc", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false));
			this.labels.Add(this.accLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Accessory", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Acc", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + yOffset, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false));
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00119154 File Offset: 0x00117354
		private void optionButtonClick(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1367651536u)
			{
				if (num <= 989237149u)
				{
					if (num != 1485672u)
					{
						if (num == 989237149u)
						{
							if (name == "Wilderness")
							{
								if (!this.wizardSource)
								{
									Game1.whichFarm = 4;
									Game1.spawnMonstersAtNight = true;
								}
							}
						}
					}
					else if (name == "Standard")
					{
						if (!this.wizardSource)
						{
							Game1.whichFarm = 0;
							Game1.spawnMonstersAtNight = false;
						}
					}
				}
				else if (num != 1216165616u)
				{
					if (num != 1265483177u)
					{
						if (num == 1367651536u)
						{
							if (name == "Riverland")
							{
								if (!this.wizardSource)
								{
									Game1.whichFarm = 1;
									Game1.spawnMonstersAtNight = false;
								}
							}
						}
					}
					else if (name == "Dog")
					{
						if (!this.wizardSource)
						{
							Game1.player.catPerson = false;
						}
					}
				}
				else if (name == "Male")
				{
					if (!this.wizardSource)
					{
						Game1.player.changeGender(true);
						Game1.player.changeHairStyle(0);
					}
				}
			}
			else if (num <= 2246359087u)
			{
				if (num != 1761538983u)
				{
					if (num == 2246359087u)
					{
						if (name == "OK")
						{
							if (!this.canLeaveMenu())
							{
								return;
							}
							Game1.player.Name = this.nameBox.Text.Trim();
							Game1.player.favoriteThing = this.favThingBox.Text.Trim();
							if (Game1.activeClickableMenu is TitleMenu)
							{
								(Game1.activeClickableMenu as TitleMenu).createdNewCharacter(this.skipIntro);
							}
							else
							{
								Game1.exitActiveMenu();
								if (Game1.currentMinigame != null && Game1.currentMinigame is Intro)
								{
									(Game1.currentMinigame as Intro).doneCreatingCharacter();
								}
								else if (this.wizardSource)
								{
									Game1.flashAlpha = 1f;
									Game1.playSound("yoba");
								}
							}
						}
					}
				}
				else if (name == "Cat")
				{
					if (!this.wizardSource)
					{
						Game1.player.catPerson = true;
					}
				}
			}
			else if (num != 2503779456u)
			{
				if (num != 2508411131u)
				{
					if (num == 3634523321u)
					{
						if (name == "Female")
						{
							if (!this.wizardSource)
							{
								Game1.player.changeGender(false);
								Game1.player.changeHairStyle(16);
							}
						}
					}
				}
				else if (name == "Hills")
				{
					if (!this.wizardSource)
					{
						Game1.whichFarm = 3;
						Game1.spawnMonstersAtNight = false;
					}
				}
			}
			else if (name == "Forest")
			{
				if (!this.wizardSource)
				{
					Game1.whichFarm = 2;
					Game1.spawnMonstersAtNight = false;
				}
			}
			Game1.playSound("coin");
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0011948C File Offset: 0x0011768C
		private void selectionClick(string name, int change)
		{
			if (name == "Skin")
			{
				Game1.player.changeSkinColor(Game1.player.skin + change);
				Game1.playSound("skeletonStep");
				return;
			}
			if (name == "Hair")
			{
				Game1.player.changeHairStyle(Game1.player.hair + change);
				Game1.playSound("grassyStep");
				return;
			}
			if (name == "Shirt")
			{
				Game1.player.changeShirt(Game1.player.shirt + change);
				Game1.playSound("coin");
				return;
			}
			if (name == "Acc")
			{
				Game1.player.changeAccessory(Game1.player.accessory + change);
				Game1.playSound("purchase");
				return;
			}
			if (!(name == "Direction"))
			{
				return;
			}
			Game1.player.faceDirection((Game1.player.facingDirection - change + 4) % 4);
			Game1.player.FarmerSprite.StopAnimation();
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.playSound("pickUpItem");
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x001195A0 File Offset: 0x001177A0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			foreach (ClickableComponent c in this.genderButtons)
			{
				if (c.containsPoint(x, y))
				{
					this.optionButtonClick(c.name);
					c.scale -= 0.5f;
					c.scale = Math.Max(3.5f, c.scale);
				}
			}
			foreach (ClickableComponent c2 in this.farmTypeButtons)
			{
				if (c2.containsPoint(x, y) && !c2.name.Contains("Gray"))
				{
					this.optionButtonClick(c2.name);
					c2.scale -= 0.5f;
					c2.scale = Math.Max(3.5f, c2.scale);
				}
			}
			foreach (ClickableComponent c3 in this.petButtons)
			{
				if (c3.containsPoint(x, y))
				{
					this.optionButtonClick(c3.name);
					c3.scale -= 0.5f;
					c3.scale = Math.Max(3.5f, c3.scale);
				}
			}
			foreach (ClickableComponent c4 in this.leftSelectionButtons)
			{
				if (c4.containsPoint(x, y))
				{
					this.selectionClick(c4.name, -1);
					c4.scale -= 0.25f;
					c4.scale = Math.Max(0.75f, c4.scale);
				}
			}
			foreach (ClickableComponent c5 in this.rightSelectionButtons)
			{
				if (c5.containsPoint(x, y))
				{
					this.selectionClick(c5.name, 1);
					c5.scale -= 0.25f;
					c5.scale = Math.Max(0.75f, c5.scale);
				}
			}
			if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
			{
				this.optionButtonClick(this.okButton.name);
				this.okButton.scale -= 0.25f;
				this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
			}
			if (this.hairColorPicker.containsPoint(x, y))
			{
				Game1.player.changeHairColor(this.hairColorPicker.click(x, y));
				this.lastHeldColorPicker = this.hairColorPicker;
			}
			else if (this.pantsColorPicker.containsPoint(x, y))
			{
				Game1.player.changePants(this.pantsColorPicker.click(x, y));
				this.lastHeldColorPicker = this.pantsColorPicker;
			}
			else if (this.eyeColorPicker.containsPoint(x, y))
			{
				Game1.player.changeEyeColor(this.eyeColorPicker.click(x, y));
				this.lastHeldColorPicker = this.eyeColorPicker;
			}
			if (!this.wizardSource)
			{
				this.nameBox.Update();
				this.farmnameBox.Update();
				this.favThingBox.Update();
				if (this.skipIntroButton.containsPoint(x, y))
				{
					Game1.playSound("drumkit6");
					this.skipIntroButton.sourceRect.X = ((this.skipIntroButton.sourceRect.X == 227) ? 236 : 227);
					this.skipIntro = !this.skipIntro;
				}
			}
			if (this.randomButton.containsPoint(x, y))
			{
				string sound = "drumkit6";
				if (this.timesRandom > 0)
				{
					switch (Game1.random.Next(15))
					{
					case 0:
						sound = "drumkit1";
						break;
					case 1:
						sound = "dirtyHit";
						break;
					case 2:
						sound = "axchop";
						break;
					case 3:
						sound = "hoeHit";
						break;
					case 4:
						sound = "fishSlap";
						break;
					case 5:
						sound = "drumkit6";
						break;
					case 6:
						sound = "drumkit5";
						break;
					case 7:
						sound = "drumkit6";
						break;
					case 8:
						sound = "junimoMeep1";
						break;
					case 9:
						sound = "coin";
						break;
					case 10:
						sound = "axe";
						break;
					case 11:
						sound = "hammer";
						break;
					case 12:
						sound = "drumkit2";
						break;
					case 13:
						sound = "drumkit4";
						break;
					case 14:
						sound = "drumkit3";
						break;
					}
				}
				Game1.playSound(sound);
				this.timesRandom++;
				if (Game1.random.NextDouble() < 0.33)
				{
					if (Game1.player.isMale)
					{
						Game1.player.changeAccessory(Game1.random.Next(19));
					}
					else
					{
						Game1.player.changeAccessory(Game1.random.Next(6, 19));
					}
				}
				else
				{
					Game1.player.changeAccessory(-1);
				}
				if (Game1.player.isMale)
				{
					Game1.player.changeHairStyle(Game1.random.Next(16));
				}
				else
				{
					Game1.player.changeHairStyle(Game1.random.Next(16, 32));
				}
				Color hairColor = new Color(Game1.random.Next(25, 254), Game1.random.Next(25, 254), Game1.random.Next(25, 254));
				if (Game1.random.NextDouble() < 0.5)
				{
					hairColor.R /= 2;
					hairColor.G /= 2;
					hairColor.B /= 2;
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					hairColor.R = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					hairColor.G = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					hairColor.B = (byte)Game1.random.Next(15, 50);
				}
				Game1.player.changeHairColor(hairColor);
				Game1.player.changeShirt(Game1.random.Next(112));
				Game1.player.changeSkinColor(Game1.random.Next(6));
				if (Game1.random.NextDouble() < 0.25)
				{
					Game1.player.changeSkinColor(Game1.random.Next(24));
				}
				Color pantsColor = new Color(Game1.random.Next(25, 254), Game1.random.Next(25, 254), Game1.random.Next(25, 254));
				if (Game1.random.NextDouble() < 0.5)
				{
					pantsColor.R /= 2;
					pantsColor.G /= 2;
					pantsColor.B /= 2;
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					pantsColor.R = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					pantsColor.G = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					pantsColor.B = (byte)Game1.random.Next(15, 50);
				}
				Game1.player.changePants(pantsColor);
				Color eyeColor = new Color(Game1.random.Next(25, 254), Game1.random.Next(25, 254), Game1.random.Next(25, 254));
				eyeColor.R /= 2;
				eyeColor.G /= 2;
				eyeColor.B /= 2;
				if (Game1.random.NextDouble() < 0.5)
				{
					eyeColor.R = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					eyeColor.G = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					eyeColor.B = (byte)Game1.random.Next(15, 50);
				}
				Game1.player.changeEyeColor(eyeColor);
				this.randomButton.scale = (float)Game1.pixelZoom - 0.5f;
				this.pantsColorPicker.setColor(Game1.player.pantsColor);
				this.eyeColorPicker.setColor(Game1.player.newEyeColor);
				this.hairColorPicker.setColor(Game1.player.hairstyleColor);
			}
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00119F24 File Offset: 0x00118124
		public override void leftClickHeld(int x, int y)
		{
			this.colorPickerTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			if (this.colorPickerTimer <= 0)
			{
				if (this.lastHeldColorPicker != null)
				{
					if (this.lastHeldColorPicker.Equals(this.hairColorPicker))
					{
						Game1.player.changeHairColor(this.hairColorPicker.clickHeld(x, y));
					}
					if (this.lastHeldColorPicker.Equals(this.pantsColorPicker))
					{
						Game1.player.changePants(this.pantsColorPicker.clickHeld(x, y));
					}
					if (this.lastHeldColorPicker.Equals(this.eyeColorPicker))
					{
						Game1.player.changeEyeColor(this.eyeColorPicker.clickHeld(x, y));
					}
				}
				this.colorPickerTimer = 100;
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00119FEA File Offset: 0x001181EA
		public override void releaseLeftClick(int x, int y)
		{
			this.hairColorPicker.releaseClick();
			this.pantsColorPicker.releaseClick();
			this.eyeColorPicker.releaseClick();
			this.lastHeldColorPicker = null;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0011A014 File Offset: 0x00118214
		public override void receiveKeyPress(Keys key)
		{
			if (!this.wizardSource && key == Keys.Tab)
			{
				if (this.nameBox.Selected)
				{
					this.farmnameBox.SelectMe();
					this.nameBox.Selected = false;
					return;
				}
				if (this.farmnameBox.Selected)
				{
					this.farmnameBox.Selected = false;
					this.favThingBox.SelectMe();
					return;
				}
				this.favThingBox.Selected = false;
				this.nameBox.SelectMe();
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0011A090 File Offset: 0x00118290
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			this.hoverTitle = "";
			using (List<ClickableComponent>.Enumerator enumerator = this.leftSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClickableTextureComponent c = (ClickableTextureComponent)enumerator.Current;
					if (c.containsPoint(x, y))
					{
						c.scale = Math.Min(c.scale + 0.02f, c.baseScale + 0.1f);
					}
					else
					{
						c.scale = Math.Max(c.scale - 0.02f, c.baseScale);
					}
				}
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.rightSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClickableTextureComponent c2 = (ClickableTextureComponent)enumerator.Current;
					if (c2.containsPoint(x, y))
					{
						c2.scale = Math.Min(c2.scale + 0.02f, c2.baseScale + 0.1f);
					}
					else
					{
						c2.scale = Math.Max(c2.scale - 0.02f, c2.baseScale);
					}
				}
			}
			if (!this.wizardSource)
			{
				foreach (ClickableTextureComponent c3 in this.farmTypeButtons)
				{
					if (c3.containsPoint(x, y) && !c3.name.Contains("Gray"))
					{
						c3.scale = Math.Min(c3.scale + 0.02f, c3.baseScale + 0.1f);
						this.hoverTitle = c3.hoverText.Split(new char[]
						{
							'_'
						})[0];
						this.hoverText = c3.hoverText.Split(new char[]
						{
							'_'
						})[1];
					}
					else
					{
						c3.scale = Math.Max(c3.scale - 0.02f, c3.baseScale);
						if (c3.name.Contains("Gray") && c3.containsPoint(x, y))
						{
							this.hoverText = "Reach level 10 " + Game1.content.LoadString("Strings\\UI:Character_" + c3.name.Split(new char[]
							{
								'_'
							})[1], new object[0]) + " to unlock.";
						}
					}
				}
			}
			if (!this.wizardSource)
			{
				using (List<ClickableComponent>.Enumerator enumerator = this.genderButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent c4 = (ClickableTextureComponent)enumerator.Current;
						if (c4.containsPoint(x, y))
						{
							c4.scale = Math.Min(c4.scale + 0.02f, c4.baseScale + 0.1f);
						}
						else
						{
							c4.scale = Math.Max(c4.scale - 0.02f, c4.baseScale);
						}
					}
				}
				using (List<ClickableComponent>.Enumerator enumerator = this.petButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent c5 = (ClickableTextureComponent)enumerator.Current;
						if (c5.containsPoint(x, y))
						{
							c5.scale = Math.Min(c5.scale + 0.02f, c5.baseScale + 0.1f);
						}
						else
						{
							c5.scale = Math.Max(c5.scale - 0.02f, c5.baseScale);
						}
					}
				}
			}
			if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
			}
			else
			{
				this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
			}
			this.randomButton.tryHover(x, y, 0.25f);
			this.randomButton.tryHover(x, y, 0.25f);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0011A534 File Offset: 0x00118734
		public bool canLeaveMenu()
		{
			return this.wizardSource || (Game1.player.name.Length > 0 && Game1.player.farmName.Length > 0 && Game1.player.favoriteThing.Length > 0);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0011A584 File Offset: 0x00118784
		public override void draw(SpriteBatch b)
		{
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			b.Draw(Game1.daybg, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
			Game1.player.FarmerRenderer.draw(b, Game1.player.FarmerSprite.CurrentAnimationFrame, Game1.player.FarmerSprite.CurrentFrame, Game1.player.FarmerSprite.SourceRect, new Vector2((float)(this.xPositionOnScreen - 2 + Game1.tileSize * 2 / 3 + Game1.tileSize * 2 - Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth - Game1.tileSize / 4 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2)), Vector2.Zero, 0.8f, Color.White, 0f, 1f, Game1.player);
			if (!this.wizardSource)
			{
				using (List<ClickableComponent>.Enumerator enumerator = this.genderButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent c = (ClickableTextureComponent)enumerator.Current;
						c.draw(b);
						if ((c.name.Equals("Male") && Game1.player.isMale) || (c.name.Equals("Female") && !Game1.player.isMale))
						{
							b.Draw(Game1.mouseCursors, c.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
						}
					}
				}
				using (List<ClickableComponent>.Enumerator enumerator = this.petButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent c2 = (ClickableTextureComponent)enumerator.Current;
						c2.draw(b);
						if ((c2.name.Equals("Cat") && Game1.player.catPerson) || (c2.name.Equals("Dog") && !Game1.player.catPerson))
						{
							b.Draw(Game1.mouseCursors, c2.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
						}
					}
				}
				Game1.player.name = this.nameBox.Text;
				Game1.player.favoriteThing = this.favThingBox.Text;
				Game1.player.farmName = this.farmnameBox.Text;
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.leftSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					((ClickableTextureComponent)enumerator.Current).draw(b);
				}
			}
			foreach (ClickableComponent c3 in this.labels)
			{
				string sub = "";
				Color color = Game1.textColor;
				if (c3 == this.nameLabel)
				{
					color = ((Game1.player.name.Length < 1) ? Color.Red : Game1.textColor);
					if (this.wizardSource)
					{
						continue;
					}
				}
				else if (c3 == this.farmLabel)
				{
					color = ((Game1.player.farmName.Length < 1) ? Color.Red : Game1.textColor);
					if (this.wizardSource)
					{
						continue;
					}
				}
				else if (c3 == this.favoriteLabel)
				{
					color = ((Game1.player.favoriteThing.Length < 1) ? Color.Red : Game1.textColor);
					if (this.wizardSource)
					{
						continue;
					}
				}
				else if (c3 == this.shirtLabel)
				{
					sub = string.Concat(Game1.player.shirt + 1);
				}
				else if (c3 == this.skinLabel)
				{
					sub = string.Concat(Game1.player.skin + 1);
				}
				else if (c3 == this.hairLabel)
				{
					if (!c3.name.Contains("Color"))
					{
						sub = string.Concat(Game1.player.hair + 1);
					}
				}
				else if (c3 == this.accLabel)
				{
					sub = string.Concat(Game1.player.accessory + 2);
				}
				else
				{
					color = Game1.textColor;
				}
				Utility.drawTextWithShadow(b, c3.name, Game1.smallFont, new Vector2((float)c3.bounds.X, (float)c3.bounds.Y), color, 1f, -1f, -1, -1, 1f, 3);
				if (sub.Length > 0)
				{
					Utility.drawTextWithShadow(b, sub, Game1.smallFont, new Vector2((float)(c3.bounds.X + Game1.tileSize / 3) - Game1.smallFont.MeasureString(sub).X / 2f, (float)(c3.bounds.Y + Game1.tileSize / 2)), color, 1f, -1f, -1, -1, 1f, 3);
				}
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.rightSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					((ClickableTextureComponent)enumerator.Current).draw(b);
				}
			}
			if (!this.wizardSource)
			{
				IClickableMenu.drawTextureBox(b, this.farmTypeButtons[0].bounds.X - Game1.pixelZoom * 4, this.farmTypeButtons[0].bounds.Y - Game1.pixelZoom * 5, 30 * Game1.pixelZoom, 110 * Game1.pixelZoom + Game1.pixelZoom * 9, Color.White);
				for (int i = 0; i < this.farmTypeButtons.Count; i++)
				{
					this.farmTypeButtons[i].draw(b, this.farmTypeButtons[i].name.Contains("Gray") ? (Color.Black * 0.5f) : Color.White, 0.88f);
					if (this.farmTypeButtons[i].name.Contains("Gray"))
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.farmTypeButtons[i].bounds.Center.X - Game1.pixelZoom * 3), (float)(this.farmTypeButtons[i].bounds.Center.Y - Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(107, 442, 7, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
					}
					if (i == Game1.whichFarm)
					{
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), this.farmTypeButtons[i].bounds.X, this.farmTypeButtons[i].bounds.Y - Game1.pixelZoom, this.farmTypeButtons[i].bounds.Width, this.farmTypeButtons[i].bounds.Height + Game1.pixelZoom * 2, Color.White, (float)Game1.pixelZoom, false);
					}
				}
			}
			if (this.canLeaveMenu())
			{
				this.okButton.draw(b, Color.White, 0.75f);
			}
			else
			{
				this.okButton.draw(b, Color.White, 0.75f);
				this.okButton.draw(b, Color.Black * 0.5f, 0.751f);
			}
			this.hairColorPicker.draw(b);
			this.pantsColorPicker.draw(b);
			this.eyeColorPicker.draw(b);
			if (!this.wizardSource)
			{
				this.nameBox.Draw(b);
				this.farmnameBox.Draw(b);
				if (this.skipIntroButton != null)
				{
					this.skipIntroButton.draw(b);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:Character_SkipIntro", new object[0]), Game1.smallFont, new Vector2((float)(this.skipIntroButton.bounds.X + this.skipIntroButton.bounds.Width + Game1.pixelZoom * 2), (float)(this.skipIntroButton.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
				Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:Character_FarmNameSuffix", new object[0]), Game1.smallFont, new Vector2((float)(this.farmnameBox.X + this.farmnameBox.Width + Game1.pixelZoom * 2), (float)(this.farmnameBox.Y + Game1.pixelZoom * 3)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				this.favThingBox.Draw(b);
			}
			if (this.hoverText != null && this.hoverTitle != null && this.hoverText.Count<char>() > 0)
			{
				IClickableMenu.drawHoverText(b, Game1.parseText(this.hoverText, Game1.smallFont, Game1.tileSize * 4), Game1.smallFont, 0, 0, -1, this.hoverTitle, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			this.randomButton.draw(b);
			base.drawMouse(b);
		}

		// Token: 0x04000E87 RID: 3719
		public const int colorPickerTimerDelay = 100;

		// Token: 0x04000E88 RID: 3720
		private List<int> shirtOptions;

		// Token: 0x04000E89 RID: 3721
		private List<int> hairStyleOptions;

		// Token: 0x04000E8A RID: 3722
		private List<int> accessoryOptions;

		// Token: 0x04000E8B RID: 3723
		private int currentShirt;

		// Token: 0x04000E8C RID: 3724
		private int currentHair;

		// Token: 0x04000E8D RID: 3725
		private int currentAccessory;

		// Token: 0x04000E8E RID: 3726
		private int colorPickerTimer;

		// Token: 0x04000E8F RID: 3727
		private ColorPicker pantsColorPicker;

		// Token: 0x04000E90 RID: 3728
		private ColorPicker hairColorPicker;

		// Token: 0x04000E91 RID: 3729
		private ColorPicker eyeColorPicker;

		// Token: 0x04000E92 RID: 3730
		private List<ClickableComponent> labels = new List<ClickableComponent>();

		// Token: 0x04000E93 RID: 3731
		private List<ClickableComponent> leftSelectionButtons = new List<ClickableComponent>();

		// Token: 0x04000E94 RID: 3732
		private List<ClickableComponent> rightSelectionButtons = new List<ClickableComponent>();

		// Token: 0x04000E95 RID: 3733
		private List<ClickableComponent> genderButtons = new List<ClickableComponent>();

		// Token: 0x04000E96 RID: 3734
		private List<ClickableComponent> petButtons = new List<ClickableComponent>();

		// Token: 0x04000E97 RID: 3735
		private List<ClickableTextureComponent> farmTypeButtons = new List<ClickableTextureComponent>();

		// Token: 0x04000E98 RID: 3736
		private ClickableTextureComponent okButton;

		// Token: 0x04000E99 RID: 3737
		private ClickableTextureComponent skipIntroButton;

		// Token: 0x04000E9A RID: 3738
		private ClickableTextureComponent randomButton;

		// Token: 0x04000E9B RID: 3739
		private TextBox nameBox;

		// Token: 0x04000E9C RID: 3740
		private TextBox farmnameBox;

		// Token: 0x04000E9D RID: 3741
		private TextBox favThingBox;

		// Token: 0x04000E9E RID: 3742
		private bool skipIntro;

		// Token: 0x04000E9F RID: 3743
		private bool wizardSource;

		// Token: 0x04000EA0 RID: 3744
		private string hoverText;

		// Token: 0x04000EA1 RID: 3745
		private string hoverTitle;

		// Token: 0x04000EA2 RID: 3746
		private ClickableComponent nameLabel;

		// Token: 0x04000EA3 RID: 3747
		private ClickableComponent farmLabel;

		// Token: 0x04000EA4 RID: 3748
		private ClickableComponent favoriteLabel;

		// Token: 0x04000EA5 RID: 3749
		private ClickableComponent shirtLabel;

		// Token: 0x04000EA6 RID: 3750
		private ClickableComponent skinLabel;

		// Token: 0x04000EA7 RID: 3751
		private ClickableComponent hairLabel;

		// Token: 0x04000EA8 RID: 3752
		private ClickableComponent accLabel;

		// Token: 0x04000EA9 RID: 3753
		private ColorPicker lastHeldColorPicker;

		// Token: 0x04000EAA RID: 3754
		private int timesRandom;
	}
}
