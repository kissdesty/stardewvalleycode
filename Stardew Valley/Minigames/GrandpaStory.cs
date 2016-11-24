using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using xTile;

namespace StardewValley.Minigames
{
	// Token: 0x020000C6 RID: 198
	public class GrandpaStory : IMinigame
	{
		// Token: 0x06000CA7 RID: 3239 RVA: 0x00101650 File Offset: 0x000FF850
		public GrandpaStory()
		{
			Game1.changeMusicTrack("none");
			this.temporaryContent = Game1.content.CreateTemporary();
			this.texture = this.temporaryContent.Load<Texture2D>("Minigames\\jojacorps");
			this.backgroundFadeChange = 0.0003f;
			this.grandpaSpeech = new Queue<string>();
			this.grandpaSpeech.Enqueue("...and for my very special " + (Game1.player.isMale ? "grandson:" : "granddaughter:"));
			this.grandpaSpeech.Enqueue("I want you to have this sealed envelope.");
			this.grandpaSpeech.Enqueue("No, no, don't open it yet... have patience.");
			this.grandpaSpeech.Enqueue("Now, listen close...");
			this.grandpaSpeech.Enqueue("There will come a day when you feel crushed by the burden of modern life...");
			this.grandpaSpeech.Enqueue("...and your bright spirit will fade before a growing emptiness.");
			this.grandpaSpeech.Enqueue("When that happens," + (Game1.player.isMale ? " my boy," : " my dear,") + " you'll be ready for this gift.");
			this.grandpaSpeech.Enqueue("Now, let Grandpa rest...");
			Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3000f, 376f);
			Game1.viewport.X = 0;
			Game1.viewport.Y = 0;
			Map expr_1A2 = this.temporaryContent.Load<Map>("Maps\\FarmHouse");
			expr_1A2.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.currentLocation = new FarmHouse(expr_1A2, "FarmHouse");
			Game1.player.currentLocation = Game1.currentLocation;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00101828 File Offset: 0x000FFA28
		public bool tick(GameTime time)
		{
			if (this.quit)
			{
				this.unload();
				Game1.currentMinigame = new Intro();
				return false;
			}
			if (this.letterView != null)
			{
				this.letterView.update(time);
			}
			this.totalMilliseconds += time.ElapsedGameTime.Milliseconds;
			this.totalMilliseconds %= 9000000;
			this.backgroundFade += this.backgroundFadeChange * (float)time.ElapsedGameTime.Milliseconds;
			this.backgroundFade = Math.Max(0f, Math.Min(1f, this.backgroundFade));
			this.foregroundFade += this.foregroundFadeChange * (float)time.ElapsedGameTime.Milliseconds;
			this.foregroundFade = Math.Max(0f, Math.Min(1f, this.foregroundFade));
			int old = this.grandpaSpeechTimer;
			if (this.foregroundFade >= 1f && this.fadingToQuit)
			{
				this.unload();
				Game1.currentMinigame = new Intro();
				return false;
			}
			switch (this.scene)
			{
			case 0:
				if (this.backgroundFade == 1f)
				{
					if (!this.drawGrandpa)
					{
						this.foregroundFade = 1f;
						this.foregroundFadeChange = -0.0005f;
						this.drawGrandpa = true;
					}
					if (this.foregroundFade == 0f)
					{
						this.scene = 1;
						Game1.changeMusicTrack("grandpas_theme");
					}
				}
				break;
			case 1:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer >= 60000)
				{
					this.foregroundFadeChange = 0.0005f;
				}
				if (this.foregroundFade >= 1f)
				{
					this.drawGrandpa = false;
					this.scene = 3;
					this.grandpaSpeechTimer = 0;
					this.foregroundFade = 0f;
					this.foregroundFadeChange = 0f;
				}
				if (old % 10000 > this.grandpaSpeechTimer % 10000 && this.grandpaSpeech.Count > 0)
				{
					this.grandpaSpeech.Dequeue();
				}
				if (old < 25000 && this.grandpaSpeechTimer > 25000 && this.grandpaSpeech.Count > 0)
				{
					this.grandpaSpeech.Dequeue();
				}
				if (old < 17000 && this.grandpaSpeechTimer >= 17000)
				{
					Game1.playSound("newRecipe");
					this.letterReceived = true;
					this.letterDy = -0.6f;
					this.letterDyDy = 0.001f;
				}
				if (this.letterReceived && this.letterPosition.Y <= (float)Game1.graphics.GraphicsDevice.Viewport.Height)
				{
					this.letterDy += this.letterDyDy * (float)time.ElapsedGameTime.Milliseconds;
					this.letterPosition.Y = this.letterPosition.Y + this.letterDy * (float)time.ElapsedGameTime.Milliseconds;
					this.letterPosition.X = this.letterPosition.X + 0.01f * (float)time.ElapsedGameTime.Milliseconds;
					this.letterScale += 0.00125f * (float)time.ElapsedGameTime.Milliseconds;
					if (this.letterPosition.Y > (float)Game1.graphics.GraphicsDevice.Viewport.Height)
					{
						Game1.playSound("coin");
					}
				}
				break;
			case 3:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer > 2600 && old <= 2600)
				{
					Game1.changeMusicTrack("jojaOfficeSoundscape");
				}
				else if (this.grandpaSpeechTimer > 4000)
				{
					this.grandpaSpeechTimer = 0;
					this.scene = 4;
				}
				break;
			case 4:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer >= 9000)
				{
					this.grandpaSpeechTimer = 0;
					this.scene = 5;
					Game1.player.faceDirection(1);
					Game1.player.currentEyes = 1;
				}
				if (this.grandpaSpeechTimer >= 7000)
				{
					Game1.viewport.X = 0;
					Game1.viewport.Y = 0;
					this.panX -= 0.2f * (float)time.ElapsedGameTime.Milliseconds;
					Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
				}
				break;
			case 5:
				if (this.panX > (float)(-1200 * Game1.pixelZoom + Math.Max(400 * Game1.pixelZoom, Game1.graphics.GraphicsDevice.Viewport.Width)))
				{
					Game1.viewport.X = 0;
					Game1.viewport.Y = 0;
					this.panX -= 0.2f * (float)time.ElapsedGameTime.Milliseconds;
					Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
				}
				else
				{
					this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
					if (old < 2000 && this.grandpaSpeechTimer >= 2000)
					{
						Game1.player.currentEyes = 4;
					}
					if (old < 3000 && this.grandpaSpeechTimer >= 3000)
					{
						Game1.player.currentEyes = 1;
						Game1.player.jitterStrength = 1f;
					}
					if (old < 3500 && this.grandpaSpeechTimer >= 3500)
					{
						Game1.player.stopJittering();
					}
					if (old < 4000 && this.grandpaSpeechTimer >= 4000)
					{
						Game1.player.currentEyes = 1;
						Game1.player.jitterStrength = 1f;
					}
					if (old < 4500 && this.grandpaSpeechTimer >= 4500)
					{
						Game1.player.stopJittering();
						Game1.player.doEmote(28);
					}
					if (old < 7000 && this.grandpaSpeechTimer >= 7000)
					{
						Game1.player.currentEyes = 4;
					}
					if (old < 8000 && this.grandpaSpeechTimer >= 8000)
					{
						Game1.player.showFrame(33, false);
					}
					if (this.grandpaSpeechTimer >= 10000)
					{
						this.scene = 6;
						this.grandpaSpeechTimer = 0;
					}
					Game1.player.position = new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
				}
				break;
			case 6:
				this.grandpaSpeechTimer += time.ElapsedGameTime.Milliseconds;
				if (this.grandpaSpeechTimer >= 2000)
				{
					this.parallaxPan += (int)Math.Ceiling(0.1 * (double)time.ElapsedGameTime.Milliseconds);
					if (this.parallaxPan >= 107)
					{
						this.parallaxPan = 107;
					}
				}
				if (old < 3500 && this.grandpaSpeechTimer >= 3500)
				{
					Game1.changeMusicTrack("none");
				}
				if (old < 5000 && this.grandpaSpeechTimer >= 5000)
				{
					Game1.playSound("doorCreak");
				}
				if (old < 6000 && this.grandpaSpeechTimer >= 6000)
				{
					this.mouseActive = true;
					Mouse.SetPosition(Game1.viewport.Width / 2, Game1.viewport.Height / 2);
				}
				if (this.clickedLetter)
				{
					this.letterOpenTimer += time.ElapsedGameTime.Milliseconds;
				}
				break;
			}
			Game1.player.updateEmote(time);
			if (Game1.player.jitterStrength > 0f)
			{
				Game1.player.jitter = new Vector2((float)Game1.random.Next(-(int)(Game1.player.jitterStrength * 100f), (int)((Game1.player.jitterStrength + 1f) * 100f)) / 100f, (float)Game1.random.Next(-(int)(Game1.player.jitterStrength * 100f), (int)((Game1.player.jitterStrength + 1f) * 100f)) / 100f);
			}
			return false;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00002834 File Offset: 0x00000A34
		public void afterFade()
		{
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0010211C File Offset: 0x0010031C
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.clickedLetter && this.mouseActive && new Rectangle((int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).X + (286 - this.parallaxPan) * 4, (int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 218 + Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8)), 524, 344).Contains(x, y))
			{
				this.clickedLetter = true;
				Game1.playSound("newRecipe");
				Game1.changeMusicTrack("musicboxsong");
				this.letterView = new LetterViewerMenu(string.Concat(new string[]
				{
					"Dear ",
					Game1.player.name,
					",^^If you're reading this, you must be in dire need of a change.^^The same thing happened to me, long ago. I'd lost sight of what mattered most in life... real connections with other people and nature. So I dropped everything and moved to the place I truly belong.^^^I've enclosed the deed to that place... my pride and joy: ",
					Game1.player.farmName,
					" Farm. It's located in Stardew Valley, on the southern coast. It's the perfect place to start your new life.^^This was my most precious gift of all, and now it's yours. I know you'll honor the family name, my ",
					Game1.player.isMale ? "boy" : "dear",
					". Good luck.^^Love, Grandpa^^P.S. If Lewis is still alive say hi to the old guy for me, will ya?"
				}));
				this.letterView.exitFunction = new IClickableMenu.onExit(this.onLetterExit);
			}
			if (this.letterView != null)
			{
				this.letterView.receiveLeftClick(x, y, true);
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0010226F File Offset: 0x0010046F
		public void onLetterExit()
		{
			this.mouseActive = false;
			this.foregroundFadeChange = 0.0003f;
			this.fadingToQuit = true;
			if (this.letterView != null)
			{
				this.letterView.unload();
				this.letterView = null;
			}
			Game1.playSound("newRecipe");
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x001022B0 File Offset: 0x001004B0
		public void receiveKeyPress(Keys k)
		{
			if (k == Keys.Escape)
			{
				if (!this.quit && !this.fadingToQuit)
				{
					Game1.playSound("bigDeSelect");
				}
				if (this.letterView != null)
				{
					this.letterView.unload();
					this.letterView = null;
				}
				this.quit = true;
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00102300 File Offset: 0x00100500
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Color(64, 136, 248));
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Black * this.backgroundFade);
			if (this.drawGrandpa)
			{
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(427, (this.totalMilliseconds % 300 < 150) ? 240 : 0, 427, 240)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(317f, 74f) * 3f, new Rectangle?(new Rectangle(427 + 74 * (this.totalMilliseconds % 400 / 100), 480, 74, 42)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(320f, 75f) * 3f, new Rectangle?(new Rectangle(427, 522, 70, 32)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				if (this.grandpaSpeechTimer > 8000 && this.grandpaSpeechTimer % 10000 < 5000)
				{
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(189f, 69f) * 3f, new Rectangle?(new Rectangle(497 + 18 * (this.totalMilliseconds % 400 / 200), 523, 18, 18)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				}
				if (this.grandpaSpeech.Count > 0 && this.grandpaSpeechTimer > 3000)
				{
					b.DrawString(Game1.dialogueFont, this.grandpaSpeech.Peek(), new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(this.grandpaSpeech.Peek()).X / 2f - 3f, (float)((int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 669) + 3f), Color.White * 0.25f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
					b.DrawString(Game1.dialogueFont, this.grandpaSpeech.Peek(), new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(this.grandpaSpeech.Peek()).X / 2f, (float)((int)Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 669)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
				}
				if (this.letterReceived)
				{
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(157f, 113f) * 3f, new Rectangle?(new Rectangle(463, 556, 37, 17)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					if (this.grandpaSpeechTimer > 8000 && this.grandpaSpeechTimer % 10000 > 7000 && this.grandpaSpeechTimer % 10000 < 9000 && this.totalMilliseconds % 600 < 300)
					{
						b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(157f, 113f) * 3f, new Rectangle?(new Rectangle(500, 556, 37, 17)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
					}
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + this.letterPosition, new Rectangle?(new Rectangle(729, 524, 131, 63)), Color.White, 0f, Vector2.Zero, this.letterScale, SpriteEffects.None, 1f);
				}
			}
			else if (this.scene == 3)
			{
				SpriteText.drawString(b, "XX Years later", (int)Utility.getTopLeftPositionForCenteringOnScreen(0, 0, -200, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(0, 0, 0, -50).Y, 999, -1, 999, 1f, 1f, false, -1, "", 4);
			}
			else if (this.scene == 4)
			{
				float alpha = 1f - ((float)this.grandpaSpeechTimer - 7000f) / 2000f;
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(0, 0, 427, 240)), Color.White * alpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(22f, 211f) * 3f, new Rectangle?(new Rectangle(264 + this.totalMilliseconds % 500 / 250 * 19, 581, 19, 17)), Color.White * alpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(332f, 215f) * 3f, new Rectangle?(new Rectangle(305 + this.totalMilliseconds % 600 / 200 * 12, 581, 12, 12)), Color.White * alpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(414f, 211f) * 3f, new Rectangle?(new Rectangle(460 + this.totalMilliseconds % 400 / 200 * 13, 581, 13, 17)), Color.White * alpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(189f, 81f) * 3f, new Rectangle?(new Rectangle(426 + this.totalMilliseconds % 800 / 400 * 16, 581, 16, 16)), Color.White * alpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
			}
			if ((this.scene == 4 && this.grandpaSpeechTimer >= 5000) || this.scene == 5)
			{
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)), new Rectangle?(new Rectangle(0, 600, 1200, 180)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(1080f, 524f), new Rectangle?(new Rectangle(350 + this.totalMilliseconds % 800 / 400 * 14, 581, 14, 9)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(1564f, 520f), new Rectangle?(new Rectangle(383 + this.totalMilliseconds % 400 / 200 * 9, 581, 9, 7)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(2632f, 520f), new Rectangle?(new Rectangle(403 + this.totalMilliseconds % 600 / 300 * 8, 582, 8, 8)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(2604f, 504f), new Rectangle?(new Rectangle(364 + this.totalMilliseconds % 1100 / 100 * 5, 594, 5, 3)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3116f, 492f), new Rectangle?(new Rectangle(343 + this.totalMilliseconds % 3000 / 1000 * 6, 593, 6, 5)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				if (this.scene == 5)
				{
					Game1.player.draw(b);
				}
				b.Draw(this.texture, new Vector2(this.panX, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3580f, 540f), new Rectangle?(new Rectangle(895, 735, 29, 36)), Color.White * ((this.scene == 5) ? 1f : (((float)this.grandpaSpeechTimer - 7000f) / 2000f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
			}
			if (this.scene == 6)
			{
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float)(261 - this.parallaxPan), 145f) * 4f, new Rectangle?(new Rectangle(550, 540, 56 + this.parallaxPan, 35)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float)(261 - this.parallaxPan), 4f + (float)Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8))) * 4f, new Rectangle?(new Rectangle(264, 434, 56 + this.parallaxPan, 141)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				if (this.grandpaSpeechTimer > 3000)
				{
					b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float)(286 - this.parallaxPan), 32f + (float)Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8)) + Math.Min(30f, (float)this.letterOpenTimer / 4f)) * 4f, new Rectangle?(new Rectangle(729 + Math.Min(2, this.letterOpenTimer / 200) * 131, 508, 131, 79)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
				}
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(this.parallaxPan, 240, 320, 180)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(187f - (float)this.parallaxPan * 2.5f, 8f) * 4f, new Rectangle?(new Rectangle(20, 428, 232, 172)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
			}
			if (this.letterView != null)
			{
				this.letterView.draw(b);
			}
			if (this.mouseActive)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), this.fadingToQuit ? (new Color(64, 136, 248) * this.foregroundFade) : (Color.Black * this.foregroundFade));
			b.End();
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x001034AB File Offset: 0x001016AB
		public void changeScreenSize()
		{
			Game1.viewport.X = 0;
			Game1.viewport.Y = 0;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x001034C3 File Offset: 0x001016C3
		public void unload()
		{
			this.temporaryContent.Unload();
			this.temporaryContent = null;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000CD0 RID: 3280
		public const int sceneWidth = 1294;

		// Token: 0x04000CD1 RID: 3281
		public const int sceneHeight = 730;

		// Token: 0x04000CD2 RID: 3282
		public const int scene_beforeGrandpa = 0;

		// Token: 0x04000CD3 RID: 3283
		public const int scene_grandpaSpeech = 1;

		// Token: 0x04000CD4 RID: 3284
		public const int scene_fadeOutFromGrandpa = 2;

		// Token: 0x04000CD5 RID: 3285
		public const int scene_timePass = 3;

		// Token: 0x04000CD6 RID: 3286
		public const int scene_jojaCorpOverhead = 4;

		// Token: 0x04000CD7 RID: 3287
		public const int scene_jojaCorpPan = 5;

		// Token: 0x04000CD8 RID: 3288
		public const int scene_desk = 6;

		// Token: 0x04000CD9 RID: 3289
		private LocalizedContentManager temporaryContent;

		// Token: 0x04000CDA RID: 3290
		private Texture2D texture;

		// Token: 0x04000CDB RID: 3291
		private float foregroundFade;

		// Token: 0x04000CDC RID: 3292
		private float backgroundFade;

		// Token: 0x04000CDD RID: 3293
		private float foregroundFadeChange;

		// Token: 0x04000CDE RID: 3294
		private float backgroundFadeChange;

		// Token: 0x04000CDF RID: 3295
		private float panX;

		// Token: 0x04000CE0 RID: 3296
		private float letterScale = 0.5f;

		// Token: 0x04000CE1 RID: 3297
		private float letterDy;

		// Token: 0x04000CE2 RID: 3298
		private float letterDyDy;

		// Token: 0x04000CE3 RID: 3299
		private int scene;

		// Token: 0x04000CE4 RID: 3300
		private int totalMilliseconds;

		// Token: 0x04000CE5 RID: 3301
		private int grandpaSpeechTimer;

		// Token: 0x04000CE6 RID: 3302
		private int parallaxPan;

		// Token: 0x04000CE7 RID: 3303
		private int letterOpenTimer;

		// Token: 0x04000CE8 RID: 3304
		private bool drawGrandpa;

		// Token: 0x04000CE9 RID: 3305
		private bool letterReceived;

		// Token: 0x04000CEA RID: 3306
		private bool mouseActive;

		// Token: 0x04000CEB RID: 3307
		private bool clickedLetter;

		// Token: 0x04000CEC RID: 3308
		private bool quit;

		// Token: 0x04000CED RID: 3309
		private bool fadingToQuit;

		// Token: 0x04000CEE RID: 3310
		private Queue<string> grandpaSpeech;

		// Token: 0x04000CEF RID: 3311
		private Vector2 letterPosition = new Vector2(477f, 345f);

		// Token: 0x04000CF0 RID: 3312
		private LetterViewerMenu letterView;
	}
}
