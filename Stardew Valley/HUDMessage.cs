using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace StardewValley
{
	// Token: 0x0200002E RID: 46
	public class HUDMessage
	{
		// Token: 0x17000024 RID: 36
		public string Message
		{
			// Token: 0x06000263 RID: 611 RVA: 0x000351D4 File Offset: 0x000333D4
			get
			{
				if (this.type == null)
				{
					return this.message;
				}
				if (this.type.Equals("Money"))
				{
					return (this.add ? "+ " : "- ") + this.number + "g";
				}
				return string.Concat(new object[]
				{
					this.add ? "+ " : "- ",
					this.number,
					" ",
					this.type
				});
			}
			// Token: 0x06000264 RID: 612 RVA: 0x0003526D File Offset: 0x0003346D
			set
			{
				this.message = value;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00035276 File Offset: 0x00033476
		public HUDMessage(string message)
		{
			this.message = message;
			this.color = Color.SeaGreen;
			this.timeLeft = 3500f;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000352B0 File Offset: 0x000334B0
		public HUDMessage(string message, bool achievement)
		{
			if (achievement)
			{
				this.message = "New Achievement: " + message;
				this.color = Color.OrangeRed;
				this.timeLeft = 5250f;
				this.achievement = true;
				this.whatType = 1;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00035310 File Offset: 0x00033510
		public HUDMessage(string message, int whatType)
		{
			this.message = message;
			this.color = Color.OrangeRed;
			this.timeLeft = 5250f;
			this.achievement = true;
			this.whatType = whatType;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00035360 File Offset: 0x00033560
		public HUDMessage(string type, int number, bool add, Color color, Item messageSubject = null)
		{
			this.type = type;
			this.add = add;
			this.color = color;
			this.timeLeft = 3500f;
			this.number = number;
			this.messageSubject = messageSubject;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x000353B5 File Offset: 0x000335B5
		public HUDMessage(string message, Color color, float timeLeft) : this(message, color, timeLeft, false)
		{
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000353C4 File Offset: 0x000335C4
		public HUDMessage(string message, string leaveMeNull)
		{
			this.message = Game1.parseText(message, Game1.dialogueFont, Game1.tileSize * 6);
			this.timeLeft = 3500f;
			this.color = Game1.textColor;
			this.noIcon = true;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00035420 File Offset: 0x00033620
		public HUDMessage(string message, Color color, float timeLeft, bool fadeIn)
		{
			this.message = message;
			this.color = color;
			this.timeLeft = timeLeft;
			this.fadeIn = fadeIn;
			if (fadeIn)
			{
				this.transparency = 0f;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00035474 File Offset: 0x00033674
		public bool update(GameTime time)
		{
			this.timeLeft -= (float)time.ElapsedGameTime.Milliseconds;
			HUDMessage.numberAchievements = 0;
			if (this.timeLeft < 0f)
			{
				this.transparency -= 0.02f;
				if (this.transparency < 0f)
				{
					return true;
				}
			}
			else if (this.fadeIn)
			{
				this.transparency = Math.Min(this.transparency + 0.02f, 1f);
			}
			return false;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000354F8 File Offset: 0x000336F8
		public void draw(SpriteBatch b, int i)
		{
			if (this.noIcon)
			{
				IClickableMenu.drawHoverText(b, this.message, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, Game1.tileSize / 4, ((Game1.viewport.Width < 1400) ? (-Game1.tileSize) : 0) + Game1.graphics.GraphicsDevice.Viewport.Height - (i + 1) * Game1.tileSize * 7 / 4 - Game1.tileSize / 3 - (int)Game1.dialogueFont.MeasureString(this.message).Y, this.transparency, null);
				return;
			}
			Vector2 itemBoxPosition = new Vector2((float)(Game1.tileSize / 4), (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - (i + 1) * Game1.tileSize * 7 / 4 - Game1.tileSize));
			if (Game1.isOutdoorMapSmallerThanViewport())
			{
				itemBoxPosition.X = (float)Math.Max(Game1.tileSize / 4, -Game1.viewport.X + Game1.tileSize / 4);
			}
			if (Game1.viewport.Width < 1400)
			{
				itemBoxPosition.Y -= (float)(Game1.tileSize * 3 / 4);
			}
			b.Draw(Game1.mouseCursors, itemBoxPosition, new Rectangle?((this.messageSubject != null && this.messageSubject is Object && (this.messageSubject as Object).sellToStorePrice() > 500) ? new Rectangle(163, 399, 26, 24) : new Rectangle(293, 360, 26, 24)), Color.White * this.transparency, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			float messageWidth = Game1.smallFont.MeasureString((this.messageSubject == null || this.messageSubject.Name == null) ? ((this.message == null) ? "" : this.message) : this.messageSubject.Name).X;
			b.Draw(Game1.mouseCursors, new Vector2(itemBoxPosition.X + (float)(26 * Game1.pixelZoom), itemBoxPosition.Y), new Rectangle?(new Rectangle(319, 360, 1, 24)), Color.White * this.transparency, 0f, Vector2.Zero, new Vector2(messageWidth, (float)Game1.pixelZoom), SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(itemBoxPosition.X + (float)(26 * Game1.pixelZoom) + messageWidth, itemBoxPosition.Y), new Rectangle?(new Rectangle(323, 360, 6, 24)), Color.White * this.transparency, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			itemBoxPosition.X += (float)(Game1.pixelZoom * 4);
			itemBoxPosition.Y += (float)(Game1.pixelZoom * 4);
			if (this.messageSubject == null)
			{
				switch (this.whatType)
				{
				case 1:
					b.Draw(Game1.mouseCursors, itemBoxPosition + new Vector2(8f, 8f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(294, 392, 16, 16)), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom + Math.Max(0f, (this.timeLeft - 3000f) / 900f), SpriteEffects.None, 1f);
					break;
				case 2:
					b.Draw(Game1.mouseCursors, itemBoxPosition + new Vector2(8f, 8f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(403, 496, 5, 14)), Color.White * this.transparency, 0f, new Vector2(3f, 7f), (float)Game1.pixelZoom + Math.Max(0f, (this.timeLeft - 3000f) / 900f), SpriteEffects.None, 1f);
					break;
				case 3:
					b.Draw(Game1.mouseCursors, itemBoxPosition + new Vector2(8f, 8f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(268, 470, 16, 16)), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom + Math.Max(0f, (this.timeLeft - 3000f) / 900f), SpriteEffects.None, 1f);
					break;
				case 4:
					b.Draw(Game1.mouseCursors, itemBoxPosition + new Vector2(8f, 8f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(0, 411, 16, 16)), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom + Math.Max(0f, (this.timeLeft - 3000f) / 900f), SpriteEffects.None, 1f);
					break;
				case 5:
					b.Draw(Game1.mouseCursors, itemBoxPosition + new Vector2(8f, 8f) * (float)Game1.pixelZoom, new Rectangle?(new Rectangle(16, 411, 16, 16)), Color.White * this.transparency, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom + Math.Max(0f, (this.timeLeft - 3000f) / 900f), SpriteEffects.None, 1f);
					break;
				}
			}
			else
			{
				this.messageSubject.drawInMenu(b, itemBoxPosition, 1f + Math.Max(0f, (this.timeLeft - 3000f) / 900f), this.transparency, 1f, false);
			}
			itemBoxPosition.X += (float)(Game1.tileSize * 4 / 5);
			itemBoxPosition.Y += (float)(Game1.tileSize * 4 / 5);
			if (this.number > 1)
			{
				Game1.drawWithBorder(string.Concat(this.number), Game1.textColor * this.transparency, Color.White * this.transparency, itemBoxPosition, 0f, 0.666f, 1f, false);
			}
			itemBoxPosition.X += (float)(Game1.tileSize / 2);
			itemBoxPosition.Y -= (float)(Game1.tileSize * 2 / 5 + Game1.pixelZoom * 2);
			Utility.drawTextWithShadow(b, (this.messageSubject == null) ? this.message : this.messageSubject.Name, Game1.smallFont, itemBoxPosition, Game1.textColor * this.transparency, 1f, 1f, -1, -1, this.transparency, 3);
		}

		// Token: 0x04000294 RID: 660
		public const float defaultTime = 3500f;

		// Token: 0x04000295 RID: 661
		public const int achievement_type = 1;

		// Token: 0x04000296 RID: 662
		public const int newQuest_type = 2;

		// Token: 0x04000297 RID: 663
		public const int error_type = 3;

		// Token: 0x04000298 RID: 664
		public const int stamina_type = 4;

		// Token: 0x04000299 RID: 665
		public const int health_type = 5;

		// Token: 0x0400029A RID: 666
		public string message;

		// Token: 0x0400029B RID: 667
		public string type;

		// Token: 0x0400029C RID: 668
		public Color color;

		// Token: 0x0400029D RID: 669
		public float timeLeft;

		// Token: 0x0400029E RID: 670
		public float transparency = 1f;

		// Token: 0x0400029F RID: 671
		public int number = -1;

		// Token: 0x040002A0 RID: 672
		public int whatType;

		// Token: 0x040002A1 RID: 673
		public bool add;

		// Token: 0x040002A2 RID: 674
		public bool achievement;

		// Token: 0x040002A3 RID: 675
		public bool fadeIn;

		// Token: 0x040002A4 RID: 676
		public bool noIcon;

		// Token: 0x040002A5 RID: 677
		private static int numberAchievements;

		// Token: 0x040002A6 RID: 678
		private Item messageSubject;
	}
}
