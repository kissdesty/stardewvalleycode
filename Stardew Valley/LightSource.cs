using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000030 RID: 48
	public class LightSource
	{
		// Token: 0x0600028F RID: 655 RVA: 0x0000282C File Offset: 0x00000A2C
		public LightSource()
		{
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00035E51 File Offset: 0x00034051
		public LightSource(Texture2D texture, Vector2 position, float radius, Color color)
		{
			this.lightTexture = texture;
			this.position = position;
			this.radius = radius;
			this.color = color;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00035E76 File Offset: 0x00034076
		public LightSource(int lightSource, Vector2 position, float radius, Color color)
		{
			this.loadTextureFromConstantValue(lightSource);
			this.position = position;
			this.radius = radius;
			this.color = color;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00035E9B File Offset: 0x0003409B
		public LightSource(Texture2D texture, Vector2 position, float radius) : this(texture, position, radius, new Color(0, 131, 255), -1)
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00035EB7 File Offset: 0x000340B7
		public LightSource(Texture2D texture, Vector2 position, float radius, Color color, int identifier)
		{
			this.lightTexture = texture;
			this.position = position;
			this.radius = radius;
			this.color = color;
			this.identifier = identifier;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00035EE4 File Offset: 0x000340E4
		public LightSource(int texture, Vector2 position, float radius, Color color, int identifier)
		{
			this.loadTextureFromConstantValue(texture);
			this.position = position;
			this.radius = radius;
			this.color = color;
			this.identifier = identifier;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00035F14 File Offset: 0x00034114
		private void loadTextureFromConstantValue(int value)
		{
			switch (value)
			{
			case 1:
				this.lightTexture = Game1.lantern;
				return;
			case 2:
				this.lightTexture = Game1.windowLight;
				return;
			case 3:
				this.lightTexture = Game1.sconceLight;
				this.position.X = this.position.X + (float)(Game1.tileSize / 2);
				return;
			case 4:
				this.lightTexture = Game1.sconceLight;
				return;
			case 5:
				this.lightTexture = Game1.cauldronLight;
				return;
			case 6:
				this.lightTexture = Game1.indoorWindowLight;
				return;
			case 7:
			case 8:
				break;
			case 9:
				this.lightTexture = Game1.sconceLight;
				this.radius = 3f;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00035FC3 File Offset: 0x000341C3
		public LightSource(int lightSource, Vector2 position, float radius)
		{
			this.loadTextureFromConstantValue(lightSource);
			this.position = position;
			this.radius = radius;
			this.color = Color.Black;
		}

		// Token: 0x040002AB RID: 683
		public const int lantern = 1;

		// Token: 0x040002AC RID: 684
		public const int windowLight = 2;

		// Token: 0x040002AD RID: 685
		public const int tableLight = 3;

		// Token: 0x040002AE RID: 686
		public const int sconceLight = 4;

		// Token: 0x040002AF RID: 687
		public const int cauldronLight = 5;

		// Token: 0x040002B0 RID: 688
		public const int indoorWindowLight = 6;

		// Token: 0x040002B1 RID: 689
		public const int bigIndoorLight = 9;

		// Token: 0x040002B2 RID: 690
		public const int maxLightsOnScreenBeforeReduction = 8;

		// Token: 0x040002B3 RID: 691
		public const float reductionPerExtraLightSource = 0.03f;

		// Token: 0x040002B4 RID: 692
		public const int playerLantern = -85736;

		// Token: 0x040002B5 RID: 693
		public Texture2D lightTexture;

		// Token: 0x040002B6 RID: 694
		public Vector2 position;

		// Token: 0x040002B7 RID: 695
		public Color color;

		// Token: 0x040002B8 RID: 696
		public float radius;

		// Token: 0x040002B9 RID: 697
		public int identifier;
	}
}
