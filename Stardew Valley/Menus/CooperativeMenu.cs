using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000E7 RID: 231
	public class CooperativeMenu : IClickableMenu
	{
		// Token: 0x06000E0E RID: 3598 RVA: 0x0011E68C File Offset: 0x0011C88C
		public CooperativeMenu() : base(Game1.viewport.Width / 2 - Game1.viewport.Width / 5, (int)((float)Game1.viewport.Height * 0.2f + (float)Game1.tileSize), (int)((float)Game1.viewport.Width * 0.4f), (int)((float)Game1.viewport.Height * 0.2f), false)
		{
			this.keyboardDispatcher = Game1.keyboardDispatcher;
			this.textBox = new TextBox(null, null, Game1.dialogueFont, Color.Black);
			this.textBox.X = this.xPositionOnScreen;
			this.textBox.Y = this.yPositionOnScreen;
			this.textBox.Width = Game1.tileSize * 6;
			this.textBox.Height = Game1.tileSize * 3;
			this.e = new TextBoxEvent(this.textBoxEnter);
			this.textBox.OnEnterPressed += this.e;
			this.keyboardDispatcher.Subscriber = this.textBox;
			this.joinGame = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height + Game1.tileSize, this.width * 2 / 3, this.height), "Join Valley");
			this.hostGame = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height * 2 + Game1.tileSize, this.width * 2 / 3, this.height), "Host Valley");
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0011E810 File Offset: 0x0011CA10
		public void textBoxEnter(TextBox sender)
		{
			if (this.currentView == 1)
			{
				this.connectionError = "";
				this.attemptingConnectToServer = true;
				Game1.client.initializeConnection(sender.Text);
				this.endTimeForConnectionAttempt = DateTime.Now.Ticks / 10000L + 5000L;
				Game1.playSound("phone");
				return;
			}
			if (this.currentView == 2)
			{
				this.textBox.OnEnterPressed -= this.e;
				Game1.multiplayerMode = 2;
				Game1.fadeIn = false;
				Game1.fadeToBlack = true;
				Game1.fadeToBlackAlpha = 0.99f;
				Game1.playSound("select");
				Game1.exitActiveMenu();
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0011E8BC File Offset: 0x0011CABC
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.currentView == 0)
			{
				if (this.joinGame.containsPoint(x, y))
				{
					this.currentView = 1;
					this.textBox.Text = "";
					Game1.playSound("smallSelect");
				}
				else if (this.hostGame.containsPoint(x, y))
				{
					this.currentView = 2;
					this.textBox.Text = "";
					Game1.playSound("smallSelect");
				}
			}
			else
			{
				this.textBox.Update();
			}
			this.isWithinBounds(x, y);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0011E949 File Offset: 0x0011CB49
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.currentView != 0)
			{
				this.currentView = 0;
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0011E95C File Offset: 0x0011CB5C
		public override void performHoverAction(int x, int y)
		{
			if (this.joinGame.containsPoint(x, y))
			{
				this.joinGame.scale = Math.Min(this.joinGame.scale + 1f, 8f);
			}
			else
			{
				this.joinGame.scale = Math.Max(this.joinGame.scale - 1f, 1f);
			}
			if (this.hostGame.containsPoint(x, y))
			{
				this.hostGame.scale = Math.Min(this.hostGame.scale + 1f, 8f);
				return;
			}
			this.hostGame.scale = Math.Max(this.hostGame.scale - 1f, 1f);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0011EA24 File Offset: 0x0011CC24
		public void updateConnection()
		{
			if (!Game1.client.hasHandshaked && DateTime.Now.Ticks / 10000L < this.endTimeForConnectionAttempt)
			{
				Game1.client.receiveMessages();
				return;
			}
			if (!Game1.client.hasHandshaked)
			{
				this.connectionError = "No response";
				this.attemptingConnectToServer = false;
				return;
			}
			this.textBox.OnEnterPressed -= this.e;
			Game1.multiplayerMode = 1;
			Game1.fadeIn = false;
			Game1.fadeToBlack = true;
			Game1.fadeToBlackAlpha = 0.99f;
			Game1.playSound("select");
			Game1.exitActiveMenu();
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0011EAC0 File Offset: 0x0011CCC0
		public override void draw(SpriteBatch b)
		{
			if (this.attemptingConnectToServer)
			{
				this.updateConnection();
				int earthFrame = 20 + (int)(DateTime.Now.Ticks / 10000L % 1000L / 250L);
				b.Draw(Game1.animations, new Vector2((float)(this.textBox.X + this.textBox.Width + Game1.tileSize / 2), (float)(this.yPositionOnScreen - Game1.tileSize / 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.animations, earthFrame, -1, -1)), Color.White);
			}
			if (this.currentView == 0)
			{
				Game1.drawDialogueBox(this.joinGame.bounds.X - (int)this.joinGame.scale, this.joinGame.bounds.Y - Game1.tileSize, this.joinGame.bounds.Width + (int)this.joinGame.scale, this.joinGame.bounds.Height + Game1.tileSize, false, true, null, false);
				b.DrawString(Game1.dialogueFont, "Join a Valley", new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 / 3), (float)(this.joinGame.bounds.Y + Game1.tileSize * 5 / 4)), Game1.textColor);
				Game1.drawDialogueBox(this.hostGame.bounds.X - (int)this.hostGame.scale, this.hostGame.bounds.Y - Game1.tileSize, this.hostGame.bounds.Width + (int)this.hostGame.scale, this.hostGame.bounds.Height + Game1.tileSize, false, true, null, false);
				b.DrawString(Game1.dialogueFont, "Host a Valley", new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 / 3), (float)(this.hostGame.bounds.Y + Game1.tileSize * 5 / 4)), Game1.textColor);
				return;
			}
			if (this.currentView == 2)
			{
				this.textBox.Draw(b);
				return;
			}
			if (this.currentView == 1)
			{
				this.textBox.Draw(b);
				b.DrawString(Game1.dialogueFont, this.connectionError, new Vector2((float)this.xPositionOnScreen, (float)(this.yPositionOnScreen + Game1.tileSize)), Color.Red);
			}
		}

		// Token: 0x04000EF1 RID: 3825
		public const int timeOutForServerFind = 5000;

		// Token: 0x04000EF2 RID: 3826
		public const byte joinOrHostScreen = 0;

		// Token: 0x04000EF3 RID: 3827
		public const byte joinGameScreen = 1;

		// Token: 0x04000EF4 RID: 3828
		public const byte hostGameScreen = 2;

		// Token: 0x04000EF5 RID: 3829
		private KeyboardDispatcher keyboardDispatcher;

		// Token: 0x04000EF6 RID: 3830
		private TextBox textBox;

		// Token: 0x04000EF7 RID: 3831
		private string connectionError = "";

		// Token: 0x04000EF8 RID: 3832
		private ClickableComponent joinGame;

		// Token: 0x04000EF9 RID: 3833
		private ClickableComponent hostGame;

		// Token: 0x04000EFA RID: 3834
		private byte currentView;

		// Token: 0x04000EFB RID: 3835
		private bool attemptingConnectToServer;

		// Token: 0x04000EFC RID: 3836
		private long endTimeForConnectionAttempt;

		// Token: 0x04000EFD RID: 3837
		private TextBoxEvent e;
	}
}
