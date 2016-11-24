using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace StardewValley.Events
{
	// Token: 0x0200013C RID: 316
	public class FairyEvent : FarmEvent
	{
		// Token: 0x060011EE RID: 4590 RVA: 0x0016F5F0 File Offset: 0x0016D7F0
		public bool setUp()
		{
			this.f = (Game1.getLocationFromName("Farm") as Farm);
			if (Game1.isRaining)
			{
				return true;
			}
			int attemptsLeft = 100;
			Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			this.targetCrop = Vector2.Zero;
			while (attemptsLeft > 0 && this.targetCrop.Equals(Vector2.Zero))
			{
				attemptsLeft--;
				if (this.f.terrainFeatures.Count != 0)
				{
					KeyValuePair<Vector2, TerrainFeature> t = this.f.terrainFeatures.ElementAt(r.Next(this.f.terrainFeatures.Count));
					if (t.Value is HoeDirt && (t.Value as HoeDirt).crop != null && !(t.Value as HoeDirt).crop.isWildSeedCrop() && (t.Value as HoeDirt).crop.currentPhase < (t.Value as HoeDirt).crop.phaseDays.Count - 1)
					{
						this.targetCrop = t.Key;
					}
				}
			}
			if (this.targetCrop.Equals(Vector2.Zero))
			{
				return true;
			}
			Game1.currentLightSources.Add(new LightSource(4, this.fairyPosition, 1f, Color.Black, 942069));
			Game1.currentLocation = this.f;
			this.f.resetForPlayerEntry();
			Game1.fadeClear();
			Game1.nonWarpFade = true;
			Game1.timeOfDay = 2400;
			Game1.displayHUD = false;
			Game1.freezeControls = true;
			Game1.viewportFreeze = true;
			Game1.displayFarmer = false;
			Game1.viewport.X = Math.Max(0, Math.Min(this.f.map.DisplayWidth - Game1.viewport.Width, (int)this.targetCrop.X * Game1.tileSize - Game1.viewport.Width / 2));
			Game1.viewport.Y = Math.Max(0, Math.Min(this.f.map.DisplayHeight - Game1.viewport.Height, (int)this.targetCrop.Y * Game1.tileSize - Game1.viewport.Height / 2));
			this.fairyPosition = new Vector2((float)(Game1.viewport.X + Game1.viewport.Width + Game1.tileSize * 2), this.targetCrop.Y * (float)Game1.tileSize - (float)Game1.tileSize);
			Game1.changeMusicTrack("nightTime");
			return false;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0016F884 File Offset: 0x0016DA84
		public bool tickUpdate(GameTime time)
		{
			if (this.terminate)
			{
				return true;
			}
			Game1.UpdateGameClock(time);
			this.f.UpdateWhenCurrentLocation(time);
			this.f.updateEvenIfFarmerIsntHere(time, false);
			Game1.UpdateOther(time);
			Utility.repositionLightSource(942069, this.fairyPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
			if (this.animationLoopsDone < 1)
			{
				this.timerSinceFade += time.ElapsedGameTime.Milliseconds;
			}
			if (this.fairyPosition.X > this.targetCrop.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2))
			{
				if (this.timerSinceFade < 2000)
				{
					return false;
				}
				this.fairyPosition.X = this.fairyPosition.X - (float)time.ElapsedGameTime.Milliseconds * 0.1f;
				this.fairyPosition.Y = this.fairyPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) * 1f;
				int arg_150_0 = this.fairyFrame;
				if (time.TotalGameTime.Milliseconds % 500 > 250)
				{
					this.fairyFrame = 1;
				}
				else
				{
					this.fairyFrame = 0;
				}
				if (arg_150_0 != this.fairyFrame && this.fairyFrame == 1)
				{
					Game1.playSound("batFlap");
					this.f.temporarySprites.Add(new TemporaryAnimatedSprite(11, this.fairyPosition + new Vector2((float)(Game1.tileSize / 2), 0f), Color.Purple, 8, false, 100f, 0, -1, -1f, -1, 0));
				}
				if (this.fairyPosition.X <= this.targetCrop.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2))
				{
					this.fairyFrame = 1;
				}
			}
			else if (this.animationLoopsDone < 4)
			{
				this.fairyAnimationTimer += time.ElapsedGameTime.Milliseconds;
				if (this.fairyAnimationTimer > 250)
				{
					this.fairyAnimationTimer = 0;
					if (!this.animateLeft)
					{
						this.fairyFrame++;
						if (this.fairyFrame == 3)
						{
							this.animateLeft = true;
							this.f.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.fairyPosition + new Vector2(-16f, (float)Game1.tileSize), Color.LightPink, 8, false, 100f, 0, -1, -1f, -1, 0));
							Game1.playSound("yoba");
							if (this.f.terrainFeatures.ContainsKey(this.targetCrop))
							{
								(this.f.terrainFeatures[this.targetCrop] as HoeDirt).crop.currentPhase = Math.Min((this.f.terrainFeatures[this.targetCrop] as HoeDirt).crop.currentPhase + 1, (this.f.terrainFeatures[this.targetCrop] as HoeDirt).crop.phaseDays.Count - 1);
							}
						}
					}
					else
					{
						this.fairyFrame--;
						if (this.fairyFrame == 1)
						{
							this.animateLeft = false;
							this.animationLoopsDone++;
							if (this.animationLoopsDone >= 4)
							{
								for (int i = 0; i < 10; i++)
								{
									DelayedAction.playSoundAfterDelay("batFlap", 4000 + 500 * i);
								}
							}
						}
					}
				}
			}
			else
			{
				this.fairyAnimationTimer += time.ElapsedGameTime.Milliseconds;
				if (time.TotalGameTime.Milliseconds % 500 > 250)
				{
					this.fairyFrame = 1;
				}
				else
				{
					this.fairyFrame = 0;
				}
				if (this.fairyAnimationTimer > 2000 && this.fairyPosition.Y > -999999f)
				{
					this.fairyPosition.X = this.fairyPosition.X + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 256.0) * 2f;
					this.fairyPosition.Y = this.fairyPosition.Y - (float)time.ElapsedGameTime.Milliseconds * 0.2f;
				}
				if (this.fairyPosition.Y < (float)(Game1.viewport.Y - Game1.tileSize * 2) || float.IsNaN(this.fairyPosition.Y))
				{
					if (!Game1.fadeToBlack && this.fairyPosition.Y != -999999f)
					{
						Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterLastFade), 0.02f);
						Game1.changeMusicTrack("none");
						this.timerSinceFade = 0;
						this.fairyPosition.Y = -999999f;
					}
					this.timerSinceFade += time.ElapsedGameTime.Milliseconds;
				}
			}
			return false;
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0016FD97 File Offset: 0x0016DF97
		public void afterLastFade()
		{
			this.terminate = true;
			Game1.globalFadeToClear(null, 0.02f);
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0016FDAC File Offset: 0x0016DFAC
		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.fairyPosition), new Rectangle?(new Rectangle(16 + this.fairyFrame * 16, 592, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9999999f);
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0016FE10 File Offset: 0x0016E010
		public void makeChangesToLocation()
		{
			int x = (int)this.targetCrop.X - 2;
			while ((float)x <= this.targetCrop.X + 2f)
			{
				int y = (int)this.targetCrop.Y - 2;
				while ((float)y <= this.targetCrop.Y + 2f)
				{
					Vector2 v = new Vector2((float)x, (float)y);
					if (this.f.terrainFeatures.ContainsKey(v) && this.f.terrainFeatures[v] is HoeDirt && (this.f.terrainFeatures[v] as HoeDirt).crop != null)
					{
						(this.f.terrainFeatures[v] as HoeDirt).crop.growCompletely();
					}
					y++;
				}
				x++;
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00002834 File Offset: 0x00000A34
		public void drawAboveEverything(SpriteBatch b)
		{
		}

		// Token: 0x040012C5 RID: 4805
		public const int identifier = 942069;

		// Token: 0x040012C6 RID: 4806
		private Vector2 fairyPosition;

		// Token: 0x040012C7 RID: 4807
		private Vector2 targetCrop;

		// Token: 0x040012C8 RID: 4808
		private Farm f;

		// Token: 0x040012C9 RID: 4809
		private int fairyFrame;

		// Token: 0x040012CA RID: 4810
		private int fairyAnimationTimer;

		// Token: 0x040012CB RID: 4811
		private int animationLoopsDone;

		// Token: 0x040012CC RID: 4812
		private int timerSinceFade;

		// Token: 0x040012CD RID: 4813
		private bool animateLeft;

		// Token: 0x040012CE RID: 4814
		private bool terminate;
	}
}
