using System;
using Microsoft.Xna.Framework;
using xTile.ObjectModel;

namespace StardewValley.Tools
{
	// Token: 0x02000058 RID: 88
	public class Axe : Tool
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x000BABCF File Offset: 0x000B8DCF
		public Axe() : base("Axe", 0, 189, 215, "Used to chop wood.", false, 0)
		{
			this.upgradeLevel = 0;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000BABF8 File Offset: 0x000B8DF8
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			base.Update(who.facingDirection, 0, who);
			if (who.IsMainPlayer)
			{
				Game1.releaseUseToolButton();
				return;
			}
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentFrame(176);
				who.CurrentTool.Update(0, 0);
				return;
			case 1:
				who.FarmerSprite.setCurrentFrame(168);
				who.CurrentTool.Update(1, 0);
				return;
			case 2:
				who.FarmerSprite.setCurrentFrame(160);
				who.CurrentTool.Update(2, 0);
				return;
			case 3:
				who.FarmerSprite.setCurrentFrame(184);
				who.CurrentTool.Update(3, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000BACC4 File Offset: 0x000B8EC4
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			who.Stamina -= (float)(2 * power) - (float)who.ForagingLevel * 0.1f;
			int tileX = x / Game1.tileSize;
			int tileY = y / Game1.tileSize;
			Rectangle tileRect = new Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			Vector2 tile = new Vector2((float)tileX, (float)tileY);
			if (location.map.GetLayer("Buildings").Tiles[tileX, tileY] != null)
			{
				PropertyValue value = null;
				location.map.GetLayer("Buildings").Tiles[tileX, tileY].TileIndexProperties.TryGetValue("TreeStump", out value);
				if (value != null)
				{
					Game1.drawObjectDialogue("You're only allowed to chop trees on your farm... It's a town rule.");
					return;
				}
			}
			location.performToolAction(this, tileX, tileY);
			if (location.terrainFeatures.ContainsKey(tile) && location.terrainFeatures[tile].performToolAction(this, 0, tile, null))
			{
				location.terrainFeatures.Remove(tile);
			}
			if (location.largeTerrainFeatures != null)
			{
				for (int i = location.largeTerrainFeatures.Count - 1; i >= 0; i--)
				{
					if (location.largeTerrainFeatures[i].getBoundingBox().Intersects(tileRect) && location.largeTerrainFeatures[i].performToolAction(this, 0, tile, null))
					{
						location.largeTerrainFeatures.RemoveAt(i);
					}
				}
			}
			Vector2 toolTilePosition = new Vector2((float)tileX, (float)tileY);
			if (location.Objects.ContainsKey(toolTilePosition) && location.Objects[toolTilePosition].Type != null && location.Objects[toolTilePosition].performToolAction(this))
			{
				if (location.Objects[toolTilePosition].type.Equals("Crafting") && location.Objects[toolTilePosition].fragility != 2)
				{
					location.debris.Add(new Debris(location.Objects[toolTilePosition].bigCraftable ? (-location.Objects[toolTilePosition].ParentSheetIndex) : location.Objects[toolTilePosition].ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
				}
				location.Objects[toolTilePosition].performRemoveAction(toolTilePosition, location);
				location.Objects.Remove(toolTilePosition);
			}
		}

		// Token: 0x040008BF RID: 2239
		public const int StumpStrength = 4;

		// Token: 0x040008C0 RID: 2240
		private int stumpTileX;

		// Token: 0x040008C1 RID: 2241
		private int stumpTileY;

		// Token: 0x040008C2 RID: 2242
		private int hitsToStump;
	}
}
