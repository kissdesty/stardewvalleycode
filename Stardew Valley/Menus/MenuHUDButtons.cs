using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000FE RID: 254
	public class MenuHUDButtons : IClickableMenu
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x00139F38 File Offset: 0x00138138
		public MenuHUDButtons() : base(Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.tileSize, Game1.viewport.Height - 21 * Game1.pixelZoom - Game1.pixelZoom * 4, 70 * Game1.pixelZoom, 21 * Game1.pixelZoom, false)
		{
			for (int i = 0; i < 7; i++)
			{
				this.buttons.Add(new ClickableComponent(new Rectangle(Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.pixelZoom * 4 + i * 9 * Game1.pixelZoom, this.yPositionOnScreen + 5 * Game1.pixelZoom, 9 * Game1.pixelZoom, 11 * Game1.pixelZoom), string.Concat(i)));
			}
			this.position = new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen);
			this.sourceRect = new Rectangle(221, 362, 70, 21);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0013A050 File Offset: 0x00138250
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!Game1.player.usingTool)
			{
				foreach (ClickableComponent c in this.buttons)
				{
					if (c.containsPoint(x, y))
					{
						Game1.activeClickableMenu = new GameMenu(Convert.ToInt32(c.name), -1);
						Game1.playSound("bigSelect");
						break;
					}
				}
			}
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0013A0D4 File Offset: 0x001382D4
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			foreach (ClickableComponent c in this.buttons)
			{
				if (c.containsPoint(x, y))
				{
					int slotNumber = Convert.ToInt32(c.name);
					this.hoverText = GameMenu.getLabelOfTabFromIndex(slotNumber);
				}
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00002834 File Offset: 0x00000A34
		public override void update(GameTime time)
		{
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0013A150 File Offset: 0x00138350
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.tileSize;
			this.yPositionOnScreen = Game1.viewport.Height - 21 * Game1.pixelZoom - Game1.pixelZoom * 4;
			for (int i = 0; i < 7; i++)
			{
				this.buttons[i].bounds = new Rectangle(Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.pixelZoom * 4 + i * 9 * Game1.pixelZoom, this.yPositionOnScreen + 5 * Game1.pixelZoom, 9 * Game1.pixelZoom, 11 * Game1.pixelZoom);
			}
			this.position = new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen);
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0013A224 File Offset: 0x00138424
		public override bool isWithinBounds(int x, int y)
		{
			return new Rectangle(this.buttons.First<ClickableComponent>().bounds.X, this.buttons.First<ClickableComponent>().bounds.Y, this.buttons.Last<ClickableComponent>().bounds.X - this.buttons.First<ClickableComponent>().bounds.X + Game1.tileSize, Game1.tileSize).Contains(x, y);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0013A2A0 File Offset: 0x001384A0
		public override void draw(SpriteBatch b)
		{
			if (Game1.activeClickableMenu != null)
			{
				return;
			}
			b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			if (!this.hoverText.Equals("") && this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04001060 RID: 4192
		public new const int width = 70;

		// Token: 0x04001061 RID: 4193
		public new const int height = 21;

		// Token: 0x04001062 RID: 4194
		private List<ClickableComponent> buttons = new List<ClickableComponent>();

		// Token: 0x04001063 RID: 4195
		private string hoverText = "";

		// Token: 0x04001064 RID: 4196
		private Vector2 position;

		// Token: 0x04001065 RID: 4197
		private Rectangle sourceRect;
	}
}
