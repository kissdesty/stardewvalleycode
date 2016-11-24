using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x02000106 RID: 262
	public class OptionsPlusMinus : OptionsElement
	{
		// Token: 0x06000F6B RID: 3947 RVA: 0x0013CAFC File Offset: 0x0013ACFC
		public OptionsPlusMinus(string label, int whichOption, List<string> options, int x = -1, int y = -1) : base(label, x, y, 7 * Game1.pixelZoom, 7 * Game1.pixelZoom, whichOption)
		{
			this.options = options;
			Game1.options.setPlusMinusToProperValue(this);
			if (x == -1)
			{
				x = 8 * Game1.pixelZoom;
			}
			if (y == -1)
			{
				y = 4 * Game1.pixelZoom;
			}
			this.bounds = new Rectangle(x, y, 7 * Game1.pixelZoom * 2 + (int)Game1.dialogueFont.MeasureString("%%%%").X, 8 * Game1.pixelZoom);
			this.label = label;
			this.whichOption = whichOption;
			this.minusButton = new Rectangle(x, 4 + Game1.pixelZoom * 3, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom);
			this.plusButton = new Rectangle(this.bounds.Right - 8 * Game1.pixelZoom, 4 + Game1.pixelZoom * 3, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0013CBF8 File Offset: 0x0013ADF8
		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut && this.options.Count > 0)
			{
				int arg_D9_0 = this.selected;
				if (this.minusButton.Contains(x, y) && this.selected != 0)
				{
					this.selected--;
					OptionsPlusMinus.snapZoomMinus = true;
					Game1.playSound("drumkit6");
				}
				else if (this.plusButton.Contains(x, y) && this.selected != this.options.Count - 1)
				{
					this.selected++;
					OptionsPlusMinus.snapZoomPlus = true;
					Game1.playSound("drumkit6");
				}
				if (this.selected < 0)
				{
					this.selected = 0;
				}
				else if (this.selected >= this.options.Count)
				{
					this.selected = this.options.Count - 1;
				}
				if (arg_D9_0 != this.selected)
				{
					Game1.options.changeDropDownOption(this.whichOption, this.selected, this.options);
				}
			}
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0013CCFC File Offset: 0x0013AEFC
		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.minusButton.X), (float)(slotY + this.minusButton.Y)), new Rectangle?(OptionsPlusMinus.minusButtonSource), Color.White * (this.greyedOut ? 0.33f : 1f) * ((this.selected == 0) ? 0.5f : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
			b.DrawString(Game1.dialogueFont, (this.selected < this.options.Count && this.selected != -1) ? this.options[this.selected] : "", new Vector2((float)(slotX + this.minusButton.X + this.minusButton.Width + Game1.pixelZoom), (float)(slotY + this.minusButton.Y)), Game1.textColor);
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.plusButton.X), (float)(slotY + this.plusButton.Y)), new Rectangle?(OptionsPlusMinus.plusButtonSource), Color.White * (this.greyedOut ? 0.33f : 1f) * ((this.selected == this.options.Count - 1) ? 0.5f : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
			if (OptionsPlusMinus.snapZoomMinus)
			{
				Game1.setMousePosition(slotX + this.minusButton.Center.X, slotY + this.minusButton.Center.Y);
				OptionsPlusMinus.snapZoomMinus = false;
			}
			else if (OptionsPlusMinus.snapZoomPlus)
			{
				Game1.setMousePosition(slotX + this.plusButton.Center.X, slotY + this.plusButton.Center.Y);
				OptionsPlusMinus.snapZoomPlus = false;
			}
			base.draw(b, slotX, slotY);
		}

		// Token: 0x0400109B RID: 4251
		public const int pixelsWide = 7;

		// Token: 0x0400109C RID: 4252
		public List<string> options = new List<string>();

		// Token: 0x0400109D RID: 4253
		public int selected;

		// Token: 0x0400109E RID: 4254
		public bool isChecked;

		// Token: 0x0400109F RID: 4255
		public static bool snapZoomPlus;

		// Token: 0x040010A0 RID: 4256
		public static bool snapZoomMinus;

		// Token: 0x040010A1 RID: 4257
		private Rectangle minusButton;

		// Token: 0x040010A2 RID: 4258
		private Rectangle plusButton;

		// Token: 0x040010A3 RID: 4259
		public static Rectangle minusButtonSource = new Rectangle(177, 345, 7, 8);

		// Token: 0x040010A4 RID: 4260
		public static Rectangle plusButtonSource = new Rectangle(184, 345, 7, 8);
	}
}
