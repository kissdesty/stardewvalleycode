using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x02000103 RID: 259
	public class NamingMenu : IClickableMenu
	{
		// Token: 0x06000F55 RID: 3925 RVA: 0x0013BC0C File Offset: 0x00139E0C
		public NamingMenu(NamingMenu.doneNamingBehavior b, string title, string defaultName = null)
		{
			this.doneNaming = b;
			this.xPositionOnScreen = 0;
			this.yPositionOnScreen = 0;
			this.width = Game1.viewport.Width;
			this.height = Game1.viewport.Height;
			this.title = title;
			this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
			this.textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
			this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
			this.textBox.Y = Game1.viewport.Height / 2;
			this.textBox.Width = Game1.tileSize * 4;
			this.textBox.Height = Game1.tileSize * 3;
			this.e = new TextBoxEvent(this.textBoxEnter);
			this.textBox.OnEnterPressed += this.e;
			Game1.keyboardDispatcher.Subscriber = this.textBox;
			this.textBox.Text = ((defaultName != null) ? defaultName : Dialogue.randomName());
			this.textBox.Selected = true;
			this.randomButton = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false);
			this.doneNamingButton = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0013BE70 File Offset: 0x0013A070
		public void textBoxEnter(TextBox sender)
		{
			if (sender.Text.Length >= this.minLength)
			{
				if (this.doneNaming != null)
				{
					this.doneNaming(sender.Text);
					this.textBox.Selected = false;
					return;
				}
				Game1.exitActiveMenu();
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveKeyPress(Keys key)
		{
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0013BEB0 File Offset: 0x0013A0B0
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			if (this.doneNamingButton != null)
			{
				if (this.doneNamingButton.containsPoint(x, y))
				{
					this.doneNamingButton.scale = Math.Min(1.1f, this.doneNamingButton.scale + 0.05f);
				}
				else
				{
					this.doneNamingButton.scale = Math.Max(1f, this.doneNamingButton.scale - 0.05f);
				}
			}
			this.randomButton.tryHover(x, y, 0.5f);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0013BF3C File Offset: 0x0013A13C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (this.doneNamingButton.containsPoint(x, y))
			{
				this.textBoxEnter(this.textBox);
				Game1.playSound("smallSelect");
				return;
			}
			if (this.randomButton.containsPoint(x, y))
			{
				this.textBox.Text = Dialogue.randomName();
				this.randomButton.scale = this.randomButton.baseScale;
				Game1.playSound("drumkit6");
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0013BFB8 File Offset: 0x0013A1B8
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			SpriteText.drawStringWithScrollCenteredAt(b, this.title, Game1.viewport.Width / 2, Game1.viewport.Height / 2 - Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
			this.textBox.Draw(b);
			this.doneNamingButton.draw(b);
			this.randomButton.draw(b);
			base.drawMouse(b);
		}

		// Token: 0x04001081 RID: 4225
		protected ClickableTextureComponent doneNamingButton;

		// Token: 0x04001082 RID: 4226
		protected ClickableTextureComponent randomButton;

		// Token: 0x04001083 RID: 4227
		protected TextBox textBox;

		// Token: 0x04001084 RID: 4228
		private TextBoxEvent e;

		// Token: 0x04001085 RID: 4229
		private NamingMenu.doneNamingBehavior doneNaming;

		// Token: 0x04001086 RID: 4230
		private string title;

		// Token: 0x04001087 RID: 4231
		protected int minLength = 1;

		// Token: 0x02000185 RID: 389
		// Token: 0x060013D8 RID: 5080
		public delegate void doneNamingBehavior(string s);
	}
}
