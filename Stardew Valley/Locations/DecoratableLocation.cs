using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x02000123 RID: 291
	public class DecoratableLocation : GameLocation
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x00153AAA File Offset: 0x00151CAA
		public DecoratableLocation()
		{
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00153AD3 File Offset: 0x00151CD3
		public DecoratableLocation(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00153B00 File Offset: 0x00151D00
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

		// Token: 0x06001081 RID: 4225 RVA: 0x00153B88 File Offset: 0x00151D88
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

		// Token: 0x06001082 RID: 4226 RVA: 0x00153C18 File Offset: 0x00151E18
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

		// Token: 0x06001083 RID: 4227 RVA: 0x00153CD4 File Offset: 0x00151ED4
		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.minutesElapsed(10, this);
				}
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00153D30 File Offset: 0x00151F30
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			foreach (Furniture expr_1C in this.furniture)
			{
				expr_1C.minutesElapsed(3000 - Game1.timeOfDay, this);
				expr_1C.DayUpdate(this);
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00153D9C File Offset: 0x00151F9C
		public override bool leftClick(int x, int y, Farmer who)
		{
			if (Game1.activeClickableMenu != null)
			{
				return false;
			}
			for (int i = this.furniture.Count - 1; i >= 0; i--)
			{
				if (this.furniture[i].boundingBox.Contains(x, y) && this.furniture[i].clicked(who))
				{
					if (this.furniture[i].flaggedForPickUp && who.couldInventoryAcceptThisItem(this.furniture[i]))
					{
						this.furniture[i].flaggedForPickUp = false;
						this.furniture[i].performRemoveAction(new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize)), this);
						bool foundInToolbar = false;
						for (int j = 0; j < 12; j++)
						{
							if (who.items[j] == null)
							{
								who.items[j] = this.furniture[i];
								who.CurrentToolIndex = j;
								foundInToolbar = true;
								break;
							}
						}
						if (!foundInToolbar)
						{
							Item item = who.addItemToInventory(this.furniture[i], 11);
							who.addItemToInventory(item);
							who.CurrentToolIndex = 11;
						}
						this.furniture.RemoveAt(i);
						Game1.playSound("coin");
					}
					return true;
				}
			}
			return base.leftClick(x, y, who);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00153EF4 File Offset: 0x001520F4
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (base.checkAction(tileLocation, viewport, who))
			{
				return true;
			}
			Point vect = new Point(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize);
			Point vectOnWall = new Point(tileLocation.X * Game1.tileSize, (tileLocation.Y - 1) * Game1.tileSize);
			foreach (Furniture f in this.furniture)
			{
				if (f.boundingBox.Contains(vect) && f.furniture_type != 12)
				{
					bool result;
					if (who.ActiveObject == null)
					{
						result = f.checkForAction(who, false);
						return result;
					}
					result = f.performObjectDropInAction(who.ActiveObject, false, who);
					return result;
				}
				else if (f.furniture_type == 6 && f.boundingBox.Contains(vectOnWall))
				{
					bool result;
					if (who.ActiveObject == null)
					{
						result = f.checkForAction(who, false);
						return result;
					}
					result = f.performObjectDropInAction(who.ActiveObject, false, who);
					return result;
				}
			}
			return false;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00154014 File Offset: 0x00152214
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00154026 File Offset: 0x00152226
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00154030 File Offset: 0x00152230
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (!Game1.player.mailReceived.Contains("button_tut_1"))
			{
				Game1.player.mailReceived.Add("button_tut_1");
				Game1.onScreenMenus.Add(new ButtonTutorialMenu(0));
			}
			if (!(this is FarmHouse))
			{
				this.setWallpapers();
				this.setFloors();
			}
			if (base.getTileIndexAt(Game1.player.getTileX(), Game1.player.getTileY(), "Buildings") != -1)
			{
				Farmer expr_85_cp_0_cp_0 = Game1.player;
				expr_85_cp_0_cp_0.position.Y = expr_85_cp_0_cp_0.position.Y + (float)Game1.tileSize;
			}
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.resetOnPlayerEntry(this);
				}
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00154110 File Offset: 0x00152310
		public override void shiftObjects(int dx, int dy)
		{
			base.shiftObjects(dx, dy);
			foreach (Furniture expr_1D in this.furniture)
			{
				expr_1D.tileLocation.X = expr_1D.tileLocation.X + (float)dx;
				expr_1D.tileLocation.Y = expr_1D.tileLocation.Y + (float)dy;
				expr_1D.boundingBox.X = expr_1D.boundingBox.X + dx * Game1.tileSize;
				expr_1D.boundingBox.Y = expr_1D.boundingBox.Y + dy * Game1.tileSize;
				expr_1D.updateDrawPosition();
			}
			SerializableDictionary<Vector2, TerrainFeature> shifted = new SerializableDictionary<Vector2, TerrainFeature>();
			foreach (Vector2 v in this.terrainFeatures.Keys)
			{
				shifted.Add(new Vector2(v.X + (float)dx, v.Y + (float)dy), this.terrainFeatures[v]);
			}
			this.terrainFeatures = shifted;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0015422C File Offset: 0x0015242C
		public override bool isObjectAt(int x, int y)
		{
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.boundingBox.Contains(x, y))
					{
						return true;
					}
				}
			}
			return base.isObjectAt(x, y);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00154294 File Offset: 0x00152494
		public override Object getObjectAt(int x, int y)
		{
			foreach (Furniture f in this.furniture)
			{
				if (f.boundingBox.Contains(x, y))
				{
					return f;
				}
			}
			return base.getObjectAt(x, y);
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00154300 File Offset: 0x00152500
		public void moveFurniture(int oldX, int oldY, int newX, int newY)
		{
			Vector2 oldSpot = new Vector2((float)oldX, (float)oldY);
			foreach (Furniture f in this.furniture)
			{
				if (f.tileLocation.Equals(oldSpot))
				{
					f.tileLocation = new Vector2((float)newX, (float)newY);
					f.boundingBox.X = newX * Game1.tileSize;
					f.boundingBox.Y = newY * Game1.tileSize;
					f.updateDrawPosition();
					return;
				}
			}
			if (this.objects.ContainsKey(oldSpot))
			{
				Object o = this.objects[oldSpot];
				this.objects.Remove(oldSpot);
				o.tileLocation = new Vector2((float)newX, (float)newY);
				this.objects.Add(new Vector2((float)newX, (float)newY), o);
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x001543F0 File Offset: 0x001525F0
		public bool isTileOnWall(int x, int y)
		{
			foreach (Microsoft.Xna.Framework.Rectangle r in DecoratableLocation.getWalls())
			{
				if (r.Contains(x, y))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00154450 File Offset: 0x00152650
		public static List<Microsoft.Xna.Framework.Rectangle> getWalls()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
			{
				new Microsoft.Xna.Framework.Rectangle(1, 1, 11, 3)
			};
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00154468 File Offset: 0x00152668
		public void setFloors()
		{
			for (int i = 0; i < this.floor.Count; i++)
			{
				this.setFloor(this.floor[i], i, true);
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x001544A0 File Offset: 0x001526A0
		public void setWallpapers()
		{
			for (int i = 0; i < this.wallPaper.Count; i++)
			{
				this.setWallpaper(this.wallPaper[i], i, true);
			}
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x001544D8 File Offset: 0x001526D8
		public virtual void setWallpaper(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> rooms = DecoratableLocation.getWalls();
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

		// Token: 0x06001093 RID: 4243 RVA: 0x00154774 File Offset: 0x00152974
		public override bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
		{
			return base.getTileIndexAt((int)p.X, (int)p.Y, "Front") == -1;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00154794 File Offset: 0x00152994
		public int getFloorAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> floors = DecoratableLocation.getFloors();
			for (int i = 0; i < floors.Count; i++)
			{
				if (floors[i].Contains(p))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x001547CD File Offset: 0x001529CD
		public Furniture getRandomFurniture(Random r)
		{
			if (this.furniture.Count > 0)
			{
				return this.furniture.ElementAt(r.Next(this.furniture.Count));
			}
			return null;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x001547FC File Offset: 0x001529FC
		public int getWallForRoomAt(Point p)
		{
			List<Microsoft.Xna.Framework.Rectangle> walls = DecoratableLocation.getWalls();
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

		// Token: 0x06001097 RID: 4247 RVA: 0x00154850 File Offset: 0x00152A50
		public virtual void setFloor(int which, int whichRoom = -1, bool persist = false)
		{
			List<Microsoft.Xna.Framework.Rectangle> rooms = DecoratableLocation.getFloors();
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

		// Token: 0x06001098 RID: 4248 RVA: 0x00154AE0 File Offset: 0x00152CE0
		public static List<Microsoft.Xna.Framework.Rectangle> getFloors()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
			{
				new Microsoft.Xna.Framework.Rectangle(1, 3, 11, 11)
			};
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00154AF8 File Offset: 0x00152CF8
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			using (List<Furniture>.Enumerator enumerator = this.furniture.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, -1, -1, 1f);
				}
			}
		}

		// Token: 0x040011DB RID: 4571
		public List<int> wallPaper = new List<int>();

		// Token: 0x040011DC RID: 4572
		public List<int> floor = new List<int>();

		// Token: 0x040011DD RID: 4573
		public List<Furniture> furniture = new List<Furniture>();
	}
}
