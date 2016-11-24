using System;
using Microsoft.Xna.Framework;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x0200007A RID: 122
	public class TerrainFeatureFactory
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x000D2128 File Offset: 0x000D0328
		public static TerrainFeature getNewTerrainFeatureFromIndex(byte index, int extraInfo)
		{
			switch (index)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				return new Grass((int)(index + 1), extraInfo);
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return new Tree((int)(index - 3), extraInfo);
			case 10:
				return new Quartz(1, (Game1.mine != null) ? Game1.mine.getCrystalColorForThisLevel() : Color.Green);
			case 11:
				return new Quartz(2, (Game1.mine != null) ? Game1.mine.getCrystalColorForThisLevel() : Color.Green);
			case 12:
				return new HoeDirt(Game1.isRaining ? 1 : 0);
			case 13:
				return new CosmeticPlant(extraInfo);
			case 14:
				return new Flooring(0);
			case 15:
				return new Flooring(1);
			case 16:
				return new Tree(7, extraInfo);
			case 17:
				return new Flooring(3);
			case 18:
				return new Flooring(2);
			case 19:
				return new Flooring(4);
			case 20:
				return new ResourceClump(600, 2, 2, Vector2.Zero);
			default:
				throw new MissingMethodException();
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x000D2240 File Offset: 0x000D0440
		public static TerrainFeatureDescription getIndexFromTerrainFeature(TerrainFeature f)
		{
			if (f.GetType() == typeof(CosmeticPlant))
			{
				return new TerrainFeatureDescription(13, (int)((CosmeticPlant)f).grassType);
			}
			if (f.GetType() == typeof(Grass))
			{
				switch (((Grass)f).grassType)
				{
				case 1:
					return new TerrainFeatureDescription(0, ((Grass)f).numberOfWeeds);
				case 2:
					return new TerrainFeatureDescription(1, ((Grass)f).numberOfWeeds);
				case 3:
					return new TerrainFeatureDescription(2, ((Grass)f).numberOfWeeds);
				case 4:
					return new TerrainFeatureDescription(3, ((Grass)f).numberOfWeeds);
				}
			}
			else if (f.GetType() == typeof(Tree))
			{
				switch (((Tree)f).treeType)
				{
				case 1:
					return new TerrainFeatureDescription(4, ((Tree)f).growthStage);
				case 2:
					return new TerrainFeatureDescription(5, ((Tree)f).growthStage);
				case 3:
					return new TerrainFeatureDescription(6, ((Tree)f).growthStage);
				case 4:
					return new TerrainFeatureDescription(7, ((Tree)f).growthStage);
				case 5:
					return new TerrainFeatureDescription(8, ((Tree)f).growthStage);
				case 7:
					return new TerrainFeatureDescription(16, ((Tree)f).growthStage);
				}
			}
			else if (f.GetType() == typeof(Quartz))
			{
				int num = ((Quartz)f).bigness;
				if (num == 1)
				{
					return new TerrainFeatureDescription(10, -1);
				}
				if (num == 2)
				{
					return new TerrainFeatureDescription(11, -1);
				}
			}
			else
			{
				if (f.GetType() == typeof(HoeDirt))
				{
					return new TerrainFeatureDescription(12, -1);
				}
				if (f.GetType() == typeof(Flooring))
				{
					int num = ((Flooring)f).whichFloor;
					if (num == 0)
					{
						return new TerrainFeatureDescription(14, -1);
					}
					if (num == 1)
					{
						return new TerrainFeatureDescription(15, -1);
					}
				}
				else if (f is ResourceClump)
				{
					int num = (f as ResourceClump).parentSheetIndex;
					if (num == 600)
					{
						return new TerrainFeatureDescription(20, -1);
					}
				}
			}
			throw new MissingMethodException();
		}

		// Token: 0x04000A1A RID: 2586
		public const byte grass1 = 0;

		// Token: 0x04000A1B RID: 2587
		public const byte grass2 = 1;

		// Token: 0x04000A1C RID: 2588
		public const byte grass3 = 2;

		// Token: 0x04000A1D RID: 2589
		public const byte grass4 = 3;

		// Token: 0x04000A1E RID: 2590
		public const byte bushyTree = 4;

		// Token: 0x04000A1F RID: 2591
		public const byte leafyTree = 5;

		// Token: 0x04000A20 RID: 2592
		public const byte pineTree = 6;

		// Token: 0x04000A21 RID: 2593
		public const byte winterTree1 = 7;

		// Token: 0x04000A22 RID: 2594
		public const byte winterTree2 = 8;

		// Token: 0x04000A23 RID: 2595
		public const byte palmTree = 9;

		// Token: 0x04000A24 RID: 2596
		public const byte quartzSmall = 10;

		// Token: 0x04000A25 RID: 2597
		public const byte quartzMedium = 11;

		// Token: 0x04000A26 RID: 2598
		public const byte hoeDirt = 12;

		// Token: 0x04000A27 RID: 2599
		public const byte cosmeticPlant = 13;

		// Token: 0x04000A28 RID: 2600
		public const byte floorWood = 14;

		// Token: 0x04000A29 RID: 2601
		public const byte floorStone = 15;

		// Token: 0x04000A2A RID: 2602
		public const byte mushroomTree = 16;

		// Token: 0x04000A2B RID: 2603
		public const byte floorIce = 17;

		// Token: 0x04000A2C RID: 2604
		public const byte floorGhost = 18;

		// Token: 0x04000A2D RID: 2605
		public const byte floorStraw = 19;

		// Token: 0x04000A2E RID: 2606
		public const byte stump = 20;
	}
}
