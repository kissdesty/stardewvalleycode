using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000FF RID: 255
	public class MenuWithInventory : IClickableMenu
	{
		// Token: 0x06000F3C RID: 3900 RVA: 0x0013A334 File Offset: 0x00138534
		public MenuWithInventory(InventoryMenu.highlightThisItem highlighterMethod = null, bool okButton = false, bool trashCan = false, int inventoryXOffset = 0, int inventoryYOffset = 0) : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 + Game1.tileSize, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, false)
		{
			if (this.yPositionOnScreen < IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder)
			{
				this.yPositionOnScreen = IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
			}
			if (this.xPositionOnScreen < 0)
			{
				this.xPositionOnScreen = 0;
			}
			int yPositionForInventory = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4 + inventoryYOffset;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + inventoryXOffset, yPositionForInventory, false, null, highlighterMethod, -1, 3, 0, 0, true);
			if (okButton)
			{
				this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			}
			if (trashCan)
			{
				this.trashCan = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom, false);
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0013A514 File Offset: 0x00138714
		public void movePosition(int dx, int dy)
		{
			this.xPositionOnScreen += dx;
			this.yPositionOnScreen += dy;
			this.inventory.movePosition(dx, dy);
			if (this.okButton != null)
			{
				ClickableTextureComponent expr_41_cp_0_cp_0 = this.okButton;
				expr_41_cp_0_cp_0.bounds.X = expr_41_cp_0_cp_0.bounds.X + dx;
				ClickableTextureComponent expr_56_cp_0_cp_0 = this.okButton;
				expr_56_cp_0_cp_0.bounds.Y = expr_56_cp_0_cp_0.bounds.Y + dy;
			}
			if (this.trashCan != null)
			{
				ClickableTextureComponent expr_73_cp_0_cp_0 = this.trashCan;
				expr_73_cp_0_cp_0.bounds.X = expr_73_cp_0_cp_0.bounds.X + dx;
				ClickableTextureComponent expr_88_cp_0_cp_0 = this.trashCan;
				expr_88_cp_0_cp_0.bounds.Y = expr_88_cp_0_cp_0.bounds.Y + dy;
			}
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0013A5AE File Offset: 0x001387AE
		public override bool readyToClose()
		{
			return this.heldItem == null;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0013A5B9 File Offset: 0x001387B9
		public override bool isWithinBounds(int x, int y)
		{
			return base.isWithinBounds(x, y);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0013A5C4 File Offset: 0x001387C4
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			this.heldItem = this.inventory.leftClick(x, y, this.heldItem, playSound);
			if (!this.isWithinBounds(x, y) && this.readyToClose() && this.trashCan != null)
			{
				this.trashCan.containsPoint(x, y);
			}
			if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
			{
				base.exitThisMenu(true);
				if (Game1.currentLocation.currentEvent != null)
				{
					Event expr_7E = Game1.currentLocation.currentEvent;
					int currentCommand = expr_7E.CurrentCommand;
					expr_7E.CurrentCommand = currentCommand + 1;
				}
				Game1.playSound("bigDeSelect");
			}
			if (this.trashCan != null && this.trashCan.containsPoint(x, y) && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is Object && Game1.player.specialItems.Contains((this.heldItem as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0013A6F6 File Offset: 0x001388F6
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.heldItem = this.inventory.rightClick(x, y, this.heldItem, playSound);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0013A714 File Offset: 0x00138914
		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.descriptionTitle = "";
			this.hoveredItem = this.inventory.hover(x, y, this.heldItem);
			this.hoverText = this.inventory.hoverText;
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
			if (this.trashCan != null)
			{
				if (this.trashCan.containsPoint(x, y))
				{
					if (this.trashCanLidRotation <= 0f)
					{
						Game1.playSound("trashcanlid");
					}
					this.trashCanLidRotation = Math.Min(this.trashCanLidRotation + 0.06544985f, 1.57079637f);
					return;
				}
				this.trashCanLidRotation = Math.Max(this.trashCanLidRotation - 0.06544985f, 0f);
			}
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0013A830 File Offset: 0x00138A30
		public override void update(GameTime time)
		{
			if (this.wiggleWordsTimer > 0)
			{
				this.wiggleWordsTimer -= time.ElapsedGameTime.Milliseconds;
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0013A864 File Offset: 0x00138A64
		public virtual void draw(SpriteBatch b, bool drawUpperPortion = true, bool drawDescriptionArea = true)
		{
			if (this.trashCan != null)
			{
				this.trashCan.draw(b);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.trashCan.bounds.X + 60), (float)(this.trashCan.bounds.Y + 40)), new Rectangle?(new Rectangle(686, 256, 18, 10)), Color.White, this.trashCanLidRotation, new Vector2(16f, 10f), (float)Game1.pixelZoom, SpriteEffects.None, 0.86f);
			}
			if (drawUpperPortion)
			{
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
				base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
				if (drawDescriptionArea)
				{
					base.drawVerticalUpperIntersectingPartition(b, this.xPositionOnScreen + Game1.tileSize * 9, 5 * Game1.tileSize + Game1.tileSize / 8);
					if (!this.descriptionText.Equals(""))
					{
						int xPosition = this.xPositionOnScreen + Game1.tileSize * 9 + Game1.tileSize * 2 / 3 + ((this.wiggleWordsTimer > 0) ? Game1.random.Next(-2, 3) : 0);
						int yPosition = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 2 + ((this.wiggleWordsTimer > 0) ? Game1.random.Next(-2, 3) : 0);
						b.DrawString(Game1.smallFont, Game1.parseText(this.descriptionText, Game1.smallFont, Game1.tileSize * 3 + Game1.tileSize / 2), new Vector2((float)xPosition, (float)yPosition), Game1.textColor * 0.75f);
					}
				}
			}
			else
			{
				Game1.drawDialogueBox(this.xPositionOnScreen - IClickableMenu.borderWidth / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize, this.width, this.height - (IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize), false, true, null, false);
			}
			if (this.okButton != null)
			{
				this.okButton.draw(b);
			}
			this.inventory.draw(b);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0013AAA4 File Offset: 0x00138CA4
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			if (this.yPositionOnScreen < IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder)
			{
				this.yPositionOnScreen = IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
			}
			if (this.xPositionOnScreen < 0)
			{
				this.xPositionOnScreen = 0;
			}
			int yPositionForInventory = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2, yPositionForInventory, false, null, this.inventory.highlightMethod, -1, 3, 0, 0, true);
			if (this.okButton != null)
			{
				this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			}
			if (this.trashCan != null)
			{
				this.trashCan = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom, false);
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000A8788 File Offset: 0x000A6988
		public override void draw(SpriteBatch b)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001066 RID: 4198
		public string descriptionText = "";

		// Token: 0x04001067 RID: 4199
		public string hoverText = "";

		// Token: 0x04001068 RID: 4200
		public string descriptionTitle = "";

		// Token: 0x04001069 RID: 4201
		public InventoryMenu inventory;

		// Token: 0x0400106A RID: 4202
		public Item heldItem;

		// Token: 0x0400106B RID: 4203
		public Item hoveredItem;

		// Token: 0x0400106C RID: 4204
		public int wiggleWordsTimer;

		// Token: 0x0400106D RID: 4205
		public ClickableTextureComponent okButton;

		// Token: 0x0400106E RID: 4206
		public ClickableTextureComponent trashCan;

		// Token: 0x0400106F RID: 4207
		public float trashCanLidRotation;
	}
}
