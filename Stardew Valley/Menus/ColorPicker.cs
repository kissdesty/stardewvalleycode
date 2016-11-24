using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000E5 RID: 229
	public class ColorPicker
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x0011D760 File Offset: 0x0011B960
		public ColorPicker(int x, int y)
		{
			this.hueBar = new SliderBar(0, 0, 50);
			this.saturationBar = new SliderBar(0, 20, 50);
			this.valueBar = new SliderBar(0, 40, 50);
			this.bounds = new Rectangle(x, y, SliderBar.defaultWidth, 60);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0011D7B8 File Offset: 0x0011B9B8
		public Color getSelectedColor()
		{
			return this.HsvToRgb((double)this.hueBar.value / 100.0 * 360.0, (double)this.saturationBar.value / 100.0, (double)this.valueBar.value / 100.0);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0011D818 File Offset: 0x0011BA18
		public Color click(int x, int y)
		{
			if (this.bounds.Contains(x, y))
			{
				x -= this.bounds.X;
				y -= this.bounds.Y;
				if (this.hueBar.bounds.Contains(x, y))
				{
					this.hueBar.click(x, y);
					this.recentSliderBar = this.hueBar;
				}
				if (this.saturationBar.bounds.Contains(x, y))
				{
					this.recentSliderBar = this.saturationBar;
					this.saturationBar.click(x, y);
				}
				if (this.valueBar.bounds.Contains(x, y))
				{
					this.recentSliderBar = this.valueBar;
					this.valueBar.click(x, y);
				}
			}
			return this.getSelectedColor();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0011D8E8 File Offset: 0x0011BAE8
		public Color clickHeld(int x, int y)
		{
			if (this.recentSliderBar != null)
			{
				x = Math.Max(x, this.bounds.X);
				x = Math.Min(x, this.bounds.Right - 1);
				y = this.recentSliderBar.bounds.Center.Y;
				x -= this.bounds.X;
				if (this.recentSliderBar.Equals(this.hueBar))
				{
					this.hueBar.click(x, y);
				}
				if (this.recentSliderBar.Equals(this.saturationBar))
				{
					this.saturationBar.click(x, y);
				}
				if (this.recentSliderBar.Equals(this.valueBar))
				{
					this.valueBar.click(x, y);
				}
			}
			return this.getSelectedColor();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0011D9B7 File Offset: 0x0011BBB7
		public void releaseClick()
		{
			this.hueBar.release(0, 0);
			this.saturationBar.release(0, 0);
			this.valueBar.release(0, 0);
			this.recentSliderBar = null;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0011D9E8 File Offset: 0x0011BBE8
		public void draw(SpriteBatch b)
		{
			for (int i = 0; i < 24; i++)
			{
				Color c = this.HsvToRgb((double)i / 24.0 * 360.0, 0.9, 0.9);
				b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * i, this.bounds.Y + this.hueBar.bounds.Center.Y - 2, this.hueBar.bounds.Width / 24, 4), c);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.hueBar.value / 100f * (float)this.hueBar.bounds.Width)), (float)(this.bounds.Y + this.hueBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
			Utility.drawTextWithShadow(b, string.Concat(this.hueBar.value), Game1.smallFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + this.hueBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			for (int j = 0; j < 24; j++)
			{
				Color c2 = this.HsvToRgb((double)this.hueBar.value / 100.0 * 360.0, (double)j / 24.0, (double)this.valueBar.value / 100.0);
				b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * j, this.bounds.Y + this.saturationBar.bounds.Center.Y - 2, this.saturationBar.bounds.Width / 24, 4), c2);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.saturationBar.value / 100f * (float)this.saturationBar.bounds.Width)), (float)(this.bounds.Y + this.saturationBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.87f);
			Utility.drawTextWithShadow(b, string.Concat(this.saturationBar.value), Game1.smallFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + this.saturationBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			for (int k = 0; k < 24; k++)
			{
				Color c3 = this.HsvToRgb((double)this.hueBar.value / 100.0 * 360.0, (double)this.saturationBar.value / 100.0, (double)k / 24.0);
				b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * k, this.bounds.Y + this.valueBar.bounds.Center.Y - 2, this.valueBar.bounds.Width / 24, 4), c3);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.bounds.X + (int)((float)this.valueBar.value / 100f * (float)this.valueBar.bounds.Width)), (float)(this.bounds.Y + this.valueBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
			Utility.drawTextWithShadow(b, string.Concat(this.valueBar.value), Game1.smallFont, new Vector2((float)(this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float)(this.bounds.Y + this.valueBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0011DF65 File Offset: 0x0011C165
		public bool containsPoint(int x, int y)
		{
			return this.bounds.Contains(x, y);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0011DF74 File Offset: 0x0011C174
		public void setColor(Color color)
		{
			float hue;
			float sat;
			float val;
			this.RGBtoHSV((float)color.R, (float)color.G, (float)color.B, out hue, out sat, out val);
			this.hueBar.value = (int)(hue / 360f * 100f);
			this.saturationBar.value = (int)(sat * 100f);
			this.valueBar.value = (int)(val / 255f * 100f);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0011DFEC File Offset: 0x0011C1EC
		private void RGBtoHSV(float r, float g, float b, out float h, out float s, out float v)
		{
			float min = Math.Min(Math.Min(r, g), b);
			float max = Math.Max(Math.Max(r, g), b);
			v = max;
			float delta = max - min;
			if (max != 0f)
			{
				s = delta / max;
				if (r == max)
				{
					h = (g - b) / delta;
				}
				else if (g == max)
				{
					h = 2f + (b - r) / delta;
				}
				else
				{
					h = 4f + (r - g) / delta;
				}
				h *= 60f;
				if (h < 0f)
				{
					h += 360f;
				}
				return;
			}
			s = 0f;
			h = -1f;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0011E090 File Offset: 0x0011C290
		private Color HsvToRgb(double h, double S, double V)
		{
			double H = h;
			while (H < 0.0)
			{
				H += 1.0;
				if (H < -1000000.0)
				{
					H = 0.0;
				}
			}
			while (H >= 360.0)
			{
				H -= 1.0;
			}
			double B;
			double R;
			double G;
			if (V <= 0.0)
			{
				G = (R = (B = 0.0));
			}
			else if (S <= 0.0)
			{
				B = V;
				G = V;
				R = V;
			}
			else
			{
				double expr_8D = H / 60.0;
				int i = (int)Math.Floor(expr_8D);
				double f = expr_8D - (double)i;
				double pv = V * (1.0 - S);
				double qv = V * (1.0 - S * f);
				double tv = V * (1.0 - S * (1.0 - f));
				switch (i)
				{
				case -1:
					R = V;
					G = pv;
					B = qv;
					break;
				case 0:
					R = V;
					G = tv;
					B = pv;
					break;
				case 1:
					R = qv;
					G = V;
					B = pv;
					break;
				case 2:
					R = pv;
					G = V;
					B = tv;
					break;
				case 3:
					R = pv;
					G = qv;
					B = V;
					break;
				case 4:
					R = tv;
					G = pv;
					B = V;
					break;
				case 5:
					R = V;
					G = pv;
					B = qv;
					break;
				case 6:
					R = V;
					G = tv;
					B = pv;
					break;
				default:
					B = V;
					G = V;
					R = V;
					break;
				}
			}
			return new Color(this.Clamp((int)(R * 255.0)), this.Clamp((int)(G * 255.0)), this.Clamp((int)(B * 255.0)));
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0011E232 File Offset: 0x0011C432
		private int Clamp(int i)
		{
			if (i < 0)
			{
				return 0;
			}
			if (i > 255)
			{
				return 255;
			}
			return i;
		}

		// Token: 0x04000EE4 RID: 3812
		public const int sliderChunks = 24;

		// Token: 0x04000EE5 RID: 3813
		private int colorIndexSelection;

		// Token: 0x04000EE6 RID: 3814
		private Rectangle bounds;

		// Token: 0x04000EE7 RID: 3815
		public SliderBar hueBar;

		// Token: 0x04000EE8 RID: 3816
		public SliderBar valueBar;

		// Token: 0x04000EE9 RID: 3817
		public SliderBar saturationBar;

		// Token: 0x04000EEA RID: 3818
		public SliderBar recentSliderBar;
	}
}
