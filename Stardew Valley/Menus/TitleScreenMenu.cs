using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x02000119 RID: 281
	public class TitleScreenMenu : IClickableMenu
	{
		// Token: 0x06001029 RID: 4137 RVA: 0x0014EDF0 File Offset: 0x0014CFF0
		public TitleScreenMenu() : base(Game1.tileSize / 8, Game1.tileSize / 12, 256, 600, false)
		{
			this.texture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\TitleButtons");
			this.buttons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + 4, this.yPositionOnScreen, 256, 157), "by ConcernedApe"));
			this.buttons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + 4, this.yPositionOnScreen + 184, 256, 128), "New Game"));
			this.buttons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + 4, this.yPositionOnScreen + 317, 256, 128), "Load Game"));
			this.buttons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + 4, this.yPositionOnScreen + 448, 256, 128), "Co-op"));
			this.buttons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + 4, this.yPositionOnScreen + 579, 256, 128), "Leave Stardew Valley"));
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0014EF50 File Offset: 0x0014D150
		public void performAction(string buttonName)
		{
			if (buttonName == "Leave Stardew Valley")
			{
				Game1.playSound("bigDeSelect");
				Game1.quit = true;
				return;
			}
			if (buttonName == "New Game")
			{
				Game1.fadeIn = false;
				Game1.fadeToBlack = true;
				Game1.fadeToBlackAlpha = 0.99f;
				Game1.playSound("select");
				Game1.changeMusicTrack("none");
				Game1.exitActiveMenu();
				return;
			}
			if (buttonName == "Load Game")
			{
				Game1.playSound("smallSelect");
				this.loadScreen = new LoadGameScreen();
				return;
			}
			if (!(buttonName == "Co-op"))
			{
				return;
			}
			Game1.playSound("smallSelect");
			this.coopScreen = new CooperativeMenu();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0014F000 File Offset: 0x0014D200
		public override void receiveLeftClick(int xPositionOnScreen, int yPositionOnScreen, bool playSound = true)
		{
			if (this.loadScreen != null)
			{
				this.loadScreen.receiveLeftClick(xPositionOnScreen, yPositionOnScreen, true);
			}
			if (this.coopScreen != null)
			{
				this.coopScreen.receiveLeftClick(xPositionOnScreen, yPositionOnScreen, true);
				return;
			}
			new Vector2((float)(Game1.viewport.X + xPositionOnScreen), (float)(Game1.viewport.Y + yPositionOnScreen));
			foreach (ClickableComponent c in this.buttons)
			{
				if (c.containsPoint(xPositionOnScreen, yPositionOnScreen))
				{
					this.performAction(c.name);
					break;
				}
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0014F0B4 File Offset: 0x0014D2B4
		public override void receiveRightClick(int xPositionOnScreen, int yPositionOnScreen, bool playSound = true)
		{
			if (this.loadScreen != null || this.coopScreen != null)
			{
				this.loadScreen = null;
				this.coopScreen = null;
				Game1.keyboardDispatcher.Subscriber = null;
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0014F0DF File Offset: 0x0014D2DF
		public override void receiveKeyPress(Keys key)
		{
			if (key.Equals(Keys.Escape))
			{
				this.loadScreen = null;
				this.coopScreen = null;
				Game1.keyboardDispatcher.Subscriber = null;
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0014F110 File Offset: 0x0014D310
		public override void performHoverAction(int xPositionOnScreen, int yPositionOnScreen)
		{
			if (this.loadScreen != null)
			{
				this.loadScreen.performHoverAction(xPositionOnScreen, yPositionOnScreen);
				return;
			}
			if (this.coopScreen != null)
			{
				this.coopScreen.performHoverAction(xPositionOnScreen, yPositionOnScreen);
				return;
			}
			this.hoverText = "";
			foreach (ClickableComponent c in this.buttons)
			{
				if (c.containsPoint(xPositionOnScreen, yPositionOnScreen))
				{
					if (c.name.Contains("Ape"))
					{
						this.hoverText = c.name;
					}
					else
					{
						c.scale = Math.Min(c.scale + 0.01f, 1.03f);
					}
				}
				else
				{
					c.scale = Math.Max(c.scale - 0.01f, 1f);
				}
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0014F1F8 File Offset: 0x0014D3F8
		public override void draw(SpriteBatch b)
		{
			b.Draw(this.texture, new Vector2((float)(this.xPositionOnScreen + 12), (float)(this.yPositionOnScreen + 5)), new Rectangle?(this.texture.Bounds), Color.White);
			foreach (ClickableComponent c in this.buttons)
			{
				b.Draw(this.texture, new Vector2((float)(this.xPositionOnScreen + c.bounds.Center.X), (float)(this.yPositionOnScreen + c.bounds.Center.Y)), new Rectangle?(new Rectangle(c.bounds.X - this.xPositionOnScreen - 4, c.bounds.Y - this.yPositionOnScreen, c.bounds.Width, c.bounds.Height)), Color.White, 0f, new Vector2((float)(c.bounds.Width / 2), (float)(c.bounds.Height / 2)), c.scale, SpriteEffects.None, 0.9f);
			}
			if (this.hoverText != null && !this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.loadScreen != null)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
				this.loadScreen.draw(b);
			}
			else if (this.coopScreen != null)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
				this.coopScreen.draw(b);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
		}

		// Token: 0x040011A3 RID: 4515
		private Texture2D texture;

		// Token: 0x040011A4 RID: 4516
		private string hoverText;

		// Token: 0x040011A5 RID: 4517
		private List<ClickableComponent> buttons = new List<ClickableComponent>();

		// Token: 0x040011A6 RID: 4518
		private IClickableMenu loadScreen;

		// Token: 0x040011A7 RID: 4519
		private IClickableMenu coopScreen;
	}
}
