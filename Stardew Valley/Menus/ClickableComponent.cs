using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Menus
{
	// Token: 0x020000E2 RID: 226
	public class ClickableComponent
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x0011C109 File Offset: 0x0011A309
		public ClickableComponent(Rectangle bounds, string name)
		{
			this.bounds = bounds;
			this.name = name;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0011C131 File Offset: 0x0011A331
		public ClickableComponent(Rectangle bounds, string name, string label)
		{
			this.bounds = bounds;
			this.name = name;
			this.label = label;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0011C160 File Offset: 0x0011A360
		public ClickableComponent(Rectangle bounds, Item item)
		{
			this.bounds = bounds;
			this.item = item;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0011C188 File Offset: 0x0011A388
		public virtual bool containsPoint(int x, int y)
		{
			return this.visible && this.bounds.Contains(x, y);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0011C1A1 File Offset: 0x0011A3A1
		public virtual void snapMouseCursor()
		{
			Game1.setMousePosition(this.bounds.Right - this.bounds.Width / 8, this.bounds.Bottom - this.bounds.Height / 8);
		}

		// Token: 0x04000EC8 RID: 3784
		public Rectangle bounds;

		// Token: 0x04000EC9 RID: 3785
		public string name;

		// Token: 0x04000ECA RID: 3786
		public string label;

		// Token: 0x04000ECB RID: 3787
		public float scale = 1f;

		// Token: 0x04000ECC RID: 3788
		public Item item;

		// Token: 0x04000ECD RID: 3789
		public bool visible = true;
	}
}
