using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x02000108 RID: 264
	public class OptionsSlider : OptionsElement
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x0013D06D File Offset: 0x0013B26D
		public OptionsSlider(string label, int whichOption, int x = -1, int y = -1) : base(label, x, y, 48 * Game1.pixelZoom, 6 * Game1.pixelZoom, whichOption)
		{
			Game1.options.setSliderToProperValue(this);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0013D094 File Offset: 0x0013B294
		public override void leftClickHeld(int x, int y)
		{
			if (!this.greyedOut)
			{
				base.leftClickHeld(x, y);
				if (x < this.bounds.X)
				{
					this.value = 0;
				}
				else if (x > this.bounds.Right - 10 * Game1.pixelZoom)
				{
					this.value = 100;
				}
				else
				{
					this.value = (int)((float)(x - this.bounds.X) / (float)(this.bounds.Width - 10 * Game1.pixelZoom) * 100f);
				}
				Game1.options.changeSliderOption(this.whichOption, this.value);
			}
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0013D134 File Offset: 0x0013B334
		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut)
			{
				base.receiveLeftClick(x, y);
				this.leftClickHeld(x, y);
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0013D150 File Offset: 0x0013B350
		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			base.draw(b, slotX, slotY);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float)Game1.pixelZoom, false);
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X) + (float)(this.bounds.Width - 10 * Game1.pixelZoom) * ((float)this.value / 100f), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsSlider.sliderButtonRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
		}

		// Token: 0x040010A9 RID: 4265
		public const int pixelsWide = 48;

		// Token: 0x040010AA RID: 4266
		public const int pixelsHigh = 6;

		// Token: 0x040010AB RID: 4267
		public const int sliderButtonWidth = 10;

		// Token: 0x040010AC RID: 4268
		public const int sliderMaxValue = 100;

		// Token: 0x040010AD RID: 4269
		public int value;

		// Token: 0x040010AE RID: 4270
		public static Rectangle sliderBGSource = new Rectangle(403, 383, 6, 6);

		// Token: 0x040010AF RID: 4271
		public static Rectangle sliderButtonRect = new Rectangle(420, 441, 10, 6);
	}
}
