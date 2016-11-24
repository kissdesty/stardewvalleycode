using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x02000111 RID: 273
	public class SkillsPage : IClickableMenu
	{
		// Token: 0x06000FCE RID: 4046 RVA: 0x00147358 File Offset: 0x00145558
		public SkillsPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			int baseX = this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 / 4;
			int baseY = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)height / 2f) + Game1.tileSize * 5 / 4;
			this.playerPanel = new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder, Game1.tileSize * 2, Game1.tileSize * 3);
			if (Game1.player.canUnderstandDwarves)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX, baseY, Game1.tileSize, Game1.tileSize), null, "Dwarvish Translation Guide", Game1.mouseCursors, new Rectangle(129, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			if (Game1.player.hasRustyKey)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + (Game1.tileSize + Game1.pixelZoom), baseY, Game1.tileSize, Game1.tileSize), null, "Rusty Key", Game1.mouseCursors, new Rectangle(145, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			if (Game1.player.hasClubCard)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 2 * (Game1.tileSize + Game1.pixelZoom), baseY, Game1.tileSize, Game1.tileSize), null, "Club Card", Game1.mouseCursors, new Rectangle(161, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			if (Game1.player.hasSpecialCharm)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 3 * (Game1.tileSize + Game1.pixelZoom), baseY, Game1.tileSize, Game1.tileSize), null, "Special Charm", Game1.mouseCursors, new Rectangle(177, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			if (Game1.player.hasSkullKey)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 4 * (Game1.tileSize + Game1.pixelZoom), baseY, Game1.tileSize, Game1.tileSize), null, "Skull Key", Game1.mouseCursors, new Rectangle(193, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			if (Game1.player.hasDarkTalisman)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 5 * (Game1.tileSize + Game1.pixelZoom), baseY, Game1.tileSize, Game1.tileSize), null, "Dark Talisman", Game1.mouseCursors, new Rectangle(225, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			if (Game1.player.hasMagicInk)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(baseX + 6 * (Game1.tileSize + Game1.pixelZoom), baseY, Game1.tileSize, Game1.tileSize), null, "Magic Ink", Game1.mouseCursors, new Rectangle(241, 320, 16, 16), (float)Game1.pixelZoom, true));
			}
			int addedX = 0;
			int drawX = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - Game1.pixelZoom;
			int drawY = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 3;
			for (int i = 4; i < 10; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					string professionBlurb = "";
					string professionTitle = "";
					bool drawRed = false;
					int professionNumber = -1;
					switch (j)
					{
					case 0:
						drawRed = (Game1.player.FarmingLevel > i);
						professionNumber = Game1.player.getProfessionForSkill(0, i + 1);
						this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 1:
						drawRed = (Game1.player.MiningLevel > i);
						professionNumber = Game1.player.getProfessionForSkill(3, i + 1);
						this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 2:
						drawRed = (Game1.player.ForagingLevel > i);
						professionNumber = Game1.player.getProfessionForSkill(2, i + 1);
						this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 3:
						drawRed = (Game1.player.FishingLevel > i);
						professionNumber = Game1.player.getProfessionForSkill(1, i + 1);
						this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 4:
						drawRed = (Game1.player.CombatLevel > i);
						professionNumber = Game1.player.getProfessionForSkill(4, i + 1);
						this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					case 5:
						drawRed = (Game1.player.LuckLevel > i);
						professionNumber = Game1.player.getProfessionForSkill(5, i + 1);
						this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(professionNumber));
						break;
					}
					if (drawRed && (i + 1) % 5 == 0)
					{
						this.skillBars.Add(new ClickableTextureComponent(string.Concat(professionNumber), new Rectangle(addedX + drawX - Game1.pixelZoom + i * (Game1.tileSize / 2 + Game1.pixelZoom), drawY + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6), 14 * Game1.pixelZoom, 9 * Game1.pixelZoom), null, professionBlurb, Game1.mouseCursors, new Rectangle(159, 338, 14, 9), (float)Game1.pixelZoom, true));
					}
				}
				addedX += Game1.pixelZoom * 6;
			}
			for (int k = 0; k < 5; k++)
			{
				int index = k;
				if (index == 1)
				{
					index = 3;
				}
				else if (index == 3)
				{
					index = 1;
				}
				string text = "";
				switch (index)
				{
				case 0:
					if (Game1.player.FarmingLevel > 0)
					{
						text = string.Concat(new object[]
						{
							"+",
							Game1.player.FarmingLevel,
							" Hoe Efficiency",
							Environment.NewLine,
							"+",
							Game1.player.FarmingLevel,
							" Water Can Efficiency"
						});
					}
					break;
				case 1:
					if (Game1.player.FishingLevel > 0)
					{
						text = "+" + Game1.player.FishingLevel + " Fishing Rod Efficiency";
					}
					break;
				case 2:
					if (Game1.player.ForagingLevel > 0)
					{
						text = "+" + Game1.player.ForagingLevel + " Axe Efficiency";
					}
					break;
				case 3:
					if (Game1.player.MiningLevel > 0)
					{
						text = "+" + Game1.player.MiningLevel + " Pickaxe Efficiency";
					}
					break;
				case 4:
					if (Game1.player.CombatLevel > 0)
					{
						text = "+" + Game1.player.CombatLevel * 5 + " Health";
					}
					break;
				}
				this.skillAreas.Add(new ClickableTextureComponent(string.Concat(index), new Rectangle(drawX - Game1.tileSize * 2 - Game1.tileSize * 3 / 4, drawY + k * (Game1.tileSize / 2 + Game1.pixelZoom * 6), Game1.tileSize * 2 + Game1.pixelZoom * 5, 9 * Game1.pixelZoom), string.Concat(index), text, null, Rectangle.Empty, 1f, false));
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00147B68 File Offset: 0x00145D68
		private void parseProfessionDescription(ref string professionBlurb, ref string professionTitle, List<string> professionDescription)
		{
			if (professionDescription.Count > 0)
			{
				professionTitle = professionDescription[0];
				for (int i = 1; i < professionDescription.Count; i++)
				{
					professionBlurb += professionDescription[i];
					if (i < professionDescription.Count - 1)
					{
						professionBlurb += Environment.NewLine;
					}
				}
			}
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00147BC4 File Offset: 0x00145DC4
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			this.hoverTitle = "";
			this.professionImage = -1;
			foreach (ClickableTextureComponent c in this.specialItems)
			{
				if (c.containsPoint(x, y))
				{
					this.hoverText = c.hoverText;
					break;
				}
			}
			foreach (ClickableTextureComponent c2 in this.skillBars)
			{
				c2.scale = (float)Game1.pixelZoom;
				if (c2.containsPoint(x, y) && c2.hoverText.Length > 0)
				{
					if (!c2.name.Equals("-1"))
					{
						this.hoverText = c2.hoverText;
						this.hoverTitle = LevelUpMenu.getProfessionTitleFromNumber(Convert.ToInt32(c2.name));
						this.professionImage = Convert.ToInt32(c2.name);
						c2.scale = 0f;
						break;
					}
					break;
				}
			}
			foreach (ClickableTextureComponent c3 in this.skillAreas)
			{
				if (c3.containsPoint(x, y) && c3.hoverText.Length > 0)
				{
					this.hoverText = c3.hoverText;
					this.hoverTitle = Farmer.getSkillNameFromIndex(Convert.ToInt32(c3.name));
					break;
				}
			}
			if (this.playerPanel.Contains(x, y))
			{
				this.playerPanelTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (this.playerPanelTimer <= 0)
				{
					this.playerPanelIndex = (this.playerPanelIndex + 1) % 4;
					this.playerPanelTimer = 150;
					return;
				}
			}
			else
			{
				this.playerPanelIndex = 0;
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00147DCC File Offset: 0x00145FCC
		public override void draw(SpriteBatch b)
		{
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			int x = this.xPositionOnScreen + Game1.tileSize;
			int y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
			b.Draw((Game1.timeOfDay >= 1900) ? Game1.nightbg : Game1.daybg, new Vector2((float)x, (float)y), Color.White);
			Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(Game1.player.bathingClothes ? 108 : this.playerPanelFrames[this.playerPanelIndex], 0, false, false, null, false), Game1.player.bathingClothes ? 108 : this.playerPanelFrames[this.playerPanelIndex], new Rectangle(this.playerPanelFrames[this.playerPanelIndex] * 16, Game1.player.bathingClothes ? 576 : 0, 16, 32), new Vector2((float)(x + Game1.tileSize / 2), (float)(y + Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.White, 0f, 1f, Game1.player);
			if (Game1.timeOfDay >= 1900)
			{
				Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(this.playerPanelFrames[this.playerPanelIndex], 0, false, false, null, false), this.playerPanelFrames[this.playerPanelIndex], new Rectangle(this.playerPanelFrames[this.playerPanelIndex] * 16, 0, 16, 32), new Vector2((float)(x + Game1.tileSize / 2), (float)(y + Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.DarkBlue * 0.3f, 0f, 1f, Game1.player);
			}
			b.DrawString(Game1.smallFont, Game1.player.name, new Vector2((float)(x + Game1.tileSize) - Game1.smallFont.MeasureString(Game1.player.name).X / 2f, (float)(y + 3 * Game1.tileSize + 4)), Game1.textColor);
			b.DrawString(Game1.smallFont, Game1.player.getTitle(), new Vector2((float)(x + Game1.tileSize) - Game1.smallFont.MeasureString(Game1.player.getTitle()).X / 2f, (float)(y + 4 * Game1.tileSize - Game1.tileSize / 2)), Game1.textColor);
			x = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 8;
			y = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 2;
			int addedX = 0;
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					bool drawRed = false;
					bool addedSkill = false;
					string skill = "";
					int skillLevel = 0;
					Rectangle iconSource = Rectangle.Empty;
					switch (j)
					{
					case 0:
						drawRed = (Game1.player.FarmingLevel > i);
						if (i == 0)
						{
							skill = "Farming";
						}
						skillLevel = Game1.player.FarmingLevel;
						addedSkill = (Game1.player.addedFarmingLevel > 0);
						iconSource = new Rectangle(10, 428, 10, 10);
						break;
					case 1:
						drawRed = (Game1.player.MiningLevel > i);
						if (i == 0)
						{
							skill = "Mining";
						}
						skillLevel = Game1.player.MiningLevel;
						addedSkill = (Game1.player.addedMiningLevel > 0);
						iconSource = new Rectangle(30, 428, 10, 10);
						break;
					case 2:
						drawRed = (Game1.player.ForagingLevel > i);
						if (i == 0)
						{
							skill = "Foraging";
						}
						skillLevel = Game1.player.ForagingLevel;
						addedSkill = (Game1.player.addedForagingLevel > 0);
						iconSource = new Rectangle(60, 428, 10, 10);
						break;
					case 3:
						drawRed = (Game1.player.FishingLevel > i);
						if (i == 0)
						{
							skill = "Fishing";
						}
						skillLevel = Game1.player.FishingLevel;
						addedSkill = (Game1.player.addedFishingLevel > 0);
						iconSource = new Rectangle(20, 428, 10, 10);
						break;
					case 4:
						drawRed = (Game1.player.CombatLevel > i);
						if (i == 0)
						{
							skill = "Combat";
						}
						skillLevel = Game1.player.CombatLevel;
						addedSkill = (Game1.player.addedCombatLevel > 0);
						iconSource = new Rectangle(120, 428, 10, 10);
						break;
					case 5:
						drawRed = (Game1.player.LuckLevel > i);
						if (i == 0)
						{
							skill = "Luck";
						}
						skillLevel = Game1.player.LuckLevel;
						addedSkill = (Game1.player.addedLuckLevel > 0);
						iconSource = new Rectangle(50, 428, 10, 10);
						break;
					}
					if (!skill.Equals(""))
					{
						b.DrawString(Game1.smallFont, skill, new Vector2((float)x - Game1.smallFont.MeasureString(skill).X - (float)(Game1.pixelZoom * 4) - (float)Game1.tileSize, (float)(y + Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Game1.textColor);
						b.Draw(Game1.mouseCursors, new Vector2((float)(x - Game1.pixelZoom * 16), (float)(y + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(iconSource), Color.Black * 0.3f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.85f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(x - Game1.pixelZoom * 15), (float)(y - Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(iconSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
					}
					if (!drawRed && (i + 1) % 5 == 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(addedX + x - Game1.pixelZoom + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(y + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145, 338, 14, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(addedX + x + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(y - Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145 + (drawRed ? 14 : 0), 338, 14, 9)), Color.White * (drawRed ? 1f : 0.65f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
					}
					else if ((i + 1) % 5 != 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(addedX + x - Game1.pixelZoom + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(y + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129, 338, 8, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.85f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(addedX + x + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(y - Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129 + (drawRed ? 8 : 0), 338, 8, 9)), Color.White * (drawRed ? 1f : 0.65f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
					}
					if (i == 9)
					{
						NumberSprite.draw(skillLevel, b, new Vector2((float)(addedX + x + (i + 2) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 3 + ((skillLevel >= 10) ? (Game1.pixelZoom * 3) : 0)), (float)(y + Game1.pixelZoom * 4 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Color.Black * 0.35f, 1f, 0.85f, 1f, 0, 0);
						NumberSprite.draw(skillLevel, b, new Vector2((float)(addedX + x + (i + 2) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 4 + ((skillLevel >= 10) ? (Game1.pixelZoom * 3) : 0)), (float)(y + Game1.pixelZoom * 3 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), (addedSkill ? Color.LightGreen : Color.SandyBrown) * ((skillLevel == 0) ? 0.75f : 1f), 1f, 0.87f, 1f, 0, 0);
					}
				}
				if ((i + 1) % 5 == 0)
				{
					addedX += Game1.pixelZoom * 6;
				}
			}
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.skillBars.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			foreach (ClickableTextureComponent c in this.skillBars)
			{
				if (c.scale == 0f)
				{
					IClickableMenu.drawTextureBox(b, c.bounds.X - Game1.tileSize / 4 - Game1.pixelZoom * 2, c.bounds.Y - Game1.tileSize / 4 - Game1.pixelZoom * 4, Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4, Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4, Color.White);
					b.Draw(Game1.mouseCursors, new Vector2((float)(c.bounds.X - Game1.pixelZoom * 2), (float)(c.bounds.Y - Game1.tileSize / 2 + Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionImage % 6 * 16, 624 + this.professionImage / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
			Game1.drawDialogueBox(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)this.height / 2f) - Game1.tileSize / 2, this.width - Game1.tileSize - IClickableMenu.spaceToClearSideBorder * 2, this.height / 4 + Game1.tileSize, false, true, null, false);
			base.drawBorderLabel(b, "Wallet", Game1.smallFont, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 3 / 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)this.height / 2f) - Game1.tileSize / 2);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.specialItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (this.hoverTitle.Length > 0) ? this.hoverTitle : null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04001133 RID: 4403
		private List<ClickableTextureComponent> skillBars = new List<ClickableTextureComponent>();

		// Token: 0x04001134 RID: 4404
		private List<ClickableTextureComponent> skillAreas = new List<ClickableTextureComponent>();

		// Token: 0x04001135 RID: 4405
		private List<ClickableTextureComponent> specialItems = new List<ClickableTextureComponent>();

		// Token: 0x04001136 RID: 4406
		private string hoverText = "";

		// Token: 0x04001137 RID: 4407
		private string hoverTitle = "";

		// Token: 0x04001138 RID: 4408
		private int professionImage = -1;

		// Token: 0x04001139 RID: 4409
		private int playerPanelIndex;

		// Token: 0x0400113A RID: 4410
		private int playerPanelTimer;

		// Token: 0x0400113B RID: 4411
		private Rectangle playerPanel;

		// Token: 0x0400113C RID: 4412
		private int[] playerPanelFrames = new int[]
		{
			0,
			1,
			0,
			2
		};
	}
}
