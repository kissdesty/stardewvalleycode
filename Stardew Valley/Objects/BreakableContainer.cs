using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Tools;

namespace StardewValley.Objects
{
	// Token: 0x0200008C RID: 140
	public class BreakableContainer : Object
	{
		// Token: 0x06000A5A RID: 2650 RVA: 0x000D9F75 File Offset: 0x000D8175
		public BreakableContainer()
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x000D9F80 File Offset: 0x000D8180
		public BreakableContainer(Vector2 tile, int type) : base(tile, BreakableContainer.typeToIndex(type), false)
		{
			this.type = type;
			if (type == 118)
			{
				if (Game1.mine.getMineArea(-1) == 40)
				{
					this.parentSheetIndex = 120;
					this.type = 120;
				}
				if (Game1.mine.getMineArea(-1) == 80)
				{
					this.parentSheetIndex = 122;
					this.type = 122;
				}
				if (Game1.mine.getMineArea(-1) == 121)
				{
					this.parentSheetIndex = 124;
					this.type = 124;
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					this.parentSheetIndex++;
				}
				this.health = 3;
				this.debris = 12;
				this.hitSound = "woodWhack";
				this.breakSound = "barrelBreak";
				this.breakDebrisSource = new Rectangle(598, 1275, 13, 4);
				this.breakDebrisSource2 = new Rectangle(611, 1275, 10, 4);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000DA07F File Offset: 0x000D827F
		public static int typeToIndex(int type)
		{
			if (type == 118)
			{
				return type;
			}
			if (type != 120)
			{
				return 0;
			}
			return type;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000DA094 File Offset: 0x000D8294
		public override bool performToolAction(Tool t)
		{
			if (t.isHeavyHitter())
			{
				this.health--;
				if (t is MeleeWeapon && (t as MeleeWeapon).type == 2)
				{
					this.health--;
				}
				if (this.health <= 0)
				{
					if (this.breakSound != null)
					{
						Game1.playSound(this.breakSound);
					}
					this.releaseContents(t.getLastFarmerToUse().currentLocation, t.getLastFarmerToUse());
					t.getLastFarmerToUse().currentLocation.objects.Remove(this.tileLocation);
					int numDebris = Game1.random.Next(4, 12);
					Color c = (this.type == 120) ? Color.White : ((this.type == 122) ? new Color(109, 122, 80) : ((this.type == 124) ? new Color(229, 171, 84) : new Color(130, 80, 30)));
					for (int i = 0; i < numDebris; i++)
					{
						t.getLastFarmerToUse().currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, (Game1.random.NextDouble() < 0.5) ? this.breakDebrisSource : this.breakDebrisSource2, 999f, 1, 0, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, (this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) / 10000f, 0.01f, c, (float)Game1.pixelZoom, 0f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 8f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 64f, false)
						{
							motion = new Vector2((float)Game1.random.Next(-30, 31) / 10f, (float)Game1.random.Next(-10, -7)),
							acceleration = new Vector2(0f, 0.3f)
						});
					}
				}
				else if (this.hitSound != null)
				{
					this.shakeTimer = 300;
					Game1.playSound(this.hitSound);
					Game1.createRadialDebris(t.getLastFarmerToUse().currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(4, 7), false, -1, false, (this.type == 120) ? 10000 : -1);
				}
			}
			return false;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000DA348 File Offset: 0x000D8548
		public override bool onExplosion(Farmer who, GameLocation location)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			this.releaseContents(location, who);
			int numDebris = Game1.random.Next(4, 12);
			Color c = (this.type == 120) ? Color.White : ((this.type == 122) ? new Color(109, 122, 80) : new Color(130, 80, 30));
			for (int i = 0; i < numDebris; i++)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, (Game1.random.NextDouble() < 0.5) ? this.breakDebrisSource : this.breakDebrisSource2, 999f, 1, 0, this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, (this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)) / 10000f, 0.01f, c, (float)Game1.pixelZoom, 0f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 8f, (float)Game1.random.Next(-5, 6) * 3.14159274f / 64f, false)
				{
					motion = new Vector2((float)Game1.random.Next(-30, 31) / 10f, (float)Game1.random.Next(-10, -7)),
					acceleration = new Vector2(0f, 0.3f)
				});
			}
			return true;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000DA4EC File Offset: 0x000D86EC
		public void releaseContents(GameLocation location, Farmer who)
		{
			Random r = new Random((int)this.tileLocation.X + (int)this.tileLocation.Y * 10000 + (int)Game1.stats.DaysPlayed);
			int x = (int)this.tileLocation.X;
			int y = (int)this.tileLocation.Y;
			int mineLevel = -1;
			if (location is MineShaft)
			{
				mineLevel = ((MineShaft)location).mineLevel;
				if (((MineShaft)location).isContainerPlatform(x, y))
				{
					((MineShaft)location).updateMineLevelData(0, -1);
				}
			}
			if (r.NextDouble() < 0.2)
			{
				return;
			}
			switch (this.type)
			{
			case 118:
				if (r.NextDouble() < 0.65)
				{
					if (r.NextDouble() < 0.8)
					{
						switch (r.Next(8))
						{
						case 0:
							Game1.createMultipleObjectDebris(382, x, y, r.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(378, x, y, r.Next(1, 4));
							return;
						case 2:
							break;
						case 3:
							Game1.createMultipleObjectDebris(390, x, y, r.Next(2, 6));
							return;
						case 4:
							Game1.createMultipleObjectDebris(388, x, y, r.Next(2, 3));
							return;
						case 5:
							Game1.createMultipleObjectDebris(92, x, y, r.Next(2, 4));
							return;
						case 6:
							Game1.createMultipleObjectDebris(388, x, y, r.Next(2, 6));
							return;
						case 7:
							Game1.createMultipleObjectDebris(390, x, y, r.Next(2, 6));
							return;
						default:
							return;
						}
					}
					else
					{
						switch (r.Next(4))
						{
						case 0:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 2:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 3:
							Game1.createMultipleObjectDebris(535, x, y, r.Next(1, 3));
							return;
						default:
							return;
						}
					}
				}
				else if (r.NextDouble() < 0.4)
				{
					switch (r.Next(5))
					{
					case 0:
						Game1.createMultipleObjectDebris(66, x, y, 1);
						return;
					case 1:
						Game1.createMultipleObjectDebris(68, x, y, 1);
						return;
					case 2:
						Game1.createMultipleObjectDebris(709, x, y, 1);
						return;
					case 3:
						Game1.createMultipleObjectDebris(535, x, y, 1);
						return;
					case 4:
						Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(mineLevel, x, y), new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), r.Next(4), null);
						return;
					default:
						return;
					}
				}
				break;
			case 119:
			case 121:
			case 123:
				break;
			case 120:
				if (r.NextDouble() < 0.65)
				{
					if (r.NextDouble() < 0.8)
					{
						switch (r.Next(8))
						{
						case 0:
							Game1.createMultipleObjectDebris(382, x, y, r.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(380, x, y, r.Next(1, 4));
							return;
						case 2:
							break;
						case 3:
							Game1.createMultipleObjectDebris(378, x, y, r.Next(2, 6));
							return;
						case 4:
							Game1.createMultipleObjectDebris(388, x, y, r.Next(2, 6));
							return;
						case 5:
							Game1.createMultipleObjectDebris(92, x, y, r.Next(2, 4));
							return;
						case 6:
							Game1.createMultipleObjectDebris(390, x, y, r.Next(2, 4));
							return;
						case 7:
							Game1.createMultipleObjectDebris(390, x, y, r.Next(2, 6));
							return;
						default:
							return;
						}
					}
					else
					{
						switch (r.Next(4))
						{
						case 0:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(536, x, y, r.Next(1, 3));
							return;
						case 2:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 3:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						default:
							return;
						}
					}
				}
				else if (r.NextDouble() < 4.0)
				{
					switch (r.Next(5))
					{
					case 0:
						Game1.createMultipleObjectDebris(62, x, y, 1);
						return;
					case 1:
						Game1.createMultipleObjectDebris(70, x, y, 1);
						return;
					case 2:
						Game1.createMultipleObjectDebris(709, x, y, r.Next(1, 4));
						return;
					case 3:
						Game1.createMultipleObjectDebris(536, x, y, 1);
						return;
					case 4:
						Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(mineLevel, x, y), new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), r.Next(4), null);
						return;
					default:
						return;
					}
				}
				break;
			case 122:
			case 124:
				if (r.NextDouble() < 0.65)
				{
					if (r.NextDouble() < 0.8)
					{
						switch (r.Next(8))
						{
						case 0:
							Game1.createMultipleObjectDebris(382, x, y, r.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(384, x, y, r.Next(1, 4));
							return;
						case 2:
							break;
						case 3:
							Game1.createMultipleObjectDebris(380, x, y, r.Next(2, 6));
							return;
						case 4:
							Game1.createMultipleObjectDebris(378, x, y, r.Next(2, 6));
							return;
						case 5:
							Game1.createMultipleObjectDebris(390, x, y, r.Next(2, 6));
							return;
						case 6:
							Game1.createMultipleObjectDebris(388, x, y, r.Next(2, 6));
							return;
						case 7:
							Game1.createMultipleObjectDebris(92, x, y, r.Next(2, 6));
							return;
						default:
							return;
						}
					}
					else
					{
						switch (r.Next(4))
						{
						case 0:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 1:
							Game1.createMultipleObjectDebris(537, x, y, r.Next(1, 3));
							return;
						case 2:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						case 3:
							Game1.createMultipleObjectDebris(78, x, y, r.Next(1, 3));
							return;
						default:
							return;
						}
					}
				}
				else if (r.NextDouble() < 4.0)
				{
					switch (r.Next(5))
					{
					case 0:
						Game1.createMultipleObjectDebris(60, x, y, 1);
						return;
					case 1:
						Game1.createMultipleObjectDebris(64, x, y, 1);
						return;
					case 2:
						Game1.createMultipleObjectDebris(709, x, y, r.Next(1, 4));
						return;
					case 3:
						Game1.createMultipleObjectDebris(749, x, y, 1);
						return;
					case 4:
						Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(mineLevel, x, y), new Vector2((float)x, (float)y) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), r.Next(4), null);
						break;
					default:
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000DABD0 File Offset: 0x000D8DD0
		public override void updateWhenCurrentLocation(GameTime time)
		{
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000DAC04 File Offset: 0x000D8E04
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			Vector2 scaleFactor = base.getScale();
			scaleFactor *= (float)Game1.pixelZoom;
			Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
			Rectangle destination = new Rectangle((int)(position.X - scaleFactor.X / 2f), (int)(position.Y - scaleFactor.Y / 2f), (int)((float)Game1.tileSize + scaleFactor.X), (int)((float)(Game1.tileSize * 2) + scaleFactor.Y / 2f));
			if (this.shakeTimer > 0)
			{
				int intensity = this.shakeTimer / 100 + 1;
				destination.X += Game1.random.Next(-intensity, intensity + 1);
				destination.Y += Game1.random.Next(-intensity, intensity + 1);
			}
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, destination, new Rectangle?(Object.getSourceRectForBigCraftable(this.showNextIndex ? (base.ParentSheetIndex + 1) : base.ParentSheetIndex)), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 1) / 10000f) + ((this.parentSheetIndex == 105) ? 0.0015f : 0f));
		}

		// Token: 0x04000ACA RID: 2762
		public const int barrel = 118;

		// Token: 0x04000ACB RID: 2763
		public const int frostBarrel = 120;

		// Token: 0x04000ACC RID: 2764
		public const int darkBarrel = 122;

		// Token: 0x04000ACD RID: 2765
		public const int desertBarrel = 124;

		// Token: 0x04000ACE RID: 2766
		private int debris;

		// Token: 0x04000ACF RID: 2767
		private new int shakeTimer;

		// Token: 0x04000AD0 RID: 2768
		private new int health;

		// Token: 0x04000AD1 RID: 2769
		private new int type;

		// Token: 0x04000AD2 RID: 2770
		private string hitSound;

		// Token: 0x04000AD3 RID: 2771
		private string breakSound;

		// Token: 0x04000AD4 RID: 2772
		private Rectangle breakDebrisSource;

		// Token: 0x04000AD5 RID: 2773
		private Rectangle breakDebrisSource2;
	}
}
