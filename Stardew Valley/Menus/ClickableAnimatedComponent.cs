using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000E1 RID: 225
	public class ClickableAnimatedComponent : ClickableComponent
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x0011C000 File Offset: 0x0011A200
		public ClickableAnimatedComponent(Rectangle bounds, string name, string hoverText, TemporaryAnimatedSprite sprite, bool drawLabel) : base(bounds, name)
		{
			this.sprite = sprite;
			this.sprite.position = new Vector2((float)bounds.X, (float)bounds.Y);
			this.baseScale = sprite.scale;
			this.hoverText = hoverText;
			this.drawLabel = drawLabel;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0011C062 File Offset: 0x0011A262
		public ClickableAnimatedComponent(Rectangle bounds, string name, string hoverText, TemporaryAnimatedSprite sprite) : this(bounds, name, hoverText, sprite, true)
		{
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0011C070 File Offset: 0x0011A270
		public void update(GameTime time)
		{
			this.sprite.update(time);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0011C080 File Offset: 0x0011A280
		public string tryHover(int x, int y)
		{
			if (this.bounds.Contains(x, y))
			{
				this.sprite.scale = Math.Min(this.sprite.scale + 0.02f, this.baseScale + 0.1f);
				return this.hoverText;
			}
			this.sprite.scale = Math.Max(this.sprite.scale - 0.02f, this.baseScale);
			return null;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0011C0F8 File Offset: 0x0011A2F8
		public void draw(SpriteBatch b)
		{
			this.sprite.draw(b, true, 0, 0);
		}

		// Token: 0x04000EC3 RID: 3779
		public TemporaryAnimatedSprite sprite;

		// Token: 0x04000EC4 RID: 3780
		public Rectangle sourceRect;

		// Token: 0x04000EC5 RID: 3781
		public float baseScale;

		// Token: 0x04000EC6 RID: 3782
		public string hoverText = "";

		// Token: 0x04000EC7 RID: 3783
		private bool drawLabel;
	}
}
