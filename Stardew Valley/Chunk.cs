using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000011 RID: 17
	public class Chunk
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x0000282C File Offset: 0x00000A2C
		public Chunk()
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000A16C File Offset: 0x0000836C
		public Chunk(Vector2 position, float xVelocity, float yVelocity, int debrisType)
		{
			this.position = position;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.debrisType = debrisType;
			this.alpha = 1f;
		}

		// Token: 0x04000101 RID: 257
		public Vector2 position;

		// Token: 0x04000102 RID: 258
		[XmlIgnore]
		public float xVelocity;

		// Token: 0x04000103 RID: 259
		[XmlIgnore]
		public float yVelocity;

		// Token: 0x04000104 RID: 260
		[XmlIgnore]
		public bool hasPassedRestingLineOnce;

		// Token: 0x04000105 RID: 261
		[XmlIgnore]
		public int bounces;

		// Token: 0x04000106 RID: 262
		public int debrisType;

		// Token: 0x04000107 RID: 263
		[XmlIgnore]
		public bool hitWall;

		// Token: 0x04000108 RID: 264
		public int xSpriteSheet;

		// Token: 0x04000109 RID: 265
		public int ySpriteSheet;

		// Token: 0x0400010A RID: 266
		[XmlIgnore]
		public float rotation;

		// Token: 0x0400010B RID: 267
		[XmlIgnore]
		public float rotationVelocity;

		// Token: 0x0400010C RID: 268
		public float scale;

		// Token: 0x0400010D RID: 269
		public float alpha;
	}
}
