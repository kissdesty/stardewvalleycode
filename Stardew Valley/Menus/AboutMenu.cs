using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x020000D3 RID: 211
	public class AboutMenu : IClickableMenu
	{
		// Token: 0x06000D63 RID: 3427 RVA: 0x0010DA14 File Offset: 0x0010BC14
		public AboutMenu()
		{
			Vector2 topLeft = Utility.getTopLeftPositionForCenteringOnScreen(800, 600, 0, 0);
			this.linkToSVSite = new ClickableComponent(new Rectangle((int)topLeft.X + Game1.tileSize / 2, (int)topLeft.Y + 600 - 100 - Game1.tileSize * 3 - Game1.pixelZoom * 4, 800 - Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:About_Website", new object[0]));
			this.linkToTwitter = new ClickableComponent(new Rectangle((int)topLeft.X + Game1.tileSize / 2, (int)topLeft.Y + 600 - 100 - Game1.tileSize * 2 - Game1.pixelZoom * 4, 800 - Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:About_ConcernedApe", new object[0]));
			this.linkToChucklefish = new ClickableComponent(new Rectangle((int)topLeft.X + Game1.tileSize / 2, (int)topLeft.Y + 600 - 100 - Game1.tileSize - Game1.pixelZoom * 4, 800 - Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:About_Chucklefish", new object[0]));
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0010DB74 File Offset: 0x0010BD74
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (this.linkToSVSite.containsPoint(x, y))
			{
				try
				{
					Process.Start("http://www.stardewvalley.net");
				}
				catch (Exception)
				{
				}
				Game1.playSound("bigSelect");
				return;
			}
			if (this.linkToTwitter.containsPoint(x, y))
			{
				try
				{
					Process.Start("http://www.twitter.com/ConcernedApe");
				}
				catch (Exception)
				{
				}
				Game1.playSound("bigSelect");
				return;
			}
			if (this.linkToChucklefish.containsPoint(x, y))
			{
				try
				{
					Process.Start("http://blog.chucklefish.org/");
				}
				catch (Exception)
				{
				}
				Game1.playSound("bigSelect");
				return;
			}
			this.isWithinBounds(x, y);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0010DC3C File Offset: 0x0010BE3C
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.linkToSVSite.scale = 1f;
			this.linkToTwitter.scale = 1f;
			this.linkToChucklefish.scale = 1f;
			if (this.linkToSVSite.containsPoint(x, y))
			{
				this.linkToSVSite.scale = 2f;
				return;
			}
			if (this.linkToTwitter.containsPoint(x, y))
			{
				this.linkToTwitter.scale = 2f;
				return;
			}
			if (this.linkToChucklefish.containsPoint(x, y))
			{
				this.linkToChucklefish.scale = 2f;
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0010DCE0 File Offset: 0x0010BEE0
		public override void draw(SpriteBatch b)
		{
			Vector2 topLeft = Utility.getTopLeftPositionForCenteringOnScreen(800, 500, 0, 0);
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(473, 36, 24, 24), (int)topLeft.X, (int)topLeft.Y, 800, 450, Color.White, (float)Game1.pixelZoom, true);
			SpriteText.drawString(b, Game1.content.LoadString("Strings\\UI:About_Title", new object[0]), (int)topLeft.X + Game1.tileSize / 2, (int)topLeft.Y + Game1.tileSize / 2, 9999, -1, 9999, 1f, 0.88f, false, -1, "", 6);
			SpriteText.drawString(b, Game1.content.LoadString("Strings\\UI:About_Credit", new object[0]).Replace('\n', '^'), (int)topLeft.X + Game1.tileSize / 2, (int)topLeft.Y + Game1.tileSize / 2, 9999, -1, 9999, 1f, 0.88f, false, -1, "", 4);
			SpriteText.drawString(b, "= " + this.linkToSVSite.label, (int)topLeft.X + Game1.tileSize / 2, this.linkToSVSite.bounds.Y, 999, -1, 999, 1f, 1f, false, -1, "", (this.linkToSVSite.scale == 1f) ? 3 : 7);
			SpriteText.drawString(b, "= " + this.linkToTwitter.label, (int)topLeft.X + Game1.tileSize / 2, this.linkToTwitter.bounds.Y, 999, -1, 999, 1f, 1f, false, -1, "", (this.linkToTwitter.scale == 1f) ? 3 : 7);
			SpriteText.drawString(b, "< " + this.linkToChucklefish.label, (int)topLeft.X + Game1.tileSize / 2, this.linkToChucklefish.bounds.Y, 999, -1, 999, 1f, 1f, false, -1, "", (this.linkToChucklefish.scale == 1f) ? 3 : 7);
			if (this.linkToChucklefish.scale > 1f)
			{
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(this.linkToChucklefish.bounds.Right - Game1.tileSize * 5), (float)(this.linkToChucklefish.bounds.Y - Game1.pixelZoom)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 128, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
			}
			else if (this.linkToSVSite.scale <= 1f)
			{
				float arg_325_0 = this.linkToTwitter.scale;
			}
			b.Draw(Game1.mouseCursors, new Vector2(topLeft.X + 800f - (float)(Game1.tileSize * 3 / 2), topLeft.Y + (float)(Game1.tileSize * 2)), new Rectangle?(new Rectangle(540 + 13 * (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 150.0), 333, 13, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(topLeft.X + 800f - (float)(Game1.tileSize * 3 / 2), topLeft.Y + (float)(Game1.tileSize * 5)), new Rectangle?(new Rectangle(592 + 13 * (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 150.0), 333, 13, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is TitleMenu && (Game1.activeClickableMenu as TitleMenu).startupMessage.Length > 0)
			{
				b.DrawString(Game1.smallFont, Game1.parseText((Game1.activeClickableMenu as TitleMenu).startupMessage, Game1.smallFont, Game1.tileSize * 10), new Vector2((float)(Game1.pixelZoom * 2), (float)Game1.viewport.Height - Game1.smallFont.MeasureString(Game1.parseText((Game1.activeClickableMenu as TitleMenu).startupMessage, Game1.smallFont, Game1.tileSize * 10)).Y - (float)Game1.pixelZoom), Color.White);
				return;
			}
			b.DrawString(Game1.smallFont, "v" + Game1.version, new Vector2((float)(Game1.tileSize / 4), (float)Game1.viewport.Height - Game1.smallFont.MeasureString("v" + Game1.version).Y - (float)(Game1.pixelZoom * 2)), Color.White);
		}

		// Token: 0x04000DD5 RID: 3541
		public new const int width = 800;

		// Token: 0x04000DD6 RID: 3542
		public new const int height = 600;

		// Token: 0x04000DD7 RID: 3543
		private ClickableComponent linkToTwitter;

		// Token: 0x04000DD8 RID: 3544
		private ClickableComponent linkToSVSite;

		// Token: 0x04000DD9 RID: 3545
		private ClickableComponent linkToChucklefish;
	}
}
