using System;
using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000028 RID: 40
	public class LevelBuilder
	{
		// Token: 0x06000229 RID: 553 RVA: 0x000328AC File Offset: 0x00030AAC
		public static bool tryToAddObject(int index, bool bigCraftable, Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				if (bigCraftable)
				{
					Game1.mine.objects.Add(position, new Object(position, index, false));
				}
				else
				{
					Game1.mine.Objects.Add(position, new Object(position, index, null, false, false, false, false));
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00032904 File Offset: 0x00030B04
		public static bool tryToAddMonster(Monster m, Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null) && Game1.mine.isTileLocationOpen(new Location((int)position.X, (int)position.Y)) && Game1.mine.isTileOnMap(position) && Game1.mine.isTileOnClearAndSolidGround(position))
			{
				m.position = new Vector2(position.X * (float)Game1.tileSize, position.Y * (float)Game1.tileSize - (float)(m.sprite.spriteHeight - Game1.tileSize));
				Game1.mine.characters.Add(m);
				return true;
			}
			return false;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000329A6 File Offset: 0x00030BA6
		public static bool tryToAddFence(int which, Vector2 position, bool gate)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				Game1.mine.objects.Add(position, new Fence(position, which, gate));
			}
			return false;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000329CF File Offset: 0x00030BCF
		public static bool addTorch(Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				Game1.mine.Objects.Add(position, new Torch(position, 1));
				return true;
			}
			return false;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000329F9 File Offset: 0x00030BF9
		public static bool tryToAddObject(Object obj, Vector2 position)
		{
			if (!Game1.mine.isTileOccupiedForPlacement(position, null))
			{
				Game1.mine.Objects.Add(position, obj);
				return true;
			}
			return false;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00032A1D File Offset: 0x00030C1D
		public static bool tryToAddObject(int index, bool bigCraftable, Vector2 position, int heldItem)
		{
			if (LevelBuilder.tryToAddObject(index, bigCraftable, position))
			{
				Game1.mine.objects[position].heldObject = new Object(position, heldItem, null, false, false, false, false);
			}
			return false;
		}
	}
}
