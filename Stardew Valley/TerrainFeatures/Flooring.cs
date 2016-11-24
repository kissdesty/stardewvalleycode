using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200006F RID: 111
	public class Flooring : TerrainFeature
	{
		// Token: 0x0600096D RID: 2413 RVA: 0x000CA855 File Offset: 0x000C8A55
		public Flooring()
		{
			this.loadSprite();
			if (Flooring.drawGuide == null)
			{
				Flooring.populateDrawGuide();
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000CA870 File Offset: 0x000C8A70
		public Flooring(int which) : this()
		{
			this.whichFloor = which;
			if (this.whichFloor == 5 || this.whichFloor == 6 || this.whichFloor == 8 || this.whichFloor == 7)
			{
				this.isPathway = true;
			}
			if (this.whichFloor == 9)
			{
				this.whichView = Game1.random.Next(16);
				this.isSteppingStone = true;
				this.isPathway = true;
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000CA8DF File Offset: 0x000C8ADF
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.Y * (float)Game1.tileSize), Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000CA90C File Offset: 0x000C8B0C
		public static void populateDrawGuide()
		{
			Flooring.drawGuide = new Dictionary<byte, int>();
			Flooring.drawGuide.Add(0, 0);
			Flooring.drawGuide.Add(6, 1);
			Flooring.drawGuide.Add(14, 2);
			Flooring.drawGuide.Add(12, 3);
			Flooring.drawGuide.Add(4, 16);
			Flooring.drawGuide.Add(7, 17);
			Flooring.drawGuide.Add(15, 18);
			Flooring.drawGuide.Add(13, 19);
			Flooring.drawGuide.Add(5, 32);
			Flooring.drawGuide.Add(3, 33);
			Flooring.drawGuide.Add(11, 34);
			Flooring.drawGuide.Add(9, 35);
			Flooring.drawGuide.Add(1, 48);
			Flooring.drawGuide.Add(2, 49);
			Flooring.drawGuide.Add(10, 50);
			Flooring.drawGuide.Add(8, 51);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x000CA9F8 File Offset: 0x000C8BF8
		public override void loadSprite()
		{
			if (Flooring.floorsTexture == null)
			{
				try
				{
					Flooring.floorsTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\Flooring");
				}
				catch (Exception)
				{
				}
			}
			if (this.whichFloor == 5 || this.whichFloor == 6 || this.whichFloor == 8 || this.whichFloor == 7 || this.whichFloor == 9)
			{
				this.isPathway = true;
			}
			if (this.whichFloor == 9)
			{
				this.isSteppingStone = true;
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000CAA7C File Offset: 0x000C8C7C
		public override void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
		{
			base.doCollisionAction(positionOfCollider, speedOfCollision, tileLocation, who, location);
			if (who != null && who is Farmer && location is Farm)
			{
				(who as Farmer).temporarySpeedBuff = 0.1f;
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool isPassable(Character c = null)
		{
			return true;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x000CAAB4 File Offset: 0x000C8CB4
		public string getFootstepSound()
		{
			switch (this.whichFloor)
			{
			case 0:
			case 2:
			case 4:
				return "woodyStep";
			case 1:
				return "stoneStep";
			case 3:
			case 6:
				return "thudStep";
			case 5:
				return "dirtyHit";
			default:
				return "stoneStep";
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000CAB0C File Offset: 0x000C8D0C
		public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if ((t != null || damage > 0) && (damage > 0 || t.GetType() == typeof(Pickaxe) || t.GetType() == typeof(Axe)))
			{
				Game1.createRadialDebris(location, (this.whichFloor == 0) ? 12 : 14, (int)tileLocation.X, (int)tileLocation.Y, 4, false, -1, false, -1);
				int item = -1;
				switch (this.whichFloor)
				{
				case 0:
					Game1.playSound("axchop");
					item = 328;
					break;
				case 1:
					Game1.playSound("hammer");
					item = 329;
					break;
				case 2:
					Game1.playSound("axchop");
					item = 331;
					break;
				case 3:
					Game1.playSound("hammer");
					item = 333;
					break;
				case 4:
					Game1.playSound("axchop");
					item = 401;
					break;
				case 5:
					Game1.playSound("hammer");
					item = 407;
					break;
				case 6:
					Game1.playSound("axchop");
					item = 405;
					break;
				case 7:
					Game1.playSound("hammer");
					item = 409;
					break;
				case 8:
					Game1.playSound("hammer");
					item = 411;
					break;
				case 9:
					Game1.playSound("hammer");
					item = 415;
					break;
				}
				location.debris.Add(new Debris(new Object(item, 1, false, -1, 0), tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
				return true;
			}
			return false;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x000CACC4 File Offset: 0x000C8EC4
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
			int sourceRectOffset = this.whichFloor * 4 * Game1.tileSize;
			byte drawSum = 0;
			Vector2 surroundingLocations = tileLocation;
			surroundingLocations.X += 1f;
			GameLocation farm = Game1.getLocationFromName("Farm");
			if (farm.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(Flooring))
			{
				drawSum += 2;
			}
			surroundingLocations.X -= 2f;
			if (farm.terrainFeatures.ContainsKey(surroundingLocations) && Game1.currentLocation.terrainFeatures[surroundingLocations].GetType() == typeof(Flooring))
			{
				drawSum += 8;
			}
			surroundingLocations.X += 1f;
			surroundingLocations.Y += 1f;
			if (Game1.currentLocation.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(Flooring))
			{
				drawSum += 4;
			}
			surroundingLocations.Y -= 2f;
			if (farm.terrainFeatures.ContainsKey(surroundingLocations) && farm.terrainFeatures[surroundingLocations].GetType() == typeof(Flooring))
			{
				drawSum += 1;
			}
			int sourceRectPosition = Flooring.drawGuide[drawSum];
			spriteBatch.Draw(Flooring.floorsTexture, positionOnScreen, new Rectangle?(new Rectangle(sourceRectPosition % 16 * 16, sourceRectPosition / 16 * 16 + sourceRectOffset, 16, 16)), Color.White, 0f, Vector2.Zero, scale * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth + positionOnScreen.Y / 20000f);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x000CAE84 File Offset: 0x000C9084
		private bool doesTileCountForDrawing(Vector2 surroundingLocations)
		{
			TerrainFeature f;
			Game1.currentLocation.terrainFeatures.TryGetValue(surroundingLocations, out f);
			return f != null && f is Flooring && (f as Flooring).whichFloor == this.whichFloor;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000CAEC4 File Offset: 0x000C90C4
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			byte drawSum = 0;
			Vector2 surroundingLocations = tileLocation;
			surroundingLocations.X += 1f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				drawSum += 2;
			}
			surroundingLocations.X -= 2f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				drawSum += 8;
			}
			surroundingLocations.X += 1f;
			surroundingLocations.Y += 1f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				drawSum += 4;
			}
			surroundingLocations.Y -= 2f;
			if (this.doesTileCountForDrawing(surroundingLocations))
			{
				drawSum += 1;
			}
			surroundingLocations.X -= 1f;
			if (!this.isPathway)
			{
				if (!this.doesTileCountForDrawing(surroundingLocations) && (drawSum & 9) == 9)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(60 + 64 * (this.whichFloor % 4), 44 + this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f) / 20000f);
				}
				surroundingLocations.X += 2f;
				if (!this.doesTileCountForDrawing(surroundingLocations) && (drawSum & 3) == 3)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4), tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(16 + 64 * (this.whichFloor % 4), 44 + this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f + (float)this.whichFloor) / 20000f);
				}
				surroundingLocations.Y += 2f;
				if (!this.doesTileCountForDrawing(surroundingLocations) && (drawSum & 6) == 6)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(16 + 64 * (this.whichFloor % 4), this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f) / 20000f);
				}
				surroundingLocations.X -= 2f;
				if (!this.doesTileCountForDrawing(surroundingLocations) && (drawSum & 12) == 12)
				{
					spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(60 + 64 * (this.whichFloor % 4), this.whichFloor / 4 * 64, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + 2f + tileLocation.X / 10000f) / 20000f);
				}
				spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)(tileLocation.X * (float)Game1.tileSize) - 4 - Game1.viewport.X, (int)(tileLocation.Y * (float)Game1.tileSize) + 4 - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Black * 0.33f);
			}
			int sourceRectPosition = Flooring.drawGuide[drawSum];
			if (this.isSteppingStone)
			{
				sourceRectPosition = Flooring.drawGuide.ElementAt(this.whichView).Value;
			}
			spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(this.whichFloor % 4 * 64 + sourceRectPosition * 16 % 256, sourceRectPosition / 16 * 16 + this.whichFloor / 4 * 64, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-09f);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			return false;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00002834 File Offset: 0x00000A34
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x04000963 RID: 2403
		public const byte N = 1;

		// Token: 0x04000964 RID: 2404
		public const byte E = 2;

		// Token: 0x04000965 RID: 2405
		public const byte S = 4;

		// Token: 0x04000966 RID: 2406
		public const byte W = 8;

		// Token: 0x04000967 RID: 2407
		public const int wood = 0;

		// Token: 0x04000968 RID: 2408
		public const int stone = 1;

		// Token: 0x04000969 RID: 2409
		public const int ghost = 2;

		// Token: 0x0400096A RID: 2410
		public const int iceTile = 3;

		// Token: 0x0400096B RID: 2411
		public const int straw = 4;

		// Token: 0x0400096C RID: 2412
		public const int gravel = 5;

		// Token: 0x0400096D RID: 2413
		public const int boardwalk = 6;

		// Token: 0x0400096E RID: 2414
		public const int colored_cobblestone = 7;

		// Token: 0x0400096F RID: 2415
		public const int cobblestone = 8;

		// Token: 0x04000970 RID: 2416
		public const int steppingStone = 9;

		// Token: 0x04000971 RID: 2417
		public static Texture2D floorsTexture;

		// Token: 0x04000972 RID: 2418
		public static Dictionary<byte, int> drawGuide;

		// Token: 0x04000973 RID: 2419
		public int whichFloor;

		// Token: 0x04000974 RID: 2420
		public int whichView;

		// Token: 0x04000975 RID: 2421
		private bool isPathway;

		// Token: 0x04000976 RID: 2422
		private bool isSteppingStone;
	}
}
