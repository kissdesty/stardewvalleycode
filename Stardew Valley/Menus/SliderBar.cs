using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x02000112 RID: 274
	public class SliderBar
	{
		// Token: 0x06000FD4 RID: 4052 RVA: 0x001489F4 File Offset: 0x00146BF4
		public SliderBar(int x, int y, int initialValue)
		{
			this.bounds = new Rectangle(x, y, SliderBar.defaultWidth, 20);
			this.value = initialValue;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00148A18 File Offset: 0x00146C18
		public int click(int x, int y)
		{
			if (this.bounds.Contains(x, y) && (SliderBar.currentlySelected == null || SliderBar.currentlySelected.Equals(this)))
			{
				x -= this.bounds.X;
				this.value = (int)((float)x / (float)this.bounds.Width * 100f);
			}
			return this.value;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00148A79 File Offset: 0x00146C79
		public void release(int x, int y)
		{
			SliderBar.currentlySelected = null;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00148A84 File Offset: 0x00146C84
		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X, this.bounds.Center.Y - 2, this.bounds.Width, 4), Color.DarkGray);
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.value / 100f * (float)this.bounds.Width) + 4), (float)this.bounds.Center.Y), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
		}

		// Token: 0x0400113D RID: 4413
		public static int defaultWidth = Game1.tileSize * 2;

		// Token: 0x0400113E RID: 4414
		public const int defaultHeight = 20;

		// Token: 0x0400113F RID: 4415
		public int value;

		// Token: 0x04001140 RID: 4416
		public static SliderBar currentlySelected;

		// Token: 0x04001141 RID: 4417
		public Rectangle bounds;
	}
}
