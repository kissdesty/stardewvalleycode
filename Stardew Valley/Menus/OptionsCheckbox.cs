using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x02000107 RID: 263
	public class OptionsCheckbox : OptionsElement
	{
		// Token: 0x06000F6F RID: 3951 RVA: 0x0013CF38 File Offset: 0x0013B138
		public OptionsCheckbox(string label, int whichOption, int x = -1, int y = -1) : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
		{
			Game1.options.setCheckBoxToProperValue(this);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0013CF60 File Offset: 0x0013B160
		public override void receiveLeftClick(int x, int y)
		{
			if (!this.greyedOut)
			{
				Game1.playSound("drumkit6");
				base.receiveLeftClick(x, y);
				this.isChecked = !this.isChecked;
				Game1.options.changeCheckBoxOption(this.whichOption, this.isChecked);
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0013CFAC File Offset: 0x0013B1AC
		public override void draw(SpriteBatch b, int slotX, int slotY)
		{
			b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X), (float)(slotY + this.bounds.Y)), new Rectangle?(this.isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked), Color.White * (this.greyedOut ? 0.33f : 1f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.4f);
			base.draw(b, slotX, slotY);
		}

		// Token: 0x040010A5 RID: 4261
		public const int pixelsWide = 9;

		// Token: 0x040010A6 RID: 4262
		public bool isChecked;

		// Token: 0x040010A7 RID: 4263
		public static Rectangle sourceRectUnchecked = new Rectangle(227, 425, 9, 9);

		// Token: 0x040010A8 RID: 4264
		public static Rectangle sourceRectChecked = new Rectangle(236, 425, 9, 9);
	}
}
