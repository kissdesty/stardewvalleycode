using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Events;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000006 RID: 6
	public class AnimalHouse : GameLocation
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002836 File Offset: 0x00000A36
		public AnimalHouse()
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002866 File Offset: 0x00000A66
		public AnimalHouse(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002898 File Offset: 0x00000A98
		public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
		{
			if (!Game1.currentLocation.Equals(this))
			{
				try
				{
					foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
					{
						kvp.Value.updateWhenNotCurrentLocation(parentBuilding, time, this);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002914 File Offset: 0x00000B14
		public void incubator()
		{
			if (this.incubatingEgg.Y <= 0 && Game1.player.ActiveObject != null && Game1.player.ActiveObject.Category == -5)
			{
				this.incubatingEgg.X = 2;
				this.incubatingEgg.Y = Game1.player.ActiveObject.ParentSheetIndex;
				this.map.GetLayer("Front").Tiles[1, 2].TileIndex += ((Game1.player.ActiveObject.ParentSheetIndex == 180 || Game1.player.ActiveObject.ParentSheetIndex == 182) ? 2 : 1);
				Game1.throwActiveObjectDown();
				return;
			}
			if (Game1.player.ActiveObject == null && this.incubatingEgg.Y > 0)
			{
				base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_RemoveEgg_Question", new object[0]), base.createYesNoResponses(), "RemoveIncubatingEgg");
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A1B File Offset: 0x00000C1B
		public bool isFull()
		{
			return this.animalsThatLiveHere.Count >= this.animalLimit;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A34 File Offset: 0x00000C34
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (who.ActiveObject != null && who.ActiveObject.Name.Equals("Hay") && base.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Trough", "Back") != null && !this.objects.ContainsKey(new Vector2((float)tileLocation.X, (float)tileLocation.Y)))
			{
				this.objects.Add(new Vector2((float)tileLocation.X, (float)tileLocation.Y), (Object)who.ActiveObject.getOne());
				who.reduceActiveItemByOne();
				Game1.playSound("coin");
				return false;
			}
			bool b = base.checkAction(tileLocation, viewport, who);
			if (!b)
			{
				foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
				{
					if (kvp.Value.GetBoundingBox().Intersects(tileRect) && !kvp.Value.wasPet)
					{
						kvp.Value.pet(who);
						bool result = true;
						return result;
					}
				}
				foreach (KeyValuePair<long, FarmAnimal> kvp2 in this.animals)
				{
					if (kvp2.Value.GetBoundingBox().Intersects(tileRect))
					{
						kvp2.Value.pet(who);
						bool result = true;
						return result;
					}
				}
				return b;
			}
			return b;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002BFC File Offset: 0x00000DFC
		public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
		{
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (kvp.Value.getTileLocation().Equals(tileLocation))
				{
					return true;
				}
			}
			return base.isTileOccupiedForPlacement(tileLocation, toPlace);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002C70 File Offset: 0x00000E70
		public override void resetForPlayerEntry()
		{
			this.resetPositionsOfAllAnimals();
			foreach (Object o in this.objects.Values)
			{
				if (o.bigCraftable && o.Name.Contains("Incubator") && o.heldObject != null && o.minutesUntilReady <= 0 && !this.isFull())
				{
					string whatHatched = "??";
					int parentSheetIndex = o.heldObject.ParentSheetIndex;
					if (parentSheetIndex <= 176)
					{
						if (parentSheetIndex == 107)
						{
							whatHatched = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_DinosaurEgg", new object[0]);
							goto IL_127;
						}
						if (parentSheetIndex != 174 && parentSheetIndex != 176)
						{
							goto IL_127;
						}
					}
					else if (parentSheetIndex <= 182)
					{
						if (parentSheetIndex != 180 && parentSheetIndex != 182)
						{
							goto IL_127;
						}
					}
					else
					{
						if (parentSheetIndex == 305)
						{
							whatHatched = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_VoidEgg", new object[0]);
							goto IL_127;
						}
						if (parentSheetIndex != 442)
						{
							goto IL_127;
						}
						whatHatched = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_DuckEgg", new object[0]);
						goto IL_127;
					}
					whatHatched = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_RegularEgg", new object[0]);
					IL_127:
					this.currentEvent = new Event("none/-1000 -1000/farmer 2 9 0/pause 250/message \"" + whatHatched + "\"/pause 500/animalNaming/pause 500/end", -1);
					break;
				}
			}
			base.resetForPlayerEntry();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002E00 File Offset: 0x00001000
		public Building getBuilding()
		{
			foreach (Building b in Game1.getFarm().buildings)
			{
				if (b.indoors != null && b.indoors.Equals(this))
				{
					return b;
				}
			}
			return null;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002E70 File Offset: 0x00001070
		public void addNewHatchedAnimal(string name)
		{
			if (this.getBuilding() is Coop)
			{
				using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator = this.objects.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Object o = enumerator.Current;
						if (o.bigCraftable && o.Name.Contains("Incubator") && o.heldObject != null && o.minutesUntilReady <= 0 && !this.isFull())
						{
							string animalName = "??";
							if (o.heldObject == null)
							{
								animalName = "White Chicken";
							}
							else
							{
								int num = o.heldObject.ParentSheetIndex;
								if (num <= 176)
								{
									if (num != 107)
									{
										if (num == 174 || num == 176)
										{
											animalName = "White Chicken";
										}
									}
									else
									{
										animalName = "Dinosaur";
									}
								}
								else if (num <= 182)
								{
									if (num == 180 || num == 182)
									{
										animalName = "Brown Chicken";
									}
								}
								else if (num != 305)
								{
									if (num == 442)
									{
										animalName = "Duck";
									}
								}
								else
								{
									animalName = "Void Chicken";
								}
							}
							FarmAnimal a = new FarmAnimal(animalName, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
							a.name = name;
							Building newAnimalHome = this.getBuilding();
							a.home = newAnimalHome;
							a.homeLocation = new Vector2((float)newAnimalHome.tileX, (float)newAnimalHome.tileY);
							a.setRandomPosition(a.home.indoors);
							(newAnimalHome.indoors as AnimalHouse).animals.Add(a.myID, a);
							(newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(a.myID);
							o.heldObject = null;
							o.ParentSheetIndex = 101;
							break;
						}
					}
					goto IL_2C7;
				}
			}
			if (Game1.farmEvent != null && Game1.farmEvent is QuestionEvent)
			{
				FarmAnimal a2 = new FarmAnimal((Game1.farmEvent as QuestionEvent).animal.type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
				a2.name = name;
				a2.parentId = (Game1.farmEvent as QuestionEvent).animal.myID;
				Building newAnimalHome2 = this.getBuilding();
				a2.home = newAnimalHome2;
				a2.homeLocation = new Vector2((float)newAnimalHome2.tileX, (float)newAnimalHome2.tileY);
				(Game1.farmEvent as QuestionEvent).forceProceed = true;
				a2.setRandomPosition(a2.home.indoors);
				(newAnimalHome2.indoors as AnimalHouse).animals.Add(a2.myID, a2);
				(newAnimalHome2.indoors as AnimalHouse).animalsThatLiveHere.Add(a2.myID);
			}
			IL_2C7:
			if (Game1.currentLocation.currentEvent != null)
			{
				Event expr_2DD = Game1.currentLocation.currentEvent;
				int num = expr_2DD.CurrentCommand;
				expr_2DD.CurrentCommand = num + 1;
			}
			Game1.exitActiveMenu();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000318C File Offset: 0x0000138C
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (character != null && !character.Equals(kvp.Value) && position.Intersects(kvp.Value.GetBoundingBox()))
				{
					kvp.Value.farmerPushing();
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, false, false);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003228 File Offset: 0x00001428
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				if (kvp.Value.updateWhenCurrentLocation(time, this))
				{
					this.animalsToRemove.Add(kvp.Key);
				}
			}
			for (int i = 0; i < this.animalsToRemove.Count; i++)
			{
				this.animals.Remove(this.animalsToRemove[i]);
			}
			this.animalsToRemove.Clear();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000032D8 File Offset: 0x000014D8
		public void resetPositionsOfAllAnimals()
		{
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				kvp.Value.setRandomPosition(this);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003334 File Offset: 0x00001534
		public override bool dropObject(Object obj, Vector2 location, xTile.Dimensions.Rectangle viewport, bool initialPlacement, Farmer who = null)
		{
			Vector2 tileLocation = new Vector2((float)((int)(location.X / (float)Game1.tileSize)), (float)((int)(location.Y / (float)Game1.tileSize)));
			if (!obj.Name.Equals("Hay") || base.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Trough", "Back") == null)
			{
				return base.dropObject(obj, location, viewport, initialPlacement, null);
			}
			if (!this.objects.ContainsKey(tileLocation))
			{
				this.objects.Add(tileLocation, obj);
				return true;
			}
			return false;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000033C4 File Offset: 0x000015C4
		public void feedAllAnimals()
		{
			int fed = 0;
			for (int x = 0; x < this.map.Layers[0].LayerWidth; x++)
			{
				for (int y = 0; y < this.map.Layers[0].LayerHeight; y++)
				{
					if (base.doesTileHaveProperty(x, y, "Trough", "Back") != null)
					{
						Vector2 tileLocation = new Vector2((float)x, (float)y);
						if (!this.objects.ContainsKey(tileLocation) && Game1.getFarm().piecesOfHay > 0)
						{
							this.objects.Add(tileLocation, new Object(178, 1, false, -1, 0));
							fed++;
							Game1.getFarm().piecesOfHay--;
						}
						if (fed >= this.animalLimit)
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003494 File Offset: 0x00001694
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				kvp.Value.dayUpdate(this);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000034F4 File Offset: 0x000016F4
		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (t is MeleeWeapon)
			{
				foreach (FarmAnimal a in this.animals.Values)
				{
					if (a.GetBoundingBox().Intersects((t as MeleeWeapon).mostRecentArea))
					{
						a.hitWithWeapon(t as MeleeWeapon);
					}
				}
			}
			return false;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003578 File Offset: 0x00001778
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.animals)
			{
				kvp.Value.draw(b);
			}
		}

		// Token: 0x04000014 RID: 20
		public SerializableDictionary<long, FarmAnimal> animals = new SerializableDictionary<long, FarmAnimal>();

		// Token: 0x04000015 RID: 21
		public int animalLimit = 4;

		// Token: 0x04000016 RID: 22
		public List<long> animalsThatLiveHere = new List<long>();

		// Token: 0x04000017 RID: 23
		public Point incubatingEgg;

		// Token: 0x04000018 RID: 24
		private List<long> animalsToRemove = new List<long>();
	}
}
