using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x020000E9 RID: 233
	public class DayTimeMoneyBox : IClickableMenu
	{
		// Token: 0x06000E1E RID: 3614 RVA: 0x001200E8 File Offset: 0x0011E2E8
		public DayTimeMoneyBox() : base(Game1.viewport.Width - 300 + Game1.tileSize / 2, Game1.tileSize / 8, 300, 284, false)
		{
			this.position = new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen);
			this.sourceRect = new Rectangle(333, 431, 71, 43);
			this.questButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 220, this.yPositionOnScreen + 240, 44, 46), Game1.mouseCursors, new Rectangle(383, 493, 11, 14), 4f, false);
			this.zoomOutButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 23, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(177, 345, 7, 8), 4f, false);
			this.zoomInButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 31, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(184, 345, 7, 8), 4f, false);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0012027C File Offset: 0x0011E47C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.player.questLog.Count > 0 && this.questButton.containsPoint(x, y))
			{
				Game1.activeClickableMenu = new QuestLog();
			}
			if (Game1.options.zoomButtons)
			{
				if (this.zoomInButton.containsPoint(x, y) && Game1.options.zoomLevel < 1f)
				{
					int zoom = (int)((decimal)Game1.options.zoomLevel * 100m);
					zoom -= zoom % 5;
					zoom += 5;
					Game1.options.zoomLevel = Math.Min(1f, (float)zoom / 100f);
					Program.gamePtr.refreshWindowSettings();
					Game1.playSound("drumkit6");
					Game1.setMousePosition(this.zoomInButton.bounds.Center);
					return;
				}
				if (this.zoomOutButton.containsPoint(x, y) && Game1.options.zoomLevel > 0.75f)
				{
					int zoom2 = (int)((decimal)Game1.options.zoomLevel * 100m);
					zoom2 -= zoom2 % 5;
					zoom2 -= 5;
					Game1.options.zoomLevel = Math.Max(0.75f, (float)zoom2 / 100f);
					Program.gamePtr.refreshWindowSettings();
					Game1.playSound("drumkit6");
					Game1.setMousePosition(this.zoomOutButton.bounds.Center);
				}
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x001203EA File Offset: 0x0011E5EA
		public void questIconPulse()
		{
			this.questPulseTimer = 2000;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x001203F8 File Offset: 0x0011E5F8
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			if (Game1.player.questLog.Count > 0 && this.questButton.containsPoint(x, y))
			{
				this.hoverText = Game1.content.LoadString("Strings\\UI:QuestButton_Hover", new object[]
				{
					Game1.options.journalButton[0].ToString()
				});
			}
			if (Game1.options.zoomButtons)
			{
				if (this.zoomInButton.containsPoint(x, y))
				{
					this.hoverText = Game1.content.LoadString("Strings\\UI:ZoomInButton_Hover", new object[0]);
				}
				if (this.zoomOutButton.containsPoint(x, y))
				{
					this.hoverText = Game1.content.LoadString("Strings\\UI:ZoomOutButton_Hover", new object[0]);
				}
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x001204CC File Offset: 0x0011E6CC
		public void drawMoneyBox(SpriteBatch b, int overrideX = -1, int overrideY = -1)
		{
			b.Draw(Game1.mouseCursors, ((overrideY != -1) ? new Vector2((overrideX == -1) ? this.position.X : ((float)overrideX), (float)(overrideY - 172)) : this.position) + new Vector2((float)(28 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0)), (float)(172 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0))), new Rectangle?(new Rectangle(340, 472, 65, 17)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			this.moneyDial.draw(b, ((overrideY != -1) ? new Vector2((overrideX == -1) ? this.position.X : ((float)overrideX), (float)(overrideY - 172)) : this.position) + new Vector2((float)(68 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0)), (float)(196 + ((this.moneyShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0))), Game1.player.money);
			if (this.moneyShakeTimer > 0)
			{
				this.moneyShakeTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0012063C File Offset: 0x0011E83C
		public override void draw(SpriteBatch b)
		{
			if (this.timeShakeTimer > 0)
			{
				this.timeShakeTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			}
			if (this.questPulseTimer > 0)
			{
				this.questPulseTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			}
			if (this.whenToPulseTimer >= 0)
			{
				this.whenToPulseTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (this.whenToPulseTimer <= 0)
				{
					this.whenToPulseTimer = 3000;
					if (Game1.player.hasNewQuestActivity())
					{
						this.questPulseTimer = 1000;
					}
				}
			}
			b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			int dayPosition = (Game1.dayOfMonth < 10) ? 128 : 115;
			b.DrawString(Game1.dialogueFont, Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + ". " + Game1.dayOfMonth, this.position + new Vector2((float)(dayPosition + 3), 22f), this.fadeColor);
			b.DrawString(Game1.dialogueFont, Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + ". " + Game1.dayOfMonth, this.position + new Vector2((float)(dayPosition + 3), 25f), this.fadeColor);
			b.DrawString(Game1.dialogueFont, Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + ". " + Game1.dayOfMonth, this.position + new Vector2((float)dayPosition, 25f), this.fadeColor);
			b.DrawString(Game1.dialogueFont, Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + ". " + Game1.dayOfMonth, this.position + new Vector2((float)dayPosition, 22f), Game1.textColor);
			b.Draw(Game1.mouseCursors, this.position + new Vector2(212f, 68f), new Rectangle?(new Rectangle(406, 441 + Utility.getSeasonNumber(Game1.currentSeason) * 8, 12, 8)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			b.Draw(Game1.mouseCursors, this.position + new Vector2(116f, 68f), new Rectangle?(new Rectangle(317 + 12 * Game1.weatherIcon, 421, 12, 8)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
			string zeroPad = (Game1.timeOfDay % 100 == 0) ? "0" : "";
			string hours = (Game1.timeOfDay / 100 % 12 == 0) ? "12" : string.Concat(Game1.timeOfDay / 100 % 12);
			Vector2 timePosition = new Vector2((float)(110 + ((hours.Length == 1) ? 20 : 0) + ((this.timeShakeTimer > 0) ? Game1.random.Next(-2, 3) : 0)), (float)(112 + ((this.timeShakeTimer > 0) ? Game1.random.Next(-2, 3) : 0)));
			bool nofade = Game1.shouldTimePass() || Game1.fadeToBlack || Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 2000.0 > 1000.0;
			b.DrawString(Game1.dialogueFont, string.Concat(new object[]
			{
				hours,
				":",
				Game1.timeOfDay % 100,
				zeroPad
			}), this.position + timePosition + new Vector2(3f, 0f), this.fadeColor * (nofade ? 1f : 0.5f));
			b.DrawString(Game1.dialogueFont, string.Concat(new object[]
			{
				hours,
				":",
				Game1.timeOfDay % 100,
				zeroPad
			}), this.position + timePosition + new Vector2(3f, 3f), this.fadeColor * (nofade ? 1f : 0.5f));
			b.DrawString(Game1.dialogueFont, string.Concat(new object[]
			{
				hours,
				":",
				Game1.timeOfDay % 100,
				zeroPad
			}), this.position + timePosition + new Vector2(0f, 3f), this.fadeColor * (nofade ? 1f : 0.5f));
			b.DrawString(Game1.dialogueFont, string.Concat(new object[]
			{
				hours,
				":",
				Game1.timeOfDay % 100,
				zeroPad
			}), this.position + timePosition, (Game1.timeOfDay >= 2400) ? Color.Red : (Game1.textColor * (nofade ? 1f : 0.5f)));
			b.DrawString(Game1.dialogueFont, (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? "am" : "pm", this.position + new Vector2(207f, 115f), this.fadeColor * 0.33f * (nofade ? 1f : 0.5f));
			b.DrawString(Game1.dialogueFont, (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? "am" : "pm", this.position + new Vector2(207f, 112f), this.fadeColor * 0.33f * (nofade ? 1f : 0.5f));
			b.DrawString(Game1.dialogueFont, (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? "am" : "pm", this.position + new Vector2(204f, 115f), this.fadeColor * 0.33f * (nofade ? 1f : 0.5f));
			b.DrawString(Game1.dialogueFont, (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? "am" : "pm", this.position + new Vector2(204f, 112f), (Game1.timeOfDay >= 2400) ? Color.Red : (Game1.textColor * (nofade ? 1f : 0.5f)));
			int adjustedTime = (int)((float)(Game1.timeOfDay - Game1.timeOfDay % 100) + (float)(Game1.timeOfDay % 100 / 10) * 16.66f);
			if (Game1.player.questLog.Count > 0)
			{
				this.questButton.draw(b);
				if (this.questPulseTimer > 0)
				{
					float scaleMult = 1f / (Math.Max(300f, (float)Math.Abs(this.questPulseTimer % 1000 - 500)) / 500f);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.questButton.bounds.X + 6 * Game1.pixelZoom), (float)(this.questButton.bounds.Y + 8 * Game1.pixelZoom)) + ((scaleMult > 1f) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(395, 497, 3, 8)), Color.White, 0f, new Vector2(2f, 4f), (float)Game1.pixelZoom * scaleMult, SpriteEffects.None, 0.99f);
				}
			}
			if (Game1.options.zoomButtons)
			{
				this.zoomInButton.draw(b, Color.White * ((Game1.options.zoomLevel >= 1f) ? 0.5f : 1f), 1f);
				this.zoomOutButton.draw(b, Color.White * ((Game1.options.zoomLevel <= 0.75f) ? 0.5f : 1f), 1f);
			}
			this.drawMoneyBox(b, -1, -1);
			if (!this.hoverText.Equals("") && this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			b.Draw(Game1.mouseCursors, this.position + new Vector2(88f, 88f), new Rectangle?(new Rectangle(324, 477, 7, 19)), Color.White, (float)(3.1415926535897931 + Math.Min(3.1415926535897931, (double)(((float)adjustedTime + (float)Game1.gameTimeInterval / 7000f * 16.6f - 600f) / 2000f) * 3.1415926535897931)), new Vector2(3f, 17f), 4f, SpriteEffects.None, 0.9f);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00121008 File Offset: 0x0011F208
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.position = new Vector2((float)(Game1.viewport.Width - 300), (float)(Game1.tileSize / 8));
			if (Game1.isOutdoorMapSmallerThanViewport())
			{
				this.position = new Vector2(Math.Min(this.position.X, (float)(-(float)Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize - 300)), (float)(Game1.tileSize / 8));
			}
			Game1.debugOutput = "position = " + this.position.X;
			this.xPositionOnScreen = (int)this.position.X;
			this.yPositionOnScreen = (int)this.position.Y;
			this.questButton.bounds = new Rectangle(this.xPositionOnScreen + 212, this.yPositionOnScreen + 240, 44, 46);
			this.zoomOutButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 23, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(177, 345, 7, 8), 4f, false);
			this.zoomInButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 31, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(184, 345, 7, 8), 4f, false);
		}

		// Token: 0x04000F0E RID: 3854
		public new const int width = 300;

		// Token: 0x04000F0F RID: 3855
		public new const int height = 284;

		// Token: 0x04000F10 RID: 3856
		public Vector2 position;

		// Token: 0x04000F11 RID: 3857
		private Rectangle sourceRect;

		// Token: 0x04000F12 RID: 3858
		public MoneyDial moneyDial = new MoneyDial(8, true);

		// Token: 0x04000F13 RID: 3859
		private Color fadeColor = new Color(214, 170, 104);

		// Token: 0x04000F14 RID: 3860
		public int timeShakeTimer;

		// Token: 0x04000F15 RID: 3861
		public int moneyShakeTimer;

		// Token: 0x04000F16 RID: 3862
		public int questPulseTimer;

		// Token: 0x04000F17 RID: 3863
		public int whenToPulseTimer;

		// Token: 0x04000F18 RID: 3864
		public ClickableTextureComponent questButton;

		// Token: 0x04000F19 RID: 3865
		public ClickableTextureComponent zoomOutButton;

		// Token: 0x04000F1A RID: 3866
		public ClickableTextureComponent zoomInButton;

		// Token: 0x04000F1B RID: 3867
		private string hoverText = "";
	}
}
