using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	// Token: 0x02000045 RID: 69
	public class NPC : Character, IComparable
	{
		// Token: 0x1700005E RID: 94
		[XmlIgnore]
		public SchedulePathDescription DirectionsToNewLocation
		{
			// Token: 0x06000513 RID: 1299 RVA: 0x0006A376 File Offset: 0x00068576
			get
			{
				return this.directionsToNewLocation;
			}
			// Token: 0x06000514 RID: 1300 RVA: 0x0006A37E File Offset: 0x0006857E
			set
			{
				this.directionsToNewLocation = value;
			}
		}

		// Token: 0x1700005F RID: 95
		[XmlIgnore]
		public int DirectionIndex
		{
			// Token: 0x06000515 RID: 1301 RVA: 0x0006A387 File Offset: 0x00068587
			get
			{
				return this.directionIndex;
			}
			// Token: 0x06000516 RID: 1302 RVA: 0x0006A38F File Offset: 0x0006858F
			set
			{
				this.directionIndex = value;
			}
		}

		// Token: 0x17000060 RID: 96
		public int DefaultFacingDirection
		{
			// Token: 0x06000517 RID: 1303 RVA: 0x0006A398 File Offset: 0x00068598
			get
			{
				return this.defaultFacingDirection;
			}
			// Token: 0x06000518 RID: 1304 RVA: 0x0006A3A0 File Offset: 0x000685A0
			set
			{
				this.defaultFacingDirection = value;
			}
		}

		// Token: 0x17000061 RID: 97
		[XmlIgnore]
		public Dictionary<string, string> Dialogue
		{
			// Token: 0x06000519 RID: 1305 RVA: 0x0006A3A9 File Offset: 0x000685A9
			get
			{
				return this.dialogue;
			}
			// Token: 0x0600051A RID: 1306 RVA: 0x0006A3B1 File Offset: 0x000685B1
			set
			{
				this.dialogue = value;
			}
		}

		// Token: 0x17000062 RID: 98
		public string DefaultMap
		{
			// Token: 0x0600051B RID: 1307 RVA: 0x0006A3BA File Offset: 0x000685BA
			get
			{
				return this.defaultMap;
			}
			// Token: 0x0600051C RID: 1308 RVA: 0x0006A3C2 File Offset: 0x000685C2
			set
			{
				this.defaultMap = value;
			}
		}

		// Token: 0x17000063 RID: 99
		public Vector2 DefaultPosition
		{
			// Token: 0x0600051D RID: 1309 RVA: 0x0006A3CB File Offset: 0x000685CB
			get
			{
				return this.defaultPosition;
			}
			// Token: 0x0600051E RID: 1310 RVA: 0x0006A3D3 File Offset: 0x000685D3
			set
			{
				this.defaultPosition = value;
			}
		}

		// Token: 0x17000064 RID: 100
		[XmlIgnore]
		public Texture2D Portrait
		{
			// Token: 0x0600051F RID: 1311 RVA: 0x0006A3DC File Offset: 0x000685DC
			get
			{
				return this.portrait;
			}
			// Token: 0x06000520 RID: 1312 RVA: 0x0006A3E4 File Offset: 0x000685E4
			set
			{
				this.portrait = value;
			}
		}

		// Token: 0x17000065 RID: 101
		[XmlIgnore]
		public Dictionary<int, SchedulePathDescription> Schedule
		{
			// Token: 0x06000521 RID: 1313 RVA: 0x0006A3ED File Offset: 0x000685ED
			get
			{
				return this.schedule;
			}
			// Token: 0x06000522 RID: 1314 RVA: 0x0006A3F5 File Offset: 0x000685F5
			set
			{
				this.schedule = value;
			}
		}

		// Token: 0x17000066 RID: 102
		public bool IsWalkingInSquare
		{
			// Token: 0x06000523 RID: 1315 RVA: 0x0006A3FE File Offset: 0x000685FE
			get
			{
				return this.isWalkingInSquare;
			}
			// Token: 0x06000524 RID: 1316 RVA: 0x0006A406 File Offset: 0x00068606
			set
			{
				this.isWalkingInSquare = value;
			}
		}

		// Token: 0x17000067 RID: 103
		public bool IsWalkingTowardPlayer
		{
			// Token: 0x06000525 RID: 1317 RVA: 0x0006A40F File Offset: 0x0006860F
			get
			{
				return this.isWalkingTowardPlayer;
			}
			// Token: 0x06000526 RID: 1318 RVA: 0x0006A417 File Offset: 0x00068617
			set
			{
				this.isWalkingTowardPlayer = value;
			}
		}

		// Token: 0x17000068 RID: 104
		[XmlIgnore]
		public Stack<Dialogue> CurrentDialogue
		{
			// Token: 0x06000527 RID: 1319 RVA: 0x0006A420 File Offset: 0x00068620
			get
			{
				return this.currentDialogue;
			}
			// Token: 0x06000528 RID: 1320 RVA: 0x0006A428 File Offset: 0x00068628
			set
			{
				this.currentDialogue = value;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0006A434 File Offset: 0x00068634
		public NPC()
		{
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0006A49C File Offset: 0x0006869C
		public NPC(AnimatedSprite sprite, Vector2 position, int facingDir, string name, SerializableDictionary<int, int[]> schedule, Texture2D portrait, int idForClones) : base(sprite, position, 2, name)
		{
			this.idForClones = idForClones;
			this.portrait = portrait;
			this.faceDirection(facingDir);
			sprite.standAndFaceDirection(facingDir);
			this.defaultPosition = position;
			this.defaultMap = "";
			this.defaultFacingDirection = this.facingDirection;
			try
			{
				bool wasKing = false;
				if (name.Equals("Dwarf King"))
				{
					name = "Dwarf";
					wasKing = true;
				}
				this.dialogue = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + name);
				if (wasKing)
				{
					this.name = "DwarfKing";
				}
			}
			catch (Exception)
			{
			}
			this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(base.getStandingX() - base.getStandingX() % Game1.tileSize, base.getStandingY() - base.getStandingY() % Game1.tileSize, Game1.tileSize, Game1.tileSize);
			this.updateDialogue();
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0006A5DC File Offset: 0x000687DC
		public NPC(AnimatedSprite sprite, Vector2 position, int facingDir, string name, LocalizedContentManager content = null) : base(sprite, position, 2, name)
		{
			this.faceDirection(facingDir);
			sprite.standAndFaceDirection(facingDir);
			this.defaultPosition = position;
			this.defaultFacingDirection = facingDir;
			this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y + Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (content != null)
			{
				try
				{
					this.portrait = content.Load<Texture2D>("Portraits\\" + name);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0006A6C0 File Offset: 0x000688C0
		public NPC(AnimatedSprite sprite, Vector2 position, string defaultMap, int facingDirection, string name, bool datable, Dictionary<int, int[]> schedule, Texture2D portrait) : this(sprite, position, defaultMap, facingDirection, name, schedule, portrait, false)
		{
			this.datable = datable;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0006A6E8 File Offset: 0x000688E8
		public NPC(AnimatedSprite sprite, Vector2 position, string defaultMap, int facingDir, string name, Dictionary<int, int[]> schedule, Texture2D portrait, bool eventActor) : base(sprite, position, 2, name)
		{
			this.portrait = portrait;
			this.faceDirection(facingDir);
			if (sprite != null)
			{
				sprite.faceDirectionStandard(facingDir);
			}
			this.defaultPosition = position;
			this.defaultMap = defaultMap;
			this.currentLocation = Game1.getLocationFromName(defaultMap);
			this.defaultFacingDirection = facingDir;
			if (!eventActor)
			{
				if ((name.Equals("Lewis") || name.Equals("Robin")) && Game1.NPCGiftTastes.ContainsKey(name) && !Game1.player.friendships.ContainsKey(name))
				{
					Game1.player.friendships.Add(name, new int[6]);
				}
				this.loadSeasonalDialogue();
				this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y + Game1.tileSize, Game1.tileSize, Game1.tileSize);
			}
			try
			{
				Dictionary<string, string> NPCDispositions = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
				if (NPCDispositions.ContainsKey(name))
				{
					string[] dataSplit = NPCDispositions[name].Split(new char[]
					{
						'/'
					});
					string a = dataSplit[0];
					if (!(a == "teen"))
					{
						if (a == "child")
						{
							this.age = 2;
						}
					}
					else
					{
						this.age = 1;
					}
					a = dataSplit[1];
					if (!(a == "rude"))
					{
						if (a == "polite")
						{
							this.manners = 1;
						}
					}
					else
					{
						this.manners = 2;
					}
					a = dataSplit[2];
					if (!(a == "shy"))
					{
						if (a == "outgoing")
						{
							this.socialAnxiety = 0;
						}
					}
					else
					{
						this.socialAnxiety = 1;
					}
					a = dataSplit[3];
					if (!(a == "positive"))
					{
						if (a == "negative")
						{
							this.optimism = 1;
						}
					}
					else
					{
						this.optimism = 0;
					}
					a = dataSplit[4];
					if (!(a == "female"))
					{
						if (a == "undefined")
						{
							this.gender = 2;
						}
					}
					else
					{
						this.gender = 1;
					}
					a = dataSplit[5];
					if (!(a == "datable"))
					{
						if (a == "not-datable")
						{
							this.datable = false;
						}
					}
					else
					{
						this.datable = true;
					}
					this.loveInterest = dataSplit[6];
					a = dataSplit[7];
					if (!(a == "Desert"))
					{
						if (!(a == "Other"))
						{
							if (a == "Town")
							{
								this.homeRegion = 2;
							}
						}
						else
						{
							this.homeRegion = 0;
						}
					}
					else
					{
						this.homeRegion = 1;
					}
					if (dataSplit.Length > 8)
					{
						this.birthday_Season = dataSplit[8].Split(new char[]
						{
							' '
						})[0];
						this.birthday_Day = Convert.ToInt32(dataSplit[8].Split(new char[]
						{
							' '
						})[1]);
					}
					for (int i = 0; i < NPCDispositions.Count; i++)
					{
						if (NPCDispositions.ElementAt(i).Key.Equals(name))
						{
							this.id = i;
							break;
						}
					}
					this.displayName = dataSplit[11];
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0006AA60 File Offset: 0x00068C60
		public string getName()
		{
			if (this.displayName != null && this.displayName.Length > 0)
			{
				return this.displayName;
			}
			return this.name;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0006AA88 File Offset: 0x00068C88
		public virtual void reloadSprite()
		{
			string name = this.name;
			string textureName;
			if (!(name == "Old Mariner"))
			{
				if (!(name == "Dwarf King"))
				{
					if (!(name == "Mister Qi"))
					{
						if (!(name == "???"))
						{
							textureName = this.name;
						}
						else
						{
							textureName = "Monsters\\Shadow Guy";
						}
					}
					else
					{
						textureName = "MrQi";
					}
				}
				else
				{
					textureName = "DwarfKing";
				}
			}
			else
			{
				textureName = "Mariner";
			}
			if (this.name.Equals(Utility.getOtherFarmerNames()[0]))
			{
				textureName = (Game1.player.isMale ? "maleRival" : "femaleRival");
			}
			if (!this.IsMonster)
			{
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + textureName));
				if (!this.name.Contains("Dwarf"))
				{
					this.sprite.spriteHeight = 32;
				}
			}
			else
			{
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Monsters\\" + textureName));
			}
			try
			{
				this.portrait = Game1.content.Load<Texture2D>("Portraits\\" + textureName);
			}
			catch (Exception)
			{
				this.portrait = null;
			}
			bool arg_12C_0 = this.isInvisible;
			if (!Game1.newDay && Game1.gameMode != 6)
			{
				return;
			}
			this.faceDirection(this.DefaultFacingDirection);
			this.scheduleTimeToTry = 9999999;
			this.previousEndPoint = new Point((int)this.defaultPosition.X / Game1.tileSize, (int)this.defaultPosition.Y / Game1.tileSize);
			this.Schedule = this.getSchedule(Game1.dayOfMonth);
			this.faceDirection(this.defaultFacingDirection);
			this.sprite.standAndFaceDirection(this.defaultFacingDirection);
			this.loadSeasonalDialogue();
			this.updateDialogue();
			if (this.isMarried())
			{
				this.marriageDuties();
			}
			bool isFestivalToday = Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason);
			if (this.name.Equals("Robin") && Game1.player.daysUntilHouseUpgrade > 0 && !isFestivalToday)
			{
				this.setTilePosition(68, 14);
				this.ignoreMultiplayerUpdates = true;
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(24, 75),
					new FarmerSprite.AnimationFrame(25, 75),
					new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
					new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
				});
				this.ignoreScheduleToday = true;
				this.CurrentDialogue.Clear();
				this.currentDialogue.Push(new Dialogue((Game1.player.daysUntilHouseUpgrade == 2) ? "Be patient, I still have a lot of work to do." : "Your house should be ready tomorrow.", this));
			}
			else if (this.name.Equals("Robin") && Game1.getFarm().isThereABuildingUnderConstruction() && !isFestivalToday)
			{
				this.ignoreMultiplayerUpdates = true;
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(24, 75),
					new FarmerSprite.AnimationFrame(25, 75),
					new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
					new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
				});
				this.ignoreScheduleToday = true;
				Building b = Game1.getFarm().getBuildingUnderConstruction();
				if (b.daysUntilUpgrade > 0)
				{
					if (!b.indoors.characters.Contains(this))
					{
						b.indoors.addCharacter(this);
					}
					if (this.currentLocation != null)
					{
						this.currentLocation.characters.Remove(this);
					}
					this.currentLocation = b.indoors;
					this.setTilePosition(1, 5);
				}
				else
				{
					Game1.warpCharacter(this, "Farm", new Vector2((float)(b.tileX + b.tilesWide / 2), (float)(b.tileY + b.tilesHigh / 2)), false, false);
					this.position.X = this.position.X + (float)(Game1.tileSize / 4);
					this.position.Y = this.position.Y - (float)(Game1.tileSize / 2);
				}
				this.CurrentDialogue.Clear();
				this.currentDialogue.Push(new Dialogue("Be patient, I still have a lot of work to do.", this));
			}
			if (this.name.Equals("Shane") || this.name.Equals("Emily"))
			{
				this.datable = true;
			}
			try
			{
				this.displayName = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[this.name].Split(new char[]
				{
					'/'
				})[11];
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0006AF6C File Offset: 0x0006916C
		public void showTextAboveHead(string Text, int spriteTextColor = -1, int style = 2, int duration = 3000, int preTimer = 0)
		{
			this.textAboveHeadAlpha = 0f;
			this.textAboveHead = Text;
			this.textAboveHeadPreTimer = preTimer;
			this.textAboveHeadTimer = duration;
			this.textAboveHeadStyle = style;
			this.textAboveHeadColor = spriteTextColor;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0006AFA0 File Offset: 0x000691A0
		public void loadSeasonalDialogue()
		{
			try
			{
				this.dialogue = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + this.name);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0006AFE4 File Offset: 0x000691E4
		public void moveToNewPlaceForEvent(int xTile, int yTile, string oldMap)
		{
			this.mapBeforeEvent = oldMap;
			this.positionBeforeEvent = this.position;
			this.position = new Vector2((float)(xTile * Game1.tileSize), (float)(yTile * Game1.tileSize - Game1.tileSize * 3 / 2));
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool hitWithTool(Tool t)
		{
			return false;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0006B01E File Offset: 0x0006921E
		public bool canReceiveThisItemAsGift(Item i)
		{
			return i is Object || i is Ring || i is Hat || i is Boots || i is MeleeWeapon;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0006B04C File Offset: 0x0006924C
		public int getGiftTasteForThisItem(Item item)
		{
			int tasteForItem = 8;
			if (item is Object)
			{
				Object o = item as Object;
				string NPCLikes;
				Game1.NPCGiftTastes.TryGetValue(this.name, out NPCLikes);
				string[] split = NPCLikes.Split(new char[]
				{
					'/'
				});
				int itemNumber = o.ParentSheetIndex;
				int categoryNumber = o.Category;
				string itemNumberString = string.Concat(itemNumber);
				string categoryNumberString = string.Concat(categoryNumber);
				if (Game1.NPCGiftTastes["Universal_Love"].Split(new char[]
				{
					' '
				}).Contains(categoryNumberString))
				{
					tasteForItem = 0;
				}
				else if (Game1.NPCGiftTastes["Universal_Hate"].Split(new char[]
				{
					' '
				}).Contains(categoryNumberString))
				{
					tasteForItem = 6;
				}
				else if (Game1.NPCGiftTastes["Universal_Like"].Split(new char[]
				{
					' '
				}).Contains(categoryNumberString))
				{
					tasteForItem = 2;
				}
				else if (Game1.NPCGiftTastes["Universal_Dislike"].Split(new char[]
				{
					' '
				}).Contains(categoryNumberString))
				{
					tasteForItem = 4;
				}
				bool wasIndividualUniversal = false;
				bool skipDefaultValueRules = false;
				if (Game1.NPCGiftTastes["Universal_Love"].Split(new char[]
				{
					' '
				}).Contains(itemNumberString))
				{
					tasteForItem = 0;
					wasIndividualUniversal = true;
				}
				else if (Game1.NPCGiftTastes["Universal_Hate"].Split(new char[]
				{
					' '
				}).Contains(itemNumberString))
				{
					tasteForItem = 6;
					wasIndividualUniversal = true;
				}
				else if (Game1.NPCGiftTastes["Universal_Like"].Split(new char[]
				{
					' '
				}).Contains(itemNumberString))
				{
					tasteForItem = 2;
					wasIndividualUniversal = true;
				}
				else if (Game1.NPCGiftTastes["Universal_Dislike"].Split(new char[]
				{
					' '
				}).Contains(itemNumberString))
				{
					tasteForItem = 4;
					wasIndividualUniversal = true;
				}
				else if (Game1.NPCGiftTastes["Universal_Neutral"].Split(new char[]
				{
					' '
				}).Contains(itemNumberString))
				{
					tasteForItem = 8;
					wasIndividualUniversal = true;
					skipDefaultValueRules = true;
				}
				if (tasteForItem == 8 && !skipDefaultValueRules)
				{
					if (o.edibility != -300 && o.edibility < 0)
					{
						tasteForItem = 6;
					}
					else if (o.price < 20)
					{
						tasteForItem = 4;
					}
					else if (o.type.Contains("Arch"))
					{
						tasteForItem = 4;
						if (this.name.Equals("Penny"))
						{
							tasteForItem = 2;
						}
					}
				}
				if (NPCLikes != null)
				{
					List<int[]> items = new List<int[]>();
					for (int i = 0; i < 10; i += 2)
					{
						string[] splitItems = split[i + 1].Split(new char[]
						{
							' '
						});
						int[] thisItems = new int[splitItems.Length];
						for (int j = 0; j < splitItems.Length; j++)
						{
							if (splitItems[j].Length > 0)
							{
								thisItems[j] = Convert.ToInt32(splitItems[j]);
							}
						}
						items.Add(thisItems);
					}
					if ((items[0].Contains(itemNumber) || (categoryNumber != 0 && items[0].Contains(categoryNumber))) && (categoryNumber == 0 || !items[0].Contains(categoryNumber) || !wasIndividualUniversal))
					{
						return 0;
					}
					if ((items[3].Contains(itemNumber) || (categoryNumber != 0 && items[3].Contains(categoryNumber))) && (categoryNumber == 0 || !items[3].Contains(categoryNumber) || !wasIndividualUniversal))
					{
						return 6;
					}
					if ((items[1].Contains(itemNumber) || (categoryNumber != 0 && items[1].Contains(categoryNumber))) && (categoryNumber == 0 || !items[1].Contains(categoryNumber) || !wasIndividualUniversal))
					{
						return 2;
					}
					if ((items[2].Contains(itemNumber) || (categoryNumber != 0 && items[2].Contains(categoryNumber))) && (categoryNumber == 0 || !items[2].Contains(categoryNumber) || !wasIndividualUniversal))
					{
						return 4;
					}
					if ((items[4].Contains(itemNumber) || (categoryNumber != 0 && items[4].Contains(categoryNumber))) && (categoryNumber == 0 || !items[4].Contains(categoryNumber) || !wasIndividualUniversal))
					{
						return 8;
					}
				}
			}
			return tasteForItem;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0006B47E File Offset: 0x0006967E
		private void goblinDoorEndBehavior(Character c, GameLocation l)
		{
			l.characters.Remove(this);
			Game1.soundBank.PlayCue("doorClose");
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0006B49C File Offset: 0x0006969C
		public virtual void tryToReceiveActiveObject(Farmer who)
		{
			who.Halt();
			who.faceGeneralDirection(base.getStandingPosition(), 0);
			if (this.name.Equals("Henchman") && Game1.currentLocation.name.Equals("WitchSwamp"))
			{
				if (who.ActiveObject == null || who.ActiveObject.parentSheetIndex != 308)
				{
					if (who.ActiveObject != null)
					{
						if (who.ActiveObject.parentSheetIndex == 684)
						{
							this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman4", new object[0]), this));
						}
						else
						{
							this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman3", new object[0]), this));
						}
						Game1.drawDialogue(this);
					}
					return;
				}
				if (this.controller != null)
				{
					return;
				}
				Game1.playSound("coin");
				who.reduceActiveItemByOne();
				this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman5", new object[0]), this));
				Game1.drawDialogue(this);
				this.sprite.CurrentFrame = 4;
				Game1.player.removeQuest(27);
				Stack<Point> p = new Stack<Point>();
				p.Push(new Point(20, 21));
				p.Push(new Point(20, 22));
				p.Push(new Point(20, 23));
				p.Push(new Point(20, 24));
				p.Push(new Point(20, 25));
				p.Push(new Point(20, 26));
				p.Push(new Point(20, 27));
				p.Push(new Point(20, 28));
				this.addedSpeed = 2;
				this.controller = new PathFindController(p, this, Game1.currentLocation);
				this.controller.endBehaviorFunction = new PathFindController.endBehavior(this.goblinDoorEndBehavior);
				this.showTextAboveHead(Game1.content.LoadString("Strings\\Characters:Henchman6", new object[0]), -1, 2, 3000, 0);
				Game1.player.mailReceived.Add("henchmanGone");
				Game1.currentLocation.removeTile(20, 29, "Buildings");
				who.freezePause = 2000;
				return;
			}
			else
			{
				if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("ItemDeliveryQuest") && ((ItemDeliveryQuest)Game1.questOfTheDay).checkIfComplete(this, -1, -1, who.ActiveObject, null))
				{
					who.reduceActiveItemByOne();
					who.completelyStopAnimatingOrDoingAction();
					if (Game1.random.NextDouble() < 0.3 && !this.name.Equals("Wizard"))
					{
						base.doEmote(32, true);
					}
					return;
				}
				if (Game1.questOfTheDay != null && Game1.questOfTheDay.GetType().Name.Equals("FishingQuest") && ((FishingQuest)Game1.questOfTheDay).checkIfComplete(this, who.ActiveObject.ParentSheetIndex, -1, null, null))
				{
					who.reduceActiveItemByOne();
					who.completelyStopAnimatingOrDoingAction();
					if (Game1.random.NextDouble() < 0.3 && !this.name.Equals("Wizard"))
					{
						base.doEmote(32, true);
					}
					return;
				}
				if (who.ActiveObject != null && who.ActiveObject.questItem)
				{
					if (!who.checkForQuestComplete(this, -1, -1, who.ActiveObject, "", 9, 3))
					{
						Game1.showRedMessage("Wrong Person");
					}
					return;
				}
				if (who.checkForQuestComplete(this, -1, -1, null, "", 10, -1))
				{
					return;
				}
				if (Game1.NPCGiftTastes.ContainsKey(this.name))
				{
					who.completeQuest(25);
					if (who.ActiveObject.ParentSheetIndex == 458)
					{
						if (!this.datable)
						{
							if (Game1.random.NextDouble() < 0.5)
							{
								Game1.drawObjectDialogue("You can't date " + this.name);
								return;
							}
							this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "Umm... I don't think so." : "Is this a joke? I don't get it.", this));
							Game1.drawDialogue(this);
							return;
						}
						else
						{
							if (this.datable && this.divorcedFromFarmer)
							{
								this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Divorced_bouquet", new object[0]), this));
								Game1.drawDialogue(this);
								return;
							}
							if (this.datable && who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 1000)
							{
								this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "...I don't really know you well enough...$s" : "Uh... This is awkward. Look, I don't really know you that well...$s", this));
								Game1.drawDialogue(this);
								return;
							}
							if (this.datable && who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 2000)
							{
								this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "Oh? ...Sorry... I'm not ready for that.$s" : "...I didn't know. Uh... Sorry. No.$s", this));
								Game1.drawDialogue(this);
								return;
							}
							this.datingFarmer = true;
							this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "...!! I'll accept this. Thank you.$l#$e#I didn't know you felt the same.$l" : "...You want to get more serious? I feel the same way.$l#$e#I'm kind of nervous. Aren't you?$l", this));
							who.changeFriendship(25, this);
							who.reduceActiveItemByOne();
							who.completelyStopAnimatingOrDoingAction();
							base.doEmote(20, true);
							Game1.drawDialogue(this);
							return;
						}
					}
					else if (who.ActiveObject.ParentSheetIndex == 460)
					{
						if (who.spouse != null)
						{
							if (who.spouse.Contains("engaged"))
							{
								this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "You just got engaged! What are you thinking?$s" : "What? Didn't you just ask someone else to marry you? You're crazy!$s", this));
								Game1.drawDialogue(this);
								return;
							}
							this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "Um, aren't you already married?" : "You're already married, you weirdo!$s", this));
							Game1.drawDialogue(this);
							return;
						}
						else if (!this.datable || this.divorcedFromFarmer || (who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] < 1500))
						{
							if (Game1.random.NextDouble() < 0.5)
							{
								Game1.drawObjectDialogue(this.name + " doesn't want to marry you right now.");
								return;
							}
							this.CurrentDialogue.Push(new Dialogue((this.gender == 1) ? "You're strange.$s" : "Heh. Funny joke.", this));
							Game1.drawDialogue(this);
							return;
						}
						else
						{
							if (!this.datable || !who.friendships.ContainsKey(this.name) || who.friendships[this.name][0] >= 2500)
							{
								Game1.changeMusicTrack("none");
								who.spouse = this.name + "engaged";
								Game1.countdownToWedding = 3;
								this.datingFarmer = true;
								this.CurrentDialogue.Clear();
								this.CurrentDialogue.Push(new Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\EngagementDialogue")[this.name + "0"], this));
								this.CurrentDialogue.Push(new Dialogue("...!!! $l#$b#I accept!! $h#$b#...$l#$b#I'll set everything up. We'll have the ceremony in 3 days, okay?$h", this));
								who.changeFriendship(1, this);
								who.reduceActiveItemByOne();
								who.completelyStopAnimatingOrDoingAction();
								Game1.drawDialogue(this);
								return;
							}
							if (who.friendships[this.name][4] == 0)
							{
								this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "Oh!! ... $l #$b#@... I'm sorry, but no. I won't marry you right now.$s" : "!! Er... I'm sorry, I don't feel that way about you...$s", this));
								Game1.drawDialogue(this);
								who.changeFriendship(-20, this);
								who.friendships[this.name][4] = 1;
								return;
							}
							this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "Look. I already told you no! Leave me alone.$a" : "You already asked me that. What part of 'NO' don't you understand?!$a", this));
							Game1.drawDialogue(this);
							who.changeFriendship(-50, this);
							return;
						}
					}
					else if ((who.friendships.ContainsKey(this.name) && who.friendships[this.name][1] < 2) || (who.spouse != null && who.spouse.Equals(this.name)) || this is Child || this.isBirthday(Game1.currentSeason, Game1.dayOfMonth))
					{
						if (this.divorcedFromFarmer)
						{
							this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Divorced_gift", new object[0]), this));
							Game1.drawDialogue(this);
							return;
						}
						if (who.friendships[this.name][3] == 1)
						{
							Game1.drawObjectDialogue(Game1.parseText("You've already given " + this.name + " a gift today."));
							return;
						}
						this.receiveGift(who.ActiveObject, who, true, 1f, true);
						who.reduceActiveItemByOne();
						who.completelyStopAnimatingOrDoingAction();
						base.faceTowardFarmerForPeriod(4000, 3, false, who);
						if (this.datable && who.spouse != null && !who.spouse.Contains(this.name) && Utility.isMale(who.spouse.Replace("engaged", "")) == Utility.isMale(this.name) && Game1.random.NextDouble() < 0.3 - (double)((float)who.LuckLevel / 100f) - Game1.dailyLuck && !this.isBirthday(Game1.currentSeason, Game1.dayOfMonth))
						{
							NPC spouse = Game1.getCharacterFromName(who.spouse.Replace("engaged", ""), false);
							who.changeFriendship(-30, spouse);
							spouse.CurrentDialogue.Clear();
							spouse.CurrentDialogue.Push(new Dialogue("So...I heard you secretly gave " + this.name + " a gift today...$s#$e#Do I have to be suspicious of you?$s#$e#...$a", spouse));
							return;
						}
					}
					else
					{
						Game1.drawObjectDialogue(Game1.parseText(string.Concat(new object[]
						{
							"You've already given ",
							this.name,
							" ",
							2,
							" gifts this week! That's enough."
						})));
					}
				}
				return;
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0006BF3B File Offset: 0x0006A13B
		public void haltMe(Farmer who)
		{
			this.Halt();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0006BF44 File Offset: 0x0006A144
		public virtual bool checkAction(Farmer who, GameLocation l)
		{
			if (this.isInvisible)
			{
				return false;
			}
			if (who.isRidingHorse())
			{
				who.Halt();
			}
			if (this.name.Equals("Henchman") && l.name.Equals("WitchSwamp"))
			{
				if (!Game1.player.mailReceived.Contains("Henchman1"))
				{
					Game1.player.mailReceived.Add("Henchman1");
					this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman1", new object[0]), this));
					Game1.drawDialogue(this);
					Game1.player.addQuest(27);
					Game1.player.friendships.Add("Henchman", new int[6]);
				}
				else
				{
					if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
					{
						this.tryToReceiveActiveObject(who);
						return true;
					}
					if (this.controller == null)
					{
						this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:Henchman2", new object[0]), this));
						Game1.drawDialogue(this);
					}
				}
				return true;
			}
			if (Game1.NPCGiftTastes.ContainsKey(this.name) && !Game1.player.friendships.ContainsKey(this.name))
			{
				Game1.player.friendships.Add(this.name, new int[6]);
				if (this.name.Equals("Krobus"))
				{
					this.currentDialogue.Push(new Dialogue("A human visitor? This is most unusual...#$e#I'm Krobus, merchant of rare and exotic goods.", this));
					Game1.drawDialogue(this);
					return true;
				}
			}
			if (who.checkForQuestComplete(this, -1, -1, who.ActiveObject, null, -1, 5))
			{
				base.faceTowardFarmerForPeriod(6000, 3, false, who);
				return true;
			}
			if (this.name.Equals("Dwarf") && this.currentDialogue.Count <= 0 && who.canUnderstandDwarves && l.name.Equals("Mine"))
			{
				Game1.activeClickableMenu = new ShopMenu(Utility.getDwarfShopStock(), 0, "Dwarf");
			}
			if (this.name.Equals("Krobus"))
			{
				if (who.hasQuest(28))
				{
					this.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:KrobusDarkTalisman", new object[0]), this));
					Game1.drawDialogue(this);
					who.removeQuest(28);
					who.mailReceived.Add("krobusUnseal");
					DelayedAction.addTemporarySpriteAfterDelay(new TemporaryAnimatedSprite(Projectile.projectileSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 3000f, 1, 0, new Vector2(31f, 17f) * (float)Game1.tileSize, false, false)
					{
						scale = (float)Game1.pixelZoom,
						delayBeforeAnimationStart = 1,
						startSound = "debuffSpell",
						motion = new Vector2(-9f, 1f),
						rotationChange = 0.0490873866f,
						light = true,
						lightRadius = 1f,
						lightcolor = new Color(150, 0, 50),
						layerDepth = 1f,
						alphaFade = 0.003f
					}, l, 200, true);
					DelayedAction.addTemporarySpriteAfterDelay(new TemporaryAnimatedSprite(Projectile.projectileSheet, new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), 3000f, 1, 0, new Vector2(31f, 17f) * (float)Game1.tileSize, false, false)
					{
						startSound = "debuffSpell",
						delayBeforeAnimationStart = 1,
						scale = (float)Game1.pixelZoom,
						motion = new Vector2(-9f, 1f),
						rotationChange = 0.0490873866f,
						light = true,
						lightRadius = 1f,
						lightcolor = new Color(150, 0, 50),
						layerDepth = 1f,
						alphaFade = 0.003f
					}, l, 700, true);
					return true;
				}
				if (this.currentDialogue.Count <= 0 && l is Sewer)
				{
					Game1.activeClickableMenu = new ShopMenu((l as Sewer).getShadowShopStock(), 0, "Krobus");
				}
			}
			if (this.name.Equals(who.spouse) && who.IsMainPlayer)
			{
				int arg_43A_0 = Game1.timeOfDay;
				if (this.sprite.currentAnimation == null)
				{
					this.faceDirection(-3);
				}
				if (this.sprite.currentAnimation == null && who.friendships.ContainsKey(this.name) && who.friendships[this.name][0] >= 3375 && !who.mailReceived.Contains("CF_Spouse"))
				{
					this.CurrentDialogue.Push(new Dialogue("Honey... I wanted to give you that fruit as a symbol of my love.$l", this));
					Game1.player.addItemByMenuIfNecessary(new Object(Vector2.Zero, 434, "Cosmic Fruit", false, false, false, false), null);
					who.mailReceived.Add("CF_Spouse");
					return true;
				}
				if (this.sprite.currentAnimation == null && !this.hasTemporaryMessageAvailable() && this.CurrentDialogue.Count == 0 && Game1.timeOfDay < 2200 && this.controller == null && who.ActiveObject == null)
				{
					base.faceGeneralDirection(who.getStandingPosition(), 0);
					who.faceGeneralDirection(base.getStandingPosition(), 0);
					if (this.facingDirection == 3 || this.facingDirection == 1)
					{
						int spouseFrame = 28;
						bool facingRight = true;
						string name = this.name;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
						if (num <= 1708213605u)
						{
							if (num <= 587846041u)
							{
								if (num != 161540545u)
								{
									if (num == 587846041u)
									{
										if (name == "Penny")
										{
											spouseFrame = 35;
											facingRight = true;
										}
									}
								}
								else if (name == "Sebastian")
								{
									spouseFrame = 40;
									facingRight = false;
								}
							}
							else if (num != 1067922812u)
							{
								if (num != 1281010426u)
								{
									if (num == 1708213605u)
									{
										if (name == "Alex")
										{
											spouseFrame = 42;
											facingRight = true;
										}
									}
								}
								else if (name == "Maru")
								{
									spouseFrame = 28;
									facingRight = false;
								}
							}
							else if (name == "Sam")
							{
								spouseFrame = 36;
								facingRight = true;
							}
						}
						else if (num <= 2571828641u)
						{
							if (num != 1866496948u)
							{
								if (num != 2010304804u)
								{
									if (num == 2571828641u)
									{
										if (name == "Emily")
										{
											spouseFrame = 33;
											facingRight = false;
										}
									}
								}
								else if (name == "Harvey")
								{
									spouseFrame = 31;
									facingRight = false;
								}
							}
							else if (name == "Shane")
							{
								spouseFrame = 34;
								facingRight = false;
							}
						}
						else if (num != 2732913340u)
						{
							if (num != 2826247323u)
							{
								if (num == 3066176300u)
								{
									if (name == "Elliott")
									{
										spouseFrame = 35;
										facingRight = false;
									}
								}
							}
							else if (name == "Leah")
							{
								spouseFrame = 25;
								facingRight = true;
							}
						}
						else if (name == "Abigail")
						{
							spouseFrame = 33;
							facingRight = false;
						}
						bool flip = (facingRight && this.facingDirection == 3) || (!facingRight && this.facingDirection == 1);
						if (who.getFriendshipHeartLevelForNPC(this.name) > 9)
						{
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(spouseFrame, Game1.IsMultiplayer ? 1000 : 10, false, flip, new AnimatedSprite.endOfAnimationBehavior(this.haltMe), true)
							});
							if (!this.hasBeenKissedToday)
							{
								who.changeFriendship(10, this);
								who.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2((float)base.getTileX(), (float)base.getTileY()) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(-(float)Game1.tileSize)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -0.5f),
									alphaFade = 0.01f
								});
								Game1.playSound("dwop");
								who.exhausted = false;
							}
							this.hasBeenKissedToday = true;
						}
						else
						{
							this.faceDirection((Game1.random.NextDouble() < 0.5) ? 2 : 0);
							base.doEmote(12, true);
						}
						who.CanMove = false;
						who.FarmerSprite.pauseForSingleAnimation = false;
						if ((facingRight && !flip) || (!facingRight & flip))
						{
							who.faceDirection(3);
						}
						else
						{
							who.faceDirection(1);
						}
						who.FarmerSprite.animateOnce(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(101, 1000, 0, false, who.facingDirection == 3, null, false, 0),
							new FarmerSprite.AnimationFrame(6, 1, false, who.facingDirection == 3, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
						}.ToArray());
						return true;
					}
				}
			}
			bool newCurrentDialogue = false;
			if (who.friendships.ContainsKey(this.name))
			{
				newCurrentDialogue = this.checkForNewCurrentDialogue(who.friendships[this.name][0], false);
				if (!newCurrentDialogue)
				{
					newCurrentDialogue = this.checkForNewCurrentDialogue(who.friendships[this.name][0], true);
				}
			}
			if (who.IsMainPlayer && who.friendships.ContainsKey(this.name) && (this.endOfRouteMessage != null | newCurrentDialogue))
			{
				if (!newCurrentDialogue && this.setTemporaryMessages(who))
				{
					Game1.player.checkForQuestComplete(this, -1, -1, null, null, 5, -1);
					return false;
				}
				if (this.sprite.Texture.Bounds.Height > 32)
				{
					base.faceTowardFarmerForPeriod(5000, 4, false, who);
				}
				if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift())
				{
					this.tryToReceiveActiveObject(who);
					Game1.stats.checkForFriendshipAchievements();
					base.faceTowardFarmerForPeriod(3000, 4, false, who);
					return true;
				}
				if (!this.name.Contains("King") && !who.hasPlayerTalkedToNPC(this.name) && who.friendships.ContainsKey(this.name))
				{
					who.friendships[this.name][2] = 1;
					who.changeFriendship(10, this);
					Game1.stats.checkForFriendshipAchievements();
					Game1.player.checkForQuestComplete(this, -1, -1, null, null, 5, -1);
				}
				Game1.drawDialogue(this);
			}
			else if (this.CurrentDialogue.Count > 0)
			{
				if (!this.name.Contains("King") && who.ActiveObject != null)
				{
					if (who.IsMainPlayer)
					{
						this.tryToReceiveActiveObject(who);
						Game1.stats.checkForFriendshipAchievements();
					}
					else
					{
						base.faceTowardFarmerForPeriod(3000, 4, false, who);
					}
				}
				else if (who.hasClubCard && this.name.Equals("Bouncer") && who.IsMainPlayer)
				{
					Response[] responses = new Response[]
					{
						new Response("Yes.", "Yes."),
						new Response("That's", "That's right. Now step aside, blockhead.")
					};
					l.createQuestionDialogue("!!! Is... is that a 'Club Card'?", responses, "ClubCard");
				}
				else if (this.CurrentDialogue.Count >= 1 || this.endOfRouteMessage != null)
				{
					if (this.setTemporaryMessages(who))
					{
						Game1.player.checkForQuestComplete(this, -1, -1, null, null, 5, -1);
						return false;
					}
					if (this.sprite.Texture.Bounds.Height > 32)
					{
						base.faceTowardFarmerForPeriod(5000, 4, false, who);
					}
					if (who.IsMainPlayer)
					{
						if (!this.name.Contains("King") && !who.hasPlayerTalkedToNPC(this.name) && who.friendships.ContainsKey(this.name))
						{
							who.friendships[this.name][2] = 1;
							Game1.player.checkForQuestComplete(this, -1, -1, null, null, 5, -1);
							who.changeFriendship(20, this);
							Game1.stats.checkForFriendshipAchievements();
						}
						Game1.drawDialogue(this);
					}
				}
				else if (!this.doingEndOfRouteAnimation)
				{
					try
					{
						if (who.friendships.ContainsKey(this.name))
						{
							base.faceTowardFarmerForPeriod(who.friendships[this.name][0] / 125 * 1000 + 1000, 4, false, who);
						}
					}
					catch (Exception)
					{
					}
					if (Game1.random.NextDouble() < 0.1)
					{
						base.doEmote(8, true);
					}
				}
			}
			else if (this.name.Equals("Cat") && !(this as StardewValley.Monsters.Cat).wasPet)
			{
				(this as StardewValley.Monsters.Cat).wasPet = true;
				(this as StardewValley.Monsters.Cat).loveForMaster += 10;
				base.doEmote(20, true);
				Game1.playSound("purr");
			}
			else if (who.ActiveObject != null)
			{
				this.tryToReceiveActiveObject(who);
				Game1.stats.checkForFriendshipAchievements();
				base.faceTowardFarmerForPeriod(3000, 4, false, who);
				return true;
			}
			if (this.setTemporaryMessages(who))
			{
				return false;
			}
			if ((this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation) && this.endOfRouteMessage != null)
			{
				Game1.drawDialogue(this);
			}
			return false;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0006CCD0 File Offset: 0x0006AED0
		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (this.movementPause <= 0)
			{
				base.MovePosition(time, viewport, currentLocation);
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0006CCE4 File Offset: 0x0006AEE4
		public GameLocation getHome()
		{
			return Game1.getLocationFromName(this.defaultMap);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool canPassThroughActionTiles()
		{
			return true;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void behaviorOnFarmerPushing()
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0006CCF4 File Offset: 0x0006AEF4
		public virtual void behaviorOnFarmerLocationEntry(GameLocation location, Farmer who)
		{
			if (this.sprite != null && this.sprite.currentAnimation == null && this.sprite.sourceRect.Height > 32)
			{
				this.sprite.spriteWidth = 16;
				this.sprite.spriteHeight = 16;
				this.sprite.CurrentFrame = 0;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0006CD50 File Offset: 0x0006AF50
		public override void updateMovement(GameLocation location, GameTime time)
		{
			this.lastPosition = this.position;
			if (this.DirectionsToNewLocation != null && !Game1.newDay)
			{
				if (base.getStandingX() < -Game1.tileSize || base.getStandingX() > location.map.DisplayWidth + Game1.tileSize || base.getStandingY() < -Game1.tileSize || base.getStandingY() > location.map.DisplayHeight + Game1.tileSize)
				{
					this.IsWalkingInSquare = false;
					Game1.warpCharacter(this, this.DefaultMap, this.DefaultPosition, true, true);
					location.characters.Remove(this);
					return;
				}
				if (this.IsWalkingInSquare)
				{
					this.returnToEndPoint();
					this.MovePosition(time, Game1.viewport, location);
					return;
				}
				if (this.followSchedule)
				{
					this.MovePosition(time, Game1.viewport, location);
					Warp tmpWarp = location.isCollidingWithWarp(this.GetBoundingBox());
					PropertyValue door = null;
					Tile tmpTile = location.map.GetLayer("Buildings").PickTile(base.nextPositionPoint(), Game1.viewport.Size);
					if (tmpTile != null)
					{
						tmpTile.Properties.TryGetValue("Action", out door);
					}
					string[] isDoor = (door == null) ? null : door.ToString().Split(new char[]
					{
						' '
					});
					if (tmpWarp != null)
					{
						if (location is BusStop && tmpWarp.TargetName.Equals("Farm"))
						{
							Point tmp = ((this.isMarried() ? (this.getHome() as FarmHouse) : Game1.getLocationFromName("FarmHouse")) as FarmHouse).getEntryLocation();
							tmpWarp = new Warp(tmpWarp.X, tmpWarp.Y, "FarmHouse", tmp.X, tmp.Y, false);
						}
						else if (location is FarmHouse && tmpWarp.TargetName.Equals("Farm"))
						{
							tmpWarp = new Warp(tmpWarp.X, tmpWarp.Y, "BusStop", 0, 23, false);
						}
						Game1.warpCharacter(this, tmpWarp.TargetName, new Vector2((float)(tmpWarp.TargetX * Game1.tileSize), (float)(tmpWarp.TargetY * Game1.tileSize - base.Sprite.getHeight() / 2 - Game1.tileSize / 4)), false, location.IsOutdoors);
						location.characters.Remove(this);
						return;
					}
					if (isDoor != null && isDoor.Length >= 1 && isDoor[0].Contains("Warp"))
					{
						Game1.warpCharacter(this, isDoor[3], new Vector2((float)Convert.ToInt32(isDoor[1]), (float)Convert.ToInt32(isDoor[2])), false, location.IsOutdoors);
						if (Game1.currentLocation.name.Equals(location.name) && Utility.isOnScreen(base.getStandingPosition(), Game1.tileSize * 3))
						{
							Game1.playSound("doorClose");
						}
						location.characters.Remove(this);
						return;
					}
					if (isDoor != null && isDoor.Length >= 1 && isDoor[0].Contains("Door"))
					{
						location.openDoor(new Location(base.nextPositionPoint().X / Game1.tileSize, base.nextPositionPoint().Y / Game1.tileSize), Game1.player.currentLocation.Equals(location));
						return;
					}
					if (location.map.GetLayer("Paths") != null)
					{
						Tile tmp2 = location.map.GetLayer("Paths").PickTile(new Location(base.getStandingX(), base.getStandingY()), Game1.viewport.Size);
						Microsoft.Xna.Framework.Rectangle boundingbox = this.GetBoundingBox();
						boundingbox.Inflate(2, 2);
						if (tmp2 != null && new Microsoft.Xna.Framework.Rectangle(base.getStandingX() - base.getStandingX() % Game1.tileSize, base.getStandingY() - base.getStandingY() % Game1.tileSize, Game1.tileSize, Game1.tileSize).Contains(boundingbox))
						{
							switch (tmp2.TileIndex)
							{
							case 0:
								if (base.getDirection() == 3)
								{
									this.SetMovingOnlyUp();
									return;
								}
								if (base.getDirection() == 2)
								{
									this.SetMovingOnlyRight();
									return;
								}
								break;
							case 1:
								if (base.getDirection() == 3)
								{
									this.SetMovingOnlyDown();
									return;
								}
								if (base.getDirection() == 0)
								{
									this.SetMovingOnlyRight();
									return;
								}
								break;
							case 2:
								if (base.getDirection() == 1)
								{
									this.SetMovingOnlyDown();
									return;
								}
								if (base.getDirection() == 0)
								{
									this.SetMovingOnlyLeft();
									return;
								}
								break;
							case 3:
								if (base.getDirection() == 1)
								{
									this.SetMovingOnlyUp();
									return;
								}
								if (base.getDirection() == 2)
								{
									this.SetMovingOnlyLeft();
									return;
								}
								break;
							case 4:
								this.changeSchedulePathDirection();
								this.moveCharacterOnSchedulePath();
								return;
							case 5:
							case 6:
								break;
							case 7:
								this.ReachedEndPoint();
								return;
							default:
								return;
							}
						}
					}
				}
			}
			else if (this.IsWalkingInSquare)
			{
				this.randomSquareMovement(time);
				this.MovePosition(time, Game1.viewport, location);
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0006D1ED File Offset: 0x0006B3ED
		public void facePlayer(Farmer who)
		{
			if (this.facingDirectionBeforeSpeakingToPlayer == -1)
			{
				this.facingDirectionBeforeSpeakingToPlayer = base.getFacingDirection();
			}
			this.faceDirection((who.FacingDirection + 2) % 4);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00002834 File Offset: 0x00000A34
		public void doneFacingPlayer(Farmer who)
		{
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0006D214 File Offset: 0x0006B414
		public override void update(GameTime time, GameLocation location)
		{
			if (this.returningToEndPoint)
			{
				this.returnToEndPoint();
				this.MovePosition(time, Game1.viewport, location);
			}
			else if (this.temporaryController != null)
			{
				if (this.temporaryController.update(time))
				{
					this.temporaryController = null;
				}
				base.updateEmote(time);
			}
			else
			{
				base.update(time, location);
			}
			if (this.textAboveHeadTimer > 0)
			{
				if (this.textAboveHeadPreTimer > 0)
				{
					this.textAboveHeadPreTimer -= time.ElapsedGameTime.Milliseconds;
				}
				else
				{
					this.textAboveHeadTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.textAboveHeadTimer > 500)
					{
						this.textAboveHeadAlpha = Math.Min(1f, this.textAboveHeadAlpha + 0.1f);
					}
					else
					{
						this.textAboveHeadAlpha = Math.Max(0f, this.textAboveHeadAlpha - 0.04f);
					}
				}
			}
			if (this.isWalkingInSquare && !this.returningToEndPoint)
			{
				this.randomSquareMovement(time);
			}
			if (base.Sprite != null && base.Sprite.currentAnimation != null && !Game1.eventUp && base.Sprite.animateOnce(time))
			{
				base.Sprite.currentAnimation = null;
			}
			if (this.movementPause > 0 && (!Game1.dialogueUp || this.controller != null))
			{
				this.freezeMotion = true;
				this.movementPause -= time.ElapsedGameTime.Milliseconds;
				if (this.movementPause <= 0)
				{
					this.freezeMotion = false;
				}
			}
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.lastPosition.Equals(this.position))
			{
				this.timerSinceLastMovement += (float)time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.timerSinceLastMovement = 0f;
			}
			if (this.swimming)
			{
				this.yOffset = (float)(Math.Cos(time.TotalGameTime.TotalMilliseconds / 2000.0) * (double)Game1.pixelZoom);
				float oldSwimTimer = this.swimTimer;
				this.swimTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.timerSinceLastMovement == 0f)
				{
					if (oldSwimTimer > 400f && this.swimTimer <= 400f && location.Equals(Game1.currentLocation))
					{
						location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2(this.position.X, (float)(base.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
						Game1.playSound("slosh");
					}
					if (this.swimTimer < 0f)
					{
						this.swimTimer = 800f;
						if (location.Equals(Game1.currentLocation))
						{
							Game1.playSound("slosh");
							location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f - (Math.Abs(this.xVelocity) + Math.Abs(this.yVelocity)) * 3f, 8, 0, new Vector2(this.position.X, (float)(base.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
							return;
						}
					}
				}
				else if (this.swimTimer < 0f)
				{
					this.swimTimer = 100f;
				}
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0006D62C File Offset: 0x0006B82C
		public virtual void performTenMinuteUpdate(int timeOfDay, GameLocation l)
		{
			if (Game1.random.NextDouble() < 0.1 && this.dialogue != null && this.dialogue.ContainsKey(l.name + "_Ambient"))
			{
				string[] split = this.dialogue[l.name + "_Ambient"].Split(new char[]
				{
					'/'
				});
				int extraTime = Game1.random.Next(4) * 1000;
				this.showTextAboveHead(split[Game1.random.Next(split.Length)], -1, 2, 3000, extraTime);
				return;
			}
			if (this.isMoving() && l.isOutdoors && timeOfDay < 1800 && Game1.random.NextDouble() < 0.3 + ((this.socialAnxiety == 0) ? 0.25 : ((this.socialAnxiety == 1) ? ((this.manners == 2) ? -1.0 : -0.2) : 0.0)) && (this.age != 1 || (this.manners == 1 && this.socialAnxiety == 0)) && !this.isMarried())
			{
				Character c = Utility.isThereAFarmerOrCharacterWithinDistance(base.getTileLocation(), 4, l);
				if (c.name.Equals(this.name) || c is Horse)
				{
					return;
				}
				if (Game1.temporaryContent == null)
				{
					Game1.temporaryContent = Game1.content.CreateTemporary();
				}
				Dictionary<string, string> dispositions = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\NPCDispositions");
				if (dispositions.ContainsKey(this.name) && !dispositions[this.name].Split(new char[]
				{
					'/'
				})[9].Contains(c.name) && this.isFacingToward(c.getTileLocation()))
				{
					this.sayHiTo(c);
				}
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0006D81C File Offset: 0x0006BA1C
		public void sayHiTo(Character c)
		{
			if (this.getHi(c.name) == null)
			{
				return;
			}
			this.showTextAboveHead(this.getHi(c.name), -1, 2, 3000, 0);
			if (c is NPC && Game1.random.NextDouble() < 0.66)
			{
				if ((c as NPC).getHi(this.name) == null)
				{
					return;
				}
				(c as NPC).showTextAboveHead((c as NPC).getHi(this.name), -1, 2, 3000, 1000 + Game1.random.Next(500));
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0006D8BC File Offset: 0x0006BABC
		public string getHi(string nameToGreet)
		{
			if (this.age == 2)
			{
				if (this.socialAnxiety != 1)
				{
					return "Hi!";
				}
				return "Hi...";
			}
			else if (this.socialAnxiety == 1)
			{
				if (Game1.random.NextDouble() >= 0.5)
				{
					return "Hi.";
				}
				return "Hello.";
			}
			else if (this.socialAnxiety == 0)
			{
				if (Game1.random.NextDouble() < 0.33)
				{
					return "Hello!";
				}
				if (Game1.random.NextDouble() >= 0.5)
				{
					return "Hi, " + nameToGreet + "!";
				}
				return ((Game1.timeOfDay < 1200) ? "Good morning" : ((Game1.timeOfDay < 1700) ? "Good afternoon" : "Good evening")) + ", " + nameToGreet + "!";
			}
			else
			{
				if (Game1.random.NextDouble() < 0.33)
				{
					return "Hello.";
				}
				if (Game1.random.NextDouble() >= 0.5)
				{
					return "Hi.";
				}
				return "Hi, " + nameToGreet;
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0006D9D8 File Offset: 0x0006BBD8
		public bool isFacingToward(Vector2 tileLocation)
		{
			switch (this.facingDirection)
			{
			case 0:
				return (float)base.getTileY() > tileLocation.Y;
			case 1:
				return (float)base.getTileX() < tileLocation.X;
			case 2:
				return (float)base.getTileY() < tileLocation.Y;
			case 3:
				return (float)base.getTileX() > tileLocation.X;
			default:
				return false;
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0006DA48 File Offset: 0x0006BC48
		public void arriveAt(GameLocation l)
		{
			if (Game1.random.NextDouble() < 0.5 && this.dialogue != null && this.dialogue.ContainsKey(l.name + "_Entry"))
			{
				string[] split = this.dialogue[l.name + "_Entry"].Split(new char[]
				{
					'/'
				});
				this.showTextAboveHead(split[Game1.random.Next(split.Length)], -1, 2, 3000, 0);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0006DAD8 File Offset: 0x0006BCD8
		public override void Halt()
		{
			base.Halt();
			this.isCharging = false;
			this.speed = 2;
			this.addedSpeed = 0;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0006DAF5 File Offset: 0x0006BCF5
		public void addExtraDialogues(string dialogues)
		{
			if (this.updatedDialogueYet)
			{
				if (dialogues != null)
				{
					this.currentDialogue.Push(new Dialogue(dialogues, this));
					return;
				}
			}
			else
			{
				this.extraDialogueMessageToAddThisMorning = dialogues;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0006DB1C File Offset: 0x0006BD1C
		public string tryToGetMarriageSpecificDialogueElseReturnDefault(string dialogueKey, string defaultMessage = "")
		{
			Dictionary<string, string> marriageDialogues = null;
			try
			{
				marriageDialogues = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\MarriageDialogue" + this.name);
			}
			catch (Exception)
			{
			}
			if (marriageDialogues != null && marriageDialogues.ContainsKey(dialogueKey))
			{
				return marriageDialogues[dialogueKey];
			}
			marriageDialogues = Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\MarriageDialogue");
			if (marriageDialogues != null && marriageDialogues.ContainsKey(dialogueKey))
			{
				return marriageDialogues[dialogueKey];
			}
			return defaultMessage;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0006DB94 File Offset: 0x0006BD94
		public void updateDialogue()
		{
			this.updatedDialogueYet = true;
			int heartLevel = Game1.player.friendships.ContainsKey(this.name) ? (Game1.player.friendships[this.name][0] / 250) : 0;
			if (this.currentDialogue == null)
			{
				this.currentDialogue = new Stack<Dialogue>();
			}
			Random r = new Random((int)(Game1.stats.DaysPlayed * 77u + (uint)((int)Game1.uniqueIDForThisGame / 2) + 2u + (uint)((int)this.defaultPosition.X * 77) + (uint)((int)this.defaultPosition.Y * 777)));
			if (r.NextDouble() < 0.025 && heartLevel >= 1)
			{
				Dictionary<string, string> npcDispositions = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
				if (npcDispositions.ContainsKey(this.name))
				{
					string[] relatives = npcDispositions[this.name].Split(new char[]
					{
						'/'
					})[9].Split(new char[]
					{
						' '
					});
					if (relatives.Length > 1)
					{
						int index = r.Next(relatives.Length / 2) * 2;
						string relativeName = relatives[index];
						string relativeTitle = relatives[index + 1].Replace("'", "").Replace("_", " ");
						bool relativeIsMale = npcDispositions.ContainsKey(relativeName) && npcDispositions[relativeName].Split(new char[]
						{
							'/'
						})[4].Equals("male");
						Dictionary<string, string> npcGiftTastes = Game1.content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
						if (npcGiftTastes.ContainsKey(relativeName))
						{
							string itemName = null;
							string nameAndTitle = (relativeTitle.Length > 2) ? ("my " + relativeTitle) : relativeName;
							string message = "Did you know that " + nameAndTitle;
							if (r.NextDouble() < 0.5)
							{
								int item = Convert.ToInt32(npcGiftTastes[relativeName].Split(new char[]
								{
									'/'
								})[1].Split(new char[]
								{
									' '
								})[r.Next(npcGiftTastes[relativeName].Split(new char[]
								{
									'/'
								})[1].Split(new char[]
								{
									' '
								}).Length)]);
								if (Game1.objectInformation.ContainsKey(item))
								{
									itemName = Game1.objectInformation[item].Split(new char[]
									{
										'/'
									})[0];
									message = message + " loves '" + itemName + "'?";
									if (this.age == 2)
									{
										message = string.Concat(new string[]
										{
											relativeName,
											" loves '",
											itemName,
											"'! ",
											relativeIsMale ? "He" : "She",
											" told me.$h"
										});
									}
									else
									{
										switch (r.Next(5))
										{
										case 0:
											message = string.Concat(new string[]
											{
												"Here's a secret. ",
												nameAndTitle,
												" loves '",
												itemName,
												"'."
											});
											break;
										case 1:
											message = string.Concat(new string[]
											{
												"If you want ",
												nameAndTitle,
												" to like you, give ",
												relativeIsMale ? "him" : "her",
												" '",
												itemName,
												"'."
											});
											break;
										case 2:
											message = string.Concat(new string[]
											{
												"If you want to get on ",
												nameAndTitle,
												"'s good side, give ",
												relativeIsMale ? "him" : "her",
												" '",
												itemName,
												"'."
											});
											break;
										case 3:
											message = string.Concat(new string[]
											{
												"If you want to make friends with ",
												nameAndTitle,
												", you can't go wrong with '",
												itemName,
												"'."
											});
											break;
										}
										if (r.NextDouble() < 0.65)
										{
											switch (r.Next(5))
											{
											case 0:
												message = message + " It's " + (relativeIsMale ? "his" : "her") + " favorite.";
												break;
											case 1:
												message = message + (relativeIsMale ? " He" : " She") + " loves " + ((r.NextDouble() < 0.5) ? "it." : "that stuff.");
												break;
											case 2:
												message = string.Concat(new string[]
												{
													message,
													" I gave it to ",
													relativeIsMale ? "him" : "her",
													" one year and ",
													relativeIsMale ? "he" : "she",
													" wouldn't stop talking about it."
												});
												break;
											case 3:
												message += " It would make a great gift.";
												break;
											case 4:
												message = message + " You could really make " + (relativeIsMale ? "him" : "her") + " happy with that.";
												break;
											}
											if (relativeName.Equals("Abigail") && r.NextDouble() < 0.5)
											{
												message = relativeName + " loves '" + itemName + ". She keeps a box of it under her bed. Weird, huh?";
											}
										}
									}
								}
							}
							else
							{
								int item;
								try
								{
									item = Convert.ToInt32(npcGiftTastes[relativeName].Split(new char[]
									{
										'/'
									})[7].Split(new char[]
									{
										' '
									})[r.Next(npcGiftTastes[relativeName].Split(new char[]
									{
										'/'
									})[7].Split(new char[]
									{
										' '
									}).Length)]);
								}
								catch (Exception)
								{
									item = Convert.ToInt32(npcGiftTastes["Universal_Hate"].Split(new char[]
									{
										' '
									})[r.Next(npcGiftTastes["Universal_Hate"].Split(new char[]
									{
										' '
									}).Length)]);
								}
								if (Game1.objectInformation.ContainsKey(item))
								{
									itemName = Game1.objectInformation[item].Split(new char[]
									{
										'/'
									})[0];
									itemName = Game1.objectInformation[item].Split(new char[]
									{
										'/'
									})[0];
									message = string.Concat(new string[]
									{
										message,
										" hates '",
										itemName,
										"'? ",
										relativeIsMale ? "He" : "She",
										" finds it absolutely ",
										Lexicon.getRandomNegativeFoodAdjective(null),
										"."
									});
									if (this.age == 2)
									{
										message = string.Concat(new string[]
										{
											relativeName,
											" really hates '",
											itemName,
											"'! ",
											relativeIsMale ? "He" : "She",
											" told me.$h"
										});
									}
									else
									{
										switch (r.Next(4))
										{
										case 0:
											message = string.Concat(new string[]
											{
												(r.NextDouble() < 0.5) ? "A word of warning. " : "",
												nameAndTitle,
												" really hates '",
												itemName,
												"'."
											});
											break;
										case 1:
											message = string.Concat(new string[]
											{
												"If you want ",
												nameAndTitle,
												" to dislike you, give ",
												relativeIsMale ? "him" : "her",
												" '",
												itemName,
												"'. ",
												relativeIsMale ? "He" : "She",
												" hates ",
												(Game1.random.NextDouble() < 0.5) ? "it " : "that stuff ",
												"with a passion."
											});
											break;
										case 2:
											message = string.Concat(new string[]
											{
												"If you want to play a cruel joke on ",
												nameAndTitle,
												", give ",
												relativeIsMale ? "him" : "her",
												" '",
												itemName,
												"'. ",
												relativeIsMale ? "He" : "She",
												" might not forgive you, though."
											});
											break;
										}
										if (r.NextDouble() < 0.65)
										{
											switch (r.Next(5))
											{
											case 0:
												message += " I learned that one the hard way.";
												break;
											case 1:
												message += " I'm not sure why.";
												break;
											case 2:
												message = message + " Just the thought of it can make " + (relativeIsMale ? "him" : "her") + " depressed.";
												break;
											case 3:
												message = message + " I think " + (relativeIsMale ? "he" : "she") + " might be allergic.";
												break;
											case 4:
												message += " I guess everyone has their hang-ups.";
												break;
											}
											if (this.name.Equals("Lewis") && r.NextDouble() < 0.5)
											{
												message = relativeName + " hates '" + itemName + ". You don't want to know the details. Trust me.";
											}
										}
									}
								}
							}
							if (itemName != null)
							{
								this.currentDialogue.Clear();
								if (message.Length > 0)
								{
									try
									{
										message = message.Substring(0, 1).ToUpper() + message.Substring(1, message.Length - 1);
									}
									catch (Exception)
									{
									}
								}
								this.currentDialogue.Push(new Dialogue(message, this));
								return;
							}
						}
					}
				}
			}
			if (this.dialogue != null)
			{
				string currentDialogueStr = "";
				this.currentDialogue.Clear();
				if (Game1.player.spouse != null && Game1.player.spouse.Contains(this.name))
				{
					if (Game1.player.spouse.Equals(this.name + "engaged"))
					{
						this.currentDialogue.Push(new Dialogue(Game1.content.Load<Dictionary<string, string>>("Data\\EngagementDialogue")[this.name + r.Next(2)], this));
					}
					else if (Game1.isRaining)
					{
						this.currentDialogue.Push(new Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Rainy_Day_" + r.Next(5), ""), this));
					}
					else
					{
						this.currentDialogue.Push(new Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Indoor_Day_" + r.Next(5), ""), this));
					}
				}
				else if (this.idForClones == -1)
				{
					if (this.divorcedFromFarmer)
					{
						try
						{
							this.currentDialogue.Push(new Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + this.name)["divorced"], this));
							return;
						}
						catch (Exception)
						{
						}
					}
					if (Game1.isRaining && r.NextDouble() < 0.5)
					{
						try
						{
							this.currentDialogue.Push(new Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\rainy")[this.name], this));
							return;
						}
						catch (Exception)
						{
						}
					}
					Dialogue d = this.tryToRetrieveDialogue(Game1.currentSeason + "_", heartLevel, "");
					if (d == null)
					{
						d = this.tryToRetrieveDialogue("", heartLevel, "");
					}
					if (d == null)
					{
						d = new Dialogue("Hi.", this);
					}
					this.currentDialogue.Push(d);
				}
				else
				{
					this.dialogue.TryGetValue(string.Concat(this.idForClones), out currentDialogueStr);
					this.currentDialogue.Push(new Dialogue(currentDialogueStr, this));
				}
			}
			else if (this.name.Equals("Bouncer"))
			{
				this.currentDialogue.Push(new Dialogue("...#$e#...#$e#...#$e#It's for members only, kid. Now Scram.#$e#...", this));
			}
			if (this.extraDialogueMessageToAddThisMorning != null)
			{
				this.currentDialogue.Push(new Dialogue(this.extraDialogueMessageToAddThisMorning, this));
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0006E80C File Offset: 0x0006CA0C
		public bool checkForNewCurrentDialogue(int heartLevel, bool noPreface = false)
		{
			string eventMessageKey = "";
			foreach (string s in Game1.player.activeDialogueEvents.Keys)
			{
				if (this.dialogue.ContainsKey(s))
				{
					eventMessageKey = s;
					break;
				}
			}
			string preface = (!Game1.currentSeason.Equals("spring") && !noPreface) ? Game1.currentSeason : "";
			if (!eventMessageKey.Equals("") && !Game1.player.mailReceived.Contains(this.name + "_" + eventMessageKey))
			{
				this.currentDialogue.Clear();
				this.currentDialogue.Push(new Dialogue(this.dialogue[eventMessageKey], this));
				Game1.player.mailReceived.Add(this.name + "_" + eventMessageKey);
				return true;
			}
			if (this.dialogue.ContainsKey(string.Concat(new object[]
			{
				preface,
				Game1.currentLocation.name,
				"_",
				base.getTileX(),
				"_",
				base.getTileY()
			})))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.currentLocation.name,
					"_",
					base.getTileX(),
					"_",
					base.getTileY()
				})], this));
				return true;
			}
			if (this.dialogue.ContainsKey(preface + Game1.currentLocation.name + "_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name + "_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)], this));
				return true;
			}
			if (heartLevel >= 10 && this.dialogue.ContainsKey(preface + Game1.currentLocation.name + "10"))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name + "10"], this));
				return true;
			}
			if (heartLevel >= 8 && this.dialogue.ContainsKey(preface + Game1.currentLocation.name + "8"))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name + "8"], this));
				return true;
			}
			if (heartLevel >= 6 && this.dialogue.ContainsKey(preface + Game1.currentLocation.name + "6"))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name + "6"], this));
				return true;
			}
			if (heartLevel >= 4 && this.dialogue.ContainsKey(preface + Game1.currentLocation.name + "4"))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name + "4"], this));
				return true;
			}
			if (heartLevel >= 2 && this.dialogue.ContainsKey(preface + Game1.currentLocation.name + "2"))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name + "2"], this));
				return true;
			}
			if (this.dialogue.ContainsKey(preface + Game1.currentLocation.name))
			{
				this.currentDialogue.Push(new Dialogue(this.dialogue[preface + Game1.currentLocation.name], this));
				return true;
			}
			return false;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0006EC44 File Offset: 0x0006CE44
		public Dialogue tryToRetrieveDialogue(string preface, int heartLevel, string appendToEnd = "")
		{
			int year = Game1.year;
			if (Game1.year > 2)
			{
				year = 2;
			}
			if (Game1.player.spouse != null && Game1.player.spouse.Length > 0 && appendToEnd.Equals(""))
			{
				Dialogue s = this.tryToRetrieveDialogue(preface, heartLevel, "_inlaw_" + Game1.player.spouse);
				if (s != null)
				{
					return s;
				}
			}
			if (this.dialogue.ContainsKey(preface + Game1.dayOfMonth + appendToEnd) && year == 1)
			{
				return new Dialogue(this.dialogue[preface + Game1.dayOfMonth + appendToEnd], this);
			}
			if (this.dialogue.ContainsKey(string.Concat(new object[]
			{
				preface,
				Game1.dayOfMonth,
				"_",
				year,
				appendToEnd
			})))
			{
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.dayOfMonth,
					"_",
					year,
					appendToEnd
				})], this);
			}
			if (heartLevel >= 10 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "10" + appendToEnd))
			{
				if (!this.dialogue.ContainsKey(string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"10_",
					year,
					appendToEnd
				})))
				{
					return new Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "10" + appendToEnd], this);
				}
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"10_",
					year,
					appendToEnd
				})], this);
			}
			else if (heartLevel >= 8 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "8" + appendToEnd))
			{
				if (!this.dialogue.ContainsKey(string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"8_",
					year,
					appendToEnd
				})))
				{
					return new Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "8" + appendToEnd], this);
				}
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"8_",
					year,
					appendToEnd
				})], this);
			}
			else if (heartLevel >= 6 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "6" + appendToEnd))
			{
				if (!this.dialogue.ContainsKey(string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"6_",
					year
				})))
				{
					return new Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "6" + appendToEnd], this);
				}
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"6_",
					year,
					appendToEnd
				})], this);
			}
			else if (heartLevel >= 4 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "4" + appendToEnd))
			{
				if (!this.dialogue.ContainsKey(string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"4_",
					year
				})))
				{
					return new Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "4" + appendToEnd], this);
				}
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"4_",
					year,
					appendToEnd
				})], this);
			}
			else if (heartLevel >= 2 && this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "2" + appendToEnd))
			{
				if (!this.dialogue.ContainsKey(string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"2_",
					year
				})))
				{
					return new Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + "2" + appendToEnd], this);
				}
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"2_",
					year,
					appendToEnd
				})], this);
			}
			else
			{
				if (!this.dialogue.ContainsKey(preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + appendToEnd))
				{
					return null;
				}
				if (!this.dialogue.ContainsKey(string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"_",
					year,
					appendToEnd
				})))
				{
					return new Dialogue(this.dialogue[preface + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth) + appendToEnd], this);
				}
				return new Dialogue(this.dialogue[string.Concat(new object[]
				{
					preface,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
					"_",
					year,
					appendToEnd
				})], this);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0006F264 File Offset: 0x0006D464
		public void checkSchedule(int timeOfDay)
		{
			this.updatedDialogueYet = false;
			this.extraDialogueMessageToAddThisMorning = null;
			if (this.ignoreScheduleToday)
			{
				return;
			}
			if (this.schedule != null)
			{
				SchedulePathDescription possibleNewDirections;
				this.schedule.TryGetValue((this.scheduleTimeToTry == 9999999) ? timeOfDay : this.scheduleTimeToTry, out possibleNewDirections);
				if (possibleNewDirections != null)
				{
					if (!this.isMarried() && (!this.isWalkingInSquare || (this.lastCrossroad.Center.X / Game1.tileSize != this.previousEndPoint.X && this.lastCrossroad.Y / Game1.tileSize != this.previousEndPoint.Y)))
					{
						Point arg_A6_0 = this.previousEndPoint;
						if (!this.previousEndPoint.Equals(Point.Zero) && !this.previousEndPoint.Equals(base.getTileLocationPoint()))
						{
							if (this.scheduleTimeToTry == 9999999)
							{
								this.scheduleTimeToTry = timeOfDay;
								return;
							}
							return;
						}
					}
					this.directionsToNewLocation = possibleNewDirections;
					this.prepareToDisembarkOnNewSchedulePath();
					if (this.schedule == null)
					{
						return;
					}
					if (this.directionsToNewLocation != null && this.directionsToNewLocation.route != null && this.directionsToNewLocation.route.Count > 0 && (Math.Abs(base.getTileLocationPoint().X - this.directionsToNewLocation.route.Peek().X) > 1 || Math.Abs(base.getTileLocationPoint().Y - this.directionsToNewLocation.route.Peek().Y) > 1) && this.temporaryController == null)
					{
						this.scheduleTimeToTry = 9999999;
						return;
					}
					this.controller = new PathFindController(this.directionsToNewLocation.route, this, Utility.getGameLocationOfCharacter(this))
					{
						finalFacingDirection = this.directionsToNewLocation.facingDirection,
						endBehaviorFunction = this.getRouteEndBehaviorFunction(this.directionsToNewLocation.endOfRouteBehavior, this.directionsToNewLocation.endOfRouteMessage)
					};
					this.scheduleTimeToTry = 9999999;
					try
					{
						this.previousEndPoint = ((this.directionsToNewLocation.route.Count > 0) ? this.directionsToNewLocation.route.Last<Point>() : Point.Zero);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0006F49C File Offset: 0x0006D69C
		private void prepareToDisembarkOnNewSchedulePath()
		{
			while (this.CurrentDialogue.Count > 0 && this.CurrentDialogue.Peek().removeOnNextMove)
			{
				this.CurrentDialogue.Pop();
			}
			this.nextEndOfRouteMessage = null;
			this.endOfRouteMessage = null;
			if (this.doingEndOfRouteAnimation)
			{
				List<FarmerSprite.AnimationFrame> outro = new List<FarmerSprite.AnimationFrame>();
				for (int i = 0; i < this.routeEndOutro.Length; i++)
				{
					if (i == this.routeEndOutro.Length - 1)
					{
						outro.Add(new FarmerSprite.AnimationFrame(this.routeEndOutro[i], 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.routeEndAnimationFinished), true, 0));
					}
					else
					{
						outro.Add(new FarmerSprite.AnimationFrame(this.routeEndOutro[i], 100, 0, false, false, null, false, 0));
					}
				}
				if (outro.Count > 0)
				{
					this.sprite.setCurrentAnimation(outro);
				}
				else
				{
					this.routeEndAnimationFinished(null);
				}
				if (this.endOfRouteBehaviorName != null)
				{
					this.finishRouteBehavior(this.endOfRouteBehaviorName);
				}
			}
			else
			{
				this.routeEndAnimationFinished(null);
			}
			if (this.isMarried())
			{
				if (this.temporaryController == null && Utility.getGameLocationOfCharacter(this) is FarmHouse)
				{
					this.temporaryController = new PathFindController(this, this.getHome(), new Point(this.getHome().warps[0].X, this.getHome().warps[0].Y), 2, true)
					{
						NPCSchedule = true
					};
					if (this.temporaryController.pathToEndPoint == null || this.temporaryController.pathToEndPoint.Count <= 0)
					{
						this.temporaryController = null;
						this.schedule = null;
						return;
					}
					this.followSchedule = true;
					return;
				}
				else if (Utility.getGameLocationOfCharacter(this) is Farm)
				{
					this.temporaryController = null;
					this.schedule = null;
				}
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0006F658 File Offset: 0x0006D858
		public void checkForMarriageDialogue(int timeOfDay, GameLocation location)
		{
			if (timeOfDay == 1100)
			{
				this.setRandomAfternoonMarriageDialogue(1100, location, true);
				return;
			}
			if (Game1.timeOfDay == 1800 && location is FarmHouse)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + timeOfDay);
				this.setNewDialogue("MarriageDialogue", (Game1.isRaining ? "Rainy" : "Indoor") + "_Night_", r.Next(6) - 1, false, false);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0006F6E0 File Offset: 0x0006D8E0
		private void routeEndAnimationFinished(Farmer who)
		{
			this.doingEndOfRouteAnimation = false;
			this.freezeMotion = false;
			this.sprite.spriteHeight = 32;
			this.sprite.StopAnimation();
			this.endOfRouteMessage = null;
			this.isCharging = false;
			this.speed = 2;
			this.addedSpeed = 0;
			this.goingToDoEndOfRouteAnimation = false;
			if (this.isWalkingInSquare)
			{
				this.returningToEndPoint = true;
				this.timeAfterSquare = Game1.timeOfDay;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0006F750 File Offset: 0x0006D950
		private void setMessageAtEndOfScheduleRoute(Character c, GameLocation l)
		{
			this.endOfRouteMessage = this.nextEndOfRouteMessage;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0006F75E File Offset: 0x0006D95E
		public bool isOnSilentTemporaryMessage()
		{
			return (this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation) && this.endOfRouteMessage != null && this.endOfRouteMessage.ToLower().Equals("silent");
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0006F792 File Offset: 0x0006D992
		public bool hasTemporaryMessageAvailable()
		{
			return this.endOfRouteMessage != null && (this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0006F7B0 File Offset: 0x0006D9B0
		public bool setTemporaryMessages(Farmer who)
		{
			if (this.isOnSilentTemporaryMessage())
			{
				return true;
			}
			if (this.endOfRouteMessage != null && (this.doingEndOfRouteAnimation || !this.goingToDoEndOfRouteAnimation))
			{
				this.CurrentDialogue.Push(new Dialogue(this.endOfRouteMessage, this)
				{
					removeOnNextMove = true
				});
			}
			return false;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0006F7FE File Offset: 0x0006D9FE
		private void walkInSquareAtEndOfRoute(Character c, GameLocation l)
		{
			this.startRouteBehavior(this.endOfRouteBehaviorName);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0006F80C File Offset: 0x0006DA0C
		private void doAnimationAtEndOfScheduleRoute(Character c, GameLocation l)
		{
			List<FarmerSprite.AnimationFrame> intro = new List<FarmerSprite.AnimationFrame>();
			for (int i = 0; i < this.routeEndIntro.Length; i++)
			{
				if (i == this.routeEndIntro.Length - 1)
				{
					intro.Add(new FarmerSprite.AnimationFrame(this.routeEndIntro[i], 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doMiddleAnimation), true, 0));
				}
				else
				{
					intro.Add(new FarmerSprite.AnimationFrame(this.routeEndIntro[i], 100, 0, false, false, null, false, 0));
				}
			}
			this.doingEndOfRouteAnimation = true;
			this.freezeMotion = true;
			this.sprite.setCurrentAnimation(intro);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0006F89C File Offset: 0x0006DA9C
		private void doMiddleAnimation(Farmer who)
		{
			List<FarmerSprite.AnimationFrame> anim = new List<FarmerSprite.AnimationFrame>();
			for (int i = 0; i < this.routeEndAnimation.Length; i++)
			{
				anim.Add(new FarmerSprite.AnimationFrame(this.routeEndAnimation[i], 100, 0, false, false, null, false, 0));
			}
			this.sprite.setCurrentAnimation(anim);
			this.sprite.loop = true;
			if (this.endOfRouteBehaviorName != null)
			{
				this.startRouteBehavior(this.endOfRouteBehaviorName);
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0006F90C File Offset: 0x0006DB0C
		private void startRouteBehavior(string behaviorName)
		{
			if (behaviorName.Length > 0 && behaviorName[0] == '"')
			{
				this.endOfRouteMessage = behaviorName.Replace("\"", "");
				return;
			}
			if (behaviorName.Contains("square_"))
			{
				this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(base.getTileX() * Game1.tileSize, base.getTileY() * Game1.tileSize, Game1.tileSize, Game1.tileSize);
				string[] squareSplit = behaviorName.Split(new char[]
				{
					'_'
				});
				this.walkInSquare(Convert.ToInt32(squareSplit[1]), Convert.ToInt32(squareSplit[2]), 6000);
				if (squareSplit.Length > 3)
				{
					this.squareMovementFacingPreference = Convert.ToInt32(squareSplit[3]);
				}
				else
				{
					this.squareMovementFacingPreference = -1;
				}
			}
			if (!(behaviorName == "abigail_videogames"))
			{
				if (!(behaviorName == "dick_fish"))
				{
					if (!(behaviorName == "clint_hammer"))
					{
						return;
					}
					base.extendSourceRect(16, 0, true);
					this.sprite.spriteWidth = 32;
					this.sprite.ignoreSourceRectUpdates = false;
					this.sprite.CurrentFrame = 8;
					this.sprite.currentAnimation[14] = new FarmerSprite.AnimationFrame(9, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.clintHammerSound), false, 0);
				}
				else
				{
					base.extendSourceRect(0, 32, true);
					if (Utility.isOnScreen(Utility.Vector2ToPoint(this.position), Game1.tileSize, this.currentLocation))
					{
						Game1.playSound("slosh");
						return;
					}
				}
				return;
			}
			Utility.getGameLocationOfCharacter(this).temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(167, 1714, 19, 14), 100f, 3, 999999, new Vector2(2f, 3f) * (float)Game1.tileSize + new Vector2(7f, 12f) * (float)Game1.pixelZoom, false, false, 0.0002f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
			{
				id = 688f
			});
			base.doEmote(52, true);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0006FB30 File Offset: 0x0006DD30
		private void finishRouteBehavior(string behaviorName)
		{
			if (behaviorName == "abigail_videogames")
			{
				Utility.getGameLocationOfCharacter(this).removeTemporarySpritesWithID(688);
				return;
			}
			if (!(behaviorName == "clint_hammer") && !(behaviorName == "dick_fish"))
			{
				return;
			}
			this.reloadSprite();
			this.sprite.spriteWidth = 16;
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
			this.Halt();
			this.movementPause = 1;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0006FBB0 File Offset: 0x0006DDB0
		private PathFindController.endBehavior getRouteEndBehaviorFunction(string behaviorName, string endMessage)
		{
			if (endMessage != null || (behaviorName != null && behaviorName.Length > 0 && behaviorName[0] == '"'))
			{
				this.nextEndOfRouteMessage = endMessage.Replace("\"", "");
			}
			if (behaviorName == null)
			{
				return null;
			}
			if (behaviorName.Length > 0 && behaviorName.Contains("square_"))
			{
				this.endOfRouteBehaviorName = behaviorName;
				return new PathFindController.endBehavior(this.walkInSquareAtEndOfRoute);
			}
			Dictionary<string, string> animationDescriptions = Game1.content.Load<Dictionary<string, string>>("Data\\animationDescriptions");
			if (animationDescriptions.ContainsKey(behaviorName))
			{
				this.endOfRouteBehaviorName = behaviorName;
				string[] rawData = animationDescriptions[behaviorName].Split(new char[]
				{
					'/'
				});
				this.routeEndIntro = Utility.parseStringToIntArray(rawData[0], ' ');
				this.routeEndAnimation = Utility.parseStringToIntArray(rawData[1], ' ');
				this.routeEndOutro = Utility.parseStringToIntArray(rawData[2], ' ');
				if (rawData.Length > 3)
				{
					this.nextEndOfRouteMessage = rawData[3];
				}
				this.goingToDoEndOfRouteAnimation = true;
				return new PathFindController.endBehavior(this.doAnimationAtEndOfScheduleRoute);
			}
			return null;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00002834 File Offset: 0x00000A34
		public void warp(bool wasOutdoors)
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0006FCAD File Offset: 0x0006DEAD
		public void shake(int duration)
		{
			this.shakeTimer = duration;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0006FCB6 File Offset: 0x0006DEB6
		public void setNewDialogue(string s, bool add = false, bool clearOnMovement = false)
		{
			if (!add)
			{
				this.CurrentDialogue.Clear();
			}
			this.CurrentDialogue.Push(new Dialogue(s, this)
			{
				removeOnNextMove = clearOnMovement
			});
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0006FCE0 File Offset: 0x0006DEE0
		public void setNewDialogue(string dialogueSheetName, string dialogueSheetKey, int numberToAppend = -1, bool add = false, bool clearOnMovement = false)
		{
			if (!add)
			{
				this.CurrentDialogue.Clear();
			}
			string nameToAppend = (numberToAppend == -1) ? this.name : "";
			if (dialogueSheetName.Contains("Marriage"))
			{
				this.CurrentDialogue.Push(new Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault(dialogueSheetKey + ((numberToAppend != -1) ? string.Concat(numberToAppend) : "") + nameToAppend, ""), this)
				{
					removeOnNextMove = clearOnMovement
				});
				return;
			}
			if (Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + dialogueSheetName).ContainsKey(dialogueSheetKey + ((numberToAppend != -1) ? string.Concat(numberToAppend) : "") + nameToAppend))
			{
				this.CurrentDialogue.Push(new Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + dialogueSheetName)[dialogueSheetKey + ((numberToAppend != -1) ? string.Concat(numberToAppend) : "") + nameToAppend], this)
				{
					removeOnNextMove = clearOnMovement
				});
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0006FDE6 File Offset: 0x0006DFE6
		public void setSpouseRoomMarriageDialogue()
		{
			this.setNewDialogue("MarriageDialogue", "spouseRoom_", -1, false, true);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0006FDFC File Offset: 0x0006DFFC
		public void setRandomAfternoonMarriageDialogue(int time, GameLocation location, bool countAsDailyAfternoon = false)
		{
			if (this.hasSaidAfternoonDialogue || this.getSpouse() == null)
			{
				return;
			}
			if (countAsDailyAfternoon)
			{
				this.hasSaidAfternoonDialogue = true;
			}
			Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + time);
			int hearts = this.getSpouse().getFriendshipHeartLevelForNPC(this.name);
			if (!(location is FarmHouse) || r.NextDouble() >= 0.5)
			{
				if (location is Farm)
				{
					if (r.NextDouble() < 0.2)
					{
						this.setNewDialogue("MarriageDialogue", "Outdoor_", -1, false, false);
						return;
					}
					this.setNewDialogue("MarriageDialogue", "Outdoor_", r.Next(5), false, false);
				}
				return;
			}
			if (hearts < 9)
			{
				this.setNewDialogue("MarriageDialogue", (r.NextDouble() < (double)((float)hearts / 11f)) ? "Neutral_" : "Bad_", r.Next(10), false, false);
				return;
			}
			if (r.NextDouble() < 0.05)
			{
				this.setNewDialogue("MarriageDialogue", Game1.currentSeason + "_", -1, false, false);
				return;
			}
			if ((hearts >= 10 && r.NextDouble() < 0.5) || (hearts >= 11 && r.NextDouble() < 0.75))
			{
				this.setNewDialogue("MarriageDialogue", "Good_", r.Next(10), false, false);
				return;
			}
			this.setNewDialogue("MarriageDialogue", "Neutral_", r.Next(10), false, false);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0006FF7F File Offset: 0x0006E17F
		public bool isBirthday(string season, int day)
		{
			return this.birthday_Season != null && (this.birthday_Season.Equals(season) && this.birthday_Day == day);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0006FFA8 File Offset: 0x0006E1A8
		public Object getFavoriteItem()
		{
			string NPCLikes;
			Game1.NPCGiftTastes.TryGetValue(this.name, out NPCLikes);
			if (NPCLikes != null)
			{
				return new Object(Convert.ToInt32(NPCLikes.Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[0]), 1, false, -1, 0);
			}
			return null;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00070000 File Offset: 0x0006E200
		public void receiveGift(Object o, Farmer giver, bool updateGiftLimitInfo = true, float friendshipChangeMultiplier = 1f, bool showResponse = true)
		{
			string NPCLikes;
			Game1.NPCGiftTastes.TryGetValue(this.name, out NPCLikes);
			string[] split = NPCLikes.Split(new char[]
			{
				'/'
			});
			float qualityChangeMultipler = 1f;
			switch (o.quality)
			{
			case 1:
				qualityChangeMultipler = 1.1f;
				break;
			case 2:
				qualityChangeMultipler = 1.25f;
				break;
			case 4:
				qualityChangeMultipler = 1.5f;
				break;
			}
			if (this.birthday_Season != null && Game1.currentSeason.Equals(this.birthday_Season) && Game1.dayOfMonth == this.birthday_Day)
			{
				friendshipChangeMultiplier = 8f;
				string positiveBirthdayMessage = (this.manners == 2) ? "You remembered my birthday? I'm impressed. Thanks.$h" : "A birthday gift? That's very kind of you! I love it.$h";
				if (Game1.random.NextDouble() < 0.5)
				{
					positiveBirthdayMessage = ((this.manners == 2) ? "Oh, is it my birthday today? I guess it is. Thanks. This is nice.$h" : "You remembered my birthday! Thank you. This is great.$h");
				}
				string negativeBirthdayMessage = (this.manners == 2) ? "It's my birthday and you give me this? Is this some kind of joke?$s" : "Oh... It's for my birthday? ... Thanks.$s";
				split[0] = positiveBirthdayMessage;
				split[2] = positiveBirthdayMessage;
				split[4] = negativeBirthdayMessage;
				split[6] = negativeBirthdayMessage;
				split[8] = ((this.manners == 2) ? "For my birthday? Thanks." : "Oh, a birthday gift! Thank you.");
			}
			if (NPCLikes != null)
			{
				Stats expr_129 = Game1.stats;
				uint giftsGiven = expr_129.GiftsGiven;
				expr_129.GiftsGiven = giftsGiven + 1u;
				Game1.playSound("give_gift");
				if (updateGiftLimitInfo)
				{
					giver.friendships[this.name][3] = 1;
				}
				int tasteForItem = this.getGiftTasteForThisItem(o);
				switch (giver.facingDirection)
				{
				case 0:
					((FarmerSprite)giver.Sprite).animateBackwardsOnce(80, 50f);
					break;
				case 1:
					((FarmerSprite)giver.Sprite).animateBackwardsOnce(72, 50f);
					break;
				case 2:
					((FarmerSprite)giver.Sprite).animateBackwardsOnce(64, 50f);
					break;
				case 3:
					((FarmerSprite)giver.Sprite).animateBackwardsOnce(88, 50f);
					break;
				}
				if (updateGiftLimitInfo)
				{
					giver.friendships[this.name][1]++;
				}
				List<string> reactions = new List<string>();
				for (int i = 0; i < 8; i += 2)
				{
					reactions.Add(split[i]);
				}
				if (tasteForItem == 0)
				{
					if (this.name.Contains("Dwarf"))
					{
						if (showResponse)
						{
							Game1.drawDialogue(this, giver.canUnderstandDwarves ? reactions[0] : StardewValley.Dialogue.convertToDwarvish(reactions[0]));
						}
					}
					else if (showResponse)
					{
						Game1.drawDialogue(this, reactions[0] + "$h");
					}
					giver.changeFriendship((int)(80f * friendshipChangeMultiplier * qualityChangeMultipler), this);
					base.doEmote(20, true);
					base.faceTowardFarmerForPeriod(15000, 4, false, giver);
					return;
				}
				if (tasteForItem == 6)
				{
					if (this.name.Contains("Dwarf"))
					{
						if (showResponse)
						{
							Game1.drawDialogue(this, giver.canUnderstandDwarves ? reactions[3] : StardewValley.Dialogue.convertToDwarvish(reactions[3]));
						}
					}
					else if (showResponse)
					{
						Game1.drawDialogue(this, reactions[3] + "$s");
					}
					giver.changeFriendship((int)(-40f * friendshipChangeMultiplier), this);
					base.faceTowardFarmerForPeriod(15000, 4, true, giver);
					base.doEmote(12, true);
					return;
				}
				if (tasteForItem == 2)
				{
					if (this.name.Contains("Dwarf"))
					{
						if (showResponse)
						{
							Game1.drawDialogue(this, giver.canUnderstandDwarves ? reactions[1] : StardewValley.Dialogue.convertToDwarvish(reactions[1]));
						}
					}
					else if (showResponse)
					{
						Game1.drawDialogue(this, reactions[1] + "$h");
					}
					giver.changeFriendship((int)(45f * friendshipChangeMultiplier * qualityChangeMultipler), this);
					base.faceTowardFarmerForPeriod(7000, 3, true, giver);
					return;
				}
				if (tasteForItem == 4)
				{
					if (this.name.Contains("Dwarf"))
					{
						if (showResponse)
						{
							Game1.drawDialogue(this, giver.canUnderstandDwarves ? reactions[2] : StardewValley.Dialogue.convertToDwarvish(reactions[2]));
						}
					}
					else if (showResponse)
					{
						Game1.drawDialogue(this, reactions[2] + "$s");
					}
					giver.changeFriendship((int)(-20f * friendshipChangeMultiplier), this);
					return;
				}
				if (this.name.Contains("Dwarf"))
				{
					if (showResponse)
					{
						Game1.drawDialogue(this, giver.canUnderstandDwarves ? reactions[2] : StardewValley.Dialogue.convertToDwarvish(reactions[2]));
					}
				}
				else if (showResponse)
				{
					Game1.drawDialogue(this, split[8]);
				}
				giver.changeFriendship((int)(20f * friendshipChangeMultiplier), this);
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00070490 File Offset: 0x0006E690
		public override void draw(SpriteBatch b, float alpha = 1f)
		{
			if (this.sprite != null && !this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				if (this.swimming)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + Game1.tileSize / 4 + this.yJumpOffset * 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero) - new Vector2(0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, this.sprite.SourceRect.Width, this.sprite.SourceRect.Height / 2 - (int)(this.yOffset / (float)Game1.pixelZoom))), Color.White, this.rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 2)) / 4f, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
					Vector2 localPosition = base.getLocalPosition(Game1.viewport);
					b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)localPosition.X + (int)this.yOffset + Game1.pixelZoom * 2, (int)localPosition.Y - 32 * Game1.pixelZoom + this.sprite.SourceRect.Height * Game1.pixelZoom + Game1.tileSize * 3 / 4 + this.yJumpOffset * 2 - (int)this.yOffset, this.sprite.SourceRect.Width * Game1.pixelZoom - (int)this.yOffset * 2 - Game1.pixelZoom * 4, Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0f, Vector2.Zero, SpriteEffects.None, (float)base.getStandingY() / 10000f + 0.001f);
				}
				else
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White * alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)this.sprite.spriteHeight * 3f / 4f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, (this.flip || (this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				}
				if (this.breather && this.shakeTimer <= 0 && !this.swimming && this.sprite.CurrentFrame < 16 && !this.farmerPassesThrough)
				{
					Microsoft.Xna.Framework.Rectangle chestBox = this.sprite.SourceRect;
					chestBox.Y += this.sprite.spriteHeight / 2 + this.sprite.spriteHeight / 32;
					chestBox.Height = this.sprite.spriteHeight / 4;
					chestBox.X += this.sprite.spriteWidth / 4;
					chestBox.Width = this.sprite.spriteWidth / 2;
					Vector2 chestPosition = new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(Game1.tileSize / 8));
					if (this.age == 2)
					{
						chestBox.Y += this.sprite.spriteHeight / 6 + 1;
						chestBox.Height /= 2;
						chestPosition.Y += (float)(this.sprite.spriteHeight / 8 * Game1.pixelZoom);
						if (this is Child)
						{
							if ((this as Child).age == 0)
							{
								chestPosition.X -= (float)(Game1.pixelZoom * 3);
							}
							else if ((this as Child).age == 1)
							{
								chestPosition.X -= (float)Game1.pixelZoom;
							}
						}
					}
					else if (this.gender == 1)
					{
						chestBox.Y++;
						chestPosition.Y -= (float)Game1.pixelZoom;
						chestBox.Height /= 2;
					}
					float breathScale = Math.Max(0f, (float)Math.Ceiling(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 600.0 + (double)(this.defaultPosition.X * 20f))) / 4f);
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + chestPosition + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(chestBox), Color.White * alpha, this.rotation, new Vector2((float)(chestBox.Width / 2), (float)(chestBox.Height / 2 + 1)), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom + breathScale, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.992f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2((float)(this.sprite.spriteWidth / 2), (float)this.sprite.spriteHeight * 3f / 4f), Math.Max(0.2f, this.scale) * 4f, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
				if (base.IsEmoting && !Game1.eventUp && !(this is Child) && !(this is Pet))
				{
					Vector2 emotePosition = base.getLocalPosition(Game1.viewport);
					emotePosition.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom);
					b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)base.getStandingY() / 10000f);
				}
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00070D0C File Offset: 0x0006EF0C
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (this.textAboveHeadTimer > 0 && this.textAboveHead != null)
			{
				Vector2 local = Game1.GlobalToLocal(new Vector2((float)base.getStandingX(), (float)(base.getStandingY() - Game1.tileSize * 3 + this.yJumpOffset)));
				if (this.textAboveHeadStyle == 0)
				{
					local += new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2));
				}
				SpriteText.drawStringWithScrollCenteredAt(b, this.textAboveHead, (int)local.X, (int)local.Y, "", this.textAboveHeadAlpha, this.textAboveHeadColor, 1, (float)(base.getTileY() * Game1.tileSize) / 10000f + 0.001f + (float)base.getTileX() / 10000f, false);
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00070DDC File Offset: 0x0006EFDC
		public void warpToPathControllerDestination()
		{
			if (this.controller != null)
			{
				while (this.controller.pathToEndPoint.Count > 2)
				{
					this.controller.pathToEndPoint.Pop();
					this.position = new Vector2((float)(this.controller.pathToEndPoint.Peek().X * Game1.tileSize), (float)(this.controller.pathToEndPoint.Peek().Y * Game1.tileSize + Game1.tileSize / 4));
					this.Halt();
				}
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00070E68 File Offset: 0x0006F068
		public virtual Microsoft.Xna.Framework.Rectangle getMugShotSourceRect()
		{
			return new Microsoft.Xna.Framework.Rectangle(0, (this.age == 2) ? 4 : 0, 16, 24);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00070E84 File Offset: 0x0006F084
		public void getHitByPlayer(Farmer who, GameLocation location)
		{
			base.doEmote(12, true);
			if (who == null)
			{
				if (Game1.IsMultiplayer)
				{
					return;
				}
				who = Game1.player;
			}
			if (who.friendships.ContainsKey(this.name))
			{
				who.friendships[this.name][0] -= 30;
				if (who.IsMainPlayer)
				{
					this.CurrentDialogue.Clear();
					this.CurrentDialogue.Push(new Dialogue((Game1.random.NextDouble() < 0.5) ? "Ow! I can't believe you would do that to me!$s" : "That hurt! What's your problem?$s", this));
				}
				location.debris.Add(new Debris(this.sprite.Texture, Game1.random.Next(3, 8), new Vector2((float)this.GetBoundingBox().Center.X, (float)this.GetBoundingBox().Center.Y)));
			}
			if (this.name.Equals("Bouncer"))
			{
				Game1.playSound("crafting");
				return;
			}
			Game1.playSound("hitEnemy");
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00070FA1 File Offset: 0x0006F1A1
		public void walkInSquare(int squareWidth, int squareHeight, int squarePauseOffset)
		{
			this.isWalkingInSquare = true;
			this.lengthOfWalkingSquareX = squareWidth;
			this.lengthOfWalkingSquareY = squareHeight;
			this.squarePauseOffset = squarePauseOffset;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00070FBF File Offset: 0x0006F1BF
		public void moveTowardPlayer(int threshold)
		{
			this.isWalkingTowardPlayer = true;
			this.moveTowardPlayerThreshold = threshold;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00070FCF File Offset: 0x0006F1CF
		public virtual bool withinPlayerThreshold()
		{
			return this.withinPlayerThreshold(this.moveTowardPlayerThreshold);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00070FE0 File Offset: 0x0006F1E0
		public bool withinPlayerThreshold(int threshold)
		{
			if (this.currentLocation != null && !this.currentLocation.Equals(Game1.currentLocation))
			{
				return false;
			}
			Vector2 tileLocationOfPlayer = Game1.player.getTileLocation();
			Vector2 tileLocationOfMonster = base.getTileLocation();
			return Math.Abs(tileLocationOfMonster.X - tileLocationOfPlayer.X) <= (float)threshold && Math.Abs(tileLocationOfMonster.Y - tileLocationOfPlayer.Y) <= (float)threshold;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0007104A File Offset: 0x0006F24A
		private Stack<Point> addToStackForSchedule(Stack<Point> original, Stack<Point> toAdd)
		{
			if (toAdd == null)
			{
				return original;
			}
			original = new Stack<Point>(original);
			while (original.Count > 0)
			{
				toAdd.Push(original.Pop());
			}
			return toAdd;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00071074 File Offset: 0x0006F274
		private SchedulePathDescription pathfindToNextScheduleLocation(string startingLocation, int startingX, int startingY, string endingLocation, int endingX, int endingY, int finalFacingDirection, string endBehavior, string endMessage)
		{
			Stack<Point> path = new Stack<Point>();
			Point locationStartPoint = new Point(startingX, startingY);
			List<string> locationsRoute = (!startingLocation.Equals(endingLocation)) ? this.getLocationRoute(startingLocation, endingLocation) : null;
			if (locationsRoute != null)
			{
				for (int i = 0; i < locationsRoute.Count; i++)
				{
					GameLocation currentLocation = Game1.getLocationFromName(locationsRoute[i]);
					if (i < locationsRoute.Count - 1)
					{
						Point target = currentLocation.getWarpPointTo(locationsRoute[i + 1]);
						if (target.Equals(Point.Zero) || locationStartPoint.Equals(Point.Zero))
						{
							throw new Exception("schedule pathing tried to find a warp point that doesn't exist.");
						}
						path = this.addToStackForSchedule(path, PathFindController.findPathForNPCSchedules(locationStartPoint, target, currentLocation, 30000));
						locationStartPoint = currentLocation.getWarpPointTarget(target);
					}
					else
					{
						path = this.addToStackForSchedule(path, PathFindController.findPathForNPCSchedules(locationStartPoint, new Point(endingX, endingY), currentLocation, 30000));
					}
				}
			}
			else if (startingLocation.Equals(endingLocation))
			{
				path = PathFindController.findPathForNPCSchedules(locationStartPoint, new Point(endingX, endingY), Game1.getLocationFromName(startingLocation), 30000);
			}
			return new SchedulePathDescription(path, finalFacingDirection, endBehavior, endMessage);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0007118C File Offset: 0x0006F38C
		private List<string> getLocationRoute(string startingLocation, string endingLocation)
		{
			foreach (List<string> s in NPC.routesFromLocationToLocation)
			{
				if (s.First<string>().Equals(startingLocation) && s.Last<string>().Equals(endingLocation) && (this.gender == 0 || !s.Contains("BathHouse_MensLocker")) && (this.gender != 0 || !s.Contains("BathHouse_WomensLocker")))
				{
					return s;
				}
			}
			return null;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00071224 File Offset: 0x0006F424
		private bool changeScheduleForLocationAccessibility(ref string locationName, ref int tileX, ref int tileY, ref int facingDirection)
		{
			string a = locationName;
			if (!(a == "JojaMart") && !(a == "Railroad"))
			{
				if (a == "CommunityCenter")
				{
					return !Game1.isLocationAccessible(locationName);
				}
			}
			else if (!Game1.isLocationAccessible(locationName))
			{
				if (!Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name).ContainsKey(locationName + "_Replacement"))
				{
					return true;
				}
				string[] split = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)[locationName + "_Replacement"].Split(new char[]
				{
					' '
				});
				locationName = split[0];
				tileX = Convert.ToInt32(split[1]);
				tileY = Convert.ToInt32(split[2]);
				facingDirection = Convert.ToInt32(split[3]);
			}
			return false;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0007130C File Offset: 0x0006F50C
		private Dictionary<int, SchedulePathDescription> parseMasterSchedule(string rawData)
		{
			string[] split = rawData.Split(new char[]
			{
				'/'
			});
			Dictionary<int, SchedulePathDescription> oneDaySchedule = new Dictionary<int, SchedulePathDescription>();
			int routesToSkip = 0;
			if (split[0].Contains("GOTO"))
			{
				string newKey = split[0].Split(new char[]
				{
					' '
				})[1];
				if (newKey.ToLower().Equals("season"))
				{
					newKey = Game1.currentSeason;
				}
				try
				{
					split = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)[newKey].Split(new char[]
					{
						'/'
					});
				}
				catch (Exception)
				{
					return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["spring"]);
				}
			}
			if (split[0].Contains("NOT"))
			{
				string[] commandSplit = split[0].Split(new char[]
				{
					' '
				});
				string a = commandSplit[1].ToLower();
				if (a == "friendship")
				{
					string who = commandSplit[2];
					int level = Convert.ToInt32(commandSplit[3]);
					bool conditionMet = false;
					using (List<Farmer>.Enumerator enumerator = Game1.getAllFarmers().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.getFriendshipLevelForNPC(who) >= level)
							{
								conditionMet = true;
								break;
							}
						}
					}
					if (conditionMet)
					{
						return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["spring"]);
					}
					routesToSkip++;
				}
			}
			if (split[routesToSkip].Contains("GOTO"))
			{
				string newKey2 = split[routesToSkip].Split(new char[]
				{
					' '
				})[1];
				if (newKey2.ToLower().Equals("season"))
				{
					newKey2 = Game1.currentSeason;
				}
				split = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)[newKey2].Split(new char[]
				{
					'/'
				});
				routesToSkip = 1;
			}
			Point previousPosition = this.isMarried() ? new Point(0, 23) : new Point((int)this.defaultPosition.X / Game1.tileSize, (int)this.defaultPosition.Y / Game1.tileSize);
			string previousGameLocation = this.isMarried() ? "BusStop" : this.defaultMap;
			int i = routesToSkip;
			while (i < split.Length && split.Length > 1)
			{
				int index = 0;
				string[] newDestinationDescription = split[i].Split(new char[]
				{
					' '
				});
				int time = Convert.ToInt32(newDestinationDescription[index]);
				index++;
				string location = newDestinationDescription[index];
				string endOfRouteAnimation = null;
				string endOfRouteMessage = null;
				int tmp;
				if (int.TryParse(location, out tmp))
				{
					location = previousGameLocation;
					index--;
				}
				index++;
				int xLocation = Convert.ToInt32(newDestinationDescription[index]);
				index++;
				int yLocation = Convert.ToInt32(newDestinationDescription[index]);
				index++;
				int localFacingDirection = 2;
				try
				{
					localFacingDirection = Convert.ToInt32(newDestinationDescription[index]);
					index++;
				}
				catch (Exception)
				{
					localFacingDirection = 2;
				}
				if (this.changeScheduleForLocationAccessibility(ref location, ref xLocation, ref yLocation, ref localFacingDirection))
				{
					if (Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name).ContainsKey("default"))
					{
						return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["default"]);
					}
					return this.parseMasterSchedule(Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name)["spring"]);
				}
				else
				{
					if (index < newDestinationDescription.Length)
					{
						if (newDestinationDescription[index].Length > 0 && newDestinationDescription[index][0] == '"')
						{
							endOfRouteMessage = split[i].Substring(split[i].IndexOf('"'));
						}
						else
						{
							endOfRouteAnimation = newDestinationDescription[index];
							index++;
							if (index < newDestinationDescription.Length && newDestinationDescription[index].Length > 0 && newDestinationDescription[index][0] == '"')
							{
								endOfRouteMessage = split[i].Substring(split[i].IndexOf('"')).Replace("\"", "");
							}
						}
					}
					oneDaySchedule.Add(time, this.pathfindToNextScheduleLocation(previousGameLocation, previousPosition.X, previousPosition.Y, location, xLocation, yLocation, localFacingDirection, endOfRouteAnimation, endOfRouteMessage));
					previousPosition.X = xLocation;
					previousPosition.Y = yLocation;
					previousGameLocation = location;
					i++;
				}
			}
			return oneDaySchedule;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000717AC File Offset: 0x0006F9AC
		public Dictionary<int, SchedulePathDescription> getSchedule(int dayOfMonth)
		{
			if (!this.name.Equals("Robin") || Game1.player.currentUpgrade != null)
			{
				this.isInvisible = false;
			}
			if (this.name.Equals("Willy") && Game1.stats.DaysPlayed < 2u)
			{
				this.isInvisible = true;
			}
			else if (this.Schedule != null)
			{
				this.followSchedule = true;
			}
			Dictionary<string, string> masterSchedule = null;
			Dictionary<int, SchedulePathDescription> result;
			try
			{
				masterSchedule = Game1.content.Load<Dictionary<string, string>>("Characters\\schedules\\" + this.name);
			}
			catch (Exception)
			{
				result = null;
				return result;
			}
			if (this.isMarried())
			{
				string day = Game1.shortDayNameFromDayOfSeason(dayOfMonth);
				if ((this.name.Equals("Penny") && (day.Equals("Tue") || day.Equals("Wed") || day.Equals("Fri"))) || (this.name.Equals("Maru") && (day.Equals("Tue") || day.Equals("Thu"))) || (this.name.Equals("Harvey") && (day.Equals("Tue") || day.Equals("Thu"))))
				{
					this.nameOfTodaysSchedule = "marriageJob";
					return this.parseMasterSchedule(masterSchedule["marriageJob"]);
				}
				if (!Game1.isRaining && masterSchedule.ContainsKey("marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
				{
					this.nameOfTodaysSchedule = "marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
					return this.parseMasterSchedule(masterSchedule["marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
				}
				this.followSchedule = false;
				return null;
			}
			else
			{
				if (masterSchedule.ContainsKey(Game1.currentSeason + "_" + Game1.dayOfMonth))
				{
					return this.parseMasterSchedule(masterSchedule[Game1.currentSeason + "_" + Game1.dayOfMonth]);
				}
				int friendship;
				for (friendship = (Game1.player.friendships.ContainsKey(this.name) ? (Game1.player.friendships[this.name][0] / 250) : -1); friendship > 0; friendship--)
				{
					if (masterSchedule.ContainsKey(Game1.dayOfMonth + "_" + friendship))
					{
						return this.parseMasterSchedule(masterSchedule[Game1.dayOfMonth + "_" + friendship]);
					}
				}
				if (masterSchedule.ContainsKey(string.Empty + Game1.dayOfMonth))
				{
					return this.parseMasterSchedule(masterSchedule[string.Empty + Game1.dayOfMonth]);
				}
				if (this.name.Equals("Pam") && Game1.player.mailReceived.Contains("ccVault"))
				{
					return this.parseMasterSchedule(masterSchedule["bus"]);
				}
				if (Game1.isRaining)
				{
					if (Game1.random.NextDouble() < 0.5 && masterSchedule.ContainsKey("rain2"))
					{
						return this.parseMasterSchedule(masterSchedule["rain2"]);
					}
					if (masterSchedule.ContainsKey("rain"))
					{
						return this.parseMasterSchedule(masterSchedule["rain"]);
					}
				}
				List<string> key = new List<string>
				{
					Game1.currentSeason,
					Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)
				};
				friendship = (Game1.player.friendships.ContainsKey(this.name) ? (Game1.player.friendships[this.name][0] / 250) : -1);
				while (friendship > 0)
				{
					key.Add(string.Empty + friendship);
					if (masterSchedule.ContainsKey(string.Join("_", key)))
					{
						return this.parseMasterSchedule(masterSchedule[string.Join("_", key)]);
					}
					friendship--;
					key.RemoveAt(key.Count - 1);
				}
				if (masterSchedule.ContainsKey(string.Join("_", key)))
				{
					return this.parseMasterSchedule(masterSchedule[string.Join("_", key)]);
				}
				if (masterSchedule.ContainsKey(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
				{
					return this.parseMasterSchedule(masterSchedule[Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
				}
				if (masterSchedule.ContainsKey(Game1.currentSeason))
				{
					return this.parseMasterSchedule(masterSchedule[Game1.currentSeason]);
				}
				if (masterSchedule.ContainsKey("spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
				{
					return this.parseMasterSchedule(masterSchedule["spring_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)]);
				}
				key.RemoveAt(key.Count - 1);
				key.Add("spring");
				friendship = (Game1.player.friendships.ContainsKey(this.name) ? (Game1.player.friendships[this.name][0] / 250) : -1);
				while (friendship > 0)
				{
					key.Add(string.Empty + friendship);
					if (masterSchedule.ContainsKey(string.Join("_", key)))
					{
						return this.parseMasterSchedule(masterSchedule[string.Join("_", key)]);
					}
					friendship--;
					key.RemoveAt(key.Count - 1);
				}
				if (masterSchedule.ContainsKey("spring"))
				{
					return this.parseMasterSchedule(masterSchedule["spring"]);
				}
				return null;
			}
			return result;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00071D54 File Offset: 0x0006FF54
		public void setMarried(bool isMarried)
		{
			this.married = (isMarried ? 1 : 0);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00071D64 File Offset: 0x0006FF64
		public bool isMarried()
		{
			if (!this.isVillager())
			{
				return false;
			}
			if (this.married != -1)
			{
				return this.married == 1;
			}
			foreach (Farmer f in Game1.getAllFarmers())
			{
				if (f.spouse != null && f.spouse.Equals(this.name))
				{
					this.married = 1;
					return true;
				}
			}
			this.married = 0;
			return false;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00071DFC File Offset: 0x0006FFFC
		public virtual void dayUpdate(int dayOfMonth)
		{
			if (this.currentLocation != null)
			{
				Game1.warpCharacter(this, this.defaultMap, this.defaultPosition / (float)Game1.tileSize, true, false);
			}
			if (this.name.Equals("Maru") || this.name.Equals("Shane"))
			{
				this.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + this.name);
			}
			if (this.name.Equals("Willy") || this.name.Equals("Clint"))
			{
				this.sprite.spriteWidth = 16;
				this.sprite.spriteHeight = 32;
				this.sprite.ignoreSourceRectUpdates = false;
				this.sprite.UpdateSourceRect();
				this.isInvisible = false;
			}
			Game1.player.mailReceived.Remove(this.name);
			Game1.player.mailReceived.Remove(this.name + "Cooking");
			this.doingEndOfRouteAnimation = false;
			this.Halt();
			this.hasBeenKissedToday = false;
			this.faceTowardFarmer = false;
			this.faceTowardFarmerTimer = 0;
			this.drawOffset = Vector2.Zero;
			this.hasSaidAfternoonDialogue = false;
			this.ignoreScheduleToday = false;
			this.Halt();
			this.controller = null;
			this.temporaryController = null;
			this.directionsToNewLocation = null;
			this.faceDirection(this.DefaultFacingDirection);
			this.scheduleTimeToTry = 9999999;
			this.previousEndPoint = new Point((int)this.defaultPosition.X / Game1.tileSize, (int)this.defaultPosition.Y / Game1.tileSize);
			this.isWalkingInSquare = false;
			this.returningToEndPoint = false;
			this.lastCrossroad = Microsoft.Xna.Framework.Rectangle.Empty;
			if (this.isVillager())
			{
				this.Schedule = this.getSchedule(dayOfMonth);
			}
			this.endOfRouteMessage = null;
			bool isFestivalToday = Utility.isFestivalDay(dayOfMonth, Game1.currentSeason);
			if (this.name.Equals("Robin") && Game1.player.daysUntilHouseUpgrade > 0 && !isFestivalToday)
			{
				this.ignoreMultiplayerUpdates = true;
				Game1.warpCharacter(this, "Farm", new Vector2(68f, 14f), false, false);
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(24, 75),
					new FarmerSprite.AnimationFrame(25, 75),
					new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
					new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
				});
				this.ignoreScheduleToday = true;
				this.CurrentDialogue.Clear();
				this.currentDialogue.Push(new Dialogue((Game1.player.daysUntilHouseUpgrade == 2) ? "Be patient, I still have a lot of work to do." : "Your house should be ready tomorrow.", this));
			}
			else if (this.name.Equals("Robin") && Game1.getFarm().isThereABuildingUnderConstruction() && !isFestivalToday)
			{
				Building b = Game1.getFarm().getBuildingUnderConstruction();
				if (b.daysUntilUpgrade > 0)
				{
					if (!b.indoors.characters.Contains(this))
					{
						b.indoors.addCharacter(this);
					}
					if (this.currentLocation != null)
					{
						this.currentLocation.characters.Remove(this);
					}
					this.currentLocation = b.indoors;
					this.setTilePosition(1, 5);
				}
				else
				{
					Game1.warpCharacter(this, "Farm", new Vector2((float)(b.tileX + b.tilesWide / 2), (float)(b.tileY + b.tilesHigh / 2)), false, false);
					this.position.X = this.position.X + (float)(Game1.tileSize / 4);
					this.position.Y = this.position.Y - (float)(Game1.tileSize / 2);
				}
				this.ignoreMultiplayerUpdates = true;
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(24, 75),
					new FarmerSprite.AnimationFrame(25, 75),
					new FarmerSprite.AnimationFrame(26, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinHammerSound), false),
					new FarmerSprite.AnimationFrame(27, 1000, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false)
				});
				this.ignoreScheduleToday = true;
				this.CurrentDialogue.Clear();
				this.currentDialogue.Push(new Dialogue("Be patient, I still have a lot of work to do.", this));
			}
			if (this.isMarried())
			{
				this.marriageDuties();
				this.daysMarried++;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00072290 File Offset: 0x00070490
		public void returnHomeFromFarmPosition(Farm farm)
		{
			Point porchPoint = ((FarmHouse)Game1.getLocationFromName("FarmHouse")).getPorchStandingSpot();
			if (base.getTileLocationPoint().Equals(porchPoint))
			{
				this.drawOffset = Vector2.Zero;
				string nameOfHome = this.getHome().name;
				this.willDestroyObjectsUnderfoot = true;
				this.controller = new PathFindController(this, farm, farm.getWarpPointTo(nameOfHome), 0)
				{
					NPCSchedule = true
				};
				return;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00072300 File Offset: 0x00070500
		public void setUpForOutdoorPatioActivity()
		{
			Game1.warpCharacter(this, "Farm", new Vector2(71f, 10f), false, false);
			this.setNewDialogue("MarriageDialogue", "patio_", -1, false, true);
			string name = this.name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1866496948u)
			{
				if (num <= 1067922812u)
				{
					if (num != 161540545u)
					{
						if (num != 587846041u)
						{
							if (num != 1067922812u)
							{
								return;
							}
							if (!(name == "Sam"))
							{
								return;
							}
							this.setTilePosition(71, 8);
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(25, 3000),
								new FarmerSprite.AnimationFrame(27, 500),
								new FarmerSprite.AnimationFrame(26, 100),
								new FarmerSprite.AnimationFrame(28, 100),
								new FarmerSprite.AnimationFrame(27, 500),
								new FarmerSprite.AnimationFrame(25, 2000),
								new FarmerSprite.AnimationFrame(27, 500),
								new FarmerSprite.AnimationFrame(26, 100),
								new FarmerSprite.AnimationFrame(29, 100),
								new FarmerSprite.AnimationFrame(30, 100),
								new FarmerSprite.AnimationFrame(32, 500),
								new FarmerSprite.AnimationFrame(31, 1000),
								new FarmerSprite.AnimationFrame(30, 100),
								new FarmerSprite.AnimationFrame(29, 100)
							});
							return;
						}
						else
						{
							if (!(name == "Penny"))
							{
								return;
							}
							this.setTilePosition(71, 8);
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(18, 6000),
								new FarmerSprite.AnimationFrame(19, 500)
							});
							return;
						}
					}
					else
					{
						if (!(name == "Sebastian"))
						{
							return;
						}
						this.setTilePosition(71, 9);
						this.drawOffset = new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 2 + Game1.pixelZoom * 2));
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(36, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(32, 500, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(36, 2000, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(33, 100, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(34, 100, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(35, 3000, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(34, 100, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(33, 100, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(32, 1500, Game1.tileSize, false, false, null, false, 0)
						});
						return;
					}
				}
				else if (num != 1281010426u)
				{
					if (num != 1708213605u)
					{
						if (num != 1866496948u)
						{
							return;
						}
						if (!(name == "Shane"))
						{
							return;
						}
						this.setTilePosition(69, 9);
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(28, 4000, Game1.tileSize, false, false, null, false, 0),
							new FarmerSprite.AnimationFrame(29, 800, Game1.tileSize, false, false, null, false, 0)
						});
						return;
					}
					else
					{
						if (!(name == "Alex"))
						{
							return;
						}
						this.setTilePosition(71, 8);
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(34, 4000),
							new FarmerSprite.AnimationFrame(33, 300),
							new FarmerSprite.AnimationFrame(28, 200),
							new FarmerSprite.AnimationFrame(29, 100),
							new FarmerSprite.AnimationFrame(30, 100),
							new FarmerSprite.AnimationFrame(31, 100),
							new FarmerSprite.AnimationFrame(32, 100),
							new FarmerSprite.AnimationFrame(31, 100),
							new FarmerSprite.AnimationFrame(30, 100),
							new FarmerSprite.AnimationFrame(29, 100),
							new FarmerSprite.AnimationFrame(28, 800),
							new FarmerSprite.AnimationFrame(29, 100),
							new FarmerSprite.AnimationFrame(30, 100),
							new FarmerSprite.AnimationFrame(31, 100),
							new FarmerSprite.AnimationFrame(32, 100),
							new FarmerSprite.AnimationFrame(31, 100),
							new FarmerSprite.AnimationFrame(30, 100),
							new FarmerSprite.AnimationFrame(29, 100),
							new FarmerSprite.AnimationFrame(28, 800),
							new FarmerSprite.AnimationFrame(33, 200)
						});
						return;
					}
				}
				else
				{
					if (!(name == "Maru"))
					{
						return;
					}
					this.setTilePosition(70, 8);
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(16, 4000),
						new FarmerSprite.AnimationFrame(17, 200),
						new FarmerSprite.AnimationFrame(18, 200),
						new FarmerSprite.AnimationFrame(19, 200),
						new FarmerSprite.AnimationFrame(20, 200),
						new FarmerSprite.AnimationFrame(21, 200),
						new FarmerSprite.AnimationFrame(22, 200),
						new FarmerSprite.AnimationFrame(23, 200)
					});
					return;
				}
			}
			else if (num <= 2571828641u)
			{
				if (num != 2010304804u)
				{
					if (num != 2434294092u)
					{
						if (num != 2571828641u)
						{
							return;
						}
						if (!(name == "Emily"))
						{
							return;
						}
						this.setTilePosition(70, 9);
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(54, 4000, Game1.tileSize, false, false, null, false, 0)
						});
						return;
					}
					else
					{
						if (!(name == "Haley"))
						{
							return;
						}
						this.setTilePosition(70, 8);
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(30, 2000),
							new FarmerSprite.AnimationFrame(31, 200),
							new FarmerSprite.AnimationFrame(24, 2000),
							new FarmerSprite.AnimationFrame(25, 1000),
							new FarmerSprite.AnimationFrame(32, 200),
							new FarmerSprite.AnimationFrame(33, 2000),
							new FarmerSprite.AnimationFrame(32, 200),
							new FarmerSprite.AnimationFrame(25, 2000),
							new FarmerSprite.AnimationFrame(32, 200),
							new FarmerSprite.AnimationFrame(33, 2000)
						});
						return;
					}
				}
				else
				{
					if (!(name == "Harvey"))
					{
						return;
					}
					this.setTilePosition(71, 8);
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(42, 6000),
						new FarmerSprite.AnimationFrame(43, 1000),
						new FarmerSprite.AnimationFrame(39, 100),
						new FarmerSprite.AnimationFrame(43, 500),
						new FarmerSprite.AnimationFrame(39, 100),
						new FarmerSprite.AnimationFrame(43, 1000),
						new FarmerSprite.AnimationFrame(42, 5000),
						new FarmerSprite.AnimationFrame(43, 3000)
					});
					return;
				}
			}
			else if (num != 2732913340u)
			{
				if (num != 2826247323u)
				{
					if (num != 3066176300u)
					{
						return;
					}
					if (!(name == "Elliott"))
					{
						return;
					}
					this.setTilePosition(71, 8);
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(33, 3000),
						new FarmerSprite.AnimationFrame(32, 500),
						new FarmerSprite.AnimationFrame(33, 3000),
						new FarmerSprite.AnimationFrame(32, 500),
						new FarmerSprite.AnimationFrame(33, 2000),
						new FarmerSprite.AnimationFrame(34, 1500)
					});
					return;
				}
				else
				{
					if (!(name == "Leah"))
					{
						return;
					}
					this.setTilePosition(71, 8);
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(16, 100),
						new FarmerSprite.AnimationFrame(17, 100),
						new FarmerSprite.AnimationFrame(18, 100),
						new FarmerSprite.AnimationFrame(19, 300),
						new FarmerSprite.AnimationFrame(16, 100),
						new FarmerSprite.AnimationFrame(17, 100),
						new FarmerSprite.AnimationFrame(18, 100),
						new FarmerSprite.AnimationFrame(19, 1000),
						new FarmerSprite.AnimationFrame(16, 100),
						new FarmerSprite.AnimationFrame(17, 100),
						new FarmerSprite.AnimationFrame(18, 100),
						new FarmerSprite.AnimationFrame(19, 300),
						new FarmerSprite.AnimationFrame(16, 100),
						new FarmerSprite.AnimationFrame(17, 100),
						new FarmerSprite.AnimationFrame(18, 100),
						new FarmerSprite.AnimationFrame(19, 300),
						new FarmerSprite.AnimationFrame(16, 100),
						new FarmerSprite.AnimationFrame(17, 100),
						new FarmerSprite.AnimationFrame(18, 100),
						new FarmerSprite.AnimationFrame(19, 2000)
					});
					return;
				}
			}
			else
			{
				if (!(name == "Abigail"))
				{
					return;
				}
				this.setTilePosition(71, 8);
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(16, 500),
					new FarmerSprite.AnimationFrame(17, 500),
					new FarmerSprite.AnimationFrame(18, 500),
					new FarmerSprite.AnimationFrame(19, 500)
				});
				return;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00072DE0 File Offset: 0x00070FE0
		public bool isGaySpouse()
		{
			Farmer spouse = this.getSpouse();
			return spouse != null && ((this.gender == 0 && spouse.isMale) || (this.gender == 1 && !spouse.isMale));
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00072E20 File Offset: 0x00071020
		public bool canGetPregnant()
		{
			if (this is Horse)
			{
				return false;
			}
			Farmer spouse = this.getSpouse();
			if (spouse == null)
			{
				return false;
			}
			int heartsWithSpouse = spouse.getFriendshipHeartLevelForNPC(this.name);
			List<Child> kids = spouse.getChildren();
			if (this.defaultMap == null)
			{
				this.defaultMap = "FarmHouse";
			}
			return (Game1.getLocationFromName(this.defaultMap) as FarmHouse).upgradeLevel >= 2 && this.daysUntilBirthing < 0 && heartsWithSpouse >= 10 && spouse.daysMarried >= 7 && (kids.Count == 0 || (kids.Count < 2 && kids[0].age > 2));
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00072EC0 File Offset: 0x000710C0
		public void marriageDuties()
		{
			if (!Game1.newDay && Game1.gameMode != 6)
			{
				return;
			}
			if (this.getSpouse() != null)
			{
				FarmHouse farmHouse = null;
				if (this.defaultMap.Contains("FarmHouse"))
				{
					farmHouse = (Game1.getLocationFromName(this.defaultMap) as FarmHouse);
				}
				Random r;
				try
				{
					r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)farmHouse.farmerNumberOfOwner));
				}
				catch (Exception)
				{
					this.defaultMap = "FarmHouse";
					farmHouse = (Game1.getLocationFromName(this.defaultMap) as FarmHouse);
					r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)farmHouse.farmerNumberOfOwner));
				}
				int heartsWithSpouse = this.getSpouse().getFriendshipHeartLevelForNPC(this.name);
				if (this.currentLocation == null || !this.currentLocation.Equals(farmHouse))
				{
					Game1.removeThisCharacterFromAllLocations(this);
					for (int i = farmHouse.characters.Count - 1; i >= 0; i--)
					{
						if (farmHouse.characters[i].name.Equals(this.name))
						{
							farmHouse.characters.RemoveAt(i);
						}
					}
					Game1.warpCharacter(this, "FarmHouse", farmHouse.getBedSpot(), false, false);
				}
				if (this.daysUntilBirthing > 0)
				{
					this.daysUntilBirthing--;
				}
				if (this.daysAfterLastBirth >= 0)
				{
					this.daysAfterLastBirth--;
					List<Child> kids = this.getSpouse().getChildren();
					if (kids.Count == 1)
					{
						this.setTilePosition(farmHouse.getKitchenStandingSpot());
						if (!this.spouseObstacleCheck("I just feel like sleeping today.$a", farmHouse, false))
						{
							this.setNewDialogue("MarriageDialogue", "OneKid_", r.Next(4), false, false);
						}
						return;
					}
					if (kids.Count == 2)
					{
						this.setTilePosition(farmHouse.getKitchenStandingSpot());
						if (!this.spouseObstacleCheck("I just feel like sleeping today.$a", farmHouse, false))
						{
							this.setNewDialogue("MarriageDialogue", "TwoKids_", r.Next(4), false, false);
						}
						return;
					}
				}
				this.setTilePosition(farmHouse.getKitchenStandingSpot());
				if (this.tryToGetMarriageSpecificDialogueElseReturnDefault(Game1.currentSeason + "_" + Game1.dayOfMonth, "").Length > 0)
				{
					this.currentDialogue.Clear();
					this.currentDialogue.Push(new Dialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault(Game1.currentSeason + "_" + Game1.dayOfMonth, ""), this));
					return;
				}
				if (this.schedule != null)
				{
					if (this.nameOfTodaysSchedule.Equals("marriage_" + Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
					{
						this.setNewDialogue("MarriageDialogue", "funLeave_", -1, false, true);
						return;
					}
					if (this.nameOfTodaysSchedule.Equals("marriageJob"))
					{
						this.setNewDialogue("MarriageDialogue", "jobLeave_", -1, false, true);
					}
					return;
				}
				else
				{
					if (!Game1.isRaining && !Game1.IsWinter && Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Sat"))
					{
						this.setUpForOutdoorPatioActivity();
						return;
					}
					if (this.daysMarried >= 1 && r.NextDouble() < (double)(1f - (float)Math.Max(1, heartsWithSpouse) / 12f))
					{
						Furniture f = farmHouse.getRandomFurniture(r);
						if (f != null && f.isGroundFurniture())
						{
							Point p = new Point((int)f.tileLocation.X - 1, (int)f.tileLocation.Y);
							if (farmHouse.isTileLocationTotallyClearAndPlaceable(p.X, p.Y))
							{
								this.setTilePosition(p);
								this.faceDirection(1);
								string sadDialogue = "";
								switch (r.Next(10))
								{
								case 0:
									sadDialogue = "*sigh*...$s";
									break;
								case 1:
									sadDialogue = "I'm bored...$s";
									break;
								case 2:
									sadDialogue = "Sometimes I wonder if I'm doing the right thing with my life...$s";
									break;
								case 3:
									sadDialogue = "Huh? Nothing's wrong... I'm fine.$s";
									break;
								case 4:
									sadDialogue = "I just don't have any energy today.$s";
									break;
								case 5:
									sadDialogue = "Life sure is different since we got married...$s";
									break;
								case 6:
									sadDialogue = "What... you want me to clean? Make you a sandwich? *sigh*...$a";
									break;
								case 7:
									sadDialogue = ((this.gender == 1) ? ("*sigh*... my " + ((r.NextDouble() < 0.5) ? "skin" : "hair") + " looks horrible today.$s#$b#It does... I can tell by the way you're looking at me.$a") : "...$s#$e#Don't you ever feel trapped?$s");
									break;
								case 8:
									sadDialogue = "*grumble*... chores... $a";
									break;
								case 9:
									sadDialogue = "Don't you have work to do?$a";
									break;
								}
								this.setNewDialogue(sadDialogue, false, false);
								return;
							}
						}
						string sleepDialogue = "";
						switch (r.Next(5))
						{
						case 0:
							sleepDialogue = "Nghh... what is it? I'm trying to sleep.$a";
							break;
						case 1:
							sleepDialogue = "I just don't feel like getting up today.$s";
							break;
						case 2:
							sleepDialogue = "*sigh*... I just want to stay in bed.$s";
							break;
						case 3:
							sleepDialogue = "Nnnghh... what is it? Make your own breakfast.$a";
							break;
						case 4:
							sleepDialogue = "...$a#$e$I had a bad dream, that's all.$s";
							break;
						}
						this.spouseObstacleCheck(sleepDialogue, farmHouse, true);
						return;
					}
					if (this.daysUntilBirthing != -1 && this.daysUntilBirthing <= 7 && r.NextDouble() < 0.5)
					{
						if (this.isGaySpouse())
						{
							this.setTilePosition(farmHouse.getKitchenStandingSpot());
							if (!this.spouseObstacleCheck("I wonder how much the new baby will change things?$s", farmHouse, false))
							{
								this.setNewDialogue((r.NextDouble() < 0.5) ? (this.getSpouse().name + ", I hope our adoption request gets approved. I want a baby.$l") : (this.getTermOfSpousalEndearment(true) + ", I filed our adoption papers. Now all we can do is cross our fingers and wait.$l"), r.NextDouble() < 0.5, false);
								return;
							}
						}
						else if (this.gender == 1)
						{
							this.setTilePosition(farmHouse.getKitchenStandingSpot());
							if (!this.spouseObstacleCheck((r.NextDouble() < 0.5) ? "Ugh... I feel a little nauseated this morning.$s" : "I'm pregnant... can't you make your own breakfast?$s", farmHouse, false))
							{
								this.setNewDialogue((r.NextDouble() < 0.5) ? (this.getSpouse().name + ", we're going to have a baby soon.$l") : (this.getTermOfSpousalEndearment(true) + ", I'm pregnant. Isn't it wonderful?$l"), r.NextDouble() < 0.5, false);
								return;
							}
						}
						else
						{
							this.setTilePosition(farmHouse.getKitchenStandingSpot());
							if (!this.spouseObstacleCheck("I just feel like sleeping today. Don't worry about it.$a", farmHouse, false))
							{
								this.setNewDialogue((r.NextDouble() < 0.5) ? (this.getSpouse().name + ", we're going to have a baby soon.$l") : (this.getTermOfSpousalEndearment(true) + ", can't you tell? You're pregnant.$l"), r.NextDouble() < 0.5, false);
							}
						}
						return;
					}
					if (r.NextDouble() < 0.07)
					{
						List<Child> kids2 = this.getSpouse().getChildren();
						if (kids2.Count == 1)
						{
							this.setTilePosition(farmHouse.getKitchenStandingSpot());
							if (!this.spouseObstacleCheck("Being a parent is annoying, sometimes...  I just don't feel like getting up.$a", farmHouse, false))
							{
								this.setNewDialogue("MarriageDialogue", "OneKid_", r.Next(4), false, false);
							}
							return;
						}
						if (kids2.Count == 2)
						{
							this.setTilePosition(farmHouse.getKitchenStandingSpot());
							if (!this.spouseObstacleCheck("I need some alone time... Why don't you take care of the kids for a change?$a", farmHouse, false))
							{
								this.setNewDialogue("MarriageDialogue", "TwoKids_", r.Next(4), false, false);
							}
							return;
						}
					}
					if (this.CurrentDialogue.Count > 0 && this.CurrentDialogue.Peek().isItemGrabDialogue())
					{
						this.setTilePosition(farmHouse.getKitchenStandingSpot());
						this.spouseObstacleCheck("I was going to make you breakfast, but I changed my mind.$a", farmHouse, false);
						return;
					}
					if (!Game1.isRaining && r.NextDouble() < 0.4)
					{
						Farm farm = Game1.getLocationFromName("Farm") as Farm;
						bool filledBowl = false;
						if (farm.getTileIndexAt(54, 7, "Buildings") == 1938)
						{
							filledBowl = true;
							farm.setMapTileIndex(54, 7, 1939, "Buildings", 0);
						}
						if (r.NextDouble() < 0.6 && !Game1.currentSeason.Equals("winter"))
						{
							Vector2 origin = Vector2.Zero;
							int tries = 0;
							bool foundWatered = false;
							while (tries < Math.Min(50, farm.terrainFeatures.Count) && origin.Equals(Vector2.Zero))
							{
								int index = r.Next(farm.terrainFeatures.Count);
								if (farm.terrainFeatures.ElementAt(index).Value is HoeDirt)
								{
									if ((farm.terrainFeatures.ElementAt(index).Value as HoeDirt).needsWatering())
									{
										origin = farm.terrainFeatures.ElementAt(index).Key;
									}
									else if ((farm.terrainFeatures.ElementAt(index).Value as HoeDirt).crop != null)
									{
										foundWatered = true;
									}
								}
								tries++;
							}
							if (!origin.Equals(Vector2.Zero))
							{
								Microsoft.Xna.Framework.Rectangle wateringArea = new Microsoft.Xna.Framework.Rectangle((int)origin.X - 30, (int)origin.Y - 30, 60, 60);
								Vector2 currentPosition = default(Vector2);
								for (int x = wateringArea.X; x < wateringArea.Right; x++)
								{
									for (int y = wateringArea.Y; y < wateringArea.Bottom; y++)
									{
										currentPosition.X = (float)x;
										currentPosition.Y = (float)y;
										if (farm.isTileOnMap(currentPosition) && farm.terrainFeatures.ContainsKey(currentPosition) && farm.terrainFeatures[currentPosition] is HoeDirt && (farm.terrainFeatures[currentPosition] as HoeDirt).needsWatering())
										{
											(farm.terrainFeatures[currentPosition] as HoeDirt).state = 1;
										}
									}
								}
								this.faceDirection(2);
								this.currentDialogue.Clear();
								this.setNewDialogue("MarriageDialogue", "Outdoor_", r.Next(5), false, false);
								this.setNewDialogue("I got up early and watered some crops for you. I hope it makes your job a little easier today." + (filledBowl ? ("#$e#I also filled " + Game1.player.getPetName() + "'s water bowl.") : ""), true, false);
							}
							else
							{
								this.currentDialogue.Clear();
								this.faceDirection(2);
								if (foundWatered)
								{
									if (Game1.gameMode == 6)
									{
										this.setNewDialogue(((r.NextDouble() < 0.5) ? "Hi " : "Good Morning, ") + this.getTermOfSpousalEndearment(true) + "! I got up early and watered some crops for you. I hope it makes your job a little easier today." + (filledBowl ? ("#$e#I also filled " + Game1.player.getPetName() + "'s water bowl.") : ""), true, false);
									}
									else
									{
										this.setNewDialogue("I got up early to water some crops and they were already done! You've really got this place under control.$h", false, false);
									}
								}
								else
								{
									this.setNewDialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Outdoor_" + r.Next(5), ""), false, false);
								}
							}
						}
						else if (r.NextDouble() < 0.6)
						{
							bool fedAnything = false;
							foreach (Building b in farm.buildings)
							{
								if (b is Barn || b is Coop)
								{
									(b.indoors as AnimalHouse).feedAllAnimals();
									fedAnything = true;
								}
							}
							this.faceDirection(2);
							this.currentDialogue.Clear();
							if (fedAnything)
							{
								this.setNewDialogue("MarriageDialogue", "Outdoor_" + r.Next(5), -1, false, false);
								this.setNewDialogue("I got up early and fed all the farm animals. I hope that makes your job a little easier today." + (filledBowl ? ("#$e#I also filled " + Game1.player.getPetName() + "'s water bowl.") : ""), true, false);
							}
							else
							{
								this.setNewDialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Outdoor_" + r.Next(5), ""), false, false);
							}
							farm.setMapTileIndex(54, 7, 1939, "Buildings", 0);
						}
						else
						{
							int tries2 = 0;
							this.faceDirection(2);
							Vector2 origin2 = Vector2.Zero;
							while (tries2 < Math.Min(50, farm.objects.Count) && origin2.Equals(Vector2.Zero))
							{
								int index2 = r.Next(farm.objects.Count);
								if (farm.objects.ElementAt(index2).Value is Fence)
								{
									origin2 = farm.objects.ElementAt(index2).Key;
								}
								tries2++;
							}
							if (!origin2.Equals(Vector2.Zero))
							{
								Microsoft.Xna.Framework.Rectangle wateringArea2 = new Microsoft.Xna.Framework.Rectangle((int)origin2.X - 10, (int)origin2.Y - 10, 20, 20);
								Vector2 currentPosition2 = default(Vector2);
								for (int x2 = wateringArea2.X; x2 < wateringArea2.Right; x2++)
								{
									for (int y2 = wateringArea2.Y; y2 < wateringArea2.Bottom; y2++)
									{
										currentPosition2.X = (float)x2;
										currentPosition2.Y = (float)y2;
										if (farm.isTileOnMap(currentPosition2) && farm.objects.ContainsKey(currentPosition2) && farm.objects[currentPosition2] is Fence)
										{
											(farm.objects[currentPosition2] as Fence).repair();
										}
									}
								}
								this.currentDialogue.Clear();
								this.setNewDialogue("MarriageDialogue", "Outdoor_", r.Next(5), false, false);
								this.setNewDialogue("I spent the morning repairing a few of the fences. They should be as good as new." + (filledBowl ? ("#$e#I also filled " + Game1.player.getPetName() + "'s water bowl.") : ""), true, false);
							}
							else
							{
								this.currentDialogue.Clear();
								this.setNewDialogue(this.tryToGetMarriageSpecificDialogueElseReturnDefault("Outdoor_" + r.Next(5), ""), false, false);
							}
						}
						Game1.warpCharacter(this, "Farm", farmHouse.getPorchStandingSpot(), false, false);
						this.faceDirection(2);
						return;
					}
					if (this.daysMarried >= 1 && r.NextDouble() < 0.045)
					{
						if (r.NextDouble() < 0.75)
						{
							Point spot = farmHouse.getRandomOpenPointInHouse(r, 1, 30);
							if (spot.X > 0 && farmHouse.isTileLocationOpen(new Location(spot.X - 1, spot.Y)))
							{
								farmHouse.furniture.Add(new Furniture(Utility.getRandomSingleTileFurniture(r), new Vector2((float)spot.X, (float)spot.Y)));
								this.setTilePosition(spot.X - 1, spot.Y);
								this.faceDirection(1);
								this.setNewDialogue("What do you think, " + this.getTermOfSpousalEndearment(true).ToLower() + "? " + ((r.NextDouble() < 0.5) ? "I ordered this the other day and it just arrived." : "I figured the room could use a little more decoration."), true, true);
								return;
							}
							this.setTilePosition(farmHouse.getKitchenStandingSpot());
							this.spouseObstacleCheck("I was going to do some decorating today, but I changed my mind.$a", farmHouse, false);
							return;
						}
						else
						{
							Point p2 = farmHouse.getRandomOpenPointInHouse(r, 0, 30);
							if (p2.X > 0)
							{
								this.setTilePosition(p2.X, p2.Y);
								this.faceDirection(0);
								if (r.NextDouble() < 0.5)
								{
									int wall = farmHouse.getWallForRoomAt(p2);
									if (wall != -1)
									{
										int style = r.Next(112);
										List<int> styles = new List<int>();
										string name = this.name;
										if (!(name == "Sebastian"))
										{
											if (!(name == "Haley"))
											{
												if (!(name == "Abigail"))
												{
													if (!(name == "Leah"))
													{
														if (!(name == "Alex"))
														{
															if (name == "Shane")
															{
																styles.AddRange(new int[]
																{
																	6,
																	21,
																	60
																});
															}
														}
														else
														{
															styles.AddRange(new int[]
															{
																6
															});
														}
													}
													else
													{
														styles.AddRange(new int[]
														{
															44,
															108,
															43,
															45,
															92,
															37,
															29
														});
													}
												}
												else
												{
													styles.AddRange(new int[]
													{
														2,
														13,
														23,
														26,
														46,
														45,
														64,
														77,
														106,
														107
													});
												}
											}
											else
											{
												styles.AddRange(new int[]
												{
													1,
													7,
													10,
													35,
													49,
													84,
													99
												});
											}
										}
										else
										{
											styles.AddRange(new int[]
											{
												3,
												4,
												12,
												14,
												30,
												46,
												47,
												56,
												58,
												59,
												107
											});
										}
										if (styles.Count > 0)
										{
											style = styles[r.Next(styles.Count)];
										}
										farmHouse.setWallpaper(style, wall, true);
										this.setNewDialogue("What do you think of the new wallpaper I chose?", true, false);
										return;
									}
								}
								else
								{
									int floor = farmHouse.getFloorAt(p2);
									if (floor != -1)
									{
										farmHouse.setFloor(r.Next(40), floor, true);
										this.setNewDialogue("What do you think of the new flooring I chose?", true, false);
										return;
									}
								}
							}
						}
					}
					else
					{
						if (Game1.isRaining && r.NextDouble() < 0.08 && heartsWithSpouse < 11)
						{
							foreach (Furniture f2 in farmHouse.furniture)
							{
								if (f2.furniture_type == 13 && farmHouse.isTileLocationTotallyClearAndPlaceable((int)f2.tileLocation.X, (int)f2.tileLocation.Y + 1))
								{
									this.setTilePosition((int)f2.tileLocation.X, (int)f2.tileLocation.Y + 1);
									this.faceDirection(0);
									this.setNewDialogue("*sigh*... sometimes I miss my old life.$s", false, false);
									return;
								}
							}
							this.spouseObstacleCheck("The weather's too gloomy to get out of bed.$s", farmHouse, true);
							return;
						}
						if (r.NextDouble() < 0.45)
						{
							Vector2 spot2 = (farmHouse.upgradeLevel == 1) ? new Vector2(32f, 5f) : new Vector2(38f, 14f);
							this.setTilePosition((int)spot2.X, (int)spot2.Y);
							this.faceDirection(0);
							this.setSpouseRoomMarriageDialogue();
							return;
						}
						this.setTilePosition(farmHouse.getKitchenStandingSpot());
						this.faceDirection(0);
						if (r.NextDouble() < 0.2)
						{
							this.setRandomAfternoonMarriageDialogue(Game1.timeOfDay, farmHouse, false);
						}
					}
				}
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00074114 File Offset: 0x00072314
		public void clearTextAboveHead()
		{
			this.textAboveHead = null;
			this.textAboveHeadPreTimer = -1;
			this.textAboveHeadTimer = -1;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0007412B File Offset: 0x0007232B
		public bool isVillager()
		{
			return !this.IsMonster && !(this is Child) && !(this is Pet) && !(this is Horse) && !(this is Junimo) && !(this is JunimoHarvester);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00074163 File Offset: 0x00072363
		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return (this.isMarried() && (this.Schedule == null || location is FarmHouse)) || base.shouldCollideWithBuildingLayer(location);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00074188 File Offset: 0x00072388
		public void arriveAtFarmHouse()
		{
			if (!Game1.newDay && this.isMarried() && Game1.timeOfDay > 630 && !base.getTileLocationPoint().Equals((this.getHome() as FarmHouse).getSpouseBedSpot()))
			{
				this.setTilePosition((this.getHome() as FarmHouse).getEntryLocation());
				this.temporaryController = null;
				this.controller = null;
				this.controller = new PathFindController(this, this.getHome(), (Game1.timeOfDay >= 2130) ? (this.getHome() as FarmHouse).getSpouseBedSpot() : (this.getHome() as FarmHouse).getKitchenStandingSpot(), 0);
				if (this.controller.pathToEndPoint == null)
				{
					this.willDestroyObjectsUnderfoot = true;
					this.controller = new PathFindController(this, this.getHome(), (this.getHome() as FarmHouse).getKitchenStandingSpot(), 0);
					this.setNewDialogue("You could have cleaned up in here a little while I was gone...$a#$e#It's not very nice to have to wade through a bunch of junk after a hard day's work.$a", false, false);
				}
				else if (Game1.timeOfDay > 1300)
				{
					if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLower().Equals("mon") || (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).ToLower().Equals("fri") && !this.name.Equals("Maru") && !this.name.Equals("Penny") && !this.name.Equals("Harvey")))
					{
						this.setNewDialogue("MarriageDialogue", "funReturn_", -1, false, true);
					}
					else
					{
						this.setNewDialogue("MarriageDialogue", "jobReturn_", -1, false, false);
					}
				}
				if (Game1.currentLocation is FarmHouse)
				{
					Game1.playSound("doorClose");
				}
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00074344 File Offset: 0x00072544
		public Farmer getSpouse()
		{
			foreach (Farmer f in Game1.getAllFarmers())
			{
				if (f.spouse != null && f.spouse.Equals(this.name))
				{
					return f;
				}
			}
			return null;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000743B4 File Offset: 0x000725B4
		public string getTermOfSpousalEndearment(bool happy = true)
		{
			Farmer spouse = this.getSpouse();
			if (spouse != null)
			{
				if (spouse.getFriendshipHeartLevelForNPC(this.name) < 9)
				{
					return spouse.name;
				}
				if (happy && Game1.random.NextDouble() < 0.08)
				{
					switch (Game1.random.Next(8))
					{
					case 0:
						return "Pookie";
					case 1:
						return "Love";
					case 2:
						return "Hot Stuff";
					case 3:
						return "Cuddlebug";
					case 4:
						if (!spouse.isMale)
						{
							return "Peach";
						}
						return "Hunky";
					case 5:
						return "Cutie";
					case 6:
						return "Ducky";
					case 7:
						if (!spouse.isMale)
						{
							return "Darling";
						}
						return "Handsome";
					}
				}
				if (!happy)
				{
					switch (Game1.random.Next(2))
					{
					case 0:
						return "Honey";
					case 1:
						return "Dear";
					case 2:
						return spouse.name;
					}
				}
				switch (Game1.random.Next(5))
				{
				case 0:
					return "Sweetie";
				case 1:
					return "Dear";
				case 2:
					return "Honey";
				case 3:
					return "Hun";
				case 4:
					return "Sweetheart";
				}
			}
			return "Honey";
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00074504 File Offset: 0x00072704
		public bool spouseObstacleCheck(string backToBedMessage, GameLocation currentLocation, bool force = false)
		{
			if (force || currentLocation.isTileOccupied(base.getTileLocation(), this.name))
			{
				Game1.warpCharacter(this, this.defaultMap, (Game1.getLocationFromName(this.defaultMap) as FarmHouse).getSpouseBedSpot(), false, false);
				this.faceDirection(1);
				this.setNewDialogue(backToBedMessage, false, true);
				return true;
			}
			return false;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0007455E File Offset: 0x0007275E
		public void setTilePosition(Point p)
		{
			this.setTilePosition(p.X, p.Y);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00074572 File Offset: 0x00072772
		public void setTilePosition(int x, int y)
		{
			this.position = new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize));
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0007458F File Offset: 0x0007278F
		private void clintHammerSound(Farmer who)
		{
			if (Game1.currentLocation.name.Equals("Blacksmith"))
			{
				Game1.playSound("hammer");
				this.sprite.currentAnimationIndex = Game1.random.Next(11);
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000745C8 File Offset: 0x000727C8
		private void robinHammerSound(Farmer who)
		{
			if (Game1.currentLocation.Equals(this.currentLocation) && Utility.isOnScreen(this.position, Game1.tileSize * 4))
			{
				Game1.playSound((Game1.random.NextDouble() < 0.1) ? "clank" : "axchop");
				this.shakeTimer = 250;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0007462C File Offset: 0x0007282C
		private void robinVariablePause(Farmer who)
		{
			if (Game1.random.NextDouble() < 0.4)
			{
				this.sprite.currentAnimation[this.sprite.currentAnimationIndex] = new FarmerSprite.AnimationFrame(27, 300, false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false);
				return;
			}
			if (Game1.random.NextDouble() < 0.25)
			{
				this.sprite.currentAnimation[this.sprite.currentAnimationIndex] = new FarmerSprite.AnimationFrame(23, Game1.random.Next(500, 4000), false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false);
				return;
			}
			this.sprite.currentAnimation[this.sprite.currentAnimationIndex] = new FarmerSprite.AnimationFrame(27, Game1.random.Next(1000, 4000), false, false, new AnimatedSprite.endOfAnimationBehavior(this.robinVariablePause), false);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00002834 File Offset: 0x00000A34
		public void ReachedEndPoint()
		{
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00074728 File Offset: 0x00072928
		public void changeSchedulePathDirection()
		{
			Microsoft.Xna.Framework.Rectangle boundingbox = this.GetBoundingBox();
			boundingbox.Inflate(2, 2);
			Microsoft.Xna.Framework.Rectangle arg_16_0 = this.lastCrossroad;
			if (!this.lastCrossroad.Intersects(boundingbox))
			{
				this.isCharging = false;
				this.speed = 2;
				this.directionIndex++;
				this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(base.getStandingX() - base.getStandingX() % Game1.tileSize, base.getStandingY() - base.getStandingY() % Game1.tileSize, Game1.tileSize, Game1.tileSize);
				this.moveCharacterOnSchedulePath();
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00002834 File Offset: 0x00000A34
		public void moveCharacterOnSchedulePath()
		{
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000747B8 File Offset: 0x000729B8
		public void randomSquareMovement(GameTime time)
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			boundingBox.Inflate(2, 2);
			Microsoft.Xna.Framework.Rectangle endRect = new Microsoft.Xna.Framework.Rectangle((int)this.nextSquarePosition.X * Game1.tileSize, (int)this.nextSquarePosition.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			Vector2 arg_4B_0 = this.nextSquarePosition;
			if (this.nextSquarePosition.Equals(Vector2.Zero))
			{
				this.squarePauseAccumulation = 0;
				this.squarePauseTotal = Game1.random.Next(6000 + this.squarePauseOffset, 12000 + this.squarePauseOffset);
				this.nextSquarePosition = new Vector2((float)(this.lastCrossroad.X / Game1.tileSize - this.lengthOfWalkingSquareX / 2 + Game1.random.Next(this.lengthOfWalkingSquareX)), (float)(this.lastCrossroad.Y / Game1.tileSize - this.lengthOfWalkingSquareY / 2 + Game1.random.Next(this.lengthOfWalkingSquareY)));
			}
			else if (endRect.Contains(boundingBox))
			{
				this.Halt();
				if (this.squareMovementFacingPreference != -1)
				{
					this.faceDirection(this.squareMovementFacingPreference);
				}
				this.isCharging = false;
				this.speed = 2;
			}
			else if (boundingBox.Left <= endRect.Left)
			{
				this.SetMovingOnlyRight();
			}
			else if (boundingBox.Right >= endRect.Right)
			{
				this.SetMovingOnlyLeft();
			}
			else if (boundingBox.Top <= endRect.Top)
			{
				this.SetMovingOnlyDown();
			}
			else if (boundingBox.Bottom >= endRect.Bottom)
			{
				this.SetMovingOnlyUp();
			}
			this.squarePauseAccumulation += time.ElapsedGameTime.Milliseconds;
			if (this.squarePauseAccumulation >= this.squarePauseTotal && endRect.Contains(boundingBox))
			{
				this.nextSquarePosition = Vector2.Zero;
				this.isCharging = false;
				this.speed = 2;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0007499C File Offset: 0x00072B9C
		public void returnToEndPoint()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			boundingBox.Inflate(2, 2);
			if (boundingBox.Left <= this.lastCrossroad.Left)
			{
				this.SetMovingOnlyRight();
			}
			else if (boundingBox.Right >= this.lastCrossroad.Right)
			{
				this.SetMovingOnlyLeft();
			}
			else if (boundingBox.Top <= this.lastCrossroad.Top)
			{
				this.SetMovingOnlyDown();
			}
			else if (boundingBox.Bottom >= this.lastCrossroad.Bottom)
			{
				this.SetMovingOnlyUp();
			}
			boundingBox.Inflate(-2, -2);
			if (this.lastCrossroad.Contains(boundingBox))
			{
				this.isWalkingInSquare = false;
				this.nextSquarePosition = Vector2.Zero;
				this.returningToEndPoint = false;
				this.Halt();
				this.checkSchedule(this.timeAfterSquare);
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00074A6B File Offset: 0x00072C6B
		public void SetMovingOnlyUp()
		{
			this.moveUp = true;
			this.moveDown = false;
			this.moveLeft = false;
			this.moveRight = false;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00074A89 File Offset: 0x00072C89
		public void SetMovingOnlyRight()
		{
			this.moveUp = false;
			this.moveDown = false;
			this.moveLeft = false;
			this.moveRight = true;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00074AA7 File Offset: 0x00072CA7
		public void SetMovingOnlyDown()
		{
			this.moveUp = false;
			this.moveDown = true;
			this.moveLeft = false;
			this.moveRight = false;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00074AC5 File Offset: 0x00072CC5
		public void SetMovingOnlyLeft()
		{
			this.moveUp = false;
			this.moveDown = false;
			this.moveLeft = true;
			this.moveRight = false;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00074AE4 File Offset: 0x00072CE4
		public static void populateRoutesFromLocationToLocationList()
		{
			NPC.routesFromLocationToLocation = new List<List<string>>();
			foreach (GameLocation i in Game1.locations)
			{
				if (!(i is Farm) && !i.name.Equals("Backwoods"))
				{
					List<string> route = new List<string>();
					NPC.exploreWarpPoints(i, route);
				}
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00074B64 File Offset: 0x00072D64
		private static bool exploreWarpPoints(GameLocation l, List<string> route)
		{
			bool added = false;
			if (l != null && !route.Contains(l.name))
			{
				route.Add(l.name);
				if (route.Count == 1 || !NPC.doesRoutesListContain(route))
				{
					if (route.Count > 1)
					{
						NPC.routesFromLocationToLocation.Add(route.ToList<string>());
						added = true;
					}
					foreach (Warp w in l.warps)
					{
						if (!w.TargetName.Equals("Farm") && !w.TargetName.Equals("Woods") && !w.TargetName.Equals("Backwoods") && !w.TargetName.Equals("Tunnel"))
						{
							NPC.exploreWarpPoints(Game1.getLocationFromName(w.TargetName), route);
						}
					}
					foreach (Point p in l.doors.Keys)
					{
						NPC.exploreWarpPoints(Game1.getLocationFromName(l.doors[p]), route);
					}
				}
				if (route.Count > 0)
				{
					route.RemoveAt(route.Count - 1);
				}
			}
			return added;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00074CD0 File Offset: 0x00072ED0
		private static bool doesRoutesListContain(List<string> route)
		{
			foreach (List<string> i in NPC.routesFromLocationToLocation)
			{
				if (i.Count == route.Count)
				{
					bool allSame = true;
					for (int j = 0; j < route.Count; j++)
					{
						if (!i[j].Equals(route[j]))
						{
							allSame = false;
							break;
						}
					}
					if (allSame)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00074D64 File Offset: 0x00072F64
		public int CompareTo(object obj)
		{
			if (obj is NPC)
			{
				return (obj as NPC).id - this.id;
			}
			return 0;
		}

		// Token: 0x04000540 RID: 1344
		public const int minimum_square_pause = 6000;

		// Token: 0x04000541 RID: 1345
		public const int maximum_square_pause = 12000;

		// Token: 0x04000542 RID: 1346
		public const int portrait_width = 64;

		// Token: 0x04000543 RID: 1347
		public const int portrait_height = 64;

		// Token: 0x04000544 RID: 1348
		public const int portrait_neutral_index = 0;

		// Token: 0x04000545 RID: 1349
		public const int portrait_happy_index = 1;

		// Token: 0x04000546 RID: 1350
		public const int portrait_sad_index = 2;

		// Token: 0x04000547 RID: 1351
		public const int portrait_custom_index = 3;

		// Token: 0x04000548 RID: 1352
		public const int portrait_blush_index = 4;

		// Token: 0x04000549 RID: 1353
		public const int portrait_angry_index = 5;

		// Token: 0x0400054A RID: 1354
		public const int startingFriendship = 0;

		// Token: 0x0400054B RID: 1355
		public const int defaultSpeed = 2;

		// Token: 0x0400054C RID: 1356
		public const int maxGiftsPerWeek = 2;

		// Token: 0x0400054D RID: 1357
		public const int friendshipPointsPerHeartLevel = 250;

		// Token: 0x0400054E RID: 1358
		public const int maxFriendshipPoints = 2500;

		// Token: 0x0400054F RID: 1359
		public const int gift_taste_love = 0;

		// Token: 0x04000550 RID: 1360
		public const int gift_taste_like = 2;

		// Token: 0x04000551 RID: 1361
		public const int gift_taste_neutral = 8;

		// Token: 0x04000552 RID: 1362
		public const int gift_taste_dislike = 4;

		// Token: 0x04000553 RID: 1363
		public const int gift_taste_hate = 6;

		// Token: 0x04000554 RID: 1364
		public const int textStyle_shake = 0;

		// Token: 0x04000555 RID: 1365
		public const int textStyle_fade = 1;

		// Token: 0x04000556 RID: 1366
		public const int textStyle_none = 2;

		// Token: 0x04000557 RID: 1367
		public const int adult = 0;

		// Token: 0x04000558 RID: 1368
		public const int teen = 1;

		// Token: 0x04000559 RID: 1369
		public const int child = 2;

		// Token: 0x0400055A RID: 1370
		public const int neutral = 0;

		// Token: 0x0400055B RID: 1371
		public const int polite = 1;

		// Token: 0x0400055C RID: 1372
		public const int rude = 2;

		// Token: 0x0400055D RID: 1373
		public const int outgoing = 0;

		// Token: 0x0400055E RID: 1374
		public const int shy = 1;

		// Token: 0x0400055F RID: 1375
		public const int positive = 0;

		// Token: 0x04000560 RID: 1376
		public const int negative = 1;

		// Token: 0x04000561 RID: 1377
		public const int male = 0;

		// Token: 0x04000562 RID: 1378
		public const int female = 1;

		// Token: 0x04000563 RID: 1379
		public const int undefined = 2;

		// Token: 0x04000564 RID: 1380
		public const int other = 0;

		// Token: 0x04000565 RID: 1381
		public const int desert = 1;

		// Token: 0x04000566 RID: 1382
		public const int town = 2;

		// Token: 0x04000567 RID: 1383
		private Dictionary<int, SchedulePathDescription> schedule;

		// Token: 0x04000568 RID: 1384
		private Dictionary<string, string> dialogue;

		// Token: 0x04000569 RID: 1385
		private SchedulePathDescription directionsToNewLocation;

		// Token: 0x0400056A RID: 1386
		private int directionIndex;

		// Token: 0x0400056B RID: 1387
		private int lengthOfWalkingSquareX;

		// Token: 0x0400056C RID: 1388
		private int lengthOfWalkingSquareY;

		// Token: 0x0400056D RID: 1389
		private int squarePauseAccumulation;

		// Token: 0x0400056E RID: 1390
		private int squarePauseTotal;

		// Token: 0x0400056F RID: 1391
		private int squarePauseOffset;

		// Token: 0x04000570 RID: 1392
		protected Microsoft.Xna.Framework.Rectangle lastCrossroad;

		// Token: 0x04000571 RID: 1393
		public string defaultMap;

		// Token: 0x04000572 RID: 1394
		public string loveInterest;

		// Token: 0x04000573 RID: 1395
		public string birthday_Season;

		// Token: 0x04000574 RID: 1396
		private Texture2D portrait;

		// Token: 0x04000575 RID: 1397
		private Vector2 defaultPosition;

		// Token: 0x04000576 RID: 1398
		private Vector2 nextSquarePosition;

		// Token: 0x04000577 RID: 1399
		protected int defaultFacingDirection;

		// Token: 0x04000578 RID: 1400
		protected int idForClones = -1;

		// Token: 0x04000579 RID: 1401
		protected int shakeTimer;

		// Token: 0x0400057A RID: 1402
		private bool isWalkingInSquare;

		// Token: 0x0400057B RID: 1403
		private bool isWalkingTowardPlayer;

		// Token: 0x0400057C RID: 1404
		private Stack<Dialogue> currentDialogue = new Stack<Dialogue>();

		// Token: 0x0400057D RID: 1405
		private static List<List<string>> routesFromLocationToLocation;

		// Token: 0x0400057E RID: 1406
		protected string textAboveHead;

		// Token: 0x0400057F RID: 1407
		protected string displayName;

		// Token: 0x04000580 RID: 1408
		protected int textAboveHeadPreTimer;

		// Token: 0x04000581 RID: 1409
		protected int textAboveHeadTimer;

		// Token: 0x04000582 RID: 1410
		protected int textAboveHeadStyle;

		// Token: 0x04000583 RID: 1411
		protected int textAboveHeadColor;

		// Token: 0x04000584 RID: 1412
		protected float textAboveHeadAlpha;

		// Token: 0x04000585 RID: 1413
		public int age;

		// Token: 0x04000586 RID: 1414
		public int manners;

		// Token: 0x04000587 RID: 1415
		public int socialAnxiety;

		// Token: 0x04000588 RID: 1416
		public int optimism;

		// Token: 0x04000589 RID: 1417
		public int gender;

		// Token: 0x0400058A RID: 1418
		public int id = -1;

		// Token: 0x0400058B RID: 1419
		public int homeRegion;

		// Token: 0x0400058C RID: 1420
		public int daysUntilBirthing = -1;

		// Token: 0x0400058D RID: 1421
		public int daysAfterLastBirth = -1;

		// Token: 0x0400058E RID: 1422
		public int birthday_Day;

		// Token: 0x0400058F RID: 1423
		private string extraDialogueMessageToAddThisMorning;

		// Token: 0x04000590 RID: 1424
		[XmlIgnore]
		public PathFindController temporaryController;

		// Token: 0x04000591 RID: 1425
		[XmlIgnore]
		public GameLocation currentLocation;

		// Token: 0x04000592 RID: 1426
		[XmlIgnore]
		public bool updatedDialogueYet;

		// Token: 0x04000593 RID: 1427
		[XmlIgnore]
		public bool uniqueSpriteActive;

		// Token: 0x04000594 RID: 1428
		[XmlIgnore]
		public bool uniquePortraitActive;

		// Token: 0x04000595 RID: 1429
		[XmlIgnore]
		public bool breather = true;

		// Token: 0x04000596 RID: 1430
		[XmlIgnore]
		public bool hideShadow;

		// Token: 0x04000597 RID: 1431
		[XmlIgnore]
		public bool hasPartnerForDance;

		// Token: 0x04000598 RID: 1432
		[XmlIgnore]
		public bool immediateSpeak;

		// Token: 0x04000599 RID: 1433
		[XmlIgnore]
		public bool ignoreScheduleToday;

		// Token: 0x0400059A RID: 1434
		public int moveTowardPlayerThreshold;

		// Token: 0x0400059B RID: 1435
		[XmlIgnore]
		public float rotation;

		// Token: 0x0400059C RID: 1436
		[XmlIgnore]
		public float yOffset;

		// Token: 0x0400059D RID: 1437
		[XmlIgnore]
		public float swimTimer;

		// Token: 0x0400059E RID: 1438
		[XmlIgnore]
		public float timerSinceLastMovement;

		// Token: 0x0400059F RID: 1439
		[XmlIgnore]
		public string mapBeforeEvent;

		// Token: 0x040005A0 RID: 1440
		[XmlIgnore]
		public Vector2 positionBeforeEvent;

		// Token: 0x040005A1 RID: 1441
		[XmlIgnore]
		public Vector2 lastPosition;

		// Token: 0x040005A2 RID: 1442
		public bool isInvisible;

		// Token: 0x040005A3 RID: 1443
		public bool followSchedule = true;

		// Token: 0x040005A4 RID: 1444
		public bool datable;

		// Token: 0x040005A5 RID: 1445
		public bool datingFarmer;

		// Token: 0x040005A6 RID: 1446
		public bool divorcedFromFarmer;

		// Token: 0x040005A7 RID: 1447
		private bool hasBeenKissedToday;

		// Token: 0x040005A8 RID: 1448
		private int timeAfterSquare;

		// Token: 0x040005A9 RID: 1449
		[XmlIgnore]
		public bool doingEndOfRouteAnimation;

		// Token: 0x040005AA RID: 1450
		[XmlIgnore]
		public bool goingToDoEndOfRouteAnimation;

		// Token: 0x040005AB RID: 1451
		private int[] routeEndIntro;

		// Token: 0x040005AC RID: 1452
		private int[] routeEndAnimation;

		// Token: 0x040005AD RID: 1453
		private int[] routeEndOutro;

		// Token: 0x040005AE RID: 1454
		[XmlIgnore]
		public string endOfRouteMessage;

		// Token: 0x040005AF RID: 1455
		[XmlIgnore]
		public string nextEndOfRouteMessage;

		// Token: 0x040005B0 RID: 1456
		private string endOfRouteBehaviorName;

		// Token: 0x040005B1 RID: 1457
		private Point previousEndPoint;

		// Token: 0x040005B2 RID: 1458
		protected int scheduleTimeToTry = 9999999;

		// Token: 0x040005B3 RID: 1459
		protected int squareMovementFacingPreference;

		// Token: 0x040005B4 RID: 1460
		private const int NO_TRY = 9999999;

		// Token: 0x040005B5 RID: 1461
		private bool returningToEndPoint;

		// Token: 0x040005B6 RID: 1462
		private bool hasSaidAfternoonDialogue;

		// Token: 0x040005B7 RID: 1463
		private string nameOfTodaysSchedule = "";

		// Token: 0x040005B8 RID: 1464
		private int married = -1;

		// Token: 0x040005B9 RID: 1465
		public int daysMarried;
	}
}
