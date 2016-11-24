using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Objects
{
	// Token: 0x02000096 RID: 150
	public class Hat : Item
	{
		// Token: 0x06000AF5 RID: 2805 RVA: 0x000E22CD File Offset: 0x000E04CD
		public Hat()
		{
			this.load(this.which);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000E22E4 File Offset: 0x000E04E4
		public void load(int which)
		{
			Dictionary<int, string> expr_0F = Game1.content.Load<Dictionary<int, string>>("Data\\hats");
			if (!expr_0F.ContainsKey(which))
			{
				which = 0;
			}
			string[] split = expr_0F[which].Split(new char[]
			{
				'/'
			});
			this.name = split[0];
			this.description = split[1];
			this.skipHairDraw = Convert.ToBoolean(split[2]);
			this.ignoreHairstyleOffset = Convert.ToBoolean(split[3]);
			this.category = -95;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x000E2359 File Offset: 0x000E0559
		public Hat(int which)
		{
			this.which = which;
			this.load(which);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000E2370 File Offset: 0x000E0570
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(FarmerRenderer.hatsTexture, location + new Vector2(10f, 10f), new Rectangle?(new Rectangle(this.which * 20 % FarmerRenderer.hatsTexture.Width, this.which * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20)), Color.White * transparency, 0f, new Vector2(3f, 3f), 3f * scaleSize, SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000E2402 File Offset: 0x000E0602
		public override string getDescription()
		{
			return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0000846E File Offset: 0x0000666E
		public override int maximumStackSize()
		{
			return 1;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0000846E File Offset: 0x0000666E
		public override int getStack()
		{
			return 1;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0000846E File Offset: 0x0000666E
		public override int addToStack(int amount)
		{
			return 1;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPlaceable()
		{
			return false;
		}

		// Token: 0x170000C8 RID: 200
		public override string Name
		{
			// Token: 0x06000AFE RID: 2814 RVA: 0x000E2423 File Offset: 0x000E0623
			get
			{
				return this.name;
			}
			// Token: 0x06000AFF RID: 2815 RVA: 0x000E242B File Offset: 0x000E062B
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		public override int Stack
		{
			// Token: 0x06000B00 RID: 2816 RVA: 0x0000846E File Offset: 0x0000666E
			get
			{
				return 1;
			}
			// Token: 0x06000B01 RID: 2817 RVA: 0x00002834 File Offset: 0x00000A34
			set
			{
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000E2434 File Offset: 0x000E0634
		public override Item getOne()
		{
			return new Hat(this.which);
		}

		// Token: 0x04000B28 RID: 2856
		public const int widthOfTileSheetSquare = 20;

		// Token: 0x04000B29 RID: 2857
		public const int heightOfTileSheetSquare = 20;

		// Token: 0x04000B2A RID: 2858
		public int which;

		// Token: 0x04000B2B RID: 2859
		public string description;

		// Token: 0x04000B2C RID: 2860
		public string name;

		// Token: 0x04000B2D RID: 2861
		public bool skipHairDraw;

		// Token: 0x04000B2E RID: 2862
		public bool ignoreHairstyleOffset;
	}
}
