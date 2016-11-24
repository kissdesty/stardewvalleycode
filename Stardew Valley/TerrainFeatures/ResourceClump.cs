using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000074 RID: 116
	public class ResourceClump : TerrainFeature
	{
		// Token: 0x060009AA RID: 2474 RVA: 0x000CF238 File Offset: 0x000CD438
		public ResourceClump()
		{
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x000CF240 File Offset: 0x000CD440
		public ResourceClump(int parentSheetIndex, int width, int height, Vector2 tile)
		{
			this.width = width;
			this.height = height;
			this.parentSheetIndex = parentSheetIndex;
			this.tile = tile;
			if (parentSheetIndex <= 602)
			{
				if (parentSheetIndex == 600)
				{
					this.health = 10f;
					return;
				}
				if (parentSheetIndex != 602)
				{
					return;
				}
				this.health = 20f;
				return;
			}
			else
			{
				if (parentSheetIndex != 622)
				{
					if (parentSheetIndex != 672)
					{
						switch (parentSheetIndex)
						{
						case 752:
						case 754:
						case 756:
						case 758:
							this.health = 8f;
							return;
						case 753:
						case 755:
						case 757:
							break;
						default:
							return;
						}
					}
					else
					{
						this.health = 10f;
					}
					return;
				}
				this.health = 20f;
				return;
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPassable(Character c = null)
		{
			return false;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000CF300 File Offset: 0x000CD500
		public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			if (t == null)
			{
				return false;
			}
			int radialDebris = 12;
			int num = this.parentSheetIndex;
			if (num <= 602)
			{
				if (num != 600)
				{
					if (num == 602)
					{
						if (t is Axe && t.upgradeLevel < 2)
						{
							Game1.playSound("axe");
							Game1.drawObjectDialogue("Your axe isn't strong enough to break this log.");
							Game1.player.jitterStrength = 1f;
							return false;
						}
						if (!(t is Axe))
						{
							return false;
						}
						Game1.playSound("axchop");
					}
				}
				else
				{
					if (t is Axe && t.upgradeLevel < 1)
					{
						Game1.playSound("axe");
						Game1.drawObjectDialogue("Your axe isn't strong enough to break this stump.");
						Game1.player.jitterStrength = 1f;
						return false;
					}
					if (!(t is Axe))
					{
						return false;
					}
					Game1.playSound("axchop");
				}
			}
			else if (num != 622)
			{
				if (num != 672)
				{
					switch (num)
					{
					case 752:
					case 754:
					case 756:
					case 758:
						if (!(t is Pickaxe))
						{
							return false;
						}
						Game1.playSound("hammer");
						radialDebris = 14;
						this.shakeTimer = 500f;
						break;
					}
				}
				else
				{
					if (t is Pickaxe && t.upgradeLevel < 2)
					{
						Game1.playSound("clubhit");
						Game1.playSound("clank");
						Game1.drawObjectDialogue("Your pickaxe isn't strong enough to break this boulder.");
						Game1.player.jitterStrength = 1f;
						return false;
					}
					if (!(t is Pickaxe))
					{
						return false;
					}
					Game1.playSound("hammer");
					radialDebris = 14;
				}
			}
			else
			{
				if (t is Pickaxe && t.upgradeLevel < 3)
				{
					Game1.playSound("clubhit");
					Game1.playSound("clank");
					Game1.drawObjectDialogue("Your pickaxe isn't strong enough to break this.");
					Game1.player.jitterStrength = 1f;
					return false;
				}
				if (!(t is Pickaxe))
				{
					return false;
				}
				Game1.playSound("hammer");
				radialDebris = 14;
			}
			float power = Math.Max(1f, (float)(t.upgradeLevel + 1) * 0.75f);
			this.health -= power;
			Game1.createRadialDebris(Game1.currentLocation, radialDebris, (int)tileLocation.X + Game1.random.Next(this.width / 2 + 1), (int)tileLocation.Y + Game1.random.Next(this.height / 2 + 1), Game1.random.Next(4, 9), false, -1, false, -1);
			if (this.health <= 0f)
			{
				if (Game1.IsMultiplayer)
				{
					Random arg_27C_0 = Game1.recentMultiplayerRandom;
				}
				else
				{
					new Random((int)(Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + Game1.stats.DaysPlayed + this.health));
				}
				num = this.parentSheetIndex;
				if (num <= 602)
				{
					if (num == 600 || num == 602)
					{
						t.getLastFarmerToUse().gainExperience(2, 25);
						int numChunks = (this.parentSheetIndex == 602) ? 8 : 2;
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
							Random arg_36D_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(709, (int)tileLocation.X, (int)tileLocation.Y, numChunks, t.getLastFarmerToUse().uniqueMultiplayerID);
						}
						else
						{
							Game1.createMultipleObjectDebris(709, (int)tileLocation.X, (int)tileLocation.Y, numChunks);
						}
						Game1.playSound("stumpCrack");
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(23, tileLocation * (float)Game1.tileSize, Color.White, 4, false, 140f, 0, Game1.tileSize * 2, -1f, Game1.tileSize * 2, 0));
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(385, 1522, 127, 79), 2000f, 1, 1, tileLocation * (float)Game1.tileSize + new Vector2(0f, 49f), false, false, 1E-05f, 0.016f, Color.White, 1f, 0f, 0f, 0f, false));
						Game1.createRadialDebris(Game1.currentLocation, 34, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(4, 9), false, -1, false, -1);
						return true;
					}
				}
				else
				{
					int numChunks;
					if (num != 622)
					{
						if (num != 672)
						{
							switch (num)
							{
							case 752:
							case 754:
							case 756:
							case 758:
								break;
							case 753:
							case 755:
							case 757:
								return false;
							default:
								return false;
							}
						}
						numChunks = ((this.parentSheetIndex == 672) ? 15 : 10);
						if (Game1.IsMultiplayer)
						{
							Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
							Random arg_514_0 = Game1.recentMultiplayerRandom;
						}
						else
						{
							new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
						}
						if (Game1.IsMultiplayer)
						{
							Game1.createMultipleObjectDebris(390, (int)tileLocation.X, (int)tileLocation.Y, numChunks, t.getLastFarmerToUse().uniqueMultiplayerID);
						}
						else
						{
							Game1.createRadialDebris(Game1.currentLocation, 390, (int)tileLocation.X, (int)tileLocation.Y, numChunks, false, -1, true, -1);
						}
						Game1.playSound("boulderBreak");
						Game1.createRadialDebris(Game1.currentLocation, 32, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(6, 12), false, -1, false, -1);
						Color c = Color.White;
						switch (this.parentSheetIndex)
						{
						case 752:
							c = new Color(188, 119, 98);
							break;
						case 754:
							c = new Color(168, 120, 95);
							break;
						case 756:
						case 758:
							c = new Color(67, 189, 238);
							break;
						}
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(48, tileLocation * (float)Game1.tileSize, c, 5, false, 180f, 0, Game1.tileSize * 2, -1f, Game1.tileSize * 2, 0)
						{
							alphaFade = 0.01f
						});
						return true;
					}
					numChunks = 6;
					if (Game1.IsMultiplayer)
					{
						Game1.recentMultiplayerRandom = new Random((int)tileLocation.X * 1000 + (int)tileLocation.Y);
						Random arg_6B0_0 = Game1.recentMultiplayerRandom;
					}
					else
					{
						new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + (int)tileLocation.X * 7 + (int)tileLocation.Y * 11);
					}
					if (Game1.IsMultiplayer)
					{
						Game1.createMultipleObjectDebris(386, (int)tileLocation.X, (int)tileLocation.Y, numChunks, t.getLastFarmerToUse().uniqueMultiplayerID);
						Game1.createMultipleObjectDebris(390, (int)tileLocation.X, (int)tileLocation.Y, numChunks, t.getLastFarmerToUse().uniqueMultiplayerID);
						Game1.createMultipleObjectDebris(535, (int)tileLocation.X, (int)tileLocation.Y, 2, t.getLastFarmerToUse().uniqueMultiplayerID);
					}
					else
					{
						Game1.createMultipleObjectDebris(386, (int)tileLocation.X, (int)tileLocation.Y, numChunks);
						Game1.createMultipleObjectDebris(390, (int)tileLocation.X, (int)tileLocation.Y, numChunks);
						Game1.createMultipleObjectDebris(535, (int)tileLocation.X, (int)tileLocation.Y, 2);
					}
					Game1.playSound("boulderBreak");
					Game1.createRadialDebris(Game1.currentLocation, 32, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(6, 12), false, -1, false, -1);
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 0f)) * (float)Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 1f)) * (float)Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(0f, 1f)) * (float)Game1.tileSize, Color.White, 8, false, 90f, 0, -1, -1f, -1, 0));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), Color.White, 8, false, 70f, 0, -1, -1f, -1, 0));
					return true;
				}
			}
			else
			{
				this.shakeTimer = 100f;
			}
			return false;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x000CFC53 File Offset: 0x000CDE53
		public override Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, this.width * Game1.tileSize, this.height * Game1.tileSize);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000CFC8C File Offset: 0x000CDE8C
		public bool occupiesTile(int x, int y)
		{
			return (float)x >= this.tile.X && (float)x - this.tile.X < (float)this.width && (float)y >= this.tile.Y && (float)y - this.tile.Y < (float)this.height;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000CFCE8 File Offset: 0x000CDEE8
		public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
			Rectangle sourceRect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.parentSheetIndex, 16, 16);
			sourceRect.Width = this.width * 16;
			sourceRect.Height = this.height * 16;
			Vector2 position = this.tile * (float)Game1.tileSize;
			if (this.shakeTimer > 0f)
			{
				position.X += (float)Math.Sin(6.2831853071795862 / (double)this.shakeTimer) * 4f;
			}
			spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, position), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.tile.Y + 1f) * (float)Game1.tileSize / 10000f + this.tile.X / 100000f);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00002834 File Offset: 0x00000A34
		public override void loadSprite()
		{
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000CFDD4 File Offset: 0x000CDFD4
		public override bool performUseAction(Vector2 tileLocation)
		{
			int num = this.parentSheetIndex;
			if (num != 602)
			{
				if (num != 622)
				{
					if (num == 672)
					{
						Game1.drawObjectDialogue(Game1.parseText("Looks like you'll need an upgraded pickaxe to destroy this dense stone."));
					}
				}
				else
				{
					Game1.drawObjectDialogue(Game1.parseText("It looks pretty solid... maybe a very strong pickaxe could break it?"));
				}
			}
			else
			{
				Game1.drawObjectDialogue(Game1.parseText("Looks like you'll need an upgraded axe to destroy this gnarled old log."));
			}
			return true;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000CFE34 File Offset: 0x000CE034
		public override bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			if (this.shakeTimer > 0f)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			}
			return false;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00002834 File Offset: 0x00000A34
		public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x040009BD RID: 2493
		public const int stumpIndex = 600;

		// Token: 0x040009BE RID: 2494
		public const int hollowLogIndex = 602;

		// Token: 0x040009BF RID: 2495
		public const int meteoriteIndex = 622;

		// Token: 0x040009C0 RID: 2496
		public const int boulderIndex = 672;

		// Token: 0x040009C1 RID: 2497
		public const int mineRock1Index = 752;

		// Token: 0x040009C2 RID: 2498
		public const int mineRock2Index = 754;

		// Token: 0x040009C3 RID: 2499
		public const int mineRock3Index = 756;

		// Token: 0x040009C4 RID: 2500
		public const int mineRock4Index = 758;

		// Token: 0x040009C5 RID: 2501
		public int width;

		// Token: 0x040009C6 RID: 2502
		public int height;

		// Token: 0x040009C7 RID: 2503
		public int parentSheetIndex;

		// Token: 0x040009C8 RID: 2504
		public float health;

		// Token: 0x040009C9 RID: 2505
		public Vector2 tile;

		// Token: 0x040009CA RID: 2506
		protected float shakeTimer;
	}
}
