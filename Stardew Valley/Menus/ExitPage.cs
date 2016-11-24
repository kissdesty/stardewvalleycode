using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000ED RID: 237
	public class ExitPage : IClickableMenu
	{
		// Token: 0x06000E51 RID: 3665 RVA: 0x00123E30 File Offset: 0x00122030
		public ExitPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.exitToTitle = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 + Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize * 4 - Game1.tileSize / 2, Game1.tileSize * 5, Game1.tileSize * 3 / 2), "", Game1.content.LoadString("Strings\\UI:ExitToTitle", new object[0]))
			{
				visible = false
			};
			this.exitToDesktop = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 + Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize * 6 + Game1.pixelZoom * 2 - Game1.tileSize / 2, Game1.tileSize * 5, Game1.tileSize * 3 / 2), "", Game1.content.LoadString("Strings\\UI:ExitToDesktop", new object[0]));
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00123F24 File Offset: 0x00122124
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.conventionMode)
			{
				return;
			}
			if (this.exitToTitle.containsPoint(x, y) && this.exitToTitle.visible)
			{
				Game1.playSound("bigDeSelect");
				Game1.changeMusicTrack("MainTheme");
				Game1.setGameMode(0);
				return;
			}
			if (this.exitToDesktop.containsPoint(x, y))
			{
				Game1.playSound("bigDeSelect");
				Game1.quit = true;
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00123F90 File Offset: 0x00122190
		public override void performHoverAction(int x, int y)
		{
			if (this.exitToTitle.containsPoint(x, y) && this.exitToTitle.visible)
			{
				if (this.exitToTitle.scale == 0f)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				this.exitToTitle.scale = 1f;
				return;
			}
			if (this.exitToDesktop.containsPoint(x, y))
			{
				if (this.exitToDesktop.scale == 0f)
				{
					Game1.playSound("Cowboy_gunshot");
				}
				this.exitToDesktop.scale = 1f;
				return;
			}
			this.exitToTitle.scale = 0f;
			this.exitToDesktop.scale = 0f;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00124044 File Offset: 0x00122244
		public override void draw(SpriteBatch b)
		{
			if (this.exitToTitle.visible)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), this.exitToTitle.bounds.X, this.exitToTitle.bounds.Y, this.exitToTitle.bounds.Width, this.exitToTitle.bounds.Height, (this.exitToTitle.scale > 0f) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, true);
				Utility.drawTextWithShadow(b, this.exitToTitle.label, Game1.dialogueFont, new Vector2((float)this.exitToTitle.bounds.Center.X, (float)(this.exitToTitle.bounds.Center.Y + Game1.pixelZoom)) - Game1.dialogueFont.MeasureString(this.exitToTitle.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f, 3);
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), this.exitToDesktop.bounds.X, this.exitToDesktop.bounds.Y, this.exitToDesktop.bounds.Width, this.exitToDesktop.bounds.Height, (this.exitToDesktop.scale > 0f) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, true);
			Utility.drawTextWithShadow(b, this.exitToDesktop.label, Game1.dialogueFont, new Vector2((float)this.exitToDesktop.bounds.Center.X, (float)(this.exitToDesktop.bounds.Center.Y + Game1.pixelZoom)) - Game1.dialogueFont.MeasureString(this.exitToDesktop.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0f, 3);
		}

		// Token: 0x04000F45 RID: 3909
		private ClickableComponent exitToDesktop;

		// Token: 0x04000F46 RID: 3910
		private ClickableComponent exitToTitle;
	}
}
