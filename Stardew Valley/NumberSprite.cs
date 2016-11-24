using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200002B RID: 43
	public class NumberSprite
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00032FD0 File Offset: 0x000311D0
		public static void draw(int number, SpriteBatch b, Vector2 position, Color c, float scale, float layerDepth, float alpha, int secondDigitOffset, int spaceBetweenDigits = 0)
		{
			int digit = 1;
			secondDigitOffset = Math.Min(0, secondDigitOffset);
			do
			{
				int currentDigit = number % 10;
				number /= 10;
				int textX = 512 + currentDigit * 8 % 48;
				int textY = 128 + currentDigit * 8 / 48 * 8;
				b.Draw(Game1.mouseCursors, position + new Vector2(0f, (float)((digit == 2) ? secondDigitOffset : 0)), new Rectangle?(new Rectangle(textX, textY, 8, 8)), c * alpha, 0f, new Vector2(4f, 4f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth);
				position.X -= 8f * scale * (float)Game1.pixelZoom - 4f - (float)spaceBetweenDigits;
				digit++;
			}
			while (number > 0);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0003309A File Offset: 0x0003129A
		public static int getHeight()
		{
			return 8;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0003309D File Offset: 0x0003129D
		public static int getWidth(string number)
		{
			return NumberSprite.getWidth(Convert.ToInt32(number));
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000330AC File Offset: 0x000312AC
		public static int getWidth(int number)
		{
			int width = 8;
			number /= 10;
			while (number != 0)
			{
				number /= 10;
				width += 8;
			}
			return width;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000330D4 File Offset: 0x000312D4
		public static int numberOfDigits(int number)
		{
			int num = 1;
			number /= 10;
			while (number != 0)
			{
				number /= 10;
				num++;
			}
			return num;
		}

		// Token: 0x04000277 RID: 631
		public const int textureX = 512;

		// Token: 0x04000278 RID: 632
		public const int textureY = 128;

		// Token: 0x04000279 RID: 633
		public const int digitWidth = 8;

		// Token: 0x0400027A RID: 634
		public const int digitHeight = 8;

		// Token: 0x0400027B RID: 635
		public const int groupWidth = 48;
	}
}
