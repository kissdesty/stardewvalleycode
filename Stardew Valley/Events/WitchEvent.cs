using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;

namespace StardewValley.Events
{
	// Token: 0x02000138 RID: 312
	public class WitchEvent : FarmEvent
	{
		// Token: 0x060011D3 RID: 4563 RVA: 0x0016CE7C File Offset: 0x0016B07C
		public bool setUp()
		{
			this.f = (Game1.getLocationFromName("Farm") as Farm);
			this.r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			foreach (Building b in this.f.buildings)
			{
				if (b is Coop && !b.buildingType.Equals("Coop") && !(b.indoors as AnimalHouse).isFull() && b.indoors.objects.Count < 50 && this.r.NextDouble() < 0.8)
				{
					this.targetBuilding = b;
				}
			}
			if (this.targetBuilding == null)
			{
				foreach (Building b2 in this.f.buildings)
				{
					if (b2.buildingType.Equals("Slime Hutch") && b2.indoors.characters.Count > 0 && this.r.NextDouble() < 0.5 && b2.indoors.numberOfObjectsOfType(83, true) == 0)
					{
						this.targetBuilding = b2;
					}
				}
			}
			if (this.targetBuilding == null)
			{
				return true;
			}
			Game1.currentLightSources.Add(new LightSource(4, this.witchPosition, 2f, Color.Black, 942069));
			Game1.currentLocation = this.f;
			this.f.resetForPlayerEntry();
			Game1.fadeClear();
			Game1.nonWarpFade = true;
			Game1.timeOfDay = 2400;
			Game1.ambientLight = new Color(200, 190, 40);
			Game1.displayHUD = false;
			Game1.freezeControls = true;
			Game1.viewportFreeze = true;
			Game1.displayFarmer = false;
			Game1.viewport.X = Math.Max(0, Math.Min(this.f.map.DisplayWidth - Game1.viewport.Width, this.targetBuilding.tileX * Game1.tileSize - Game1.viewport.Width / 2));
			Game1.viewport.Y = Math.Max(0, Math.Min(this.f.map.DisplayHeight - Game1.viewport.Height, (this.targetBuilding.tileY - 3) * Game1.tileSize - Game1.viewport.Height / 2));
			this.witchPosition = new Vector2((float)(Game1.viewport.X + Game1.viewport.Width + Game1.tileSize * 2), (float)(this.targetBuilding.tileY * Game1.tileSize - Game1.tileSize));
			Game1.changeMusicTrack("nightTime");
			DelayedAction.playSoundAfterDelay("cacklingWitch", 3200);
			return false;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0016D17C File Offset: 0x0016B37C
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
			Utility.repositionLightSource(942069, this.witchPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
			if (this.animationLoopsDone < 1)
			{
				this.timerSinceFade += time.ElapsedGameTime.Milliseconds;
			}
			if (this.witchPosition.X > (float)(this.targetBuilding.tileX * Game1.tileSize + Game1.tileSize * 3 / 2))
			{
				if (this.timerSinceFade < 2000)
				{
					return false;
				}
				this.witchPosition.X = this.witchPosition.X - (float)time.ElapsedGameTime.Milliseconds * 0.4f;
				this.witchPosition.Y = this.witchPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) * 1f;
			}
			else if (this.animationLoopsDone < 4)
			{
				this.witchPosition.Y = this.witchPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) * 1f;
				this.witchAnimationTimer += time.ElapsedGameTime.Milliseconds;
				if (this.witchAnimationTimer > 2000)
				{
					this.witchAnimationTimer = 0;
					if (!this.animateLeft)
					{
						this.witchFrame++;
						if (this.witchFrame == 1)
						{
							this.animateLeft = true;
							for (int i = 0; i < 75; i++)
							{
								this.f.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.witchPosition + new Vector2((float)(2 * Game1.pixelZoom), (float)(Game1.tileSize * 3 / 2 - Game1.pixelZoom * 4)), (this.r.NextDouble() < 0.5) ? Color.Lime : Color.DarkViolet, 8, false, 100f, 0, -1, -1f, -1, 0)
								{
									motion = new Vector2((float)this.r.Next(-100, 100) / 100f, 1.5f),
									alphaFade = 0.015f,
									delayBeforeAnimationStart = i * 30,
									layerDepth = 1f
								});
							}
							Game1.playSound("debuffSpell");
						}
					}
					else
					{
						this.witchFrame--;
						this.animationLoopsDone = 4;
						DelayedAction.playSoundAfterDelay("cacklingWitch", 2500);
					}
				}
			}
			else
			{
				this.witchAnimationTimer += time.ElapsedGameTime.Milliseconds;
				this.witchFrame = 0;
				if (this.witchAnimationTimer > 1000 && this.witchPosition.X > -999999f)
				{
					this.witchPosition.Y = this.witchPosition.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 256.0) * 2f;
					this.witchPosition.X = this.witchPosition.X - (float)time.ElapsedGameTime.Milliseconds * 0.4f;
				}
				if (this.witchPosition.X < (float)(Game1.viewport.X - Game1.tileSize * 2) || float.IsNaN(this.witchPosition.X))
				{
					if (!Game1.fadeToBlack && this.witchPosition.X != -999999f)
					{
						Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterLastFade), 0.02f);
						Game1.changeMusicTrack("none");
						this.timerSinceFade = 0;
						this.witchPosition.X = -999999f;
					}
					this.timerSinceFade += time.ElapsedGameTime.Milliseconds;
				}
			}
			return false;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0016D592 File Offset: 0x0016B792
		public void afterLastFade()
		{
			this.terminate = true;
			Game1.globalFadeToClear(null, 0.02f);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0016D5A8 File Offset: 0x0016B7A8
		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.witchPosition), new Rectangle?(new Rectangle(277, 1886 + this.witchFrame * 29, 34, 29)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9999999f);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0016D60C File Offset: 0x0016B80C
		public void makeChangesToLocation()
		{
			if (this.targetBuilding.buildingType.Equals("Slime Hutch"))
			{
				using (List<NPC>.Enumerator enumerator = this.targetBuilding.indoors.characters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC i = enumerator.Current;
						if (i is GreenSlime)
						{
							(i as GreenSlime).color = new Color(40 + this.r.Next(10), 40 + this.r.Next(10), 40 + this.r.Next(10));
						}
					}
					return;
				}
			}
			for (int tries = 0; tries < 200; tries++)
			{
				Vector2 v = new Vector2((float)this.r.Next(2, this.targetBuilding.indoors.map.Layers[0].LayerWidth - 2), (float)this.r.Next(2, this.targetBuilding.indoors.map.Layers[0].LayerHeight - 2));
				if (this.targetBuilding.indoors.isTileLocationTotallyClearAndPlaceable(v) || (this.targetBuilding.indoors.terrainFeatures.ContainsKey(v) && this.targetBuilding.indoors.terrainFeatures[v] is Flooring))
				{
					this.targetBuilding.indoors.objects.Add(v, new Object(Vector2.Zero, 305, null, false, true, false, true));
					return;
				}
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00002834 File Offset: 0x00000A34
		public void drawAboveEverything(SpriteBatch b)
		{
		}

		// Token: 0x0400129F RID: 4767
		public const int identifier = 942069;

		// Token: 0x040012A0 RID: 4768
		private Vector2 witchPosition;

		// Token: 0x040012A1 RID: 4769
		private Building targetBuilding;

		// Token: 0x040012A2 RID: 4770
		private Farm f;

		// Token: 0x040012A3 RID: 4771
		private Random r;

		// Token: 0x040012A4 RID: 4772
		private int witchFrame;

		// Token: 0x040012A5 RID: 4773
		private int witchAnimationTimer;

		// Token: 0x040012A6 RID: 4774
		private int animationLoopsDone;

		// Token: 0x040012A7 RID: 4775
		private int timerSinceFade;

		// Token: 0x040012A8 RID: 4776
		private bool animateLeft;

		// Token: 0x040012A9 RID: 4777
		private bool terminate;
	}
}
