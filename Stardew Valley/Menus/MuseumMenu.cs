using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	// Token: 0x02000102 RID: 258
	public class MuseumMenu : MenuWithInventory
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x0013B2DC File Offset: 0x001394DC
		public MuseumMenu() : base(new InventoryMenu.highlightThisItem((Game1.currentLocation as LibraryMuseum).isItemSuitableForDonation), true, false, 0, 0)
		{
			this.fadeTimer = 800;
			this.fadeIntoBlack = true;
			base.movePosition(0, Game1.viewport.Height - this.yPositionOnScreen - this.height);
			Game1.player.forceCanMove();
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0013B344 File Offset: 0x00139544
		public override void receiveKeyPress(Keys key)
		{
			if (this.fadeTimer <= 0)
			{
				if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
				{
					this.state = 2;
					this.fadeTimer = 500;
					this.fadeIntoBlack = true;
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

		// Token: 0x06000F50 RID: 3920 RVA: 0x0013B414 File Offset: 0x00139614
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.fadeTimer <= 0)
			{
				Item oldItem = this.heldItem;
				if (!this.holdingMuseumPiece)
				{
					this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
				}
				if (oldItem != null && this.heldItem != null && (y < Game1.viewport.Height - (this.height - (IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize)) || this.menuMovingDown))
				{
					int mapXTile = (x + Game1.viewport.X) / Game1.tileSize;
					int mapYTile = (y + Game1.viewport.Y) / Game1.tileSize;
					if ((Game1.currentLocation as LibraryMuseum).isTileSuitableForMuseumPiece(mapXTile, mapYTile) && (Game1.currentLocation as LibraryMuseum).isItemSuitableForDonation(this.heldItem))
					{
						int rewardsCount = (Game1.currentLocation as LibraryMuseum).getRewardsForPlayer(Game1.player).Count;
						(Game1.currentLocation as LibraryMuseum).museumPieces.Add(new Vector2((float)mapXTile, (float)mapYTile), (this.heldItem as Object).parentSheetIndex);
						Game1.playSound("stoneStep");
						this.holdingMuseumPiece = false;
						if ((Game1.currentLocation as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > rewardsCount)
						{
							this.sparkleText = new SparklingText(Game1.dialogueFont, "New Reward!", Color.MediumSpringGreen, Color.White, false, 0.1, 2500, -1, 500);
							Game1.playSound("reward");
							this.globalLocationOfSparklingArtifact = new Vector2((float)(mapXTile * Game1.tileSize + Game1.tileSize / 2) - this.sparkleText.textWidth / 2f, (float)(mapYTile * Game1.tileSize - Game1.tileSize * 3 / 4));
						}
						else
						{
							Game1.playSound("newArtifact");
						}
						Game1.player.completeQuest(24);
						Item expr_1DE = this.heldItem;
						int stack = expr_1DE.Stack;
						expr_1DE.Stack = stack - 1;
						if (this.heldItem.Stack <= 0)
						{
							this.heldItem = null;
						}
						this.menuMovingDown = false;
						int pieces = (Game1.currentLocation as LibraryMuseum).museumPieces.Count;
						if (pieces >= 95)
						{
							Game1.getAchievement(5);
						}
						else if (pieces >= 40)
						{
							Game1.getAchievement(28);
						}
					}
				}
				else if (this.heldItem == null)
				{
					int mapXTile2 = (x + Game1.viewport.X) / Game1.tileSize;
					int mapYTile2 = (y + Game1.viewport.Y) / Game1.tileSize;
					Vector2 v = new Vector2((float)mapXTile2, (float)mapYTile2);
					if ((Game1.currentLocation as LibraryMuseum).museumPieces.ContainsKey(v))
					{
						this.heldItem = new Object((Game1.currentLocation as LibraryMuseum).museumPieces[v], 1, false, -1, 0);
						(Game1.currentLocation as LibraryMuseum).museumPieces.Remove(v);
						this.holdingMuseumPiece = true;
					}
				}
				if (this.heldItem != null && oldItem == null)
				{
					this.menuMovingDown = true;
				}
				if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
				{
					this.state = 2;
					this.fadeTimer = 800;
					this.fadeIntoBlack = true;
					Game1.playSound("bigDeSelect");
				}
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0013B758 File Offset: 0x00139958
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			Item oldItem = this.heldItem;
			if (this.fadeTimer <= 0)
			{
				base.receiveRightClick(x, y, true);
			}
			if (this.heldItem != null && oldItem == null)
			{
				this.menuMovingDown = true;
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0013B790 File Offset: 0x00139990
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.sparkleText != null && this.sparkleText.update(time))
			{
				this.sparkleText = null;
			}
			if (this.fadeTimer > 0)
			{
				this.fadeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.fadeIntoBlack)
				{
					this.blackFadeAlpha = 0f + (1500f - (float)this.fadeTimer) / 1500f;
				}
				else
				{
					this.blackFadeAlpha = 1f - (1500f - (float)this.fadeTimer) / 1500f;
				}
				if (this.fadeTimer <= 0)
				{
					switch (this.state)
					{
					case 0:
						this.state = 1;
						Game1.viewportFreeze = true;
						Game1.viewport.Location = new Location(18 * Game1.tileSize, 2 * Game1.tileSize);
						Game1.clampViewportToGameMap();
						this.fadeTimer = 800;
						this.fadeIntoBlack = false;
						break;
					case 2:
						Game1.viewportFreeze = false;
						this.fadeIntoBlack = false;
						this.fadeTimer = 800;
						this.state = 3;
						break;
					case 3:
						Game1.exitActiveMenu();
						break;
					}
				}
			}
			if (this.menuMovingDown && this.menuPositionOffset < this.height / 3)
			{
				this.menuPositionOffset += 8;
				base.movePosition(0, 8);
			}
			else if (!this.menuMovingDown && this.menuPositionOffset > 0)
			{
				this.menuPositionOffset -= 8;
				base.movePosition(0, -8);
			}
			int mouseX = Game1.getOldMouseX() + Game1.viewport.X;
			int mouseY = Game1.getOldMouseY() + Game1.viewport.Y;
			if (mouseX - Game1.viewport.X < Game1.tileSize)
			{
				Game1.panScreen(-4, 0);
			}
			else if (mouseX - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
			{
				Game1.panScreen(4, 0);
			}
			if (mouseY - Game1.viewport.Y < Game1.tileSize)
			{
				Game1.panScreen(0, -4);
			}
			else if (mouseY - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
			{
				Game1.panScreen(0, 4);
				if (this.menuMovingDown)
				{
					this.menuMovingDown = false;
				}
			}
			Keys[] pressedKeys = Game1.oldKBState.GetPressedKeys();
			for (int i = 0; i < pressedKeys.Length; i++)
			{
				Keys key = pressedKeys[i];
				this.receiveKeyPress(key);
			}
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0013B9FD File Offset: 0x00139BFD
		public override void gameWindowSizeChanged(Microsoft.Xna.Framework.Rectangle oldBounds, Microsoft.Xna.Framework.Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			base.movePosition(0, Game1.viewport.Height - this.yPositionOnScreen - this.height);
			Game1.player.forceCanMove();
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0013BA30 File Offset: 0x00139C30
		public override void draw(SpriteBatch b)
		{
			if ((this.fadeTimer <= 0 || !this.fadeIntoBlack) && this.state != 3)
			{
				if (this.heldItem != null)
				{
					for (int y = Game1.viewport.Y / Game1.tileSize - 1; y < (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 2; y++)
					{
						for (int x = Game1.viewport.X / Game1.tileSize - 1; x < (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 1; x++)
						{
							if ((Game1.currentLocation as LibraryMuseum).isTileSuitableForMuseumPiece(x, y))
							{
								b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)x, (float)y) * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1)), Color.LightGreen);
							}
						}
					}
				}
				if (!this.holdingMuseumPiece)
				{
					base.draw(b, false, false);
				}
				if (!this.hoverText.Equals(""))
				{
					IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
				}
				if (this.heldItem != null)
				{
					this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
				}
				base.drawMouse(b);
				if (this.sparkleText != null)
				{
					this.sparkleText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.globalLocationOfSparklingArtifact));
				}
			}
			b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * this.blackFadeAlpha);
		}

		// Token: 0x04001075 RID: 4213
		public const int startingState = 0;

		// Token: 0x04001076 RID: 4214
		public const int placingInMuseumState = 1;

		// Token: 0x04001077 RID: 4215
		public const int exitingState = 2;

		// Token: 0x04001078 RID: 4216
		public int fadeTimer;

		// Token: 0x04001079 RID: 4217
		public int state;

		// Token: 0x0400107A RID: 4218
		public int menuPositionOffset;

		// Token: 0x0400107B RID: 4219
		public bool fadeIntoBlack;

		// Token: 0x0400107C RID: 4220
		public bool menuMovingDown;

		// Token: 0x0400107D RID: 4221
		public float blackFadeAlpha;

		// Token: 0x0400107E RID: 4222
		public SparklingText sparkleText;

		// Token: 0x0400107F RID: 4223
		public Vector2 globalLocationOfSparklingArtifact;

		// Token: 0x04001080 RID: 4224
		private bool holdingMuseumPiece;
	}
}
