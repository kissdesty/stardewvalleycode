using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200015E RID: 350
	public class SparklingText
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x001834B8 File Offset: 0x001816B8
		public SparklingText(SpriteFont font, string text, Color color, Color sparkleColor, bool rainbow = false, double sparkleFrequency = 0.1, int millisecondsDuration = 2500, int amplitude = -1, int speed = 500)
		{
			if (amplitude == -1)
			{
				amplitude = Game1.tileSize;
			}
			SparklingText.maxDistanceForSparkle = Game1.tileSize / 2;
			this.font = font;
			this.color = color;
			this.sparkleColor = sparkleColor;
			this.text = text;
			this.rainbow = rainbow;
			if (rainbow)
			{
				color = Color.Yellow;
			}
			this.sparkleFrequency = sparkleFrequency;
			this.millisecondsDuration = millisecondsDuration;
			this.individualCharacterOffsets = new float[text.Length];
			this.amplitude = amplitude;
			this.period = speed;
			this.sparkles = new List<TemporaryAnimatedSprite>();
			this.boundingBox = new Rectangle(-SparklingText.maxDistanceForSparkle, -SparklingText.maxDistanceForSparkle, (int)font.MeasureString(text).X + SparklingText.maxDistanceForSparkle * 2, (int)font.MeasureString(text).Y + SparklingText.maxDistanceForSparkle * 2);
			this.sparkleTrash = new List<Vector2>();
			this.textWidth = font.MeasureString(text).X;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x001835C4 File Offset: 0x001817C4
		public bool update(GameTime time)
		{
			this.millisecondsDuration -= time.ElapsedGameTime.Milliseconds;
			this.offsetDecay -= 0.001f;
			this.amplitude = (int)((float)this.amplitude * this.offsetDecay);
			if (this.millisecondsDuration <= 500)
			{
				this.alpha = (float)this.millisecondsDuration / 500f;
			}
			for (int i = 0; i < this.individualCharacterOffsets.Length; i++)
			{
				this.individualCharacterOffsets[i] = (float)((double)(this.amplitude / 2) * Math.Sin(6.2831853071795862 / (double)this.period * (double)(this.millisecondsDuration - i * 100)));
			}
			if (this.millisecondsDuration > 500 && Game1.random.NextDouble() < this.sparkleFrequency)
			{
				int speed = Game1.random.Next(100, 600);
				this.sparkles.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 704, 64, 64), (float)(speed / 6), 6, 0, new Vector2((float)Game1.random.Next(this.boundingBox.X, this.boundingBox.Right), (float)Game1.random.Next(this.boundingBox.Y, this.boundingBox.Bottom)), false, false, 1f, 0f, this.rainbow ? this.color : this.sparkleColor, 1f, 0f, 0f, 0f, false));
			}
			for (int j = this.sparkles.Count - 1; j >= 0; j--)
			{
				if (this.sparkles[j].update(time))
				{
					this.sparkles.RemoveAt(j);
				}
			}
			if (this.rainbow)
			{
				this.incrementRainbowColors();
			}
			return this.millisecondsDuration <= 0;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x001837B0 File Offset: 0x001819B0
		private void incrementRainbowColors()
		{
			if (this.colorCycle == 0)
			{
				if ((this.color.G = this.color.G + 4) >= 255)
				{
					this.colorCycle = 1;
					return;
				}
				if (this.colorCycle == 1)
				{
					if ((this.color.R = this.color.R - 4) <= 0)
					{
						this.colorCycle = 2;
						return;
					}
					if (this.colorCycle == 2)
					{
						if ((this.color.B = this.color.B + 4) >= 255)
						{
							this.colorCycle = 3;
							return;
						}
						if (this.colorCycle == 3)
						{
							if ((this.color.G = this.color.G - 4) <= 0)
							{
								this.colorCycle = 4;
								return;
							}
							if (this.colorCycle == 4)
							{
								if ((this.color.R = this.color.R + 1) >= 255)
								{
									this.colorCycle = 5;
									return;
								}
								if (this.colorCycle == 5 && (this.color.B = this.color.B - 4) <= 0)
								{
									this.colorCycle = 0;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x001838D4 File Offset: 0x00181AD4
		private static Color getRainbowColorFromIndex(int index)
		{
			switch (index % 8)
			{
			case 0:
				return Color.Red;
			case 1:
				return Color.Orange;
			case 2:
				return Color.Yellow;
			case 3:
				return Color.Chartreuse;
			case 4:
				return Color.Green;
			case 5:
				return Color.Cyan;
			case 6:
				return Color.Blue;
			case 7:
				return Color.Violet;
			default:
				return Color.White;
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00183944 File Offset: 0x00181B44
		public void draw(SpriteBatch b, Vector2 onScreenPosition)
		{
			int xOffset = 0;
			for (int i = 0; i < this.text.Length; i++)
			{
				b.DrawString(this.font, this.text[i].ToString() ?? "", onScreenPosition + new Vector2((float)(xOffset - 2), this.individualCharacterOffsets[i]), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
				b.DrawString(this.font, this.text[i].ToString() ?? "", onScreenPosition + new Vector2((float)(xOffset + 2), this.individualCharacterOffsets[i]), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.991f);
				b.DrawString(this.font, this.text[i].ToString() ?? "", onScreenPosition + new Vector2((float)xOffset, this.individualCharacterOffsets[i] - 2f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.992f);
				b.DrawString(this.font, this.text[i].ToString() ?? "", onScreenPosition + new Vector2((float)xOffset, this.individualCharacterOffsets[i] + 2f), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.993f);
				b.DrawString(this.font, this.text[i].ToString() ?? "", onScreenPosition + new Vector2((float)xOffset, this.individualCharacterOffsets[i]), this.rainbow ? SparklingText.getRainbowColorFromIndex(i) : (this.color * this.alpha), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				xOffset += (int)this.font.MeasureString(this.text[i].ToString() ?? "").X;
			}
			foreach (TemporaryAnimatedSprite expr_248 in this.sparkles)
			{
				expr_248.Position += onScreenPosition;
				expr_248.draw(b, true, 0, 0);
				expr_248.Position -= onScreenPosition;
			}
		}

		// Token: 0x040013D8 RID: 5080
		public static int maxDistanceForSparkle = Game1.tileSize / 2;

		// Token: 0x040013D9 RID: 5081
		private SpriteFont font;

		// Token: 0x040013DA RID: 5082
		private Color color;

		// Token: 0x040013DB RID: 5083
		private Color sparkleColor;

		// Token: 0x040013DC RID: 5084
		private bool rainbow;

		// Token: 0x040013DD RID: 5085
		private int millisecondsDuration;

		// Token: 0x040013DE RID: 5086
		private int amplitude;

		// Token: 0x040013DF RID: 5087
		private int period;

		// Token: 0x040013E0 RID: 5088
		private int colorCycle;

		// Token: 0x040013E1 RID: 5089
		public string text;

		// Token: 0x040013E2 RID: 5090
		private float[] individualCharacterOffsets;

		// Token: 0x040013E3 RID: 5091
		public float offsetDecay = 1f;

		// Token: 0x040013E4 RID: 5092
		public float alpha = 1f;

		// Token: 0x040013E5 RID: 5093
		public float textWidth;

		// Token: 0x040013E6 RID: 5094
		private double sparkleFrequency;

		// Token: 0x040013E7 RID: 5095
		private List<TemporaryAnimatedSprite> sparkles;

		// Token: 0x040013E8 RID: 5096
		private List<Vector2> sparkleTrash;

		// Token: 0x040013E9 RID: 5097
		private Rectangle boundingBox;
	}
}
