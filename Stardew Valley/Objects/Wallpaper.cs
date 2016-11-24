using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace StardewValley.Objects
{
	// Token: 0x02000092 RID: 146
	public class Wallpaper : Object
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x000D9F75 File Offset: 0x000D8175
		public Wallpaper()
		{
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000DEFAC File Offset: 0x000DD1AC
		public Wallpaper(int which, bool isFloor = false)
		{
			if (Wallpaper.wallpaperTexture == null)
			{
				Wallpaper.wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
			}
			this.isFloor = isFloor;
			this.parentSheetIndex = which;
			this.name = (isFloor ? "Flooring" : "Wallpaper");
			this.sourceRect = (isFloor ? new Rectangle(which % 8 * 32, 336 + which / 8 * 32, 28, 26) : new Rectangle(which % 16 * 16, which / 16 * 48 + 8, 16, 28));
			this.price = 100;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000DF044 File Offset: 0x000DD244
		public override string getDescription()
		{
			if (!this.isFloor)
			{
				return "Decorates the walls of one room";
			}
			return "Decorates the floor of one room.";
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool performDropDownAction(Farmer who)
		{
			return true;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			return false;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000DF05C File Offset: 0x000DD25C
		public override bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			Vector2 nonTile = tile * (float)Game1.tileSize;
			nonTile.X += (float)(Game1.tileSize / 2);
			nonTile.Y += (float)(Game1.tileSize / 2);
			if (l is DecoratableLocation)
			{
				foreach (Furniture f in (l as DecoratableLocation).furniture)
				{
					if (f.furniture_type != 12 && f.getBoundingBox(f.tileLocation).Contains((int)nonTile.X, (int)nonTile.Y))
					{
						return false;
					}
				}
			}
			return base.canBePlacedHere(l, tile);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000DF128 File Offset: 0x000DD328
		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			if (who.currentLocation is DecoratableLocation)
			{
				Point tile = new Point(x / Game1.tileSize, y / Game1.tileSize);
				DecoratableLocation farmHouse = who.currentLocation as DecoratableLocation;
				if (this.isFloor)
				{
					List<Rectangle> floors;
					if (farmHouse is FarmHouse)
					{
						floors = FarmHouse.getFloors((farmHouse as FarmHouse).upgradeLevel);
					}
					else
					{
						floors = DecoratableLocation.getFloors();
					}
					for (int i = 0; i < floors.Count; i++)
					{
						if (floors[i].Contains(tile))
						{
							farmHouse.setFloor(this.parentSheetIndex, i, true);
							Game1.playSound("coin");
							return true;
						}
					}
				}
				else
				{
					List<Rectangle> walls;
					if (farmHouse is FarmHouse)
					{
						walls = FarmHouse.getWalls((farmHouse as FarmHouse).upgradeLevel);
					}
					else
					{
						walls = DecoratableLocation.getWalls();
					}
					for (int j = 0; j < walls.Count; j++)
					{
						if (walls[j].Contains(tile))
						{
							farmHouse.setWallpaper(this.parentSheetIndex, j, true);
							Game1.playSound("coin");
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool isPlaceable()
		{
			return true;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000DF246 File Offset: 0x000DD446
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return this.boundingBox;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00074E66 File Offset: 0x00073066
		public override int salePrice()
		{
			return this.price;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0000846E File Offset: 0x0000666E
		public override int maximumStackSize()
		{
			return 1;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0007D8B5 File Offset: 0x0007BAB5
		public override int getStack()
		{
			return this.stack;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0000846E File Offset: 0x0000666E
		public override int addToStack(int amount)
		{
			return 1;
		}

		// Token: 0x170000C6 RID: 198
		public override string Name
		{
			// Token: 0x06000AB4 RID: 2740 RVA: 0x000DF24E File Offset: 0x000DD44E
			get
			{
				return this.name;
			}
			// Token: 0x06000AB5 RID: 2741 RVA: 0x00074DEA File Offset: 0x00072FEA
			set
			{
				this.name = value;
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000DF256 File Offset: 0x000DD456
		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			base.drawInMenu(spriteBatch, objectPosition, 1f);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000DF268 File Offset: 0x000DD468
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			if (Wallpaper.wallpaperTexture == null)
			{
				Wallpaper.wallpaperTexture = Game1.content.Load<Texture2D>("Maps\\walls_and_floors");
			}
			if (this.isFloor)
			{
				spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(Wallpaper.floorContainerRect), Color.White * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
				spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2 - 2)), new Rectangle?(this.sourceRect), Color.White * transparency, 0f, new Vector2(14f, 13f), 2f * scaleSize, SpriteEffects.None, layerDepth + 0.001f);
				return;
			}
			spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(Wallpaper.wallpaperContainerRect), Color.White * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
			spriteBatch.Draw(Wallpaper.wallpaperTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.sourceRect), Color.White * transparency, 0f, new Vector2(8f, 14f), 2f * scaleSize, SpriteEffects.None, layerDepth + 0.001f);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000DF414 File Offset: 0x000DD614
		public override Item getOne()
		{
			return new Wallpaper(this.parentSheetIndex, this.isFloor);
		}

		// Token: 0x04000AF7 RID: 2807
		public Rectangle sourceRect;

		// Token: 0x04000AF8 RID: 2808
		public static Texture2D wallpaperTexture;

		// Token: 0x04000AF9 RID: 2809
		public bool isFloor;

		// Token: 0x04000AFA RID: 2810
		private static Rectangle wallpaperContainerRect = new Rectangle(193, 496, 16, 16);

		// Token: 0x04000AFB RID: 2811
		private static Rectangle floorContainerRect = new Rectangle(209, 496, 16, 16);
	}
}
