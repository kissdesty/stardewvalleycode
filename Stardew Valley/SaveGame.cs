using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;

namespace StardewValley
{
	// Token: 0x0200004D RID: 77
	public class SaveGame
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x000A3CB3 File Offset: 0x000A1EB3
		public static IEnumerator<int> Save()
		{
			return SaveGame.getSaveEnumerator();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x000A3CBA File Offset: 0x000A1EBA
		public static IEnumerator<int> getSaveEnumerator()
		{
			return new SaveGame.<getSaveEnumerator>d__46(0);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000A3CC4 File Offset: 0x000A1EC4
		public static void ensureFolderStructureExists(string tmpString = "")
		{
			string friendlyName = Game1.player.Name;
			string text = friendlyName;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (!char.IsLetterOrDigit(c))
				{
					friendlyName = friendlyName.Replace(c.ToString() ?? "", "");
				}
			}
			string filename = string.Concat(new object[]
			{
				friendlyName,
				"_",
				Game1.uniqueIDForThisGame,
				tmpString
			});
			FileInfo info = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), filename));
			if (!info.Directory.Exists)
			{
				info.Directory.Create();
			}
			info = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), filename), "dummy"));
			if (!info.Directory.Exists)
			{
				info.Directory.Create();
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000A3DD8 File Offset: 0x000A1FD8
		public static bool doesMineFileExist(int mineLevel, string specialBranch)
		{
			string friendlyName = Game1.player.Name;
			string text = friendlyName;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (!char.IsLetterOrDigit(c))
				{
					friendlyName = friendlyName.Replace(c.ToString() ?? "", "");
				}
			}
			string filename = friendlyName + "_" + Game1.uniqueIDForThisGame;
			if (specialBranch == null)
			{
				specialBranch = "";
			}
			return new FileInfo(Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), filename), "Mines"), mineLevel + specialBranch)).Exists;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000A3E98 File Offset: 0x000A2098
		public static void saveMineLevel()
		{
			SaveGame.ensureFolderStructureExists("");
			string friendlyName = Game1.player.Name;
			string text = friendlyName;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (!char.IsLetterOrDigit(c))
				{
					friendlyName = friendlyName.Replace(c.ToString() ?? "", "");
				}
			}
			friendlyName + "_" + Game1.uniqueIDForThisGame;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000A3F0F File Offset: 0x000A210F
		public static void Load(string filename)
		{
			Game1.currentLoader = SaveGame.getLoadEnumerator(filename);
			Game1.gameMode = 6;
			Game1.loadingMessage = "Loading...";
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000A3F2C File Offset: 0x000A212C
		public static IEnumerator<int> getLoadEnumerator(string file)
		{
			SaveGame.<getLoadEnumerator>d__51 expr_06 = new SaveGame.<getLoadEnumerator>d__51(0);
			expr_06.file = file;
			return expr_06;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000A3F3C File Offset: 0x000A213C
		public static void loadDataToFarmer(Farmer tmp, Farmer target = null)
		{
			if (target == null)
			{
				target = Game1.player;
			}
			target = tmp;
			target.items = tmp.items;
			target.canMove = true;
			target.sprite = new FarmerSprite(null);
			target.FarmerSprite.setOwner(target);
			target.reloadLivestockSprites();
			if (target.cookingRecipes == null || target.cookingRecipes.Count == 0)
			{
				target.cookingRecipes.Add("Fried Egg", 0);
			}
			if (target.craftingRecipes == null || target.craftingRecipes.Count == 0)
			{
				target.craftingRecipes.Add("Lumber", 0);
			}
			if (!target.songsHeard.Contains("title_day"))
			{
				target.songsHeard.Add("title_day");
			}
			if (!target.songsHeard.Contains("title_night"))
			{
				target.songsHeard.Add("title_night");
			}
			if (target.addedSpeed > 0)
			{
				target.addedSpeed = 0;
			}
			target.maxItems = tmp.maxItems;
			for (int i = 0; i < target.maxItems; i++)
			{
				if (target.items.Count <= i)
				{
					target.items.Add(null);
				}
			}
			if (target.FarmerRenderer == null)
			{
				target.FarmerRenderer = new FarmerRenderer(target.getTexture());
			}
			target.changeGender(tmp.isMale);
			target.changeAccessory(tmp.accessory);
			target.changeShirt(tmp.shirt);
			target.changePants(tmp.pantsColor);
			target.changeSkinColor(tmp.skin);
			target.changeHairColor(tmp.hairstyleColor);
			target.changeHairStyle(tmp.hair);
			if (target.boots != null)
			{
				target.changeShoeColor(tmp.boots.indexInColorSheet);
			}
			target.Stamina = tmp.Stamina;
			target.health = tmp.health;
			target.MaxStamina = tmp.MaxStamina;
			target.mostRecentBed = tmp.mostRecentBed;
			target.position = target.mostRecentBed;
			Farmer expr_1E2_cp_0_cp_0 = target;
			expr_1E2_cp_0_cp_0.position.X = expr_1E2_cp_0_cp_0.position.X - (float)Game1.tileSize;
			Game1.player = target;
			Game1.player.checkForLevelTenStatus();
			if (!Game1.player.craftingRecipes.ContainsKey("Wood Path"))
			{
				Game1.player.craftingRecipes.Add("Wood Path", 1);
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Gravel Path"))
			{
				Game1.player.craftingRecipes.Add("Gravel Path", 1);
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Cobblestone Path"))
			{
				Game1.player.craftingRecipes.Add("Cobblestone Path", 1);
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000A41C8 File Offset: 0x000A23C8
		public static void loadDataToLocations(List<GameLocation> gamelocations)
		{
			foreach (GameLocation i in gamelocations)
			{
				if (i is FarmHouse)
				{
					GameLocation realLocation = Game1.getLocationFromName(i.name);
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).upgradeLevel = (i as FarmHouse).upgradeLevel;
					(realLocation as FarmHouse).upgradeLevel = (i as FarmHouse).upgradeLevel;
					(realLocation as FarmHouse).setMapForUpgradeLevel((realLocation as FarmHouse).upgradeLevel, true);
					(realLocation as FarmHouse).wallPaper = (i as FarmHouse).wallPaper;
					(realLocation as FarmHouse).floor = (i as FarmHouse).floor;
					(realLocation as FarmHouse).furniture = (i as FarmHouse).furniture;
					(realLocation as FarmHouse).fireplaceOn = (i as FarmHouse).fireplaceOn;
					(realLocation as FarmHouse).fridge = (i as FarmHouse).fridge;
					(realLocation as FarmHouse).farmerNumberOfOwner = (i as FarmHouse).farmerNumberOfOwner;
					(realLocation as FarmHouse).resetForPlayerEntry();
					using (List<Furniture>.Enumerator enumerator2 = (realLocation as FarmHouse).furniture.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.updateDrawPosition();
						}
					}
					realLocation.lastTouchActionLocation = Game1.player.getTileLocation();
				}
				if (i.name.Equals("Farm"))
				{
					GameLocation realLocation2 = Game1.getLocationFromName(i.name);
					using (List<Building>.Enumerator enumerator3 = ((Farm)i).buildings.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							enumerator3.Current.load();
						}
					}
					((Farm)realLocation2).buildings = ((Farm)i).buildings;
					using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = ((Farm)i).animals.Values.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							enumerator4.Current.reload();
						}
					}
				}
			}
			foreach (GameLocation j in gamelocations)
			{
				GameLocation realLocation3 = Game1.getLocationFromName(j.name);
				j.name.Equals("Farm");
				for (int k = j.characters.Count - 1; k >= 0; k--)
				{
					if (!j.characters[k].DefaultPosition.Equals(Vector2.Zero))
					{
						j.characters[k].position = j.characters[k].DefaultPosition;
					}
					j.characters[k].currentLocation = realLocation3;
					if (k < j.characters.Count)
					{
						j.characters[k].reloadSprite();
					}
				}
				using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator5 = j.terrainFeatures.Values.GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						enumerator5.Current.loadSprite();
					}
				}
				foreach (KeyValuePair<Vector2, Object> v in j.objects)
				{
					v.Value.initializeLightSource(v.Key);
					v.Value.reloadSprite();
				}
				if (j.name.Equals("Farm"))
				{
					((Farm)realLocation3).buildings = ((Farm)j).buildings;
					using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = ((Farm)j).animals.Values.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							enumerator4.Current.reload();
						}
					}
					foreach (Building b in Game1.getFarm().buildings)
					{
						Vector2 v2 = new Vector2((float)b.tileX, (float)b.tileY);
						if (b.indoors is Shed)
						{
							(b.indoors as Shed).furniture = ((j as Farm).getBuildingAt(v2).indoors as Shed).furniture;
							(b.indoors as Shed).wallPaper = ((j as Farm).getBuildingAt(v2).indoors as Shed).wallPaper;
							(b.indoors as Shed).floor = ((j as Farm).getBuildingAt(v2).indoors as Shed).floor;
						}
						b.load();
						if (b.indoors is Shed)
						{
							(b.indoors as Shed).furniture = ((j as Farm).getBuildingAt(v2).indoors as Shed).furniture;
							(b.indoors as Shed).wallPaper = ((j as Farm).getBuildingAt(v2).indoors as Shed).wallPaper;
							(b.indoors as Shed).floor = ((j as Farm).getBuildingAt(v2).indoors as Shed).floor;
						}
					}
				}
				if (realLocation3 != null)
				{
					realLocation3.characters = j.characters;
					realLocation3.objects = j.objects;
					realLocation3.numberOfSpawnedObjectsOnMap = j.numberOfSpawnedObjectsOnMap;
					realLocation3.terrainFeatures = j.terrainFeatures;
					realLocation3.largeTerrainFeatures = j.largeTerrainFeatures;
					if (realLocation3.name.Equals("Farm"))
					{
						((Farm)realLocation3).animals = ((Farm)j).animals;
						(realLocation3 as Farm).piecesOfHay = (j as Farm).piecesOfHay;
						(realLocation3 as Farm).resourceClumps = (j as Farm).resourceClumps;
						(realLocation3 as Farm).hasSeenGrandpaNote = (j as Farm).hasSeenGrandpaNote;
						(realLocation3 as Farm).grandpaScore = (j as Farm).grandpaScore;
					}
					if (realLocation3 is Sewer)
					{
						(realLocation3 as Sewer).populateShopStock(Game1.dayOfMonth);
					}
					if (realLocation3 is Beach)
					{
						(realLocation3 as Beach).bridgeFixed = (j as Beach).bridgeFixed;
					}
					if (realLocation3 is Woods)
					{
						(realLocation3 as Woods).stumps = (j as Woods).stumps;
						(realLocation3 as Woods).hasFoundStardrop = (j as Woods).hasFoundStardrop;
						(realLocation3 as Woods).hasUnlockedStatue = (j as Woods).hasUnlockedStatue;
					}
					if (realLocation3 is LibraryMuseum)
					{
						(realLocation3 as LibraryMuseum).museumPieces = (j as LibraryMuseum).museumPieces;
					}
					if (realLocation3 is CommunityCenter)
					{
						(realLocation3 as CommunityCenter).bundleRewards = (j as CommunityCenter).bundleRewards;
						(realLocation3 as CommunityCenter).bundles = (j as CommunityCenter).bundles;
						(realLocation3 as CommunityCenter).areasComplete = (j as CommunityCenter).areasComplete;
					}
					if (realLocation3 is SeedShop)
					{
						(realLocation3 as SeedShop).itemsFromPlayerToSell = (j as SeedShop).itemsFromPlayerToSell;
						(realLocation3 as SeedShop).itemsToStartSellingTomorrow = (j as SeedShop).itemsToStartSellingTomorrow;
					}
					if (realLocation3 is Forest)
					{
						if (Game1.dayOfMonth % 7 % 5 == 0)
						{
							(realLocation3 as Forest).travelingMerchantDay = true;
							(realLocation3 as Forest).travelingMerchantBounds = new List<Rectangle>();
							(realLocation3 as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize, 10 * Game1.tileSize, 123 * Game1.pixelZoom, 28 * Game1.pixelZoom));
							(realLocation3 as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize + 45 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 19 * Game1.pixelZoom, 12 * Game1.pixelZoom));
							(realLocation3 as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize + 85 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 26 * Game1.pixelZoom, 12 * Game1.pixelZoom));
							(realLocation3 as Forest).travelingMerchantStock = Utility.getTravelingMerchantStock();
							using (List<Rectangle>.Enumerator enumerator7 = (realLocation3 as Forest).travelingMerchantBounds.GetEnumerator())
							{
								while (enumerator7.MoveNext())
								{
									Utility.clearObjectsInArea(enumerator7.Current, realLocation3);
								}
							}
						}
						(realLocation3 as Forest).log = (j as Forest).log;
					}
				}
			}
			Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
		}

		// Token: 0x040007BD RID: 1981
		public static XmlSerializer serializer = new XmlSerializer(typeof(SaveGame), new Type[]
		{
			typeof(Tool),
			typeof(GameLocation),
			typeof(Crow),
			typeof(Duggy),
			typeof(Bug),
			typeof(BigSlime),
			typeof(Fireball),
			typeof(Ghost),
			typeof(Child),
			typeof(Pet),
			typeof(Dog),
			typeof(StardewValley.Characters.Cat),
			typeof(Horse),
			typeof(GreenSlime),
			typeof(LavaCrab),
			typeof(RockCrab),
			typeof(ShadowGuy),
			typeof(SkeletonMage),
			typeof(SquidKid),
			typeof(Grub),
			typeof(Fly),
			typeof(DustSpirit),
			typeof(Quest),
			typeof(MetalHead),
			typeof(ShadowGirl),
			typeof(Monster),
			typeof(TerrainFeature)
		});

		// Token: 0x040007BE RID: 1982
		public static XmlSerializer farmerSerializer = new XmlSerializer(typeof(Farmer), new Type[]
		{
			typeof(Tool)
		});

		// Token: 0x040007BF RID: 1983
		public static XmlSerializer locationSerializer = new XmlSerializer(typeof(GameLocation), new Type[]
		{
			typeof(Tool),
			typeof(Crow),
			typeof(Duggy),
			typeof(Fireball),
			typeof(Ghost),
			typeof(GreenSlime),
			typeof(LavaCrab),
			typeof(RockCrab),
			typeof(ShadowGuy),
			typeof(SkeletonWarrior),
			typeof(Child),
			typeof(Pet),
			typeof(Dog),
			typeof(StardewValley.Characters.Cat),
			typeof(Horse),
			typeof(SquidKid),
			typeof(Grub),
			typeof(Fly),
			typeof(DustSpirit),
			typeof(Bug),
			typeof(BigSlime),
			typeof(BreakableContainer),
			typeof(MetalHead),
			typeof(ShadowGirl),
			typeof(Monster),
			typeof(TerrainFeature)
		});

		// Token: 0x040007C0 RID: 1984
		public Farmer player;

		// Token: 0x040007C1 RID: 1985
		public List<GameLocation> locations;

		// Token: 0x040007C2 RID: 1986
		public string currentSeason;

		// Token: 0x040007C3 RID: 1987
		public string samBandName;

		// Token: 0x040007C4 RID: 1988
		public string elliottBookName;

		// Token: 0x040007C5 RID: 1989
		public List<string> mailbox;

		// Token: 0x040007C6 RID: 1990
		public int dayOfMonth;

		// Token: 0x040007C7 RID: 1991
		public int year;

		// Token: 0x040007C8 RID: 1992
		public int farmerWallpaper;

		// Token: 0x040007C9 RID: 1993
		public int FarmerFloor;

		// Token: 0x040007CA RID: 1994
		public int countdownToWedding;

		// Token: 0x040007CB RID: 1995
		public int currentWallpaper;

		// Token: 0x040007CC RID: 1996
		public int currentFloor;

		// Token: 0x040007CD RID: 1997
		public int currentSongIndex;

		// Token: 0x040007CE RID: 1998
		public Point incubatingEgg;

		// Token: 0x040007CF RID: 1999
		public double chanceToRainTomorrow;

		// Token: 0x040007D0 RID: 2000
		public double dailyLuck;

		// Token: 0x040007D1 RID: 2001
		public ulong uniqueIDForThisGame;

		// Token: 0x040007D2 RID: 2002
		public bool weddingToday;

		// Token: 0x040007D3 RID: 2003
		public bool isRaining;

		// Token: 0x040007D4 RID: 2004
		public bool isDebrisWeather;

		// Token: 0x040007D5 RID: 2005
		public bool shippingTax;

		// Token: 0x040007D6 RID: 2006
		public bool bloomDay;

		// Token: 0x040007D7 RID: 2007
		public bool isLightning;

		// Token: 0x040007D8 RID: 2008
		public bool isSnowing;

		// Token: 0x040007D9 RID: 2009
		public bool shouldSpawnMonsters;

		// Token: 0x040007DA RID: 2010
		public Stats stats;

		// Token: 0x040007DB RID: 2011
		public static SaveGame loaded;

		// Token: 0x040007DC RID: 2012
		public float musicVolume;

		// Token: 0x040007DD RID: 2013
		public float soundVolume;

		// Token: 0x040007DE RID: 2014
		public int[] cropsOfTheWeek;

		// Token: 0x040007DF RID: 2015
		public Object dishOfTheDay;

		// Token: 0x040007E0 RID: 2016
		public long latestID;

		// Token: 0x040007E1 RID: 2017
		public Options options;

		// Token: 0x040007E2 RID: 2018
		public SerializableDictionary<int, MineInfo> mine_permanentMineChanges;

		// Token: 0x040007E3 RID: 2019
		public List<ResourceClump> mine_resourceClumps = new List<ResourceClump>();

		// Token: 0x040007E4 RID: 2020
		public int mine_mineLevel;

		// Token: 0x040007E5 RID: 2021
		public int mine_nextLevel;

		// Token: 0x040007E6 RID: 2022
		public int mine_lowestLevelReached;

		// Token: 0x040007E7 RID: 2023
		public int minecartHighScore;

		// Token: 0x040007E8 RID: 2024
		public int weatherForTomorrow;

		// Token: 0x040007E9 RID: 2025
		public int whichFarm;
	}
}
