using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
	// Token: 0x020000CB RID: 203
	public class FantasyBoardGame : IMinigame
	{
		// Token: 0x06000CF6 RID: 3318 RVA: 0x00107BEC File Offset: 0x00105DEC
		public FantasyBoardGame()
		{
			this.content = Game1.content.CreateTemporary();
			this.slides = this.content.Load<Texture2D>("LooseSprites\\boardGame");
			this.border = this.content.Load<Texture2D>("LooseSprites\\boardGameBorder");
			Game1.globalFadeToClear(null, 0.02f);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00107C78 File Offset: 0x00105E78
		public bool tick(GameTime time)
		{
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			Game1.currentLocation.currentEvent.checkForNextCommand(Game1.currentLocation, time);
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.update(time);
			}
			if (this.endTimer > 0)
			{
				this.endTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.endTimer <= 0 && this.whichSlide == -1)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.end), 0.02f);
				}
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.performHoverAction(Game1.getOldMouseX(), Game1.getOldMouseY());
			}
			return false;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00107D38 File Offset: 0x00105F38
		public void end()
		{
			this.unload();
			Event expr_10 = Game1.currentLocation.currentEvent;
			int currentCommand = expr_10.CurrentCommand;
			expr_10.CurrentCommand = currentCommand + 1;
			Game1.currentMinigame = null;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00107D6A File Offset: 0x00105F6A
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.receiveLeftClick(x, y, true);
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00107D80 File Offset: 0x00105F80
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
			Game1.pressActionButton(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(PlayerIndex.One));
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.receiveRightClick(x, y, true);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00107DAC File Offset: 0x00105FAC
		public void receiveKeyPress(Keys k)
		{
			if (Game1.isQuestion)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					Game1.currentQuestionChoice = Math.Max(Game1.currentQuestionChoice - 1, 0);
					Game1.playSound("toolSwap");
					return;
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					Game1.currentQuestionChoice = Math.Min(Game1.currentQuestionChoice + 1, Game1.questionChoices.Count - 1);
					Game1.playSound("toolSwap");
				}
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00107E34 File Offset: 0x00106034
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (this.whichSlide >= 0)
			{
				Vector2 offset = default(Vector2);
				if (this.shakeTimer > 0)
				{
					offset = new Vector2((float)Game1.random.Next(-2, 2), (float)Game1.random.Next(-2, 2));
				}
				b.Draw(this.border, offset + new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.borderSourceWidth * Game1.pixelZoom / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - this.borderSourceHeight * Game1.pixelZoom / 2 - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, 0, this.borderSourceWidth, this.borderSourceHeight)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				b.Draw(this.slides, offset + new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.slideSourceWidth * Game1.pixelZoom / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - this.slideSourceHeight * Game1.pixelZoom / 2 - Game1.tileSize * 2)), new Rectangle?(new Rectangle(this.whichSlide % 2 * this.slideSourceWidth, this.whichSlide / 2 * this.slideSourceHeight, this.slideSourceWidth, this.slideSourceHeight)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.01f);
			}
			else
			{
				string s = string.Concat(new string[]
				{
					"You finished the Scenario with ",
					Game1.getProperArticleForWord(this.grade),
					" '",
					this.grade,
					"' rating."
				});
				float yOffset = (float)Math.Sin((double)(this.endTimer / 1000)) * (float)(Game1.pixelZoom * 2);
				Game1.drawWithBorder(s, Game1.textColor, Color.Purple, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(s).X / 2f, yOffset + (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2)));
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.draw(b);
			}
			b.End();
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00002834 File Offset: 0x00000A34
		public void changeScreenSize()
		{
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x001080CF File Offset: 0x001062CF
		public void unload()
		{
			this.content.Unload();
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x001080DC File Offset: 0x001062DC
		public void afterFade()
		{
			this.whichSlide = -1;
			int score = 0;
			if (Game1.player.mailReceived.Contains("savedFriends"))
			{
				score++;
			}
			if (Game1.player.mailReceived.Contains("destroyedPods"))
			{
				score++;
			}
			if (Game1.player.mailReceived.Contains("killedSkeleton"))
			{
				score++;
			}
			switch (score)
			{
			case 0:
				this.grade = "D";
				break;
			case 1:
				this.grade = "C";
				break;
			case 2:
				this.grade = "B";
				break;
			case 3:
				this.grade = "A";
				break;
			}
			Game1.playSound("newArtifact");
			this.endTimer = 5500;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0010819F File Offset: 0x0010639F
		public void receiveEventPoke(int data)
		{
			if (data == -1)
			{
				this.shakeTimer = 1000;
				return;
			}
			if (data == -2)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 0.02f);
				return;
			}
			this.whichSlide = data;
		}

		// Token: 0x04000D25 RID: 3365
		public int borderSourceWidth = 138;

		// Token: 0x04000D26 RID: 3366
		public int borderSourceHeight = 74;

		// Token: 0x04000D27 RID: 3367
		public int slideSourceWidth = 128;

		// Token: 0x04000D28 RID: 3368
		public int slideSourceHeight = 64;

		// Token: 0x04000D29 RID: 3369
		private LocalizedContentManager content;

		// Token: 0x04000D2A RID: 3370
		private Texture2D slides;

		// Token: 0x04000D2B RID: 3371
		private Texture2D border;

		// Token: 0x04000D2C RID: 3372
		public int whichSlide;

		// Token: 0x04000D2D RID: 3373
		public int shakeTimer;

		// Token: 0x04000D2E RID: 3374
		public int endTimer;

		// Token: 0x04000D2F RID: 3375
		private string grade = "";
	}
}
