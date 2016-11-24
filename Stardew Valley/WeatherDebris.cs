using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000057 RID: 87
	public class WeatherDebris
	{
		// Token: 0x060008B7 RID: 2231 RVA: 0x000BA660 File Offset: 0x000B8860
		public WeatherDebris(Vector2 position, int which, float rotationVelocity, float dx, float dy)
		{
			this.position = position;
			this.which = which;
			this.dx = dx;
			this.dy = dy;
			switch (which)
			{
			case 0:
				this.sourceRect = new Rectangle(352, 1184, 16, 16);
				this.animationIntervalOffset = (Game1.random.Next(25) - 12) * 2;
				return;
			case 1:
				this.sourceRect = new Rectangle(352, 1200, 16, 16);
				this.animationIntervalOffset = (Game1.random.Next(25) - 12) * 2;
				return;
			case 2:
				this.sourceRect = new Rectangle(352, 1216, 16, 16);
				this.animationIntervalOffset = (Game1.random.Next(25) - 12) * 2;
				return;
			case 3:
				this.sourceRect = new Rectangle(391 + 4 * Game1.random.Next(5), 1236, 4, 4);
				return;
			default:
				return;
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000BA76F File Offset: 0x000B896F
		public void update()
		{
			this.update(false);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000BA778 File Offset: 0x000B8978
		public void update(bool slow)
		{
			this.position.X = this.position.X + (this.dx + (slow ? 0f : WeatherDebris.globalWind));
			this.position.Y = this.position.Y + (this.dy - (slow ? 0f : -0.5f));
			if (this.dy < 0f && !this.blowing)
			{
				this.dy += 0.01f;
			}
			if (!Game1.fadeToBlack && Game1.fadeToBlackAlpha <= 0f)
			{
				if (this.position.X < (float)(-(float)Game1.tileSize - Game1.tileSize / 4))
				{
					this.position.X = (float)Game1.viewport.Width;
					this.position.Y = (float)Game1.random.Next(0, Game1.viewport.Height - Game1.tileSize);
				}
				if (this.position.Y > (float)(Game1.viewport.Height + Game1.tileSize / 4))
				{
					this.position.X = (float)Game1.random.Next(0, Game1.viewport.Width);
					this.position.Y = (float)(-(float)Game1.tileSize);
					this.dy = (float)Game1.random.Next(-15, 10) / (slow ? ((Game1.random.NextDouble() < 0.1) ? 5f : 200f) : 50f);
					this.dx = (float)Game1.random.Next(-10, 0) / (slow ? 200f : 50f);
				}
				else if (this.position.Y < (float)(-(float)Game1.tileSize))
				{
					this.position.Y = (float)Game1.viewport.Height;
					this.position.X = (float)Game1.random.Next(0, Game1.viewport.Width);
				}
			}
			if (this.blowing)
			{
				this.dy -= 0.01f;
				if (Game1.random.NextDouble() < 0.006 || this.dy < -2f)
				{
					this.blowing = false;
				}
			}
			else if (!slow && Game1.random.NextDouble() < 0.001 && Game1.currentSeason != null && (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("summer")))
			{
				this.blowing = true;
			}
			switch (this.which)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				this.animationTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (this.animationTimer <= 0)
				{
					this.animationTimer = 100 + this.animationIntervalOffset;
					this.animationIndex += this.animationDirection;
					if (this.animationDirection == 0)
					{
						if (this.animationIndex >= 9)
						{
							this.animationDirection = -1;
						}
						else
						{
							this.animationDirection = 1;
						}
					}
					if (this.animationIndex > 10)
					{
						if (Game1.random.NextDouble() < 0.82)
						{
							this.animationIndex--;
							this.animationDirection = 0;
							this.dx += 0.1f;
							this.dy -= 0.2f;
						}
						else
						{
							this.animationIndex = 0;
						}
					}
					else if (this.animationIndex == 4 && this.animationDirection == -1)
					{
						this.animationIndex++;
						this.animationDirection = 0;
						this.dx -= 0.1f;
						this.dy -= 0.1f;
					}
					if (this.animationIndex == 7 && this.animationDirection == -1)
					{
						this.dy -= 0.2f;
					}
					if (this.which != 3)
					{
						this.sourceRect.X = 352 + this.animationIndex * 16;
					}
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000BAB80 File Offset: 0x000B8D80
		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1E-06f);
		}

		// Token: 0x040008AE RID: 2222
		public const int pinkPetals = 0;

		// Token: 0x040008AF RID: 2223
		public const int greenLeaves = 1;

		// Token: 0x040008B0 RID: 2224
		public const int fallLeaves = 2;

		// Token: 0x040008B1 RID: 2225
		public const int snow = 3;

		// Token: 0x040008B2 RID: 2226
		public const int animationInterval = 100;

		// Token: 0x040008B3 RID: 2227
		public const float gravity = -0.5f;

		// Token: 0x040008B4 RID: 2228
		public Vector2 position;

		// Token: 0x040008B5 RID: 2229
		private Rectangle sourceRect;

		// Token: 0x040008B6 RID: 2230
		public int which;

		// Token: 0x040008B7 RID: 2231
		public int animationIndex;

		// Token: 0x040008B8 RID: 2232
		public int animationTimer = 100;

		// Token: 0x040008B9 RID: 2233
		public int animationDirection = 1;

		// Token: 0x040008BA RID: 2234
		public int animationIntervalOffset;

		// Token: 0x040008BB RID: 2235
		public float dx;

		// Token: 0x040008BC RID: 2236
		public float dy;

		// Token: 0x040008BD RID: 2237
		public static float globalWind = -0.25f;

		// Token: 0x040008BE RID: 2238
		private bool blowing;
	}
}
