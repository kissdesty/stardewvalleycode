using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000027 RID: 39
	public class FarmAnimal : Character
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0002F958 File Offset: 0x0002DB58
		public FarmAnimal()
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0002F978 File Offset: 0x0002DB78
		public FarmAnimal(string type, long id, long ownerID) : base(null, new Vector2((float)(Game1.tileSize * Game1.random.Next(2, 9)), (float)(Game1.tileSize * Game1.random.Next(4, 8))), 2, type)
		{
			this.ownerID = ownerID;
			this.health = 3;
			if (type.Contains("Chicken") && !type.Equals("Void Chicken"))
			{
				if (Game1.IsMultiplayer)
				{
					type = ((type.Contains("Brown") || new Random((int)id).NextDouble() < 0.5) ? "Brown Chicken" : "White Chicken");
				}
				else
				{
					type = ((Game1.random.NextDouble() < 0.5 || type.Contains("Brown")) ? "Brown Chicken" : "White Chicken");
					if (Game1.player.eventsSeen.Contains(3900074) && Game1.random.NextDouble() < 0.25)
					{
						type = "Blue Chicken";
					}
				}
			}
			if (type.Contains("Cow"))
			{
				if (Game1.IsMultiplayer)
				{
					type = ((!type.Contains("White") && (type.Contains("Brown") || new Random((int)id).NextDouble() < 0.5)) ? "Brown Cow" : "White Cow");
				}
				else
				{
					type = ((!type.Contains("White") && (Game1.random.NextDouble() < 0.5 || type.Contains("Brown"))) ? "Brown Cow" : "White Cow");
				}
			}
			Dictionary<string, string> arg_1C4_0 = Game1.content.Load<Dictionary<string, string>>("Data\\FarmAnimals");
			this.myID = id;
			string rawData;
			arg_1C4_0.TryGetValue(type, out rawData);
			if (rawData != null)
			{
				string[] split = rawData.Split(new char[]
				{
					'/'
				});
				this.daysToLay = Convert.ToByte(split[0]);
				this.ageWhenMature = Convert.ToByte(split[1]);
				this.defaultProduceIndex = Convert.ToInt32(split[2]);
				this.deluxeProduceIndex = Convert.ToInt32(split[3]);
				this.sound = (split[4].Equals("none") ? null : split[4]);
				this.frontBackBoundingBox = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(split[5]), Convert.ToInt32(split[6]), Convert.ToInt32(split[7]), Convert.ToInt32(split[8]));
				this.sidewaysBoundingBox = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(split[9]), Convert.ToInt32(split[10]), Convert.ToInt32(split[11]), Convert.ToInt32(split[12]));
				this.harvestType = Convert.ToByte(split[13]);
				this.showDifferentTextureWhenReadyForHarvest = Convert.ToBoolean(split[14]);
				this.buildingTypeILiveIn = split[15];
				int sourceWidth = Convert.ToInt32(split[16]);
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + ((this.ageWhenMature > 0) ? "Baby" : "") + (type.Equals("Duck") ? "White Chicken" : type)), 0, sourceWidth, Convert.ToInt32(split[17]));
				this.frontBackSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, Convert.ToInt32(split[16]), Convert.ToInt32(split[17]));
				this.sidewaysSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, Convert.ToInt32(split[18]), Convert.ToInt32(split[19]));
				this.fullnessDrain = Convert.ToByte(split[20]);
				this.happinessDrain = Convert.ToByte(split[21]);
				this.happiness = 255;
				this.fullness = 255;
				this.toolUsedForHarvest = ((split[22].Length > 0) ? split[22] : "");
				this.meatIndex = Convert.ToInt32(split[23]);
				this.price = Convert.ToInt32(split[24]);
			}
			this.type = type;
			this.name = Dialogue.randomName();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0002FD59 File Offset: 0x0002DF59
		public bool isCoopDweller()
		{
			return this.home != null && this.home is Coop;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0002FD74 File Offset: 0x0002DF74
		public override Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			return new Microsoft.Xna.Framework.Rectangle((int)(this.position.X + (float)(this.sprite.getWidth() * 4 / 2) - (float)(Game1.tileSize / 2) + 8f), (int)(this.position.Y + (float)(this.sprite.getHeight() * 4) - (float)Game1.tileSize + 8f), Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0002FDEC File Offset: 0x0002DFEC
		public void reload()
		{
			string textureName = this.type;
			if (this.age < (int)this.ageWhenMature)
			{
				textureName = "Baby" + (this.type.Equals("Duck") ? "White Chicken" : this.type);
			}
			else if (this.showDifferentTextureWhenReadyForHarvest && this.currentProduce <= 0)
			{
				textureName = "Sheared" + this.type;
			}
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + textureName), 0, this.frontBackSourceRect.Width, this.frontBackSourceRect.Height);
			if (Game1.getLocationFromName("Farm") != null)
			{
				foreach (Building b in (Game1.getLocationFromName("Farm") as Farm).buildings)
				{
					if (b.tileX == (int)this.homeLocation.X && b.tileY == (int)this.homeLocation.Y)
					{
						this.home = b;
						break;
					}
				}
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0002FF20 File Offset: 0x0002E120
		public void pet(Farmer who)
		{
			if (who.FarmerSprite.pauseForSingleAnimation)
			{
				return;
			}
			who.Halt();
			who.faceGeneralDirection(this.position, 0);
			if (Game1.timeOfDay >= 1900 && !this.isMoving())
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\FarmAnimals:TryingToSleep", new object[]
				{
					this.name
				}));
				return;
			}
			this.Halt();
			this.sprite.StopAnimation();
			this.uniqueFrameAccumulator = -1;
			switch (Game1.player.FacingDirection)
			{
			case 0:
				this.sprite.currentFrame = 0;
				break;
			case 1:
				this.sprite.currentFrame = 12;
				break;
			case 2:
				this.sprite.currentFrame = 8;
				break;
			case 3:
				this.sprite.currentFrame = 4;
				break;
			}
			if (!this.wasPet)
			{
				this.wasPet = true;
				this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 15);
				if (who.professions.Contains(3) && !this.isCoopDweller())
				{
					this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 15);
					this.happiness = Math.Min(255, (byte)((int)this.happiness + Math.Max(5, (int)(40 - this.happinessDrain))));
				}
				else if (who.professions.Contains(2) && this.isCoopDweller())
				{
					this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 15);
					this.happiness = Math.Min(255, (byte)((int)this.happiness + Math.Max(5, (int)(40 - this.happinessDrain))));
				}
				base.doEmote((this.moodMessage == 4) ? 12 : 20, true);
				this.happiness = Math.Min(255, (byte)((int)this.happiness + Math.Max(5, (int)(40 - this.happinessDrain))));
				if (this.sound != null && Game1.soundBank != null)
				{
					Cue expr_200 = Game1.soundBank.GetCue(this.sound);
					expr_200.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
					expr_200.Play();
				}
				who.gainExperience(0, 5);
			}
			else if (who.ActiveObject == null || who.ActiveObject.parentSheetIndex != 178)
			{
				Game1.activeClickableMenu = new AnimalQueryMenu(this);
			}
			if (this.type.Equals("Sheep") && this.friendshipTowardFarmer >= 900)
			{
				this.daysToLay = 2;
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000301B0 File Offset: 0x0002E3B0
		public void farmerPushing()
		{
			this.pushAccumulator++;
			if (this.pushAccumulator > 40)
			{
				switch (Game1.player.facingDirection)
				{
				case 0:
					this.Halt();
					this.SetMovingUp(true);
					break;
				case 1:
					this.Halt();
					this.SetMovingRight(true);
					break;
				case 2:
					this.Halt();
					this.SetMovingDown(true);
					break;
				case 3:
					this.Halt();
					this.SetMovingLeft(true);
					break;
				}
				Game1.player.temporaryImpassableTile = this.GetBoundingBox();
				this.pushAccumulator = 0;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0003024C File Offset: 0x0002E44C
		public void setRandomPosition(GameLocation location)
		{
			string[] expr_1B = location.getMapProperty("ProduceArea").Split(new char[]
			{
				' '
			});
			int produceX = Convert.ToInt32(expr_1B[0]);
			int produceY = Convert.ToInt32(expr_1B[1]);
			int produceWidth = Convert.ToInt32(expr_1B[2]);
			int produceHeight = Convert.ToInt32(expr_1B[3]);
			this.position = new Vector2((float)(Game1.random.Next(produceX, produceX + produceWidth) * Game1.tileSize), (float)(Game1.random.Next(produceY, produceY + produceHeight) * Game1.tileSize));
			int tries = 0;
			while (this.position.Equals(Vector2.Zero) || location.Objects.ContainsKey(this.position) || location.isCollidingPosition(this.GetBoundingBox(), Game1.viewport, false, 0, false, this))
			{
				this.position = new Vector2((float)Game1.random.Next(produceX, produceX + produceWidth), (float)Game1.random.Next(produceY, produceY + produceHeight)) * (float)Game1.tileSize;
				tries++;
				if (tries > 64)
				{
					break;
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00030350 File Offset: 0x0002E550
		public void dayUpdate(GameLocation environtment)
		{
			this.controller = null;
			this.health = 3;
			bool wasLeftOutLastNight = false;
			if (this.home != null && !(this.home.indoors as AnimalHouse).animals.ContainsKey(this.myID) && environtment is Farm)
			{
				if (!this.home.animalDoorOpen)
				{
					this.moodMessage = 6;
					wasLeftOutLastNight = true;
					this.happiness /= 2;
				}
				else
				{
					(environtment as Farm).animals.Remove(this.myID);
					(this.home.indoors as AnimalHouse).animals.Add(this.myID, this);
					if (Game1.timeOfDay > 1800 && this.controller == null)
					{
						this.happiness /= 2;
					}
					environtment = this.home.indoors;
					this.setRandomPosition(environtment);
				}
			}
			this.daysSinceLastLay += 1;
			if (!this.wasPet)
			{
				this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - (10 - this.friendshipTowardFarmer / 200));
				this.happiness = (byte)Math.Max(0, (int)(this.happiness - this.happinessDrain * 5));
			}
			this.wasPet = false;
			if ((this.fullness < 200 || Game1.timeOfDay < 1700) && environtment is AnimalHouse)
			{
				for (int i = environtment.objects.Count - 1; i >= 0; i--)
				{
					if (environtment.objects.ElementAt(i).Value.Name.Equals("Hay"))
					{
						environtment.objects.Remove(environtment.objects.ElementAt(i).Key);
						this.fullness = 255;
						break;
					}
				}
			}
			Random r = new Random((int)this.myID / 2 + (int)Game1.stats.DaysPlayed);
			if (this.fullness > 200 || r.NextDouble() < (double)(this.fullness - 30) / 170.0)
			{
				this.age++;
				if (this.age == (int)this.ageWhenMature)
				{
					this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\" + this.type);
					if (this.type.Contains("Sheep"))
					{
						this.currentProduce = this.defaultProduceIndex;
					}
					this.daysSinceLastLay = 99;
				}
				this.happiness = (byte)Math.Min(255, (int)(this.happiness + this.happinessDrain * 2));
			}
			if (this.fullness < 200)
			{
				this.happiness = (byte)Math.Max(0, (int)(this.happiness - 100));
				this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - 20);
			}
			bool produceToday = this.daysSinceLastLay >= this.daysToLay - ((this.type.Equals("Sheep") && Game1.getFarmer(this.ownerID).professions.Contains(3)) ? 1 : 0) && r.NextDouble() < (double)this.fullness / 200.0 && r.NextDouble() < (double)this.happiness / 70.0;
			int whichProduce;
			if (!produceToday || this.age < (int)this.ageWhenMature)
			{
				whichProduce = -1;
			}
			else
			{
				whichProduce = this.defaultProduceIndex;
				if (r.NextDouble() < (double)this.happiness / 150.0)
				{
					int happinessModifier = (int)((this.happiness > 200) ? (this.happiness * 2) : (-this.happiness * 2));
					if (this.type.Equals("Duck") && r.NextDouble() < (double)(this.friendshipTowardFarmer + happinessModifier) / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.01)
					{
						whichProduce = this.deluxeProduceIndex;
					}
					else if (this.type.Equals("Rabbit") && r.NextDouble() < (double)(this.friendshipTowardFarmer + happinessModifier) / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.02)
					{
						whichProduce = this.deluxeProduceIndex;
					}
					this.daysSinceLastLay = 0;
					if (whichProduce <= 180)
					{
						if (whichProduce != 176)
						{
							if (whichProduce == 180)
							{
								Stats expr_499 = Game1.stats;
								uint num = expr_499.ChickenEggsLayed;
								expr_499.ChickenEggsLayed = num + 1u;
							}
						}
						else
						{
							Stats expr_481 = Game1.stats;
							uint num = expr_481.ChickenEggsLayed;
							expr_481.ChickenEggsLayed = num + 1u;
						}
					}
					else if (whichProduce != 440)
					{
						if (whichProduce == 442)
						{
							Stats expr_4B1 = Game1.stats;
							uint num = expr_4B1.DuckEggsLayed;
							expr_4B1.DuckEggsLayed = num + 1u;
						}
					}
					else
					{
						Stats expr_4C9 = Game1.stats;
						uint num = expr_4C9.RabbitWoolProduced;
						expr_4C9.RabbitWoolProduced = num + 1u;
					}
					if (r.NextDouble() < (double)(this.friendshipTowardFarmer + happinessModifier) / 1200.0 && !this.type.Equals("Duck") && !this.type.Equals("Rabbit"))
					{
						whichProduce = this.deluxeProduceIndex;
					}
					double chanceForQuality = (double)((float)this.friendshipTowardFarmer / 1000f - (1f - (float)this.happiness / 225f));
					if ((!this.isCoopDweller() && Game1.getFarmer(this.ownerID).professions.Contains(3)) || (this.isCoopDweller() && Game1.getFarmer(this.ownerID).professions.Contains(2)))
					{
						chanceForQuality += 0.33;
					}
					if (chanceForQuality >= 0.95 && r.NextDouble() < chanceForQuality / 2.0)
					{
						this.produceQuality = 4;
					}
					else if (r.NextDouble() < chanceForQuality / 2.0)
					{
						this.produceQuality = 2;
					}
					else if (r.NextDouble() < chanceForQuality)
					{
						this.produceQuality = 1;
					}
					else
					{
						this.produceQuality = 0;
					}
				}
			}
			if (this.harvestType == 1 & produceToday)
			{
				this.currentProduce = whichProduce;
				whichProduce = -1;
			}
			if (whichProduce != -1 && this.home != null && !this.home.indoors.Objects.ContainsKey(base.getTileLocation()))
			{
				this.home.indoors.Objects.Add(base.getTileLocation(), new Object(Vector2.Zero, whichProduce, null, false, true, false, true)
				{
					quality = this.produceQuality
				});
			}
			if (!wasLeftOutLastNight)
			{
				if (this.fullness < 30)
				{
					this.moodMessage = 4;
				}
				else if (this.happiness < 30)
				{
					this.moodMessage = 3;
				}
				else if (this.happiness < 200)
				{
					this.moodMessage = 2;
				}
				else
				{
					this.moodMessage = 1;
				}
			}
			if (Game1.timeOfDay < 1700)
			{
				this.fullness = (byte)Math.Max(0, (int)this.fullness - (int)this.fullnessDrain * (1700 - Game1.timeOfDay) / 100);
			}
			this.fullness = 0;
			if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
			{
				this.fullness = 250;
			}
			this.reload();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00030A6C File Offset: 0x0002EC6C
		public int getSellPrice()
		{
			double adjustedFriendship = (double)this.friendshipTowardFarmer / 1000.0 + 0.3;
			return (int)((double)this.price * adjustedFriendship);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00030AA0 File Offset: 0x0002ECA0
		public bool isMale()
		{
			string a = this.type;
			if (!(a == "Rabbit"))
			{
				return (a == "Truffle Pig" || a == "Hog" || a == "Pig") && this.myID % 2L == 0L;
			}
			return this.myID % 2L == 0L;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00030B08 File Offset: 0x0002ED08
		public string getMoodMessage()
		{
			if (this.harvestType == 2)
			{
				this.name = "It";
			}
			string gender = this.isMale() ? "Male" : "Female";
			switch (this.moodMessage)
			{
			case 0:
				if (this.parentId != -1L)
				{
					return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_NewHome_Baby_" + gender, new object[]
					{
						this.name
					});
				}
				return Game1.content.LoadString(string.Concat(new object[]
				{
					"Strings\\FarmAnimals:MoodMessage_NewHome_Adult_",
					gender,
					"_",
					Game1.dayOfMonth % 2 + 1
				}), new object[]
				{
					this.name
				});
			case 4:
				return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_" + ((((long)Game1.dayOfMonth + this.myID) % 2L == 0L) ? "Hungry1" : "Hungry2"), new object[]
				{
					this.name
				});
			case 5:
				return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_DisturbedByDog_" + gender, new object[]
				{
					this.name
				});
			case 6:
				return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_LeftOutsideAtNight_" + gender, new object[]
				{
					this.name
				});
			}
			if (this.happiness < 30)
			{
				this.moodMessage = 3;
			}
			else if (this.happiness < 200)
			{
				this.moodMessage = 2;
			}
			else
			{
				this.moodMessage = 1;
			}
			switch (this.moodMessage)
			{
			case 1:
				return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_Happy", new object[]
				{
					this.name
				});
			case 2:
				return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_Fine", new object[]
				{
					this.name
				});
			case 3:
				return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_Sad", new object[]
				{
					this.name
				});
			default:
				return "";
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00030D20 File Offset: 0x0002EF20
		public bool isBaby()
		{
			return this.age < (int)this.ageWhenMature;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00030D30 File Offset: 0x0002EF30
		public void warpHome(Farm f, FarmAnimal a)
		{
			if (this.home != null)
			{
				(this.home.indoors as AnimalHouse).animals.Add(this.myID, this);
				f.animals.Remove(this.myID);
				this.controller = null;
				this.setRandomPosition(this.home.indoors);
				this.home.currentOccupants++;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00030DA4 File Offset: 0x0002EFA4
		public override void draw(SpriteBatch b)
		{
			if (this.isCoopDweller())
			{
				this.sprite.drawShadow(b, Game1.GlobalToLocal(Game1.viewport, this.position - new Vector2(0f, (float)(Game1.tileSize * 3 / 8))), this.isBaby() ? 3f : 4f);
			}
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position - new Vector2(0f, (float)(Game1.tileSize * 3 / 8))), ((float)(this.GetBoundingBox().Center.Y + Game1.pixelZoom) + this.position.X / 1000f) / 10000f, 0, 0, (this.hitGlowTimer > 0) ? Color.Red : Color.White, base.FacingDirection == 3, 4f, 0f, false);
			if (this.isEmoting)
			{
				Vector2 emotePosition = Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(this.frontBackSourceRect.Width / 2 * 4 - Game1.tileSize / 2), (float)(this.isCoopDweller() ? (-(float)Game1.tileSize * 3 / 2) : (-(float)Game1.tileSize))));
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.GetBoundingBox().Bottom / 10000f);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00030F60 File Offset: 0x0002F160
		public virtual void updateWhenNotCurrentLocation(Building currentBuilding, GameTime time, GameLocation environment)
		{
			if (!Game1.shouldTimePass())
			{
				return;
			}
			this.update(time, environment, this.myID, false);
			if (currentBuilding != null && Game1.random.NextDouble() < 0.002 && currentBuilding.animalDoorOpen && Game1.timeOfDay < 1630 && !Game1.isRaining && !Game1.currentSeason.Equals("winter") && environment.getFarmers().Count == 0 && !environment.Equals(Game1.currentLocation))
			{
				Farm farm = (Farm)Game1.getLocationFromName("Farm");
				if (farm.isCollidingPosition(new Microsoft.Xna.Framework.Rectangle((currentBuilding.tileX + currentBuilding.animalDoor.X) * Game1.tileSize + 2, (currentBuilding.tileY + currentBuilding.animalDoor.Y) * Game1.tileSize + 2, (this.isCoopDweller() ? Game1.tileSize : (Game1.tileSize * 2)) - 4, Game1.tileSize - 4), Game1.viewport, false, 0, false, this, false, false, false) || farm.isCollidingPosition(new Microsoft.Xna.Framework.Rectangle((currentBuilding.tileX + currentBuilding.animalDoor.X) * Game1.tileSize + 2, (currentBuilding.tileY + currentBuilding.animalDoor.Y + 1) * Game1.tileSize + 2, (this.isCoopDweller() ? Game1.tileSize : (Game1.tileSize * 2)) - 4, Game1.tileSize - 4), Game1.viewport, false, 0, false, this, false, false, false))
				{
					return;
				}
				if (farm.animals.ContainsKey(this.myID))
				{
					for (int i = farm.animals.Count - 1; i >= 0; i--)
					{
						if (farm.animals.ElementAt(i).Key.Equals(this.myID))
						{
							farm.animals.Remove(this.myID);
							break;
						}
					}
				}
				farm.animals.Add(this.myID, this);
				((AnimalHouse)currentBuilding.indoors).animals.Remove(this.myID);
				this.faceDirection(2);
				this.SetMovingDown(true);
				this.position = new Vector2((float)currentBuilding.getRectForAnimalDoor().X, (float)((currentBuilding.tileY + currentBuilding.animalDoor.Y) * Game1.tileSize - (this.sprite.getHeight() * Game1.pixelZoom - this.GetBoundingBox().Height) + Game1.tileSize / 2));
				this.controller = new PathFindController(this, farm, new PathFindController.isAtEnd(FarmAnimal.grassEndPointFunction), Game1.random.Next(4), false, new PathFindController.endBehavior(FarmAnimal.behaviorAfterFindingGrassPatch), 200, Point.Zero);
				if (this.controller == null || this.controller.pathToEndPoint == null || this.controller.pathToEndPoint.Count < 3)
				{
					this.SetMovingDown(true);
					this.controller = null;
				}
				else
				{
					this.faceDirection(2);
					this.position = new Vector2((float)(this.controller.pathToEndPoint.Peek().X * Game1.tileSize), (float)(this.controller.pathToEndPoint.Peek().Y * Game1.tileSize - (this.sprite.getHeight() * Game1.pixelZoom - this.GetBoundingBox().Height) + Game1.tileSize / 4));
					if (!this.isCoopDweller())
					{
						this.position.X = this.position.X - (float)(Game1.tileSize / 2);
					}
				}
				this.noWarpTimer = 3000;
				currentBuilding.currentOccupants--;
				if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 3, farm))
				{
					Game1.playSound("sandyStep");
				}
				if (environment.isTileOccupiedByFarmer(base.getTileLocation()) != null)
				{
					environment.isTileOccupiedByFarmer(base.getTileLocation()).temporaryImpassableTile = this.GetBoundingBox();
				}
			}
			this.behaviors(time, environment);
			this.MovePosition(time, Game1.viewport, environment);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00031352 File Offset: 0x0002F552
		public static void behaviorAfterFindingGrassPatch(Character c, GameLocation environment)
		{
			if (((FarmAnimal)c).fullness < 255)
			{
				((FarmAnimal)c).eatGrass(environment);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00031374 File Offset: 0x0002F574
		public static bool animalDoorEndPointFunction(PathNode currentPoint, Point endPoint, GameLocation location, Character c)
		{
			Vector2 tileLocation = new Vector2((float)currentPoint.x, (float)currentPoint.y);
			foreach (Building building in ((Farm)location).buildings)
			{
				if (building.animalDoor.X >= 0 && (float)(building.animalDoor.X + building.tileX) == tileLocation.X && (float)(building.animalDoor.Y + building.tileY) == tileLocation.Y && building.buildingType.Contains(((FarmAnimal)c).buildingTypeILiveIn) && building.currentOccupants < building.maxOccupants)
				{
					building.currentOccupants++;
					Game1.playSound("dwop");
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0003146C File Offset: 0x0002F66C
		public static bool grassEndPointFunction(PathNode currentPoint, Point endPoint, GameLocation location, Character c)
		{
			Vector2 tileLocation = new Vector2((float)currentPoint.x, (float)currentPoint.y);
			if (location.terrainFeatures.ContainsKey(tileLocation) && location.terrainFeatures[tileLocation].GetType() == typeof(Grass))
			{
				foreach (KeyValuePair<long, FarmAnimal> kvp in ((Farm)location).animals)
				{
					if (kvp.Value.getTileLocation().X == (float)currentPoint.x && kvp.Value.getTileLocation().Y == (float)currentPoint.y)
					{
						break;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00031540 File Offset: 0x0002F740
		public virtual void updatePerTenMinutes(int timeOfDay, GameLocation environment)
		{
			if (timeOfDay >= 1800)
			{
				if ((environment.IsOutdoors && timeOfDay > 1900) || (!environment.IsOutdoors && this.happiness > 150) || (environment.isOutdoors && Game1.isRaining) || (environment.isOutdoors && Game1.currentSeason.Equals("winter")))
				{
					this.happiness = (byte)Math.Min(255, Math.Max(0, (int)(this.happiness - ((environment.numberOfObjectsWithName("Heater") > 0 && Game1.currentSeason.Equals("winter")) ? (-this.happinessDrain) : this.happinessDrain))));
				}
				else if (environment.IsOutdoors)
				{
					this.happiness = (byte)Math.Min(255, (int)(this.happiness + this.happinessDrain));
				}
			}
			if (environment.isTileOccupiedByFarmer(base.getTileLocation()) != null)
			{
				environment.isTileOccupiedByFarmer(base.getTileLocation()).temporaryImpassableTile = this.GetBoundingBox();
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00031640 File Offset: 0x0002F840
		public void eatGrass(GameLocation environment)
		{
			Vector2 tilePosition = new Vector2((float)(this.GetBoundingBox().Center.X / Game1.tileSize), (float)(this.GetBoundingBox().Center.Y / Game1.tileSize));
			if (environment.terrainFeatures.ContainsKey(tilePosition) && environment.terrainFeatures[tilePosition].GetType() == typeof(Grass))
			{
				this.isEating = true;
				if (((Grass)environment.terrainFeatures[tilePosition]).reduceBy(this.isCoopDweller() ? 2 : 4, tilePosition, environment.Equals(Game1.currentLocation)))
				{
					environment.terrainFeatures.Remove(tilePosition);
				}
				this.sprite.loop = false;
				this.fullness = 255;
				if (this.moodMessage != 5 && this.moodMessage != 6 && !Game1.isRaining)
				{
					this.happiness = 255;
					this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 8);
				}
				if (Game1.IsServer)
				{
					MultiplayerUtility.broadcastNPCBehavior(this.myID, environment, 0);
				}
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00031769 File Offset: 0x0002F969
		public override void performBehavior(byte which)
		{
			if (which == 0)
			{
				this.eatGrass(Game1.currentLocation);
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0003177C File Offset: 0x0002F97C
		private bool behaviors(GameTime time, GameLocation location)
		{
			if (this.home == null)
			{
				return false;
			}
			if (!this.isEating)
			{
				if (!Game1.IsClient)
				{
					if (this.controller != null)
					{
						return true;
					}
					if (location.IsOutdoors && this.fullness < 195 && Game1.random.NextDouble() < 0.002)
					{
						this.controller = new PathFindController(this, location, new PathFindController.isAtEnd(FarmAnimal.grassEndPointFunction), -1, false, new PathFindController.endBehavior(FarmAnimal.behaviorAfterFindingGrassPatch), 200, Point.Zero);
					}
					if (Game1.timeOfDay >= 1700 && location.IsOutdoors && this.controller == null && Game1.random.NextDouble() < 0.002)
					{
						this.controller = new PathFindController(this, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), 0, false, null, 200, new Point(this.home.tileX + this.home.animalDoor.X, this.home.tileY + this.home.animalDoor.Y));
						if (location.getFarmers().Count == 0)
						{
							((AnimalHouse)this.home.indoors).animals.Add(this.myID, this);
							this.setRandomPosition(this.home.indoors);
							this.faceDirection(Game1.random.Next(4));
							this.controller = null;
							(location as Farm).animals.Remove(this.myID);
							return true;
						}
					}
					if (location.IsOutdoors && !Game1.isRaining && !Game1.currentSeason.Equals("winter") && this.currentProduce != -1 && this.age >= (int)this.ageWhenMature && this.type.Contains("Pig") && Game1.random.NextDouble() < 0.0002)
					{
						foreach (Vector2 v in Utility.getCornersOfThisRectangle(this.GetBoundingBox()))
						{
							Vector2 vec = new Vector2((float)((int)(v.X / (float)Game1.tileSize)), (float)((int)(v.Y / (float)Game1.tileSize)));
							if (location.terrainFeatures.ContainsKey(vec) || location.objects.ContainsKey(vec))
							{
								bool result = false;
								return result;
							}
						}
						if (Game1.player.currentLocation.Equals(location))
						{
							DelayedAction.playSoundAfterDelay("dirtyHit", 450);
							DelayedAction.playSoundAfterDelay("dirtyHit", 900);
							DelayedAction.playSoundAfterDelay("dirtyHit", 1350);
						}
						if (location.Equals(Game1.currentLocation))
						{
							switch (base.FacingDirection)
							{
							case 0:
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(9, 250),
									new FarmerSprite.AnimationFrame(11, 250),
									new FarmerSprite.AnimationFrame(9, 250),
									new FarmerSprite.AnimationFrame(11, 250),
									new FarmerSprite.AnimationFrame(9, 250),
									new FarmerSprite.AnimationFrame(11, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
								});
								break;
							case 1:
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(5, 250),
									new FarmerSprite.AnimationFrame(7, 250),
									new FarmerSprite.AnimationFrame(5, 250),
									new FarmerSprite.AnimationFrame(7, 250),
									new FarmerSprite.AnimationFrame(5, 250),
									new FarmerSprite.AnimationFrame(7, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
								});
								break;
							case 2:
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(1, 250),
									new FarmerSprite.AnimationFrame(3, 250),
									new FarmerSprite.AnimationFrame(1, 250),
									new FarmerSprite.AnimationFrame(3, 250),
									new FarmerSprite.AnimationFrame(1, 250),
									new FarmerSprite.AnimationFrame(3, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
								});
								break;
							case 3:
								this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
								{
									new FarmerSprite.AnimationFrame(5, 250, false, true, null, false),
									new FarmerSprite.AnimationFrame(7, 250, false, true, null, false),
									new FarmerSprite.AnimationFrame(5, 250, false, true, null, false),
									new FarmerSprite.AnimationFrame(7, 250, false, true, null, false),
									new FarmerSprite.AnimationFrame(5, 250, false, true, null, false),
									new FarmerSprite.AnimationFrame(7, 250, false, true, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
								});
								break;
							}
							this.sprite.loop = false;
							return false;
						}
						this.findTruffle(Game1.player);
						return false;
					}
				}
				return false;
			}
			if (this.home != null && this.home.getRectForAnimalDoor().Intersects(this.GetBoundingBox()))
			{
				FarmAnimal.behaviorAfterFindingGrassPatch(this, location);
				this.isEating = false;
				this.Halt();
				return false;
			}
			if (this.buildingTypeILiveIn.Contains("Barn"))
			{
				this.sprite.Animate(time, 16, 4, 100f);
				if (this.sprite.CurrentFrame >= 20)
				{
					this.isEating = false;
					this.sprite.loop = true;
					this.sprite.CurrentFrame = 0;
					this.faceDirection(2);
				}
			}
			else
			{
				this.sprite.Animate(time, 24, 4, 100f);
				if (this.sprite.CurrentFrame >= 28)
				{
					this.isEating = false;
					this.sprite.loop = true;
					this.sprite.CurrentFrame = 0;
					this.faceDirection(2);
				}
			}
			return true;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00031DC8 File Offset: 0x0002FFC8
		private void findTruffle(Farmer who)
		{
			Utility.spawnObjectAround(Utility.getTranslatedVector2(base.getTileLocation(), base.FacingDirection, 1f), new Object(base.getTileLocation(), 430, 1), Game1.getFarm());
			if (new Random((int)this.myID / 2 + (int)Game1.stats.DaysPlayed + Game1.timeOfDay).NextDouble() > (double)this.friendshipTowardFarmer / 1500.0)
			{
				this.currentProduce = -1;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00031E44 File Offset: 0x00030044
		public void hitWithWeapon(MeleeWeapon t)
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00031E54 File Offset: 0x00030054
		public void makeSound()
		{
			if (this.sound != null && Game1.soundBank != null)
			{
				Cue expr_1F = Game1.soundBank.GetCue(this.sound);
				expr_1F.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
				expr_1F.Play();
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00031EAC File Offset: 0x000300AC
		public virtual bool updateWhenCurrentLocation(GameTime time, GameLocation location)
		{
			if (!Game1.shouldTimePass())
			{
				return false;
			}
			if (this.health <= 0)
			{
				return true;
			}
			if (this.hitGlowTimer > 0)
			{
				this.hitGlowTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.sprite.currentAnimation != null)
			{
				if (this.sprite.animateOnce(time))
				{
					this.sprite.currentAnimation = null;
				}
				return false;
			}
			this.update(time, location, this.myID, false);
			if (this.behaviors(time, location))
			{
				return false;
			}
			if (this.sprite.currentAnimation != null)
			{
				return false;
			}
			if (this.controller != null && this.controller.timerSinceLastCheckPoint > 10000)
			{
				this.controller = null;
				this.Halt();
			}
			if (location.GetType() == typeof(Farm) && this.noWarpTimer <= 0 && this.home != null && this.home.getRectForAnimalDoor().Contains(this.GetBoundingBox().Center.X, this.GetBoundingBox().Top))
			{
				((AnimalHouse)this.home.indoors).animals.Add(this.myID, this);
				this.setRandomPosition(this.home.indoors);
				this.faceDirection(Game1.random.Next(4));
				this.controller = null;
				if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 3, location))
				{
					Game1.playSound("dwoop");
				}
				return true;
			}
			this.noWarpTimer = Math.Max(0, this.noWarpTimer - time.ElapsedGameTime.Milliseconds);
			if (this.pauseTimer > 0)
			{
				this.pauseTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (Game1.timeOfDay >= 2000)
			{
				this.sprite.currentFrame = (this.buildingTypeILiveIn.Contains("Coop") ? 16 : 12);
				this.sprite.UpdateSourceRect();
				if (!this.isEmoting && Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
				}
			}
			else if (this.pauseTimer <= 0)
			{
				if (Game1.random.NextDouble() < 0.001 && this.age >= (int)this.ageWhenMature && Game1.gameMode == 3 && this.sound != null && Utility.isOnScreen(this.position, Game1.tileSize * 3) && Game1.soundBank != null)
				{
					Cue expr_29E = Game1.soundBank.GetCue(this.sound);
					expr_29E.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
					expr_29E.Play();
				}
				if (!Game1.IsClient && Game1.random.NextDouble() < 0.007 && this.uniqueFrameAccumulator == -1)
				{
					int newDirection = Game1.random.Next(5);
					if (newDirection != (this.facingDirection + 2) % 4)
					{
						if (newDirection < 4)
						{
							int oldDirection = this.facingDirection;
							this.faceDirection(newDirection);
							if (!location.isOutdoors && location.isCollidingPosition(this.nextPosition(newDirection), Game1.viewport, this))
							{
								this.faceDirection(oldDirection);
								return false;
							}
						}
						switch (newDirection)
						{
						case 0:
							this.SetMovingUp(true);
							break;
						case 1:
							this.SetMovingRight(true);
							break;
						case 2:
							this.SetMovingDown(true);
							break;
						case 3:
							this.SetMovingLeft(true);
							break;
						default:
							this.Halt();
							this.sprite.StopAnimation();
							break;
						}
					}
					else if (this.noWarpTimer <= 0)
					{
						this.Halt();
						this.sprite.StopAnimation();
					}
				}
				if ((!Game1.IsClient && this.isMoving() && Game1.random.NextDouble() < 0.014 && this.uniqueFrameAccumulator == -1) || (Game1.IsClient && Game1.random.NextDouble() < 0.014 && base.distanceFromLastServerPosition() <= 4f))
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.75)
					{
						this.uniqueFrameAccumulator = 0;
						if (this.buildingTypeILiveIn.Contains("Coop"))
						{
							switch (this.facingDirection)
							{
							case 0:
								this.sprite.currentFrame = 20;
								break;
							case 1:
								this.sprite.currentFrame = 18;
								break;
							case 2:
								this.sprite.currentFrame = 16;
								break;
							case 3:
								this.sprite.currentFrame = 22;
								break;
							}
						}
						else if (this.buildingTypeILiveIn.Contains("Barn"))
						{
							switch (this.facingDirection)
							{
							case 0:
								this.sprite.currentFrame = 15;
								break;
							case 1:
								this.sprite.currentFrame = 14;
								break;
							case 2:
								this.sprite.currentFrame = 13;
								break;
							case 3:
								this.sprite.currentFrame = 14;
								break;
							}
						}
					}
					this.sprite.UpdateSourceRect();
				}
				if (this.uniqueFrameAccumulator != -1)
				{
					this.uniqueFrameAccumulator += time.ElapsedGameTime.Milliseconds;
					if (this.uniqueFrameAccumulator > 500)
					{
						if (this.buildingTypeILiveIn.Contains("Coop"))
						{
							this.sprite.CurrentFrame = this.sprite.CurrentFrame + 1 - this.sprite.CurrentFrame % 2 * 2;
						}
						else if (this.sprite.CurrentFrame > 12)
						{
							this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 13) * 4;
						}
						else
						{
							switch (this.facingDirection)
							{
							case 0:
								this.sprite.CurrentFrame = 15;
								break;
							case 1:
								this.sprite.CurrentFrame = 14;
								break;
							case 2:
								this.sprite.CurrentFrame = 13;
								break;
							case 3:
								this.sprite.CurrentFrame = 14;
								break;
							}
						}
						this.uniqueFrameAccumulator = 0;
						if (Game1.random.NextDouble() < 0.4)
						{
							this.uniqueFrameAccumulator = -1;
						}
					}
				}
				else if (!Game1.IsClient)
				{
					this.MovePosition(time, Game1.viewport, location);
				}
			}
			if (!this.isMoving() && location is Farm && this.controller == null)
			{
				this.Halt();
				Microsoft.Xna.Framework.Rectangle r = this.GetBoundingBox();
				foreach (KeyValuePair<long, FarmAnimal> v in (location as Farm).animals)
				{
					if (!v.Value.Equals(this) && v.Value.GetBoundingBox().Intersects(r))
					{
						int xOffset = r.Center.X - v.Value.GetBoundingBox().Center.X;
						int yOffset = r.Center.Y - v.Value.GetBoundingBox().Center.Y;
						if (xOffset > 0 && Math.Abs(xOffset) > Math.Abs(yOffset))
						{
							this.SetMovingUp(true);
						}
						else if (xOffset < 0 && Math.Abs(xOffset) > Math.Abs(yOffset))
						{
							this.SetMovingDown(true);
						}
						else if (yOffset > 0)
						{
							this.SetMovingLeft(true);
						}
						else
						{
							this.SetMovingRight(true);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00032670 File Offset: 0x00030870
		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (this.pauseTimer > 0)
			{
				return;
			}
			if (this.moveUp)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, 0, false, this, false, false, false))
				{
					this.position.Y = this.position.Y - (float)this.speed;
					this.sprite.AnimateUp(time, 0, "");
				}
				else
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.6)
					{
						this.SetMovingDown(true);
					}
				}
				this.faceDirection(0);
				return;
			}
			if (this.moveRight)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, 0, false, this))
				{
					this.position.X = this.position.X + (float)this.speed;
					this.sprite.AnimateRight(time, 0, "");
				}
				else
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.6)
					{
						this.SetMovingLeft(true);
					}
				}
				this.faceDirection(1);
				return;
			}
			if (this.moveDown)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, 0, false, this))
				{
					this.position.Y = this.position.Y + (float)this.speed;
					this.sprite.AnimateDown(time, 0, "");
				}
				else
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.6)
					{
						this.SetMovingUp(true);
					}
				}
				this.faceDirection(2);
				return;
			}
			if (this.moveLeft)
			{
				if (!currentLocation.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, 0, false, this))
				{
					this.position.X = this.position.X - (float)this.speed;
					this.sprite.AnimateRight(time, 0, "");
				}
				else
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.6)
					{
						this.SetMovingRight(true);
					}
				}
				this.facingDirection = 3;
				if (!this.isCoopDweller() && this.sprite.CurrentFrame > 7)
				{
					this.sprite.CurrentFrame = 4;
				}
			}
		}

		// Token: 0x04000232 RID: 562
		public const byte eatGrassBehavior = 0;

		// Token: 0x04000233 RID: 563
		public const short newHome = 0;

		// Token: 0x04000234 RID: 564
		public const short happy = 1;

		// Token: 0x04000235 RID: 565
		public const short neutral = 2;

		// Token: 0x04000236 RID: 566
		public const short unhappy = 3;

		// Token: 0x04000237 RID: 567
		public const short hungry = 4;

		// Token: 0x04000238 RID: 568
		public const short disturbedByDog = 5;

		// Token: 0x04000239 RID: 569
		public const short leftOutAtNight = 6;

		// Token: 0x0400023A RID: 570
		public const int hitsTillDead = 3;

		// Token: 0x0400023B RID: 571
		public const double chancePerUpdateToChangeDirection = 0.007;

		// Token: 0x0400023C RID: 572
		public const byte fullnessValueOfGrass = 60;

		// Token: 0x0400023D RID: 573
		public const int noWarpTimerTime = 3000;

		// Token: 0x0400023E RID: 574
		public new const double chanceForSound = 0.002;

		// Token: 0x0400023F RID: 575
		public const double chanceToGoOutside = 0.002;

		// Token: 0x04000240 RID: 576
		public const int uniqueDownFrame = 16;

		// Token: 0x04000241 RID: 577
		public const int uniqueRightFrame = 18;

		// Token: 0x04000242 RID: 578
		public const int uniqueUpFrame = 20;

		// Token: 0x04000243 RID: 579
		public const int uniqueLeftFrame = 22;

		// Token: 0x04000244 RID: 580
		public const int pushAccumulatorTimeTillPush = 40;

		// Token: 0x04000245 RID: 581
		public const int timePerUniqueFrame = 500;

		// Token: 0x04000246 RID: 582
		public const byte layHarvestType = 0;

		// Token: 0x04000247 RID: 583
		public const byte grabHarvestType = 1;

		// Token: 0x04000248 RID: 584
		public int defaultProduceIndex;

		// Token: 0x04000249 RID: 585
		public int deluxeProduceIndex;

		// Token: 0x0400024A RID: 586
		public int currentProduce;

		// Token: 0x0400024B RID: 587
		public int friendshipTowardFarmer;

		// Token: 0x0400024C RID: 588
		public int daysSinceLastFed;

		// Token: 0x0400024D RID: 589
		public int pushAccumulator;

		// Token: 0x0400024E RID: 590
		public int uniqueFrameAccumulator = -1;

		// Token: 0x0400024F RID: 591
		public int age;

		// Token: 0x04000250 RID: 592
		public int meatIndex;

		// Token: 0x04000251 RID: 593
		public int health;

		// Token: 0x04000252 RID: 594
		public int price;

		// Token: 0x04000253 RID: 595
		public int produceQuality;

		// Token: 0x04000254 RID: 596
		public byte daysToLay;

		// Token: 0x04000255 RID: 597
		public byte daysSinceLastLay;

		// Token: 0x04000256 RID: 598
		public byte ageWhenMature;

		// Token: 0x04000257 RID: 599
		public byte harvestType;

		// Token: 0x04000258 RID: 600
		public byte happiness;

		// Token: 0x04000259 RID: 601
		public byte fullness;

		// Token: 0x0400025A RID: 602
		public byte happinessDrain;

		// Token: 0x0400025B RID: 603
		public byte fullnessDrain;

		// Token: 0x0400025C RID: 604
		public bool wasPet;

		// Token: 0x0400025D RID: 605
		public bool showDifferentTextureWhenReadyForHarvest;

		// Token: 0x0400025E RID: 606
		public bool allowReproduction = true;

		// Token: 0x0400025F RID: 607
		public string sound;

		// Token: 0x04000260 RID: 608
		public string type;

		// Token: 0x04000261 RID: 609
		public string buildingTypeILiveIn;

		// Token: 0x04000262 RID: 610
		public string toolUsedForHarvest;

		// Token: 0x04000263 RID: 611
		public Microsoft.Xna.Framework.Rectangle frontBackBoundingBox;

		// Token: 0x04000264 RID: 612
		public Microsoft.Xna.Framework.Rectangle sidewaysBoundingBox;

		// Token: 0x04000265 RID: 613
		public Microsoft.Xna.Framework.Rectangle frontBackSourceRect;

		// Token: 0x04000266 RID: 614
		public Microsoft.Xna.Framework.Rectangle sidewaysSourceRect;

		// Token: 0x04000267 RID: 615
		public long myID;

		// Token: 0x04000268 RID: 616
		public long ownerID;

		// Token: 0x04000269 RID: 617
		public long parentId = -1L;

		// Token: 0x0400026A RID: 618
		[XmlIgnore]
		public Building home;

		// Token: 0x0400026B RID: 619
		public Vector2 homeLocation;

		// Token: 0x0400026C RID: 620
		[XmlIgnore]
		public int noWarpTimer;

		// Token: 0x0400026D RID: 621
		[XmlIgnore]
		public int hitGlowTimer;

		// Token: 0x0400026E RID: 622
		[XmlIgnore]
		public int pauseTimer;

		// Token: 0x0400026F RID: 623
		public short moodMessage;

		// Token: 0x04000270 RID: 624
		private bool isEating;
	}
}
