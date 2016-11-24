using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using xTile.Dimensions;

namespace StardewValley.Tools
{
	// Token: 0x02000061 RID: 97
	public class Hoe : Tool
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x000C5F42 File Offset: 0x000C4142
		public Hoe() : base("Hoe", 0, 21, 47, "Used to dig and till soil.", false, 0)
		{
			this.upgradeLevel = 0;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000C5F64 File Offset: 0x000C4164
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			if (location.Name.Equals("UndergroundMine"))
			{
				power = 1;
			}
			who.Stamina -= (float)(2 * power) - (float)who.FarmingLevel * 0.1f;
			power = who.toolPower;
			who.stopJittering();
			Game1.playSound("woodyHit");
			Vector2 initialTile = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			List<Vector2> tileLocations = base.tilesAffected(initialTile, power, who);
			foreach (Vector2 tileLocation in tileLocations)
			{
				tileLocation.Equals(initialTile);
				if (location.terrainFeatures.ContainsKey(tileLocation))
				{
					if (location.terrainFeatures[tileLocation].performToolAction(this, 0, tileLocation, null))
					{
						location.terrainFeatures.Remove(tileLocation);
					}
				}
				else
				{
					if (location.objects.ContainsKey(tileLocation) && location.Objects[tileLocation].performToolAction(this))
					{
						if (location.Objects[tileLocation].type.Equals("Crafting") && location.Objects[tileLocation].fragility != 2)
						{
							location.debris.Add(new Debris(location.Objects[tileLocation].bigCraftable ? (-location.Objects[tileLocation].ParentSheetIndex) : location.Objects[tileLocation].ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
						}
						location.Objects[tileLocation].performRemoveAction(tileLocation, location);
						location.Objects.Remove(tileLocation);
					}
					if (location.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Diggable", "Back") != null)
					{
						if (location.Name.Equals("UndergroundMine") && !location.isTileOccupied(tileLocation, ""))
						{
							if (Game1.mine.mineLevel < 40 || Game1.mine.mineLevel >= 80)
							{
								location.terrainFeatures.Add(tileLocation, new HoeDirt());
								Game1.playSound("hoeHit");
							}
							else if (Game1.mine.mineLevel < 80)
							{
								location.terrainFeatures.Add(tileLocation, new HoeDirt());
								Game1.playSound("hoeHit");
							}
							Game1.removeSquareDebrisFromTile((int)tileLocation.X, (int)tileLocation.Y);
							location.checkForBuriedItem((int)tileLocation.X, (int)tileLocation.Y, false, false);
							location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(initialTile.X * (float)Game1.tileSize, initialTile.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
							if (tileLocations.Count > 2)
							{
								location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(initialTile, tileLocation) * 30f, 0, -1, -1f, -1, 0));
							}
						}
						else if (!location.isTileOccupied(tileLocation, "") && location.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), Game1.viewport))
						{
							location.makeHoeDirt(tileLocation);
							Game1.playSound("hoeHit");
							Game1.removeSquareDebrisFromTile((int)tileLocation.X, (int)tileLocation.Y);
							location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
							if (tileLocations.Count > 2)
							{
								location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(initialTile, tileLocation) * 30f, 0, -1, -1f, -1, 0));
							}
							location.checkForBuriedItem((int)tileLocation.X, (int)tileLocation.Y, false, false);
						}
						Stats expr_4B2 = Game1.stats;
						uint dirtHoed = expr_4B2.DirtHoed;
						expr_4B2.DirtHoed = dirtHoed + 1u;
					}
				}
			}
		}
	}
}
