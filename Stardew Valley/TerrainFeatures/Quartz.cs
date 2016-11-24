using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000077 RID: 119
	public class Quartz : TerrainFeature
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x000CF238 File Offset: 0x000CD438
		public Quartz()
		{
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000D1703 File Offset: 0x000CF903
		public Quartz(int bigness, Color color)
		{
			this.loadSprite();
			this.health = (float)(10 - (3 - bigness) * 2);
			this.bigness = bigness;
			if (bigness >= 3)
			{
				this.bigness = 2;
			}
			this.color = color;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000D173C File Offset: 0x000CF93C
		public override void loadSprite()
		{
			try
			{
				this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Quartz");
			}
			catch (Exception)
			{
			}
			this.identifier = Game1.random.Next(-999999, 999999);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000D1790 File Offset: 0x000CF990
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			int arg_06_0 = this.bigness;
			return new Rectangle((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.Y * (float)Game1.tileSize), Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000D17C4 File Offset: 0x000CF9C4
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.glow > 0f)
			{
				this.glow -= 0.01f;
			}
			if (this.maxShake > 0f)
			{
				if (this.shakeLeft)
				{
					this.shakeRotation -= 0.0157079641f;
					if (this.shakeRotation <= -this.maxShake)
					{
						this.shakeLeft = false;
					}
				}
				else
				{
					this.shakeRotation += 0.0157079641f;
					if (this.shakeRotation >= this.maxShake)
					{
						this.shakeLeft = true;
					}
				}
			}
			if (this.maxShake > 0f)
			{
				this.maxShake = Math.Max(0f, this.maxShake - 0.00306796166f);
			}
			return this.health <= 0f;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000D1890 File Offset: 0x000CFA90
		public override void performPlayerEntryAction(Vector2 tileLocation)
		{
			Color glowColor = (this.glow > 0f) ? new Color((float)this.color.R + this.glow * 50f, (float)this.color.G + this.glow * 50f, (float)this.color.B + this.glow * 50f) : this.color;
			glowColor *= 0.3f + this.glow;
			if (this.bigness < 2)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), 1f, Utility.getOppositeColor(glowColor), (int)(tileLocation.X * 1000f + tileLocation.Y)));
				return;
			}
			if (this.bigness == 2)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), 1f, Utility.getOppositeColor(glowColor), (int)(tileLocation.X * 1000f + tileLocation.Y)));
				Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2)), 1f, Utility.getOppositeColor(glowColor), (int)(tileLocation.X * 1000f + tileLocation.Y)));
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000D1A50 File Offset: 0x000CFC50
		private void shake(Vector2 tileLocation)
		{
			if (this.maxShake == 0f)
			{
				this.shakeLeft = (Game1.player.getTileLocation().X > tileLocation.X || (Game1.player.getTileLocation().X == tileLocation.X && Game1.random.NextDouble() < 0.5));
				this.maxShake = 0.0245436933f;
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000D1AC8 File Offset: 0x000CFCC8
		public override bool performUseAction(Vector2 tileLocation)
		{
			if (Game1.soundBank != null)
			{
				Random arg_4F_0 = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)Game1.mine.mineLevel));
				Cue c = Game1.soundBank.GetCue("crystal");
				int pitch = arg_4F_0.Next(2400);
				pitch -= pitch % 100;
				c.SetVariable("Pitch", (float)pitch);
				c.Play();
			}
			this.glow = 0.7f;
			return false;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x000D1B50 File Offset: 0x000CFD50
		public override bool isPassable(Character c = null)
		{
			return this.health <= 0f;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00002834 File Offset: 0x00000A34
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x000D1B64 File Offset: 0x000CFD64
		private Rectangle getSourceRect(int size)
		{
			switch (size)
			{
			case 0:
				return new Rectangle(Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
			case 1:
				return new Rectangle(4 * Game1.tileSize + ((this.health <= 3f) ? Game1.tileSize : 0), Game1.tileSize, Game1.tileSize, Game1.tileSize);
			case 2:
				return new Rectangle((int)((8f - this.health) / 2f) * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 2);
			default:
				return Rectangle.Empty;
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x000D1C00 File Offset: 0x000CFE00
		public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
		{
			if (this.health > 0f)
			{
				float damage = 0f;
				if (t == null && explosion > 0)
				{
					damage = (float)explosion;
				}
				else if (t.name.Contains("Pickaxe"))
				{
					switch (t.upgradeLevel)
					{
					case 0:
						damage = 2f;
						break;
					case 1:
						damage = 2.5f;
						break;
					case 2:
						damage = 3.34f;
						break;
					case 3:
						damage = 5f;
						break;
					case 4:
						damage = 10f;
						break;
					}
					Game1.playSound("hammer");
				}
				if (damage > 0f)
				{
					this.glow = 0.7f;
					this.shake(tileLocation);
					this.health -= damage;
					if (this.health <= 0f)
					{
						Random crystalRandom = new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)Game1.mine.mineLevel + (float)Game1.player.timesReachedMineBottom));
						double luckModifier = 1.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel / 100.0 + (double)Game1.player.miningLevel / 50.0;
						if (crystalRandom.NextDouble() < 0.005 * luckModifier)
						{
							Game1.createObjectDebris(74, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						else if (crystalRandom.NextDouble() < 0.007 * luckModifier)
						{
							Game1.createDebris(10, (int)tileLocation.X, (int)tileLocation.Y, 2, null);
						}
						else if (crystalRandom.NextDouble() < 0.02 * luckModifier)
						{
							Game1.createObjectDebris(72, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						else if (crystalRandom.NextDouble() < 0.03 * luckModifier)
						{
							Game1.createObjectDebris((Game1.mine.mineLevel < 40) ? 86 : ((Game1.mine.mineLevel < 80) ? 84 : 82), (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						else if (crystalRandom.NextDouble() < 0.04 * luckModifier)
						{
							Game1.createObjectDebris(338, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
						}
						for (int i = 0; i < this.bigness * 3; i++)
						{
							int xPos = Game1.random.Next(this.getBoundingBox(tileLocation).X, this.getBoundingBox(tileLocation).Right);
							int yPos = Game1.random.Next(this.getBoundingBox(tileLocation).Y, this.getBoundingBox(tileLocation).Bottom);
							Game1.currentLocation.TemporarySprites.Add(new CosmeticDebris(this.texture, new Vector2((float)xPos, (float)yPos), (float)Game1.random.Next(-25, 25) / 100f, (float)(xPos - this.getBoundingBox(tileLocation).Center.X) / 30f, (float)Game1.random.Next(-800, -100) / 100f, (int)tileLocation.Y * Game1.tileSize + Game1.tileSize, new Rectangle(Game1.random.Next(4, 8) * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize), this.color, (Game1.soundBank != null) ? Game1.soundBank.GetCue("boulderCrack") : null, new LightSource(4, Vector2.Zero, 0.1f, Utility.getOppositeColor(this.color)), 24, 1000));
						}
						Utility.removeLightSource((int)(tileLocation.X * 1000f + tileLocation.Y));
					}
				}
			}
			return false;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x000D1FDC File Offset: 0x000D01DC
		private Vector2 getPivot()
		{
			switch (this.bigness)
			{
			case 1:
				return new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize);
			case 2:
				return new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 2));
			case 3:
				return new Vector2((float)Game1.tileSize, (float)(Game1.tileSize * 3));
			default:
				return Vector2.Zero;
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000D204C File Offset: 0x000D024C
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			if (this.health > 0f)
			{
				spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)this.getBoundingBox(tileLocation).Center.X, (float)this.getBoundingBox(tileLocation).Bottom)), new Rectangle?(this.getSourceRect(this.bigness)), this.color, this.shakeRotation, this.getPivot(), 1f, SpriteEffects.None, (tileLocation.Y * (float)Game1.tileSize + (float)Game1.tileSize) / 10000f);
			}
		}

		// Token: 0x040009FE RID: 2558
		public const float shakeRate = 0.0157079641f;

		// Token: 0x040009FF RID: 2559
		public const float shakeDecayRate = 0.00306796166f;

		// Token: 0x04000A00 RID: 2560
		public const double chanceForDiamond = 0.02;

		// Token: 0x04000A01 RID: 2561
		public const double chanceForPrismaticShard = 0.005;

		// Token: 0x04000A02 RID: 2562
		public const double chanceForIridium = 0.007;

		// Token: 0x04000A03 RID: 2563
		public const double chanceForLevelUnique = 0.03;

		// Token: 0x04000A04 RID: 2564
		public const double chanceForRefinedQuartz = 0.04;

		// Token: 0x04000A05 RID: 2565
		public const int startingHealth = 10;

		// Token: 0x04000A06 RID: 2566
		public const int large = 3;

		// Token: 0x04000A07 RID: 2567
		public const int medium = 2;

		// Token: 0x04000A08 RID: 2568
		public const int small = 1;

		// Token: 0x04000A09 RID: 2569
		public const int tiny = 0;

		// Token: 0x04000A0A RID: 2570
		public const int pointingLeft = 0;

		// Token: 0x04000A0B RID: 2571
		public const int pointingUp = 1;

		// Token: 0x04000A0C RID: 2572
		public const int pointingRight = 2;

		// Token: 0x04000A0D RID: 2573
		private Texture2D texture;

		// Token: 0x04000A0E RID: 2574
		public float health;

		// Token: 0x04000A0F RID: 2575
		public bool flipped;

		// Token: 0x04000A10 RID: 2576
		private bool shakeLeft;

		// Token: 0x04000A11 RID: 2577
		private bool falling;

		// Token: 0x04000A12 RID: 2578
		private float shakeRotation;

		// Token: 0x04000A13 RID: 2579
		private float maxShake;

		// Token: 0x04000A14 RID: 2580
		private float glow;

		// Token: 0x04000A15 RID: 2581
		public int bigness;

		// Token: 0x04000A16 RID: 2582
		private int identifier;

		// Token: 0x04000A17 RID: 2583
		private Color color;
	}
}
