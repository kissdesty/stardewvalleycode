using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x020000FB RID: 251
	public class LevelUpMenu : IClickableMenu
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x001348E8 File Offset: 0x00132AE8
		public LevelUpMenu() : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2 - 256, 768, 512, false)
		{
			this.width = Game1.tileSize * 12;
			this.height = Game1.tileSize * 8;
			this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x001349F8 File Offset: 0x00132BF8
		public LevelUpMenu(int skill, int level) : base(Game1.viewport.Width / 2 - 384, Game1.viewport.Height / 2 - 256, 768, 512, false)
		{
			this.timerBeforeStart = 250;
			this.isActive = true;
			this.width = Game1.tileSize * 12;
			this.height = Game1.tileSize * 8;
			this.okButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.newCraftingRecipes.Clear();
			this.extraInfoForLevel.Clear();
			Game1.player.completelyStopAnimatingOrDoingAction();
			this.informationUp = true;
			this.isProfessionChooser = false;
			this.currentLevel = level;
			this.currentSkill = skill;
			if (level == 10)
			{
				Game1.getSteamAchievement("Achievement_SingularTalent");
				if (Game1.player.farmingLevel == 10 && Game1.player.miningLevel == 10 && Game1.player.fishingLevel == 10 && Game1.player.foragingLevel == 10 && Game1.player.combatLevel == 10)
				{
					Game1.getSteamAchievement("Achievement_MasterOfTheFiveWays");
				}
			}
			this.title = Game1.content.LoadString("Strings\\UI:LevelUp_Title", new object[]
			{
				this.currentLevel,
				Farmer.getSkillNameFromIndex(this.currentSkill)
			});
			this.extraInfoForLevel = this.getExtraInfoForLevel(this.currentSkill, this.currentLevel);
			switch (this.currentSkill)
			{
			case 0:
				this.sourceRectForLevelIcon = new Rectangle(0, 0, 16, 16);
				break;
			case 1:
				this.sourceRectForLevelIcon = new Rectangle(16, 0, 16, 16);
				break;
			case 2:
				this.sourceRectForLevelIcon = new Rectangle(80, 0, 16, 16);
				break;
			case 3:
				this.sourceRectForLevelIcon = new Rectangle(32, 0, 16, 16);
				break;
			case 4:
				this.sourceRectForLevelIcon = new Rectangle(128, 16, 16, 16);
				break;
			case 5:
				this.sourceRectForLevelIcon = new Rectangle(64, 0, 16, 16);
				break;
			}
			if ((this.currentLevel == 5 || this.currentLevel == 10) && this.currentSkill != 5)
			{
				this.professionsToChoose.Clear();
				this.isProfessionChooser = true;
				if (this.currentLevel == 5)
				{
					this.professionsToChoose.Add(this.currentSkill * 6);
					this.professionsToChoose.Add(this.currentSkill * 6 + 1);
				}
				else if (Game1.player.professions.Contains(this.currentSkill * 6))
				{
					this.professionsToChoose.Add(this.currentSkill * 6 + 2);
					this.professionsToChoose.Add(this.currentSkill * 6 + 3);
				}
				else
				{
					this.professionsToChoose.Add(this.currentSkill * 6 + 4);
					this.professionsToChoose.Add(this.currentSkill * 6 + 5);
				}
				this.leftProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[0]);
				this.rightProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[1]);
			}
			int newHeight = 0;
			foreach (KeyValuePair<string, string> v in CraftingRecipe.craftingRecipes)
			{
				string conditions = v.Value.Split(new char[]
				{
					'/'
				})[4];
				if (conditions.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && conditions.Contains(string.Concat(this.currentLevel)))
				{
					this.newCraftingRecipes.Add(new CraftingRecipe(v.Key, false));
					if (!Game1.player.craftingRecipes.ContainsKey(v.Key))
					{
						Game1.player.craftingRecipes.Add(v.Key, 0);
					}
					newHeight += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
				}
			}
			foreach (KeyValuePair<string, string> v2 in CraftingRecipe.cookingRecipes)
			{
				string conditions2 = v2.Value.Split(new char[]
				{
					'/'
				})[3];
				if (conditions2.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && conditions2.Contains(string.Concat(this.currentLevel)))
				{
					this.newCraftingRecipes.Add(new CraftingRecipe(v2.Key, true));
					if (!Game1.player.cookingRecipes.ContainsKey(v2.Key))
					{
						Game1.player.cookingRecipes.Add(v2.Key, 0);
						if (!Game1.player.hasOrWillReceiveMail("robinKitchenLetter"))
						{
							Game1.mailbox.Enqueue("robinKitchenLetter");
						}
					}
					newHeight += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
				}
			}
			this.height = newHeight + Game1.tileSize * 4 + this.extraInfoForLevel.Count * Game1.tileSize * 3 / 4;
			Game1.player.freezePause = 100;
			this.gameWindowSizeChanged(Rectangle.Empty, Rectangle.Empty);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00135000 File Offset: 0x00133200
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = Game1.viewport.Width / 2 - this.width / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;
			this.okButton.bounds = new Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00135088 File Offset: 0x00133288
		public List<string> getExtraInfoForLevel(int whichSkill, int whichLevel)
		{
			List<string> extraInfo = new List<string>();
			switch (whichSkill)
			{
			case 0:
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Farming1", new object[0]));
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Farming2", new object[0]));
				break;
			case 1:
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Fishing", new object[0]));
				break;
			case 2:
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Foraging1", new object[0]));
				if (whichLevel == 1)
				{
					extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Foraging2", new object[0]));
				}
				if (whichLevel == 4 || whichLevel == 8)
				{
					extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Foraging3", new object[0]));
				}
				break;
			case 3:
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Mining", new object[0]));
				break;
			case 4:
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Combat", new object[0]));
				break;
			case 5:
				extraInfo.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ExtraInfo_Luck", new object[0]));
				break;
			}
			return extraInfo;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x001351D4 File Offset: 0x001333D4
		private static void addProfessionDescriptions(List<string> descriptions, string professionName)
		{
			descriptions.Add(Game1.content.LoadString("Strings\\UI:LevelUp_ProfessionName_" + professionName, new object[0]));
			descriptions.AddRange(Game1.content.LoadString("Strings\\UI:LevelUp_ProfessionDescription_" + professionName, new object[0]).Split(new char[]
			{
				'\n'
			}));
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00135234 File Offset: 0x00133434
		private static string getProfessionName(int whichProfession)
		{
			switch (whichProfession)
			{
			case 0:
				return "Rancher";
			case 1:
				return "Tiller";
			case 2:
				return "Coopmaster";
			case 3:
				return "Shepherd";
			case 4:
				return "Artisan";
			case 5:
				return "Agriculturist";
			case 6:
				return "Fisher";
			case 7:
				return "Trapper";
			case 8:
				return "Angler";
			case 9:
				return "Pirate";
			case 10:
				return "Mariner";
			case 11:
				return "Luremaster";
			case 12:
				return "Forester";
			case 13:
				return "Gatherer";
			case 14:
				return "Lumberjack";
			case 15:
				return "Tapper";
			case 16:
				return "Botanist";
			case 17:
				return "Tracker";
			case 18:
				return "Miner";
			case 19:
				return "Geologist";
			case 20:
				return "Blacksmith";
			case 21:
				return "Prospector";
			case 22:
				return "Excavator";
			case 23:
				return "Gemologist";
			case 24:
				return "Fighter";
			case 25:
				return "Scout";
			case 26:
				return "Brute";
			case 27:
				return "Defender";
			case 28:
				return "Acrobat";
			}
			return "Desperado";
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00135377 File Offset: 0x00133577
		public static List<string> getProfessionDescription(int whichProfession)
		{
			List<string> expr_05 = new List<string>();
			LevelUpMenu.addProfessionDescriptions(expr_05, LevelUpMenu.getProfessionName(whichProfession));
			return expr_05;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0013538A File Offset: 0x0013358A
		public static string getProfessionTitleFromNumber(int whichProfession)
		{
			return Game1.content.LoadString("Strings\\UI:LevelUp_ProfessionName_" + LevelUpMenu.getProfessionName(whichProfession), new object[0]);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x001353AC File Offset: 0x001335AC
		public void getImmediateProfessionPerk(int whichProfession)
		{
			if (whichProfession == 24)
			{
				Game1.player.maxHealth += 15;
				return;
			}
			if (whichProfession != 27)
			{
				return;
			}
			Game1.player.maxHealth += 25;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x001353E0 File Offset: 0x001335E0
		public override void update(GameTime time)
		{
			if (!this.isActive)
			{
				base.exitThisMenu(true);
				return;
			}
			for (int i = this.littleStars.Count - 1; i >= 0; i--)
			{
				if (this.littleStars[i].update(time))
				{
					this.littleStars.RemoveAt(i);
				}
			}
			if (Game1.random.NextDouble() < 0.03)
			{
				Vector2 position = new Vector2(0f, (float)(Game1.random.Next(this.yPositionOnScreen - Game1.tileSize * 2, this.yPositionOnScreen - Game1.pixelZoom) / (Game1.pixelZoom * 5) * Game1.pixelZoom * 5 + Game1.tileSize / 2));
				if (Game1.random.NextDouble() < 0.5)
				{
					position.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 - 57 * Game1.pixelZoom, this.xPositionOnScreen + this.width / 2 - 33 * Game1.pixelZoom);
				}
				else
				{
					position.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 + 29 * Game1.pixelZoom, this.xPositionOnScreen + this.width - 40 * Game1.pixelZoom);
				}
				if (position.Y < (float)(this.yPositionOnScreen - Game1.tileSize - Game1.pixelZoom * 2))
				{
					position.X = (float)Game1.random.Next(this.xPositionOnScreen + this.width / 2 - 29 * Game1.pixelZoom, this.xPositionOnScreen + this.width / 2 + 29 * Game1.pixelZoom);
				}
				position.X = position.X / (float)(Game1.pixelZoom * 5) * (float)Game1.pixelZoom * 5f;
				this.littleStars.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(364, 79, 5, 5), 80f, 7, 1, position, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					local = true
				});
			}
			if (this.timerBeforeStart > 0)
			{
				this.timerBeforeStart -= time.ElapsedGameTime.Milliseconds;
				return;
			}
			if (this.isActive && this.isProfessionChooser)
			{
				this.leftProfessionColor = Game1.textColor;
				this.rightProfessionColor = Game1.textColor;
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.player.freezePause = 100;
				if (Game1.getMouseY() > this.yPositionOnScreen + Game1.tileSize * 3 && Game1.getMouseY() < this.yPositionOnScreen + this.height)
				{
					if (Game1.getMouseX() > this.xPositionOnScreen && Game1.getMouseX() < this.xPositionOnScreen + this.width / 2)
					{
						this.leftProfessionColor = Color.Green;
						if (((Mouse.GetState().LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released) || (Game1.options.gamepadControls && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
						{
							Game1.player.professions.Add(this.professionsToChoose[0]);
							this.getImmediateProfessionPerk(this.professionsToChoose[0]);
							this.isActive = false;
							this.informationUp = false;
							this.isProfessionChooser = false;
						}
					}
					else if (Game1.getMouseX() > this.xPositionOnScreen + this.width / 2 && Game1.getMouseX() < this.xPositionOnScreen + this.width)
					{
						this.rightProfessionColor = Color.Green;
						if (((Mouse.GetState().LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released) || (Game1.options.gamepadControls && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
						{
							Game1.player.professions.Add(this.professionsToChoose[1]);
							this.getImmediateProfessionPerk(this.professionsToChoose[1]);
							this.isActive = false;
							this.informationUp = false;
							this.isProfessionChooser = false;
						}
					}
				}
				this.height = Game1.tileSize * 8;
			}
			this.oldMouseState = Mouse.GetState();
			if (this.isActive && !this.informationUp && this.starIcon != null)
			{
				if (this.starIcon.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
				{
					this.starIcon.sourceRect.X = 294;
				}
				else
				{
					this.starIcon.sourceRect.X = 310;
				}
			}
			if (this.isActive && this.starIcon != null && !this.informationUp && (this.oldMouseState.LeftButton == ButtonState.Pressed || (Game1.options.gamepadControls && Game1.oldPadState.IsButtonDown(Buttons.A))) && this.starIcon.containsPoint(this.oldMouseState.X, this.oldMouseState.Y))
			{
				this.newCraftingRecipes.Clear();
				this.extraInfoForLevel.Clear();
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.playSound("bigSelect");
				this.informationUp = true;
				this.isProfessionChooser = false;
				this.currentLevel = Game1.player.newLevels.First<Point>().Y;
				this.currentSkill = Game1.player.newLevels.First<Point>().X;
				this.title = Game1.content.LoadString("Strings\\UI:LevelUp_Title", new object[]
				{
					this.currentLevel,
					Farmer.getSkillNameFromIndex(this.currentSkill)
				});
				this.extraInfoForLevel = this.getExtraInfoForLevel(this.currentSkill, this.currentLevel);
				switch (this.currentSkill)
				{
				case 0:
					this.sourceRectForLevelIcon = new Rectangle(0, 0, 16, 16);
					break;
				case 1:
					this.sourceRectForLevelIcon = new Rectangle(16, 0, 16, 16);
					break;
				case 2:
					this.sourceRectForLevelIcon = new Rectangle(80, 0, 16, 16);
					break;
				case 3:
					this.sourceRectForLevelIcon = new Rectangle(32, 0, 16, 16);
					break;
				case 4:
					this.sourceRectForLevelIcon = new Rectangle(128, 16, 16, 16);
					break;
				case 5:
					this.sourceRectForLevelIcon = new Rectangle(64, 0, 16, 16);
					break;
				}
				if ((this.currentLevel == 5 || this.currentLevel == 10) && this.currentSkill != 5)
				{
					this.professionsToChoose.Clear();
					this.isProfessionChooser = true;
					if (this.currentLevel == 5)
					{
						this.professionsToChoose.Add(this.currentSkill * 6);
						this.professionsToChoose.Add(this.currentSkill * 6 + 1);
					}
					else if (Game1.player.professions.Contains(this.currentSkill * 6))
					{
						this.professionsToChoose.Add(this.currentSkill * 6 + 2);
						this.professionsToChoose.Add(this.currentSkill * 6 + 3);
					}
					else
					{
						this.professionsToChoose.Add(this.currentSkill * 6 + 4);
						this.professionsToChoose.Add(this.currentSkill * 6 + 5);
					}
					this.leftProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[0]);
					this.rightProfessionDescription = LevelUpMenu.getProfessionDescription(this.professionsToChoose[1]);
				}
				int newHeight = 0;
				foreach (KeyValuePair<string, string> v in CraftingRecipe.craftingRecipes)
				{
					string conditions = v.Value.Split(new char[]
					{
						'/'
					})[4];
					if (conditions.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && conditions.Contains(string.Concat(this.currentLevel)))
					{
						this.newCraftingRecipes.Add(new CraftingRecipe(v.Key, false));
						if (!Game1.player.craftingRecipes.ContainsKey(v.Key))
						{
							Game1.player.craftingRecipes.Add(v.Key, 0);
						}
						newHeight += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
					}
				}
				foreach (KeyValuePair<string, string> v2 in CraftingRecipe.cookingRecipes)
				{
					string conditions2 = v2.Value.Split(new char[]
					{
						'/'
					})[3];
					if (conditions2.Contains(Farmer.getSkillNameFromIndex(this.currentSkill)) && conditions2.Contains(string.Concat(this.currentLevel)))
					{
						this.newCraftingRecipes.Add(new CraftingRecipe(v2.Key, true));
						if (!Game1.player.cookingRecipes.ContainsKey(v2.Key))
						{
							Game1.player.cookingRecipes.Add(v2.Key, 0);
						}
						newHeight += (this.newCraftingRecipes.Last<CraftingRecipe>().bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize);
					}
				}
				this.height = newHeight + Game1.tileSize * 4 + this.extraInfoForLevel.Count * Game1.tileSize * 3 / 4;
				Game1.player.freezePause = 100;
			}
			if (this.isActive && this.informationUp)
			{
				Game1.player.completelyStopAnimatingOrDoingAction();
				if (this.okButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !this.isProfessionChooser)
				{
					this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
					if ((this.oldMouseState.LeftButton == ButtonState.Pressed || (Game1.options.gamepadControls && Game1.oldPadState.IsButtonDown(Buttons.A))) && this.readyToClose())
					{
						this.getLevelPerk(this.currentSkill, this.currentLevel);
						this.isActive = false;
						this.informationUp = false;
					}
				}
				else
				{
					this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
				}
				Game1.player.freezePause = 100;
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveKeyPress(Keys key)
		{
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00135EBC File Offset: 0x001340BC
		public void getLevelPerk(int skill, int level)
		{
			if (skill != 1)
			{
				if (skill == 4)
				{
					Game1.player.maxHealth += 5;
				}
			}
			else if (level != 2)
			{
				if (level == 6)
				{
					if (!Game1.player.hasOrWillReceiveMail("fishing6"))
					{
						Game1.addMailForTomorrow("fishing6", false, false);
					}
				}
			}
			else if (!Game1.player.hasOrWillReceiveMail("fishing2"))
			{
				Game1.addMailForTomorrow("fishing2", false, false);
			}
			Game1.player.health = Game1.player.maxHealth;
			Game1.player.Stamina = (float)Game1.player.maxStamina;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00135F54 File Offset: 0x00134154
		public override void draw(SpriteBatch b)
		{
			if (this.timerBeforeStart > 0)
			{
				return;
			}
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.littleStars.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, false, 0, 0);
				}
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + this.width / 2 - 58 * Game1.pixelZoom / 2), (float)(this.yPositionOnScreen - Game1.tileSize / 2 + Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(363, 87, 58, 22)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			if (!this.informationUp && this.isActive && this.starIcon != null)
			{
				this.starIcon.draw(b);
				return;
			}
			if (this.informationUp)
			{
				if (this.isProfessionChooser)
				{
					Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
					base.drawHorizontalPartition(b, this.yPositionOnScreen + Game1.tileSize * 3, false);
					base.drawVerticalIntersectingPartition(b, this.xPositionOnScreen + this.width / 2 - Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize * 3);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					b.DrawString(Game1.dialogueFont, this.title, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.title).X / 2f, (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), Game1.textColor);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + this.width - IClickableMenu.spaceToClearSideBorder - IClickableMenu.borderWidth - Game1.tileSize), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					string chooseProfession = Game1.content.LoadString("Strings\\UI:LevelUp_ChooseProfession", new object[0]);
					b.DrawString(Game1.smallFont, chooseProfession, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(chooseProfession).X / 2f, (float)(this.yPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearTopBorder)), Game1.textColor);
					b.DrawString(Game1.dialogueFont, this.leftProfessionDescription[0], new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2)), this.leftProfessionColor);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2 - Game1.tileSize * 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 - Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionsToChoose[0] % 6 * 16, 624 + this.professionsToChoose[0] / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					for (int i = 1; i < this.leftProfessionDescription.Count; i++)
					{
						b.DrawString(Game1.smallFont, Game1.parseText(this.leftProfessionDescription[i], Game1.smallFont, this.width / 2 - 64), new Vector2((float)(-4 + this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 + 8 + Game1.tileSize * (i + 1))), this.leftProfessionColor);
					}
					b.DrawString(Game1.dialogueFont, this.rightProfessionDescription[0], new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2)), this.rightProfessionColor);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width - Game1.tileSize * 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 - Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionsToChoose[1] % 6 * 16, 624 + this.professionsToChoose[1] / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					for (int j = 1; j < this.rightProfessionDescription.Count; j++)
					{
						b.DrawString(Game1.smallFont, Game1.parseText(this.rightProfessionDescription[j], Game1.smallFont, this.width / 2 - 48), new Vector2((float)(-4 + this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + this.width / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 2 + 8 + Game1.tileSize * (j + 1))), this.rightProfessionColor);
					}
				}
				else
				{
					Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					b.DrawString(Game1.dialogueFont, this.title, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.title).X / 2f, (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), Game1.textColor);
					Utility.drawWithShadow(b, Game1.buffsIcons, new Vector2((float)(this.xPositionOnScreen + this.width - IClickableMenu.spaceToClearSideBorder - IClickableMenu.borderWidth - Game1.tileSize), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4)), this.sourceRectForLevelIcon, Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.88f, -1, -1, 0.35f);
					int y = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 5 / 4;
					foreach (string s in this.extraInfoForLevel)
					{
						b.DrawString(Game1.smallFont, s, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(s).X / 2f, (float)y), Game1.textColor);
						y += Game1.tileSize * 3 / 4;
					}
					foreach (CraftingRecipe s2 in this.newCraftingRecipes)
					{
						string cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_" + (s2.isCookingRecipe ? "cooking" : "crafting"), new object[0]);
						string message = Game1.content.LoadString("Strings\\UI:LevelUp_NewRecipe", new object[]
						{
							cookingOrCrafting,
							s2.name
						});
						b.DrawString(Game1.smallFont, message, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.smallFont.MeasureString(message).X / 2f - (float)Game1.tileSize, (float)(y + (s2.bigCraftable ? (Game1.tileSize * 3 / 5) : (Game1.tileSize / 5)))), Game1.textColor);
						s2.drawMenuView(b, (int)((float)(this.xPositionOnScreen + this.width / 2) + Game1.smallFont.MeasureString(message).X / 2f - (float)(Game1.tileSize * 3 / 4)), y - Game1.tileSize / 4, 0.88f, true);
						y += (s2.bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize) + Game1.pixelZoom * 2;
					}
					this.okButton.draw(b);
				}
				base.drawMouse(b);
			}
		}

		// Token: 0x0400102F RID: 4143
		public const int basewidth = 768;

		// Token: 0x04001030 RID: 4144
		public const int baseheight = 512;

		// Token: 0x04001031 RID: 4145
		public bool informationUp;

		// Token: 0x04001032 RID: 4146
		public bool isActive;

		// Token: 0x04001033 RID: 4147
		public bool isProfessionChooser;

		// Token: 0x04001034 RID: 4148
		private int currentLevel;

		// Token: 0x04001035 RID: 4149
		private int currentSkill;

		// Token: 0x04001036 RID: 4150
		private int timerBeforeStart;

		// Token: 0x04001037 RID: 4151
		private float scale;

		// Token: 0x04001038 RID: 4152
		private Color leftProfessionColor = Game1.textColor;

		// Token: 0x04001039 RID: 4153
		private Color rightProfessionColor = Game1.textColor;

		// Token: 0x0400103A RID: 4154
		private MouseState oldMouseState;

		// Token: 0x0400103B RID: 4155
		private ClickableTextureComponent starIcon;

		// Token: 0x0400103C RID: 4156
		private ClickableTextureComponent okButton;

		// Token: 0x0400103D RID: 4157
		private List<CraftingRecipe> newCraftingRecipes = new List<CraftingRecipe>();

		// Token: 0x0400103E RID: 4158
		private List<string> extraInfoForLevel = new List<string>();

		// Token: 0x0400103F RID: 4159
		private List<string> leftProfessionDescription = new List<string>();

		// Token: 0x04001040 RID: 4160
		private List<string> rightProfessionDescription = new List<string>();

		// Token: 0x04001041 RID: 4161
		private Rectangle sourceRectForLevelIcon;

		// Token: 0x04001042 RID: 4162
		private string title;

		// Token: 0x04001043 RID: 4163
		private List<int> professionsToChoose = new List<int>();

		// Token: 0x04001044 RID: 4164
		private List<TemporaryAnimatedSprite> littleStars = new List<TemporaryAnimatedSprite>();
	}
}
