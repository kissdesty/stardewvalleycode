using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x020000E6 RID: 230
	public class ConfirmationDialog : IClickableMenu
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x0011E24C File Offset: 0x0011C44C
		public ConfirmationDialog(string message, ConfirmationDialog.behavior onConfirm, ConfirmationDialog.behavior onCancel = null) : base(Game1.viewport.Width / 2 - (int)Game1.dialogueFont.MeasureString(message).X / 2 - IClickableMenu.borderWidth, Game1.viewport.Height / 2 - (int)Game1.dialogueFont.MeasureString(message).Y / 2, (int)Game1.dialogueFont.MeasureString(message).X + IClickableMenu.borderWidth * 2, (int)Game1.dialogueFont.MeasureString(message).Y + IClickableMenu.borderWidth * 2 + Game1.tileSize * 5 / 2, false)
		{
			if (onCancel == null)
			{
				onCancel = new ConfirmationDialog.behavior(this.closeDialog);
			}
			else
			{
				this.onCancel = onCancel;
			}
			this.onConfirm = onConfirm;
			this.message = message;
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0011E40D File Offset: 0x0011C60D
		public void closeDialog(Farmer who)
		{
			Game1.exitActiveMenu();
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0011E414 File Offset: 0x0011C614
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.active)
			{
				if (this.okButton.containsPoint(x, y))
				{
					if (this.onConfirm != null)
					{
						this.onConfirm(Game1.player);
					}
					Game1.playSound("smallSelect");
					this.active = false;
				}
				if (this.cancelButton.containsPoint(x, y))
				{
					if (this.onCancel != null)
					{
						this.onCancel(Game1.player);
					}
					else
					{
						Game1.exitActiveMenu();
					}
					Game1.playSound("bigDeSelect");
				}
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0011E499 File Offset: 0x0011C699
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (this.active && Game1.activeClickableMenu == null && this.onCancel != null)
			{
				this.onCancel(Game1.player);
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0011E4C9 File Offset: 0x0011C6C9
		public override void update(GameTime time)
		{
			base.update(time);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0011E4D4 File Offset: 0x0011C6D4
		public override void performHoverAction(int x, int y)
		{
			if (this.okButton.containsPoint(x, y))
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
				return;
			}
			this.cancelButton.scale = Math.Max(this.cancelButton.scale - 0.02f, this.cancelButton.baseScale);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0011E5C0 File Offset: 0x0011C7C0
		public override void draw(SpriteBatch b)
		{
			if (this.active)
			{
				b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
				b.DrawString(Game1.dialogueFont, this.message, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2)), Game1.textColor);
				this.okButton.draw(b);
				this.cancelButton.draw(b);
				base.drawMouse(b);
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04000EEB RID: 3819
		private string message;

		// Token: 0x04000EEC RID: 3820
		private ClickableTextureComponent okButton;

		// Token: 0x04000EED RID: 3821
		private ClickableTextureComponent cancelButton;

		// Token: 0x04000EEE RID: 3822
		private ConfirmationDialog.behavior onConfirm;

		// Token: 0x04000EEF RID: 3823
		private ConfirmationDialog.behavior onCancel;

		// Token: 0x04000EF0 RID: 3824
		private bool active = true;

		// Token: 0x02000181 RID: 385
		// Token: 0x060013C8 RID: 5064
		public delegate void behavior(Farmer who);
	}
}
