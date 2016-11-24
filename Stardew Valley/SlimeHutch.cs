using System;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Monsters;
using StardewValley.Tools;
using xTile;

namespace StardewValley
{
	// Token: 0x0200001C RID: 28
	public class SlimeHutch : GameLocation
	{
		// Token: 0x0600012A RID: 298 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		public SlimeHutch()
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		public SlimeHutch(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00002834 File Offset: 0x00000A34
		public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000C7F2 File Offset: 0x0000A9F2
		public bool isFull()
		{
			return this.characters.Count >= 20;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000C808 File Offset: 0x0000AA08
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			for (int i = 0; i < this.waterSpots.Length; i++)
			{
				if (this.waterSpots[i])
				{
					base.setMapTileIndex(16, 6 + i, 2135, "Buildings", 0);
				}
				else
				{
					base.setMapTileIndex(16, 6 + i, 2134, "Buildings", 0);
				}
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000C868 File Offset: 0x0000AA68
		public Building getBuilding()
		{
			foreach (Building b in Game1.getFarm().buildings)
			{
				if (b.indoors != null && b.indoors.Equals(this))
				{
					return b;
				}
			}
			return null;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		public override bool canSlimeMateHere()
		{
			int matesLeft = this.slimeMatingsLeft;
			this.slimeMatingsLeft--;
			return !this.isFull() && matesLeft > 0;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000C908 File Offset: 0x0000AB08
		public override bool canSlimeHatchHere()
		{
			return !this.isFull();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000C914 File Offset: 0x0000AB14
		public override void DayUpdate(int dayOfMonth)
		{
			int waters = 0;
			for (int i = 0; i < this.waterSpots.Length; i++)
			{
				if (this.waterSpots[i] && waters * 5 < this.characters.Count)
				{
					waters++;
					this.waterSpots[i] = false;
					base.setMapTileIndex(16, 6 + i, 2134, "Buildings", 0);
				}
			}
			for (int numSlimeBalls = Math.Min(this.characters.Count / 5, waters); numSlimeBalls > 0; numSlimeBalls--)
			{
				int tries = 50;
				Vector2 tile = base.getRandomTile();
				while ((!this.isTileLocationTotallyClearAndPlaceable(tile) || base.doesTileHaveProperty((int)tile.X, (int)tile.Y, "NPCBarrier", "Back") != null || tile.Y >= 12f) && tries > 0)
				{
					tile = base.getRandomTile();
					tries--;
				}
				if (tries > 0)
				{
					this.objects.Add(tile, new Object(tile, 56, false));
				}
			}
			while (this.slimeMatingsLeft > 0)
			{
				if (this.characters.Count > 1 && !this.isFull())
				{
					NPC mate = this.characters[Game1.random.Next(this.characters.Count)];
					if (mate is GreenSlime)
					{
						GreenSlime mate2 = mate as GreenSlime;
						if (mate2.ageUntilFullGrown <= 0)
						{
							for (int distance = 1; distance < 10; distance++)
							{
								GreenSlime mate3 = (GreenSlime)Utility.checkForCharacterWithinArea(mate2.GetType(), mate.position, this, new Rectangle((int)mate2.position.X - Game1.tileSize * distance, (int)mate2.position.Y - Game1.tileSize * distance, Game1.tileSize * (distance * 2 + 1), Game1.tileSize * (distance * 2 + 1)));
								if (mate3 != null && mate3.cute != mate2.cute && mate3.ageUntilFullGrown <= 0)
								{
									mate2.mateWith(mate3, this);
									break;
								}
							}
						}
					}
				}
				this.slimeMatingsLeft--;
			}
			this.slimeMatingsLeft = this.characters.Count / 5 + 1;
			base.DayUpdate(dayOfMonth);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000CB44 File Offset: 0x0000AD44
		public override bool performToolAction(Tool t, int tileX, int tileY)
		{
			if (t is WateringCan && tileX == 16)
			{
				switch (tileY)
				{
				case 6:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[0] = true;
					break;
				case 7:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[1] = true;
					break;
				case 8:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[2] = true;
					break;
				case 9:
					base.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
					this.waterSpots[3] = true;
					break;
				}
			}
			return false;
		}

		// Token: 0x04000141 RID: 321
		private int slimeMatingsLeft;

		// Token: 0x04000142 RID: 322
		public bool[] waterSpots = new bool[4];
	}
}
