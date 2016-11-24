using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200015B RID: 347
	public class SpriteText
	{
		// Token: 0x06001327 RID: 4903 RVA: 0x00181E00 File Offset: 0x00180000
		public static void drawStringHorizontallyCenteredAt(SpriteBatch b, string s, int x, int y, int characterPosition = 999999, int width = -1, int height = 999999, float alpha = 1f, float layerDepth = 0.88f, bool junimoText = false, int color = -1)
		{
			SpriteText.drawString(b, s, x - SpriteText.getWidthOfString(s) / 2, y, characterPosition, width, height, alpha, layerDepth, junimoText, -1, "", color);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00181E34 File Offset: 0x00180034
		public static int getWidthOfString(string s)
		{
			int width = 0;
			int maxWidth = 0;
			for (int i = 0; i < s.Length; i++)
			{
				width += 8 + SpriteText.getWidthOffsetForChar(s[i]);
				maxWidth = Math.Max(width, maxWidth);
				if (s[i] == '^')
				{
					width = 0;
				}
			}
			return maxWidth * SpriteText.fontPixelZoom;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00181E84 File Offset: 0x00180084
		public static int getHeightOfString(string s, int widthConstraint = 999999)
		{
			if (s.Length == 0)
			{
				return 0;
			}
			Vector2 position = default(Vector2);
			int accumulatedHorizontalSpaceBetweenCharacters = 0;
			s = s.Replace(Environment.NewLine, "");
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '^')
				{
					position.Y += (float)(18 * SpriteText.fontPixelZoom);
					position.X = 0f;
					accumulatedHorizontalSpaceBetweenCharacters = 0;
				}
				else
				{
					if (i > 0)
					{
						position.X += (float)(8 * SpriteText.fontPixelZoom + accumulatedHorizontalSpaceBetweenCharacters + (SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[i - 1])) * SpriteText.fontPixelZoom);
					}
					accumulatedHorizontalSpaceBetweenCharacters = 0;
					if (SpriteText.positionOfNextSpace(s, i, (int)position.X, accumulatedHorizontalSpaceBetweenCharacters) >= widthConstraint)
					{
						position.Y += (float)(18 * SpriteText.fontPixelZoom);
						accumulatedHorizontalSpaceBetweenCharacters = 0;
						position.X = 0f;
					}
				}
			}
			return (int)position.Y + 16 * SpriteText.fontPixelZoom;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00181F80 File Offset: 0x00180180
		public static Color getColorFromIndex(int index)
		{
			switch (index)
			{
			case -1:
			case 4:
				return Color.White;
			case 1:
				return Color.SkyBlue;
			case 2:
				return Color.Red;
			case 3:
				return new Color(110, 43, 255);
			case 5:
				return Color.OrangeRed;
			case 6:
				return Color.LimeGreen;
			case 7:
				return Color.Cyan;
			}
			return Color.Black;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00181FF4 File Offset: 0x001801F4
		public static string getSubstringBeyondHeight(string s, int width, int height)
		{
			Vector2 position = default(Vector2);
			int accumulatedHorizontalSpaceBetweenCharacters = 0;
			s = s.Replace(Environment.NewLine, "");
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '^')
				{
					position.Y += (float)(18 * SpriteText.fontPixelZoom);
					position.X = 0f;
					accumulatedHorizontalSpaceBetweenCharacters = 0;
				}
				else
				{
					if (i > 0)
					{
						position.X += (float)(8 * SpriteText.fontPixelZoom + accumulatedHorizontalSpaceBetweenCharacters + (SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[i - 1])) * SpriteText.fontPixelZoom);
					}
					accumulatedHorizontalSpaceBetweenCharacters = 0;
					if (SpriteText.positionOfNextSpace(s, i, (int)position.X, accumulatedHorizontalSpaceBetweenCharacters) >= width)
					{
						position.Y += (float)(18 * SpriteText.fontPixelZoom);
						accumulatedHorizontalSpaceBetweenCharacters = 0;
						position.X = 0f;
					}
					if (position.Y >= (float)(height - 16 * SpriteText.fontPixelZoom * 2))
					{
						return s.Substring(SpriteText.getLastSpace(s, i));
					}
				}
			}
			return "";
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00182100 File Offset: 0x00180300
		public static int getIndexOfSubstringBeyondHeight(string s, int width, int height)
		{
			Vector2 position = default(Vector2);
			int accumulatedHorizontalSpaceBetweenCharacters = 0;
			s = s.Replace(Environment.NewLine, "");
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '^')
				{
					position.Y += (float)(18 * SpriteText.fontPixelZoom);
					position.X = 0f;
					accumulatedHorizontalSpaceBetweenCharacters = 0;
				}
				else
				{
					if (i > 0)
					{
						position.X += (float)(8 * SpriteText.fontPixelZoom + accumulatedHorizontalSpaceBetweenCharacters + (SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[i - 1])) * SpriteText.fontPixelZoom);
					}
					accumulatedHorizontalSpaceBetweenCharacters = 0;
					if (SpriteText.positionOfNextSpace(s, i, (int)position.X, accumulatedHorizontalSpaceBetweenCharacters) >= width)
					{
						position.Y += (float)(18 * SpriteText.fontPixelZoom);
						accumulatedHorizontalSpaceBetweenCharacters = 0;
						position.X = 0f;
					}
					if (position.Y >= (float)(height - 16 * SpriteText.fontPixelZoom))
					{
						return i - 1;
					}
				}
			}
			return s.Length - 1;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00182204 File Offset: 0x00180404
		public static List<string> getStringBrokenIntoSectionsOfHeight(string s, int width, int height)
		{
			List<string> brokenUp = new List<string>();
			while (s.Length > 0)
			{
				string tmp = SpriteText.getStringPreviousToThisHeightCutoff(s, width, height);
				if (tmp.Length <= 0)
				{
					break;
				}
				brokenUp.Add(tmp);
				s = s.Substring(brokenUp.Last<string>().Length);
			}
			return brokenUp;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0018224F File Offset: 0x0018044F
		public static string getStringPreviousToThisHeightCutoff(string s, int width, int height)
		{
			return s.Substring(0, SpriteText.getIndexOfSubstringBeyondHeight(s, width, height) + 1);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00182264 File Offset: 0x00180464
		private static int getLastSpace(string s, int startIndex)
		{
			for (int i = startIndex; i >= 0; i--)
			{
				if (s[i] == ' ')
				{
					return i;
				}
			}
			return startIndex;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0018228C File Offset: 0x0018048C
		public static int getWidthOffsetForChar(char c)
		{
			if (c <= ',')
			{
				if (c == '!')
				{
					return -1;
				}
				if (c == '$')
				{
					return 1;
				}
				if (c != ',')
				{
					return 0;
				}
			}
			else if (c != '.')
			{
				if (c == '^')
				{
					return -8;
				}
				switch (c)
				{
				case 'i':
					return -2;
				case 'j':
				case 'l':
					return -1;
				case 'k':
					return 0;
				default:
					return 0;
				}
			}
			return -2;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x001822E4 File Offset: 0x001804E4
		public static void drawStringWithScrollCenteredAt(SpriteBatch b, string s, int x, int y, string placeHolderWidthText = "", float alpha = 1f, int color = -1, int scrollType = 0, float layerDepth = 0.88f, bool junimoText = false)
		{
			SpriteText.drawString(b, s, x - SpriteText.getWidthOfString((placeHolderWidthText.Length > 0) ? placeHolderWidthText : s) / 2, y, 999999, -1, 999999, alpha, layerDepth, junimoText, scrollType, placeHolderWidthText, color);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00182328 File Offset: 0x00180528
		public static void drawStringWithScrollBackground(SpriteBatch b, string s, int x, int y, string placeHolderWidthText = "", float alpha = 1f, int color = -1)
		{
			SpriteText.drawString(b, s, x, y, 999999, -1, 999999, alpha, 0.88f, false, 0, placeHolderWidthText, color);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00182358 File Offset: 0x00180558
		public static void drawString(SpriteBatch b, string s, int x, int y, int characterPosition = 999999, int width = -1, int height = 999999, float alpha = 1f, float layerDepth = 0.88f, bool junimoText = false, int drawBGScroll = -1, string placeHolderScrollWidthText = "", int color = -1)
		{
			if (width == -1)
			{
				width = Game1.graphics.GraphicsDevice.Viewport.Width - x;
				if (drawBGScroll == 1)
				{
					width = SpriteText.getWidthOfString(s) * 2;
				}
			}
			if (SpriteText.fontPixelZoom < 4)
			{
				y += (4 - SpriteText.fontPixelZoom) * Game1.pixelZoom;
			}
			Vector2 position = new Vector2((float)x, (float)y);
			int accumulatedHorizontalSpaceBetweenCharacters = 0;
			if (drawBGScroll != 1)
			{
				if (position.X + (float)width > (float)(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.pixelZoom))
				{
					position.X = (float)(Game1.graphics.GraphicsDevice.Viewport.Width - width - Game1.pixelZoom);
				}
				if (position.X < 0f)
				{
					position.X = 0f;
				}
			}
			if (drawBGScroll == 0)
			{
				b.Draw(Game1.mouseCursors, position + new Vector2(-12f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(325, 318, 12, 18)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2(0f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(337, 318, 1, 18)), Color.White * alpha, 0f, Vector2.Zero, new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)Game1.pixelZoom), SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)(-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(338, 318, 12, 18)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				if (placeHolderScrollWidthText.Length > 0)
				{
					x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
					position.X = (float)x;
				}
				position.Y += (float)((4 - SpriteText.fontPixelZoom) * Game1.pixelZoom);
			}
			else if (drawBGScroll == 1)
			{
				b.Draw(Game1.mouseCursors, position + new Vector2(-7f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(324, 299, 7, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2(0f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(331, 299, 1, 17)), Color.White * alpha, 0f, Vector2.Zero, new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)Game1.pixelZoom), SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2((float)SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s), (float)(-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(332, 299, 7, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2((float)(SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s) / 2), (float)(13 * Game1.pixelZoom)), new Rectangle?(new Rectangle(341, 308, 6, 5)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.0001f);
				if (placeHolderScrollWidthText.Length > 0)
				{
					x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
					position.X = (float)x;
				}
				position.Y += (float)((4 - SpriteText.fontPixelZoom) * Game1.pixelZoom);
			}
			else if (drawBGScroll == 2)
			{
				b.Draw(Game1.mouseCursors, position + new Vector2(-3f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(327, 281, 3, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2(0f, -3f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(330, 281, 1, 17)), Color.White * alpha, 0f, Vector2.Zero, new Vector2((float)(SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s) + Game1.pixelZoom), (float)Game1.pixelZoom), SpriteEffects.None, layerDepth - 0.001f);
				b.Draw(Game1.mouseCursors, position + new Vector2((float)(SpriteText.getWidthOfString((placeHolderScrollWidthText.Length > 0) ? placeHolderScrollWidthText : s) + Game1.pixelZoom), (float)(-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(333, 281, 3, 17)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.001f);
				if (placeHolderScrollWidthText.Length > 0)
				{
					x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
					position.X = (float)x;
				}
				position.Y += (float)((4 - SpriteText.fontPixelZoom) * Game1.pixelZoom);
			}
			s = s.Replace(Environment.NewLine, "");
			for (int i = 0; i < Math.Min(s.Length, characterPosition); i++)
			{
				if (s[i] == '^')
				{
					position.Y += (float)(18 * SpriteText.fontPixelZoom);
					position.X = (float)x;
					accumulatedHorizontalSpaceBetweenCharacters = 0;
				}
				else
				{
					if (i > 0)
					{
						position.X += (float)(8 * SpriteText.fontPixelZoom + accumulatedHorizontalSpaceBetweenCharacters + (SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[i - 1])) * SpriteText.fontPixelZoom);
					}
					accumulatedHorizontalSpaceBetweenCharacters = 0;
					if (SpriteText.positionOfNextSpace(s, i, (int)position.X, accumulatedHorizontalSpaceBetweenCharacters) >= x + width - Game1.pixelZoom)
					{
						position.Y += (float)(18 * SpriteText.fontPixelZoom);
						accumulatedHorizontalSpaceBetweenCharacters = 0;
						position.X = (float)x;
					}
					b.Draw((color != -1) ? SpriteText.coloredTexture : SpriteText.spriteTexture, position, new Rectangle?(SpriteText.getSourceRectForChar(s[i], junimoText)), SpriteText.getColorFromIndex(color) * alpha, 0f, Vector2.Zero, (float)SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
				}
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00182ABC File Offset: 0x00180CBC
		public static int positionOfNextSpace(string s, int index, int currentXPosition, int accumulatedHorizontalSpaceBetweenCharacters)
		{
			for (int i = index; i < s.Length; i++)
			{
				if (s[i] == ' ')
				{
					return currentXPosition;
				}
				currentXPosition += 8 * SpriteText.fontPixelZoom + accumulatedHorizontalSpaceBetweenCharacters + (SpriteText.getWidthOffsetForChar(s[i]) + SpriteText.getWidthOffsetForChar(s[Math.Max(0, i - 1)])) * SpriteText.fontPixelZoom;
				accumulatedHorizontalSpaceBetweenCharacters = 0;
			}
			return currentXPosition;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00182B20 File Offset: 0x00180D20
		private static Rectangle getSourceRectForChar(char c, bool junimoText)
		{
			int i = (int)(c - ' ');
			return new Rectangle(i * 8 % SpriteText.spriteTexture.Width, i * 8 / SpriteText.spriteTexture.Width * 16 + (junimoText ? 96 : 0), 8, 16);
		}

		// Token: 0x040013AD RID: 5037
		public const int scrollStyle_scroll = 0;

		// Token: 0x040013AE RID: 5038
		public const int scrollStyle_speechBubble = 1;

		// Token: 0x040013AF RID: 5039
		public const int scrollStyle_darkMetal = 2;

		// Token: 0x040013B0 RID: 5040
		public const int maxCharacter = 999999;

		// Token: 0x040013B1 RID: 5041
		public const int maxHeight = 999999;

		// Token: 0x040013B2 RID: 5042
		public const int characterWidth = 8;

		// Token: 0x040013B3 RID: 5043
		public const int characterHeight = 16;

		// Token: 0x040013B4 RID: 5044
		public const int horizontalSpaceBetweenCharacters = 0;

		// Token: 0x040013B5 RID: 5045
		public const int verticalSpaceBetweenCharacters = 2;

		// Token: 0x040013B6 RID: 5046
		public const char newLine = '^';

		// Token: 0x040013B7 RID: 5047
		public static int fontPixelZoom = 3;

		// Token: 0x040013B8 RID: 5048
		public static Texture2D spriteTexture = Game1.content.Load<Texture2D>("LooseSprites\\font_bold");

		// Token: 0x040013B9 RID: 5049
		public static Texture2D coloredTexture = Game1.content.Load<Texture2D>("LooseSprites\\font_colored");

		// Token: 0x040013BA RID: 5050
		public const int color_Black = 0;

		// Token: 0x040013BB RID: 5051
		public const int color_Blue = 1;

		// Token: 0x040013BC RID: 5052
		public const int color_Red = 2;

		// Token: 0x040013BD RID: 5053
		public const int color_Purple = 3;

		// Token: 0x040013BE RID: 5054
		public const int color_White = 4;

		// Token: 0x040013BF RID: 5055
		public const int color_Orange = 5;

		// Token: 0x040013C0 RID: 5056
		public const int color_Green = 6;

		// Token: 0x040013C1 RID: 5057
		public const int color_Cyan = 7;
	}
}
