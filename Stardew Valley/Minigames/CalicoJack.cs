using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;

namespace StardewValley.Minigames
{
	// Token: 0x020000C4 RID: 196
	public class CalicoJack : IMinigame
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x000FFDC0 File Offset: 0x000FDFC0
		public CalicoJack(int toBet = -1, bool highStakes = false)
		{
			this.highStakes = highStakes;
			this.startTimer = 1000;
			this.playerCards = new List<int[]>();
			this.dealerCards = new List<int[]>();
			if (toBet == -1)
			{
				this.currentBet = (highStakes ? 1000 : 100);
			}
			else
			{
				this.currentBet = toBet;
			}
			Club.timesPlayedCalicoJack++;
			this.r = new Random(Club.timesPlayedCalicoJack + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
			this.hit = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2 - SpriteText.getWidthOfString("Hit"), Game1.graphics.GraphicsDevice.Viewport.Height / 2 - Game1.tileSize, SpriteText.getWidthOfString("Hit "), Game1.tileSize), "", "Hit");
			this.stand = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2 - SpriteText.getWidthOfString("Stand"), Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize / 2, SpriteText.getWidthOfString("Stand "), Game1.tileSize), "", "Stand");
			this.doubleOrNothing = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - SpriteText.getWidthOfString("Double Or Nothing") / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, SpriteText.getWidthOfString("Double Or Nothing") + Game1.tileSize, Game1.tileSize), "", "Double Or Nothing");
			this.playAgain = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - SpriteText.getWidthOfString("New Game") / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize + Game1.tileSize / 4, SpriteText.getWidthOfString("New Game") + Game1.tileSize, Game1.tileSize), "", "New Game");
			this.quit = new ClickableComponent(new Rectangle(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - SpriteText.getWidthOfString("Quit") / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize + Game1.tileSize * 3 / 2, SpriteText.getWidthOfString("Quit") + Game1.tileSize, Game1.tileSize), "", "Quit");
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x001000B3 File Offset: 0x000FE2B3
		public bool playButtonsActive()
		{
			return this.startTimer <= 0 && this.dealerTurnTimer < 0 && !this.showingResultsScreen;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x001000D4 File Offset: 0x000FE2D4
		public bool tick(GameTime time)
		{
			for (int i = 0; i < this.playerCards.Count; i++)
			{
				if (this.playerCards[i][1] > 0)
				{
					this.playerCards[i][1] -= time.ElapsedGameTime.Milliseconds;
					if (this.playerCards[i][1] <= 0)
					{
						this.playerCards[i][1] = 0;
					}
				}
			}
			for (int j = 0; j < this.dealerCards.Count; j++)
			{
				if (this.dealerCards[j][1] > 0)
				{
					this.dealerCards[j][1] -= time.ElapsedGameTime.Milliseconds;
					if (this.dealerCards[j][1] <= 0)
					{
						this.dealerCards[j][1] = 0;
					}
				}
			}
			if (this.startTimer > 0)
			{
				int oldTimer = this.startTimer;
				this.startTimer -= time.ElapsedGameTime.Milliseconds;
				if (oldTimer % 250 < this.startTimer % 250)
				{
					switch (oldTimer / 250)
					{
					case 1:
						this.playerCards.Add(new int[]
						{
							this.r.Next(1, 10),
							400
						});
						break;
					case 2:
						this.playerCards.Add(new int[]
						{
							this.r.Next(1, 12),
							400
						});
						break;
					case 3:
						this.dealerCards.Add(new int[]
						{
							this.r.Next(1, 10),
							400
						});
						break;
					case 4:
						this.dealerCards.Add(new int[]
						{
							this.r.Next(1, 12),
							-1
						});
						break;
					}
					Game1.playSound("shwip");
				}
			}
			else if (this.bustTimer > 0)
			{
				this.bustTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.bustTimer <= 0)
				{
					this.endGame();
				}
			}
			else if (this.dealerTurnTimer > 0 && !this.showingResultsScreen)
			{
				this.dealerTurnTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.dealerTurnTimer <= 0)
				{
					int dealerTotal = 0;
					foreach (int[] k in this.dealerCards)
					{
						dealerTotal += k[0];
					}
					int playertotal = 0;
					foreach (int[] l in this.playerCards)
					{
						playertotal += l[0];
					}
					if (this.dealerCards[0][1] == -1)
					{
						this.dealerCards[0][1] = 400;
						Game1.playSound("shwip");
					}
					else if (dealerTotal < 18 || (dealerTotal < playertotal && playertotal <= 21))
					{
						this.dealerCards.Add(new int[]
						{
							this.r.Next(1, 10),
							400
						});
						dealerTotal += this.dealerCards.Last<int[]>()[0];
						Game1.playSound("shwip");
						if (dealerTotal > 21)
						{
							this.bustTimer = 2000;
						}
					}
					else
					{
						this.bustTimer = 50;
					}
					this.dealerTurnTimer = 1000;
				}
			}
			if (this.playButtonsActive())
			{
				this.hit.scale = (this.hit.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
				this.stand.scale = (this.stand.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
			}
			else if (this.showingResultsScreen)
			{
				this.doubleOrNothing.scale = (this.doubleOrNothing.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
				this.playAgain.scale = (this.playAgain.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
				this.quit.scale = (this.quit.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f);
			}
			return false;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x001005B8 File Offset: 0x000FE7B8
		public void endGame()
		{
			this.showingResultsScreen = true;
			int playertotal = 0;
			foreach (int[] i in this.playerCards)
			{
				playertotal += i[0];
			}
			if (playertotal == 21)
			{
				Game1.playSound("reward");
				this.playerWon = true;
				this.endTitle = "You Win!";
				this.endMessage = "That's a CalicoJack!";
				Game1.player.clubCoins += this.currentBet;
				return;
			}
			if (playertotal > 21)
			{
				Game1.playSound("fishEscape");
				this.endTitle = "You Lose";
				this.endMessage = "Bust!";
				Game1.player.clubCoins -= this.currentBet;
				return;
			}
			int dealerTotal = 0;
			foreach (int[] j in this.dealerCards)
			{
				dealerTotal += j[0];
			}
			if (dealerTotal > 21)
			{
				Game1.playSound("reward");
				this.playerWon = true;
				this.endTitle = "You Win!";
				this.endMessage = "Dealer busts!";
				Game1.player.clubCoins += this.currentBet;
				return;
			}
			if (playertotal == dealerTotal)
			{
				this.endTitle = "Standoff";
				this.endMessage = "No Winner";
				return;
			}
			if (playertotal > dealerTotal)
			{
				Game1.playSound("reward");
				this.endTitle = "You Win!";
				this.endMessage = "Closer to " + 21;
				Game1.player.clubCoins += this.currentBet;
				this.playerWon = true;
				return;
			}
			Game1.playSound("fishEscape");
			this.endTitle = "You Lose";
			this.endMessage = "Dealer's closer to " + 21;
			Game1.player.clubCoins -= this.currentBet;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x001007CC File Offset: 0x000FE9CC
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.playButtonsActive() && this.bustTimer <= 0)
			{
				if (this.hit.bounds.Contains(x, y))
				{
					this.playerCards.Add(new int[]
					{
						this.r.Next(1, 10),
						400
					});
					Game1.playSound("shwip");
					int total = 0;
					foreach (int[] i in this.playerCards)
					{
						total += i[0];
					}
					if (total == 21)
					{
						this.bustTimer = 1000;
					}
					else if (total > 21)
					{
						this.bustTimer = 1000;
					}
				}
				if (this.stand.bounds.Contains(x, y))
				{
					this.dealerTurnTimer = 1000;
					Game1.playSound("coin");
					return;
				}
			}
			else if (this.showingResultsScreen)
			{
				if (this.playerWon && this.doubleOrNothing.containsPoint(x, y))
				{
					Game1.currentMinigame = new CalicoJack(this.currentBet * 2, this.highStakes);
					Game1.playSound("bigSelect");
				}
				if (Game1.player.clubCoins >= this.currentBet && this.playAgain.containsPoint(x, y))
				{
					Game1.currentMinigame = new CalicoJack(-1, this.highStakes);
					Game1.playSound("smallSelect");
				}
				if (this.quit.containsPoint(x, y))
				{
					Game1.currentMinigame = null;
					Game1.playSound("bigDeSelect");
				}
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyPress(Keys k)
		{
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00100970 File Offset: 0x000FEB70
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), this.highStakes ? new Color(130, 0, 82) : Color.DarkGreen);
			if (this.showingResultsScreen)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, this.endMessage, Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.tileSize * 3 / 4, "", 1f, -1, 0, 0.88f, false);
				SpriteText.drawStringWithScrollCenteredAt(b, this.endTitle, Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
				if (!this.endTitle.Equals("Standoff"))
				{
					SpriteText.drawStringWithScrollCenteredAt(b, string.Concat(new object[]
					{
						"Result: ",
						this.playerWon ? "" : "-",
						this.currentBet,
						" "
					}), Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.tileSize * 4, "", 1f, -1, 0, 0.88f, false);
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - Game1.tileSize / 2 + SpriteText.getWidthOfString("Results: " + (this.playerWon ? "" : "-") + this.currentBet) / 2), (float)(Game1.tileSize * 4 + Game1.pixelZoom)) + new Vector2((float)(Game1.pixelZoom * 2), 0f), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
				}
				if (this.playerWon)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.doubleOrNothing.bounds.X, this.doubleOrNothing.bounds.Y, this.doubleOrNothing.bounds.Width, this.doubleOrNothing.bounds.Height, Color.White, (float)Game1.pixelZoom * this.doubleOrNothing.scale, true);
					SpriteText.drawString(b, this.doubleOrNothing.label, this.doubleOrNothing.bounds.X + Game1.pixelZoom * 8, this.doubleOrNothing.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				}
				if (Game1.player.clubCoins >= this.currentBet)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.playAgain.bounds.X, this.playAgain.bounds.Y, this.playAgain.bounds.Width, this.playAgain.bounds.Height, Color.White, (float)Game1.pixelZoom * this.playAgain.scale, true);
					SpriteText.drawString(b, this.playAgain.label, this.playAgain.bounds.X + Game1.pixelZoom * 8, this.playAgain.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				}
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.quit.bounds.X, this.quit.bounds.Y, this.quit.bounds.Width, this.quit.bounds.Height, Color.White, (float)Game1.pixelZoom * this.quit.scale, true);
				SpriteText.drawString(b, this.quit.label, this.quit.bounds.X + Game1.pixelZoom * 8, this.quit.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
			}
			else
			{
				Vector2 start = new Vector2((float)(Game1.tileSize * 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 5));
				int total = 0;
				foreach (int[] i in this.playerCards)
				{
					int cardHeight = 144;
					if (i[1] > 0)
					{
						cardHeight = (int)(Math.Abs((float)i[1] - 200f) / 200f * 144f);
					}
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, (i[1] > 200 || i[1] == -1) ? new Rectangle(399, 396, 15, 15) : new Rectangle(384, 396, 15, 15), (int)start.X, (int)start.Y + 72 - cardHeight / 2, 96, cardHeight, Color.White, (float)Game1.pixelZoom, true);
					if (i[1] == 0)
					{
						SpriteText.drawStringHorizontallyCenteredAt(b, string.Concat(i[0]), (int)start.X + 48 - Game1.tileSize / 8 + Game1.pixelZoom, (int)start.Y + 72 - Game1.tileSize / 4, 999999, -1, 999999, 1f, 0.88f, false, -1);
					}
					start.X += (float)(96 + Game1.tileSize / 4);
					if (i[1] == 0)
					{
						total += i[0];
					}
				}
				SpriteText.drawStringWithScrollBackground(b, Game1.player.name + ": " + total, Game1.tileSize * 2 + Game1.tileSize / 2, (int)start.Y + 144 + Game1.tileSize / 2, "", 1f, -1);
				start.X = (float)(Game1.tileSize * 2);
				start.Y = (float)(Game1.tileSize * 2);
				total = 0;
				foreach (int[] j in this.dealerCards)
				{
					int cardHeight2 = 144;
					if (j[1] > 0)
					{
						cardHeight2 = (int)(Math.Abs((float)j[1] - 200f) / 200f * 144f);
					}
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, (j[1] > 200 || j[1] == -1) ? new Rectangle(399, 396, 15, 15) : new Rectangle(384, 396, 15, 15), (int)start.X, (int)start.Y + 72 - cardHeight2 / 2, 96, cardHeight2, Color.White, (float)Game1.pixelZoom, true);
					if (j[1] == 0)
					{
						SpriteText.drawStringHorizontallyCenteredAt(b, string.Concat(j[0]), (int)start.X + 48 - Game1.tileSize / 8 + Game1.pixelZoom, (int)start.Y + 72 - Game1.tileSize / 4, 999999, -1, 999999, 1f, 0.88f, false, -1);
					}
					start.X += (float)(96 + Game1.tileSize / 4);
					if (j[1] == 0)
					{
						total += j[0];
					}
					else if (j[1] == -1)
					{
						total = -99999;
					}
				}
				SpriteText.drawStringWithScrollBackground(b, "Dealer: " + ((total > 0) ? string.Concat(total) : "?"), Game1.tileSize * 2 + Game1.tileSize / 2, Game1.tileSize / 2, "", 1f, -1);
				SpriteText.drawStringWithScrollBackground(b, "Bet:" + this.currentBet + "  ", Game1.tileSize * 3, Game1.graphics.GraphicsDevice.Viewport.Height / 2, "", 1f, -1);
				Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 3 + Game1.pixelZoom * 3 + SpriteText.getWidthOfString("Bet:" + this.currentBet)), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
				if (this.playButtonsActive())
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.hit.bounds.X, this.hit.bounds.Y, this.hit.bounds.Width, this.hit.bounds.Height, Color.White, (float)Game1.pixelZoom * this.hit.scale, true);
					SpriteText.drawString(b, this.hit.label, this.hit.bounds.X + Game1.pixelZoom * 2, this.hit.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.stand.bounds.X, this.stand.bounds.Y, this.stand.bounds.Width, this.stand.bounds.Height, Color.White, (float)Game1.pixelZoom * this.stand.scale, true);
					SpriteText.drawString(b, this.stand.label, this.stand.bounds.X + Game1.pixelZoom * 2, this.stand.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
				}
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
			b.End();
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00002834 File Offset: 0x00000A34
		public void changeScreenSize()
		{
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00002834 File Offset: 0x00000A34
		public void unload()
		{
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000CB5 RID: 3253
		public const int cardState_flipped = -1;

		// Token: 0x04000CB6 RID: 3254
		public const int cardState_up = 0;

		// Token: 0x04000CB7 RID: 3255
		public const int cardState_transitioning = 400;

		// Token: 0x04000CB8 RID: 3256
		public const int bet = 100;

		// Token: 0x04000CB9 RID: 3257
		public const int cardWidth = 96;

		// Token: 0x04000CBA RID: 3258
		public const int dealTime = 1000;

		// Token: 0x04000CBB RID: 3259
		public const int playingTo = 21;

		// Token: 0x04000CBC RID: 3260
		public const int passNumber = 18;

		// Token: 0x04000CBD RID: 3261
		public const int dealerTurnDelay = 1000;

		// Token: 0x04000CBE RID: 3262
		public List<int[]> playerCards;

		// Token: 0x04000CBF RID: 3263
		public List<int[]> dealerCards;

		// Token: 0x04000CC0 RID: 3264
		private Random r;

		// Token: 0x04000CC1 RID: 3265
		private int currentBet;

		// Token: 0x04000CC2 RID: 3266
		private int startTimer;

		// Token: 0x04000CC3 RID: 3267
		private int dealerTurnTimer = -1;

		// Token: 0x04000CC4 RID: 3268
		private int bustTimer;

		// Token: 0x04000CC5 RID: 3269
		private ClickableComponent hit;

		// Token: 0x04000CC6 RID: 3270
		private ClickableComponent stand;

		// Token: 0x04000CC7 RID: 3271
		private ClickableComponent doubleOrNothing;

		// Token: 0x04000CC8 RID: 3272
		private ClickableComponent playAgain;

		// Token: 0x04000CC9 RID: 3273
		private ClickableComponent quit;

		// Token: 0x04000CCA RID: 3274
		private bool showingResultsScreen;

		// Token: 0x04000CCB RID: 3275
		private bool playerWon;

		// Token: 0x04000CCC RID: 3276
		private bool highStakes;

		// Token: 0x04000CCD RID: 3277
		private string endMessage = "";

		// Token: 0x04000CCE RID: 3278
		private string endTitle = "";
	}
}
