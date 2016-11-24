using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000073 RID: 115
	public class LargeTerrainFeature : TerrainFeature
	{
		// Token: 0x060009A5 RID: 2469 RVA: 0x000CF1FD File Offset: 0x000CD3FD
		public Rectangle getBoundingBox()
		{
			return this.getBoundingBox(this.tilePosition);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000CF20B File Offset: 0x000CD40B
		public void dayUpdate(GameLocation l)
		{
			this.dayUpdate(l, this.tilePosition);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x000CF21A File Offset: 0x000CD41A
		public bool tickUpdate(GameTime time)
		{
			return this.tickUpdate(time, this.tilePosition);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x000CF229 File Offset: 0x000CD429
		public void draw(SpriteBatch b)
		{
			this.draw(b, this.tilePosition);
		}

		// Token: 0x040009BC RID: 2492
		public Vector2 tilePosition;
	}
}
