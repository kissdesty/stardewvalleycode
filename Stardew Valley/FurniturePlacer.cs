using System;
using System.Collections.Generic;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	// Token: 0x0200002D RID: 45
	internal class FurniturePlacer
	{
		// Token: 0x06000260 RID: 608 RVA: 0x00034ED8 File Offset: 0x000330D8
		public static void addAllFurnitureOwnedByFarmer()
		{
			using (List<string>.Enumerator enumerator = Game1.player.furnitureOwned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FurniturePlacer.addFurniture(enumerator.Current);
				}
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00034F2C File Offset: 0x0003312C
		public static void addFurniture(string furnitureName)
		{
			if (furnitureName.Equals("Television"))
			{
				GameLocation farmhouse = Game1.getLocationFromName("FarmHouse");
				farmhouse.Map.GetLayer("Buildings").Tiles[6, 3] = new StaticTile(farmhouse.Map.GetLayer("Buildings"), farmhouse.Map.GetTileSheet("Farmhouse"), BlendMode.Alpha, 12);
				farmhouse.Map.GetLayer("Buildings").Tiles[6, 3].Properties.Add("Action", new PropertyValue("TV"));
				farmhouse.Map.GetLayer("Buildings").Tiles[7, 3] = new StaticTile(farmhouse.Map.GetLayer("Buildings"), farmhouse.Map.GetTileSheet("Farmhouse"), BlendMode.Alpha, 13);
				farmhouse.Map.GetLayer("Buildings").Tiles[7, 3].Properties.Add("Action", new PropertyValue("TV"));
				farmhouse.Map.GetLayer("Buildings").Tiles[6, 2] = new StaticTile(farmhouse.Map.GetLayer("Buildings"), farmhouse.Map.GetTileSheet("Farmhouse"), BlendMode.Alpha, 4);
				farmhouse.Map.GetLayer("Buildings").Tiles[7, 2] = new StaticTile(farmhouse.Map.GetLayer("Buildings"), farmhouse.Map.GetTileSheet("Farmhouse"), BlendMode.Alpha, 5);
			}
			else if (furnitureName.Equals("Incubator"))
			{
				GameLocation coop = Game1.getLocationFromName("Coop");
				coop.map.GetLayer("Buildings").Tiles[1, 3] = new StaticTile(coop.map.GetLayer("Buildings"), coop.map.TileSheets[0], BlendMode.Alpha, 44);
				coop.map.GetLayer("Buildings").Tiles[1, 3].Properties.Add(new KeyValuePair<string, PropertyValue>("Action", new PropertyValue("Incubator")));
				coop.map.GetLayer("Front").Tiles[1, 2] = new StaticTile(coop.map.GetLayer("Front"), coop.map.TileSheets[0], BlendMode.Alpha, 45);
			}
			if (!Game1.player.furnitureOwned.Contains(furnitureName))
			{
				Game1.player.furnitureOwned.Add(furnitureName);
			}
		}
	}
}
