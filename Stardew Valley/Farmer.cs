using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.Tools;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	// Token: 0x02000042 RID: 66
	public class Farmer : Character, IComparable
	{
		// Token: 0x1700002A RID: 42
		[XmlIgnore]
		public int MaxItems
		{
			// Token: 0x06000329 RID: 809 RVA: 0x00040718 File Offset: 0x0003E918
			get
			{
				return this.maxItems;
			}
			// Token: 0x0600032A RID: 810 RVA: 0x00040720 File Offset: 0x0003E920
			set
			{
				this.maxItems = value;
			}
		}

		// Token: 0x1700002B RID: 43
		[XmlIgnore]
		public int Level
		{
			// Token: 0x0600032B RID: 811 RVA: 0x00040729 File Offset: 0x0003E929
			get
			{
				return (this.farmingLevel + this.fishingLevel + this.foragingLevel + this.combatLevel + this.miningLevel + this.luckLevel) / 2;
			}
		}

		// Token: 0x1700002C RID: 44
		[XmlIgnore]
		public int CraftingTime
		{
			// Token: 0x0600032C RID: 812 RVA: 0x00040756 File Offset: 0x0003E956
			get
			{
				return this.craftingTime;
			}
			// Token: 0x0600032D RID: 813 RVA: 0x0004075E File Offset: 0x0003E95E
			set
			{
				this.craftingTime = value;
			}
		}

		// Token: 0x1700002D RID: 45
		[XmlIgnore]
		public int NewSkillPointsToSpend
		{
			// Token: 0x0600032E RID: 814 RVA: 0x00040767 File Offset: 0x0003E967
			get
			{
				return this.newSkillPointsToSpend;
			}
			// Token: 0x0600032F RID: 815 RVA: 0x0004076F File Offset: 0x0003E96F
			set
			{
				this.newSkillPointsToSpend = value;
			}
		}

		// Token: 0x1700002E RID: 46
		[XmlIgnore]
		public int FarmingLevel
		{
			// Token: 0x06000330 RID: 816 RVA: 0x00040778 File Offset: 0x0003E978
			get
			{
				return this.farmingLevel + this.addedFarmingLevel;
			}
			// Token: 0x06000331 RID: 817 RVA: 0x00040787 File Offset: 0x0003E987
			set
			{
				this.farmingLevel = value;
			}
		}

		// Token: 0x1700002F RID: 47
		[XmlIgnore]
		public int MiningLevel
		{
			// Token: 0x06000332 RID: 818 RVA: 0x00040790 File Offset: 0x0003E990
			get
			{
				return this.miningLevel + this.addedMiningLevel;
			}
			// Token: 0x06000333 RID: 819 RVA: 0x0004079F File Offset: 0x0003E99F
			set
			{
				this.miningLevel = value;
			}
		}

		// Token: 0x17000030 RID: 48
		[XmlIgnore]
		public int CombatLevel
		{
			// Token: 0x06000334 RID: 820 RVA: 0x000407A8 File Offset: 0x0003E9A8
			get
			{
				return this.combatLevel + this.addedCombatLevel;
			}
			// Token: 0x06000335 RID: 821 RVA: 0x000407B7 File Offset: 0x0003E9B7
			set
			{
				this.combatLevel = value;
			}
		}

		// Token: 0x17000031 RID: 49
		[XmlIgnore]
		public int ForagingLevel
		{
			// Token: 0x06000336 RID: 822 RVA: 0x000407C0 File Offset: 0x0003E9C0
			get
			{
				return this.foragingLevel + this.addedForagingLevel;
			}
			// Token: 0x06000337 RID: 823 RVA: 0x000407CF File Offset: 0x0003E9CF
			set
			{
				this.foragingLevel = value;
			}
		}

		// Token: 0x17000032 RID: 50
		[XmlIgnore]
		public int FishingLevel
		{
			// Token: 0x06000338 RID: 824 RVA: 0x000407D8 File Offset: 0x0003E9D8
			get
			{
				return this.fishingLevel + this.addedFishingLevel;
			}
			// Token: 0x06000339 RID: 825 RVA: 0x000407E7 File Offset: 0x0003E9E7
			set
			{
				this.fishingLevel = value;
			}
		}

		// Token: 0x17000033 RID: 51
		[XmlIgnore]
		public int LuckLevel
		{
			// Token: 0x0600033A RID: 826 RVA: 0x000407F0 File Offset: 0x0003E9F0
			get
			{
				return this.luckLevel + this.addedLuckLevel;
			}
			// Token: 0x0600033B RID: 827 RVA: 0x000407FF File Offset: 0x0003E9FF
			set
			{
				this.luckLevel = value;
			}
		}

		// Token: 0x17000034 RID: 52
		[XmlIgnore]
		public int HouseUpgradeLevel
		{
			// Token: 0x0600033C RID: 828 RVA: 0x00040808 File Offset: 0x0003EA08
			get
			{
				return this.houseUpgradeLevel;
			}
			// Token: 0x0600033D RID: 829 RVA: 0x00040810 File Offset: 0x0003EA10
			set
			{
				this.houseUpgradeLevel = value;
			}
		}

		// Token: 0x17000035 RID: 53
		[XmlIgnore]
		public int CoopUpgradeLevel
		{
			// Token: 0x0600033E RID: 830 RVA: 0x00040819 File Offset: 0x0003EA19
			get
			{
				return this.coopUpgradeLevel;
			}
			// Token: 0x0600033F RID: 831 RVA: 0x00040821 File Offset: 0x0003EA21
			set
			{
				this.coopUpgradeLevel = value;
			}
		}

		// Token: 0x17000036 RID: 54
		[XmlIgnore]
		public int BarnUpgradeLevel
		{
			// Token: 0x06000340 RID: 832 RVA: 0x0004082A File Offset: 0x0003EA2A
			get
			{
				return this.barnUpgradeLevel;
			}
			// Token: 0x06000341 RID: 833 RVA: 0x00040832 File Offset: 0x0003EA32
			set
			{
				this.barnUpgradeLevel = value;
			}
		}

		// Token: 0x17000037 RID: 55
		[XmlIgnore]
		public Microsoft.Xna.Framework.Rectangle TemporaryImpassableTile
		{
			// Token: 0x06000342 RID: 834 RVA: 0x0004083B File Offset: 0x0003EA3B
			get
			{
				return this.temporaryImpassableTile;
			}
			// Token: 0x06000343 RID: 835 RVA: 0x00040843 File Offset: 0x0003EA43
			set
			{
				this.temporaryImpassableTile = value;
			}
		}

		// Token: 0x17000038 RID: 56
		[XmlIgnore]
		public List<Item> Items
		{
			// Token: 0x06000344 RID: 836 RVA: 0x0004084C File Offset: 0x0003EA4C
			get
			{
				return this.items;
			}
			// Token: 0x06000345 RID: 837 RVA: 0x00040854 File Offset: 0x0003EA54
			set
			{
				this.items = value;
			}
		}

		// Token: 0x17000039 RID: 57
		[XmlIgnore]
		public int MagneticRadius
		{
			// Token: 0x06000346 RID: 838 RVA: 0x0004085D File Offset: 0x0003EA5D
			get
			{
				return this.magneticRadius;
			}
			// Token: 0x06000347 RID: 839 RVA: 0x00040865 File Offset: 0x0003EA65
			set
			{
				this.magneticRadius = value;
			}
		}

		// Token: 0x1700003A RID: 58
		[XmlIgnore]
		public Object ActiveObject
		{
			// Token: 0x06000348 RID: 840 RVA: 0x00040870 File Offset: 0x0003EA70
			get
			{
				if (this.currentToolIndex < this.items.Count && this.items[this.currentToolIndex] != null && this.items[this.currentToolIndex] is Object)
				{
					return (Object)this.items[this.currentToolIndex];
				}
				return null;
			}
			// Token: 0x06000349 RID: 841 RVA: 0x000408D3 File Offset: 0x0003EAD3
			set
			{
				if (value == null)
				{
					this.removeItemFromInventory(this.ActiveObject);
					return;
				}
				this.addItemToInventory(value, this.CurrentToolIndex);
			}
		}

		// Token: 0x1700003B RID: 59
		[XmlIgnore]
		public string Name
		{
			// Token: 0x0600034A RID: 842 RVA: 0x000408F3 File Offset: 0x0003EAF3
			get
			{
				return this.name;
			}
			// Token: 0x0600034B RID: 843 RVA: 0x000408FB File Offset: 0x0003EAFB
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700003C RID: 60
		[XmlIgnore]
		public bool IsMale
		{
			// Token: 0x0600034C RID: 844 RVA: 0x00040904 File Offset: 0x0003EB04
			get
			{
				return this.isMale;
			}
			// Token: 0x0600034D RID: 845 RVA: 0x0004090C File Offset: 0x0003EB0C
			set
			{
				this.isMale = value;
			}
		}

		// Token: 0x1700003D RID: 61
		[XmlIgnore]
		public List<int> DialogueQuestionsAnswered
		{
			// Token: 0x0600034E RID: 846 RVA: 0x00040915 File Offset: 0x0003EB15
			get
			{
				return this.dialogueQuestionsAnswered;
			}
			// Token: 0x0600034F RID: 847 RVA: 0x0004091D File Offset: 0x0003EB1D
			set
			{
				this.dialogueQuestionsAnswered = value;
			}
		}

		// Token: 0x1700003E RID: 62
		[XmlIgnore]
		public int WoodPieces
		{
			// Token: 0x06000350 RID: 848 RVA: 0x00040926 File Offset: 0x0003EB26
			get
			{
				return this.woodPieces;
			}
			// Token: 0x06000351 RID: 849 RVA: 0x0004092E File Offset: 0x0003EB2E
			set
			{
				this.woodPieces = value;
			}
		}

		// Token: 0x1700003F RID: 63
		[XmlIgnore]
		public int StonePieces
		{
			// Token: 0x06000352 RID: 850 RVA: 0x00040937 File Offset: 0x0003EB37
			get
			{
				return this.stonePieces;
			}
			// Token: 0x06000353 RID: 851 RVA: 0x0004093F File Offset: 0x0003EB3F
			set
			{
				this.stonePieces = value;
			}
		}

		// Token: 0x17000040 RID: 64
		[XmlIgnore]
		public int CopperPieces
		{
			// Token: 0x06000354 RID: 852 RVA: 0x00040948 File Offset: 0x0003EB48
			get
			{
				return this.copperPieces;
			}
			// Token: 0x06000355 RID: 853 RVA: 0x00040950 File Offset: 0x0003EB50
			set
			{
				this.copperPieces = value;
			}
		}

		// Token: 0x17000041 RID: 65
		[XmlIgnore]
		public int IronPieces
		{
			// Token: 0x06000356 RID: 854 RVA: 0x00040959 File Offset: 0x0003EB59
			get
			{
				return this.ironPieces;
			}
			// Token: 0x06000357 RID: 855 RVA: 0x00040961 File Offset: 0x0003EB61
			set
			{
				this.ironPieces = value;
			}
		}

		// Token: 0x17000042 RID: 66
		[XmlIgnore]
		public int CoalPieces
		{
			// Token: 0x06000358 RID: 856 RVA: 0x0004096A File Offset: 0x0003EB6A
			get
			{
				return this.coalPieces;
			}
			// Token: 0x06000359 RID: 857 RVA: 0x00040972 File Offset: 0x0003EB72
			set
			{
				this.coalPieces = value;
			}
		}

		// Token: 0x17000043 RID: 67
		[XmlIgnore]
		public int GoldPieces
		{
			// Token: 0x0600035A RID: 858 RVA: 0x0004097B File Offset: 0x0003EB7B
			get
			{
				return this.goldPieces;
			}
			// Token: 0x0600035B RID: 859 RVA: 0x00040983 File Offset: 0x0003EB83
			set
			{
				this.goldPieces = value;
			}
		}

		// Token: 0x17000044 RID: 68
		[XmlIgnore]
		public int IridiumPieces
		{
			// Token: 0x0600035C RID: 860 RVA: 0x0004098C File Offset: 0x0003EB8C
			get
			{
				return this.iridiumPieces;
			}
			// Token: 0x0600035D RID: 861 RVA: 0x00040994 File Offset: 0x0003EB94
			set
			{
				this.iridiumPieces = value;
			}
		}

		// Token: 0x17000045 RID: 69
		[XmlIgnore]
		public int QuartzPieces
		{
			// Token: 0x0600035E RID: 862 RVA: 0x0004099D File Offset: 0x0003EB9D
			get
			{
				return this.quartzPieces;
			}
			// Token: 0x0600035F RID: 863 RVA: 0x000409A5 File Offset: 0x0003EBA5
			set
			{
				this.quartzPieces = value;
			}
		}

		// Token: 0x17000046 RID: 70
		[XmlIgnore]
		public int Feed
		{
			// Token: 0x06000360 RID: 864 RVA: 0x000409AE File Offset: 0x0003EBAE
			get
			{
				return this.feed;
			}
			// Token: 0x06000361 RID: 865 RVA: 0x000409B6 File Offset: 0x0003EBB6
			set
			{
				this.feed = value;
			}
		}

		// Token: 0x17000047 RID: 71
		[XmlIgnore]
		public bool CanMove
		{
			// Token: 0x06000362 RID: 866 RVA: 0x000409BF File Offset: 0x0003EBBF
			get
			{
				return this.canMove;
			}
			// Token: 0x06000363 RID: 867 RVA: 0x000409C7 File Offset: 0x0003EBC7
			set
			{
				this.canMove = value;
			}
		}

		// Token: 0x17000048 RID: 72
		[XmlIgnore]
		public bool UsingTool
		{
			// Token: 0x06000364 RID: 868 RVA: 0x000409D0 File Offset: 0x0003EBD0
			get
			{
				return this.usingTool;
			}
			// Token: 0x06000365 RID: 869 RVA: 0x000409D8 File Offset: 0x0003EBD8
			set
			{
				this.usingTool = value;
			}
		}

		// Token: 0x17000049 RID: 73
		[XmlIgnore]
		public Tool CurrentTool
		{
			// Token: 0x06000366 RID: 870 RVA: 0x000409E1 File Offset: 0x0003EBE1
			get
			{
				if (this.CurrentItem != null && this.CurrentItem is Tool)
				{
					return (Tool)this.CurrentItem;
				}
				return null;
			}
			// Token: 0x06000367 RID: 871 RVA: 0x00040A05 File Offset: 0x0003EC05
			set
			{
				this.items[this.CurrentToolIndex] = value;
			}
		}

		// Token: 0x1700004A RID: 74
		[XmlIgnore]
		public Item CurrentItem
		{
			// Token: 0x06000368 RID: 872 RVA: 0x00040A19 File Offset: 0x0003EC19
			get
			{
				if (this.currentToolIndex >= this.items.Count)
				{
					return null;
				}
				return this.items[this.currentToolIndex];
			}
		}

		// Token: 0x1700004B RID: 75
		[XmlIgnore]
		public int CurrentToolIndex
		{
			// Token: 0x06000369 RID: 873 RVA: 0x00040A41 File Offset: 0x0003EC41
			get
			{
				return this.currentToolIndex;
			}
			// Token: 0x0600036A RID: 874 RVA: 0x00040A49 File Offset: 0x0003EC49
			set
			{
				if (this.currentToolIndex >= 0 && this.CurrentItem != null && value != this.currentToolIndex)
				{
					this.CurrentItem.actionWhenStopBeingHeld(this);
				}
				this.currentToolIndex = value;
			}
		}

		// Token: 0x1700004C RID: 76
		[XmlIgnore]
		public float Stamina
		{
			// Token: 0x0600036B RID: 875 RVA: 0x00040A78 File Offset: 0x0003EC78
			get
			{
				return this.stamina;
			}
			// Token: 0x0600036C RID: 876 RVA: 0x00040A80 File Offset: 0x0003EC80
			set
			{
				this.stamina = Math.Min((float)this.maxStamina, Math.Max(value, -16f));
			}
		}

		// Token: 0x1700004D RID: 77
		[XmlIgnore]
		public int MaxStamina
		{
			// Token: 0x0600036D RID: 877 RVA: 0x00040A9F File Offset: 0x0003EC9F
			get
			{
				return this.maxStamina;
			}
			// Token: 0x0600036E RID: 878 RVA: 0x00040AA7 File Offset: 0x0003ECA7
			set
			{
				this.maxStamina = value;
			}
		}

		// Token: 0x1700004E RID: 78
		[XmlIgnore]
		public bool IsMainPlayer
		{
			// Token: 0x0600036F RID: 879 RVA: 0x00040AB0 File Offset: 0x0003ECB0
			get
			{
				return this.uniqueMultiplayerID == Game1.player.uniqueMultiplayerID;
			}
		}

		// Token: 0x1700004F RID: 79
		[XmlIgnore]
		public FarmerSprite FarmerSprite
		{
			// Token: 0x06000370 RID: 880 RVA: 0x00040AC4 File Offset: 0x0003ECC4
			get
			{
				return (FarmerSprite)this.sprite;
			}
			// Token: 0x06000371 RID: 881 RVA: 0x000081E8 File Offset: 0x000063E8
			set
			{
				this.sprite = value;
			}
		}

		// Token: 0x17000050 RID: 80
		[XmlIgnore]
		public FarmerRenderer FarmerRenderer
		{
			// Token: 0x06000372 RID: 882 RVA: 0x00040AD1 File Offset: 0x0003ECD1
			get
			{
				return this.farmerRenderer;
			}
			// Token: 0x06000373 RID: 883 RVA: 0x00040AD9 File Offset: 0x0003ECD9
			set
			{
				this.farmerRenderer = value;
			}
		}

		// Token: 0x17000051 RID: 81
		[XmlIgnore]
		public int Money
		{
			// Token: 0x06000374 RID: 884 RVA: 0x00040AE2 File Offset: 0x0003ECE2
			get
			{
				return this.money;
			}
			// Token: 0x06000375 RID: 885 RVA: 0x00040AEA File Offset: 0x0003ECEA
			set
			{
				if (value > this.money)
				{
					this.totalMoneyEarned += (uint)(value - this.money);
					Game1.stats.checkForMoneyAchievements();
				}
				else
				{
					int arg_31_0 = this.money;
				}
				this.money = value;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00040B28 File Offset: 0x0003ED28
		public Farmer()
		{
			this.farmerTextureManager = Game1.content.CreateTemporary();
			this.farmerRenderer = new FarmerRenderer(this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (this.isMale ? "" : "girl_") + "base"));
			this.currentLocation = Game1.getLocationFromName("FarmHouse");
			Game1.player.sprite = new FarmerSprite(null);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00040DD4 File Offset: 0x0003EFD4
		public Farmer(FarmerSprite sprite, Vector2 position, int speed, string name, List<Item> initialTools, bool isMale) : base(sprite, position, speed, name)
		{
			this.farmerTextureManager = Game1.content.CreateTemporary();
			this.pantsColor = new Color(46, 85, 183);
			this.hairstyleColor = new Color(193, 90, 50);
			this.newEyeColor = new Color(122, 68, 52);
			this.name = name;
			this.currentToolIndex = 0;
			this.isMale = isMale;
			this.basicShipped = new SerializableDictionary<int, int>();
			this.fishCaught = new SerializableDictionary<int, int[]>();
			this.archaeologyFound = new SerializableDictionary<int, int[]>();
			this.mineralsFound = new SerializableDictionary<int, int>();
			this.recipesCooked = new SerializableDictionary<int, int>();
			this.friendships = new SerializableDictionary<string, int[]>();
			this.stamina = (float)this.maxStamina;
			this.items = initialTools;
			if (this.items == null)
			{
				this.items = new List<Item>();
			}
			for (int i = this.items.Count; i < this.maxItems; i++)
			{
				this.items.Add(null);
			}
			this.activeDialogueEvents.Add("Introduction", 6);
			name = "Cam";
			this.farmerRenderer = new FarmerRenderer(this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (isMale ? "" : "girl_") + "base"));
			this.currentLocation = Game1.getLocationFromName("FarmHouse");
			if (this.currentLocation != null)
			{
				this.mostRecentBed = Utility.PointToVector2((this.currentLocation as FarmHouse).getBedSpot()) * (float)Game1.tileSize;
				return;
			}
			this.mostRecentBed = new Vector2(9f, 9f) * (float)Game1.tileSize;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000411B8 File Offset: 0x0003F3B8
		public Texture2D getTexture()
		{
			if (this.farmerTextureManager == null)
			{
				this.farmerTextureManager = Game1.content.CreateTemporary();
			}
			return this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (this.isMale ? "" : "girl_") + "base");
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00002834 File Offset: 0x00000A34
		public void checkForLevelTenStatus()
		{
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0004120B File Offset: 0x0003F40B
		public void unload()
		{
			if (this.farmerTextureManager != null)
			{
				this.farmerTextureManager.Unload();
				this.farmerTextureManager.Dispose();
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0004122C File Offset: 0x0003F42C
		public void setInventory(List<Item> newInventory)
		{
			this.items = newInventory;
			if (this.items == null)
			{
				this.items = new List<Item>();
			}
			for (int i = this.items.Count; i < this.maxItems; i++)
			{
				this.items.Add(null);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0004127C File Offset: 0x0003F47C
		public void makeThisTheActiveObject(Object o)
		{
			if (this.freeSpotsInInventory() > 0)
			{
				Item i = this.CurrentItem;
				this.ActiveObject = o;
				this.addItemToInventory(i);
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000412A8 File Offset: 0x0003F4A8
		public int getNumberOfChildren()
		{
			int number = 0;
			foreach (NPC i in Utility.getHomeOfFarmer(Game1.player).characters)
			{
				if (i is Child && (i as Child).isChildOf(Game1.player))
				{
					number++;
				}
			}
			foreach (NPC j in Game1.getLocationFromName("Farm").characters)
			{
				if (j is Child && (j as Child).isChildOf(Game1.player))
				{
					number++;
				}
			}
			return number;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00041380 File Offset: 0x0003F580
		public void mountUp(Horse mount)
		{
			this.mount = mount;
			this.xOffset = -11f;
			this.position = Utility.PointToVector2(mount.GetBoundingBox().Location);
			this.position.Y = this.position.Y - (float)(Game1.pixelZoom * 4);
			this.position.X = this.position.X - (float)(Game1.pixelZoom * 2);
			this.speed = 2;
			this.showNotCarrying();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000413F3 File Offset: 0x0003F5F3
		public Horse getMount()
		{
			return this.mount;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000413FC File Offset: 0x0003F5FC
		public void dismount()
		{
			if (this.mount != null)
			{
				this.mount = null;
			}
			this.collisionNPC = null;
			this.running = false;
			this.speed = ((Game1.isOneOfTheseKeysDown(Keyboard.GetState(), Game1.options.runButton) && !Game1.options.autoRun) ? 5 : 2);
			bool isRunning = this.speed == 5;
			this.running = isRunning;
			if (this.running)
			{
				this.speed = 5;
			}
			else
			{
				this.speed = 2;
				this.Halt();
			}
			this.Halt();
			this.xOffset = 0f;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00041492 File Offset: 0x0003F692
		public bool isRidingHorse()
		{
			return this.mount != null && !Game1.eventUp;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000414A8 File Offset: 0x0003F6A8
		public List<Child> getChildren()
		{
			List<Child> children = new List<Child>();
			foreach (NPC i in Utility.getHomeOfFarmer(Game1.player).characters)
			{
				if (i is Child && (i as Child).isChildOf(Game1.player))
				{
					children.Add(i as Child);
				}
			}
			foreach (NPC j in Game1.getLocationFromName("Farm").characters)
			{
				if (j is Child && (j as Child).isChildOf(Game1.player))
				{
					children.Add(j as Child);
				}
			}
			return children;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00041594 File Offset: 0x0003F794
		public string getHisOrHer()
		{
			if (!this.isMale)
			{
				return "her";
			}
			return "his";
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000415AC File Offset: 0x0003F7AC
		public Tool getToolFromName(string name)
		{
			foreach (Item t in this.items)
			{
				if (t != null && t is Tool && t.Name.Contains(name))
				{
					return (Tool)t;
				}
			}
			return null;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00041620 File Offset: 0x0003F820
		public override void SetMovingDown(bool b)
		{
			this.setMoving((byte)(4 + (b ? 0 : 32)));
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00041633 File Offset: 0x0003F833
		public override void SetMovingRight(bool b)
		{
			this.setMoving((byte)(2 + (b ? 0 : 32)));
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00041646 File Offset: 0x0003F846
		public override void SetMovingUp(bool b)
		{
			this.setMoving((byte)(1 + (b ? 0 : 32)));
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00041659 File Offset: 0x0003F859
		public override void SetMovingLeft(bool b)
		{
			this.setMoving((byte)(8 + (b ? 0 : 32)));
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0004166C File Offset: 0x0003F86C
		public int getFriendshipLevelForNPC(string name)
		{
			if (this.friendships.ContainsKey(name))
			{
				return this.friendships[name][0];
			}
			return 0;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0004168C File Offset: 0x0003F88C
		public int getFriendshipHeartLevelForNPC(string name)
		{
			return this.getFriendshipLevelForNPC(name) / 250;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0004169C File Offset: 0x0003F89C
		public bool hasAFriendWithHeartLevel(int heartLevel, bool datablesOnly)
		{
			foreach (NPC i in Utility.getAllCharacters())
			{
				if ((!datablesOnly || i.datable) && this.getFriendshipHeartLevelForNPC(i.name) >= heartLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00041708 File Offset: 0x0003F908
		public int getTallyOfObject(int index, bool bigCraftable)
		{
			int tally = 0;
			foreach (Item i in this.items)
			{
				if (i is Object && (i as Object).ParentSheetIndex == index && (i as Object).bigCraftable == bigCraftable)
				{
					tally += i.Stack;
				}
			}
			return tally;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00041784 File Offset: 0x0003F984
		public bool areAllItemsNull()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000417B8 File Offset: 0x0003F9B8
		public void shipAll()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && this.items[i] is Object)
				{
					this.shippedBasic(((Object)this.items[i]).ParentSheetIndex, this.items[i].Stack);
					for (int j = 0; j < this.items[i].Stack; j++)
					{
						Game1.shipObject((Object)this.items[i].getOne());
					}
					this.items[i] = null;
				}
			}
			Game1.playSound("Ship");
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00041880 File Offset: 0x0003FA80
		public void shippedBasic(int index, int number)
		{
			if (this.basicShipped.ContainsKey(index))
			{
				SerializableDictionary<int, int> serializableDictionary = this.basicShipped;
				serializableDictionary[index] += number;
				return;
			}
			this.basicShipped.Add(index, number);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x000418C4 File Offset: 0x0003FAC4
		public void shiftToolbar(bool right)
		{
			if (this.items == null || this.items.Count < 12)
			{
				return;
			}
			if (this.UsingTool || Game1.dialogueUp || (!Game1.pickingTool && !Game1.player.CanMove) || this.areAllItemsNull() || Game1.eventUp)
			{
				return;
			}
			Game1.soundBank.PlayCue("shwip");
			if (right)
			{
				List<Item> toMove = this.items.GetRange(0, 12);
				this.items.RemoveRange(0, 12);
				this.items.AddRange(toMove);
			}
			else
			{
				List<Item> toMove2 = this.items.GetRange(this.items.Count - 12, 12);
				for (int i = 0; i < this.items.Count - 12; i++)
				{
					toMove2.Add(this.items[i]);
				}
				this.items = toMove2;
			}
			for (int j = 0; j < Game1.onScreenMenus.Count; j++)
			{
				if (Game1.onScreenMenus[j] is Toolbar)
				{
					(Game1.onScreenMenus[j] as Toolbar).shifted(right);
					return;
				}
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000419E4 File Offset: 0x0003FBE4
		public void foundArtifact(int index, int number)
		{
			if (this.archaeologyFound == null)
			{
				this.archaeologyFound = new SerializableDictionary<int, int[]>();
			}
			if (this.archaeologyFound.ContainsKey(index))
			{
				this.archaeologyFound[index][0] += number;
				this.archaeologyFound[index][1] += number;
				return;
			}
			if (this.archaeologyFound.Count == 0)
			{
				if (!this.eventsSeen.Contains(0) && index != 102)
				{
					this.addQuest(23);
				}
				this.mailReceived.Add("artifactFound");
				this.holdUpItemThenMessage(new Object(index, 1, false, -1, 0), true);
			}
			this.archaeologyFound.Add(index, new int[]
			{
				number,
				number
			});
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00041AA4 File Offset: 0x0003FCA4
		public void cookedRecipe(int index)
		{
			if (this.recipesCooked == null)
			{
				this.recipesCooked = new SerializableDictionary<int, int>();
			}
			if (this.recipesCooked.ContainsKey(index))
			{
				SerializableDictionary<int, int> expr_29 = this.recipesCooked;
				int num = expr_29[index];
				expr_29[index] = num + 1;
				return;
			}
			this.recipesCooked.Add(index, 1);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00041AFC File Offset: 0x0003FCFC
		public bool caughtFish(int index, int size)
		{
			if (this.fishCaught == null)
			{
				this.fishCaught = new SerializableDictionary<int, int[]>();
			}
			if (index >= 167 && index < 173)
			{
				return false;
			}
			bool sizeRecord = false;
			if (this.fishCaught.ContainsKey(index))
			{
				this.fishCaught[index][0]++;
				Game1.stats.checkForFishingAchievements();
				if (size > this.fishCaught[index][1])
				{
					this.fishCaught[index][1] = size;
					sizeRecord = true;
				}
			}
			else
			{
				this.fishCaught.Add(index, new int[]
				{
					1,
					size
				});
				Game1.stats.checkForFishingAchievements();
			}
			this.checkForQuestComplete(null, index, -1, null, null, 7, -1);
			return sizeRecord;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00041BB8 File Offset: 0x0003FDB8
		public void gainExperience(int which, int howMuch)
		{
			if (which == 5 || howMuch <= 0)
			{
				return;
			}
			int newLevel = Farmer.checkForLevelGain(this.experiencePoints[which], this.experiencePoints[which] + howMuch);
			this.experiencePoints[which] += howMuch;
			int oldLevel = -1;
			if (newLevel != -1)
			{
				switch (which)
				{
				case 0:
					oldLevel = this.farmingLevel;
					this.farmingLevel = newLevel;
					break;
				case 1:
					oldLevel = this.fishingLevel;
					this.fishingLevel = newLevel;
					break;
				case 2:
					oldLevel = this.foragingLevel;
					this.foragingLevel = newLevel;
					break;
				case 3:
					oldLevel = this.miningLevel;
					this.miningLevel = newLevel;
					break;
				case 4:
					oldLevel = this.combatLevel;
					this.combatLevel = newLevel;
					break;
				case 5:
					oldLevel = this.luckLevel;
					this.luckLevel = newLevel;
					break;
				}
			}
			if (newLevel > oldLevel)
			{
				for (int i = oldLevel + 1; i <= newLevel; i++)
				{
					this.newLevels.Add(new Point(which, i));
					int arg_DF_0 = this.newLevels.Count;
				}
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00041CB0 File Offset: 0x0003FEB0
		public int getEffectiveSkillLevel(int whichSkill)
		{
			if (whichSkill < 0 || whichSkill > 5)
			{
				return -1;
			}
			int[] effectiveSkillLevels = new int[]
			{
				this.farmingLevel,
				this.fishingLevel,
				this.foragingLevel,
				this.miningLevel,
				this.combatLevel,
				this.luckLevel
			};
			for (int i = 0; i < this.newLevels.Count; i++)
			{
				effectiveSkillLevels[this.newLevels[i].X] -= this.newLevels[i].Y;
			}
			return effectiveSkillLevels[whichSkill];
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00041D4C File Offset: 0x0003FF4C
		public static int checkForLevelGain(int oldXP, int newXP)
		{
			int highestLevel = -1;
			if (oldXP < 100 && newXP >= 100)
			{
				highestLevel = 1;
			}
			if (oldXP < 380 && newXP >= 380)
			{
				highestLevel = 2;
			}
			if (oldXP < 770 && newXP >= 770)
			{
				highestLevel = 3;
			}
			if (oldXP < 1300 && newXP >= 1300)
			{
				highestLevel = 4;
			}
			if (oldXP < 2150 && newXP >= 2150)
			{
				highestLevel = 5;
			}
			if (oldXP < 3300 && newXP >= 3300)
			{
				highestLevel = 6;
			}
			if (oldXP < 4800 && newXP >= 4800)
			{
				highestLevel = 7;
			}
			if (oldXP < 6900 && newXP >= 6900)
			{
				highestLevel = 8;
			}
			if (oldXP < 10000 && newXP >= 10000)
			{
				highestLevel = 9;
			}
			if (oldXP < 15000 && newXP >= 15000)
			{
				highestLevel = 10;
			}
			return highestLevel;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00041E0C File Offset: 0x0004000C
		public void foundMineral(int index)
		{
			if (this.mineralsFound == null)
			{
				this.mineralsFound = new SerializableDictionary<int, int>();
			}
			if (this.mineralsFound.ContainsKey(index))
			{
				SerializableDictionary<int, int> expr_29 = this.mineralsFound;
				int num = expr_29[index];
				expr_29[index] = num + 1;
			}
			else
			{
				this.mineralsFound.Add(index, 1);
			}
			if (!this.hasOrWillReceiveMail("artifactFound"))
			{
				this.mailReceived.Add("artifactFound");
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00041E80 File Offset: 0x00040080
		public void increaseBackpackSize(int howMuch)
		{
			this.MaxItems += howMuch;
			for (int i = 0; i < howMuch; i++)
			{
				this.items.Add(null);
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00041EB4 File Offset: 0x000400B4
		public void consumeObject(int index, int quantity)
		{
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				if (this.items[i] != null && this.items[i] is Object && ((Object)this.items[i]).parentSheetIndex == index)
				{
					int toRemove = quantity;
					quantity -= this.items[i].Stack;
					this.items[i].Stack -= toRemove;
					if (this.items[i].Stack <= 0)
					{
						this.items[i] = null;
					}
					if (quantity <= 0)
					{
						return;
					}
				}
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00041F74 File Offset: 0x00040174
		public bool hasItemInInventory(int itemIndex, int quantity, int minPrice = 0)
		{
			int numberFound = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && ((this.items[i] is Object && !(this.items[i] is Furniture) && !(this.items[i] as Object).bigCraftable && ((Object)this.items[i]).ParentSheetIndex == itemIndex) || (this.items[i] is Object && ((Object)this.items[i]).Category == itemIndex)))
				{
					numberFound += this.items[i].Stack;
				}
			}
			return numberFound >= quantity;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00042050 File Offset: 0x00040250
		public bool hasItemInList(List<Item> list, int itemIndex, int quantity, int minPrice = 0)
		{
			int numberFound = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && ((list[i] is Object && !(list[i] is Furniture) && !(list[i] as Object).bigCraftable && ((Object)list[i]).ParentSheetIndex == itemIndex) || (list[i] is Object && ((Object)list[i]).Category == itemIndex)))
				{
					numberFound += list[i].Stack;
				}
			}
			return numberFound >= quantity;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000420FA File Offset: 0x000402FA
		public void addItemByMenuIfNecessaryElseHoldUp(Item item, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
		{
			this.mostRecentlyGrabbedItem = item;
			this.addItemsByMenuIfNecessary(new List<Item>
			{
				item
			}, itemSelectedCallback);
			if (Game1.activeClickableMenu == null && this.mostRecentlyGrabbedItem.parentSheetIndex != 434)
			{
				this.holdUpItemThenMessage(item, true);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00042137 File Offset: 0x00040337
		public void addItemByMenuIfNecessary(Item item, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
		{
			this.addItemsByMenuIfNecessary(new List<Item>
			{
				item
			}, itemSelectedCallback);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0004214C File Offset: 0x0004034C
		public void addItemsByMenuIfNecessary(List<Item> itemsToAdd, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
		{
			if (itemsToAdd == null)
			{
				return;
			}
			if (itemsToAdd.Count > 0 && itemsToAdd[0] is Object && (itemsToAdd[0] as Object).parentSheetIndex == 434)
			{
				Game1.playerEatObject(itemsToAdd[0] as Object, true);
				if (Game1.activeClickableMenu != null)
				{
					Game1.activeClickableMenu.exitThisMenu(false);
				}
				return;
			}
			for (int i = itemsToAdd.Count - 1; i >= 0; i--)
			{
				if (this.addItemToInventoryBool(itemsToAdd[i], false))
				{
					if (itemSelectedCallback != null)
					{
						itemSelectedCallback(itemsToAdd[i], this);
					}
					itemsToAdd.Remove(itemsToAdd[i]);
				}
			}
			if (itemsToAdd.Count > 0)
			{
				Game1.activeClickableMenu = new ItemGrabMenu(itemsToAdd);
				(Game1.activeClickableMenu as ItemGrabMenu).inventory.showGrayedOutSlots = true;
				(Game1.activeClickableMenu as ItemGrabMenu).inventory.onAddItem = itemSelectedCallback;
				(Game1.activeClickableMenu as ItemGrabMenu).source = 2;
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00042244 File Offset: 0x00040444
		public void showCarrying()
		{
			if (Game1.eventUp || this.isRidingHorse())
			{
				return;
			}
			if (this.ActiveObject != null && (this.ActiveObject is Furniture || this.ActiveObject is Wallpaper))
			{
				return;
			}
			if (!this.FarmerSprite.pauseForSingleAnimation && !this.isMoving())
			{
				int oldIndex = this.FarmerSprite.indexInCurrentAnimation;
				float oldInterval = this.FarmerSprite.timer;
				switch (this.facingDirection)
				{
				case 0:
					this.FarmerSprite.setCurrentFrame(this.running ? 144 : 112);
					break;
				case 1:
					this.FarmerSprite.setCurrentFrame(this.running ? 136 : 104);
					break;
				case 2:
					this.FarmerSprite.setCurrentFrame(this.running ? 128 : 96);
					break;
				case 3:
					this.FarmerSprite.setCurrentFrame(this.running ? 152 : 120);
					break;
				}
				this.FarmerSprite.CurrentFrame = this.FarmerSprite.CurrentAnimation[oldIndex].frame;
				this.FarmerSprite.indexInCurrentAnimation = oldIndex;
				this.FarmerSprite.currentAnimationIndex = oldIndex;
				this.FarmerSprite.timer = oldInterval;
				if (this.IsMainPlayer && this.ActiveObject != null)
				{
					MultiplayerUtility.sendSwitchHeldItemMessage(this.ActiveObject.ParentSheetIndex, this.ActiveObject.bigCraftable ? 1 : 0, this.uniqueMultiplayerID);
				}
			}
			if (this.ActiveObject != null)
			{
				this.mostRecentlyGrabbedItem = this.ActiveObject;
			}
			if (this.mostRecentlyGrabbedItem != null && this.mostRecentlyGrabbedItem is Object && (this.mostRecentlyGrabbedItem as Object).ParentSheetIndex == 434)
			{
				Game1.eatHeldObject();
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00042410 File Offset: 0x00040610
		public void showNotCarrying()
		{
			if (!this.FarmerSprite.pauseForSingleAnimation && !this.isMoving())
			{
				int oldIndex = this.FarmerSprite.indexInCurrentAnimation;
				float oldInterval = this.FarmerSprite.timer;
				switch (this.facingDirection)
				{
				case 0:
					this.FarmerSprite.setCurrentFrame(this.running ? 48 : 16);
					break;
				case 1:
					this.FarmerSprite.setCurrentFrame(this.running ? 40 : 8);
					break;
				case 2:
					this.FarmerSprite.setCurrentFrame(this.running ? 32 : 0);
					break;
				case 3:
					this.FarmerSprite.setCurrentFrame(this.running ? 56 : 24);
					break;
				}
				this.FarmerSprite.CurrentFrame = this.FarmerSprite.CurrentAnimation[Math.Min(oldIndex, this.FarmerSprite.CurrentAnimation.Count - 1)].frame;
				this.FarmerSprite.indexInCurrentAnimation = oldIndex;
				this.FarmerSprite.currentAnimationIndex = oldIndex;
				this.FarmerSprite.timer = oldInterval;
				if (this.IsMainPlayer)
				{
					MultiplayerUtility.sendSwitchHeldItemMessage(-1, 0, this.uniqueMultiplayerID);
				}
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00042548 File Offset: 0x00040748
		public bool isThereALostItemQuestThatTakesThisItem(int index)
		{
			foreach (Quest q in Game1.player.questLog)
			{
				if (q is LostItemQuest && (q as LostItemQuest).itemIndex == index)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000425B8 File Offset: 0x000407B8
		public bool hasDailyQuest()
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].dailyQuest)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000425F4 File Offset: 0x000407F4
		public void dayupdate()
		{
			this.attack = 0;
			this.addedSpeed = 0;
			this.dancePartner = null;
			this.festivalScore = 0;
			this.forceTimePass = false;
			if (this.daysLeftForToolUpgrade > 0)
			{
				this.daysLeftForToolUpgrade--;
			}
			if (this.daysUntilHouseUpgrade > 0)
			{
				this.daysUntilHouseUpgrade--;
				if (this.daysUntilHouseUpgrade <= 0)
				{
					this.daysUntilHouseUpgrade = -1;
					this.houseUpgradeLevel++;
					Utility.getHomeOfFarmer(this).moveObjectsForHouseUpgrade(this.houseUpgradeLevel);
					Utility.getHomeOfFarmer(this).upgradeLevel++;
					if (this.houseUpgradeLevel == 1)
					{
						this.position = new Vector2(20f, 4f) * (float)Game1.tileSize;
					}
					if (this.houseUpgradeLevel == 2)
					{
						this.position = new Vector2(29f, 13f) * (float)Game1.tileSize;
					}
					Game1.stats.checkForBuildingUpgradeAchievements();
				}
			}
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].dailyQuest)
				{
					this.questLog[i].daysLeft--;
					if (this.questLog[i].daysLeft <= 0 && !this.questLog[i].completed)
					{
						this.questLog.RemoveAt(i);
					}
				}
			}
			using (List<Buff>.Enumerator enumerator = this.buffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.removeBuff();
				}
			}
			Game1.buffsDisplay.clearAllBuffs();
			base.stopGlowing();
			this.buffs.Clear();
			this.addedCombatLevel = 0;
			this.addedFarmingLevel = 0;
			this.addedFishingLevel = 0;
			this.addedForagingLevel = 0;
			this.addedLuckLevel = 0;
			this.addedMiningLevel = 0;
			this.addedSpeed = 0;
			this.bobber = "";
			float oldStamina = this.Stamina;
			this.Stamina = (float)this.MaxStamina;
			if (this.exhausted)
			{
				this.exhausted = false;
				this.Stamina = (float)(this.MaxStamina / 2 + 1);
			}
			if (Game1.timeOfDay > 2400)
			{
				this.Stamina -= (1f - (float)(2600 - Math.Min(2600, Game1.timeOfDay)) / 200f) * (float)(this.MaxStamina / 2);
				if (Game1.timeOfDay > 2700)
				{
					this.Stamina /= 2f;
				}
			}
			if (Game1.timeOfDay < 2700 && oldStamina > this.Stamina)
			{
				this.Stamina = oldStamina;
			}
			this.health = this.maxHealth;
			List<string> toRemove = new List<string>();
			foreach (string s in this.activeDialogueEvents.Keys.ToList<string>())
			{
				SerializableDictionary<string, int> arg_2E7_0 = this.activeDialogueEvents;
				string key = s;
				int num = arg_2E7_0[key];
				arg_2E7_0[key] = num - 1;
				if (this.activeDialogueEvents[s] < 0)
				{
					toRemove.Add(s);
				}
			}
			foreach (string s2 in toRemove)
			{
				this.activeDialogueEvents.Remove(s2);
			}
			if (this.isMarried())
			{
				this.daysMarried++;
			}
			if (this.isMarried() && this.divorceTonight)
			{
				NPC currentSpouse = this.getSpouse();
				if (currentSpouse != null)
				{
					this.spouse = null;
					string rawDefaultLocationString = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[currentSpouse.name].Split(new char[]
					{
						'/'
					})[10];
					currentSpouse.defaultMap = rawDefaultLocationString.Split(new char[]
					{
						' '
					})[0];
					currentSpouse.DefaultPosition = new Vector2((float)Convert.ToInt32(rawDefaultLocationString.Split(new char[]
					{
						' '
					})[1]), (float)Convert.ToInt32(rawDefaultLocationString.Split(new char[]
					{
						' '
					})[2])) * (float)Game1.tileSize;
					currentSpouse.datingFarmer = false;
					currentSpouse.divorcedFromFarmer = true;
					currentSpouse.setMarried(false);
					for (int j = this.specialItems.Count - 1; j >= 0; j--)
					{
						if (this.specialItems[j] == 460)
						{
							this.specialItems.RemoveAt(j);
						}
					}
					if (this.friendships.ContainsKey(currentSpouse.name))
					{
						this.friendships[currentSpouse.name][0] = 0;
					}
					Game1.warpCharacter(currentSpouse, currentSpouse.defaultMap, currentSpouse.DefaultPosition, true, false);
					Utility.getHomeOfFarmer(this).showSpouseRoom();
					Game1.getFarm().addSpouseOutdoorArea("");
				}
				this.divorceTonight = false;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00042B20 File Offset: 0x00040D20
		public static void showReceiveNewItemMessage(Farmer who)
		{
			string possibleSpecialMessage = who.mostRecentlyGrabbedItem.checkForSpecialItemHoldUpMeessage();
			if (possibleSpecialMessage != null)
			{
				Game1.drawObjectDialogue(possibleSpecialMessage);
			}
			else if (who.mostRecentlyGrabbedItem.parentSheetIndex == 472 && who.mostRecentlyGrabbedItem.Stack == 15)
			{
				Game1.drawObjectDialogue("You received 15 Parsnip Seeds!^^'Here's a little something to get you started.^-Mayor Lewis'");
			}
			else
			{
				Game1.drawObjectDialogue("You received " + ((who.mostRecentlyGrabbedItem.Stack > 1) ? string.Concat(who.mostRecentlyGrabbedItem.Stack) : Game1.getProperArticleForWord(who.mostRecentlyGrabbedItem.Name)) + " " + who.mostRecentlyGrabbedItem.Name + ((who.mostRecentlyGrabbedItem.Stack > 1 && who.mostRecentlyGrabbedItem.Name.Last<char>() != 's') ? "s" : "") + "!");
			}
			who.completelyStopAnimatingOrDoingAction();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00042C0C File Offset: 0x00040E0C
		public static void showEatingItem(Farmer who)
		{
			TemporaryAnimatedSprite tempSprite = null;
			if (who.itemToEat == null)
			{
				return;
			}
			switch (who.FarmerSprite.indexInCurrentAnimation)
			{
			case 1:
				if (who.itemToEat != null && who.itemToEat is Object && (who.itemToEat as Object).ParentSheetIndex == 434)
				{
					tempSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 62.75f, 8, 2, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
				}
				else
				{
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16), 254f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
				}
				break;
			case 2:
				if (who.itemToEat != null && who.itemToEat is Object && (who.itemToEat as Object).ParentSheetIndex == 434)
				{
					tempSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 81.25f, 8, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + 4 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
					{
						motion = new Vector2(0.8f, -11f),
						acceleration = new Vector2(0f, 0.5f)
					};
				}
				else
				{
					Game1.playSound("dwop");
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16), 650f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 3), (float)(-(float)Game1.tileSize * 2 + 4 + Game1.tileSize / 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
					{
						motion = new Vector2(0.8f, -11f),
						acceleration = new Vector2(0f, 0.5f)
					};
				}
				break;
			case 3:
				who.yJumpVelocity = 6f;
				who.yJumpOffset = 1;
				break;
			case 4:
				Game1.playSound("eat");
				for (int i = 0; i < 8; i++)
				{
					Microsoft.Xna.Framework.Rectangle r = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16);
					r.X += 8;
					r.Y += 8;
					r.Width = Game1.pixelZoom;
					r.Height = Game1.pixelZoom;
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, r, 400f, 1, 0, who.position + new Vector2(24f, -48f), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						motion = new Vector2((float)Game1.random.Next(-30, 31) / 10f, (float)Game1.random.Next(-6, -3)),
						acceleration = new Vector2(0f, 0.5f)
					};
					who.currentLocation.temporarySprites.Add(tempSprite);
				}
				return;
			default:
				who.freezePause = 0;
				break;
			}
			if (tempSprite != null)
			{
				who.currentLocation.temporarySprites.Add(tempSprite);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00002834 File Offset: 0x00000A34
		public static void eatItem(Farmer who)
		{
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000430A4 File Offset: 0x000412A4
		public bool hasBuff(int whichBuff)
		{
			using (List<Buff>.Enumerator enumerator = this.buffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.which == whichBuff)
					{
						bool result = true;
						return result;
					}
				}
			}
			using (List<Buff>.Enumerator enumerator = Game1.buffsDisplay.otherBuffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.which == whichBuff)
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00043148 File Offset: 0x00041348
		public bool hasOrWillReceiveMail(string id)
		{
			return this.mailReceived.Contains(id) || this.mailForTomorrow.Contains(id) || Game1.mailbox.Contains(id) || this.mailForTomorrow.Contains(id + "%&NL&%");
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00043198 File Offset: 0x00041398
		public static void showHoldingItem(Farmer who)
		{
			if (who.mostRecentlyGrabbedItem is SpecialItem)
			{
				TemporaryAnimatedSprite t = (who.mostRecentlyGrabbedItem as SpecialItem).getTemporarySpriteForHoldingUp(who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)));
				t.motion = new Vector2(0f, -0.1f);
				t.scale = (float)Game1.pixelZoom;
				t.interval = 2500f;
				t.totalNumberOfLoops = 0;
				t.animationLength = 1;
				Game1.currentLocation.temporarySprites.Add(t);
			}
			else if (who.mostRecentlyGrabbedItem is Slingshot)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Tool.weaponsTexture, Game1.getSquareSourceRectForNonStandardTileSheet(Tool.weaponsTexture, 16, 16, (who.mostRecentlyGrabbedItem as Slingshot).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is MeleeWeapon)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Tool.weaponsTexture, Game1.getSquareSourceRectForNonStandardTileSheet(Tool.weaponsTexture, 16, 16, (who.mostRecentlyGrabbedItem as MeleeWeapon).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is Boots)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSquareSourceRectForNonStandardTileSheet(Game1.objectSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, (who.mostRecentlyGrabbedItem as Boots).indexInTileSheet), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is Tool)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.toolSpriteSheet, Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, (who.mostRecentlyGrabbedItem as Tool).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
				{
					motion = new Vector2(0f, -0.1f)
				});
			}
			else if (who.mostRecentlyGrabbedItem is Furniture)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Furniture.furnitureTexture, (who.mostRecentlyGrabbedItem as Furniture).sourceRect, 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 + 4)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
			}
			else if (who.mostRecentlyGrabbedItem is Object && !(who.mostRecentlyGrabbedItem as Object).bigCraftable)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, who.mostRecentlyGrabbedItem.parentSheetIndex, 16, 16), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				if (who.mostRecentlyGrabbedItem.parentSheetIndex == 434)
				{
					Game1.eatHeldObject();
				}
			}
			else if (who.mostRecentlyGrabbedItem is Object)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.bigCraftableSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, who.mostRecentlyGrabbedItem.parentSheetIndex, 16, 32), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 + 4)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
			}
			if (who.mostRecentlyGrabbedItem == null)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(420, 489, 25, 18), 2500f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 - Game1.pixelZoom * 6)), false, false)
				{
					motion = new Vector2(0f, -0.1f),
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				});
				return;
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(10, who.position + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 3 / 2)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -0.1f)
			});
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00043800 File Offset: 0x00041A00
		public void holdUpItemThenMessage(Item item, bool showMessage = true)
		{
			this.completelyStopAnimatingOrDoingAction();
			if (showMessage)
			{
				DelayedAction.playSoundAfterDelay("getNewSpecialItem", 750);
			}
			Game1.player.faceDirection(2);
			this.freezePause = 4000;
			this.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(57, 0),
				new FarmerSprite.AnimationFrame(57, 2500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showHoldingItem), false),
				showMessage ? new FarmerSprite.AnimationFrame((int)((short)this.FarmerSprite.currentFrame), 500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showReceiveNewItemMessage), true) : new FarmerSprite.AnimationFrame((int)((short)this.FarmerSprite.currentFrame), 500, false, false, null, false)
			});
			this.mostRecentlyGrabbedItem = item;
			this.canMove = false;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000438D8 File Offset: 0x00041AD8
		private void checkForLevelUp()
		{
			int xpAtLevel = 600;
			int lastLevel = 0;
			int level = this.Level;
			for (int i = 0; i <= 35; i++)
			{
				if (level <= i && (ulong)this.totalMoneyEarned >= (ulong)((long)xpAtLevel))
				{
					this.NewSkillPointsToSpend += 2;
					Game1.addHUDMessage(new HUDMessage("Level Up", Color.Violet, 3500f));
				}
				else if ((ulong)this.totalMoneyEarned < (ulong)((long)xpAtLevel))
				{
					return;
				}
				int arg_6A_0 = xpAtLevel;
				xpAtLevel += (int)((double)(xpAtLevel - lastLevel) * 1.2);
				lastLevel = arg_6A_0;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0004395C File Offset: 0x00041B5C
		public void clearBackpack()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i] = null;
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0004398C File Offset: 0x00041B8C
		public int numberOfItemsInInventory()
		{
			int num = 0;
			foreach (Item o in this.items)
			{
				if (o != null && o is Object)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000439EC File Offset: 0x00041BEC
		public void resetFriendshipsForNewDay()
		{
			string[] array = this.friendships.Keys.ToArray<string>();
			for (int j = 0; j < array.Length; j++)
			{
				string name = array[j];
				this.friendships[name][3] = 0;
				bool single = false;
				NPC i = Game1.getCharacterFromName(name, false);
				if (i != null && i.datable && !i.datingFarmer && !i.isMarried())
				{
					single = true;
				}
				if (this.spouse != null && name.Equals(this.spouse) && !this.hasPlayerTalkedToNPC(name))
				{
					this.friendships[name][0] = Math.Max(this.friendships[name][0] - 20, 0);
				}
				else if (i != null && i.datingFarmer && !this.hasPlayerTalkedToNPC(name) && this.friendships[name][0] < 2500)
				{
					this.friendships[name][0] = Math.Max(this.friendships[name][0] - 8, 0);
				}
				if (this.hasPlayerTalkedToNPC(name))
				{
					this.friendships[name][2] = 0;
				}
				else if ((!single && this.friendships[name][0] < 2500) || (single && this.friendships[name][0] < 2000))
				{
					this.friendships[name][0] = Math.Max(this.friendships[name][0] - 2, 0);
				}
				if (Game1.dayOfMonth % 7 == 0)
				{
					if (this.friendships[name][1] == 2)
					{
						this.friendships[name][0] = Math.Min(this.friendships[name][0] + 10, 2749);
					}
					this.friendships[name][1] = 0;
				}
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00043BB4 File Offset: 0x00041DB4
		public bool hasPlayerTalkedToNPC(string name)
		{
			if (!this.friendships.ContainsKey(name) && Game1.NPCGiftTastes.ContainsKey(name))
			{
				this.friendships.Add(name, new int[4]);
			}
			return this.friendships.ContainsKey(name) && this.friendships[name][2] == 1;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00043C10 File Offset: 0x00041E10
		public void fuelLantern(int units)
		{
			Tool lantern = this.getToolFromName("Lantern");
			if (lantern != null)
			{
				((Lantern)lantern).fuelLeft = Math.Min(100, ((Lantern)lantern).fuelLeft + units);
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00043C4C File Offset: 0x00041E4C
		public bool tryToCraftItem(List<int[]> ingredients, double successRate, int itemToCraft, bool bigCraftable, string craftingOrCooking)
		{
			List<int[]> locationOfIngredients = new List<int[]>();
			foreach (int[] ingredient in ingredients)
			{
				if (ingredient[0] <= -100)
				{
					int farmerStock = 0;
					switch (ingredient[0])
					{
					case -106:
						farmerStock = this.IridiumPieces;
						break;
					case -105:
						farmerStock = this.GoldPieces;
						break;
					case -104:
						farmerStock = this.CoalPieces;
						break;
					case -103:
						farmerStock = this.IronPieces;
						break;
					case -102:
						farmerStock = this.CopperPieces;
						break;
					case -101:
						farmerStock = this.stonePieces;
						break;
					case -100:
						farmerStock = this.WoodPieces;
						break;
					}
					if (farmerStock < ingredient[1])
					{
						bool result = false;
						return result;
					}
					locationOfIngredients.Add(ingredient);
				}
				else
				{
					for (int i = 0; i < ingredient[1]; i++)
					{
						int[] cheapestIndex = new int[]
						{
							99999,
							-1
						};
						int j = 0;
						while (j < this.items.Count)
						{
							if (this.items[j] != null && this.items[j] is Object && ((Object)this.items[j]).ParentSheetIndex == ingredient[0] && !Farmer.containsIndex(locationOfIngredients, j))
							{
								locationOfIngredients.Add(new int[]
								{
									j,
									1
								});
								break;
							}
							if (this.items[j] != null && this.items[j] is Object && ((Object)this.items[j]).Category == ingredient[0] && !Farmer.containsIndex(locationOfIngredients, j) && ((Object)this.items[j]).Price < cheapestIndex[0])
							{
								cheapestIndex[0] = ((Object)this.items[j]).Price;
								cheapestIndex[1] = j;
							}
							if (j == this.items.Count - 1)
							{
								if (cheapestIndex[1] != -1)
								{
									locationOfIngredients.Add(new int[]
									{
										cheapestIndex[1],
										ingredient[1]
									});
									break;
								}
								bool result = false;
								return result;
							}
							else
							{
								j++;
							}
						}
					}
				}
			}
			string fishType = "";
			if (itemToCraft == 291)
			{
				fishType = ((Object)this.items[locationOfIngredients[0][0]]).Name;
			}
			else if (itemToCraft == 216 && Game1.random.NextDouble() < 0.5)
			{
				itemToCraft++;
			}
			Game1.drawObjectDialogue("You begin " + craftingOrCooking + "...");
			this.isCrafting = true;
			Game1.playSound("crafting");
			int locationToPlace = -1;
			string message = "...And you succeed!";
			if (bigCraftable)
			{
				Game1.player.ActiveObject = new Object(Vector2.Zero, itemToCraft, false);
				Game1.player.showCarrying();
			}
			else if (itemToCraft < 0)
			{
				if (!true)
				{
					message = "...but you don't have any space in your toolbox or inventory.";
				}
			}
			else
			{
				locationToPlace = locationOfIngredients[0][0];
				if (locationOfIngredients[0][0] < 0)
				{
					for (int k = 0; k < this.items.Count; k++)
					{
						if (this.items[k] == null)
						{
							locationToPlace = k;
							break;
						}
						if (k == this.maxItems - 1)
						{
							Game1.pauseThenMessage(this.craftingTime + ingredients.Count * 500, "...but you have too many things in your backpack.", true);
							return false;
						}
					}
				}
				if (!fishType.Equals(""))
				{
					this.items[locationToPlace] = new Object(Vector2.Zero, itemToCraft, fishType + " Bobber", true, true, false, false);
				}
				else
				{
					this.items[locationToPlace] = new Object(Vector2.Zero, itemToCraft, null, true, true, false, false);
				}
			}
			Game1.pauseThenMessage(this.craftingTime + ingredients.Count * 500, message, true);
			string a = craftingOrCooking.ToLower();
			if (!(a == "crafting"))
			{
				if (a == "cooking")
				{
					Stats expr_429 = Game1.stats;
					uint num = expr_429.ItemsCooked;
					expr_429.ItemsCooked = num + 1u;
				}
			}
			else
			{
				Stats expr_411 = Game1.stats;
				uint num = expr_411.ItemsCrafted;
				expr_411.ItemsCrafted = num + 1u;
			}
			foreach (int[] l in locationOfIngredients)
			{
				if (l[0] <= -100)
				{
					switch (l[0])
					{
					case -106:
						this.IridiumPieces -= l[1];
						break;
					case -105:
						this.GoldPieces -= l[1];
						break;
					case -104:
						this.CoalPieces -= l[1];
						break;
					case -103:
						this.IronPieces -= l[1];
						break;
					case -102:
						this.CopperPieces -= l[1];
						break;
					case -101:
						this.stonePieces -= l[1];
						break;
					case -100:
						this.WoodPieces -= l[1];
						break;
					}
				}
				else if (l[0] != locationToPlace)
				{
					this.items[l[0]] = null;
				}
			}
			return true;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000441D8 File Offset: 0x000423D8
		private static bool containsIndex(List<int[]> locationOfIngredients, int index)
		{
			for (int i = 0; i < locationOfIngredients.Count; i++)
			{
				if (locationOfIngredients[i][0] == index)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00044208 File Offset: 0x00042408
		public override bool collideWith(Object o)
		{
			base.collideWith(o);
			if (this.isRidingHorse() && o is Fence)
			{
				this.mount.squeezeForGate();
				int facingDirection = this.facingDirection;
				if (facingDirection != 1)
				{
					if (facingDirection == 3 && o.tileLocation.X > (float)base.getTileX())
					{
						return false;
					}
				}
				else if (o.tileLocation.X < (float)base.getTileX())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00044274 File Offset: 0x00042474
		public void changeIntoSwimsuit()
		{
			this.bathingClothes = true;
			this.Halt();
			this.setRunning(false, false);
			this.canOnlyWalk = true;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00044292 File Offset: 0x00042492
		public void changeOutOfSwimSuit()
		{
			this.bathingClothes = false;
			this.canOnlyWalk = false;
			this.Halt();
			this.FarmerSprite.StopAnimation();
			if (Game1.options.autoRun)
			{
				this.setRunning(true, false);
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000442C8 File Offset: 0x000424C8
		public bool ownsFurniture(string name)
		{
			using (List<string>.Enumerator enumerator = this.furnitureOwned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Equals(name))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00044324 File Offset: 0x00042524
		public void showFrame(int frame, bool flip = false)
		{
			List<FarmerSprite.AnimationFrame> animationFrames = new List<FarmerSprite.AnimationFrame>();
			animationFrames.Add(new FarmerSprite.AnimationFrame(Convert.ToInt32(frame), 100, false, flip, null, false));
			this.FarmerSprite.setCurrentAnimation(animationFrames.ToArray());
			this.FarmerSprite.loopThisAnimation = true;
			this.FarmerSprite.PauseForSingleAnimation = true;
			this.sprite.CurrentFrame = Convert.ToInt32(frame);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00044388 File Offset: 0x00042588
		public void stopShowingFrame()
		{
			this.FarmerSprite.loopThisAnimation = false;
			this.FarmerSprite.PauseForSingleAnimation = false;
			this.completelyStopAnimatingOrDoingAction();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000443A8 File Offset: 0x000425A8
		public Item addItemToInventory(Item item)
		{
			if (item == null)
			{
				return null;
			}
			if (item is SpecialItem)
			{
				return item;
			}
			for (int i = 0; i < this.maxItems; i++)
			{
				if (i < this.items.Count && this.items[i] != null && this.items[i].maximumStackSize() != -1 && this.items[i].getStack() < this.items[i].maximumStackSize() && this.items[i].Name.Equals(item.Name) && (!(item is Object) || !(this.items[i] is Object) || ((item as Object).quality == (this.items[i] as Object).quality && (item as Object).parentSheetIndex == (this.items[i] as Object).parentSheetIndex)) && item.canStackWith(this.items[i]))
				{
					int stackLeft = this.items[i].addToStack(item.getStack());
					if (stackLeft <= 0)
					{
						return null;
					}
					item.Stack = stackLeft;
				}
			}
			for (int j = 0; j < this.maxItems; j++)
			{
				if (this.items.Count > j && this.items[j] == null)
				{
					this.items[j] = item;
					return null;
				}
			}
			return item;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00044534 File Offset: 0x00042734
		public bool isInventoryFull()
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && this.items[i] == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00044574 File Offset: 0x00042774
		public bool couldInventoryAcceptThisItem(Item item)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && (this.items[i] == null || (item is Object && this.items[i] is Object && this.items[i].Stack + item.Stack <= this.items[i].maximumStackSize() && (this.items[i] as Object).canStackWith(item))))
				{
					return true;
				}
			}
			if (this.isInventoryFull() && Game1.hudMessages.Count == 0)
			{
				Game1.showRedMessage("Inventory Full");
			}
			return false;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00044634 File Offset: 0x00042834
		public bool couldInventoryAcceptThisObject(int index, int stack, int quality = 0)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && (this.items[i] == null || (this.items[i] is Object && this.items[i].Stack + stack <= this.items[i].maximumStackSize() && (this.items[i] as Object).ParentSheetIndex == index && (this.items[i] as Object).quality == quality)))
				{
					return true;
				}
			}
			if (this.isInventoryFull() && Game1.hudMessages.Count == 0)
			{
				Game1.showRedMessage("Inventory Full");
			}
			return false;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00044700 File Offset: 0x00042900
		public bool hasItemOfType(string type)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (this.items.Count > i && this.items[i] is Object && (this.items[i] as Object).type.Equals(type))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00044760 File Offset: 0x00042960
		public NPC getSpouse()
		{
			if (this.isMarried())
			{
				return Game1.getCharacterFromName(this.spouse, false);
			}
			return null;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00044778 File Offset: 0x00042978
		public int freeSpotsInInventory()
		{
			int number = 0;
			for (int i = 0; i < this.maxItems; i++)
			{
				if (i < this.items.Count && this.items[i] == null)
				{
					number++;
				}
			}
			return number;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000447BC File Offset: 0x000429BC
		public Item hasItemWithNameThatContains(string name)
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (i < this.items.Count && this.items[i] != null && this.items[i].Name.Contains(name))
				{
					return this.items[i];
				}
			}
			return null;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00044820 File Offset: 0x00042A20
		public bool addItemToInventoryBool(Item item, bool makeActiveObject = false)
		{
			if (item == null)
			{
				return false;
			}
			int arg_0B_0 = item.Stack;
			Item tmp = this.IsMainPlayer ? this.addItemToInventory(item) : null;
			bool success = tmp == null || tmp.Stack != item.Stack || item is SpecialItem;
			if (item is Object)
			{
				(item as Object).reloadSprite();
			}
			if (success && this.IsMainPlayer)
			{
				if (item != null)
				{
					if (this.IsMainPlayer && !item.hasBeenInInventory)
					{
						if (item is SpecialItem)
						{
							(item as SpecialItem).actionWhenReceived(this);
							return true;
						}
						if (item is Object && (item as Object).specialItem)
						{
							if ((item as Object).bigCraftable || item is Furniture)
							{
								if (!this.specialBigCraftables.Contains((item as Object).parentSheetIndex))
								{
									this.specialBigCraftables.Add((item as Object).parentSheetIndex);
								}
							}
							else if (!this.specialItems.Contains((item as Object).parentSheetIndex))
							{
								this.specialItems.Add((item as Object).parentSheetIndex);
							}
						}
						if (item is Object && (item as Object).Category == -2 && !(item as Object).hasBeenPickedUpByFarmer)
						{
							this.foundMineral((item as Object).parentSheetIndex);
						}
						else if (!(item is Furniture) && item is Object && (item as Object).type != null && (item as Object).type.Contains("Arch") && !(item as Object).hasBeenPickedUpByFarmer)
						{
							this.foundArtifact((item as Object).parentSheetIndex, 1);
						}
						if (item.parentSheetIndex == 102)
						{
							this.foundArtifact((item as Object).parentSheetIndex, 1);
							this.removeItemFromInventory(item);
						}
					}
					if (item is Object && !item.hasBeenInInventory)
					{
						if (!(item is Furniture) && !(item as Object).bigCraftable && !(item as Object).hasBeenPickedUpByFarmer)
						{
							this.checkForQuestComplete(null, (item as Object).parentSheetIndex, (item as Object).stack, item, null, 9, -1);
						}
						(item as Object).hasBeenPickedUpByFarmer = true;
						if ((item as Object).questItem)
						{
							return true;
						}
						if (Game1.activeClickableMenu == null)
						{
							int parentSheetIndex = (item as Object).parentSheetIndex;
							if (parentSheetIndex <= 378)
							{
								if (parentSheetIndex == 102)
								{
									Stats expr_2E2 = Game1.stats;
									uint num = expr_2E2.NotesFound;
									expr_2E2.NotesFound = num + 1u;
									Game1.playSound("newRecipe");
									this.holdUpItemThenMessage(item, true);
									return true;
								}
								if (parentSheetIndex == 378)
								{
									if (!Game1.player.hasOrWillReceiveMail("copperFound"))
									{
										Game1.addMailForTomorrow("copperFound", true, false);
									}
								}
							}
							else if (parentSheetIndex != 390)
							{
								if (parentSheetIndex == 535 && !Game1.player.hasOrWillReceiveMail("geodeFound"))
								{
									this.mailReceived.Add("geodeFound");
									this.holdUpItemThenMessage(item, true);
								}
							}
							else
							{
								Stats expr_30C = Game1.stats;
								uint num = expr_30C.StoneGathered;
								expr_30C.StoneGathered = num + 1u;
								if (Game1.stats.StoneGathered >= 100u && !Game1.player.hasOrWillReceiveMail("robinWell"))
								{
									Game1.addMailForTomorrow("robinWell", false, false);
								}
							}
						}
					}
					Color fontColor = Color.WhiteSmoke;
					string name = item.Name;
					if (item is Object)
					{
						string type = (item as Object).type;
						if (!(type == "Arch"))
						{
							if (!(type == "Fish"))
							{
								if (!(type == "Mineral"))
								{
									if (!(type == "Vegetable"))
									{
										if (type == "Fruit")
										{
											fontColor = Color.Pink;
										}
									}
									else
									{
										fontColor = Color.PaleGreen;
									}
								}
								else
								{
									fontColor = Color.PaleVioletRed;
								}
							}
							else
							{
								fontColor = Color.SkyBlue;
							}
						}
						else
						{
							fontColor = Color.Tan;
							name += " Artifact";
						}
					}
					if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is ItemGrabMenu))
					{
						Game1.addHUDMessage(new HUDMessage(name, Math.Max(1, item.Stack), true, fontColor, item));
					}
					this.mostRecentlyGrabbedItem = item;
					if ((tmp != null & makeActiveObject) && item.Stack <= 1)
					{
						int newItemPosition = this.getIndexOfInventoryItem(item);
						Item i = this.items[this.currentToolIndex];
						this.items[this.currentToolIndex] = this.items[newItemPosition];
						this.items[newItemPosition] = i;
					}
				}
				if (item is Object && !item.hasBeenInInventory)
				{
					this.checkForQuestComplete(null, item.parentSheetIndex, item.Stack, item, "", 10, -1);
				}
				item.hasBeenInInventory = true;
				return success;
			}
			return false;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00044CDC File Offset: 0x00042EDC
		public int getIndexOfInventoryItem(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == item || (this.items[i] != null && item != null && item.canStackWith(this.items[i])))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00044D38 File Offset: 0x00042F38
		public void reduceActiveItemByOne()
		{
			if (this.CurrentItem != null)
			{
				Item expr_0E = this.CurrentItem;
				int stack = expr_0E.Stack;
				expr_0E.Stack = stack - 1;
				if (this.CurrentItem.Stack <= 0)
				{
					this.removeItemFromInventory(this.CurrentItem);
					this.showNotCarrying();
				}
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00044D84 File Offset: 0x00042F84
		public bool removeItemsFromInventory(int index, int stack)
		{
			if (this.hasItemInInventory(index, stack, 0))
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (this.items[i] != null && this.items[i] is Object && (this.items[i] as Object).parentSheetIndex == index)
					{
						if (this.items[i].Stack > stack)
						{
							this.items[i].Stack -= stack;
							return true;
						}
						stack -= this.items[i].Stack;
						this.items[i] = null;
					}
					if (stack <= 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00044E50 File Offset: 0x00043050
		public Item addItemToInventory(Item item, int position)
		{
			if (item != null && item is Object && (item as Object).specialItem)
			{
				if ((item as Object).bigCraftable)
				{
					if (!this.specialBigCraftables.Contains((item as Object).parentSheetIndex))
					{
						this.specialBigCraftables.Add((item as Object).parentSheetIndex);
					}
				}
				else if (!this.specialItems.Contains((item as Object).parentSheetIndex))
				{
					this.specialItems.Add((item as Object).parentSheetIndex);
				}
			}
			if (position < 0 || position >= this.items.Count)
			{
				return item;
			}
			if (this.items[position] == null)
			{
				this.items[position] = item;
				return null;
			}
			if (item == null || this.items[position].maximumStackSize() == -1 || !this.items[position].Name.Equals(item.Name) || (item is Object && this.items[position] is Object && (item as Object).quality != (this.items[position] as Object).quality))
			{
				Item arg_174_0 = this.items[position];
				this.items[position] = item;
				return arg_174_0;
			}
			int stackLeft = this.items[position].addToStack(item.getStack());
			if (stackLeft <= 0)
			{
				return null;
			}
			item.Stack = stackLeft;
			return item;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00044FD4 File Offset: 0x000431D4
		public void removeItemFromInventory(Item which)
		{
			int i = this.items.IndexOf(which);
			if (i >= 0 && i < this.items.Count)
			{
				this.items[this.items.IndexOf(which)] = null;
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00045018 File Offset: 0x00043218
		public Item removeItemFromInventory(int whichItemIndex)
		{
			if (whichItemIndex >= 0 && whichItemIndex < this.items.Count && this.items[whichItemIndex] != null)
			{
				Item arg_39_0 = this.items[whichItemIndex];
				this.items[whichItemIndex] = null;
				return arg_39_0;
			}
			return null;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00045055 File Offset: 0x00043255
		public bool isMarried()
		{
			return this.spouse != null && !this.spouse.Contains("engaged");
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00045074 File Offset: 0x00043274
		public void removeFirstOfThisItemFromInventory(int parentSheetIndexOfItem)
		{
			if (this.ActiveObject != null && this.ActiveObject.ParentSheetIndex == parentSheetIndexOfItem)
			{
				Object expr_1C = this.ActiveObject;
				int stack = expr_1C.Stack;
				expr_1C.Stack = stack - 1;
				if (this.ActiveObject.Stack <= 0)
				{
					this.ActiveObject = null;
					this.showNotCarrying();
					return;
				}
			}
			else
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (this.items[i] != null && this.items[i] is Object && ((Object)this.items[i]).ParentSheetIndex == parentSheetIndexOfItem)
					{
						Item expr_94 = this.items[i];
						int stack = expr_94.Stack;
						expr_94.Stack = stack - 1;
						if (this.items[i].Stack <= 0)
						{
							this.items[i] = null;
						}
						return;
					}
				}
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0004515C File Offset: 0x0004335C
		public bool hasCoopDweller(string type)
		{
			using (List<CoopDweller>.Enumerator enumerator = this.coopDwellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.type.Equals(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000451BC File Offset: 0x000433BC
		public void changeShirt(int whichShirt)
		{
			if (whichShirt < 0)
			{
				whichShirt = FarmerRenderer.shirtsTexture.Height / 32 * (FarmerRenderer.shirtsTexture.Width / 8) - 1;
			}
			else if (whichShirt > FarmerRenderer.shirtsTexture.Height / 32 * (FarmerRenderer.shirtsTexture.Width / 8) - 1)
			{
				whichShirt = 0;
			}
			this.shirt = whichShirt;
			this.FarmerRenderer.changeShirt(whichShirt);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00045222 File Offset: 0x00043422
		public void changeHairStyle(int whichHair)
		{
			if (whichHair < 0)
			{
				whichHair = FarmerRenderer.hairStylesTexture.Height / 96 * 8 - 1;
			}
			else if (whichHair > FarmerRenderer.hairStylesTexture.Height / 96 * 8 - 1)
			{
				whichHair = 0;
			}
			this.hair = whichHair;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0004525B File Offset: 0x0004345B
		public void changeShoeColor(int which)
		{
			this.FarmerRenderer.recolorShoes(which);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00045269 File Offset: 0x00043469
		public void changeHairColor(Color c)
		{
			this.hairstyleColor = c;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00045272 File Offset: 0x00043472
		public void changePants(Color color)
		{
			this.pantsColor = color;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0004527B File Offset: 0x0004347B
		public void changeHat(int newHat)
		{
			if (newHat < 0)
			{
				this.hat = null;
				return;
			}
			this.hat = new Hat(newHat);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00045295 File Offset: 0x00043495
		public void changeAccessory(int which)
		{
			if (which < -1)
			{
				which = 18;
			}
			if (which >= -1)
			{
				if (which >= 19)
				{
					which = -1;
				}
				this.accessory = which;
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000452B2 File Offset: 0x000434B2
		public void changeSkinColor(int which)
		{
			this.skin = this.FarmerRenderer.recolorSkin(which);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000452C6 File Offset: 0x000434C6
		public bool hasDarkSkin()
		{
			return (this.skin >= 4 && this.skin <= 8) || this.skin == 14;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000452E7 File Offset: 0x000434E7
		public void changeEyeColor(Color c)
		{
			this.newEyeColor = c;
			this.FarmerRenderer.recolorEyes(c);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000452FC File Offset: 0x000434FC
		public int getHair()
		{
			if (this.hat == null || this.hat.skipHairDraw || this.bathingClothes)
			{
				return this.hair;
			}
			switch (this.hair)
			{
			case 1:
			case 5:
			case 6:
			case 9:
			case 11:
				return this.hair;
			case 3:
				return 11;
			case 17:
			case 20:
			case 23:
			case 24:
			case 25:
			case 27:
			case 28:
			case 29:
			case 30:
				return this.hair;
			case 18:
			case 19:
			case 21:
			case 31:
				return 23;
			}
			if (this.hair >= 16)
			{
				return 30;
			}
			return 7;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000453E8 File Offset: 0x000435E8
		public void changeGender(bool male)
		{
			if (male)
			{
				this.isMale = true;
				this.FarmerRenderer.baseTexture = this.getTexture();
				this.FarmerRenderer.heightOffset = 0;
			}
			else
			{
				this.isMale = false;
				this.FarmerRenderer.heightOffset = 4;
				this.FarmerRenderer.baseTexture = this.getTexture();
			}
			this.changeShirt(this.shirt);
			this.changeEyeColor(this.newEyeColor);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0004545C File Offset: 0x0004365C
		public bool hasBarnDweller(string type)
		{
			using (List<BarnDweller>.Enumerator enumerator = this.barnDwellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.type.Equals(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000454BC File Offset: 0x000436BC
		public void changeFriendship(int amount, NPC n)
		{
			if (amount > 0 && n.name.Equals("Dwarf") && !this.canUnderstandDwarves)
			{
				return;
			}
			if (this.friendships.ContainsKey(n.name))
			{
				if (!n.datable || n.datingFarmer || (this.spouse != null && this.spouse.Equals(n.name)) || this.friendships[n.name][0] < 2000)
				{
					this.friendships[n.name][0] = Math.Max(0, Math.Min(this.friendships[n.name][0] + amount, ((this.spouse != null && n.name.Equals(this.spouse)) ? 14 : 11) * 250 - 1));
					if (n.datable && !n.datingFarmer && (this.spouse == null || !this.spouse.Equals(n.name)))
					{
						this.friendships[n.name][0] = Math.Min(2498, this.friendships[n.name][0]);
					}
				}
				if (n.datable && this.friendships[n.name][0] >= 2000 && !this.hasOrWillReceiveMail("Bouquet"))
				{
					Game1.addMailForTomorrow("Bouquet", false, false);
				}
				if (n.datable && this.friendships[n.name][0] >= 2500 && !this.hasOrWillReceiveMail("SeaAmulet"))
				{
					Game1.addMailForTomorrow("SeaAmulet", false, false);
					return;
				}
			}
			else
			{
				Game1.debugOutput = "Tried to change friendship for a friend that wasn't there.";
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0004567E File Offset: 0x0004387E
		public bool knowsRecipe(string name)
		{
			return this.craftingRecipes.Keys.Contains(name) || this.cookingRecipes.Keys.Contains(name);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000456A8 File Offset: 0x000438A8
		public Vector2 getUniformPositionAwayFromBox(int direction, int distance)
		{
			switch (this.facingDirection)
			{
			case 0:
				return new Vector2((float)this.GetBoundingBox().Center.X, (float)(this.GetBoundingBox().Y - distance));
			case 1:
				return new Vector2((float)(this.GetBoundingBox().Right + distance), (float)this.GetBoundingBox().Center.Y);
			case 2:
				return new Vector2((float)this.GetBoundingBox().Center.X, (float)(this.GetBoundingBox().Bottom + distance));
			case 3:
				return new Vector2((float)(this.GetBoundingBox().X - distance), (float)this.GetBoundingBox().Center.Y);
			default:
				return Vector2.Zero;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00045782 File Offset: 0x00043982
		public bool hasTalkedToFriendToday(string npcName)
		{
			return this.friendships.ContainsKey(npcName) && this.friendships[npcName][2] == 1;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000457A8 File Offset: 0x000439A8
		public void talkToFriend(NPC n, int friendshipPointChange = 20)
		{
			if (this.friendships.ContainsKey(n.name) && this.friendships[n.name][2] == 0)
			{
				this.changeFriendship(friendshipPointChange, n);
				this.friendships[n.name][2] = 1;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000457FC File Offset: 0x000439FC
		public void moveRaft(GameLocation currentLocation, GameTime time)
		{
			float raftInertia = 0.2f;
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton))
			{
				this.yVelocity = Math.Max(this.yVelocity - raftInertia, -3f + Math.Abs(this.xVelocity) / 2f);
				this.faceDirection(0);
			}
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton))
			{
				this.xVelocity = Math.Min(this.xVelocity + raftInertia, 3f - Math.Abs(this.yVelocity) / 2f);
				this.faceDirection(1);
			}
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton))
			{
				this.yVelocity = Math.Min(this.yVelocity + raftInertia, 3f - Math.Abs(this.xVelocity) / 2f);
				this.faceDirection(2);
			}
			if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton))
			{
				this.xVelocity = Math.Max(this.xVelocity - raftInertia, -3f + Math.Abs(this.yVelocity) / 2f);
				this.faceDirection(3);
			}
			Microsoft.Xna.Framework.Rectangle collidingBox = new Microsoft.Xna.Framework.Rectangle((int)this.position.X, (int)(this.position.Y + (float)Game1.tileSize + (float)(Game1.tileSize / 4)), Game1.tileSize, Game1.tileSize);
			collidingBox.X += (int)Math.Ceiling((double)this.xVelocity);
			if (!currentLocation.isCollidingPosition(collidingBox, Game1.viewport, true))
			{
				this.position.X = this.position.X + this.xVelocity;
			}
			collidingBox.X -= (int)Math.Ceiling((double)this.xVelocity);
			collidingBox.Y += (int)Math.Floor((double)this.yVelocity);
			if (!currentLocation.isCollidingPosition(collidingBox, Game1.viewport, true))
			{
				this.position.Y = this.position.Y + this.yVelocity;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				this.raftPuddleCounter -= time.ElapsedGameTime.Milliseconds;
				if (this.raftPuddleCounter <= 0)
				{
					this.raftPuddleCounter = 250;
					currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2((float)collidingBox.X, (float)(collidingBox.Y - Game1.tileSize)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
					if (Game1.random.NextDouble() < 0.6)
					{
						Game1.playSound("wateringCan");
					}
					if (Game1.random.NextDouble() < 0.6)
					{
						this.raftBobCounter /= 2;
					}
				}
			}
			this.raftBobCounter -= time.ElapsedGameTime.Milliseconds;
			if (this.raftBobCounter <= 0)
			{
				this.raftBobCounter = Game1.random.Next(15, 28) * 100;
				if (this.yOffset <= 0f)
				{
					this.yOffset = 4f;
					currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2((float)collidingBox.X, (float)(collidingBox.Y - Game1.tileSize)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
				}
				else
				{
					this.yOffset = 0f;
				}
			}
			if (this.xVelocity > 0f)
			{
				this.xVelocity = Math.Max(0f, this.xVelocity - raftInertia / 2f);
			}
			else if (this.xVelocity < 0f)
			{
				this.xVelocity = Math.Min(0f, this.xVelocity + raftInertia / 2f);
			}
			if (this.yVelocity > 0f)
			{
				this.yVelocity = Math.Max(0f, this.yVelocity - raftInertia / 2f);
				return;
			}
			if (this.yVelocity < 0f)
			{
				this.yVelocity = Math.Min(0f, this.yVelocity + raftInertia / 2f);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00045CF8 File Offset: 0x00043EF8
		public void warpFarmer(Warp w)
		{
			if (w != null && !Game1.eventUp)
			{
				this.Halt();
				Game1.warpFarmer(w.TargetName, w.TargetX, w.TargetY, w.flipFarmer);
				if ((Game1.currentLocation.Name.Equals("Town") || Game1.jukeboxPlaying) && Game1.getLocationFromName(w.TargetName).IsOutdoors && Game1.currentSong != null && (Game1.currentSong.Name.Contains("town") || Game1.jukeboxPlaying))
				{
					Game1.changeMusicTrack("none");
				}
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00045D98 File Offset: 0x00043F98
		public static void passOutFromTired(Farmer who)
		{
			if (who.isRidingHorse())
			{
				who.getMount().dismount();
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.emergencyShutDown();
				Game1.exitActiveMenu();
			}
			Game1.warpFarmer(Utility.getHomeOfFarmer(who), (int)who.mostRecentBed.X / Game1.tileSize, (int)who.mostRecentBed.Y / Game1.tileSize, 2, false);
			Game1.newDay = true;
			who.currentLocation.lastTouchActionLocation = new Vector2((float)((int)who.mostRecentBed.X / Game1.tileSize), (float)((int)who.mostRecentBed.Y / Game1.tileSize));
			who.completelyStopAnimatingOrDoingAction();
			if (who.bathingClothes)
			{
				who.changeOutOfSwimSuit();
			}
			who.swimming = false;
			Game1.player.CanMove = false;
			Game1.changeMusicTrack("none");
			if (!(who.currentLocation is FarmHouse) && !(who.currentLocation is Cellar))
			{
				int moneyToTake = Math.Min(1000, who.Money / 10);
				who.Money -= moneyToTake;
				who.mailForTomorrow.Add("passedOut " + moneyToTake);
			}
			who.FarmerSprite.setCurrentSingleFrame(5, 3000, false, false);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00045ED5 File Offset: 0x000440D5
		public static void doSleepEmote(Farmer who)
		{
			who.doEmote(24);
			who.yJumpVelocity = -2f;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00045EEC File Offset: 0x000440EC
		public override Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			if (this.mount != null && !this.mount.dismounting)
			{
				return this.mount.GetBoundingBox();
			}
			return new Microsoft.Xna.Framework.Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, Game1.tileSize * 3 / 4, Game1.tileSize / 2);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00045F64 File Offset: 0x00044164
		public string getPetName()
		{
			foreach (NPC i in Game1.getFarm().characters)
			{
				if (i is Pet)
				{
					string name = i.name;
					return name;
				}
			}
			foreach (NPC j in Utility.getHomeOfFarmer(this).characters)
			{
				if (j is Pet)
				{
					string name = j.name;
					return name;
				}
			}
			return "the farm";
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00046020 File Offset: 0x00044220
		public bool hasPet()
		{
			using (List<NPC>.Enumerator enumerator = Game1.getFarm().characters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pet)
					{
						bool result = true;
						return result;
					}
				}
			}
			using (List<NPC>.Enumerator enumerator = Utility.getHomeOfFarmer(this).characters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pet)
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000460C8 File Offset: 0x000442C8
		public bool movedDuringLastTick()
		{
			return !this.position.Equals(this.lastPosition);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000460DE File Offset: 0x000442DE
		public int CompareTo(object obj)
		{
			return ((Farmer)obj).saveTime - this.saveTime;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000460F4 File Offset: 0x000442F4
		public override void draw(SpriteBatch b)
		{
			if (this.currentLocation == null || (!this.currentLocation.Equals(Game1.currentLocation) && !this.IsMainPlayer))
			{
				return;
			}
			Vector2 origin = new Vector2(this.xOffset, (this.yOffset + (float)(Game1.tileSize * 2) - (float)(this.GetBoundingBox().Height / 2)) / (float)Game1.pixelZoom + (float)Game1.pixelZoom);
			this.numUpdatesSinceLastDraw = 0;
			PropertyValue shadow = null;
			Tile shadowTile = Game1.currentLocation.Map.GetLayer("Buildings").PickTile(new Location(base.getStandingX(), base.getStandingY()), Game1.viewport.Size);
			if (this.isGlowing && this.coloredBorder)
			{
				b.Draw(base.Sprite.Texture, new Vector2(base.getLocalPosition(Game1.viewport).X - (float)Game1.pixelZoom, base.getLocalPosition(Game1.viewport).Y - (float)Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f - 0.001f));
			}
			else if (this.isGlowing && !this.coloredBorder)
			{
				this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.00011f), this.glowingColor * this.glowingTransparency, this.rotation, this);
			}
			if (shadowTile != null)
			{
				shadowTile.TileIndexProperties.TryGetValue("Shadow", out shadow);
			}
			if (shadow == null)
			{
				if (!this.temporarilyInvincible || this.temporaryInvincibilityTimer % 100 < 50)
				{
					this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport) + this.jitter + new Vector2(0f, (float)this.yJumpOffset), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.0001f), Color.White, this.rotation, this);
				}
			}
			else
			{
				this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.0001f), Color.White, this.rotation, this);
				this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, base.getLocalPosition(Game1.viewport), origin, Math.Max(0f, (float)base.getStandingY() / 10000f + 0.0002f), Color.Black * 0.25f, this.rotation, this);
			}
			if (this.isRafting)
			{
				b.Draw(Game1.toolSpriteSheet, base.getLocalPosition(Game1.viewport) + new Vector2(0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.toolSpriteSheet, 1, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)base.getStandingY() / 10000f - 0.001f);
			}
			if (Game1.activeClickableMenu == null && !Game1.eventUp && this.IsMainPlayer && this.CurrentTool != null && (Game1.oldKBState.IsKeyDown(Keys.LeftShift) || Game1.options.alwaysShowToolHitLocation) && this.CurrentTool.doesShowTileLocationMarker() && (!Game1.options.hideToolHitLocationWhenInMotion || !this.isMoving()))
			{
				Vector2 drawLocation = Game1.GlobalToLocal(Game1.viewport, Utility.withinRadiusOfPlayer(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 1, this) ? (new Vector2((float)((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize), (float)((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize)) * (float)Game1.tileSize) : Utility.clampToTile(this.GetToolLocation(false)));
				if (Game1.mouseCursorTransparency == 0f || Game1.isAnyGamePadButtonBeingPressed())
				{
					drawLocation = this.GetToolLocation(false);
				}
				b.Draw(Game1.mouseCursors, drawLocation, new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (this.GetToolLocation(false).Y + (float)Game1.tileSize) / 10000f);
			}
			if (base.IsEmoting)
			{
				Vector2 emotePosition = base.getLocalPosition(Game1.viewport);
				emotePosition.Y -= (float)(Game1.tileSize * 2 + Game1.tileSize / 2);
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)base.getStandingY() / 10000f);
			}
			if (this.ActiveObject != null)
			{
				Game1.drawPlayerHeldObject(this);
			}
			if (!this.IsMainPlayer)
			{
				if (this.FarmerSprite.isOnToolAnimation())
				{
					Game1.drawTool(this, this.FarmerSprite.CurrentToolIndex);
				}
				if (new Microsoft.Xna.Framework.Rectangle((int)this.position.X - Game1.viewport.X, (int)this.position.Y - Game1.viewport.Y, Game1.tileSize, Game1.tileSize * 3 / 2).Contains(new Point(Game1.getOldMouseX(), Game1.getOldMouseY())))
				{
					Game1.drawWithBorder(this.name, Color.Black, Color.White, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(this.name).X / 2f, -Game1.dialogueFont.MeasureString(this.name).Y));
				}
			}
			if (this.sparklingText != null)
			{
				this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.tileSize / 2) - this.sparklingText.textWidth / 2f, (float)(-(float)Game1.tileSize * 2))));
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000467D0 File Offset: 0x000449D0
		public static void drinkGlug(Farmer who)
		{
			Color c = Color.LightBlue;
			if (who.itemToEat != null)
			{
				string text = who.itemToEat.Name.Split(new char[]
				{
					' '
				}).Last<string>();
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2470525844u)
				{
					if (num <= 948615682u)
					{
						if (num != 154965655u)
						{
							if (num != 948615682u)
							{
								goto IL_168;
							}
							if (!(text == "Tonic"))
							{
								goto IL_168;
							}
							c = Color.Red;
							goto IL_168;
						}
						else
						{
							if (!(text == "Beer"))
							{
								goto IL_168;
							}
							c = Color.Orange;
							goto IL_168;
						}
					}
					else if (num != 1702016080u)
					{
						if (num != 2470525844u)
						{
							goto IL_168;
						}
						if (!(text == "Wine"))
						{
							goto IL_168;
						}
						c = Color.Purple;
						goto IL_168;
					}
					else if (!(text == "Cola"))
					{
						goto IL_168;
					}
				}
				else if (num <= 3224132511u)
				{
					if (num != 2679015821u)
					{
						if (num != 3224132511u)
						{
							goto IL_168;
						}
						if (!(text == "Juice"))
						{
							goto IL_168;
						}
						c = Color.LightGreen;
						goto IL_168;
					}
					else
					{
						if (!(text == "Remedy"))
						{
							goto IL_168;
						}
						c = Color.LimeGreen;
						goto IL_168;
					}
				}
				else if (num != 3560961217u)
				{
					if (num != 4017071298u)
					{
						goto IL_168;
					}
					if (!(text == "Milk"))
					{
						goto IL_168;
					}
					c = Color.White;
					goto IL_168;
				}
				else if (!(text == "Coffee"))
				{
					goto IL_168;
				}
				c = new Color(46, 20, 0);
			}
			IL_168:
			Game1.playSound("gulp");
			who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(653, 858, 1, 1), 9999f, 1, 1, who.position + new Vector2((float)(32 + Game1.random.Next(-2, 3) * 4), -48f), false, false, (float)who.getStandingY() / 10000f + 0.001f, 0.04f, c, 5f, 0f, 0f, 0f, false)
			{
				acceleration = new Vector2(0f, 0.5f)
			});
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000469F0 File Offset: 0x00044BF0
		public bool isDivorced()
		{
			using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.divorcedFromFarmer)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00046A48 File Offset: 0x00044C48
		public void wipeExMemories()
		{
			foreach (NPC i in Utility.getAllCharacters())
			{
				if (i.divorcedFromFarmer)
				{
					i.divorcedFromFarmer = false;
					i.datingFarmer = false;
					i.daysMarried = 0;
					try
					{
						this.friendships[i.name][0] = 0;
						this.friendships[i.name][1] = 0;
						this.friendships[i.name][2] = 0;
						this.friendships[i.name][3] = 0;
						this.friendships[i.name][4] = 0;
					}
					catch (Exception)
					{
					}
					i.CurrentDialogue.Clear();
					i.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:WipedMemory", new object[0]), i));
				}
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00046B60 File Offset: 0x00044D60
		public void getRidOfChildren()
		{
			for (int i = Utility.getHomeOfFarmer(this).characters.Count<NPC>() - 1; i >= 0; i--)
			{
				if (Utility.getHomeOfFarmer(this).characters[i] is Child && (Utility.getHomeOfFarmer(this).characters[i] as Child).isChildOf(this))
				{
					Utility.getHomeOfFarmer(this).characters.RemoveAt(i);
				}
			}
			for (int j = Game1.getLocationFromName("Farm").characters.Count<NPC>() - 1; j >= 0; j--)
			{
				if (Game1.getLocationFromName("Farm").characters[j] is Child && (Game1.getLocationFromName("Farm").characters[j] as Child).isChildOf(this))
				{
					Game1.getLocationFromName("Farm").characters.RemoveAt(j);
				}
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00046C45 File Offset: 0x00044E45
		public void animateOnce(int whichAnimation)
		{
			this.FarmerSprite.animateOnce(whichAnimation, 100f, 6);
			this.CanMove = false;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00046C60 File Offset: 0x00044E60
		public static void showItemIntake(Farmer who)
		{
			TemporaryAnimatedSprite tempSprite = null;
			Object toShow = (who.mostRecentlyGrabbedItem == null || !(who.mostRecentlyGrabbedItem is Object)) ? ((who.ActiveObject == null) ? null : who.ActiveObject) : ((Object)who.mostRecentlyGrabbedItem);
			if (toShow == null)
			{
				return;
			}
			switch (who.facingDirection)
			{
			case 0:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 2)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 3)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f - 0.001f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			case 1:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(Game1.tileSize / 2 - 4), (float)(-(float)Game1.tileSize)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(Game1.tileSize / 2 - 8), (float)(-(float)Game1.tileSize - 8)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(4f, (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			case 2:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize + Game1.tileSize / 3)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 8)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			case 3:
				switch (who.FarmerSprite.indexInCurrentAnimation)
				{
				case 1:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 2:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 2 + 4), (float)(-(float)Game1.tileSize - 12)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 3:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize * 2)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 4:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false);
					break;
				case 5:
					tempSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 + 4)), false, false, (float)who.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.02f, 0f, 0f, false);
					break;
				}
				break;
			}
			if ((toShow.Equals(who.ActiveObject) || (who.ActiveObject != null && toShow != null && toShow.ParentSheetIndex == who.ActiveObject.parentSheetIndex)) && who.FarmerSprite.indexInCurrentAnimation == 5)
			{
				tempSprite = null;
			}
			if (tempSprite != null)
			{
				who.currentLocation.temporarySprites.Add(tempSprite);
			}
			if (who.mostRecentlyGrabbedItem is ColoredObject && tempSprite != null)
			{
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, toShow.parentSheetIndex + 1, 16, 16), tempSprite.interval, 1, 0, tempSprite.Position, false, false, tempSprite.layerDepth + 0.0001f, tempSprite.alphaFade, (who.mostRecentlyGrabbedItem as ColoredObject).color, (float)Game1.pixelZoom, tempSprite.scaleChange, 0f, 0f, false));
			}
			if (who.FarmerSprite.indexInCurrentAnimation == 5)
			{
				who.Halt();
				who.FarmerSprite.CurrentAnimation = null;
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00047884 File Offset: 0x00045A84
		public static void showSwordSwipe(Farmer who)
		{
			TemporaryAnimatedSprite tempSprite = null;
			bool dagger = who.CurrentTool != null && who.CurrentTool is MeleeWeapon && (who.CurrentTool as MeleeWeapon).type == 1;
			Vector2 actionTile = who.GetToolLocation(true);
			if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
			{
				(who.CurrentTool as MeleeWeapon).DoDamage(who.currentLocation, (int)actionTile.X, (int)actionTile.Y, who.facingDirection, 1, who);
			}
			switch (who.facingDirection)
			{
			case 0:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.yVelocity = -0.3f;
							tempSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(0f, -32f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								alpha = 0.5f,
								rotation = 3.926991f
							};
						}
					}
					else
					{
						who.yVelocity = (dagger ? -0.5f : 0.5f);
					}
				}
				else if (dagger)
				{
					who.yVelocity = 0.6f;
				}
				break;
			}
			case 1:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.xVelocity = -0.3f;
							tempSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(4f, -12f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								alpha = 0.5f
							};
						}
					}
					else
					{
						who.xVelocity = (dagger ? -0.5f : 0.5f);
					}
				}
				else if (dagger)
				{
					who.xVelocity = 0.6f;
				}
				break;
			}
			case 2:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.yVelocity = 0.3f;
							tempSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(503, 256, 42, 17), who.position + new Vector2(-16f, -2f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								alpha = 0.5f,
								layerDepth = (who.position.Y + (float)Game1.tileSize) / 10000f
							};
						}
					}
					else
					{
						who.yVelocity = (dagger ? 0.5f : -0.5f);
					}
				}
				else if (dagger)
				{
					who.yVelocity = -0.6f;
				}
				break;
			}
			case 3:
			{
				int indexInCurrentAnimation = who.FarmerSprite.indexInCurrentAnimation;
				if (indexInCurrentAnimation != 0)
				{
					if (indexInCurrentAnimation != 1)
					{
						if (indexInCurrentAnimation == 5)
						{
							who.xVelocity = 0.3f;
							tempSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(-15f, -12f) * (float)Game1.pixelZoom, false, 0.07f, Color.White)
							{
								scale = (float)Game1.pixelZoom,
								animationLength = 1,
								interval = (float)who.FarmerSprite.CurrentAnimationFrame.milliseconds,
								flipped = true,
								alpha = 0.5f
							};
						}
					}
					else
					{
						who.xVelocity = (dagger ? 0.5f : -0.5f);
					}
				}
				else if (dagger)
				{
					who.xVelocity = -0.6f;
				}
				break;
			}
			}
			if (tempSprite != null)
			{
				if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon && who.CurrentTool.initialParentTileIndex == 4)
				{
					tempSprite.color = Color.HotPink;
				}
				who.currentLocation.temporarySprites.Add(tempSprite);
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00047D14 File Offset: 0x00045F14
		public static void showToolSwipeEffect(Farmer who)
		{
			if (who.CurrentTool != null && who.CurrentTool is WateringCan)
			{
				int arg_1B_0 = who.FacingDirection;
				return;
			}
			switch (who.FacingDirection)
			{
			case 0:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(18, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2 - 4)), Color.White, 4, false, (who.stamina <= 0f) ? 100f : 50f, 0, Game1.tileSize, 1f, Game1.tileSize, 0)
				{
					layerDepth = (float)(who.getStandingY() - Game1.tileSize / 7) / 10000f
				});
				return;
			case 1:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(15, who.position + new Vector2(20f, (float)(-(float)Game1.tileSize * 2 - 4)), Color.White, 4, false, (who.stamina <= 0f) ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
				{
					layerDepth = (float)(who.GetBoundingBox().Bottom + 1) / 10000f
				});
				return;
			case 2:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(19, who.position + new Vector2(-4f, (float)(-(float)Game1.tileSize * 2)), Color.White, 4, false, (who.stamina <= 0f) ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
				{
					layerDepth = (float)(who.GetBoundingBox().Bottom + 1) / 10000f
				});
				return;
			case 3:
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(15, who.position + new Vector2((float)(-(float)Game1.tileSize - 28), (float)(-(float)Game1.tileSize * 2 - 4)), Color.White, 4, true, (who.stamina <= 0f) ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
				{
					layerDepth = (float)(who.GetBoundingBox().Bottom + 1) / 10000f
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00047F80 File Offset: 0x00046180
		public static void canMoveNow(Farmer who)
		{
			who.CanMove = true;
			who.usingTool = false;
			who.usingSlingshot = false;
			who.FarmerSprite.pauseForSingleAnimation = false;
			who.yVelocity = 0f;
			who.xVelocity = 0f;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00047FBC File Offset: 0x000461BC
		public static void useTool(Farmer who)
		{
			if (who.toolOverrideFunction != null)
			{
				who.toolOverrideFunction(who);
				return;
			}
			if (who.CurrentTool != null)
			{
				float oldStamina = who.stamina;
				who.CurrentTool.DoFunction(who.currentLocation, (int)who.GetToolLocation(false).X, (int)who.GetToolLocation(false).Y, 1, who);
				who.lastClick = Vector2.Zero;
				who.checkForExhaustion(oldStamina);
				Game1.toolHold = 0f;
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00048038 File Offset: 0x00046238
		public void checkForExhaustion(float oldStamina)
		{
			if (this.stamina <= 0f && oldStamina > 0f)
			{
				if (!this.exhausted)
				{
					Game1.showGlobalMessage("You feel sluggish from over-exertion.");
				}
				this.setRunning(false, false);
				this.doEmote(36);
			}
			else if (this.stamina <= 15f && oldStamina > 15f)
			{
				Game1.showGlobalMessage("You're starting to feel exhausted.");
			}
			if (this.stamina <= 0f)
			{
				this.exhausted = true;
			}
			if (this.stamina <= -15f)
			{
				Game1.farmerShouldPassOut = true;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000480C4 File Offset: 0x000462C4
		public void setMoving(byte command)
		{
			bool didSomething = false;
			if (this.movementDirections.Count < 2)
			{
				if (command == 1 && !this.movementDirections.Contains(0) && !this.movementDirections.Contains(2))
				{
					this.movementDirections.Insert(0, 0);
					didSomething = true;
				}
				if (command == 2 && !this.movementDirections.Contains(1) && !this.movementDirections.Contains(3))
				{
					this.movementDirections.Insert(0, 1);
					didSomething = true;
				}
				if (command == 4 && !this.movementDirections.Contains(2) && !this.movementDirections.Contains(0))
				{
					this.movementDirections.Insert(0, 2);
					didSomething = true;
				}
				if (command == 8 && !this.movementDirections.Contains(3) && !this.movementDirections.Contains(1))
				{
					this.movementDirections.Insert(0, 3);
					didSomething = true;
				}
			}
			if (command == 33)
			{
				this.movementDirections.Remove(0);
				didSomething = true;
			}
			if (command == 34)
			{
				this.movementDirections.Remove(1);
				didSomething = true;
			}
			if (command == 36)
			{
				this.movementDirections.Remove(2);
				didSomething = true;
			}
			if (command == 40)
			{
				this.movementDirections.Remove(3);
				didSomething = true;
			}
			if (command == 16)
			{
				this.setRunning(true, false);
				didSomething = true;
			}
			else if (command == 48)
			{
				this.setRunning(false, false);
				didSomething = true;
			}
			if ((command & 64) == 64)
			{
				this.Halt();
				this.running = false;
				didSomething = true;
			}
			if ((Game1.IsClient & didSomething) && (command & 32) != 32)
			{
				this.timeOfLastPositionPacket = 60f;
			}
			if (Game1.IsServer & didSomething)
			{
				MultiplayerUtility.broadcastFarmerMovement(this.uniqueMultiplayerID, command, this.currentLocation.name);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00048264 File Offset: 0x00046464
		public void toolPowerIncrease()
		{
			if (this.toolPower == 0)
			{
				this.toolPitchAccumulator = 0;
			}
			this.toolPower++;
			if (this.CurrentTool is Pickaxe && this.toolPower == 1)
			{
				this.toolPower += 2;
			}
			Color powerUpColor = Color.White;
			int frameOffset = (base.FacingDirection == 0) ? 4 : ((base.FacingDirection == 2) ? 2 : 0);
			switch (this.toolPower)
			{
			case 1:
				powerUpColor = Color.Orange;
				if (!(this.CurrentTool is WateringCan))
				{
					this.FarmerSprite.CurrentFrame = 72 + frameOffset;
				}
				this.jitterStrength = 0.25f;
				break;
			case 2:
				powerUpColor = Color.LightSteelBlue;
				if (!(this.CurrentTool is WateringCan))
				{
					FarmerSprite expr_CE = this.FarmerSprite;
					int currentFrame = expr_CE.CurrentFrame;
					expr_CE.CurrentFrame = currentFrame + 1;
				}
				this.jitterStrength = 0.5f;
				break;
			case 3:
				powerUpColor = Color.Gold;
				this.jitterStrength = 1f;
				break;
			case 4:
				powerUpColor = Color.Violet;
				this.jitterStrength = 2f;
				break;
			}
			int xAnimation = (base.FacingDirection == 1) ? Game1.tileSize : ((base.FacingDirection == 3) ? (-Game1.tileSize) : ((base.FacingDirection == 2) ? (Game1.tileSize / 2) : 0));
			int yAnimation = Game1.tileSize * 3;
			if (this.CurrentTool is WateringCan)
			{
				xAnimation = -xAnimation;
				yAnimation = Game1.tileSize * 2;
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(21, this.position - new Vector2((float)xAnimation, (float)yAnimation), powerUpColor, 8, false, 70f, 0, Game1.tileSize, (float)base.getStandingY() / 10000f + 0.005f, Game1.tileSize * 2, 0));
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(192, 1152, Game1.tileSize, Game1.tileSize), 50f, 4, 0, this.position - new Vector2((float)((base.FacingDirection == 1) ? 0 : (-(float)Game1.tileSize)), (float)(Game1.tileSize * 2)), false, base.FacingDirection == 1, (float)base.getStandingY() / 10000f, 0.01f, Color.White, 1f, 0f, 0f, 0f, false));
			if (Game1.soundBank != null)
			{
				Cue arg_294_0 = Game1.soundBank.GetCue("toolCharge");
				Random r = new Random(Game1.dayOfMonth + (int)this.position.X * 1000 + (int)this.position.Y);
				arg_294_0.SetVariable("Pitch", (float)(r.Next(12, 16) * 100 + this.toolPower * 100));
				arg_294_0.Play();
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00048530 File Offset: 0x00046730
		public override void updatePositionFromServer(Vector2 position)
		{
			if (!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence)
			{
				this.remotePosition = position;
				if (Game1.IsClient && Game1.client.isConnected)
				{
					float msPrediction = Game1.client.averageRoundtripTime / 2f * 60f;
					if (this.movementDirections.Contains(0))
					{
						this.remotePosition.Y = this.remotePosition.Y - msPrediction / 64f * this.getMovementSpeed();
					}
					if (this.movementDirections.Contains(1))
					{
						this.remotePosition.X = this.remotePosition.X + msPrediction / 64f * this.getMovementSpeed();
					}
					if (this.movementDirections.Contains(2))
					{
						this.remotePosition.Y = this.remotePosition.Y + msPrediction / 64f * this.getMovementSpeed();
					}
					if (this.movementDirections.Contains(3))
					{
						this.remotePosition.X = this.remotePosition.X - msPrediction / 64f * this.getMovementSpeed();
					}
				}
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0004863C File Offset: 0x0004683C
		public override void lerpPosition(Vector2 target)
		{
			if (target.Equals(Vector2.Zero))
			{
				return;
			}
			int difference = (int)(target.X - this.position.X);
			if (Math.Abs(difference) > Game1.tileSize * 8)
			{
				this.position.X = target.X;
			}
			else
			{
				this.position.X = this.position.X + (float)difference * this.getMovementSpeed() * Math.Min(0.04f, this.timeOfLastPositionPacket / 40000f);
			}
			difference = (int)(target.Y - this.position.Y);
			if (Math.Abs(difference) > Game1.tileSize * 8)
			{
				this.position.Y = target.Y;
				return;
			}
			this.position.Y = this.position.Y + (float)difference * this.getMovementSpeed() * Math.Min(0.04f, this.timeOfLastPositionPacket / 40000f);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00048724 File Offset: 0x00046924
		public void UpdateIfOtherPlayer(GameTime time)
		{
			if (this.currentLocation == null)
			{
				return;
			}
			((FarmerSprite)this.sprite).checkForSingleAnimation(time);
			this.timeOfLastPositionPacket += (float)time.ElapsedGameTime.Milliseconds;
			if (!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence)
			{
				this.lerpPosition(this.remotePosition);
			}
			Vector2 arg_60_0 = this.position;
			this.MovePosition(time, Game1.viewport, this.currentLocation);
			this.rotation = 0f;
			if (this.movementDirections.Count == 0 && !this.FarmerSprite.pauseForSingleAnimation && !this.UsingTool)
			{
				this.sprite.StopAnimation();
			}
			if (Game1.IsServer && this.movementDirections.Count > 0)
			{
				MultiplayerUtility.broadcastFarmerPosition(this.uniqueMultiplayerID, this.position, this.currentLocation.name);
			}
			if (this.movementDirections.Count > 0)
			{
				Game1.debugOutput = this.position.ToString();
			}
			else
			{
				Game1.debugOutput = "no movemement";
			}
			if (this.CurrentTool != null)
			{
				this.CurrentTool.tickUpdate(time, this);
			}
			else if (this.ActiveObject != null)
			{
				this.ActiveObject.actionWhenBeingHeld(this);
			}
			base.updateEmote(time);
			base.updateGlow();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00048878 File Offset: 0x00046A78
		public void forceCanMove()
		{
			this.forceTimePass = false;
			this.movementDirections.Clear();
			Game1.isEating = false;
			this.CanMove = true;
			Game1.freezeControls = false;
			this.freezePause = 0;
			this.usingTool = false;
			this.usingSlingshot = false;
			this.FarmerSprite.pauseForSingleAnimation = false;
			if (this.CurrentTool is FishingRod)
			{
				(this.CurrentTool as FishingRod).isFishing = false;
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000488E9 File Offset: 0x00046AE9
		public void dropItem(Item i)
		{
			if (i != null && i.canBeDropped())
			{
				Game1.createItemDebris(i.getOne(), base.getStandingPosition(), base.FacingDirection, null);
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0004890E File Offset: 0x00046B0E
		public bool addEvent(string eventName, int daysActive)
		{
			if (!this.activeDialogueEvents.ContainsKey(eventName))
			{
				this.activeDialogueEvents.Add(eventName, daysActive);
				return true;
			}
			return false;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00048930 File Offset: 0x00046B30
		public void dropObjectFromInventory(int parentSheetIndex, int quantity)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && this.items[i] is Object && (this.items[i] as Object).parentSheetIndex == parentSheetIndex)
				{
					while (quantity > 0)
					{
						this.dropItem(this.items[i].getOne());
						Item expr_69 = this.items[i];
						int stack = expr_69.Stack;
						expr_69.Stack = stack - 1;
						quantity--;
						if (this.items[i].Stack <= 0)
						{
							this.items[i] = null;
							break;
						}
					}
					if (quantity <= 0)
					{
						return;
					}
				}
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000489FB File Offset: 0x00046BFB
		public Vector2 getMostRecentMovementVector()
		{
			return new Vector2(this.position.X - this.lastPosition.X, this.position.Y - this.lastPosition.Y);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00048A30 File Offset: 0x00046C30
		public void dropActiveItem()
		{
			if (this.CurrentItem != null && this.CurrentItem.canBeDropped())
			{
				Game1.createItemDebris(this.CurrentItem.getOne(), base.getStandingPosition(), base.FacingDirection, null);
				this.reduceActiveItemByOne();
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00048A6C File Offset: 0x00046C6C
		public static string getSkillNameFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return "Farming";
			case 1:
				return "Fishing";
			case 2:
				return "Foraging";
			case 3:
				return "Mining";
			case 4:
				return "Combat";
			case 5:
				return "Luck";
			default:
				return "";
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00048AC2 File Offset: 0x00046CC2
		public override bool isMoving()
		{
			return this.movementDirections.Count > 0;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00048AD4 File Offset: 0x00046CD4
		public bool hasCompletedCommunityCenter()
		{
			return this.mailReceived.Contains("ccBoilerRoom") && this.mailReceived.Contains("ccCraftsRoom") && this.mailReceived.Contains("ccPantry") && this.mailReceived.Contains("ccFishTank") && this.mailReceived.Contains("ccVault") && this.mailReceived.Contains("ccBulletin");
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00048B50 File Offset: 0x00046D50
		public void Update(GameTime time, GameLocation location)
		{
			if (Game1.CurrentEvent == null && !this.bathingClothes)
			{
				this.canOnlyWalk = false;
			}
			if (this.exhausted && this.stamina <= 1f)
			{
				this.currentEyes = 4;
				this.blinkTimer = -1000;
			}
			if (this.noMovementPause > 0)
			{
				this.CanMove = false;
				this.noMovementPause -= time.ElapsedGameTime.Milliseconds;
				if (this.noMovementPause <= 0)
				{
					this.CanMove = true;
				}
			}
			if (this.freezePause > 0)
			{
				this.CanMove = false;
				this.freezePause -= time.ElapsedGameTime.Milliseconds;
				if (this.freezePause <= 0)
				{
					this.CanMove = true;
				}
			}
			if (this.sparklingText != null && this.sparklingText.update(time))
			{
				this.sparklingText = null;
			}
			if (this.newLevelSparklingTexts.Count > 0 && this.sparklingText == null && !this.usingTool && this.CanMove && Game1.activeClickableMenu == null)
			{
				this.sparklingText = new SparklingText(Game1.dialogueFont, Farmer.getSkillNameFromIndex(this.newLevelSparklingTexts.Peek()) + " Level Up", Color.White, Color.White, true, 0.1, 2500, -1, 500);
				this.newLevelSparklingTexts.Dequeue();
			}
			if (this.jitterStrength > 0f)
			{
				this.jitter = new Vector2((float)Game1.random.Next(-(int)(this.jitterStrength * 100f), (int)((this.jitterStrength + 1f) * 100f)) / 100f, (float)Game1.random.Next(-(int)(this.jitterStrength * 100f), (int)((this.jitterStrength + 1f) * 100f)) / 100f);
			}
			this.blinkTimer += time.ElapsedGameTime.Milliseconds;
			if (this.blinkTimer > 2200 && Game1.random.NextDouble() < 0.01)
			{
				this.blinkTimer = -150;
				this.currentEyes = 4;
			}
			else if (this.blinkTimer > -100)
			{
				if (this.blinkTimer < -50)
				{
					this.currentEyes = 1;
				}
				else if (this.blinkTimer < 0)
				{
					this.currentEyes = 4;
				}
				else
				{
					this.currentEyes = 0;
				}
			}
			if (this.swimming)
			{
				this.yOffset = (float)(Math.Cos(time.TotalGameTime.TotalMilliseconds / 2000.0) * (double)Game1.pixelZoom);
				int oldSwimTimer = this.swimTimer;
				this.swimTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.timerSinceLastMovement == 0)
				{
					if (oldSwimTimer > 400 && this.swimTimer <= 400)
					{
						this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2(this.position.X, (float)(base.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
					}
					if (this.swimTimer < 0)
					{
						this.swimTimer = 800;
						Game1.playSound("slosh");
						this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2(this.position.X, (float)(base.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
					}
				}
				else if (!Game1.eventUp && Game1.activeClickableMenu == null && !Game1.paused)
				{
					if (this.timerSinceLastMovement > 700)
					{
						this.currentEyes = 4;
					}
					if (this.timerSinceLastMovement > 800)
					{
						this.currentEyes = 1;
					}
					if (this.swimTimer < 0)
					{
						this.swimTimer = 100;
						if (this.stamina < (float)this.maxStamina)
						{
							this.stamina += 1f;
						}
						if (this.health < this.maxHealth)
						{
							this.health++;
						}
					}
				}
			}
			((FarmerSprite)this.sprite).checkForSingleAnimation(time);
			if (Game1.IsClient && (!Game1.eventUp || (location.currentEvent != null && location.currentEvent.playerControlSequence)))
			{
				this.lerpPosition(this.remotePosition);
				this.timeOfLastPositionPacket += (float)time.ElapsedGameTime.Milliseconds;
			}
			if (this.CanMove)
			{
				this.rotation = 0f;
				if (this.health <= 0 && !Game1.killScreen)
				{
					this.CanMove = false;
					Game1.screenGlowOnce(Color.Red, true, 0.005f, 0.3f);
					Game1.killScreen = true;
					this.FarmerSprite.setCurrentFrame(5);
					this.jitterStrength = 1f;
					Game1.pauseTime = 3000f;
					Rumble.rumbleAndFade(0.75f, 1500f);
					this.freezePause = 8000;
					if (Game1.currentSong != null && Game1.currentSong.IsPlaying)
					{
						Game1.currentSong.Stop(AudioStopOptions.Immediate);
					}
					Game1.playSound("death");
					Game1.dialogueUp = false;
					Stats expr_5F1 = Game1.stats;
					uint timesUnconscious = expr_5F1.TimesUnconscious;
					expr_5F1.TimesUnconscious = timesUnconscious + 1u;
				}
				switch (base.getDirection())
				{
				case 0:
					location.isCollidingWithWarp(this.nextPosition(0));
					break;
				case 1:
					location.isCollidingWithWarp(this.nextPosition(1));
					break;
				case 2:
					location.isCollidingWithWarp(this.nextPosition(2));
					break;
				case 3:
					location.isCollidingWithWarp(this.nextPosition(3));
					break;
				}
				if (this.collisionNPC != null)
				{
					this.collisionNPC.farmerPassesThrough = true;
				}
				if (this.isMoving() && !this.isRidingHorse() && location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)) != null)
				{
					this.charactercollisionTimer += time.ElapsedGameTime.Milliseconds;
					if (this.charactercollisionTimer > 400)
					{
						location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)).shake(50);
					}
					if (this.charactercollisionTimer >= 1500 && this.collisionNPC == null)
					{
						this.collisionNPC = location.isCollidingWithCharacter(this.nextPosition(this.facingDirection));
						if (this.collisionNPC.name.Equals("Bouncer") && this.currentLocation != null && this.currentLocation.name.Equals("SandyHouse"))
						{
							this.collisionNPC.showTextAboveHead("Nice try...", -1, 2, 3000, 0);
							this.collisionNPC = null;
							this.charactercollisionTimer = 0;
						}
						else if (this.collisionNPC.name.Equals("Henchman") && this.currentLocation != null && this.currentLocation.name.Equals("WitchSwamp"))
						{
							this.collisionNPC = null;
							this.charactercollisionTimer = 0;
						}
					}
				}
				else
				{
					this.charactercollisionTimer = 0;
					if (this.collisionNPC != null && location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)) == null)
					{
						this.collisionNPC.farmerPassesThrough = false;
						this.collisionNPC = null;
					}
				}
			}
			MeleeWeapon.weaponsTypeUpdate(time);
			if (!Game1.eventUp || !this.isMoving() || this.currentLocation.currentEvent == null || this.currentLocation.currentEvent.playerControlSequence)
			{
				this.lastPosition = this.position;
				if (this.controller != null)
				{
					if (this.controller.update(time))
					{
						this.controller = null;
					}
				}
				else if (this.controller == null)
				{
					this.MovePosition(time, Game1.viewport, location);
				}
			}
			if (this.lastPosition.Equals(this.position))
			{
				this.timerSinceLastMovement += time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.timerSinceLastMovement = 0;
			}
			if (Game1.IsServer && this.movementDirections.Count > 0)
			{
				MultiplayerUtility.broadcastFarmerPosition(this.uniqueMultiplayerID, this.position, this.currentLocation.name);
			}
			if (this.yJumpOffset != 0)
			{
				this.yJumpVelocity -= 0.5f;
				this.yJumpOffset -= (int)this.yJumpVelocity;
				if (this.yJumpOffset >= 0)
				{
					this.yJumpOffset = 0;
					this.yJumpVelocity = 0f;
				}
			}
			base.updateEmote(time);
			base.updateGlow();
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				if (this.items[i] != null && this.items[i] is Tool)
				{
					((Tool)this.items[i]).tickUpdate(time, this);
				}
			}
			if (this.rightRing != null)
			{
				this.rightRing.update(time, location, this);
			}
			if (this.leftRing != null)
			{
				this.leftRing.update(time, location, this);
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000494FF File Offset: 0x000476FF
		public void addQuest(int questID)
		{
			if (!this.hasQuest(questID))
			{
				this.questLog.Add(Quest.getQuestFromId(questID));
				Game1.addHUDMessage(new HUDMessage("New Journal Entry", 2));
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0004952C File Offset: 0x0004772C
		public void removeQuest(int questID)
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].id == questID)
				{
					this.questLog.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00049574 File Offset: 0x00047774
		public void completeQuest(int questID)
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].id == questID)
				{
					this.questLog[i].questComplete();
				}
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000495C0 File Offset: 0x000477C0
		public bool hasQuest(int id)
		{
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i].id == id)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000495FC File Offset: 0x000477FC
		public bool hasNewQuestActivity()
		{
			foreach (Quest q in this.questLog)
			{
				if (q.showNew || (q.completed && !q.destroy))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00049668 File Offset: 0x00047868
		public float getMovementSpeed()
		{
			float movementSpeed;
			if (Game1.CurrentEvent == null || Game1.CurrentEvent.playerControlSequence)
			{
				this.movementMultiplier = 0.066f;
				movementSpeed = Math.Max(1f, ((float)this.speed + (Game1.eventUp ? 0f : ((float)this.addedSpeed + (this.isRidingHorse() ? 4.6f : this.temporarySpeedBuff)))) * this.movementMultiplier * (float)Game1.currentGameTime.ElapsedGameTime.Milliseconds);
				if (this.movementDirections.Count > 1)
				{
					movementSpeed = 0.7f * movementSpeed;
				}
			}
			else
			{
				movementSpeed = Math.Max(1f, (float)this.speed + (Game1.eventUp ? ((float)Math.Max(0, Game1.CurrentEvent.farmerAddedSpeed - 2)) : ((float)this.addedSpeed + (this.isRidingHorse() ? 5f : this.temporarySpeedBuff))));
				if (this.movementDirections.Count > 1)
				{
					movementSpeed = (float)Math.Max(1, (int)Math.Sqrt((double)(2f * (movementSpeed * movementSpeed))) / 2);
				}
			}
			return movementSpeed;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0004977F File Offset: 0x0004797F
		public bool isWearingRing(int ringIndex)
		{
			return (this.rightRing != null && this.rightRing.indexInTileSheet == ringIndex) || (this.leftRing != null && this.leftRing.indexInTileSheet == ringIndex);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000497B4 File Offset: 0x000479B4
		public override void Halt()
		{
			if (!this.FarmerSprite.pauseForSingleAnimation)
			{
				base.Halt();
			}
			this.movementDirections.Clear();
			this.stopJittering();
			this.armOffset = Vector2.Zero;
			if (this.isRidingHorse())
			{
				this.mount.Halt();
				this.mount.sprite.CurrentAnimation = null;
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00049814 File Offset: 0x00047A14
		public void stopJittering()
		{
			this.jitterStrength = 0f;
			this.jitter = Vector2.Zero;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0004982C File Offset: 0x00047A2C
		public override Microsoft.Xna.Framework.Rectangle nextPosition(int direction)
		{
			Microsoft.Xna.Framework.Rectangle nextPosition = this.GetBoundingBox();
			switch (direction)
			{
			case 0:
				nextPosition.Y -= (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			case 1:
				nextPosition.X += (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			case 2:
				nextPosition.Y += (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			case 3:
				nextPosition.X -= (int)Math.Ceiling((double)this.getMovementSpeed());
				break;
			}
			return nextPosition;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000498C0 File Offset: 0x00047AC0
		public Microsoft.Xna.Framework.Rectangle nextPositionHalf(int direction)
		{
			Microsoft.Xna.Framework.Rectangle nextPosition = this.GetBoundingBox();
			switch (direction)
			{
			case 0:
				nextPosition.Y -= (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			case 1:
				nextPosition.X += (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			case 2:
				nextPosition.Y += (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			case 3:
				nextPosition.X -= (int)Math.Ceiling((double)this.getMovementSpeed() / 2.0);
				break;
			}
			return nextPosition;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00049980 File Offset: 0x00047B80
		public int getProfessionForSkill(int skillType, int skillLevel)
		{
			if (skillLevel == 5)
			{
				switch (skillType)
				{
				case 0:
					if (this.professions.Contains(0))
					{
						return 0;
					}
					if (this.professions.Contains(1))
					{
						return 1;
					}
					break;
				case 1:
					if (this.professions.Contains(6))
					{
						return 6;
					}
					if (this.professions.Contains(7))
					{
						return 7;
					}
					break;
				case 2:
					if (this.professions.Contains(12))
					{
						return 12;
					}
					if (this.professions.Contains(13))
					{
						return 13;
					}
					break;
				case 3:
					if (this.professions.Contains(18))
					{
						return 18;
					}
					if (this.professions.Contains(19))
					{
						return 19;
					}
					break;
				case 4:
					if (this.professions.Contains(24))
					{
						return 24;
					}
					if (this.professions.Contains(25))
					{
						return 25;
					}
					break;
				}
			}
			else if (skillLevel == 10)
			{
				switch (skillType)
				{
				case 0:
					if (this.professions.Contains(1))
					{
						if (this.professions.Contains(4))
						{
							return 4;
						}
						if (this.professions.Contains(5))
						{
							return 5;
						}
					}
					else
					{
						if (this.professions.Contains(2))
						{
							return 2;
						}
						if (this.professions.Contains(3))
						{
							return 3;
						}
					}
					break;
				case 1:
					if (this.professions.Contains(6))
					{
						if (this.professions.Contains(8))
						{
							return 8;
						}
						if (this.professions.Contains(9))
						{
							return 9;
						}
					}
					else
					{
						if (this.professions.Contains(10))
						{
							return 10;
						}
						if (this.professions.Contains(11))
						{
							return 11;
						}
					}
					break;
				case 2:
					if (this.professions.Contains(12))
					{
						if (this.professions.Contains(14))
						{
							return 14;
						}
						if (this.professions.Contains(15))
						{
							return 15;
						}
					}
					else
					{
						if (this.professions.Contains(16))
						{
							return 16;
						}
						if (this.professions.Contains(17))
						{
							return 17;
						}
					}
					break;
				case 3:
					if (this.professions.Contains(18))
					{
						if (this.professions.Contains(20))
						{
							return 20;
						}
						if (this.professions.Contains(21))
						{
							return 21;
						}
					}
					else
					{
						if (this.professions.Contains(23))
						{
							return 23;
						}
						if (this.professions.Contains(22))
						{
							return 22;
						}
					}
					break;
				case 4:
					if (this.professions.Contains(24))
					{
						if (this.professions.Contains(26))
						{
							return 26;
						}
						if (this.professions.Contains(27))
						{
							return 27;
						}
					}
					else
					{
						if (this.professions.Contains(28))
						{
							return 28;
						}
						if (this.professions.Contains(29))
						{
							return 29;
						}
					}
					break;
				}
			}
			return -1;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00002834 File Offset: 0x00000A34
		public void behaviorOnMovement(int direction)
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00049C50 File Offset: 0x00047E50
		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (Game1.activeClickableMenu != null && (!Game1.eventUp || Game1.CurrentEvent.playerControlSequence))
			{
				return;
			}
			if (this.isRafting)
			{
				this.moveRaft(currentLocation, time);
				return;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				if (double.IsNaN((double)this.xVelocity) || double.IsNaN((double)this.yVelocity))
				{
					this.xVelocity = 0f;
					this.yVelocity = 0f;
				}
				Microsoft.Xna.Framework.Rectangle nextPosition = this.GetBoundingBox();
				nextPosition.X += (int)this.xVelocity;
				nextPosition.Y -= (int)this.yVelocity;
				if (!currentLocation.isCollidingPosition(nextPosition, viewport, true, -1, false, this))
				{
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
					this.xVelocity -= this.xVelocity / 16f;
					this.yVelocity -= this.yVelocity / 16f;
					if (Math.Abs(this.xVelocity) <= 0.05f)
					{
						this.xVelocity = 0f;
					}
					if (Math.Abs(this.yVelocity) <= 0.05f)
					{
						this.yVelocity = 0f;
					}
				}
				else
				{
					this.xVelocity -= this.xVelocity / 16f;
					this.yVelocity -= this.yVelocity / 16f;
					if (Math.Abs(this.xVelocity) <= 0.05f)
					{
						this.xVelocity = 0f;
					}
					if (Math.Abs(this.yVelocity) <= 0.05f)
					{
						this.yVelocity = 0f;
					}
				}
			}
			if (this.CanMove || Game1.eventUp || this.controller != null)
			{
				if (!this.temporaryImpassableTile.Intersects(this.GetBoundingBox()))
				{
					this.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
				}
				float movementSpeed = this.getMovementSpeed();
				this.temporarySpeedBuff = 0f;
				if (this.movementDirections.Contains(0))
				{
					this.facingDirection = 0;
					Warp warp = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(0));
					if (warp != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp);
						return;
					}
					if (this.isRidingHorse())
					{
						currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this);
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y - movementSpeed;
						this.behaviorOnMovement(0);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(0), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y - movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle tmp = this.nextPosition(0);
						tmp.Width /= 4;
						bool leftCorner = currentLocation.isCollidingPosition(tmp, viewport, true, 0, false, this);
						tmp.X += tmp.Width * 3;
						bool rightCorner = currentLocation.isCollidingPosition(tmp, viewport, true, 0, false, this);
						if (leftCorner && !rightCorner && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (rightCorner && !leftCorner && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Count == 1)
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								((FarmerSprite)this.sprite).animate(48, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(16, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(144, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(112, time);
						}
					}
				}
				if (this.movementDirections.Contains(2))
				{
					this.facingDirection = 2;
					Warp warp2 = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(2));
					if (warp2 != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp2);
						return;
					}
					if (this.isRidingHorse())
					{
						currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this);
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y + movementSpeed;
						this.behaviorOnMovement(2);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(2), viewport, true, 0, false, this))
					{
						this.position.Y = this.position.Y + movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle tmp2 = this.nextPosition(2);
						tmp2.Width /= 4;
						bool leftCorner2 = currentLocation.isCollidingPosition(tmp2, viewport, true, 0, false, this);
						tmp2.X += tmp2.Width * 3;
						bool rightCorner2 = currentLocation.isCollidingPosition(tmp2, viewport, true, 0, false, this);
						if (leftCorner2 && !rightCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (rightCorner2 && !leftCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, this))
						{
							this.position.X = this.position.X - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Count == 1)
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								((FarmerSprite)this.sprite).animate(32, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(0, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(128, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(96, time);
						}
					}
				}
				if (this.movementDirections.Contains(1))
				{
					this.facingDirection = 1;
					Warp warp3 = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(1));
					if (warp3 != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp3);
						return;
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X + movementSpeed;
						this.behaviorOnMovement(1);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(1), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X + movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle tmp3 = this.nextPosition(1);
						tmp3.Height /= 4;
						bool topCorner = currentLocation.isCollidingPosition(tmp3, viewport, true, 0, false, this);
						tmp3.Y += tmp3.Height * 3;
						bool bottomCorner = currentLocation.isCollidingPosition(tmp3, viewport, true, 0, false, this);
						if (topCorner && !bottomCorner && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (bottomCorner && !topCorner && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Contains(1))
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								this.FarmerSprite.animate(40, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(8, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(136, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(104, time);
						}
					}
				}
				if (this.movementDirections.Contains(3))
				{
					this.facingDirection = 3;
					Warp warp4 = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(3));
					if (warp4 != null && this.IsMainPlayer)
					{
						this.warpFarmer(warp4);
						return;
					}
					if (!currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X - movementSpeed;
						this.behaviorOnMovement(3);
					}
					else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(3), viewport, true, 0, false, this))
					{
						this.position.X = this.position.X - movementSpeed / 2f;
						this.behaviorOnMovement(0);
					}
					else if (this.movementDirections.Count == 1)
					{
						Microsoft.Xna.Framework.Rectangle tmp4 = this.nextPosition(3);
						tmp4.Height /= 4;
						bool topCorner2 = currentLocation.isCollidingPosition(tmp4, viewport, true, 0, false, this);
						tmp4.Y += tmp4.Height * 3;
						bool bottomCorner2 = currentLocation.isCollidingPosition(tmp4, viewport, true, 0, false, this);
						if (topCorner2 && !bottomCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y + (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
						else if (bottomCorner2 && !topCorner2 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, this))
						{
							this.position.Y = this.position.Y - (float)this.speed * ((float)time.ElapsedGameTime.Milliseconds / 64f);
						}
					}
					if (this.movementDirections.Contains(3))
					{
						if (this.ActiveObject == null || Game1.eventUp)
						{
							if (this.running)
							{
								((FarmerSprite)this.sprite).animate(56, time);
							}
							else
							{
								((FarmerSprite)this.sprite).animate(24, time);
							}
						}
						else if (this.running)
						{
							((FarmerSprite)this.sprite).animate(152, time);
						}
						else
						{
							((FarmerSprite)this.sprite).animate(120, time);
						}
					}
				}
				else if (this.moveUp && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(48, time);
				}
				else if (this.moveRight && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(40, time);
				}
				else if (this.moveDown && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(32, time);
				}
				else if (this.moveLeft && this.running && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(56, time);
				}
				else if (this.moveUp && this.running)
				{
					((FarmerSprite)this.sprite).animate(144, time);
				}
				else if (this.moveRight && this.running)
				{
					((FarmerSprite)this.sprite).animate(136, time);
				}
				else if (this.moveDown && this.running)
				{
					((FarmerSprite)this.sprite).animate(128, time);
				}
				else if (this.moveLeft && this.running)
				{
					((FarmerSprite)this.sprite).animate(152, time);
				}
				else if (this.moveUp && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(16, time);
				}
				else if (this.moveRight && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(8, time);
				}
				else if (this.moveDown && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(0, time);
				}
				else if (this.moveLeft && this.ActiveObject == null)
				{
					((FarmerSprite)this.sprite).animate(24, time);
				}
				else if (this.moveUp)
				{
					((FarmerSprite)this.sprite).animate(112, time);
				}
				else if (this.moveRight)
				{
					((FarmerSprite)this.sprite).animate(104, time);
				}
				else if (this.moveDown)
				{
					((FarmerSprite)this.sprite).animate(96, time);
				}
				else if (this.moveLeft)
				{
					((FarmerSprite)this.sprite).animate(120, time);
				}
			}
			if (this.isMoving() && !this.usingTool)
			{
				this.FarmerSprite.intervalModifier = 1f - (this.running ? 0.03f : 0.025f) * (Math.Max(1f, ((float)this.speed + (Game1.eventUp ? 0f : ((float)this.addedSpeed + (this.isRidingHorse() ? 4.6f : 0f)))) * this.movementMultiplier * (float)Game1.currentGameTime.ElapsedGameTime.Milliseconds) * 1.25f);
			}
			else
			{
				this.FarmerSprite.intervalModifier = 1f;
			}
			if (this.moveUp)
			{
				this.facingDirection = 0;
			}
			else if (this.moveRight)
			{
				this.facingDirection = 1;
			}
			else if (this.moveDown)
			{
				this.facingDirection = 2;
			}
			else if (this.moveLeft)
			{
				this.facingDirection = 3;
			}
			if (this.temporarilyInvincible)
			{
				this.temporaryInvincibilityTimer += time.ElapsedGameTime.Milliseconds;
				if (this.temporaryInvincibilityTimer > 1200)
				{
					this.temporarilyInvincible = false;
					this.temporaryInvincibilityTimer = 0;
				}
			}
			if (currentLocation != null && currentLocation.isFarmerCollidingWithAnyCharacter())
			{
				this.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle((int)base.getTileLocation().X * Game1.tileSize, (int)base.getTileLocation().Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			}
			if (this.isRidingHorse() && !this.mount.dismounting)
			{
				this.speed = 2;
				if (this.movementDirections.Count > 0 && (this.mount.facingDirection != this.movementDirections.First<int>() || (this.mount.facingDirection != 1 && this.movementDirections.Contains(1)) || (this.mount.facingDirection != 3 && this.movementDirections.Contains(3))) && (this.movementDirections.Count <= 1 || !this.movementDirections.Contains(1) || this.mount.facingDirection != 1) && (this.movementDirections.Count <= 1 || !this.movementDirections.Contains(3) || this.mount.facingDirection != 3))
				{
					this.mount.sprite.currentAnimation = null;
				}
				if (this.movementDirections.Count > 0)
				{
					if (this.movementDirections.Contains(1))
					{
						this.mount.faceDirection(1);
					}
					else if (this.movementDirections.Contains(3))
					{
						this.mount.faceDirection(3);
					}
					else
					{
						this.mount.faceDirection(this.movementDirections.First<int>());
					}
				}
				if (this.isMoving() && this.mount.sprite.currentAnimation == null)
				{
					if (this.movementDirections.Contains(1))
					{
						this.faceDirection(1);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(8, 70),
							new FarmerSprite.AnimationFrame(9, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(10, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(11, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(12, 70),
							new FarmerSprite.AnimationFrame(13, 70)
						});
					}
					else if (this.movementDirections.Contains(3))
					{
						this.faceDirection(3);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(8, 70, false, true, null, false),
							new FarmerSprite.AnimationFrame(9, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(10, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(11, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(12, 70, false, true, null, false),
							new FarmerSprite.AnimationFrame(13, 70, false, true, null, false)
						});
					}
					else if (this.movementDirections.First<int>().Equals(0))
					{
						this.faceDirection(0);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(15, 70),
							new FarmerSprite.AnimationFrame(16, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(17, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(18, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(19, 70),
							new FarmerSprite.AnimationFrame(20, 70)
						});
					}
					else if (this.movementDirections.First<int>().Equals(2))
					{
						this.faceDirection(2);
						this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(1, 70),
							new FarmerSprite.AnimationFrame(2, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(3, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(4, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
							new FarmerSprite.AnimationFrame(5, 70),
							new FarmerSprite.AnimationFrame(6, 70)
						});
					}
				}
				else if (!this.isMoving())
				{
					this.mount.Halt();
					this.mount.sprite.currentAnimation = null;
				}
				this.mount.position = this.position;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0004AF88 File Offset: 0x00049188
		public bool checkForQuestComplete(NPC n, int number1, int number2, Item item, string str, int questType = -1, int questTypeToIgnore = -1)
		{
			bool worked = false;
			for (int i = this.questLog.Count - 1; i >= 0; i--)
			{
				if (this.questLog[i] != null && (questType == -1 || this.questLog[i].questType == questType) && (questTypeToIgnore == -1 || this.questLog[i].questType != questTypeToIgnore) && this.questLog[i].checkIfComplete(n, number1, number2, item, str))
				{
					worked = true;
				}
			}
			return worked;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0004B00E File Offset: 0x0004920E
		public static void completelyStopAnimating(Farmer who)
		{
			who.completelyStopAnimatingOrDoingAction();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0004B018 File Offset: 0x00049218
		public void completelyStopAnimatingOrDoingAction()
		{
			this.CanMove = !Game1.eventUp;
			this.usingTool = false;
			this.FarmerSprite.pauseForSingleAnimation = false;
			this.usingSlingshot = false;
			this.canReleaseTool = false;
			this.Halt();
			this.sprite.StopAnimation();
			if (this.CurrentTool != null && this.CurrentTool is MeleeWeapon)
			{
				(this.CurrentTool as MeleeWeapon).isOnSpecial = false;
			}
			this.stopJittering();
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0004B091 File Offset: 0x00049291
		public void doEmote(int whichEmote)
		{
			if (!this.isEmoting)
			{
				this.isEmoting = true;
				this.currentEmote = whichEmote;
				this.currentEmoteFrame = 0;
				this.emoteInterval = 0f;
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0004B0BC File Offset: 0x000492BC
		public void reloadLivestockSprites()
		{
			using (List<CoopDweller>.Enumerator enumerator = this.coopDwellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.reload();
				}
			}
			using (List<BarnDweller>.Enumerator enumerator2 = this.barnDwellers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.reload();
				}
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0004B14C File Offset: 0x0004934C
		public void performTenMinuteUpdate()
		{
			if (this.addedSpeed > 0 && this.buffs.Count == 0 && Game1.buffsDisplay.otherBuffs.Count == 0 && Game1.buffsDisplay.food == null && Game1.buffsDisplay.drink == null)
			{
				this.addedSpeed = 0;
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0004B1A0 File Offset: 0x000493A0
		public void setRunning(bool isRunning, bool force = false)
		{
			if (this.canOnlyWalk || (this.bathingClothes && !this.running) || ((Game1.CurrentEvent != null & isRunning) && !Game1.CurrentEvent.isFestival && !Game1.CurrentEvent.playerControlSequence))
			{
				return;
			}
			if (this.isRidingHorse())
			{
				this.running = true;
				return;
			}
			if (this.stamina <= 0f)
			{
				this.speed = 2;
				if (this.running)
				{
					this.Halt();
				}
				this.running = false;
				return;
			}
			if (!force && (!this.CanMove || Game1.isEating || (Game1.currentLocation.currentEvent != null && !Game1.currentLocation.currentEvent.playerControlSequence) || (!isRunning && this.usingTool) || (!isRunning && Game1.pickingTool) || (this.sprite != null && ((FarmerSprite)this.sprite).pauseForSingleAnimation)))
			{
				if (this.usingTool)
				{
					this.running = isRunning;
					if (this.running)
					{
						this.speed = 5;
						return;
					}
					this.speed = 2;
				}
				return;
			}
			this.running = isRunning;
			if (this.running)
			{
				this.speed = 5;
				return;
			}
			this.speed = 2;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0004B2C7 File Offset: 0x000494C7
		public void addSeenResponse(int id)
		{
			this.dialogueQuestionsAnswered.Add(id);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0004B2D8 File Offset: 0x000494D8
		public void grabObject(Object obj)
		{
			if (obj != null)
			{
				this.CanMove = false;
				this.activeObject = obj;
				switch (this.facingDirection)
				{
				case 0:
					((FarmerSprite)this.sprite).animateOnce(80, 50f, 8);
					break;
				case 1:
					((FarmerSprite)this.sprite).animateOnce(72, 50f, 8);
					break;
				case 2:
					((FarmerSprite)this.sprite).animateOnce(64, 50f, 8);
					break;
				case 3:
					((FarmerSprite)this.sprite).animateOnce(88, 50f, 8);
					break;
				}
				Game1.playSound("pickUpItem");
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0004B388 File Offset: 0x00049588
		public string getTitle()
		{
			int level = this.Level;
			if (level >= 30)
			{
				return "Farm King";
			}
			if (level > 28)
			{
				return "Cropmaster";
			}
			if (level > 26)
			{
				return "Agriculturist";
			}
			if (level > 24)
			{
				return "Farmer";
			}
			if (level > 22)
			{
				return "Rancher";
			}
			if (level > 20)
			{
				return "Planter";
			}
			if (level > 18)
			{
				return "Granger";
			}
			if (level > 16)
			{
				if (!this.isMale)
				{
					return "Farmgirl";
				}
				return "Farmboy";
			}
			else
			{
				if (level > 14)
				{
					return "Sodbuster";
				}
				if (level > 12)
				{
					return "Smallholder";
				}
				if (level > 10)
				{
					return "Tiller";
				}
				if (level > 8)
				{
					return "Farmhand";
				}
				if (level > 6)
				{
					return "Cowpoke";
				}
				if (level > 4)
				{
					return "Bumpkin";
				}
				if (level > 2)
				{
					return "Greenhorn";
				}
				return "Newcomer";
			}
		}

		// Token: 0x040003B8 RID: 952
		public const int millisecondsPerSpeedUnit = 64;

		// Token: 0x040003B9 RID: 953
		public const byte halt = 64;

		// Token: 0x040003BA RID: 954
		public const byte up = 1;

		// Token: 0x040003BB RID: 955
		public const byte right = 2;

		// Token: 0x040003BC RID: 956
		public const byte down = 4;

		// Token: 0x040003BD RID: 957
		public const byte left = 8;

		// Token: 0x040003BE RID: 958
		public const byte run = 16;

		// Token: 0x040003BF RID: 959
		public const byte release = 32;

		// Token: 0x040003C0 RID: 960
		public const int FESTIVAL_WINNER = -9999;

		// Token: 0x040003C1 RID: 961
		public const int farmingSkill = 0;

		// Token: 0x040003C2 RID: 962
		public const int miningSkill = 3;

		// Token: 0x040003C3 RID: 963
		public const int fishingSkill = 1;

		// Token: 0x040003C4 RID: 964
		public const int foragingSkill = 2;

		// Token: 0x040003C5 RID: 965
		public const int combatSkill = 4;

		// Token: 0x040003C6 RID: 966
		public const int luckSkill = 5;

		// Token: 0x040003C7 RID: 967
		public const float interpolationConstant = 0.5f;

		// Token: 0x040003C8 RID: 968
		public const int runningSpeed = 5;

		// Token: 0x040003C9 RID: 969
		public const int walkingSpeed = 2;

		// Token: 0x040003CA RID: 970
		public const int caveNothing = 0;

		// Token: 0x040003CB RID: 971
		public const int caveBats = 1;

		// Token: 0x040003CC RID: 972
		public const int caveMushrooms = 2;

		// Token: 0x040003CD RID: 973
		public const int millisecondsInvincibleAfterDamage = 1200;

		// Token: 0x040003CE RID: 974
		public const int millisecondsPerFlickerWhenInvincible = 50;

		// Token: 0x040003CF RID: 975
		public const int startingStamina = 270;

		// Token: 0x040003D0 RID: 976
		public const int totalLevels = 35;

		// Token: 0x040003D1 RID: 977
		public static int tileSlideThreshold = Game1.tileSize / 2;

		// Token: 0x040003D2 RID: 978
		public const int maxInventorySpace = 36;

		// Token: 0x040003D3 RID: 979
		public const int hotbarSize = 12;

		// Token: 0x040003D4 RID: 980
		public const int eyesOpen = 0;

		// Token: 0x040003D5 RID: 981
		public const int eyesHalfShut = 4;

		// Token: 0x040003D6 RID: 982
		public const int eyesClosed = 1;

		// Token: 0x040003D7 RID: 983
		public const int eyesRight = 2;

		// Token: 0x040003D8 RID: 984
		public const int eyesLeft = 3;

		// Token: 0x040003D9 RID: 985
		public const int eyesWide = 5;

		// Token: 0x040003DA RID: 986
		public const int rancher = 0;

		// Token: 0x040003DB RID: 987
		public const int tiller = 1;

		// Token: 0x040003DC RID: 988
		public const int butcher = 2;

		// Token: 0x040003DD RID: 989
		public const int shepherd = 3;

		// Token: 0x040003DE RID: 990
		public const int artisan = 4;

		// Token: 0x040003DF RID: 991
		public const int agriculturist = 5;

		// Token: 0x040003E0 RID: 992
		public const int fisher = 6;

		// Token: 0x040003E1 RID: 993
		public const int trapper = 7;

		// Token: 0x040003E2 RID: 994
		public const int angler = 8;

		// Token: 0x040003E3 RID: 995
		public const int pirate = 9;

		// Token: 0x040003E4 RID: 996
		public const int baitmaster = 10;

		// Token: 0x040003E5 RID: 997
		public const int mariner = 11;

		// Token: 0x040003E6 RID: 998
		public const int forester = 12;

		// Token: 0x040003E7 RID: 999
		public const int gatherer = 13;

		// Token: 0x040003E8 RID: 1000
		public const int lumberjack = 14;

		// Token: 0x040003E9 RID: 1001
		public const int tapper = 15;

		// Token: 0x040003EA RID: 1002
		public const int botanist = 16;

		// Token: 0x040003EB RID: 1003
		public const int tracker = 17;

		// Token: 0x040003EC RID: 1004
		public const int miner = 18;

		// Token: 0x040003ED RID: 1005
		public const int geologist = 19;

		// Token: 0x040003EE RID: 1006
		public const int blacksmith = 20;

		// Token: 0x040003EF RID: 1007
		public const int burrower = 21;

		// Token: 0x040003F0 RID: 1008
		public const int excavator = 22;

		// Token: 0x040003F1 RID: 1009
		public const int gemologist = 23;

		// Token: 0x040003F2 RID: 1010
		public const int fighter = 24;

		// Token: 0x040003F3 RID: 1011
		public const int scout = 25;

		// Token: 0x040003F4 RID: 1012
		public const int brute = 26;

		// Token: 0x040003F5 RID: 1013
		public const int defender = 27;

		// Token: 0x040003F6 RID: 1014
		public const int acrobat = 28;

		// Token: 0x040003F7 RID: 1015
		public const int desperado = 29;

		// Token: 0x040003F8 RID: 1016
		public List<Quest> questLog = new List<Quest>();

		// Token: 0x040003F9 RID: 1017
		public List<int> professions = new List<int>();

		// Token: 0x040003FA RID: 1018
		public List<Point> newLevels = new List<Point>();

		// Token: 0x040003FB RID: 1019
		private Queue<int> newLevelSparklingTexts = new Queue<int>();

		// Token: 0x040003FC RID: 1020
		private SparklingText sparklingText;

		// Token: 0x040003FD RID: 1021
		public int[] experiencePoints = new int[6];

		// Token: 0x040003FE RID: 1022
		[XmlIgnore]
		private Item activeObject;

		// Token: 0x040003FF RID: 1023
		public List<Item> items;

		// Token: 0x04000400 RID: 1024
		public List<int> dialogueQuestionsAnswered = new List<int>();

		// Token: 0x04000401 RID: 1025
		public List<string> furnitureOwned = new List<string>();

		// Token: 0x04000402 RID: 1026
		public SerializableDictionary<string, int> cookingRecipes = new SerializableDictionary<string, int>();

		// Token: 0x04000403 RID: 1027
		public SerializableDictionary<string, int> craftingRecipes = new SerializableDictionary<string, int>();

		// Token: 0x04000404 RID: 1028
		public SerializableDictionary<string, int> activeDialogueEvents = new SerializableDictionary<string, int>();

		// Token: 0x04000405 RID: 1029
		public List<int> eventsSeen = new List<int>();

		// Token: 0x04000406 RID: 1030
		public List<string> songsHeard = new List<string>();

		// Token: 0x04000407 RID: 1031
		public List<int> achievements = new List<int>();

		// Token: 0x04000408 RID: 1032
		public List<int> specialItems = new List<int>();

		// Token: 0x04000409 RID: 1033
		public List<int> specialBigCraftables = new List<int>();

		// Token: 0x0400040A RID: 1034
		public List<string> mailReceived = new List<string>();

		// Token: 0x0400040B RID: 1035
		public List<string> mailForTomorrow = new List<string>();

		// Token: 0x0400040C RID: 1036
		public List<string> blueprints = new List<string>();

		// Token: 0x0400040D RID: 1037
		public List<CoopDweller> coopDwellers = new List<CoopDweller>();

		// Token: 0x0400040E RID: 1038
		public List<BarnDweller> barnDwellers = new List<BarnDweller>();

		// Token: 0x0400040F RID: 1039
		public Tool[] toolBox = new Tool[30];

		// Token: 0x04000410 RID: 1040
		public Object[] cupboard = new Object[30];

		// Token: 0x04000411 RID: 1041
		[XmlIgnore]
		public List<int> movementDirections = new List<int>();

		// Token: 0x04000412 RID: 1042
		public string farmName = "";

		// Token: 0x04000413 RID: 1043
		public string favoriteThing = "";

		// Token: 0x04000414 RID: 1044
		[XmlIgnore]
		public List<Buff> buffs = new List<Buff>();

		// Token: 0x04000415 RID: 1045
		[XmlIgnore]
		public List<object[]> multiplayerMessage = new List<object[]>();

		// Token: 0x04000416 RID: 1046
		[XmlIgnore]
		public GameLocation currentLocation = Game1.getLocationFromName("FarmHouse");

		// Token: 0x04000417 RID: 1047
		[XmlIgnore]
		public long uniqueMultiplayerID = -6666666L;

		// Token: 0x04000418 RID: 1048
		[XmlIgnore]
		public string _tmpLocationName = "FarmHouse";

		// Token: 0x04000419 RID: 1049
		[XmlIgnore]
		public string previousLocationName = "";

		// Token: 0x0400041A RID: 1050
		public bool catPerson = true;

		// Token: 0x0400041B RID: 1051
		[XmlIgnore]
		public Item mostRecentlyGrabbedItem;

		// Token: 0x0400041C RID: 1052
		[XmlIgnore]
		public Item itemToEat;

		// Token: 0x0400041D RID: 1053
		private FarmerRenderer farmerRenderer;

		// Token: 0x0400041E RID: 1054
		[XmlIgnore]
		public int toolPower;

		// Token: 0x0400041F RID: 1055
		[XmlIgnore]
		public int toolHold;

		// Token: 0x04000420 RID: 1056
		public Vector2 mostRecentBed;

		// Token: 0x04000421 RID: 1057
		public int shirt;

		// Token: 0x04000422 RID: 1058
		public int hair;

		// Token: 0x04000423 RID: 1059
		public int skin;

		// Token: 0x04000424 RID: 1060
		public int accessory = -1;

		// Token: 0x04000425 RID: 1061
		public int facialHair = -1;

		// Token: 0x04000426 RID: 1062
		[XmlIgnore]
		public int currentEyes;

		// Token: 0x04000427 RID: 1063
		[XmlIgnore]
		public int blinkTimer;

		// Token: 0x04000428 RID: 1064
		[XmlIgnore]
		public int festivalScore;

		// Token: 0x04000429 RID: 1065
		[XmlIgnore]
		public float temporarySpeedBuff;

		// Token: 0x0400042A RID: 1066
		public Color hairstyleColor;

		// Token: 0x0400042B RID: 1067
		public Color pantsColor;

		// Token: 0x0400042C RID: 1068
		public Color newEyeColor;

		// Token: 0x0400042D RID: 1069
		public Hat hat;

		// Token: 0x0400042E RID: 1070
		public Boots boots;

		// Token: 0x0400042F RID: 1071
		public Ring leftRing;

		// Token: 0x04000430 RID: 1072
		public Ring rightRing;

		// Token: 0x04000431 RID: 1073
		[XmlIgnore]
		public NPC dancePartner;

		// Token: 0x04000432 RID: 1074
		[XmlIgnore]
		public bool ridingMineElevator;

		// Token: 0x04000433 RID: 1075
		[XmlIgnore]
		public bool mineMovementDirectionWasUp;

		// Token: 0x04000434 RID: 1076
		[XmlIgnore]
		public bool cameFromDungeon;

		// Token: 0x04000435 RID: 1077
		[XmlIgnore]
		public bool readyConfirmation;

		// Token: 0x04000436 RID: 1078
		[XmlIgnore]
		public bool exhausted;

		// Token: 0x04000437 RID: 1079
		[XmlIgnore]
		public bool divorceTonight;

		// Token: 0x04000438 RID: 1080
		[XmlIgnore]
		public AnimatedSprite.endOfAnimationBehavior toolOverrideFunction;

		// Token: 0x04000439 RID: 1081
		public int deepestMineLevel;

		// Token: 0x0400043A RID: 1082
		private int currentToolIndex;

		// Token: 0x0400043B RID: 1083
		public int woodPieces;

		// Token: 0x0400043C RID: 1084
		public int stonePieces;

		// Token: 0x0400043D RID: 1085
		public int copperPieces;

		// Token: 0x0400043E RID: 1086
		public int ironPieces;

		// Token: 0x0400043F RID: 1087
		public int coalPieces;

		// Token: 0x04000440 RID: 1088
		public int goldPieces;

		// Token: 0x04000441 RID: 1089
		public int iridiumPieces;

		// Token: 0x04000442 RID: 1090
		public int quartzPieces;

		// Token: 0x04000443 RID: 1091
		public int caveChoice;

		// Token: 0x04000444 RID: 1092
		public int feed;

		// Token: 0x04000445 RID: 1093
		public int farmingLevel;

		// Token: 0x04000446 RID: 1094
		public int miningLevel;

		// Token: 0x04000447 RID: 1095
		public int combatLevel;

		// Token: 0x04000448 RID: 1096
		public int foragingLevel;

		// Token: 0x04000449 RID: 1097
		public int fishingLevel;

		// Token: 0x0400044A RID: 1098
		public int luckLevel;

		// Token: 0x0400044B RID: 1099
		public int newSkillPointsToSpend;

		// Token: 0x0400044C RID: 1100
		public int addedFarmingLevel;

		// Token: 0x0400044D RID: 1101
		public int addedMiningLevel;

		// Token: 0x0400044E RID: 1102
		public int addedCombatLevel;

		// Token: 0x0400044F RID: 1103
		public int addedForagingLevel;

		// Token: 0x04000450 RID: 1104
		public int addedFishingLevel;

		// Token: 0x04000451 RID: 1105
		public int addedLuckLevel;

		// Token: 0x04000452 RID: 1106
		public int maxStamina = 270;

		// Token: 0x04000453 RID: 1107
		public int maxItems = 12;

		// Token: 0x04000454 RID: 1108
		public float stamina = 270f;

		// Token: 0x04000455 RID: 1109
		public int resilience;

		// Token: 0x04000456 RID: 1110
		public int attack;

		// Token: 0x04000457 RID: 1111
		public int immunity;

		// Token: 0x04000458 RID: 1112
		public float attackIncreaseModifier;

		// Token: 0x04000459 RID: 1113
		public float knockbackModifier;

		// Token: 0x0400045A RID: 1114
		public float weaponSpeedModifier;

		// Token: 0x0400045B RID: 1115
		public float critChanceModifier;

		// Token: 0x0400045C RID: 1116
		public float critPowerModifier;

		// Token: 0x0400045D RID: 1117
		public float weaponPrecisionModifier;

		// Token: 0x0400045E RID: 1118
		public int money = 500;

		// Token: 0x0400045F RID: 1119
		public int clubCoins;

		// Token: 0x04000460 RID: 1120
		public uint totalMoneyEarned;

		// Token: 0x04000461 RID: 1121
		public uint millisecondsPlayed;

		// Token: 0x04000462 RID: 1122
		public Tool toolBeingUpgraded;

		// Token: 0x04000463 RID: 1123
		public int daysLeftForToolUpgrade;

		// Token: 0x04000464 RID: 1124
		private float timeOfLastPositionPacket;

		// Token: 0x04000465 RID: 1125
		private int numUpdatesSinceLastDraw;

		// Token: 0x04000466 RID: 1126
		public int houseUpgradeLevel;

		// Token: 0x04000467 RID: 1127
		public int daysUntilHouseUpgrade = -1;

		// Token: 0x04000468 RID: 1128
		public int coopUpgradeLevel;

		// Token: 0x04000469 RID: 1129
		public int barnUpgradeLevel;

		// Token: 0x0400046A RID: 1130
		public bool hasGreenhouse;

		// Token: 0x0400046B RID: 1131
		public bool hasRustyKey;

		// Token: 0x0400046C RID: 1132
		public bool hasSkullKey;

		// Token: 0x0400046D RID: 1133
		public bool hasUnlockedSkullDoor;

		// Token: 0x0400046E RID: 1134
		public bool hasDarkTalisman;

		// Token: 0x0400046F RID: 1135
		public bool hasMagicInk;

		// Token: 0x04000470 RID: 1136
		public bool showChestColorPicker = true;

		// Token: 0x04000471 RID: 1137
		public int magneticRadius = Game1.tileSize * 2;

		// Token: 0x04000472 RID: 1138
		public int temporaryInvincibilityTimer;

		// Token: 0x04000473 RID: 1139
		[XmlIgnore]
		public float rotation;

		// Token: 0x04000474 RID: 1140
		private int craftingTime = 1000;

		// Token: 0x04000475 RID: 1141
		private int raftPuddleCounter = 250;

		// Token: 0x04000476 RID: 1142
		private int raftBobCounter = 1000;

		// Token: 0x04000477 RID: 1143
		public int health = 100;

		// Token: 0x04000478 RID: 1144
		public int maxHealth = 100;

		// Token: 0x04000479 RID: 1145
		public int timesReachedMineBottom;

		// Token: 0x0400047A RID: 1146
		[XmlIgnore]
		public Vector2 jitter = Vector2.Zero;

		// Token: 0x0400047B RID: 1147
		[XmlIgnore]
		public Vector2 lastPosition;

		// Token: 0x0400047C RID: 1148
		[XmlIgnore]
		public Vector2 lastGrabTile = Vector2.Zero;

		// Token: 0x0400047D RID: 1149
		[XmlIgnore]
		public float jitterStrength;

		// Token: 0x0400047E RID: 1150
		[XmlIgnore]
		public float xOffset;

		// Token: 0x0400047F RID: 1151
		public bool isMale = true;

		// Token: 0x04000480 RID: 1152
		[XmlIgnore]
		public bool canMove = true;

		// Token: 0x04000481 RID: 1153
		[XmlIgnore]
		public bool running;

		// Token: 0x04000482 RID: 1154
		[XmlIgnore]
		public bool usingTool;

		// Token: 0x04000483 RID: 1155
		[XmlIgnore]
		public bool forceTimePass;

		// Token: 0x04000484 RID: 1156
		[XmlIgnore]
		public bool isRafting;

		// Token: 0x04000485 RID: 1157
		[XmlIgnore]
		public bool usingSlingshot;

		// Token: 0x04000486 RID: 1158
		[XmlIgnore]
		public bool bathingClothes;

		// Token: 0x04000487 RID: 1159
		[XmlIgnore]
		public bool canOnlyWalk;

		// Token: 0x04000488 RID: 1160
		[XmlIgnore]
		public bool temporarilyInvincible;

		// Token: 0x04000489 RID: 1161
		public bool hasBusTicket;

		// Token: 0x0400048A RID: 1162
		public bool stardewHero;

		// Token: 0x0400048B RID: 1163
		public bool hasClubCard;

		// Token: 0x0400048C RID: 1164
		public bool hasSpecialCharm;

		// Token: 0x0400048D RID: 1165
		[XmlIgnore]
		public bool canReleaseTool;

		// Token: 0x0400048E RID: 1166
		[XmlIgnore]
		public bool isCrafting;

		// Token: 0x0400048F RID: 1167
		[XmlIgnore]
		public Microsoft.Xna.Framework.Rectangle temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;

		// Token: 0x04000490 RID: 1168
		public bool canUnderstandDwarves;

		// Token: 0x04000491 RID: 1169
		public SerializableDictionary<int, int> basicShipped;

		// Token: 0x04000492 RID: 1170
		public SerializableDictionary<int, int> mineralsFound;

		// Token: 0x04000493 RID: 1171
		public SerializableDictionary<int, int> recipesCooked;

		// Token: 0x04000494 RID: 1172
		public SerializableDictionary<int, int[]> archaeologyFound;

		// Token: 0x04000495 RID: 1173
		public SerializableDictionary<int, int[]> fishCaught;

		// Token: 0x04000496 RID: 1174
		public SerializableDictionary<string, int[]> friendships;

		// Token: 0x04000497 RID: 1175
		[XmlIgnore]
		public Vector2 positionBeforeEvent;

		// Token: 0x04000498 RID: 1176
		[XmlIgnore]
		public Vector2 remotePosition;

		// Token: 0x04000499 RID: 1177
		[XmlIgnore]
		public int orientationBeforeEvent;

		// Token: 0x0400049A RID: 1178
		[XmlIgnore]
		public int swimTimer;

		// Token: 0x0400049B RID: 1179
		[XmlIgnore]
		public int timerSinceLastMovement;

		// Token: 0x0400049C RID: 1180
		[XmlIgnore]
		public int noMovementPause;

		// Token: 0x0400049D RID: 1181
		[XmlIgnore]
		public int freezePause;

		// Token: 0x0400049E RID: 1182
		[XmlIgnore]
		public float yOffset;

		// Token: 0x0400049F RID: 1183
		public BuildingUpgrade currentUpgrade;

		// Token: 0x040004A0 RID: 1184
		public string spouse;

		// Token: 0x040004A1 RID: 1185
		public string dateStringForSaveGame;

		// Token: 0x040004A2 RID: 1186
		public int overallsColor;

		// Token: 0x040004A3 RID: 1187
		public int shirtColor;

		// Token: 0x040004A4 RID: 1188
		public int skinColor;

		// Token: 0x040004A5 RID: 1189
		public int hairColor;

		// Token: 0x040004A6 RID: 1190
		public int eyeColor;

		// Token: 0x040004A7 RID: 1191
		[XmlIgnore]
		public Vector2 armOffset;

		// Token: 0x040004A8 RID: 1192
		public string bobber = "";

		// Token: 0x040004A9 RID: 1193
		private Horse mount;

		// Token: 0x040004AA RID: 1194
		private LocalizedContentManager farmerTextureManager;

		// Token: 0x040004AB RID: 1195
		public int saveTime;

		// Token: 0x040004AC RID: 1196
		public int daysMarried;

		// Token: 0x040004AD RID: 1197
		private int toolPitchAccumulator;

		// Token: 0x040004AE RID: 1198
		private int charactercollisionTimer;

		// Token: 0x040004AF RID: 1199
		private NPC collisionNPC;

		// Token: 0x040004B0 RID: 1200
		public float movementMultiplier = 0.01f;
	}
}
