using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Minigames
{
	// Token: 0x020000C8 RID: 200
	public class FishingGame : IMinigame
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x00104C9C File Offset: 0x00102E9C
		public FishingGame()
		{
			this.content = Game1.content.CreateTemporary();
			this.location = new GameLocation(this.content.Load<Map>("Maps\\FishingGame"), "fishingGame");
			Game1.player.CurrentToolIndex = 0;
			this.tempItemStash = Game1.player.addItemToInventory(new FishingRod(), 0);
			(Game1.player.CurrentTool as FishingRod).attachments[0] = new Object(690, 99, false, -1, 0);
			(Game1.player.CurrentTool as FishingRod).attachments[1] = new Object(687, 1, false, -1, 0);
			Game1.player.CurrentToolIndex = 0;
			Game1.globalFadeToClear(null, 0.01f);
			this.location.Map.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.player.position = new Vector2(14f, 7f) * (float)Game1.tileSize;
			Game1.player.currentLocation = this.location;
			this.changeScreenSize();
			this.gameEndTimer = 100000;
			this.showResultsTimer = -1;
			Game1.player.faceDirection(3);
			Game1.player.Halt();
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00104DE0 File Offset: 0x00102FE0
		public bool tick(GameTime time)
		{
			Rumble.update((float)time.ElapsedGameTime.Milliseconds);
			this.location.UpdateWhenCurrentLocation(time);
			this.location.updateEvenIfFarmerIsntHere(time, false);
			Game1.player.Stamina = (float)Game1.player.MaxStamina;
			Game1.player.Update(time, this.location);
			for (int i = Game1.screenOverlayTempSprites.Count - 1; i >= 0; i--)
			{
				if (Game1.screenOverlayTempSprites[i].update(time))
				{
					Game1.screenOverlayTempSprites.RemoveAt(i);
				}
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.updateActiveMenu(time);
			}
			if (this.timerToStart > 0)
			{
				Game1.player.faceDirection(3);
				this.timerToStart -= time.ElapsedGameTime.Milliseconds;
				if (this.timerToStart <= 0)
				{
					Game1.playSound("whistle");
				}
			}
			else if (this.showResultsTimer >= 0)
			{
				int arg_10C_0 = this.showResultsTimer;
				this.showResultsTimer -= time.ElapsedGameTime.Milliseconds;
				if (arg_10C_0 > 11000 && this.showResultsTimer <= 11000)
				{
					Game1.playSound("smallSelect");
				}
				if (arg_10C_0 > 9000 && this.showResultsTimer <= 9000)
				{
					Game1.playSound("smallSelect");
				}
				if (arg_10C_0 > 7000 && this.showResultsTimer <= 7000)
				{
					if (this.perfections > 0)
					{
						this.score += this.perfections * 10;
						this.perfectionBonus = this.perfections * 10;
						if (this.fishCaught >= 3 && this.perfections >= 3)
						{
							this.perfectionBonus += this.score;
							this.score *= 2;
						}
						Game1.playSound("newArtifact");
					}
					else
					{
						Game1.playSound("smallSelect");
					}
				}
				if (arg_10C_0 > 5000 && this.showResultsTimer <= 5000)
				{
					if (this.score >= 10)
					{
						Game1.playSound("reward");
						this.starTokensWon = (this.score + 5) / 10 * 6;
						Game1.player.festivalScore += this.starTokensWon;
					}
					else
					{
						Game1.playSound("fishEscape");
					}
				}
				if (this.showResultsTimer <= 0)
				{
					Game1.globalFadeToClear(null, 0.02f);
					return true;
				}
			}
			else if (!this.gameDone)
			{
				this.gameEndTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gameEndTimer <= 0 && Game1.activeClickableMenu == null && (!Game1.player.UsingTool || (Game1.player.CurrentTool as FishingRod).isFishing))
				{
					if (Game1.player.usingTool)
					{
						this.receiveLeftClick(0, 0, true);
						if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
						{
							(Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, false);
						}
					}
					Game1.player.completelyStopAnimatingOrDoingAction();
					Game1.playSound("whistle");
					this.gameEndTimer = 1000;
					this.gameDone = true;
				}
			}
			else if (this.gameDone && this.gameEndTimer > 0)
			{
				this.gameEndTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gameEndTimer <= 0)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.gameDoneAfterFade), 0.01f);
					Game1.exitActiveMenu();
					Game1.player.forceCanMove();
				}
			}
			return this.exit;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00105174 File Offset: 0x00103374
		public void gameDoneAfterFade()
		{
			this.showResultsTimer = 11100;
			Game1.player.canMove = false;
			Game1.player.position = new Vector2(24f, 71f) * (float)Game1.tileSize;
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(24 * Game1.tileSize, 71 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			Game1.player.faceDirection(2);
			Utility.killAllStaticLoopingSoundCues();
			if (FishingRod.reelSound != null && FishingRod.reelSound.IsPlaying)
			{
				FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00105214 File Offset: 0x00103414
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.isAnyGamePadButtonBeingPressed())
			{
				return;
			}
			if (this.timerToStart <= 0 && this.showResultsTimer < 0 && !this.gameDone && Game1.activeClickableMenu == null && !(Game1.player.CurrentTool as FishingRod).hit && !(Game1.player.CurrentTool as FishingRod).pullingOutOfWater && !(Game1.player.CurrentTool as FishingRod).isCasting && !(Game1.player.CurrentTool as FishingRod).fishCaught)
			{
				Game1.player.lastClick = Vector2.Zero;
				Game1.player.Halt();
				Game1.pressUseToolButton();
				return;
			}
			if (this.showResultsTimer > 11000)
			{
				this.showResultsTimer = 11001;
				return;
			}
			if (this.showResultsTimer > 9000)
			{
				this.showResultsTimer = 9001;
				return;
			}
			if (this.showResultsTimer > 7000)
			{
				this.showResultsTimer = 7001;
				return;
			}
			if (this.showResultsTimer > 5000)
			{
				this.showResultsTimer = 5001;
				return;
			}
			if (this.showResultsTimer < 5000 && this.showResultsTimer > 1000)
			{
				this.showResultsTimer = 1500;
				Game1.playSound("smallSelect");
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0010535C File Offset: 0x0010355C
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (!Game1.isAnyGamePadButtonBeingPressed())
			{
				return;
			}
			if (this.timerToStart <= 0 && this.showResultsTimer < 0 && !this.gameDone && Game1.activeClickableMenu == null && !(Game1.player.CurrentTool as FishingRod).hit && !(Game1.player.CurrentTool as FishingRod).pullingOutOfWater && !(Game1.player.CurrentTool as FishingRod).isCasting && !(Game1.player.CurrentTool as FishingRod).fishCaught)
			{
				Game1.player.lastClick = Vector2.Zero;
				Game1.player.Halt();
				Game1.pressUseToolButton();
				return;
			}
			if (this.showResultsTimer > 11000)
			{
				this.showResultsTimer = 11001;
				return;
			}
			if (this.showResultsTimer > 9000)
			{
				this.showResultsTimer = 9001;
				return;
			}
			if (this.showResultsTimer > 7000)
			{
				this.showResultsTimer = 7001;
				return;
			}
			if (this.showResultsTimer > 5000)
			{
				this.showResultsTimer = 5001;
				return;
			}
			if (this.showResultsTimer < 5000 && this.showResultsTimer > 1000)
			{
				this.showResultsTimer = 1500;
				Game1.playSound("smallSelect");
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x001054A4 File Offset: 0x001036A4
		public void releaseLeftClick(int x, int y)
		{
			if (this.showResultsTimer < 0 && Game1.player.CurrentTool != null && !(Game1.player.CurrentTool as FishingRod).isCasting && Game1.activeClickableMenu == null && Game1.player.CurrentTool.onRelease(this.location, x, y, Game1.player))
			{
				Game1.player.Halt();
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0010550C File Offset: 0x0010370C
		public void receiveKeyPress(Keys k)
		{
			if (!this.gameDone && Game1.player.movementDirections.Count < 2 && !Game1.player.UsingTool && this.timerToStart <= 0)
			{
				if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
				{
					Game1.player.setMoving(1);
				}
				if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
				{
					Game1.player.setMoving(2);
				}
				if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
				{
					Game1.player.setMoving(4);
				}
				if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
				{
					Game1.player.setMoving(8);
				}
			}
			if (Game1.options.doesInputListContain(Game1.options.runButton, k) || Game1.isGamePadThumbstickInMotion())
			{
				Game1.player.setRunning(true, false);
			}
			if (!this.gameDone && k == Keys.Escape)
			{
				if (Game1.activeClickableMenu == null)
				{
					this.gameEndTimer = 1;
				}
				else if (Game1.activeClickableMenu is BobberBar)
				{
					(Game1.activeClickableMenu as BobberBar).receiveKeyPress(k);
				}
			}
			if (k == Keys.End)
			{
				this.gameEndTimer -= 10000;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00105658 File Offset: 0x00103858
		public void receiveKeyRelease(Keys k)
		{
			if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
			{
				Game1.player.setMoving(33);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
			{
				Game1.player.setMoving(34);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
			{
				Game1.player.setMoving(36);
			}
			if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
			{
				Game1.player.setMoving(40);
			}
			if (Game1.options.doesInputListContain(Game1.options.runButton, k))
			{
				Game1.player.setRunning(false, false);
			}
			if (Game1.player.movementDirections.Count == 0 && !Game1.player.UsingTool)
			{
				Game1.player.Halt();
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0010573C File Offset: 0x0010393C
		public void draw(SpriteBatch b)
		{
			if (this.showResultsTimer < 0)
			{
				b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.mapDisplayDevice.BeginScene(b);
				this.location.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				this.location.drawWater(b);
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f - ((Game1.player.running || Game1.player.usingTool) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.8f) : 0f), SpriteEffects.None, Math.Max(0f, (float)Game1.player.getStandingY() / 10000f + 0.00011f) - 1E-07f);
				this.location.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				this.location.draw(b);
				if (Game1.player.UsingTool)
				{
					Game1.drawTool(Game1.player);
				}
				Game1.player.draw(b);
				this.location.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				if (Game1.lastCursorMotionWasMouse && !Game1.options.hardwareCursor)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				if (Game1.activeClickableMenu != null)
				{
					Game1.activeClickableMenu.draw(b);
				}
				b.DrawString(Game1.dialogueFont, "Time: " + Utility.getMinutesSecondsStringFromMilliseconds(Math.Max(0, this.gameEndTimer)), new Vector2((float)(Game1.tileSize / 4), (float)Game1.tileSize), Color.White);
				b.DrawString(Game1.dialogueFont, "Score: " + this.score, new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 2)), Color.White);
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.screenOverlayTempSprites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, false, 0, 0);
					}
				}
				b.End();
				return;
			}
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			Vector2 position = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - Game1.tileSize * 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - Game1.tileSize));
			if (this.showResultsTimer <= 11000)
			{
				Game1.drawWithBorder("Score: " + this.score, Game1.textColor, (this.showResultsTimer <= 7000 && this.perfectionBonus > 0) ? Color.Lime : Color.White, position);
			}
			if (this.showResultsTimer <= 9000)
			{
				position.Y += (float)(Game1.tileSize * 3 / 4);
				Game1.drawWithBorder("Fish Caught: " + this.fishCaught, Game1.textColor, Color.White, position);
			}
			if (this.showResultsTimer <= 7000)
			{
				position.Y += (float)(Game1.tileSize * 3 / 4);
				if (this.perfectionBonus > 1)
				{
					Game1.drawWithBorder(this.perfectionBonus + " Perfection Bonus!", Game1.textColor, Color.Yellow, position);
				}
				else
				{
					Game1.drawWithBorder("No Perfection Bonus", Game1.textColor, Color.Red, position);
				}
			}
			if (this.showResultsTimer <= 5000)
			{
				position.Y += (float)Game1.tileSize;
				if (this.starTokensWon > 0)
				{
					float fade = Math.Min(1f, (float)(this.showResultsTimer - 2000) / 4000f);
					Game1.drawWithBorder("Reward: " + this.starTokensWon + " Star Tokens", Game1.textColor * 0.2f * fade, Color.SkyBlue * 0.3f * fade, position + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder("Reward: " + this.starTokensWon + " Star Tokens", Game1.textColor * 0.2f * fade, Color.SkyBlue * 0.3f * fade, position + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder("Reward: " + this.starTokensWon + " Star Tokens", Game1.textColor * 0.2f * fade, Color.SkyBlue * 0.3f * fade, position + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder("Reward: " + this.starTokensWon + " Star Tokens", Game1.textColor, Color.SkyBlue, position, 0f, 1f, 1f);
				}
				else
				{
					Game1.drawWithBorder("No Reward... Try Harder", Game1.textColor, Color.Red, position);
				}
			}
			if (this.showResultsTimer <= 1000)
			{
				b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * (1f - (float)this.showResultsTimer / 1000f));
			}
			b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + ((Game1.player.festivalScore > 999) ? (Game1.tileSize / 4) : 0), Game1.tileSize), Color.Black * 0.75f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(338, 400, 8, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			Game1.drawWithBorder(string.Concat(Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2 + 10 * Game1.pixelZoom), (float)(Game1.tileSize / 3 + Game1.pixelZoom * 2)), 0f, 1f, 1f, false);
			b.End();
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00105F70 File Offset: 0x00104170
		public static void startMe()
		{
			Game1.currentMinigame = new FishingGame();
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00105F7C File Offset: 0x0010417C
		public void changeScreenSize()
		{
			Game1.viewport.X = this.location.Map.Layers[0].LayerWidth * Game1.tileSize / 2 - Game1.graphics.GraphicsDevice.Viewport.Width / 2;
			Game1.viewport.Y = this.location.Map.Layers[0].LayerHeight * Game1.tileSize / 2 - Game1.graphics.GraphicsDevice.Viewport.Height / 2;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00106018 File Offset: 0x00104218
		public void unload()
		{
			(Game1.player.CurrentTool as FishingRod).castingEndFunction(-1);
			(Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, false);
			Game1.player.addItemToInventory(this.tempItemStash, 0);
			Game1.player.currentLocation = Game1.currentLocation;
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.player.forceCanMove();
			Game1.player.faceDirection(2);
			this.content.Unload();
			this.content.Dispose();
			this.content = null;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000D01 RID: 3329
		private GameLocation location;

		// Token: 0x04000D02 RID: 3330
		private LocalizedContentManager content;

		// Token: 0x04000D03 RID: 3331
		private Item tempItemStash;

		// Token: 0x04000D04 RID: 3332
		private int timerToStart = 1000;

		// Token: 0x04000D05 RID: 3333
		private int gameEndTimer;

		// Token: 0x04000D06 RID: 3334
		private int showResultsTimer;

		// Token: 0x04000D07 RID: 3335
		private bool exit;

		// Token: 0x04000D08 RID: 3336
		private bool gameDone;

		// Token: 0x04000D09 RID: 3337
		public int score;

		// Token: 0x04000D0A RID: 3338
		public int fishCaught;

		// Token: 0x04000D0B RID: 3339
		public int starTokensWon;

		// Token: 0x04000D0C RID: 3340
		public int perfections;

		// Token: 0x04000D0D RID: 3341
		public int perfectionBonus;
	}
}
