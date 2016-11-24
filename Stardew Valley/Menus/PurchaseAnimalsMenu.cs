using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	// Token: 0x0200010C RID: 268
	public class PurchaseAnimalsMenu : IClickableMenu
	{
		// Token: 0x06000F90 RID: 3984 RVA: 0x0013EDF4 File Offset: 0x0013CFF4
		public PurchaseAnimalsMenu(List<Object> stock) : base(Game1.viewport.Width / 2 - PurchaseAnimalsMenu.menuWidth / 2 - IClickableMenu.borderWidth * 2, Game1.viewport.Height / 2 - PurchaseAnimalsMenu.menuHeight - IClickableMenu.borderWidth * 2, PurchaseAnimalsMenu.menuWidth + IClickableMenu.borderWidth * 2, PurchaseAnimalsMenu.menuHeight + IClickableMenu.borderWidth, false)
		{
			this.height += Game1.tileSize;
			for (int i = 0; i < stock.Count; i++)
			{
				this.animalsToPurchase.Add(new ClickableTextureComponent(string.Concat(stock[i].salePrice()), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + i % 3 * Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + i / 3 * (Game1.tileSize + Game1.tileSize / 3), Game1.tileSize * 2, Game1.tileSize), null, stock[i].Name, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(i % 3 * 16 * 2, 448 + i / 3 * 16, 32, 16), 4f, stock[i].type == null)
				{
					item = stock[i]
				});
			}
			this.okButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
			this.randomButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
			PurchaseAnimalsMenu.menuHeight = Game1.tileSize * 5;
			PurchaseAnimalsMenu.menuWidth = Game1.tileSize * 7;
			this.textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
			this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
			this.textBox.Y = Game1.viewport.Height / 2;
			this.textBox.Width = Game1.tileSize * 4;
			this.textBox.Height = Game1.tileSize * 3;
			this.e = new TextBoxEvent(this.textBoxEnter);
			this.randomButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
			this.doneNamingButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0013F190 File Offset: 0x0013D390
		public void textBoxEnter(TextBox sender)
		{
			if (!this.namingAnimal)
			{
				return;
			}
			if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is PurchaseAnimalsMenu))
			{
				this.textBox.OnEnterPressed -= this.e;
				return;
			}
			if (sender.Text.Length >= 1)
			{
				if (Utility.areThereAnyOtherAnimalsWithThisName(sender.Text))
				{
					Game1.showRedMessage("Name Unavailable");
					return;
				}
				this.textBox.OnEnterPressed -= this.e;
				this.animalBeingPurchased.name = sender.Text;
				this.animalBeingPurchased.home = this.newAnimalHome;
				this.animalBeingPurchased.homeLocation = new Vector2((float)this.newAnimalHome.tileX, (float)this.newAnimalHome.tileY);
				this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
				(this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
				(this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
				this.newAnimalHome = null;
				this.namingAnimal = false;
				Game1.player.money -= this.priceOfAnimal;
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnAfterPurchasingAnimal), 0.02f);
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0013F2F8 File Offset: 0x0013D4F8
		public void setUpForReturnAfterPurchasingAnimal()
		{
			Game1.currentLocation.cleanupBeforePlayerExit();
			Game1.currentLocation = Game1.getLocationFromName("AnimalShop");
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.onFarm = false;
			this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
			Game1.displayHUD = true;
			Game1.displayFarmer = true;
			this.freeze = false;
			this.textBox.OnEnterPressed -= this.e;
			this.textBox.Selected = false;
			Game1.viewportFreeze = false;
			Game1.globalFadeToClear(new Game1.afterFadeFunction(this.marnieAnimalPurchaseMessage), 0.02f);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0013F3A8 File Offset: 0x0013D5A8
		public void marnieAnimalPurchaseMessage()
		{
			base.exitThisMenu(true);
			Game1.player.forceCanMove();
			this.freeze = false;
			Game1.drawDialogue(Game1.getCharacterFromName("Marnie", false), string.Concat(new string[]
			{
				"Great! I'll send little ",
				this.animalBeingPurchased.name,
				" to ",
				this.animalBeingPurchased.isMale() ? "his" : "her",
				" new home right away."
			}));
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0013F42C File Offset: 0x0013D62C
		public void setUpForAnimalPlacement()
		{
			Game1.displayFarmer = false;
			Game1.currentLocation = Game1.getLocationFromName("Farm");
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.onFarm = true;
			this.freeze = false;
			this.okButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
			this.okButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
			Game1.displayHUD = false;
			Game1.viewportFreeze = true;
			Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
			Game1.panScreen(0, 0);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0013F4E8 File Offset: 0x0013D6E8
		public void setUpForReturnToShopMenu()
		{
			this.freeze = false;
			Game1.displayFarmer = true;
			Game1.currentLocation.cleanupBeforePlayerExit();
			Game1.currentLocation = Game1.getLocationFromName("AnimalShop");
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.onFarm = false;
			this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
			this.okButton.bounds.Y = this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth;
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			this.namingAnimal = false;
			this.textBox.OnEnterPressed -= this.e;
			this.textBox.Selected = false;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0013F5B0 File Offset: 0x0013D7B0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.globalFade || this.freeze)
			{
				return;
			}
			if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
			{
				if (this.onFarm)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
					Game1.playSound("smallSelect");
				}
				else
				{
					Game1.exitActiveMenu();
					Game1.playSound("bigDeSelect");
				}
			}
			if (this.onFarm)
			{
				Vector2 clickTile = new Vector2((float)((x + Game1.viewport.X) / Game1.tileSize), (float)((y + Game1.viewport.Y) / Game1.tileSize));
				Building selection = (Game1.getLocationFromName("Farm") as Farm).getBuildingAt(clickTile);
				if (selection != null && !this.namingAnimal)
				{
					if (selection.buildingType.Contains(this.animalBeingPurchased.buildingTypeILiveIn))
					{
						if ((selection.indoors as AnimalHouse).isFull())
						{
							Game1.showRedMessage("That Building Is Full");
						}
						else if (this.animalBeingPurchased.harvestType != 2)
						{
							this.namingAnimal = true;
							this.newAnimalHome = selection;
							if (this.animalBeingPurchased.sound != null && Game1.soundBank != null)
							{
								Cue expr_14B = Game1.soundBank.GetCue(this.animalBeingPurchased.sound);
								expr_14B.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
								expr_14B.Play();
							}
							this.textBox.OnEnterPressed += this.e;
							Game1.keyboardDispatcher.Subscriber = this.textBox;
							this.textBox.Text = this.animalBeingPurchased.name;
							this.textBox.Selected = true;
						}
						else if (Game1.player.money >= this.priceOfAnimal)
						{
							this.newAnimalHome = selection;
							this.animalBeingPurchased.home = this.newAnimalHome;
							this.animalBeingPurchased.homeLocation = new Vector2((float)this.newAnimalHome.tileX, (float)this.newAnimalHome.tileY);
							this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
							(this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
							(this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
							this.newAnimalHome = null;
							this.namingAnimal = false;
							if (this.animalBeingPurchased.sound != null && Game1.soundBank != null)
							{
								Cue expr_2B5 = Game1.soundBank.GetCue(this.animalBeingPurchased.sound);
								expr_2B5.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
								expr_2B5.Play();
							}
							Game1.player.money -= this.priceOfAnimal;
							Game1.addHUDMessage(new HUDMessage("Purchased " + this.animalBeingPurchased.type, Color.LimeGreen, 3500f));
							this.animalBeingPurchased = new FarmAnimal(this.animalBeingPurchased.type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
						}
						else if (Game1.player.money < this.priceOfAnimal)
						{
							Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
						}
					}
					else
					{
						Game1.showRedMessage(this.animalBeingPurchased.type.Split(new char[]
						{
							' '
						}).Last<string>() + "s Can't Live There.");
					}
				}
				if (this.namingAnimal && this.doneNamingButton.containsPoint(x, y))
				{
					this.textBoxEnter(this.textBox);
					Game1.playSound("smallSelect");
					return;
				}
				if (this.namingAnimal && this.randomButton.containsPoint(x, y))
				{
					this.animalBeingPurchased.name = Dialogue.randomName();
					this.textBox.Text = this.animalBeingPurchased.name;
					this.randomButton.scale = this.randomButton.baseScale;
					Game1.playSound("drumkit6");
					return;
				}
			}
			else
			{
				foreach (ClickableTextureComponent c in this.animalsToPurchase)
				{
					if (c.containsPoint(x, y) && (c.item as Object).type == null)
					{
						int price = Convert.ToInt32(c.name);
						if (Game1.player.money >= price)
						{
							Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForAnimalPlacement), 0.02f);
							Game1.playSound("smallSelect");
							this.onFarm = true;
							this.animalBeingPurchased = new FarmAnimal(c.hoverText, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
							this.priceOfAnimal = price;
						}
						else
						{
							Game1.addHUDMessage(new HUDMessage("Not Enough Money", Color.Red, 3500f));
						}
					}
				}
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0013FAD4 File Offset: 0x0013DCD4
		public override void receiveKeyPress(Keys key)
		{
			if (Game1.globalFade || this.freeze)
			{
				return;
			}
			if (!Game1.globalFade && this.onFarm)
			{
				if (!this.namingAnimal)
				{
					if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
					{
						Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
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
						return;
					}
				}
			}
			else if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && !Game1.globalFade && this.readyToClose())
			{
				Game1.player.forceCanMove();
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0013FC04 File Offset: 0x0013DE04
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.onFarm && !this.namingAnimal)
			{
				int mouseX = Game1.getOldMouseX() + Game1.viewport.X;
				int mouseY = Game1.getOldMouseY() + Game1.viewport.Y;
				if (mouseX - Game1.viewport.X < Game1.tileSize)
				{
					Game1.panScreen(-8, 0);
				}
				else if (mouseX - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
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

		// Token: 0x06000F99 RID: 3993 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0013FCFC File Offset: 0x0013DEFC
		public override void performHoverAction(int x, int y)
		{
			this.hovered = null;
			if (Game1.globalFade || this.freeze)
			{
				return;
			}
			if (this.okButton != null)
			{
				if (this.okButton.containsPoint(x, y))
				{
					this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
				}
				else
				{
					this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
				}
			}
			if (this.onFarm)
			{
				Vector2 clickTile = new Vector2((float)((x + Game1.viewport.X) / Game1.tileSize), (float)((y + Game1.viewport.Y) / Game1.tileSize));
				Farm f = Game1.getLocationFromName("Farm") as Farm;
				using (List<Building>.Enumerator enumerator = f.buildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.color = Color.White;
					}
				}
				Building selection = f.getBuildingAt(clickTile);
				if (selection != null)
				{
					if (selection.buildingType.Contains(this.animalBeingPurchased.buildingTypeILiveIn) && !(selection.indoors as AnimalHouse).isFull())
					{
						selection.color = Color.LightGreen * 0.8f;
					}
					else
					{
						selection.color = Color.Red * 0.8f;
					}
				}
				if (this.doneNamingButton != null)
				{
					if (this.doneNamingButton.containsPoint(x, y))
					{
						this.doneNamingButton.scale = Math.Min(1.1f, this.doneNamingButton.scale + 0.05f);
					}
					else
					{
						this.doneNamingButton.scale = Math.Max(1f, this.doneNamingButton.scale - 0.05f);
					}
				}
				this.randomButton.tryHover(x, y, 0.5f);
				return;
			}
			foreach (ClickableTextureComponent c in this.animalsToPurchase)
			{
				if (c.containsPoint(x, y))
				{
					c.scale = Math.Min(c.scale + 0.05f, 4.1f);
					this.hovered = c;
				}
				else
				{
					c.scale = Math.Max(4f, c.scale - 0.025f);
				}
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0013FF7C File Offset: 0x0013E17C
		public static string getAnimalDescription(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 889009852u)
			{
				if (num != 292886277u)
				{
					if (num != 613168024u)
					{
						if (num == 889009852u)
						{
							if (name == "Chicken")
							{
								return "Well cared-for adult chickens lay eggs every day." + Environment.NewLine + "Lives in the coop.";
							}
						}
					}
					else if (name == "Duck")
					{
						return "Happy adults lay duck eggs every other day." + Environment.NewLine + "Lives in the coop.";
					}
				}
				else if (name == "Rabbit")
				{
					return "These are wooly rabbits! They shed precious wool every few days." + Environment.NewLine + "Lives in the coop.";
				}
			}
			else if (num <= 2392565758u)
			{
				if (num != 1601263067u)
				{
					if (num == 2392565758u)
					{
						if (name == "Goat")
						{
							return "Happy adults provide goat milk every other day. A milk pail is required to harvest the milk." + Environment.NewLine + "Lives in the barn.";
						}
					}
				}
				else if (name == "Dairy Cow")
				{
					return "Adults can be milked daily. A milk pail is required to harvest the milk." + Environment.NewLine + "Lives in the barn.";
				}
			}
			else if (num != 2952921236u)
			{
				if (num == 3444018991u)
				{
					if (name == "Pig")
					{
						return "These pigs are trained to find truffles!" + Environment.NewLine + "Lives in the barn.";
					}
				}
			}
			else if (name == "Sheep")
			{
				return "Adults can be shorn for wool. Sheep who form a close bond with their owners can grow wool faster. A pair of shears is required to harvest the wool." + Environment.NewLine + "Lives in the barn.";
			}
			return "";
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00140118 File Offset: 0x0013E318
		public override void draw(SpriteBatch b)
		{
			if (!this.onFarm && !Game1.dialogueUp && !Game1.globalFade)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
				SpriteText.drawStringWithScrollBackground(b, "Livestock:", this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen, "", 1f, -1);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
				Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.animalsToPurchase.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent c = enumerator.Current;
						c.draw(b, ((c.item as Object).type != null) ? (Color.Black * 0.4f) : Color.White, 0.87f);
					}
					goto IL_297;
				}
			}
			if (!Game1.globalFade && this.onFarm)
			{
				string s = "Choose a " + this.animalBeingPurchased.buildingTypeILiveIn + " for your new " + this.animalBeingPurchased.type.Split(new char[]
				{
					' '
				}).Last<string>();
				SpriteText.drawStringWithScrollBackground(b, s, Game1.viewport.Width / 2 - SpriteText.getWidthOfString(s) / 2, Game1.tileSize / 4, "", 1f, -1);
				if (this.namingAnimal)
				{
					b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
					Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 4, Game1.viewport.Height / 2 - Game1.tileSize * 3 - Game1.tileSize / 2, Game1.tileSize * 8, Game1.tileSize * 3, false, true, null, false);
					Utility.drawTextWithShadow(b, "Name your new animal: ", Game1.dialogueFont, new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 4 + Game1.tileSize / 2 + 8), (float)(Game1.viewport.Height / 2 - Game1.tileSize * 2 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					this.textBox.Draw(b);
					this.doneNamingButton.draw(b);
					this.randomButton.draw(b);
				}
			}
			IL_297:
			if (!Game1.globalFade && this.okButton != null)
			{
				this.okButton.draw(b);
			}
			if (this.hovered != null)
			{
				if ((this.hovered.item as Object).type != null)
				{
					IClickableMenu.drawHoverText(b, Game1.parseText((this.hovered.item as Object).type, Game1.dialogueFont, Game1.tileSize * 5), Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
				}
				else
				{
					SpriteText.drawStringWithScrollBackground(b, this.hovered.hoverText, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize, this.yPositionOnScreen + this.height + -Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "Truffle Pig", 1f, -1);
					SpriteText.drawStringWithScrollBackground(b, "$" + this.hovered.name + "g", this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, this.yPositionOnScreen + this.height + Game1.tileSize + IClickableMenu.spaceToClearTopBorder / 2 + 8, "$99999g", (Game1.player.Money >= Convert.ToInt32(this.hovered.name)) ? 1f : 0.5f, -1);
					IClickableMenu.drawHoverText(b, Game1.parseText(PurchaseAnimalsMenu.getAnimalDescription(this.hovered.hoverText), Game1.smallFont, Game1.tileSize * 5), Game1.smallFont, 0, 0, -1, this.hovered.hoverText, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
				}
			}
			base.drawMouse(b);
		}

		// Token: 0x040010CC RID: 4300
		public static int menuHeight = Game1.tileSize * 5;

		// Token: 0x040010CD RID: 4301
		public static int menuWidth = Game1.tileSize * 7;

		// Token: 0x040010CE RID: 4302
		private List<ClickableTextureComponent> animalsToPurchase = new List<ClickableTextureComponent>();

		// Token: 0x040010CF RID: 4303
		private ClickableTextureComponent okButton;

		// Token: 0x040010D0 RID: 4304
		private ClickableTextureComponent doneNamingButton;

		// Token: 0x040010D1 RID: 4305
		private ClickableTextureComponent randomButton;

		// Token: 0x040010D2 RID: 4306
		private ClickableTextureComponent hovered;

		// Token: 0x040010D3 RID: 4307
		private bool onFarm;

		// Token: 0x040010D4 RID: 4308
		private bool namingAnimal;

		// Token: 0x040010D5 RID: 4309
		private bool freeze;

		// Token: 0x040010D6 RID: 4310
		private FarmAnimal animalBeingPurchased;

		// Token: 0x040010D7 RID: 4311
		private TextBox textBox;

		// Token: 0x040010D8 RID: 4312
		private TextBoxEvent e;

		// Token: 0x040010D9 RID: 4313
		private Building newAnimalHome;

		// Token: 0x040010DA RID: 4314
		private int priceOfAnimal;
	}
}
