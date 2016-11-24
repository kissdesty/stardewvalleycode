using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x02000105 RID: 261
	public class OptionsElement
	{
		// Token: 0x06000F63 RID: 3939 RVA: 0x0013C978 File Offset: 0x0013AB78
		public OptionsElement(string label)
		{
			this.label = label;
			this.bounds = new Rectangle(8 * Game1.pixelZoom, 4 * Game1.pixelZoom, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom);
			this.whichOption = -1;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0013C9B8 File Offset: 0x0013ABB8
		public OptionsElement(string label, int x, int y, int width, int height, int whichOption = -1)
		{
			if (x == -1)
			{
				x = 8 * Game1.pixelZoom;
			}
			if (y == -1)
			{
				y = 4 * Game1.pixelZoom;
			}
			this.bounds = new Rectangle(x, y, width, height);
			this.label = label;
			this.whichOption = whichOption;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0013CA05 File Offset: 0x0013AC05
		public OptionsElement(string label, Rectangle bounds, int whichOption)
		{
			this.whichOption = whichOption;
			this.label = label;
			this.bounds = bounds;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void receiveLeftClick(int x, int y)
		{
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void leftClickReleased(int x, int y)
		{
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void receiveKeyPress(Keys key)
		{
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0013CA24 File Offset: 0x0013AC24
		public virtual void draw(SpriteBatch b, int slotX, int slotY)
		{
			if (this.whichOption == -1)
			{
				SpriteText.drawString(b, this.label, slotX + this.bounds.X, slotY + this.bounds.Y + Game1.pixelZoom * 3, 999, -1, 999, 1f, 0.1f, false, -1, "", -1);
				return;
			}
			Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float)(slotX + this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(slotY + this.bounds.Y)), this.greyedOut ? (Game1.textColor * 0.33f) : Game1.textColor, 1f, 0.1f, -1, -1, 1f, 3);
		}

		// Token: 0x04001094 RID: 4244
		public const int defaultX = 8;

		// Token: 0x04001095 RID: 4245
		public const int defaultY = 4;

		// Token: 0x04001096 RID: 4246
		public const int defaultPixelWidth = 9;

		// Token: 0x04001097 RID: 4247
		public Rectangle bounds;

		// Token: 0x04001098 RID: 4248
		public string label;

		// Token: 0x04001099 RID: 4249
		public int whichOption;

		// Token: 0x0400109A RID: 4250
		public bool greyedOut;
	}
}
