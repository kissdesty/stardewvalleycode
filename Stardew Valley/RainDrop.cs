using System;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000049 RID: 73
	public struct RainDrop
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x0007F8C4 File Offset: 0x0007DAC4
		public RainDrop(int x, int y, int frame, int accumulator)
		{
			this.position = new Vector2((float)x, (float)y);
			this.frame = frame;
			this.accumulator = accumulator;
		}

		// Token: 0x04000657 RID: 1623
		public int frame;

		// Token: 0x04000658 RID: 1624
		public int accumulator;

		// Token: 0x04000659 RID: 1625
		public Vector2 position;
	}
}
