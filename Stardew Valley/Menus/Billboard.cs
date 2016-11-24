using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace StardewValley.Menus
{
	// Token: 0x020000D6 RID: 214
	public class Billboard : IClickableMenu
	{
		// Token: 0x06000D7A RID: 3450 RVA: 0x0010FD18 File Offset: 0x0010DF18
		public Billboard(bool dailyQuest = false) : base(0, 0, 0, 0, true)
		{
			if (!Game1.player.hasOrWillReceiveMail("checkedBulletinOnce"))
			{
				Game1.player.mailReceived.Add("checkedBulletinOnce");
				(Game1.getLocationFromName("Town") as Town).checkedBoard();
			}
			if (Game1.temporaryContent == null)
			{
				Game1.temporaryContent = Game1.content.CreateTemporary();
			}
			this.dailyQuestBoard = dailyQuest;
			this.billboardTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\Billboard");
			this.width = (dailyQuest ? 338 : 301) * Game1.pixelZoom;
			this.height = 198 * Game1.pixelZoom;
			Vector2 center = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
			this.xPositionOnScreen = (int)center.X;
			this.yPositionOnScreen = (int)center.Y;
			this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "");
			this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 5 * Game1.pixelZoom, this.yPositionOnScreen, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float)Game1.pixelZoom, false);
			Game1.playSound("bigSelect");
			if (!dailyQuest)
			{
				this.calendarDays = new List<ClickableTextureComponent>();
				Dictionary<int, NPC> birthdays = new Dictionary<int, NPC>();
				foreach (NPC i in Utility.getAllCharacters())
				{
					if (i.birthday_Season != null && i.birthday_Season.Equals(Game1.currentSeason) && !birthdays.ContainsKey(i.birthday_Day) && (Game1.player.friendships.ContainsKey(i.name) || (!i.name.Equals("Dwarf") && !i.name.Equals("Sandy") && !i.name.Equals("Krobus"))))
					{
						birthdays.Add(i.birthday_Day, i);
					}
				}
				for (int j = 1; j <= 28; j++)
				{
					string festival = "";
					string birthday = "";
					NPC k = birthdays.ContainsKey(j) ? birthdays[j] : null;
					if (Utility.isFestivalDay(j, Game1.currentSeason))
					{
						festival = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + j)["name"];
					}
					else if (k != null)
					{
						if (k.name.Last<char>() == 's')
						{
							birthday = Game1.content.LoadString("Strings\\UI:Billboard_SBirthday", new object[]
							{
								k.name
							});
						}
						else
						{
							birthday = Game1.content.LoadString("Strings\\UI:Billboard_Birthday", new object[]
							{
								k.name
							});
						}
					}
					this.calendarDays.Add(new ClickableTextureComponent(festival, new Rectangle(this.xPositionOnScreen + 38 * Game1.pixelZoom + (j - 1) % 7 * 32 * Game1.pixelZoom, this.yPositionOnScreen + 50 * Game1.pixelZoom + (j - 1) / 7 * 32 * Game1.pixelZoom, 31 * Game1.pixelZoom, 31 * Game1.pixelZoom), festival, birthday, (k != null) ? k.sprite.Texture : null, (k != null) ? new Rectangle(0, 0, 16, 24) : Rectangle.Empty, 1f, false));
				}
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x001100FC File Offset: 0x0010E2FC
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			Game1.activeClickableMenu = new Billboard(this.dailyQuestBoard);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00110116 File Offset: 0x0010E316
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			Game1.playSound("bigDeSelect");
			base.exitThisMenu(true);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0011012C File Offset: 0x0010E32C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (this.dailyQuestBoard && Game1.questOfTheDay != null && (!Game1.questOfTheDay.accepted || Game1.questOfTheDay.currentObjective == null || Game1.questOfTheDay.currentObjective.Length == 0) && this.acceptQuestButton.containsPoint(x, y))
			{
				Game1.playSound("newArtifact");
				Game1.questOfTheDay.dailyQuest = true;
				Game1.questOfTheDay.accepted = true;
				Game1.questOfTheDay.canBeCancelled = true;
				Game1.questOfTheDay.daysLeft = 2;
				Game1.player.questLog.Add(Game1.questOfTheDay);
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x001101DC File Offset: 0x0010E3DC
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverText = "";
			if (this.dailyQuestBoard && Game1.questOfTheDay != null && !Game1.questOfTheDay.accepted)
			{
				float oldScale = this.acceptQuestButton.scale;
				this.acceptQuestButton.scale = (this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f);
				if (this.acceptQuestButton.scale > oldScale)
				{
					Game1.playSound("Cowboy_gunshot");
				}
			}
			if (this.calendarDays != null)
			{
				foreach (ClickableTextureComponent c in this.calendarDays)
				{
					if (c.bounds.Contains(x, y))
					{
						if (c.hoverText.Length > 0)
						{
							this.hoverText = c.hoverText;
						}
						else
						{
							this.hoverText = c.label;
						}
					}
				}
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x001102E4 File Offset: 0x0010E4E4
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			b.Draw(this.billboardTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(this.dailyQuestBoard ? new Rectangle(0, 0, 338, 198) : new Rectangle(0, 198, 301, 198)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			if (!this.dailyQuestBoard)
			{
				b.DrawString(Game1.dialogueFont, Utility.getSeasonNameFromNumber(Utility.getSeasonNumber(Game1.currentSeason)), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 / 2), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4)), Game1.textColor);
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_Year", new object[]
				{
					Game1.year
				}), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 7), (float)(this.yPositionOnScreen + Game1.tileSize * 5 / 4)), Game1.textColor);
				for (int i = 0; i < this.calendarDays.Count; i++)
				{
					if (this.calendarDays[i].name.Length > 0)
					{
						Utility.drawWithShadow(b, this.billboardTexture, new Vector2((float)(this.calendarDays[i].bounds.X + Game1.pixelZoom * 10), (float)(this.calendarDays[i].bounds.Y + Game1.pixelZoom * 14) - Game1.dialogueButtonScale / 2f), new Rectangle(1 + (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 100.0) * 14, 398, 14, 12), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
					}
					else if (this.calendarDays[i].hoverText.Length > 0)
					{
						b.Draw(this.calendarDays[i].texture, new Vector2((float)(this.calendarDays[i].bounds.X + Game1.pixelZoom * 10), (float)(this.calendarDays[i].bounds.Y + 7 * Game1.pixelZoom)), new Rectangle?(this.calendarDays[i].sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
					}
					if (Game1.dayOfMonth > i + 1)
					{
						b.Draw(Game1.staminaRect, this.calendarDays[i].bounds, Color.Gray * 0.25f);
					}
					else if (Game1.dayOfMonth == i + 1)
					{
						int offset = (int)((float)Game1.pixelZoom * Game1.dialogueButtonScale / 8f);
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), this.calendarDays[i].bounds.X - offset, this.calendarDays[i].bounds.Y - offset, this.calendarDays[i].bounds.Width + offset * 2, this.calendarDays[i].bounds.Height + offset * 2, Color.Blue, (float)Game1.pixelZoom, false);
					}
				}
			}
			else if (Game1.questOfTheDay == null || Game1.questOfTheDay.currentObjective == null || Game1.questOfTheDay.currentObjective.Length == 0)
			{
				b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_NothingPosted", new object[0]), new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6), (float)(this.yPositionOnScreen + Game1.tileSize * 5)), Game1.textColor);
			}
			else
			{
				Utility.drawTextWithShadow(b, Game1.parseText(Game1.questOfTheDay.questDescription, Game1.dialogueFont, Game1.tileSize * 10), Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 + Game1.tileSize / 2), (float)(this.yPositionOnScreen + Game1.tileSize * 4)), Game1.textColor, 1f, -1f, -1, -1, 0.5f, 3);
				if (!Game1.questOfTheDay.accepted)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (this.acceptQuestButton.scale > 1f) ? Color.LightPink : Color.White, (float)Game1.pixelZoom * this.acceptQuestButton.scale, true);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0]), Game1.dialogueFont, new Vector2((float)(this.acceptQuestButton.bounds.X + Game1.pixelZoom * 3), (float)(this.acceptQuestButton.bounds.Y + Game1.pixelZoom * 3)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
			}
			base.draw(b);
			base.drawMouse(b);
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04000DF2 RID: 3570
		private Texture2D billboardTexture;

		// Token: 0x04000DF3 RID: 3571
		public const int basewidth = 338;

		// Token: 0x04000DF4 RID: 3572
		public const int baseWidth_calendar = 301;

		// Token: 0x04000DF5 RID: 3573
		public const int baseheight = 198;

		// Token: 0x04000DF6 RID: 3574
		private bool dailyQuestBoard;

		// Token: 0x04000DF7 RID: 3575
		private ClickableComponent acceptQuestButton;

		// Token: 0x04000DF8 RID: 3576
		private List<ClickableTextureComponent> calendarDays;

		// Token: 0x04000DF9 RID: 3577
		private string hoverText = "";
	}
}
