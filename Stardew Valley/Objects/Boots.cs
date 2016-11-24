using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Objects
{
	// Token: 0x0200008B RID: 139
	public class Boots : Item
	{
		// Token: 0x06000A48 RID: 2632 RVA: 0x000D9D13 File Offset: 0x000D7F13
		public Boots()
		{
			this.category = -97;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x000D9D24 File Offset: 0x000D7F24
		public Boots(int which)
		{
			string[] data = Game1.content.Load<Dictionary<int, string>>("Data\\Boots")[which].Split(new char[]
			{
				'/'
			});
			this.Name = data[0];
			this.description = data[1];
			this.price = Convert.ToInt32(data[2]);
			this.defenseBonus = Convert.ToInt32(data[3]);
			this.immunityBonus = Convert.ToInt32(data[4]);
			this.indexInColorSheet = Convert.ToInt32(data[5]);
			this.indexInTileSheet = which;
			this.category = -97;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000D9DB6 File Offset: 0x000D7FB6
		public override int salePrice()
		{
			return this.defenseBonus * 100 + this.immunityBonus * 100;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000D9DCB File Offset: 0x000D7FCB
		public void onEquip()
		{
			Game1.player.resilience += this.defenseBonus;
			Game1.player.immunity += this.immunityBonus;
			Game1.player.changeShoeColor(this.indexInColorSheet);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000D9E0B File Offset: 0x000D800B
		public void onUnequip()
		{
			Game1.player.resilience -= this.defenseBonus;
			Game1.player.immunity -= this.immunityBonus;
			Game1.player.changeShoeColor(12);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000D9E47 File Offset: 0x000D8047
		public int getNumberOfDescriptionCategories()
		{
			if (this.immunityBonus > 0 && this.defenseBonus > 0)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000D9E60 File Offset: 0x000D8060
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.indexInTileSheet, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f) * scaleSize, scaleSize * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00035C5D File Offset: 0x00033E5D
		public override int maximumStackSize()
		{
			return -1;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0000846E File Offset: 0x0000666E
		public override int getStack()
		{
			return 1;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0000846E File Offset: 0x0000666E
		public override int addToStack(int amount)
		{
			return 1;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000D9EE3 File Offset: 0x000D80E3
		public override string getCategoryName()
		{
			return "Footwear";
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000D9EEC File Offset: 0x000D80EC
		public override string getDescription()
		{
			string tmp = this.description;
			tmp = string.Concat(new object[]
			{
				tmp,
				Environment.NewLine,
				Environment.NewLine,
				"Level ",
				this.immunityBonus + this.defenseBonus
			});
			return Game1.parseText(tmp, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPlaceable()
		{
			return false;
		}

		// Token: 0x170000C2 RID: 194
		public override string Name
		{
			// Token: 0x06000A55 RID: 2645 RVA: 0x000D9F57 File Offset: 0x000D8157
			get
			{
				return this.name;
			}
			// Token: 0x06000A56 RID: 2646 RVA: 0x000D9F5F File Offset: 0x000D815F
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		public override int Stack
		{
			// Token: 0x06000A57 RID: 2647 RVA: 0x0000846E File Offset: 0x0000666E
			get
			{
				return 1;
			}
			// Token: 0x06000A58 RID: 2648 RVA: 0x00002834 File Offset: 0x00000A34
			set
			{
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000D9F68 File Offset: 0x000D8168
		public override Item getOne()
		{
			return new Boots(this.indexInTileSheet);
		}

		// Token: 0x04000AC3 RID: 2755
		public int defenseBonus;

		// Token: 0x04000AC4 RID: 2756
		public int immunityBonus;

		// Token: 0x04000AC5 RID: 2757
		public int indexInTileSheet;

		// Token: 0x04000AC6 RID: 2758
		public int price;

		// Token: 0x04000AC7 RID: 2759
		public int indexInColorSheet;

		// Token: 0x04000AC8 RID: 2760
		public string description;

		// Token: 0x04000AC9 RID: 2761
		public string name;
	}
}
