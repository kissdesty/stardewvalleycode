using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Projectiles;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Minigames
{
	// Token: 0x020000C9 RID: 201
	public class TargetGame : IMinigame
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x001060B4 File Offset: 0x001042B4
		public TargetGame()
		{
			TargetGame.score = 0;
			TargetGame.successShots = 0;
			TargetGame.shotsFired = 0;
			this.content = Game1.content.CreateTemporary();
			this.location = new GameLocation(this.content.Load<Map>("Maps\\TargetGame"), "tent");
			Slingshot slingshot = new Slingshot();
			slingshot.attachments[0] = new Object(390, 999, false, -1, 0);
			this.tempItemStash = Game1.player.addItemToInventory(slingshot, 0);
			Game1.player.CurrentToolIndex = 0;
			Game1.globalFadeToClear(null, 0.01f);
			this.location.Map.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.player.position = new Vector2(8f, 13f) * (float)Game1.tileSize;
			this.changeScreenSize();
			this.gameEndTimer = 50000;
			this.targets = new List<TargetGame.Target>();
			this.addTargets();
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x001061CC File Offset: 0x001043CC
		public bool tick(GameTime time)
		{
			this.location.UpdateWhenCurrentLocation(time);
			this.location.updateEvenIfFarmerIsntHere(time, false);
			Game1.player.Stamina = (float)Game1.player.MaxStamina;
			Game1.player.Update(time, this.location);
			if ((Game1.oldKBState.GetPressedKeys().Length == 0 || (Game1.oldKBState.GetPressedKeys().Length == 1 && Game1.options.doesInputListContain(Game1.options.runButton, Game1.oldKBState.GetPressedKeys()[0])) || !Game1.player.movedDuringLastTick()) && !Game1.player.UsingTool)
			{
				Game1.player.Halt();
			}
			if (this.timerToStart > 0)
			{
				this.timerToStart -= time.ElapsedGameTime.Milliseconds;
				if (this.timerToStart <= 0)
				{
					Game1.playSound("whistle");
					Game1.changeMusicTrack("tickTock");
				}
			}
			else if (this.showResultsTimer >= 0)
			{
				int arg_115_0 = this.showResultsTimer;
				this.showResultsTimer -= time.ElapsedGameTime.Milliseconds;
				if (arg_115_0 > 16000 && this.showResultsTimer <= 16000)
				{
					Game1.playSound("smallSelect");
				}
				if (arg_115_0 > 14000 && this.showResultsTimer <= 14000)
				{
					Game1.playSound("smallSelect");
					TargetGame.accuracy = (int)(Math.Round((double)((float)TargetGame.successShots / (float)(TargetGame.shotsFired - 1)), 2) * 100.0);
				}
				if (arg_115_0 > 11000 && this.showResultsTimer <= 11000)
				{
					if (TargetGame.accuracy >= 75)
					{
						Game1.playSound("newArtifact");
						float modifier = 1.5f;
						if (TargetGame.accuracy >= 85)
						{
							modifier = 2f;
						}
						if (TargetGame.accuracy >= 90)
						{
							modifier = 2.5f;
						}
						if (TargetGame.accuracy >= 95)
						{
							modifier = 3f;
						}
						if (TargetGame.accuracy >= 100)
						{
							modifier = 4f;
						}
						TargetGame.score = (int)((float)TargetGame.score * modifier);
						this.modifierBonus = modifier;
					}
					else
					{
						Game1.playSound("smallSelect");
					}
				}
				if (arg_115_0 > 9000 && this.showResultsTimer <= 9000)
				{
					TargetGame.score *= 2;
					if (TargetGame.score >= 80)
					{
						Game1.playSound("reward");
						TargetGame.starTokensWon = (int)((float)((TargetGame.score - 30) / 10) * 2.5f);
						if (TargetGame.starTokensWon > 140)
						{
							TargetGame.starTokensWon = 250;
						}
						Game1.player.festivalScore += TargetGame.starTokensWon;
					}
					else
					{
						Game1.playSound("fishEscape");
					}
				}
				if (this.showResultsTimer <= 0)
				{
					Game1.globalFadeToClear(null, 0.02f);
					Game1.player.position = new Vector2(24f, 63f) * (float)Game1.tileSize;
					return true;
				}
			}
			else if (!this.gameDone)
			{
				this.gameEndTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gameEndTimer <= 0)
				{
					Game1.playSound("whistle");
					this.gameEndTimer = 1000;
					Game1.player.completelyStopAnimatingOrDoingAction();
					Game1.player.canMove = false;
					this.gameDone = true;
				}
				for (int i = this.targets.Count - 1; i >= 0; i--)
				{
					if (this.targets[i].update(time, this.location))
					{
						this.targets.RemoveAt(i);
					}
				}
			}
			else if (this.gameDone && this.gameEndTimer > 0)
			{
				this.gameEndTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.gameEndTimer <= 0)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.gameDoneAfterFade), 0.01f);
					Game1.player.forceCanMove();
				}
			}
			return this.exit;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0010659C File Offset: 0x0010479C
		public void gameDoneAfterFade()
		{
			this.showResultsTimer = 16100;
			Game1.player.canMove = false;
			Game1.player.freezePause = 16100;
			Game1.player.position = new Vector2(24f, 63f) * (float)Game1.tileSize;
			Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(24 * Game1.tileSize, 63 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			Game1.player.faceDirection(2);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00106628 File Offset: 0x00104828
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.showResultsTimer < 0)
			{
				Game1.pressUseToolButton();
				return;
			}
			if (this.showResultsTimer > 16000)
			{
				this.showResultsTimer = 16001;
				return;
			}
			if (this.showResultsTimer > 14000)
			{
				this.showResultsTimer = 14001;
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
			if (this.showResultsTimer < 9000 && this.showResultsTimer > 1000)
			{
				this.showResultsTimer = 1500;
				Game1.player.freezePause = 1500;
				Game1.playSound("smallSelect");
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x001066E8 File Offset: 0x001048E8
		public void releaseLeftClick(int x, int y)
		{
			int projectileCount = this.location.projectiles.Count;
			if (this.showResultsTimer < 0 && Game1.player.CurrentTool != null && Game1.player.CurrentTool.onRelease(this.location, x, y, Game1.player))
			{
				Game1.player.usingSlingshot = false;
				Game1.player.canReleaseTool = true;
				Game1.player.usingTool = false;
				Game1.player.CanMove = true;
				if (this.location.projectiles.Count > projectileCount)
				{
					TargetGame.shotsFired++;
				}
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00106784 File Offset: 0x00104984
		public void receiveKeyPress(Keys k)
		{
			if (this.showResultsTimer > 0 || this.gameEndTimer > 0)
			{
				Game1.player.Halt();
				return;
			}
			if (Game1.player.movementDirections.Count < 2 && !Game1.player.UsingTool && this.timerToStart <= 0)
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
			if (Game1.options.doesInputListContain(Game1.options.runButton, k))
			{
				Game1.player.setRunning(true, false);
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0010688C File Offset: 0x00104A8C
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
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00106948 File Offset: 0x00104B48
		public void draw(SpriteBatch b)
		{
			if (this.showResultsTimer < 0)
			{
				b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.mapDisplayDevice.BeginScene(b);
				this.location.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f - ((Game1.player.running || Game1.player.usingTool) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.8f) : 0f), SpriteEffects.None, Math.Max(0f, (float)Game1.player.getStandingY() / 10000f + 0.00011f) - 1E-07f);
				this.location.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.mapDisplayDevice.EndScene();
				b.End();
				b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				this.location.draw(b);
				Game1.player.draw(b);
				if (!Game1.options.hardwareCursor)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				using (List<TargetGame.Target>.Enumerator enumerator = this.targets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				b.End();
				b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.mapDisplayDevice.BeginScene(b);
				this.location.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.mapDisplayDevice.EndScene();
				if (!Game1.options.hardwareCursor)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				Game1.player.CurrentTool.draw(b);
				Game1.drawWithBorder("Score: " + TargetGame.score, Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
				Game1.drawWithBorder("Time: " + this.gameEndTimer / 1000, Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize));
				if (TargetGame.shotsFired > 1)
				{
					Game1.drawWithBorder("Acc.: " + (int)(Math.Round((double)((float)TargetGame.successShots / (float)(TargetGame.shotsFired - 1)), 2) * 100.0) + "%", Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2)));
				}
				b.End();
				return;
			}
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			Vector2 position = new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 2), (float)(Game1.viewport.Height / 2 - Game1.tileSize));
			if (this.showResultsTimer <= 16000)
			{
				Game1.drawWithBorder("Score: " + TargetGame.score, Game1.textColor, (this.showResultsTimer <= 11000 && this.modifierBonus > 1f) ? Color.Lime : Color.White, position);
			}
			if (this.showResultsTimer <= 14000)
			{
				position.Y += (float)(Game1.tileSize * 3 / 4);
				Game1.drawWithBorder(string.Concat(new object[]
				{
					"Accuracy: ",
					TargetGame.accuracy,
					"%      ",
					TargetGame.successShots,
					" / ",
					TargetGame.shotsFired,
					" Succesful Shots"
				}), Game1.textColor, Color.White, position);
			}
			if (this.showResultsTimer <= 11000)
			{
				position.Y += (float)(Game1.tileSize * 3 / 4);
				if (this.modifierBonus > 1f)
				{
					Game1.drawWithBorder("x" + this.modifierBonus + " Accuracy Multiplier!", Game1.textColor, Color.Yellow, position);
				}
				else
				{
					Game1.drawWithBorder("No Accuracy Bonus", Game1.textColor, Color.Red, position);
				}
			}
			if (this.showResultsTimer <= 9000)
			{
				position.Y += (float)Game1.tileSize;
				if (TargetGame.starTokensWon > 0)
				{
					float fade = Math.Min(1f, (float)(this.showResultsTimer - 2000) / 4000f);
					Game1.drawWithBorder("Reward: " + TargetGame.starTokensWon + " Star Tokens", Game1.textColor * 0.2f * fade, Color.SkyBlue * 0.3f * fade, position + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder("Reward: " + TargetGame.starTokensWon + " Star Tokens", Game1.textColor * 0.2f * fade, Color.SkyBlue * 0.3f * fade, position + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder("Reward: " + TargetGame.starTokensWon + " Star Tokens", Game1.textColor * 0.2f * fade, Color.SkyBlue * 0.3f * fade, position + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) * (float)Game1.pixelZoom * 2f, 0f, 1f, 1f);
					Game1.drawWithBorder("Reward: " + TargetGame.starTokensWon + " Star Tokens", Game1.textColor, Color.SkyBlue, position, 0f, 1f, 1f);
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

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00107288 File Offset: 0x00105488
		public static void startMe()
		{
			Game1.currentMinigame = new TargetGame();
			Game1.changeMusicTrack("none");
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x001072A0 File Offset: 0x001054A0
		public void changeScreenSize()
		{
			Game1.viewport.X = this.location.Map.Layers[0].LayerWidth * Game1.tileSize / 2 - Game1.viewport.Width / 2;
			Game1.viewport.Y = this.location.Map.Layers[0].LayerHeight * Game1.tileSize / 2 - Game1.viewport.Height / 2;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00107324 File Offset: 0x00105524
		public void unload()
		{
			Game1.player.addItemToInventory(this.tempItemStash, 0);
			Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.player.forceCanMove();
			this.content.Unload();
			this.content.Dispose();
			this.content = null;
			Game1.changeMusicTrack("fallFest");
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00107388 File Offset: 0x00105588
		public void addTargets()
		{
			this.addRowOfTargetsOnLane(0, TargetGame.Target.middleLane, 1500, 5, TargetGame.Target.mediumSpeed, false, 0);
			this.addRowOfTargetsOnLane(4000, TargetGame.Target.nearLane, 1000, 5, TargetGame.Target.mediumSpeed, true, 0);
			this.addRowOfTargetsOnLane(8000, TargetGame.Target.farLane, 2000, 5, TargetGame.Target.mediumSpeed, false, TargetGame.Target.bonusTarget);
			this.addTwinPausers(8000, TargetGame.Target.superNearLane, TargetGame.Target.pauseMiddleLeft, TargetGame.Target.fastSpeed, 2000, TargetGame.Target.bonusTarget);
			this.addTwinPausers(15000, TargetGame.Target.superNearLane, TargetGame.Target.pauseFarLeft, TargetGame.Target.mediumSpeed, 4000, TargetGame.Target.bonusTarget);
			this.addRowOfTargetsOnLane(18000, TargetGame.Target.middleLane, 1500, 5, TargetGame.Target.mediumSpeed, false, 0);
			this.addRowOfTargetsOnLane(21000, TargetGame.Target.nearLane, 1000, 5, TargetGame.Target.mediumSpeed, true, 0);
			this.addTwinPausers(25000, TargetGame.Target.behindLane, TargetGame.Target.pauseFarLeft, TargetGame.Target.fastSpeed, 1500, TargetGame.Target.deluxeTarget);
			this.addRowOfTargetsOnLane(27000, TargetGame.Target.superNearLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addRowOfTargetsOnLane(28000, TargetGame.Target.nearLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addRowOfTargetsOnLane(29000, TargetGame.Target.middleLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addRowOfTargetsOnLane(30000, TargetGame.Target.farLane, 500, 8, TargetGame.Target.slowSpeed, true, 0);
			this.addTwinPausers(36000, TargetGame.Target.behindLane, TargetGame.Target.pauseFarLeft, TargetGame.Target.fastSpeed, 2000, TargetGame.Target.deluxeTarget);
			this.addRowOfTargetsOnLane(41000, TargetGame.Target.middleLane, 1500, 5, TargetGame.Target.mediumSpeed, false, 0);
			this.addRowOfTargetsOnLane(42000, TargetGame.Target.nearLane, 1000, 5, TargetGame.Target.mediumSpeed, true, 0);
			this.addRowOfTargetsOnLane(43000, TargetGame.Target.farLane, 1000, 4, TargetGame.Target.mediumSpeed, false, 0);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00107584 File Offset: 0x00105784
		private void addTwinPausers(int initialDelay, int whichLane, int pauseArea, int speed, int pauseTime, int targetType)
		{
			int otherPauseArea = -1;
			bool firstIsSpawnLeft = false;
			if (pauseArea == TargetGame.Target.pauseFarLeft)
			{
				otherPauseArea = TargetGame.Target.pauseFarRight;
				firstIsSpawnLeft = true;
			}
			if (pauseArea == TargetGame.Target.pauseLeft)
			{
				otherPauseArea = TargetGame.Target.pauseRight;
				firstIsSpawnLeft = true;
			}
			if (pauseArea == TargetGame.Target.pauseMiddleLeft)
			{
				otherPauseArea = TargetGame.Target.pauseMiddleRight;
				firstIsSpawnLeft = true;
			}
			if (pauseArea == TargetGame.Target.pauseMiddleRight)
			{
				otherPauseArea = TargetGame.Target.pauseMiddleLeft;
			}
			if (pauseArea == TargetGame.Target.pauseRight)
			{
				otherPauseArea = TargetGame.Target.pauseLeft;
			}
			if (pauseArea == TargetGame.Target.pauseFarRight)
			{
				otherPauseArea = TargetGame.Target.pauseFarLeft;
			}
			this.targets.Add(new TargetGame.Target(initialDelay, whichLane, targetType, speed, !firstIsSpawnLeft, pauseArea, pauseTime));
			this.targets.Add(new TargetGame.Target(initialDelay, whichLane, targetType, speed, firstIsSpawnLeft, otherPauseArea, pauseTime));
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00107628 File Offset: 0x00105828
		private void addRowOfTargetsOnLane(int initialDelayBeforeStarting, int whichLane, int delayBetween, int numberOfTargets, int speed, bool spawnFromRight = true, int targetType = 0)
		{
			for (int i = 0; i < numberOfTargets; i++)
			{
				this.targets.Add(new TargetGame.Target(initialDelayBeforeStarting + i * delayBetween, whichLane, targetType, speed, spawnFromRight, -1, -1));
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000D0E RID: 3342
		private GameLocation location;

		// Token: 0x04000D0F RID: 3343
		private LocalizedContentManager content;

		// Token: 0x04000D10 RID: 3344
		private Item tempItemStash;

		// Token: 0x04000D11 RID: 3345
		private int timerToStart = 1000;

		// Token: 0x04000D12 RID: 3346
		private int gameEndTimer = 61000;

		// Token: 0x04000D13 RID: 3347
		private int showResultsTimer = -1;

		// Token: 0x04000D14 RID: 3348
		private bool gameDone;

		// Token: 0x04000D15 RID: 3349
		private bool exit;

		// Token: 0x04000D16 RID: 3350
		public static int score;

		// Token: 0x04000D17 RID: 3351
		public static int shotsFired;

		// Token: 0x04000D18 RID: 3352
		public static int successShots;

		// Token: 0x04000D19 RID: 3353
		public static int accuracy = -1;

		// Token: 0x04000D1A RID: 3354
		public static int starTokensWon;

		// Token: 0x04000D1B RID: 3355
		public List<TargetGame.Target> targets;

		// Token: 0x04000D1C RID: 3356
		private float modifierBonus;

		// Token: 0x0200017D RID: 381
		public class Target
		{
			// Token: 0x060013BB RID: 5051 RVA: 0x00189D8C File Offset: 0x00187F8C
			public Target(int countdownBeforeSpawn, int whichLane, int type = 0, int speed = 4, bool spawnFromRight = true, int pauseAndReturn = -1, int pauseTime = -1)
			{
				this.countdownBeforeSpawn = countdownBeforeSpawn;
				this.targetType = type;
				this.speed = speed * (spawnFromRight ? -1 : 1);
				this.Position = new Microsoft.Xna.Framework.Rectangle(spawnFromRight ? TargetGame.Target.spawnRightPosition : TargetGame.Target.spawnLeftPosition, whichLane, TargetGame.Target.width, TargetGame.Target.width);
				this.xPausePosition = pauseAndReturn;
				this.xPauseTime = pauseTime;
				this.sourceRect = new Microsoft.Xna.Framework.Rectangle(289, 1184 + type * 16, 14, 14);
			}

			// Token: 0x060013BC RID: 5052 RVA: 0x00189E14 File Offset: 0x00188014
			public bool update(GameTime time, GameLocation location)
			{
				if (this.countdownBeforeSpawn > 0)
				{
					this.countdownBeforeSpawn -= time.ElapsedGameTime.Milliseconds;
					if (this.countdownBeforeSpawn <= 0)
					{
						this.spawned = true;
					}
				}
				if (this.spawned)
				{
					if (this.atPausePosition)
					{
						this.xPauseTime -= time.ElapsedGameTime.Milliseconds;
						if (this.xPauseTime <= 0)
						{
							this.speed = -this.speed;
							this.atPausePosition = false;
							this.xPausePosition = -1;
						}
					}
					else
					{
						this.Position.X = this.Position.X + this.speed;
						if (this.xPausePosition != -1 && Math.Abs(this.xPausePosition - this.Position.X) <= Math.Abs(this.speed))
						{
							this.atPausePosition = true;
						}
					}
					if (this.Position.X < 0 || this.Position.Right > TargetGame.Target.spawnRightPosition + Game1.tileSize)
					{
						return true;
					}
					for (int i = location.projectiles.Count - 1; i >= 0; i--)
					{
						if (location.projectiles[i].getBoundingBox().Intersects(this.Position))
						{
							this.shatter(location, location.projectiles[i]);
							if (this.targetType != TargetGame.Target.basicTarget)
							{
								location.projectiles[i].behaviorOnCollisionWithOther(location);
								location.projectiles.RemoveAt(i);
							}
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060013BD RID: 5053 RVA: 0x00189F94 File Offset: 0x00188194
			public void shatter(GameLocation location, Projectile stone)
			{
				int scoreToAdd = 0;
				if (this.targetType == TargetGame.Target.basicTarget)
				{
					Game1.playSound("breakingGlass");
					scoreToAdd++;
				}
				if (this.targetType == TargetGame.Target.bonusTarget)
				{
					Game1.playSound("potterySmash");
					scoreToAdd += 2;
				}
				if (this.targetType == TargetGame.Target.deluxeTarget)
				{
					Game1.playSound("potterySmash");
					scoreToAdd += 5;
				}
				location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(304, 1183 + this.targetType * 16, 16, 16), 60f, 3, 0, new Vector2((float)(this.Position.X - Game1.pixelZoom), (float)(this.Position.Y - Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
				location.debris.Add(new Debris(scoreToAdd, new Vector2((float)this.Position.Center.X, (float)this.Position.Center.Y), new Color(255, 130, 0), 1f, null));
				TargetGame.score += scoreToAdd;
				if (stone is BasicProjectile && (stone as BasicProjectile).damageToFarmer > 0)
				{
					TargetGame.successShots++;
					(stone as BasicProjectile).damageToFarmer = -1;
				}
			}

			// Token: 0x060013BE RID: 5054 RVA: 0x0018A108 File Offset: 0x00188308
			public void draw(SpriteBatch b)
			{
				if (this.spawned)
				{
					b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)this.Position.X, (float)(this.Position.Bottom + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.Position), new Microsoft.Xna.Framework.Rectangle?(this.sourceRect), Color.White);
				}
			}

			// Token: 0x040014A8 RID: 5288
			public static int width = 14 * Game1.pixelZoom;

			// Token: 0x040014A9 RID: 5289
			public static int spawnRightPosition = 15 * Game1.tileSize;

			// Token: 0x040014AA RID: 5290
			public static int spawnLeftPosition = 0;

			// Token: 0x040014AB RID: 5291
			public static int basicTarget = 0;

			// Token: 0x040014AC RID: 5292
			public static int bonusTarget = 1;

			// Token: 0x040014AD RID: 5293
			public static int deluxeTarget = 2;

			// Token: 0x040014AE RID: 5294
			public static int mediumSpeed = 4;

			// Token: 0x040014AF RID: 5295
			public static int slowSpeed = 2;

			// Token: 0x040014B0 RID: 5296
			public static int fastSpeed = 5;

			// Token: 0x040014B1 RID: 5297
			public static int nearLane = 7 * Game1.tileSize;

			// Token: 0x040014B2 RID: 5298
			public static int middleLane = 5 * Game1.tileSize;

			// Token: 0x040014B3 RID: 5299
			public static int farLane = 2 * Game1.tileSize;

			// Token: 0x040014B4 RID: 5300
			public static int superNearLane = 9 * Game1.tileSize;

			// Token: 0x040014B5 RID: 5301
			public static int behindLane = 13 * Game1.tileSize;

			// Token: 0x040014B6 RID: 5302
			public static int pauseFarRight = 13 * Game1.tileSize;

			// Token: 0x040014B7 RID: 5303
			public static int pauseRight = 11 * Game1.tileSize;

			// Token: 0x040014B8 RID: 5304
			public static int pauseMiddleRight = 9 * Game1.tileSize;

			// Token: 0x040014B9 RID: 5305
			public static int pauseMiddleLeft = 6 * Game1.tileSize;

			// Token: 0x040014BA RID: 5306
			public static int pauseLeft = 4 * Game1.tileSize;

			// Token: 0x040014BB RID: 5307
			public static int pauseFarLeft = 2 * Game1.tileSize;

			// Token: 0x040014BC RID: 5308
			public Microsoft.Xna.Framework.Rectangle Position;

			// Token: 0x040014BD RID: 5309
			private int targetType;

			// Token: 0x040014BE RID: 5310
			private int countdownBeforeSpawn;

			// Token: 0x040014BF RID: 5311
			private int xPausePosition;

			// Token: 0x040014C0 RID: 5312
			private int xPauseTime;

			// Token: 0x040014C1 RID: 5313
			private int speed;

			// Token: 0x040014C2 RID: 5314
			private bool spawned;

			// Token: 0x040014C3 RID: 5315
			private bool atPausePosition;

			// Token: 0x040014C4 RID: 5316
			private Microsoft.Xna.Framework.Rectangle sourceRect;
		}
	}
}
