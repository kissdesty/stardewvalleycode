using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Menus;

namespace StardewValley.Objects
{
	// Token: 0x02000093 RID: 147
	public class Furniture : Object
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x000DF459 File Offset: 0x000DD659
		public Furniture()
		{
			this.updateDrawPosition();
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000DF468 File Offset: 0x000DD668
		public Furniture(int which, Vector2 tile, int initialRotations) : this(which, tile)
		{
			for (int i = 0; i < initialRotations; i++)
			{
				this.rotate();
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x000DF490 File Offset: 0x000DD690
		public Furniture(int which, Vector2 tile)
		{
			this.tileLocation = tile;
			if (Furniture.furnitureTexture == null)
			{
				Furniture.furnitureTexture = Game1.content.Load<Texture2D>("TileSheets\\furniture");
			}
			string[] data = Game1.content.Load<Dictionary<int, string>>("Data\\Furniture")[which].Split(new char[]
			{
				'/'
			});
			this.name = data[0];
			this.furniture_type = this.getTypeNumberFromName(data[1]);
			this.description = "Can be placed inside your house.";
			if (which == 1308)
			{
				this.description = Game1.parseText(Game1.content.LoadString("Strings\\Objects:CatalogueDescription", new object[0]), Game1.smallFont, Game1.tileSize * 5);
			}
			else if (which == 1226)
			{
				this.description = Game1.parseText(Game1.content.LoadString("Strings\\Objects:FurnitureCatalogueDescription", new object[0]), Game1.smallFont, Game1.tileSize * 5);
			}
			this.defaultSourceRect = new Rectangle(which * 16 % Furniture.furnitureTexture.Width, which * 16 / Furniture.furnitureTexture.Width * 16, 1, 1);
			this.drawHeldObjectLow = this.name.ToLower().Contains("tea");
			if (data[2].Equals("-1"))
			{
				this.sourceRect = this.getDefaultSourceRectForType(which, this.furniture_type);
				this.defaultSourceRect = this.sourceRect;
			}
			else
			{
				this.defaultSourceRect.Width = Convert.ToInt32(data[2].Split(new char[]
				{
					' '
				})[0]);
				this.defaultSourceRect.Height = Convert.ToInt32(data[2].Split(new char[]
				{
					' '
				})[1]);
				this.sourceRect = new Rectangle(which * 16 % Furniture.furnitureTexture.Width, which * 16 / Furniture.furnitureTexture.Width * 16, this.defaultSourceRect.Width * 16, this.defaultSourceRect.Height * 16);
				this.defaultSourceRect = this.sourceRect;
			}
			this.defaultBoundingBox = new Rectangle((int)this.tileLocation.X, (int)this.tileLocation.Y, 1, 1);
			if (data[3].Equals("-1"))
			{
				this.boundingBox = this.getDefaultBoundingBoxForType(this.furniture_type);
				this.defaultBoundingBox = this.boundingBox;
			}
			else
			{
				this.defaultBoundingBox.Width = Convert.ToInt32(data[3].Split(new char[]
				{
					' '
				})[0]);
				this.defaultBoundingBox.Height = Convert.ToInt32(data[3].Split(new char[]
				{
					' '
				})[1]);
				this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, this.defaultBoundingBox.Width * Game1.tileSize, this.defaultBoundingBox.Height * Game1.tileSize);
				this.defaultBoundingBox = this.boundingBox;
			}
			this.updateDrawPosition();
			this.rotations = Convert.ToInt32(data[4]);
			this.price = Convert.ToInt32(data[5]);
			this.parentSheetIndex = which;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000DF7B9 File Offset: 0x000DD9B9
		public override string getDescription()
		{
			return this.description;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000DF7C1 File Offset: 0x000DD9C1
		public override bool performDropDownAction(Farmer who)
		{
			this.resetOnPlayerEntry((who == null) ? Game1.currentLocation : who.currentLocation);
			return false;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000DF7DA File Offset: 0x000DD9DA
		public override void hoverAction()
		{
			base.hoverAction();
			if (!Game1.player.isInventoryFull())
			{
				Game1.mouseCursor = 2;
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000DF7F4 File Offset: 0x000DD9F4
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			if (this.parentSheetIndex == 1402)
			{
				Game1.activeClickableMenu = new Billboard(false);
			}
			else if (this.parentSheetIndex == 1308)
			{
				Game1.activeClickableMenu = new ShopMenu(Utility.getAllWallpapersAndFloorsForFree(), 0, null);
			}
			else if (this.parentSheetIndex == 1226)
			{
				Game1.activeClickableMenu = new ShopMenu(Utility.getAllFurnituresForFree(), 0, null);
			}
			return this.clicked(who);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000DF868 File Offset: 0x000DDA68
		public override bool clicked(Farmer who)
		{
			Game1.haltAfterCheck = false;
			if (this.furniture_type == 11 && who.ActiveObject != null && who.ActiveObject != null && this.heldObject == null)
			{
				return false;
			}
			if (this.heldObject == null && (who.ActiveObject == null || !(who.ActiveObject is Furniture)))
			{
				this.flaggedForPickUp = true;
				return true;
			}
			if (this.heldObject != null && who.addItemToInventoryBool(this.heldObject, false))
			{
				this.heldObject.performRemoveAction(this.tileLocation, who.currentLocation);
				this.heldObject = null;
				Game1.playSound("coin");
				return true;
			}
			return false;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x000DF907 File Offset: 0x000DDB07
		public override void DayUpdate(GameLocation location)
		{
			base.DayUpdate(location);
			this.lightGlowAdded = false;
			if (!Game1.isDarkOut() || (Game1.newDay && !Game1.isRaining))
			{
				this.removeLights(location);
				return;
			}
			this.addLights(location);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000DF93B File Offset: 0x000DDB3B
		public void resetOnPlayerEntry(GameLocation environment)
		{
			this.removeLights(environment);
			if (Game1.isDarkOut())
			{
				this.addLights(environment);
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000DF954 File Offset: 0x000DDB54
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if ((this.furniture_type == 11 || this.furniture_type == 5) && this.heldObject == null && !dropIn.bigCraftable && (!(dropIn is Furniture) || ((dropIn as Furniture).getTilesWide() == 1 && (dropIn as Furniture).getTilesHigh() == 1)))
			{
				this.heldObject = (Object)dropIn.getOne();
				this.heldObject.tileLocation = this.tileLocation;
				this.heldObject.boundingBox.X = this.boundingBox.X;
				this.heldObject.boundingBox.Y = this.boundingBox.Y;
				this.heldObject.performDropDownAction(who);
				if (!probe)
				{
					Game1.playSound("woodyStep");
					if (who != null)
					{
						who.reduceActiveItemByOne();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000DFA34 File Offset: 0x000DDC34
		private void addLights(GameLocation environment)
		{
			if (this.furniture_type == 7)
			{
				if (this.sourceIndexOffset == 0)
				{
					this.sourceRect = this.defaultSourceRect;
					this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
				}
				this.sourceIndexOffset = 1;
				if (this.lightSource == null)
				{
					Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
					this.lightSource = new LightSource(4, new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y - Game1.tileSize)), 2f, Color.Black, (int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
					Game1.currentLightSources.Add(this.lightSource);
					return;
				}
			}
			else if (this.furniture_type == 13)
			{
				if (this.sourceIndexOffset == 0)
				{
					this.sourceRect = this.defaultSourceRect;
					this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
				}
				this.sourceIndexOffset = 1;
				if (this.lightGlowAdded)
				{
					environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
					this.lightGlowAdded = false;
				}
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x000DFBA8 File Offset: 0x000DDDA8
		private void removeLights(GameLocation environment)
		{
			if (this.furniture_type == 7)
			{
				if (this.sourceIndexOffset == 1)
				{
					this.sourceRect = this.defaultSourceRect;
				}
				this.sourceIndexOffset = 0;
				Utility.removeLightSource((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
				this.lightSource = null;
				return;
			}
			if (this.furniture_type == 13)
			{
				if (this.sourceIndexOffset == 1)
				{
					this.sourceRect = this.defaultSourceRect;
				}
				this.sourceIndexOffset = 0;
				if (Game1.isRaining)
				{
					this.sourceRect = this.defaultSourceRect;
					this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width;
					this.sourceIndexOffset = 1;
					return;
				}
				if (!this.lightGlowAdded && !environment.lightGlows.Contains(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize))))
				{
					environment.lightGlows.Add(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
				}
				this.lightGlowAdded = true;
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000DFCE1 File Offset: 0x000DDEE1
		public override bool minutesElapsed(int minutes, GameLocation environment)
		{
			if (Game1.isDarkOut())
			{
				this.addLights(environment);
			}
			else
			{
				this.removeLights(environment);
			}
			return false;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x000DFCFC File Offset: 0x000DDEFC
		public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
		{
			this.removeLights(environment);
			if (this.furniture_type == 13 && this.lightGlowAdded)
			{
				environment.lightGlows.Remove(new Vector2((float)(this.boundingBox.X + Game1.tileSize / 2), (float)(this.boundingBox.Y + Game1.tileSize)));
				this.lightGlowAdded = false;
			}
			base.performRemoveAction(tileLocation, environment);
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000DFD68 File Offset: 0x000DDF68
		public void rotate()
		{
			if (this.rotations < 2)
			{
				return;
			}
			int arg_10_0 = this.currentRotation;
			int rotationAmount = (this.rotations == 4) ? 1 : 2;
			this.currentRotation += rotationAmount;
			this.currentRotation %= 4;
			this.flipped = false;
			Point specialRotationOffsets = default(Point);
			int num = this.furniture_type;
			switch (num)
			{
			case 2:
				specialRotationOffsets.Y = 1;
				specialRotationOffsets.X = -1;
				break;
			case 3:
				specialRotationOffsets.X = -1;
				specialRotationOffsets.Y = 1;
				break;
			case 4:
				break;
			case 5:
				specialRotationOffsets.Y = 0;
				specialRotationOffsets.X = -1;
				break;
			default:
				if (num == 12)
				{
					specialRotationOffsets.X = 0;
					specialRotationOffsets.Y = 0;
				}
				break;
			}
			bool differentSizesFor2Rotations = this.furniture_type == 5 || this.furniture_type == 12 || this.parentSheetIndex == 724 || this.parentSheetIndex == 727;
			bool sourceRectRotate = this.defaultBoundingBox.Width != this.defaultBoundingBox.Height;
			if (differentSizesFor2Rotations && this.currentRotation == 2)
			{
				this.currentRotation = 1;
			}
			if (sourceRectRotate)
			{
				int oldBoundingBoxHeight = this.boundingBox.Height;
				switch (this.currentRotation)
				{
				case 0:
				case 2:
					this.boundingBox.Height = this.defaultBoundingBox.Height;
					this.boundingBox.Width = this.defaultBoundingBox.Width;
					break;
				case 1:
				case 3:
					this.boundingBox.Height = this.boundingBox.Width + specialRotationOffsets.X * Game1.tileSize;
					this.boundingBox.Width = oldBoundingBoxHeight + specialRotationOffsets.Y * Game1.tileSize;
					break;
				}
			}
			Point specialSpecialSourceRectOffset = default(Point);
			num = this.furniture_type;
			if (num == 12)
			{
				specialSpecialSourceRectOffset.X = 1;
				specialSpecialSourceRectOffset.Y = -1;
			}
			if (sourceRectRotate)
			{
				switch (this.currentRotation)
				{
				case 0:
					this.sourceRect = this.defaultSourceRect;
					break;
				case 1:
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + specialRotationOffsets.Y * 16 + specialSpecialSourceRectOffset.X * 16, this.defaultSourceRect.Width + 16 + specialRotationOffsets.X * 16 + specialSpecialSourceRectOffset.Y * 16);
					break;
				case 2:
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width + this.defaultSourceRect.Height - 16 + specialRotationOffsets.Y * 16 + specialSpecialSourceRectOffset.X * 16, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
					break;
				case 3:
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Height - 16 + specialRotationOffsets.Y * 16 + specialSpecialSourceRectOffset.X * 16, this.defaultSourceRect.Width + 16 + specialRotationOffsets.X * 16 + specialSpecialSourceRectOffset.Y * 16);
					this.flipped = true;
					break;
				}
			}
			else
			{
				this.flipped = (this.currentRotation == 3);
				if (this.rotations == 2)
				{
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 2) ? 1 : 0) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
				}
				else
				{
					this.sourceRect = new Rectangle(this.defaultSourceRect.X + ((this.currentRotation == 3) ? 1 : this.currentRotation) * this.defaultSourceRect.Width, this.defaultSourceRect.Y, this.defaultSourceRect.Width, this.defaultSourceRect.Height);
				}
			}
			if (differentSizesFor2Rotations && this.currentRotation == 1)
			{
				this.currentRotation = 2;
			}
			this.updateDrawPosition();
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x000E01CB File Offset: 0x000DE3CB
		public bool isGroundFurniture()
		{
			return this.furniture_type != 13 && this.furniture_type != 6 && this.furniture_type != 13;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool canBeGivenAsGift()
		{
			return false;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x000E01F0 File Offset: 0x000DE3F0
		public override bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			if (!(l is DecoratableLocation))
			{
				return false;
			}
			for (int x = 0; x < this.boundingBox.Width / Game1.tileSize; x++)
			{
				for (int y = 0; y < this.boundingBox.Height / Game1.tileSize; y++)
				{
					Vector2 nonTile = tile * (float)Game1.tileSize + new Vector2((float)x, (float)y) * (float)Game1.tileSize;
					nonTile.X += (float)(Game1.tileSize / 2);
					nonTile.Y += (float)(Game1.tileSize / 2);
					foreach (Furniture f in (l as DecoratableLocation).furniture)
					{
						if (f.furniture_type == 11 && f.getBoundingBox(f.tileLocation).Contains((int)nonTile.X, (int)nonTile.Y) && f.heldObject == null && this.getTilesWide() == 1)
						{
							bool result = true;
							return result;
						}
						if ((f.furniture_type != 12 || this.furniture_type == 12) && f.getBoundingBox(f.tileLocation).Contains((int)nonTile.X, (int)nonTile.Y))
						{
							bool result = false;
							return result;
						}
					}
					Vector2 currentTile = tile + new Vector2((float)x, (float)y);
					if (l.Objects.ContainsKey(currentTile))
					{
						return false;
					}
				}
			}
			return base.canBePlacedHere(l, tile);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x000E039C File Offset: 0x000DE59C
		public void updateDrawPosition()
		{
			this.drawPosition = new Vector2((float)this.boundingBox.X, (float)(this.boundingBox.Y - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)));
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x000E03EA File Offset: 0x000DE5EA
		public int getTilesWide()
		{
			return this.boundingBox.Width / Game1.tileSize;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x000E03FD File Offset: 0x000DE5FD
		public int getTilesHigh()
		{
			return this.boundingBox.Height / Game1.tileSize;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000E0410 File Offset: 0x000DE610
		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			if (location is DecoratableLocation)
			{
				Point anchor = new Point(x / Game1.tileSize, y / Game1.tileSize);
				List<Rectangle> walls;
				if (location is FarmHouse)
				{
					walls = FarmHouse.getWalls((location as FarmHouse).upgradeLevel);
				}
				else
				{
					walls = DecoratableLocation.getWalls();
				}
				this.tileLocation = new Vector2((float)anchor.X, (float)anchor.Y);
				bool paintingAtRightPlace = false;
				if (this.furniture_type == 6 || this.furniture_type == 13 || this.parentSheetIndex == 1293)
				{
					int offset = (this.parentSheetIndex == 1293) ? 3 : 0;
					bool foundWall = false;
					foreach (Rectangle w in walls)
					{
						if ((this.furniture_type == 6 || this.furniture_type == 13 || offset != 0) && w.Y + offset == anchor.Y && w.Contains(anchor.X, anchor.Y - offset))
						{
							foundWall = true;
							break;
						}
					}
					if (!foundWall)
					{
						Game1.showRedMessage("Must be placed on wall");
						return false;
					}
					paintingAtRightPlace = true;
				}
				for (int furnitureX = anchor.X; furnitureX < anchor.X + this.getTilesWide(); furnitureX++)
				{
					for (int furnitureY = anchor.Y; furnitureY < anchor.Y + this.getTilesHigh(); furnitureY++)
					{
						if (location.doesTileHaveProperty(furnitureX, furnitureY, "NoFurniture", "Back") != null)
						{
							Game1.showRedMessage("Furniture can't be placed here");
							return false;
						}
						if (!paintingAtRightPlace && Utility.pointInRectangles(walls, furnitureX, furnitureY))
						{
							Game1.showRedMessage("Can't place on wall");
							return false;
						}
						if (location.getTileIndexAt(furnitureX, furnitureY, "Buildings") != -1)
						{
							return false;
						}
					}
				}
				this.boundingBox = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, this.boundingBox.Width, this.boundingBox.Height);
				foreach (Furniture f in (location as DecoratableLocation).furniture)
				{
					if (f.furniture_type == 11 && f.heldObject == null && f.getBoundingBox(f.tileLocation).Intersects(this.boundingBox))
					{
						f.performObjectDropInAction(this, false, (who == null) ? Game1.player : who);
						bool result = true;
						return result;
					}
				}
				using (List<Farmer>.Enumerator enumerator3 = location.getFarmers().GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.GetBoundingBox().Intersects(this.boundingBox))
						{
							Game1.showRedMessage("Can't place on top of a person.");
							bool result = false;
							return result;
						}
					}
				}
				this.updateDrawPosition();
				return base.placementAction(location, x, y, who);
			}
			Game1.showRedMessage("Can only be placed in House");
			return false;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool isPlaceable()
		{
			return true;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000DF246 File Offset: 0x000DD446
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return this.boundingBox;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x000E0728 File Offset: 0x000DE928
		private Rectangle getDefaultSourceRectForType(int tileIndex, int type)
		{
			int width;
			int height;
			switch (type)
			{
			case 0:
				width = 1;
				height = 2;
				goto IL_92;
			case 1:
				width = 2;
				height = 2;
				goto IL_92;
			case 2:
				width = 3;
				height = 2;
				goto IL_92;
			case 3:
				width = 2;
				height = 2;
				goto IL_92;
			case 4:
				width = 2;
				height = 2;
				goto IL_92;
			case 5:
				width = 5;
				height = 3;
				goto IL_92;
			case 6:
				width = 2;
				height = 2;
				goto IL_92;
			case 7:
				width = 1;
				height = 3;
				goto IL_92;
			case 8:
				width = 1;
				height = 2;
				goto IL_92;
			case 10:
				width = 2;
				height = 3;
				goto IL_92;
			case 11:
				width = 2;
				height = 3;
				goto IL_92;
			case 12:
				width = 3;
				height = 2;
				goto IL_92;
			case 13:
				width = 1;
				height = 2;
				goto IL_92;
			}
			width = 1;
			height = 2;
			IL_92:
			return new Rectangle(tileIndex * 16 % Furniture.furnitureTexture.Width, tileIndex * 16 / Furniture.furnitureTexture.Width * 16, width * 16, height * 16);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000E07F8 File Offset: 0x000DE9F8
		private Rectangle getDefaultBoundingBoxForType(int type)
		{
			int width;
			int height;
			switch (type)
			{
			case 0:
				width = 1;
				height = 1;
				goto IL_92;
			case 1:
				width = 2;
				height = 1;
				goto IL_92;
			case 2:
				width = 3;
				height = 1;
				goto IL_92;
			case 3:
				width = 2;
				height = 1;
				goto IL_92;
			case 4:
				width = 2;
				height = 1;
				goto IL_92;
			case 5:
				width = 5;
				height = 2;
				goto IL_92;
			case 6:
				width = 2;
				height = 2;
				goto IL_92;
			case 7:
				width = 1;
				height = 1;
				goto IL_92;
			case 8:
				width = 1;
				height = 1;
				goto IL_92;
			case 10:
				width = 2;
				height = 1;
				goto IL_92;
			case 11:
				width = 2;
				height = 2;
				goto IL_92;
			case 12:
				width = 3;
				height = 2;
				goto IL_92;
			case 13:
				width = 1;
				height = 2;
				goto IL_92;
			}
			width = 1;
			height = 1;
			IL_92:
			return new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, width * Game1.tileSize, height * Game1.tileSize);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000E08D0 File Offset: 0x000DEAD0
		private int getTypeNumberFromName(string typeName)
		{
			string text = typeName.ToLower();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1555340682u)
			{
				if (num <= 732630053u)
				{
					if (num != 44871939u)
					{
						if (num != 600654789u)
						{
							if (num == 732630053u)
							{
								if (text == "couch")
								{
									return 2;
								}
							}
						}
						else if (text == "rug")
						{
							return 12;
						}
					}
					else if (text == "long table")
					{
						return 5;
					}
				}
				else if (num != 1049849701u)
				{
					if (num != 1251777503u)
					{
						if (num == 1555340682u)
						{
							if (text == "chair")
							{
								return 0;
							}
						}
					}
					else if (text == "table")
					{
						return 11;
					}
				}
				else if (text == "painting")
				{
					return 6;
				}
			}
			else if (num <= 2058371002u)
			{
				if (num != 1651424953u)
				{
					if (num != 1810951995u)
					{
						if (num == 2058371002u)
						{
							if (text == "armchair")
							{
								return 3;
							}
						}
					}
					else if (text == "lamp")
					{
						return 7;
					}
				}
				else if (text == "bench")
				{
					return 1;
				}
			}
			else if (num <= 2708649949u)
			{
				if (num != 2236496455u)
				{
					if (num == 2708649949u)
					{
						if (text == "window")
						{
							return 13;
						}
					}
				}
				else if (text == "dresser")
				{
					return 4;
				}
			}
			else if (num != 3104904292u)
			{
				if (num == 3358447858u)
				{
					if (text == "decor")
					{
						return 8;
					}
				}
			}
			else if (text == "bookcase")
			{
				return 10;
			}
			return 9;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000E0AAB File Offset: 0x000DECAB
		public override int salePrice()
		{
			return this.price;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0000846E File Offset: 0x0000666E
		public override int maximumStackSize()
		{
			return 1;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0007D8B5 File Offset: 0x0007BAB5
		public override int getStack()
		{
			return this.stack;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0000846E File Offset: 0x0000666E
		public override int addToStack(int amount)
		{
			return 1;
		}

		// Token: 0x170000C7 RID: 199
		public override string Name
		{
			// Token: 0x06000ADA RID: 2778 RVA: 0x000DF24E File Offset: 0x000DD44E
			get
			{
				return this.name;
			}
			// Token: 0x06000ADB RID: 2779 RVA: 0x00074DEA File Offset: 0x00072FEA
			set
			{
				this.name = value;
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x000E0AB4 File Offset: 0x000DECB4
		private float getScaleSize()
		{
			int tilesWide = this.sourceRect.Width / 16;
			int tilesHigh = this.sourceRect.Height / 16;
			if (tilesWide >= 5)
			{
				return 0.75f;
			}
			if (tilesHigh >= 3)
			{
				return 1f;
			}
			if (tilesWide <= 2)
			{
				return 2f;
			}
			if (tilesWide <= 4)
			{
				return 1f;
			}
			return 0.1f;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00002834 File Offset: 0x00000A34
		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x000E0B0C File Offset: 0x000DED0C
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Furniture.furnitureTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.defaultSourceRect), Color.White * transparency, 0f, new Vector2((float)(this.defaultSourceRect.Width / 2), (float)(this.defaultSourceRect.Height / 2)), 1f * this.getScaleSize() * scaleSize, SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x000E0B90 File Offset: 0x000DED90
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (x == -1)
			{
				spriteBatch.Draw(Furniture.furnitureTexture, Game1.GlobalToLocal(Game1.viewport, this.drawPosition), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.furniture_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
			}
			else
			{
				spriteBatch.Draw(Furniture.furnitureTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - (this.sourceRect.Height * Game1.pixelZoom - this.boundingBox.Height)))), new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.furniture_type == 12) ? 0f : ((float)(this.boundingBox.Bottom - 8) / 10000f));
			}
			if (this.heldObject != null)
			{
				if (this.heldObject is Furniture)
				{
					(this.heldObject as Furniture).drawAtNonTileSpot(spriteBatch, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.heldObject as Furniture).sourceRect.Height * Game1.pixelZoom - (this.drawHeldObjectLow ? (-Game1.tileSize / 4) : (Game1.tileSize / 4))))), (float)(this.boundingBox.Bottom - 7) / 10000f, alpha);
					return;
				}
				spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.drawHeldObjectLow ? (Game1.tileSize / 2) : (Game1.tileSize * 4 / 3))))) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White * alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)this.boundingBox.Bottom / 10000f);
				spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.boundingBox.Center.X - Game1.tileSize / 2), (float)(this.boundingBox.Center.Y - (this.drawHeldObjectLow ? (Game1.tileSize / 2) : (Game1.tileSize * 4 / 3))))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.heldObject.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.boundingBox.Bottom + 1) / 10000f);
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000E0F00 File Offset: 0x000DF100
		public void drawAtNonTileSpot(SpriteBatch spriteBatch, Vector2 location, float layerDepth, float alpha = 1f)
		{
			spriteBatch.Draw(Furniture.furnitureTexture, location, new Rectangle?(this.sourceRect), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000E0F50 File Offset: 0x000DF150
		public override Item getOne()
		{
			Furniture expr_11 = new Furniture(this.parentSheetIndex, this.tileLocation);
			expr_11.drawPosition = this.drawPosition;
			expr_11.defaultBoundingBox = this.defaultBoundingBox;
			expr_11.boundingBox = this.boundingBox;
			expr_11.currentRotation = this.currentRotation - 1;
			expr_11.rotations = this.rotations;
			expr_11.rotate();
			return expr_11;
		}

		// Token: 0x04000AFC RID: 2812
		public const int chair = 0;

		// Token: 0x04000AFD RID: 2813
		public const int bench = 1;

		// Token: 0x04000AFE RID: 2814
		public const int couch = 2;

		// Token: 0x04000AFF RID: 2815
		public const int armchair = 3;

		// Token: 0x04000B00 RID: 2816
		public const int dresser = 4;

		// Token: 0x04000B01 RID: 2817
		public const int longTable = 5;

		// Token: 0x04000B02 RID: 2818
		public const int painting = 6;

		// Token: 0x04000B03 RID: 2819
		public const int lamp = 7;

		// Token: 0x04000B04 RID: 2820
		public const int decor = 8;

		// Token: 0x04000B05 RID: 2821
		public const int other = 9;

		// Token: 0x04000B06 RID: 2822
		public const int bookcase = 10;

		// Token: 0x04000B07 RID: 2823
		public const int table = 11;

		// Token: 0x04000B08 RID: 2824
		public const int rug = 12;

		// Token: 0x04000B09 RID: 2825
		public const int window = 13;

		// Token: 0x04000B0A RID: 2826
		public new int price;

		// Token: 0x04000B0B RID: 2827
		public int furniture_type;

		// Token: 0x04000B0C RID: 2828
		public int rotations;

		// Token: 0x04000B0D RID: 2829
		public int currentRotation;

		// Token: 0x04000B0E RID: 2830
		private int sourceIndexOffset;

		// Token: 0x04000B0F RID: 2831
		protected Vector2 drawPosition;

		// Token: 0x04000B10 RID: 2832
		public Rectangle sourceRect;

		// Token: 0x04000B11 RID: 2833
		public Rectangle defaultSourceRect;

		// Token: 0x04000B12 RID: 2834
		public Rectangle defaultBoundingBox;

		// Token: 0x04000B13 RID: 2835
		public string description;

		// Token: 0x04000B14 RID: 2836
		public static Texture2D furnitureTexture;

		// Token: 0x04000B15 RID: 2837
		public new bool flipped;

		// Token: 0x04000B16 RID: 2838
		public bool drawHeldObjectLow;

		// Token: 0x04000B17 RID: 2839
		[XmlIgnore]
		public bool flaggedForPickUp;

		// Token: 0x04000B18 RID: 2840
		private bool lightGlowAdded;
	}
}
