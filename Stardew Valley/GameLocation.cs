using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	// Token: 0x02000044 RID: 68
	[XmlInclude(typeof(Farm)), XmlInclude(typeof(Beach)), XmlInclude(typeof(AnimalHouse)), XmlInclude(typeof(SlimeHutch)), XmlInclude(typeof(Shed)), XmlInclude(typeof(LibraryMuseum)), XmlInclude(typeof(AdventureGuild)), XmlInclude(typeof(Woods)), XmlInclude(typeof(Railroad)), XmlInclude(typeof(Summit)), XmlInclude(typeof(Forest)), XmlInclude(typeof(SeedShop)), XmlInclude(typeof(BathHousePool)), XmlInclude(typeof(FarmHouse)), XmlInclude(typeof(Club)), XmlInclude(typeof(BusStop)), XmlInclude(typeof(CommunityCenter)), XmlInclude(typeof(Desert)), XmlInclude(typeof(FarmCave)), XmlInclude(typeof(JojaMart)), XmlInclude(typeof(MineShaft)), XmlInclude(typeof(Mountain)), XmlInclude(typeof(Sewer)), XmlInclude(typeof(WizardHouse)), XmlInclude(typeof(Town)), XmlInclude(typeof(Cellar))]
	public class GameLocation
	{
		// Token: 0x17000057 RID: 87
		[XmlIgnore]
		public float LightLevel
		{
			// Token: 0x06000445 RID: 1093 RVA: 0x0004F524 File Offset: 0x0004D724
			get
			{
				return this.lightLevel;
			}
			// Token: 0x06000446 RID: 1094 RVA: 0x0004F52C File Offset: 0x0004D72C
			set
			{
				this.lightLevel = value;
			}
		}

		// Token: 0x17000058 RID: 88
		[XmlIgnore]
		public Map Map
		{
			// Token: 0x06000447 RID: 1095 RVA: 0x0004F535 File Offset: 0x0004D735
			get
			{
				return this.map;
			}
			// Token: 0x06000448 RID: 1096 RVA: 0x0004F53D File Offset: 0x0004D73D
			set
			{
				this.map = value;
			}
		}

		// Token: 0x17000059 RID: 89
		[XmlIgnore]
		public SerializableDictionary<Vector2, Object> Objects
		{
			// Token: 0x06000449 RID: 1097 RVA: 0x0004F546 File Offset: 0x0004D746
			get
			{
				return this.objects;
			}
		}

		// Token: 0x1700005A RID: 90
		[XmlIgnore]
		public List<TemporaryAnimatedSprite> TemporarySprites
		{
			// Token: 0x0600044A RID: 1098 RVA: 0x0004F54E File Offset: 0x0004D74E
			get
			{
				return this.temporarySprites;
			}
		}

		// Token: 0x1700005B RID: 91
		public string Name
		{
			// Token: 0x0600044B RID: 1099 RVA: 0x0004F556 File Offset: 0x0004D756
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700005C RID: 92
		[XmlIgnore]
		public bool IsFarm
		{
			// Token: 0x0600044C RID: 1100 RVA: 0x0004F55E File Offset: 0x0004D75E
			get
			{
				return this.isFarm;
			}
			// Token: 0x0600044D RID: 1101 RVA: 0x0004F566 File Offset: 0x0004D766
			set
			{
				this.isFarm = value;
			}
		}

		// Token: 0x1700005D RID: 93
		[XmlIgnore]
		public bool IsOutdoors
		{
			// Token: 0x0600044E RID: 1102 RVA: 0x0004F56F File Offset: 0x0004D76F
			get
			{
				return this.isOutdoors;
			}
			// Token: 0x0600044F RID: 1103 RVA: 0x0004F577 File Offset: 0x0004D777
			set
			{
				this.isOutdoors = value;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0004F580 File Offset: 0x0004D780
		public GameLocation()
		{
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0004F644 File Offset: 0x0004D844
		public GameLocation(Map map, string name)
		{
			this.map = map;
			this.name = name;
			if (name.Contains("Farm") || name.Contains("Coop") || name.Contains("Barn") || name.Equals("SlimeHutch"))
			{
				this.isFarm = true;
			}
			this.loadObjects();
			this.objects.CollectionChanged += new NotifyCollectionChangedEventHandler(this.objectCollectionChanged);
			this.terrainFeatures.CollectionChanged += new NotifyCollectionChangedEventHandler(this.terrainFeaturesCollectionChanged);
			if ((this.isOutdoors || this is Sewer) && !(this is Desert))
			{
				this.waterTiles = new bool[map.Layers[0].LayerWidth, map.Layers[0].LayerHeight];
				bool foundAnyWater = false;
				for (int x = 0; x < map.Layers[0].LayerWidth; x++)
				{
					for (int y = 0; y < map.Layers[0].LayerHeight; y++)
					{
						if (this.doesTileHaveProperty(x, y, "Water", "Back") != null)
						{
							foundAnyWater = true;
							this.waterTiles[x, y] = true;
						}
					}
				}
				if (!foundAnyWater)
				{
					this.waterTiles = null;
				}
			}
			if (this.isOutdoors)
			{
				this.critters = new List<Critter>();
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool canSlimeMateHere()
		{
			return true;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool canSlimeHatchHere()
		{
			return true;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0004F84C File Offset: 0x0004DA4C
		public void objectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (Game1.IsServer && Game1.server.connectionsCount > 0)
			{
				NotifyCollectionChangedAction action = e.Action;
				Vector2 v;
				if (action == NotifyCollectionChangedAction.Add)
				{
					v = (Vector2)e.NewItems[0];
					MultiplayerUtility.broadcastObjectChange((short)v.X, (short)v.Y, 0, this.objects[v].bigCraftable ? 1 : 0, (int)((short)this.objects[v].ParentSheetIndex), this.name);
					return;
				}
				if (action != NotifyCollectionChangedAction.Remove)
				{
					return;
				}
				v = (Vector2)e.OldItems[0];
				MultiplayerUtility.broadcastObjectChange((short)v.X, (short)v.Y, 1, 0, -1, this.name);
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0004F908 File Offset: 0x0004DB08
		public void terrainFeaturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (Game1.IsServer && Game1.server.connectionsCount > 0)
			{
				NotifyCollectionChangedAction action = e.Action;
				Vector2 v;
				if (action == NotifyCollectionChangedAction.Add)
				{
					v = (Vector2)e.NewItems[0];
					TerrainFeatureDescription d = TerrainFeatureFactory.getIndexFromTerrainFeature(this.terrainFeatures[v]);
					MultiplayerUtility.broadcastObjectChange((short)v.X, (short)v.Y, 2, d.index, d.extraInfo, this.name);
					return;
				}
				if (action != NotifyCollectionChangedAction.Remove)
				{
					return;
				}
				v = (Vector2)e.OldItems[0];
				MultiplayerUtility.broadcastObjectChange((short)v.X, (short)v.Y, 3, 0, -1, this.name);
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0004F9B7 File Offset: 0x0004DBB7
		public void addCharacter(NPC character)
		{
			this.characters.Add(character);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0004F9C5 File Offset: 0x0004DBC5
		public List<NPC> getCharacters()
		{
			return this.characters;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0004F9CD File Offset: 0x0004DBCD
		public Microsoft.Xna.Framework.Rectangle getSourceRectForObject(int tileIndex)
		{
			return new Microsoft.Xna.Framework.Rectangle(tileIndex * 16 % Game1.objectSpriteSheet.Width, tileIndex * 16 / Game1.objectSpriteSheet.Width * 16, 16, 16);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0004F9FC File Offset: 0x0004DBFC
		public Warp isCollidingWithWarp(Microsoft.Xna.Framework.Rectangle position)
		{
			foreach (Warp w in this.warps)
			{
				if ((w.X == (int)Math.Floor((double)position.Left / (double)Game1.tileSize) || w.X == (int)Math.Floor((double)position.Right / (double)Game1.tileSize)) && (w.Y == (int)Math.Floor((double)position.Top / (double)Game1.tileSize) || w.Y == (int)Math.Floor((double)position.Bottom / (double)Game1.tileSize)))
				{
					return w;
				}
			}
			return null;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0004FACC File Offset: 0x0004DCCC
		public Warp isCollidingWithWarpOrDoor(Microsoft.Xna.Framework.Rectangle position)
		{
			Warp w = this.isCollidingWithWarp(position);
			if (w == null)
			{
				w = this.isCollidingWithDoors(position);
			}
			return w;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0004FAF0 File Offset: 0x0004DCF0
		public Warp isCollidingWithDoors(Microsoft.Xna.Framework.Rectangle position)
		{
			foreach (Vector2 v in Utility.getCornersOfThisRectangle(position))
			{
				Point rectangleCorner = new Point((int)v.X / Game1.tileSize, (int)v.Y / Game1.tileSize);
				foreach (Point door in this.doors.Keys)
				{
					if (rectangleCorner.Equals(door))
					{
						return this.getWarpFromDoor(door);
					}
				}
			}
			return null;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0004FBBC File Offset: 0x0004DDBC
		public Warp getWarpFromDoor(Point door)
		{
			string[] split = this.doesTileHaveProperty(door.X, door.Y, "Action", "Buildings").Split(new char[]
			{
				' '
			});
			if (split[0].Equals("WarpCommunityCenter"))
			{
				return new Warp(door.X, door.Y, "CommunityCenter", 32, 23, false);
			}
			return new Warp(door.X, door.Y, split[3], Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), false);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0004FC48 File Offset: 0x0004DE48
		public bool isFarmerCollidingWithAnyCharacter()
		{
			foreach (NPC i in this.characters)
			{
				if (i != null && Game1.player.GetBoundingBox().Intersects(i.GetBoundingBox()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0004FCB8 File Offset: 0x0004DEB8
		public bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer)
		{
			return this.isCollidingPosition(position, viewport, isFarmer, 0, false, null, false, false, false);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0004FCD4 File Offset: 0x0004DED4
		public bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, Character character)
		{
			return this.isCollidingPosition(position, viewport, false, 0, false, character, false, false, false);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0004FCF0 File Offset: 0x0004DEF0
		public bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider)
		{
			return this.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, null, false, false, false);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0004FD10 File Offset: 0x0004DF10
		public virtual bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return this.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, false, false, false);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0004FD30 File Offset: 0x0004DF30
		public virtual bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			if (position.Right < 0 || position.X > this.map.Layers[0].DisplayWidth || position.Bottom < 0 || position.Top > this.map.Layers[0].DisplayHeight)
			{
				return false;
			}
			Vector2 topRight = new Vector2((float)(position.Right / Game1.tileSize), (float)(position.Top / Game1.tileSize));
			Vector2 topLeft = new Vector2((float)(position.Left / Game1.tileSize), (float)(position.Top / Game1.tileSize));
			Vector2 BottomRight = new Vector2((float)(position.Right / Game1.tileSize), (float)(position.Bottom / Game1.tileSize));
			Vector2 BottomLeft = new Vector2((float)(position.Left / Game1.tileSize), (float)(position.Bottom / Game1.tileSize));
			bool biggerThanTile = position.Width > Game1.tileSize;
			Vector2 BottomMid = new Vector2((float)(position.Center.X / Game1.tileSize), (float)(position.Bottom / Game1.tileSize));
			Vector2 TopMid = new Vector2((float)(position.Center.X / Game1.tileSize), (float)(position.Top / Game1.tileSize));
			if (character == null && !ignoreCharacterRequirement)
			{
				return true;
			}
			if (!glider && (!Game1.eventUp || (character != null && character.GetType() != typeof(Character) && !isFarmer && (!pathfinding || !character.willDestroyObjectsUnderfoot))))
			{
				Object o;
				this.objects.TryGetValue(topRight, out o);
				if (o != null && !o.IsHoeDirt && !o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(topRight)) && o.getBoundingBox(topRight).Intersects(position) && character != null && (character.GetType() != typeof(FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
				{
					return true;
				}
				this.objects.TryGetValue(BottomRight, out o);
				if (o != null && !o.IsHoeDirt && !o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(BottomRight)) && o.getBoundingBox(BottomRight).Intersects(position) && character != null && (character.GetType() != typeof(FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
				{
					return true;
				}
				this.objects.TryGetValue(topLeft, out o);
				if (o != null && !o.IsHoeDirt && !o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(topLeft)) && o.getBoundingBox(topLeft).Intersects(position) && character != null && (character.GetType() != typeof(FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
				{
					return true;
				}
				this.objects.TryGetValue(BottomLeft, out o);
				if (o != null && !o.IsHoeDirt && !o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(BottomLeft)) && o.getBoundingBox(BottomLeft).Intersects(position) && character != null && (character.GetType() != typeof(FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
				{
					return true;
				}
				if (biggerThanTile)
				{
					this.objects.TryGetValue(BottomMid, out o);
					if (o != null && !o.IsHoeDirt && !o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(BottomMid)) && o.getBoundingBox(BottomMid).Intersects(position) && (character.GetType() != typeof(FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
					{
						return true;
					}
					this.objects.TryGetValue(TopMid, out o);
					if (o != null && !o.IsHoeDirt && !o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(TopMid)) && o.getBoundingBox(TopMid).Intersects(position) && (character.GetType() != typeof(FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
					{
						return true;
					}
				}
			}
			if (this.largeTerrainFeatures != null && !glider)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getBoundingBox().Intersects(position))
						{
							return true;
						}
					}
				}
			}
			if (!Game1.eventUp && !glider)
			{
				if (this.terrainFeatures.ContainsKey(topRight) && this.terrainFeatures[topRight].getBoundingBox(topRight).Intersects(position))
				{
					if (!pathfinding)
					{
						this.terrainFeatures[topRight].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, topRight, character, this);
					}
					if (this.terrainFeatures.ContainsKey(topRight) && !this.terrainFeatures[topRight].isPassable(character))
					{
						return true;
					}
				}
				if (this.terrainFeatures.ContainsKey(topLeft) && this.terrainFeatures[topLeft].getBoundingBox(topLeft).Intersects(position))
				{
					if (!pathfinding)
					{
						this.terrainFeatures[topLeft].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, topLeft, character, this);
					}
					if (this.terrainFeatures.ContainsKey(topLeft) && !this.terrainFeatures[topLeft].isPassable(character))
					{
						return true;
					}
				}
				if (this.terrainFeatures.ContainsKey(BottomRight) && this.terrainFeatures[BottomRight].getBoundingBox(BottomRight).Intersects(position))
				{
					if (!pathfinding)
					{
						this.terrainFeatures[BottomRight].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, BottomRight, character, this);
					}
					if (this.terrainFeatures.ContainsKey(BottomRight) && !this.terrainFeatures[BottomRight].isPassable(character))
					{
						return true;
					}
				}
				if (this.terrainFeatures.ContainsKey(BottomLeft) && this.terrainFeatures[BottomLeft].getBoundingBox(BottomLeft).Intersects(position))
				{
					if (!pathfinding)
					{
						this.terrainFeatures[BottomLeft].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, BottomLeft, character, this);
					}
					if (this.terrainFeatures.ContainsKey(BottomLeft) && !this.terrainFeatures[BottomLeft].isPassable(character))
					{
						return true;
					}
				}
				if (biggerThanTile)
				{
					if (this.terrainFeatures.ContainsKey(BottomMid) && this.terrainFeatures[BottomMid].getBoundingBox(BottomMid).Intersects(position))
					{
						if (!pathfinding)
						{
							this.terrainFeatures[BottomMid].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, BottomMid, character, this);
						}
						if (this.terrainFeatures.ContainsKey(BottomMid) && !this.terrainFeatures[BottomMid].isPassable(null))
						{
							return true;
						}
					}
					if (this.terrainFeatures.ContainsKey(TopMid) && this.terrainFeatures[TopMid].getBoundingBox(TopMid).Intersects(position))
					{
						if (!pathfinding)
						{
							this.terrainFeatures[TopMid].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, TopMid, character, this);
						}
						if (this.terrainFeatures.ContainsKey(TopMid) && !this.terrainFeatures[TopMid].isPassable(null))
						{
							return true;
						}
					}
				}
			}
			if (character != null && character.hasSpecialCollisionRules() && (character.isColliding(this, topRight) || character.isColliding(this, topLeft) || character.isColliding(this, BottomRight) || character.isColliding(this, BottomLeft)))
			{
				return true;
			}
			if (isFarmer || (character != null && character.collidesWithOtherCharacters))
			{
				for (int i = this.characters.Count - 1; i >= 0; i--)
				{
					if (this.characters[i] != null && (character == null || !character.Equals(this.characters[i])))
					{
						if (this.characters[i].GetBoundingBox().Intersects(position) && !Game1.player.temporarilyInvincible)
						{
							this.characters[i].behaviorOnFarmerPushing();
						}
						if (isFarmer && !Game1.eventUp && !this.characters[i].farmerPassesThrough && this.characters[i].GetBoundingBox().Intersects(position) && !Game1.player.temporarilyInvincible && Game1.player.temporaryImpassableTile.Equals(Microsoft.Xna.Framework.Rectangle.Empty) && (!this.characters[i].IsMonster || (!((Monster)this.characters[i]).isGlider && !Game1.player.GetBoundingBox().Intersects(this.characters[i].GetBoundingBox()))) && !this.characters[i].isInvisible)
						{
							return true;
						}
						if (!isFarmer && this.characters[i].GetBoundingBox().Intersects(position))
						{
							return true;
						}
					}
				}
			}
			if (isFarmer)
			{
				if (this.currentEvent != null && this.currentEvent.checkForCollision(position, (character != null) ? (character as Farmer) : Game1.player))
				{
					return true;
				}
				if (Game1.player.currentUpgrade != null && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && this.name.Equals("Farm") && position.Intersects(new Microsoft.Xna.Framework.Rectangle((int)Game1.player.currentUpgrade.positionOfCarpenter.X, (int)Game1.player.currentUpgrade.positionOfCarpenter.Y + Game1.tileSize, Game1.tileSize, Game1.tileSize / 2)))
				{
					return true;
				}
			}
			else
			{
				if (position.Intersects(Game1.player.GetBoundingBox()))
				{
					if (damagesFarmer > 0)
					{
						if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && ((MeleeWeapon)Game1.player.CurrentTool).isOnSpecial && ((MeleeWeapon)Game1.player.CurrentTool).type == 3)
						{
							Game1.farmerTakeDamage(damagesFarmer, false, character as Monster);
							return true;
						}
						if (character != null)
						{
							character.collisionWithFarmerBehavior();
						}
						Game1.farmerTakeDamage(Math.Max(1, damagesFarmer + Game1.random.Next(-damagesFarmer / 4, damagesFarmer / 4)), false, character as Monster);
						if (!glider && (character == null || !(character as Monster).passThroughCharacters()))
						{
							return true;
						}
					}
					else if (!glider && (!pathfinding || !(character is Monster)))
					{
						return true;
					}
				}
				if (damagesFarmer > 0 && !glider)
				{
					for (int j = 0; j < this.characters.Count; j++)
					{
						if (!this.characters[j].Equals(character) && position.Intersects(this.characters[j].GetBoundingBox()) && !(character is GreenSlime) && !(character is DustSpirit))
						{
							return true;
						}
					}
				}
				if ((this.isFarm || this.name.Equals("UndergroundMine")) && character != null && !character.name.Contains("NPC") && !character.eventActor && !glider)
				{
					PropertyValue barrier = null;
					Tile t = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Top), viewport.Size);
					if (t != null)
					{
						t.Properties.TryGetValue("NPCBarrier", out barrier);
					}
					if (barrier != null)
					{
						return true;
					}
					t = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
					if (t != null)
					{
						t.Properties.TryGetValue("NPCBarrier", out barrier);
					}
					if (barrier != null)
					{
						return true;
					}
					t = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Top), viewport.Size);
					if (t != null)
					{
						t.Properties.TryGetValue("NPCBarrier", out barrier);
					}
					if (barrier != null)
					{
						return true;
					}
					t = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
					if (t != null)
					{
						t.Properties.TryGetValue("NPCBarrier", out barrier);
					}
					if (barrier != null)
					{
						return true;
					}
				}
				if (glider && !projectile)
				{
					return false;
				}
			}
			if (!isFarmer || !Game1.player.isRafting)
			{
				PropertyValue passable = null;
				Tile tmpTile = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Top), viewport.Size);
				if (tmpTile != null)
				{
					tmpTile.TileIndexProperties.TryGetValue("Passable", out passable);
				}
				if (passable != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Top)))
				{
					return true;
				}
				tmpTile = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
				if (tmpTile != null)
				{
					tmpTile.TileIndexProperties.TryGetValue("Passable", out passable);
				}
				if (passable != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Bottom)))
				{
					return true;
				}
				tmpTile = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Top), viewport.Size);
				if (tmpTile != null)
				{
					tmpTile.TileIndexProperties.TryGetValue("Passable", out passable);
				}
				if (passable != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Top)))
				{
					return true;
				}
				tmpTile = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
				if (tmpTile != null)
				{
					tmpTile.TileIndexProperties.TryGetValue("Passable", out passable);
				}
				if (passable != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Bottom)))
				{
					return true;
				}
				if (biggerThanTile)
				{
					tmpTile = this.map.GetLayer("Back").PickTile(new Location(position.Center.X, position.Bottom), viewport.Size);
					if (tmpTile != null)
					{
						tmpTile.TileIndexProperties.TryGetValue("Passable", out passable);
					}
					if (passable != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Bottom)))
					{
						return true;
					}
					tmpTile = this.map.GetLayer("Back").PickTile(new Location(position.Center.X, position.Top), viewport.Size);
					if (tmpTile != null)
					{
						tmpTile.TileIndexProperties.TryGetValue("Passable", out passable);
					}
					if (passable != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Top)))
					{
						return true;
					}
				}
				Tile tmp = this.map.GetLayer("Buildings").PickTile(new Location(position.Right, position.Top), viewport.Size);
				if (tmp != null)
				{
					tmp.TileIndexProperties.TryGetValue("Shadow", out passable);
					if (passable == null)
					{
						tmp.TileIndexProperties.TryGetValue("Passable", out passable);
					}
					if (passable == null && !isFarmer)
					{
						tmp.TileIndexProperties.TryGetValue("NPCPassable", out passable);
					}
					if (passable == null && !isFarmer && character != null && character.canPassThroughActionTiles())
					{
						tmp.Properties.TryGetValue("Action", out passable);
					}
					if ((passable == null || passable.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Top)))
					{
						return character == null || character.shouldCollideWithBuildingLayer(this);
					}
				}
				tmp = this.map.GetLayer("Buildings").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
				if (tmp != null && (passable == null | isFarmer))
				{
					tmp.TileIndexProperties.TryGetValue("Shadow", out passable);
					if (passable == null)
					{
						tmp.TileIndexProperties.TryGetValue("Passable", out passable);
					}
					if (passable == null && !isFarmer)
					{
						tmp.TileIndexProperties.TryGetValue("NPCPassable", out passable);
					}
					if (passable == null && !isFarmer && character != null && character.canPassThroughActionTiles())
					{
						tmp.Properties.TryGetValue("Action", out passable);
					}
					if ((passable == null || passable.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Bottom)))
					{
						return character == null || character.shouldCollideWithBuildingLayer(this);
					}
				}
				tmp = this.map.GetLayer("Buildings").PickTile(new Location(position.Left, position.Top), viewport.Size);
				if (tmp != null && (passable == null | isFarmer))
				{
					tmp.TileIndexProperties.TryGetValue("Shadow", out passable);
					if (passable == null)
					{
						tmp.TileIndexProperties.TryGetValue("Passable", out passable);
					}
					if (passable == null && !isFarmer)
					{
						tmp.TileIndexProperties.TryGetValue("NPCPassable", out passable);
					}
					if (passable == null && !isFarmer && character != null && character.canPassThroughActionTiles())
					{
						tmp.Properties.TryGetValue("Action", out passable);
					}
					if ((passable == null || passable.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Top)))
					{
						return character == null || character.shouldCollideWithBuildingLayer(this);
					}
				}
				tmp = this.map.GetLayer("Buildings").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
				if (tmp != null && (passable == null | isFarmer))
				{
					tmp.TileIndexProperties.TryGetValue("Shadow", out passable);
					if (passable == null)
					{
						tmp.TileIndexProperties.TryGetValue("Passable", out passable);
					}
					if (passable == null && !isFarmer)
					{
						tmp.TileIndexProperties.TryGetValue("NPCPassable", out passable);
					}
					if (passable == null && !isFarmer && character != null && character.canPassThroughActionTiles())
					{
						tmp.Properties.TryGetValue("Action", out passable);
					}
					if ((passable == null || passable.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Bottom)))
					{
						return character == null || character.shouldCollideWithBuildingLayer(this);
					}
				}
				if (biggerThanTile)
				{
					tmp = this.map.GetLayer("Buildings").PickTile(new Location(position.Center.X, position.Top), viewport.Size);
					if (tmp != null && (passable == null | isFarmer))
					{
						tmp.TileIndexProperties.TryGetValue("Shadow", out passable);
						if (passable == null)
						{
							tmp.TileIndexProperties.TryGetValue("Passable", out passable);
						}
						if (passable == null && !isFarmer)
						{
							tmp.TileIndexProperties.TryGetValue("NPCPassable", out passable);
						}
						if (passable == null && !isFarmer && character != null && character.canPassThroughActionTiles())
						{
							tmp.Properties.TryGetValue("Action", out passable);
						}
						if ((passable == null || passable.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Top)))
						{
							return character == null || character.shouldCollideWithBuildingLayer(this);
						}
					}
					tmp = this.map.GetLayer("Buildings").PickTile(new Location(position.Center.X, position.Bottom), viewport.Size);
					if (tmp != null && (passable == null | isFarmer))
					{
						tmp.TileIndexProperties.TryGetValue("Shadow", out passable);
						if (passable == null)
						{
							tmp.TileIndexProperties.TryGetValue("Passable", out passable);
						}
						if (passable == null && !isFarmer)
						{
							tmp.TileIndexProperties.TryGetValue("NPCPassable", out passable);
						}
						if (passable == null && !isFarmer && character != null && character.canPassThroughActionTiles())
						{
							tmp.Properties.TryGetValue("Action", out passable);
						}
						if ((passable == null || passable.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Bottom)))
						{
							return character == null || character.shouldCollideWithBuildingLayer(this);
						}
					}
				}
				if (!isFarmer && passable != null)
				{
					string a = passable.ToString().Split(new char[]
					{
						' '
					})[0];
					if (a == "Door")
					{
						this.openDoor(new Location(position.Center.X / Game1.tileSize, position.Bottom / Game1.tileSize), false);
						this.openDoor(new Location(position.Center.X / Game1.tileSize, position.Top / Game1.tileSize), Game1.currentLocation.Equals(this));
					}
				}
				return false;
			}
			else
			{
				PropertyValue passable2 = null;
				Tile t2 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Top), viewport.Size);
				if (t2 != null)
				{
					t2.TileIndexProperties.TryGetValue("Water", out passable2);
				}
				if (passable2 == null)
				{
					if (this.isTileLocationOpen(new Location(position.Right, position.Top)) && !this.isTileOccupiedForPlacement(new Vector2((float)(position.Right / Game1.tileSize), (float)(position.Top / Game1.tileSize)), null))
					{
						Game1.player.isRafting = false;
						Game1.player.position = new Vector2((float)(position.Right / Game1.tileSize * Game1.tileSize), (float)(position.Top / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
						Game1.player.setTrajectory(0, 0);
					}
					return true;
				}
				t2 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
				if (t2 != null)
				{
					t2.TileIndexProperties.TryGetValue("Water", out passable2);
				}
				if (passable2 == null)
				{
					if (this.isTileLocationOpen(new Location(position.Right, position.Bottom)) && !this.isTileOccupiedForPlacement(new Vector2((float)(position.Right / Game1.tileSize), (float)(position.Bottom / Game1.tileSize)), null))
					{
						Game1.player.isRafting = false;
						Game1.player.position = new Vector2((float)(position.Right / Game1.tileSize * Game1.tileSize), (float)(position.Bottom / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
						Game1.player.setTrajectory(0, 0);
					}
					return true;
				}
				t2 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Top), viewport.Size);
				if (t2 != null)
				{
					t2.TileIndexProperties.TryGetValue("Water", out passable2);
				}
				if (passable2 == null)
				{
					if (this.isTileLocationOpen(new Location(position.Left, position.Top)) && !this.isTileOccupiedForPlacement(new Vector2((float)(position.Left / Game1.tileSize), (float)(position.Top / Game1.tileSize)), null))
					{
						Game1.player.isRafting = false;
						Game1.player.position = new Vector2((float)(position.Left / Game1.tileSize * Game1.tileSize), (float)(position.Top / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
						Game1.player.setTrajectory(0, 0);
					}
					return true;
				}
				t2 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
				if (t2 != null)
				{
					t2.TileIndexProperties.TryGetValue("Water", out passable2);
				}
				if (passable2 == null)
				{
					if (this.isTileLocationOpen(new Location(position.Left, position.Bottom)) && !this.isTileOccupiedForPlacement(new Vector2((float)(position.Left / Game1.tileSize), (float)(position.Bottom / Game1.tileSize)), null))
					{
						Game1.player.isRafting = false;
						Game1.player.position = new Vector2((float)(position.Left / Game1.tileSize * Game1.tileSize), (float)(position.Bottom / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
						Game1.player.setTrajectory(0, 0);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000517D4 File Offset: 0x0004F9D4
		public bool isTilePassable(Location tileLocation, xTile.Dimensions.Rectangle viewport)
		{
			PropertyValue passable = null;
			Tile tmp = this.map.GetLayer("Back").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tmp != null)
			{
				tmp.TileIndexProperties.TryGetValue("Passable", out passable);
			}
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			return passable == null && tile == null && tmp != null;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00051878 File Offset: 0x0004FA78
		public bool isPointPassable(Location location, xTile.Dimensions.Rectangle viewport)
		{
			PropertyValue passable = null;
			PropertyValue shadow = null;
			Tile tmp = this.map.GetLayer("Back").PickTile(new Location(location.X, location.Y), viewport.Size);
			if (tmp != null)
			{
				tmp.TileIndexProperties.TryGetValue("Passable", out passable);
			}
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(location.X, location.Y), viewport.Size);
			if (tile != null)
			{
				tile.TileIndexProperties.TryGetValue("Shadow", out shadow);
			}
			return passable == null && (tile == null || shadow != null);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00051920 File Offset: 0x0004FB20
		public bool isTilePassable(Microsoft.Xna.Framework.Rectangle nextPosition, xTile.Dimensions.Rectangle viewport)
		{
			return this.isPointPassable(new Location(nextPosition.Left, nextPosition.Top), viewport) && this.isPointPassable(new Location(nextPosition.Left, nextPosition.Bottom), viewport) && this.isPointPassable(new Location(nextPosition.Right, nextPosition.Top), viewport) && this.isPointPassable(new Location(nextPosition.Right, nextPosition.Bottom), viewport);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000519A0 File Offset: 0x0004FBA0
		public bool isTileOnMap(Vector2 position)
		{
			return position.X >= 0f && position.X < (float)this.map.Layers[0].LayerWidth && position.Y >= 0f && position.Y < (float)this.map.Layers[0].LayerHeight;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00051A07 File Offset: 0x0004FC07
		public bool isTileOnMap(int x, int y)
		{
			return x >= 0 && x < this.map.Layers[0].LayerWidth && y >= 0 && y < this.map.Layers[0].LayerHeight;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00051A48 File Offset: 0x0004FC48
		public void busLeave()
		{
			NPC pam = null;
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i].name.Equals("Pam"))
				{
					pam = this.characters[i];
					this.characters.RemoveAt(i);
					break;
				}
			}
			if (pam != null)
			{
				Game1.changeMusicTrack("none");
				Game1.playSound("openBox");
				if (this.name.Equals("BusStop"))
				{
					Game1.warpFarmer("Desert", 32, 27, true);
					pam.followSchedule = false;
					pam.position = new Vector2((float)(31 * Game1.tileSize), (float)(28 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
					pam.faceDirection(2);
					pam.CurrentDialogue.Peek().temporaryDialogue = Game1.parseText(Game1.content.LoadString("Strings\\Locations:Desert_BusArrived", new object[0]));
					Game1.getLocationFromName("Desert").characters.Add(pam);
					return;
				}
				pam.CurrentDialogue.Peek().temporaryDialogue = null;
				Game1.warpFarmer("BusStop", 9, 9, true);
				if (Game1.timeOfDay >= 2300)
				{
					pam.position = new Vector2((float)(18 * Game1.tileSize), (float)(7 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
					pam.faceDirection(2);
					Game1.getLocationFromName("Trailer").characters.Add(pam);
				}
				else if (Game1.timeOfDay >= 1700)
				{
					pam.position = new Vector2((float)(7 * Game1.tileSize), (float)(18 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
					pam.faceDirection(1);
					Game1.getLocationFromName("Saloon").characters.Add(pam);
				}
				else
				{
					pam.position = new Vector2((float)(8 * Game1.tileSize), (float)(10 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
					pam.faceDirection(2);
					Game1.getLocationFromName("BusStop").characters.Add(pam);
					pam.sprite.CurrentFrame = 0;
				}
				pam.DirectionsToNewLocation = null;
				pam.followSchedule = true;
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00051C94 File Offset: 0x0004FE94
		public int numberOfObjectsWithName(string name)
		{
			int number = 0;
			using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator = this.objects.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name.Equals(name))
					{
						number++;
					}
				}
			}
			return number;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00051CF8 File Offset: 0x0004FEF8
		public Point getWarpPointTo(string location)
		{
			foreach (Warp w in this.warps)
			{
				if (w.TargetName.Equals(location))
				{
					Point result = new Point(w.X, w.Y);
					return result;
				}
			}
			foreach (KeyValuePair<Point, string> v in this.doors)
			{
				if (v.Value.Equals(location))
				{
					Point result = v.Key;
					return result;
				}
			}
			return Point.Zero;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00051DC4 File Offset: 0x0004FFC4
		public Point getWarpPointTarget(Point warpPointLocation)
		{
			foreach (Warp w in this.warps)
			{
				if (w.X == warpPointLocation.X && w.Y == warpPointLocation.Y)
				{
					Point result = new Point(w.TargetX, w.TargetY);
					return result;
				}
			}
			foreach (KeyValuePair<Point, string> v in this.doors)
			{
				if (v.Key.Equals(warpPointLocation))
				{
					string action = this.doesTileHaveProperty(warpPointLocation.X, warpPointLocation.Y, "Action", "Buildings");
					if (action != null && action.Contains("Warp"))
					{
						string[] split = action.Split(new char[]
						{
							' '
						});
						Point result;
						if (split[0].Equals("WarpCommunityCenter"))
						{
							result = new Point(32, 23);
							return result;
						}
						result = new Point(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));
						return result;
					}
				}
			}
			return Point.Zero;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00051F1C File Offset: 0x0005011C
		public void boardBus(Vector2 playerTileLocation)
		{
			if (Game1.player.hasBusTicket || this.name.Equals("Desert"))
			{
				bool isPamOnDuty = false;
				for (int i = this.characters.Count - 1; i >= 0; i--)
				{
				}
				if (isPamOnDuty)
				{
					Game1.player.hasBusTicket = false;
					Game1.player.CanMove = false;
					Game1.viewportFreeze = true;
					Game1.player.position.X = -99999f;
					Game1.boardingBus = true;
					return;
				}
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Bus_NoDriver", new object[0]));
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00051FB8 File Offset: 0x000501B8
		public NPC doesPositionCollideWithCharacter(float x, float y)
		{
			foreach (NPC i in this.characters)
			{
				if (i.GetBoundingBox().Contains((int)x, (int)y))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00052020 File Offset: 0x00050220
		public NPC doesPositionCollideWithCharacter(Microsoft.Xna.Framework.Rectangle r, bool ignoreMonsters = false)
		{
			foreach (NPC i in this.characters)
			{
				if (i.GetBoundingBox().Intersects(r) && (!i.IsMonster || !ignoreMonsters))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00052090 File Offset: 0x00050290
		public void switchOutNightTiles()
		{
			try
			{
				PropertyValue nightTiles;
				this.map.Properties.TryGetValue("NightTiles", out nightTiles);
				if (nightTiles != null)
				{
					string[] split = nightTiles.ToString().Split(new char[]
					{
						' '
					});
					for (int i = 0; i < split.Length; i += 4)
					{
						this.map.GetLayer(split[i]).Tiles[Convert.ToInt32(split[i + 1]), Convert.ToInt32(split[i + 2])].TileIndex = Convert.ToInt32(split[i + 3]);
					}
				}
			}
			catch (Exception)
			{
			}
			if (!(this is MineShaft))
			{
				this.lightGlows.Clear();
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00052140 File Offset: 0x00050340
		public virtual void checkForMusic(GameTime time)
		{
			if (this.IsOutdoors && Game1.currentSong != null && !Game1.currentSong.IsPlaying && !Game1.isRaining && !Game1.eventUp)
			{
				if (!Game1.isDarkOut())
				{
					string currentSeason = Game1.currentSeason;
					if (!(currentSeason == "spring"))
					{
						if (!(currentSeason == "summer"))
						{
							if (!(currentSeason == "fall"))
							{
								if (currentSeason == "winter")
								{
									Game1.changeMusicTrack("winter_day_ambient");
								}
							}
							else
							{
								Game1.changeMusicTrack("fall_day_ambient");
							}
						}
						else
						{
							Game1.changeMusicTrack("summer_day_ambient");
						}
					}
					else
					{
						Game1.changeMusicTrack("spring_day_ambient");
					}
				}
				else if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
				{
					string currentSeason = Game1.currentSeason;
					if (!(currentSeason == "spring"))
					{
						if (!(currentSeason == "summer"))
						{
							if (currentSeason == "fall")
							{
								Game1.changeMusicTrack("spring_night_ambient");
							}
						}
						else
						{
							Game1.changeMusicTrack("spring_night_ambient");
						}
					}
					else
					{
						Game1.changeMusicTrack("spring_night_ambient");
					}
				}
			}
			else if ((Game1.currentSong == null || !Game1.currentSong.IsPlaying) && Game1.isRaining && !Game1.showingEndOfNightStuff)
			{
				Game1.changeMusicTrack("rain");
			}
			if (Game1.currentSong != null && !Game1.isRaining && (!Game1.currentSeason.Equals("fall") || !Game1.isDebrisWeather) && !Game1.currentSeason.Equals("winter") && !Game1.eventUp && Game1.timeOfDay < 1800 && this.name.Equals("Town") && (!Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("ambient")) && Game1.locationAfterWarp != null && Game1.locationAfterWarp.name.Equals("Town"))
			{
				Game1.changeMusicTrack("springtown");
				return;
			}
			if ((this.name.Equals("AnimalShop") || this.name.Equals("ScienceHouse")) && Game1.currentSong != null && !Game1.nextMusicTrack.Contains("marnie") && (!Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("ambient")) && this.currentEvent == null)
			{
				Game1.changeMusicTrack("marnieShop");
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000523BC File Offset: 0x000505BC
		public NPC isCollidingWithCharacter(Microsoft.Xna.Framework.Rectangle box)
		{
			foreach (NPC i in this.characters)
			{
				if (i.GetBoundingBox().Intersects(box))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00052420 File Offset: 0x00050620
		public virtual void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (this.critters != null)
			{
				for (int i = 0; i < this.critters.Count; i++)
				{
					this.critters[i].drawAboveFrontLayer(b);
				}
			}
			if (Game1.isSnowing && this.isOutdoors && !(this is Desert))
			{
				Vector2 currentViewport = new Vector2((float)Game1.viewport.X, (float)Game1.viewport.Y);
				this.snowPos = Game1.updateFloatingObjectPositionForMovement(this.snowPos, currentViewport, Game1.previousViewportPosition, -1f);
				this.snowPos.X = this.snowPos.X % (float)(16 * Game1.pixelZoom);
				Vector2 v = default(Vector2);
				for (float x = (float)(-16 * Game1.pixelZoom) + this.snowPos.X % (float)(16 * Game1.pixelZoom); x < (float)Game1.viewport.Width; x += (float)(16 * Game1.pixelZoom))
				{
					for (float y = (float)(-16 * Game1.pixelZoom) + this.snowPos.Y % (float)(16 * Game1.pixelZoom); y < (float)Game1.viewport.Height; y += (float)(16 * Game1.pixelZoom))
					{
						v.X = (float)((int)x);
						v.Y = (float)((int)y);
						b.Draw(Game1.mouseCursors, v, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(368 + (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1200.0) / 75 * 16, 192, 16, 16)), Color.White * Game1.options.snowTransparency, 0f, Vector2.Zero, (float)Game1.pixelZoom + 0.001f, SpriteEffects.None, 1f);
					}
				}
			}
			using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.drawAboveAlwaysFrontLayer(b);
				}
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0005263C File Offset: 0x0005083C
		public bool moveObject(int oldX, int oldY, int newX, int newY)
		{
			Vector2 oldObjectLocation = new Vector2((float)oldX, (float)oldY);
			Vector2 newObjectLocation = new Vector2((float)newX, (float)newY);
			if (this.objects.ContainsKey(oldObjectLocation) && !this.objects.ContainsKey(newObjectLocation))
			{
				Object o = this.objects[oldObjectLocation];
				o.tileLocation = newObjectLocation;
				this.objects.Remove(oldObjectLocation);
				this.objects.Add(newObjectLocation, o);
				return true;
			}
			return false;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000526B0 File Offset: 0x000508B0
		private void getGalaxySword()
		{
			Game1.flashAlpha = 1f;
			Game1.player.holdUpItemThenMessage(new MeleeWeapon(4), true);
			Game1.player.CurrentTool = new MeleeWeapon(4);
			Game1.player.mailReceived.Add("galaxySword");
			Game1.player.jitterStrength = 0f;
			Game1.screenGlowHold = false;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00052714 File Offset: 0x00050914
		public virtual void performTouchAction(string fullActionString, Vector2 playerStandingPosition)
		{
			if (Game1.eventUp)
			{
				return;
			}
			try
			{
				string text = fullActionString.Split(new char[]
				{
					' '
				})[0];
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1817135690u)
				{
					if (num <= 799419560u)
					{
						if (num != 327122275u)
						{
							if (num != 764036487u)
							{
								if (num == 799419560u)
								{
									if (text == "Sleep")
									{
										if (!Game1.newDay && Game1.shouldTimePass() && (Game1.timeOfDay > 600 || Game1.gameTimeInterval > 2000))
										{
											if (Game1.player.ActiveObject == null)
											{
												this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:FarmHouse_Bed_GoToSleep", new object[0]), this.createYesNoResponses(), "Sleep", null);
											}
										}
									}
								}
							}
							else if (text == "legendarySword")
							{
								if (Game1.player.ActiveObject != null && Game1.player.ActiveObject.parentSheetIndex == 74 && !Game1.player.mailReceived.Contains("galaxySword"))
								{
									Game1.player.Halt();
									Game1.player.faceDirection(2);
									Game1.player.showCarrying();
									Game1.player.jitterStrength = 1f;
									Game1.pauseThenDoFunction(7000, new Game1.afterFadeFunction(this.getGalaxySword));
									Game1.changeMusicTrack("none");
									Game1.playSound("crit");
									Game1.screenGlowOnce(new Color(30, 0, 150), true, 0.01f, 0.999f);
									DelayedAction.playSoundAfterDelay("stardrop", 1500);
									Game1.screenOverlayTempSprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.White, 10, 2000, ""));
								}
								else if (!Game1.player.mailReceived.Contains("galaxySword"))
								{
									Game1.playSound("SpringBirds");
								}
							}
						}
						else if (text == "Emote")
						{
							this.getCharacterFromName(fullActionString.Split(new char[]
							{
								' '
							})[1]).doEmote(Convert.ToInt32(fullActionString.Split(new char[]
							{
								' '
							})[2]), true);
						}
					}
					else if (num != 1301151257u)
					{
						if (num != 1421563949u)
						{
							if (num == 1817135690u)
							{
								if (text == "WomensLocker")
								{
									if (Game1.player.isMale)
									{
										Farmer expr_F4A_cp_0_cp_0 = Game1.player;
										expr_F4A_cp_0_cp_0.position.Y = expr_F4A_cp_0_cp_0.position.Y + (float)((Game1.player.Speed + Game1.player.addedSpeed) * 2);
										Game1.player.Halt();
										Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WomensLocker_WrongGender", new object[0]));
									}
								}
							}
						}
						else if (text == "FaceDirection")
						{
							if (this.getCharacterFromName(fullActionString.Split(new char[]
							{
								' '
							})[1]) != null)
							{
								this.getCharacterFromName(fullActionString.Split(new char[]
								{
									' '
								})[1]).faceDirection(Convert.ToInt32(fullActionString.Split(new char[]
								{
									' '
								})[2]));
							}
						}
					}
					else if (text == "PoolEntrance")
					{
						if (!Game1.player.swimming)
						{
							Game1.player.swimTimer = 800;
							Game1.player.swimming = true;
							Farmer expr_FC7_cp_0_cp_0 = Game1.player;
							expr_FC7_cp_0_cp_0.position.Y = expr_FC7_cp_0_cp_0.position.Y + (float)(Game1.pixelZoom * 4);
							Game1.player.yVelocity = -8f;
							Game1.playSound("pullItemFromWater");
							this.temporarySprites.Add(new TemporaryAnimatedSprite(27, 100f, 4, 0, new Vector2(Game1.player.position.X, (float)(Game1.player.getStandingY() - Game1.tileSize * 5 / 8)), false, false)
							{
								layerDepth = 1f,
								motion = new Vector2(0f, 2f)
							});
						}
						else
						{
							Game1.player.jump();
							Game1.player.swimTimer = 800;
							Game1.player.position.X = playerStandingPosition.X * (float)Game1.tileSize;
							Game1.playSound("pullItemFromWater");
							Game1.player.yVelocity = 8f;
							Game1.player.swimming = false;
						}
						Game1.player.noMovementPause = 500;
					}
				}
				else if (num <= 3302649497u)
				{
					if (num != 2295680585u)
					{
						if (num != 2596419570u)
						{
							if (num == 3302649497u)
							{
								if (text == "Bus")
								{
									this.boardBus(playerStandingPosition);
								}
							}
						}
						else if (text == "ChangeIntoSwimsuit")
						{
							Game1.player.changeIntoSwimsuit();
						}
					}
					else if (text == "Door")
					{
						int i = 1;
						while (i < fullActionString.Split(new char[]
						{
							' '
						}).Length)
						{
							if (Game1.player.getFriendshipHeartLevelForNPC(fullActionString.Split(new char[]
							{
								' '
							})[i]) < 2 && i == fullActionString.Split(new char[]
							{
								' '
							}).Length - 1)
							{
								Game1.player.position -= Game1.player.getMostRecentMovementVector() * 2f;
								Game1.player.yVelocity = 0f;
								Game1.player.Halt();
								Game1.player.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
								if (Game1.player.getTileLocation().Equals(this.lastTouchActionLocation))
								{
									if (Game1.player.position.Y > this.lastTouchActionLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2))
									{
										Farmer expr_969_cp_0_cp_0 = Game1.player;
										expr_969_cp_0_cp_0.position.Y = expr_969_cp_0_cp_0.position.Y + (float)Game1.pixelZoom;
									}
									else
									{
										Farmer expr_984_cp_0_cp_0 = Game1.player;
										expr_984_cp_0_cp_0.position.Y = expr_984_cp_0_cp_0.position.Y - (float)Game1.pixelZoom;
									}
									this.lastTouchActionLocation = Vector2.Zero;
								}
								if ((Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(new char[]
								{
									' '
								})[1]) && (fullActionString.Split(new char[]
								{
									' '
								}).Length == 2 || Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(new char[]
								{
									' '
								})[2]))) || (fullActionString.Split(new char[]
								{
									' '
								}).Length == 3 && Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(new char[]
								{
									' '
								})[2])))
								{
									break;
								}
								if (fullActionString.Split(new char[]
								{
									' '
								}).Length == 2)
								{
									NPC character = Game1.getCharacterFromName(fullActionString.Split(new char[]
									{
										' '
									})[1], false);
									string gender = (character.gender == 0) ? "Male" : "Female";
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + gender, new object[]
									{
										character.name
									}));
								}
								else
								{
									NPC character2 = Game1.getCharacterFromName(fullActionString.Split(new char[]
									{
										' '
									})[1], false);
									NPC character3 = Game1.getCharacterFromName(fullActionString.Split(new char[]
									{
										' '
									})[2], false);
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", new object[]
									{
										character2.name,
										character3.name
									}));
								}
								break;
							}
							else
							{
								if (i != fullActionString.Split(new char[]
								{
									' '
								}).Length - 1 && Game1.player.getFriendshipHeartLevelForNPC(fullActionString.Split(new char[]
								{
									' '
								})[i]) >= 2)
								{
									if (!Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(new char[]
									{
										' '
									})[i]))
									{
										Game1.player.mailReceived.Add("doorUnlock" + fullActionString.Split(new char[]
										{
											' '
										})[i]);
									}
									break;
								}
								if (i == fullActionString.Split(new char[]
								{
									' '
								}).Length - 1 && Game1.player.getFriendshipHeartLevelForNPC(fullActionString.Split(new char[]
								{
									' '
								})[i]) >= 2)
								{
									if (!Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(new char[]
									{
										' '
									})[i]))
									{
										Game1.player.mailReceived.Add("doorUnlock" + fullActionString.Split(new char[]
										{
											' '
										})[i]);
									}
									break;
								}
								i++;
							}
						}
					}
				}
				else if (num <= 3711744508u)
				{
					if (num != 3579946100u)
					{
						if (num == 3711744508u)
						{
							if (text == "MagicalSeal")
							{
								if (!Game1.player.mailReceived.Contains("krobusUnseal"))
								{
									Game1.player.position -= Game1.player.getMostRecentMovementVector() * 2f;
									Game1.player.yVelocity = 0f;
									Game1.player.Halt();
									Game1.player.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
									if (Game1.player.getTileLocation().Equals(this.lastTouchActionLocation))
									{
										if (Game1.player.position.Y > this.lastTouchActionLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2))
										{
											Farmer expr_2B6_cp_0_cp_0 = Game1.player;
											expr_2B6_cp_0_cp_0.position.Y = expr_2B6_cp_0_cp_0.position.Y + (float)Game1.pixelZoom;
										}
										else
										{
											Farmer expr_2D1_cp_0_cp_0 = Game1.player;
											expr_2D1_cp_0_cp_0.position.Y = expr_2D1_cp_0_cp_0.position.Y - (float)Game1.pixelZoom;
										}
										this.lastTouchActionLocation = Vector2.Zero;
									}
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Sewer_MagicSeal", new object[0]));
									for (int j = 0; j < 40; j++)
									{
										this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(666, 1851, 8, 8), 25f, 4, 2, new Vector2(3f, 19f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.pixelZoom * 2 + j % 4 * 16), (float)(-(float)(j / 4) * Game1.tileSize / 4)), false, false)
										{
											layerDepth = (float)(18 * Game1.tileSize) / 10000f + (float)j / 10000f,
											color = new Color(100 + j * 4, j * 5, 120 + j * 4),
											pingPong = true,
											delayBeforeAnimationStart = j * 10,
											scale = (float)Game1.pixelZoom,
											alphaFade = 0.01f
										});
										this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(666, 1851, 8, 8), 25f, 4, 2, new Vector2(3f, 17f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.pixelZoom * 2 + j % 4 * 16), (float)(j / 4 * Game1.tileSize / 4)), false, false)
										{
											layerDepth = (float)(18 * Game1.tileSize) / 10000f + (float)j / 10000f,
											color = new Color(232 - j * 4, 192 - j * 6, 255 - j * 4),
											pingPong = true,
											delayBeforeAnimationStart = 320 + j * 10,
											scale = (float)Game1.pixelZoom,
											alphaFade = 0.01f
										});
										this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(666, 1851, 8, 8), 25f, 4, 2, new Vector2(3f, 19f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.pixelZoom * 2 + j % 4 * 16), (float)(-(float)(j / 4) * Game1.tileSize / 4)), false, false)
										{
											layerDepth = (float)(18 * Game1.tileSize) / 10000f + (float)j / 10000f,
											color = new Color(100 + j * 4, j * 6, 120 + j * 4),
											pingPong = true,
											delayBeforeAnimationStart = 640 + j * 10,
											scale = (float)Game1.pixelZoom,
											alphaFade = 0.01f
										});
									}
									Game1.player.jitterStrength = 2f;
									Game1.player.freezePause = 500;
									Game1.soundBank.PlayCue("debuffHit");
								}
							}
						}
					}
					else if (text == "MensLocker")
					{
						if (!Game1.player.isMale)
						{
							Farmer expr_EE7_cp_0_cp_0 = Game1.player;
							expr_EE7_cp_0_cp_0.position.Y = expr_EE7_cp_0_cp_0.position.Y + (float)((Game1.player.Speed + Game1.player.addedSpeed) * 2);
							Game1.player.Halt();
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MensLocker_WrongGender", new object[0]));
						}
					}
				}
				else if (num != 3998141403u)
				{
					if (num == 4271868850u)
					{
						if (text == "MagicWarp")
						{
							string locationToWarp = fullActionString.Split(new char[]
							{
								' '
							})[1];
							int locationX = Convert.ToInt32(fullActionString.Split(new char[]
							{
								' '
							})[2]);
							int locationY = Convert.ToInt32(fullActionString.Split(new char[]
							{
								' '
							})[3]);
							string mailRequired = (fullActionString.Split(new char[]
							{
								' '
							}).Length > 4) ? fullActionString.Split(new char[]
							{
								' '
							})[4] : null;
							if (mailRequired == null || Game1.player.mailReceived.Contains(mailRequired))
							{
								for (int k = 0; k < 12; k++)
								{
									this.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)Game1.player.position.X - Game1.tileSize * 4, (int)Game1.player.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)Game1.player.position.Y - Game1.tileSize * 4, (int)Game1.player.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
								}
								Game1.playSound("wand");
								Game1.displayFarmer = false;
								Game1.player.freezePause = 1000;
								Game1.flashAlpha = 1f;
								DelayedAction.warpAfterDelay(locationToWarp, new Point(locationX, locationY), 1000);
								Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(Game1.player.GetBoundingBox().X, Game1.player.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize);
								r.Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
								int l = 0;
								for (int x = Game1.player.getTileX() + 8; x >= Game1.player.getTileX() - 8; x--)
								{
									Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)x, (float)Game1.player.getTileY()) * (float)Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
									{
										layerDepth = 1f,
										delayBeforeAnimationStart = l * 25,
										motion = new Vector2(-0.25f, 0f)
									});
									l++;
								}
							}
						}
					}
				}
				else if (text == "ChangeOutOfSwimsuit")
				{
					Game1.player.changeOutOfSwimSuit();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00053814 File Offset: 0x00051A14
		public virtual void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			this.wasUpdated = true;
			AmbientLocationSounds.update(time);
			if (this.critters != null)
			{
				for (int i = this.critters.Count - 1; i >= 0; i--)
				{
					if (this.critters[i].update(time, this))
					{
						this.critters.RemoveAt(i);
					}
				}
			}
			if (this.fishSplashAnimation != null)
			{
				this.fishSplashAnimation.update(time);
				if (Game1.random.NextDouble() < 0.02)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(0, this.fishSplashAnimation.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), Color.White * 0.3f, 8, false, 100f, 0, -1, -1f, -1, 0));
				}
			}
			if (this.orePanAnimation != null)
			{
				this.orePanAnimation.update(time);
				if (Game1.random.NextDouble() < 0.05)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), this.orePanAnimation.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, 0.02f, Color.White * 0.8f)
					{
						scale = (float)(Game1.pixelZoom / 2),
						animationLength = 6,
						interval = 100f
					});
				}
			}
			if (this.doorSprites != null)
			{
				foreach (KeyValuePair<Point, TemporaryAnimatedSprite> v in this.doorSprites)
				{
					v.Value.update(time);
				}
			}
			this.waterAnimationTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.waterAnimationTimer <= 0)
			{
				this.waterAnimationIndex = (this.waterAnimationIndex + 1) % 10;
				this.waterAnimationTimer = 200;
			}
			if (!this.isFarm)
			{
				this.waterPosition += (float)((Math.Sin((double)((float)time.TotalGameTime.Milliseconds / 1000f)) + 1.0) * 0.15000000596046448);
			}
			else
			{
				this.waterPosition += 0.1f;
			}
			if (this.waterPosition >= (float)Game1.tileSize)
			{
				this.waterPosition -= (float)Game1.tileSize;
				this.waterTileFlip = !this.waterTileFlip;
			}
			this.map.Update((long)time.ElapsedGameTime.Milliseconds);
			for (int j = this.debris.Count - 1; j >= 0; j--)
			{
				if (this.debris[j].updateChunks(time))
				{
					this.debris.RemoveAt(j);
				}
			}
			if (Game1.shouldTimePass() || Game1.isFestival())
			{
				for (int k = this.projectiles.Count - 1; k >= 0; k--)
				{
					if (this.projectiles[k].update(time, this))
					{
						this.projectiles.RemoveAt(k);
					}
				}
			}
			foreach (KeyValuePair<Vector2, TerrainFeature> v2 in this.terrainFeatures)
			{
				if (v2.Value.tickUpdate(time, v2.Key))
				{
					this.terrainFeaturesToRemoveList.Add(v2.Key);
				}
			}
			foreach (Vector2 v3 in this.terrainFeaturesToRemoveList)
			{
				this.terrainFeatures.Remove(v3);
			}
			this.terrainFeaturesToRemoveList.Clear();
			if (this.largeTerrainFeatures != null)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator4 = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						enumerator4.Current.tickUpdate(time);
					}
				}
			}
			if (Game1.timeOfDay >= 2000 && this.lightLevel > 0f && this.name.Equals("FarmHouse"))
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)Game1.tileSize, (float)(7 * Game1.tileSize)), 2f));
			}
			if (this.currentEvent != null)
			{
				this.currentEvent.checkForNextCommand(this, time);
			}
			foreach (KeyValuePair<Vector2, Object> v4 in this.objects)
			{
				v4.Value.updateWhenCurrentLocation(time);
			}
			if (Game1.gameMode == 3)
			{
				if (this.isOutdoors && !Game1.isRaining && Game1.random.NextDouble() < 0.002 && Game1.currentSong != null && !Game1.currentSong.IsPlaying && Game1.timeOfDay < 2000 && !Game1.currentSeason.Equals("winter") && !this.name.Equals("Desert"))
				{
					Game1.playSound("SpringBirds");
				}
				else if (!Game1.isRaining && this.isOutdoors && Game1.timeOfDay > 2100 && Game1.currentSeason.Equals("summer") && Game1.random.NextDouble() < 0.0005 && !(this is Beach) && !this.name.Equals("temp"))
				{
					Game1.playSound("crickets");
				}
				else if (Game1.isRaining && this.isOutdoors && !this.name.Equals("Town") && !Game1.eventUp && Game1.options.musicVolumeLevel > 0f && Game1.random.NextDouble() < 0.00015)
				{
					Game1.playSound("rainsound");
				}
				Vector2 playerStandingPosition = new Vector2((float)(Game1.player.getStandingX() / Game1.tileSize), (float)(Game1.player.getStandingY() / Game1.tileSize));
				if (this.lastTouchActionLocation.Equals(Vector2.Zero))
				{
					string touchActionProperty = this.doesTileHaveProperty((int)playerStandingPosition.X, (int)playerStandingPosition.Y, "TouchAction", "Back");
					this.lastTouchActionLocation = new Vector2((float)(Game1.player.getStandingX() / Game1.tileSize), (float)(Game1.player.getStandingY() / Game1.tileSize));
					if (touchActionProperty != null)
					{
						this.performTouchAction(touchActionProperty, playerStandingPosition);
					}
				}
				else if (!this.lastTouchActionLocation.Equals(playerStandingPosition))
				{
					this.lastTouchActionLocation = Vector2.Zero;
					Game1.noteBlockTimer = 0f;
				}
				foreach (Vector2 v5 in Game1.player.getAdjacentTiles())
				{
					if (this.objects.ContainsKey(v5))
					{
						this.objects[v5].farmerAdjacentAction();
					}
				}
				if (Game1.boardingBus)
				{
					NPC pam = this.getCharacterFromName("Pam");
					if (pam != null && this.doesTileHaveProperty(pam.getStandingX() / Game1.tileSize, pam.getStandingY() / Game1.tileSize, "TouchAction", "Back") != null)
					{
						this.busLeave();
					}
				}
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00054018 File Offset: 0x00052218
		public NPC getCharacterFromName(string name)
		{
			NPC character = null;
			foreach (NPC i in this.characters)
			{
				if (i.name.Equals(name))
				{
					return i;
				}
			}
			return character;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0005407C File Offset: 0x0005227C
		public virtual void updateEvenIfFarmerIsntHere(GameTime time, bool ignoreWasUpdatedFlush = false)
		{
			if (!ignoreWasUpdatedFlush)
			{
				this.wasUpdated = false;
			}
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i] != null && (Game1.shouldTimePass() || this.characters[i] is Horse || this.characters[i].forceUpdateTimer > 0))
				{
					this.characters[i].update(time, this);
					if (i < this.characters.Count && this.characters[i] is Monster && ((Monster)this.characters[i]).health <= 0)
					{
						this.characters.RemoveAt(i);
					}
				}
				else if (this.characters[i] != null)
				{
					this.characters[i].updateEmote(time);
				}
			}
			for (int j = this.temporarySprites.Count - 1; j >= 0; j--)
			{
				if (this.temporarySprites[j] != null && this.temporarySprites[j].update(time) && j < this.temporarySprites.Count)
				{
					this.temporarySprites.RemoveAt(j);
				}
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000541C0 File Offset: 0x000523C0
		public Response[] createYesNoResponses()
		{
			return new Response[]
			{
				new Response("Yes", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_Yes", new object[0])),
				new Response("No", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_No", new object[0]))
			};
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00054217 File Offset: 0x00052417
		public void createQuestionDialogue(string question, Response[] answerChoices, string dialogKey)
		{
			this.lastQuestionKey = dialogKey;
			Game1.drawObjectQuestionDialogue(question, answerChoices.ToList<Response>());
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0005422C File Offset: 0x0005242C
		public void createQuestionDialogue(string question, Response[] answerChoices, GameLocation.afterQuestionBehavior afterDialogueBehavior, NPC speaker = null)
		{
			this.afterQuestion = afterDialogueBehavior;
			Game1.drawObjectQuestionDialogue(question, answerChoices.ToList<Response>());
			if (speaker != null)
			{
				Game1.objectDialoguePortraitPerson = speaker;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0005424C File Offset: 0x0005244C
		public void createQuestionDialogue(string question, Response[] answerChoices, string dialogKey, Object actionObject)
		{
			this.lastQuestionKey = dialogKey;
			Game1.drawObjectQuestionDialogue(question, answerChoices.ToList<Response>());
			this.actionObjectForQuestionDialogue = actionObject;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0005426C File Offset: 0x0005246C
		public virtual void monsterDrop(Monster monster, int x, int y)
		{
			int arg_06_0 = monster.coinsToDrop;
			List<int> objects = monster.objectsToDrop;
			Vector2 playerPosition = new Vector2((float)Game1.player.GetBoundingBox().Center.X, (float)Game1.player.GetBoundingBox().Center.Y);
			List<Item> extraDrops = monster.getExtraDropItems();
			if (Game1.player.isWearingRing(526))
			{
				string[] objectsSplit = Game1.content.Load<Dictionary<string, string>>("Data\\Monsters")[monster.name].Split(new char[]
				{
					'/'
				})[6].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < objectsSplit.Length; i += 2)
				{
					if (Game1.random.NextDouble() < Convert.ToDouble(objectsSplit[i + 1]))
					{
						objects.Add(Convert.ToInt32(objectsSplit[i]));
					}
				}
			}
			for (int j = 0; j < objects.Count; j++)
			{
				int objectToAdd = objects[j];
				if (objectToAdd < 0)
				{
					this.debris.Add(new Debris(Math.Abs(objectToAdd), Game1.random.Next(1, 4), new Vector2((float)x, (float)y), playerPosition));
				}
				else
				{
					this.debris.Add(new Debris(objectToAdd, new Vector2((float)x, (float)y), playerPosition));
				}
			}
			for (int k = 0; k < extraDrops.Count; k++)
			{
				this.debris.Add(new Debris(extraDrops[k], new Vector2((float)x, (float)y), playerPosition));
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000543F8 File Offset: 0x000525F8
		public bool damageMonster(Microsoft.Xna.Framework.Rectangle areaOfEffect, int minDamage, int maxDamage, bool isBomb, Farmer who)
		{
			return this.damageMonster(areaOfEffect, minDamage, maxDamage, isBomb, 1f, 0, 0f, 1f, false, who);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00054424 File Offset: 0x00052624
		private bool isMonsterDamageApplicable(Farmer who, Monster monster, bool horizontalBias = true)
		{
			if (!monster.isGlider && !(who.CurrentTool is Slingshot))
			{
				Point farmerStandingPoint = who.getTileLocationPoint();
				Point monsterStandingPoint = monster.getTileLocationPoint();
				if (Math.Abs(farmerStandingPoint.X - monsterStandingPoint.X) + Math.Abs(farmerStandingPoint.Y - monsterStandingPoint.Y) > 1)
				{
					int xDif = monsterStandingPoint.X - farmerStandingPoint.X;
					int yDif = monsterStandingPoint.Y - farmerStandingPoint.Y;
					Vector2 pointInQuestion = new Vector2((float)farmerStandingPoint.X, (float)farmerStandingPoint.Y);
					while (xDif != 0 || yDif != 0)
					{
						if (horizontalBias)
						{
							if (Math.Abs(xDif) >= Math.Abs(yDif))
							{
								pointInQuestion.X += (float)Math.Sign(xDif);
								xDif -= Math.Sign(xDif);
							}
							else
							{
								pointInQuestion.Y += (float)Math.Sign(yDif);
								yDif -= Math.Sign(yDif);
							}
						}
						else if (Math.Abs(yDif) >= Math.Abs(xDif))
						{
							pointInQuestion.Y += (float)Math.Sign(yDif);
							yDif -= Math.Sign(yDif);
						}
						else
						{
							pointInQuestion.X += (float)Math.Sign(xDif);
							xDif -= Math.Sign(xDif);
						}
						if (this.objects.ContainsKey(pointInQuestion) || this.getTileIndexAt((int)pointInQuestion.X, (int)pointInQuestion.Y, "Buildings") != -1)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00054588 File Offset: 0x00052788
		public bool damageMonster(Microsoft.Xna.Framework.Rectangle areaOfEffect, int minDamage, int maxDamage, bool isBomb, float knockBackModifier, int addedPrecision, float critChance, float critMultiplier, bool triggerMonsterInvincibleTimer, Farmer who)
		{
			bool didAnyDamage = false;
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i].GetBoundingBox().Intersects(areaOfEffect) && this.characters[i].IsMonster && !this.characters[i].isInvisible && !(this.characters[i] as Monster).isInvincible() && !(this.characters[i] as Monster).isInvincible() && (isBomb || this.isMonsterDamageApplicable(who, this.characters[i] as Monster, true) || this.isMonsterDamageApplicable(who, this.characters[i] as Monster, false)))
				{
					bool isDagger = who != null && who.CurrentTool != null && who.CurrentTool is MeleeWeapon && (who.CurrentTool as MeleeWeapon).type == 1;
					didAnyDamage = true;
					Rumble.rumble(0.1f + (float)(Game1.random.NextDouble() / 4.0), (float)(200 + Game1.random.Next(-50, 50)));
					Microsoft.Xna.Framework.Rectangle monsterBox = this.characters[i].GetBoundingBox();
					Vector2 trajectory = Utility.getAwayFromPlayerTrajectory(monsterBox, who);
					if (knockBackModifier > 0f)
					{
						trajectory *= knockBackModifier;
					}
					else
					{
						trajectory = new Vector2(this.characters[i].xVelocity, this.characters[i].yVelocity);
					}
					if ((this.characters[i] as Monster).slipperiness == -1)
					{
						trajectory = Vector2.Zero;
					}
					bool crit = false;
					if (who != null && who.CurrentTool != null && this.characters[i].hitWithTool(who.CurrentTool))
					{
						return false;
					}
					if (who.professions.Contains(25))
					{
						critChance += critChance * 0.5f;
					}
					int damageAmount;
					if (maxDamage >= 0)
					{
						damageAmount = Game1.random.Next(minDamage, maxDamage + 1);
						if (who != null && Game1.random.NextDouble() < (double)(critChance + (float)who.LuckLevel * (critChance / 40f)))
						{
							crit = true;
							Game1.playSound("crit");
						}
						damageAmount = (crit ? ((int)((float)damageAmount * critMultiplier)) : damageAmount);
						damageAmount = Math.Max(1, damageAmount + ((who != null) ? (who.attack * 3) : 0));
						if (who != null && who.professions.Contains(24))
						{
							damageAmount = (int)Math.Ceiling((double)((float)damageAmount * 1.1f));
						}
						if (who != null && who.professions.Contains(26))
						{
							damageAmount = (int)Math.Ceiling((double)((float)damageAmount * 1.15f));
						}
						if ((who != null & crit) && who.professions.Contains(29))
						{
							damageAmount *= 3;
						}
						damageAmount = ((Monster)this.characters[i]).takeDamage(damageAmount, (int)trajectory.X, (int)trajectory.Y, isBomb, (double)addedPrecision / 10.0);
						if (damageAmount == -1)
						{
							this.debris.Add(new Debris("Miss", 1, new Vector2((float)monsterBox.Center.X, (float)monsterBox.Center.Y), Color.LightGray, 1f, 0f));
						}
						else
						{
							for (int j = this.debris.Count - 1; j >= 0; j--)
							{
								if (this.debris[j].toHover != null && this.debris[j].toHover.Equals(this.characters[i]) && !this.debris[j].nonSpriteChunkColor.Equals(Color.Yellow) && this.debris[j].timeSinceDoneBouncing > 900f)
								{
									this.debris.RemoveAt(j);
								}
							}
							this.debris.Add(new Debris(damageAmount, new Vector2((float)(monsterBox.Center.X + Game1.pixelZoom * 4), (float)monsterBox.Center.Y), crit ? Color.Yellow : new Color(255, 130, 0), crit ? (1f + (float)damageAmount / 300f) : 1f, this.characters[i]));
						}
						if (triggerMonsterInvincibleTimer)
						{
							(this.characters[i] as Monster).setInvincibleCountdown(450 / (isDagger ? 3 : 2));
						}
					}
					else
					{
						damageAmount = -2;
						this.characters[i].setTrajectory(trajectory);
						if (((Monster)this.characters[i]).slipperiness > 10)
						{
							this.characters[i].xVelocity /= 2f;
							this.characters[i].yVelocity /= 2f;
						}
					}
					if (who != null && who.CurrentTool != null && who.CurrentTool.Name.Equals("Galaxy Sword"))
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(50, 120), 6, 1, new Vector2((float)(monsterBox.Center.X - Game1.tileSize / 2), (float)(monsterBox.Center.Y - Game1.tileSize / 2)), false, false));
					}
					if (((Monster)this.characters[i]).health <= 0)
					{
						if (!this.isFarm)
						{
							who.checkForQuestComplete(null, 1, 1, null, this.characters[i].name, 4, -1);
						}
						if (who != null && who.leftRing != null)
						{
							who.leftRing.onMonsterSlay((Monster)this.characters[i]);
						}
						if (who != null && who.rightRing != null)
						{
							who.rightRing.onMonsterSlay((Monster)this.characters[i]);
						}
						if (who != null && who.IsMainPlayer && !this.isFarm && (!(this.characters[i] is GreenSlime) || (this.characters[i] as GreenSlime).firstGeneration))
						{
							Game1.stats.monsterKilled(this.characters[i].name);
						}
						this.monsterDrop((Monster)this.characters[i], monsterBox.Center.X, monsterBox.Center.Y);
						if (who != null && !this.isFarm)
						{
							who.gainExperience(4, ((Monster)this.characters[i]).experienceGained);
						}
						this.characters.RemoveAt(i);
						Stats expr_6F8 = Game1.stats;
						uint monstersKilled = expr_6F8.MonstersKilled;
						expr_6F8.MonstersKilled = monstersKilled + 1u;
					}
					else if (damageAmount > 0)
					{
						((Monster)this.characters[i]).shedChunks(Game1.random.Next(1, 3));
						if (crit)
						{
							this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(15, 50), 6, 1, this.characters[i].getStandingPosition() - new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
							{
								scale = 0.75f,
								alpha = (crit ? 0.75f : 0.5f)
							});
							this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(15, 50), 6, 1, this.characters[i].getStandingPosition() - new Vector2((float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) + Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5)
							{
								scale = 0.5f,
								delayBeforeAnimationStart = 50,
								alpha = (crit ? 0.75f : 0.5f)
							});
							this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(15, 50), 6, 1, this.characters[i].getStandingPosition() - new Vector2((float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) - Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5)
							{
								scale = 0.5f,
								delayBeforeAnimationStart = 100,
								alpha = (crit ? 0.75f : 0.5f)
							});
							this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(15, 50), 6, 1, this.characters[i].getStandingPosition() - new Vector2((float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) + Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5)
							{
								scale = 0.5f,
								delayBeforeAnimationStart = 150,
								alpha = (crit ? 0.75f : 0.5f)
							});
							this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(15, 50), 6, 1, this.characters[i].getStandingPosition() - new Vector2((float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) - Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5)
							{
								scale = 0.5f,
								delayBeforeAnimationStart = 200,
								alpha = (crit ? 0.75f : 0.5f)
							});
						}
					}
					if (damageAmount > 0 && who != null && damageAmount > 1 && Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Dark Sword") && Game1.random.NextDouble() < 0.08)
					{
						who.health = Math.Min(who.maxHealth, Game1.player.health + damageAmount / 2);
						this.debris.Add(new Debris(damageAmount / 2, new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()), Color.Lime, 1f, who));
						Game1.playSound("healSound");
					}
				}
			}
			return didAnyDamage;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00055170 File Offset: 0x00053370
		public void moveCharacters(GameTime time)
		{
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (!this.characters[i].isInvisible)
				{
					this.characters[i].updateMovement(this, time);
				}
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000551BC File Offset: 0x000533BC
		public List<Farmer> getFarmers()
		{
			List<Farmer> farmerList = new List<Farmer>(this.farmers);
			if (Game1.player.currentLocation != null && Game1.player.currentLocation.Equals(this))
			{
				farmerList.Add(Game1.player);
			}
			return farmerList;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00055200 File Offset: 0x00053400
		public void growWeedGrass(int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				for (int j = this.terrainFeatures.Count - 1; j >= 0; j--)
				{
					KeyValuePair<Vector2, TerrainFeature> kvp = this.terrainFeatures.ElementAt(j);
					if (kvp.Value.GetType() == typeof(Grass) && Game1.random.NextDouble() < 0.65)
					{
						if (((Grass)kvp.Value).numberOfWeeds < 4)
						{
							((Grass)kvp.Value).numberOfWeeds = (int)((byte)Math.Min(4, ((Grass)kvp.Value).numberOfWeeds + (int)((byte)Game1.random.Next(3))));
						}
						else if (((Grass)kvp.Value).numberOfWeeds >= 4)
						{
							int xCoord = (int)kvp.Key.X;
							int yCoord = (int)kvp.Key.Y;
							if (this.isTileOnMap(xCoord, yCoord) && !this.isTileOccupied(kvp.Key + new Vector2(-1f, 0f), "") && this.isTileLocationOpenIgnoreFrontLayers(new Location((xCoord - 1) * Game1.tileSize, yCoord * Game1.tileSize)) && this.doesTileHaveProperty(xCoord - 1, yCoord, "Diggable", "Back") != null && Game1.random.NextDouble() < 0.25)
							{
								this.terrainFeatures.Add(kvp.Key + new Vector2(-1f, 0f), new Grass((int)((Grass)kvp.Value).grassType, Game1.random.Next(1, 3)));
							}
							if (this.isTileOnMap(xCoord, yCoord) && !this.isTileOccupied(kvp.Key + new Vector2(1f, 0f), "") && this.isTileLocationOpenIgnoreFrontLayers(new Location((xCoord + 1) * Game1.tileSize, yCoord * Game1.tileSize)) && this.doesTileHaveProperty(xCoord + 1, yCoord, "Diggable", "Back") != null && Game1.random.NextDouble() < 0.25)
							{
								this.terrainFeatures.Add(kvp.Key + new Vector2(1f, 0f), new Grass((int)((Grass)kvp.Value).grassType, Game1.random.Next(1, 3)));
							}
							if (this.isTileOnMap(xCoord, yCoord) && !this.isTileOccupied(kvp.Key + new Vector2(0f, 1f), "") && this.isTileLocationOpenIgnoreFrontLayers(new Location(xCoord * Game1.tileSize, (yCoord + 1) * Game1.tileSize)) && this.doesTileHaveProperty(xCoord, yCoord + 1, "Diggable", "Back") != null && Game1.random.NextDouble() < 0.25)
							{
								this.terrainFeatures.Add(kvp.Key + new Vector2(0f, 1f), new Grass((int)((Grass)kvp.Value).grassType, Game1.random.Next(1, 3)));
							}
							if (this.isTileOnMap(xCoord, yCoord) && !this.isTileOccupied(kvp.Key + new Vector2(0f, -1f), "") && this.isTileLocationOpenIgnoreFrontLayers(new Location(xCoord * Game1.tileSize, (yCoord - 1) * Game1.tileSize)) && this.doesTileHaveProperty(xCoord, yCoord - 1, "Diggable", "Back") != null && Game1.random.NextDouble() < 0.25)
							{
								this.terrainFeatures.Add(kvp.Key + new Vector2(0f, -1f), new Grass((int)((Grass)kvp.Value).grassType, Game1.random.Next(1, 3)));
							}
						}
					}
				}
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00055634 File Offset: 0x00053834
		public void spawnWeeds(bool weedsOnly)
		{
			int numberOfNewWeeds = Game1.random.Next(this.isFarm ? 5 : 2, this.isFarm ? 12 : 6);
			if (Game1.dayOfMonth == 1 && Game1.currentSeason.Equals("spring"))
			{
				numberOfNewWeeds *= 15;
			}
			if (this.name.Equals("Desert"))
			{
				numberOfNewWeeds = ((Game1.random.NextDouble() < 0.1) ? 1 : 0);
			}
			for (int i = 0; i < numberOfNewWeeds; i++)
			{
				for (int numberOfTries = 0; numberOfTries < 3; numberOfTries++)
				{
					int xCoord = Game1.random.Next(this.map.DisplayWidth / Game1.tileSize);
					int yCoord = Game1.random.Next(this.map.DisplayHeight / Game1.tileSize);
					Vector2 location = new Vector2((float)xCoord, (float)yCoord);
					Object o;
					this.objects.TryGetValue(location, out o);
					int grass = -1;
					int tree = -1;
					if (this.name.Equals("Desert"))
					{
						if (Game1.random.NextDouble() < 0.5)
						{
						}
					}
					else if (Game1.random.NextDouble() < 0.15 + (weedsOnly ? 0.05 : 0.0))
					{
						grass = 1;
					}
					else if (!weedsOnly && Game1.random.NextDouble() < 0.35)
					{
						tree = 1;
					}
					else if (!weedsOnly && !this.isFarm && Game1.random.NextDouble() < 0.35)
					{
						tree = 2;
					}
					if (tree != -1)
					{
						if (this is Farm && Game1.random.NextDouble() < 0.25)
						{
							return;
						}
					}
					else if (o == null && this.doesTileHaveProperty(xCoord, yCoord, "Diggable", "Back") != null && this.isTileLocationOpen(new Location(xCoord * Game1.tileSize, yCoord * Game1.tileSize)) && !this.isTileOccupied(location, "") && this.doesTileHaveProperty(xCoord, yCoord, "Water", "Back") == null)
					{
						string noSpawn = this.doesTileHaveProperty(xCoord, yCoord, "NoSpawn", "Back");
						if (noSpawn != null && (noSpawn.Equals("Grass") || noSpawn.Equals("All") || noSpawn.Equals("True")))
						{
							continue;
						}
						if (grass != -1 && !Game1.currentSeason.Equals("winter") && this.name.Equals("Farm"))
						{
							int numberOfWeeds = Game1.random.Next(1, 3);
							this.terrainFeatures.Add(location, new Grass(grass, numberOfWeeds));
						}
					}
				}
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000558EC File Offset: 0x00053AEC
		public bool addCharacterAtRandomLocation(NPC n)
		{
			Vector2 tileLocationAttempt = new Vector2((float)Game1.random.Next(0, this.map.GetLayer("Back").LayerWidth), (float)Game1.random.Next(0, this.map.GetLayer("Back").LayerHeight));
			int attempts = 0;
			while (attempts < 6 && (this.isTileOccupied(tileLocationAttempt, "") || !this.isTilePassable(new Location((int)tileLocationAttempt.X, (int)tileLocationAttempt.Y), Game1.viewport) || this.map.GetLayer("Back").Tiles[(int)tileLocationAttempt.X, (int)tileLocationAttempt.Y] == null || this.map.GetLayer("Back").Tiles[(int)tileLocationAttempt.X, (int)tileLocationAttempt.Y].Properties.ContainsKey("NPCBarrier")))
			{
				tileLocationAttempt = new Vector2((float)Game1.random.Next(0, this.map.GetLayer("Back").LayerWidth), (float)Game1.random.Next(0, this.map.GetLayer("Back").LayerHeight));
				attempts++;
			}
			if (attempts < 6)
			{
				n.position = tileLocationAttempt * new Vector2((float)Game1.tileSize, (float)Game1.tileSize) - new Vector2(0f, (float)(n.sprite.spriteHeight - Game1.tileSize));
				this.addCharacter(n);
				return true;
			}
			return false;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00055A80 File Offset: 0x00053C80
		public virtual void DayUpdate(int dayOfMonth)
		{
			new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			this.temporarySprites.Clear();
			for (int i = this.terrainFeatures.Count - 1; i >= 0; i--)
			{
				if (!this.isTileOnMap(this.terrainFeatures.ElementAt(i).Key))
				{
					this.terrainFeatures.Remove(this.terrainFeatures.ElementAt(i).Key);
				}
				else
				{
					this.terrainFeatures.ElementAt(i).Value.dayUpdate(this, this.terrainFeatures.ElementAt(i).Key);
				}
			}
			if (this.largeTerrainFeatures != null)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.dayUpdate(this);
					}
				}
			}
			using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator2 = this.objects.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.DayUpdate(this);
				}
			}
			if (!(this is FarmHouse))
			{
				this.debris.Clear();
			}
			if (this.isOutdoors)
			{
				if (Game1.dayOfMonth % 7 == 0 && !(this is Farm))
				{
					for (int j = this.objects.Count - 1; j >= 0; j--)
					{
						if (this.objects.ElementAt(j).Value.isSpawnedObject)
						{
							this.objects.Remove(this.objects.ElementAt(j).Key);
						}
					}
					this.numberOfSpawnedObjectsOnMap = 0;
					this.spawnObjects();
					this.spawnObjects();
				}
				this.spawnObjects();
				if (Game1.dayOfMonth == 1)
				{
					this.spawnObjects();
				}
				if (Game1.stats.DaysPlayed < 4u)
				{
					this.spawnObjects();
				}
				bool hasPathsLayer = false;
				using (IEnumerator<Layer> enumerator3 = this.map.Layers.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.Id.Equals("Paths"))
						{
							hasPathsLayer = true;
							break;
						}
					}
				}
				if (hasPathsLayer && !(this is Farm))
				{
					for (int x = 0; x < this.map.Layers[0].LayerWidth; x++)
					{
						for (int y = 0; y < this.map.Layers[0].LayerHeight; y++)
						{
							if (this.map.GetLayer("Paths").Tiles[x, y] != null && Game1.random.NextDouble() < 0.5)
							{
								Vector2 tile = new Vector2((float)x, (float)y);
								int treeType = -1;
								switch (this.map.GetLayer("Paths").Tiles[x, y].TileIndex)
								{
								case 9:
									treeType = 1;
									if (Game1.currentSeason.Equals("winter"))
									{
										treeType += 3;
									}
									break;
								case 10:
									treeType = 2;
									if (Game1.currentSeason.Equals("winter"))
									{
										treeType += 3;
									}
									break;
								case 11:
									treeType = 3;
									break;
								case 12:
									treeType = 6;
									break;
								}
								if (treeType != -1 && !this.terrainFeatures.ContainsKey(tile) && !this.objects.ContainsKey(tile))
								{
									this.terrainFeatures.Add(tile, new Tree(treeType, 2));
								}
							}
						}
					}
				}
			}
			if (!this.isFarm)
			{
				Dictionary<Vector2, TerrainFeature>.KeyCollection objKeys = this.terrainFeatures.Keys;
				for (int k = objKeys.Count - 1; k >= 0; k--)
				{
					if (this.terrainFeatures[objKeys.ElementAt(k)] is HoeDirt && ((this.terrainFeatures[objKeys.ElementAt(k)] as HoeDirt).crop == null || (this.terrainFeatures[objKeys.ElementAt(k)] as HoeDirt).crop.forageCrop))
					{
						this.terrainFeatures.Remove(objKeys.ElementAt(k));
					}
				}
			}
			for (int l = this.characters.Count - 1; l >= 0; l--)
			{
			}
			this.lightLevel = 0f;
			if (this.name.Equals("BugLand"))
			{
				for (int x2 = 0; x2 < this.map.Layers[0].LayerWidth; x2++)
				{
					for (int y2 = 0; y2 < this.map.Layers[0].LayerHeight; y2++)
					{
						if (Game1.random.NextDouble() < 0.33)
						{
							Tile t = this.map.GetLayer("Paths").Tiles[x2, y2];
							if (t != null)
							{
								Vector2 tile2 = new Vector2((float)x2, (float)y2);
								int tileIndex = t.TileIndex;
								switch (tileIndex)
								{
								case 13:
								case 14:
								case 15:
									if (!this.objects.ContainsKey(tile2))
									{
										this.objects.Add(tile2, new Object(tile2, GameLocation.getWeedForSeason(Game1.random, "spring"), 1));
									}
									break;
								case 16:
									if (!this.objects.ContainsKey(tile2))
									{
										this.objects.Add(tile2, new Object(tile2, (Game1.random.NextDouble() < 0.5) ? 343 : 450, 1));
									}
									break;
								case 17:
									if (!this.objects.ContainsKey(tile2))
									{
										this.objects.Add(tile2, new Object(tile2, (Game1.random.NextDouble() < 0.5) ? 343 : 450, 1));
									}
									break;
								case 18:
									if (!this.objects.ContainsKey(tile2))
									{
										this.objects.Add(tile2, new Object(tile2, (Game1.random.NextDouble() < 0.5) ? 294 : 295, 1));
									}
									break;
								default:
									if (tileIndex == 28)
									{
										if (this.isTileLocationTotallyClearAndPlaceable(tile2) && this.characters.Count < 50)
										{
											this.characters.Add(new Grub(new Vector2(tile2.X * (float)Game1.tileSize, tile2.Y * (float)Game1.tileSize), true));
										}
									}
									break;
								}
							}
						}
					}
				}
			}
			this.addLightGlows();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00056178 File Offset: 0x00054378
		public void addLightGlows()
		{
			if (!this.isOutdoors && (Game1.timeOfDay < 1900 || Game1.newDay))
			{
				this.lightGlows.Clear();
				PropertyValue dayTiles;
				this.map.Properties.TryGetValue("DayTiles", out dayTiles);
				if (dayTiles != null)
				{
					string[] split = dayTiles.ToString().Trim().Split(new char[]
					{
						' '
					});
					for (int i = 0; i < split.Length; i += 4)
					{
						if (this.map.GetLayer(split[i]).PickTile(new Location(Convert.ToInt32(split[i + 1]) * Game1.tileSize, Convert.ToInt32(split[i + 2]) * Game1.tileSize), new Size(Game1.viewport.Width, Game1.viewport.Height)) != null)
						{
							this.map.GetLayer(split[i]).Tiles[Convert.ToInt32(split[i + 1]), Convert.ToInt32(split[i + 2])].TileIndex = Convert.ToInt32(split[i + 3]);
							int num = Convert.ToInt32(split[i + 3]);
							if (num <= 257)
							{
								if (num != 256)
								{
									if (num == 257)
									{
										this.lightGlows.Add(new Vector2((float)Convert.ToInt32(split[i + 1]), (float)Convert.ToInt32(split[i + 2])) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.pixelZoom)));
									}
								}
								else
								{
									this.lightGlows.Add(new Vector2((float)Convert.ToInt32(split[i + 1]), (float)Convert.ToInt32(split[i + 2])) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize));
								}
							}
							else if (num != 405)
							{
								if (num != 469)
								{
									if (num == 1224)
									{
										this.lightGlows.Add(new Vector2((float)Convert.ToInt32(split[i + 1]), (float)Convert.ToInt32(split[i + 2])) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
									}
								}
								else
								{
									this.lightGlows.Add(new Vector2((float)Convert.ToInt32(split[i + 1]), (float)Convert.ToInt32(split[i + 2])) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2 + Game1.pixelZoom)));
								}
							}
							else
							{
								this.lightGlows.Add(new Vector2((float)Convert.ToInt32(split[i + 1]), (float)Convert.ToInt32(split[i + 2])) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
								this.lightGlows.Add(new Vector2((float)Convert.ToInt32(split[i + 1]), (float)Convert.ToInt32(split[i + 2])) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 3 / 2), (float)(Game1.tileSize / 2)));
							}
						}
					}
				}
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000564BC File Offset: 0x000546BC
		public NPC isCharacterAtTile(Vector2 tileLocation)
		{
			NPC c = null;
			tileLocation.X = (float)((int)tileLocation.X);
			tileLocation.Y = (float)((int)tileLocation.Y);
			if (this.currentEvent == null)
			{
				using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC i = enumerator.Current;
						if (i.getTileLocation().Equals(tileLocation))
						{
							NPC result = i;
							return result;
						}
					}
					return c;
				}
			}
			foreach (NPC j in this.currentEvent.actors)
			{
				if (j.getTileLocation().Equals(tileLocation))
				{
					NPC result = j;
					return result;
				}
			}
			return c;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000565A4 File Offset: 0x000547A4
		public void UpdateCharacterDialogues()
		{
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				this.characters[i].updateDialogue();
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000565DC File Offset: 0x000547DC
		public string getMapProperty(string propertyName)
		{
			PropertyValue value = null;
			this.map.Properties.TryGetValue(propertyName, out value);
			return value.ToString();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00056608 File Offset: 0x00054808
		public void tryToAddCritters(bool onlyIfOnScreen = false)
		{
			if (Game1.CurrentEvent != null)
			{
				return;
			}
			double mapArea = (double)(this.map.Layers[0].LayerWidth * this.map.Layers[0].LayerHeight);
			double birdieChance;
			double butterflyChance;
			double expr_5E = butterflyChance = (birdieChance = Math.Max(0.15, Math.Min(0.5, mapArea / 15000.0)));
			double bunnyChance = expr_5E / 2.0;
			double squirrelChance = expr_5E / 2.0;
			double woodPeckerChance = expr_5E / 8.0;
			double cloudChange = expr_5E * 2.0;
			if (!Game1.isRaining)
			{
				this.addClouds(cloudChange / (double)(onlyIfOnScreen ? 2f : 1f), onlyIfOnScreen);
				if (this is Beach || this.critters == null)
				{
					return;
				}
				if (this.critters.Count > (Game1.currentSeason.Equals("summer") ? 20 : 10))
				{
					return;
				}
				this.addBirdies(birdieChance, onlyIfOnScreen);
				this.addButterflies(butterflyChance, onlyIfOnScreen);
				this.addBunnies(bunnyChance, onlyIfOnScreen);
				this.addSquirrels(squirrelChance, onlyIfOnScreen);
				this.addWoodpecker(woodPeckerChance, onlyIfOnScreen);
				if (Game1.isDarkOut() && Game1.random.NextDouble() < 0.01)
				{
					this.addOwl();
				}
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00056750 File Offset: 0x00054950
		public void addClouds(double chance, bool onlyIfOnScreen = false)
		{
			if (Game1.currentSeason.Equals("summer") && !Game1.isRaining && Game1.weatherIcon != 4 && Game1.timeOfDay < Game1.getStartingToGetDarkTime() - 100)
			{
				while (Game1.random.NextDouble() < Math.Min(0.9, chance))
				{
					Vector2 v = this.getRandomTile();
					if (onlyIfOnScreen)
					{
						v = ((Game1.random.NextDouble() < 0.5) ? new Vector2((float)this.map.Layers[0].LayerWidth, (float)Game1.random.Next(this.map.Layers[0].LayerHeight)) : new Vector2((float)Game1.random.Next(this.map.Layers[0].LayerWidth), (float)this.map.Layers[0].LayerHeight));
					}
					if (onlyIfOnScreen || !Utility.isOnScreen(v * (float)Game1.tileSize, Game1.tileSize * 20))
					{
						Cloud cloud = new Cloud(v);
						bool freeToAdd = true;
						if (this.critters != null)
						{
							foreach (Critter c in this.critters)
							{
								if (c is Cloud && c.getBoundingBox(0, 0).Intersects(cloud.getBoundingBox(0, 0)))
								{
									freeToAdd = false;
									break;
								}
							}
						}
						if (freeToAdd)
						{
							Game1.debugOutput = string.Concat(new object[]
							{
								"added CLOUD at ",
								v.X,
								",",
								v.Y
							});
							this.addCritter(cloud);
						}
					}
				}
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0005693C File Offset: 0x00054B3C
		public void addOwl()
		{
			this.critters.Add(new Owl(new Vector2((float)Game1.random.Next(Game1.tileSize, this.map.Layers[0].LayerWidth * Game1.tileSize - Game1.tileSize), (float)(-(float)Game1.tileSize * 2))));
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0005699C File Offset: 0x00054B9C
		public void setFireplace(bool on, int tileLocationX, int tileLocationY, bool playSound = true)
		{
			int fireid = GameLocation.fireIDBase + tileLocationX * 1000 + tileLocationY;
			if (on)
			{
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), new Vector2((float)tileLocationX, (float)tileLocationY) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)), false, 0f, Color.White)
				{
					interval = 50f,
					totalNumberOfLoops = 99999,
					animationLength = 4,
					light = true,
					lightID = fireid,
					id = (float)fireid,
					lightRadius = 2f,
					scale = (float)Game1.pixelZoom,
					layerDepth = ((float)tileLocationY + 1.1f) * (float)Game1.tileSize / 10000f
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), new Vector2((float)(tileLocationX + 1), (float)tileLocationY) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 2)), false, 0f, Color.White)
				{
					delayBeforeAnimationStart = 10,
					interval = 50f,
					totalNumberOfLoops = 99999,
					animationLength = 4,
					light = true,
					lightID = fireid,
					id = (float)fireid,
					lightRadius = 2f,
					scale = (float)Game1.pixelZoom,
					layerDepth = ((float)tileLocationY + 1.1f) * (float)Game1.tileSize / 10000f
				});
				if (playSound)
				{
					Game1.playSound("fireball");
				}
				AmbientLocationSounds.addSound(new Vector2((float)tileLocationX, (float)tileLocationY), 1);
				return;
			}
			this.removeTemporarySpritesWithID(fireid);
			Utility.removeLightSource(fireid);
			if (playSound)
			{
				Game1.playSound("fireball");
			}
			AmbientLocationSounds.removeSound(new Vector2((float)tileLocationX, (float)tileLocationY));
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00056BA0 File Offset: 0x00054DA0
		public void addWoodpecker(double chance, bool onlyIfOnScreen = false)
		{
			if (Game1.isStartingToGetDarkOut())
			{
				return;
			}
			if (onlyIfOnScreen || this is Town || this is Desert)
			{
				return;
			}
			if (Game1.random.NextDouble() < chance)
			{
				for (int i = 0; i < 3; i++)
				{
					int index = Game1.random.Next(this.terrainFeatures.Count);
					if (this.terrainFeatures.Count > 0 && this.terrainFeatures.ElementAt(index).Value is Tree && (this.terrainFeatures.ElementAt(index).Value as Tree).treeType != 2 && (this.terrainFeatures.ElementAt(index).Value as Tree).growthStage >= 5)
					{
						this.critters.Add(new Woodpecker(this.terrainFeatures.ElementAt(index).Value as Tree, this.terrainFeatures.ElementAt(index).Key));
						return;
					}
				}
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00056CB0 File Offset: 0x00054EB0
		public void addSquirrels(double chance, bool onlyIfOnScreen = false)
		{
			if (Game1.isStartingToGetDarkOut())
			{
				return;
			}
			if (onlyIfOnScreen || this is Farm || this is Town || this is Desert)
			{
				return;
			}
			if (Game1.random.NextDouble() < chance)
			{
				for (int i = 0; i < 3; i++)
				{
					int index = Game1.random.Next(this.terrainFeatures.Count);
					if (this.terrainFeatures.Count > 0 && this.terrainFeatures.ElementAt(index).Value is Tree && (this.terrainFeatures.ElementAt(index).Value as Tree).growthStage >= 5 && !(this.terrainFeatures.ElementAt(index).Value as Tree).stump)
					{
						Vector2 pos = this.terrainFeatures.ElementAt(index).Key;
						int distance = Game1.random.Next(4, 7);
						bool flip = Game1.random.NextDouble() < 0.5;
						bool success = true;
						for (int j = 0; j < distance; j++)
						{
							pos.X += (float)(flip ? 1 : -1);
							if (!this.isTileLocationTotallyClearAndPlaceable(pos))
							{
								success = false;
								break;
							}
						}
						if (success)
						{
							this.critters.Add(new Squirrel(pos, flip));
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00056E18 File Offset: 0x00055018
		public void addBunnies(double chance, bool onlyIfOnScreen = false)
		{
			if (onlyIfOnScreen || this is Farm || this is Desert)
			{
				return;
			}
			if (Game1.random.NextDouble() < chance && this.largeTerrainFeatures != null)
			{
				for (int i = 0; i < 3; i++)
				{
					int index = Game1.random.Next(this.largeTerrainFeatures.Count);
					if (this.largeTerrainFeatures.Count > 0 && this.largeTerrainFeatures[index] is Bush)
					{
						Vector2 pos = this.largeTerrainFeatures[index].tilePosition;
						int distance = Game1.random.Next(5, 12);
						bool flip = Game1.random.NextDouble() < 0.5;
						bool success = true;
						for (int j = 0; j < distance; j++)
						{
							pos.X += (float)(flip ? 1 : -1);
							if (!this.largeTerrainFeatures[index].getBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle((int)pos.X * Game1.tileSize, (int)pos.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)) && !this.isTileLocationTotallyClearAndPlaceable(pos))
							{
								success = false;
								break;
							}
						}
						if (success)
						{
							this.critters.Add(new Rabbit(pos, flip));
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00056F6C File Offset: 0x0005516C
		public void addCritter(Critter c)
		{
			if (this.critters != null)
			{
				this.critters.Add(c);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00056F84 File Offset: 0x00055184
		public void addButterflies(double chance, bool onlyIfOnScreen = false)
		{
			bool firefly = Game1.currentSeason.Equals("summer") && Game1.isDarkOut();
			if (Game1.timeOfDay >= 1500 && !firefly)
			{
				return;
			}
			if (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("summer"))
			{
				chance = Math.Min(0.8, chance * 1.5);
				while (Game1.random.NextDouble() < chance)
				{
					Vector2 v = this.getRandomTile();
					if (!onlyIfOnScreen || !Utility.isOnScreen(v * (float)Game1.tileSize, Game1.tileSize))
					{
						if (firefly)
						{
							this.critters.Add(new Firefly(v));
						}
						else
						{
							this.critters.Add(new Butterfly(v));
						}
						while (Game1.random.NextDouble() < 0.4)
						{
							if (firefly)
							{
								this.critters.Add(new Firefly(v + new Vector2((float)Game1.random.Next(-2, 3), (float)Game1.random.Next(-2, 3))));
							}
							else
							{
								this.critters.Add(new Butterfly(v + new Vector2((float)Game1.random.Next(-2, 3), (float)Game1.random.Next(-2, 3))));
							}
						}
					}
				}
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000570EC File Offset: 0x000552EC
		public void addBirdies(double chance, bool onlyIfOnScreen = false)
		{
			if (Game1.timeOfDay >= 1500)
			{
				return;
			}
			if (this is Desert || this is Railroad)
			{
				return;
			}
			if (!(this is Farm) && !Game1.currentSeason.Equals("summer"))
			{
				while (Game1.random.NextDouble() < chance)
				{
					int birdiesToAdd = Game1.random.Next(1, 4);
					bool success = false;
					int tries = 0;
					while (!success && tries < 5)
					{
						Vector2 randomTile = this.getRandomTile();
						if (!onlyIfOnScreen || !Utility.isOnScreen(randomTile * (float)Game1.tileSize, Game1.tileSize))
						{
							Microsoft.Xna.Framework.Rectangle area = new Microsoft.Xna.Framework.Rectangle((int)randomTile.X - 2, (int)randomTile.Y - 2, 5, 5);
							if (this.isAreaClear(area))
							{
								List<Critter> crittersToAdd = new List<Critter>();
								for (int i = 0; i < birdiesToAdd; i++)
								{
									crittersToAdd.Add(new Birdie(-100, -100, Game1.currentSeason.Equals("fall") ? 45 : 25));
								}
								this.addCrittersStartingAtTile(randomTile, crittersToAdd);
								success = true;
							}
						}
						tries++;
					}
				}
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000571FF File Offset: 0x000553FF
		public void addJumperFrog(Vector2 tileLocation)
		{
			if (this.critters != null)
			{
				this.critters.Add(new Frog(tileLocation, false, false));
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0005721C File Offset: 0x0005541C
		public void addFrog()
		{
			if (Game1.isRaining && !Game1.currentSeason.Equals("winter"))
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 v = this.getRandomTile();
					if (this.doesTileHaveProperty((int)v.X, (int)v.Y, "Water", "Back") != null)
					{
						int distanceToCheck = 10;
						bool flip = Game1.random.NextDouble() < 0.5;
						for (int j = 0; j < distanceToCheck; j++)
						{
							v.X += (float)(flip ? 1 : -1);
							if (this.doesTileHaveProperty((int)v.X, (int)v.Y, "Water", "Back") == null && this.isTileOnMap((int)v.X, (int)v.Y))
							{
								this.critters.Add(new Frog(v, true, flip));
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00057309 File Offset: 0x00055509
		public void checkForSpecialCharacterIconAtThisTile(Vector2 tileLocation)
		{
			if (this.currentEvent != null)
			{
				this.currentEvent.checkForSpecialCharacterIconAtThisTile(tileLocation);
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00057320 File Offset: 0x00055520
		private void addCrittersStartingAtTile(Vector2 tile, List<Critter> crittersToAdd)
		{
			if (crittersToAdd == null)
			{
				return;
			}
			int tries = 0;
			while (crittersToAdd.Count > 0 && tries < 20)
			{
				if (this.isTileLocationTotallyClearAndPlaceable(tile))
				{
					crittersToAdd.Last<Critter>().position = tile * (float)Game1.tileSize;
					crittersToAdd.Last<Critter>().startingPosition = tile * (float)Game1.tileSize;
					this.critters.Add(crittersToAdd.Last<Critter>());
					crittersToAdd.RemoveAt(crittersToAdd.Count - 1);
					tile = Utility.getTranslatedVector2(tile, Game1.random.Next(4), 1f);
				}
				tries++;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000573B8 File Offset: 0x000555B8
		public bool isAreaClear(Microsoft.Xna.Framework.Rectangle area)
		{
			for (int x = area.Left; x < area.Right; x++)
			{
				for (int y = area.Top; y < area.Bottom; y++)
				{
					if (!this.isTileLocationTotallyClearAndPlaceable(new Vector2((float)x, (float)y)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0005740C File Offset: 0x0005560C
		public Vector2 getRandomTile()
		{
			return new Vector2((float)Game1.random.Next(this.map.Layers[0].LayerWidth), (float)Game1.random.Next(this.map.Layers[0].LayerHeight));
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00057460 File Offset: 0x00055660
		public void setUpLocationSpecificFlair()
		{
			if (!this.isOutdoors && !(this is FarmHouse))
			{
				PropertyValue ambientLight;
				this.map.Properties.TryGetValue("AmbientLight", out ambientLight);
				if (ambientLight == null)
				{
					Game1.ambientLight = new Color(100, 120, 30);
				}
			}
			string text = this.name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1840909614u)
			{
				if (num <= 807500499u)
				{
					if (num <= 636013712u)
					{
						if (num != 524243468u)
						{
							if (num != 636013712u)
							{
								return;
							}
							if (!(text == "HaleyHouse"))
							{
								return;
							}
						}
						else
						{
							if (!(text == "BugLand"))
							{
								return;
							}
							if (!Game1.player.hasDarkTalisman && this.isTileLocationTotallyClearAndPlaceable(31, 5))
							{
								this.objects.Add(new Vector2(31f, 5f), new Chest(0, new List<Item>
								{
									new SpecialItem(1, 6, "")
								}, new Vector2(31f, 5f), false)
								{
									tint = Color.Gray
								});
							}
							using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									NPC i = enumerator.Current;
									if (i is Grub)
									{
										(i as Grub).setHard();
									}
									else if (i is Fly)
									{
										(i as Fly).setHard();
									}
								}
								return;
							}
						}
						if (Game1.player.eventsSeen.Contains(463391) && (Game1.player.spouse == null || !Game1.player.spouse.Equals("Emily")))
						{
							this.setMapTileIndex(14, 4, 2173, "Buildings", 0);
							this.setMapTileIndex(14, 3, 2141, "Buildings", 0);
							this.setMapTileIndex(14, 3, 219, "Back", 0);
							this.temporarySprites.Add(new EmilysParrot(new Vector2((float)(14 * Game1.tileSize + Game1.pixelZoom * 4), (float)(3 * Game1.tileSize - Game1.pixelZoom * 8))));
							return;
						}
					}
					else if (num != 720888915u)
					{
						if (num != 746089795u)
						{
							if (num != 807500499u)
							{
								return;
							}
							if (!(text == "Hospital"))
							{
								return;
							}
							if (!Game1.isRaining)
							{
								Game1.changeMusicTrack("distantBanjo");
							}
							Game1.ambientLight = new Color(100, 100, 60);
							if (Game1.random.NextDouble() < 0.5)
							{
								NPC p = Game1.getCharacterFromName("Maru", false);
								if (p != null)
								{
									string toSay = "";
									switch (Game1.random.Next(5))
									{
									case 0:
										toSay = "Strings\\SpeechBubbles:Hospital_Maru_Greeting1";
										break;
									case 1:
										toSay = "Strings\\SpeechBubbles:Hospital_Maru_Greeting2";
										break;
									case 2:
										toSay = "Strings\\SpeechBubbles:Hospital_Maru_Greeting3";
										break;
									case 3:
										toSay = "Strings\\SpeechBubbles:Hospital_Maru_Greeting4";
										break;
									case 4:
										toSay = "Strings\\SpeechBubbles:Hospital_Maru_Greeting5";
										break;
									}
									if (Game1.player.spouse != null && Game1.player.spouse.Equals("Maru"))
									{
										toSay = "Strings\\SpeechBubbles:Hospital_Maru_Spouse";
										p.showTextAboveHead(Game1.content.LoadString(toSay, new object[0]), 2, 2, 3000, 0);
										return;
									}
									p.showTextAboveHead(Game1.content.LoadString(toSay, new object[0]), -1, 2, 3000, 0);
									return;
								}
							}
						}
						else
						{
							if (!(text == "ScienceHouse"))
							{
								return;
							}
							if (Game1.random.NextDouble() < 0.5 && Game1.player.currentLocation.isOutdoors)
							{
								NPC p2 = Game1.getCharacterFromName("Robin", false);
								if (p2 != null && p2.getTileY() == 18)
								{
									string toSay2 = "";
									switch (Game1.random.Next(4))
									{
									case 0:
										toSay2 = (Game1.isRaining ? "Strings\\SpeechBubbles:ScienceHouse_Robin_Raining1" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotRaining1");
										break;
									case 1:
										toSay2 = (Game1.isSnowing ? "Strings\\SpeechBubbles:ScienceHouse_Robin_Snowing" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotSnowing");
										break;
									case 2:
										toSay2 = ((Game1.player.getFriendshipHeartLevelForNPC("Robin") > 4) ? "Strings\\SpeechBubbles:ScienceHouse_Robin_CloseFriends" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotCloseFriends");
										break;
									case 3:
										toSay2 = (Game1.isRaining ? "Strings\\SpeechBubbles:ScienceHouse_Robin_Raining2" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotRaining2");
										break;
									case 4:
										toSay2 = "Strings\\SpeechBubbles:ScienceHouse_Robin_Greeting";
										break;
									}
									if (Game1.random.NextDouble() < 0.001)
									{
										toSay2 = "Strings\\SpeechBubbles:ScienceHouse_Robin_RareGreeting";
									}
									p2.showTextAboveHead(Game1.content.LoadString(toSay2, new object[]
									{
										Game1.player.name
									}), -1, 2, 3000, 0);
									return;
								}
							}
						}
					}
					else
					{
						if (!(text == "JojaMart"))
						{
							return;
						}
						Game1.changeMusicTrack("Hospital_Ambient");
						Game1.ambientLight = new Color(0, 0, 0);
						if (Game1.random.NextDouble() < 0.5)
						{
							NPC p3 = Game1.getCharacterFromName("Morris", false);
							if (p3 != null)
							{
								string toSay3 = "Strings\\SpeechBubbles:JojaMart_Morris_Greeting";
								p3.showTextAboveHead(Game1.content.LoadString(toSay3, new object[0]), -1, 2, 3000, 0);
								return;
							}
						}
					}
				}
				else if (num <= 1367472567u)
				{
					if (num != 1167876998u)
					{
						if (num != 1367472567u)
						{
							return;
						}
						if (!(text == "Blacksmith"))
						{
							return;
						}
						AmbientLocationSounds.addSound(new Vector2(9f, 10f), 2);
						AmbientLocationSounds.changeSpecificVariable("Frequency", 2f, 2);
						Game1.changeMusicTrack("none");
						return;
					}
					else
					{
						if (!(text == "ManorHouse"))
						{
							return;
						}
						Game1.ambientLight = new Color(150, 120, 50);
						NPC le = Game1.getCharacterFromName("Lewis", false);
						if (le != null)
						{
							string toSay4 = (Game1.timeOfDay < 1200) ? "Morning" : ((Game1.timeOfDay < 1700) ? "Afternoon" : "Evening");
							le.faceTowardFarmerForPeriod(3000, 15, false, Game1.player);
							le.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:ManorHouse_Lewis_" + toSay4, new object[0]), -1, 2, 3000, 0);
							return;
						}
					}
				}
				else if (num != 1428365440u)
				{
					if (num != 1446049731u)
					{
						if (num != 1840909614u)
						{
							return;
						}
						if (!(text == "SandyHouse"))
						{
							return;
						}
						Game1.changeMusicTrack("distantBanjo");
						Game1.ambientLight = new Color(0, 0, 0);
						if (Game1.random.NextDouble() < 0.5)
						{
							NPC p4 = Game1.getCharacterFromName("Sandy", false);
							if (p4 != null)
							{
								string toSay5 = "";
								switch (Game1.random.Next(5))
								{
								case 0:
									toSay5 = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting1";
									break;
								case 1:
									toSay5 = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting2";
									break;
								case 2:
									toSay5 = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting3";
									break;
								case 3:
									toSay5 = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting4";
									break;
								case 4:
									toSay5 = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting5";
									break;
								}
								p4.showTextAboveHead(Game1.content.LoadString(toSay5, new object[0]), -1, 2, 3000, 0);
								return;
							}
						}
					}
					else
					{
						if (!(text == "CommunityCenter"))
						{
							return;
						}
						if (this is CommunityCenter && Game1.isLocationAccessible("CommunityCenter"))
						{
							this.setFireplace(true, 31, 8, false);
							this.setFireplace(true, 32, 8, false);
							this.setFireplace(true, 33, 8, false);
							return;
						}
					}
				}
				else
				{
					if (!(text == "SeedShop"))
					{
						return;
					}
					this.setFireplace(true, 25, 13, false);
					if (Game1.random.NextDouble() < 0.5)
					{
						NPC p5 = Game1.getCharacterFromName("Pierre", false);
						if (p5 != null && p5.getTileY() == 17)
						{
							string toSay6 = "";
							switch (Game1.random.Next(5))
							{
							case 0:
								toSay6 = (Game1.IsWinter ? "Winter" : "NotWinter");
								break;
							case 1:
								toSay6 = (Game1.IsSummer ? "Summer" : "NotSummer");
								break;
							case 2:
								toSay6 = "Greeting1";
								break;
							case 3:
								toSay6 = "Greeting2";
								break;
							case 4:
								toSay6 = (Game1.isRaining ? "Raining" : "NotRaining");
								break;
							}
							if (Game1.random.NextDouble() < 0.001)
							{
								toSay6 = "RareGreeting";
							}
							p5.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:SeedShop_Pierre_" + toSay6, new object[]
							{
								Game1.player.Name
							}), -1, 2, 3000, 0);
						}
					}
				}
			}
			else if (num <= 2844260897u)
			{
				if (num <= 2233558176u)
				{
					if (num != 1919215024u)
					{
						if (num != 2233558176u)
						{
							return;
						}
						if (!(text == "Greenhouse"))
						{
							return;
						}
						if (Game1.isDarkOut())
						{
							Game1.ambientLight = Game1.outdoorLight;
							return;
						}
					}
					else
					{
						if (!(text == "ElliottHouse"))
						{
							return;
						}
						Game1.changeMusicTrack("communityCenter");
						NPC e = Game1.getCharacterFromName("Elliott", false);
						if (e != null)
						{
							string toSay7 = "";
							switch (Game1.random.Next(3))
							{
							case 0:
								toSay7 = "Strings\\SpeechBubbles:ElliottHouse_Elliott_Greeting1";
								break;
							case 1:
								toSay7 = "Strings\\SpeechBubbles:ElliottHouse_Elliott_Greeting2";
								break;
							case 2:
								toSay7 = "Strings\\SpeechBubbles:ElliottHouse_Elliott_Greeting3";
								break;
							}
							e.faceTowardFarmerForPeriod(3000, 15, false, Game1.player);
							e.showTextAboveHead(Game1.content.LoadString(toSay7, new object[]
							{
								Game1.player.name
							}), -1, 2, 3000, 0);
							return;
						}
					}
				}
				else if (num != 2708986271u)
				{
					if (num != 2841403676u)
					{
						if (num != 2844260897u)
						{
							return;
						}
						if (!(text == "Woods"))
						{
							return;
						}
						Game1.ambientLight = new Color(150, 120, 50);
						return;
					}
					else
					{
						if (!(text == "WitchSwamp"))
						{
							return;
						}
						if (Game1.player.mailReceived.Contains("henchmanGone"))
						{
							this.removeTile(20, 29, "Buildings");
							return;
						}
						this.setMapTileIndex(20, 29, 10, "Buildings", 0);
						return;
					}
				}
				else
				{
					if (!(text == "ArchaeologyHouse"))
					{
						return;
					}
					this.setFireplace(true, 43, 4, false);
					if (Game1.random.NextDouble() < 0.5 && Game1.player.hasOrWillReceiveMail("artifactFound"))
					{
						NPC g = Game1.getCharacterFromName("Gunther", false);
						if (g != null)
						{
							string toSay8 = "";
							switch (Game1.random.Next(5))
							{
							case 0:
								toSay8 = "Greeting1";
								break;
							case 1:
								toSay8 = "Greeting2";
								break;
							case 2:
								toSay8 = "Greeting3";
								break;
							case 3:
								toSay8 = "Greeting4";
								break;
							case 4:
								toSay8 = "Greeting5";
								break;
							}
							if (Game1.random.NextDouble() < 0.001)
							{
								toSay8 = "RareGreeting";
							}
							g.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:ArchaeologyHouse_Gunther_" + toSay8, new object[0]), -1, 2, 3000, 0);
							return;
						}
					}
				}
			}
			else if (num <= 3030632101u)
			{
				if (num != 2909376585u)
				{
					if (num != 3030632101u)
					{
						return;
					}
					if (!(text == "LeahHouse"))
					{
						return;
					}
					Game1.changeMusicTrack("distantBanjo");
					NPC j = Game1.getCharacterFromName("Leah", false);
					if (Game1.IsFall || Game1.IsWinter || Game1.isRaining)
					{
						this.setFireplace(true, 11, 4, false);
					}
					if (j != null)
					{
						string toSay9 = "";
						switch (Game1.random.Next(3))
						{
						case 0:
							toSay9 = "Strings\\SpeechBubbles:LeahHouse_Leah_Greeting1";
							break;
						case 1:
							toSay9 = "Strings\\SpeechBubbles:LeahHouse_Leah_Greeting2";
							break;
						case 2:
							toSay9 = "Strings\\SpeechBubbles:LeahHouse_Leah_Greeting3";
							break;
						}
						j.faceTowardFarmerForPeriod(3000, 15, false, Game1.player);
						j.showTextAboveHead(Game1.content.LoadString(toSay9, new object[]
						{
							Game1.player.name
						}), -1, 2, 3000, 0);
						return;
					}
				}
				else
				{
					if (!(text == "Saloon"))
					{
						return;
					}
					if (Game1.timeOfDay >= 1700)
					{
						this.setFireplace(true, 22, 17, false);
						Game1.changeMusicTrack("Saloon1");
					}
					if (Game1.random.NextDouble() < 0.25)
					{
						NPC p6 = Game1.getCharacterFromName("Gus", false);
						if (p6 != null && p6.getTileY() == 18)
						{
							string toSay10 = "";
							switch (Game1.random.Next(5))
							{
							case 0:
								toSay10 = "Greeting";
								break;
							case 1:
								toSay10 = (Game1.IsSummer ? "Summer" : "NotSummer");
								break;
							case 2:
								toSay10 = (Game1.isSnowing ? "Snowing1" : "NotSnowing1");
								break;
							case 3:
								toSay10 = (Game1.isRaining ? "Raining" : "NotRaining");
								break;
							case 4:
								toSay10 = (Game1.isSnowing ? "Snowing2" : "NotSnowing2");
								break;
							}
							if (Game1.random.NextDouble() < 0.001)
							{
								toSay10 = "RareGreeting";
							}
							p6.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:Saloon_Gus_" + toSay10, new object[0]), -1, 2, 3000, 0);
							return;
						}
					}
				}
			}
			else if (num != 3095702198u)
			{
				if (num != 3755589785u)
				{
					if (num != 3978811393u)
					{
						return;
					}
					if (!(text == "AnimalShop"))
					{
						return;
					}
					this.setFireplace(true, 3, 14, false);
					if (Game1.random.NextDouble() < 0.5)
					{
						NPC p7 = Game1.getCharacterFromName("Marnie", false);
						if (p7 != null && p7.getTileY() == 14)
						{
							string toSay11 = "";
							switch (Game1.random.Next(4))
							{
							case 0:
								toSay11 = "Strings\\SpeechBubbles:AnimalShop_Marnie_Greeting1";
								break;
							case 1:
								toSay11 = "Strings\\SpeechBubbles:AnimalShop_Marnie_Greeting2";
								break;
							case 2:
								toSay11 = ((Game1.player.getFriendshipHeartLevelForNPC("Marnie") > 4) ? "Strings\\SpeechBubbles:AnimalShop_Marnie_CloseFriends" : "Strings\\SpeechBubbles:AnimalShop_Marnie_NotCloseFriends");
								break;
							case 3:
								toSay11 = (Game1.isRaining ? "Strings\\SpeechBubbles:AnimalShop_Marnie_Raining" : "Strings\\SpeechBubbles:AnimalShop_Marnie_NotRaining");
								break;
							case 4:
								toSay11 = "Strings\\SpeechBubbles:AnimalShop_Marnie_Greeting3";
								break;
							}
							if (Game1.random.NextDouble() < 0.001)
							{
								toSay11 = "Strings\\SpeechBubbles:AnimalShop_Marnie_RareGreeting";
							}
							p7.showTextAboveHead(Game1.content.LoadString(toSay11, new object[]
							{
								Game1.player.name,
								Game1.player.farmName
							}), -1, 2, 3000, 0);
							return;
						}
					}
				}
				else
				{
					if (!(text == "WitchHut"))
					{
						return;
					}
					if (Game1.player.mailReceived.Contains("hasPickedUpMagicInk"))
					{
						this.setMapTileIndex(4, 11, 113, "Buildings", 0);
						return;
					}
				}
			}
			else
			{
				if (!(text == "AdventureGuild"))
				{
					return;
				}
				this.setFireplace(true, 9, 11, false);
				if (Game1.random.NextDouble() < 0.5)
				{
					NPC p8 = Game1.getCharacterFromName("Marlon", false);
					if (p8 != null)
					{
						string toSay12 = "";
						switch (Game1.random.Next(5))
						{
						case 0:
							toSay12 = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting_" + (Game1.player.isMale ? "Male" : "Female");
							break;
						case 1:
							toSay12 = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting1";
							break;
						case 2:
							toSay12 = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting2";
							break;
						case 3:
							toSay12 = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting3";
							break;
						case 4:
							toSay12 = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting4";
							break;
						}
						p8.showTextAboveHead(Game1.content.LoadString(toSay12, new object[0]), -1, 2, 3000, 0);
						return;
					}
				}
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00058458 File Offset: 0x00056658
		public virtual void resetForPlayerEntry()
		{
			Utility.killAllStaticLoopingSoundCues();
			if (Game1.CurrentEvent == null && !this.Name.ToLower().Contains("bath"))
			{
				Game1.player.canOnlyWalk = false;
			}
			if (!(this is Farm))
			{
				this.temporarySprites.Clear();
			}
			if (Game1.options != null)
			{
				if (Game1.isOneOfTheseKeysDown(Keyboard.GetState(), Game1.options.runButton))
				{
					Game1.player.setRunning(!Game1.options.autoRun, true);
				}
				else
				{
					Game1.player.setRunning(Game1.options.autoRun, true);
				}
			}
			Game1.UpdateViewPort(false, new Point(Game1.player.getStandingX(), Game1.player.getStandingY()));
			Game1.previousViewportPosition = new Vector2((float)Game1.viewport.X, (float)Game1.viewport.Y);
			using (List<IClickableMenu>.Enumerator enumerator = Game1.onScreenMenus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.gameWindowSizeChanged(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height), new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
				}
			}
			if (Game1.player.rightRing != null)
			{
				Game1.player.rightRing.onNewLocation(Game1.player, this);
			}
			if (Game1.player.leftRing != null)
			{
				Game1.player.leftRing.onNewLocation(Game1.player, this);
			}
			this.forceViewportPlayerFollow = this.map.Properties.ContainsKey("ViewportFollowPlayer");
			this.lastTouchActionLocation = Game1.player.getTileLocation();
			for (int i = Game1.player.questLog.Count - 1; i >= 0; i--)
			{
				Game1.player.questLog[i].adjustGameLocation(this);
			}
			for (int j = this.characters.Count - 1; j >= 0; j--)
			{
				this.characters[j].behaviorOnFarmerLocationEntry(this, Game1.player);
			}
			if (!this.isOutdoors)
			{
				Game1.player.FarmerSprite.currentStep = "thudStep";
				if (this.doorSprites != null)
				{
					foreach (KeyValuePair<Point, TemporaryAnimatedSprite> v in this.doorSprites)
					{
						v.Value.reset();
						v.Value.paused = true;
					}
				}
			}
			if (!this.isOutdoors || this.ignoreOutdoorLighting)
			{
				PropertyValue ambientLight;
				this.map.Properties.TryGetValue("AmbientLight", out ambientLight);
				if (ambientLight != null)
				{
					string[] colorSplit = ambientLight.ToString().Split(new char[]
					{
						' '
					});
					Game1.ambientLight = new Color(Convert.ToInt32(colorSplit[0]), Convert.ToInt32(colorSplit[1]), Convert.ToInt32(colorSplit[2]));
				}
				else if (Game1.isDarkOut() || this.lightLevel > 0f)
				{
					Game1.ambientLight = new Color(180, 180, 0);
				}
				else
				{
					Game1.ambientLight = Color.White;
				}
				if (Game1.bloom != null)
				{
					Game1.bloom.Visible = false;
				}
				if (Game1.currentSong != null && Game1.currentSong.Name.Contains("ambient"))
				{
					Game1.changeMusicTrack("none");
				}
			}
			else if (!(this is Desert))
			{
				Game1.ambientLight = (Game1.isRaining ? new Color(255, 200, 80) : Color.White);
				if (Game1.bloom != null)
				{
					Game1.bloom.Visible = Game1.bloomDay;
				}
			}
			this.setUpLocationSpecificFlair();
			PropertyValue uniqueSprites;
			this.map.Properties.TryGetValue("UniqueSprite", out uniqueSprites);
			if (uniqueSprites != null)
			{
				string[] array = uniqueSprites.ToString().Split(new char[]
				{
					' '
				});
				for (int num = 0; num < array.Length; num++)
				{
					NPC k = Game1.getCharacterFromName(array[num], false);
					if (this.characters.Contains(k))
					{
						try
						{
							k.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + k.name + "_" + this.name);
							k.uniqueSpriteActive = true;
						}
						catch (Exception)
						{
						}
					}
				}
			}
			PropertyValue uniquePortraits;
			this.map.Properties.TryGetValue("UniquePortrait", out uniquePortraits);
			if (uniquePortraits != null)
			{
				string[] array = uniquePortraits.ToString().Split(new char[]
				{
					' '
				});
				for (int num = 0; num < array.Length; num++)
				{
					NPC l = Game1.getCharacterFromName(array[num], false);
					if (this.characters.Contains(l))
					{
						try
						{
							l.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + l.name + "_" + this.name);
							l.uniquePortraitActive = true;
						}
						catch (Exception)
						{
						}
					}
				}
			}
			PropertyValue lights;
			this.map.Properties.TryGetValue("Light", out lights);
			if (lights != null && !this.ignoreLights)
			{
				string[] split = lights.ToString().Split(new char[]
				{
					' '
				});
				for (int m = 0; m < split.Length; m += 3)
				{
					Game1.currentLightSources.Add(new LightSource(Convert.ToInt32(split[m + 2]), new Vector2((float)(Convert.ToInt32(split[m]) * Game1.tileSize + Game1.tileSize / 2), (float)(Convert.ToInt32(split[m + 1]) * Game1.tileSize + Game1.tileSize / 2)), 1f));
				}
			}
			if (this.isOutdoors)
			{
				PropertyValue brookSounds;
				this.map.Properties.TryGetValue("BrookSounds", out brookSounds);
				if (brookSounds != null)
				{
					string[] split2 = brookSounds.ToString().Split(new char[]
					{
						' '
					});
					for (int n = 0; n < split2.Length; n += 3)
					{
						AmbientLocationSounds.addSound(new Vector2((float)Convert.ToInt32(split2[n]), (float)Convert.ToInt32(split2[n + 1])), Convert.ToInt32(split2[n + 2]));
					}
				}
				Game1.randomizeDebrisWeatherPositions(Game1.debrisWeather);
			}
			foreach (KeyValuePair<Vector2, TerrainFeature> kvp in this.terrainFeatures)
			{
				kvp.Value.performPlayerEntryAction(kvp.Key);
			}
			if (this.largeTerrainFeatures != null)
			{
				foreach (LargeTerrainFeature expr_6A4 in this.largeTerrainFeatures)
				{
					expr_6A4.performPlayerEntryAction(expr_6A4.tilePosition);
				}
			}
			foreach (KeyValuePair<Vector2, Object> kvp2 in this.objects)
			{
				kvp2.Value.actionOnPlayerEntry();
			}
			if (this.isOutdoors && !Game1.eventUp && Game1.options.musicVolumeLevel > 0.025f && Game1.timeOfDay < 1200 && (Game1.currentSong == null || Game1.currentSong.Name.ToLower().Contains("ambient")))
			{
				Game1.playMorningSong();
			}
			if (!(this is MineShaft))
			{
				string a = Game1.currentSeason.ToLower();
				if (!(a == "spring"))
				{
					if (!(a == "summer"))
					{
						if (!(a == "fall"))
						{
							if (a == "winter")
							{
								this.waterColor = new Color(130, 80, 255) * 0.5f;
							}
						}
						else
						{
							this.waterColor = new Color(255, 130, 200) * 0.5f;
						}
					}
					else
					{
						this.waterColor = new Color(60, 240, 255) * 0.5f;
					}
				}
				else
				{
					this.waterColor = new Color(120, 200, 255) * 0.5f;
				}
			}
			PropertyValue musicValue = null;
			this.map.Properties.TryGetValue("Music", out musicValue);
			if (musicValue != null)
			{
				string[] split3 = musicValue.ToString().Split(new char[]
				{
					' '
				});
				if (split3.Length > 1)
				{
					if (Game1.timeOfDay >= Convert.ToInt32(split3[0]) && Game1.timeOfDay < Convert.ToInt32(split3[1]) && !split3[2].Equals(Game1.currentSong.Name))
					{
						Game1.changeMusicTrack(split3[2]);
					}
				}
				else if (Game1.currentSong == null || Game1.currentSong.IsStopped || !split3[0].Equals(Game1.currentSong.Name))
				{
					Game1.changeMusicTrack(split3[0]);
				}
			}
			if (this.isOutdoors)
			{
				((FarmerSprite)Game1.player.Sprite).currentStep = "sandyStep";
				this.tryToAddCritters(false);
			}
			PropertyValue doorsValue = null;
			this.map.Properties.TryGetValue("Doors", out doorsValue);
			if (doorsValue != null)
			{
				string[] doorsParams = doorsValue.ToString().Split(new char[]
				{
					' '
				});
				for (int i2 = 0; i2 < doorsParams.Length; i2 += 4)
				{
					int doorTileX = Convert.ToInt32(doorsParams[i2]);
					int doorTileY = Convert.ToInt32(doorsParams[i2 + 1]);
					Location doorLocation = new Location(doorTileX, doorTileY);
					if (this.map.GetLayer("Buildings").Tiles[doorLocation] == null)
					{
						Tile tmp = new StaticTile(this.map.GetLayer("Buildings"), this.map.GetTileSheet(doorsParams[i2 + 2]), BlendMode.Alpha, Convert.ToInt32(doorsParams[i2 + 3]));
						tmp.Properties.Add("Action", new PropertyValue("Door" + ((this.doorSprites[new Point(doorTileX, doorTileY)].endSound != null) ? (" " + this.doorSprites[new Point(doorTileX, doorTileY)].endSound) : "")));
						this.map.GetLayer("Buildings").Tiles[doorLocation] = tmp;
						doorLocation.Y--;
						this.map.GetLayer("Front").Tiles[doorLocation] = new StaticTile(this.map.GetLayer("Front"), this.map.GetTileSheet(doorsParams[i2 + 2]), BlendMode.Alpha, Convert.ToInt32(doorsParams[i2 + 3]) - this.map.GetTileSheet(doorsParams[i2 + 2]).SheetWidth);
						doorLocation.Y--;
						this.map.GetLayer("Front").Tiles[doorLocation] = new StaticTile(this.map.GetLayer("Front"), this.map.GetTileSheet(doorsParams[i2 + 2]), BlendMode.Alpha, Convert.ToInt32(doorsParams[i2 + 3]) - this.map.GetTileSheet(doorsParams[i2 + 2]).SheetWidth * 2);
					}
				}
			}
			if (Game1.timeOfDay < 1900 && (!Game1.isRaining || this.name.Equals("SandyHouse")))
			{
				PropertyValue dayTiles;
				this.map.Properties.TryGetValue("DayTiles", out dayTiles);
				if (dayTiles != null)
				{
					string[] split4 = dayTiles.ToString().Trim().Split(new char[]
					{
						' '
					});
					for (int i3 = 0; i3 < split4.Length; i3 += 4)
					{
						if (this.map.GetLayer(split4[i3]).Tiles[Convert.ToInt32(split4[i3 + 1]), Convert.ToInt32(split4[i3 + 2])] != null)
						{
							this.map.GetLayer(split4[i3]).Tiles[Convert.ToInt32(split4[i3 + 1]), Convert.ToInt32(split4[i3 + 2])].TileIndex = Convert.ToInt32(split4[i3 + 3]);
						}
					}
				}
			}
			else if (Game1.timeOfDay >= 1900 || (Game1.isRaining && !this.name.Equals("SandyHouse")))
			{
				if (!(this is MineShaft))
				{
					this.lightGlows.Clear();
				}
				PropertyValue nightTiles;
				this.map.Properties.TryGetValue("NightTiles", out nightTiles);
				if (nightTiles != null)
				{
					string[] split5 = nightTiles.ToString().Split(new char[]
					{
						' '
					});
					for (int i4 = 0; i4 < split5.Length; i4 += 4)
					{
						if (this.map.GetLayer(split5[i4]).Tiles[Convert.ToInt32(split5[i4 + 1]), Convert.ToInt32(split5[i4 + 2])] != null)
						{
							this.map.GetLayer(split5[i4]).Tiles[Convert.ToInt32(split5[i4 + 1]), Convert.ToInt32(split5[i4 + 2])].TileIndex = Convert.ToInt32(split5[i4 + 3]);
						}
					}
				}
			}
			if (this.name.Equals("Coop"))
			{
				using (List<CoopDweller>.Enumerator enumerator6 = Game1.player.coopDwellers.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						enumerator6.Current.setRandomPosition();
					}
				}
				string[] feedLocation = this.getMapProperty("Feed").Split(new char[]
				{
					' '
				});
				if (Game1.player.Feed <= 0)
				{
					this.map.GetLayer("Buildings").Tiles[Convert.ToInt32(feedLocation[0]), Convert.ToInt32(feedLocation[1])].TileIndex = 35;
				}
				else
				{
					this.map.GetLayer("Buildings").Tiles[Convert.ToInt32(feedLocation[0]), Convert.ToInt32(feedLocation[1])].TileIndex = 31;
				}
			}
			else if (this.name.Equals("Barn"))
			{
				using (List<BarnDweller>.Enumerator enumerator7 = Game1.player.barnDwellers.GetEnumerator())
				{
					while (enumerator7.MoveNext())
					{
						enumerator7.Current.setRandomPosition();
					}
				}
				string[] feedLocation2 = this.getMapProperty("Feed").Split(new char[]
				{
					' '
				});
				if (Game1.player.Feed <= 0)
				{
					this.map.GetLayer("Buildings").Tiles[Convert.ToInt32(feedLocation2[0]), Convert.ToInt32(feedLocation2[1])].TileIndex = 35;
				}
				else
				{
					this.map.GetLayer("Buildings").Tiles[Convert.ToInt32(feedLocation2[0]), Convert.ToInt32(feedLocation2[1])].TileIndex = 31;
				}
			}
			if (this.name.Equals("Mountain") && (Game1.timeOfDay < 2000 || !Game1.currentSeason.Equals("summer") || Game1.random.NextDouble() >= 0.3) && Game1.isRaining && !Game1.currentSeason.Equals("winter"))
			{
				Game1.random.NextDouble();
			}
			if (this.name.Equals("Club"))
			{
				Game1.changeMusicTrack("clubloop");
			}
			else if (Game1.currentSong != null && Game1.currentSong.Name.Equals("clubloop") && (Game1.nextMusicTrack == null || Game1.nextMusicTrack.Count<char>() == 0))
			{
				Game1.changeMusicTrack("none");
			}
			if (Game1.activeClickableMenu == null)
			{
				this.checkForEvents();
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000594BC File Offset: 0x000576BC
		public virtual bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
		{
			Object o;
			this.objects.TryGetValue(tileLocation, out o);
			Microsoft.Xna.Framework.Rectangle tileLocationRect = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (this.characters[i] != null && this.characters[i].GetBoundingBox().Intersects(tileLocationRect))
				{
					return true;
				}
			}
			if (this.isTileOccupiedByFarmer(tileLocation) != null && (toPlace == null || !toPlace.isPassable()))
			{
				return true;
			}
			if (this.largeTerrainFeatures != null)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getBoundingBox().Intersects(tileLocationRect))
						{
							return true;
						}
					}
				}
			}
			return (this.terrainFeatures.ContainsKey(tileLocation) && tileLocationRect.Intersects(this.terrainFeatures[tileLocation].getBoundingBox(tileLocation)) && (!this.terrainFeatures[tileLocation].isPassable(null) || (this.terrainFeatures[tileLocation].GetType() == typeof(HoeDirt) && ((HoeDirt)this.terrainFeatures[tileLocation]).crop != null))) || (!this.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), Game1.viewport) && (toPlace == null || !(toPlace is Wallpaper))) || o != null;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00059668 File Offset: 0x00057868
		public Farmer isTileOccupiedByFarmer(Vector2 tileLocation)
		{
			foreach (Farmer f in this.getFarmers())
			{
				if (f.getTileLocation().Equals(tileLocation))
				{
					return f;
				}
			}
			return null;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000596CC File Offset: 0x000578CC
		public virtual bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
		{
			Object o;
			this.objects.TryGetValue(tileLocation, out o);
			Microsoft.Xna.Framework.Rectangle tileLocationRect = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize + 1, (int)tileLocation.Y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2);
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (this.characters[i] != null && !this.characters[i].name.Equals(characterToIgnore) && this.characters[i].GetBoundingBox().Intersects(tileLocationRect))
				{
					return true;
				}
			}
			if (this.terrainFeatures.ContainsKey(tileLocation) && tileLocationRect.Intersects(this.terrainFeatures[tileLocation].getBoundingBox(tileLocation)))
			{
				return true;
			}
			if (this.largeTerrainFeatures != null)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getBoundingBox().Intersects(tileLocationRect))
						{
							return true;
						}
					}
				}
			}
			return o != null;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00059804 File Offset: 0x00057A04
		public virtual bool isTileOccupiedIgnoreFloors(Vector2 tileLocation, string characterToIgnore = "")
		{
			Object o;
			this.objects.TryGetValue(tileLocation, out o);
			Microsoft.Xna.Framework.Rectangle tileLocationRect = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize + 1, (int)tileLocation.Y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2);
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (this.characters[i] != null && !this.characters[i].name.Equals(characterToIgnore) && this.characters[i].GetBoundingBox().Intersects(tileLocationRect))
				{
					return true;
				}
			}
			if (this.terrainFeatures.ContainsKey(tileLocation) && tileLocationRect.Intersects(this.terrainFeatures[tileLocation].getBoundingBox(tileLocation)) && !this.terrainFeatures[tileLocation].isPassable(null))
			{
				return true;
			}
			if (this.largeTerrainFeatures != null)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getBoundingBox().Intersects(tileLocationRect))
						{
							return true;
						}
					}
				}
			}
			return o != null;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00059950 File Offset: 0x00057B50
		public bool isTileHoeDirt(Vector2 tileLocation)
		{
			return this.terrainFeatures.ContainsKey(tileLocation) && this.terrainFeatures[tileLocation] is HoeDirt;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00059978 File Offset: 0x00057B78
		public void playTerrainSound(Vector2 tileLocation, Character who = null, bool showTerrainDisturbAnimation = true)
		{
			string currentStep = "thudStep";
			if (Game1.currentLocation.IsOutdoors || Game1.currentLocation.Name.ToLower().Contains("mine"))
			{
				string stepType = Game1.currentLocation.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Type", "Back");
				if (stepType != null)
				{
					if (!(stepType == "Dirt"))
					{
						if (!(stepType == "Stone"))
						{
							if (!(stepType == "Grass"))
							{
								if (stepType == "Wood")
								{
									currentStep = "woodyStep";
								}
							}
							else
							{
								currentStep = (Game1.currentSeason.Equals("winter") ? "snowyStep" : "grassyStep");
							}
						}
						else
						{
							currentStep = "stoneStep";
						}
					}
					else
					{
						currentStep = "sandyStep";
					}
				}
				else
				{
					stepType = Game1.currentLocation.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Water", "Back");
					if (stepType != null)
					{
						currentStep = "waterSlosh";
					}
				}
			}
			else
			{
				currentStep = "thudStep";
			}
			if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) && Game1.currentLocation.terrainFeatures[tileLocation].GetType() == typeof(Flooring))
			{
				currentStep = ((Flooring)Game1.currentLocation.terrainFeatures[tileLocation]).getFootstepSound();
			}
			if ((who != null & showTerrainDisturbAnimation) && currentStep.Equals("sandyStep"))
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, Game1.tileSize, Game1.tileSize, Game1.tileSize), 50f, 4, 1, new Vector2(who.position.X + (float)Game1.random.Next(-8, 8), who.position.Y + (float)Game1.random.Next(-16, 0)), false, Game1.random.NextDouble() < 0.5, 0.0001f, 0f, Color.White, 1f, 0.01f, 0f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 128f, false));
			}
			else if ((who != null & showTerrainDisturbAnimation) && Game1.currentSeason.Equals("winter") && currentStep.Equals("grassyStep"))
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(247, 407, 6, 6), 2000f, 1, 10000, new Vector2(who.position.X, who.position.Y), false, false, 0.0001f, 0.001f, Color.White, 1f, 0.01f, 0f, 0f, false));
			}
			if (currentStep.Length > 0)
			{
				Game1.playSound(currentStep);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00059C60 File Offset: 0x00057E60
		public virtual bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.currentEvent != null && this.currentEvent.isFestival)
			{
				return this.currentEvent.checkAction(tileLocation, viewport, who);
			}
			foreach (NPC i in this.characters)
			{
				if (i != null && !i.IsMonster && (!who.isRidingHorse() || !(i is Horse)) && i.GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
				{
					bool result = i.checkAction(who, this);
					return result;
				}
			}
			if (who.IsMainPlayer && who.currentUpgrade != null && this.name.Equals("Farm") && tileLocation.Equals(new Location((int)(who.currentUpgrade.positionOfCarpenter.X + (float)(Game1.tileSize / 2)) / Game1.tileSize, (int)(who.currentUpgrade.positionOfCarpenter.Y + (float)(Game1.tileSize / 2)) / Game1.tileSize)))
			{
				if (who.currentUpgrade.daysLeftTillUpgradeDone == 1)
				{
					Game1.drawDialogue(Game1.getCharacterFromName("Robin", false), Game1.content.LoadString("Data\\ExtraDialogue:Farm_RobinWorking_ReadyTomorrow", new object[0]));
				}
				else
				{
					Game1.drawDialogue(Game1.getCharacterFromName("Robin", false), Game1.content.LoadString("Data\\ExtraDialogue:Farm_RobinWorking" + (Game1.random.Next(2) + 1), new object[0]));
				}
			}
			Vector2 vect = new Vector2((float)tileLocation.X, (float)tileLocation.Y);
			if (this.objects.ContainsKey(vect) && this.objects[vect].Type != null)
			{
				if (who.isRidingHorse() && !(this.objects[vect] is Fence))
				{
					return false;
				}
				if (vect.Equals(who.getTileLocation()) && !this.objects[vect].isPassable())
				{
					Tool t = new Pickaxe();
					t.DoFunction(Game1.currentLocation, -1, -1, 0, who);
					if (this.objects[vect].performToolAction(t))
					{
						this.objects[vect].performRemoveAction(this.objects[vect].tileLocation, Game1.currentLocation);
						if ((this.objects[vect].type.Equals("Crafting") || this.objects[vect].Type.Equals("interactive")) && this.objects[vect].fragility != 2)
						{
							Game1.currentLocation.debris.Add(new Debris(this.objects[vect].bigCraftable ? (-this.objects[vect].ParentSheetIndex) : this.objects[vect].ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
						}
						Game1.currentLocation.Objects.Remove(vect);
						return true;
					}
					t = new Axe();
					t.DoFunction(Game1.currentLocation, -1, -1, 0, who);
					if (this.objects.ContainsKey(vect) && this.objects[vect].performToolAction(t))
					{
						this.objects[vect].performRemoveAction(this.objects[vect].tileLocation, Game1.currentLocation);
						if ((this.objects[vect].type.Equals("Crafting") || this.objects[vect].Type.Equals("interactive")) && this.objects[vect].fragility != 2)
						{
							Game1.currentLocation.debris.Add(new Debris(this.objects[vect].bigCraftable ? (-this.objects[vect].ParentSheetIndex) : this.objects[vect].ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
						}
						Game1.currentLocation.Objects.Remove(vect);
						return true;
					}
					if (!this.objects.ContainsKey(vect))
					{
						return true;
					}
				}
				if (this.objects.ContainsKey(vect) && (this.objects[vect].Type.Equals("Crafting") || this.objects[vect].Type.Equals("interactive")))
				{
					if (who.ActiveObject == null)
					{
						return this.objects[vect].checkForAction(who, false);
					}
					if (this.objects[vect].performObjectDropInAction(who.ActiveObject, false, who))
					{
						who.reduceActiveItemByOne();
						return true;
					}
					return this.objects[vect].checkForAction(who, false);
				}
				else if (this.objects.ContainsKey(vect) && this.objects[vect].isSpawnedObject)
				{
					int oldQuality = this.objects[vect].quality;
					Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + (int)vect.X + (int)vect.Y * 777);
					if (who.professions.Contains(16) && this.objects[vect].isForage(this))
					{
						this.objects[vect].quality = 4;
					}
					else if (this.objects[vect].isForage(this))
					{
						if (r.NextDouble() < (double)((float)who.ForagingLevel / 30f))
						{
							this.objects[vect].quality = 2;
						}
						else if (r.NextDouble() < (double)((float)who.ForagingLevel / 15f))
						{
							this.objects[vect].quality = 1;
						}
					}
					if (who.couldInventoryAcceptThisItem(this.objects[vect]))
					{
						if (who.IsMainPlayer)
						{
							Game1.playSound("pickUpItem");
							DelayedAction.playSoundAfterDelay("coin", 300);
						}
						who.animateOnce(279 + who.FacingDirection);
						if (!this.isFarmBuildingInterior())
						{
							if (this.objects[vect].isForage(this))
							{
								who.gainExperience(2, 7);
							}
						}
						else
						{
							who.gainExperience(0, 5);
						}
						who.addItemToInventoryBool(this.objects[vect].getOne(), false);
						if (who.professions.Contains(13) && r.NextDouble() < 0.2 && !this.objects[vect].questItem && who.couldInventoryAcceptThisItem(this.objects[vect]) && !this.isFarmBuildingInterior())
						{
							who.addItemToInventoryBool(this.objects[vect].getOne(), false);
							who.gainExperience(2, 7);
						}
						this.objects.Remove(vect);
						return true;
					}
					this.objects[vect].quality = oldQuality;
				}
			}
			if (who.isRidingHorse())
			{
				who.getMount().checkAction(who, this);
				return true;
			}
			if (this.terrainFeatures.ContainsKey(vect) && this.terrainFeatures[vect].GetType() == typeof(HoeDirt) && who.ActiveObject != null && (who.ActiveObject.Category == -74 || who.ActiveObject.Category == -19) && ((HoeDirt)this.terrainFeatures[vect]).canPlantThisSeedHere(who.ActiveObject.ParentSheetIndex, tileLocation.X, tileLocation.Y, who.ActiveObject.Category == -19))
			{
				if (((HoeDirt)this.terrainFeatures[vect]).plant(who.ActiveObject.ParentSheetIndex, tileLocation.X, tileLocation.Y, who, who.ActiveObject.Category == -19) && who.IsMainPlayer)
				{
					who.reduceActiveItemByOne();
				}
				Game1.haltAfterCheck = false;
				return true;
			}
			Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			foreach (KeyValuePair<Vector2, TerrainFeature> v in this.terrainFeatures)
			{
				if (v.Value.getBoundingBox(v.Key).Intersects(tileRect))
				{
					Game1.haltAfterCheck = false;
					bool result = v.Value.performUseAction(v.Key);
					return result;
				}
			}
			if (this.largeTerrainFeatures != null)
			{
				foreach (LargeTerrainFeature f in this.largeTerrainFeatures)
				{
					if (f.getBoundingBox().Intersects(tileRect))
					{
						Game1.haltAfterCheck = false;
						bool result = f.performUseAction(f.tilePosition);
						return result;
					}
				}
			}
			PropertyValue action = null;
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null)
			{
				tile.Properties.TryGetValue("Action", out action);
			}
			return action != null && (this.currentEvent != null || this.isCharacterAtTile(vect + new Vector2(0f, 1f)) == null) && this.performAction(action, who, tileLocation);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0005A6CC File Offset: 0x000588CC
		public virtual bool leftClick(int x, int y, Farmer who)
		{
			Vector2 clickTile = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			if (this.objects.ContainsKey(clickTile) && this.objects[clickTile].clicked(who))
			{
				this.objects.Remove(clickTile);
				return true;
			}
			return false;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual int getExtraMillisecondsPerInGameMinuteForThisLocation()
		{
			return 0;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0005A722 File Offset: 0x00058922
		public bool isTileLocationTotallyClearAndPlaceable(int x, int y)
		{
			return this.isTileLocationTotallyClearAndPlaceable(new Vector2((float)x, (float)y));
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0005A734 File Offset: 0x00058934
		public virtual bool isTileLocationTotallyClearAndPlaceableIgnoreFloors(Vector2 v)
		{
			return this.isTileOnMap(v) && !this.isTileOccupiedIgnoreFloors(v, "") && this.isTilePassable(new Location((int)v.X, (int)v.Y), Game1.viewport) && this.isTilePlaceable(v, null);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0005A784 File Offset: 0x00058984
		public virtual bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
		{
			return this.isTileOnMap(v) && !this.isTileOccupied(v, "") && this.isTilePassable(new Location((int)v.X, (int)v.Y), Game1.viewport) && this.isTilePlaceable(v, null);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0005A7D4 File Offset: 0x000589D4
		public virtual bool isTilePlaceable(Vector2 v, Item item = null)
		{
			return (this.doesTileHaveProperty((int)v.X, (int)v.Y, "NoFurniture", "Back") == null || (item != null && item is Object && (item as Object).isPassable() && Game1.currentLocation.IsOutdoors && !this.doesTileHavePropertyNoNull((int)v.X, (int)v.Y, "NoFurniture", "Back").Equals("total"))) && (this.doesTileHaveProperty((int)v.X, (int)v.Y, "Water", "Back") == null || (item != null && item.canBePlacedInWater()));
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0005A880 File Offset: 0x00058A80
		public virtual bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
		{
			return (this.objects.ContainsKey(p) && this.objects[p].isPassable()) || (this.terrainFeatures.ContainsKey(p) && this.terrainFeatures[p].isPassable(null));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0005A8D8 File Offset: 0x00058AD8
		public void openDoor(Location tileLocation, bool playSound)
		{
			try
			{
				int tileIndex = this.getTileIndexAt(tileLocation.X, tileLocation.Y, "Buildings");
				if (this.doorSprites.ContainsKey(new Point(tileLocation.X, tileLocation.Y)) && this.doorSprites[new Point(tileLocation.X, tileLocation.Y)].paused)
				{
					this.doorSprites[new Point(tileLocation.X, tileLocation.Y)].paused = false;
					if (tileIndex == 824)
					{
						this.openDoor(new Location(tileLocation.X + 1, tileLocation.Y), false);
					}
					else if (tileIndex == 825)
					{
						this.openDoor(new Location(tileLocation.X - 1, tileLocation.Y), false);
					}
				}
				DelayedAction.removeTileAfterDelay(tileLocation.X, tileLocation.Y, 400, this, "Buildings");
				this.removeTile(new Location(tileLocation.X, tileLocation.Y - 1), "Front");
				this.removeTile(new Location(tileLocation.X, tileLocation.Y - 2), "Front");
				if (playSound)
				{
					if (tileIndex == 120)
					{
						Game1.playSound("doorOpen");
					}
					else
					{
						Game1.playSound("doorCreak");
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0005AA40 File Offset: 0x00058C40
		public void doStarpoint(string which)
		{
			if (!(which == "3"))
			{
				if (!(which == "4"))
				{
					return;
				}
				if (Game1.player.ActiveObject != null && Game1.player.ActiveObject != null && !Game1.player.ActiveObject.bigCraftable && Game1.player.ActiveObject.parentSheetIndex == 203)
				{
					Game1.player.ActiveObject = new Object(Vector2.Zero, 162, false);
					Game1.playSound("croak");
					Game1.flashAlpha = 1f;
				}
			}
			else if (Game1.player.ActiveObject != null && Game1.player.ActiveObject != null && !Game1.player.ActiveObject.bigCraftable && Game1.player.ActiveObject.parentSheetIndex == 307)
			{
				Game1.player.ActiveObject = new Object(Vector2.Zero, 161, false);
				Game1.playSound("discoverMineral");
				Game1.flashAlpha = 1f;
				return;
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0005AB50 File Offset: 0x00058D50
		public bool performAction(string action, Farmer who, Location tileLocation)
		{
			if (action != null && who.IsMainPlayer)
			{
				string[] actionParams = action.ToString().Split(new char[]
				{
					' '
				});
				string text = actionParams[0];
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2413466880u)
				{
					if (num <= 1135412759u)
					{
						if (num <= 297990791u)
						{
							if (num <= 139067618u)
							{
								if (num <= 48641340u)
								{
									if (num != 4774130u)
									{
										if (num != 48641340u)
										{
											return true;
										}
										if (!(text == "SpiritAltar"))
										{
											return true;
										}
										if (who.ActiveObject != null && Game1.dailyLuck != -0.12 && Game1.dailyLuck != 0.12)
										{
											if (who.ActiveObject.Price >= 60)
											{
												this.temporarySprites.Add(new TemporaryAnimatedSprite(352, 70f, 2, 2, new Vector2((float)(tileLocation.X * Game1.tileSize), (float)(tileLocation.Y * Game1.tileSize)), false, false));
												Game1.dailyLuck = 0.12;
												Game1.playSound("money");
											}
											else
											{
												this.temporarySprites.Add(new TemporaryAnimatedSprite(362, 50f, 6, 1, new Vector2((float)(tileLocation.X * Game1.tileSize), (float)(tileLocation.Y * Game1.tileSize)), false, false));
												Game1.dailyLuck = -0.12;
												Game1.playSound("thunder");
											}
											who.ActiveObject = null;
											who.showNotCarrying();
											return true;
										}
										return true;
									}
									else
									{
										if (!(text == "EvilShrineRight"))
										{
											return true;
										}
										if (Game1.spawnMonstersAtNight)
										{
											this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineRightDeActivate", new object[0]), this.createYesNoResponses(), "evilShrineRightDeActivate");
											return true;
										}
										this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineRightActivate", new object[0]), this.createYesNoResponses(), "evilShrineRightActivate");
										return true;
									}
								}
								else if (num != 49977355u)
								{
									if (num != 135767117u)
									{
										if (num != 139067618u)
										{
											return true;
										}
										if (!(text == "IceCreamStand"))
										{
											return true;
										}
										if (this.isCharacterAtTile(new Vector2((float)tileLocation.X, (float)(tileLocation.Y - 2))) != null || this.isCharacterAtTile(new Vector2((float)tileLocation.X, (float)(tileLocation.Y - 1))) != null || this.isCharacterAtTile(new Vector2((float)tileLocation.X, (float)(tileLocation.Y - 3))) != null)
										{
											Game1.activeClickableMenu = new ShopMenu(new Dictionary<Item, int[]>
											{
												{
													new Object(233, 1, false, -1, 0),
													new int[]
													{
														250,
														2147483647
													}
												}
											}, 0, null);
											return true;
										}
										if (Game1.currentSeason.Equals("summer"))
										{
											Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:IceCreamStand_ComeBackLater", new object[0]));
											return true;
										}
										Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:IceCreamStand_NotSummer", new object[0]));
										return true;
									}
									else
									{
										if (!(text == "Jukebox"))
										{
											return true;
										}
										Game1.activeClickableMenu = new ChooseFromListMenu(Game1.player.songsHeard, new ChooseFromListMenu.actionOnChoosingListOption(ChooseFromListMenu.playSongAction), true);
										return true;
									}
								}
								else
								{
									if (!(text == "Crib"))
									{
										return true;
									}
									using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											NPC i = enumerator.Current;
											if (i is Child && (i as Child).age == 1)
											{
												(i as Child).toss(who);
											}
											else if (i is Child && (i as Child).age == 0)
											{
												Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:FarmHouse_Crib_NewbornSleeping", new object[]
												{
													i.name
												})));
											}
										}
										return true;
									}
								}
							}
							else if (num <= 234320812u)
							{
								if (num != 183343509u)
								{
									if (num != 234320812u)
									{
										return true;
									}
									if (!(text == "SandDragon"))
									{
										return true;
									}
									if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 768 && !who.hasOrWillReceiveMail("TH_SandDragon") && who.hasOrWillReceiveMail("TH_MayorFridge"))
									{
										who.reduceActiveItemByOne();
										Game1.player.CanMove = false;
										Game1.playSound("eat");
										Game1.player.mailReceived.Add("TH_SandDragon");
										Game1.multipleDialogues(new string[]
										{
											Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_ConsumeEssence", new object[0]),
											Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_MrQiNote", new object[0])
										});
										Game1.player.removeQuest(4);
										Game1.player.addQuest(5);
										return true;
									}
									if (who.hasOrWillReceiveMail("TH_SandDragon"))
									{
										Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_MrQiNote", new object[0]));
										return true;
									}
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_Initial", new object[0]));
									return true;
								}
								else
								{
									if (!(text == "ColaMachine"))
									{
										return true;
									}
									this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_ColaMachine_Question", new object[0]), this.createYesNoResponses(), "buyJojaCola");
									return true;
								}
							}
							else if (num != 267393898u)
							{
								if (num != 295776207u)
								{
									if (num != 297990791u)
									{
										return true;
									}
									if (!(text == "NPCMessage"))
									{
										return true;
									}
									NPC npc = Game1.getCharacterFromName(actionParams[1], false);
									if (npc != null && npc.withinPlayerThreshold(14))
									{
										try
										{
											npc.setNewDialogue(Game1.content.LoadString(action.Substring(action.IndexOf('"') + 1).Split(new char[]
											{
												'/'
											})[0], new object[0]), true, false);
											Game1.drawDialogue(npc);
											bool result = false;
											return result;
										}
										catch (Exception)
										{
											bool result = false;
											return result;
										}
									}
									try
									{
										Game1.drawDialogueNoTyping(Game1.content.LoadString(action.Substring(action.IndexOf('"')).Split(new char[]
										{
											'/'
										})[1].Replace("\"", ""), new object[0]));
										bool result = false;
										return result;
									}
									catch (Exception)
									{
										bool result = false;
										return result;
									}
									goto IL_1F10;
								}
								else
								{
									if (!(text == "Warp"))
									{
										return true;
									}
									who.faceGeneralDirection(new Vector2((float)tileLocation.X, (float)tileLocation.Y) * (float)Game1.tileSize, 0);
									Rumble.rumble(0.15f, 200f);
									Game1.warpFarmer(actionParams[3], Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]), false);
									if (actionParams.Length < 5)
									{
										Game1.playSound("doorClose");
										return true;
									}
									return true;
								}
							}
							else
							{
								if (!(text == "Notes"))
								{
									return true;
								}
								this.readNote(Convert.ToInt32(actionParams[1]));
								return true;
							}
						}
						else if (num <= 837292325u)
						{
							if (num <= 414528787u)
							{
								if (num != 371676316u)
								{
									if (num != 414528787u)
									{
										return true;
									}
									if (!(text == "QiCoins"))
									{
										return true;
									}
									if (who.clubCoins > 0)
									{
										Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_QiCoins", new object[]
										{
											who.clubCoins
										}));
										return true;
									}
									this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_QiCoins_BuyStarter", new object[0]), this.createYesNoResponses(), "BuyClubCoins");
									return true;
								}
								else
								{
									if (!(text == "MineElevator"))
									{
										return true;
									}
									if (Game1.mine == null || Game1.mine.lowestLevelReached < 5)
									{
										Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Mines_MineElevator_NotWorking", new object[0])));
										return true;
									}
									Game1.activeClickableMenu = new MineElevatorMenu();
									return true;
								}
							}
							else if (num != 570160120u)
							{
								if (num != 634795166u)
								{
									if (num != 837292325u)
									{
										return true;
									}
									if (!(text == "BuyBackpack"))
									{
										return true;
									}
									Response purchase2000 = new Response("Purchase", Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Response2000", new object[0]));
									Response purchase2001 = new Response("Purchase", Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Response10000", new object[0]));
									Response notNow = new Response("Not", Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_ResponseNo", new object[0]));
									if (Game1.player.maxItems == 12)
									{
										this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Question24", new object[0]), new Response[]
										{
											purchase2000,
											notNow
										}, "Backpack");
										return true;
									}
									if (Game1.player.maxItems < 36)
									{
										this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Question36", new object[0]), new Response[]
										{
											purchase2001,
											notNow
										}, "Backpack");
										return true;
									}
									return true;
								}
								else
								{
									if (!(text == "ClubSlots"))
									{
										return true;
									}
									Game1.currentMinigame = new Slots(-1, false);
									return true;
								}
							}
							else
							{
								if (!(text == "MagicInk"))
								{
									return true;
								}
								if (!who.mailReceived.Contains("hasPickedUpMagicInk"))
								{
									who.mailReceived.Add("hasPickedUpMagicInk");
									who.hasMagicInk = true;
									this.setMapTileIndex(4, 11, 113, "Buildings", 0);
									who.addItemByMenuIfNecessaryElseHoldUp(new SpecialItem(1, 7, ""), null);
									return true;
								}
								return true;
							}
						}
						else if (num <= 895720287u)
						{
							if (num != 870733615u)
							{
								if (num != 895720287u)
								{
									return true;
								}
								if (!(text == "HospitalShop"))
								{
									return true;
								}
								if (this.isCharacterAtTile(who.getTileLocation() + new Vector2(0f, -2f)) != null || this.isCharacterAtTile(who.getTileLocation() + new Vector2(-1f, -2f)) != null)
								{
									Game1.activeClickableMenu = new ShopMenu(Utility.getHospitalStock(), 0, null);
									return true;
								}
								return true;
							}
							else
							{
								if (!(text == "kitchen"))
								{
									return true;
								}
								Vector2 center = Utility.getTopLeftPositionForCenteringOnScreen(800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, 0, 0);
								Game1.activeClickableMenu = new CraftingPage((int)center.X, (int)center.Y, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true);
								return true;
							}
						}
						else if (num != 908820861u)
						{
							if (num != 1094091226u)
							{
								if (num != 1135412759u)
								{
									return true;
								}
								if (!(text == "Carpenter"))
								{
									return true;
								}
								if (who.getTileY() > tileLocation.Y)
								{
									this.carpenters(tileLocation);
									return true;
								}
								return true;
							}
							else
							{
								if (!(text == "Arcade_Prairie"))
								{
									return true;
								}
								Game1.currentMinigame = new AbigailGame(false);
								return true;
							}
						}
						else
						{
							if (!(text == "Letter"))
							{
								return true;
							}
							Game1.drawLetterMessage(this.actionParamsToString(actionParams));
							return true;
						}
					}
					else if (num <= 1719994463u)
					{
						if (num <= 1555723527u)
						{
							if (num <= 1288111488u)
							{
								if (num != 1140116675u)
								{
									if (num != 1288111488u)
									{
										return true;
									}
									if (!(text == "ShippingBin"))
									{
										return true;
									}
									Game1.shipHeldItem();
									return true;
								}
								else
								{
									if (!(text == "JojaShop"))
									{
										return true;
									}
									goto IL_EEF;
								}
							}
							else if (num != 1367472567u)
							{
								if (num != 1379459566u)
								{
									if (num != 1555723527u)
									{
										return true;
									}
									if (!(text == "BlackJack"))
									{
										return true;
									}
								}
								else
								{
									if (!(text == "WarpMensLocker"))
									{
										return true;
									}
									if (!who.isMale)
									{
										if (who.IsMainPlayer)
										{
											Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MensLocker_WrongGender", new object[0]));
										}
										return false;
									}
									who.faceGeneralDirection(new Vector2((float)tileLocation.X, (float)tileLocation.Y) * (float)Game1.tileSize, 0);
									Game1.warpFarmer(actionParams[3], Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]), false);
									if (actionParams.Length < 5)
									{
										Game1.playSound("doorClose");
										return true;
									}
									return true;
								}
							}
							else
							{
								if (!(text == "Blacksmith"))
								{
									return true;
								}
								if (who.getTileY() > tileLocation.Y)
								{
									this.blacksmith(tileLocation);
									return true;
								}
								return true;
							}
						}
						else if (num <= 1673854597u)
						{
							if (num != 1573286044u)
							{
								if (num != 1673854597u)
								{
									return true;
								}
								if (!(text == "WizardShrine"))
								{
									return true;
								}
								this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WizardTower_WizardShrine", new object[0]).Replace('\n', '^'), this.createYesNoResponses(), "WizardShrine");
								return true;
							}
							else if (!(text == "ClubCards"))
							{
								return true;
							}
						}
						else if (num != 1687261568u)
						{
							if (num != 1716769139u)
							{
								if (num != 1719994463u)
								{
									return true;
								}
								if (!(text == "WizardBook"))
								{
									return true;
								}
								if (who.mailReceived.Contains("hasPickedUpMagicInk") || who.hasMagicInk)
								{
									Game1.activeClickableMenu = new CarpenterMenu(true);
									return true;
								}
								return true;
							}
							else
							{
								if (!(text == "Dialogue"))
								{
									return true;
								}
								goto IL_1E3C;
							}
						}
						else
						{
							if (!(text == "ClubComputer"))
							{
								return true;
							}
							goto IL_25B9;
						}
						if (actionParams.Length > 1 && actionParams[1].Equals("1000"))
						{
							this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_HS", new object[0]), new Response[]
							{
								new Response("Play", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Play", new object[0])),
								new Response("Leave", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Leave", new object[0]))
							}, "CalicoJackHS");
							return true;
						}
						this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJack", new object[0]), new Response[]
						{
							new Response("Play", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Play", new object[0])),
							new Response("Leave", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Leave", new object[0])),
							new Response("Rules", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Rules", new object[0]))
						}, "CalicoJack");
						return true;
					}
					else if (num <= 1959071256u)
					{
						if (num <= 1806134029u)
						{
							if (num != 1722787773u)
							{
								if (num != 1806134029u)
								{
									return true;
								}
								if (!(text == "BuyQiCoins"))
								{
									return true;
								}
								this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_Buy100Coins", new object[0]), this.createYesNoResponses(), "BuyQiCoins");
								return true;
							}
							else
							{
								if (!(text == "MessageOnce"))
								{
									return true;
								}
								if (!who.eventsSeen.Contains(Convert.ToInt32(actionParams[1])))
								{
									who.eventsSeen.Add(Convert.ToInt32(actionParams[1]));
									Game1.drawObjectDialogue(Game1.parseText(this.actionParamsToString(actionParams).Substring(this.actionParamsToString(actionParams).IndexOf(' '))));
									return true;
								}
								return true;
							}
						}
						else if (num != 1852246243u)
						{
							if (num != 1856350152u)
							{
								if (num != 1959071256u)
								{
									return true;
								}
								if (!(text == "WizardHatch"))
								{
									return true;
								}
								if (who.friendships.ContainsKey("Wizard") && who.friendships["Wizard"][0] >= 1000)
								{
									Game1.warpFarmer("WizardHouseBasement", 4, 4, true);
									Game1.playSound("doorClose");
									return true;
								}
								NPC wizard = this.characters[0];
								wizard.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Data\\ExtraDialogue:Wizard_Hatch", new object[0]), wizard));
								Game1.drawDialogue(wizard);
								return true;
							}
							else
							{
								if (!(text == "Yoba"))
								{
									return true;
								}
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:SeedShop_Yoba", new object[0]));
								return true;
							}
						}
						else
						{
							if (!(text == "NextMineLevel"))
							{
								return true;
							}
							goto IL_21E5;
						}
					}
					else if (num <= 2250926415u)
					{
						if (num != 2039622173u)
						{
							if (num != 2050472952u)
							{
								if (num != 2250926415u)
								{
									return true;
								}
								if (!(text == "Tutorial"))
								{
									return true;
								}
								Game1.activeClickableMenu = new TutorialMenu();
								return true;
							}
							else
							{
								if (!(text == "DivorceBook"))
								{
									return true;
								}
								if (Game1.player.divorceTonight)
								{
									this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_CancelQuestion", new object[0]), this.createYesNoResponses(), "divorceCancel");
									return true;
								}
								if (Game1.player.isMarried())
								{
									this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_Question", new object[0]), this.createYesNoResponses(), "divorce");
									return true;
								}
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_NoSpouse", new object[0]));
								return true;
							}
						}
						else
						{
							if (!(text == "LockedDoorWarp"))
							{
								return true;
							}
							who.faceGeneralDirection(new Vector2((float)tileLocation.X, (float)tileLocation.Y) * (float)Game1.tileSize, 0);
							this.lockedDoorWarp(actionParams);
							return true;
						}
					}
					else if (num != 2279498422u)
					{
						if (num != 2295680585u)
						{
							if (num != 2413466880u)
							{
								return true;
							}
							if (!(text == "RailroadBox"))
							{
								return true;
							}
							if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 394 && !who.hasOrWillReceiveMail("TH_Railroad") && who.hasOrWillReceiveMail("TH_Tunnel"))
							{
								who.reduceActiveItemByOne();
								Game1.player.CanMove = false;
								Game1.playSound("Ship");
								Game1.player.mailReceived.Add("TH_Railroad");
								Game1.multipleDialogues(new string[]
								{
									Game1.content.LoadString("Strings\\Locations:Railroad_Box_ConsumeShell", new object[0]),
									Game1.content.LoadString("Strings\\Locations:Railroad_Box_MrQiNote", new object[0])
								});
								Game1.player.removeQuest(2);
								Game1.player.addQuest(3);
								return true;
							}
							if (who.hasOrWillReceiveMail("TH_Railroad"))
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Railroad_Box_MrQiNote", new object[0]));
								return true;
							}
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Railroad_Box_Initial", new object[0]));
							return true;
						}
						else
						{
							if (!(text == "Door"))
							{
								return true;
							}
							if (actionParams.Length <= 1 || Game1.eventUp)
							{
								this.openDoor(tileLocation, true);
								return false;
							}
							for (int j = 1; j < actionParams.Length; j++)
							{
								if (who.getFriendshipHeartLevelForNPC(actionParams[j]) >= 2 || Game1.player.mailReceived.Contains("doorUnlock" + actionParams[j]))
								{
									Rumble.rumble(0.1f, 100f);
									if (!Game1.player.mailReceived.Contains("doorUnlock" + actionParams[j]))
									{
										Game1.player.mailReceived.Add("doorUnlock" + actionParams[j]);
									}
									this.openDoor(tileLocation, true);
									return false;
								}
							}
							if (actionParams.Length == 2 && Game1.getCharacterFromName(actionParams[1], false) != null)
							{
								NPC character = Game1.getCharacterFromName(actionParams[1], false);
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + ((character.gender == 0) ? "Male" : "Female"), new object[]
								{
									character.name
								}));
								return true;
							}
							if (Game1.getCharacterFromName(actionParams[1], false) != null && Game1.getCharacterFromName(actionParams[2], false) != null)
							{
								NPC character2 = Game1.getCharacterFromName(actionParams[1], false);
								NPC character3 = Game1.getCharacterFromName(actionParams[2], false);
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", new object[]
								{
									character2.name,
									character3.name
								}));
								return true;
							}
							return true;
						}
					}
					else if (!(text == "WarpGreenhouse"))
					{
						return true;
					}
					if (Game1.player.mailReceived.Contains("ccPantry"))
					{
						who.faceGeneralDirection(new Vector2((float)tileLocation.X, (float)tileLocation.Y) * (float)Game1.tileSize, 0);
						Game1.warpFarmer("Greenhouse", 10, 23, false);
						Game1.playSound("doorClose");
						return true;
					}
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Farm_GreenhouseRuins", new object[0]));
					return true;
				}
				else if (num <= 3371180897u)
				{
					if (num <= 2909376585u)
					{
						if (num <= 2764184545u)
						{
							if (num <= 2510785065u)
							{
								if (num != 2471112148u)
								{
									if (num != 2510785065u)
									{
										return true;
									}
									if (!(text == "EvilShrineCenter"))
									{
										return true;
									}
									if (who.isDivorced())
									{
										this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineCenter", new object[0]), this.createYesNoResponses(), "evilShrineCenter");
										return true;
									}
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineCenterInactive", new object[0]));
									return true;
								}
								else
								{
									if (!(text == "FarmerFile"))
									{
										return true;
									}
									goto IL_25B9;
								}
							}
							else if (num != 2528917107u)
							{
								if (num != 2738932126u)
								{
									if (num != 2764184545u)
									{
										return true;
									}
									if (!(text == "MinecartTransport"))
									{
										return true;
									}
									if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
									{
										Response[] destinations;
										if (Game1.player.mailReceived.Contains("ccCraftsRoom"))
										{
											destinations = new Response[]
											{
												new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town", new object[0])),
												new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
												new Response("Quarry", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Quarry", new object[0])),
												new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
											};
										}
										else
										{
											destinations = new Response[]
											{
												new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town", new object[0])),
												new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
												new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
											};
										}
										this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination", new object[0]), destinations, "Minecart");
										return true;
									}
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder", new object[0]));
									return true;
								}
								else
								{
									if (!(text == "ClubSeller"))
									{
										return true;
									}
									this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_ClubSeller", new object[0]), new Response[]
									{
										new Response("I'll", Game1.content.LoadString("Strings\\Locations:Club_ClubSeller_Yes", new object[0])),
										new Response("No", Game1.content.LoadString("Strings\\Locations:Club_ClubSeller_No", new object[0]))
									}, "ClubSeller");
									return true;
								}
							}
							else
							{
								if (!(text == "StorageBox"))
								{
									return true;
								}
								this.openStorageBox(this.actionParamsToString(actionParams));
								return true;
							}
						}
						else if (num <= 2815316590u)
						{
							if (num != 2802141700u)
							{
								if (num != 2815316590u)
								{
									return true;
								}
								if (!(text == "MayorFridge"))
								{
									return true;
								}
								int numberFound = 0;
								for (int k = 0; k < who.items.Count; k++)
								{
									if (who.items[k] != null && who.items[k].parentSheetIndex == 284)
									{
										numberFound += who.items[k].Stack;
									}
								}
								if (numberFound >= 10 && !who.hasOrWillReceiveMail("TH_MayorFridge") && who.hasOrWillReceiveMail("TH_Railroad"))
								{
									int numRemoved = 0;
									for (int l = 0; l < who.items.Count; l++)
									{
										if (who.items[l] != null && who.items[l].parentSheetIndex == 284)
										{
											while (numRemoved < 10)
											{
												Item expr_1581 = who.items[l];
												int stack = expr_1581.Stack;
												expr_1581.Stack = stack - 1;
												numRemoved++;
												if (who.items[l].Stack == 0)
												{
													who.items[l] = null;
													break;
												}
											}
											if (numRemoved >= 10)
											{
												break;
											}
										}
									}
									Game1.player.CanMove = false;
									Game1.playSound("coin");
									Game1.player.mailReceived.Add("TH_MayorFridge");
									Game1.multipleDialogues(new string[]
									{
										Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_ConsumeBeets", new object[0]),
										Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_MrQiNote", new object[0])
									});
									Game1.player.removeQuest(3);
									Game1.player.addQuest(4);
									return true;
								}
								if (who.hasOrWillReceiveMail("TH_MayorFridge"))
								{
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_MrQiNote", new object[0]));
									return true;
								}
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_Initial", new object[0]));
								return true;
							}
							else
							{
								if (!(text == "Billboard"))
								{
									return true;
								}
								Game1.activeClickableMenu = new Billboard(actionParams[1].Equals("3"));
								return true;
							}
						}
						else if (num != 2817094304u)
						{
							if (num != 2832988535u)
							{
								if (num != 2909376585u)
								{
									return true;
								}
								if (!(text == "Saloon"))
								{
									return true;
								}
								if (who.getTileY() > tileLocation.Y)
								{
									this.saloon(tileLocation);
									return true;
								}
								return true;
							}
							else
							{
								if (!(text == "AdventureShop"))
								{
									return true;
								}
								this.adventureShop();
								return true;
							}
						}
						else
						{
							if (!(text == "Incubator"))
							{
								return true;
							}
							(this as AnimalHouse).incubator();
							return true;
						}
					}
					else if (num <= 3158608557u)
					{
						if (num <= 2959114096u)
						{
							if (num != 2920208772u)
							{
								if (num != 2959114096u)
								{
									return true;
								}
								if (!(text == "GetLumber"))
								{
									return true;
								}
								this.GetLumber();
								return true;
							}
							else
							{
								if (!(text == "Message"))
								{
									return true;
								}
								goto IL_1E3C;
							}
						}
						else if (num != 2987480683u)
						{
							if (num != 3155064199u)
							{
								if (num != 3158608557u)
								{
									return true;
								}
								if (!(text == "Starpoint"))
								{
									return true;
								}
								try
								{
									this.doStarpoint(actionParams[1]);
									return true;
								}
								catch (Exception)
								{
									return true;
								}
							}
							else
							{
								if (!(text == "farmstand"))
								{
									return true;
								}
								Game1.shipHeldItem();
								return true;
							}
						}
						else
						{
							if (!(text == "Mailbox"))
							{
								return true;
							}
							this.mailbox();
							return true;
						}
					}
					else if (num <= 3244018497u)
					{
						if (num != 3162274371u)
						{
							if (num != 3211203767u)
							{
								if (num != 3244018497u)
								{
									return true;
								}
								if (!(text == "WarpCommunityCenter"))
								{
									return true;
								}
								if (who.mailReceived.Contains("ccDoorUnlock") || who.mailReceived.Contains("JojaMember"))
								{
									Game1.warpFarmer("CommunityCenter", 32, 23, false);
									Game1.playSound("doorClose");
									return true;
								}
								Game1.drawObjectDialogue("It's locked.");
								return true;
							}
							else
							{
								if (!(text == "HMTGF"))
								{
									return true;
								}
								if (who.ActiveObject != null && who.ActiveObject != null && !who.ActiveObject.bigCraftable && who.ActiveObject.parentSheetIndex == 155)
								{
									who.ActiveObject = new Object(Vector2.Zero, 155, false);
									Game1.playSound("discoverMineral");
									Game1.flashAlpha = 1f;
									return true;
								}
								return true;
							}
						}
						else
						{
							if (!(text == "MineSign"))
							{
								return true;
							}
							Game1.drawObjectDialogue(Game1.parseText(this.actionParamsToString(actionParams)));
							return true;
						}
					}
					else if (num != 3327972754u)
					{
						if (num != 3329002772u)
						{
							if (num != 3371180897u)
							{
								return true;
							}
							if (!(text == "Craft"))
							{
								return true;
							}
							GameLocation.openCraftingMenu(this.actionParamsToString(actionParams));
							return true;
						}
						else
						{
							if (!(text == "Minecart"))
							{
								return true;
							}
							this.openChest(tileLocation, 4, Game1.random.Next(3, 7));
							return true;
						}
					}
					else
					{
						if (!(text == "TunnelSafe"))
						{
							return true;
						}
						if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 787 && !who.hasOrWillReceiveMail("TH_Tunnel"))
						{
							who.reduceActiveItemByOne();
							Game1.player.CanMove = false;
							Game1.playSound("openBox");
							DelayedAction.playSoundAfterDelay("doorCreakReverse", 500);
							Game1.player.mailReceived.Add("TH_Tunnel");
							Game1.multipleDialogues(new string[]
							{
								Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_ConsumeBattery", new object[0]),
								Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_MrQiNote", new object[0])
							});
							Game1.player.addQuest(2);
							return true;
						}
						if (who.hasOrWillReceiveMail("TH_Tunnel"))
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_MrQiNote", new object[0]));
							return true;
						}
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_Initial", new object[0]));
						return true;
					}
				}
				else if (num <= 3912414904u)
				{
					if (num <= 3424064554u)
					{
						if (num <= 3385614082u)
						{
							if (num != 3372476774u)
							{
								if (num != 3385614082u)
								{
									return true;
								}
								if (!(text == "playSound"))
								{
									return true;
								}
								goto IL_1F10;
							}
							else
							{
								if (!(text == "LumberPile"))
								{
									return true;
								}
								if (!who.hasOrWillReceiveMail("TH_LumberPile") && who.hasOrWillReceiveMail("TH_SandDragon"))
								{
									Game1.player.hasClubCard = true;
									Game1.player.CanMove = false;
									Game1.player.mailReceived.Add("TH_LumberPile");
									Game1.player.addItemByMenuIfNecessaryElseHoldUp(new SpecialItem(1, 2, ""), null);
									Game1.player.removeQuest(5);
									return true;
								}
								return true;
							}
						}
						else if (num != 3403315211u)
						{
							if (num != 3419754368u)
							{
								if (num != 3424064554u)
								{
									return true;
								}
								if (!(text == "Gunther"))
								{
									return true;
								}
								this.gunther();
								return true;
							}
							else
							{
								if (!(text == "Material"))
								{
									return true;
								}
								Game1.drawObjectDialogue(string.Concat(new object[]
								{
									"Material Stockpile: \n     ",
									who.WoodPieces,
									" pieces of lumber\n     ",
									who.StonePieces,
									" pieces of stone."
								}));
								return true;
							}
						}
						else
						{
							if (!(text == "Buy"))
							{
								return true;
							}
							if (who.getTileY() >= tileLocation.Y)
							{
								this.openShopMenu(actionParams[1]);
								return true;
							}
							return true;
						}
					}
					else if (num <= 3642125430u)
					{
						if (num != 3603749081u)
						{
							if (num != 3642125430u)
							{
								return true;
							}
							if (!(text == "ExitMine"))
							{
								return true;
							}
							Response[] responses = new Response[]
							{
								new Response("Leave", Game1.content.LoadString("Strings\\Locations:Mines_LeaveMine", new object[0])),
								new Response("Go", Game1.content.LoadString("Strings\\Locations:Mines_GoUp", new object[0])),
								new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing", new object[0]))
							};
							this.createQuestionDialogue(" ", responses, "ExitMine");
							return true;
						}
						else
						{
							if (!(text == "EnterSewer"))
							{
								return true;
							}
							if (who.hasRustyKey && !who.mailReceived.Contains("OpenedSewer"))
							{
								Game1.playSound("openBox");
								Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Forest_OpenedSewer", new object[0])));
								who.mailReceived.Add("OpenedSewer");
								return true;
							}
							if (who.mailReceived.Contains("OpenedSewer"))
							{
								Game1.warpFarmer("Sewer", 16, 11, 2);
								Game1.playSound("stairsdown");
								return true;
							}
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor", new object[0]));
							return true;
						}
					}
					else if (num != 3754457510u)
					{
						if (num != 3848897750u)
						{
							if (num != 3912414904u)
							{
								return true;
							}
							if (!(text == "DwarfGrave"))
							{
								return true;
							}
							if (who.canUnderstandDwarves)
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_DwarfGrave_Translated", new object[0]).Replace('\n', '^'));
								return true;
							}
							Game1.drawObjectDialogue("Unop dunyuu doo pusutn snaus^Op hanp o toeday na doo smol^Vhu lonozol yenn huot olait tol");
							return true;
						}
						else
						{
							if (!(text == "Mine"))
							{
								return true;
							}
							goto IL_21E5;
						}
					}
					else
					{
						if (!(text == "SkullDoor"))
						{
							return true;
						}
						if (!who.hasSkullKey)
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:SkullCave_SkullDoor_Locked", new object[0]));
							return true;
						}
						if (!who.hasUnlockedSkullDoor)
						{
							Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:SkullCave_SkullDoor_Unlock", new object[0])));
							DelayedAction.playSoundAfterDelay("openBox", 500);
							DelayedAction.playSoundAfterDelay("openBox", 700);
							Game1.addMailForTomorrow("skullCave", false, false);
							who.hasUnlockedSkullDoor = true;
							who.completeQuest(19);
							return true;
						}
						who.completelyStopAnimatingOrDoingAction();
						Game1.playSound("doorClose");
						DelayedAction.playSoundAfterDelay("stairsdown", 500);
						Game1.enterMine(true, 121, null);
						return true;
					}
				}
				else if (num <= 4067857873u)
				{
					if (num <= 3970342769u)
					{
						if (num != 3961338715u)
						{
							if (num != 3970342769u)
							{
								return true;
							}
							if (!(text == "ClubShop"))
							{
								return true;
							}
							Game1.activeClickableMenu = new ShopMenu(Utility.getQiShopStock(), 2, null);
							return true;
						}
						else
						{
							if (!(text == "Lamp"))
							{
								return true;
							}
							if (this.lightLevel == 0f)
							{
								this.lightLevel = 0.6f;
							}
							else
							{
								this.lightLevel = 0f;
							}
							Game1.playSound("openBox");
							return true;
						}
					}
					else if (num != 3978811393u)
					{
						if (num != 4012092003u)
						{
							if (num != 4067857873u)
							{
								return true;
							}
							if (!(text == "EvilShrineLeft"))
							{
								return true;
							}
							if (who.getChildren().Count<Child>() == 0)
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineLeftInactive", new object[0]));
								return true;
							}
							this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineLeft", new object[0]), this.createYesNoResponses(), "evilShrineLeft");
							return true;
						}
						else
						{
							if (!(text == "Arcade_Minecart"))
							{
								return true;
							}
							if (who.hasSkullKey)
							{
								Response[] junimoKartOptions = new Response[]
								{
									new Response("Progress", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_ProgressMode", new object[0])),
									new Response("Endless", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_EndlessMode", new object[0])),
									new Response("Exit", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Exit", new object[0]))
								};
								this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Menu", new object[0]), junimoKartOptions, "MinecartGame");
								return true;
							}
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Inactive", new object[0]));
							return true;
						}
					}
					else
					{
						if (!(text == "AnimalShop"))
						{
							return true;
						}
						if (who.getTileY() > tileLocation.Y)
						{
							this.animalShop(tileLocation);
							return true;
						}
						return true;
					}
				}
				else if (num <= 4097465949u)
				{
					if (num != 4073653847u)
					{
						if (num != 4090469326u)
						{
							if (num != 4097465949u)
							{
								return true;
							}
							if (!(text == "EmilyRoomObject"))
							{
								return true;
							}
							if (!Game1.player.eventsSeen.Contains(463391) || (Game1.player.spouse != null && Game1.player.spouse.Equals("Emily")))
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:HaleyHouse_EmilyRoomObject", new object[0]));
								return true;
							}
							TemporaryAnimatedSprite t = this.getTemporarySpriteByID(5858585);
							if (t != null && t is EmilysParrot)
							{
								(t as EmilysParrot).doAction();
								return true;
							}
							return true;
						}
						else
						{
							if (!(text == "RemoveChest"))
							{
								return true;
							}
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:RemoveChest", new object[0]));
							this.map.GetLayer("Buildings").Tiles[tileLocation.X, tileLocation.Y] = null;
							return true;
						}
					}
					else
					{
						if (!(text == "ItemChest"))
						{
							return true;
						}
						this.openItemChest(tileLocation, Convert.ToInt32(actionParams[1]));
						return true;
					}
				}
				else if (num != 4104253281u)
				{
					if (num != 4212892660u)
					{
						if (num != 4284593235u)
						{
							return true;
						}
						if (!(text == "MineWallDecor"))
						{
							return true;
						}
						this.getWallDecorItem(tileLocation);
						return true;
					}
					else
					{
						if (!(text == "WarpWomensLocker"))
						{
							return true;
						}
						if (who.isMale)
						{
							if (who.IsMainPlayer)
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WomensLocker_WrongGender", new object[0]));
							}
							return false;
						}
						who.faceGeneralDirection(new Vector2((float)tileLocation.X, (float)tileLocation.Y) * (float)Game1.tileSize, 0);
						Game1.warpFarmer(actionParams[3], Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]), false);
						if (actionParams.Length < 5)
						{
							Game1.playSound("doorClose");
							return true;
						}
						return true;
					}
				}
				else
				{
					if (!(text == "ElliottBook"))
					{
						return true;
					}
					if (who.eventsSeen.Contains(41))
					{
						Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ElliottHouse_ElliottBook_Filled", new object[]
						{
							Game1.elliottBookName,
							who.name
						})));
						return true;
					}
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ElliottHouse_ElliottBook_Blank", new object[0]));
					return true;
				}
				IL_EEF:
				Game1.activeClickableMenu = new ShopMenu(Utility.getJojaStock(), 0, null);
				return true;
				IL_1E3C:
				Game1.drawDialogueNoTyping(this.actionParamsToString(actionParams));
				return true;
				IL_1F10:
				Game1.playSound(actionParams[1]);
				return true;
				IL_21E5:
				Game1.playSound("stairsdown");
				Game1.enterMine(this.name.Equals("Mine"), 1, null);
				return true;
				IL_25B9:
				this.farmerFile();
				return true;
			}
			if (action != null && !who.IsMainPlayer)
			{
				string text = action.ToString().Split(new char[]
				{
					' '
				})[0];
				if (!(text == "Minecart"))
				{
					if (!(text == "RemoveChest"))
					{
						if (!(text == "Door"))
						{
							if (text == "TV")
							{
								Game1.tvStation = Game1.random.Next(2);
							}
						}
						else
						{
							this.openDoor(tileLocation, true);
						}
					}
					else
					{
						this.map.GetLayer("Buildings").Tiles[tileLocation.X, tileLocation.Y] = null;
					}
				}
				else
				{
					this.openChest(tileLocation, 4, Game1.random.Next(3, 7));
				}
			}
			return false;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0005D584 File Offset: 0x0005B784
		public Vector2 findNearestObject(Vector2 startingPoint, int objectIndex, bool bigCraftable)
		{
			int attempts = 0;
			Queue<Vector2> openList = new Queue<Vector2>();
			openList.Enqueue(startingPoint);
			HashSet<Vector2> closedList = new HashSet<Vector2>();
			List<Vector2> adjacent = new List<Vector2>();
			while (attempts < 1000)
			{
				if (this.objects.ContainsKey(startingPoint) && this.objects[startingPoint].parentSheetIndex == objectIndex && this.objects[startingPoint].bigCraftable == bigCraftable)
				{
					return startingPoint;
				}
				attempts++;
				closedList.Add(startingPoint);
				adjacent = Utility.getAdjacentTileLocations(startingPoint);
				for (int i = 0; i < adjacent.Count; i++)
				{
					if (!closedList.Contains(adjacent[i]))
					{
						openList.Enqueue(adjacent[i]);
					}
				}
				startingPoint = openList.Dequeue();
			}
			return Vector2.Zero;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0005D650 File Offset: 0x0005B850
		public void lockedDoorWarp(string[] actionParams)
		{
			if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason) && Utility.getStartTimeOfFestival() < 1900)
			{
				Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:FestivalDay_DoorLocked", new object[0])));
				return;
			}
			if (actionParams[3].Equals("SeedShop") && Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Wed") && !Game1.player.eventsSeen.Contains(191393))
			{
				Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:SeedShop_LockedWed", new object[0])));
				return;
			}
			if (Game1.timeOfDay >= Convert.ToInt32(actionParams[4]) && Game1.timeOfDay < Convert.ToInt32(actionParams[5]) && (actionParams.Length < 7 || Game1.currentSeason.Equals("winter") || (Game1.player.friendships.ContainsKey(actionParams[6]) && Game1.player.friendships[actionParams[6]][0] >= Convert.ToInt32(actionParams[7]))))
			{
				Rumble.rumble(0.15f, 200f);
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.warpFarmer(actionParams[3], Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]), false);
				Game1.playSound("doorClose");
				return;
			}
			if (actionParams.Length < 7)
			{
				string openTime = Game1.getTimeOfDayString(Convert.ToInt32(actionParams[4])).Replace(" ", "");
				string closeTime = Game1.getTimeOfDayString(Convert.ToInt32(actionParams[5])).Replace(" ", "");
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor_OpenRange", new object[]
				{
					openTime,
					closeTime
				}));
				return;
			}
			if (Game1.timeOfDay < Convert.ToInt32(actionParams[4]) || Game1.timeOfDay >= Convert.ToInt32(actionParams[5]))
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor", new object[0]));
				return;
			}
			NPC character = Game1.getCharacterFromName(actionParams[6], false);
			Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor_FriendsOnly", new object[]
			{
				character.name
			}));
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0005D868 File Offset: 0x0005BA68
		public void readNote(int which)
		{
			if (Game1.player.archaeologyFound.ContainsKey(102) && Game1.player.archaeologyFound[102][0] >= which)
			{
				string arg_9C_0 = Game1.content.LoadString("Strings\\Notes:" + which, new object[0]).Replace('\n', '^');
				if (!Game1.player.mailReceived.Contains("lb_" + which))
				{
					Game1.player.mailReceived.Add("lb_" + which);
				}
				this.removeTemporarySpritesWithID(which);
				Game1.drawLetterMessage(arg_9C_0);
				return;
			}
			Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Notes:Missing", new object[0])));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0005D938 File Offset: 0x0005BB38
		public void mailbox()
		{
			if (Game1.mailbox.Count <= 0 || Game1.player.ActiveObject != null)
			{
				if (Game1.mailbox.Count == 0)
				{
					Game1.drawObjectDialogue("You don't have any mail.");
				}
				return;
			}
			if (!Game1.player.mailReceived.Contains(Game1.mailbox.Peek()) && !Game1.mailbox.Peek().Contains("passedOut") && !Game1.mailbox.Peek().Contains("Cooking"))
			{
				Game1.player.mailReceived.Add(Game1.mailbox.Peek());
			}
			string mailTitle = Game1.mailbox.Dequeue();
			Dictionary<string, string> mails = Game1.content.Load<Dictionary<string, string>>("Data\\mail");
			string mail = mails.ContainsKey(mailTitle) ? mails[mailTitle] : "";
			if (mailTitle.Contains("passedOut"))
			{
				int moneyTaken = Convert.ToInt32(mailTitle.Split(new char[]
				{
					' '
				})[1]);
				switch (new Random(moneyTaken).Next((Game1.player.getSpouse() != null && Game1.player.getSpouse().name.Equals("Harvey")) ? 2 : 3))
				{
				case 0:
					mail = string.Format(mails["passedOut1_" + ((moneyTaken > 0) ? "Billed" : "NotBilled") + "_" + (Game1.player.isMale ? "Male" : "Female")], moneyTaken);
					break;
				case 1:
					mail = string.Format(mails["passedOut2"], moneyTaken);
					break;
				case 2:
					mail = string.Format(mails["passedOut3_" + ((moneyTaken > 0) ? "Billed" : "NotBilled")], moneyTaken);
					break;
				}
			}
			if (mail.Length == 0)
			{
				return;
			}
			mail = mail.Replace("@", Game1.player.Name);
			if (mail.Contains("%update"))
			{
				mail = mail.Replace("%update", Utility.getStardewHeroStandingsString());
			}
			Game1.activeClickableMenu = new LetterViewerMenu(mail, mailTitle);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0005DB74 File Offset: 0x0005BD74
		public void farmerFile()
		{
			Game1.multipleDialogues(new string[]
			{
				Game1.content.LoadString("Strings\\UI:FarmerFile_1", new object[]
				{
					Game1.player.name,
					Game1.stats.StepsTaken,
					Game1.stats.GiftsGiven,
					Game1.stats.DaysPlayed,
					Game1.stats.DirtHoed,
					Game1.stats.ItemsCrafted,
					Game1.stats.ItemsCooked,
					Game1.stats.PiecesOfTrashRecycled
				}).Replace('\n', '^'),
				Game1.content.LoadString("Strings\\UI:FarmerFile_2", new object[]
				{
					Game1.stats.MonstersKilled,
					Game1.stats.FishCaught,
					Game1.stats.TimesFished,
					Game1.stats.SeedsSown,
					Game1.stats.ItemsShipped
				}).Replace('\n', '^')
			});
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0005DCB4 File Offset: 0x0005BEB4
		public void openItemChest(Location location, int whichObject)
		{
			Game1.playSound("openBox");
			if (Game1.player.ActiveObject == null)
			{
				if (whichObject == 434)
				{
					Game1.player.ActiveObject = new Object(Vector2.Zero, 434, "Cosmic Fruit", false, false, false, false);
					Game1.eatHeldObject();
				}
				else
				{
					this.debris.Add(new Debris(whichObject, new Vector2((float)(location.X * Game1.tileSize), (float)(location.Y * Game1.tileSize)), Game1.player.position));
				}
				Tile expr_AA = this.map.GetLayer("Buildings").Tiles[location.X, location.Y];
				int tileIndex = expr_AA.TileIndex;
				expr_AA.TileIndex = tileIndex + 1;
				this.map.GetLayer("Buildings").Tiles[location].Properties["Action"] = new PropertyValue("RemoveChest");
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00002834 File Offset: 0x00000A34
		public void getWallDecorItem(Location location)
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0005DDB0 File Offset: 0x0005BFB0
		public static string getFavoriteItemName(string character)
		{
			string favoriteItem = "???";
			if (Game1.NPCGiftTastes.ContainsKey(character))
			{
				string[] favoriteItems = Game1.NPCGiftTastes[character].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				});
				favoriteItem = Game1.objectInformation[Convert.ToInt32(favoriteItems[Game1.random.Next(favoriteItems.Length)])].Split(new char[]
				{
					'/'
				})[0];
			}
			return favoriteItem;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0005DE30 File Offset: 0x0005C030
		public static void openCraftingMenu(string nameOfCraftingDevice)
		{
			Game1.activeClickableMenu = new GameMenu(4, -1);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00002834 File Offset: 0x00000A34
		private void openStorageBox(string which)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0005DE40 File Offset: 0x0005C040
		private void adventureShop()
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			int infiniteStock = 2147483647;
			stock.Add(new MeleeWeapon(12), new int[]
			{
				250,
				infiniteStock
			});
			if (Game1.mine != null)
			{
				if (Game1.mine.lowestLevelReached >= 15)
				{
					stock.Add(new MeleeWeapon(17), new int[]
					{
						500,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 20)
				{
					stock.Add(new MeleeWeapon(1), new int[]
					{
						750,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 25)
				{
					stock.Add(new MeleeWeapon(43), new int[]
					{
						850,
						infiniteStock
					});
					stock.Add(new MeleeWeapon(44), new int[]
					{
						1500,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 40)
				{
					stock.Add(new MeleeWeapon(27), new int[]
					{
						2000,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 45)
				{
					stock.Add(new MeleeWeapon(10), new int[]
					{
						2000,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 55)
				{
					stock.Add(new MeleeWeapon(7), new int[]
					{
						4000,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 75)
				{
					stock.Add(new MeleeWeapon(5), new int[]
					{
						6000,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 90)
				{
					stock.Add(new MeleeWeapon(50), new int[]
					{
						9000,
						infiniteStock
					});
				}
				if (Game1.mine.lowestLevelReached >= 120)
				{
					stock.Add(new MeleeWeapon(9), new int[]
					{
						25000,
						infiniteStock
					});
				}
				if (Game1.player.mailReceived.Contains("galaxySword"))
				{
					stock.Add(new MeleeWeapon(4), new int[]
					{
						50000,
						infiniteStock
					});
					stock.Add(new MeleeWeapon(23), new int[]
					{
						35000,
						infiniteStock
					});
					stock.Add(new MeleeWeapon(29), new int[]
					{
						75000,
						infiniteStock
					});
				}
			}
			stock.Add(new Boots(504), new int[]
			{
				500,
				infiniteStock
			});
			if (Game1.mine != null && Game1.mine.lowestLevelReached >= 40)
			{
				stock.Add(new Boots(508), new int[]
				{
					1250,
					infiniteStock
				});
			}
			if (Game1.mine != null && Game1.mine.lowestLevelReached >= 80)
			{
				stock.Add(new Boots(511), new int[]
				{
					2500,
					infiniteStock
				});
			}
			stock.Add(new Ring(529), new int[]
			{
				1000,
				infiniteStock
			});
			stock.Add(new Ring(530), new int[]
			{
				1000,
				infiniteStock
			});
			if (Game1.mine != null && Game1.mine.lowestLevelReached >= 40)
			{
				stock.Add(new Ring(531), new int[]
				{
					2500,
					infiniteStock
				});
				stock.Add(new Ring(532), new int[]
				{
					2500,
					infiniteStock
				});
			}
			if (Game1.mine != null && Game1.mine.lowestLevelReached >= 80)
			{
				stock.Add(new Ring(533), new int[]
				{
					5000,
					infiniteStock
				});
				stock.Add(new Ring(534), new int[]
				{
					5000,
					infiniteStock
				});
			}
			if (Game1.mine != null)
			{
				int arg_3F2_0 = Game1.mine.lowestLevelReached;
			}
			if (Game1.player.hasItemWithNameThatContains("Slingshot") != null)
			{
				stock.Add(new Object(441, 2147483647, false, -1, 0), new int[]
				{
					100,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring"))
			{
				stock.Add(new Ring(520), new int[]
				{
					25000,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Savage Ring"))
			{
				stock.Add(new Ring(523), new int[]
				{
					25000,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Burglar's Ring"))
			{
				stock.Add(new Ring(526), new int[]
				{
					20000,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Vampire Ring"))
			{
				stock.Add(new Ring(522), new int[]
				{
					15000,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Skeleton Mask"))
			{
				stock.Add(new Hat(8), new int[]
				{
					20000,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Hard Hat"))
			{
				stock.Add(new Hat(27), new int[]
				{
					20000,
					infiniteStock
				});
			}
			if (Game1.player.mailReceived.Contains("Gil_Insect Head"))
			{
				stock.Add(new MeleeWeapon(13), new int[]
				{
					10000,
					infiniteStock
				});
			}
			Game1.activeClickableMenu = new ShopMenu(stock, 0, "Marlon");
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0005E408 File Offset: 0x0005C608
		private void openShopMenu(string which)
		{
			if (which.Equals("Fish"))
			{
				if (this.getCharacterFromName("Willy") != null && this.getCharacterFromName("Willy").getTileLocation().Y < (float)Game1.player.getTileY())
				{
					Game1.activeClickableMenu = new ShopMenu(Utility.getFishShopStock(Game1.player), 0, "Willy");
					return;
				}
			}
			else if (this is SeedShop)
			{
				if (this.getCharacterFromName("Pierre") != null && this.getCharacterFromName("Pierre").getTileLocation().Equals(new Vector2(4f, 17f)) && Game1.player.getTileY() > this.getCharacterFromName("Pierre").getTileY())
				{
					Game1.activeClickableMenu = new ShopMenu((this as SeedShop).shopStock(), 0, "Pierre");
					return;
				}
				Game1.drawObjectDialogue("Come back when Pierre's tending the shop.");
				return;
			}
			else if (this.name.Equals("SandyHouse"))
			{
				Game1.activeClickableMenu = new ShopMenu(this.sandyShopStock(), 0, "Sandy");
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0005E51C File Offset: 0x0005C71C
		public virtual bool isObjectAt(int x, int y)
		{
			Vector2 v = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			return this.objects.ContainsKey(v);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0005E554 File Offset: 0x0005C754
		public virtual Object getObjectAt(int x, int y)
		{
			Vector2 v = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			if (this.objects.ContainsKey(v))
			{
				return this.objects[v];
			}
			return null;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0005E594 File Offset: 0x0005C794
		private List<Item> sandyShopStock()
		{
			List<Item> stock = new List<Item>();
			stock.Add(new Object(478, 2147483647, false, -1, 0));
			stock.Add(new Object(486, 2147483647, false, -1, 0));
			stock.Add(new Object(494, 2147483647, false, -1, 0));
			switch (Game1.dayOfMonth % 7)
			{
			case 0:
				stock.Add(new Object(233, 2147483647, false, -1, 0));
				break;
			case 1:
				stock.Add(new Object(88, 2147483647, false, -1, 0));
				break;
			case 2:
				stock.Add(new Object(90, 2147483647, false, -1, 0));
				break;
			case 3:
				stock.Add(new Object(749, 2147483647, false, 500, 0));
				break;
			case 4:
				stock.Add(new Object(466, 2147483647, false, -1, 0));
				break;
			case 5:
				stock.Add(new Object(340, 2147483647, false, -1, 0));
				break;
			case 6:
				stock.Add(new Object(371, 2147483647, false, 100, 0));
				break;
			}
			return stock;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0005E6D8 File Offset: 0x0005C8D8
		private void saloon(Location tileLocation)
		{
			foreach (NPC i in this.characters)
			{
				if (i.name.Equals("Gus"))
				{
					if (i.getTileY() != Game1.player.getTileY() - 1 && i.getTileY() != Game1.player.getTileY() - 2)
					{
						break;
					}
					i.facePlayer(Game1.player);
					Game1.activeClickableMenu = new ShopMenu(Utility.getSaloonStock(), 0, "Gus");
					break;
				}
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0005E784 File Offset: 0x0005C984
		private void carpenters(Location tileLocation)
		{
			if (Game1.player.currentUpgrade == null)
			{
				foreach (NPC i in this.characters)
				{
					if (i.name.Equals("Robin"))
					{
						if (Vector2.Distance(i.getTileLocation(), new Vector2((float)tileLocation.X, (float)tileLocation.Y)) > 3f)
						{
							return;
						}
						i.faceDirection(2);
						if (Game1.player.daysUntilHouseUpgrade < 0 && !Game1.getFarm().isThereABuildingUnderConstruction())
						{
							Response[] options;
							if (Game1.player.houseUpgradeLevel < 3)
							{
								options = new Response[]
								{
									new Response("Shop", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Shop", new object[0])),
									new Response("Upgrade", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_UpgradeHouse", new object[0])),
									new Response("Construct", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Construct", new object[0])),
									new Response("Leave", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Leave", new object[0]))
								};
							}
							else
							{
								options = new Response[]
								{
									new Response("Shop", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Shop", new object[0])),
									new Response("Construct", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Construct", new object[0])),
									new Response("Leave", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Leave", new object[0]))
								};
							}
							this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu", new object[0]), options, "carpenter");
							return;
						}
						Game1.activeClickableMenu = new ShopMenu(Utility.getCarpenterStock(), 0, "Robin");
						return;
					}
				}
				if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Tue"))
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_RobinAbsent", new object[0]).Replace('\n', '^'));
				}
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0005E9D8 File Offset: 0x0005CBD8
		private void blacksmith(Location tileLocation)
		{
			foreach (NPC i in this.characters)
			{
				if (i.name.Equals("Clint"))
				{
					if (!i.getTileLocation().Equals(new Vector2((float)tileLocation.X, (float)(tileLocation.Y - 1))))
					{
						i.getTileLocation().Equals(new Vector2((float)(tileLocation.X - 1), (float)(tileLocation.Y - 1)));
					}
					i.faceDirection(2);
					if (Game1.player.toolBeingUpgraded == null)
					{
						Response[] responses;
						if (Game1.player.hasItemInInventory(535, 1, 0) || Game1.player.hasItemInInventory(536, 1, 0) || Game1.player.hasItemInInventory(537, 1, 0) || Game1.player.hasItemInInventory(749, 1, 0))
						{
							responses = new Response[]
							{
								new Response("Shop", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Shop", new object[0])),
								new Response("Upgrade", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Upgrade", new object[0])),
								new Response("Process", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Geodes", new object[0])),
								new Response("Leave", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Leave", new object[0]))
							};
						}
						else
						{
							responses = new Response[]
							{
								new Response("Shop", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Shop", new object[0])),
								new Response("Upgrade", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Upgrade", new object[0])),
								new Response("Leave", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Leave", new object[0]))
							};
						}
						this.createQuestionDialogue("", responses, "Blacksmith");
						break;
					}
					if (Game1.player.daysLeftForToolUpgrade > 0)
					{
						Game1.drawDialogue(i, Game1.content.LoadString("Data\\ExtraDialogue:Clint_StillWorking", new object[]
						{
							Game1.player.toolBeingUpgraded.Name
						}));
						break;
					}
					if (Game1.player.freeSpotsInInventory() > 0)
					{
						Game1.player.holdUpItemThenMessage(Game1.player.toolBeingUpgraded, true);
						Game1.player.addItemToInventoryBool(Game1.player.toolBeingUpgraded, false);
						Game1.player.toolBeingUpgraded = null;
						break;
					}
					Game1.drawDialogue(i, Game1.content.LoadString("Data\\ExtraDialogue:Clint_NoInventorySpace", new object[0]));
					break;
				}
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0005ECB0 File Offset: 0x0005CEB0
		private void animalShop(Location tileLocation)
		{
			foreach (NPC i in this.characters)
			{
				if (i.name.Equals("Marnie"))
				{
					if (!i.getTileLocation().Equals(new Vector2((float)tileLocation.X, (float)(tileLocation.Y - 1))))
					{
						return;
					}
					i.faceDirection(2);
					Response[] options = new Response[]
					{
						new Response("Supplies", Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Supplies", new object[0])),
						new Response("Purchase", Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Animals", new object[0])),
						new Response("Leave", Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Leave", new object[0]))
					};
					this.createQuestionDialogue("", options, "Marnie");
					return;
				}
			}
			if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Tue"))
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Absent", new object[0]).Replace('\n', '^'));
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0005EE04 File Offset: 0x0005D004
		private void gunther()
		{
			if ((this as LibraryMuseum).doesFarmerHaveAnythingToDonate(Game1.player))
			{
				Response[] choice;
				if ((this as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > 0)
				{
					choice = new Response[]
					{
						new Response("Donate", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Donate", new object[0])),
						new Response("Collect", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Collect", new object[0])),
						new Response("Leave", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Leave", new object[0]))
					};
				}
				else
				{
					choice = new Response[]
					{
						new Response("Donate", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Donate", new object[0])),
						new Response("Leave", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Leave", new object[0]))
					};
				}
				this.createQuestionDialogue("", choice, "Museum");
				return;
			}
			if ((this as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > 0)
			{
				this.createQuestionDialogue("", new Response[]
				{
					new Response("Collect", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Collect", new object[0])),
					new Response("Leave", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Leave", new object[0]))
				}, "Museum");
				return;
			}
			if (Game1.player.achievements.Contains(5))
			{
				Game1.drawDialogue(Game1.getCharacterFromName("Gunther", false), Game1.parseText(Game1.content.LoadString("Data\\ExtraDialogue:Gunther_MuseumComplete", new object[0])));
				return;
			}
			Game1.drawDialogue(Game1.getCharacterFromName("Gunther", false), Game1.player.mailReceived.Contains("artifactFound") ? Game1.parseText(Game1.content.LoadString("Data\\ExtraDialogue:Gunther_NothingToDonate", new object[0])) : Game1.content.LoadString("Data\\ExtraDialogue:Gunther_NoArtifactsFound", new object[0]));
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0005F014 File Offset: 0x0005D214
		public void openChest(Location location, int debrisType, int numberOfChunks)
		{
			int[] debris = new int[]
			{
				debrisType
			};
			this.openChest(location, debris, numberOfChunks);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0005F038 File Offset: 0x0005D238
		public void openChest(Location location, int[] debrisType, int numberOfChunks)
		{
			Game1.playSound("openBox");
			Tile expr_30 = this.map.GetLayer("Buildings").Tiles[location.X, location.Y];
			int tileIndex = expr_30.TileIndex;
			expr_30.TileIndex = tileIndex + 1;
			for (int i = 0; i < debrisType.Length; i++)
			{
				Game1.createDebris(debrisType[i], location.X, location.Y, numberOfChunks, null);
			}
			this.map.GetLayer("Buildings").Tiles[location].Properties["Action"] = new PropertyValue("RemoveChest");
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0005F0DC File Offset: 0x0005D2DC
		public string actionParamsToString(string[] actionparams)
		{
			string str = actionparams[1];
			for (int i = 2; i < actionparams.Length; i++)
			{
				str = str + " " + actionparams[i];
			}
			return str;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0005F10C File Offset: 0x0005D30C
		private void GetLumber()
		{
			if (Game1.player.ActiveObject == null && Game1.player.WoodPieces > 0)
			{
				Game1.player.grabObject(new Object(Vector2.Zero, 30, "Lumber", true, true, false, false));
				Farmer expr_3D = Game1.player;
				int woodPieces = expr_3D.WoodPieces;
				expr_3D.WoodPieces = woodPieces - 1;
				return;
			}
			if (Game1.player.ActiveObject != null && Game1.player.ActiveObject.Name.Equals("Lumber"))
			{
				Game1.player.CanMove = false;
				switch (Game1.player.FacingDirection)
				{
				case 0:
					((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(80, 75f);
					break;
				case 1:
					((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(72, 75f);
					break;
				case 2:
					((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(64, 75f);
					break;
				case 3:
					((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(88, 75f);
					break;
				}
				Game1.player.ActiveObject = null;
				Farmer expr_12A = Game1.player;
				int woodPieces = expr_12A.WoodPieces;
				expr_12A.WoodPieces = woodPieces + 1;
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0005F252 File Offset: 0x0005D452
		public void removeTile(Location tileLocation, string layer)
		{
			this.map.GetLayer(layer).Tiles[tileLocation.X, tileLocation.Y] = null;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0005F277 File Offset: 0x0005D477
		public void removeTile(int x, int y, string layer)
		{
			this.map.GetLayer(layer).Tiles[x, y] = null;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0005F294 File Offset: 0x0005D494
		public bool characterDestroyObjectWithinRectangle(Microsoft.Xna.Framework.Rectangle rect, bool showDestroyedObject)
		{
			if (this is FarmHouse)
			{
				return false;
			}
			if (rect.Intersects(Game1.player.GetBoundingBox()))
			{
				return false;
			}
			Vector2 tilePositionToTry = new Vector2((float)(rect.X / Game1.tileSize), (float)(rect.Y / Game1.tileSize));
			Object o;
			this.objects.TryGetValue(tilePositionToTry, out o);
			if (o != null && !o.IsHoeDirt && !o.isPassable() && !this.map.GetLayer("Back").Tiles[(int)tilePositionToTry.X, (int)tilePositionToTry.Y].Properties.ContainsKey("NPCBarrier"))
			{
				if (!o.IsHoeDirt)
				{
					if (o.IsSpawnedObject)
					{
						this.numberOfSpawnedObjectsOnMap--;
					}
					if (showDestroyedObject && !o.bigCraftable)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(o.ParentSheetIndex, 150f, 1, 3, new Vector2(tilePositionToTry.X * (float)Game1.tileSize, tilePositionToTry.Y * (float)Game1.tileSize), false, o.flipped)
						{
							alphaFade = 0.01f
						});
					}
					o.performToolAction(null);
					if (!(o is Chest) && this.objects.ContainsKey(tilePositionToTry))
					{
						this.objects.Remove(tilePositionToTry);
					}
				}
				return true;
			}
			tilePositionToTry.X = (float)(rect.Right / Game1.tileSize);
			this.objects.TryGetValue(tilePositionToTry, out o);
			if (o != null && !o.IsHoeDirt && !o.isPassable() && !this.map.GetLayer("Back").Tiles[(int)tilePositionToTry.X, (int)tilePositionToTry.Y].Properties.ContainsKey("NPCBarrier"))
			{
				if (!o.IsHoeDirt)
				{
					if (o.IsSpawnedObject)
					{
						this.numberOfSpawnedObjectsOnMap--;
					}
					if (showDestroyedObject && !o.bigCraftable)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(o.ParentSheetIndex, 150f, 1, 3, new Vector2(tilePositionToTry.X * (float)Game1.tileSize, tilePositionToTry.Y * (float)Game1.tileSize), false, o.flipped)
						{
							alphaFade = 0.01f
						});
					}
					o.performToolAction(null);
					if (this.objects.ContainsKey(tilePositionToTry))
					{
						this.objects.Remove(tilePositionToTry);
					}
				}
				return true;
			}
			tilePositionToTry.X = (float)(rect.X / Game1.tileSize);
			tilePositionToTry.Y = (float)(rect.Bottom / Game1.tileSize);
			this.objects.TryGetValue(tilePositionToTry, out o);
			if (o != null && !o.IsHoeDirt && !o.isPassable() && !this.map.GetLayer("Back").Tiles[(int)tilePositionToTry.X, (int)tilePositionToTry.Y].Properties.ContainsKey("NPCBarrier"))
			{
				if (!o.IsHoeDirt)
				{
					if (o.IsSpawnedObject)
					{
						this.numberOfSpawnedObjectsOnMap--;
					}
					if (showDestroyedObject && !o.bigCraftable)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(o.ParentSheetIndex, 150f, 1, 3, new Vector2(tilePositionToTry.X * (float)Game1.tileSize, tilePositionToTry.Y * (float)Game1.tileSize), false, o.flipped)
						{
							alphaFade = 0.01f
						});
					}
					o.performToolAction(null);
					if (this.objects.ContainsKey(tilePositionToTry))
					{
						this.objects.Remove(tilePositionToTry);
					}
				}
				return true;
			}
			tilePositionToTry.X = (float)(rect.Right / Game1.tileSize);
			this.objects.TryGetValue(tilePositionToTry, out o);
			if (o != null && !o.IsHoeDirt && !o.isPassable() && !this.map.GetLayer("Back").Tiles[(int)tilePositionToTry.X, (int)tilePositionToTry.Y].Properties.ContainsKey("NPCBarrier"))
			{
				if (!o.IsHoeDirt)
				{
					if (o.IsSpawnedObject)
					{
						this.numberOfSpawnedObjectsOnMap--;
					}
					if (showDestroyedObject && !o.bigCraftable)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(o.ParentSheetIndex, 150f, 1, 3, new Vector2(tilePositionToTry.X * (float)Game1.tileSize, tilePositionToTry.Y * (float)Game1.tileSize), false, o.flipped)
						{
							alphaFade = 0.01f
						});
					}
					o.performToolAction(null);
					if (this.objects.ContainsKey(tilePositionToTry))
					{
						this.objects.Remove(tilePositionToTry);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0005F758 File Offset: 0x0005D958
		public Object removeObject(Vector2 location, bool showDestroyedObject)
		{
			Object o;
			this.objects.TryGetValue(location, out o);
			if (o != null && (o.CanBeGrabbed | showDestroyedObject))
			{
				if (o.IsSpawnedObject)
				{
					this.numberOfSpawnedObjectsOnMap--;
				}
				Object tmp = this.objects[location];
				this.objects.Remove(location);
				if (showDestroyedObject)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(tmp.Type.Equals("Crafting") ? tmp.ParentSheetIndex : (tmp.ParentSheetIndex + 1), 150f, 1, 3, new Vector2(location.X * (float)Game1.tileSize, location.Y * (float)Game1.tileSize), true, tmp.bigCraftable, tmp.flipped));
				}
				if (o.Name.Contains("Weed"))
				{
					Stats expr_D1 = Game1.stats;
					uint weedsEliminated = expr_D1.WeedsEliminated;
					expr_D1.WeedsEliminated = weedsEliminated + 1u;
					if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest"))
					{
						((WeedingQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
					}
				}
				return tmp;
			}
			return null;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0005F898 File Offset: 0x0005DA98
		public void setTileProperty(int tileX, int tileY, string layer, string key, string value)
		{
			try
			{
				if (!this.map.GetLayer(layer).Tiles[tileX, tileY].Properties.ContainsKey(key))
				{
					this.map.GetLayer(layer).Tiles[tileX, tileY].Properties.Add(key, new PropertyValue(value));
				}
				else
				{
					this.map.GetLayer(layer).Tiles[tileX, tileY].Properties[key] = value;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0005F938 File Offset: 0x0005DB38
		private void removeDirt(Vector2 location)
		{
			Object o;
			this.objects.TryGetValue(location, out o);
			if (o != null && o.IsHoeDirt)
			{
				this.objects.Remove(location);
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0005F96C File Offset: 0x0005DB6C
		public void removeBatch(List<Vector2> locations)
		{
			foreach (Vector2 v in locations)
			{
				this.objects.Remove(v);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0005F9C0 File Offset: 0x0005DBC0
		public void setObjectAt(float x, float y, Object o)
		{
			Vector2 v = new Vector2(x, y);
			if (this.objects.ContainsKey(v))
			{
				this.objects[v] = o;
				return;
			}
			this.objects.Add(v, o);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0005FA00 File Offset: 0x0005DC00
		public virtual void cleanupBeforePlayerExit()
		{
			Game1.currentLightSources.Clear();
			if (this.critters != null)
			{
				this.critters.Clear();
			}
			for (int i = Game1.onScreenMenus.Count - 1; i >= 0; i--)
			{
				if (Game1.onScreenMenus[i].destroy)
				{
					Game1.onScreenMenus.RemoveAt(i);
				}
			}
			if (Game1.currentSong != null && !Game1.currentSong.Name.ToLower().Contains(Game1.currentSeason) && !Game1.currentSong.Name.Contains("ambient") && !Game1.currentSong.Name.Equals("rain") && !Game1.eventUp && (Game1.locationAfterWarp == null || (this.isOutdoors && Game1.locationAfterWarp.isOutdoors)))
			{
				Game1.changeMusicTrack("none");
			}
			AmbientLocationSounds.onLocationLeave();
			if ((this.name.Equals("AnimalShop") || this.name.Equals("ScienceHouse")) && Game1.currentSong != null && Game1.currentSong.Name.Equals("marnieShop") && (Game1.locationAfterWarp == null || Game1.locationAfterWarp.IsOutdoors))
			{
				Game1.changeMusicTrack("none");
			}
			if (this.name.Equals("Saloon") && Game1.currentSong != null)
			{
				Game1.changeMusicTrack("none");
			}
			if ((this is LibraryMuseum || this is JojaMart) && Game1.currentSong != null)
			{
				Game1.changeMusicTrack("none");
			}
			if (this.name.Equals("Hospital") && Game1.currentSong != null && Game1.currentSong.Name.Equals("distantBanjo") && (Game1.locationAfterWarp == null || Game1.locationAfterWarp.IsOutdoors))
			{
				Game1.changeMusicTrack("none");
			}
			if (Game1.player.rightRing != null)
			{
				Game1.player.rightRing.onLeaveLocation(Game1.player, this);
			}
			if (Game1.player.leftRing != null)
			{
				Game1.player.leftRing.onLeaveLocation(Game1.player, this);
			}
			foreach (NPC j in this.characters)
			{
				if (j.uniqueSpriteActive)
				{
					j.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + j.name);
					j.uniqueSpriteActive = false;
				}
				if (j.uniquePortraitActive)
				{
					j.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + j.name);
					j.uniquePortraitActive = false;
				}
			}
			if (Game1.temporaryContent != null)
			{
				Game1.temporaryContent.Unload();
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0005FCC0 File Offset: 0x0005DEC0
		public static int getWeedForSeason(Random r, string season)
		{
			if (!(season == "spring"))
			{
				if (!(season == " summer"))
				{
					if (!(season == "fall"))
					{
						if (!(season == "winter"))
						{
						}
						return 674;
					}
					if (r.NextDouble() < 0.33)
					{
						return 786;
					}
					if (r.NextDouble() >= 0.5)
					{
						return 679;
					}
					return 678;
				}
				else
				{
					if (r.NextDouble() < 0.33)
					{
						return 785;
					}
					if (r.NextDouble() >= 0.5)
					{
						return 677;
					}
					return 676;
				}
			}
			else
			{
				if (r.NextDouble() < 0.33)
				{
					return 784;
				}
				if (r.NextDouble() >= 0.5)
				{
					return 675;
				}
				return 674;
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0005FDAC File Offset: 0x0005DFAC
		public virtual bool answerDialogueAction(string questionAndAnswer, string[] questionParams)
		{
			if (questionAndAnswer == null)
			{
				return false;
			}
			if (questionAndAnswer.Equals("Mine_Return to level " + Game1.player.deepestMineLevel))
			{
				Game1.mine.enterMine(Game1.player, Game1.player.deepestMineLevel, false);
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(questionAndAnswer);
			if (num <= 1823658541u)
			{
				if (num <= 799649041u)
				{
					if (num <= 455134809u)
					{
						if (num <= 247303129u)
						{
							if (num <= 134883726u)
							{
								if (num != 112637151u)
								{
									if (num != 134883726u)
									{
										return false;
									}
									if (!(questionAndAnswer == "ExitMine_Yes"))
									{
										return false;
									}
									goto IL_1FB4;
								}
								else
								{
									if (!(questionAndAnswer == "Mine_Enter"))
									{
										return false;
									}
									Game1.enterMine(false, 1, null);
									return true;
								}
							}
							else if (num != 245425821u)
							{
								if (num != 247303129u)
								{
									return false;
								}
								if (!(questionAndAnswer == "divorce_Yes"))
								{
									return false;
								}
								if (Game1.player.Money >= 50000)
								{
									Game1.player.Money -= 50000;
									Game1.player.divorceTonight = true;
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_Filed", new object[0]));
									return true;
								}
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "Eat_No"))
								{
									return false;
								}
								Game1.isEating = false;
								Game1.player.completelyStopAnimatingOrDoingAction();
								return true;
							}
						}
						else if (num <= 348312305u)
						{
							if (num != 279582718u)
							{
								if (num != 348312305u)
								{
									return false;
								}
								if (!(questionAndAnswer == "Quest_Yes"))
								{
									return false;
								}
								Game1.questOfTheDay.dailyQuest = true;
								Game1.questOfTheDay.accept();
								Game1.currentBillboard = 0;
								Game1.player.questLog.Add(Game1.questOfTheDay);
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "taxvote_Against"))
								{
									return false;
								}
								Game1.addMailForTomorrow("taxRejected", false, false);
								this.currentEvent.currentCommand++;
								return true;
							}
						}
						else if (num != 365638282u)
						{
							if (num != 455134809u)
							{
								return false;
							}
							if (!(questionAndAnswer == "Marnie_Supplies"))
							{
								return false;
							}
							Game1.activeClickableMenu = new ShopMenu(Utility.getAnimalShopStock(), 0, "Marnie");
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "ClearHouse_Yes"))
							{
								return false;
							}
							using (List<Vector2>.Enumerator enumerator = Game1.player.getAdjacentTiles().GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Vector2 v = enumerator.Current;
									if (this.objects.ContainsKey(v))
									{
										this.objects.Remove(v);
									}
								}
								return true;
							}
						}
					}
					else if (num <= 706840292u)
					{
						if (num <= 602270239u)
						{
							if (num != 564255041u)
							{
								if (num != 602270239u)
								{
									return false;
								}
								if (!(questionAndAnswer == "Shipping_Yes"))
								{
									return false;
								}
								Game1.player.shipAll();
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "BuyQiCoins_Yes"))
								{
									return false;
								}
								if (Game1.player.Money >= 1000)
								{
									Game1.player.Money -= 1000;
									Game1.playSound("Pickup_Coin15");
									Game1.player.clubCoins += 100;
									return true;
								}
								Game1.drawObjectDialogue("Error: Not enough money.");
								return true;
							}
						}
						else if (num != 606529510u)
						{
							if (num != 706840292u)
							{
								return false;
							}
							if (!(questionAndAnswer == "Dungeon_Go"))
							{
								return false;
							}
							Game1.enterMine(false, Game1.mine.mineLevel + 1, "Dungeon");
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "taxvote_For"))
							{
								return false;
							}
							Game1.shippingTax = true;
							Game1.addMailForTomorrow("taxPassed", false, false);
							this.currentEvent.currentCommand++;
							return true;
						}
					}
					else if (num <= 767799469u)
					{
						if (num != 761217684u)
						{
							if (num != 767799469u)
							{
								return false;
							}
							if (!(questionAndAnswer == "CalicoJackHS_Play"))
							{
								return false;
							}
							if (Game1.player.clubCoins >= 1000)
							{
								Game1.currentMinigame = new CalicoJack(-1, true);
								return true;
							}
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJackHS_NotEnoughCoins", new object[0]));
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "Blacksmith_Upgrade"))
							{
								return false;
							}
							Game1.activeClickableMenu = new ShopMenu(Utility.getBlacksmithUpgradeStock(Game1.player), 0, "ClintUpgrade");
							return true;
						}
					}
					else if (num != 778541444u)
					{
						if (num != 799649041u)
						{
							return false;
						}
						if (!(questionAndAnswer == "ExitMine_Go"))
						{
							return false;
						}
						Game1.enterMine(false, Game1.mine.mineLevel - 1, null);
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "carpenter_Shop"))
						{
							return false;
						}
						Game1.player.forceCanMove();
						Game1.activeClickableMenu = new ShopMenu(Utility.getCarpenterStock(), 0, "Robin");
						return true;
					}
				}
				else
				{
					if (num > 1098628691u)
					{
						if (num <= 1446971618u)
						{
							if (num <= 1299599336u)
							{
								if (num != 1157692071u)
								{
									if (num != 1299599336u)
									{
										return false;
									}
									if (!(questionAndAnswer == "evilShrineRightDeActivate_Yes"))
									{
										return false;
									}
									if (Game1.player.removeItemsFromInventory(203, 1))
									{
										this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(12 * Game1.tileSize + 3 * Game1.pixelZoom), (float)(6 * Game1.tileSize + Game1.pixelZoom)), false, 0f, Color.White)
										{
											interval = 50f,
											totalNumberOfLoops = 99999,
											animationLength = 7,
											layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
											scale = (float)Game1.pixelZoom
										});
										Game1.soundBank.PlayCue("fireball");
										for (int i = 0; i < 20; i++)
										{
											this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(12f, 6f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float)Game1.random.Next(Game1.tileSize / 4)), false, 0.002f, Color.DarkSlateBlue)
											{
												alpha = 0.75f,
												motion = new Vector2(0f, -0.5f),
												acceleration = new Vector2(-0.002f, 0f),
												interval = 99999f,
												layerDepth = (float)(6 * Game1.tileSize) / 10000f + (float)Game1.random.Next(100) / 10000f,
												scale = (float)(Game1.pixelZoom * 3) / 4f,
												scaleChange = 0.01f,
												rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f,
												delayBeforeAnimationStart = i * 25
											});
										}
										Game1.spawnMonstersAtNight = false;
										return true;
									}
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering", new object[0]));
									return true;
								}
								else if (!(questionAndAnswer == "ClubCard_Yes."))
								{
									return false;
								}
							}
							else if (num != 1326189771u)
							{
								if (num != 1446971618u)
								{
									return false;
								}
								if (!(questionAndAnswer == "Drum_Change"))
								{
									return false;
								}
								Game1.drawItemNumberSelection("drumTone", -1);
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "ClubSeller_I'll"))
								{
									return false;
								}
								if (Game1.player.Money >= 1000000)
								{
									Game1.player.Money -= 1000000;
									Game1.exitActiveMenu();
									Game1.player.forceCanMove();
									Game1.player.addItemByMenuIfNecessaryElseHoldUp(new Object(Vector2.Zero, 127, false), null);
									return true;
								}
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_ClubSeller_NotEnoughMoney", new object[0]));
								return true;
							}
						}
						else if (num <= 1766532371u)
						{
							if (num != 1712806090u)
							{
								if (num != 1766532371u)
								{
									return false;
								}
								if (!(questionAndAnswer == "ClubCard_That's"))
								{
									return false;
								}
							}
							else
							{
								if (!(questionAndAnswer == "ExitToTitle_Yes"))
								{
									return false;
								}
								goto IL_25E9;
							}
						}
						else if (num != 1775651564u)
						{
							if (num != 1815784496u)
							{
								if (num != 1823658541u)
								{
									return false;
								}
								if (!(questionAndAnswer == "evilShrineCenter_Yes"))
								{
									return false;
								}
								if (Game1.player.Money >= 30000)
								{
									Game1.player.Money -= 30000;
									Game1.player.wipeExMemories();
									this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(7 * Game1.tileSize + 5 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 2 * Game1.pixelZoom)), false, 0f, Color.White)
									{
										interval = 50f,
										totalNumberOfLoops = 99999,
										animationLength = 7,
										layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
										scale = (float)Game1.pixelZoom
									});
									Game1.soundBank.PlayCue("fireball");
									DelayedAction.playSoundAfterDelay("debuffHit", 500);
									int count = 0;
									Game1.player.faceDirection(2);
									Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
									{
										new FarmerSprite.AnimationFrame(94, 1500),
										new FarmerSprite.AnimationFrame(0, 1)
									});
									Game1.player.freezePause = 1500;
									Game1.player.jitterStrength = 1f;
									for (int j = 0; j < 20; j++)
									{
										this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(7f, 5f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float)Game1.random.Next(Game1.tileSize / 4)), false, 0.002f, Color.SlateGray)
										{
											alpha = 0.75f,
											motion = new Vector2(0f, -0.5f),
											acceleration = new Vector2(-0.002f, 0f),
											interval = 99999f,
											layerDepth = (float)(5 * Game1.tileSize) / 10000f + (float)Game1.random.Next(100) / 10000f,
											scale = (float)(Game1.pixelZoom * 3) / 4f,
											scaleChange = 0.01f,
											rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f,
											delayBeforeAnimationStart = j * 25
										});
									}
									for (int k = 0; k < 16; k++)
									{
										foreach (Vector2 v2 in Utility.getBorderOfThisRectangle(Utility.getRectangleCenteredAt(new Vector2(7f, 5f), 2 + k * 2)))
										{
											if (count % 2 == 0)
											{
												this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(692, 1853, 4, 4), 25f, 1, 16, v2 * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, false)
												{
													layerDepth = 1f,
													delayBeforeAnimationStart = k * 50,
													scale = (float)Game1.pixelZoom,
													scaleChange = 1f,
													color = new Color((int)(255 - Utility.getRedToGreenLerpColor(1f / (float)(k + 1)).R), (int)(255 - Utility.getRedToGreenLerpColor(1f / (float)(k + 1)).G), (int)(255 - Utility.getRedToGreenLerpColor(1f / (float)(k + 1)).B)),
													acceleration = new Vector2(-0.1f, 0f)
												});
											}
											count++;
										}
									}
									return true;
								}
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering", new object[0]));
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "Mine_No"))
								{
									return false;
								}
								Response[] noYesResponses = new Response[]
								{
									new Response("No", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_No", new object[0])),
									new Response("Yes", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_Yes", new object[0]))
								};
								this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Mines_ResetMine", new object[0])), noYesResponses, "ResetMine");
								return true;
							}
						}
						else
						{
							if (!(questionAndAnswer == "MinecartGame_Progress"))
							{
								return false;
							}
							Game1.currentMinigame = new MineCart(5, 3);
							return true;
						}
						Game1.playSound("explosion");
						Game1.flashAlpha = 5f;
						this.characters.Remove(this.getCharacterFromName("Bouncer"));
						if (this.characters.Count > 0)
						{
							this.characters[0].faceDirection(1);
							this.characters[0].setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Sandy_PlayerClubMember", new object[0]), false, false);
							this.characters[0].doEmote(16, true);
						}
						Game1.pauseThenMessage(500, Game1.content.LoadString("Strings\\Locations:Club_Bouncer_PlayerClubMember", new object[0]), false);
						Game1.player.Halt();
						Game1.getCharacterFromName("Mister Qi", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:MisterQi_PlayerClubMember", new object[0]), false, false);
						return true;
					}
					if (num <= 970402917u)
					{
						if (num <= 877311563u)
						{
							if (num != 830669632u)
							{
								if (num != 877311563u)
								{
									return false;
								}
								if (!(questionAndAnswer == "Smelt_Iron"))
								{
									return false;
								}
								Game1.player.IronPieces -= 10;
								this.smeltBar(new Object(Vector2.Zero, 335, "Iron Bar", false, true, false, false), 120);
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "mariner_Buy"))
								{
									return false;
								}
								if (Game1.player.Money < 5000)
								{
									Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
									return true;
								}
								Game1.player.Money -= 5000;
								Game1.player.addItemByMenuIfNecessary(new Object(460, 1, false, -1, 0)
								{
									specialItem = true
								}, null);
								if (Game1.activeClickableMenu == null)
								{
									Game1.player.holdUpItemThenMessage(new Object(460, 1, false, -1, 0), true);
									return true;
								}
								return true;
							}
						}
						else if (num != 956781401u)
						{
							if (num != 970402917u)
							{
								return false;
							}
							if (!(questionAndAnswer == "Minecart_Bus"))
							{
								return false;
							}
							Game1.player.Halt();
							Game1.player.freezePause = 700;
							Game1.warpFarmer("BusStop", 4, 4, 2);
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "Mine_Return"))
							{
								return false;
							}
							Game1.enterMine(false, Game1.player.deepestMineLevel, null);
							return true;
						}
					}
					else if (num <= 1034734869u)
					{
						if (num != 993094382u)
						{
							if (num != 1034734869u)
							{
								return false;
							}
							if (!(questionAndAnswer == "Blacksmith_Process"))
							{
								return false;
							}
							Game1.activeClickableMenu = new GeodeMenu();
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "Mine_Yes"))
							{
								return false;
							}
							if (Game1.mine != null && Game1.mine.mineLevel > 120)
							{
								Game1.warpFarmer("SkullCave", 3, 4, 2);
								return true;
							}
							Game1.warpFarmer("UndergroundMine", 16, 16, false);
							return true;
						}
					}
					else if (num != 1091116656u)
					{
						if (num != 1098628691u)
						{
							return false;
						}
						if (!(questionAndAnswer == "BuyClubCoins_Yes"))
						{
							return false;
						}
						if (Game1.player.Money >= 1000)
						{
							Game1.player.Money -= 1000;
							Game1.player.clubCoins += 10;
							return true;
						}
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "Museum_Collect"))
						{
							return false;
						}
						Game1.activeClickableMenu = new ItemGrabMenu((this as LibraryMuseum).getRewardsForPlayer(Game1.player), false, true, null, null, "Rewards", new ItemGrabMenu.behaviorOnItemSelect((this as LibraryMuseum).collectedReward), false, false, false, false, false, 0, null, -1, null);
						return true;
					}
				}
				IL_25E9:
				Game1.fadeScreenToBlack();
				Game1.exitToTitle = true;
				return true;
			}
			if (num <= 2908868305u)
			{
				if (num <= 2433686860u)
				{
					if (num <= 2009433326u)
					{
						if (num <= 1874877668u)
						{
							if (num != 1843424219u)
							{
								if (num != 1874877668u)
								{
									return false;
								}
								if (!(questionAndAnswer == "Smelt_Copper"))
								{
									return false;
								}
								Game1.player.CopperPieces -= 10;
								this.smeltBar(new Object(Vector2.Zero, 334, "Copper Bar", false, true, false, false), 60);
								return true;
							}
							else
							{
								if (!(questionAndAnswer == "Minecart_Mines"))
								{
									return false;
								}
								Game1.player.Halt();
								Game1.player.freezePause = 700;
								Game1.warpFarmer("Mine", 13, 9, 1);
								return true;
							}
						}
						else if (num != 1970483540u)
						{
							if (num != 2009433326u)
							{
								return false;
							}
							if (!(questionAndAnswer == "ExitMine_Leave"))
							{
								return false;
							}
						}
						else
						{
							if (!(questionAndAnswer == "carpenter_Upgrade"))
							{
								return false;
							}
							this.houseUpgradeOffer();
							return true;
						}
					}
					else if (num <= 2250286071u)
					{
						if (num != 2136192597u)
						{
							if (num != 2250286071u)
							{
								return false;
							}
							if (!(questionAndAnswer == "divorceCancel_Yes"))
							{
								return false;
							}
							if (Game1.player.divorceTonight)
							{
								Game1.player.divorceTonight = false;
								Game1.player.money += 50000;
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_Cancelled", new object[0]));
								return true;
							}
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "evilShrineLeft_Yes"))
							{
								return false;
							}
							if (Game1.player.removeItemsFromInventory(74, 1))
							{
								this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(2 * Game1.tileSize + 7 * Game1.pixelZoom), (float)(6 * Game1.tileSize + Game1.pixelZoom)), false, 0f, Color.White)
								{
									interval = 50f,
									totalNumberOfLoops = 99999,
									animationLength = 7,
									layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
									scale = (float)Game1.pixelZoom
								});
								for (int l = 0; l < 20; l++)
								{
									this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(2f, 6f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float)Game1.random.Next(Game1.tileSize / 4)), false, 0.002f, Color.LightGray)
									{
										alpha = 0.75f,
										motion = new Vector2(1f, -0.5f),
										acceleration = new Vector2(-0.002f, 0f),
										interval = 99999f,
										layerDepth = (float)(6 * Game1.tileSize) / 10000f + (float)Game1.random.Next(100) / 10000f,
										scale = (float)(Game1.pixelZoom * 3) / 4f,
										scaleChange = 0.01f,
										rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f,
										delayBeforeAnimationStart = l * 25
									});
								}
								Game1.soundBank.PlayCue("fireball");
								this.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(2f, 5f) * (float)Game1.tileSize, false, true, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(4f, -2f)
								});
								if (Game1.player.getChildren().Count<Child>() > 1)
								{
									this.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(2f, 5f) * (float)Game1.tileSize, false, true, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										motion = new Vector2(4f, -1.5f),
										delayBeforeAnimationStart = 50
									});
								}
								string message = "";
								foreach (NPC m in Game1.player.getChildren())
								{
									message = string.Concat(new string[]
									{
										message,
										Game1.content.LoadString("Strings\\Locations:WitchHut_Goodbye", new object[0]),
										", ",
										m.getName(),
										". "
									});
								}
								Game1.showGlobalMessage(message);
								Game1.player.getRidOfChildren();
								return true;
							}
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering", new object[0]));
							return true;
						}
					}
					else if (num != 2316922503u)
					{
						if (num != 2433686860u)
						{
							return false;
						}
						if (!(questionAndAnswer == "diary_...I"))
						{
							return false;
						}
						Game1.player.friendships[(Game1.farmEvent as DiaryEvent).NPCname][5] = 1;
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "Backpack_Yes"))
						{
							return false;
						}
						this.tryToBuyNewBackpack();
						return true;
					}
				}
				else if (num <= 2764458791u)
				{
					if (num <= 2714077224u)
					{
						if (num != 2548386307u)
						{
							if (num != 2714077224u)
							{
								return false;
							}
							if (!(questionAndAnswer == "Smelt_Iridium"))
							{
								return false;
							}
							Game1.player.IridiumPieces -= 10;
							this.smeltBar(new Object(Vector2.Zero, 337, "Iridium Bar", false, true, false, false), 1440);
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "upgrade_Yes"))
							{
								return false;
							}
							this.houseUpgradeAccept();
							return true;
						}
					}
					else if (num != 2748303264u)
					{
						if (num != 2764458791u)
						{
							return false;
						}
						if (!(questionAndAnswer == "Backpack_Purchase"))
						{
							return false;
						}
						if (Game1.player.maxItems == 12 && Game1.player.Money >= 2000)
						{
							Game1.player.Money -= 2000;
							Game1.player.maxItems += 12;
							for (int n = 0; n < Game1.player.maxItems; n++)
							{
								if (Game1.player.items.Count <= n)
								{
									Game1.player.items.Add(null);
								}
							}
							Game1.player.holdUpItemThenMessage(new SpecialItem(-1, 99, "Large Pack"), true);
							return true;
						}
						if (Game1.player.maxItems < 36 && Game1.player.Money >= 10000)
						{
							Game1.player.Money -= 10000;
							Game1.player.maxItems += 12;
							Game1.player.holdUpItemThenMessage(new SpecialItem(-1, 99, "Deluxe Pack"), true);
							for (int i2 = 0; i2 < Game1.player.maxItems; i2++)
							{
								if (Game1.player.items.Count <= i2)
								{
									Game1.player.items.Add(null);
								}
							}
							return true;
						}
						if (Game1.player.maxItems != 36)
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney2", new object[0]));
							return true;
						}
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "Sleep_Yes"))
						{
							return false;
						}
						if (this.lightLevel == 0f && Game1.timeOfDay < 2000)
						{
							this.lightLevel = 0.6f;
							Game1.playSound("openBox");
							Game1.NewDay(600f);
						}
						else if (this.lightLevel > 0f && Game1.timeOfDay >= 2000)
						{
							this.lightLevel = 0f;
							Game1.playSound("openBox");
							Game1.NewDay(600f);
						}
						else
						{
							Game1.NewDay(0f);
						}
						Game1.player.mostRecentBed = Game1.player.position;
						Game1.player.doEmote(24);
						Game1.player.freezePause = 2000;
						return true;
					}
				}
				else if (num <= 2784483468u)
				{
					if (num != 2779518255u)
					{
						if (num != 2784483468u)
						{
							return false;
						}
						if (!(questionAndAnswer == "Shaft_Jump"))
						{
							return false;
						}
						if (this is MineShaft)
						{
							(this as MineShaft).enterMineShaft();
							return true;
						}
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "CalicoJack_Rules"))
						{
							return false;
						}
						Game1.multipleDialogues(new string[]
						{
							Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Rules1", new object[0]),
							Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Rules2", new object[0])
						});
						return true;
					}
				}
				else if (num != 2813271193u)
				{
					if (num != 2908868305u)
					{
						return false;
					}
					if (!(questionAndAnswer == "Minecart_Town"))
					{
						return false;
					}
					Game1.player.Halt();
					Game1.player.freezePause = 700;
					Game1.warpFarmer("Town", 105, 80, 1);
					return true;
				}
				else
				{
					if (!(questionAndAnswer == "Eat_Yes"))
					{
						return false;
					}
					Game1.isEating = false;
					Game1.eatHeldObject();
					return true;
				}
			}
			else if (num <= 3389677340u)
			{
				if (num <= 3048358753u)
				{
					if (num <= 2994182275u)
					{
						if (num != 2983264862u)
						{
							if (num != 2994182275u)
							{
								return false;
							}
							if (!(questionAndAnswer == "Smelt_Gold"))
							{
								return false;
							}
							Game1.player.GoldPieces -= 10;
							this.smeltBar(new Object(Vector2.Zero, 336, "Gold Bar", false, true, false, false), 300);
							return true;
						}
						else
						{
							if (!(questionAndAnswer == "Flute_Change"))
							{
								return false;
							}
							Game1.drawItemNumberSelection("flutePitch", -1);
							return true;
						}
					}
					else if (num != 3033860223u)
					{
						if (num != 3048358753u)
						{
							return false;
						}
						if (!(questionAndAnswer == "jukebox_Yes"))
						{
							return false;
						}
						Game1.drawItemNumberSelection("jukebox", -1);
						Game1.jukeboxPlaying = true;
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "evilShrineRightActivate_Yes"))
						{
							return false;
						}
						if (Game1.player.removeItemsFromInventory(203, 1))
						{
							this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(12 * Game1.tileSize + 3 * Game1.pixelZoom), (float)(6 * Game1.tileSize + Game1.pixelZoom)), false, 0f, Color.White)
							{
								interval = 50f,
								totalNumberOfLoops = 99999,
								animationLength = 7,
								layerDepth = (float)(6 * Game1.tileSize) / 10000f + 0.0001f,
								scale = (float)Game1.pixelZoom
							});
							Game1.soundBank.PlayCue("fireball");
							DelayedAction.playSoundAfterDelay("batScreech", 500);
							for (int i3 = 0; i3 < 20; i3++)
							{
								this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(12f, 6f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float)Game1.random.Next(Game1.tileSize / 4)), false, 0.002f, Color.DarkSlateBlue)
								{
									alpha = 0.75f,
									motion = new Vector2(-0.1f, -0.5f),
									acceleration = new Vector2(-0.002f, 0f),
									interval = 99999f,
									layerDepth = (float)(6 * Game1.tileSize) / 10000f + (float)Game1.random.Next(100) / 10000f,
									scale = (float)(Game1.pixelZoom * 3) / 4f,
									scaleChange = 0.01f,
									rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f,
									delayBeforeAnimationStart = i3 * 60
								});
							}
							Game1.player.freezePause = 1501;
							for (int i4 = 0; i4 < 28; i4++)
							{
								this.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(540, 347, 13, 13), 50f, 4, 9999, new Vector2(12f, 5f) * (float)Game1.tileSize, false, true, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									delayBeforeAnimationStart = 500 + i4 * 25,
									motion = new Vector2((float)(Game1.random.Next(1, 5) * ((Game1.random.NextDouble() < 0.5) ? -1 : 1)), (float)(Game1.random.Next(1, 5) * ((Game1.random.NextDouble() < 0.5) ? -1 : 1)))
								});
							}
							Game1.spawnMonstersAtNight = true;
							return true;
						}
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering", new object[0]));
						return true;
					}
				}
				else if (num <= 3125820853u)
				{
					if (num != 3052043945u)
					{
						if (num != 3125820853u)
						{
							return false;
						}
						if (!(questionAndAnswer == "Minecart_Quarry"))
						{
							return false;
						}
						Game1.player.Halt();
						Game1.player.freezePause = 700;
						Game1.warpFarmer("Mountain", 124, 12, 2);
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "MinecartGame_Endless"))
						{
							return false;
						}
						Game1.currentMinigame = new MineCart(5, 2);
						return true;
					}
				}
				else if (num != 3332356948u)
				{
					if (num != 3389677340u)
					{
						return false;
					}
					if (!(questionAndAnswer == "RemoveIncubatingEgg_Yes"))
					{
						return false;
					}
					Game1.player.ActiveObject = new Object(Vector2.Zero, (this as AnimalHouse).incubatingEgg.Y, null, false, true, false, false);
					Game1.player.showCarrying();
					(this as AnimalHouse).incubatingEgg.Y = -1;
					this.map.GetLayer("Front").Tiles[1, 2].TileIndex = 45;
					return true;
				}
				else
				{
					if (!(questionAndAnswer == "buyJojaCola_Yes"))
					{
						return false;
					}
					if (Game1.player.Money >= 75)
					{
						Game1.player.Money -= 75;
						Game1.player.addItemByMenuIfNecessary(new Object(167, 1, false, -1, 0), null);
						return true;
					}
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
					return true;
				}
			}
			else if (num <= 3654804704u)
			{
				if (num <= 3491527061u)
				{
					if (num != 3434251353u)
					{
						if (num != 3491527061u)
						{
							return false;
						}
						if (!(questionAndAnswer == "Museum_Donate"))
						{
							return false;
						}
						Game1.activeClickableMenu = new MuseumMenu();
						return true;
					}
					else
					{
						if (!(questionAndAnswer == "WizardShrine_Yes"))
						{
							return false;
						}
						if (Game1.player.Money >= 500)
						{
							Game1.activeClickableMenu = new CharacterCustomization(new List<int>
							{
								0,
								1,
								2,
								3,
								4,
								5
							}, new List<int>
							{
								0,
								1,
								2,
								3,
								4,
								5
							}, new List<int>
							{
								0,
								1,
								2,
								3,
								4,
								5
							}, true);
							Game1.player.Money -= 500;
							return true;
						}
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney2", new object[0]));
						return true;
					}
				}
				else if (num != 3499821461u)
				{
					if (num != 3654804704u)
					{
						return false;
					}
					if (!(questionAndAnswer == "Mariner_Buy"))
					{
						return false;
					}
					if (Game1.player.Money >= 5000)
					{
						Game1.player.Money -= 5000;
						Game1.player.grabObject(new Object(Vector2.Zero, 460, null, false, true, false, false));
						return true;
					}
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
					return true;
				}
				else
				{
					if (!(questionAndAnswer == "Marnie_Purchase"))
					{
						return false;
					}
					Game1.player.forceCanMove();
					Game1.activeClickableMenu = new PurchaseAnimalsMenu(Utility.getPurchaseAnimalStock());
					return true;
				}
			}
			else if (num <= 3694948413u)
			{
				if (num != 3684797429u)
				{
					if (num != 3694948413u)
					{
						return false;
					}
					if (!(questionAndAnswer == "carpenter_Construct"))
					{
						return false;
					}
					Game1.activeClickableMenu = new CarpenterMenu(false);
					return true;
				}
				else
				{
					if (!(questionAndAnswer == "Quest_No"))
					{
						return false;
					}
					Game1.currentBillboard = 0;
					return true;
				}
			}
			else if (num != 3945991834u)
			{
				if (num != 4157255876u)
				{
					if (num != 4232700822u)
					{
						return false;
					}
					if (!(questionAndAnswer == "Bouquet_Yes"))
					{
						return false;
					}
					if (Game1.player.Money < 500)
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
						return true;
					}
					if (Game1.player.ActiveObject == null)
					{
						Game1.player.Money -= 500;
						Game1.player.grabObject(new Object(Vector2.Zero, 458, null, false, true, false, false));
						return true;
					}
					return true;
				}
				else
				{
					if (!(questionAndAnswer == "Blacksmith_Shop"))
					{
						return false;
					}
					Game1.activeClickableMenu = new ShopMenu(Utility.getBlacksmithStock(), 0, "Clint");
					return true;
				}
			}
			else
			{
				if (!(questionAndAnswer == "CalicoJack_Play"))
				{
					return false;
				}
				if (Game1.player.clubCoins >= 100)
				{
					Game1.currentMinigame = new CalicoJack(-1, false);
					return true;
				}
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_NotEnoughCoins", new object[0]));
				return true;
			}
			IL_1FB4:
			if (Game1.mine != null && Game1.mine.mineLevel > 120)
			{
				Game1.warpFarmer("SkullCave", 3, 4, 2);
			}
			else
			{
				Game1.warpFarmer("Mine", 23, 8, false);
			}
			Game1.changeMusicTrack("none");
			return true;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00062448 File Offset: 0x00060648
		public virtual bool answerDialogue(Response answer)
		{
			string[] questionParams = (this.lastQuestionKey != null) ? this.lastQuestionKey.Split(new char[]
			{
				' '
			}) : null;
			string questionAndAnswer = (questionParams != null) ? (questionParams[0] + "_" + answer.responseKey) : null;
			if (answer.responseKey.Equals("Move"))
			{
				Game1.player.grabObject(this.actionObjectForQuestionDialogue);
				this.removeObject(this.actionObjectForQuestionDialogue.TileLocation, false);
				this.actionObjectForQuestionDialogue = null;
				return true;
			}
			if (this.afterQuestion != null)
			{
				this.afterQuestion(Game1.player, answer.responseKey);
				this.afterQuestion = null;
				Game1.objectDialoguePortraitPerson = null;
				return true;
			}
			return questionAndAnswer != null && this.answerDialogueAction(questionAndAnswer, questionParams);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00062509 File Offset: 0x00060709
		public void setObject(Vector2 v, Object o)
		{
			if (this.objects.ContainsKey(v))
			{
				this.objects[v] = o;
				return;
			}
			this.objects.Add(v, o);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00062534 File Offset: 0x00060734
		private void houseUpgradeOffer()
		{
			switch (Game1.player.houseUpgradeLevel)
			{
			case 0:
				this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_UpgradeHouse1", new object[0])), this.createYesNoResponses(), "upgrade");
				return;
			case 1:
				this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_UpgradeHouse2", new object[0])), this.createYesNoResponses(), "upgrade");
				return;
			case 2:
				this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_UpgradeHouse3", new object[0])), this.createYesNoResponses(), "upgrade");
				return;
			default:
				return;
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000625E4 File Offset: 0x000607E4
		private void houseUpgradeAccept()
		{
			switch (Game1.player.houseUpgradeLevel)
			{
			case 0:
				if (Game1.player.Money >= 10000 && Game1.player.hasItemInInventory(388, 450, 0))
				{
					Game1.player.daysUntilHouseUpgrade = 3;
					Game1.player.Money -= 10000;
					Game1.player.removeItemsFromInventory(388, 450);
					Game1.getCharacterFromName("Robin", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Robin_HouseUpgrade_Accepted", new object[0]), false, false);
					Game1.drawDialogue(Game1.getCharacterFromName("Robin", false));
					return;
				}
				if (Game1.player.Money < 10000)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney3", new object[0]));
					return;
				}
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_NotEnoughWood1", new object[0]));
				return;
			case 1:
				if (Game1.player.Money >= 50000 && Game1.player.hasItemInInventory(709, 150, 0))
				{
					Game1.player.daysUntilHouseUpgrade = 3;
					Game1.player.Money -= 50000;
					Game1.player.removeItemsFromInventory(709, 150);
					Game1.getCharacterFromName("Robin", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Robin_HouseUpgrade_Accepted", new object[0]), false, false);
					Game1.drawDialogue(Game1.getCharacterFromName("Robin", false));
					return;
				}
				if (Game1.player.Money < 50000)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney3", new object[0]));
					return;
				}
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_NotEnoughWood2", new object[0]));
				return;
			case 2:
				if (Game1.player.Money >= 100000)
				{
					Game1.player.daysUntilHouseUpgrade = 3;
					Game1.player.Money -= 100000;
					Game1.getCharacterFromName("Robin", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Robin_HouseUpgrade_Accepted", new object[0]), false, false);
					Game1.drawDialogue(Game1.getCharacterFromName("Robin", false));
					return;
				}
				if (Game1.player.Money < 100000)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney3", new object[0]));
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00062864 File Offset: 0x00060A64
		private void smeltBar(Object bar, int minutesUntilReady)
		{
			Farmer expr_05 = Game1.player;
			int coalPieces = expr_05.CoalPieces;
			expr_05.CoalPieces = coalPieces - 1;
			this.actionObjectForQuestionDialogue.heldObject = bar;
			this.actionObjectForQuestionDialogue.minutesUntilReady = minutesUntilReady;
			this.actionObjectForQuestionDialogue.showNextIndex = true;
			this.actionObjectForQuestionDialogue = null;
			Game1.playSound("openBox");
			Game1.playSound("furnace");
			Stats expr_58 = Game1.stats;
			uint barsSmelted = expr_58.BarsSmelted;
			expr_58.BarsSmelted = barsSmelted + 1u;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000628D8 File Offset: 0x00060AD8
		public void tryToBuyNewBackpack()
		{
			int cost = 0;
			int maxItems = Game1.player.MaxItems;
			if (maxItems != 4)
			{
				if (maxItems != 9)
				{
					if (maxItems == 14)
					{
						cost = 15000;
					}
				}
				else
				{
					cost = 7500;
				}
			}
			else
			{
				cost = 3500;
			}
			if (Game1.player.Money >= cost)
			{
				Game1.player.increaseBackpackSize(5);
				Game1.player.Money -= cost;
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:Backpack_Bought", new object[]
				{
					Game1.player.MaxItems
				}));
				this.checkForMapChanges();
				return;
			}
			Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00062990 File Offset: 0x00060B90
		public void checkForMapChanges()
		{
			if (this.name.Equals("SeedShop") && Game1.player.MaxItems == 19)
			{
				this.map.GetLayer("Front").Tiles[10, 21] = new StaticTile(this.map.GetLayer("Front"), this.map.GetTileSheet("TownHouseIndoors"), BlendMode.Alpha, 203);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00062A08 File Offset: 0x00060C08
		public void removeStumpOrBoulder(int tileX, int tileY, Object o)
		{
			List<Vector2> boulderBatch = new List<Vector2>();
			string text = o.Name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 3002255887u)
			{
				if (num <= 1478161722u)
				{
					if (num != 1443354848u)
					{
						if (num != 1478161722u)
						{
							goto IL_250;
						}
						if (!(text == "Stump3/4"))
						{
							goto IL_250;
						}
					}
					else if (!(text == "Boulder3/4"))
					{
						goto IL_250;
					}
					boulderBatch.Add(new Vector2((float)tileX, (float)tileY));
					boulderBatch.Add(new Vector2((float)(tileX + 1), (float)tileY));
					boulderBatch.Add(new Vector2((float)tileX, (float)(tileY - 1)));
					boulderBatch.Add(new Vector2((float)(tileX + 1), (float)(tileY - 1)));
				}
				else
				{
					if (num != 2100107977u)
					{
						if (num != 3002255887u)
						{
							goto IL_250;
						}
						if (!(text == "Stump4/4"))
						{
							goto IL_250;
						}
					}
					else if (!(text == "Boulder4/4"))
					{
						goto IL_250;
					}
					boulderBatch.Add(new Vector2((float)tileX, (float)tileY));
					boulderBatch.Add(new Vector2((float)(tileX - 1), (float)tileY));
					boulderBatch.Add(new Vector2((float)tileX, (float)(tileY - 1)));
					boulderBatch.Add(new Vector2((float)(tileX - 1), (float)(tileY - 1)));
				}
			}
			else
			{
				if (num <= 3696585573u)
				{
					if (num != 3025558466u)
					{
						if (num != 3696585573u)
						{
							goto IL_250;
						}
						if (!(text == "Stump2/4"))
						{
							goto IL_250;
						}
						goto IL_17D;
					}
					else if (!(text == "Boulder1/4"))
					{
						goto IL_250;
					}
				}
				else if (num != 4196148280u)
				{
					if (num != 4214227979u)
					{
						goto IL_250;
					}
					if (!(text == "Boulder2/4"))
					{
						goto IL_250;
					}
					goto IL_17D;
				}
				else if (!(text == "Stump1/4"))
				{
					goto IL_250;
				}
				boulderBatch.Add(new Vector2((float)tileX, (float)tileY));
				boulderBatch.Add(new Vector2((float)(tileX + 1), (float)tileY));
				boulderBatch.Add(new Vector2((float)tileX, (float)(tileY + 1)));
				boulderBatch.Add(new Vector2((float)(tileX + 1), (float)(tileY + 1)));
				goto IL_250;
				IL_17D:
				boulderBatch.Add(new Vector2((float)tileX, (float)tileY));
				boulderBatch.Add(new Vector2((float)(tileX - 1), (float)tileY));
				boulderBatch.Add(new Vector2((float)tileX, (float)(tileY + 1)));
				boulderBatch.Add(new Vector2((float)(tileX - 1), (float)(tileY + 1)));
			}
			IL_250:
			int whichDebris = o.Name.Contains("Stump") ? 5 : 3;
			if (Game1.currentSeason.Equals("winter"))
			{
				whichDebris += 376;
			}
			for (int i = 0; i < boulderBatch.Count; i++)
			{
				this.TemporarySprites.Add(new TemporaryAnimatedSprite(whichDebris, (float)Game1.random.Next(150, 400), 1, 3, new Vector2(boulderBatch[i].X * (float)Game1.tileSize, boulderBatch[i].Y * (float)Game1.tileSize), true, o.flipped));
			}
			this.removeBatch(boulderBatch);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00062D0D File Offset: 0x00060F0D
		public void destroyObject(Vector2 tileLocation, Farmer who)
		{
			this.destroyObject(tileLocation, false, who);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00062D18 File Offset: 0x00060F18
		public void destroyObject(Vector2 tileLocation, bool hardDestroy, Farmer who)
		{
			if (this.objects.ContainsKey(tileLocation) && !this.objects[tileLocation].IsHoeDirt && this.objects[tileLocation].fragility != 2 && !(this.objects[tileLocation] is Chest))
			{
				Object obj = this.objects[tileLocation];
				bool remove = false;
				if (obj.Type != null && (obj.Type.Equals("Fish") || obj.Type.Equals("Cooking") || obj.Type.Equals("Crafting")))
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(obj.ParentSheetIndex, 150f, 1, 3, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize), true, obj.bigCraftable, obj.flipped));
					remove = true;
				}
				else if (obj.Name.Contains("Stump") || obj.Name.Contains("Boulder"))
				{
					remove = true;
					this.removeStumpOrBoulder((int)tileLocation.X, (int)tileLocation.Y, obj);
				}
				else if (obj.CanBeGrabbed | hardDestroy)
				{
					remove = true;
				}
				if (this.Name.Equals("UndergroundMine") && !obj.Name.Contains("Lumber"))
				{
					remove = true;
					Game1.mine.checkStoneForItems(obj.ParentSheetIndex, (int)tileLocation.X, (int)tileLocation.Y, who);
				}
				if (remove)
				{
					this.objects.Remove(tileLocation);
				}
				if (this.name.Equals("Town") && obj.Name.Contains("Weed") && Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest"))
				{
					((WeedingQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
				}
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00062F28 File Offset: 0x00061128
		public string doesTileHaveProperty(int xTile, int yTile, string propertyName, string layerName)
		{
			PropertyValue property = null;
			if (this.map.GetLayer(layerName) != null)
			{
				Tile tmp = this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size);
				if (tmp != null)
				{
					tmp.TileIndexProperties.TryGetValue(propertyName, out property);
				}
				if (property == null && tmp != null)
				{
					this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size).Properties.TryGetValue(propertyName, out property);
				}
			}
			if (property != null)
			{
				return property.ToString();
			}
			return null;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00062FDC File Offset: 0x000611DC
		public string doesTileHavePropertyNoNull(int xTile, int yTile, string propertyName, string layerName)
		{
			PropertyValue property = null;
			PropertyValue propertyTile = null;
			if (this.map.GetLayer(layerName) != null)
			{
				Tile tmp = this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size);
				if (tmp != null)
				{
					tmp.TileIndexProperties.TryGetValue(propertyName, out property);
				}
				if (tmp != null)
				{
					this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size).Properties.TryGetValue(propertyName, out propertyTile);
				}
				if (propertyTile != null)
				{
					property = propertyTile;
				}
			}
			if (property != null)
			{
				return property.ToString();
			}
			return "";
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00063098 File Offset: 0x00061298
		public bool isOpenWater(int xTile, int yTile)
		{
			return this.doesTileHaveProperty(xTile, yTile, "Water", "Back") != null && this.doesTileHaveProperty(xTile, yTile, "Passable", "Buildings") == null && !this.objects.ContainsKey(new Vector2((float)xTile, (float)yTile));
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000630E8 File Offset: 0x000612E8
		public bool isCropAtTile(int tileX, int tileY)
		{
			Vector2 v = new Vector2((float)tileX, (float)tileY);
			return this.terrainFeatures.ContainsKey(v) && this.terrainFeatures[v].GetType() == typeof(HoeDirt) && ((HoeDirt)this.terrainFeatures[v]).crop != null;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0006314C File Offset: 0x0006134C
		public virtual bool dropObject(Object obj, Vector2 dropLocation, xTile.Dimensions.Rectangle viewport, bool initialPlacement, Farmer who = null)
		{
			obj.isSpawnedObject = true;
			Vector2 tileLocation = new Vector2((float)((int)dropLocation.X / Game1.tileSize), (float)((int)dropLocation.Y / Game1.tileSize));
			if (!this.isTileOnMap(tileLocation) || this.map.GetLayer("Back").PickTile(new Location((int)dropLocation.X, (int)dropLocation.Y), Game1.viewport.Size) == null || this.map.GetLayer("Back").Tiles[(int)tileLocation.X, (int)tileLocation.Y].TileIndexProperties.ContainsKey("Unplaceable"))
			{
				return false;
			}
			if (obj.bigCraftable)
			{
				obj.tileLocation = tileLocation;
				if (!this.isFarm)
				{
					return false;
				}
				if (!obj.setOutdoors && this.isOutdoors)
				{
					return false;
				}
				if (!obj.setIndoors && !this.isOutdoors)
				{
					return false;
				}
				if (obj.performDropDownAction(who))
				{
					return false;
				}
			}
			else if (obj.Type != null && obj.Type.Equals("Crafting") && obj.performDropDownAction(who))
			{
				obj.CanBeSetDown = false;
			}
			bool tilePassable = this.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), viewport) && !this.isTileOccupiedForPlacement(tileLocation, null);
			if (((obj.CanBeSetDown | initialPlacement) & tilePassable) && !this.isTileHoeDirt(tileLocation))
			{
				obj.TileLocation = tileLocation;
				if (this.objects.ContainsKey(tileLocation))
				{
					return false;
				}
				this.objects.Add(tileLocation, obj);
			}
			else if (this.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Water", "Back") != null)
			{
				this.temporarySprites.Add(new TemporaryAnimatedSprite(28, 300f, 2, 1, dropLocation, false, obj.flipped));
				Game1.playSound("dropItemInWater");
			}
			else
			{
				if (obj.CanBeSetDown && !tilePassable)
				{
					return false;
				}
				if (obj.ParentSheetIndex >= 0 && obj.Type != null)
				{
					if (obj.Type.Equals("Fish") || obj.Type.Equals("Cooking") || obj.Type.Equals("Crafting"))
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(obj.ParentSheetIndex, 150f, 1, 3, dropLocation, true, obj.flipped));
					}
					else
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(obj.ParentSheetIndex + 1, 150f, 1, 3, dropLocation, true, obj.flipped));
					}
				}
			}
			return true;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000633D4 File Offset: 0x000615D4
		public void detectTreasures(double successRate, int radius, Vector2 tileLocation)
		{
			bool insideCircle = false;
			Vector2 currentTile = new Vector2(Math.Min((float)(this.map.Layers[0].LayerWidth - 1), Math.Max(0f, tileLocation.X - (float)radius)), Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, tileLocation.Y - (float)radius)));
			bool[,] circleOutline = Game1.getCircleOutlineGrid(radius);
			for (int i = 0; i < radius * 2 + 1; i++)
			{
				for (int j = 0; j < radius * 2 + 1; j++)
				{
					if (i == 0 || j == 0 || i == radius * 2 || j == radius * 2)
					{
						insideCircle = circleOutline[i, j];
					}
					else if (circleOutline[i, j])
					{
						insideCircle = !insideCircle;
						if (!insideCircle)
						{
							if (!this.isTileOccupied(currentTile, "") && this.isTilePassable(new Location((int)currentTile.X, (int)currentTile.Y), Game1.viewport) && this.doesTileHaveProperty((int)currentTile.X, (int)currentTile.Y, "Diggable", "Back") != null && Game1.random.NextDouble() < successRate)
							{
								this.checkForBuriedItem((int)currentTile.X, (int)currentTile.Y, false, true);
							}
							if (Game1.random.NextDouble() < 0.2)
							{
								this.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float)Game1.random.Next(100, 150), 2, 1, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), false, false));
							}
						}
					}
					if (insideCircle)
					{
						if (!this.isTileOccupied(currentTile, "") && this.isTilePassable(new Location((int)currentTile.X, (int)currentTile.Y), Game1.viewport) && this.doesTileHaveProperty((int)currentTile.X, (int)currentTile.Y, "Diggable", "Back") != null && Game1.random.NextDouble() < successRate)
						{
							this.checkForBuriedItem((int)currentTile.X, (int)currentTile.Y, false, true);
						}
						if (Game1.random.NextDouble() < 0.2)
						{
							this.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float)Game1.random.Next(100, 150), 2, 1, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), false, false));
						}
					}
					currentTile.Y += 1f;
					currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, currentTile.Y));
				}
				currentTile.X += 1f;
				currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerWidth - 1), Math.Max(0f, currentTile.X));
				currentTile.Y = tileLocation.Y - (float)radius;
				currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, currentTile.Y));
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00063738 File Offset: 0x00061938
		public void explode(Vector2 tileLocation, int radius, Farmer who)
		{
			bool insideCircle = false;
			Vector2 currentTile = new Vector2(Math.Min((float)(this.map.Layers[0].LayerWidth - 1), Math.Max(0f, tileLocation.X - (float)radius)), Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, tileLocation.Y - (float)radius)));
			bool[,] circleOutline = Game1.getCircleOutlineGrid(radius);
			Microsoft.Xna.Framework.Rectangle areaOfEffect = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - (float)radius - 1f) * Game1.tileSize, (int)(tileLocation.Y - (float)radius - 1f) * Game1.tileSize, (radius * 2 + 1) * Game1.tileSize, (radius * 2 + 1) * Game1.tileSize);
			this.damageMonster(areaOfEffect, radius * 6, radius * 8, true, who);
			this.temporarySprites.Add(new TemporaryAnimatedSprite(23, 9999f, 6, 1, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), false, Game1.random.NextDouble() < 0.5)
			{
				light = true,
				lightRadius = (float)radius,
				lightcolor = Color.Black,
				alphaFade = 0.03f - (float)radius * 0.003f
			});
			Rumble.rumbleAndFade(1f, (float)(300 + radius * 100));
			if (Game1.player.GetBoundingBox().Intersects(areaOfEffect))
			{
				Game1.farmerTakeDamage(radius * 3, true, null);
			}
			for (int i = this.terrainFeatures.Count - 1; i >= 0; i--)
			{
				KeyValuePair<Vector2, TerrainFeature> j = this.terrainFeatures.ElementAt(i);
				if (j.Value.getBoundingBox(j.Key).Intersects(areaOfEffect) && j.Value.performToolAction(null, radius / 2, j.Key, null))
				{
					this.terrainFeatures.Remove(j.Key);
				}
			}
			for (int k = 0; k < radius * 2 + 1; k++)
			{
				for (int l = 0; l < radius * 2 + 1; l++)
				{
					if (k == 0 || l == 0 || k == radius * 2 || l == radius * 2)
					{
						insideCircle = circleOutline[k, l];
					}
					else if (circleOutline[k, l])
					{
						insideCircle = !insideCircle;
						if (!insideCircle)
						{
							if (this.objects.ContainsKey(currentTile) && this.objects[currentTile].onExplosion(who, this))
							{
								this.destroyObject(currentTile, who);
							}
							if (Game1.random.NextDouble() < 0.45)
							{
								if (Game1.random.NextDouble() < 0.5)
								{
									this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), false, Game1.random.NextDouble() < 0.5)
									{
										delayBeforeAnimationStart = Game1.random.Next(700)
									});
								}
								else
								{
									this.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
									{
										delayBeforeAnimationStart = Game1.random.Next(200),
										scale = (float)Game1.random.Next(5, 15) / 10f
									});
								}
							}
						}
					}
					if (insideCircle)
					{
						if (this.objects.ContainsKey(currentTile) && this.objects[currentTile].onExplosion(who, this))
						{
							this.destroyObject(currentTile, who);
						}
						if (Game1.random.NextDouble() < 0.45)
						{
							if (Game1.random.NextDouble() < 0.5)
							{
								this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), false, Game1.random.NextDouble() < 0.5)
								{
									delayBeforeAnimationStart = Game1.random.Next(700)
								});
							}
							else
							{
								this.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
								{
									delayBeforeAnimationStart = Game1.random.Next(200),
									scale = (float)Game1.random.Next(5, 15) / 10f
								});
							}
						}
						this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(currentTile.X * (float)Game1.tileSize, currentTile.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(currentTile, tileLocation) * 20f, 0, -1, -1f, -1, 0));
					}
					currentTile.Y += 1f;
					currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, currentTile.Y));
				}
				currentTile.X += 1f;
				currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerWidth - 1), Math.Max(0f, currentTile.X));
				currentTile.Y = tileLocation.Y - (float)radius;
				currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, currentTile.Y));
			}
			radius /= 2;
			circleOutline = Game1.getCircleOutlineGrid(radius);
			currentTile = new Vector2((float)((int)(tileLocation.X - (float)radius)), (float)((int)(tileLocation.Y - (float)radius)));
			for (int m = 0; m < radius * 2 + 1; m++)
			{
				for (int n = 0; n < radius * 2 + 1; n++)
				{
					if (m == 0 || n == 0 || m == radius * 2 || n == radius * 2)
					{
						insideCircle = circleOutline[m, n];
					}
					else if (circleOutline[m, n])
					{
						insideCircle = !insideCircle;
						if (!insideCircle && !this.objects.ContainsKey(currentTile) && Game1.random.NextDouble() < 0.9 && this.doesTileHaveProperty((int)currentTile.X, (int)currentTile.Y, "Diggable", "Back") != null && !this.isTileHoeDirt(currentTile))
						{
							this.checkForBuriedItem((int)currentTile.X, (int)currentTile.Y, true, false);
							this.makeHoeDirt(currentTile);
						}
					}
					if (insideCircle && !this.objects.ContainsKey(currentTile) && Game1.random.NextDouble() < 0.9 && this.doesTileHaveProperty((int)currentTile.X, (int)currentTile.Y, "Diggable", "Back") != null && !this.isTileHoeDirt(currentTile))
					{
						this.checkForBuriedItem((int)currentTile.X, (int)currentTile.Y, true, false);
						this.makeHoeDirt(currentTile);
					}
					currentTile.Y += 1f;
					currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, currentTile.Y));
				}
				currentTile.X += 1f;
				currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerWidth - 1), Math.Max(0f, currentTile.X));
				currentTile.Y = tileLocation.Y - (float)radius;
				currentTile.Y = Math.Min((float)(this.map.Layers[0].LayerHeight - 1), Math.Max(0f, currentTile.Y));
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00063FA4 File Offset: 0x000621A4
		public void removeTemporarySpritesWithID(int id)
		{
			this.removeTemporarySpritesWithID((float)id);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00063FB0 File Offset: 0x000621B0
		public void removeTemporarySpritesWithID(float id)
		{
			for (int i = this.temporarySprites.Count - 1; i >= 0; i--)
			{
				if (this.temporarySprites[i].id == id)
				{
					if (this.temporarySprites[i].hasLit)
					{
						Utility.removeLightSource(this.temporarySprites[i].lightID);
					}
					this.temporarySprites.RemoveAt(i);
				}
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00064020 File Offset: 0x00062220
		public void makeHoeDirt(Vector2 tileLocation)
		{
			if (this.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Diggable", "Back") != null && !this.isTileOccupied(tileLocation, "") && this.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), Game1.viewport))
			{
				this.terrainFeatures.Add(tileLocation, new HoeDirt((Game1.isRaining && this.isOutdoors) ? 1 : 0));
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000640A0 File Offset: 0x000622A0
		public int numberOfObjectsOfType(int index, bool bigCraftable)
		{
			int number = 0;
			foreach (KeyValuePair<Vector2, Object> v in this.Objects)
			{
				if (v.Value.parentSheetIndex == index && v.Value.bigCraftable == bigCraftable)
				{
					number++;
				}
			}
			return number;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00064114 File Offset: 0x00062314
		public virtual void performTenMinuteUpdate(int timeOfDay)
		{
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (!this.characters[i].isInvisible)
				{
					this.characters[i].checkSchedule(timeOfDay);
					this.characters[i].performTenMinuteUpdate(timeOfDay, this);
				}
			}
			for (int j = this.objects.Count - 1; j >= 0; j--)
			{
				if (this.objects[this.objects.Keys.ElementAt(j)].minutesElapsed(10, this))
				{
					this.objects.Remove(this.objects.Keys.ElementAt(j));
				}
			}
			if (this.isOutdoors)
			{
				Random r = new Random(timeOfDay + (int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed);
				if (this.Equals(Game1.currentLocation))
				{
					this.tryToAddCritters(true);
				}
				if (this.fishSplashPoint.Equals(Point.Zero) && r.NextDouble() < 0.5 && (!(this is Farm) || Game1.whichFarm == 1))
				{
					for (int tries = 0; tries < 2; tries++)
					{
						Point p = new Point(r.Next(0, this.map.GetLayer("Back").LayerWidth), r.Next(0, this.map.GetLayer("Back").LayerHeight));
						if (this.isOpenWater(p.X, p.Y))
						{
							int toLand = FishingRod.distanceToLand(p.X, p.Y, this);
							if (toLand > 1 && toLand <= 5)
							{
								if (Game1.player.currentLocation.Equals(this))
								{
									Game1.playSound("waterSlosh");
								}
								this.fishSplashPoint = p;
								this.fishSplashAnimation = new TemporaryAnimatedSprite(51, new Vector2((float)(p.X * Game1.tileSize), (float)(p.Y * Game1.tileSize)), Color.White, 10, false, 80f, 999999, -1, -1f, -1, 0);
								break;
							}
						}
					}
				}
				else if (!this.fishSplashPoint.Equals(Point.Zero) && r.NextDouble() < 0.1)
				{
					this.fishSplashPoint = Point.Zero;
					this.fishSplashAnimation = null;
				}
				if (Game1.player.mailReceived.Contains("ccFishTank") && !(this is Beach) && this.orePanPoint.Equals(Point.Zero) && r.NextDouble() < 0.5)
				{
					for (int tries2 = 0; tries2 < 6; tries2++)
					{
						Point p2 = new Point(r.Next(0, this.map.GetLayer("Back").LayerWidth), r.Next(0, this.map.GetLayer("Back").LayerHeight));
						if (this.isOpenWater(p2.X, p2.Y) && FishingRod.distanceToLand(p2.X, p2.Y, this) <= 1 && this.getTileIndexAt(p2, "Buildings") == -1)
						{
							if (Game1.player.currentLocation.Equals(this))
							{
								Game1.playSound("slosh");
							}
							this.orePanPoint = p2;
							this.orePanAnimation = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), new Vector2((float)(p2.X * Game1.tileSize + Game1.tileSize / 2), (float)(p2.Y * Game1.tileSize + Game1.tileSize / 2)), false, 0f, Color.White)
							{
								totalNumberOfLoops = 9999999,
								interval = 100f,
								scale = (float)(Game1.pixelZoom * 3) / 4f,
								animationLength = 6
							};
							break;
						}
					}
				}
				else if (!this.orePanPoint.Equals(Point.Zero) && r.NextDouble() < 0.1)
				{
					this.orePanPoint = Point.Zero;
					this.orePanAnimation = null;
				}
			}
			if (this.name.Equals("BugLand") && Game1.random.NextDouble() <= 0.2 && Game1.currentLocation.Equals(this))
			{
				this.characters.Add(new Fly(this.getRandomTile() * (float)Game1.tileSize, true));
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0006459A File Offset: 0x0006279A
		public bool dropObject(Object obj)
		{
			return this.dropObject(obj, obj.TileLocation, Game1.viewport, false, null);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00035C5D File Offset: 0x00033E5D
		public virtual int getFishingLocation(Vector2 tile)
		{
			return -1;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000645B0 File Offset: 0x000627B0
		public virtual Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, null);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000645C0 File Offset: 0x000627C0
		public virtual Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency, string locationName = null)
		{
			int whichFish = -1;
			Dictionary<string, string> locationData = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
			bool favBait = false;
			string nameToUse = (locationName == null) ? this.name : locationName;
			if (this.name.Equals("WitchSwamp") && !Game1.player.mailReceived.Contains("henchmanGone") && Game1.random.NextDouble() < 0.25 && !Game1.player.hasItemInInventory(308, 1, 0))
			{
				return new Object(308, 1, false, -1, 0);
			}
			if (locationData.ContainsKey(nameToUse))
			{
				string[] rawFishData = locationData[nameToUse].Split(new char[]
				{
					'/'
				})[4 + Utility.getSeasonNumber(Game1.currentSeason)].Split(new char[]
				{
					' '
				});
				Dictionary<string, string> rawFishDataWithLocation = new Dictionary<string, string>();
				if (rawFishData.Length > 1)
				{
					for (int i = 0; i < rawFishData.Length; i += 2)
					{
						rawFishDataWithLocation.Add(rawFishData[i], rawFishData[i + 1]);
					}
				}
				string[] keys = rawFishDataWithLocation.Keys.ToArray<string>();
				Dictionary<int, string> fishData = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
				Utility.Shuffle<string>(Game1.random, keys);
				for (int j = 0; j < keys.Length; j++)
				{
					bool fail = true;
					string[] specificFishData = fishData[Convert.ToInt32(keys[j])].Split(new char[]
					{
						'/'
					});
					string[] timeSpans = specificFishData[5].Split(new char[]
					{
						' '
					});
					int location = Convert.ToInt32(rawFishDataWithLocation[keys[j]]);
					if (location == -1 || this.getFishingLocation(who.getTileLocation()) == location)
					{
						for (int k = 0; k < timeSpans.Length; k += 2)
						{
							if (Game1.timeOfDay >= Convert.ToInt32(timeSpans[k]) && Game1.timeOfDay < Convert.ToInt32(timeSpans[k + 1]))
							{
								fail = false;
								break;
							}
						}
					}
					if (!specificFishData[7].Equals("both"))
					{
						if (specificFishData[7].Equals("rainy") && !Game1.isRaining)
						{
							fail = true;
						}
						else if (specificFishData[7].Equals("sunny") && Game1.isRaining)
						{
							fail = true;
						}
					}
					if (who.FishingLevel < Convert.ToInt32(specificFishData[12]))
					{
						fail = true;
					}
					if (!fail)
					{
						double chance = Convert.ToDouble(specificFishData[10]);
						double dropOffAmount = Convert.ToDouble(specificFishData[11]) * chance;
						chance -= (double)Math.Max(0, Convert.ToInt32(specificFishData[9]) - waterDepth) * dropOffAmount;
						chance += (double)((float)who.FishingLevel / 50f);
						chance = Math.Min(chance, 0.89999997615814209);
						if (Game1.random.NextDouble() <= chance)
						{
							whichFish = Convert.ToInt32(keys[j]);
							break;
						}
					}
				}
			}
			if (whichFish == -1)
			{
				whichFish = Game1.random.Next(167, 173);
			}
			if ((who.fishCaught == null || who.fishCaught.Count == 0) && whichFish >= 152)
			{
				whichFish = 145;
			}
			Object caught = new Object(whichFish, 1, false, -1, 0);
			if (favBait)
			{
				caught.scale.X = 1f;
			}
			return caught;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000648E4 File Offset: 0x00062AE4
		public virtual bool isActionableTile(int xTile, int yTile, Farmer who)
		{
			bool isActionable = false;
			if (this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings") != null)
			{
				isActionable = true;
				if (this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings").Contains("Message"))
				{
					Game1.isInspectionAtCurrentCursorTile = true;
				}
			}
			if (this.objects.ContainsKey(new Vector2((float)xTile, (float)yTile)) && this.objects[new Vector2((float)xTile, (float)yTile)].isActionable(who))
			{
				isActionable = true;
			}
			if (this.terrainFeatures.ContainsKey(new Vector2((float)xTile, (float)yTile)) && this.terrainFeatures[new Vector2((float)xTile, (float)yTile)].isActionable())
			{
				isActionable = true;
			}
			if (isActionable && !Utility.tileWithinRadiusOfPlayer(xTile, yTile, 1, who))
			{
				Game1.mouseCursorTransparency = 0.5f;
			}
			return isActionable;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000649B0 File Offset: 0x00062BB0
		public void digUpArtifactSpot(int xLocation, int yLocation, Farmer who)
		{
			Random r = new Random(xLocation * 2000 + yLocation + (int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed);
			int toDigUp = -1;
			foreach (KeyValuePair<int, string> v in Game1.objectInformation)
			{
				string[] split = v.Value.Split(new char[]
				{
					'/'
				});
				if (split[3].Contains("Arch"))
				{
					string[] archSplit = split[5].Split(new char[]
					{
						' '
					});
					for (int i = 0; i < archSplit.Length; i += 2)
					{
						if (archSplit[i].Equals(this.name) && r.NextDouble() < Convert.ToDouble(archSplit[i + 1], CultureInfo.InvariantCulture))
						{
							toDigUp = v.Key;
							break;
						}
					}
				}
				if (toDigUp != -1)
				{
					break;
				}
			}
			if (r.NextDouble() < 0.2 && !(this is Farm))
			{
				toDigUp = 102;
			}
			if (toDigUp == 102 && who.archaeologyFound.ContainsKey(102) && who.archaeologyFound[102][0] >= 21)
			{
				toDigUp = 770;
			}
			if (toDigUp != -1)
			{
				Game1.createObjectDebris(toDigUp, xLocation, yLocation, who.uniqueMultiplayerID);
				who.gainExperience(5, 25);
				return;
			}
			if (!Game1.currentSeason.Equals("winter") || r.NextDouble() >= 0.5 || this is Desert)
			{
				Dictionary<string, string> locationData = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
				if (locationData.ContainsKey(this.name))
				{
					string[] rawData = locationData[this.name].Split(new char[]
					{
						'/'
					})[8].Split(new char[]
					{
						' '
					});
					if (rawData.Length != 0 && !rawData[0].Equals("-1"))
					{
						int j = 0;
						while (j < rawData.Length)
						{
							if (r.NextDouble() <= Convert.ToDouble(rawData[j + 1]))
							{
								toDigUp = Convert.ToInt32(rawData[j]);
								if (Game1.objectInformation.ContainsKey(toDigUp) && (Game1.objectInformation[toDigUp].Split(new char[]
								{
									'/'
								})[3].Contains("Arch") || toDigUp == 102))
								{
									if (toDigUp == 102 && who.archaeologyFound.ContainsKey(102) && who.archaeologyFound[102][0] >= 21)
									{
										toDigUp = 770;
									}
									Game1.createObjectDebris(toDigUp, xLocation, yLocation, who.uniqueMultiplayerID);
									return;
								}
								Game1.createMultipleObjectDebris(toDigUp, xLocation, yLocation, r.Next(1, 4), who.uniqueMultiplayerID);
								return;
							}
							else
							{
								j += 2;
							}
						}
					}
				}
				return;
			}
			if (r.NextDouble() < 0.4)
			{
				Game1.createObjectDebris(416, xLocation, yLocation, who.uniqueMultiplayerID);
				return;
			}
			Game1.createObjectDebris(412, xLocation, yLocation, who.uniqueMultiplayerID);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00064CB4 File Offset: 0x00062EB4
		public virtual string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly)
		{
			Random r = new Random(xLocation * 2000 + yLocation * 77 + (int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + (int)Game1.stats.DirtHoed);
			string item = this.doesTileHaveProperty(xLocation, yLocation, "Treasure", "Back");
			if (item != null)
			{
				string[] treasureDescription = item.Split(new char[]
				{
					' '
				});
				if (detectOnly)
				{
					return treasureDescription[0];
				}
				string text = treasureDescription[0];
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1821685427u)
				{
					if (num <= 849800425u)
					{
						if (num != 116937720u)
						{
							if (num == 849800425u)
							{
								if (text == "Arch")
								{
									Game1.createObjectDebris(Convert.ToInt32(treasureDescription[1]), xLocation, yLocation, -1, 0, 1f, null);
								}
							}
						}
						else if (text == "Iridium")
						{
							Game1.createDebris(10, xLocation, yLocation, Convert.ToInt32(treasureDescription[1]), null);
						}
					}
					else if (num != 872197005u)
					{
						if (num == 1821685427u)
						{
							if (text == "Gold")
							{
								Game1.createDebris(6, xLocation, yLocation, Convert.ToInt32(treasureDescription[1]), null);
							}
						}
					}
					else if (text == "Coins")
					{
						Game1.createObjectDebris(330, xLocation, yLocation, -1, 0, 1f, null);
					}
				}
				else if (num <= 2535266071u)
				{
					if (num != 1952841722u)
					{
						if (num == 2535266071u)
						{
							if (text == "CaveCarrot")
							{
								Game1.createObjectDebris(78, xLocation, yLocation, -1, 0, 1f, null);
							}
						}
					}
					else if (text == "Coal")
					{
						Game1.createDebris(4, xLocation, yLocation, Convert.ToInt32(treasureDescription[1]), null);
					}
				}
				else if (num != 3420196507u)
				{
					if (num != 3821421172u)
					{
						if (num == 3851314394u)
						{
							if (text == "Object")
							{
								Game1.createObjectDebris(Convert.ToInt32(treasureDescription[1]), xLocation, yLocation, -1, 0, 1f, null);
								if (Convert.ToInt32(treasureDescription[1]) == 78)
								{
									Stats expr_292 = Game1.stats;
									num = expr_292.CaveCarrotsFound;
									expr_292.CaveCarrotsFound = num + 1u;
								}
							}
						}
					}
					else if (text == "Copper")
					{
						Game1.createDebris(0, xLocation, yLocation, Convert.ToInt32(treasureDescription[1]), null);
					}
				}
				else if (text == "Iron")
				{
					Game1.createDebris(2, xLocation, yLocation, Convert.ToInt32(treasureDescription[1]), null);
				}
				this.map.GetLayer("Back").Tiles[xLocation, yLocation].Properties["Treasure"] = null;
			}
			else
			{
				if (!this.isFarm && this.isOutdoors && Game1.currentSeason.Equals("winter") && r.NextDouble() < 0.08 && !explosion && !detectOnly)
				{
					Game1.createObjectDebris((r.NextDouble() < 0.5) ? 412 : 416, xLocation, yLocation, -1, 0, 1f, null);
					return "";
				}
				if (this.isOutdoors && r.NextDouble() < 0.03 && !explosion)
				{
					if (detectOnly)
					{
						this.map.GetLayer("Back").Tiles[xLocation, yLocation].Properties.Add("Treasure", new PropertyValue("Object " + 330));
						return "Object";
					}
					Game1.createObjectDebris(330, xLocation, yLocation, -1, 0, 1f, null);
					return "";
				}
			}
			return "";
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0006508C File Offset: 0x0006328C
		public void setMapTile(int tileX, int tileY, int index, string layer, string action, int whichTileSheet = 0)
		{
			this.map.GetLayer(layer).Tiles[tileX, tileY] = new StaticTile(this.map.GetLayer(layer), this.map.TileSheets[whichTileSheet], BlendMode.Alpha, index);
			if (action != null && layer != null && layer.Equals("Buildings"))
			{
				this.map.GetLayer("Buildings").Tiles[tileX, tileY].Properties.Add("Action", new PropertyValue(action));
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00065120 File Offset: 0x00063320
		public void setMapTileIndex(int tileX, int tileY, int index, string layer, int whichTileSheet = 0)
		{
			try
			{
				if (this.map.GetLayer(layer).Tiles[tileX, tileY] != null)
				{
					this.map.GetLayer(layer).Tiles[tileX, tileY].TileIndex = index;
				}
				else
				{
					this.map.GetLayer(layer).Tiles[tileX, tileY] = new StaticTile(this.map.GetLayer(layer), this.map.TileSheets[whichTileSheet], BlendMode.Alpha, index);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000651C0 File Offset: 0x000633C0
		public virtual void shiftObjects(int dx, int dy)
		{
			SerializableDictionary<Vector2, Object> newObjects = new SerializableDictionary<Vector2, Object>();
			foreach (KeyValuePair<Vector2, Object> v in this.objects)
			{
				v.Value.tileLocation = new Vector2(v.Key.X + (float)dx, v.Key.Y + (float)dy);
				newObjects.Add(v.Value.tileLocation, v.Value);
			}
			this.objects = null;
			this.objects = newObjects;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00065268 File Offset: 0x00063468
		public int getTileIndexAt(Point p, string layer)
		{
			Tile tmp = this.map.GetLayer(layer).Tiles[p.X, p.Y];
			if (tmp != null)
			{
				return tmp.TileIndex;
			}
			return -1;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000652A4 File Offset: 0x000634A4
		public int getTileIndexAt(int x, int y, string layer)
		{
			if (this.map.GetLayer(layer) == null)
			{
				return -1;
			}
			Tile tmp = this.map.GetLayer(layer).Tiles[x, y];
			if (tmp != null)
			{
				return tmp.TileIndex;
			}
			return -1;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000652E8 File Offset: 0x000634E8
		public bool breakStone(int indexOfStone, int x, int y, Farmer who, Random r)
		{
			int experience = 0;
			int addedOres = who.professions.Contains(18) ? 1 : 0;
			if (indexOfStone <= 668)
			{
				if (indexOfStone <= 77)
				{
					switch (indexOfStone)
					{
					case 2:
						Game1.createObjectDebris(72, x, y, who.uniqueMultiplayerID, this);
						experience = 150;
						goto IL_547;
					case 3:
					case 5:
					case 7:
					case 9:
					case 11:
					case 13:
						goto IL_547;
					case 4:
						Game1.createObjectDebris(64, x, y, who.uniqueMultiplayerID, this);
						experience = 80;
						goto IL_547;
					case 6:
						Game1.createObjectDebris(70, x, y, who.uniqueMultiplayerID, this);
						experience = 40;
						goto IL_547;
					case 8:
						Game1.createObjectDebris(66, x, y, who.uniqueMultiplayerID, this);
						experience = 16;
						goto IL_547;
					case 10:
						Game1.createObjectDebris(68, x, y, who.uniqueMultiplayerID, this);
						experience = 16;
						goto IL_547;
					case 12:
						Game1.createObjectDebris(60, x, y, who.uniqueMultiplayerID, this);
						experience = 80;
						goto IL_547;
					case 14:
						Game1.createObjectDebris(62, x, y, who.uniqueMultiplayerID, this);
						experience = 40;
						goto IL_547;
					default:
						switch (indexOfStone)
						{
						case 75:
							Game1.createObjectDebris(535, x, y, who.uniqueMultiplayerID, this);
							experience = 8;
							goto IL_547;
						case 76:
							Game1.createObjectDebris(536, x, y, who.uniqueMultiplayerID, this);
							experience = 16;
							goto IL_547;
						case 77:
							Game1.createObjectDebris(537, x, y, who.uniqueMultiplayerID, this);
							experience = 32;
							goto IL_547;
						default:
							goto IL_547;
						}
						break;
					}
				}
				else
				{
					if (indexOfStone == 290)
					{
						Game1.createMultipleObjectDebris(380, x, y, addedOres + r.Next(1, 4) + ((r.NextDouble() < (double)((float)who.LuckLevel / 100f)) ? 1 : 0) + ((r.NextDouble() < (double)((float)who.MiningLevel / 100f)) ? 1 : 0), who.uniqueMultiplayerID, this);
						experience = 12;
						this.temporarySprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 3, Color.White * 0.5f, 175, 100, ""));
						goto IL_547;
					}
					if (indexOfStone != 668)
					{
						goto IL_547;
					}
				}
			}
			else if (indexOfStone <= 751)
			{
				if (indexOfStone != 670)
				{
					if (indexOfStone != 751)
					{
						goto IL_547;
					}
					Game1.createMultipleObjectDebris(378, x, y, addedOres + r.Next(1, 4) + ((r.NextDouble() < (double)((float)who.LuckLevel / 100f)) ? 1 : 0) + ((r.NextDouble() < (double)((float)who.MiningLevel / 100f)) ? 1 : 0), who.uniqueMultiplayerID, this);
					experience = 5;
					this.temporarySprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 3, Color.Orange * 0.5f, 175, 100, ""));
					goto IL_547;
				}
			}
			else
			{
				if (indexOfStone == 764)
				{
					Game1.createMultipleObjectDebris(384, x, y, addedOres + r.Next(1, 4) + ((r.NextDouble() < (double)((float)who.LuckLevel / 100f)) ? 1 : 0) + ((r.NextDouble() < (double)((float)who.MiningLevel / 100f)) ? 1 : 0), who.uniqueMultiplayerID, this);
					experience = 18;
					this.temporarySprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 3, Color.Yellow * 0.5f, 175, 100, ""));
					goto IL_547;
				}
				if (indexOfStone != 765)
				{
					goto IL_547;
				}
				Game1.createMultipleObjectDebris(386, x, y, addedOres + r.Next(1, 4) + ((r.NextDouble() < (double)((float)who.LuckLevel / 100f)) ? 1 : 0) + ((r.NextDouble() < (double)((float)who.MiningLevel / 100f)) ? 1 : 0), who.uniqueMultiplayerID, this);
				this.temporarySprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 6, Color.BlueViolet * 0.5f, 175, 100, ""));
				if (r.NextDouble() < 0.04)
				{
					Game1.createMultipleObjectDebris(74, x, y, 1);
				}
				experience = 50;
				goto IL_547;
			}
			Game1.createMultipleObjectDebris(390, x, y, addedOres + r.Next(1, 3) + ((r.NextDouble() < (double)((float)who.LuckLevel / 100f)) ? 1 : 0) + ((r.NextDouble() < (double)((float)who.MiningLevel / 100f)) ? 1 : 0), who.uniqueMultiplayerID, this);
			experience = 3;
			if (r.NextDouble() < 0.08)
			{
				Game1.createMultipleObjectDebris(382, x, y, 1 + addedOres, who.uniqueMultiplayerID, this);
				experience = 4;
			}
			IL_547:
			if (who.professions.Contains(19) && r.NextDouble() < 0.5)
			{
				switch (indexOfStone)
				{
				case 2:
					Game1.createObjectDebris(72, x, y, who.uniqueMultiplayerID, this);
					experience = 100;
					break;
				case 4:
					Game1.createObjectDebris(64, x, y, who.uniqueMultiplayerID, this);
					experience = 50;
					break;
				case 6:
					Game1.createObjectDebris(70, x, y, who.uniqueMultiplayerID, this);
					experience = 20;
					break;
				case 8:
					Game1.createObjectDebris(66, x, y, who.uniqueMultiplayerID, this);
					experience = 8;
					break;
				case 10:
					Game1.createObjectDebris(68, x, y, who.uniqueMultiplayerID, this);
					experience = 8;
					break;
				case 12:
					Game1.createObjectDebris(60, x, y, who.uniqueMultiplayerID, this);
					experience = 50;
					break;
				case 14:
					Game1.createObjectDebris(62, x, y, who.uniqueMultiplayerID, this);
					experience = 20;
					break;
				}
			}
			if (indexOfStone == 46)
			{
				Game1.createDebris(10, x, y, r.Next(1, 4), null);
				Game1.createDebris(6, x, y, r.Next(1, 5), null);
				if (r.NextDouble() < 0.25)
				{
					Game1.createMultipleObjectDebris(74, x, y, 1, who.uniqueMultiplayerID, this);
				}
				experience = 50;
				Stats expr_69F = Game1.stats;
				uint mysticStonesCrushed = expr_69F.MysticStonesCrushed;
				expr_69F.MysticStonesCrushed = mysticStonesCrushed + 1u;
			}
			if ((this.isOutdoors || this.treatAsOutdoors) && experience == 0)
			{
				double chanceModifier = Game1.dailyLuck / 2.0 + (double)who.MiningLevel * 0.005 + (double)who.LuckLevel * 0.001;
				Random ran = new Random(x * 1000 + y + (int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
				Game1.createDebris(14, x, y, 1, this);
				who.gainExperience(3, 1);
				if (who.professions.Contains(21) && ran.NextDouble() < 0.05 * (1.0 + chanceModifier))
				{
					Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, this);
				}
				if (ran.NextDouble() < 0.05 * (1.0 + chanceModifier))
				{
					ran.Next(1, 3);
					ran.NextDouble();
					double arg_7BE_0 = 0.1 * (1.0 + chanceModifier);
					Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, this);
					this.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)(Game1.tileSize * x), (float)(Game1.tileSize * y)), Color.White, 8, Game1.random.NextDouble() < 0.5, 80f, 0, -1, -1f, Game1.tileSize * 2, 0));
					who.gainExperience(3, 5);
				}
			}
			who.gainExperience(3, experience);
			return experience > 0;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00065B38 File Offset: 0x00063D38
		public bool isBehindBush(Vector2 Tile)
		{
			if (this.largeTerrainFeatures != null)
			{
				Microsoft.Xna.Framework.Rectangle down = new Microsoft.Xna.Framework.Rectangle((int)Tile.X * Game1.tileSize, (int)(Tile.Y + 1f) * Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getBoundingBox().Intersects(down))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00065BD8 File Offset: 0x00063DD8
		public bool isBehindTree(Vector2 Tile)
		{
			if (this.terrainFeatures != null)
			{
				Microsoft.Xna.Framework.Rectangle down = new Microsoft.Xna.Framework.Rectangle((int)(Tile.X - 1f) * Game1.tileSize, (int)Tile.Y * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize * 4);
				foreach (KeyValuePair<Vector2, TerrainFeature> i in this.terrainFeatures)
				{
					if (i.Value is Tree && i.Value.getBoundingBox(i.Key).Intersects(down))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00065C9C File Offset: 0x00063E9C
		public virtual void spawnObjects()
		{
			Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed);
			Dictionary<string, string> locationData = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
			if (locationData.ContainsKey(this.name))
			{
				string rawData = locationData[this.name].Split(new char[]
				{
					'/'
				})[Utility.getSeasonNumber(Game1.currentSeason)];
				if (!rawData.Equals("-1") && this.numberOfSpawnedObjectsOnMap < 6)
				{
					string[] split = rawData.Split(new char[]
					{
						' '
					});
					int numberToSpawn = r.Next(1, Math.Min(5, 7 - this.numberOfSpawnedObjectsOnMap));
					for (int i = 0; i < numberToSpawn; i++)
					{
						for (int j = 0; j < 11; j++)
						{
							int xCoord = r.Next(this.map.DisplayWidth / Game1.tileSize);
							int yCoord = r.Next(this.map.DisplayHeight / Game1.tileSize);
							Vector2 location = new Vector2((float)xCoord, (float)yCoord);
							Object o;
							this.objects.TryGetValue(location, out o);
							int whichObject = r.Next(split.Length / 2) * 2;
							if (o == null && this.doesTileHaveProperty(xCoord, yCoord, "Spawnable", "Back") != null && r.NextDouble() < Convert.ToDouble(split[whichObject + 1], CultureInfo.InvariantCulture) && this.isTileLocationTotallyClearAndPlaceable(xCoord, yCoord) && this.getTileIndexAt(xCoord, yCoord, "AlwaysFront") == -1 && !this.isBehindBush(location) && (Game1.random.NextDouble() < 0.1 || !this.isBehindTree(location)) && this.dropObject(new Object(location, Convert.ToInt32(split[whichObject]), null, false, true, false, true), new Vector2((float)(xCoord * Game1.tileSize), (float)(yCoord * Game1.tileSize)), Game1.viewport, true, null))
							{
								this.numberOfSpawnedObjectsOnMap++;
								break;
							}
						}
					}
				}
			}
			List<Vector2> positionOfArtifactSpots = new List<Vector2>();
			foreach (KeyValuePair<Vector2, Object> v in this.objects)
			{
				if (v.Value.parentSheetIndex == 590)
				{
					positionOfArtifactSpots.Add(v.Key);
				}
			}
			if (!(this is Farm))
			{
				this.spawnWeedsAndStones(-1, false, true);
			}
			for (int k = positionOfArtifactSpots.Count - 1; k >= 0; k--)
			{
				if (Game1.random.NextDouble() < 0.15)
				{
					this.objects.Remove(positionOfArtifactSpots.ElementAt(k));
					positionOfArtifactSpots.RemoveAt(k);
				}
			}
			if (positionOfArtifactSpots.Count <= ((this is Farm) ? 0 : 1) || (Game1.currentSeason.Equals("winter") && positionOfArtifactSpots.Count <= 4))
			{
				double chanceForNewArtifactAttempt = 1.0;
				while (r.NextDouble() < chanceForNewArtifactAttempt)
				{
					int xCoord2 = r.Next(this.map.DisplayWidth / Game1.tileSize);
					int yCoord2 = r.Next(this.map.DisplayHeight / Game1.tileSize);
					Vector2 location2 = new Vector2((float)xCoord2, (float)yCoord2);
					if (this.isTileLocationTotallyClearAndPlaceable(location2) && this.getTileIndexAt(xCoord2, yCoord2, "AlwaysFront") == -1 && this.getTileIndexAt(xCoord2, yCoord2, "Front") == -1 && !this.isBehindBush(location2) && (this.doesTileHaveProperty(xCoord2, yCoord2, "Diggable", "Back") != null || (Game1.currentSeason.Equals("winter") && this.doesTileHaveProperty(xCoord2, yCoord2, "Type", "Back") != null && this.doesTileHaveProperty(xCoord2, yCoord2, "Type", "Back").Equals("Grass"))))
					{
						if (this.name.Equals("Forest") && xCoord2 >= 93 && yCoord2 <= 22)
						{
							continue;
						}
						this.objects.Add(location2, new Object(location2, 590, 1));
					}
					chanceForNewArtifactAttempt *= 0.75;
					if (Game1.currentSeason.Equals("winter"))
					{
						chanceForNewArtifactAttempt += 0.10000000149011612;
					}
				}
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00066108 File Offset: 0x00064308
		public bool isTileLocationOpen(Location location)
		{
			return this.map.GetLayer("Buildings").PickTile(location, Game1.viewport.Size) == null && this.doesTileHaveProperty(location.X, location.X, "Water", "Back") == null && this.map.GetLayer("Front").PickTile(location, Game1.viewport.Size) == null && (this.map.GetLayer("AlwaysFront") == null || this.map.GetLayer("AlwaysFront").PickTile(location, Game1.viewport.Size) == null);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000661B0 File Offset: 0x000643B0
		public bool isTileLocationOpenIgnoreFrontLayers(Location location)
		{
			return this.map.GetLayer("Buildings").PickTile(location, Game1.viewport.Size) == null && this.doesTileHaveProperty(location.X, location.X, "Water", "Back") == null;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00066200 File Offset: 0x00064400
		public void spawnWeedsAndStones(int numDebris = -1, bool weedsOnly = false, bool spawnFromOldWeeds = true)
		{
			if (this is Farm && (this as Farm).isBuildingConstructed("Gold Clock"))
			{
				return;
			}
			if (!(this is Beach) && !Game1.currentSeason.Equals("winter") && !(this is Desert))
			{
				int numWeedsAndStones = (numDebris != -1) ? numDebris : ((Game1.random.NextDouble() < 0.95) ? ((Game1.random.NextDouble() < 0.25) ? Game1.random.Next(10, 21) : Game1.random.Next(5, 11)) : 0);
				if (Game1.isRaining)
				{
					numWeedsAndStones *= 2;
				}
				if (Game1.dayOfMonth == 1)
				{
					numWeedsAndStones *= 5;
				}
				if (this.objects.Count <= 0 & spawnFromOldWeeds)
				{
					return;
				}
				if (!(this is Farm))
				{
					numWeedsAndStones /= 2;
				}
				for (int i = 0; i < numWeedsAndStones; i++)
				{
					Vector2 v = spawnFromOldWeeds ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : new Vector2((float)Game1.random.Next(this.map.Layers[0].LayerWidth), (float)Game1.random.Next(this.map.Layers[0].LayerHeight));
					while (spawnFromOldWeeds && v.Equals(Vector2.Zero))
					{
						v = new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2));
					}
					KeyValuePair<Vector2, Object> o = new KeyValuePair<Vector2, Object>(Vector2.Zero, null);
					if (spawnFromOldWeeds)
					{
						o = this.objects.ElementAt(Game1.random.Next(this.objects.Count));
					}
					Vector2 baseVect = spawnFromOldWeeds ? o.Key : Vector2.Zero;
					bool spawnOnDiggable = this is Farm;
					if (((spawnOnDiggable && this.doesTileHaveProperty((int)(v.X + baseVect.X), (int)(v.Y + baseVect.Y), "Diggable", "Back") != null) || (!spawnOnDiggable && this.doesTileHaveProperty((int)(v.X + baseVect.X), (int)(v.Y + baseVect.Y), "Diggable", "Back") == null)) && (this.doesTileHaveProperty((int)(v.X + baseVect.X), (int)(v.Y + baseVect.Y), "Type", "Back") == null || !this.doesTileHaveProperty((int)(v.X + baseVect.X), (int)(v.Y + baseVect.Y), "Type", "Back").Equals("Wood")) && (this.isTileLocationTotallyClearAndPlaceable(v + baseVect) || (spawnFromOldWeeds && ((this.objects.ContainsKey(v + baseVect) && this.objects[v + baseVect].parentSheetIndex != 105) || (this.terrainFeatures.ContainsKey(v + baseVect) && (this.terrainFeatures[v + baseVect] is HoeDirt || this.terrainFeatures[v + baseVect] is Flooring))))) && this.doesTileHaveProperty((int)(v.X + baseVect.X), (int)(v.Y + baseVect.Y), "NoSpawn", "Back") == null && (spawnFromOldWeeds || !this.objects.ContainsKey(v + baseVect)))
					{
						int whatToAdd = -1;
						if (this is Desert)
						{
							whatToAdd = 750;
						}
						else
						{
							if (Game1.random.NextDouble() < 0.5 && !weedsOnly && (!spawnFromOldWeeds || o.Value.Name.Equals("Stone") || o.Value.Name.Contains("Twig")))
							{
								if (Game1.random.NextDouble() < 0.5)
								{
									whatToAdd = ((Game1.random.NextDouble() < 0.5) ? 294 : 295);
								}
								else
								{
									whatToAdd = ((Game1.random.NextDouble() < 0.5) ? 343 : 450);
								}
							}
							else if (!spawnFromOldWeeds || o.Value.Name.Contains("Weed"))
							{
								whatToAdd = GameLocation.getWeedForSeason(Game1.random, Game1.currentSeason);
							}
							if (this is Farm && !spawnFromOldWeeds && Game1.random.NextDouble() < 0.05)
							{
								this.terrainFeatures.Add(v + baseVect, new Tree(Game1.random.Next(3) + 1, Game1.random.Next(3)));
								goto IL_658;
							}
						}
						if (whatToAdd != -1)
						{
							bool destroyed = false;
							if (this.objects.ContainsKey(v + baseVect))
							{
								Object removed = this.objects[v + baseVect];
								if (removed is Fence || removed is Chest)
								{
									goto IL_658;
								}
								if (removed.name != null && !removed.Name.Contains("Weed") && !removed.Name.Equals("Stone") && !removed.name.Contains("Twig") && removed.name.Length > 0)
								{
									destroyed = true;
									Game1.debugOutput = removed.Name + " was destroyed";
								}
								this.objects.Remove(v + baseVect);
							}
							else if (this.terrainFeatures.ContainsKey(v + baseVect))
							{
								try
								{
									destroyed = (this.terrainFeatures[v + baseVect] is HoeDirt || this.terrainFeatures[v + baseVect] is Flooring);
								}
								catch (Exception)
								{
								}
								if (!destroyed)
								{
									return;
								}
								this.terrainFeatures.Remove(v + baseVect);
							}
							if (destroyed && this is Farm)
							{
								Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:Farm_WeedsDestruction", new object[0]));
							}
							this.objects.Add(v + baseVect, new Object(v + baseVect, whatToAdd, 1));
						}
					}
					IL_658:;
				}
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00066880 File Offset: 0x00064A80
		public virtual void removeEverythingExceptCharactersFromThisTile(int x, int y)
		{
			Vector2 v = new Vector2((float)x, (float)y);
			if (this.terrainFeatures.ContainsKey(v))
			{
				this.terrainFeatures.Remove(v);
			}
			if (this.objects.ContainsKey(v))
			{
				this.objects.Remove(v);
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000668D0 File Offset: 0x00064AD0
		public virtual void removeEverythingFromThisTile(int x, int y)
		{
			Vector2 v = new Vector2((float)x, (float)y);
			if (this.terrainFeatures.ContainsKey(v))
			{
				this.terrainFeatures.Remove(v);
			}
			if (this.objects.ContainsKey(v))
			{
				this.objects.Remove(v);
			}
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i].getTileLocation().Equals(v) && this.characters[i] is Monster)
				{
					this.characters.RemoveAt(i);
				}
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00066974 File Offset: 0x00064B74
		public void checkForEvents()
		{
			if (Game1.killScreen && !Game1.eventUp)
			{
				if (this.name.Equals("Mine"))
				{
					string rescuer;
					string uniquemessage;
					switch (Game1.random.Next(7))
					{
					case 0:
						rescuer = "Robin";
						uniquemessage = "Data\\ExtraDialogue:Mines_PlayerKilled_Robin";
						goto IL_B8;
					case 1:
						rescuer = "Clint";
						uniquemessage = "Data\\ExtraDialogue:Mines_PlayerKilled_Clint";
						goto IL_B8;
					case 2:
						rescuer = "Maru";
						uniquemessage = ((Game1.player.spouse != null && Game1.player.spouse.Equals("Maru")) ? "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_Spouse" : "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_NotSpouse");
						goto IL_B8;
					}
					rescuer = "Linus";
					uniquemessage = "Data\\ExtraDialogue:Mines_PlayerKilled_Linus";
					IL_B8:
					if (Game1.random.NextDouble() < 0.1 && Game1.player.spouse != null && !Game1.player.spouse.Contains("engaged") && Game1.player.spouse.Length > 1)
					{
						rescuer = Game1.player.spouse;
						uniquemessage = (Game1.player.isMale ? "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerMale" : "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerFemale");
					}
					this.currentEvent = new Event(Game1.content.LoadString("Data\\Events\\Mine:PlayerKilled", new object[]
					{
						rescuer,
						uniquemessage,
						Game1.player.name
					}), -1);
				}
				else if (this.name.Equals("Hospital"))
				{
					this.currentEvent = new Event(Game1.content.LoadString("Data\\Events\\Hospital:PlayerKilled", new object[]
					{
						Game1.player.name
					}), -1);
				}
				Game1.eventUp = true;
				Game1.killScreen = false;
				Game1.player.health = 10;
				return;
			}
			if (!Game1.eventUp && Game1.farmEvent == null)
			{
				string key = Game1.currentSeason + Game1.dayOfMonth;
				try
				{
					Event festival = new Event();
					if (festival.tryToLoadFestival(key))
					{
						this.currentEvent = festival;
					}
				}
				catch (Exception)
				{
				}
				if (!Game1.eventUp && this.currentEvent == null)
				{
					Dictionary<string, string> events = null;
					try
					{
						events = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + this.name);
					}
					catch (Exception)
					{
						return;
					}
					if (events != null)
					{
						string[] keys = events.Keys.ToArray<string>();
						for (int i = 0; i < keys.Length; i++)
						{
							int precondition = this.checkEventPrecondition(keys[i]);
							if (precondition != -1)
							{
								this.currentEvent = new Event(events[keys[i]], precondition);
								break;
							}
						}
						if (this.currentEvent == null && this.name.Equals("Farm") && !Game1.player.mailReceived.Contains("rejectedPet") && Game1.stats.DaysPlayed >= 20u && !Game1.player.hasPet())
						{
							for (int j = 0; j < keys.Length; j++)
							{
								if ((keys[j].Contains("dog") && !Game1.player.catPerson) || (keys[j].Contains("cat") && Game1.player.catPerson))
								{
									this.currentEvent = new Event(events[keys[j]], -1);
									Game1.player.eventsSeen.Add(Convert.ToInt32(keys[j].Split(new char[]
									{
										'/'
									})[0]));
									break;
								}
							}
						}
					}
				}
				if (this.currentEvent != null)
				{
					if (Game1.player.getMount() != null)
					{
						this.currentEvent.playerWasMounted = true;
						Game1.player.getMount().dismount();
					}
					using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.clearTextAboveHead();
						}
					}
					Game1.eventUp = true;
					Game1.displayHUD = false;
					Game1.player.CanMove = false;
					Game1.player.showNotCarrying();
					if (this.critters != null)
					{
						this.critters.Clear();
					}
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00066DC4 File Offset: 0x00064FC4
		public virtual void drawWater(SpriteBatch b)
		{
			if (this.currentEvent != null)
			{
				this.currentEvent.drawUnderWater(b);
			}
			if (this.waterTiles == null)
			{
				return;
			}
			for (int y = Math.Max(0, Game1.viewport.Y / Game1.tileSize - 1); y < Math.Min(this.map.Layers[0].LayerHeight, (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 2); y++)
			{
				for (int x = Math.Max(0, Game1.viewport.X / Game1.tileSize - 1); x < Math.Min(this.map.Layers[0].LayerWidth, (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 1); x++)
				{
					if (this.waterTiles[x, y])
					{
						bool arg_17B_0 = y == this.map.Layers[0].LayerHeight - 1 || !this.waterTiles[x, y + 1];
						bool topY = y == 0 || !this.waterTiles[x, y - 1];
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - (int)((!topY) ? this.waterPosition : 0f)))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.waterAnimationIndex * 64, 2064 + (((x + y) % 2 == 0) ? (this.waterTileFlip ? 128 : 0) : (this.waterTileFlip ? 0 : 128)) + (topY ? ((int)this.waterPosition) : 0), 64, 64 + (topY ? ((int)(-(int)this.waterPosition)) : 0))), this.waterColor, 0f, Vector2.Zero, 1f * ((float)Game1.pixelZoom / 4f), SpriteEffects.None, 0.56f);
						if (arg_17B_0)
						{
							b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y + 1) * Game1.tileSize - (int)this.waterPosition))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.waterAnimationIndex * 64, 2064 + (((x + (y + 1)) % 2 == 0) ? (this.waterTileFlip ? 128 : 0) : (this.waterTileFlip ? 0 : 128)), 64, 64 - (int)(64f - this.waterPosition) - 1)), this.waterColor, 0f, Vector2.Zero, 1f * ((float)Game1.pixelZoom / 4f), SpriteEffects.None, 0.56f);
						}
					}
				}
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0006708C File Offset: 0x0006528C
		public TemporaryAnimatedSprite getTemporarySpriteByID(int id)
		{
			for (int i = 0; i < this.temporarySprites.Count; i++)
			{
				if (this.temporarySprites[i].id == (float)id)
				{
					return this.temporarySprites[i];
				}
			}
			return null;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x000670D4 File Offset: 0x000652D4
		protected void drawDebris(SpriteBatch b)
		{
			int counter = 0;
			foreach (Debris d in this.debris)
			{
				counter++;
				if (d.item != null)
				{
					d.item.drawInMenu(b, Game1.GlobalToLocal(Game1.viewport, d.Chunks[0].position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))), 0.8f + (float)d.itemQuality * 0.1f, 1f, ((float)(d.chunkFinalYLevel + Game1.tileSize) + d.Chunks[0].position.X / 10000f) / 10000f, false);
				}
				else if (d.debrisType == Debris.DebrisType.LETTERS)
				{
					Game1.drawWithBorder(d.debrisMessage, Color.Black, d.nonSpriteChunkColor, Game1.GlobalToLocal(Game1.viewport, d.Chunks[0].position), d.Chunks[0].rotation, d.Chunks[0].scale, (d.Chunks[0].position.Y + (float)Game1.tileSize) / 10000f);
				}
				else if (d.debrisType == Debris.DebrisType.NUMBERS)
				{
					NumberSprite.draw(d.chunkType, b, Game1.GlobalToLocal(Game1.viewport, new Vector2(d.Chunks[0].position.X, (float)d.chunkFinalYLevel - ((float)d.chunkFinalYLevel - d.Chunks[0].position.Y))), d.nonSpriteChunkColor, d.Chunks[0].scale * 0.75f, 0.98f + 0.0001f * (float)counter, d.Chunks[0].alpha, -1 * (int)((float)d.chunkFinalYLevel - d.Chunks[0].position.Y) / 2, 0);
				}
				else if (d.debrisType == Debris.DebrisType.SPRITECHUNKS)
				{
					for (int i = 0; i < d.Chunks.Count; i++)
					{
						b.Draw(d.spriteChunkSheet, Game1.GlobalToLocal(Game1.viewport, d.Chunks[i].position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(d.Chunks[i].xSpriteSheet, d.Chunks[i].ySpriteSheet, Math.Min(d.sizeOfSourceRectSquares, d.spriteChunkSheet.Bounds.Width), Math.Min(d.sizeOfSourceRectSquares, d.spriteChunkSheet.Bounds.Height))), d.nonSpriteChunkColor * d.Chunks[i].alpha, d.Chunks[i].rotation, new Vector2((float)(d.sizeOfSourceRectSquares / 2), (float)(d.sizeOfSourceRectSquares / 2)), d.Chunks[i].scale, SpriteEffects.None, ((float)(d.chunkFinalYLevel + Game1.tileSize / 4) + d.Chunks[i].position.X / 10000f) / 10000f);
					}
				}
				else if (d.debrisType == Debris.DebrisType.SQUARES)
				{
					for (int j = 0; j < d.Chunks.Count; j++)
					{
						b.Draw(Game1.littleEffect, Game1.GlobalToLocal(Game1.viewport, d.Chunks[j].position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 4, 4)), d.nonSpriteChunkColor, 0f, Vector2.Zero, 1f + d.Chunks[j].yVelocity / 2f, SpriteEffects.None, (d.Chunks[j].position.Y + (float)Game1.tileSize) / 10000f);
					}
				}
				else if (d.debrisType != Debris.DebrisType.CHUNKS)
				{
					for (int k = 0; k < d.Chunks.Count; k++)
					{
						if (d.Chunks[k].debrisType <= 0)
						{
							b.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, d.Chunks[k].position + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.getArbitrarySourceRect(Game1.bigCraftableSpriteSheet, 16, 32, -d.Chunks[k].debrisType)), Color.White, 0f, new Vector2(8f, 32f), 3.2f, SpriteEffects.None, (d.Chunks[k].position.Y + (float)(Game1.tileSize / 4) + d.Chunks[k].position.X / 20000f) / 10000f);
							b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(d.Chunks[k].position.X + (float)Game1.tileSize * 0.4f, (float)(d.chunkFinalYLevel + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f - ((float)d.chunkFinalYLevel - d.Chunks[k].position.Y) / 128f, SpriteEffects.None, (float)d.chunkFinalYLevel / 10000f);
						}
						else
						{
							b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, d.Chunks[k].position), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, d.Chunks[k].debrisType, 16, 16)), Color.White, 0f, Vector2.Zero, (d.debrisType == Debris.DebrisType.RESOURCE || d.floppingFish) ? ((float)Game1.pixelZoom) : ((float)Game1.pixelZoom * (0.8f + (float)d.itemQuality * 0.1f)), (d.floppingFish && d.Chunks[k].bounces % 2 == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, ((float)(d.chunkFinalYLevel + Game1.tileSize / 2) + d.Chunks[k].position.X / 10000f) / 10000f);
							b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(d.Chunks[k].position.X + (float)Game1.tileSize * 0.4f, (float)(d.chunkFinalYLevel + Game1.tileSize / 2 + Game1.tileSize / 5 * d.itemQuality))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * 0.75f, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f - ((float)d.chunkFinalYLevel - d.Chunks[k].position.Y) / 96f, SpriteEffects.None, (float)d.chunkFinalYLevel / 10000f);
						}
					}
				}
				else
				{
					for (int l = 0; l < d.Chunks.Count; l++)
					{
						b.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, d.Chunks[l].position), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, d.Chunks[l].debrisType, 16, 16)), d.chunksColor, 0f, Vector2.Zero, (float)Game1.pixelZoom * d.scale, SpriteEffects.None, (d.Chunks[l].position.Y + (float)(Game1.tileSize * 2) + d.Chunks[l].position.X / 10000f) / 10000f);
					}
				}
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000679A0 File Offset: 0x00065BA0
		public virtual void draw(SpriteBatch b)
		{
			if (!Game1.eventUp)
			{
				for (int i = 0; i < this.characters.Count; i++)
				{
					if (this.characters[i] != null)
					{
						this.characters[i].draw(b);
					}
				}
			}
			for (int j = 0; j < this.projectiles.Count; j++)
			{
				this.projectiles[j].draw(b);
			}
			for (int k = 0; k < this.farmers.Count; k++)
			{
				if (!this.farmers[k].uniqueMultiplayerID.Equals(Game1.player.uniqueMultiplayerID))
				{
					this.farmers[k].draw(b);
				}
			}
			if (this.critters != null)
			{
				for (int l = 0; l < this.critters.Count; l++)
				{
					this.critters[l].draw(b);
				}
			}
			this.drawDebris(b);
			if (!Game1.eventUp || (this.currentEvent != null && this.currentEvent.showGroundObjects))
			{
				foreach (KeyValuePair<Vector2, Object> v in this.objects)
				{
					v.Value.draw(b, (int)v.Key.X, (int)v.Key.Y, 1f);
				}
			}
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = this.TemporarySprites.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.draw(b, false, 0, 0);
				}
			}
			if (this.doorSprites != null)
			{
				using (Dictionary<Point, TemporaryAnimatedSprite>.ValueCollection.Enumerator enumerator3 = this.doorSprites.Values.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.draw(b, false, 0, 0);
					}
				}
			}
			if (this.largeTerrainFeatures != null)
			{
				using (List<LargeTerrainFeature>.Enumerator enumerator4 = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						enumerator4.Current.draw(b);
					}
				}
			}
			if (this.fishSplashAnimation != null)
			{
				this.fishSplashAnimation.draw(b, false, 0, 0);
			}
			if (this.orePanAnimation != null)
			{
				this.orePanAnimation.draw(b, false, 0, 0);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00067C28 File Offset: 0x00065E28
		public virtual void drawAboveFrontLayer(SpriteBatch b)
		{
			if (!Game1.isFestival())
			{
				Vector2 tile = default(Vector2);
				for (int y = Game1.viewport.Y / Game1.tileSize - 1; y < (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 7; y++)
				{
					for (int x = Game1.viewport.X / Game1.tileSize - 1; x < (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 3; x++)
					{
						tile.X = (float)x;
						tile.Y = (float)y;
						if (this.terrainFeatures.ContainsKey(tile))
						{
							this.terrainFeatures[tile].draw(b, tile);
						}
					}
				}
			}
			if (this.lightGlows.Count > 0)
			{
				this.drawLightGlows(b);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00067D04 File Offset: 0x00065F04
		public virtual void drawLightGlows(SpriteBatch b)
		{
			foreach (Vector2 v in this.lightGlows)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, v), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(21, 1695, 41, 67)), Color.White, 0f, new Vector2(19f, 22f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool performToolAction(Tool t, int tileX, int tileY)
		{
			return false;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00067DA0 File Offset: 0x00065FA0
		public virtual void seasonUpdate(string season, bool onLoad = false)
		{
			for (int i = this.terrainFeatures.Count - 1; i >= 0; i--)
			{
				if (this.terrainFeatures.Values.ElementAt(i).seasonUpdate(onLoad))
				{
					this.terrainFeatures.Remove(this.terrainFeatures.Keys.ElementAt(i));
				}
			}
			if (this.largeTerrainFeatures != null)
			{
				for (int j = this.largeTerrainFeatures.Count - 1; j >= 0; j--)
				{
					if (this.largeTerrainFeatures.ElementAt(j).seasonUpdate(onLoad))
					{
						this.largeTerrainFeatures.Remove(this.largeTerrainFeatures.ElementAt(j));
					}
				}
			}
			foreach (NPC k in this.getCharacters())
			{
				if (!k.IsMonster)
				{
					k.loadSeasonalDialogue();
				}
			}
			if (this.IsOutdoors && !onLoad)
			{
				for (int l = this.objects.Count - 1; l >= 0; l--)
				{
					if (this.objects.ElementAt(l).Value.IsSpawnedObject && !this.objects.ElementAt(l).Value.Name.Equals("Stone"))
					{
						this.objects.Remove(this.objects.ElementAt(l).Key);
					}
				}
				this.numberOfSpawnedObjectsOnMap = 0;
			}
			string a = season.ToLower();
			if (a == "spring")
			{
				this.waterColor = new Color(120, 200, 255) * 0.5f;
				return;
			}
			if (a == "summer")
			{
				this.waterColor = new Color(60, 240, 255) * 0.5f;
				return;
			}
			if (a == "fall")
			{
				this.waterColor = new Color(255, 130, 200) * 0.5f;
				return;
			}
			if (!(a == "winter"))
			{
				return;
			}
			this.waterColor = new Color(130, 80, 255) * 0.5f;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00067FFC File Offset: 0x000661FC
		private int checkEventPrecondition(string precondition)
		{
			string[] split = precondition.Split(new char[]
			{
				'/'
			});
			try
			{
				if (Game1.player.eventsSeen.Contains(Convert.ToInt32(split[0])))
				{
					int result = -1;
					return result;
				}
			}
			catch (Exception)
			{
				int result = -1;
				return result;
			}
			for (int i = 1; i < split.Length; i++)
			{
				if (split[i][0] == 'e')
				{
					if (this.checkEventsSeenPreconditions(split[i].Split(new char[]
					{
						' '
					})))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'h')
				{
					if (Game1.player.hasPet())
					{
						return -1;
					}
					if ((Game1.player.catPerson && !split[i].Split(new char[]
					{
						' '
					})[1].ToString().ToLower().Equals("cat")) || (!Game1.player.catPerson && !split[i].Split(new char[]
					{
						' '
					})[1].ToString().ToLower().Equals("dog")))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'm')
				{
					string[] moneySplit = split[i].Split(new char[]
					{
						' '
					});
					if ((ulong)Game1.player.totalMoneyEarned < (ulong)((long)Convert.ToInt32(moneySplit[1])))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'M')
				{
					string[] moneySplit2 = split[i].Split(new char[]
					{
						' '
					});
					if (Game1.player.money < Convert.ToInt32(moneySplit2[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'c')
				{
					if (Game1.player.freeSpotsInInventory() < Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'C')
				{
					if (!Game1.player.eventsSeen.Contains(191393) && !Game1.player.eventsSeen.Contains(502261) && !Game1.player.hasCompletedCommunityCenter())
					{
						return -1;
					}
				}
				else if (split[i][0] == 'D')
				{
					if (!Game1.getCharacterFromName(split[i].Split(new char[]
					{
						' '
					})[1], true).datingFarmer)
					{
						return -1;
					}
				}
				else if (split[i][0] == 'j')
				{
					if ((ulong)Game1.stats.DaysPlayed <= (ulong)((long)Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1])))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'J')
				{
					if (!this.checkJojaCompletePrerequisite())
					{
						return -1;
					}
				}
				else if (split[i][0] == 'f')
				{
					if (!this.checkFriendshipPrecondition(split[i].Split(new char[]
					{
						' '
					})))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'F')
				{
					if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'r')
				{
					string[] rollSplit = split[i].Split(new char[]
					{
						' '
					});
					if (Game1.random.NextDouble() > Convert.ToDouble(rollSplit[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 's')
				{
					if (!this.checkItemsPrecondition(split[i].Split(new char[]
					{
						' '
					})))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'q')
				{
					if (!this.checkDialoguePrecondition(split[i].Split(new char[]
					{
						' '
					})))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'n')
				{
					if (!Game1.player.mailReceived.Contains(split[i].Split(new char[]
					{
						' '
					})[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'l')
				{
					if (Game1.player.mailReceived.Contains(split[i].Split(new char[]
					{
						' '
					})[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 't')
				{
					string[] timeSplit = split[i].Split(new char[]
					{
						' '
					});
					if (Game1.timeOfDay < Convert.ToInt32(timeSplit[1]) || Game1.timeOfDay > Convert.ToInt32(timeSplit[2]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'w')
				{
					string[] weatherSplit = split[i].Split(new char[]
					{
						' '
					});
					if ((weatherSplit[1].Equals("rainy") && !Game1.isRaining) || (weatherSplit[1].Equals("sunny") && Game1.isRaining))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'd')
				{
					if (split[i].Split(new char[]
					{
						' '
					}).Contains(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'o')
				{
					if (Game1.player.spouse != null && Game1.player.spouse.Equals(split[i].Split(new char[]
					{
						' '
					})[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'v')
				{
					if (Game1.getCharacterFromName(split[i].Split(new char[]
					{
						' '
					})[1], false).isInvisible)
					{
						return -1;
					}
				}
				else if (split[i][0] == 'p')
				{
					string[] presentSplit = split[i].Split(new char[]
					{
						' '
					});
					if (!this.isCharacterHere(presentSplit[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'z')
				{
					string[] seasonSplit = split[i].Split(new char[]
					{
						' '
					});
					if (Game1.currentSeason.Equals(seasonSplit[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'b')
				{
					string[] mineSplit = split[i].Split(new char[]
					{
						' '
					});
					if (Game1.player.timesReachedMineBottom < Convert.ToInt32(mineSplit[1]))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'y')
				{
					if (Game1.year < Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1]) || (Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1]) == 1 && Game1.year != 1))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'g')
				{
					if (!(Game1.player.isMale ? "male" : "female").Equals(split[i].Split(new char[]
					{
						' '
					})[1].ToLower()))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'i')
				{
					if (!Game1.player.hasItemInInventory(Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1]), 1, 0) && (Game1.player.ActiveObject == null || Game1.player.ActiveObject.ParentSheetIndex != Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1])))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'k')
				{
					if (!this.checkEventsSeenPreconditions(split[i].Split(new char[]
					{
						' '
					})))
					{
						return -1;
					}
				}
				else if (split[i][0] == 'a')
				{
					if (Game1.player.getTileLocation().X != (float)Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[1]) || Game1.player.getTileLocation().Y != (float)Convert.ToInt32(split[i].Split(new char[]
					{
						' '
					})[2]))
					{
						return -1;
					}
				}
				else
				{
					if (split[i][0] == 'x')
					{
						Game1.addMailForTomorrow(split[i].Split(new char[]
						{
							' '
						})[1], false, false);
						Game1.player.eventsSeen.Add(Convert.ToInt32(split[0]));
						return -1;
					}
					if (split[i][0] != 'u')
					{
						return -1;
					}
					bool foundDay = false;
					string[] dayssplit = split[i].Split(new char[]
					{
						' '
					});
					for (int j = 1; j < dayssplit.Length; j++)
					{
						if (Game1.dayOfMonth == Convert.ToInt32(dayssplit[j]))
						{
							foundDay = true;
							break;
						}
					}
					if (!foundDay)
					{
						return -1;
					}
				}
			}
			return Convert.ToInt32(split[0]);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00068854 File Offset: 0x00066A54
		private bool isCharacterHere(string name)
		{
			using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name.Equals(name))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000688B4 File Offset: 0x00066AB4
		private bool checkJojaCompletePrerequisite()
		{
			bool foundJoja = false;
			if (Game1.player.mailReceived.Contains("jojaVault"))
			{
				foundJoja = true;
			}
			else if (!Game1.player.mailReceived.Contains("ccVault"))
			{
				return false;
			}
			if (Game1.player.mailReceived.Contains("jojaPantry"))
			{
				foundJoja = true;
			}
			else if (!Game1.player.mailReceived.Contains("ccPantry"))
			{
				return false;
			}
			if (Game1.player.mailReceived.Contains("jojaBoilerRoom"))
			{
				foundJoja = true;
			}
			else if (!Game1.player.mailReceived.Contains("ccBoilerRoom"))
			{
				return false;
			}
			if (Game1.player.mailReceived.Contains("jojaCraftsRoom"))
			{
				foundJoja = true;
			}
			else if (!Game1.player.mailReceived.Contains("ccCraftsRoom"))
			{
				return false;
			}
			if (Game1.player.mailReceived.Contains("jojaFishTank"))
			{
				foundJoja = true;
			}
			else if (!Game1.player.mailReceived.Contains("ccFishTank"))
			{
				return false;
			}
			return foundJoja || Game1.player.mailReceived.Contains("JojaMember");
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000689DC File Offset: 0x00066BDC
		private bool checkEventsSeenPreconditions(string[] eventIDs)
		{
			for (int i = 1; i < eventIDs.Length; i++)
			{
				int junk;
				if (int.TryParse(eventIDs[i], out junk) && Game1.player.eventsSeen.Contains(Convert.ToInt32(eventIDs[i])))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00068A20 File Offset: 0x00066C20
		private bool checkFriendshipPrecondition(string[] friendshipString)
		{
			for (int i = 1; i < friendshipString.Length; i += 2)
			{
				if (!Game1.player.friendships.ContainsKey(friendshipString[i]) || Game1.player.friendships[friendshipString[i]][0] < Convert.ToInt32(friendshipString[i + 1]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00068A74 File Offset: 0x00066C74
		private bool checkItemsPrecondition(string[] itemString)
		{
			for (int i = 1; i < itemString.Length; i += 2)
			{
				if (!Game1.player.basicShipped.ContainsKey(Convert.ToInt32(itemString[i])) || Game1.player.basicShipped[Convert.ToInt32(itemString[i])] < Convert.ToInt32(itemString[i + 1]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00068AD0 File Offset: 0x00066CD0
		private bool checkDialoguePrecondition(string[] dialogueString)
		{
			for (int i = 1; i < dialogueString.Length; i += 2)
			{
				if (!Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(dialogueString[i])))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00068B08 File Offset: 0x00066D08
		public void loadObjects()
		{
			if (this.map != null)
			{
				this.warps.Clear();
				PropertyValue warpsUnparsed;
				this.map.Properties.TryGetValue("Warp", out warpsUnparsed);
				if (warpsUnparsed != null)
				{
					string[] warpInfo = warpsUnparsed.ToString().Split(new char[]
					{
						' '
					});
					for (int i = 0; i < warpInfo.Length; i += 5)
					{
						this.warps.Add(new Warp(Convert.ToInt32(warpInfo[i]), Convert.ToInt32(warpInfo[i + 1]), warpInfo[i + 2], Convert.ToInt32(warpInfo[i + 3]), Convert.ToInt32(warpInfo[i + 4]), false));
					}
				}
				PropertyValue isOutdoorsValue;
				this.map.Properties.TryGetValue("Outdoors", out isOutdoorsValue);
				if (isOutdoorsValue != null)
				{
					this.isOutdoors = true;
				}
				if (this.isOutdoors)
				{
					this.largeTerrainFeatures = new List<LargeTerrainFeature>();
				}
				PropertyValue treatAsOutdoorsValue;
				this.map.Properties.TryGetValue("TreatAsOutdoors", out treatAsOutdoorsValue);
				if (treatAsOutdoorsValue != null)
				{
					this.treatAsOutdoors = true;
				}
				PropertyValue springObjects;
				this.map.Properties.TryGetValue(Game1.currentSeason.Substring(0, 1).ToUpper() + Game1.currentSeason.Substring(1) + "_Objects", out springObjects);
				if (springObjects != null && !Game1.eventUp)
				{
					this.spawnObjects();
				}
				bool hasPathsLayer = false;
				using (IEnumerator<Layer> enumerator = this.map.Layers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Id.Equals("Paths"))
						{
							hasPathsLayer = true;
							break;
						}
					}
				}
				PropertyValue trees;
				this.map.Properties.TryGetValue("Trees", out trees);
				if (trees != null)
				{
					string[] rawTreeString = trees.ToString().Split(new char[]
					{
						' '
					});
					for (int j = 0; j < rawTreeString.Length; j += 3)
					{
						int x = Convert.ToInt32(rawTreeString[j]);
						int y = Convert.ToInt32(rawTreeString[j + 1]);
						int treeType = Convert.ToInt32(rawTreeString[j + 2]) + 1;
						this.terrainFeatures.Add(new Vector2((float)x, (float)y), new Tree(treeType, 5));
					}
				}
				if ((this.isOutdoors || this.name.Equals("BathHouse_Entry") || this.treatAsOutdoors) & hasPathsLayer)
				{
					for (int x2 = 0; x2 < this.map.Layers[0].LayerWidth; x2++)
					{
						for (int y2 = 0; y2 < this.map.Layers[0].LayerHeight; y2++)
						{
							Tile t = this.map.GetLayer("Paths").Tiles[x2, y2];
							if (t != null)
							{
								Vector2 tile = new Vector2((float)x2, (float)y2);
								int treeType2 = -1;
								switch (t.TileIndex)
								{
								case 9:
									treeType2 = 1;
									if (Game1.currentSeason.Equals("winter"))
									{
										treeType2 += 3;
									}
									break;
								case 10:
									treeType2 = 2;
									if (Game1.currentSeason.Equals("winter"))
									{
										treeType2 += 3;
									}
									break;
								case 11:
									treeType2 = 3;
									break;
								case 12:
									treeType2 = 6;
									break;
								}
								if (treeType2 != -1)
								{
									if (!this.terrainFeatures.ContainsKey(tile) && !this.objects.ContainsKey(tile))
									{
										this.terrainFeatures.Add(tile, new Tree(treeType2, 5));
									}
								}
								else
								{
									switch (t.TileIndex)
									{
									case 13:
									case 14:
									case 15:
										if (!this.objects.ContainsKey(tile))
										{
											this.objects.Add(tile, new Object(tile, GameLocation.getWeedForSeason(Game1.random, Game1.currentSeason), 1));
										}
										break;
									case 16:
										if (!this.objects.ContainsKey(tile))
										{
											this.objects.Add(tile, new Object(tile, (Game1.random.NextDouble() < 0.5) ? 343 : 450, 1));
										}
										break;
									case 17:
										if (!this.objects.ContainsKey(tile))
										{
											this.objects.Add(tile, new Object(tile, (Game1.random.NextDouble() < 0.5) ? 343 : 450, 1));
										}
										break;
									case 18:
										if (!this.objects.ContainsKey(tile))
										{
											this.objects.Add(tile, new Object(tile, (Game1.random.NextDouble() < 0.5) ? 294 : 295, 1));
										}
										break;
									case 19:
										if (this is Farm)
										{
											(this as Farm).addResourceClumpAndRemoveUnderlyingTerrain(602, 2, 2, tile);
										}
										break;
									case 20:
										if (this is Farm)
										{
											(this as Farm).addResourceClumpAndRemoveUnderlyingTerrain(672, 2, 2, tile);
										}
										break;
									case 21:
										if (this is Farm)
										{
											(this as Farm).addResourceClumpAndRemoveUnderlyingTerrain(600, 2, 2, tile);
										}
										break;
									case 22:
										if (!this.terrainFeatures.ContainsKey(tile))
										{
											this.terrainFeatures.Add(tile, new Grass(1, 3));
										}
										break;
									case 23:
										if (!this.terrainFeatures.ContainsKey(tile))
										{
											this.terrainFeatures.Add(tile, new Tree(Game1.random.Next(1, 4), Game1.random.Next(2, 4)));
										}
										break;
									case 24:
										if (!this.terrainFeatures.ContainsKey(tile))
										{
											this.largeTerrainFeatures.Add(new Bush(tile, 2, this));
										}
										break;
									case 25:
										if (!this.terrainFeatures.ContainsKey(tile))
										{
											this.largeTerrainFeatures.Add(new Bush(tile, 1, this));
										}
										break;
									case 26:
										if (!this.terrainFeatures.ContainsKey(tile))
										{
											this.largeTerrainFeatures.Add(new Bush(tile, 0, this));
										}
										break;
									case 27:
										this.changeMapProperties("BrookSounds", string.Concat(new object[]
										{
											tile.X,
											" ",
											tile.Y,
											" 0"
										}));
										break;
									case 28:
									{
										string a = this.name;
										if (a == "BugLand")
										{
											this.characters.Add(new Grub(new Vector2(tile.X * (float)Game1.tileSize, tile.Y * (float)Game1.tileSize), true));
										}
										break;
									}
									}
								}
							}
							if (this.map.GetLayer("Buildings").Tiles[x2, y2] != null)
							{
								PropertyValue door = null;
								this.map.GetLayer("Buildings").Tiles[x2, y2].Properties.TryGetValue("Action", out door);
								if (door != null && door.ToString().Contains("Warp"))
								{
									string[] split = door.ToString().Split(new char[]
									{
										' '
									});
									if (split[0].Equals("WarpCommunityCenter"))
									{
										this.doors.Add(new Point(x2, y2), "CommunityCenter");
									}
									else if (!this.name.Equals("Mountain") || x2 != 8 || y2 != 20)
									{
										try
										{
											this.doors.Add(new Point(x2, y2), split[3]);
										}
										catch (Exception)
										{
										}
									}
								}
							}
						}
					}
				}
				this.loadLights();
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00069304 File Offset: 0x00067504
		public bool isTerrainFeatureAt(int x, int y)
		{
			Vector2 v = new Vector2((float)x, (float)y);
			if (this.terrainFeatures.ContainsKey(v) && !this.terrainFeatures[v].isPassable(null))
			{
				return true;
			}
			if (this.largeTerrainFeatures != null)
			{
				Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
				using (List<LargeTerrainFeature>.Enumerator enumerator = this.largeTerrainFeatures.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getBoundingBox().Intersects(tileRect))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000693C0 File Offset: 0x000675C0
		public void loadLights()
		{
			if ((!this.isOutdoors || Game1.isFestival()) && !(this is FarmHouse))
			{
				bool hasPathsLayer = false;
				using (IEnumerator<Layer> enumerator = this.map.Layers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Id.Equals("Paths"))
						{
							hasPathsLayer = true;
							break;
						}
					}
				}
				if (this.doorSprites == null)
				{
					this.doorSprites = new Dictionary<Point, TemporaryAnimatedSprite>();
				}
				else
				{
					this.doorSprites.Clear();
				}
				for (int x = 0; x < this.map.Layers[0].LayerWidth; x++)
				{
					for (int y = 0; y < this.map.Layers[0].LayerHeight; y++)
					{
						if (!this.isOutdoors)
						{
							Tile t = this.map.GetLayer("Front").Tiles[x, y];
							if (t != null)
							{
								this.adjustMapLightPropertiesForLamp(t.TileIndex, x, y, "Front");
							}
							t = this.map.GetLayer("Buildings").Tiles[x, y];
							if (t != null)
							{
								this.adjustMapLightPropertiesForLamp(t.TileIndex, x, y, "Buildings");
								PropertyValue door = null;
								this.map.GetLayer("Buildings").Tiles[x, y].Properties.TryGetValue("Action", out door);
								if (door != null && door.ToString().Contains("Door"))
								{
									int doorIndex = this.map.GetLayer("Buildings").Tiles[x, y].TileIndex;
									Microsoft.Xna.Framework.Rectangle sourceRect = default(Microsoft.Xna.Framework.Rectangle);
									bool flip = false;
									if (doorIndex <= 824)
									{
										if (doorIndex != 120)
										{
											if (doorIndex == 824)
											{
												sourceRect = new Microsoft.Xna.Framework.Rectangle(640, 144, 16, 48);
											}
										}
										else
										{
											sourceRect = new Microsoft.Xna.Framework.Rectangle(512, 144, 16, 48);
										}
									}
									else if (doorIndex != 825)
									{
										if (doorIndex == 838)
										{
											sourceRect = new Microsoft.Xna.Framework.Rectangle(576, 144, 16, 48);
											if (x == 10 && y == 5)
											{
												flip = true;
											}
										}
									}
									else
									{
										sourceRect = new Microsoft.Xna.Framework.Rectangle(640, 144, 16, 48);
										flip = true;
									}
									this.doorSprites.Add(new Point(x, y), new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 100f, 4, 1, new Vector2((float)x, (float)(y - 2)) * (float)Game1.tileSize, false, flip, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 3) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										holdLastFrame = true,
										paused = true,
										endSound = ((door.ToString().Split(new char[]
										{
											' '
										}).Length > 1) ? door.ToString().Substring(door.ToString().IndexOf(' ') + 1) : null)
									});
									if (door.ToString().Split(new char[]
									{
										' '
									}).Length > 1 && !this.map.GetLayer("Back").Tiles[x, y].Properties.ContainsKey("TouchAction"))
									{
										this.map.GetLayer("Back").Tiles[x, y].Properties.Add("TouchAction", new PropertyValue("Door " + door.ToString().Substring(door.ToString().IndexOf(' ') + 1)));
									}
								}
							}
						}
						if (hasPathsLayer)
						{
							Tile t = this.map.GetLayer("Paths").Tiles[x, y];
							if (t != null)
							{
								this.adjustMapLightPropertiesForLamp(t.TileIndex, x, y, "Paths");
							}
						}
					}
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000697EC File Offset: 0x000679EC
		public bool isFarmBuildingInterior()
		{
			return this is AnimalHouse;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000697FC File Offset: 0x000679FC
		protected void adjustMapLightPropertiesForLamp(int tile, int x, int y, string layer)
		{
			if (this.isFarmBuildingInterior())
			{
				if (tile == 24)
				{
					this.changeMapProperties("DayTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						tile
					}));
					this.changeMapProperties("NightTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						26
					}));
					this.changeMapProperties("Light", string.Concat(new object[]
					{
						x,
						" ",
						y + 1,
						" 4"
					}));
					this.changeMapProperties("Light", string.Concat(new object[]
					{
						x,
						" ",
						y + 3,
						" 4"
					}));
					return;
				}
				if (tile == 25)
				{
					this.changeMapProperties("DayTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						tile
					}));
					this.changeMapProperties("NightTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						12
					}));
					return;
				}
				if (tile != 46)
				{
					return;
				}
				this.changeMapProperties("DayTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y,
					" ",
					tile
				}));
				this.changeMapProperties("NightTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y,
					" ",
					53
				}));
				return;
			}
			else if (tile <= 256)
			{
				if (tile == 8)
				{
					this.changeMapProperties("Light", string.Concat(new object[]
					{
						x,
						" ",
						y,
						" 4"
					}));
					return;
				}
				if (tile == 225)
				{
					if (!this.name.Contains("BathHouse") && !this.name.Contains("Club"))
					{
						this.changeMapProperties("DayTiles", string.Concat(new object[]
						{
							layer,
							" ",
							x,
							" ",
							y,
							" ",
							tile
						}));
						this.changeMapProperties("NightTiles", string.Concat(new object[]
						{
							layer,
							" ",
							x,
							" ",
							y,
							" ",
							1222
						}));
						this.changeMapProperties("DayTiles", string.Concat(new object[]
						{
							layer,
							" ",
							x,
							" ",
							y + 1,
							" ",
							257
						}));
						this.changeMapProperties("NightTiles", string.Concat(new object[]
						{
							layer,
							" ",
							x,
							" ",
							y + 1,
							" ",
							1254
						}));
						this.changeMapProperties("Light", string.Concat(new object[]
						{
							x,
							" ",
							y,
							" 4"
						}));
						this.changeMapProperties("Light", string.Concat(new object[]
						{
							x,
							" ",
							y + 1,
							" 4"
						}));
					}
					return;
				}
				if (tile != 256)
				{
					return;
				}
				this.changeMapProperties("DayTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y,
					" ",
					tile
				}));
				this.changeMapProperties("NightTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y,
					" ",
					1253
				}));
				this.changeMapProperties("DayTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y + 1,
					" ",
					288
				}));
				this.changeMapProperties("NightTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y + 1,
					" ",
					1285
				}));
				this.changeMapProperties("Light", string.Concat(new object[]
				{
					x,
					" ",
					y,
					" 4"
				}));
				this.changeMapProperties("Light", string.Concat(new object[]
				{
					x,
					" ",
					y + 1,
					" 4"
				}));
				return;
			}
			else if (tile <= 826)
			{
				if (tile == 480)
				{
					this.changeMapProperties("DayTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						tile
					}));
					this.changeMapProperties("NightTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						809
					}));
					this.changeMapProperties("Light", string.Concat(new object[]
					{
						x,
						" ",
						y,
						" 4"
					}));
					return;
				}
				if (tile != 826)
				{
					return;
				}
				this.changeMapProperties("DayTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y,
					" ",
					tile
				}));
				this.changeMapProperties("NightTiles", string.Concat(new object[]
				{
					layer,
					" ",
					x,
					" ",
					y,
					" ",
					827
				}));
				this.changeMapProperties("Light", string.Concat(new object[]
				{
					x,
					" ",
					y,
					" 4"
				}));
				return;
			}
			else
			{
				if (tile == 1344)
				{
					this.changeMapProperties("DayTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						tile
					}));
					this.changeMapProperties("NightTiles", string.Concat(new object[]
					{
						layer,
						" ",
						x,
						" ",
						y,
						" ",
						1345
					}));
					this.changeMapProperties("Light", string.Concat(new object[]
					{
						x,
						" ",
						y,
						" 4"
					}));
					return;
				}
				if (tile != 1346)
				{
					return;
				}
				this.changeMapProperties("DayTiles", string.Concat(new object[]
				{
					"Front ",
					x,
					" ",
					y,
					" ",
					tile
				}));
				this.changeMapProperties("NightTiles", string.Concat(new object[]
				{
					"Front ",
					x,
					" ",
					y,
					" ",
					1347
				}));
				this.changeMapProperties("DayTiles", string.Concat(new object[]
				{
					"Buildings ",
					x,
					" ",
					y + 1,
					" ",
					452
				}));
				this.changeMapProperties("NightTiles", string.Concat(new object[]
				{
					"Buildings ",
					x,
					" ",
					y + 1,
					" ",
					453
				}));
				this.changeMapProperties("Light", string.Concat(new object[]
				{
					x,
					" ",
					y,
					" 4"
				}));
				return;
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0006A2A0 File Offset: 0x000684A0
		private void changeMapProperties(string propertyName, string toAdd)
		{
			try
			{
				bool addSpaceToFront = true;
				if (!this.map.Properties.ContainsKey(propertyName))
				{
					this.map.Properties.Add(propertyName, new PropertyValue(string.Empty));
					addSpaceToFront = false;
				}
				if (!this.map.Properties[propertyName].ToString().Contains(toAdd))
				{
					StringBuilder b = new StringBuilder(this.map.Properties[propertyName].ToString());
					if (addSpaceToFront)
					{
						b.Append(" ");
					}
					b.Append(toAdd);
					this.map.Properties[propertyName] = new PropertyValue(b.ToString());
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000506 RID: 1286
		public const int minDailyWeeds = 5;

		// Token: 0x04000507 RID: 1287
		public const int maxDailyWeeds = 12;

		// Token: 0x04000508 RID: 1288
		public const int minDailyObjectSpawn = 1;

		// Token: 0x04000509 RID: 1289
		public const int maxDailyObjectSpawn = 4;

		// Token: 0x0400050A RID: 1290
		public const int maxSpawnedObjectsAtOnce = 6;

		// Token: 0x0400050B RID: 1291
		public const int maxTriesForDebrisPlacement = 3;

		// Token: 0x0400050C RID: 1292
		public const int maxTriesForObjectSpawn = 6;

		// Token: 0x0400050D RID: 1293
		public const double chanceForStumpOrBoulderRespawn = 0.2;

		// Token: 0x0400050E RID: 1294
		public const double chanceForClay = 0.03;

		// Token: 0x0400050F RID: 1295
		public const int forageDataIndex = 0;

		// Token: 0x04000510 RID: 1296
		public const int fishDataIndex = 4;

		// Token: 0x04000511 RID: 1297
		public const int diggablesDataIndex = 8;

		// Token: 0x04000512 RID: 1298
		private GameLocation.afterQuestionBehavior afterQuestion;

		// Token: 0x04000513 RID: 1299
		[XmlIgnore]
		public Map map;

		// Token: 0x04000514 RID: 1300
		public List<NPC> characters = new List<NPC>();

		// Token: 0x04000515 RID: 1301
		public SerializableDictionary<Vector2, Object> objects = new SerializableDictionary<Vector2, Object>();

		// Token: 0x04000516 RID: 1302
		[XmlIgnore]
		public List<TemporaryAnimatedSprite> temporarySprites = new List<TemporaryAnimatedSprite>();

		// Token: 0x04000517 RID: 1303
		[XmlIgnore]
		public List<Warp> warps = new List<Warp>();

		// Token: 0x04000518 RID: 1304
		[XmlIgnore]
		public Dictionary<Point, string> doors = new Dictionary<Point, string>();

		// Token: 0x04000519 RID: 1305
		[XmlIgnore]
		public Dictionary<Point, TemporaryAnimatedSprite> doorSprites;

		// Token: 0x0400051A RID: 1306
		[XmlIgnore]
		public List<Farmer> farmers = new List<Farmer>();

		// Token: 0x0400051B RID: 1307
		[XmlIgnore]
		public List<Projectile> projectiles = new List<Projectile>();

		// Token: 0x0400051C RID: 1308
		public List<LargeTerrainFeature> largeTerrainFeatures;

		// Token: 0x0400051D RID: 1309
		protected List<Critter> critters;

		// Token: 0x0400051E RID: 1310
		public SerializableDictionary<Vector2, TerrainFeature> terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();

		// Token: 0x0400051F RID: 1311
		public List<Debris> debris = new List<Debris>();

		// Token: 0x04000520 RID: 1312
		[XmlIgnore]
		public Point fishSplashPoint = Point.Zero;

		// Token: 0x04000521 RID: 1313
		[XmlIgnore]
		public Point orePanPoint = Point.Zero;

		// Token: 0x04000522 RID: 1314
		[XmlIgnore]
		public TemporaryAnimatedSprite fishSplashAnimation;

		// Token: 0x04000523 RID: 1315
		[XmlIgnore]
		public TemporaryAnimatedSprite orePanAnimation;

		// Token: 0x04000524 RID: 1316
		[XmlIgnore]
		public bool[,] waterTiles;

		// Token: 0x04000525 RID: 1317
		[XmlIgnore]
		public string uniqueName;

		// Token: 0x04000526 RID: 1318
		public string name;

		// Token: 0x04000527 RID: 1319
		public Color waterColor = Color.White * 0.33f;

		// Token: 0x04000528 RID: 1320
		[XmlIgnore]
		public string lastQuestionKey;

		// Token: 0x04000529 RID: 1321
		[XmlIgnore]
		public Vector2 lastTouchActionLocation = Vector2.Zero;

		// Token: 0x0400052A RID: 1322
		protected float lightLevel;

		// Token: 0x0400052B RID: 1323
		public bool isFarm;

		// Token: 0x0400052C RID: 1324
		public bool isOutdoors;

		// Token: 0x0400052D RID: 1325
		public bool isStructure;

		// Token: 0x0400052E RID: 1326
		public bool ignoreDebrisWeather;

		// Token: 0x0400052F RID: 1327
		public bool ignoreOutdoorLighting;

		// Token: 0x04000530 RID: 1328
		public bool ignoreLights;

		// Token: 0x04000531 RID: 1329
		public bool treatAsOutdoors;

		// Token: 0x04000532 RID: 1330
		protected bool wasUpdated;

		// Token: 0x04000533 RID: 1331
		private List<Vector2> terrainFeaturesToRemoveList = new List<Vector2>();

		// Token: 0x04000534 RID: 1332
		public int numberOfSpawnedObjectsOnMap;

		// Token: 0x04000535 RID: 1333
		[XmlIgnore]
		public Event currentEvent;

		// Token: 0x04000536 RID: 1334
		[XmlIgnore]
		public Object actionObjectForQuestionDialogue;

		// Token: 0x04000537 RID: 1335
		[XmlIgnore]
		public int waterAnimationIndex;

		// Token: 0x04000538 RID: 1336
		[XmlIgnore]
		public int waterAnimationTimer;

		// Token: 0x04000539 RID: 1337
		[XmlIgnore]
		public bool waterTileFlip;

		// Token: 0x0400053A RID: 1338
		[XmlIgnore]
		public bool forceViewportPlayerFollow;

		// Token: 0x0400053B RID: 1339
		private float waterPosition;

		// Token: 0x0400053C RID: 1340
		private Vector2 snowPos;

		// Token: 0x0400053D RID: 1341
		[XmlIgnore]
		public List<Vector2> lightGlows = new List<Vector2>();

		// Token: 0x0400053E RID: 1342
		public static int fireIDBase = 944468;

		// Token: 0x0400053F RID: 1343
		public static int clicks = 0;

		// Token: 0x0200016E RID: 366
		// Token: 0x06001373 RID: 4979
		public delegate void afterQuestionBehavior(Farmer who, string whichAnswer);
	}
}
