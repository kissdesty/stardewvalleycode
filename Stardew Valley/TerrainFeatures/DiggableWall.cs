using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200006E RID: 110
	public class DiggableWall : TerrainFeature
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x000CA56C File Offset: 0x000C876C
		public DiggableWall()
		{
			this.loadSprite();
			this.health = 3;
			if (DiggableWall.fenceDrawGuide == null)
			{
				DiggableWall.populateFenceDrawGuide();
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000CA590 File Offset: 0x000C8790
		public static void populateFenceDrawGuide()
		{
			DiggableWall.fenceDrawGuide = new Dictionary<int, int>();
			DiggableWall.fenceDrawGuide.Add(0, 0);
			DiggableWall.fenceDrawGuide.Add(10, 15);
			DiggableWall.fenceDrawGuide.Add(100, 13);
			DiggableWall.fenceDrawGuide.Add(1000, 12);
			DiggableWall.fenceDrawGuide.Add(500, 4);
			DiggableWall.fenceDrawGuide.Add(1010, 11);
			DiggableWall.fenceDrawGuide.Add(1100, 9);
			DiggableWall.fenceDrawGuide.Add(1500, 8);
			DiggableWall.fenceDrawGuide.Add(600, 1);
			DiggableWall.fenceDrawGuide.Add(510, 3);
			DiggableWall.fenceDrawGuide.Add(110, 14);
			DiggableWall.fenceDrawGuide.Add(1600, 5);
			DiggableWall.fenceDrawGuide.Add(1610, 6);
			DiggableWall.fenceDrawGuide.Add(1510, 7);
			DiggableWall.fenceDrawGuide.Add(1110, 10);
			DiggableWall.fenceDrawGuide.Add(610, 2);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000CA6A4 File Offset: 0x000C88A4
		public override void loadSprite()
		{
			try
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\DiggableWall");
			}
			catch (Exception)
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\DiggableWall");
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000CA6F0 File Offset: 0x000C88F0
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			if (this.health > 0)
			{
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			}
			return Rectangle.Empty;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			return false;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x000CA72A File Offset: 0x000C892A
		public override bool isPassable(Character c = null)
		{
			return this.health <= -99;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00002834 File Offset: 0x00000A34
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x000CA73C File Offset: 0x000C893C
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (this.health <= -99)
			{
				return false;
			}
			if (t != null && t.name.Contains("Pickaxe"))
			{
				Game1.playSound("hammer");
				Game1.currentLocation.debris.Add(new Debris(this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), Game1.player.GetToolLocation(false) + new Vector2((float)(Game1.tileSize / 4), 0f)));
			}
			else if (explosion <= 0)
			{
				return false;
			}
			int damage = 1;
			if (explosion > 0)
			{
				damage = explosion;
			}
			else
			{
				if (t == null)
				{
					return false;
				}
				switch (t.upgradeLevel)
				{
				case 0:
					damage = 1;
					break;
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
			}
			this.health -= damage;
			return this.health <= 0;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00002834 File Offset: 0x00000A34
		public override void draw(SpriteBatch b, Vector2 tileLocation)
		{
		}

		// Token: 0x0400095B RID: 2395
		public const int startingHealth = 3;

		// Token: 0x0400095C RID: 2396
		public const int N = 1000;

		// Token: 0x0400095D RID: 2397
		public const int E = 100;

		// Token: 0x0400095E RID: 2398
		public const int S = 500;

		// Token: 0x0400095F RID: 2399
		public const int W = 10;

		// Token: 0x04000960 RID: 2400
		private Texture2D texture;

		// Token: 0x04000961 RID: 2401
		private int health;

		// Token: 0x04000962 RID: 2402
		public static Dictionary<int, int> fenceDrawGuide;
	}
}
