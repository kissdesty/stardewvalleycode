using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Quests;

namespace StardewValley.Menus
{
	// Token: 0x0200010D RID: 269
	public class QuestLog : IClickableMenu
	{
		// Token: 0x06000F9E RID: 3998 RVA: 0x0014058C File Offset: 0x0013E78C
		public QuestLog() : base(0, 0, 0, 0, true)
		{
			Game1.playSound("bigSelect");
			this.pages = new List<List<Quest>>();
			for (int i = Game1.player.questLog.Count - 1; i >= 0; i--)
			{
				if (Game1.player.questLog[i] == null || Game1.player.questLog[i].destroy)
				{
					Game1.player.questLog.RemoveAt(i);
				}
				else
				{
					int which = Game1.player.questLog.Count - 1 - i;
					if (this.pages.Count <= which / 6)
					{
						this.pages.Add(new List<Quest>());
					}
					this.pages[which / 6].Add(Game1.player.questLog[i]);
				}
			}
			this.width = Game1.tileSize * 12;
			this.height = Game1.tileSize * 9;
			Vector2 topLeft = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
			this.xPositionOnScreen = (int)topLeft.X;
			this.yPositionOnScreen = (int)topLeft.Y + Game1.tileSize / 2;
			this.questLogButtons = new List<ClickableComponent>();
			for (int j = 0; j < 6; j++)
			{
				this.questLogButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + j * ((this.height - Game1.tileSize / 2) / 6), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize / 2) / 6 + Game1.pixelZoom), string.Concat(j)));
			}
			this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 5 * Game1.pixelZoom, this.yPositionOnScreen - 2 * Game1.pixelZoom, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize, this.yPositionOnScreen + Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 12 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.rewardBox = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.pixelZoom * 20, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(293, 360, 24, 24), (float)Game1.pixelZoom, true);
			this.cancelQuestButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom, this.yPositionOnScreen + this.height + Game1.pixelZoom, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float)Game1.pixelZoom, true);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0014095C File Offset: 0x0013EB5C
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			base.performHoverAction(x, y);
			if (this.questPage == -1)
			{
				for (int i = 0; i < this.questLogButtons.Count; i++)
				{
					if (this.pages.Count > 0 && this.pages[0].Count > i && this.questLogButtons[i].containsPoint(x, y) && !this.questLogButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
					{
						Game1.playSound("Cowboy_gunshot");
					}
				}
			}
			else if (this.pages[this.currentPage][this.questPage].canBeCancelled && this.cancelQuestButton.containsPoint(x, y))
			{
				this.hoverText = "Cancel Quest";
			}
			this.forwardButton.tryHover(x, y, 0.2f);
			this.backButton.tryHover(x, y, 0.2f);
			this.cancelQuestButton.tryHover(x, y, 0.2f);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00140A6E File Offset: 0x0013EC6E
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (Game1.options.doesInputListContain(Game1.options.journalButton, key) && this.readyToClose())
			{
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00140AA8 File Offset: 0x0013ECA8
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (Game1.activeClickableMenu == null)
			{
				return;
			}
			if (this.questPage == -1)
			{
				for (int i = 0; i < this.questLogButtons.Count; i++)
				{
					if (this.pages.Count > 0 && this.pages[this.currentPage].Count > i && this.questLogButtons[i].containsPoint(x, y))
					{
						Game1.playSound("smallSelect");
						this.questPage = i;
						this.pages[this.currentPage][i].showNew = false;
						return;
					}
				}
				if (this.currentPage < this.pages.Count - 1 && this.forwardButton.containsPoint(x, y))
				{
					this.currentPage++;
					Game1.playSound("shwip");
					return;
				}
				if (this.currentPage > 0 && this.backButton.containsPoint(x, y))
				{
					this.currentPage--;
					Game1.playSound("shwip");
					return;
				}
				Game1.playSound("bigDeSelect");
				base.exitThisMenu(true);
				return;
			}
			else
			{
				if (this.questPage != -1 && this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].moneyReward > 0 && this.rewardBox.containsPoint(x, y))
				{
					Game1.player.Money += this.pages[this.currentPage][this.questPage].moneyReward;
					Game1.playSound("purchaseRepeat");
					this.pages[this.currentPage][this.questPage].moneyReward = 0;
					this.pages[this.currentPage][this.questPage].destroy = true;
					return;
				}
				if (this.questPage != -1 && !this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].canBeCancelled && this.cancelQuestButton.containsPoint(x, y))
				{
					this.pages[this.currentPage][this.questPage].accepted = false;
					Game1.player.questLog.Remove(this.pages[this.currentPage][this.questPage]);
					this.pages[this.currentPage].RemoveAt(this.questPage);
					this.questPage = -1;
					Game1.playSound("trashcan");
					return;
				}
				if (this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].moneyReward <= 0)
				{
					this.pages[this.currentPage][this.questPage].destroy = true;
				}
				if (this.pages[this.currentPage][this.questPage].destroy)
				{
					Game1.player.questLog.Remove(this.pages[this.currentPage][this.questPage]);
					this.pages[this.currentPage].RemoveAt(this.questPage);
				}
				this.questPage = -1;
				Game1.playSound("shwip");
				return;
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00140E88 File Offset: 0x0013F088
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.questPage != -1 && this.pages[this.currentPage][this.questPage].hasReward())
			{
				this.rewardBox.scale = this.rewardBox.baseScale + Game1.dialogueButtonScale / 20f;
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00140EEC File Offset: 0x0013F0EC
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			SpriteText.drawStringWithScrollCenteredAt(b, "Journal", this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen - Game1.tileSize, "", 1f, -1, 0, 0.88f, false);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.White, (float)Game1.pixelZoom, true);
			if (this.questPage == -1)
			{
				for (int i = 0; i < this.questLogButtons.Count; i++)
				{
					if (this.pages.Count > 0 && this.pages[this.currentPage].Count > i)
					{
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.questLogButtons[i].bounds.X, this.questLogButtons[i].bounds.Y, this.questLogButtons[i].bounds.Width, this.questLogButtons[i].bounds.Height, this.questLogButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, false);
						if (this.pages[this.currentPage][i].showNew || this.pages[this.currentPage][i].completed)
						{
							Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.questLogButtons[i].bounds.X + Game1.tileSize + Game1.pixelZoom), (float)(this.questLogButtons[i].bounds.Y + Game1.pixelZoom * 11)), new Rectangle(this.pages[0][i].completed ? 341 : 317, 410, 23, 9), Color.White, 0f, new Vector2(11f, 4f), (float)Game1.pixelZoom + Game1.dialogueButtonScale * 10f / 250f, false, 0.99f, -1, -1, 0.35f);
						}
						else
						{
							Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.questLogButtons[i].bounds.X + Game1.tileSize / 2), (float)(this.questLogButtons[i].bounds.Y + Game1.pixelZoom * 7)), this.pages[this.currentPage][i].dailyQuest ? new Rectangle(410, 501, 9, 9) : new Rectangle(395 + (this.pages[this.currentPage][i].dailyQuest ? 3 : 0), 497, 3, 8), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.99f, -1, -1, 0.35f);
						}
						bool arg_38C_0 = this.pages[this.currentPage][i].dailyQuest;
						SpriteText.drawString(b, this.pages[this.currentPage][i].questTitle, this.questLogButtons[i].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom, this.questLogButtons[i].bounds.Y + Game1.pixelZoom * 5, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					}
				}
			}
			else
			{
				SpriteText.drawStringHorizontallyCenteredAt(b, this.pages[this.currentPage][this.questPage].questTitle, this.xPositionOnScreen + this.width / 2 + ((this.pages[this.currentPage][this.questPage].dailyQuest && this.pages[this.currentPage][this.questPage].daysLeft > 0) ? (Math.Max(8 * Game1.pixelZoom, SpriteText.getWidthOfString(this.pages[this.currentPage][this.questPage].questTitle) / 3) - 8 * Game1.pixelZoom) : 0), this.yPositionOnScreen + Game1.tileSize / 2, 999999, -1, 999999, 1f, 0.88f, false, -1);
				if (this.pages[this.currentPage][this.questPage].dailyQuest && this.pages[this.currentPage][this.questPage].daysLeft > 0)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.pixelZoom * 8), (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2)), new Rectangle(410, 501, 9, 9), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.99f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].daysLeft + " Day" + ((this.pages[this.currentPage][this.questPage].daysLeft > 1) ? "s" : ""), Game1.dialogueFont, this.width - Game1.tileSize * 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + 20 * Game1.pixelZoom), (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
				Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].questDescription, Game1.dialogueFont, this.width - Game1.tileSize * 2), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize), (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				float yPos = (float)(this.yPositionOnScreen + Game1.tileSize * 3 / 2) + Game1.dialogueFont.MeasureString(Game1.parseText(this.pages[this.currentPage][this.questPage].questDescription, Game1.dialogueFont, this.width - Game1.tileSize * 2)).Y + (float)(Game1.tileSize / 2);
				if (this.pages[this.currentPage][this.questPage].completed)
				{
					SpriteText.drawString(b, "Reward:", this.xPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom, this.rewardBox.bounds.Y + Game1.tileSize / 3 + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					this.rewardBox.draw(b);
					if (this.pages[this.currentPage][this.questPage].moneyReward > 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.rewardBox.bounds.X + Game1.pixelZoom * 4), (float)(this.rewardBox.bounds.Y + Game1.pixelZoom * 4) - Game1.dialogueButtonScale / 2f), new Rectangle?(new Rectangle(280, 410, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
						SpriteText.drawString(b, this.pages[this.currentPage][this.questPage].moneyReward + "g", this.xPositionOnScreen + Game1.tileSize * 7, this.rewardBox.bounds.Y + Game1.tileSize / 3 + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					}
				}
				else
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 / 2) + (float)(Game1.pixelZoom * 2) * Game1.dialogueButtonScale / 10f, yPos), new Rectangle(412, 495, 5, 4), Color.White, 1.57079637f, Vector2.Zero, -1f, false, -1f, -1, -1, 0.35f);
					Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].currentObjective, Game1.dialogueFont, this.width - Game1.tileSize * 4), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 8 / 4), yPos - (float)(Game1.pixelZoom * 2)), Color.DarkBlue, 1f, -1f, -1, -1, 1f, 3);
					if (this.pages[this.currentPage][this.questPage].canBeCancelled)
					{
						this.cancelQuestButton.draw(b);
					}
				}
			}
			if (this.currentPage < this.pages.Count - 1 && this.questPage == -1)
			{
				this.forwardButton.draw(b);
			}
			else if (this.currentPage > 0 || this.questPage != -1)
			{
				this.backButton.draw(b);
			}
			base.draw(b);
			base.drawMouse(b);
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x040010DB RID: 4315
		public const int questsPerPage = 6;

		// Token: 0x040010DC RID: 4316
		private List<List<Quest>> pages;

		// Token: 0x040010DD RID: 4317
		private List<ClickableComponent> questLogButtons;

		// Token: 0x040010DE RID: 4318
		private int currentPage;

		// Token: 0x040010DF RID: 4319
		private int questPage = -1;

		// Token: 0x040010E0 RID: 4320
		private ClickableTextureComponent forwardButton;

		// Token: 0x040010E1 RID: 4321
		private ClickableTextureComponent backButton;

		// Token: 0x040010E2 RID: 4322
		private ClickableTextureComponent rewardBox;

		// Token: 0x040010E3 RID: 4323
		private ClickableTextureComponent cancelQuestButton;

		// Token: 0x040010E4 RID: 4324
		private string hoverText = "";
	}
}
