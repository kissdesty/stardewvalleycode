using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000160 RID: 352
	public class TrainCar
	{
		// Token: 0x0600134C RID: 4940 RVA: 0x001842CC File Offset: 0x001824CC
		public TrainCar(Random random, int carType, int frontDecal, Color color, int resourceType = 0, int loaded = 0)
		{
			this.carType = carType;
			this.frontDecal = frontDecal;
			this.color = color;
			this.resourceType = resourceType;
			this.loaded = loaded;
			if (carType != 0 && carType != 1)
			{
				this.color = Color.White;
			}
			if (carType == 0 && !color.Equals(Color.DimGray))
			{
				for (int i = 0; i < this.topFeatures.Length; i++)
				{
					if (random.NextDouble() < 0.2)
					{
						this.topFeatures[i] = random.Next(2);
					}
					else
					{
						this.topFeatures[i] = -1;
					}
				}
			}
			if (carType == 2 && random.NextDouble() < 0.5)
			{
				this.alternateCar = true;
			}
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00184390 File Offset: 0x00182590
		public void draw(SpriteBatch b, Vector2 globalPosition, float wheelRotation)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(192 + this.carType * 128, 512 - (this.alternateCar ? 64 : 0), 128, 57)), this.color, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 256f) / 10000f);
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(0f, 228f)), new Rectangle?(new Rectangle(192 + this.carType * 128, 569, 128, 7)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 256f) / 10000f);
			if (this.carType == 1)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(448 + this.resourceType * 128 % 256, 576 + this.resourceType / 2 * 32, 128, 32)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				if (this.loaded > 0 && Game1.random.NextDouble() < 0.003 && globalPosition.X > (float)(Game1.tileSize * 4) && globalPosition.X < (float)(Game1.currentLocation.map.DisplayWidth - Game1.tileSize * 4))
				{
					this.loaded--;
					int debrisType = -1;
					switch (this.resourceType)
					{
					case 0:
						debrisType = 382;
						break;
					case 1:
						debrisType = ((this.color.R > this.color.G) ? 378 : ((this.color.G > this.color.B) ? 380 : ((this.color.B > this.color.R) ? 384 : 378)));
						break;
					case 2:
						debrisType = 388;
						break;
					case 6:
						debrisType = 390;
						break;
					case 7:
						debrisType = (Game1.currentSeason.Equals("winter") ? 536 : ((Game1.stats.DaysPlayed > 120u && this.color.R > this.color.G) ? 537 : 535));
						break;
					}
					if (debrisType != -1)
					{
						Game1.createObjectDebris(debrisType, (int)globalPosition.X / Game1.tileSize, (int)globalPosition.Y / Game1.tileSize, (int)(globalPosition.Y + (float)(Game1.tileSize * 5)), 0, 1f, null);
					}
				}
			}
			if (this.carType == 0)
			{
				for (int i = 0; i < this.topFeatures.Length; i += Game1.tileSize)
				{
					if (this.topFeatures[i] != -1)
					{
						b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2((float)(Game1.tileSize + i), 20f)), new Rectangle?(new Rectangle(192, 608 + this.topFeatures[i] * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
					}
				}
			}
			if (this.frontDecal != -1 && (this.carType == 0 || this.carType == 1))
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(192f, 92f)), new Rectangle?(new Rectangle(224 + this.frontDecal * 32 % 224, 576 + this.frontDecal * 32 / 224 * 32, 32, 32)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
			}
			if (this.carType == 3)
			{
				Vector2 backWheel = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(72f, 208f));
				Vector2 frontWheel = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(316f, 208f));
				b.Draw(Game1.mouseCursors, backWheel, new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(228f, 208f)), new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				b.Draw(Game1.mouseCursors, frontWheel, new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				int startX = (int)((double)(backWheel.X + 4f) + 24.0 * Math.Cos((double)wheelRotation));
				int startY = (int)((double)(backWheel.Y + 4f) + 24.0 * Math.Sin((double)wheelRotation));
				int endX = (int)((double)(frontWheel.X + 4f) + 24.0 * Math.Cos((double)wheelRotation));
				int endY = (int)((double)(frontWheel.Y + 4f) + 24.0 * Math.Sin((double)wheelRotation));
				Utility.drawLineWithScreenCoordinates(startX, startY, endX, endY, b, new Color(112, 98, 92), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(startX, startY + 2, endX, endY + 2, b, new Color(112, 98, 92), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(startX, startY + 4, endX, endY + 4, b, new Color(53, 46, 43), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(startX, startY + 6, endX, endY + 6, b, new Color(53, 46, 43), (globalPosition.Y + 264f) / 10000f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(startX - 8), (float)(startY - 8)), new Rectangle?(new Rectangle(192, 640, 24, 24)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 268f) / 10000f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(endX - 8), (float)(endY - 8)), new Rectangle?(new Rectangle(192, 640, 24, 24)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 268f) / 10000f);
			}
		}

		// Token: 0x040013FB RID: 5115
		public const int spotsForTopFeatures = 6;

		// Token: 0x040013FC RID: 5116
		public const double chanceForTopFeature = 0.2;

		// Token: 0x040013FD RID: 5117
		public const int engine = 3;

		// Token: 0x040013FE RID: 5118
		public const int passengerCar = 2;

		// Token: 0x040013FF RID: 5119
		public const int coalCar = 1;

		// Token: 0x04001400 RID: 5120
		public const int plainCar = 0;

		// Token: 0x04001401 RID: 5121
		public const int coal = 0;

		// Token: 0x04001402 RID: 5122
		public const int metal = 1;

		// Token: 0x04001403 RID: 5123
		public const int wood = 2;

		// Token: 0x04001404 RID: 5124
		public const int compartments = 3;

		// Token: 0x04001405 RID: 5125
		public const int grass = 4;

		// Token: 0x04001406 RID: 5126
		public const int hay = 5;

		// Token: 0x04001407 RID: 5127
		public const int bricks = 6;

		// Token: 0x04001408 RID: 5128
		public const int rocks = 7;

		// Token: 0x04001409 RID: 5129
		public const int packages = 8;

		// Token: 0x0400140A RID: 5130
		public const int presents = 9;

		// Token: 0x0400140B RID: 5131
		public int frontDecal;

		// Token: 0x0400140C RID: 5132
		public int carType;

		// Token: 0x0400140D RID: 5133
		public int resourceType;

		// Token: 0x0400140E RID: 5134
		public int loaded;

		// Token: 0x0400140F RID: 5135
		public int[] topFeatures = new int[6];

		// Token: 0x04001410 RID: 5136
		public bool alternateCar;

		// Token: 0x04001411 RID: 5137
		public Color color;
	}
}
