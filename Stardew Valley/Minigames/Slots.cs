using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;

namespace StardewValley.Minigames
{
	// Token: 0x020000C7 RID: 199
	public class Slots : IMinigame
	{
		// Token: 0x06000CB6 RID: 3254 RVA: 0x001034D8 File Offset: 0x001016D8
		public Slots(int toBet = -1, bool highStakes = false)
		{
			this.currentBet = toBet;
			if (this.currentBet == -1)
			{
				this.currentBet = 10;
			}
			this.slots = new List<float>();
			this.slots.Add(0f);
			this.slots.Add(0f);
			this.slots.Add(0f);
			this.slotResults = new List<float>();
			this.slotResults.Add(0f);
			this.slotResults.Add(0f);
			this.slotResults.Add(0f);
			Game1.playSound("newArtifact");
			this.r = new Random(Club.timesPlayedSlots + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
			this.setSlotResults(this.slots);
			Vector2 pos = Utility.getTopLeftPositionForCenteringOnScreen(26 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize / 2);
			this.spinButton10 = new ClickableComponent(new Rectangle((int)pos.X, (int)pos.Y, 26 * Game1.pixelZoom, 13 * Game1.pixelZoom), "Bet10");
			pos = Utility.getTopLeftPositionForCenteringOnScreen(31 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize * 3 / 2);
			this.spinButton100 = new ClickableComponent(new Rectangle((int)pos.X, (int)pos.Y, 31 * Game1.pixelZoom, 13 * Game1.pixelZoom), "Bet100");
			pos = Utility.getTopLeftPositionForCenteringOnScreen(24 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize * 5 / 2);
			this.doneButton = new ClickableComponent(new Rectangle((int)pos.X, (int)pos.Y, 24 * Game1.pixelZoom, 13 * Game1.pixelZoom), "Done");
			if (Game1.isAnyGamePadButtonBeingPressed())
			{
				Game1.setMousePosition(this.spinButton10.bounds.Center);
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x001036DC File Offset: 0x001018DC
		public void setSlotResults(List<float> toSet)
		{
			double d = this.r.NextDouble();
			double modifier = 0.858 + Game1.dailyLuck * 2.0 + (double)Game1.player.LuckLevel * 0.08;
			if (d < 5E-05 * modifier)
			{
				this.set(toSet, 5);
				this.payoutModifier = 2500f;
				return;
			}
			if (d < 0.0005 * modifier)
			{
				this.set(toSet, 6);
				this.payoutModifier = 1000f;
				return;
			}
			if (d < 0.001 * modifier)
			{
				this.set(toSet, 7);
				this.payoutModifier = 500f;
				return;
			}
			if (d < 0.002 * modifier)
			{
				this.set(toSet, 4);
				this.payoutModifier = 200f;
				return;
			}
			if (d < 0.004 * modifier)
			{
				this.set(toSet, 3);
				this.payoutModifier = 120f;
				return;
			}
			if (d < 0.006 * modifier)
			{
				this.set(toSet, 2);
				this.payoutModifier = 80f;
				return;
			}
			if (d < 0.01 * modifier)
			{
				this.set(toSet, 1);
				this.payoutModifier = 30f;
				return;
			}
			if (d < 0.03 * modifier)
			{
				int whereToPutNonStar = this.r.Next(3);
				for (int i = 0; i < 3; i++)
				{
					toSet[i] = (float)((i == whereToPutNonStar) ? this.r.Next(7) : 7);
				}
				this.payoutModifier = 3f;
				return;
			}
			if (d < 0.08 * modifier)
			{
				this.set(toSet, 0);
				this.payoutModifier = 5f;
				return;
			}
			if (d < 0.2 * modifier)
			{
				int whereToPutStar = this.r.Next(3);
				for (int j = 0; j < 3; j++)
				{
					toSet[j] = (float)((j == whereToPutStar) ? 7 : this.r.Next(7));
				}
				this.payoutModifier = 2f;
				return;
			}
			this.payoutModifier = 0f;
			int[] used = new int[8];
			for (int k = 0; k < 3; k++)
			{
				int next = this.r.Next(6);
				while (used[next] > 1)
				{
					next = this.r.Next(6);
				}
				toSet[k] = (float)next;
				used[next]++;
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0010393B File Offset: 0x00101B3B
		private void set(List<float> toSet, int number)
		{
			toSet[0] = (float)number;
			toSet[1] = (float)number;
			toSet[2] = (float)number;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00103958 File Offset: 0x00101B58
		public bool tick(GameTime time)
		{
			if (this.spinning && this.endTimer <= 0)
			{
				for (int i = this.slotsFinished; i < this.slots.Count; i++)
				{
					float old = this.slots[i];
					List<float> list = this.slots;
					int index = i;
					list[index] += (float)time.ElapsedGameTime.Milliseconds * 0.008f * (1f - (float)i * 0.05f);
					list = this.slots;
					index = i;
					list[index] %= 8f;
					if (i == 2)
					{
						if (old % (0.25f + (float)this.slotsFinished * 0.5f) > this.slots[i] % (0.25f + (float)this.slotsFinished * 0.5f))
						{
							Game1.playSound("shiny4");
						}
						if (old > this.slots[i])
						{
							this.spinsCount++;
						}
					}
					if (this.spinsCount > 0 && i == this.slotsFinished && Math.Abs(this.slots[i] - this.slotResults[i]) <= (float)time.ElapsedGameTime.Milliseconds * 0.008f)
					{
						this.slots[i] = this.slotResults[i];
						this.slotsFinished++;
						this.spinsCount--;
						Game1.playSound("Cowboy_gunshot");
					}
				}
				if (this.slotsFinished >= 3)
				{
					this.endTimer = ((this.payoutModifier == 0f) ? 600 : 1000);
				}
			}
			if (this.endTimer > 0)
			{
				this.endTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.endTimer <= 0)
				{
					this.spinning = false;
					this.spinsCount = 0;
					this.slotsFinished = 0;
					if (this.payoutModifier > 0f)
					{
						this.showResult = true;
						Game1.playSound((this.payoutModifier >= 5f) ? ((this.payoutModifier >= 10f) ? "reward" : "money") : "newArtifact");
					}
					else
					{
						Game1.playSound("breathout");
					}
					Game1.player.clubCoins += (int)((float)this.currentBet * this.payoutModifier);
				}
			}
			this.spinButton10.scale = ((!this.spinning && this.spinButton10.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY())) ? 1.05f : 1f);
			this.spinButton100.scale = ((!this.spinning && this.spinButton100.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY())) ? 1.05f : 1f);
			this.doneButton.scale = ((!this.spinning && this.doneButton.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY())) ? 1.05f : 1f);
			return false;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00103C84 File Offset: 0x00101E84
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.spinning && Game1.player.clubCoins >= 10 && this.spinButton10.bounds.Contains(x, y))
			{
				Club.timesPlayedSlots++;
				this.r = new Random(Club.timesPlayedSlots + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
				this.setSlotResults(this.slotResults);
				this.spinning = true;
				Game1.playSound("bigSelect");
				this.currentBet = 10;
				this.slotsFinished = 0;
				this.spinsCount = 0;
				this.showResult = false;
				Game1.player.clubCoins -= 10;
			}
			if (!this.spinning && Game1.player.clubCoins >= 100 && this.spinButton100.bounds.Contains(x, y))
			{
				Club.timesPlayedSlots++;
				this.r = new Random(Club.timesPlayedSlots + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame);
				this.setSlotResults(this.slotResults);
				Game1.playSound("bigSelect");
				this.spinning = true;
				this.slotsFinished = 0;
				this.spinsCount = 0;
				this.showResult = false;
				this.currentBet = 100;
				Game1.player.clubCoins -= 100;
			}
			if (!this.spinning && this.doneButton.bounds.Contains(x, y))
			{
				Game1.playSound("bigDeSelect");
				Game1.currentMinigame = null;
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00103E14 File Offset: 0x00102014
		public void receiveKeyPress(Keys k)
		{
			if (!this.spinning && (k.Equals(Keys.Escape) || Game1.options.doesInputListContain(Game1.options.menuButton, k)))
			{
				this.unload();
				Game1.playSound("bigDeSelect");
				Game1.currentMinigame = null;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00103E6C File Offset: 0x0010206C
		public int getIconIndex(int index)
		{
			switch (index)
			{
			case 0:
				return 24;
			case 1:
				return 186;
			case 2:
				return 138;
			case 3:
				return 392;
			case 4:
				return 254;
			case 5:
				return 434;
			case 6:
				return 72;
			case 7:
				return 638;
			default:
				return 24;
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00103ED0 File Offset: 0x001020D0
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Color(38, 0, 7));
			b.Draw(Game1.mouseCursors, Utility.getTopLeftPositionForCenteringOnScreen(57 * Game1.pixelZoom, 13 * Game1.pixelZoom, 0, -4 * Game1.tileSize), new Rectangle?(new Rectangle(441, 424, 57, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			for (int i = 0; i < 3; i++)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 28 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 32 * Game1.pixelZoom)), new Rectangle?(new Rectangle(306, 320, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				float faceValue = (this.slots[i] + 1f) % 8f;
				int previous = this.getIconIndex(((int)faceValue + 8 - 1) % 8);
				int current = this.getIconIndex((previous + 1) % 8);
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 28 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 32 * Game1.pixelZoom)) - new Vector2(0f, (float)(-(float)Game1.tileSize) * (faceValue % 1f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, previous, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 28 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 32 * Game1.pixelZoom)) - new Vector2(0f, (float)Game1.tileSize - (float)Game1.tileSize * (faceValue % 1f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, current, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 33 * Game1.pixelZoom + i * 26 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 48 * Game1.pixelZoom)), new Rectangle?(new Rectangle(415, 385, 26, 48)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			}
			if (this.showResult)
			{
				SpriteText.drawString(b, "+" + this.payoutModifier * (float)this.currentBet, Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 93 * Game1.pixelZoom, this.spinButton10.bounds.Y - Game1.tileSize + Game1.pixelZoom * 2, 9999, -1, 9999, 1f, 1f, false, -1, "", 4);
			}
			b.Draw(Game1.mouseCursors, new Vector2((float)this.spinButton10.bounds.X, (float)this.spinButton10.bounds.Y), new Rectangle?(new Rectangle(441, 385, 26, 13)), Color.White * ((!this.spinning && Game1.player.clubCoins >= 10) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom * this.spinButton10.scale, SpriteEffects.None, 0.99f);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.spinButton100.bounds.X, (float)this.spinButton100.bounds.Y), new Rectangle?(new Rectangle(441, 398, 31, 13)), Color.White * ((!this.spinning && Game1.player.clubCoins >= 100) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom * this.spinButton100.scale, SpriteEffects.None, 0.99f);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.doneButton.bounds.X, (float)this.doneButton.bounds.Y), new Rectangle?(new Rectangle(441, 411, 24, 13)), Color.White * ((!this.spinning) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom * this.doneButton.scale, SpriteEffects.None, 0.99f);
			SpriteText.drawStringWithScrollBackground(b, "  " + Game1.player.clubCoins, Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 94 * Game1.pixelZoom, Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 30 * Game1.pixelZoom, "", 1f, -1);
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - 94 * Game1.pixelZoom + Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 30 * Game1.pixelZoom + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
			Vector2 basePos = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 + 50 * Game1.pixelZoom), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 88 * Game1.pixelZoom));
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), (int)basePos.X, (int)basePos.Y, Game1.tileSize * 6, Game1.tileSize * 11, Color.White, (float)Game1.pixelZoom, true);
			b.Draw(Game1.objectSpriteSheet, basePos + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.pixelZoom * 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			SpriteText.drawString(b, "x2", (int)basePos.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int)basePos.Y + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
			b.Draw(Game1.objectSpriteSheet, basePos + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			b.Draw(Game1.objectSpriteSheet, basePos + new Vector2((float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom)), (float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
			SpriteText.drawString(b, "x3", (int)basePos.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int)basePos.Y + (Game1.tileSize + Game1.pixelZoom) + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
			for (int j = 0; j < 8; j++)
			{
				int which = j;
				if (j == 5)
				{
					which = 7;
				}
				else if (j == 7)
				{
					which = 5;
				}
				b.Draw(Game1.objectSpriteSheet, basePos + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.pixelZoom * 2 + (j + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(which), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.objectSpriteSheet, basePos + new Vector2((float)(Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom)), (float)(Game1.pixelZoom * 2 + (j + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(which), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.objectSpriteSheet, basePos + new Vector2((float)(Game1.pixelZoom * 2 + 2 * (Game1.tileSize + Game1.pixelZoom)), (float)(Game1.pixelZoom * 2 + (j + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(which), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				int payout = 0;
				switch (j)
				{
				case 0:
					payout = 5;
					break;
				case 1:
					payout = 30;
					break;
				case 2:
					payout = 80;
					break;
				case 3:
					payout = 120;
					break;
				case 4:
					payout = 200;
					break;
				case 5:
					payout = 500;
					break;
				case 6:
					payout = 1000;
					break;
				case 7:
					payout = 2500;
					break;
				}
				SpriteText.drawString(b, "x" + payout, (int)basePos.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int)basePos.Y + (j + 2) * (Game1.tileSize + Game1.pixelZoom) + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), (int)basePos.X - Game1.tileSize * 10, (int)basePos.Y, Game1.tileSize * 16, Game1.tileSize * 11, Color.Red, (float)Game1.pixelZoom, false);
			for (int k = 1; k < 8; k++)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), (int)basePos.X - Game1.tileSize * 10 - Game1.pixelZoom * k, (int)basePos.Y - Game1.pixelZoom * k, Game1.tileSize * 16 + Game1.pixelZoom * 2 * k, Game1.tileSize * 11 + Game1.pixelZoom * 2 * k, Color.Red * (1f - (float)k * 0.15f), (float)Game1.pixelZoom, false);
			}
			for (int l = 0; l < 17; l++)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(147, 472, 3, 3), (int)basePos.X - Game1.tileSize * 10 + Game1.pixelZoom * 2, (int)basePos.Y + l * Game1.pixelZoom * 3 + Game1.pixelZoom * 3, (int)((float)Game1.tileSize * 9.5f - (float)(l * Game1.tileSize) * 1.2f + (float)(l * l * Game1.pixelZoom) * 0.7f), Game1.pixelZoom, new Color(l * 25, (l > 8) ? (l * 10) : 0, 255 - l * 25), (float)Game1.pixelZoom, false);
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
			b.End();
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00002834 File Offset: 0x00000A34
		public void changeScreenSize()
		{
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00002834 File Offset: 0x00000A34
		public void unload()
		{
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000CF1 RID: 3313
		public const float slotTurnRate = 0.008f;

		// Token: 0x04000CF2 RID: 3314
		public const int numberOfIcons = 8;

		// Token: 0x04000CF3 RID: 3315
		public const int defaultBet = 10;

		// Token: 0x04000CF4 RID: 3316
		private List<float> slots;

		// Token: 0x04000CF5 RID: 3317
		private List<float> slotResults;

		// Token: 0x04000CF6 RID: 3318
		private ClickableComponent spinButton10;

		// Token: 0x04000CF7 RID: 3319
		private ClickableComponent spinButton100;

		// Token: 0x04000CF8 RID: 3320
		private ClickableComponent doneButton;

		// Token: 0x04000CF9 RID: 3321
		private Random r;

		// Token: 0x04000CFA RID: 3322
		private bool spinning;

		// Token: 0x04000CFB RID: 3323
		private bool showResult;

		// Token: 0x04000CFC RID: 3324
		private float payoutModifier;

		// Token: 0x04000CFD RID: 3325
		private int currentBet;

		// Token: 0x04000CFE RID: 3326
		private int spinsCount;

		// Token: 0x04000CFF RID: 3327
		private int slotsFinished;

		// Token: 0x04000D00 RID: 3328
		private int endTimer;
	}
}
