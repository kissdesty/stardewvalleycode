using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000075 RID: 117
	public class Stalagmite : TerrainFeature
	{
		// Token: 0x060009B6 RID: 2486 RVA: 0x000CFE6B File Offset: 0x000CE06B
		public Stalagmite()
		{
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x000CFE7E File Offset: 0x000CE07E
		public Stalagmite(bool tall)
		{
			this.loadSprite();
			this.health = 10f;
			this.tall = tall;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x000CFEAC File Offset: 0x000CE0AC
		public override void loadSprite()
		{
			try
			{
				string nonDefault = (Game1.mine.mineLevel >= 40 && Game1.mine.mineLevel < 80) ? "_Frost" : ((Game1.mine.mineLevel < 40) ? "" : "_Lava");
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Stalagmite" + nonDefault);
			}
			catch (Exception)
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Stalagmite");
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x000CFF3C File Offset: 0x000CE13C
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			if (this.health > 0f)
			{
				return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			}
			return Rectangle.Empty;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000CFF7A File Offset: 0x000CE17A
		public override bool performUseAction(Vector2 tileLocation)
		{
			this.shake(tileLocation);
			return false;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000CFF84 File Offset: 0x000CE184
		private int extraStoneCalculator(Vector2 tileLocation)
		{
			Random arg_2D_0 = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
			int extraWood = 0;
			if (arg_2D_0.NextDouble() < Game1.dailyLuck)
			{
				extraWood++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.MiningLevel / 12.5)
			{
				extraWood++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.MiningLevel / 12.5)
			{
				extraWood++;
			}
			if (arg_2D_0.NextDouble() < (double)Game1.player.LuckLevel / 25.0)
			{
				extraWood++;
			}
			return extraWood;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x000D0034 File Offset: 0x000CE234
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (!this.falling)
			{
				if (this.maxShake > 0f)
				{
					if (this.shakeLeft)
					{
						this.shakeRotation -= 0.0122718466f;
						if (this.shakeRotation <= -this.maxShake)
						{
							this.shakeLeft = false;
						}
					}
					else
					{
						this.shakeRotation += 0.0122718466f;
						if (this.shakeRotation >= this.maxShake)
						{
							this.shakeLeft = true;
						}
					}
				}
				if (this.maxShake > 0f)
				{
					this.maxShake = Math.Max(0f, this.maxShake - 0.00613592332f);
				}
				if (this.drop)
				{
					this.dropY += 10f;
					if (this.dropY >= tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 2))
					{
						this.drop = false;
						Game1.playSound("cavedrip");
						Game1.createWaterDroplets(this.texture, new Rectangle(Game1.tileSize, 0, 4, 4), (int)tileLocation.X * Game1.tileSize + Game1.tileSize, (int)(tileLocation.Y - 2f) * Game1.tileSize, Game1.random.Next(4, 5), (int)(tileLocation.Y + 1f));
					}
				}
				if (!this.drop && Game1.random.NextDouble() < 0.005)
				{
					this.drop = true;
					this.dropY = tileLocation.Y * (float)Game1.tileSize - (float)Game1.viewport.Height;
				}
			}
			else
			{
				this.shakeRotation += (this.shakeLeft ? (-(this.maxShake * this.maxShake)) : (this.maxShake * this.maxShake));
				this.maxShake += 0.00204530777f;
				if ((double)Math.Abs(this.shakeRotation) > 1.5707963267948966)
				{
					this.falling = false;
					this.maxShake = 0f;
					Game1.playSound("stoneCrack");
					Game1.createRadialDebris(Game1.currentLocation, 14, (int)tileLocation.X + (this.shakeLeft ? -2 : 2), (int)tileLocation.Y, 8 + this.extraStoneCalculator(tileLocation), true, -1, false, -1);
					Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4), (int)tileLocation.X + (this.shakeLeft ? -2 : 2), (int)tileLocation.Y, Game1.random.Next(40, 60));
					if (this.health <= 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000D02E4 File Offset: 0x000CE4E4
		private void shake(Vector2 tileLocation)
		{
			if (this.maxShake == 0f && !this.stump)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = 0.0490873866f;
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000D0361 File Offset: 0x000CE561
		public override bool isPassable(Character c = null)
		{
			return this.health <= -99f;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00002834 File Offset: 0x00000A34
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000D0374 File Offset: 0x000CE574
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (this.health <= -99f)
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
			this.shake(tileLocation);
			float damage = 1f;
			if (explosion > 0)
			{
				damage = (float)explosion;
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
					damage = 1f;
					break;
				case 1:
					damage = 1.25f;
					break;
				case 2:
					damage = 1.67f;
					break;
				case 3:
					damage = 2.5f;
					break;
				case 4:
					damage = 5f;
					break;
				}
			}
			this.health -= damage;
			if (this.health <= 0f)
			{
				if (!this.stump)
				{
					Game1.playSound("treecrack");
					this.maxShake = 0f;
					this.stump = true;
					this.health = 1f;
					this.falling = true;
					this.shakeLeft = (Game1.player.getTileLocation().X >= tileLocation.X && (Game1.player.getTileLocation().X != tileLocation.X || Game1.random.NextDouble() >= 0.5));
				}
				else
				{
					this.health = -100f;
					Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(2 * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 7, Game1.tileSize / 2, Game1.tileSize / 2), (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(30, 40));
					Game1.createRadialDebris(Game1.currentLocation, 14, (int)tileLocation.X, (int)tileLocation.Y, 1, true, -1, false, -1);
					if (!this.falling)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000D05CC File Offset: 0x000CE7CC
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (!this.stump || this.falling)
			{
				if (this.tall)
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(Game1.tileSize, Game1.tileSize, Game1.tileSize, Game1.tileSize * 3)), Color.White, this.shakeRotation, new Vector2((float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), (float)(Game1.tileSize * 2)), 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 1E-06f);
				}
				else
				{
					spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(0, 0, Game1.tileSize, Game1.tileSize * 3)), Color.White, this.shakeRotation, new Vector2((float)((this.falling && this.shakeLeft) ? (Game1.tileSize / 4) : (this.falling ? (Game1.tileSize * 3 / 4) : (Game1.tileSize / 2))), (float)(Game1.tileSize * 2)), 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f + 1E-06f);
				}
			}
			if (this.health > 0f || !this.falling)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float)Game1.tileSize, tileLocation.Y * (float)Game1.tileSize)), new Rectangle?(new Rectangle(0, 3 * Game1.tileSize, Game1.tileSize, Game1.tileSize)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.getBoundingBox(tileLocation).Bottom / 10000f);
			}
			if (this.drop)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)Game1.tileSize * tileLocation.X + (float)(Game1.tileSize / 2), this.dropY)), new Rectangle?(new Rectangle(Game1.tileSize, 0, 4, 8)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
			}
		}

		// Token: 0x040009CB RID: 2507
		public const float shakeRate = 0.0122718466f;

		// Token: 0x040009CC RID: 2508
		public const float shakeDecayRate = 0.00613592332f;

		// Token: 0x040009CD RID: 2509
		public const int minWoodDebrisForFallenTree = 8;

		// Token: 0x040009CE RID: 2510
		public const int minWoodDebrisForStump = 4;

		// Token: 0x040009CF RID: 2511
		public const int startingHealth = 10;

		// Token: 0x040009D0 RID: 2512
		public const int leafFallRate = 3;

		// Token: 0x040009D1 RID: 2513
		public const int bushyTree = 1;

		// Token: 0x040009D2 RID: 2514
		public const int leafyTree = 2;

		// Token: 0x040009D3 RID: 2515
		public const int pineTree = 3;

		// Token: 0x040009D4 RID: 2516
		public const int winterTree1 = 4;

		// Token: 0x040009D5 RID: 2517
		public const int winterTree2 = 5;

		// Token: 0x040009D6 RID: 2518
		public const int palmTree = 6;

		// Token: 0x040009D7 RID: 2519
		public const int seedStage = 0;

		// Token: 0x040009D8 RID: 2520
		public const int sproutStage = 1;

		// Token: 0x040009D9 RID: 2521
		public const int saplingStage = 2;

		// Token: 0x040009DA RID: 2522
		public const int bushStage = 3;

		// Token: 0x040009DB RID: 2523
		public const int treeStage = 5;

		// Token: 0x040009DC RID: 2524
		private Texture2D texture;

		// Token: 0x040009DD RID: 2525
		public float health;

		// Token: 0x040009DE RID: 2526
		public bool stump;

		// Token: 0x040009DF RID: 2527
		private bool shakeLeft;

		// Token: 0x040009E0 RID: 2528
		private bool falling;

		// Token: 0x040009E1 RID: 2529
		private bool tall;

		// Token: 0x040009E2 RID: 2530
		private bool drop;

		// Token: 0x040009E3 RID: 2531
		private float shakeRotation;

		// Token: 0x040009E4 RID: 2532
		private float maxShake;

		// Token: 0x040009E5 RID: 2533
		private float dropY;

		// Token: 0x040009E6 RID: 2534
		private List<Leaf> leaves = new List<Leaf>();
	}
}
