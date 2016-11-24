using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
	// Token: 0x0200012B RID: 299
	public class FarmHouse : DecoratableLocation
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x0015FC32 File Offset: 0x0015DE32
		public FarmHouse()
		{
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0015FC46 File Offset: 0x0015DE46
		public FarmHouse(int ownerNumber = 1)
		{
			this.farmerNumberOfOwner = ownerNumber;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0015FC64 File Offset: 0x0015DE64
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			foreach (Furniture f in this.furniture)
			{
				if (f.furniture_type != 12 && f.getBoundingBox(f.tileLocation).Intersects(position))
				{
					return true;
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, false, false);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0015FCEC File Offset: 0x0015DEEC
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			if (character == null || character.willDestroyObjectsUnderfoot)
			{
				foreach (Furniture f in this.furniture)
				{
					if (f.furniture_type != 12 && f.getBoundingBox(f.tileLocation).Intersects(position))
					{
						return true;
					}
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0015FD7C File Offset: 0x0015DF7C
		public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
		{
			Vector2 nonTile = v * (float)Game1.tileSize;
			nonTile.X += (float)(Game1.tileSize / 2);
			nonTile.Y += (float)(Game1.tileSize / 2);
			foreach (Furniture f in this.furniture)
			{
				if (f.furniture_type != 12 && f.getBoundingBox(f.tileLocation).Contains((int)nonTile.X, (int)nonTile.Y))
				{
					return false;
				}
			}
			return base.isTileLocationTotallyClearAndPlaceable(v);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0015FE38 File Offset: 0x0015E038
		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			foreach (NPC c in this.characters)
			{
				if (c.isMarried())
				{
					c.checkForMarriageDialogue(timeOfDay, this);
					if (Game1.timeOfDay == 2200)
					{
						c.controller = null;
						c.controller = new PathFindController(c, this, this.getSpouseBedSpot(), 0);
						if (c.controller.pathToEndPoint == null || !base.isTileOnMap(c.controller.pathToEndPoint.Last<Point>().X, c.controller.pathToEndPoint.Last<Point>().Y))
						{
							c.controller = null;
						}
					}
				}
				if (c is Child)
				{
					(c as Child).tenMinuteUpdate();
				}
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0015FF24 File Offset: 0x0015E124
		public Point getPorchStandingSpot()
		{
			int num = this.farmerNumberOfOwner;
			if (num == 0 || num == 1)
			{
				return new Point(66, 15);
			}
			return new Point(-1000, -1000);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0015FF58 File Offset: 0x0015E158
		public Point getKitchenStandingSpot()
		{
			switch (this.upgradeLevel)
			{
			case 1:
				return new Point(4, 5);
			case 2:
			case 3:
				return new Point(7, 14);
			default:
				return new Point(-1000, -1000);
			}
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0015FFA4 File Offset: 0x0015E1A4
		public Point getSpouseBedSpot()
		{
			switch (this.upgradeLevel)
			{
			case 1:
				return new Point(23, 4);
			case 2:
			case 3:
				return new Point(29, 13);
			default:
				return new Point(-1000, -1000);
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0015FFF0 File Offset: 0x0015E1F0
		public Point getBedSpot()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return new Point(10, 9);
			case 1:
				return new Point(22, 4);
			case 2:
			case 3:
				return new Point(28, 13);
			default:
				return new Point(-1000, -1000);
			}
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00160048 File Offset: 0x0015E248
		public Point getEntryLocation()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return new Point(3, 11);
			case 1:
				return new Point(9, 11);
			case 2:
			case 3:
				return new Point(12, 20);
			default:
				return new Point(-1000, -1000);
			}
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x001600A0 File Offset: 0x0015E2A0
		public Point getChildBed(int gender)
		{
			if (gender == 0)
			{
				return new Point(23, 5);
			}
			if (gender == 1)
			{
				return new Point(27, 5);
			}
			return Point.Zero;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x001600C0 File Offset: 0x0015E2C0
		public Point getRandomOpenPointInHouse(Random r, int buffer = 0, int tries = 30)
		{
			Point point = Point.Zero;
			for (int numTries = 0; numTries < tries; numTries++)
			{
				point = new Point(r.Next(this.map.Layers[0].LayerWidth), r.Next(this.map.Layers[0].LayerHeight));
				Microsoft.Xna.Framework.Rectangle zone = new Microsoft.Xna.Framework.Rectangle(point.X - buffer, point.Y - buffer, 1 + buffer * 2, 1 + buffer * 2);
				bool obstacleFound = false;
				for (int x = zone.X; x < zone.Right; x++)
				{
					for (int y = zone.Y; y < zone.Bottom; y++)
					{
						obstacleFound = (base.getTileIndexAt(x, y, "Back") == -1 || !base.isTileLocationTotallyClearAndPlaceable(x, y) || Utility.pointInRectangles(FarmHouse.getWalls(this.upgradeLevel), x, y));
						if (obstacleFound)
						{
							break;
						}
					}
					if (obstacleFound)
					{
						break;
					}
				}
				if (!obstacleFound)
				{
					return point;
				}
			}
			return Point.Zero;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x001601C4 File Offset: 0x0015E3C4
		public void setSpouseInKitchen()
		{
			Farmer expr_0B = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner);
			NPC spouse = Game1.getCharacterFromName(expr_0B.spouse, false);
			if (expr_0B != null && spouse != null)
			{
				Game1.warpCharacter(spouse, this.name, this.getKitchenStandingSpot(), false, false);
				spouse.spouseObstacleCheck(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_KitchenBlocked", new object[0]), this, false);
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00160220 File Offset: 0x0015E420
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex == 173)
				{
					this.fridge.fridge = true;
					this.fridge.checkForAction(who, false);
					return true;
				}
				switch (tileIndex)
				{
				case 794:
				case 795:
				case 796:
				case 797:
					this.fireplaceOn = !this.fireplaceOn;
					base.setFireplace(this.fireplaceOn, tileLocation.X - ((tileIndex == 795 || tileIndex == 797) ? 1 : 0), tileLocation.Y, true);
					return true;
				default:
					if (tileIndex == 2173)
					{
						if (Game1.player.eventsSeen.Contains(463391) && Game1.player.spouse != null && Game1.player.spouse.Equals("Emily"))
						{
							TemporaryAnimatedSprite t = base.getTemporarySpriteByID(5858585);
							if (t != null && t is EmilysParrot)
							{
								(t as EmilysParrot).doAction();
							}
						}
						return true;
					}
					break;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00160364 File Offset: 0x0015E564
		public FarmHouse(Map m, string name) : base(m, name)
		{
			switch (Game1.whichFarm)
			{
			case 0:
				this.furniture.Add(new Furniture(1120, new Vector2(5f, 4f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1364, new Vector2(5f, 4f));
				this.furniture.Add(new Furniture(1376, new Vector2(1f, 10f)));
				this.furniture.Add(new Furniture(0, new Vector2(4f, 4f)));
				this.furniture.Add(new TV(1466, new Vector2(1f, 4f)));
				this.furniture.Add(new Furniture(1614, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1618, new Vector2(6f, 8f)));
				this.furniture.Add(new Furniture(1602, new Vector2(5f, 1f)));
				this.objects.Add(new Vector2(3f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15, false, -1, 0)
				}, new Vector2(3f, 7f), true));
				return;
			case 1:
				this.setWallpaper(11, -1, true);
				this.setFloor(1, -1, true);
				this.furniture.Add(new Furniture(1122, new Vector2(1f, 6f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1367, new Vector2(1f, 6f));
				this.furniture.Add(new Furniture(3, new Vector2(1f, 5f)));
				this.furniture.Add(new TV(1680, new Vector2(5f, 4f)));
				this.furniture.Add(new Furniture(1673, new Vector2(1f, 1f)));
				this.furniture.Add(new Furniture(1673, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1676, new Vector2(5f, 1f)));
				this.furniture.Add(new Furniture(1737, new Vector2(6f, 8f)));
				this.furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
				this.furniture.Add(new Furniture(1675, new Vector2(10f, 1f)));
				this.objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15, false, -1, 0)
				}, new Vector2(4f, 7f), true));
				return;
			case 2:
				this.setWallpaper(92, -1, true);
				this.setFloor(34, -1, true);
				this.furniture.Add(new Furniture(1134, new Vector2(1f, 7f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1748, new Vector2(1f, 7f));
				this.furniture.Add(new Furniture(3, new Vector2(1f, 6f)));
				this.furniture.Add(new TV(1680, new Vector2(6f, 4f)));
				this.furniture.Add(new Furniture(1296, new Vector2(1f, 4f)));
				this.furniture.Add(new Furniture(1682, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1777, new Vector2(6f, 5f)));
				this.furniture.Add(new Furniture(1745, new Vector2(6f, 1f)));
				this.furniture.Add(new Furniture(1747, new Vector2(5f, 4f)));
				this.furniture.Add(new Furniture(1296, new Vector2(10f, 4f)));
				this.objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15, false, -1, 0)
				}, new Vector2(4f, 7f), true));
				return;
			case 3:
				this.setWallpaper(12, -1, true);
				this.setFloor(18, -1, true);
				this.furniture.Add(new Furniture(1218, new Vector2(1f, 6f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1368, new Vector2(1f, 6f));
				this.furniture.Add(new Furniture(1755, new Vector2(1f, 5f)));
				this.furniture.Add(new Furniture(1755, new Vector2(3f, 6f), 1));
				this.furniture.Add(new TV(1680, new Vector2(5f, 4f)));
				this.furniture.Add(new Furniture(1751, new Vector2(5f, 10f)));
				this.furniture.Add(new Furniture(1749, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1753, new Vector2(5f, 1f)));
				this.furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
				this.objects.Add(new Vector2(2f, 9f), new Chest(0, new List<Item>
				{
					new Object(472, 15, false, -1, 0)
				}, new Vector2(2f, 9f), true));
				return;
			case 4:
				this.setWallpaper(95, -1, true);
				this.setFloor(4, -1, true);
				this.furniture.Add(new TV(1680, new Vector2(1f, 4f)));
				this.furniture.Add(new Furniture(1628, new Vector2(1f, 5f)));
				this.furniture.Add(new Furniture(1393, new Vector2(3f, 4f)));
				this.furniture.Last<Furniture>().heldObject = new Furniture(1369, new Vector2(3f, 4f));
				this.furniture.Add(new Furniture(1678, new Vector2(10f, 1f)));
				this.furniture.Add(new Furniture(1812, new Vector2(3f, 1f)));
				this.furniture.Add(new Furniture(1630, new Vector2(1f, 1f)));
				this.furniture.Add(new Furniture(1811, new Vector2(6f, 1f)));
				this.furniture.Add(new Furniture(1389, new Vector2(10f, 4f)));
				this.objects.Add(new Vector2(4f, 7f), new Chest(0, new List<Item>
				{
					new Object(472, 15, false, -1, 0)
				}, new Vector2(4f, 7f), true));
				this.furniture.Add(new Furniture(1758, new Vector2(1f, 10f)));
				return;
			default:
				return;
			}
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00160C14 File Offset: 0x0015EE14
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			this.fridge.updateWhenCurrentLocation(time);
			if (Game1.player.isMarried())
			{
				NPC spouse = base.getCharacterFromName(Game1.player.spouse);
				if (spouse != null && Game1.timeOfDay < 1500 && Game1.random.NextDouble() < 0.0006 && spouse.controller == null && spouse.Schedule == null && !spouse.getTileLocation().Equals(Utility.PointToVector2(this.getSpouseBedSpot())) && this.furniture.Count > 0)
				{
					Microsoft.Xna.Framework.Rectangle b = this.furniture[Game1.random.Next(this.furniture.Count)].boundingBox;
					Vector2 possibleLocation = new Vector2((float)(b.X / Game1.tileSize), (float)(b.Y / Game1.tileSize));
					int tries = 0;
					int facingDirection = -3;
					while (tries < 3)
					{
						int xMove = Game1.random.Next(-1, 2);
						int yMove = Game1.random.Next(-1, 2);
						possibleLocation.X += (float)xMove;
						if (xMove == 0)
						{
							possibleLocation.Y += (float)yMove;
						}
						if (xMove == -1)
						{
							facingDirection = 1;
						}
						else if (xMove == 1)
						{
							facingDirection = 3;
						}
						else if (yMove == -1)
						{
							facingDirection = 2;
						}
						else if (yMove == 1)
						{
							facingDirection = 0;
						}
						if (this.isTileLocationTotallyClearAndPlaceable(possibleLocation))
						{
							break;
						}
						tries++;
					}
					if (tries < 3)
					{
						base.getCharacterFromName(Game1.player.spouse).controller = new PathFindController(base.getCharacterFromName(Game1.player.spouse), this, new Point((int)possibleLocation.X, (int)possibleLocation.Y), facingDirection);
					}
				}
				if (spouse != null && !spouse.isEmoting)
				{
					foreach (Vector2 v in base.getCharacterFromName(Game1.player.spouse).getAdjacentTiles())
					{
						NPC i = base.isCharacterAtTile(v);
						if (i != null && i.IsMonster && !i.name.Equals("Cat"))
						{
							spouse.faceGeneralDirection(v * new Vector2((float)Game1.tileSize, (float)Game1.tileSize), 0);
							Game1.showSwordswipeAnimation(spouse.facingDirection, spouse.position, 60f, false);
							Game1.playSound("swordswipe");
							spouse.shake(500);
							spouse.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:FarmHouse_SpouseAttacked" + (Game1.random.Next(12) + 1), new object[0]), -1, 2, 3000, 0);
							((Monster)i).takeDamage(50, (int)Utility.getAwayFromPositionTrajectory(i.GetBoundingBox(), spouse.position).X, (int)Utility.getAwayFromPositionTrajectory(i.GetBoundingBox(), spouse.position).Y, false, 1.0);
							if (((Monster)i).health <= 0)
							{
								this.debris.Add(new Debris(i.sprite.Texture, Game1.random.Next(6, 16), new Vector2((float)i.getStandingX(), (float)i.getStandingY())));
								this.monsterDrop((Monster)i, i.getStandingX(), i.getStandingY());
								this.characters.Remove(i);
								Stats expr_374 = Game1.stats;
								uint monstersKilled = expr_374.MonstersKilled;
								expr_374.MonstersKilled = monstersKilled + 1u;
								Game1.player.changeFriendship(-10, spouse);
							}
							else
							{
								((Monster)i).shedChunks(4);
							}
							spouse.CurrentDialogue.Clear();
							spouse.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_MonstersInHouse", new object[0]), spouse));
						}
					}
				}
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0016102C File Offset: 0x0015F22C
		public Point getFireplacePoint()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return new Point(8, 4);
			case 1:
				return new Point(26, 4);
			case 2:
			case 3:
				return new Point(2, 13);
			default:
				return new Point(-50, -50);
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0016107C File Offset: 0x0015F27C
		public bool shouldShowSpouseRoom()
		{
			bool showSpouse;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				showSpouse = Game1.player.isMarried();
			}
			else
			{
				showSpouse = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried();
			}
			return showSpouse;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x001610B8 File Offset: 0x0015F2B8
		public void showSpouseRoom()
		{
			int level = this.upgradeLevel;
			bool showSpouse;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				showSpouse = Game1.player.isMarried();
			}
			else
			{
				showSpouse = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried();
			}
			bool arg_9A_0 = this.displayingSpouseRoom;
			this.displayingSpouseRoom = showSpouse;
			this.map = Game1.content.Load<Map>("Maps\\FarmHouse" + ((level == 0) ? "" : ((level == 3) ? "2" : string.Concat(level))) + (showSpouse ? "_marriage" : ""));
			this.map.LoadTileSheets(Game1.mapDisplayDevice);
			if (arg_9A_0 && !this.displayingSpouseRoom)
			{
				Microsoft.Xna.Framework.Rectangle spouseRoomBounds = default(Microsoft.Xna.Framework.Rectangle);
				switch (this.upgradeLevel)
				{
				case 1:
					spouseRoomBounds = new Microsoft.Xna.Framework.Rectangle(29, 4, 6, 6);
					break;
				case 2:
				case 3:
					spouseRoomBounds = new Microsoft.Xna.Framework.Rectangle(35, 13, 6, 6);
					break;
				}
				for (int x = spouseRoomBounds.X; x <= spouseRoomBounds.Right; x++)
				{
					for (int y = spouseRoomBounds.Y; y <= spouseRoomBounds.Bottom; y++)
					{
						Vector2 tile = new Vector2((float)x, (float)y);
						for (int i = this.furniture.Count - 1; i >= 0; i--)
						{
							if (this.furniture[i].tileLocation.Equals(tile))
							{
								Game1.createItemDebris(this.furniture[i], new Vector2((float)spouseRoomBounds.X, (float)spouseRoomBounds.Center.Y) * (float)Game1.tileSize, 3, null);
								this.furniture.RemoveAt(i);
							}
						}
					}
				}
			}
			base.loadObjects();
			if (level == 3)
			{
				base.setMapTileIndex(3, 22, 162, "Front", 0);
				base.removeTile(4, 22, "Front");
				base.removeTile(5, 22, "Front");
				base.setMapTileIndex(6, 22, 163, "Front", 0);
				base.setMapTileIndex(3, 23, 64, "Buildings", 0);
				base.setMapTileIndex(3, 24, 96, "Buildings", 0);
				base.setMapTileIndex(4, 24, 165, "Front", 0);
				base.setMapTileIndex(5, 24, 165, "Front", 0);
				base.removeTile(4, 23, "Back");
				base.removeTile(5, 23, "Back");
				base.setMapTileIndex(4, 23, 1043, "Back", 0);
				base.setMapTileIndex(5, 23, 1043, "Back", 0);
				base.setMapTileIndex(4, 24, 1075, "Back", 0);
				base.setMapTileIndex(5, 24, 1075, "Back", 0);
				base.setMapTileIndex(6, 23, 68, "Buildings", 0);
				base.setMapTileIndex(6, 24, 130, "Buildings", 0);
				base.setMapTileIndex(4, 25, 0, "Front", 0);
				base.setMapTileIndex(5, 25, 0, "Front", 0);
				base.removeTile(4, 23, "Buildings");
				base.removeTile(5, 23, "Buildings");
				this.warps.Add(new Warp(4, 25, "Cellar", 3, 2, false));
				this.warps.Add(new Warp(5, 25, "Cellar", 4, 2, false));
				if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
				{
					Game1.player.craftingRecipes.Add("Cask", 0);
				}
			}
			if (showSpouse)
			{
				this.loadSpouseRoom();
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0016144C File Offset: 0x0015F64C
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (this.fireplaceOn)
			{
				Point firePlace = this.getFireplacePoint();
				base.setFireplace(true, firePlace.X, firePlace.Y, false);
			}
			if (Game1.player.isMarried() && Game1.player.spouse.Equals("Emily") && Game1.player.eventsSeen.Contains(463391))
			{
				Vector2 parrotSpot = new Vector2((float)(32 * Game1.tileSize + Game1.pixelZoom * 4), (float)(3 * Game1.tileSize - Game1.pixelZoom * 8));
				int num = this.upgradeLevel;
				if (num == 2 || num == 3)
				{
					parrotSpot = new Vector2((float)(38 * Game1.tileSize + Game1.pixelZoom * 4), (float)(12 * Game1.tileSize - Game1.pixelZoom * 8));
				}
				this.temporarySprites.Add(new EmilysParrot(parrotSpot));
			}
			if (this.currentlyDisplayedUpgradeLevel != this.upgradeLevel)
			{
				this.setMapForUpgradeLevel(this.upgradeLevel, false);
			}
			if ((!this.displayingSpouseRoom && this.shouldShowSpouseRoom()) || (this.displayingSpouseRoom && !this.shouldShowSpouseRoom()))
			{
				this.showSpouseRoom();
			}
			base.setWallpapers();
			base.setFloors();
			if (Game1.player.currentLocation == null || (!Game1.player.currentLocation.Equals(this) && !Game1.player.currentLocation.name.Equals("Cellar")))
			{
				switch (this.upgradeLevel)
				{
				case 1:
					Game1.player.position = new Vector2(9f, 11f) * (float)Game1.tileSize;
					break;
				case 2:
				case 3:
					Game1.player.position = new Vector2(12f, 20f) * (float)Game1.tileSize;
					break;
				}
				Game1.xLocationAfterWarp = Game1.player.getTileX();
				Game1.yLocationAfterWarp = Game1.player.getTileY();
				Game1.player.currentLocation = this;
			}
			if (Game1.timeOfDay >= 2200 && base.getCharacterFromName(Game1.player.spouse) != null && !Game1.player.spouse.Contains("engaged"))
			{
				NPC spouse = Game1.removeCharacterFromItsLocation(Game1.player.spouse);
				spouse.position = new Vector2((float)(this.getSpouseBedSpot().X * Game1.tileSize), (float)(this.getSpouseBedSpot().Y * Game1.tileSize));
				spouse.faceDirection(0);
				this.characters.Add(spouse);
			}
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i] is Pet && (!base.isTileOnMap(this.characters[i].getTileX(), this.characters[i].getTileY()) || base.getTileIndexAt(this.characters[i].getTileLocationPoint(), "Buildings") != -1 || base.getTileIndexAt(this.characters[i].getTileX() + 1, this.characters[i].getTileY(), "Buildings") != -1))
				{
					this.characters[i].faceDirection(2);
					Game1.warpCharacter(this.characters[i], "Farm", new Vector2(54f, 8f), false, false);
					break;
				}
			}
			Farm farm = Game1.getFarm();
			for (int j = this.characters.Count - 1; j >= 0; j--)
			{
				for (int k = j - 1; k >= 0; k--)
				{
					if (j < this.characters.Count && k < this.characters.Count && (this.characters[k].Equals(this.characters[j]) || (this.characters[k].name.Equals(this.characters[j].name) && this.characters[k].isVillager() && this.characters[j].isVillager())) && k != j)
					{
						this.characters.RemoveAt(k);
					}
				}
				for (int l = farm.characters.Count - 1; l >= 0; l--)
				{
					if (j < this.characters.Count && l < this.characters.Count && farm.characters[l].Equals(this.characters[j]))
					{
						farm.characters.RemoveAt(l);
					}
				}
			}
			if (Game1.timeOfDay >= 1800)
			{
				using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.isMarried();
					}
				}
			}
			foreach (NPC m in this.characters)
			{
				if (m is Child)
				{
					(m as Child).resetForPlayerEntry(this);
				}
				if (Game1.timeOfDay >= 2000)
				{
					m.controller = null;
					m.Halt();
				}
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x001619E0 File Offset: 0x0015FBE0
		public void moveObjectsForHouseUpgrade(int whichUpgrade)
		{
			switch (whichUpgrade)
			{
			case 0:
				if (this.upgradeLevel == 1)
				{
					this.shiftObjects(-6, 0);
					return;
				}
				break;
			case 1:
				if (this.upgradeLevel == 0)
				{
					this.shiftObjects(6, 0);
				}
				if (this.upgradeLevel == 2)
				{
					this.shiftObjects(-3, 0);
					return;
				}
				break;
			case 2:
			case 3:
				if (this.upgradeLevel == 1)
				{
					this.shiftObjects(3, 9);
					foreach (Furniture v in this.furniture)
					{
						if (v.tileLocation.X >= 10f && v.tileLocation.X <= 13f && v.tileLocation.Y >= 10f && v.tileLocation.Y <= 11f)
						{
							Furniture expr_D7_cp_0_cp_0 = v;
							expr_D7_cp_0_cp_0.tileLocation.X = expr_D7_cp_0_cp_0.tileLocation.X - 3f;
							Furniture expr_EB_cp_0_cp_0 = v;
							expr_EB_cp_0_cp_0.boundingBox.X = expr_EB_cp_0_cp_0.boundingBox.X - 3 * Game1.tileSize;
							Furniture expr_101_cp_0_cp_0 = v;
							expr_101_cp_0_cp_0.tileLocation.Y = expr_101_cp_0_cp_0.tileLocation.Y - 9f;
							Furniture expr_115_cp_0_cp_0 = v;
							expr_115_cp_0_cp_0.boundingBox.Y = expr_115_cp_0_cp_0.boundingBox.Y - 9 * Game1.tileSize;
							v.updateDrawPosition();
						}
					}
					base.moveFurniture(27, 13, 1, 4);
					base.moveFurniture(28, 13, 2, 4);
					base.moveFurniture(29, 13, 3, 4);
					base.moveFurniture(28, 14, 7, 4);
					base.moveFurniture(29, 14, 8, 4);
					base.moveFurniture(27, 14, 4, 4);
					base.moveFurniture(28, 15, 5, 4);
					base.moveFurniture(29, 16, 6, 4);
				}
				if (this.upgradeLevel == 0)
				{
					this.shiftObjects(9, 9);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00161BB4 File Offset: 0x0015FDB4
		public void setMapForUpgradeLevel(int level, bool persist = false)
		{
			if (persist)
			{
				this.upgradeLevel = level;
			}
			this.currentlyDisplayedUpgradeLevel = level;
			bool showSpouse;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				showSpouse = Game1.player.isMarried();
			}
			else
			{
				showSpouse = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried();
			}
			if (this.displayingSpouseRoom && !showSpouse)
			{
				this.displayingSpouseRoom = false;
			}
			this.map = Game1.content.Load<Map>("Maps\\FarmHouse" + ((level == 0) ? "" : ((level == 3) ? "2" : string.Concat(level))) + (showSpouse ? "_marriage" : ""));
			this.map.LoadTileSheets(Game1.mapDisplayDevice);
			if (showSpouse)
			{
				this.showSpouseRoom();
			}
			base.loadObjects();
			if (level == 3)
			{
				base.setMapTileIndex(3, 22, 162, "Front", 0);
				base.removeTile(4, 22, "Front");
				base.removeTile(5, 22, "Front");
				base.setMapTileIndex(6, 22, 163, "Front", 0);
				base.setMapTileIndex(3, 23, 64, "Buildings", 0);
				base.setMapTileIndex(3, 24, 96, "Buildings", 0);
				base.setMapTileIndex(4, 24, 165, "Front", 0);
				base.setMapTileIndex(5, 24, 165, "Front", 0);
				base.removeTile(4, 23, "Back");
				base.removeTile(5, 23, "Back");
				base.setMapTileIndex(4, 23, 1043, "Back", 0);
				base.setMapTileIndex(5, 23, 1043, "Back", 0);
				base.setTileProperty(4, 23, "Back", "NoFurniture", "t");
				base.setTileProperty(5, 23, "Back", "NoFurniture", "t");
				base.setTileProperty(4, 23, "Back", "NPCBarrier", "t");
				base.setTileProperty(5, 23, "Back", "NPCBarrier", "t");
				base.setMapTileIndex(4, 24, 1075, "Back", 0);
				base.setMapTileIndex(5, 24, 1075, "Back", 0);
				base.setTileProperty(4, 24, "Back", "NoFurniture", "t");
				base.setTileProperty(5, 24, "Back", "NoFurniture", "t");
				base.setMapTileIndex(6, 23, 68, "Buildings", 0);
				base.setMapTileIndex(6, 24, 130, "Buildings", 0);
				base.setMapTileIndex(4, 25, 0, "Front", 0);
				base.setMapTileIndex(5, 25, 0, "Front", 0);
				base.removeTile(4, 23, "Buildings");
				base.removeTile(5, 23, "Buildings");
				this.warps.Add(new Warp(4, 25, "Cellar", 3, 2, false));
				this.warps.Add(new Warp(5, 25, "Cellar", 4, 2, false));
				if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
				{
					Game1.player.craftingRecipes.Add("Cask", 0);
				}
			}
			if (this.wallPaper.Count > 0 && this.floor.Count > 0)
			{
				List<Microsoft.Xna.Framework.Rectangle> rooms = FarmHouse.getWalls(this.upgradeLevel);
				if (persist)
				{
					while (this.wallPaper.Count < rooms.Count)
					{
						this.wallPaper.Add(0);
					}
				}
				rooms = FarmHouse.getFloors(this.upgradeLevel);
				if (persist)
				{
					while (this.floor.Count < rooms.Count)
					{
						this.floor.Add(0);
					}
				}
				if (this.upgradeLevel == 1)
				{
					this.setFloor(this.floor[0], 1, true);
					this.setFloor(this.floor[0], 2, true);
					this.setFloor(this.floor[0], 3, true);
					this.setFloor(22, 0, true);
				}
				if (this.upgradeLevel == 2)
				{
					this.setWallpaper(this.wallPaper[0], 4, true);
					this.setWallpaper(this.wallPaper[2], 6, true);
					this.setWallpaper(this.wallPaper[1], 5, true);
					this.setWallpaper(11, 0, true);
					this.setWallpaper(61, 1, true);
					this.setWallpaper(61, 2, true);
					int bedroomFloor = this.floor[3];
					this.setFloor(this.floor[2], 5, true);
					this.setFloor(this.floor[0], 3, true);
					this.setFloor(this.floor[1], 4, true);
					this.setFloor(bedroomFloor, 6, true);
					this.setFloor(1, 0, true);
					this.setFloor(31, 1, true);
					this.setFloor(31, 2, true);
				}
			}
			this.lightGlows.Clear();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00162080 File Offset: 0x00160280
		public void loadSpouseRoom()
		{
			NPC spouse;
			if (Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) == null)
			{
				spouse = Game1.player.getSpouse();
			}
			else
			{
				spouse = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).getSpouse();
			}
			if (spouse != null)
			{
				int indexInSpouseMapSheet = -1;
				string name = spouse.name;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1866496948u)
				{
					if (num <= 1067922812u)
					{
						if (num != 161540545u)
						{
							if (num != 587846041u)
							{
								if (num == 1067922812u)
								{
									if (name == "Sam")
									{
										indexInSpouseMapSheet = 9;
									}
								}
							}
							else if (name == "Penny")
							{
								indexInSpouseMapSheet = 1;
							}
						}
						else if (name == "Sebastian")
						{
							indexInSpouseMapSheet = 5;
						}
					}
					else if (num != 1281010426u)
					{
						if (num != 1708213605u)
						{
							if (num == 1866496948u)
							{
								if (name == "Shane")
								{
									indexInSpouseMapSheet = 10;
								}
							}
						}
						else if (name == "Alex")
						{
							indexInSpouseMapSheet = 6;
						}
					}
					else if (name == "Maru")
					{
						indexInSpouseMapSheet = 4;
					}
				}
				else if (num <= 2571828641u)
				{
					if (num != 2010304804u)
					{
						if (num != 2434294092u)
						{
							if (num == 2571828641u)
							{
								if (name == "Emily")
								{
									indexInSpouseMapSheet = 11;
								}
							}
						}
						else if (name == "Haley")
						{
							indexInSpouseMapSheet = 3;
						}
					}
					else if (name == "Harvey")
					{
						indexInSpouseMapSheet = 7;
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
								indexInSpouseMapSheet = 8;
							}
						}
					}
					else if (name == "Leah")
					{
						indexInSpouseMapSheet = 2;
					}
				}
				else if (name == "Abigail")
				{
					indexInSpouseMapSheet = 0;
				}
				Microsoft.Xna.Framework.Rectangle areaToRefurbish = (this.upgradeLevel == 1) ? new Microsoft.Xna.Framework.Rectangle(29, 1, 6, 9) : new Microsoft.Xna.Framework.Rectangle(35, 10, 6, 9);
				Map refurbishedMap = Game1.content.Load<Map>("Maps\\spouseRooms");
				Point mapReader = new Point(indexInSpouseMapSheet % 5 * 6, indexInSpouseMapSheet / 5 * 9);
				this.map.Properties.Remove("DayTiles");
				this.map.Properties.Remove("NightTiles");
				for (int x = 0; x < areaToRefurbish.Width; x++)
				{
					for (int y = 0; y < areaToRefurbish.Height; y++)
					{
						if (refurbishedMap.GetLayer("Back").Tiles[mapReader.X + x, mapReader.Y + y] != null)
						{
							this.map.GetLayer("Back").Tiles[areaToRefurbish.X + x, areaToRefurbish.Y + y] = new StaticTile(this.map.GetLayer("Back"), this.map.TileSheets[0], BlendMode.Alpha, refurbishedMap.GetLayer("Back").Tiles[mapReader.X + x, mapReader.Y + y].TileIndex);
						}
						if (refurbishedMap.GetLayer("Buildings").Tiles[mapReader.X + x, mapReader.Y + y] != null)
						{
							this.map.GetLayer("Buildings").Tiles[areaToRefurbish.X + x, areaToRefurbish.Y + y] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, refurbishedMap.GetLayer("Buildings").Tiles[mapReader.X + x, mapReader.Y + y].TileIndex);
							base.adjustMapLightPropertiesForLamp(refurbishedMap.GetLayer("Buildings").Tiles[mapReader.X + x, mapReader.Y + y].TileIndex, areaToRefurbish.X + x, areaToRefurbish.Y + y, "Buildings");
						}
						else
						{
							this.map.GetLayer("Buildings").Tiles[areaToRefurbish.X + x, areaToRefurbish.Y + y] = null;
						}
						if (y < areaToRefurbish.Height - 1 && refurbishedMap.GetLayer("Front").Tiles[mapReader.X + x, mapReader.Y + y] != null)
						{
							this.map.GetLayer("Front").Tiles[areaToRefurbish.X + x, areaToRefurbish.Y + y] = new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, refurbishedMap.GetLayer("Front").Tiles[mapReader.X + x, mapReader.Y + y].TileIndex);
							base.adjustMapLightPropertiesForLamp(refurbishedMap.GetLayer("Front").Tiles[mapReader.X + x, mapReader.Y + y].TileIndex, areaToRefurbish.X + x, areaToRefurbish.Y + y, "Front");
						}
						else if (y < areaToRefurbish.Height - 1)
						{
							this.map.GetLayer("Front").Tiles[areaToRefurbish.X + x, areaToRefurbish.Y + y] = null;
						}
						if (x == 4 && y == 4)
						{
							this.map.GetLayer("Back").Tiles[areaToRefurbish.X + x, areaToRefurbish.Y + y].Properties.Add(new KeyValuePair<string, PropertyValue>("NoFurniture", new PropertyValue("T")));
						}
					}
				}
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x001626B3 File Offset: 0x001608B3
		public void playerDivorced()
		{
			this.displayingSpouseRoom = false;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x001626BC File Offset: 0x001608BC
		public new bool isTileOnWall(int x, int y)
		{
			foreach (Microsoft.Xna.Framework.Rectangle r in FarmHouse.getWalls(this.upgradeLevel))
			{
				if (r.Contains(x, y))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00162720 File Offset: 0x00160920
		public static List<Microsoft.Xna.Framework.Rectangle> getWalls(int upgradeLevel)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 10, 3));
				break;
			case 1:
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 17, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(18, 6, 2, 2));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(20, 1, 9, 3));
				break;
			case 2:
			case 3:
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 12, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(15, 1, 13, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(13, 3, 2, 2));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(1, 10, 10, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(13, 10, 8, 3));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(21, 15, 2, 2));
				walls.Add(new Microsoft.Xna.Framework.Rectangle(23, 10, 11, 3));
				break;
			}
			return walls;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00162810 File Offset: 0x00160A10
		public override void setWallpaper(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> rooms = FarmHouse.getWalls(this.upgradeLevel);
			if (persist)
			{
				while (this.wallPaper.Count < rooms.Count)
				{
					this.wallPaper.Add(0);
				}
				if (whichRoom == -1)
				{
					for (int i = 0; i < this.wallPaper.Count; i++)
					{
						this.wallPaper[i] = which;
					}
				}
				else if (whichRoom <= this.wallPaper.Count - 1)
				{
					this.wallPaper[whichRoom] = which;
				}
			}
			int tileSheetIndex = which % 16 + which / 16 * 48;
			if (whichRoom == -1)
			{
				using (List<Microsoft.Xna.Framework.Rectangle>.Enumerator enumerator = rooms.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Microsoft.Xna.Framework.Rectangle r = enumerator.Current;
						for (int x = r.X; x < r.Right; x++)
						{
							base.setMapTileIndex(x, r.Y, tileSheetIndex, "Back", 0);
							base.setMapTileIndex(x, r.Y + 1, tileSheetIndex + 16, "Back", 0);
							if (r.Height >= 3)
							{
								if (this.map.GetLayer("Buildings").Tiles[x, r.Y + 2].TileSheet.Equals(this.map.TileSheets[2]))
								{
									base.setMapTileIndex(x, r.Y + 2, tileSheetIndex + 32, "Buildings", 0);
								}
								else
								{
									base.setMapTileIndex(x, r.Y + 2, tileSheetIndex + 32, "Back", 0);
								}
							}
						}
					}
					return;
				}
			}
			Microsoft.Xna.Framework.Rectangle r2 = rooms[Math.Min(rooms.Count - 1, whichRoom)];
			for (int x2 = r2.X; x2 < r2.Right; x2++)
			{
				base.setMapTileIndex(x2, r2.Y, tileSheetIndex, "Back", 0);
				base.setMapTileIndex(x2, r2.Y + 1, tileSheetIndex + 16, "Back", 0);
				if (r2.Height >= 3)
				{
					if (this.map.GetLayer("Buildings").Tiles[x2, r2.Y + 2].TileSheet.Equals(this.map.TileSheets[2]))
					{
						base.setMapTileIndex(x2, r2.Y + 2, tileSheetIndex + 32, "Buildings", 0);
					}
					else
					{
						base.setMapTileIndex(x2, r2.Y + 2, tileSheetIndex + 32, "Back", 0);
					}
				}
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00162AB0 File Offset: 0x00160CB0
		public new int getFloorAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = FarmHouse.getFloors(this.upgradeLevel);
			for (int i = 0; i < floors.Count; i++)
			{
				if (floors[i].Contains(p))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00162AF0 File Offset: 0x00160CF0
		public new int getWallForRoomAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = FarmHouse.getWalls(this.upgradeLevel);
			for (int y = 0; y < 16; y++)
			{
				for (int i = 0; i < walls.Count; i++)
				{
					if (walls[i].Contains(p))
					{
						return i;
					}
				}
				p.Y--;
			}
			return -1;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00162B48 File Offset: 0x00160D48
		public override void setFloor(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> rooms = FarmHouse.getFloors(this.upgradeLevel);
			if (persist)
			{
				while (this.floor.Count < rooms.Count)
				{
					this.floor.Add(0);
				}
				if (whichRoom == -1)
				{
					for (int i = 0; i < this.floor.Count; i++)
					{
						this.floor[i] = which;
					}
				}
				else
				{
					if (whichRoom > this.floor.Count - 1)
					{
						return;
					}
					this.floor[whichRoom] = which;
				}
			}
			int tileSheetIndex = 336 + which % 8 * 2 + which / 8 * 32;
			if (whichRoom == -1)
			{
				using (List<Microsoft.Xna.Framework.Rectangle>.Enumerator enumerator = rooms.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Microsoft.Xna.Framework.Rectangle r = enumerator.Current;
						for (int x = r.X; x < r.Right; x += 2)
						{
							for (int y = r.Y; y < r.Bottom; y += 2)
							{
								if (r.Contains(x, y))
								{
									base.setMapTileIndex(x, y, tileSheetIndex, "Back", 0);
								}
								if (r.Contains(x + 1, y))
								{
									base.setMapTileIndex(x + 1, y, tileSheetIndex + 1, "Back", 0);
								}
								if (r.Contains(x, y + 1))
								{
									base.setMapTileIndex(x, y + 1, tileSheetIndex + 16, "Back", 0);
								}
								if (r.Contains(x + 1, y + 1))
								{
									base.setMapTileIndex(x + 1, y + 1, tileSheetIndex + 17, "Back", 0);
								}
							}
						}
					}
					return;
				}
			}
			Microsoft.Xna.Framework.Rectangle r2 = rooms[whichRoom];
			for (int x2 = r2.X; x2 < r2.Right; x2 += 2)
			{
				for (int y2 = r2.Y; y2 < r2.Bottom; y2 += 2)
				{
					if (r2.Contains(x2, y2))
					{
						base.setMapTileIndex(x2, y2, tileSheetIndex, "Back", 0);
					}
					if (r2.Contains(x2 + 1, y2))
					{
						base.setMapTileIndex(x2 + 1, y2, tileSheetIndex + 1, "Back", 0);
					}
					if (r2.Contains(x2, y2 + 1))
					{
						base.setMapTileIndex(x2, y2 + 1, tileSheetIndex + 16, "Back", 0);
					}
					if (r2.Contains(x2 + 1, y2 + 1))
					{
						base.setMapTileIndex(x2 + 1, y2 + 1, tileSheetIndex + 17, "Back", 0);
					}
				}
			}
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00162DE0 File Offset: 0x00160FE0
		public static List<Microsoft.Xna.Framework.Rectangle> getFloors(int upgradeLevel)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = new List<Microsoft.Xna.Framework.Rectangle>();
			switch (upgradeLevel)
			{
			case 0:
				floors.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 10, 9));
				break;
			case 1:
				floors.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 6, 9));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(7, 3, 11, 9));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(18, 8, 2, 2));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(20, 3, 9, 8));
				break;
			case 2:
			case 3:
				floors.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 12, 6));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(15, 3, 13, 6));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(13, 5, 2, 2));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(0, 12, 10, 11));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(10, 12, 11, 9));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(21, 17, 2, 2));
				floors.Add(new Microsoft.Xna.Framework.Rectangle(23, 12, 11, 11));
				break;
			}
			return floors;
		}

		// Token: 0x04001243 RID: 4675
		public int upgradeLevel;

		// Token: 0x04001244 RID: 4676
		public int farmerNumberOfOwner;

		// Token: 0x04001245 RID: 4677
		public bool fireplaceOn;

		// Token: 0x04001246 RID: 4678
		public Chest fridge = new Chest(true);

		// Token: 0x04001247 RID: 4679
		private int currentlyDisplayedUpgradeLevel;

		// Token: 0x04001248 RID: 4680
		private bool displayingSpouseRoom;
	}
}
