using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000E3 RID: 227
	public class ClickableTextureComponent : ClickableComponent
	{
		// Token: 0x06000DEC RID: 3564 RVA: 0x0011C1DC File Offset: 0x0011A3DC
		public ClickableTextureComponent(string name, Rectangle bounds, string label, string hoverText, Texture2D texture, Rectangle sourceRect, float scale, bool drawShadow = false) : base(bounds, name, label)
		{
			this.texture = texture;
			if (sourceRect.Equals(Rectangle.Empty) && texture != null)
			{
				this.sourceRect = texture.Bounds;
			}
			else
			{
				this.sourceRect = sourceRect;
			}
			this.scale = scale;
			this.baseScale = scale;
			this.hoverText = hoverText;
			this.drawShadow = drawShadow;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0011C250 File Offset: 0x0011A450
		public ClickableTextureComponent(Rectangle bounds, Texture2D texture, Rectangle sourceRect, float scale, bool drawShadow = false) : this("", bounds, "", "", texture, sourceRect, scale, drawShadow)
		{
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0011C279 File Offset: 0x0011A479
		public Vector2 getVector2()
		{
			return new Vector2((float)this.bounds.X, (float)this.bounds.Y);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0011C298 File Offset: 0x0011A498
		public void tryHover(int x, int y, float maxScaleIncrease = 0.1f)
		{
			if (this.bounds.Contains(x, y))
			{
				this.scale = Math.Min(this.scale + 0.04f, this.baseScale + maxScaleIncrease);
				return;
			}
			this.scale = Math.Max(this.scale - 0.04f, this.baseScale);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0011C2F1 File Offset: 0x0011A4F1
		public void draw(SpriteBatch b)
		{
			if (this.visible)
			{
				this.draw(b, Color.White, 0.86f + (float)this.bounds.Y / 20000f);
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0011C320 File Offset: 0x0011A520
		public void draw(SpriteBatch b, Color c, float layerDepth)
		{
			if (this.visible)
			{
				if (this.drawShadow)
				{
					Utility.drawWithShadow(b, this.texture, new Vector2((float)this.bounds.X + (float)(this.sourceRect.Width / 2) * this.baseScale, (float)this.bounds.Y + (float)(this.sourceRect.Height / 2) * this.baseScale), this.sourceRect, c, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, false, layerDepth, -1, -1, 0.35f);
				}
				else
				{
					b.Draw(this.texture, new Vector2((float)this.bounds.X + (float)(this.sourceRect.Width / 2) * this.baseScale, (float)this.bounds.Y + (float)(this.sourceRect.Height / 2) * this.baseScale), new Rectangle?(this.sourceRect), c, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, SpriteEffects.None, layerDepth);
				}
				if (this.label != null && this.label != "")
				{
					b.DrawString(Game1.smallFont, this.label, new Vector2((float)(this.bounds.X + this.bounds.Width), (float)this.bounds.Y + ((float)(this.bounds.Height / 2) - Game1.smallFont.MeasureString(this.label).Y / 2f)), Game1.textColor);
				}
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0011C4EC File Offset: 0x0011A6EC
		public void drawItem(SpriteBatch b, int xOffset = 0, int yOffset = 0)
		{
			if (this.item != null && this.visible)
			{
				this.item.drawInMenu(b, new Vector2((float)(this.bounds.X + xOffset), (float)(this.bounds.Y + yOffset)), this.scale / (float)Game1.pixelZoom);
			}
		}

		// Token: 0x04000ECE RID: 3790
		public Texture2D texture;

		// Token: 0x04000ECF RID: 3791
		public Rectangle sourceRect;

		// Token: 0x04000ED0 RID: 3792
		public float baseScale;

		// Token: 0x04000ED1 RID: 3793
		public string hoverText = "";

		// Token: 0x04000ED2 RID: 3794
		public bool drawShadow;
	}
}
