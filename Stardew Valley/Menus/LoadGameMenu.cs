using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x020000FC RID: 252
	public class LoadGameMenu : IClickableMenu
	{
		// Token: 0x06000F1F RID: 3871 RVA: 0x0013690C File Offset: 0x00134B0C
		public LoadGameMenu() : base(Game1.viewport.Width / 2 - (1100 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 1100 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, false)
		{
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
			this.okDeleteButton = new ClickableTextureComponent("OK", new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X - Game1.tileSize, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.cancelDeleteButton = new ClickableTextureComponent("Cancel", new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X + Game1.tileSize, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
			for (int i = 0; i < 4; i++)
			{
				this.gamesToLoadButton.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * (this.height / 4), this.width - Game1.tileSize / 2, this.height / 4 + Game1.pixelZoom), string.Concat(i)));
				this.deleteButtons.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom + i * (this.height / 4), 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "Delete File", Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float)Game1.pixelZoom * 3f / 4f, false));
			}
			string pathToDirectory = Path.Combine(new string[]
			{
				Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves")
			});
			if (Directory.Exists(pathToDirectory))
			{
				string[] directories = Directory.GetDirectories(pathToDirectory);
				if (directories.Length != 0)
				{
					string[] array = directories;
					for (int j = 0; j < array.Length; j++)
					{
						string s = array[j];
						try
						{
							Stream stream = null;
							try
							{
								stream = File.Open(Path.Combine(pathToDirectory, s, "SaveGameInfo"), FileMode.Open);
							}
							catch (IOException)
							{
								if (stream != null)
								{
									stream.Close();
								}
							}
							if (stream != null)
							{
								Farmer f = (Farmer)SaveGame.farmerSerializer.Deserialize(stream);
								SaveGame.loadDataToFarmer(f, f);
								f.favoriteThing = s.Split(new char[]
								{
									Path.DirectorySeparatorChar
								}).Last<string>();
								this.saveGames.Add(f);
								stream.Close();
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			this.saveGames.Sort();
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00136E6C File Offset: 0x0013506C
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
			this.okDeleteButton = new ClickableTextureComponent("OK", new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X - Game1.tileSize, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.cancelDeleteButton = new ClickableTextureComponent("Cancel", new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X + Game1.tileSize, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
			this.gamesToLoadButton.Clear();
			this.deleteButtons.Clear();
			for (int i = 0; i < 4; i++)
			{
				this.gamesToLoadButton.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * (this.height / 4), this.width - Game1.tileSize / 2, this.height / 4 + Game1.pixelZoom), string.Concat(i)));
				this.deleteButtons.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom + i * (this.height / 4), 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "Delete File", Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float)Game1.pixelZoom * 3f / 4f, false));
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0013724C File Offset: 0x0013544C
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			base.performHoverAction(x, y);
			if (!this.deleteConfirmationScreen)
			{
				this.upArrow.tryHover(x, y, 0.1f);
				this.downArrow.tryHover(x, y, 0.1f);
				this.scrollBar.tryHover(x, y, 0.1f);
				foreach (ClickableTextureComponent expr_C0 in this.deleteButtons)
				{
					expr_C0.tryHover(x, y, 0.2f);
					if (expr_C0.containsPoint(x, y))
					{
						this.hoverText = "Delete File";
						return;
					}
				}
				if (this.scrolling)
				{
					return;
				}
				for (int i = 0; i < this.gamesToLoadButton.Count; i++)
				{
					if (this.currentItemIndex + i < this.saveGames.Count && this.gamesToLoadButton[i].containsPoint(x, y))
					{
						if (this.gamesToLoadButton[i].scale == 1f)
						{
							Game1.playSound("Cowboy_gunshot");
						}
						this.gamesToLoadButton[i].scale = Math.Min(this.gamesToLoadButton[i].scale + 0.03f, 1.1f);
					}
					else
					{
						this.gamesToLoadButton[i].scale = Math.Max(1f, this.gamesToLoadButton[i].scale - 0.03f);
					}
				}
				return;
			}
			this.okDeleteButton.tryHover(x, y, 0.1f);
			this.cancelDeleteButton.tryHover(x, y, 0.1f);
			if (this.okDeleteButton.containsPoint(x, y))
			{
				this.hoverText = "";
				return;
			}
			if (this.cancelDeleteButton.containsPoint(x, y))
			{
				this.hoverText = "Cancel";
			}
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00137440 File Offset: 0x00135640
		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_E8_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
				float percentage = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.currentItemIndex = Math.Min(this.saveGames.Count - 4, Math.Max(0, (int)((float)this.saveGames.Count * percentage)));
				this.setScrollBarToCurrentIndex();
				if (arg_E8_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
				}
			}
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00137541 File Offset: 0x00135741
		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.scrolling = false;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00137554 File Offset: 0x00135754
		private void setScrollBarToCurrentIndex()
		{
			if (this.saveGames.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.saveGames.Count - 4 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
				if (this.currentItemIndex == this.saveGames.Count - 4)
				{
					this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00137610 File Offset: 0x00135810
		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				return;
			}
			if (direction < 0 && this.currentItemIndex < Math.Max(0, this.saveGames.Count - 4))
			{
				this.downArrowPressed();
			}
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0013765D File Offset: 0x0013585D
		private void downArrowPressed()
		{
			this.downArrow.scale = this.downArrow.baseScale;
			this.currentItemIndex++;
			Game1.playSound("shwip");
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00137693 File Offset: 0x00135893
		private void upArrowPressed()
		{
			this.upArrow.scale = this.upArrow.baseScale;
			this.currentItemIndex--;
			Game1.playSound("shwip");
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x001376CC File Offset: 0x001358CC
		public void deleteFile(int which)
		{
			Farmer f = this.saveGames[which];
			string filenameNoTmpString = f.favoriteThing;
			string fullFilePath = Path.Combine(new string[]
			{
				Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), filenameNoTmpString)
			});
			if (Directory.Exists(fullFilePath))
			{
				Directory.Delete(fullFilePath, true);
				this.saveGames.Remove(f);
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0013773C File Offset: 0x0013593C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.timerToLoad > 0 || this.loading)
			{
				return;
			}
			if (!this.deleteConfirmationScreen)
			{
				base.receiveLeftClick(x, y, playSound);
				if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.saveGames.Count - 4))
				{
					this.downArrowPressed();
				}
				else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
				{
					this.upArrowPressed();
				}
				else if (this.scrollBar.containsPoint(x, y))
				{
					this.scrolling = true;
				}
				else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
				{
					this.scrolling = true;
					this.leftClickHeld(x, y);
					this.releaseLeftClick(x, y);
				}
				if (this.selected == -1)
				{
					for (int i = 0; i < this.deleteButtons.Count; i++)
					{
						if (this.deleteButtons[i].containsPoint(x, y) && i < this.saveGames.Count && !this.deleteConfirmationScreen)
						{
							this.deleteConfirmationScreen = true;
							Game1.playSound("drumkit6");
							this.selectedForDelete = this.currentItemIndex + i;
							return;
						}
					}
				}
				if (!this.deleteConfirmationScreen)
				{
					for (int j = 0; j < this.gamesToLoadButton.Count; j++)
					{
						if (this.gamesToLoadButton[j].containsPoint(x, y) && j < this.saveGames.Count)
						{
							this.timerToLoad = 2150;
							this.loading = true;
							Game1.playSound("select");
							this.selected = this.currentItemIndex + j;
							return;
						}
					}
				}
				this.currentItemIndex = Math.Max(0, Math.Min(this.saveGames.Count - 4, this.currentItemIndex));
				return;
			}
			if (this.cancelDeleteButton.containsPoint(x, y))
			{
				this.deleteConfirmationScreen = false;
				this.selectedForDelete = -1;
				Game1.playSound("smallSelect");
				return;
			}
			if (this.okDeleteButton.containsPoint(x, y))
			{
				this.deleteFile(this.selectedForDelete);
				this.deleteConfirmationScreen = false;
				Game1.playSound("trashcan");
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00137990 File Offset: 0x00135B90
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.timerToLoad > 0)
			{
				this.timerToLoad -= time.ElapsedGameTime.Milliseconds;
				if (this.timerToLoad <= 0)
				{
					SaveGame.Load(this.saveGames[this.selected].favoriteThing);
					for (int i = 0; i < this.saveGames.Count; i++)
					{
						if (i != this.selected)
						{
							this.saveGames[i].unload();
						}
					}
					Game1.exitActiveMenu();
				}
			}
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00137A24 File Offset: 0x00135C24
		public override void draw(SpriteBatch b)
		{
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height + Game1.tileSize / 2, Color.White, (float)Game1.pixelZoom, true);
			for (int i = 0; i < this.gamesToLoadButton.Count; i++)
			{
				if (this.currentItemIndex + i < this.saveGames.Count)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.gamesToLoadButton[i].bounds.X, this.gamesToLoadButton[i].bounds.Y, this.gamesToLoadButton[i].bounds.Width, this.gamesToLoadButton[i].bounds.Height, ((this.currentItemIndex + i == this.selected && this.timerToLoad % 150 > 75 && this.timerToLoad > 1000) || (this.selected == -1 && this.gamesToLoadButton[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !this.scrolling && !this.deleteConfirmationScreen)) ? (this.deleteButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) ? Color.White : Color.Wheat) : Color.White, (float)Game1.pixelZoom, false);
					SpriteText.drawString(b, this.currentItemIndex + i + 1 + ".", this.gamesToLoadButton[i].bounds.X + Game1.pixelZoom * 7 + Game1.tileSize / 2 - SpriteText.getWidthOfString(this.currentItemIndex + i + 1 + ".") / 2, this.gamesToLoadButton[i].bounds.Y + Game1.pixelZoom * 9, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					SpriteText.drawString(b, this.saveGames[this.currentItemIndex + i].Name, this.gamesToLoadButton[i].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom * 9, this.gamesToLoadButton[i].bounds.Y + Game1.pixelZoom * 9, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					b.Draw(Game1.shadowTexture, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + Game1.tileSize + Game1.tileSize - Game1.pixelZoom), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.tileSize * 2 + Game1.pixelZoom * 4)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 0.8f);
					this.saveGames[this.currentItemIndex + i].FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(0, 0, false, false, null, false), 0, new Rectangle(0, 0, 16, 32), new Vector2((float)(this.gamesToLoadButton[i].bounds.X + Game1.tileSize / 4 + Game1.tileSize + Game1.pixelZoom * 3), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.pixelZoom * 5)), Vector2.Zero, 0.8f, 2, Color.White, 0f, 1f, this.saveGames[this.currentItemIndex + i]);
					Utility.drawTextWithShadow(b, this.saveGames[this.currentItemIndex + i].dateStringForSaveGame, Game1.dialogueFont, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom * 8), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.tileSize + Game1.pixelZoom * 10)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					Utility.drawTextWithShadow(b, this.saveGames[this.currentItemIndex + i].farmName + " Farm", Game1.dialogueFont, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + this.width - Game1.tileSize * 2) - Game1.dialogueFont.MeasureString(this.saveGames[this.currentItemIndex + i].farmName + " Farm").X, (float)(this.gamesToLoadButton[i].bounds.Y + Game1.pixelZoom * 11)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					int moneyWidth = (int)Game1.dialogueFont.MeasureString(Utility.getNumberWithCommas(this.saveGames[this.currentItemIndex + i].Money) + "g").X;
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom * 25 - moneyWidth), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.tileSize + Game1.pixelZoom * 11)), new Rectangle(193, 373, 9, 9), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Utility.getNumberWithCommas(this.saveGames[this.currentItemIndex + i].Money) + "g", Game1.dialogueFont, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom * 15 - moneyWidth), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.tileSize + Game1.pixelZoom * 11)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom * 11), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.tileSize + Game1.pixelZoom * 9)), new Rectangle(595, 1748, 9, 11), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Utility.getHoursMinutesStringFromMilliseconds(this.saveGames[this.currentItemIndex + i].millisecondsPlayed), Game1.dialogueFont, new Vector2((float)(this.gamesToLoadButton[i].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom), (float)(this.gamesToLoadButton[i].bounds.Y + Game1.tileSize + Game1.pixelZoom * 11)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					if (this.deleteButtons.Count > i)
					{
						this.deleteButtons[i].draw(b, Color.White * 0.75f, 1f);
					}
				}
			}
			if (this.saveGames.Count == 0)
			{
				SpriteText.drawStringHorizontallyCenteredAt(b, "No Saved Games Found", Game1.graphics.GraphicsDevice.Viewport.Bounds.Center.X, Game1.graphics.GraphicsDevice.Viewport.Bounds.Center.Y, 999999, -1, 999999, 1f, 0.88f, false, -1);
			}
			this.upArrow.draw(b);
			this.downArrow.draw(b);
			if (this.saveGames.Count > 4)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
				this.scrollBar.draw(b);
			}
			if (this.deleteConfirmationScreen)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.75f);
				SpriteText.drawString(b, "Really delete file: " + this.saveGames[this.selectedForDelete].name + "?", (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 8, Game1.tileSize, 0, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize, 0, 0).Y, 9999, -1, 9999, 1f, 1f, false, -1, "", 4);
				this.okDeleteButton.draw(b);
				this.cancelDeleteButton.draw(b);
			}
			base.draw(b);
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.selected != -1 && this.timerToLoad < 1000)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * (1f - (float)this.timerToLoad / 1000f));
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04001045 RID: 4165
		public const int itemsPerPage = 4;

		// Token: 0x04001046 RID: 4166
		private List<ClickableComponent> gamesToLoadButton = new List<ClickableComponent>();

		// Token: 0x04001047 RID: 4167
		private List<ClickableTextureComponent> deleteButtons = new List<ClickableTextureComponent>();

		// Token: 0x04001048 RID: 4168
		private int currentItemIndex;

		// Token: 0x04001049 RID: 4169
		private int timerToLoad;

		// Token: 0x0400104A RID: 4170
		private int selected = -1;

		// Token: 0x0400104B RID: 4171
		private int selectedForDelete = -1;

		// Token: 0x0400104C RID: 4172
		private ClickableTextureComponent upArrow;

		// Token: 0x0400104D RID: 4173
		private ClickableTextureComponent downArrow;

		// Token: 0x0400104E RID: 4174
		private ClickableTextureComponent scrollBar;

		// Token: 0x0400104F RID: 4175
		private ClickableTextureComponent okDeleteButton;

		// Token: 0x04001050 RID: 4176
		private ClickableTextureComponent cancelDeleteButton;

		// Token: 0x04001051 RID: 4177
		private bool scrolling;

		// Token: 0x04001052 RID: 4178
		private bool deleteConfirmationScreen;

		// Token: 0x04001053 RID: 4179
		private List<Farmer> saveGames = new List<Farmer>();

		// Token: 0x04001054 RID: 4180
		private Rectangle scrollBarRunner;

		// Token: 0x04001055 RID: 4181
		private string hoverText = "";

		// Token: 0x04001056 RID: 4182
		private bool loading;
	}
}
