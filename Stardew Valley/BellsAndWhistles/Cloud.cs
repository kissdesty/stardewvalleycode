using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200014F RID: 335
	public class Cloud : Critter
	{
		// Token: 0x060012EE RID: 4846 RVA: 0x0017EAB8 File Offset: 0x0017CCB8
		public Cloud()
		{
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0017EAC8 File Offset: 0x0017CCC8
		public Cloud(Vector2 position)
		{
			this.position = position * (float)Game1.tileSize;
			this.startingPosition = position;
			this.verticalFlip = (Game1.random.NextDouble() < 0.5);
			this.horizontalFlip = (Game1.random.NextDouble() < 0.5);
			this.zoom = Game1.random.Next(4, 7);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0017EB44 File Offset: 0x0017CD44
		public override bool update(GameTime time, GameLocation environment)
		{
			this.position.Y = this.position.Y - (float)time.ElapsedGameTime.TotalMilliseconds * 0.02f;
			this.position.X = this.position.X - (float)time.ElapsedGameTime.TotalMilliseconds * 0.02f;
			return this.position.X < (float)(-147 * this.zoom) || this.position.Y < (float)(-100 * this.zoom);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0017EBCD File Offset: 0x0017CDCD
		public override Rectangle getBoundingBox(int xOffset, int yOffset)
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, 147 * this.zoom, 100 * this.zoom);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00002834 File Offset: 0x00000A34
		public override void draw(SpriteBatch b)
		{
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0017EC04 File Offset: 0x0017CE04
		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(this.position), new Rectangle?(new Rectangle(128, 0, 146, 99)), Color.White, (this.verticalFlip && this.horizontalFlip) ? 3.14159274f : 0f, Vector2.Zero, (float)this.zoom, (this.verticalFlip && !this.horizontalFlip) ? SpriteEffects.FlipVertically : ((this.horizontalFlip && !this.verticalFlip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);
		}

		// Token: 0x04001367 RID: 4967
		public const int width = 147;

		// Token: 0x04001368 RID: 4968
		public const int height = 100;

		// Token: 0x04001369 RID: 4969
		public int zoom = 5;

		// Token: 0x0400136A RID: 4970
		private bool verticalFlip;

		// Token: 0x0400136B RID: 4971
		private bool horizontalFlip;
	}
}
