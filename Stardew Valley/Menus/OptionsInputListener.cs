using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x02000109 RID: 265
	public class OptionsInputListener : OptionsElement
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x0013D258 File Offset: 0x0013B458
		public OptionsInputListener(string label, int whichOption, int slotWidth, int x = -1, int y = -1) : base(label, x, y, slotWidth - x, 11 * Game1.pixelZoom, whichOption)
		{
			this.setbuttonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, y + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
			if (whichOption != -1)
			{
				Game1.options.setInputListenerToProperValue(this);
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0013D2C6 File Offset: 0x0013B4C6
		public override void leftClickHeld(int x, int y)
		{
			bool arg_06_0 = this.greyedOut;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0013D2D0 File Offset: 0x0013B4D0
		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut && !this.listening && this.setbuttonBounds.Contains(x, y))
			{
				if (this.whichOption == -1)
				{
					Game1.options.setControlsToDefault();
					Game1.activeClickableMenu = new GameMenu(6, 17);
					return;
				}
				this.listening = true;
				Game1.playSound("breathin");
				GameMenu.forcePreventClose = true;
				this.listenerMessage = "Press new key...";
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0013D340 File Offset: 0x0013B540
		public override void receiveKeyPress(Keys key)
		{
			if (!this.greyedOut && this.listening)
			{
				if (key == Keys.Escape)
				{
					Game1.playSound("bigDeSelect");
					this.listening = false;
					GameMenu.forcePreventClose = false;
					return;
				}
				if (!Game1.options.isKeyInUse(key) || new InputButton(key).ToString().Equals(this.buttonNames.First<string>()))
				{
					Game1.options.changeInputListenerValue(this.whichOption, key);
					this.buttonNames[0] = key.ToString();
					Game1.playSound("coin");
					this.listening = false;
					GameMenu.forcePreventClose = false;
					return;
				}
				this.listenerMessage = "Key already in use. Try again...";
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0013D400 File Offset: 0x0013B600
		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			if (this.buttonNames.Count > 0 || this.whichOption == -1)
			{
				if (this.whichOption == -1)
				{
					Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
				}
				else
				{
					Utility.drawTextWithShadow(b, this.label + ": " + this.buttonNames.Last<string>() + ((this.buttonNames.Count > 1) ? (", " + this.buttonNames.First<string>()) : ""), Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
				}
			}
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.setbuttonBounds.X + slotX), (float)(this.setbuttonBounds.Y + slotY)), OptionsInputListener.setButtonSource, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.15f, -1, -1, 0.35f);
			if (this.listening)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * 0.75f, 0f, Vector2.Zero, SpriteEffects.None, 0.999f);
				b.DrawString(Game1.dialogueFont, this.listenerMessage, Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize, 0, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
			}
		}

		// Token: 0x040010B0 RID: 4272
		public List<string> buttonNames = new List<string>();

		// Token: 0x040010B1 RID: 4273
		private string listenerMessage;

		// Token: 0x040010B2 RID: 4274
		private bool listening;

		// Token: 0x040010B3 RID: 4275
		private Rectangle setbuttonBounds;

		// Token: 0x040010B4 RID: 4276
		public static Rectangle setButtonSource = new Rectangle(294, 428, 21, 11);
	}
}
