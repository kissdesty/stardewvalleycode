using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace StardewValley.Tools
{
	// Token: 0x0200006B RID: 107
	public class WateringCan : Tool
	{
		// Token: 0x170000C0 RID: 192
		public int WaterLeft
		{
			// Token: 0x06000941 RID: 2369 RVA: 0x000C8B38 File Offset: 0x000C6D38
			get
			{
				return this.waterLeft;
			}
			// Token: 0x06000942 RID: 2370 RVA: 0x000C8B40 File Offset: 0x000C6D40
			set
			{
				this.waterLeft = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		public override int UpgradeLevel
		{
			// Token: 0x06000943 RID: 2371 RVA: 0x000A8200 File Offset: 0x000A6400
			get
			{
				return this.upgradeLevel;
			}
			// Token: 0x06000944 RID: 2372 RVA: 0x000A8208 File Offset: 0x000A6408
			set
			{
				this.upgradeLevel = value;
				this.setNewTileIndexForUpgradeLevel();
			}
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000C8B49 File Offset: 0x000C6D49
		public WateringCan() : base("Watering Can", 0, 273, 296, "Used to water crops. It can be refilled at any water source.", false, 0)
		{
			this.upgradeLevel = 0;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000C8B80 File Offset: 0x000C6D80
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			base.drawInMenu(spriteBatch, location + new Vector2(0f, (float)(-(float)Game1.tileSize / 4 + 4)), scaleSize, transparency, layerDepth, drawStackNumber);
			if (drawStackNumber)
			{
				spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(4f, (float)(Game1.tileSize - 20)), new Rectangle?(new Rectangle(297, 420, 14, 5)), Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth + 0.0001f);
				spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)location.X + 8, (int)location.Y + Game1.tileSize - 16, (int)((float)this.waterLeft / (float)this.waterCanMax * 48f), 8), Color.DodgerBlue * 0.7f * transparency);
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000C8C74 File Offset: 0x000C6E74
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			power = who.toolPower;
			who.stopJittering();
			List<Vector2> tileLocations = base.tilesAffected(new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize)), power, who);
			if (location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "Water", "Back") != null || location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "WaterSource", "Back") != null || (location is BuildableGameLocation && (location as BuildableGameLocation).getBuildingAt(tileLocations.First<Vector2>()) != null && (location as BuildableGameLocation).getBuildingAt(tileLocations.First<Vector2>()).buildingType.Equals("Well") && (location as BuildableGameLocation).getBuildingAt(tileLocations.First<Vector2>()).daysOfConstructionLeft <= 0))
			{
				who.jitterStrength = 0.5f;
				switch (this.upgradeLevel)
				{
				case 0:
					this.waterCanMax = 40;
					break;
				case 1:
					this.waterCanMax = 55;
					break;
				case 2:
					this.waterCanMax = 70;
					break;
				case 3:
					this.waterCanMax = 85;
					break;
				case 4:
					this.waterCanMax = 100;
					break;
				}
				this.waterLeft = this.waterCanMax;
				Game1.playSound("slosh");
				DelayedAction.playSoundAfterDelay("glug", 250);
				return;
			}
			if (this.waterLeft > 0)
			{
				who.Stamina -= (float)(2 * (power + 1)) - (float)who.FarmingLevel * 0.1f;
				int i = 0;
				foreach (Vector2 tileLocation in tileLocations)
				{
					if (location.terrainFeatures.ContainsKey(tileLocation))
					{
						location.terrainFeatures[tileLocation].performToolAction(this, 0, tileLocation, null);
					}
					if (location.objects.ContainsKey(tileLocation))
					{
						location.Objects[tileLocation].performToolAction(this);
					}
					location.performToolAction(this, (int)tileLocation.X, (int)tileLocation.Y);
					location.temporarySprites.Add(new TemporaryAnimatedSprite(13, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize), Color.White, 10, Game1.random.NextDouble() < 0.5, 70f, 0, Game1.tileSize, (tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) / 10000f - 0.01f, -1, 0)
					{
						delayBeforeAnimationStart = 200 + i * 10
					});
					i++;
				}
				this.waterLeft -= power + 1;
				Vector2 basePosition = new Vector2(who.position.X - (float)(Game1.tileSize / 2) - (float)Game1.pixelZoom, who.position.Y - (float)(Game1.tileSize / 4) - (float)Game1.pixelZoom);
				switch (who.facingDirection)
				{
				case 0:
					basePosition = Vector2.Zero;
					break;
				case 1:
					basePosition.X += (float)(Game1.tileSize * 2 + Game1.pixelZoom * 2);
					break;
				case 2:
					basePosition.X += (float)(Game1.tileSize + Game1.pixelZoom * 2);
					basePosition.Y += (float)(Game1.tileSize / 2 + Game1.pixelZoom * 3);
					break;
				}
				if (!basePosition.Equals(Vector2.Zero))
				{
					for (int j = 0; j < 30; j++)
					{
						location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.staminaRect, new Rectangle(0, 0, 1, 1), 999f, 1, 999, basePosition + new Vector2((float)(Game1.random.Next(-3, 0) * Game1.pixelZoom), (float)(Game1.random.Next(2) * Game1.pixelZoom)), false, false, (float)(who.GetBoundingBox().Bottom + Game1.tileSize / 2) / 10000f, 0.04f, (Game1.random.NextDouble() < 0.5) ? Color.DeepSkyBlue : Color.LightBlue, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							delayBeforeAnimationStart = j * 15,
							motion = new Vector2((float)Game1.random.Next(-10, 11) / 100f, 0.5f),
							acceleration = new Vector2(0f, 0.1f)
						});
					}
					return;
				}
			}
			else
			{
				who.doEmote(4);
				Game1.showRedMessage("Out of water");
			}
		}

		// Token: 0x0400093F RID: 2367
		public int waterCanMax = 40;

		// Token: 0x04000940 RID: 2368
		private int waterLeft = 40;
	}
}
