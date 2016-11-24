using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace StardewValley
{
	// Token: 0x0200001E RID: 30
	public class LoadGameScreen : IClickableMenu
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000CC68 File Offset: 0x0000AE68
		public LoadGameScreen() : base(Game1.viewport.Width / 2 - Game1.viewport.Width / 5, (int)((float)Game1.viewport.Height * 0.2f + (float)Game1.tileSize), (int)((float)Game1.viewport.Width * 0.4f), (int)((float)Game1.viewport.Height * 0.2f), false)
		{
			this.downArrow = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width, Game1.viewport.Height - this.height, Game1.tileSize, Game1.tileSize), "Down");
			this.upArrow = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width, this.height + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Up");
			this.slot1 = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height + Game1.tileSize, this.width, this.height), "Slot1");
			this.slot2 = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height * 2 + Game1.tileSize, this.width, this.height), "Slot2");
			this.slot3 = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height * 3 + Game1.tileSize, this.width, this.height), "Slot3");
			string pathToDirectory = Path.Combine(new string[]
			{
				Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves")
			});
			if (Directory.Exists(pathToDirectory))
			{
				string[] directories = Directory.GetDirectories(pathToDirectory);
				if (directories.Length != 0)
				{
					string[] array = directories;
					for (int i = 0; i < array.Length; i++)
					{
						string s = array[i];
						StreamReader fileReader = null;
						try
						{
							string[] filesInDirectory = Directory.GetFiles(s);
							fileReader = new StreamReader(filesInDirectory[0]);
							fileReader.ReadLine();
							string[] metaInfo = fileReader.ReadLine().Split(new char[]
							{
								'_'
							});
							if (metaInfo.Length == 11)
							{
								this.savedGames.Add(new SaveGameMeta(metaInfo[1], Convert.ToInt32(metaInfo[2]), metaInfo[3], Convert.ToInt32(metaInfo[4]), Convert.ToInt32(metaInfo[5]), Convert.ToInt32(metaInfo[6]), Convert.ToInt32(metaInfo[7]), metaInfo[8], metaInfo[9], filesInDirectory[0]));
							}
						}
						catch (Exception)
						{
						}
						finally
						{
							if (fileReader != null)
							{
								fileReader.Close();
							}
						}
					}
				}
			}
			if (this.savedGames.Count > 1)
			{
				this.savedGames.Sort();
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000CF28 File Offset: 0x0000B128
		public void moveSelection(int direction)
		{
			if (direction == 0)
			{
				if (this.currentChoice > 0)
				{
					this.currentChoice--;
					if (this.positionOfHandOnScreen > 0 && this.currentChoice < this.savesToDisplayOnScreen)
					{
						this.positionOfHandOnScreen--;
					}
					Game1.playSound("toolSwap");
					return;
				}
			}
			else if (direction == 2 && this.currentChoice < this.savedGames.Count - 1)
			{
				this.currentChoice++;
				if (this.positionOfHandOnScreen < this.savesToDisplayOnScreen)
				{
					this.positionOfHandOnScreen++;
				}
				Game1.playSound("toolSwap");
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000CFCF File Offset: 0x0000B1CF
		public void select()
		{
			SaveGame.Load(this.savedGames[this.currentChoice].filename);
			Game1.playSound("select");
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
		public override void draw(SpriteBatch b)
		{
			if (this.savedGames.Count > 0)
			{
				int yPosition = this.yPositionOnScreen;
				for (int i = this.currentChoice; i < Math.Min(this.savedGames.Count, this.currentChoice + 3); i++)
				{
					switch (i - this.currentChoice)
					{
					case 0:
						Game1.drawDialogueBox(this.slot1.bounds.X - (int)this.slot1.scale, this.slot1.bounds.Y - Game1.tileSize, this.slot1.bounds.Width + (int)this.slot1.scale, this.slot1.bounds.Height + Game1.tileSize, false, true, null, false);
						break;
					case 1:
						Game1.drawDialogueBox(this.slot2.bounds.X - (int)this.slot2.scale, this.slot2.bounds.Y - Game1.tileSize, this.slot2.bounds.Width + (int)this.slot2.scale, this.slot2.bounds.Height + Game1.tileSize, false, true, null, false);
						break;
					case 2:
						Game1.drawDialogueBox(this.slot3.bounds.X - (int)this.slot3.scale, this.slot3.bounds.Y - Game1.tileSize, this.slot3.bounds.Width + (int)this.slot3.scale, this.slot3.bounds.Height + Game1.tileSize, false, true, null, false);
						break;
					}
					b.DrawString(Game1.dialogueFont, string.Concat(new object[]
					{
						i + 1,
						")  ",
						this.savedGames[i].name,
						", Lvl. ",
						this.savedGames[i].level + 1
					}), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 2), (float)(yPosition + Game1.tileSize)), Game1.textColor);
					yPosition += (int)((float)Game1.viewport.Height * 0.2f);
				}
				if (this.currentChoice > 0)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.upArrow.bounds.X + Game1.tileSize / 2), (float)(this.upArrow.bounds.Y + Game1.tileSize / 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 12, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.upArrow.scale, SpriteEffects.None, 0.99f);
				}
				if (this.currentChoice + 3 < this.savedGames.Count)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.downArrow.bounds.X + Game1.tileSize / 2), (float)(this.downArrow.bounds.Y + Game1.tileSize / 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 11, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.downArrow.scale, SpriteEffects.None, 0.99f);
					return;
				}
			}
			else
			{
				Game1.drawWithBorder("No Save Files Found", Color.Black, Color.White, new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize), (float)(Game1.viewport.Height / 2)));
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.savedGames.Count > 0)
			{
				if (this.currentChoice > 0 && this.upArrow.containsPoint(x, y))
				{
					this.currentChoice--;
					this.upArrow.scale = 1f;
					Game1.playSound("coin");
				}
				if (this.currentChoice + 3 < this.savedGames.Count && this.downArrow.containsPoint(x, y))
				{
					this.currentChoice++;
					this.downArrow.scale = 1f;
					Game1.playSound("coin");
				}
				if (this.slot1.containsPoint(x, y))
				{
					SaveGame.Load(this.savedGames[this.currentChoice].filename);
					Game1.playSound("smallSelect");
					Game1.exitActiveMenu();
					return;
				}
				if (this.slot2.containsPoint(x, y) && this.currentChoice + 1 < this.savedGames.Count)
				{
					SaveGame.Load(this.savedGames[this.currentChoice + 1].filename);
					Game1.playSound("smallSelect");
					Game1.exitActiveMenu();
					return;
				}
				if (this.slot3.containsPoint(x, y) && this.currentChoice + 2 < this.savedGames.Count)
				{
					SaveGame.Load(this.savedGames[this.currentChoice + 2].filename);
					Game1.playSound("smallSelect");
					Game1.exitActiveMenu();
				}
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000D548 File Offset: 0x0000B748
		public override void performHoverAction(int x, int y)
		{
			if (this.upArrow.containsPoint(x, y))
			{
				this.upArrow.scale = Math.Min(this.upArrow.scale + 0.008f, 1.1f);
			}
			else
			{
				this.upArrow.scale = Math.Max(this.upArrow.scale - 0.1f, 1f);
			}
			if (this.downArrow.containsPoint(x, y))
			{
				this.downArrow.scale = Math.Min(this.downArrow.scale + 0.008f, 1.1f);
			}
			else
			{
				this.downArrow.scale = Math.Max(this.downArrow.scale - 0.1f, 1f);
			}
			if (this.slot1.containsPoint(x, y))
			{
				this.slot1.scale = Math.Min(this.slot1.scale + 1f, 8f);
			}
			else
			{
				this.slot1.scale = Math.Max(this.slot1.scale - 1f, 1f);
			}
			if (this.slot2.containsPoint(x, y))
			{
				this.slot2.scale = Math.Min(this.slot2.scale + 1f, 8f);
			}
			else
			{
				this.slot2.scale = Math.Max(this.slot2.scale - 1f, 1f);
			}
			if (this.slot3.containsPoint(x, y))
			{
				this.slot3.scale = Math.Min(this.slot3.scale + 1f, 8f);
				return;
			}
			this.slot3.scale = Math.Max(this.slot3.scale - 1f, 1f);
		}

		// Token: 0x0400014D RID: 333
		private List<SaveGameMeta> savedGames = new List<SaveGameMeta>();

		// Token: 0x0400014E RID: 334
		private int currentChoice;

		// Token: 0x0400014F RID: 335
		private int positionOfHandOnScreen;

		// Token: 0x04000150 RID: 336
		private int savesToDisplayOnScreen;

		// Token: 0x04000151 RID: 337
		private ClickableComponent downArrow;

		// Token: 0x04000152 RID: 338
		private ClickableComponent upArrow;

		// Token: 0x04000153 RID: 339
		private ClickableComponent slot1;

		// Token: 0x04000154 RID: 340
		private ClickableComponent slot2;

		// Token: 0x04000155 RID: 341
		private ClickableComponent slot3;
	}
}
