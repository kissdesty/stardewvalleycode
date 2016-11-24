using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000008 RID: 8
	public class Background
	{
		// Token: 0x0600005A RID: 90 RVA: 0x000041B2 File Offset: 0x000023B2
		public Background()
		{
			this.summitBG = true;
			this.c = Color.White;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000041D8 File Offset: 0x000023D8
		public Background(Texture2D bgImage, int seedValue, int chunksWide, int chunksHigh, int chunkWidth, int chunkHeight, float zoom, int defaultChunkIndex, int numChunksInSheet, double chanceForDeviation, Color c)
		{
			this.backgroundImage = bgImage;
			this.chunksWide = chunksWide;
			this.chunksHigh = chunksHigh;
			this.zoom = zoom;
			this.chunkWidth = chunkWidth;
			this.chunkHeight = chunkHeight;
			this.defaultChunkIndex = defaultChunkIndex;
			this.numChunksInSheet = numChunksInSheet;
			this.chanceForDeviationFromDefault = chanceForDeviation;
			this.c = c;
			Random r = new Random(seedValue);
			this.chunks = new int[chunksWide * chunksHigh];
			for (int i = 0; i < chunksHigh * chunksWide; i++)
			{
				if (r.NextDouble() < this.chanceForDeviationFromDefault)
				{
					this.chunks[i] = r.Next(numChunksInSheet);
				}
				else
				{
					this.chunks[i] = defaultChunkIndex;
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004294 File Offset: 0x00002494
		public void update(xTile.Dimensions.Rectangle viewport)
		{
			this.position.X = -((float)(viewport.X + viewport.Width / 2) / ((float)Game1.currentLocation.map.GetLayer("Back").LayerWidth * (float)Game1.tileSize) * ((float)(this.chunksWide * this.chunkWidth) * this.zoom - (float)viewport.Width));
			this.position.Y = -((float)(viewport.Y + viewport.Height / 2) / ((float)Game1.currentLocation.map.GetLayer("Back").LayerHeight * (float)Game1.tileSize) * ((float)(this.chunksHigh * this.chunkHeight) * this.zoom - (float)viewport.Height));
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004360 File Offset: 0x00002560
		public void draw(SpriteBatch b)
		{
			if (this.summitBG)
			{
				int seasonOffset = 0;
				string currentSeason = Game1.currentSeason;
				if (!(currentSeason == "summer"))
				{
					if (!(currentSeason == "fall"))
					{
						if (currentSeason == "winter")
						{
							seasonOffset = 2;
						}
					}
					else
					{
						seasonOffset = 1;
					}
				}
				else
				{
					seasonOffset = 0;
				}
				float alpha = 1f;
				Color bgColor = Color.White;
				if (Game1.timeOfDay >= 1800)
				{
					int adjustedTime = (int)((float)(Game1.timeOfDay - Game1.timeOfDay % 100) + (float)(Game1.timeOfDay % 100 / 10) * 16.66f);
					this.c = new Color(255f, 255f - Math.Max(100f, (float)adjustedTime + (float)Game1.gameTimeInterval / 7000f * 16.6f - 1800f), 255f - Math.Max(100f, ((float)adjustedTime + (float)Game1.gameTimeInterval / 7000f * 16.6f - 1800f) / 2f));
					bgColor = Color.Blue * 0.5f;
					alpha = Math.Max(0f, Math.Min(1f, (2000f - ((float)adjustedTime + (float)Game1.gameTimeInterval / 7000f * 16.6f)) / 200f));
				}
				b.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height * 3 / 4), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(639 + (seasonOffset + 1), 1051, 1, 400)), this.c * alpha, 0f, Vector2.Zero, SpriteEffects.None, 1E-07f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - 596)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 736 + seasonOffset * 149, 639, 149)), Color.White * Math.Max((float)this.c.A, 0.5f), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - 596)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 736 + seasonOffset * 149, 639, 149)), bgColor * (0.75f - alpha), 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
				return;
			}
			Vector2 v = Vector2.Zero;
			Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, 0, this.chunkWidth, this.chunkHeight);
			for (int i = 0; i < this.chunks.Length; i++)
			{
				v.X = this.position.X + (float)(i * this.chunkWidth % (this.chunksWide * this.chunkWidth)) * this.zoom;
				v.Y = this.position.Y + (float)(i * this.chunkWidth / (this.chunksWide * this.chunkWidth) * this.chunkHeight) * this.zoom;
				if (this.backgroundImage == null)
				{
					b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)v.X, (int)v.Y, Game1.viewport.Width, Game1.viewport.Height), new Microsoft.Xna.Framework.Rectangle?(r), this.c, 0f, Vector2.Zero, SpriteEffects.None, 0f);
				}
				else
				{
					r.X = this.chunks[i] * this.chunkWidth % this.backgroundImage.Width;
					r.Y = this.chunks[i] * this.chunkWidth / this.backgroundImage.Width * this.chunkHeight;
					b.Draw(this.backgroundImage, v, new Microsoft.Xna.Framework.Rectangle?(r), this.c, 0f, Vector2.Zero, this.zoom, SpriteEffects.None, 0f);
				}
			}
		}

		// Token: 0x0400002B RID: 43
		public int defaultChunkIndex;

		// Token: 0x0400002C RID: 44
		public int numChunksInSheet;

		// Token: 0x0400002D RID: 45
		public double chanceForDeviationFromDefault;

		// Token: 0x0400002E RID: 46
		private Texture2D backgroundImage;

		// Token: 0x0400002F RID: 47
		private Vector2 position = Vector2.Zero;

		// Token: 0x04000030 RID: 48
		private int chunksWide;

		// Token: 0x04000031 RID: 49
		private int chunksHigh;

		// Token: 0x04000032 RID: 50
		private int chunkWidth;

		// Token: 0x04000033 RID: 51
		private int chunkHeight;

		// Token: 0x04000034 RID: 52
		private int[] chunks;

		// Token: 0x04000035 RID: 53
		private float zoom;

		// Token: 0x04000036 RID: 54
		public Color c;

		// Token: 0x04000037 RID: 55
		private bool summitBG;

		// Token: 0x04000038 RID: 56
		public int yOffset;
	}
}
