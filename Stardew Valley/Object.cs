using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000047 RID: 71
	[XmlInclude(typeof(Fence)), XmlInclude(typeof(Torch)), XmlInclude(typeof(SpecialItem)), XmlInclude(typeof(Wallpaper)), XmlInclude(typeof(Boots)), XmlInclude(typeof(Hat)), XmlInclude(typeof(Ring)), XmlInclude(typeof(TV)), XmlInclude(typeof(CrabPot)), XmlInclude(typeof(Chest)), XmlInclude(typeof(Door)), XmlInclude(typeof(Cask)), XmlInclude(typeof(SwitchFloor)), XmlInclude(typeof(WickedStatue)), XmlInclude(typeof(ColoredObject))]
	public class Object : Item
	{
		// Token: 0x17000069 RID: 105
		[XmlIgnore]
		public int ParentSheetIndex
		{
			// Token: 0x06000596 RID: 1430 RVA: 0x00074DA7 File Offset: 0x00072FA7
			get
			{
				return this.parentSheetIndex;
			}
			// Token: 0x06000597 RID: 1431 RVA: 0x00074DAF File Offset: 0x00072FAF
			set
			{
				this.parentSheetIndex = value;
			}
		}

		// Token: 0x1700006A RID: 106
		[XmlIgnore]
		public Vector2 TileLocation
		{
			// Token: 0x06000598 RID: 1432 RVA: 0x00074DB8 File Offset: 0x00072FB8
			get
			{
				return this.tileLocation;
			}
			// Token: 0x06000599 RID: 1433 RVA: 0x00074DC0 File Offset: 0x00072FC0
			set
			{
				this.tileLocation = value;
			}
		}

		// Token: 0x1700006B RID: 107
		[XmlIgnore]
		public override string Name
		{
			// Token: 0x0600059A RID: 1434 RVA: 0x00074DC9 File Offset: 0x00072FC9
			get
			{
				return this.name + (this.isRecipe ? " Recipe" : "");
			}
			// Token: 0x0600059B RID: 1435 RVA: 0x00074DEA File Offset: 0x00072FEA
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700006C RID: 108
		[XmlIgnore]
		public string Type
		{
			// Token: 0x0600059C RID: 1436 RVA: 0x00074DF3 File Offset: 0x00072FF3
			get
			{
				return this.type;
			}
			// Token: 0x0600059D RID: 1437 RVA: 0x00074DFB File Offset: 0x00072FFB
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700006D RID: 109
		[XmlIgnore]
		public override int Stack
		{
			// Token: 0x0600059E RID: 1438 RVA: 0x00074E04 File Offset: 0x00073004
			get
			{
				return Math.Max(0, this.stack);
			}
			// Token: 0x0600059F RID: 1439 RVA: 0x00074E12 File Offset: 0x00073012
			set
			{
				this.stack = Math.Min(Math.Max(0, value), this.maximumStackSize());
			}
		}

		// Token: 0x1700006E RID: 110
		[XmlIgnore]
		public int Category
		{
			// Token: 0x060005A0 RID: 1440 RVA: 0x00074E2C File Offset: 0x0007302C
			get
			{
				return this.category;
			}
			// Token: 0x060005A1 RID: 1441 RVA: 0x00074E34 File Offset: 0x00073034
			set
			{
				this.category = value;
			}
		}

		// Token: 0x1700006F RID: 111
		[XmlIgnore]
		public bool CanBeSetDown
		{
			// Token: 0x060005A2 RID: 1442 RVA: 0x00074E3D File Offset: 0x0007303D
			get
			{
				return this.canBeSetDown;
			}
			// Token: 0x060005A3 RID: 1443 RVA: 0x00074E45 File Offset: 0x00073045
			set
			{
				this.canBeSetDown = value;
			}
		}

		// Token: 0x17000070 RID: 112
		[XmlIgnore]
		public bool CanBeGrabbed
		{
			// Token: 0x060005A4 RID: 1444 RVA: 0x00074E4E File Offset: 0x0007304E
			get
			{
				return this.canBeGrabbed;
			}
		}

		// Token: 0x17000071 RID: 113
		[XmlIgnore]
		public bool IsHoeDirt
		{
			// Token: 0x060005A5 RID: 1445 RVA: 0x00074E56 File Offset: 0x00073056
			get
			{
				return this.isHoedirt;
			}
		}

		// Token: 0x17000072 RID: 114
		[XmlIgnore]
		public bool IsSpawnedObject
		{
			// Token: 0x060005A6 RID: 1446 RVA: 0x00074E5E File Offset: 0x0007305E
			get
			{
				return this.isSpawnedObject;
			}
		}

		// Token: 0x17000073 RID: 115
		[XmlIgnore]
		public int Price
		{
			// Token: 0x060005A7 RID: 1447 RVA: 0x00074E66 File Offset: 0x00073066
			get
			{
				return this.price;
			}
			// Token: 0x060005A8 RID: 1448 RVA: 0x00074E6E File Offset: 0x0007306E
			set
			{
				this.price = value;
			}
		}

		// Token: 0x17000074 RID: 116
		[XmlIgnore]
		public int Edibility
		{
			// Token: 0x060005A9 RID: 1449 RVA: 0x00074E77 File Offset: 0x00073077
			get
			{
				return this.edibility;
			}
			// Token: 0x060005AA RID: 1450 RVA: 0x00074E7F File Offset: 0x0007307F
			set
			{
				this.edibility = value;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00074E88 File Offset: 0x00073088
		public Object()
		{
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00074EB8 File Offset: 0x000730B8
		public Object(Vector2 tileLocation, int parentSheetIndex, bool isRecipe = false)
		{
			this.isRecipe = isRecipe;
			this.tileLocation = tileLocation;
			this.parentSheetIndex = parentSheetIndex;
			this.canBeSetDown = true;
			this.bigCraftable = true;
			string objectInformation;
			Game1.bigCraftablesInformation.TryGetValue(parentSheetIndex, out objectInformation);
			if (objectInformation != null)
			{
				string[] objectInfoArray = objectInformation.Split(new char[]
				{
					'/'
				});
				this.name = objectInfoArray[0];
				this.price = Convert.ToInt32(objectInfoArray[1]);
				this.edibility = Convert.ToInt32(objectInfoArray[2]);
				string[] typeAndCategory = objectInfoArray[3].Split(new char[]
				{
					' '
				});
				this.type = typeAndCategory[0];
				if (typeAndCategory.Length > 1)
				{
					this.category = Convert.ToInt32(typeAndCategory[1]);
				}
				this.setOutdoors = Convert.ToBoolean(objectInfoArray[5]);
				this.setIndoors = Convert.ToBoolean(objectInfoArray[6]);
				this.fragility = Convert.ToInt32(objectInfoArray[7]);
				this.isLamp = (objectInfoArray.Length > 8 && objectInfoArray[8].Equals("true"));
			}
			this.scale = new Vector2(0f, 5f);
			this.initializeLightSource(this.tileLocation);
			this.boundingBox = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00075028 File Offset: 0x00073228
		public Object(int parentSheetIndex, int initialStack, bool isRecipe = false, int price = -1, int quality = 0) : this(Vector2.Zero, parentSheetIndex, initialStack)
		{
			this.isRecipe = isRecipe;
			if (price != -1)
			{
				this.price = price;
			}
			this.quality = quality;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00075053 File Offset: 0x00073253
		public Object(Vector2 tileLocation, int parentSheetIndex, int initialStack) : this(tileLocation, parentSheetIndex, null, true, true, false, false)
		{
			this.stack = initialStack;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0007506C File Offset: 0x0007326C
		public Object(Vector2 tileLocation, int parentSheetIndex, string name, bool canBeSetDown, bool canBeGrabbed, bool isHoedirt, bool isSpawnedObject)
		{
			this.tileLocation = tileLocation;
			this.parentSheetIndex = parentSheetIndex;
			string objectInformation;
			Game1.objectInformation.TryGetValue(parentSheetIndex, out objectInformation);
			try
			{
				if (objectInformation != null)
				{
					string[] objectInfoArray = objectInformation.Split(new char[]
					{
						'/'
					});
					this.name = objectInfoArray[0];
					this.price = Convert.ToInt32(objectInfoArray[1]);
					this.edibility = Convert.ToInt32(objectInfoArray[2]);
					string[] typeAndCategory = objectInfoArray[3].Split(new char[]
					{
						' '
					});
					this.type = typeAndCategory[0];
					if (typeAndCategory.Length > 1)
					{
						this.category = Convert.ToInt32(typeAndCategory[1]);
					}
				}
			}
			catch (Exception)
			{
			}
			if (this.name == null && name != null)
			{
				this.name = name;
			}
			else if (this.name == null)
			{
				this.name = "Error Item";
			}
			this.canBeSetDown = canBeSetDown;
			this.canBeGrabbed = canBeGrabbed;
			this.isHoedirt = isHoedirt;
			this.isSpawnedObject = isSpawnedObject;
			if (Game1.random.NextDouble() < 0.5 && parentSheetIndex > 52 && (parentSheetIndex < 8 || parentSheetIndex > 15) && (parentSheetIndex < 384 || parentSheetIndex > 391))
			{
				this.flipped = true;
			}
			if (this.name.Contains("Block"))
			{
				this.scale = new Vector2(1f, 1f);
			}
			if (parentSheetIndex == 449 || this.name.Contains("Weed") || this.name.Contains("Twig"))
			{
				this.fragility = 2;
			}
			else if (this.name.Contains("Fence"))
			{
				this.scale = new Vector2(10f, 0f);
				canBeSetDown = false;
			}
			else if (this.name.Contains("Stone"))
			{
				switch (parentSheetIndex)
				{
				case 8:
					this.minutesUntilReady = 4;
					goto IL_22B;
				case 10:
					this.minutesUntilReady = 8;
					goto IL_22B;
				case 12:
					this.minutesUntilReady = 16;
					goto IL_22B;
				case 14:
					this.minutesUntilReady = 12;
					goto IL_22B;
				}
				this.minutesUntilReady = 1;
			}
			IL_22B:
			if (parentSheetIndex >= 75 && parentSheetIndex <= 77)
			{
				isSpawnedObject = false;
			}
			this.initializeLightSource(this.tileLocation);
			if (this.category == -22)
			{
				this.scale.Y = 1f;
			}
			this.boundingBox = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00075318 File Offset: 0x00073518
		public Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
		{
			return new Vector2(this.tileLocation.X * (float)Game1.tileSize - (float)viewport.X, this.tileLocation.Y * (float)Game1.tileSize - (float)viewport.Y);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00075355 File Offset: 0x00073555
		public static Microsoft.Xna.Framework.Rectangle getSourceRectForBigCraftable(int index)
		{
			return new Microsoft.Xna.Framework.Rectangle(index % (Game1.bigCraftableSpriteSheet.Width / 16) * 16, index * 16 / Game1.bigCraftableSpriteSheet.Width * 16 * 2, 16, 32);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00075388 File Offset: 0x00073588
		public virtual bool performToolAction(Tool t)
		{
			if (t == null)
			{
				if (Game1.currentLocation.objects.ContainsKey(this.tileLocation) && Game1.currentLocation.objects[this.tileLocation].Equals(this))
				{
					Game1.createRadialDebris(Game1.currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
					Game1.currentLocation.objects.Remove(this.tileLocation);
				}
				return false;
			}
			GameLocation location = t.getLastFarmerToUse().currentLocation;
			if (this.name.Equals("Stone") && t.GetType() == typeof(Pickaxe))
			{
				int damage = 1;
				switch (t.upgradeLevel)
				{
				case 1:
					damage = 2;
					break;
				case 2:
					damage = 3;
					break;
				case 3:
					damage = 4;
					break;
				case 4:
					damage = 5;
					break;
				}
				if ((this.parentSheetIndex == 12 && t.upgradeLevel == 1) || ((this.parentSheetIndex == 12 || this.parentSheetIndex == 14) && t.upgradeLevel == 0))
				{
					damage = 0;
					Game1.playSound("crafting");
				}
				this.minutesUntilReady -= damage;
				if (this.minutesUntilReady <= 0)
				{
					return true;
				}
				Game1.playSound("hammer");
				this.shakeTimer = 100;
				return false;
			}
			else
			{
				if (this.name.Equals("Stone") && t.GetType() == typeof(Pickaxe))
				{
					return false;
				}
				if (this.name.Equals("Boulder") && (t.upgradeLevel != 4 || !(t is Pickaxe)))
				{
					if (t.isHeavyHitter())
					{
						Game1.playSound("hammer");
					}
					return false;
				}
				if (this.name.Contains("Weeds") && t.isHeavyHitter())
				{
					this.cutWeed(t.getLastFarmerToUse());
					return true;
				}
				if (this.name.Contains("Twig") && t is Axe)
				{
					this.fragility = 2;
					Game1.playSound("axchop");
					t.getLastFarmerToUse().currentLocation.debris.Add(new Debris(new Object(388, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
					Game1.createRadialDebris(location, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
					location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
					return true;
				}
				if (this.parentSheetIndex == 590)
				{
					if (t is Hoe)
					{
						location.digUpArtifactSpot((int)this.tileLocation.X, (int)this.tileLocation.Y, t.getLastFarmerToUse());
						if (!location.terrainFeatures.ContainsKey(this.tileLocation))
						{
							location.terrainFeatures.Add(this.tileLocation, new HoeDirt());
						}
						Game1.playSound("hoeHit");
						if (location.objects.ContainsKey(this.tileLocation))
						{
							location.objects.Remove(this.tileLocation);
						}
					}
					return false;
				}
				if (this.fragility == 2)
				{
					return false;
				}
				if (this.type == null || !this.type.Equals("Crafting") || !(t.GetType() != typeof(MeleeWeapon)) || !t.isHeavyHitter())
				{
					return false;
				}
				Game1.playSound("hammer");
				if (this.fragility == 1)
				{
					Game1.createRadialDebris(location, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
					if (location.objects.ContainsKey(this.tileLocation))
					{
						location.objects.Remove(this.tileLocation);
					}
					return false;
				}
				if (this.name.Equals("Tapper") && t.getLastFarmerToUse().currentLocation.terrainFeatures.ContainsKey(this.tileLocation) && t.getLastFarmerToUse().currentLocation.terrainFeatures[this.tileLocation] is Tree)
				{
					(t.getLastFarmerToUse().currentLocation.terrainFeatures[this.tileLocation] as Tree).tapped = false;
				}
				if (this.heldObject != null && this.readyForHarvest)
				{
					t.getLastFarmerToUse().currentLocation.debris.Add(new Debris(this.heldObject, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
				}
				if (this.parentSheetIndex == 157)
				{
					this.parentSheetIndex = 156;
					this.heldObject = null;
					this.minutesUntilReady = -1;
				}
				return true;
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000758E4 File Offset: 0x00073AE4
		private void cutWeed(Farmer who)
		{
			Color c = Color.Green;
			string sound = "cut";
			int animation = 50;
			this.fragility = 2;
			int toDrop = -1;
			if (Game1.random.NextDouble() < 0.5)
			{
				toDrop = 771;
			}
			else if (Game1.random.NextDouble() < 0.05)
			{
				toDrop = 770;
			}
			int num = this.parentSheetIndex;
			if (num <= 678)
			{
				switch (num)
				{
				case 313:
				case 314:
				case 315:
					c = new Color(84, 101, 27);
					break;
				case 316:
				case 317:
				case 318:
					c = new Color(109, 49, 196);
					break;
				case 319:
					c = new Color(30, 216, 255);
					sound = "breakingGlass";
					animation = 47;
					Game1.playSound("drumkit2");
					toDrop = -1;
					break;
				case 320:
					c = new Color(175, 143, 255);
					sound = "breakingGlass";
					animation = 47;
					Game1.playSound("drumkit2");
					toDrop = -1;
					break;
				case 321:
					c = new Color(73, 255, 158);
					sound = "breakingGlass";
					animation = 47;
					Game1.playSound("drumkit2");
					toDrop = -1;
					break;
				default:
					if (num == 678)
					{
						c = new Color(228, 109, 159);
					}
					break;
				}
			}
			else if (num != 679)
			{
				switch (num)
				{
				case 792:
				case 793:
				case 794:
					toDrop = 770;
					break;
				}
			}
			else
			{
				c = new Color(253, 191, 46);
			}
			if (sound.Equals("breakingGlass") && Game1.random.NextDouble() < 0.0025)
			{
				toDrop = 338;
			}
			Game1.playSound(sound);
			who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(animation, this.tileLocation * (float)Game1.tileSize, c, 8, false, 100f, 0, -1, -1f, -1, 0));
			who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(animation, this.tileLocation * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4)), c * 0.75f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				scale = 0.75f,
				flipped = true
			});
			who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(animation, this.tileLocation * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4)), c * 0.75f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				scale = 0.75f,
				delayBeforeAnimationStart = 50
			});
			who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(animation, this.tileLocation * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float)Game1.random.Next(-Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4)), c * 0.75f, 8, false, 100f, 0, -1, -1f, -1, 0)
			{
				scale = 0.75f,
				flipped = true,
				delayBeforeAnimationStart = 100
			});
			if (toDrop != -1)
			{
				who.currentLocation.debris.Add(new Debris(new Object(toDrop, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
			}
			if (Game1.random.NextDouble() < 0.02)
			{
				who.currentLocation.addJumperFrog(this.tileLocation);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00075D3B File Offset: 0x00073F3B
		public bool isAnimalProduct()
		{
			return this.Category == -18 || this.category == -5 || this.category == -6 || this.parentSheetIndex == 430;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00075D6C File Offset: 0x00073F6C
		public virtual bool onExplosion(Farmer who, GameLocation location)
		{
			if (who == null)
			{
				return false;
			}
			if (this.name.Contains("Weed"))
			{
				this.fragility = 0;
				this.cutWeed(who);
				location.removeObject(this.tileLocation, false);
			}
			if (this.name.Contains("Twig"))
			{
				this.fragility = 0;
				Game1.createRadialDebris(location, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
				location.debris.Add(new Debris(new Object(388, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
			}
			if (this.name.Contains("Stone"))
			{
				this.fragility = 0;
				Game1.createRadialDebris(location, 14, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 10), false, -1, false, -1);
				if (Game1.random.NextDouble() < 0.5)
				{
					location.debris.Add(new Debris(new Object(390, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
				}
			}
			this.performRemoveAction(this.tileLocation, who.currentLocation);
			return true;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00075F00 File Offset: 0x00074100
		public bool canBeShipped()
		{
			return !this.bigCraftable && this.type != null && !this.type.Equals("Quest") && this.canBeTrashed();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00075F2C File Offset: 0x0007412C
		public virtual void DayUpdate(GameLocation location)
		{
			this.health = 10;
			if ((this.parentSheetIndex == 599 || this.parentSheetIndex == 621 || this.parentSheetIndex == 645) && (!Game1.isRaining || !location.isOutdoors))
			{
				int k = this.parentSheetIndex;
				if (k != 599)
				{
					if (k != 621)
					{
						if (k == 645)
						{
							int i = (int)this.tileLocation.X - 2;
							while ((float)i <= this.tileLocation.X + 2f)
							{
								int y = (int)this.tileLocation.Y - 2;
								while ((float)y <= this.tileLocation.Y + 2f)
								{
									Vector2 v = new Vector2((float)i, (float)y);
									if (location.terrainFeatures.ContainsKey(v) && location.terrainFeatures[v] is HoeDirt)
									{
										(location.terrainFeatures[v] as HoeDirt).state = 1;
									}
									y++;
								}
								i++;
							}
							location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 2176, Game1.tileSize * 5, Game1.tileSize * 5), 60f, 4, 100, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize * 3 + Game1.tileSize), (float)(-(float)Game1.tileSize * 2)), false, false)
							{
								color = Color.White * 0.4f,
								delayBeforeAnimationStart = Game1.random.Next(1000),
								id = this.tileLocation.X * 4000f + this.tileLocation.Y
							});
						}
					}
					else
					{
						Vector2[] surroundingTileLocationsArray = Utility.getSurroundingTileLocationsArray(this.tileLocation);
						for (k = 0; k < surroundingTileLocationsArray.Length; k++)
						{
							Vector2 v2 = surroundingTileLocationsArray[k];
							if (location.terrainFeatures.ContainsKey(v2) && location.terrainFeatures[v2] is HoeDirt)
							{
								(location.terrainFeatures[v2] as HoeDirt).state = 1;
							}
						}
						location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 1984, Game1.tileSize * 3, Game1.tileSize * 3), 60f, 3, 100, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize), (float)(-(float)Game1.tileSize)), false, false)
						{
							color = Color.White * 0.4f,
							delayBeforeAnimationStart = Game1.random.Next(1000),
							id = this.tileLocation.X * 4000f + this.tileLocation.Y
						});
					}
				}
				else
				{
					foreach (Vector2 v3 in Utility.getAdjacentTileLocations(this.tileLocation))
					{
						if (location.terrainFeatures.ContainsKey(v3) && location.terrainFeatures[v3] is HoeDirt)
						{
							(location.terrainFeatures[v3] as HoeDirt).state = 1;
						}
					}
					int delay = Game1.random.Next(1000);
					location.temporarySprites.Add(new TemporaryAnimatedSprite(29, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 4)), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0)
					{
						delayBeforeAnimationStart = delay,
						id = this.tileLocation.X * 4000f + this.tileLocation.Y
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(29, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 3 / 4), 0f), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0)
					{
						rotation = 1.57079637f,
						delayBeforeAnimationStart = delay,
						id = this.tileLocation.X * 4000f + this.tileLocation.Y
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(29, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize * 3 / 4)), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0)
					{
						rotation = 3.14159274f,
						delayBeforeAnimationStart = delay,
						id = this.tileLocation.X * 4000f + this.tileLocation.Y
					});
					location.temporarySprites.Add(new TemporaryAnimatedSprite(29, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize * 3 / 4), 0f), Color.White * 0.5f, 4, false, 60f, 100, -1, -1f, -1, 0)
					{
						rotation = 4.712389f,
						delayBeforeAnimationStart = delay,
						id = this.tileLocation.X * 4000f + this.tileLocation.Y
					});
				}
			}
			if (this.bigCraftable)
			{
				int k = this.parentSheetIndex;
				if (k <= 109)
				{
					if (k <= 104)
					{
						if (k != 10)
						{
							if (k == 104)
							{
								if (Game1.currentSeason.Equals("winter"))
								{
									this.minutesUntilReady = 9999;
								}
								else
								{
									this.minutesUntilReady = -1;
								}
							}
						}
						else if (Game1.currentSeason.Equals("winter"))
						{
							this.heldObject = null;
							this.readyForHarvest = false;
							this.showNextIndex = false;
							this.minutesUntilReady = -1;
						}
						else if (this.heldObject == null)
						{
							this.heldObject = new Object(Vector2.Zero, 340, null, false, true, false, false);
							this.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
						}
					}
					else if (k == 108 || k == 109)
					{
						this.parentSheetIndex = 108;
						if (Game1.currentSeason.Equals("winter") || Game1.currentSeason.Equals("fall"))
						{
							this.parentSheetIndex = 109;
						}
					}
				}
				else if (k <= 127)
				{
					if (k != 117)
					{
						if (k == 127)
						{
							NPC j = Utility.getTodaysBirthdayNPC(Game1.currentSeason, Game1.dayOfMonth);
							this.minutesUntilReady = 1;
							if (j != null)
							{
								this.heldObject = j.getFavoriteItem();
							}
							else
							{
								int index = 80;
								switch (Game1.random.Next(4))
								{
								case 0:
									index = 72;
									break;
								case 1:
									index = 337;
									break;
								case 2:
									index = 749;
									break;
								case 3:
									index = 336;
									break;
								}
								this.heldObject = new Object(index, 1, false, -1, 0);
							}
						}
					}
					else
					{
						this.heldObject = new Object(167, 1, false, -1, 0);
					}
				}
				else if (k != 128)
				{
					if (k != 157)
					{
						if (k == 160)
						{
							this.minutesUntilReady = 1;
							this.heldObject = new Object(386, Game1.random.Next(2, 9), false, -1, 0);
						}
					}
					else if (this.minutesUntilReady <= 0 && this.heldObject != null && location.canSlimeHatchHere())
					{
						GreenSlime slime = null;
						Vector2 v4 = new Vector2((float)((int)this.tileLocation.X), (float)((int)this.tileLocation.Y + 1)) * (float)Game1.tileSize;
						k = this.heldObject.parentSheetIndex;
						if (k <= 437)
						{
							if (k != 413)
							{
								if (k == 437)
								{
									slime = new GreenSlime(v4, 80);
								}
							}
							else
							{
								slime = new GreenSlime(v4, 40);
							}
						}
						else if (k != 439)
						{
							if (k == 680)
							{
								slime = new GreenSlime(v4, 0);
							}
						}
						else
						{
							slime = new GreenSlime(v4, 121);
						}
						if (slime != null)
						{
							Game1.showGlobalMessage("A " + (slime.cute ? "male" : "female") + " slime has hatched");
							slime.setTilePosition((int)this.tileLocation.X, (int)this.tileLocation.Y + 1);
							location.characters.Add(slime);
							this.heldObject = null;
							this.parentSheetIndex = 156;
							this.minutesUntilReady = -1;
						}
					}
				}
				else if (this.heldObject == null)
				{
					int whichMushroom;
					if (Game1.random.NextDouble() < 0.025)
					{
						whichMushroom = 422;
					}
					else if (Game1.random.NextDouble() < 0.075)
					{
						whichMushroom = 281;
					}
					else if (Game1.random.NextDouble() < 0.09)
					{
						whichMushroom = 257;
					}
					else if (Game1.random.NextDouble() < 0.15)
					{
						whichMushroom = 420;
					}
					else
					{
						whichMushroom = 404;
					}
					this.heldObject = new Object(whichMushroom, 1, false, -1, 0);
					this.minutesUntilReady = 3000 - Game1.timeOfDay;
				}
			}
			if (!this.bigCraftable)
			{
				int k = this.parentSheetIndex;
				switch (k)
				{
				case 674:
				case 675:
					if (Game1.dayOfMonth == 1 && Game1.currentSeason.Equals("summer") && location.isOutdoors)
					{
						this.parentSheetIndex += 2;
						return;
					}
					break;
				case 676:
				case 677:
					if (Game1.dayOfMonth == 1 && Game1.currentSeason.Equals("fall") && location.isOutdoors)
					{
						this.parentSheetIndex += 2;
					}
					break;
				default:
					if (k != 746)
					{
						switch (k)
						{
						case 784:
						case 785:
						case 786:
							if (Game1.dayOfMonth == 1 && !Game1.currentSeason.Equals("spring") && location.isOutdoors)
							{
								this.parentSheetIndex++;
								return;
							}
							break;
						default:
							return;
						}
					}
					else if (Game1.currentSeason.Equals("winter"))
					{
						this.rot();
						return;
					}
					break;
				}
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000769F8 File Offset: 0x00074BF8
		public void rot()
		{
			Random r = new Random(Game1.year * 999 + Game1.dayOfMonth + Utility.getSeasonNumber(Game1.currentSeason));
			this.ParentSheetIndex = r.Next(747, 749);
			this.price = 0;
			this.quality = 0;
			this.name = "Rotten Plant";
			this.lightSource = null;
			this.bigCraftable = false;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00076A64 File Offset: 0x00074C64
		public override void actionWhenBeingHeld(Farmer who)
		{
			if (this.lightSource != null && (!this.bigCraftable || this.isLamp))
			{
				if (!Utility.alreadyHasLightSourceWithThisID((int)who.uniqueMultiplayerID))
				{
					Game1.currentLightSources.Add(new LightSource(this.lightSource.lightTexture, this.lightSource.position, this.lightSource.radius, this.lightSource.color, (int)who.uniqueMultiplayerID));
				}
				Utility.repositionLightSource((int)who.uniqueMultiplayerID, who.position + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize)));
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00076B08 File Offset: 0x00074D08
		public override void actionWhenStopBeingHeld(Farmer who)
		{
			if (this.lightSource != null)
			{
				Utility.removeLightSource((int)who.uniqueMultiplayerID);
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00076B20 File Offset: 0x00074D20
		public virtual bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if (this.heldObject != null && !this.name.Equals("Recycling Machine") && !this.name.Equals("Crystalarium"))
			{
				return false;
			}
			if (dropIn != null && dropIn.bigCraftable)
			{
				return false;
			}
			if (this.name.Equals("Incubator"))
			{
				if (this.heldObject == null && (dropIn.Category == -5 || dropIn.parentSheetIndex == 107))
				{
					this.heldObject = new Object(dropIn.parentSheetIndex, 1, false, -1, 0);
					if (!probe)
					{
						Game1.playSound("coin");
						this.minutesUntilReady = 9000 * ((dropIn.parentSheetIndex == 107) ? 2 : 1);
						if (who.professions.Contains(2))
						{
							this.minutesUntilReady /= 2;
						}
						if (dropIn.ParentSheetIndex == 180 || dropIn.ParentSheetIndex == 182 || dropIn.ParentSheetIndex == 305)
						{
							this.parentSheetIndex += 2;
						}
						else
						{
							this.parentSheetIndex++;
						}
					}
					return true;
				}
			}
			else if (this.name.Equals("Slime Incubator"))
			{
				if (this.heldObject == null && dropIn.name.Contains("Slime Egg"))
				{
					this.heldObject = new Object(dropIn.parentSheetIndex, 1, false, -1, 0);
					if (!probe)
					{
						Game1.playSound("coin");
						this.minutesUntilReady = 4000;
						if (who.professions.Contains(2))
						{
							this.minutesUntilReady /= 2;
						}
						this.parentSheetIndex++;
					}
					return true;
				}
			}
			else if (this.name.Equals("Keg"))
			{
				int category = dropIn.parentSheetIndex;
				if (category <= 304)
				{
					if (category == 262)
					{
						this.heldObject = new Object(Vector2.Zero, 346, "Beer", false, true, false, false);
						if (!probe)
						{
							this.heldObject.name = "Beer";
							Game1.playSound("Ship");
							Game1.playSound("bubbles");
							this.minutesUntilReady = 1750;
							who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
							{
								alphaFade = 0.005f
							});
						}
						return true;
					}
					if (category == 304)
					{
						this.heldObject = new Object(Vector2.Zero, 303, "Pale Ale", false, true, false, false);
						if (!probe)
						{
							this.heldObject.name = "Pale Ale";
							Game1.playSound("Ship");
							Game1.playSound("bubbles");
							this.minutesUntilReady = 2250;
							who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
							{
								alphaFade = 0.005f
							});
						}
						return true;
					}
				}
				else
				{
					if (category == 340)
					{
						this.heldObject = new Object(Vector2.Zero, 459, "Mead", false, true, false, false);
						if (!probe)
						{
							this.heldObject.name = "Mead";
							Game1.playSound("Ship");
							Game1.playSound("bubbles");
							this.minutesUntilReady = 600;
							who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
							{
								alphaFade = 0.005f
							});
						}
						return true;
					}
					if (category == 433)
					{
						if (dropIn.Stack < 5 && !probe)
						{
							Game1.showRedMessage("Requires 5 beans.");
							return false;
						}
						this.heldObject = new Object(Vector2.Zero, 395, "Coffee", false, true, false, false);
						if (!probe)
						{
							this.heldObject.name = "Coffee";
							Game1.playSound("Ship");
							Game1.playSound("bubbles");
							dropIn.Stack -= 4;
							if (dropIn.Stack <= 0)
							{
								who.removeItemFromInventory(dropIn);
							}
							this.minutesUntilReady = 120;
							who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.DarkGray * 0.75f, 1f, 0f, 0f, 0f, false)
							{
								alphaFade = 0.005f
							});
						}
						return true;
					}
				}
				category = dropIn.Category;
				if (category == -79)
				{
					this.heldObject = new Object(Vector2.Zero, 348, dropIn.Name + " Wine", false, true, false, false);
					this.heldObject.Price = dropIn.Price * 3;
					if (!probe)
					{
						this.heldObject.name = dropIn.Name + " Wine";
						Game1.playSound("Ship");
						Game1.playSound("bubbles");
						this.minutesUntilReady = 10000;
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Lavender * 0.75f, 1f, 0f, 0f, 0f, false)
						{
							alphaFade = 0.005f
						});
					}
					return true;
				}
				if (category == -75)
				{
					this.heldObject = new Object(Vector2.Zero, 350, dropIn.Name + " Juice", false, true, false, false);
					this.heldObject.Price = (int)((double)dropIn.Price * 2.25);
					if (!probe)
					{
						this.heldObject.name = dropIn.Name + " Juice";
						Game1.playSound("bubbles");
						Game1.playSound("Ship");
						this.minutesUntilReady = 6000;
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.White * 0.75f, 1f, 0f, 0f, 0f, false)
						{
							alphaFade = 0.005f
						});
					}
					return true;
				}
			}
			else if (this.name.Equals("Preserves Jar"))
			{
				int category = dropIn.Category;
				if (category == -79)
				{
					this.heldObject = new Object(Vector2.Zero, 344, dropIn.Name + " Jelly", false, true, false, false);
					this.heldObject.Price = 50 + dropIn.Price * 2;
					if (!probe)
					{
						this.minutesUntilReady = 4000;
						this.heldObject.name = dropIn.Name + " Jelly";
						Game1.playSound("Ship");
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.LightBlue * 0.75f, 1f, 0f, 0f, 0f, false)
						{
							alphaFade = 0.005f
						});
					}
					return true;
				}
				if (category == -75)
				{
					this.heldObject = new Object(Vector2.Zero, 342, "Pickled " + dropIn.Name, false, true, false, false);
					this.heldObject.Price = 50 + dropIn.Price * 2;
					if (!probe)
					{
						this.heldObject.name = "Pickled " + dropIn.Name;
						Game1.playSound("Ship");
						this.minutesUntilReady = 4000;
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.White * 0.75f, 1f, 0f, 0f, 0f, false)
						{
							alphaFade = 0.005f
						});
					}
					return true;
				}
			}
			else if (this.name.Equals("Cheese Press"))
			{
				int category = dropIn.ParentSheetIndex;
				if (category <= 186)
				{
					if (category == 184)
					{
						this.heldObject = new Object(Vector2.Zero, 424, null, false, true, false, false);
						if (!probe)
						{
							this.minutesUntilReady = 200;
							Game1.playSound("Ship");
						}
						return true;
					}
					if (category == 186)
					{
						this.heldObject = new Object(Vector2.Zero, 424, "Cheese (=)", false, true, false, false)
						{
							quality = 2
						};
						if (!probe)
						{
							this.minutesUntilReady = 200;
							Game1.playSound("Ship");
						}
						return true;
					}
				}
				else
				{
					if (category == 436)
					{
						this.heldObject = new Object(Vector2.Zero, 426, null, false, true, false, false);
						if (!probe)
						{
							this.minutesUntilReady = 200;
							Game1.playSound("Ship");
						}
						return true;
					}
					if (category == 438)
					{
						this.heldObject = new Object(Vector2.Zero, 426, null, false, true, false, false)
						{
							quality = 2
						};
						if (!probe)
						{
							this.minutesUntilReady = 200;
							Game1.playSound("Ship");
						}
						return true;
					}
				}
			}
			else
			{
				if (this.name.Equals("Mayonnaise Machine"))
				{
					int category = dropIn.ParentSheetIndex;
					if (category <= 176)
					{
						if (category != 107 && category != 174)
						{
							if (category != 176)
							{
								goto IL_1E67;
							}
							goto IL_D88;
						}
					}
					else if (category <= 182)
					{
						if (category == 180)
						{
							goto IL_D88;
						}
						if (category != 182)
						{
							goto IL_1E67;
						}
					}
					else
					{
						if (category == 305)
						{
							this.heldObject = new Object(Vector2.Zero, 308, null, false, true, false, false);
							if (!probe)
							{
								this.minutesUntilReady = 180;
								Game1.playSound("Ship");
							}
							return true;
						}
						if (category != 442)
						{
							goto IL_1E67;
						}
						this.heldObject = new Object(Vector2.Zero, 307, null, false, true, false, false);
						if (!probe)
						{
							this.minutesUntilReady = 180;
							Game1.playSound("Ship");
						}
						return true;
					}
					this.heldObject = new Object(Vector2.Zero, 306, null, false, true, false, false)
					{
						quality = 2
					};
					if (!probe)
					{
						this.minutesUntilReady = 180;
						Game1.playSound("Ship");
					}
					return true;
					IL_D88:
					this.heldObject = new Object(Vector2.Zero, 306, null, false, true, false, false);
					if (!probe)
					{
						this.minutesUntilReady = 180;
						Game1.playSound("Ship");
					}
					return true;
				}
				if (this.name.Equals("Loom"))
				{
					int category = dropIn.ParentSheetIndex;
					if (category == 440)
					{
						this.heldObject = new Object(Vector2.Zero, 428, null, false, true, false, false);
						if (!probe)
						{
							this.minutesUntilReady = 240;
							Game1.playSound("Ship");
						}
						return true;
					}
				}
				else if (this.name.Equals("Oil Maker"))
				{
					int category = dropIn.ParentSheetIndex;
					if (category <= 421)
					{
						if (category == 270)
						{
							this.heldObject = new Object(Vector2.Zero, 247, null, false, true, false, false);
							if (!probe)
							{
								this.minutesUntilReady = 1000;
								Game1.playSound("bubbles");
								Game1.playSound("sipTea");
								who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
								{
									alphaFade = 0.005f
								});
							}
							return true;
						}
						if (category == 421)
						{
							this.heldObject = new Object(Vector2.Zero, 247, null, false, true, false, false);
							if (!probe)
							{
								this.minutesUntilReady = 60;
								Game1.playSound("bubbles");
								Game1.playSound("sipTea");
								who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
								{
									alphaFade = 0.005f
								});
							}
							return true;
						}
					}
					else
					{
						if (category == 430)
						{
							this.heldObject = new Object(Vector2.Zero, 432, null, false, true, false, false);
							if (!probe)
							{
								this.minutesUntilReady = 360;
								Game1.playSound("bubbles");
								Game1.playSound("sipTea");
								who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
								{
									alphaFade = 0.005f
								});
							}
							return true;
						}
						if (category == 431)
						{
							this.heldObject = new Object(247, 1, false, -1, 0);
							if (!probe)
							{
								this.minutesUntilReady = 3200;
								Game1.playSound("bubbles");
								Game1.playSound("sipTea");
								who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
								{
									alphaFade = 0.005f
								});
							}
							return true;
						}
					}
				}
				else if (this.name.Equals("Seed Maker"))
				{
					if (dropIn != null && dropIn.parentSheetIndex == 433)
					{
						return false;
					}
					if (Game1.temporaryContent == null)
					{
						Game1.temporaryContent = Game1.content.CreateTemporary();
					}
					Dictionary<int, string> arg_1316_0 = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Crops");
					bool found = false;
					int seed = -1;
					foreach (KeyValuePair<int, string> v in arg_1316_0)
					{
						if (Convert.ToInt32(v.Value.Split(new char[]
						{
							'/'
						})[3]) == dropIn.ParentSheetIndex)
						{
							found = true;
							seed = v.Key;
							break;
						}
					}
					if (found)
					{
						Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)((int)this.tileLocation.X) + (uint)((int)this.tileLocation.Y * 77) + (uint)Game1.timeOfDay));
						this.heldObject = new Object(seed, r.Next(1, 4), false, -1, 0);
						if (!probe)
						{
							if (r.NextDouble() < 0.005)
							{
								this.heldObject = new Object(499, 1, false, -1, 0);
							}
							else if (r.NextDouble() < 0.02)
							{
								this.heldObject = new Object(770, r.Next(1, 5), false, -1, 0);
							}
							this.minutesUntilReady = 20;
							Game1.playSound("Ship");
							DelayedAction.playSoundAfterDelay("dirtyHit", 250);
						}
						return true;
					}
				}
				else if (this.name.Equals("Crystalarium"))
				{
					if ((dropIn.Category == -2 || dropIn.Category == -12) && dropIn.ParentSheetIndex != 74 && (this.heldObject == null || this.heldObject.ParentSheetIndex != dropIn.ParentSheetIndex))
					{
						this.heldObject = (Object)dropIn.getOne();
						if (!probe)
						{
							Game1.playSound("select");
							this.minutesUntilReady = this.getMinutesForCrystalarium(dropIn.ParentSheetIndex);
						}
						return true;
					}
				}
				else if (this.name.Equals("Recycling Machine"))
				{
					if (dropIn.ParentSheetIndex >= 168 && dropIn.ParentSheetIndex <= 172 && this.heldObject == null)
					{
						Random r2 = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + Game1.timeOfDay + (int)this.tileLocation.X * 200 + (int)this.tileLocation.Y);
						switch (dropIn.ParentSheetIndex)
						{
						case 168:
							this.heldObject = new Object((r2.NextDouble() < 0.3) ? 382 : ((r2.NextDouble() < 0.3) ? 380 : 390), r2.Next(1, 4), false, -1, 0);
							break;
						case 169:
							this.heldObject = new Object((r2.NextDouble() < 0.25) ? 382 : 388, r2.Next(1, 4), false, -1, 0);
							break;
						case 170:
							this.heldObject = new Object(338, 1, false, -1, 0);
							break;
						case 171:
							this.heldObject = new Object(338, 1, false, -1, 0);
							break;
						case 172:
							this.heldObject = ((r2.NextDouble() < 0.1) ? new Object(428, 1, false, -1, 0) : new Torch(Vector2.Zero, 3));
							break;
						}
						if (!probe)
						{
							Game1.playSound("trashcan");
							this.minutesUntilReady = 60;
							Stats expr_167E = Game1.stats;
							uint piecesOfTrashRecycled = expr_167E.PiecesOfTrashRecycled;
							expr_167E.PiecesOfTrashRecycled = piecesOfTrashRecycled + 1u;
						}
						return true;
					}
				}
				else if (this.name.Equals("Furnace"))
				{
					if (who.IsMainPlayer && who.getTallyOfObject(382, false) <= 0)
					{
						if (!probe && who.IsMainPlayer)
						{
							Game1.showRedMessage("Requires 1 Coal");
						}
						return false;
					}
					if (this.heldObject == null && !probe)
					{
						if (dropIn.stack < 5 && dropIn.parentSheetIndex != 80 && dropIn.parentSheetIndex != 330)
						{
							Game1.showRedMessage("Requires 5 ores.");
							return false;
						}
						int toRemove = 5;
						int category = dropIn.ParentSheetIndex;
						if (category <= 378)
						{
							if (category == 80)
							{
								this.heldObject = new Object(Vector2.Zero, 338, "Refined Quartz", false, true, false, false);
								this.minutesUntilReady = 90;
								toRemove = 1;
								goto IL_180B;
							}
							if (category == 378)
							{
								this.heldObject = new Object(Vector2.Zero, 334, 1);
								this.minutesUntilReady = 30;
								goto IL_180B;
							}
						}
						else
						{
							if (category == 380)
							{
								this.heldObject = new Object(Vector2.Zero, 335, 1);
								this.minutesUntilReady = 120;
								goto IL_180B;
							}
							if (category == 384)
							{
								this.heldObject = new Object(Vector2.Zero, 336, 1);
								this.minutesUntilReady = 300;
								goto IL_180B;
							}
							if (category == 386)
							{
								this.heldObject = new Object(Vector2.Zero, 337, 1);
								this.minutesUntilReady = 480;
								goto IL_180B;
							}
						}
						return false;
						IL_180B:
						Game1.playSound("openBox");
						DelayedAction.playSoundAfterDelay("furnace", 50);
						this.initializeLightSource(this.tileLocation);
						this.showNextIndex = true;
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(30, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 4)), Color.White, 4, false, 50f, 10, Game1.tileSize, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, -1, 0)
						{
							alphaFade = 0.005f
						});
						int i = who.items.Count - 1;
						while (i >= 0)
						{
							if (who.Items[i] is Object && (who.Items[i] as Object).ParentSheetIndex == 382)
							{
								Item expr_1908 = who.Items[i];
								category = expr_1908.Stack;
								expr_1908.Stack = category - 1;
								if (who.Items[i].Stack <= 0)
								{
									who.Items[i] = null;
									break;
								}
								break;
							}
							else
							{
								i--;
							}
						}
						dropIn.Stack -= toRemove;
						return dropIn.Stack <= 0;
					}
					else if (this.heldObject == null & probe)
					{
						int category = dropIn.ParentSheetIndex;
						if (category <= 378)
						{
							if (category != 80 && category != 378)
							{
								goto IL_1E67;
							}
						}
						else if (category != 380 && category != 384 && category != 386)
						{
							goto IL_1E67;
						}
						this.heldObject = new Object();
						return true;
					}
				}
				else if (this.name.Equals("Charcoal Kiln"))
				{
					if (who.IsMainPlayer && (dropIn.parentSheetIndex != 388 || dropIn.Stack < 10))
					{
						if (!probe && who.IsMainPlayer)
						{
							Game1.showRedMessage("Requires 10 Wood");
						}
						return false;
					}
					if (this.heldObject == null && !probe && dropIn.parentSheetIndex == 388 && dropIn.Stack >= 10)
					{
						dropIn.Stack -= 10;
						if (dropIn.Stack <= 0)
						{
							who.removeItemFromInventory(dropIn);
						}
						Game1.playSound("openBox");
						DelayedAction.playSoundAfterDelay("fireball", 50);
						this.showNextIndex = true;
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(27, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize * 2)), Color.White, 4, false, 50f, 10, Game1.tileSize, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, -1, 0)
						{
							alphaFade = 0.005f
						});
						this.heldObject = new Object(382, 1, false, -1, 0);
						this.minutesUntilReady = 30;
					}
					else if ((this.heldObject == null & probe) && dropIn.parentSheetIndex == 388 && dropIn.Stack >= 10)
					{
						this.heldObject = new Object();
						return true;
					}
				}
				else if (this.name.Equals("Slime Egg-Press"))
				{
					if (who.IsMainPlayer && (dropIn.parentSheetIndex != 766 || dropIn.Stack < 100))
					{
						if (!probe && who.IsMainPlayer)
						{
							Game1.showRedMessage("Requires 100 Slime");
						}
						return false;
					}
					if (this.heldObject == null && !probe && dropIn.parentSheetIndex == 766 && dropIn.Stack >= 100)
					{
						dropIn.Stack -= 100;
						if (dropIn.Stack <= 0)
						{
							who.removeItemFromInventory(dropIn);
						}
						Game1.playSound("slimeHit");
						DelayedAction.playSoundAfterDelay("bubbles", 50);
						who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize) * 2.5f), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Lime, 1f, 0f, 0f, 0f, false)
						{
							alphaFade = 0.005f
						});
						int slime = 680;
						if (Game1.random.NextDouble() < 0.05)
						{
							slime = 439;
						}
						else if (Game1.random.NextDouble() < 0.1)
						{
							slime = 437;
						}
						else if (Game1.random.NextDouble() < 0.25)
						{
							slime = 413;
						}
						this.heldObject = new Object(slime, 1, false, -1, 0);
						this.minutesUntilReady = 1200;
					}
					else if ((this.heldObject == null & probe) && dropIn.parentSheetIndex == 766 && dropIn.Stack >= 100)
					{
						this.heldObject = new Object();
						return true;
					}
				}
				else if (this.name.Contains("Hopper") && dropIn.ParentSheetIndex == 178)
				{
					if (probe)
					{
						this.heldObject = new Object();
						return true;
					}
					if (Utility.numSilos() <= 0)
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:NeedSilo", new object[0]));
						return false;
					}
					Game1.playSound("Ship");
					DelayedAction.playSoundAfterDelay("grassyStep", 100);
					if (dropIn.Stack == 0)
					{
						dropIn.Stack = 1;
					}
					int arg_1E2F_0 = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
					int numLeft = (Game1.getLocationFromName("Farm") as Farm).tryToAddHay(dropIn.Stack);
					int now = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
					if (arg_1E2F_0 <= 0 && now > 0)
					{
						this.showNextIndex = true;
					}
					else if (now <= 0)
					{
						this.showNextIndex = false;
					}
					dropIn.Stack = numLeft;
					if (numLeft <= 0)
					{
						return true;
					}
				}
			}
			IL_1E67:
			if (this.name.Contains("Table") && this.heldObject == null && !dropIn.bigCraftable && !dropIn.Name.Contains("Table"))
			{
				this.heldObject = (Object)dropIn.getOne();
				if (!probe)
				{
					Game1.playSound("woodyStep");
				}
				return true;
			}
			Object arg_1EC1_0 = this.heldObject;
			return false;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00078A00 File Offset: 0x00076C00
		public virtual void updateWhenCurrentLocation(GameTime time)
		{
			if (this.lightSource != null && this.isOn)
			{
				Game1.currentLightSources.Add(this.lightSource);
			}
			if (this.heldObject != null && this.heldObject.lightSource != null)
			{
				Game1.currentLightSources.Add(this.heldObject.lightSource);
			}
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.shakeTimer <= 0)
				{
					this.health = 10;
				}
			}
			if (this.parentSheetIndex == 590 && Game1.random.NextDouble() < 0.01)
			{
				this.shakeTimer = 100;
			}
			if (this.bigCraftable && this.name.Equals("Slime Ball"))
			{
				this.parentSheetIndex = 56 + (int)(time.TotalGameTime.TotalMilliseconds % 600.0 / 100.0);
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00078B00 File Offset: 0x00076D00
		public virtual void actionOnPlayerEntry()
		{
			this.health = 10;
			if (this.name != null && this.name.Contains("Hopper"))
			{
				this.showNextIndex = ((Game1.getLocationFromName("Farm") as Farm).piecesOfHay > 0);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00078B4C File Offset: 0x00076D4C
		public override bool canBeTrashed()
		{
			return (!this.questItem || !Game1.player.isThereALostItemQuestThatTakesThisItem(this.parentSheetIndex)) && (this.bigCraftable || this.parentSheetIndex != 460) && base.canBeTrashed();
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00078B88 File Offset: 0x00076D88
		public bool isForage(GameLocation location)
		{
			return this.Category == -79 || this.Category == -81 || this.Category == -80 || this.Category == -75 || location is Beach || this.parentSheetIndex == 430;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00078BD4 File Offset: 0x00076DD4
		public void initializeLightSource(Vector2 tileLocation)
		{
			if (this.name == null)
			{
				return;
			}
			if (this.bigCraftable)
			{
				if (this is Torch && this.isOn)
				{
					this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize), 2.5f, new Color(0, 80, 160), (int)(tileLocation.X * 2000f + tileLocation.Y));
					return;
				}
				if (this.isLamp)
				{
					this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)Game1.tileSize), 3f, new Color(0, 40, 80), (int)(tileLocation.X * 2000f + tileLocation.Y));
					return;
				}
				if ((this.name.Equals("Furnace") && this.minutesUntilReady > 0) || this.name.Equals("Bonfire"))
				{
					this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize), 1.5f, Color.DarkCyan, (int)(tileLocation.X * 2000f + tileLocation.Y));
					return;
				}
				if (this.name.Equals("Strange Capsule"))
				{
					this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize), 1f, Color.HotPink * 0.75f, (int)(tileLocation.X * 2000f + tileLocation.Y));
					return;
				}
			}
			else
			{
				if (this.parentSheetIndex == 93 || this.parentSheetIndex == 94)
				{
					Color c = Color.White;
					int num = this.parentSheetIndex;
					if (num != 93)
					{
						if (num == 94)
						{
							c = Color.Yellow;
							goto IL_230;
						}
						if (num != 746)
						{
							goto IL_230;
						}
					}
					c = new Color(1, 1, 1) * 0.9f;
					IL_230:
					this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 4), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 4)), (Game1.currentLocation.GetType() == typeof(MineShaft)) ? 1.5f : 1.25f, c, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
					return;
				}
				if (this.parentSheetIndex == 746)
				{
					this.lightSource = new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4)), 0.5f, new Color(1, 1, 1) * 0.65f, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
				}
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00078F18 File Offset: 0x00077118
		public virtual void performRemoveAction(Vector2 tileLocation, GameLocation environment)
		{
			if (this.lightSource != null)
			{
				Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
				Utility.removeLightSource((int)Game1.player.uniqueMultiplayerID);
			}
			if (this.bigCraftable)
			{
				if (this.parentSheetIndex == 126 && this.quality != 0)
				{
					Game1.createItemDebris(new Hat(this.quality - 1), tileLocation * (float)Game1.tileSize, (Game1.player.facingDirection + 2) % 4, null);
				}
				this.quality = 0;
			}
			if (this.name != null && this.name.Contains("Sprinkler"))
			{
				environment.removeTemporarySpritesWithID((int)tileLocation.X * 4000 + (int)tileLocation.Y);
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00078FE8 File Offset: 0x000771E8
		public virtual bool isPassable()
		{
			if (this.bigCraftable)
			{
				return false;
			}
			int num = this.parentSheetIndex;
			if (num <= 333)
			{
				switch (num)
				{
				case 286:
				case 287:
				case 288:
					break;
				default:
					if (num != 297)
					{
						switch (num)
						{
						case 328:
						case 329:
						case 331:
						case 333:
							break;
						case 330:
						case 332:
							return false;
						default:
							return false;
						}
					}
					break;
				}
			}
			else if (num <= 411)
			{
				if (num != 401)
				{
					switch (num)
					{
					case 405:
					case 407:
					case 409:
					case 411:
						break;
					case 406:
					case 408:
					case 410:
						return false;
					default:
						return false;
					}
				}
			}
			else if (num != 415 && num != 590)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000790A1 File Offset: 0x000772A1
		public virtual void reloadSprite()
		{
			this.initializeLightSource(this.tileLocation);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000790AF File Offset: 0x000772AF
		public void consumeRecipe(Farmer who)
		{
			if (this.isRecipe)
			{
				if (this.category == -7)
				{
					who.cookingRecipes.Add(this.name, 0);
					return;
				}
				who.craftingRecipes.Add(this.name, 0);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000790E8 File Offset: 0x000772E8
		public virtual Microsoft.Xna.Framework.Rectangle getBoundingBox(Vector2 tileLocation)
		{
			this.boundingBox.X = (int)tileLocation.X * Game1.tileSize;
			this.boundingBox.Y = (int)tileLocation.Y * Game1.tileSize;
			if ((this is Torch && !this.bigCraftable) || this.parentSheetIndex == 590)
			{
				this.boundingBox.X = (int)tileLocation.X * Game1.tileSize + Game1.tileSize * 3 / 8;
				this.boundingBox.Y = (int)tileLocation.Y * Game1.tileSize + Game1.tileSize * 3 / 8;
			}
			return this.boundingBox;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0007918C File Offset: 0x0007738C
		public override bool canBeGivenAsGift()
		{
			return !this.bigCraftable && !(this is Furniture);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000791A4 File Offset: 0x000773A4
		public virtual bool performDropDownAction(Farmer who)
		{
			if (who == null)
			{
				who = Game1.getFarmer(this.owner);
			}
			if (this.name.Equals("Worm Bin"))
			{
				if (this.heldObject == null)
				{
					this.heldObject = new Object(685, Game1.random.Next(2, 6), false, -1, 0);
					this.minutesUntilReady = 2600 - Game1.timeOfDay;
				}
				return false;
			}
			if (this.name.Equals("Bee House"))
			{
				if (this.heldObject == null)
				{
					this.heldObject = new Object(Vector2.Zero, 340, null, false, true, false, false);
					this.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
				}
				return false;
			}
			if (this.name.Contains("Strange Capsule"))
			{
				this.minutesUntilReady = 6000 - Game1.timeOfDay;
			}
			else if (this.name.Contains("Hopper"))
			{
				this.showNextIndex = false;
				if ((Game1.getLocationFromName("Farm") as Farm).piecesOfHay >= 0)
				{
					this.showNextIndex = true;
				}
			}
			return false;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000792B8 File Offset: 0x000774B8
		private void totemWarp(Farmer who)
		{
			for (int i = 0; i < 12; i++)
			{
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float)Game1.random.Next(25, 75), 6, 1, new Vector2((float)Game1.random.Next((int)who.position.X - Game1.tileSize * 4, (int)who.position.X + Game1.tileSize * 3), (float)Game1.random.Next((int)who.position.Y - Game1.tileSize * 4, (int)who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
			}
			Game1.playSound("wand");
			Game1.displayFarmer = false;
			Game1.player.freezePause = 1000;
			Game1.flashAlpha = 1f;
			DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.totemWarpForReal), 1000);
			Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize);
			r.Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
			int j = 0;
			for (int x = who.getTileX() + 8; x >= who.getTileX() - 8; x--)
			{
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)x, (float)who.getTileY()) * (float)Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					delayBeforeAnimationStart = j * 25,
					motion = new Vector2(-0.25f, 0f)
				});
				j++;
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00079494 File Offset: 0x00077694
		private void totemWarpForReal()
		{
			switch (this.parentSheetIndex)
			{
			case 688:
				Game1.warpFarmer("Farm", 48, 7, false);
				break;
			case 689:
				Game1.warpFarmer("Mountain", 31, 20, false);
				break;
			case 690:
				Game1.warpFarmer("Beach", 20, 4, false);
				break;
			}
			Game1.fadeToBlackAlpha = 0.99f;
			Game1.screenGlow = false;
			Game1.player.temporarilyInvincible = false;
			Game1.player.temporaryInvincibilityTimer = 0;
			Game1.displayFarmer = true;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00079520 File Offset: 0x00077720
		private void rainTotem(Farmer who)
		{
			if (!Game1.IsMultiplayer && !Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
			{
				Game1.weatherForTomorrow = 1;
				Game1.pauseThenMessage(2000, "Clouds gather in the distance...", false);
			}
			Game1.screenGlow = false;
			Game1.playSound("thunder");
			who.canMove = false;
			Game1.screenGlowOnce(Color.SlateBlue, false, 0.005f, 0.3f);
			Game1.player.faceDirection(2);
			Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(57, 2000, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
			});
			for (int i = 0; i < 6; i++)
			{
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 1045, 52, 33), 9999f, 1, 999, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, 1f, 0.01f, Color.White * 0.8f, (float)(Game1.pixelZoom / 2), 0.01f, 0f, 0f, false)
				{
					motion = new Vector2((float)Game1.random.Next(-10, 11) / 10f, -2f),
					delayBeforeAnimationStart = i * 200
				});
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 1045, 52, 33), 9999f, 1, 999, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, 1f, 0.01f, Color.White * 0.8f, (float)(Game1.pixelZoom / 4), 0.01f, 0f, 0f, false)
				{
					motion = new Vector2((float)Game1.random.Next(-30, -10) / 10f, -1f),
					delayBeforeAnimationStart = 100 + i * 200
				});
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 1045, 52, 33), 9999f, 1, 999, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, 1f, 0.01f, Color.White * 0.8f, (float)(Game1.pixelZoom / 4), 0.01f, 0f, 0f, false)
				{
					motion = new Vector2((float)Game1.random.Next(10, 30) / 10f, -1f),
					delayBeforeAnimationStart = 200 + i * 200
				});
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, false, 0f)
			{
				motion = new Vector2(0f, -7f),
				acceleration = new Vector2(0f, 0.1f),
				scaleChange = 0.015f,
				alpha = 1f,
				alphaFade = 0.0075f,
				shakeIntensity = 1f,
				initialPosition = Game1.player.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)),
				xPeriodic = true,
				xPeriodicLoopTime = 1000f,
				xPeriodicRange = (float)(Game1.tileSize / 16),
				layerDepth = 1f
			});
			DelayedAction.playSoundAfterDelay("rainsound", 2000);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00079924 File Offset: 0x00077B24
		public bool performUseAction()
		{
			if (!Game1.player.canMove)
			{
				return false;
			}
			if (this.name != null && this.name.Contains("Totem") && !Game1.eventUp && !Game1.isFestival() && !Game1.fadeToBlack && !Game1.player.swimming && !Game1.player.bathingClothes)
			{
				int num = this.parentSheetIndex;
				if (num == 681)
				{
					this.rainTotem(Game1.player);
					return true;
				}
				switch (num)
				{
				case 688:
				case 689:
				case 690:
				{
					Game1.player.jitterStrength = 1f;
					Color sprinkleColor = (this.parentSheetIndex == 681) ? Color.SlateBlue : ((this.parentSheetIndex == 688) ? Color.LimeGreen : ((this.parentSheetIndex == 689) ? Color.OrangeRed : Color.LightBlue));
					Game1.playSound("warrior");
					Game1.player.faceDirection(2);
					Game1.player.CanMove = false;
					Game1.player.temporarilyInvincible = true;
					Game1.player.temporaryInvincibilityTimer = -4000;
					Game1.changeMusicTrack("none");
					if (this.parentSheetIndex == 681)
					{
						Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(57, 2000, false, false, null, false),
							new FarmerSprite.AnimationFrame((int)((short)Game1.player.FarmerSprite.currentFrame), 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.rainTotem), true)
						});
					}
					else
					{
						Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(57, 2000, false, false, null, false),
							new FarmerSprite.AnimationFrame((int)((short)Game1.player.FarmerSprite.currentFrame), 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.totemWarp), true)
						});
					}
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, false, 0f)
					{
						motion = new Vector2(0f, -1f),
						scaleChange = 0.01f,
						alpha = 1f,
						alphaFade = 0.0075f,
						shakeIntensity = 1f,
						initialPosition = Game1.player.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)),
						xPeriodic = true,
						xPeriodicLoopTime = 1000f,
						xPeriodicRange = (float)(Game1.tileSize / 16),
						layerDepth = 1f
					});
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2((float)(-(float)Game1.tileSize), (float)(-(float)Game1.tileSize * 3 / 2)), false, false, false, 0f)
					{
						motion = new Vector2(0f, -0.5f),
						scaleChange = 0.005f,
						scale = 0.5f,
						alpha = 1f,
						alphaFade = 0.0075f,
						shakeIntensity = 1f,
						delayBeforeAnimationStart = 10,
						initialPosition = Game1.player.position + new Vector2((float)(-(float)Game1.tileSize), (float)(-(float)Game1.tileSize * 3 / 2)),
						xPeriodic = true,
						xPeriodicLoopTime = 1000f,
						xPeriodicRange = (float)(Game1.tileSize / 16),
						layerDepth = 0.9999f
					});
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 9999f, 1, 999, Game1.player.position + new Vector2((float)Game1.tileSize, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, false, 0f)
					{
						motion = new Vector2(0f, -0.5f),
						scaleChange = 0.005f,
						scale = 0.5f,
						alpha = 1f,
						alphaFade = 0.0075f,
						delayBeforeAnimationStart = 20,
						shakeIntensity = 1f,
						initialPosition = Game1.player.position + new Vector2((float)Game1.tileSize, (float)(-(float)Game1.tileSize * 3 / 2)),
						xPeriodic = true,
						xPeriodicLoopTime = 1000f,
						xPeriodicRange = (float)(Game1.tileSize / 16),
						layerDepth = 0.9988f
					});
					Game1.screenGlowOnce(sprinkleColor, false, 0.005f, 0.3f);
					Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 16, 16, 1300, 20, Color.White, null, true);
					return true;
				}
				}
			}
			string arg_524_0 = this.name;
			return false;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00079E58 File Offset: 0x00078058
		public override Color getCategoryColor()
		{
			if (this is Furniture)
			{
				return new Color(100, 25, 190);
			}
			if (this.type != null && this.type.Equals("Arch"))
			{
				return new Color(110, 0, 90);
			}
			int category = this.category;
			switch (category)
			{
			case -81:
				return new Color(10, 130, 50);
			case -80:
				return new Color(219, 54, 211);
			case -79:
				return Color.DeepPink;
			case -78:
			case -77:
			case -76:
				break;
			case -75:
				return Color.Green;
			case -74:
				return Color.Brown;
			default:
				switch (category)
				{
				case -28:
					return new Color(50, 10, 70);
				case -27:
				case -26:
					return new Color(0, 155, 111);
				case -24:
					return Color.Plum;
				case -22:
					return Color.DarkCyan;
				case -21:
					return Color.DarkRed;
				case -20:
					return Color.DarkGray;
				case -19:
					return Color.SlateGray;
				case -18:
				case -14:
				case -6:
				case -5:
					return new Color(255, 0, 100);
				case -16:
				case -15:
					return new Color(64, 102, 114);
				case -12:
				case -2:
					return new Color(110, 0, 90);
				case -8:
					return new Color(148, 61, 40);
				case -7:
					return new Color(220, 60, 0);
				case -4:
					return Color.DarkBlue;
				}
				break;
			}
			return Color.Black;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0007A004 File Offset: 0x00078204
		public override string getCategoryName()
		{
			if (this is Furniture)
			{
				return "Furniture";
			}
			if (this.type != null && this.type.Equals("Arch"))
			{
				return "Artifact";
			}
			int category = this.category;
			switch (category)
			{
			case -81:
				return "Forage";
			case -80:
				return "Flower";
			case -79:
				return "Fruit";
			case -78:
			case -77:
			case -76:
				break;
			case -75:
				return "Vegetable";
			case -74:
				return "Seed";
			default:
				switch (category)
				{
				case -28:
					return "Monster Loot";
				case -27:
				case -26:
					return "Artisan Goods";
				case -25:
					return "Cooking";
				case -24:
					return "Decor";
				case -22:
					return "Fishing Tackle";
				case -21:
					return "Bait";
				case -20:
					return "Trash";
				case -19:
					return "Fertilizer";
				case -18:
				case -14:
				case -5:
					return "Animal Product";
				case -16:
				case -15:
					return "Resource";
				case -12:
				case -2:
					return "Mineral";
				case -8:
					return "Crafting";
				case -7:
					return "Cooking";
				case -6:
					return "Animal Product";
				case -4:
					return "Fish";
				}
				break;
			}
			return "";
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0007A163 File Offset: 0x00078363
		public virtual bool isActionable(Farmer who)
		{
			return this.checkForAction(who, true);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0007A16D File Offset: 0x0007836D
		public int getHealth()
		{
			return this.health;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0007A175 File Offset: 0x00078375
		public void setHealth(int health)
		{
			this.health = health;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0007A180 File Offset: 0x00078380
		public virtual bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (!justCheckingForActivity && who != null && who.currentLocation.isObjectAt(who.getTileX(), who.getTileY() - 1) && who.currentLocation.isObjectAt(who.getTileX(), who.getTileY() + 1) && who.currentLocation.isObjectAt(who.getTileX() + 1, who.getTileY()) && who.currentLocation.isObjectAt(who.getTileX() - 1, who.getTileY()))
			{
				this.performToolAction(null);
			}
			if (this.name.Equals("Prairie King Arcade System"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				Game1.currentMinigame = new AbigailGame(false);
			}
			else if (this.name.Equals("Junimo Kart Arcade System"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				Response[] responses = new Response[]
				{
					new Response("Progress", "Progress Mode"),
					new Response("Endless", "Endless Mode"),
					new Response("Exit", "Exit")
				};
				Game1.currentLocation.createQuestionDialogue("= Junimo Kart =", responses, "MinecartGame");
			}
			else if (this.name.Equals("Staircase"))
			{
				if (who.currentLocation is MineShaft && (who.currentLocation as MineShaft).mineLevel != 120)
				{
					if (justCheckingForActivity)
					{
						return true;
					}
					Game1.enterMine(false, Game1.mine.mineLevel + 1, null);
					Game1.playSound("stairsdown");
				}
			}
			else if (this.name.Equals("Slime Ball"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				who.currentLocation.objects.Remove(this.tileLocation);
				DelayedAction.playSoundAfterDelay("slimedead", 40);
				DelayedAction.playSoundAfterDelay("slimeHit", 100);
				Game1.playSound("slimeHit");
				Random r = new Random((int)(Game1.stats.daysPlayed + (uint)((int)Game1.uniqueIDForThisGame) + (uint)((int)this.tileLocation.X * 77) + (uint)((int)this.tileLocation.Y * 777) + 2u));
				Game1.createMultipleObjectDebris(766, (int)this.tileLocation.X, (int)this.tileLocation.Y, r.Next(10, 21), 1f + ((who.facingDirection == 2) ? 0f : ((float)Game1.random.NextDouble())));
				Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.tileLocation * (float)Game1.tileSize, Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					interval = 70f,
					holdLastFrame = true,
					alphaFade = 0.01f
				}, Game1.currentLocation, 4, 64, 64);
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					interval = 70f,
					delayBeforeAnimationStart = 0,
					holdLastFrame = true,
					alphaFade = 0.01f
				});
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 4)), Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					interval = 70f,
					delayBeforeAnimationStart = 100,
					holdLastFrame = true,
					alphaFade = 0.01f
				});
				who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), 0f), Color.Lime, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					interval = 70f,
					delayBeforeAnimationStart = 200,
					holdLastFrame = true,
					alphaFade = 0.01f
				});
				while (r.NextDouble() < 0.33)
				{
					Game1.createObjectDebris(557, (int)this.tileLocation.X, (int)this.tileLocation.Y, who.uniqueMultiplayerID);
				}
				return false;
			}
			else if (this.name.Equals("Bookcase"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				Game1.drawObjectDialogue(Game1.parseText(Utility.getExcerptText(new Random((int)this.tileLocation.X * 1000 + (int)this.tileLocation.Y + (int)Game1.uniqueIDForThisGame + (int)(Game1.stats.DaysPlayed / 28u)))));
			}
			else if (this.name.Equals("Furnace") && who.ActiveObject == null && !this.readyForHarvest)
			{
				if (this.heldObject != null)
				{
					return true;
				}
			}
			else if (this.name.Contains("Table"))
			{
				if (this.heldObject != null)
				{
					if (justCheckingForActivity)
					{
						return true;
					}
					if (who.addItemToInventoryBool(this.heldObject, false))
					{
						this.heldObject = null;
						Game1.playSound("coin");
					}
					return true;
				}
				else
				{
					if (!this.name.Equals("Tile Table"))
					{
						return false;
					}
					if (justCheckingForActivity)
					{
						return true;
					}
					this.parentSheetIndex++;
					if (this.parentSheetIndex == 322)
					{
						this.parentSheetIndex -= 9;
						return false;
					}
					return true;
				}
			}
			else if (this.name.Contains("Stool"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				this.parentSheetIndex++;
				if (this.parentSheetIndex == 305)
				{
					this.parentSheetIndex -= 9;
					return false;
				}
				return true;
			}
			else if (this.bigCraftable && (this.name.Contains("Chair") || this.name.Contains("Painting") || this.name.Equals("House Plant")))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				this.parentSheetIndex++;
				int total = -1;
				int baseIndex = -1;
				string a = this.name;
				if (!(a == "Red Chair"))
				{
					if (!(a == "Patio Chair"))
					{
						if (!(a == "Dark Chair"))
						{
							if (!(a == "Wood Chair"))
							{
								if (!(a == "House Plant"))
								{
									if (a == "Painting")
									{
										total = 8;
										baseIndex = 32;
									}
								}
								else
								{
									total = 8;
									baseIndex = 0;
								}
							}
							else
							{
								total = 4;
								baseIndex = 24;
							}
						}
						else
						{
							total = 4;
							baseIndex = 60;
						}
					}
					else
					{
						total = 4;
						baseIndex = 52;
					}
				}
				else
				{
					total = 4;
					baseIndex = 44;
				}
				if (this.parentSheetIndex == baseIndex + total)
				{
					this.parentSheetIndex -= total;
					return false;
				}
				return true;
			}
			else if (this.name.Equals("Flute Block"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				this.scale.X = this.scale.X + 100f;
				this.scale.X = this.scale.X % 2400f;
				this.shakeTimer = 200;
				if (Game1.soundBank != null)
				{
					if (this.internalSound != null)
					{
						this.internalSound.Stop(AudioStopOptions.Immediate);
						this.internalSound = Game1.soundBank.GetCue("flute");
					}
					else
					{
						this.internalSound = Game1.soundBank.GetCue("flute");
					}
					this.internalSound.SetVariable("Pitch", this.scale.X);
					this.internalSound.Play();
				}
				this.scale.Y = 1.3f;
				this.shakeTimer = 200;
				return true;
			}
			else if (this.name.Equals("Drum Block"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				this.scale.X = this.scale.X + 1f;
				this.scale.X = this.scale.X % 7f;
				this.shakeTimer = 200;
				if (Game1.soundBank != null)
				{
					if (this.internalSound != null)
					{
						this.internalSound.Stop(AudioStopOptions.Immediate);
						this.internalSound = Game1.soundBank.GetCue("drumkit" + this.scale.X);
					}
					else
					{
						this.internalSound = Game1.soundBank.GetCue("drumkit" + this.scale.X);
					}
					this.internalSound.Play();
				}
				this.scale.Y = 1.3f;
				this.shakeTimer = 200;
				return true;
			}
			else if (this.name.Contains("arecrow"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				this.shakeTimer = 100;
				if (this.parentSheetIndex == 126 && who.CurrentItem != null && who.CurrentItem is Hat)
				{
					if (this.quality != 0)
					{
						Game1.createItemDebris(new Hat(this.quality - 1), this.tileLocation * (float)Game1.tileSize, (who.facingDirection + 2) % 4, null);
					}
					this.quality = (who.CurrentItem as Hat).which + 1;
					who.items[who.CurrentToolIndex] = null;
					Game1.playSound("dirtyHit");
					return true;
				}
				if (this.specialVariable == 0)
				{
					Game1.drawObjectDialogue("I haven't encountered any crows yet.");
				}
				else
				{
					Game1.drawObjectDialogue(string.Concat(new object[]
					{
						"I've scared off ",
						this.specialVariable,
						" crow",
						(this.specialVariable == 1) ? "." : "s."
					}));
				}
				return true;
			}
			else if (this.name.Equals("Singing Stone"))
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				if (Game1.soundBank != null)
				{
					Cue arg_9C6_0 = Game1.soundBank.GetCue("crystal");
					int pitch = Game1.random.Next(2400);
					pitch -= pitch % 100;
					arg_9C6_0.SetVariable("Pitch", (float)pitch);
					this.shakeTimer = 100;
					arg_9C6_0.Play();
				}
			}
			else if (this.name.Contains("Hopper") && who.ActiveObject == null)
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				if (who.freeSpotsInInventory() > 0)
				{
					int piecesHay = (Game1.getLocationFromName("Farm") as Farm).piecesOfHay;
					if (piecesHay > 0)
					{
						if (Game1.currentLocation is AnimalHouse)
						{
							int piecesOfHayToRemove = Math.Min((Game1.currentLocation as AnimalHouse).animalsThatLiveHere.Count, piecesHay);
							piecesOfHayToRemove = Math.Max(1, piecesOfHayToRemove);
							AnimalHouse i = Game1.currentLocation as AnimalHouse;
							int alreadyHay = i.numberOfObjectsWithName("Hay");
							piecesOfHayToRemove = Math.Min(piecesOfHayToRemove, i.animalLimit - alreadyHay);
							if (piecesOfHayToRemove != 0 && Game1.player.couldInventoryAcceptThisObject(178, piecesOfHayToRemove, 0))
							{
								(Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= Math.Max(1, piecesOfHayToRemove);
								who.addItemToInventoryBool(new Object(178, piecesOfHayToRemove, false, -1, 0), false);
								Game1.playSound("shwip");
							}
						}
						else if (Game1.player.couldInventoryAcceptThisObject(178, 1, 0))
						{
							(Game1.getLocationFromName("Farm") as Farm).piecesOfHay--;
							who.addItemToInventoryBool(new Object(178, 1, false, -1, 0), false);
							Game1.playSound("shwip");
						}
						if ((Game1.getLocationFromName("Farm") as Farm).piecesOfHay <= 0)
						{
							this.showNextIndex = false;
						}
					}
					else
					{
						Game1.drawObjectDialogue("The hopper is empty. Build a silo and cut grass with your scythe to refill it.");
					}
				}
				else
				{
					Game1.showRedMessage("Inventory Full");
				}
			}
			if (!this.readyForHarvest)
			{
				return false;
			}
			if (justCheckingForActivity)
			{
				return true;
			}
			if (this.name.Equals("Bee House"))
			{
				string honeyName = "Wild";
				int honeyPriceAddition = 0;
				if (who.currentLocation is Farm)
				{
					Crop c = Utility.findCloseFlower(this.tileLocation);
					if (c != null)
					{
						honeyName = Game1.objectInformation[c.indexOfHarvest].Split(new char[]
						{
							'/'
						})[0];
						honeyPriceAddition = Convert.ToInt32(Game1.objectInformation[c.indexOfHarvest].Split(new char[]
						{
							'/'
						})[1]) * 2;
					}
				}
				if (this.heldObject != null)
				{
					this.heldObject.name = honeyName + " Honey";
					this.heldObject.price += honeyPriceAddition;
					if (Game1.currentSeason.Equals("winter"))
					{
						this.heldObject = null;
						this.readyForHarvest = false;
						this.showNextIndex = false;
						return false;
					}
					if (who.IsMainPlayer && !who.addItemToInventoryBool(this.heldObject, false))
					{
						Game1.showRedMessage("Inventory Full");
						return false;
					}
					Game1.playSound("coin");
				}
			}
			else
			{
				if (who.IsMainPlayer && !who.addItemToInventoryBool(this.heldObject, false))
				{
					Game1.showRedMessage("Inventory Full");
					return false;
				}
				Game1.playSound("coin");
				string a = this.name;
				if (!(a == "Keg"))
				{
					if (!(a == "Preserves Jar"))
					{
						if (a == "Cheese Press")
						{
							if (this.heldObject.ParentSheetIndex == 426)
							{
								Stats expr_D57 = Game1.stats;
								uint num = expr_D57.GoatCheeseMade;
								expr_D57.GoatCheeseMade = num + 1u;
							}
							else
							{
								Stats expr_D6F = Game1.stats;
								uint num = expr_D6F.CheeseMade;
								expr_D6F.CheeseMade = num + 1u;
							}
						}
					}
					else
					{
						Stats expr_D2D = Game1.stats;
						uint num = expr_D2D.PreservesMade;
						expr_D2D.PreservesMade = num + 1u;
					}
				}
				else
				{
					Stats expr_D15 = Game1.stats;
					uint num = expr_D15.BeveragesMade;
					expr_D15.BeveragesMade = num + 1u;
				}
			}
			if (this.name.Equals("Crystalarium"))
			{
				this.minutesUntilReady = this.getMinutesForCrystalarium(this.heldObject.ParentSheetIndex);
				this.heldObject = (Object)this.heldObject.getOne();
			}
			else if (this.name.Equals("Tapper"))
			{
				int num2 = this.heldObject.ParentSheetIndex;
				if (num2 <= 420)
				{
					if (num2 == 404 || num2 == 420)
					{
						this.minutesUntilReady = 3000 - Game1.timeOfDay;
						if (!Game1.currentSeason.Equals("fall"))
						{
							this.heldObject = new Object(404, 1, false, -1, 0);
							this.minutesUntilReady = 6000 - Game1.timeOfDay;
						}
						if (Game1.dayOfMonth % 10 == 0)
						{
							this.heldObject = new Object(422, 1, false, -1, 0);
						}
						if (Game1.currentSeason.Equals("winter"))
						{
							this.minutesUntilReady = 80000 - Game1.timeOfDay;
						}
					}
				}
				else if (num2 != 422)
				{
					switch (num2)
					{
					case 724:
						this.minutesUntilReady = 16000 - Game1.timeOfDay;
						break;
					case 725:
						this.minutesUntilReady = 13000 - Game1.timeOfDay;
						break;
					case 726:
						this.minutesUntilReady = 10000 - Game1.timeOfDay;
						break;
					}
				}
				else
				{
					this.minutesUntilReady = 3000 - Game1.timeOfDay;
					this.heldObject = new Object(420, 1, false, -1, 0);
				}
				if (this.heldObject != null)
				{
					this.heldObject = (Object)this.heldObject.getOne();
				}
			}
			else
			{
				this.heldObject = null;
			}
			this.readyForHarvest = false;
			this.showNextIndex = false;
			if (this.name.Equals("Bee House") && !Game1.currentSeason.Equals("winter"))
			{
				this.heldObject = new Object(Vector2.Zero, 340, null, false, true, false, false);
				this.minutesUntilReady = 2400 - Game1.timeOfDay + 4320;
			}
			else if (this.name.Equals("Worm Bin"))
			{
				this.heldObject = new Object(685, Game1.random.Next(2, 6), false, -1, 0);
				this.minutesUntilReady = 2600 - Game1.timeOfDay;
			}
			return true;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0007B184 File Offset: 0x00079384
		public void farmerAdjacentAction()
		{
			if (this.name == null)
			{
				return;
			}
			if (this.name.Equals("Flute Block") && (this.internalSound == null || (Game1.noteBlockTimer == 0f && !this.internalSound.IsPlaying)) && !Game1.dialogueUp)
			{
				if (Game1.soundBank != null)
				{
					this.internalSound = Game1.soundBank.GetCue("flute");
					this.internalSound.SetVariable("Pitch", this.scale.X);
					this.internalSound.Play();
				}
				this.scale.Y = 1.3f;
				this.shakeTimer = 200;
				return;
			}
			if (this.name.Equals("Drum Block") && (this.internalSound == null || (Game1.noteBlockTimer == 0f && !this.internalSound.IsPlaying)) && !Game1.dialogueUp)
			{
				if (Game1.soundBank != null)
				{
					this.internalSound = Game1.soundBank.GetCue("drumkit" + this.scale.X);
					this.internalSound.Play();
				}
				this.scale.Y = 1.3f;
				this.shakeTimer = 200;
				return;
			}
			if (this.name.Equals("Obelisk"))
			{
				this.scale.X = this.scale.X + 1f;
				if (this.scale.X > 30f)
				{
					this.parentSheetIndex = ((this.parentSheetIndex == 29) ? 30 : 29);
					this.scale.X = 0f;
					this.scale.Y = this.scale.Y + 2f;
				}
				if (this.scale.Y >= 20f && Game1.random.NextDouble() < 0.0001 && Game1.currentLocation.characters.Count < 4)
				{
					foreach (Vector2 v in Game1.player.getAdjacentTiles())
					{
						if (!Game1.currentLocation.isTileOccupied(v, "") && Game1.currentLocation.isTilePassable(new Location((int)v.X, (int)v.Y), Game1.viewport) && Game1.currentLocation.isCharacterAtTile(v) == null)
						{
							if (Game1.random.NextDouble() < 0.1)
							{
								Game1.currentLocation.characters.Add(new GreenSlime(v * new Vector2((float)Game1.tileSize, (float)Game1.tileSize)));
							}
							else if (Game1.random.NextDouble() < 0.5)
							{
								Game1.currentLocation.characters.Add(new ShadowGuy(v * new Vector2((float)Game1.tileSize, (float)Game1.tileSize)));
							}
							else
							{
								Game1.currentLocation.characters.Add(new ShadowGirl(v * new Vector2((float)Game1.tileSize, (float)Game1.tileSize)));
							}
							((Monster)Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1]).moveTowardPlayerThreshold = 4;
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(352, 400f, 2, 1, v * new Vector2((float)Game1.tileSize, (float)Game1.tileSize), false, false));
							Game1.playSound("shadowpeep");
							break;
						}
					}
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0007B544 File Offset: 0x00079744
		public void addWorkingAnimation(GameLocation environment)
		{
			if (environment == null || environment.getFarmers().Count == 0)
			{
				return;
			}
			string a = this.name;
			if (a == "Keg")
			{
				Color c = Color.DarkGray;
				if (this.heldObject.Name.Contains("Wine"))
				{
					c = Color.Lavender;
				}
				else if (this.heldObject.Name.Contains("Juice"))
				{
					c = Color.White;
				}
				else if (this.heldObject.name.Equals("Beer"))
				{
					c = Color.Yellow;
				}
				environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, c * 0.75f, 1f, 0f, 0f, 0f, false)
				{
					alphaFade = 0.005f
				});
				Game1.playSound("bubbles");
				return;
			}
			if (a == "Preserves Jar")
			{
				Color c = Color.White;
				if (this.heldObject.Name.Contains("Pickled"))
				{
					c = Color.White;
				}
				else if (this.heldObject.Name.Contains("Jelly"))
				{
					c = Color.LightBlue;
				}
				environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, c * 0.75f, 1f, 0f, 0f, 0f, false)
				{
					alphaFade = 0.005f
				});
				return;
			}
			if (!(a == "Oil Maker"))
			{
				if (!(a == "Furnace"))
				{
					if (!(a == "Slime Egg-Press"))
					{
						return;
					}
					environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize) * 2.5f), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Lime, 1f, 0f, 0f, 0f, false)
					{
						alphaFade = 0.005f
					});
				}
				else if (Game1.random.NextDouble() < 0.5)
				{
					environment.temporarySprites.Add(new TemporaryAnimatedSprite(30, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 4)), Color.White, 4, false, 50f, 10, Game1.tileSize, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, -1, 0)
					{
						alphaFade = 0.005f,
						light = true,
						lightcolor = Color.Black
					});
					Game1.playSound("fireball");
					return;
				}
				return;
			}
			environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow, 1f, 0f, 0f, 0f, false)
			{
				alphaFade = 0.005f
			});
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0007B9E8 File Offset: 0x00079BE8
		public virtual bool minutesElapsed(int minutes, GameLocation environment)
		{
			if (this.heldObject != null && !this.name.Contains("Table"))
			{
				if (this.name.Equals("Bee House") && !environment.IsOutdoors)
				{
					return false;
				}
				this.minutesUntilReady -= minutes;
				if (this.minutesUntilReady <= 0 && !this.name.Contains("Incubator"))
				{
					if (!this.readyForHarvest && Game1.currentLocation.Equals(environment))
					{
						Game1.playSound("dwop");
					}
					this.readyForHarvest = true;
					this.minutesUntilReady = 0;
					this.showNextIndex = false;
					if (this.name.Equals("Bee House") || this.name.Equals("Loom") || this.name.Equals("Mushroom Box"))
					{
						this.showNextIndex = true;
					}
					if (this.lightSource != null)
					{
						Utility.removeLightSource(this.lightSource.identifier);
						this.lightSource = null;
					}
				}
				if (!this.readyForHarvest && Game1.random.NextDouble() < 0.33)
				{
					this.addWorkingAnimation(environment);
				}
			}
			else if (this.bigCraftable)
			{
				int num = this.parentSheetIndex;
				if (num <= 83)
				{
					if (num != 29 && num != 30)
					{
						if (num == 83)
						{
							this.showNextIndex = false;
							Utility.removeLightSource((int)(this.tileLocation.X * 797f + this.tileLocation.Y * 13f + 666f));
						}
					}
					else
					{
						this.showNextIndex = (this.parentSheetIndex == 29);
						this.scale.Y = Math.Max(0f, this.scale.Y = this.scale.Y - (float)(minutes / 2 + 1));
					}
				}
				else if (num <= 97)
				{
					if (num == 96 || num == 97)
					{
						this.minutesUntilReady -= minutes;
						this.showNextIndex = (this.parentSheetIndex == 96);
						if (this.minutesUntilReady <= 0)
						{
							environment.objects.Remove(this.tileLocation);
							environment.objects.Add(this.tileLocation, new Object(this.tileLocation, 98, false));
						}
					}
				}
				else if (num == 141 || num == 142)
				{
					this.showNextIndex = (this.parentSheetIndex == 141);
				}
			}
			return false;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0007BC68 File Offset: 0x00079E68
		public override string checkForSpecialItemHoldUpMeessage()
		{
			if (!this.bigCraftable)
			{
				if (this.type != null && this.type.Equals("Arch"))
				{
					return "You found an artifact! The curator of the local museum might want to know about this.";
				}
				int num = this.parentSheetIndex;
				if (num == 102)
				{
					return "You found a 'Lost Book'. The library's collection has expanded!";
				}
				if (num == 535)
				{
					return "You found a 'Geode'! The local blacksmith can break it open for you. Who knows what might be hidden inside?";
				}
			}
			else
			{
				int num = this.parentSheetIndex;
				if (num == 160)
				{
					return "You found a peculiar statue behind Grandpa's Shrine.";
				}
			}
			return base.checkForSpecialItemHoldUpMeessage();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0007BCDC File Offset: 0x00079EDC
		public bool countsForShippedCollection()
		{
			if (this.type == null || this.type.Contains("Arch") || this.bigCraftable)
			{
				return false;
			}
			if (this.parentSheetIndex == 433)
			{
				return true;
			}
			int category = this.Category;
			if (category <= -12)
			{
				if (category <= -19)
				{
					if (category != -74)
					{
						switch (category)
						{
						case -29:
						case -24:
						case -22:
						case -21:
						case -20:
						case -19:
							break;
						case -28:
						case -27:
						case -26:
						case -25:
						case -23:
							goto IL_A7;
						default:
							goto IL_A7;
						}
					}
				}
				else if (category != -14 && category != -12)
				{
					goto IL_A7;
				}
			}
			else if (category <= -7)
			{
				if (category != -8 && category != -7)
				{
					goto IL_A7;
				}
			}
			else if (category != -2 && category != 0)
			{
				goto IL_A7;
			}
			return false;
			IL_A7:
			return Object.isIndexOkForBasicShippedCategory(this.parentSheetIndex);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0007BD9B File Offset: 0x00079F9B
		public static bool isIndexOkForBasicShippedCategory(int index)
		{
			return index != 434;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0007BDA8 File Offset: 0x00079FA8
		public static bool isPotentialBasicShippedCategory(int index, string category)
		{
			int cat = 0;
			int.TryParse(category, out cat);
			if (index == 433)
			{
				return true;
			}
			if (cat != 0)
			{
				if (cat <= -14)
				{
					if (cat != -74)
					{
						switch (cat)
						{
						case -29:
						case -24:
						case -22:
						case -21:
						case -20:
						case -19:
							break;
						case -28:
						case -27:
						case -26:
						case -25:
						case -23:
							goto IL_7D;
						default:
							if (cat != -14)
							{
								goto IL_7D;
							}
							break;
						}
					}
				}
				else if (cat <= -8)
				{
					if (cat != -12 && cat != -8)
					{
						goto IL_7D;
					}
				}
				else if (cat != -7 && cat != -2)
				{
					goto IL_7D;
				}
				return false;
				IL_7D:
				return Object.isIndexOkForBasicShippedCategory(index);
			}
			return false;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0007BE38 File Offset: 0x0007A038
		public Vector2 getScale()
		{
			if (this.category == -22)
			{
				return Vector2.Zero;
			}
			if (!this.bigCraftable)
			{
				this.scale.Y = Math.Max((float)Game1.pixelZoom, this.scale.Y - (float)Game1.pixelZoom / 100f);
				return this.scale;
			}
			if ((this.heldObject == null && this.minutesUntilReady <= 0) || this.readyForHarvest || this.name.Equals("Bee House") || this.name.Contains("Table") || this.name.Equals("Tapper"))
			{
				return Vector2.Zero;
			}
			if (this.name.Equals("Loom"))
			{
				this.scale.X = (float)((double)(this.scale.X + (float)Game1.pixelZoom / 100f) % 6.2831853071795862);
				return Vector2.Zero;
			}
			this.scale.X = this.scale.X - 0.1f;
			this.scale.Y = this.scale.Y + 0.1f;
			if (this.scale.X <= 0f)
			{
				this.scale.X = 10f;
			}
			if (this.scale.Y >= 10f)
			{
				this.scale.Y = 0f;
			}
			return new Vector2(Math.Abs(this.scale.X - 5f), Math.Abs(this.scale.Y - 5f));
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0007BFCC File Offset: 0x0007A1CC
		public virtual void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			if (f.ActiveObject.bigCraftable)
			{
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
				return;
			}
			spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition, new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
			if (f.ActiveObject != null && f.ActiveObject.Name.Contains("="))
			{
				spriteBatch.Draw(Game1.objectSpriteSheet, objectPosition + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(f.ActiveObject.ParentSheetIndex)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), (float)Game1.pixelZoom + Math.Abs(Game1.starCropShimmerPause) / 8f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
				if (Math.Abs(Game1.starCropShimmerPause) <= 0.05f && Game1.random.NextDouble() < 0.97)
				{
					return;
				}
				Game1.starCropShimmerPause += 0.04f;
				if (Game1.starCropShimmerPause >= 0.8f)
				{
					Game1.starCropShimmerPause = -0.8f;
				}
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0007C190 File Offset: 0x0007A390
		public virtual void drawPlacementBounds(SpriteBatch spriteBatch, GameLocation location)
		{
			if (this.isPlaceable())
			{
				int X = Game1.getOldMouseX() + Game1.viewport.X;
				int Y = Game1.getOldMouseY() + Game1.viewport.Y;
				if (Game1.mouseCursorTransparency == 0f)
				{
					X = (int)Game1.player.GetGrabTile().X * Game1.tileSize;
					Y = (int)Game1.player.GetGrabTile().Y * Game1.tileSize;
				}
				if (Game1.player.GetGrabTile().Equals(Game1.player.getTileLocation()) && Game1.mouseCursorTransparency == 0f)
				{
					Vector2 expr_AF = Utility.getTranslatedVector2(Game1.player.GetGrabTile(), Game1.player.facingDirection, 1f);
					X = (int)expr_AF.X * Game1.tileSize;
					Y = (int)expr_AF.Y * Game1.tileSize;
				}
				bool canPlaceHere = Utility.playerCanPlaceItemHere(location, this, X, Y, Game1.player);
				spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(X / Game1.tileSize * Game1.tileSize - Game1.viewport.X), (float)(Y / Game1.tileSize * Game1.tileSize - Game1.viewport.Y)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(canPlaceHere ? 194 : 210, 388, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.01f);
				if (this.bigCraftable || this is Furniture)
				{
					this.draw(spriteBatch, X / Game1.tileSize, Y / Game1.tileSize, 0.5f);
				}
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0007C320 File Offset: 0x0007A520
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			if (this.isRecipe)
			{
				transparency = 0.5f;
				scaleSize *= 0.75f;
			}
			if (this.bigCraftable)
			{
				Microsoft.Xna.Framework.Rectangle sourceRect = Object.getSourceRectForBigCraftable(this.parentSheetIndex);
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White * transparency, 0f, new Vector2(8f, 16f), (float)Game1.pixelZoom * (((double)scaleSize < 0.2) ? scaleSize : (scaleSize / 2f)), SpriteEffects.None, layerDepth);
			}
			else
			{
				if (this.parentSheetIndex != 590)
				{
					spriteBatch.Draw(Game1.shadowTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 4)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * 0.5f, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f, SpriteEffects.None, layerDepth - 0.0001f);
				}
				spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)((int)((float)(Game1.tileSize / 2) * scaleSize)), (float)((int)((float)(Game1.tileSize / 2) * scaleSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f) * scaleSize, (float)Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);
				if (drawStackNumber && this.maximumStackSize() > 1 && (double)scaleSize > 0.3 && this.Stack != 2147483647 && this.Stack > 1)
				{
					if (!Game1.options.useCrisperNumberFont)
					{
						float drawScale = 0.5f + scaleSize;
						Game1.drawWithBorder(string.Concat(this.stack), Color.Black, Color.White, location + new Vector2((float)Game1.tileSize - Game1.tinyFont.MeasureString(string.Concat(this.stack)).X * drawScale, (float)Game1.tileSize - Game1.tinyFont.MeasureString(string.Concat(this.stack)).Y * 3f / 4f * drawScale), 0f, drawScale, 1f, true);
					}
					else
					{
						Utility.drawTinyDigits(this.stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.stack, 3f * scaleSize)) + 3f * scaleSize, (float)Game1.tileSize - 18f * scaleSize + 2f), 3f * scaleSize, 1f, Color.White);
					}
				}
				if (drawStackNumber && this.quality > 0)
				{
					float yOffset = (this.quality < 4) ? 0f : (((float)Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) + 1f) * 0.05f);
					spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(12f, (float)(Game1.tileSize - 12) + yOffset), new Microsoft.Xna.Framework.Rectangle?((this.quality < 4) ? new Microsoft.Xna.Framework.Rectangle(338 + (this.quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8)), Color.White * transparency, 0f, new Vector2(4f, 4f), 3f * scaleSize * (1f + yOffset), SpriteEffects.None, layerDepth);
				}
				if (this.category == -22 && this.scale.Y < 1f)
				{
					spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)location.X, (int)(location.Y + (float)(Game1.tileSize - 2 * Game1.pixelZoom) * scaleSize), (int)((float)Game1.tileSize * scaleSize * this.scale.Y), (int)((float)(2 * Game1.pixelZoom) * scaleSize)), Utility.getRedToGreenLerpColor(this.scale.Y));
				}
			}
			if (this.isRecipe)
			{
				spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 451, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * 3f / 4f, SpriteEffects.None, layerDepth + 0.0001f);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0007C808 File Offset: 0x0007AA08
		public void drawAsProp(SpriteBatch b)
		{
			int x = (int)this.tileLocation.X;
			int y = (int)this.tileLocation.Y;
			if (this.bigCraftable)
			{
				Vector2 scaleFactor = this.getScale();
				scaleFactor *= (float)Game1.pixelZoom;
				Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
				Microsoft.Xna.Framework.Rectangle destination = new Microsoft.Xna.Framework.Rectangle((int)(position.X - scaleFactor.X / 2f), (int)(position.Y - scaleFactor.Y / 2f), (int)((float)Game1.tileSize + scaleFactor.X), (int)((float)(Game1.tileSize * 2) + scaleFactor.Y / 2f));
				b.Draw(Game1.bigCraftableSpriteSheet, destination, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? (this.ParentSheetIndex + 1) : this.ParentSheetIndex)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f) + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
				if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
				{
					b.Draw(Game1.objectSpriteSheet, this.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), 0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, -1, -1)), Color.White, this.scale.X, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f + 0.0001f));
					return;
				}
			}
			else
			{
				if (this.parentSheetIndex != 590 && this.parentSheetIndex != 742)
				{
					b.Draw(Game1.shadowTexture, this.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 15000f);
				}
				Texture2D arg_357_1 = Game1.objectSpriteSheet;
				Vector2 arg_357_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize + Game1.tileSize / 2)));
				Microsoft.Xna.Framework.Rectangle? arg_357_3 = new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
				Color arg_357_4 = Color.White;
				float arg_357_5 = 0f;
				Vector2 arg_357_6 = new Vector2(8f, 8f);
				Vector2 arg_306_0 = this.scale;
				b.Draw(arg_357_1, arg_357_2, arg_357_3, arg_357_4, arg_357_5, arg_357_6, (this.scale.Y > 1f) ? this.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0007CB74 File Offset: 0x0007AD74
		public virtual void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (this.bigCraftable)
			{
				Vector2 scaleFactor = this.getScale();
				scaleFactor *= (float)Game1.pixelZoom;
				Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
				Microsoft.Xna.Framework.Rectangle destination = new Microsoft.Xna.Framework.Rectangle((int)(position.X - scaleFactor.X / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(position.Y - scaleFactor.Y / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)((float)Game1.tileSize + scaleFactor.X), (int)((float)(Game1.tileSize * 2) + scaleFactor.Y / 2f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destination, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? (this.ParentSheetIndex + 1) : this.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 6) / 10000f) + ((this.parentSheetIndex == 105) ? 0.0035f : 0f) + (float)x * 1E-05f);
				if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
				{
					spriteBatch.Draw(Game1.objectSpriteSheet, this.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), 0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White * alpha, this.scale.X, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize) / 10000f + 0.0001f + (float)x * 1E-05f));
				}
				if (this.isLamp && Game1.isDarkOut())
				{
					spriteBatch.Draw(Game1.mouseCursors, position + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 32, 32)), Color.White * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 5) / 10000f));
				}
				if (this.parentSheetIndex == 126 && this.quality != 0)
				{
					spriteBatch.Draw(FarmerRenderer.hatsTexture, position + new Vector2(-3f, -6f) * (float)Game1.pixelZoom, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((this.quality - 1) * 20 % FarmerRenderer.hatsTexture.Width, (this.quality - 1) * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - Game1.pixelZoom * 5) / 10000f) + (float)x * 1E-05f);
				}
			}
			else if (!Game1.eventUp || (Game1.CurrentEvent != null && !Game1.CurrentEvent.isTileWalkedOn(x, y)))
			{
				if (this.parentSheetIndex == 590)
				{
					Texture2D arg_509_1 = Game1.mouseCursors;
					Vector2 arg_509_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(y * Game1.tileSize + Game1.tileSize / 2 + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0))));
					Microsoft.Xna.Framework.Rectangle? arg_509_3 = new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(368 + ((Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1200.0 <= 400.0) ? ((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 / 100.0) * 16) : 0), 32, 16, 16));
					Color arg_509_4 = Color.White * alpha;
					float arg_509_5 = 0f;
					Vector2 arg_509_6 = new Vector2(8f, 8f);
					Vector2 arg_496_0 = this.scale;
					spriteBatch.Draw(arg_509_1, arg_509_2, arg_509_3, arg_509_4, arg_509_5, arg_509_6, (this.scale.Y > 1f) ? this.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.isPassable() ? this.getBoundingBox(new Vector2((float)x, (float)y)).Top : this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom) / 10000f);
					return;
				}
				if (this.fragility != 2)
				{
					spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize + Game1.tileSize * 4 / 5 + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 15000f);
				}
				Texture2D arg_6EE_1 = Game1.objectSpriteSheet;
				Vector2 arg_6EE_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(y * Game1.tileSize + Game1.tileSize / 2 + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0))));
				Microsoft.Xna.Framework.Rectangle? arg_6EE_3 = new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
				Color arg_6EE_4 = Color.White * alpha;
				float arg_6EE_5 = 0f;
				Vector2 arg_6EE_6 = new Vector2(8f, 8f);
				Vector2 arg_67B_0 = this.scale;
				spriteBatch.Draw(arg_6EE_1, arg_6EE_2, arg_6EE_3, arg_6EE_4, arg_6EE_5, arg_6EE_6, (this.scale.Y > 1f) ? this.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float)(this.isPassable() ? this.getBoundingBox(new Vector2((float)x, (float)y)).Top : this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom) / 10000f);
			}
			if (this.readyForHarvest)
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize - 8), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f + 1E-06f + this.tileLocation.X / 10000f + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
				spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.heldObject.parentSheetIndex, 16, 16)), Color.White * 0.75f, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f + 1E-05f + this.tileLocation.X / 10000f + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0007D430 File Offset: 0x0007B630
		public virtual void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
		{
			if (this.bigCraftable)
			{
				Vector2 scaleFactor = this.getScale();
				scaleFactor *= (float)Game1.pixelZoom;
				Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)xNonTile, (float)yNonTile));
				Microsoft.Xna.Framework.Rectangle destination = new Microsoft.Xna.Framework.Rectangle((int)(position.X - scaleFactor.X / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(position.Y - scaleFactor.Y / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)((float)Game1.tileSize + scaleFactor.X), (int)((float)(Game1.tileSize * 2) + scaleFactor.Y / 2f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destination, new Microsoft.Xna.Framework.Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? (this.ParentSheetIndex + 1) : this.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
				if (this.Name.Equals("Loom") && this.minutesUntilReady > 0)
				{
					spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(position) + new Vector2((float)(Game1.tileSize / 2), 0f), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 435, 16, 16)), Color.White * alpha, this.scale.X, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
				}
				if (this.isLamp && Game1.isDarkOut())
				{
					spriteBatch.Draw(Game1.mouseCursors, position + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 32, 32)), Color.White * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
					return;
				}
			}
			else if (!Game1.eventUp || !Game1.CurrentEvent.isTileWalkedOn(xNonTile / Game1.tileSize, yNonTile / Game1.tileSize))
			{
				if (this.parentSheetIndex != 590 && this.fragility != 2)
				{
					spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + Game1.tileSize / 2), (float)(yNonTile + Game1.tileSize * 4 / 5 + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, layerDepth);
				}
				Texture2D arg_39B_1 = Game1.objectSpriteSheet;
				Vector2 arg_39B_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + Game1.tileSize / 2 + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(yNonTile + Game1.tileSize / 2 + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0))));
				Microsoft.Xna.Framework.Rectangle? arg_39B_3 = new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex));
				Color arg_39B_4 = Color.White * alpha;
				float arg_39B_5 = 0f;
				Vector2 arg_39B_6 = new Vector2(8f, 8f);
				Vector2 arg_367_0 = this.scale;
				spriteBatch.Draw(arg_39B_1, arg_39B_2, arg_39B_3, arg_39B_4, arg_39B_5, arg_39B_6, (this.scale.Y > 1f) ? this.getScale().Y : ((float)Game1.pixelZoom), this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0007D7E0 File Offset: 0x0007B9E0
		private int getMinutesForCrystalarium(int whichGem)
		{
			switch (whichGem)
			{
			case 60:
				return 3000;
			case 61:
			case 63:
			case 65:
			case 67:
			case 69:
			case 71:
				break;
			case 62:
				return 2240;
			case 64:
				return 3000;
			case 66:
				return 1360;
			case 68:
				return 1120;
			case 70:
				return 2400;
			case 72:
				return 7200;
			default:
				switch (whichGem)
				{
				case 80:
					return 420;
				case 82:
					return 1300;
				case 84:
					return 1120;
				case 86:
					return 800;
				}
				break;
			}
			return 5000;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0007D898 File Offset: 0x0007BA98
		public override int maximumStackSize()
		{
			if (this.category == -22)
			{
				return 1;
			}
			if (this.bigCraftable)
			{
				return -1;
			}
			return 999;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0007D8B5 File Offset: 0x0007BAB5
		public override int getStack()
		{
			return this.stack;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0007D8C0 File Offset: 0x0007BAC0
		public override int addToStack(int amount)
		{
			int maxStack = this.maximumStackSize();
			if (maxStack == 1)
			{
				return amount;
			}
			this.stack += amount;
			if (this.stack > maxStack)
			{
				int arg_31_0 = this.stack - maxStack;
				this.stack = maxStack;
				return arg_31_0;
			}
			return 0;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void hoverAction()
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool clicked(Farmer who)
		{
			return false;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0007D904 File Offset: 0x0007BB04
		public override Item getOne()
		{
			if (!this.bigCraftable)
			{
				return new Object(this.tileLocation, this.parentSheetIndex, 1)
				{
					scale = this.scale,
					quality = this.quality,
					isSpawnedObject = this.isSpawnedObject,
					isRecipe = this.isRecipe,
					questItem = this.questItem,
					stack = 1,
					name = this.name,
					specialVariable = this.specialVariable,
					price = this.price
				};
			}
			return new Object(this.tileLocation, this.parentSheetIndex, false)
			{
				isRecipe = this.isRecipe
			};
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0007D9B4 File Offset: 0x0007BBB4
		public override bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			return (this.parentSheetIndex == 710 && l.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Water", "Back") != null && !l.objects.ContainsKey(tile) && l.doesTileHaveProperty((int)tile.X + 1, (int)tile.Y, "Water", "Back") != null && l.doesTileHaveProperty((int)tile.X - 1, (int)tile.Y, "Water", "Back") != null) || (l.doesTileHaveProperty((int)tile.X, (int)tile.Y + 1, "Water", "Back") != null && l.doesTileHaveProperty((int)tile.X, (int)tile.Y - 1, "Water", "Back") != null) || (this.parentSheetIndex == 105 && this.bigCraftable && l.terrainFeatures.ContainsKey(tile) && l.terrainFeatures[tile] is Tree && !l.objects.ContainsKey(tile)) || (this.name != null && this.name.Contains("Bomb") && (!l.isTileOccupiedForPlacement(tile, this) || l.isTileOccupiedByFarmer(tile) != null)) || !l.isTileOccupiedForPlacement(tile, this);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0007DB04 File Offset: 0x0007BD04
		public override bool isPlaceable()
		{
			int arg_06_0 = this.category;
			return this.type != null && (this.category == -8 || this.category == -9 || this.type.Equals("Crafting") || this.name.Contains("Sapling") || this.parentSheetIndex == 710) && this.edibility < 0;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0007DB74 File Offset: 0x0007BD74
		public virtual bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			Vector2 placementTile = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			this.health = 10;
			if (who != null)
			{
				this.owner = who.uniqueMultiplayerID;
			}
			else
			{
				this.owner = Game1.player.uniqueMultiplayerID;
			}
			if (!this.bigCraftable && !(this is Furniture))
			{
				int num = this.ParentSheetIndex;
				if (num <= 298)
				{
					if (num > 94)
					{
						bool result;
						switch (num)
						{
						case 286:
						{
							using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.currentLocation.temporarySprites.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (enumerator.Current.position.Equals(placementTile * (float)Game1.tileSize))
									{
										result = false;
										return result;
									}
								}
							}
							int idNum = Game1.random.Next();
							Game1.playSound("thudStep");
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 100f, 1, 24, placementTile * (float)Game1.tileSize, true, false, Game1.currentLocation, who)
							{
								shakeIntensity = 0.5f,
								shakeIntensityChange = 0.002f,
								extraInfoForEndBehavior = idNum,
								endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 3f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = (float)idNum
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 3f) * (float)Game1.pixelZoom, true, true, (float)(y + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								delayBeforeAnimationStart = 100,
								id = (float)idNum
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 3f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
							{
								delayBeforeAnimationStart = 200,
								id = (float)idNum
							});
							if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
							{
								Game1.fuseSound = Game1.soundBank.GetCue("fuse");
								Game1.fuseSound.Play();
							}
							return true;
						}
						case 287:
						{
							using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.currentLocation.temporarySprites.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (enumerator.Current.position.Equals(placementTile * (float)Game1.tileSize))
									{
										result = false;
										return result;
									}
								}
							}
							int idNum = Game1.random.Next();
							Game1.playSound("thudStep");
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 100f, 1, 24, placementTile * (float)Game1.tileSize, true, false, Game1.currentLocation, who)
							{
								shakeIntensity = 0.5f,
								shakeIntensityChange = 0.002f,
								extraInfoForEndBehavior = idNum,
								endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize, true, false, (float)(y + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = (float)idNum
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize, true, false, (float)(y + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								delayBeforeAnimationStart = 100,
								id = (float)idNum
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize, true, false, (float)(y + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
							{
								delayBeforeAnimationStart = 200,
								id = (float)idNum
							});
							if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
							{
								Game1.fuseSound = Game1.soundBank.GetCue("fuse");
								Game1.fuseSound.Play();
							}
							return true;
						}
						case 288:
						{
							using (List<TemporaryAnimatedSprite>.Enumerator enumerator = Game1.currentLocation.temporarySprites.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (enumerator.Current.position.Equals(placementTile * (float)Game1.tileSize))
									{
										result = false;
										return result;
									}
								}
							}
							int idNum = Game1.random.Next();
							Game1.playSound("thudStep");
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(this.parentSheetIndex, 100f, 1, 24, placementTile * (float)Game1.tileSize, true, false, Game1.currentLocation, who)
							{
								shakeIntensity = 0.5f,
								shakeIntensityChange = 0.002f,
								extraInfoForEndBehavior = idNum,
								endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = (float)idNum
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, true, (float)(y + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								delayBeforeAnimationStart = 100,
								id = (float)idNum
							});
							Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(y + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
							{
								delayBeforeAnimationStart = 200,
								id = (float)idNum
							});
							if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
							{
								Game1.fuseSound = Game1.soundBank.GetCue("fuse");
								Game1.fuseSound.Play();
							}
							return true;
						}
						default:
							if (num != 297)
							{
								if (num != 298)
								{
									goto IL_104A;
								}
								if (location.objects.ContainsKey(placementTile))
								{
									return false;
								}
								location.objects.Add(placementTile, new Fence(placementTile, 5, false));
								Game1.playSound("axe");
								return true;
							}
							else
							{
								if (location.objects.ContainsKey(placementTile) || location.terrainFeatures.ContainsKey(placementTile))
								{
									return false;
								}
								location.terrainFeatures.Add(placementTile, new Grass(1, 4));
								Game1.playSound("dirtyHit");
								return true;
							}
							break;
						}
						return result;
					}
					if (num != 93)
					{
						if (num == 94)
						{
							if (location.objects.ContainsKey(placementTile))
							{
								return false;
							}
							new Torch(placementTile, 1, 94).placementAction(location, x, y, who);
							return true;
						}
					}
					else
					{
						if (location.objects.ContainsKey(placementTile))
						{
							return false;
						}
						Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
						Utility.removeLightSource((int)Game1.player.uniqueMultiplayerID);
						new Torch(placementTile, 1).placementAction(location, x, y, (who == null) ? Game1.player : who);
						return true;
					}
				}
				else if (num <= 401)
				{
					switch (num)
					{
					case 309:
					case 310:
					case 311:
					{
						bool isLocationOpenHoeDirt = location.terrainFeatures.ContainsKey(placementTile) && location.terrainFeatures[placementTile] is HoeDirt && (location.terrainFeatures[placementTile] as HoeDirt).crop == null;
						string noSpawn = location.doesTileHaveProperty((int)placementTile.X, (int)placementTile.Y, "NoSpawn", "Back");
						if (!isLocationOpenHoeDirt && (location.objects.ContainsKey(placementTile) || location.terrainFeatures.ContainsKey(placementTile) || (!(location is Farm) && !location.name.Contains("Greenhouse")) || (noSpawn != null && (noSpawn.Equals("Tree") || noSpawn.Equals("All") || noSpawn.Equals("True")))))
						{
							Game1.showRedMessage("Invalid Position");
							return false;
						}
						if (noSpawn != null && (noSpawn.Equals("Tree") || noSpawn.Equals("All") || noSpawn.Equals("True")))
						{
							return false;
						}
						if (isLocationOpenHoeDirt || (location.isTileLocationOpen(new Location(x * Game1.tileSize, y * Game1.tileSize)) && !location.isTileOccupied(new Vector2((float)x, (float)y), "") && location.doesTileHaveProperty(x, y, "Water", "Back") == null))
						{
							int whichTree = 1;
							num = this.parentSheetIndex;
							if (num != 310)
							{
								if (num == 311)
								{
									whichTree = 3;
								}
							}
							else
							{
								whichTree = 2;
							}
							location.terrainFeatures.Remove(placementTile);
							location.terrainFeatures.Add(placementTile, new Tree(whichTree, 0));
							Game1.playSound("dirtyHit");
							return true;
						}
						break;
					}
					default:
						switch (num)
						{
						case 322:
							if (location.objects.ContainsKey(placementTile))
							{
								return false;
							}
							location.objects.Add(placementTile, new Fence(placementTile, 1, false));
							Game1.playSound("axe");
							return true;
						case 323:
							if (location.objects.ContainsKey(placementTile))
							{
								return false;
							}
							location.objects.Add(placementTile, new Fence(placementTile, 2, false));
							Game1.playSound("stoneStep");
							return true;
						case 324:
							if (location.objects.ContainsKey(placementTile))
							{
								return false;
							}
							location.objects.Add(placementTile, new Fence(placementTile, 3, false));
							Game1.playSound("hammer");
							return true;
						case 325:
							if (location.objects.ContainsKey(placementTile))
							{
								return false;
							}
							location.objects.Add(placementTile, new Fence(placementTile, 4, true));
							Game1.playSound("axe");
							return true;
						case 326:
						case 327:
						case 330:
						case 332:
							break;
						case 328:
							if (location.terrainFeatures.ContainsKey(placementTile))
							{
								return false;
							}
							location.terrainFeatures.Add(placementTile, new Flooring(0));
							Game1.playSound("axchop");
							return true;
						case 329:
							if (location.terrainFeatures.ContainsKey(placementTile))
							{
								return false;
							}
							location.terrainFeatures.Add(placementTile, new Flooring(1));
							Game1.playSound("thudStep");
							return true;
						case 331:
							if (location.terrainFeatures.ContainsKey(placementTile))
							{
								return false;
							}
							location.terrainFeatures.Add(placementTile, new Flooring(2));
							Game1.playSound("axchop");
							return true;
						case 333:
							if (location.terrainFeatures.ContainsKey(placementTile))
							{
								return false;
							}
							location.terrainFeatures.Add(placementTile, new Flooring(3));
							Game1.playSound("thudStep");
							return true;
						default:
							if (num == 401)
							{
								if (location.terrainFeatures.ContainsKey(placementTile))
								{
									return false;
								}
								location.terrainFeatures.Add(placementTile, new Flooring(4));
								Game1.playSound("thudStep");
								return true;
							}
							break;
						}
						break;
					}
				}
				else
				{
					switch (num)
					{
					case 405:
						if (location.terrainFeatures.ContainsKey(placementTile))
						{
							return false;
						}
						location.terrainFeatures.Add(placementTile, new Flooring(6));
						Game1.playSound("woodyStep");
						return true;
					case 406:
					case 408:
					case 410:
						break;
					case 407:
						if (location.terrainFeatures.ContainsKey(placementTile))
						{
							return false;
						}
						location.terrainFeatures.Add(placementTile, new Flooring(5));
						Game1.playSound("dirtyHit");
						return true;
					case 409:
						if (location.terrainFeatures.ContainsKey(placementTile))
						{
							return false;
						}
						location.terrainFeatures.Add(placementTile, new Flooring(7));
						Game1.playSound("stoneStep");
						return true;
					case 411:
						if (location.terrainFeatures.ContainsKey(placementTile))
						{
							return false;
						}
						location.terrainFeatures.Add(placementTile, new Flooring(8));
						Game1.playSound("stoneStep");
						return true;
					default:
						if (num != 415)
						{
							if (num == 710)
							{
								if (location.objects.ContainsKey(placementTile) || location.doesTileHaveProperty((int)placementTile.X, (int)placementTile.Y, "Water", "Back") == null || location.doesTileHaveProperty((int)placementTile.X, (int)placementTile.Y, "Passable", "Buildings") != null)
								{
									return false;
								}
								new CrabPot(placementTile, 1).placementAction(location, x, y, who);
								return true;
							}
						}
						else
						{
							if (location.terrainFeatures.ContainsKey(placementTile))
							{
								return false;
							}
							location.terrainFeatures.Add(placementTile, new Flooring(9));
							Game1.playSound("stoneStep");
							return true;
						}
						break;
					}
				}
			}
			else
			{
				int num = this.ParentSheetIndex;
				if (num <= 130)
				{
					if (num == 71)
					{
						if (location is MineShaft)
						{
							if ((location as MineShaft).mineLevel != 120 && (location as MineShaft).recursiveTryToCreateLadderDown(placementTile, "hoeHit", 16))
							{
								return true;
							}
							Game1.showRedMessage("Unsuitable Location");
						}
						return false;
					}
					if (num == 130)
					{
						if (location.objects.ContainsKey(placementTile) || Game1.currentLocation is MineShaft)
						{
							Game1.showRedMessage("Unsuitable Location");
							return false;
						}
						location.objects.Add(placementTile, new Chest(true)
						{
							shakeTimer = 50
						});
						Game1.playSound("axe");
						return true;
					}
				}
				else
				{
					switch (num)
					{
					case 143:
					case 144:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 150:
					case 151:
						if (location.objects.ContainsKey(placementTile))
						{
							return false;
						}
						new Torch(placementTile, this.parentSheetIndex, true)
						{
							shakeTimer = 25
						}.placementAction(location, x, y, who);
						return true;
					default:
						if (num == 163)
						{
							location.objects.Add(placementTile, new Cask(placementTile));
							Game1.playSound("hammer");
						}
						break;
					}
				}
			}
			IL_104A:
			if (this.name.Equals("Tapper"))
			{
				if (location.terrainFeatures.ContainsKey(placementTile) && location.terrainFeatures[placementTile] is Tree && (location.terrainFeatures[placementTile] as Tree).growthStage >= 5 && !(location.terrainFeatures[placementTile] as Tree).stump && !location.objects.ContainsKey(placementTile))
				{
					this.tileLocation = placementTile;
					location.objects.Add(placementTile, this);
					int treeType = (location.terrainFeatures[placementTile] as Tree).treeType;
					(location.terrainFeatures[placementTile] as Tree).tapped = true;
					switch (treeType)
					{
					case 1:
						this.heldObject = new Object(725, 1, false, -1, 0);
						this.minutesUntilReady = 13000 - Game1.timeOfDay;
						break;
					case 2:
						this.heldObject = new Object(724, 1, false, -1, 0);
						this.minutesUntilReady = 16000 - Game1.timeOfDay;
						break;
					case 3:
						this.heldObject = new Object(726, 1, false, -1, 0);
						this.minutesUntilReady = 10000 - Game1.timeOfDay;
						break;
					case 7:
						this.heldObject = new Object(420, 1, false, -1, 0);
						this.minutesUntilReady = 3000 - Game1.timeOfDay;
						if (!Game1.currentSeason.Equals("fall"))
						{
							this.heldObject = new Object(404, 1, false, -1, 0);
							this.minutesUntilReady = 6000 - Game1.timeOfDay;
						}
						break;
					}
					Game1.playSound("axe");
					return true;
				}
				return false;
			}
			else if (this.name.Contains("Sapling"))
			{
				Vector2 v = default(Vector2);
				for (int i = x / Game1.tileSize - 2; i <= x / Game1.tileSize + 2; i++)
				{
					for (int j = y / Game1.tileSize - 2; j <= y / Game1.tileSize + 2; j++)
					{
						v.X = (float)i;
						v.Y = (float)j;
						if (location.terrainFeatures.ContainsKey(v) && (location.terrainFeatures[v] is Tree || location.terrainFeatures[v] is FruitTree))
						{
							Game1.showRedMessage("Too close to another tree");
							return false;
						}
					}
				}
				if (location.terrainFeatures.ContainsKey(placementTile))
				{
					if (!(location.terrainFeatures[placementTile] is HoeDirt) || (location.terrainFeatures[placementTile] as HoeDirt).crop != null)
					{
						return false;
					}
					location.terrainFeatures.Remove(placementTile);
				}
				if ((location is Farm && (location.doesTileHaveProperty((int)placementTile.X, (int)placementTile.Y, "Diggable", "Back") != null || location.doesTileHavePropertyNoNull((int)placementTile.X, (int)placementTile.Y, "Type", "Back").Equals("Grass")) && !location.doesTileHavePropertyNoNull((int)placementTile.X, (int)placementTile.Y, "NoSpawn", "Back").Equals("Tree")) || (location.name.Equals("Greenhouse") && (location.doesTileHaveProperty((int)placementTile.X, (int)placementTile.Y, "Diggable", "Back") != null || location.doesTileHavePropertyNoNull((int)placementTile.X, (int)placementTile.Y, "Type", "Back").Equals("Stone"))))
				{
					Game1.playSound("dirtyHit");
					DelayedAction.playSoundAfterDelay("coin", 100);
					location.terrainFeatures.Add(placementTile, new FruitTree(this.parentSheetIndex)
					{
						greenHouseTree = location.name.Equals("Greenhouse"),
						greenHouseTileTree = location.doesTileHavePropertyNoNull((int)placementTile.X, (int)placementTile.Y, "Type", "Back").Equals("Stone")
					});
					return true;
				}
				Game1.showRedMessage("Can't be planted here.");
				return false;
			}
			else
			{
				if (this.category == -74)
				{
					return true;
				}
				if (!this.performDropDownAction(who))
				{
					Object toPlace = (Object)this.getOne();
					toPlace.shakeTimer = 50;
					toPlace.tileLocation = placementTile;
					toPlace.performDropDownAction(who);
					if (location.objects.ContainsKey(placementTile))
					{
						if (location.objects[placementTile].ParentSheetIndex != this.parentSheetIndex)
						{
							Game1.createItemDebris(location.objects[placementTile], placementTile * (float)Game1.tileSize, Game1.random.Next(4), null);
							location.objects[placementTile] = toPlace;
						}
					}
					else if (toPlace is Furniture)
					{
						(location as DecoratableLocation).furniture.Add(this as Furniture);
					}
					else
					{
						location.objects.Add(placementTile, toPlace);
					}
					toPlace.initializeLightSource(placementTile);
				}
				Game1.playSound("woodyStep");
				return true;
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0007F10C File Offset: 0x0007D30C
		public override bool actionWhenPurchased()
		{
			if (this.type != null && this.type.Contains("Blueprint"))
			{
				string blueprintname = this.name.Substring(this.name.IndexOf(' ') + 1);
				if (!Game1.player.blueprints.Contains(this.name))
				{
					Game1.player.blueprints.Add(blueprintname);
				}
				return true;
			}
			if (this.parentSheetIndex == 434)
			{
				if (!Game1.isFestival())
				{
					Game1.player.mailReceived.Add("CF_Sewer");
				}
				else
				{
					Game1.player.mailReceived.Add("CF_Fair");
				}
				Game1.exitActiveMenu();
				Game1.playerEatObject(this, true);
			}
			return this.isRecipe;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0007F1C7 File Offset: 0x0007D3C7
		public override bool canBePlacedInWater()
		{
			return this.parentSheetIndex == 710;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0007F1D8 File Offset: 0x0007D3D8
		public override string getDescription()
		{
			if (this.isRecipe)
			{
				if (this.category == -7)
				{
					return "A recipe to make " + this.name;
				}
				return "Blueprints for crafting " + Game1.getProperArticleForWord(this.name) + " " + this.name;
			}
			else
			{
				if (!this.bigCraftable && this.type != null && (this.type.Equals("Minerals") || this.type.Equals("Arch")) && !(Game1.getLocationFromName("ArchaeologyHouse") as LibraryMuseum).museumAlreadyHasArtifact(this.parentSheetIndex))
				{
					return Game1.parseText("Gunther can tell you more about this if you donate it to the museum.", Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
				}
				return Game1.parseText(this.bigCraftable ? Game1.bigCraftablesInformation[this.parentSheetIndex].Split(new char[]
				{
					'/'
				})[4] : (Game1.objectInformation.ContainsKey(this.parentSheetIndex) ? Game1.objectInformation[this.parentSheetIndex].Split(new char[]
				{
					'/'
				})[4] : "???"), Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0007F318 File Offset: 0x0007D518
		public int sellToStorePrice()
		{
			if (this is Fence)
			{
				return this.price;
			}
			if (this.category == -22)
			{
				return (int)((float)this.price * (1f + (float)this.quality * 0.25f) * this.scale.Y);
			}
			float salePrice = (float)((int)((float)this.price * (1f + (float)this.quality * 0.25f)));
			bool animalGood = false;
			if (this.name.ToLower().Contains("mayonnaise") || this.name.ToLower().Contains("cheese") || this.name.ToLower().Contains("cloth") || this.name.ToLower().Contains("wool"))
			{
				animalGood = true;
			}
			if (Game1.player.professions.Contains(0) && (animalGood || this.category == -5 || this.category == -6 || this.category == -18))
			{
				salePrice *= 1.2f;
			}
			if (Game1.player.professions.Contains(1) && (this.category == -75 || this.category == -80 || (this.category == -79 && !this.isSpawnedObject)))
			{
				salePrice *= 1.1f;
			}
			if (Game1.player.professions.Contains(4) && this.category == -26)
			{
				salePrice *= 1.4f;
			}
			if (Game1.player.professions.Contains(6) && this.category == -4)
			{
				salePrice *= (Game1.player.professions.Contains(8) ? 1.5f : 1.25f);
			}
			if (Game1.player.professions.Contains(12) && (this.parentSheetIndex == 388 || this.parentSheetIndex == 709))
			{
				salePrice *= 1.5f;
			}
			if (Game1.player.professions.Contains(15) && this.category == -27)
			{
				salePrice *= 1.25f;
			}
			if (Game1.player.professions.Contains(20) && this.parentSheetIndex >= 334 && this.parentSheetIndex <= 337)
			{
				salePrice *= 1.5f;
			}
			if (Game1.player.professions.Contains(23) && (this.category == -2 || this.category == -12))
			{
				salePrice *= 1.3f;
			}
			if (this.parentSheetIndex == 493)
			{
				salePrice /= 2f;
			}
			return (int)salePrice;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0007F598 File Offset: 0x0007D798
		public override int salePrice()
		{
			if (this is Fence)
			{
				return this.price;
			}
			if (this.isRecipe)
			{
				return this.price * 10;
			}
			int num = this.parentSheetIndex;
			switch (num)
			{
			case 378:
				return 50;
			case 379:
			case 381:
			case 383:
				break;
			case 380:
				return 100;
			case 382:
				return 100;
			case 384:
				return 300;
			default:
				if (num == 388)
				{
					return 10;
				}
				if (num == 390)
				{
					return 20;
				}
				break;
			}
			return (int)((float)(this.price * 2) * (1f + (float)this.quality * 0.25f));
		}

		// Token: 0x040005BE RID: 1470
		public const int copperBar = 334;

		// Token: 0x040005BF RID: 1471
		public const int ironBar = 335;

		// Token: 0x040005C0 RID: 1472
		public const int goldBar = 336;

		// Token: 0x040005C1 RID: 1473
		public const int iridiumBar = 337;

		// Token: 0x040005C2 RID: 1474
		public const int wood = 388;

		// Token: 0x040005C3 RID: 1475
		public const int stone = 390;

		// Token: 0x040005C4 RID: 1476
		public const int copper = 378;

		// Token: 0x040005C5 RID: 1477
		public const int iron = 380;

		// Token: 0x040005C6 RID: 1478
		public const int coal = 382;

		// Token: 0x040005C7 RID: 1479
		public const int gold = 384;

		// Token: 0x040005C8 RID: 1480
		public const int iridium = 386;

		// Token: 0x040005C9 RID: 1481
		public const int inedible = -300;

		// Token: 0x040005CA RID: 1482
		public const int GreensCategory = -81;

		// Token: 0x040005CB RID: 1483
		public const int GemCategory = -2;

		// Token: 0x040005CC RID: 1484
		public const int VegetableCategory = -75;

		// Token: 0x040005CD RID: 1485
		public const int FishCategory = -4;

		// Token: 0x040005CE RID: 1486
		public const int EggCategory = -5;

		// Token: 0x040005CF RID: 1487
		public const int MilkCategory = -6;

		// Token: 0x040005D0 RID: 1488
		public const int CookingCategory = -7;

		// Token: 0x040005D1 RID: 1489
		public const int CraftingCategory = -8;

		// Token: 0x040005D2 RID: 1490
		public const int BigCraftableCategory = -9;

		// Token: 0x040005D3 RID: 1491
		public const int FruitsCategory = -79;

		// Token: 0x040005D4 RID: 1492
		public const int SeedsCategory = -74;

		// Token: 0x040005D5 RID: 1493
		public const int mineralsCategory = -12;

		// Token: 0x040005D6 RID: 1494
		public const int flowersCategory = -80;

		// Token: 0x040005D7 RID: 1495
		public const int meatCategory = -14;

		// Token: 0x040005D8 RID: 1496
		public const int metalResources = -15;

		// Token: 0x040005D9 RID: 1497
		public const int buildingResources = -16;

		// Token: 0x040005DA RID: 1498
		public const int sellAtPierres = -17;

		// Token: 0x040005DB RID: 1499
		public const int sellAtPierresAndMarnies = -18;

		// Token: 0x040005DC RID: 1500
		public const int fertilizerCategory = -19;

		// Token: 0x040005DD RID: 1501
		public const int junkCategory = -20;

		// Token: 0x040005DE RID: 1502
		public const int baitCategory = -21;

		// Token: 0x040005DF RID: 1503
		public const int tackleCategory = -22;

		// Token: 0x040005E0 RID: 1504
		public const int sellAtFishShopCategory = -23;

		// Token: 0x040005E1 RID: 1505
		public const int furnitureCategory = -24;

		// Token: 0x040005E2 RID: 1506
		public const int ingredientsCategory = -25;

		// Token: 0x040005E3 RID: 1507
		public const int artisanGoodsCategory = -26;

		// Token: 0x040005E4 RID: 1508
		public const int syrupCategory = -27;

		// Token: 0x040005E5 RID: 1509
		public const int monsterLootCategory = -28;

		// Token: 0x040005E6 RID: 1510
		public const int equipmentCategory = -29;

		// Token: 0x040005E7 RID: 1511
		public const int hatCategory = -95;

		// Token: 0x040005E8 RID: 1512
		public const int ringCategory = -96;

		// Token: 0x040005E9 RID: 1513
		public const int weaponCategory = -98;

		// Token: 0x040005EA RID: 1514
		public const int bootsCategory = -97;

		// Token: 0x040005EB RID: 1515
		public const int toolCategory = -99;

		// Token: 0x040005EC RID: 1516
		public const int objectInfoNameIndex = 0;

		// Token: 0x040005ED RID: 1517
		public const int objectInfoPriceIndex = 1;

		// Token: 0x040005EE RID: 1518
		public const int objectInfoEdibilityIndex = 2;

		// Token: 0x040005EF RID: 1519
		public const int objectInfoDescriptionIndex = 4;

		// Token: 0x040005F0 RID: 1520
		public const int objectTypeIndex = 3;

		// Token: 0x040005F1 RID: 1521
		public const int archInfoIndex = 5;

		// Token: 0x040005F2 RID: 1522
		public const int WeedsIndex = 0;

		// Token: 0x040005F3 RID: 1523
		public const int StoneIndex = 2;

		// Token: 0x040005F4 RID: 1524
		public const int StickIndex = 4;

		// Token: 0x040005F5 RID: 1525
		public const int DryDirtTileIndex = 6;

		// Token: 0x040005F6 RID: 1526
		public const int WateredTileIndex = 7;

		// Token: 0x040005F7 RID: 1527
		public const int StumpTopLeftIndex = 8;

		// Token: 0x040005F8 RID: 1528
		public const int BoulderTopLeftIndex = 10;

		// Token: 0x040005F9 RID: 1529
		public const int StumpBottomLeftIndex = 12;

		// Token: 0x040005FA RID: 1530
		public const int BoulderBottomLeftIndex = 14;

		// Token: 0x040005FB RID: 1531
		public const int WildHorseradishIndex = 16;

		// Token: 0x040005FC RID: 1532
		public const int TulipIndex = 18;

		// Token: 0x040005FD RID: 1533
		public const int LeekIndex = 20;

		// Token: 0x040005FE RID: 1534
		public const int DandelionIndex = 22;

		// Token: 0x040005FF RID: 1535
		public const int ParsnipIndex = 24;

		// Token: 0x04000600 RID: 1536
		public const int HandCursorIndex = 26;

		// Token: 0x04000601 RID: 1537
		public const int WaterAnimationIndex = 28;

		// Token: 0x04000602 RID: 1538
		public const int LumberIndex = 30;

		// Token: 0x04000603 RID: 1539
		public const int mineStoneGrey1Index = 32;

		// Token: 0x04000604 RID: 1540
		public const int mineStoneBlue1Index = 34;

		// Token: 0x04000605 RID: 1541
		public const int mineStoneBlue2Index = 36;

		// Token: 0x04000606 RID: 1542
		public const int mineStoneGrey2Index = 38;

		// Token: 0x04000607 RID: 1543
		public const int mineStoneBrown1Index = 40;

		// Token: 0x04000608 RID: 1544
		public const int mineStoneBrown2Index = 42;

		// Token: 0x04000609 RID: 1545
		public const int mineStonePurpleIndex = 44;

		// Token: 0x0400060A RID: 1546
		public const int mineStoneMysticIndex = 46;

		// Token: 0x0400060B RID: 1547
		public const int mineStoneSnow1 = 48;

		// Token: 0x0400060C RID: 1548
		public const int mineStoneSnow2 = 50;

		// Token: 0x0400060D RID: 1549
		public const int mineStoneSnow3 = 52;

		// Token: 0x0400060E RID: 1550
		public const int mineStonePurpleSnowIndex = 54;

		// Token: 0x0400060F RID: 1551
		public const int mineStoneRed1Index = 56;

		// Token: 0x04000610 RID: 1552
		public const int mineStoneRed2Index = 58;

		// Token: 0x04000611 RID: 1553
		public const int emeraldIndex = 60;

		// Token: 0x04000612 RID: 1554
		public const int aquamarineIndex = 62;

		// Token: 0x04000613 RID: 1555
		public const int rubyIndex = 64;

		// Token: 0x04000614 RID: 1556
		public const int amethystClusterIndex = 66;

		// Token: 0x04000615 RID: 1557
		public const int topazIndex = 68;

		// Token: 0x04000616 RID: 1558
		public const int sapphireIndex = 70;

		// Token: 0x04000617 RID: 1559
		public const int diamondIndex = 72;

		// Token: 0x04000618 RID: 1560
		public const int prismaticShardIndex = 74;

		// Token: 0x04000619 RID: 1561
		public const int snowHoedDirtIndex = 76;

		// Token: 0x0400061A RID: 1562
		public const int beachHoedDirtIndex = 77;

		// Token: 0x0400061B RID: 1563
		public const int caveCarrotIndex = 78;

		// Token: 0x0400061C RID: 1564
		public const int quartzIndex = 80;

		// Token: 0x0400061D RID: 1565
		public const int bobberIndex = 133;

		// Token: 0x0400061E RID: 1566
		public const int stardrop = 434;

		// Token: 0x0400061F RID: 1567
		public const int spriteSheetTileSize = 16;

		// Token: 0x04000620 RID: 1568
		public const int lowQuality = 0;

		// Token: 0x04000621 RID: 1569
		public const int medQuality = 1;

		// Token: 0x04000622 RID: 1570
		public const int highQuality = 2;

		// Token: 0x04000623 RID: 1571
		public const int bestQuality = 4;

		// Token: 0x04000624 RID: 1572
		public const int copperPerBar = 10;

		// Token: 0x04000625 RID: 1573
		public const int ironPerBar = 10;

		// Token: 0x04000626 RID: 1574
		public const int goldPerBar = 10;

		// Token: 0x04000627 RID: 1575
		public const int iridiumPerBar = 10;

		// Token: 0x04000628 RID: 1576
		public const float wobbleAmountWhenWorking = 10f;

		// Token: 0x04000629 RID: 1577
		public const int fragility_Removable = 0;

		// Token: 0x0400062A RID: 1578
		public const int fragility_Delicate = 1;

		// Token: 0x0400062B RID: 1579
		public const int fragility_Indestructable = 2;

		// Token: 0x0400062C RID: 1580
		public Vector2 tileLocation;

		// Token: 0x0400062D RID: 1581
		public new int parentSheetIndex;

		// Token: 0x0400062E RID: 1582
		public long owner;

		// Token: 0x0400062F RID: 1583
		public string name;

		// Token: 0x04000630 RID: 1584
		public string type;

		// Token: 0x04000631 RID: 1585
		public bool canBeSetDown;

		// Token: 0x04000632 RID: 1586
		public bool canBeGrabbed = true;

		// Token: 0x04000633 RID: 1587
		public bool isHoedirt;

		// Token: 0x04000634 RID: 1588
		public bool isSpawnedObject;

		// Token: 0x04000635 RID: 1589
		public bool questItem;

		// Token: 0x04000636 RID: 1590
		public bool isOn = true;

		// Token: 0x04000637 RID: 1591
		public int fragility;

		// Token: 0x04000638 RID: 1592
		private bool isActive;

		// Token: 0x04000639 RID: 1593
		public int price;

		// Token: 0x0400063A RID: 1594
		public int edibility = -300;

		// Token: 0x0400063B RID: 1595
		public int stack = 1;

		// Token: 0x0400063C RID: 1596
		public int quality;

		// Token: 0x0400063D RID: 1597
		public bool bigCraftable;

		// Token: 0x0400063E RID: 1598
		public bool setOutdoors;

		// Token: 0x0400063F RID: 1599
		public bool setIndoors;

		// Token: 0x04000640 RID: 1600
		public bool readyForHarvest;

		// Token: 0x04000641 RID: 1601
		public bool showNextIndex;

		// Token: 0x04000642 RID: 1602
		public bool flipped;

		// Token: 0x04000643 RID: 1603
		public bool hasBeenPickedUpByFarmer;

		// Token: 0x04000644 RID: 1604
		public bool isRecipe;

		// Token: 0x04000645 RID: 1605
		public bool isLamp;

		// Token: 0x04000646 RID: 1606
		public Object heldObject;

		// Token: 0x04000647 RID: 1607
		public int minutesUntilReady;

		// Token: 0x04000648 RID: 1608
		public Microsoft.Xna.Framework.Rectangle boundingBox;

		// Token: 0x04000649 RID: 1609
		public Vector2 scale;

		// Token: 0x0400064A RID: 1610
		[XmlIgnore]
		public LightSource lightSource;

		// Token: 0x0400064B RID: 1611
		[XmlIgnore]
		public int shakeTimer;

		// Token: 0x0400064C RID: 1612
		[XmlIgnore]
		public Cue internalSound;

		// Token: 0x0400064D RID: 1613
		protected int health = 10;
	}
}
