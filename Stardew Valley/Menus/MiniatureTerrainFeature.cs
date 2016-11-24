using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace StardewValley.Menus
{
	// Token: 0x02000101 RID: 257
	public class MiniatureTerrainFeature
	{
		// Token: 0x06000F4C RID: 3916 RVA: 0x0013B290 File Offset: 0x00139490
		public MiniatureTerrainFeature(TerrainFeature feature, Vector2 positionOnScreen, Vector2 tileLocation, float scale)
		{
			this.feature = feature;
			this.positionOnScreen = positionOnScreen;
			this.scale = scale;
			this.tileLocation = tileLocation;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0013B2B5 File Offset: 0x001394B5
		public void draw(SpriteBatch b)
		{
			this.feature.drawInMenu(b, this.positionOnScreen, this.tileLocation, this.scale, 0.86f);
		}

		// Token: 0x04001071 RID: 4209
		private TerrainFeature feature;

		// Token: 0x04001072 RID: 4210
		private Vector2 positionOnScreen;

		// Token: 0x04001073 RID: 4211
		private Vector2 tileLocation;

		// Token: 0x04001074 RID: 4212
		private float scale;
	}
}
