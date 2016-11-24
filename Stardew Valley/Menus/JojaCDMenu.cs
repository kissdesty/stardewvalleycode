using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Locations;

namespace StardewValley.Menus
{
	// Token: 0x020000F6 RID: 246
	public class JojaCDMenu : IClickableMenu
	{
		// Token: 0x06000ECF RID: 3791 RVA: 0x0012F498 File Offset: 0x0012D698
		public JojaCDMenu(Texture2D noteTexture) : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 288, 1280, 576, true)
		{
			Game1.player.forceCanMove();
			this.noteTexture = noteTexture;
			int x = this.xPositionOnScreen + Game1.pixelZoom;
			int y = this.yPositionOnScreen + 52 * Game1.pixelZoom;
			for (int i = 0; i < 5; i++)
			{
				this.checkboxes.Add(new ClickableComponent(new Rectangle(x, y, 147 * Game1.pixelZoom, 30 * Game1.pixelZoom), string.Concat(i)));
				x += 148 * Game1.pixelZoom;
				if (x > this.xPositionOnScreen + 148 * Game1.pixelZoom * 2)
				{
					x = this.xPositionOnScreen + Game1.pixelZoom;
					y += 30 * Game1.pixelZoom;
				}
			}
			if (Game1.player.hasOrWillReceiveMail("ccVault"))
			{
				this.checkboxes[0].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccBoilerRoom"))
			{
				this.checkboxes[1].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccCraftsRoom"))
			{
				this.checkboxes[2].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccPantry"))
			{
				this.checkboxes[3].name = "complete";
			}
			if (Game1.player.hasOrWillReceiveMail("ccFishTank"))
			{
				this.checkboxes[4].name = "complete";
			}
			this.exitFunction = new IClickableMenu.onExit(this.onExitFunction);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0012F66D File Offset: 0x0012D86D
		private void onExitFunction()
		{
			if (this.boughtSomething)
			{
				JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_JojaCDConfirm", new object[0]), false, false);
				Game1.drawDialogue(JojaMart.Morris);
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0012F6A4 File Offset: 0x0012D8A4
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.exitTimer >= 0)
			{
				return;
			}
			base.receiveLeftClick(x, y, true);
			foreach (ClickableComponent b in this.checkboxes)
			{
				if (b.containsPoint(x, y) && !b.name.Equals("complete"))
				{
					int buttonNumber = Convert.ToInt32(b.name);
					int price = this.getPriceFromButtonNumber(buttonNumber);
					if (Game1.player.money >= price)
					{
						Game1.player.money -= price;
						Game1.playSound("reward");
						b.name = "complete";
						this.boughtSomething = true;
						switch (buttonNumber)
						{
						case 0:
							Game1.addMailForTomorrow("jojaVault", true, true);
							Game1.addMailForTomorrow("ccVault", true, true);
							break;
						case 1:
							Game1.addMailForTomorrow("jojaBoilerRoom", true, true);
							Game1.addMailForTomorrow("ccBoilerRoom", true, true);
							break;
						case 2:
							Game1.addMailForTomorrow("jojaCraftsRoom", true, true);
							Game1.addMailForTomorrow("ccCraftsRoom", true, true);
							break;
						case 3:
							Game1.addMailForTomorrow("jojaPantry", true, true);
							Game1.addMailForTomorrow("ccPantry", true, true);
							break;
						case 4:
							Game1.addMailForTomorrow("jojaFishTank", true, true);
							Game1.addMailForTomorrow("ccFishTank", true, true);
							break;
						}
						this.exitTimer = 1000;
					}
					else
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
					}
				}
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool readyToClose()
		{
			return true;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0012F844 File Offset: 0x0012DA44
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.exitTimer >= 0)
			{
				this.exitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.exitTimer <= 0)
				{
					base.exitThisMenu(true);
				}
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0012F88C File Offset: 0x0012DA8C
		public int getPriceFromButtonNumber(int buttonNumber)
		{
			switch (buttonNumber)
			{
			case 0:
				return 40000;
			case 1:
				return 15000;
			case 2:
				return 25000;
			case 3:
				return 35000;
			case 4:
				return 20000;
			default:
				return -1;
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0012F8C9 File Offset: 0x0012DAC9
		public string getDescriptionFromButtonNumber(int buttonNumber)
		{
			return Game1.content.LoadString("Strings\\UI:JojaCDMenu_Hover" + buttonNumber, new object[0]);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0012F8EC File Offset: 0x0012DAEC
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverText = "";
			foreach (ClickableComponent b in this.checkboxes)
			{
				if (b.containsPoint(x, y))
				{
					this.hoverText = (b.name.Equals("complete") ? "" : Game1.parseText(this.getDescriptionFromButtonNumber(Convert.ToInt32(b.name)), Game1.dialogueFont, Game1.tileSize * 6));
				}
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0012F998 File Offset: 0x0012DB98
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - 640;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - 288;
			int x = this.xPositionOnScreen + Game1.pixelZoom;
			int y = this.yPositionOnScreen + 52 * Game1.pixelZoom;
			this.checkboxes.Clear();
			for (int i = 0; i < 5; i++)
			{
				this.checkboxes.Add(new ClickableComponent(new Rectangle(x, y, 147 * Game1.pixelZoom, 30 * Game1.pixelZoom), string.Concat(i)));
				x += 148 * Game1.pixelZoom;
				if (x > this.xPositionOnScreen + 148 * Game1.pixelZoom * 2)
				{
					x = this.xPositionOnScreen + Game1.pixelZoom;
					y += 30 * Game1.pixelZoom;
				}
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0012FA83 File Offset: 0x0012DC83
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0012FA8C File Offset: 0x0012DC8C
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			b.Draw(this.noteTexture, Utility.getTopLeftPositionForCenteringOnScreen(1280, 576, 0, 0), new Rectangle?(new Rectangle(0, 0, 320, 144)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			base.draw(b);
			foreach (ClickableComponent c in this.checkboxes)
			{
				if (c.name.Equals("complete"))
				{
					b.Draw(this.noteTexture, new Vector2((float)(c.bounds.Left + 4 * Game1.pixelZoom), (float)(c.bounds.Y + 4 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 144, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
				}
			}
			Game1.dayTimeMoneyBox.drawMoneyBox(b, Game1.viewport.Width - 300 - IClickableMenu.spaceToClearSideBorder * 2, Game1.pixelZoom);
			base.drawMouse(b);
			if (this.hoverText != null && !this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04000FE4 RID: 4068
		public new const int width = 1280;

		// Token: 0x04000FE5 RID: 4069
		public new const int height = 576;

		// Token: 0x04000FE6 RID: 4070
		public const int buttonWidth = 147;

		// Token: 0x04000FE7 RID: 4071
		public const int buttonHeight = 30;

		// Token: 0x04000FE8 RID: 4072
		private Texture2D noteTexture;

		// Token: 0x04000FE9 RID: 4073
		private List<ClickableComponent> checkboxes = new List<ClickableComponent>();

		// Token: 0x04000FEA RID: 4074
		private string hoverText;

		// Token: 0x04000FEB RID: 4075
		private bool boughtSomething;

		// Token: 0x04000FEC RID: 4076
		private int exitTimer = -1;
	}
}
