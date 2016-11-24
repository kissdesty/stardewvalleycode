using System;
using Microsoft.Xna.Framework;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200007B RID: 123
	public class Leaf
	{
		// Token: 0x060009F4 RID: 2548 RVA: 0x000D2485 File Offset: 0x000D0685
		public Leaf(Vector2 position, float rotationRate, int type, float yVelocity)
		{
			this.position = position;
			this.rotationRate = rotationRate;
			this.type = type;
			this.yVelocity = yVelocity;
		}

		// Token: 0x04000A2F RID: 2607
		public Vector2 position;

		// Token: 0x04000A30 RID: 2608
		public float rotation;

		// Token: 0x04000A31 RID: 2609
		public float rotationRate;

		// Token: 0x04000A32 RID: 2610
		public float yVelocity;

		// Token: 0x04000A33 RID: 2611
		public int type;
	}
}
