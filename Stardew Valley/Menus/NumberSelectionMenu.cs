using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x02000104 RID: 260
	public class NumberSelectionMenu : IClickableMenu
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x0013C068 File Offset: 0x0013A268
		public NumberSelectionMenu(string message, NumberSelectionMenu.behaviorOnNumberSelect behaviorOnSelection, int price = -1, int minValue = 0, int maxValue = 99, int defaultNumber = 0) : base(Game1.viewport.Width / 2 - (int)Game1.dialogueFont.MeasureString(message).X / 2 - IClickableMenu.borderWidth, Game1.viewport.Height / 2 - (int)Game1.dialogueFont.MeasureString(message).Y / 2, (int)Game1.dialogueFont.MeasureString(message).X + IClickableMenu.borderWidth * 2, (int)Game1.dialogueFont.MeasureString(message).Y + IClickableMenu.borderWidth * 2 + Game1.tileSize * 5 / 2, false)
		{
			this.message = message;
			this.price = price;
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.currentValue = defaultNumber;
			this.behaviorFunction = behaviorOnSelection;
			this.numberSelectedBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + IClickableMenu.borderWidth + 14 * Game1.pixelZoom,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2,
				Text = string.Concat(this.currentValue),
				numbersOnly = true,
				textLimit = string.Concat(maxValue).Length
			};
			this.numberSelectedBox.SelectMe();
			this.leftButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.rightButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + 16 * Game1.pixelZoom + this.numberSelectedBox.Width, this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0013C390 File Offset: 0x0013A590
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.leftButton.containsPoint(x, y))
			{
				int tempNumber = this.currentValue - 1;
				if (tempNumber >= this.minValue)
				{
					this.leftButton.scale = this.leftButton.baseScale;
					this.currentValue = tempNumber;
					this.numberSelectedBox.Text = string.Concat(this.currentValue);
					Game1.playSound("smallSelect");
				}
			}
			if (this.rightButton.containsPoint(x, y))
			{
				int tempNumber2 = this.currentValue + 1;
				if (tempNumber2 <= this.maxValue && (this.price == -1 || tempNumber2 * this.price <= Game1.player.Money))
				{
					this.rightButton.scale = this.rightButton.baseScale;
					this.currentValue = tempNumber2;
					this.numberSelectedBox.Text = string.Concat(this.currentValue);
					Game1.playSound("smallSelect");
				}
			}
			if (this.okButton.containsPoint(x, y))
			{
				if (this.currentValue > this.maxValue || this.currentValue < this.minValue)
				{
					this.currentValue = Math.Max(this.minValue, Math.Min(this.maxValue, this.currentValue));
					this.numberSelectedBox.Text = string.Concat(this.currentValue);
				}
				else
				{
					this.behaviorFunction(this.currentValue, this.price, Game1.player);
				}
				Game1.playSound("smallSelect");
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
				Game1.player.canMove = true;
			}
			this.numberSelectedBox.Update();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0013C547 File Offset: 0x0013A747
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (key == Keys.Enter)
			{
				this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
			}
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0013C588 File Offset: 0x0013A788
		public override void update(GameTime time)
		{
			base.update(time);
			this.currentValue = ((this.numberSelectedBox.Text != null && this.numberSelectedBox.Text.Length > 0) ? Convert.ToInt32(this.numberSelectedBox.Text) : 0);
			if (this.priceShake > 0)
			{
				this.priceShake -= time.ElapsedGameTime.Milliseconds;
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0013C5FC File Offset: 0x0013A7FC
		public override void performHoverAction(int x, int y)
		{
			if (this.okButton.containsPoint(x, y) && (this.price == -1 || this.currentValue > this.minValue))
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.2f);
			}
			else
			{
				this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				this.cancelButton.scale = Math.Min(this.cancelButton.scale + 0.02f, this.cancelButton.baseScale + 0.2f);
			}
			else
			{
				this.cancelButton.scale = Math.Max(this.cancelButton.scale - 0.02f, this.cancelButton.baseScale);
			}
			if (this.leftButton.containsPoint(x, y))
			{
				this.leftButton.scale = Math.Min(this.leftButton.scale + 0.02f, this.leftButton.baseScale + 0.2f);
			}
			else
			{
				this.leftButton.scale = Math.Max(this.leftButton.scale - 0.02f, this.leftButton.baseScale);
			}
			if (this.rightButton.containsPoint(x, y))
			{
				this.rightButton.scale = Math.Min(this.rightButton.scale + 0.02f, this.rightButton.baseScale + 0.2f);
				return;
			}
			this.rightButton.scale = Math.Max(this.rightButton.scale - 0.02f, this.rightButton.baseScale);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0013C7DC File Offset: 0x0013A9DC
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			b.DrawString(Game1.dialogueFont, this.message, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2)), Game1.textColor);
			this.okButton.draw(b);
			this.cancelButton.draw(b);
			this.leftButton.draw(b);
			this.rightButton.draw(b);
			if (this.price != -1)
			{
				b.DrawString(Game1.dialogueFont, this.price * this.currentValue + "g", new Vector2((float)(this.rightButton.bounds.Right + Game1.tileSize / 2 + ((this.priceShake > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(this.rightButton.bounds.Y + ((this.priceShake > 0) ? Game1.random.Next(-1, 2) : 0))), (this.currentValue * this.price > Game1.player.Money) ? Color.Red : Game1.textColor);
			}
			this.numberSelectedBox.Draw(b);
			base.drawMouse(b);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04001088 RID: 4232
		private string message;

		// Token: 0x04001089 RID: 4233
		private int price;

		// Token: 0x0400108A RID: 4234
		private int minValue;

		// Token: 0x0400108B RID: 4235
		private int maxValue;

		// Token: 0x0400108C RID: 4236
		private int currentValue;

		// Token: 0x0400108D RID: 4237
		private int priceShake;

		// Token: 0x0400108E RID: 4238
		private NumberSelectionMenu.behaviorOnNumberSelect behaviorFunction;

		// Token: 0x0400108F RID: 4239
		private TextBox numberSelectedBox;

		// Token: 0x04001090 RID: 4240
		private ClickableTextureComponent leftButton;

		// Token: 0x04001091 RID: 4241
		private ClickableTextureComponent rightButton;

		// Token: 0x04001092 RID: 4242
		private ClickableTextureComponent okButton;

		// Token: 0x04001093 RID: 4243
		private ClickableTextureComponent cancelButton;

		// Token: 0x02000186 RID: 390
		// Token: 0x060013DC RID: 5084
		public delegate void behaviorOnNumberSelect(int number, int price, Farmer who);
	}
}
