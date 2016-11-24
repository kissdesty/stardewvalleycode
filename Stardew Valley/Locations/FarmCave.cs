using System;
using Microsoft.Xna.Framework;
using xTile;

namespace StardewValley.Locations
{
	// Token: 0x02000124 RID: 292
	public class FarmCave : GameLocation
	{
		// Token: 0x0600109A RID: 4250 RVA: 0x00151B90 File Offset: 0x0014FD90
		public FarmCave()
		{
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00151B98 File Offset: 0x0014FD98
		public FarmCave(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00154B58 File Offset: 0x00152D58
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.caveChoice == 1)
			{
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2(0f, 0f), false, 0f, Color.White)
				{
					interval = 3000f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(Game1.pixelZoom * 2), 0f), false, 0f, Color.White)
				{
					interval = 3000f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(Game1.tileSize * 5), (float)(-(float)Game1.tileSize)), false, 0f, Color.White)
				{
					interval = 2000f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 500,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(Game1.tileSize * 5 + Game1.pixelZoom * 2), (float)(-(float)Game1.tileSize)), false, 0f, Color.White)
				{
					interval = 2000f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 500,
					layerDepth = 1f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(Game1.tileSize * 2), (float)(this.map.Layers[0].LayerHeight * Game1.tileSize - Game1.tileSize)), false, 0f, Color.White)
				{
					interval = 1600f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 250,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(Game1.tileSize * 2 + Game1.pixelZoom * 2), (float)(this.map.Layers[0].LayerHeight * Game1.tileSize - Game1.tileSize)), false, 0f, Color.White)
				{
					interval = 1600f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 250,
					layerDepth = 1f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom), (float)(Game1.tileSize * 3)), false, 0f, Color.White)
				{
					interval = 2800f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 750,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom * 3), (float)(Game1.tileSize * 3)), false, 0f, Color.White)
				{
					interval = 2800f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 750,
					layerDepth = 1f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom), (float)(Game1.tileSize * 9)), false, 0f, Color.White)
				{
					interval = 2200f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 750,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom * 3), (float)(Game1.tileSize * 9)), false, 0f, Color.White)
				{
					interval = 2200f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 750,
					layerDepth = 1f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(-(float)Game1.tileSize + Game1.pixelZoom), (float)(Game1.tileSize * 2)), false, 0f, Color.White)
				{
					interval = 2600f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 750,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(-(float)Game1.tileSize + Game1.pixelZoom * 3), (float)(Game1.tileSize * 2)), false, 0f, Color.White)
				{
					interval = 2600f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 750,
					layerDepth = 1f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(-(float)Game1.tileSize), (float)(Game1.tileSize * 6)), false, 0f, Color.White)
				{
					interval = 3400f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 650,
					layerDepth = 1f,
					light = true,
					lightRadius = 0.5f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float)(-(float)Game1.tileSize + Game1.pixelZoom * 3), (float)(Game1.tileSize * 6)), false, 0f, Color.White)
				{
					interval = 3400f,
					animationLength = 3,
					totalNumberOfLoops = 99999,
					scale = (float)Game1.pixelZoom,
					delayBeforeAnimationStart = 650,
					layerDepth = 1f
				});
				Game1.ambientLight = new Color(70, 90, 0);
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00155400 File Offset: 0x00153600
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (Game1.player.caveChoice == 1 && Game1.random.NextDouble() < 0.002)
			{
				base.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2((float)Game1.random.Next(this.map.Layers[0].LayerWidth), (float)this.map.Layers[0].LayerHeight) * (float)Game1.tileSize, false, false, 1f, 0f, Color.Black, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					xPeriodic = true,
					xPeriodicLoopTime = 2000f,
					xPeriodicRange = (float)Game1.tileSize,
					motion = new Vector2(0f, (float)(-(float)Game1.pixelZoom * 2))
				});
				if (Game1.random.NextDouble() < 0.15)
				{
					Game1.playSound("batScreech");
				}
				for (int i = 1; i < 5; i++)
				{
					DelayedAction.playSoundAfterDelay("batFlap", 320 * i - 80);
				}
				return;
			}
			if (Game1.player.caveChoice == 1 && Game1.random.NextDouble() < 0.005)
			{
				this.temporarySprites.Add(new BatTemporarySprite(new Vector2((float)((Game1.random.NextDouble() < 0.5) ? 0 : (this.map.DisplayWidth - Game1.tileSize)), (float)(this.map.DisplayHeight - Game1.tileSize))));
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00002834 File Offset: 0x00000A34
		public override void checkForMusic(GameTime time)
		{
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00154026 File Offset: 0x00152226
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x001555C8 File Offset: 0x001537C8
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			if (Game1.player.caveChoice == 1)
			{
				while (Game1.random.NextDouble() < 0.5)
				{
					int whichFruit = 410;
					switch (Game1.random.Next(5))
					{
					case 0:
						whichFruit = 296;
						break;
					case 1:
						whichFruit = 396;
						break;
					case 2:
						whichFruit = 406;
						break;
					case 3:
						whichFruit = 410;
						break;
					case 4:
						if (Game1.random.NextDouble() < 0.75)
						{
							whichFruit = ((Game1.random.NextDouble() < 0.1) ? 613 : Game1.random.Next(634, 639));
						}
						break;
					}
					Vector2 v = new Vector2((float)Game1.random.Next(1, this.map.Layers[0].LayerWidth - 1), (float)Game1.random.Next(1, this.map.Layers[0].LayerHeight - 4));
					if (this.isTileLocationTotallyClearAndPlaceable(v))
					{
						base.setObject(v, new Object(whichFruit, 1, false, -1, 0)
						{
							isSpawnedObject = true
						});
					}
				}
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00155710 File Offset: 0x00153910
		public void setUpMushroomHouse()
		{
			base.setObject(new Vector2(4f, 5f), new Object(new Vector2(4f, 5f), 128, false));
			base.setObject(new Vector2(6f, 5f), new Object(new Vector2(6f, 5f), 128, false));
			base.setObject(new Vector2(8f, 5f), new Object(new Vector2(8f, 5f), 128, false));
			base.setObject(new Vector2(4f, 7f), new Object(new Vector2(4f, 7f), 128, false));
			base.setObject(new Vector2(6f, 7f), new Object(new Vector2(6f, 7f), 128, false));
			base.setObject(new Vector2(8f, 7f), new Object(new Vector2(8f, 7f), 128, false));
		}

		// Token: 0x040011DE RID: 4574
		public static int timesPlayedCalicoJack;

		// Token: 0x040011DF RID: 4575
		public static int timesPlayedSlots;
	}
}
