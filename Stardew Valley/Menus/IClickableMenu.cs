using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;
using StardewValley.Tools;

namespace StardewValley.Menus
{
	// Token: 0x020000F2 RID: 242
	public abstract class IClickableMenu
	{
		// Token: 0x06000E7A RID: 3706 RVA: 0x0000282C File Offset: 0x00000A2C
		public IClickableMenu()
		{
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00127773 File Offset: 0x00125973
		public IClickableMenu(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
		{
			this.initialize(x, y, width, height, showUpperRightCloseButton);
			if (Game1.gameMode == 3 && Game1.player != null && !Game1.eventUp)
			{
				Game1.player.Halt();
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x001277A8 File Offset: 0x001259A8
		public void initialize(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
		{
			if (Game1.player != null && !Game1.player.UsingTool && !Game1.eventUp)
			{
				Game1.player.forceCanMove();
			}
			this.xPositionOnScreen = x;
			this.yPositionOnScreen = y;
			this.width = width;
			this.height = height;
			if (showUpperRightCloseButton)
			{
				this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width - 9 * Game1.pixelZoom, this.yPositionOnScreen - Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00127858 File Offset: 0x00125A58
		public virtual bool areGamePadControlsImplemented()
		{
			return this.gamePadControlsImplemented;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00127860 File Offset: 0x00125A60
		public ClickableComponent getLastClickableComponentInThisListThatContainsThisXCoord(List<ClickableComponent> ccList, int xCoord)
		{
			for (int i = ccList.Count - 1; i >= 0; i--)
			{
				if (ccList[i].bounds.Contains(xCoord, ccList[i].bounds.Center.Y))
				{
					return ccList[i];
				}
			}
			return null;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x001278B4 File Offset: 0x00125AB4
		public ClickableComponent getFirstClickableComponentInThisListThatContainsThisXCoord(List<ClickableComponent> ccList, int xCoord)
		{
			for (int i = 0; i < ccList.Count; i++)
			{
				if (ccList[i].bounds.Contains(xCoord, ccList[i].bounds.Center.Y))
				{
					return ccList[i];
				}
			}
			return null;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00127908 File Offset: 0x00125B08
		public ClickableComponent getLastClickableComponentInThisListThatContainsThisYCoord(List<ClickableComponent> ccList, int yCoord)
		{
			for (int i = ccList.Count - 1; i >= 0; i--)
			{
				if (ccList[i].bounds.Contains(ccList[i].bounds.Center.X, yCoord))
				{
					return ccList[i];
				}
			}
			return null;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0012795C File Offset: 0x00125B5C
		public ClickableComponent getFirstClickableComponentInThisListThatContainsThisYCoord(List<ClickableComponent> ccList, int yCoord)
		{
			for (int i = 0; i < ccList.Count; i++)
			{
				if (ccList[i].bounds.Contains(ccList[i].bounds.Center.X, yCoord))
				{
					return ccList[i];
				}
			}
			return null;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void receiveGamePadButton(Buttons b)
		{
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x001279B0 File Offset: 0x00125BB0
		public void drawMouse(SpriteBatch b)
		{
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White * Game1.mouseCursorTransparency, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00127A2C File Offset: 0x00125C2C
		public virtual int applyMovementKey(Keys key)
		{
			if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
			{
				return this.moveCursorInDirection(0);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
			{
				return this.moveCursorInDirection(1);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
			{
				return this.moveCursorInDirection(2);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
			{
				return this.moveCursorInDirection(3);
			}
			return -1;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00035C5D File Offset: 0x00033E5D
		public virtual int moveCursorInDirection(int direction)
		{
			return -1;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00127AB8 File Offset: 0x00125CB8
		public void initializeUpperRightCloseButton()
		{
			this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 9 * Game1.pixelZoom, this.yPositionOnScreen - Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00127B28 File Offset: 0x00125D28
		public virtual void drawBackground(SpriteBatch b)
		{
			if (this is ShopMenu)
			{
				for (int x = 0; x < Game1.viewport.Width; x += 100 * Game1.pixelZoom)
				{
					for (int y = 0; y < Game1.viewport.Height; y += 96 * Game1.pixelZoom)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(527, 0, 100, 96)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.08f);
					}
				}
				return;
			}
			if (Game1.isDarkOut())
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 144)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
			}
			else if (Game1.isRaining)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(640, 858, 1, 184)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
			}
			else
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639 + Utility.getSeasonNumber(Game1.currentSeason), 1051, 1, 400)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(-30 * Game1.pixelZoom), (float)(Game1.viewport.Height - 148 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1035 : ((Game1.isRaining || Game1.isDarkOut()) ? 886 : 737), 639, 148)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.08f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(-30 * Game1.pixelZoom + 639 * Game1.pixelZoom), (float)(Game1.viewport.Height - 148 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1035 : ((Game1.isRaining || Game1.isDarkOut()) ? 886 : 737), 639, 148)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.08f);
			if (Game1.isRaining)
			{
				b.Draw(Game1.staminaRect, Utility.xTileToMicrosoftRectangle(Game1.viewport), Color.Blue * 0.2f);
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00127E33 File Offset: 0x00126033
		public virtual bool showWithoutTransparencyIfOptionIsSet()
		{
			return this is GameMenu || this is ShopMenu || this is WheelSpinGame || this is ItemGrabMenu;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void clickAway()
		{
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00127E58 File Offset: 0x00126058
		public virtual void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = (int)((float)newBounds.Width * ((float)this.xPositionOnScreen / (float)oldBounds.Width));
			this.yPositionOnScreen = (int)((float)newBounds.Height * ((float)this.yPositionOnScreen / (float)oldBounds.Height));
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void setUpForGamePadMode()
		{
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00127E96 File Offset: 0x00126096
		public virtual void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.upperRightCloseButton != null && this.readyToClose() && this.upperRightCloseButton.containsPoint(x, y))
			{
				if (playSound)
				{
					Game1.playSound("bigDeSelect");
				}
				this.exitThisMenu(true);
			}
		}

		// Token: 0x06000E8F RID: 3727
		public abstract void receiveRightClick(int x, int y, bool playSound = true);

		// Token: 0x06000E90 RID: 3728 RVA: 0x00127ECC File Offset: 0x001260CC
		public virtual void receiveKeyPress(Keys key)
		{
			if (key == Keys.None)
			{
				return;
			}
			if (Game1.options.doesInputListContain(Game1.options.actionButton, key))
			{
				this.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
				return;
			}
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
			{
				this.exitThisMenu(true);
			}
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void receiveScrollWheelAction(int direction)
		{
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00127F2C File Offset: 0x0012612C
		public virtual void performHoverAction(int x, int y)
		{
			if (this.upperRightCloseButton != null)
			{
				this.upperRightCloseButton.tryHover(x, y, 0.5f);
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00127F48 File Offset: 0x00126148
		public virtual void draw(SpriteBatch b)
		{
			if (this.upperRightCloseButton != null)
			{
				this.upperRightCloseButton.draw(b);
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00127F5E File Offset: 0x0012615E
		public virtual bool isWithinBounds(int x, int y)
		{
			return x - this.xPositionOnScreen < this.width && x - this.xPositionOnScreen >= 0 && y - this.yPositionOnScreen < this.height && y - this.yPositionOnScreen >= 0;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void update(GameTime time)
		{
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00127F9B File Offset: 0x0012619B
		public void exitThisMenuNoSound()
		{
			Game1.exitActiveMenu();
			if (this.exitFunction != null)
			{
				this.exitFunction();
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00127FB5 File Offset: 0x001261B5
		public void exitThisMenu(bool playSound = true)
		{
			if (playSound)
			{
				Game1.playSound("bigDeSelect");
			}
			Game1.exitActiveMenu();
			if (this.exitFunction != null)
			{
				this.exitFunction();
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool autoCenterMouseCursorForGamepad()
		{
			return true;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void emergencyShutDown()
		{
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool readyToClose()
		{
			return true;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00127FDC File Offset: 0x001261DC
		protected void drawHorizontalPartition(SpriteBatch b, int yPosition, bool small = false)
		{
			if (small)
			{
				b.Draw(Game1.menuTexture, new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, yPosition, this.width - Game1.tileSize, Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 25, -1, -1)), Color.White);
				return;
			}
			b.Draw(Game1.menuTexture, new Vector2((float)this.xPositionOnScreen, (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 4, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(this.xPositionOnScreen + Game1.tileSize, yPosition, this.width - Game1.tileSize * 2, Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 6, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)(this.xPositionOnScreen + this.width - Game1.tileSize), (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 7, -1, -1)), Color.White);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x001280EC File Offset: 0x001262EC
		protected void drawVerticalPartition(SpriteBatch b, int xPosition, bool small = false)
		{
			if (small)
			{
				b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize + Game1.tileSize / 2, Game1.tileSize, this.height - Game1.tileSize * 2), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 26, -1, -1)), Color.White);
				return;
			}
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 1, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, this.height - Game1.tileSize * 3), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 5, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + this.height - Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 13, -1, -1)), Color.White);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0012820C File Offset: 0x0012640C
		protected void drawVerticalIntersectingPartition(SpriteBatch b, int xPosition, int yPosition)
		{
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 59, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(xPosition, yPosition + Game1.tileSize, Game1.tileSize, this.yPositionOnScreen + this.height - Game1.tileSize - yPosition - Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 63, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + this.height - Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 62, -1, -1)), Color.White);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x001282D4 File Offset: 0x001264D4
		protected void drawVerticalUpperIntersectingPartition(SpriteBatch b, int xPosition, int partitionHeight)
		{
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 44, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, partitionHeight - Game1.tileSize / 2), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 63, -1, -1)), Color.White);
			b.Draw(Game1.menuTexture, new Vector2((float)xPosition, (float)(this.yPositionOnScreen + partitionHeight + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 39, -1, -1)), Color.White);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00128398 File Offset: 0x00126598
		public static void drawTextureBox(SpriteBatch b, int x, int y, int width, int height, Color color)
		{
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, color, 1f, true);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x001283CC File Offset: 0x001265CC
		public static void drawTextureBox(SpriteBatch b, Texture2D texture, Rectangle sourceRect, int x, int y, int width, int height, Color color, float scale = 1f, bool drawShadow = true)
		{
			int cornerSize = sourceRect.Width / 3;
			if (drawShadow)
			{
				b.Draw(texture, new Vector2((float)(x + width - (int)((float)cornerSize * scale) - Game1.pixelZoom * 2), (float)(y + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X + cornerSize * 2, sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Vector2((float)(x - Game1.pixelZoom * 2), (float)(y + height - (int)((float)cornerSize * scale) + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X, cornerSize * 2 + sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Vector2((float)(x + width - (int)((float)cornerSize * scale) - Game1.pixelZoom * 2), (float)(y + height - (int)((float)cornerSize * scale) + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X + cornerSize * 2, cornerSize * 2 + sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x + (int)((float)cornerSize * scale) - Game1.pixelZoom * 2, y + Game1.pixelZoom * 2, width - (int)((float)cornerSize * scale) * 2, (int)((float)cornerSize * scale)), new Rectangle?(new Rectangle(sourceRect.X + cornerSize, sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x + (int)((float)cornerSize * scale) - Game1.pixelZoom * 2, y + height - (int)((float)cornerSize * scale) + Game1.pixelZoom * 2, width - (int)((float)cornerSize * scale) * 2, (int)((float)cornerSize * scale)), new Rectangle?(new Rectangle(sourceRect.X + cornerSize, cornerSize * 2 + sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x - Game1.pixelZoom * 2, y + (int)((float)cornerSize * scale) + Game1.pixelZoom * 2, (int)((float)cornerSize * scale), height - (int)((float)cornerSize * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X, cornerSize + sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle(x + width - (int)((float)cornerSize * scale) - Game1.pixelZoom * 2, y + (int)((float)cornerSize * scale) + Game1.pixelZoom * 2, (int)((float)cornerSize * scale), height - (int)((float)cornerSize * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X + cornerSize * 2, cornerSize + sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
				b.Draw(texture, new Rectangle((int)((float)cornerSize * scale / 2f) + x - Game1.pixelZoom * 2, (int)((float)cornerSize * scale / 2f) + y + Game1.pixelZoom * 2, width - (int)((float)cornerSize * scale), height - (int)((float)cornerSize * scale)), new Rectangle?(new Rectangle(cornerSize + sourceRect.X, cornerSize + sourceRect.Y, cornerSize, cornerSize)), Color.Black * 0.4f, 0f, Vector2.Zero, SpriteEffects.None, 0.77f);
			}
			b.Draw(texture, new Rectangle((int)((float)cornerSize * scale) + x, (int)((float)cornerSize * scale) + y, width - (int)((float)cornerSize * scale * 2f), height - (int)((float)cornerSize * scale * 2f)), new Rectangle?(new Rectangle(cornerSize + sourceRect.X, cornerSize + sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(sourceRect.X, sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)(x + width - (int)((float)cornerSize * scale)), (float)y), new Rectangle?(new Rectangle(sourceRect.X + cornerSize * 2, sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)x, (float)(y + height - (int)((float)cornerSize * scale))), new Rectangle?(new Rectangle(sourceRect.X, cornerSize * 2 + sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Vector2((float)(x + width - (int)((float)cornerSize * scale)), (float)(y + height - (int)((float)cornerSize * scale))), new Rectangle?(new Rectangle(sourceRect.X + cornerSize * 2, cornerSize * 2 + sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x + (int)((float)cornerSize * scale), y, width - (int)((float)cornerSize * scale) * 2, (int)((float)cornerSize * scale)), new Rectangle?(new Rectangle(sourceRect.X + cornerSize, sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x + (int)((float)cornerSize * scale), y + height - (int)((float)cornerSize * scale), width - (int)((float)cornerSize * scale) * 2, (int)((float)cornerSize * scale)), new Rectangle?(new Rectangle(sourceRect.X + cornerSize, cornerSize * 2 + sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x, y + (int)((float)cornerSize * scale), (int)((float)cornerSize * scale), height - (int)((float)cornerSize * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X, cornerSize + sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
			b.Draw(texture, new Rectangle(x + width - (int)((float)cornerSize * scale), y + (int)((float)cornerSize * scale), (int)((float)cornerSize * scale), height - (int)((float)cornerSize * scale) * 2), new Rectangle?(new Rectangle(sourceRect.X + cornerSize * 2, cornerSize + sourceRect.Y, cornerSize, cornerSize)), color, 0f, Vector2.Zero, SpriteEffects.None, 0.8f - (float)y * 1E-06f);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00128AE8 File Offset: 0x00126CE8
		public void drawBorderLabel(SpriteBatch b, string text, SpriteFont font, int x, int y)
		{
			int width = (int)font.MeasureString(text).X;
			y += Game1.tileSize - Game1.pixelZoom * 3;
			b.Draw(Game1.mouseCursors, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(256, 267, 6, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(x + 6 * Game1.pixelZoom), (float)y), new Rectangle?(new Rectangle(262, 267, 1, 16)), Color.White, 0f, Vector2.Zero, new Vector2((float)width, (float)Game1.pixelZoom), SpriteEffects.None, 0.87f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(x + 6 * Game1.pixelZoom + width), (float)y), new Rectangle?(new Rectangle(263, 267, 6, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
			Utility.drawTextWithShadow(b, text, font, new Vector2((float)(x + 6 * Game1.pixelZoom), (float)(y + Game1.pixelZoom * 5)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00128C40 File Offset: 0x00126E40
		public static void drawToolTip(SpriteBatch b, string hoverText, string hoverTitle, Item hoveredItem, bool heldItem = false, int healAmountToDisplay = -1, int currencySymbol = 0, int extraItemToShowIndex = -1, int extraItemToShowAmount = -1, CraftingRecipe craftingIngredients = null, int moneyAmountToShowAtBottom = -1)
		{
			bool edibleItem = hoveredItem != null && hoveredItem is Object && (hoveredItem as Object).edibility != -300;
			IClickableMenu.drawHoverText(b, hoverText, Game1.smallFont, heldItem ? (Game1.tileSize / 2 + 8) : 0, heldItem ? (Game1.tileSize / 2 + 8) : 0, moneyAmountToShowAtBottom, hoverTitle, edibleItem ? (hoveredItem as Object).edibility : -1, (edibleItem && Game1.objectInformation[(hoveredItem as Object).parentSheetIndex].Split(new char[]
			{
				'/'
			}).Length >= 7) ? Game1.objectInformation[(hoveredItem as Object).parentSheetIndex].Split(new char[]
			{
				'/'
			})[6].Split(new char[]
			{
				' '
			}) : null, hoveredItem, currencySymbol, extraItemToShowIndex, extraItemToShowAmount, -1, -1, 1f, craftingIngredients);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00128D28 File Offset: 0x00126F28
		public static void drawHoverText(SpriteBatch b, string text, SpriteFont font, int xOffset = 0, int yOffset = 0, int moneyAmountToDisplayAtBottom = -1, string boldTitleText = null, int healAmountToDisplay = -1, string[] buffIconsToDisplay = null, Item hoveredItem = null, int currencySymbol = 0, int extraItemToShowIndex = -1, int extraItemToShowAmount = -1, int overrideX = -1, int overrideY = -1, float alpha = 1f, CraftingRecipe craftingIngredients = null)
		{
			if (text == null || text.Length == 0)
			{
				return;
			}
			if (boldTitleText != null && boldTitleText.Length == 0)
			{
				boldTitleText = null;
			}
			int cornerSize = 20;
			int width = Math.Max((healAmountToDisplay != -1) ? ((int)font.MeasureString(healAmountToDisplay + "+ Energy" + Game1.tileSize / 2).X) : 0, Math.Max((int)font.MeasureString(text).X, (boldTitleText != null) ? ((int)Game1.dialogueFont.MeasureString(boldTitleText).X) : 0)) + Game1.tileSize / 2;
			int height = Math.Max(cornerSize * 3, (int)font.MeasureString(text).Y + Game1.tileSize / 2 + (int)((moneyAmountToDisplayAtBottom > -1) ? (font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).Y + 4f) : 0f) + (int)((boldTitleText != null) ? (Game1.dialogueFont.MeasureString(boldTitleText).Y + (float)(Game1.tileSize / 4)) : 0f) + ((healAmountToDisplay != -1) ? 38 : 0));
			if (buffIconsToDisplay != null)
			{
				for (int j = 0; j < buffIconsToDisplay.Length; j++)
				{
					if (!buffIconsToDisplay[j].Equals("0"))
					{
						height += 34;
					}
				}
				height += 4;
			}
			string categoryName = null;
			if (hoveredItem != null)
			{
				height += (Game1.tileSize + 4) * hoveredItem.attachmentSlots();
				categoryName = hoveredItem.getCategoryName();
				if (categoryName.Length > 0)
				{
					height += (int)font.MeasureString("T").Y;
				}
				if (hoveredItem is MeleeWeapon)
				{
					height = Math.Max(cornerSize * 3, (int)((boldTitleText != null) ? (Game1.dialogueFont.MeasureString(boldTitleText).Y + (float)(Game1.tileSize / 4)) : 0f) + Game1.tileSize / 2) + (int)font.MeasureString("T").Y + (int)((moneyAmountToDisplayAtBottom > -1) ? (font.MeasureString(string.Concat(moneyAmountToDisplayAtBottom)).Y + 4f) : 0f);
					height += (int)((float)((hoveredItem as MeleeWeapon).getNumberOfDescriptionCategories() * Game1.pixelZoom * 12) + font.MeasureString(Game1.parseText((hoveredItem as MeleeWeapon).description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y);
					width = (int)Math.Max((float)width, font.MeasureString("99-99 Damage").X + (float)(15 * Game1.pixelZoom) + (float)(Game1.tileSize / 2));
				}
				else if (hoveredItem is Boots)
				{
					height -= (int)font.MeasureString(text).Y;
					height += (int)((float)((hoveredItem as Boots).getNumberOfDescriptionCategories() * Game1.pixelZoom * 12) + font.MeasureString(Game1.parseText((hoveredItem as Boots).description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y);
					width = (int)Math.Max((float)width, font.MeasureString("99-99 Damage").X + (float)(15 * Game1.pixelZoom) + (float)(Game1.tileSize / 2));
				}
				else if (hoveredItem is Object && (hoveredItem as Object).edibility != -300)
				{
					if (healAmountToDisplay == -1)
					{
						height += (Game1.tileSize / 2 + Game1.pixelZoom * 2) * ((healAmountToDisplay > 0) ? 2 : 1);
					}
					else
					{
						height += Game1.tileSize / 2 + Game1.pixelZoom * 2;
					}
					healAmountToDisplay = (int)Math.Ceiling((double)(hoveredItem as Object).Edibility * 2.5) + (hoveredItem as Object).quality * (hoveredItem as Object).Edibility;
				}
			}
			if (craftingIngredients != null)
			{
				width = Math.Max((int)Game1.dialogueFont.MeasureString(boldTitleText).X + Game1.pixelZoom * 3, Game1.tileSize * 6);
				height += craftingIngredients.getDescriptionHeight(width - Game1.pixelZoom * 2) + ((healAmountToDisplay == -1) ? (-Game1.tileSize / 2) : 0);
			}
			int x = Game1.getOldMouseX() + Game1.tileSize / 2 + xOffset;
			int y = Game1.getOldMouseY() + Game1.tileSize / 2 + yOffset;
			if (overrideX != -1)
			{
				x = overrideX;
			}
			if (overrideY != -1)
			{
				y = overrideY;
			}
			if (x + width > Game1.viewport.Width)
			{
				x = Game1.viewport.Width - width;
				y += Game1.tileSize / 4;
			}
			if (y + height > Game1.viewport.Height)
			{
				x += Game1.tileSize / 4;
				y = Game1.viewport.Height - height;
			}
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width + ((craftingIngredients != null) ? (Game1.tileSize / 3) : 0), height, Color.White * alpha, 1f, true);
			if (boldTitleText != null)
			{
				IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width + ((craftingIngredients != null) ? (Game1.tileSize / 3) : 0), (int)Game1.dialogueFont.MeasureString(boldTitleText).Y + Game1.tileSize / 2 + (int)((hoveredItem != null && categoryName.Length > 0) ? font.MeasureString("asd").Y : 0f) - Game1.pixelZoom, Color.White * alpha, 1f, false);
				b.Draw(Game1.menuTexture, new Rectangle(x + Game1.pixelZoom * 3, y + (int)Game1.dialogueFont.MeasureString(boldTitleText).Y + Game1.tileSize / 2 + (int)((hoveredItem != null && categoryName.Length > 0) ? font.MeasureString("asd").Y : 0f) - Game1.pixelZoom, width - Game1.pixelZoom * ((craftingIngredients == null) ? 6 : 1), Game1.pixelZoom), new Rectangle?(new Rectangle(44, 300, 4, 4)), Color.White);
				b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor);
				b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor);
				b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), Game1.textColor);
				y += (int)Game1.dialogueFont.MeasureString(boldTitleText).Y;
			}
			if (hoveredItem != null && categoryName.Length > 0)
			{
				y -= 4;
				Utility.drawTextWithShadow(b, categoryName, font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), hoveredItem.getCategoryColor(), 1f, -1f, 2, 2, 1f, 3);
				y += (int)font.MeasureString("T").Y + ((boldTitleText != null) ? (Game1.tileSize / 4) : 0) + Game1.pixelZoom;
			}
			else
			{
				y += ((boldTitleText != null) ? (Game1.tileSize / 4) : 0);
			}
			if (hoveredItem != null && hoveredItem is Boots)
			{
				Boots boots = hoveredItem as Boots;
				Utility.drawTextWithShadow(b, Game1.parseText(boots.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4), font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				y += (int)font.MeasureString(Game1.parseText(boots.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
				if (boots.defenseBonus > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(110, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", new object[]
					{
						boots.defenseBonus
					}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
					y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
				}
				if (boots.immunityBonus > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(150, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_ImmunityBonus", new object[]
					{
						boots.immunityBonus
					}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
					y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
				}
			}
			else if (hoveredItem != null && hoveredItem is MeleeWeapon)
			{
				MeleeWeapon weap = hoveredItem as MeleeWeapon;
				Utility.drawTextWithShadow(b, Game1.parseText(weap.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4), font, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				y += (int)font.MeasureString(Game1.parseText(weap.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
				if (weap.indexOfMenuItemView != 47)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(120, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Damage", new object[]
					{
						weap.minDamage,
						weap.maxDamage
					}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
					y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					if (weap.speed != ((weap.type == 2) ? -8 : 0))
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(130, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						bool negativeSpeed = (weap.type == 2 && weap.speed < -8) || (weap.type != 2 && weap.speed < 0);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Speed", new object[]
						{
							((weap.speed > 0) ? "+" : "") + ((weap.type == 2) ? (weap.speed - -8) : weap.speed) / 2
						}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), negativeSpeed ? Color.DarkRed : (Game1.textColor * 0.9f * alpha), 1f, -1f, -1, -1, 1f, 3);
						y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if (weap.addedDefense > 0)
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(110, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", new object[]
						{
							weap.addedDefense
						}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if ((double)weap.critChance / 0.02 >= 2.0)
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(40, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_CritChanceBonus", new object[]
						{
							(double)((int)weap.critChance) / 0.02
						}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if ((double)(weap.critMultiplier - 3f) / 0.02 >= 1.0)
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(160, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_CritChanceBonus", new object[]
						{
							(int)((double)(weap.critMultiplier - 3f) / 0.02)
						}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 11), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
					if (weap.knockback != weap.defaultKnockBackForThisType(weap.type))
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 4)), new Rectangle(70, 428, 10, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
						Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Weight", new object[]
						{
							((weap.knockback > weap.defaultKnockBackForThisType(weap.type)) ? "+" : "") + (int)Math.Ceiling((double)(Math.Abs(weap.knockback - weap.defaultKnockBackForThisType(weap.type)) * 10f))
						}), font, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float)(y + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
						y += (int)Math.Max(font.MeasureString("TT").Y, (float)(12 * Game1.pixelZoom));
					}
				}
			}
			else if (text.Length > 1)
			{
				b.DrawString(font, text, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor * alpha);
				b.DrawString(font, text, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor * alpha);
				b.DrawString(font, text, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0f), Game1.textShadowColor * alpha);
				b.DrawString(font, text, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), Game1.textColor * 0.9f * alpha);
				y += (int)font.MeasureString(text).Y + 4;
			}
			if (craftingIngredients != null)
			{
				craftingIngredients.drawRecipeDescription(b, new Vector2((float)(x + Game1.tileSize / 4), (float)(y - Game1.pixelZoom * 2)), width);
				y += craftingIngredients.getDescriptionHeight(width);
			}
			if (healAmountToDisplay != -1)
			{
				Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4)), new Rectangle((healAmountToDisplay < 0) ? 140 : 0, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
				Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", new object[]
				{
					((healAmountToDisplay > 0) ? "+" : "") + healAmountToDisplay
				}), font, new Vector2((float)(x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				y += 34;
				if (healAmountToDisplay > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4)), new Rectangle(0, 438, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Health", new object[]
					{
						((healAmountToDisplay > 0) ? "+" : "") + (int)((float)healAmountToDisplay * 0.4f)
					}), font, new Vector2((float)(x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
					y += 34;
				}
			}
			if (buffIconsToDisplay != null)
			{
				for (int i = 0; i < buffIconsToDisplay.Length; i++)
				{
					if (!buffIconsToDisplay[i].Equals("0"))
					{
						Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 4 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4)), new Rectangle(10 + i * 10, 428, 10, 10), Color.White, 0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
						string buffName = ((Convert.ToInt32(buffIconsToDisplay[i]) > 0) ? "+" : "") + buffIconsToDisplay[i] + " ";
						if (i <= 10)
						{
							buffName = Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + i, new object[]
							{
								buffName
							});
						}
						Utility.drawTextWithShadow(b, buffName, font, new Vector2((float)(x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float)(y + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
						y += 34;
					}
				}
			}
			if (hoveredItem != null && hoveredItem.attachmentSlots() > 0)
			{
				y += 16;
				hoveredItem.drawAttachments(b, x + Game1.tileSize / 4, y);
				if (moneyAmountToDisplayAtBottom > -1)
				{
					y += Game1.tileSize * hoveredItem.attachmentSlots();
				}
			}
			if (moneyAmountToDisplayAtBottom > -1)
			{
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor);
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(0f, 2f), Game1.textShadowColor);
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0f), Game1.textShadowColor);
				b.DrawString(font, string.Concat(moneyAmountToDisplayAtBottom), new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.tileSize / 4 + 4)), Game1.textColor);
				if (currencySymbol == 0)
				{
					b.Draw(Game1.debrisSpriteSheet, new Vector2((float)(x + Game1.tileSize / 4) + font.MeasureString(moneyAmountToDisplayAtBottom + "  ").X, (float)(y + Game1.tileSize / 4 + 16)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.95f);
				}
				else if (currencySymbol == 1)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 8) + font.MeasureString(moneyAmountToDisplayAtBottom + "  ").X, (float)(y + Game1.tileSize / 4)), new Rectangle?(new Rectangle(338, 400, 8, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				else if (currencySymbol == 2)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(x + Game1.tileSize / 8) + font.MeasureString(moneyAmountToDisplayAtBottom + "  ").X, (float)(y + Game1.tileSize / 4)), new Rectangle?(new Rectangle(211, 373, 9, 10)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				y += Game1.tileSize * 3 / 4;
			}
			if (extraItemToShowIndex != -1)
			{
				IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y + Game1.pixelZoom, width, Game1.tileSize * 3 / 2, Color.White, 1f, true);
				y += Game1.pixelZoom * 5;
				string requirement = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", new object[]
				{
					extraItemToShowAmount,
					Game1.objectInformation[extraItemToShowIndex].Split(new char[]
					{
						'/'
					})[0]
				});
				b.DrawString(font, requirement, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.pixelZoom)) + new Vector2(2f, 2f), Game1.textShadowColor);
				b.DrawString(font, requirement, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.pixelZoom)) + new Vector2(0f, 2f), Game1.textShadowColor);
				b.DrawString(font, requirement, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.pixelZoom)) + new Vector2(2f, 0f), Game1.textShadowColor);
				b.DrawString(Game1.smallFont, requirement, new Vector2((float)(x + Game1.tileSize / 4), (float)(y + Game1.pixelZoom)), Game1.textColor);
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(x + Game1.tileSize * 2 + Game1.tileSize * 2 / 3), (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, extraItemToShowIndex, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x04000F89 RID: 3977
		public const int currency_g = 0;

		// Token: 0x04000F8A RID: 3978
		public const int currency_starTokens = 1;

		// Token: 0x04000F8B RID: 3979
		public const int currency_qiCoins = 2;

		// Token: 0x04000F8C RID: 3980
		public const int greyedOutSpotIndex = 57;

		// Token: 0x04000F8D RID: 3981
		public const int outerBorderWithUpArrow = 61;

		// Token: 0x04000F8E RID: 3982
		public const int lvlMarkerRedIndex = 54;

		// Token: 0x04000F8F RID: 3983
		public const int lvlMarkerGreyIndex = 55;

		// Token: 0x04000F90 RID: 3984
		public const int borderWithDownArrowIndex = 46;

		// Token: 0x04000F91 RID: 3985
		public const int borderWithUpArrowIndex = 47;

		// Token: 0x04000F92 RID: 3986
		public const int littleHeartIndex = 49;

		// Token: 0x04000F93 RID: 3987
		public const int uncheckedBoxIndex = 50;

		// Token: 0x04000F94 RID: 3988
		public const int checkedBoxIndex = 51;

		// Token: 0x04000F95 RID: 3989
		public const int presentIconIndex = 58;

		// Token: 0x04000F96 RID: 3990
		public const int itemSpotIndex = 10;

		// Token: 0x04000F97 RID: 3991
		public static int borderWidth = Game1.tileSize / 2 + Game1.tileSize / 8;

		// Token: 0x04000F98 RID: 3992
		public static int tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;

		// Token: 0x04000F99 RID: 3993
		public static int spaceToClearTopBorder = Game1.tileSize * 3 / 2;

		// Token: 0x04000F9A RID: 3994
		public static int spaceToClearSideBorder = Game1.tileSize / 4;

		// Token: 0x04000F9B RID: 3995
		public const int spaceBetweenTabs = 4;

		// Token: 0x04000F9C RID: 3996
		public int width;

		// Token: 0x04000F9D RID: 3997
		public int height;

		// Token: 0x04000F9E RID: 3998
		public int xPositionOnScreen;

		// Token: 0x04000F9F RID: 3999
		public int yPositionOnScreen;

		// Token: 0x04000FA0 RID: 4000
		public int currentRegion;

		// Token: 0x04000FA1 RID: 4001
		public static Texture2D hoverBox = Game1.content.Load<Texture2D>("LooseSprites\\hoverBox");

		// Token: 0x04000FA2 RID: 4002
		public IClickableMenu.onExit exitFunction;

		// Token: 0x04000FA3 RID: 4003
		public ClickableTextureComponent upperRightCloseButton;

		// Token: 0x04000FA4 RID: 4004
		public bool destroy;

		// Token: 0x04000FA5 RID: 4005
		public bool gamePadControlsImplemented;

		// Token: 0x02000182 RID: 386
		// Token: 0x060013CC RID: 5068
		public delegate void onExit();
	}
}
