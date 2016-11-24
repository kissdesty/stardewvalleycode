using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x0200010A RID: 266
	public class OptionsDropDown : OptionsElement
	{
		// Token: 0x06000F7E RID: 3966 RVA: 0x0013D610 File Offset: 0x0013B810
		public OptionsDropDown(string label, int whichOption, int x = -1, int y = -1) : base(label, x, y, (int)Game1.smallFont.MeasureString("Windowed Borderless ").X + Game1.pixelZoom * 12, 11 * Game1.pixelZoom, whichOption)
		{
			Game1.options.setDropDownToProperValue(this);
			this.dropDownBounds = new Rectangle(this.bounds.X, this.bounds.Y, this.bounds.Width - Game1.pixelZoom * 12, this.bounds.Height * this.dropDownOptions.Count);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0013D6B0 File Offset: 0x0013B8B0
		public override void leftClickHeld(int x, int y)
		{
			if (!this.greyedOut)
			{
				base.leftClickHeld(x, y);
				this.clicked = true;
				this.dropDownBounds.Y = Math.Min(this.dropDownBounds.Y, Game1.viewport.Height - this.dropDownBounds.Height - this.recentSlotY);
				this.selectedOption = (int)Math.Max(Math.Min((float)(y - this.dropDownBounds.Y) / (float)this.bounds.Height, (float)(this.dropDownOptions.Count - 1)), 0f);
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0013D74E File Offset: 0x0013B94E
		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut)
			{
				base.receiveLeftClick(x, y);
				this.startingSelected = this.selectedOption;
				this.leftClickHeld(x, y);
				Game1.playSound("shwip");
				OptionsDropDown.selected = this;
			}
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0013D784 File Offset: 0x0013B984
		public override void leftClickReleased(int x, int y)
		{
			if (!this.greyedOut && this.dropDownOptions.Count > 0)
			{
				base.leftClickReleased(x, y);
				this.clicked = false;
				if (this.dropDownBounds.Contains(x, y))
				{
					Game1.options.changeDropDownOption(this.whichOption, this.selectedOption, this.dropDownOptions);
				}
				else
				{
					this.selectedOption = this.startingSelected;
				}
				OptionsDropDown.selected = null;
			}
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0013D7F8 File Offset: 0x0013B9F8
		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			this.recentSlotY = slotY;
			base.draw(b, slotX, slotY);
			float alpha = this.greyedOut ? 0.33f : 1f;
			if (this.clicked)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, slotX + this.dropDownBounds.X, slotY + this.dropDownBounds.Y, this.dropDownBounds.Width, this.dropDownBounds.Height, Color.White * alpha, (float)Game1.pixelZoom, false);
				for (int i = 0; i < this.dropDownOptions.Count; i++)
				{
					if (i == this.selectedOption)
					{
						b.Draw(Game1.staminaRect, new Rectangle(slotX + this.dropDownBounds.X, slotY + this.dropDownBounds.Y + i * this.bounds.Height, this.dropDownBounds.Width, this.bounds.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Wheat, 0f, Vector2.Zero, SpriteEffects.None, 0.975f);
					}
					b.DrawString(Game1.smallFont, this.dropDownOptions[i], new Vector2((float)(slotX + this.dropDownBounds.X + Game1.pixelZoom), (float)(slotY + this.dropDownBounds.Y + Game1.pixelZoom * 2 + this.bounds.Height * i)), Game1.textColor * alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.98f);
				}
				b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.Wheat * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.981f);
				return;
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsDropDown.dropDownBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width - Game1.pixelZoom * 12, this.bounds.Height, Color.White * alpha, (float)Game1.pixelZoom, false);
			if (OptionsDropDown.selected == null || OptionsDropDown.selected.Equals(this))
			{
				b.DrawString(Game1.smallFont, (this.selectedOption < this.dropDownOptions.Count && this.selectedOption >= 0) ? this.dropDownOptions[this.selectedOption] : "", new Vector2((float)(slotX + this.bounds.X + Game1.pixelZoom), (float)(slotY + this.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor * alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.88f);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + this.bounds.Width - Game1.pixelZoom * 12), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsDropDown.dropDownButtonSource), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
		}

		// Token: 0x040010B5 RID: 4277
		public const int pixelsHigh = 11;

		// Token: 0x040010B6 RID: 4278
		public static OptionsDropDown selected;

		// Token: 0x040010B7 RID: 4279
		public List<string> dropDownOptions = new List<string>();

		// Token: 0x040010B8 RID: 4280
		public int selectedOption;

		// Token: 0x040010B9 RID: 4281
		public int recentSlotY;

		// Token: 0x040010BA RID: 4282
		public int startingSelected;

		// Token: 0x040010BB RID: 4283
		private bool clicked;

		// Token: 0x040010BC RID: 4284
		private Rectangle dropDownBounds;

		// Token: 0x040010BD RID: 4285
		public static Rectangle dropDownBGSource = new Rectangle(433, 451, 3, 3);

		// Token: 0x040010BE RID: 4286
		public static Rectangle dropDownButtonSource = new Rectangle(437, 450, 10, 11);
	}
}
