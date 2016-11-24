using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000039 RID: 57
	public class NameSelect
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x0003AB64 File Offset: 0x00038D64
		public static void load()
		{
			NameSelect.namingCharacters = new List<char>();
			for (int i = 0; i < 25; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					NameSelect.namingCharacters.Add((char)(97 + i + j));
				}
				for (int k = 0; k < 5; k++)
				{
					NameSelect.namingCharacters.Add((char)(65 + i + k));
				}
				if (i < 10)
				{
					for (int l = 0; l < 5; l++)
					{
						NameSelect.namingCharacters.Add((char)(48 + i + l));
					}
				}
				else if (i < 15)
				{
					NameSelect.namingCharacters.Add('?');
					NameSelect.namingCharacters.Add('$');
					NameSelect.namingCharacters.Add('\'');
					NameSelect.namingCharacters.Add('#');
					NameSelect.namingCharacters.Add('[');
				}
				else if (i < 20)
				{
					NameSelect.namingCharacters.Add('-');
					NameSelect.namingCharacters.Add('=');
					NameSelect.namingCharacters.Add('~');
					NameSelect.namingCharacters.Add('&');
					NameSelect.namingCharacters.Add('!');
				}
				else
				{
					NameSelect.namingCharacters.Add('Z');
					NameSelect.namingCharacters.Add('z');
					NameSelect.namingCharacters.Add('<');
					NameSelect.namingCharacters.Add('"');
					NameSelect.namingCharacters.Add(']');
				}
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0003ACB4 File Offset: 0x00038EB4
		public static void draw()
		{
			int width = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width % Game1.tileSize, Game1.graphics.GraphicsDevice.Viewport.Width - Game1.graphics.GraphicsDevice.Viewport.Width % Game1.tileSize - Game1.tileSize * 2);
			int height = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height % Game1.tileSize, Game1.graphics.GraphicsDevice.Viewport.Height - Game1.graphics.GraphicsDevice.Viewport.Height % Game1.tileSize - Game1.tileSize);
			int xLocation = Game1.graphics.GraphicsDevice.Viewport.Width / 2 - width / 2;
			int yLocation = Game1.graphics.GraphicsDevice.Viewport.Height / 2 - height / 2;
			int widthBetweenCharacters = (width - Game1.tileSize * 2) / 15;
			int heightBetweenCharacters = (height - Game1.tileSize * 4) / 5;
			Game1.drawDialogueBox(xLocation, yLocation, width, height, false, true, null, false);
			string titleMessage = "";
			string nameSelectType = Game1.nameSelectType;
			if (!(nameSelectType == "samBand"))
			{
				if (nameSelectType == "Animal" || nameSelectType == "playerName" || nameSelectType == "coopDwellerBorn")
				{
					titleMessage = "Name: ";
				}
			}
			else
			{
				titleMessage = "Band Name: ";
			}
			Game1.spriteBatch.DrawString(Game1.dialogueFont, titleMessage, new Vector2((float)(xLocation + 2 * Game1.tileSize), (float)(yLocation + Game1.tileSize * 2)), Game1.textColor);
			int titleMessageWidth = (int)Game1.dialogueFont.MeasureString(titleMessage).X;
			string underline = "";
			for (int i = 0; i < 9; i++)
			{
				if (NameSelect.name.Length > i)
				{
					Game1.spriteBatch.DrawString(Game1.dialogueFont, NameSelect.name[i].ToString() ?? "", new Vector2((float)(xLocation + 2 * Game1.tileSize + titleMessageWidth) + Game1.dialogueFont.MeasureString(underline).X + (Game1.dialogueFont.MeasureString("_").X - Game1.dialogueFont.MeasureString(NameSelect.name[i].ToString() ?? "").X) / 2f - 2f, (float)(yLocation + Game1.tileSize * 2 - Game1.tileSize / 10)), Game1.textColor);
				}
				underline += "_ ";
			}
			Game1.spriteBatch.DrawString(Game1.dialogueFont, "_ _ _ _ _ _ _ _ _", new Vector2((float)(xLocation + 2 * Game1.tileSize + titleMessageWidth), (float)(yLocation + Game1.tileSize * 2)), Game1.textColor);
			Game1.spriteBatch.DrawString(Game1.dialogueFont, "Done", new Vector2((float)(xLocation + width - Game1.tileSize * 3), (float)(yLocation + height - Game1.tileSize * 3 / 2)), Game1.textColor);
			for (int j = 0; j < 5; j++)
			{
				int sectionBuffer = 0;
				for (int k = 0; k < 15; k++)
				{
					if (k != 0 && k % 5 == 0)
					{
						sectionBuffer += widthBetweenCharacters / 3;
					}
					Game1.spriteBatch.DrawString(Game1.dialogueFont, NameSelect.namingCharacters[j * 15 + k].ToString() ?? "", new Vector2((float)(sectionBuffer + xLocation + Game1.tileSize + widthBetweenCharacters * k), (float)(yLocation + Game1.tileSize * 3 + heightBetweenCharacters * j)), Game1.textColor);
					if (NameSelect.selection == j * 15 + k)
					{
						Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float)(sectionBuffer + xLocation + widthBetweenCharacters * k - Game1.tileSize / 10), (float)(yLocation + Game1.tileSize * 3 + heightBetweenCharacters * j - Game1.tileSize / 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 26, -1, -1)), Color.White);
					}
				}
			}
			if (NameSelect.selection == -1)
			{
				Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float)(xLocation + width - Game1.tileSize * 3 - Game1.tileSize - Game1.tileSize / 10), (float)(yLocation + height - Game1.tileSize * 3 / 2 - Game1.tileSize / 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 26, -1, -1)), Color.White);
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0003B19C File Offset: 0x0003939C
		public static bool select()
		{
			if (NameSelect.selection == -1)
			{
				if (NameSelect.name.Length > 0)
				{
					return true;
				}
			}
			else if (NameSelect.name.Length < 9)
			{
				NameSelect.name += NameSelect.namingCharacters[NameSelect.selection].ToString();
				Game1.playSound("smallSelect");
			}
			return false;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0003B1FF File Offset: 0x000393FF
		public static void startButton()
		{
			if (NameSelect.name.Length > 0)
			{
				NameSelect.selection = -1;
				Game1.playSound("smallSelect");
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0003B21E File Offset: 0x0003941E
		public static bool isOnDone()
		{
			return NameSelect.selection == -1;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0003B228 File Offset: 0x00039428
		public static void backspace()
		{
			if (NameSelect.name.Length > 0)
			{
				NameSelect.name = NameSelect.name.Remove(NameSelect.name.Length - 1);
				Game1.playSound("toolSwap");
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0003B25C File Offset: 0x0003945C
		public static bool cancel()
		{
			if ((Game1.nameSelectType.Equals("samBand") || Game1.nameSelectType.Equals("coopDwellerBorn")) && NameSelect.name.Length > 0)
			{
				Game1.playSound("toolSwap");
				NameSelect.name = NameSelect.name.Remove(NameSelect.name.Length - 1);
				return false;
			}
			NameSelect.selection = 0;
			NameSelect.name = "";
			return true;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0003B2D0 File Offset: 0x000394D0
		public static void moveSelection(int direction)
		{
			Game1.playSound("toolSwap");
			if (!direction.Equals(0))
			{
				if (direction.Equals(1))
				{
					NameSelect.selection++;
					if (NameSelect.selection % 15 == 0)
					{
						NameSelect.selection -= 15;
						return;
					}
				}
				else if (direction.Equals(2))
				{
					if (NameSelect.selection >= NameSelect.namingCharacters.Count - 2)
					{
						NameSelect.selection = -1;
						return;
					}
					NameSelect.selection += 15;
					if (NameSelect.selection >= NameSelect.namingCharacters.Count)
					{
						NameSelect.selection -= NameSelect.namingCharacters.Count;
						return;
					}
				}
				else if (direction.Equals(3))
				{
					if (NameSelect.selection % 15 == 0)
					{
						NameSelect.selection += 14;
						return;
					}
					NameSelect.selection--;
				}
				return;
			}
			if (NameSelect.selection == -1)
			{
				NameSelect.selection = NameSelect.namingCharacters.Count - 2;
				return;
			}
			if (NameSelect.selection - 15 < 0)
			{
				NameSelect.selection = NameSelect.namingCharacters.Count - 15 + NameSelect.selection;
				return;
			}
			NameSelect.selection -= 15;
		}

		// Token: 0x04000346 RID: 838
		public const int maxNameLength = 9;

		// Token: 0x04000347 RID: 839
		public const int charactersPerRow = 15;

		// Token: 0x04000348 RID: 840
		public static string name = "";

		// Token: 0x04000349 RID: 841
		private static int selection = 0;

		// Token: 0x0400034A RID: 842
		private static List<char> namingCharacters;
	}
}
