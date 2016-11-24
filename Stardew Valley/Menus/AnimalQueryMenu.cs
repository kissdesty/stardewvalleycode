using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	// Token: 0x020000D5 RID: 213
	public class AnimalQueryMenu : IClickableMenu
	{
		// Token: 0x06000D6D RID: 3437 RVA: 0x0010E334 File Offset: 0x0010C534
		public AnimalQueryMenu(FarmAnimal animal) : base(Game1.viewport.Width / 2 - AnimalQueryMenu.width / 2, Game1.viewport.Height / 2 - AnimalQueryMenu.height / 2, AnimalQueryMenu.width, AnimalQueryMenu.height, false)
		{
			Game1.player.Halt();
			Game1.player.faceGeneralDirection(animal.position, 0);
			AnimalQueryMenu.width = Game1.tileSize * 6;
			AnimalQueryMenu.height = Game1.tileSize * 8;
			this.animal = animal;
			this.textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
			this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 2 - 12;
			this.textBox.Y = this.yPositionOnScreen - 4 + Game1.tileSize * 2;
			this.textBox.Width = Game1.tileSize * 4;
			this.textBox.Height = Game1.tileSize * 3;
			this.textBox.Text = animal.name;
			this.textBox.Highlighted = false;
			Game1.keyboardDispatcher.Subscriber = this.textBox;
			this.textBox.Selected = false;
			if (animal.parentId != -1L)
			{
				FarmAnimal parent = Utility.getAnimal(animal.parentId);
				if (parent != null)
				{
					this.parentName = parent.name;
				}
			}
			if (animal.sound != null && Game1.soundBank != null)
			{
				Cue expr_173 = Game1.soundBank.GetCue(animal.sound);
				expr_173.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
				expr_173.Play();
			}
			this.okButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.sellButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 384, 16, 16), 4f, false);
			this.moveHomeButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 4 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(16, 384, 16, 16), 4f, false);
			if (!animal.isBaby() && !animal.isCoopDweller())
			{
				this.allowReproductionButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + Game1.pixelZoom * 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 2 - IClickableMenu.borderWidth + Game1.pixelZoom * 2, Game1.pixelZoom * 9, Game1.pixelZoom * 9), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(animal.allowReproduction ? 128 : 137, 393, 9, 9), 4f, false);
			}
			this.love = new ClickableTextureComponent(Math.Round((double)animal.friendshipTowardFarmer, 0) / 10.0 + "<", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2 + 16, this.yPositionOnScreen - Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 2, AnimalQueryMenu.width - Game1.tileSize * 2, Game1.tileSize), null, "Friendship", Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(172, 512, 16, 16), 4f, false);
			this.loveHover = new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 2, AnimalQueryMenu.width, Game1.tileSize), "Friendship");
			this.fullnessLevel = (double)((float)animal.fullness / 255f);
			if (animal.home != null && animal.home.indoors != null)
			{
				int piecesHay = animal.home.indoors.numberOfObjectsWithName("Hay");
				if (piecesHay > 0)
				{
					int numAnimals = (animal.home.indoors as AnimalHouse).animalsThatLiveHere.Count;
					this.fullnessLevel = Math.Min(1.0, this.fullnessLevel + (double)piecesHay / (double)numAnimals);
				}
			}
			else
			{
				Utility.fixAllAnimals();
			}
			this.happinessLevel = (double)((float)animal.happiness / 255f);
			this.loveLevel = (double)((float)animal.friendshipTowardFarmer / 1000f);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00002834 File Offset: 0x00000A34
		public void textBoxEnter(TextBox sender)
		{
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0010E828 File Offset: 0x0010CA28
		public override void receiveKeyPress(Keys key)
		{
			if (Game1.globalFade)
			{
				return;
			}
			if (Game1.options.menuButton.Contains(new InputButton(key)) && (this.textBox == null || !this.textBox.Selected))
			{
				Game1.playSound("smallSelect");
				if (this.readyToClose())
				{
					Game1.exitActiveMenu();
					if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
					{
						this.animal.name = this.textBox.Text;
						return;
					}
				}
				else if (this.movingAnimal)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
				}
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0010E8E0 File Offset: 0x0010CAE0
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.movingAnimal)
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

		// Token: 0x06000D71 RID: 3441 RVA: 0x0010E9D0 File Offset: 0x0010CBD0
		public void finishedPlacingAnimal()
		{
			Game1.exitActiveMenu();
			Game1.currentLocation = Game1.player.currentLocation;
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			Game1.displayFarmer = true;
			Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_HomeChanged", new object[0]), Color.LimeGreen, 3500f));
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0010EA44 File Offset: 0x0010CC44
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.globalFade)
			{
				return;
			}
			if (this.movingAnimal)
			{
				if (this.okButton != null && this.okButton.containsPoint(x, y))
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
					Game1.playSound("smallSelect");
				}
				Vector2 clickTile = new Vector2((float)((x + Game1.viewport.X) / Game1.tileSize), (float)((y + Game1.viewport.Y) / Game1.tileSize));
				Building selection = (Game1.getLocationFromName("Farm") as Farm).getBuildingAt(clickTile);
				if (selection != null)
				{
					if (!selection.buildingType.Contains(this.animal.buildingTypeILiveIn))
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_CantLiveThere", new object[]
						{
							this.animal.type.Split(new char[]
							{
								' '
							}).Last<string>()
						}));
						return;
					}
					if ((selection.indoors as AnimalHouse).isFull())
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_BuildingFull", new object[0]));
						return;
					}
					if (selection.Equals(this.animal.home))
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_AlreadyHome", new object[0]));
						return;
					}
					(this.animal.home.indoors as AnimalHouse).animalsThatLiveHere.Remove(this.animal.myID);
					if ((this.animal.home.indoors as AnimalHouse).animals.ContainsKey(this.animal.myID))
					{
						(selection.indoors as AnimalHouse).animals.Add(this.animal.myID, this.animal);
						(this.animal.home.indoors as AnimalHouse).animals.Remove(this.animal.myID);
					}
					this.animal.home = selection;
					this.animal.homeLocation = new Vector2((float)selection.tileX, (float)selection.tileY);
					(selection.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animal.myID);
					this.animal.makeSound();
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.finishedPlacingAnimal), 0.02f);
					return;
				}
			}
			else if (this.confirmingSell)
			{
				if (this.yesButton.containsPoint(x, y))
				{
					Game1.player.money += this.animal.getSellPrice();
					(this.animal.home.indoors as AnimalHouse).animalsThatLiveHere.Remove(this.animal.myID);
					this.animal.health = -1;
					int numClouds = this.animal.frontBackSourceRect.Width / 2;
					for (int i = 0; i < numClouds; i++)
					{
						int nonRedness = Game1.random.Next(25, 200);
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, this.animal.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, this.animal.frontBackSourceRect.Width * 3), (float)Game1.random.Next(-Game1.tileSize / 2, this.animal.frontBackSourceRect.Height * 3)), new Color(255 - nonRedness, 255, 255 - nonRedness), 8, false, (float)((Game1.random.NextDouble() < 0.5) ? 50 : Game1.random.Next(30, 200)), 0, Game1.tileSize, -1f, Game1.tileSize, (Game1.random.NextDouble() < 0.5) ? 0 : Game1.random.Next(0, 600))
						{
							scale = (float)Game1.random.Next(2, 5) * 0.25f,
							alpha = (float)Game1.random.Next(2, 5) * 0.25f,
							motion = new Vector2(0f, (float)(-(float)Game1.random.NextDouble()))
						});
					}
					Game1.playSound("newRecipe");
					Game1.playSound("money");
					Game1.exitActiveMenu();
					return;
				}
				if (this.noButton.containsPoint(x, y))
				{
					this.confirmingSell = false;
					Game1.playSound("smallSelect");
					return;
				}
			}
			else
			{
				if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
				{
					Game1.exitActiveMenu();
					if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
					{
						this.animal.name = this.textBox.Text;
					}
					Game1.playSound("smallSelect");
				}
				if (this.sellButton.containsPoint(x, y))
				{
					this.confirmingSell = true;
					this.yesButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.Width / 2 - Game1.tileSize - 4, Game1.viewport.Height / 2 - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
					this.noButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.Width / 2 + 4, Game1.viewport.Height / 2 - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
					Game1.playSound("smallSelect");
				}
				if (this.moveHomeButton.containsPoint(x, y))
				{
					Game1.playSound("smallSelect");
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForAnimalPlacement), 0.02f);
				}
				if (this.allowReproductionButton != null && this.allowReproductionButton.containsPoint(x, y))
				{
					Game1.playSound("drumkit6");
					this.animal.allowReproduction = !this.animal.allowReproduction;
					if (this.animal.allowReproduction)
					{
						this.allowReproductionButton.sourceRect.X = 128;
					}
					else
					{
						this.allowReproductionButton.sourceRect.X = 137;
					}
				}
				this.textBox.Update();
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0010F0BC File Offset: 0x0010D2BC
		public void prepareForAnimalPlacement()
		{
			this.movingAnimal = true;
			Game1.currentLocation = Game1.getLocationFromName("Farm");
			Game1.globalFadeToClear(null, 0.02f);
			this.okButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
			this.okButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
			Game1.displayHUD = false;
			Game1.viewportFreeze = true;
			Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
			Game1.panScreen(0, 0);
			Game1.currentLocation.resetForPlayerEntry();
			Game1.displayFarmer = false;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0010F170 File Offset: 0x0010D370
		public void prepareForReturnFromPlacement()
		{
			Game1.currentLocation = Game1.player.currentLocation;
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.okButton.bounds.X = this.xPositionOnScreen + AnimalQueryMenu.width + 4;
			this.okButton.bounds.Y = this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize - IClickableMenu.borderWidth;
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			Game1.displayFarmer = true;
			this.movingAnimal = false;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0010F200 File Offset: 0x0010D400
		public override bool readyToClose()
		{
			this.textBox.Selected = false;
			return base.readyToClose() && !this.movingAnimal && !Game1.globalFade;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0010F228 File Offset: 0x0010D428
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (Game1.globalFade)
			{
				return;
			}
			if (this.readyToClose())
			{
				Game1.exitActiveMenu();
				if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
				{
					this.animal.name = this.textBox.Text;
				}
				Game1.playSound("smallSelect");
				return;
			}
			if (this.movingAnimal)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0010F2B0 File Offset: 0x0010D4B0
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			if (this.movingAnimal)
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
					if (selection.buildingType.Contains(this.animal.buildingTypeILiveIn) && !(selection.indoors as AnimalHouse).isFull() && !selection.Equals(this.animal.home))
					{
						selection.color = Color.LightGreen * 0.8f;
					}
					else
					{
						selection.color = Color.Red * 0.8f;
					}
				}
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
			if (this.sellButton != null)
			{
				if (this.sellButton.containsPoint(x, y))
				{
					this.sellButton.scale = Math.Min(4.1f, this.sellButton.scale + 0.05f);
					this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Sell", new object[]
					{
						this.animal.getSellPrice()
					});
				}
				else
				{
					this.sellButton.scale = Math.Max(4f, this.sellButton.scale - 0.05f);
				}
			}
			if (this.moveHomeButton != null)
			{
				if (this.moveHomeButton.containsPoint(x, y))
				{
					this.moveHomeButton.scale = Math.Min(4.1f, this.moveHomeButton.scale + 0.05f);
					this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Move", new object[0]);
				}
				else
				{
					this.moveHomeButton.scale = Math.Max(4f, this.moveHomeButton.scale - 0.05f);
				}
			}
			if (this.allowReproductionButton != null)
			{
				if (this.allowReproductionButton.containsPoint(x, y))
				{
					this.allowReproductionButton.scale = Math.Min(4.1f, this.allowReproductionButton.scale + 0.05f);
					this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_AllowReproduction", new object[0]);
				}
				else
				{
					this.allowReproductionButton.scale = Math.Max(4f, this.allowReproductionButton.scale - 0.05f);
				}
			}
			if (this.yesButton != null)
			{
				if (this.yesButton.containsPoint(x, y))
				{
					this.yesButton.scale = Math.Min(1.1f, this.yesButton.scale + 0.05f);
				}
				else
				{
					this.yesButton.scale = Math.Max(1f, this.yesButton.scale - 0.05f);
				}
			}
			if (this.noButton != null)
			{
				if (this.noButton.containsPoint(x, y))
				{
					this.noButton.scale = Math.Min(1.1f, this.noButton.scale + 0.05f);
					return;
				}
				this.noButton.scale = Math.Max(1f, this.noButton.scale - 0.05f);
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0010F690 File Offset: 0x0010D890
		public override void draw(SpriteBatch b)
		{
			if (!this.movingAnimal && !Game1.globalFade)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen + Game1.tileSize * 2, AnimalQueryMenu.width, AnimalQueryMenu.height - Game1.tileSize * 2, false, true, null, false);
				if (this.animal.harvestType != 2)
				{
					this.textBox.Draw(b);
				}
				int age = (this.animal.age + 1) / 28 + 1;
				string ageText;
				if (age > 1)
				{
					ageText = Game1.content.LoadString("Strings\\UI:AnimalQuery_AgeN", new object[]
					{
						age
					});
				}
				else
				{
					ageText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Age1", new object[0]);
				}
				if (this.animal.age < (int)this.animal.ageWhenMature)
				{
					ageText += Game1.content.LoadString("Strings\\UI:AnimalQuery_AgeBaby", new object[0]);
				}
				Utility.drawTextWithShadow(b, ageText, Game1.smallFont, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4 + Game1.tileSize * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				int yOffset = 0;
				if (this.parentName != null)
				{
					yOffset = Game1.tileSize / 3;
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AnimalQuery_Parent", new object[]
					{
						this.parentName
					}), Game1.smallFont, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(Game1.tileSize / 2 + this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4 + Game1.tileSize * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
				int halfHeart = (int)((this.loveLevel * 1000.0 % 200.0 >= 100.0) ? (this.loveLevel * 1000.0 / 200.0) : -100.0);
				for (int i = 0; i < 5; i++)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 / 2 + 8 * Game1.pixelZoom * i), (float)(yOffset + this.yPositionOnScreen - Game1.tileSize / 2 + Game1.tileSize * 5)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(211 + ((this.loveLevel * 1000.0 <= (double)((i + 1) * 195)) ? 7 : 0), 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
					if (halfHeart == i)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 / 2 + 8 * Game1.pixelZoom * i), (float)(yOffset + this.yPositionOnScreen - Game1.tileSize / 2 + Game1.tileSize * 5)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(211, 428, 4, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.891f);
					}
				}
				Utility.drawTextWithShadow(b, Game1.parseText(this.animal.getMoodMessage(), Game1.smallFont, AnimalQueryMenu.width - IClickableMenu.spaceToClearSideBorder * 2 - Game1.tileSize), Game1.smallFont, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(yOffset + this.yPositionOnScreen + Game1.tileSize * 6 - Game1.tileSize + Game1.pixelZoom)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				this.okButton.draw(b);
				this.sellButton.draw(b);
				this.moveHomeButton.draw(b);
				if (this.allowReproductionButton != null)
				{
					this.allowReproductionButton.draw(b);
				}
				if (this.confirmingSell)
				{
					b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
					Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 5 / 2, Game1.viewport.Height / 2 - Game1.tileSize * 3, Game1.tileSize * 5, Game1.tileSize * 4, false, true, null, false);
					string confirmText = Game1.content.LoadString("Strings\\UI:AnimalQuery_ConfirmSell", new object[0]);
					b.DrawString(Game1.dialogueFont, confirmText, new Vector2((float)(Game1.viewport.Width / 2) - Game1.dialogueFont.MeasureString(confirmText).X / 2f, (float)(Game1.viewport.Height / 2 - Game1.tileSize * 3 / 2 + 8)), Game1.textColor);
					this.yesButton.draw(b);
					this.noButton.draw(b);
				}
				else if (this.hoverText != null && this.hoverText.Length > 0)
				{
					IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
				}
			}
			else if (!Game1.globalFade)
			{
				string s = Game1.content.LoadString("Strings\\UI:AnimalQuery_ChooseBuilding", new object[]
				{
					this.animal.buildingTypeILiveIn,
					this.animal.type.Split(new char[]
					{
						' '
					}).Last<string>()
				});
				Game1.drawDialogueBox(Game1.tileSize / 2, -Game1.tileSize, (int)Game1.dialogueFont.MeasureString(s).X + IClickableMenu.borderWidth * 2 + Game1.tileSize / 4, Game1.tileSize * 2 + IClickableMenu.borderWidth * 2, false, true, null, false);
				b.DrawString(Game1.dialogueFont, s, new Vector2((float)(Game1.tileSize / 2 + IClickableMenu.spaceToClearSideBorder * 2 + 8), (float)(Game1.tileSize / 2 + Game1.pixelZoom * 3)), Game1.textColor);
				this.okButton.draw(b);
			}
			base.drawMouse(b);
		}

		// Token: 0x04000DDC RID: 3548
		public new static int width = Game1.tileSize * 6;

		// Token: 0x04000DDD RID: 3549
		public new static int height = Game1.tileSize * 8;

		// Token: 0x04000DDE RID: 3550
		private FarmAnimal animal;

		// Token: 0x04000DDF RID: 3551
		private TextBox textBox;

		// Token: 0x04000DE0 RID: 3552
		private TextBoxEvent e;

		// Token: 0x04000DE1 RID: 3553
		private ClickableTextureComponent okButton;

		// Token: 0x04000DE2 RID: 3554
		private ClickableTextureComponent love;

		// Token: 0x04000DE3 RID: 3555
		private ClickableTextureComponent sellButton;

		// Token: 0x04000DE4 RID: 3556
		private ClickableTextureComponent moveHomeButton;

		// Token: 0x04000DE5 RID: 3557
		private ClickableTextureComponent yesButton;

		// Token: 0x04000DE6 RID: 3558
		private ClickableTextureComponent noButton;

		// Token: 0x04000DE7 RID: 3559
		private ClickableTextureComponent allowReproductionButton;

		// Token: 0x04000DE8 RID: 3560
		private ClickableComponent fullnessHover;

		// Token: 0x04000DE9 RID: 3561
		private ClickableComponent happinessHover;

		// Token: 0x04000DEA RID: 3562
		private ClickableComponent loveHover;

		// Token: 0x04000DEB RID: 3563
		private double fullnessLevel;

		// Token: 0x04000DEC RID: 3564
		private double happinessLevel;

		// Token: 0x04000DED RID: 3565
		private double loveLevel;

		// Token: 0x04000DEE RID: 3566
		private bool confirmingSell;

		// Token: 0x04000DEF RID: 3567
		private bool movingAnimal;

		// Token: 0x04000DF0 RID: 3568
		private string hoverText = "";

		// Token: 0x04000DF1 RID: 3569
		private string parentName;
	}
}
