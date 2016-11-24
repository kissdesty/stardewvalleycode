using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Locations;
using xTile;

namespace StardewValley
{
	// Token: 0x0200001B RID: 27
	public class Shed : DecoratableLocation
	{
		// Token: 0x06000125 RID: 293 RVA: 0x0000C6C8 File Offset: 0x0000A8C8
		public Shed()
		{
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000C6D0 File Offset: 0x0000A8D0
		public Shed(Map m, string name) : base(m, name)
		{
			List<Rectangle> rooms = DecoratableLocation.getWalls();
			while (this.wallPaper.Count < rooms.Count)
			{
				this.wallPaper.Add(0);
			}
			rooms = DecoratableLocation.getFloors();
			while (this.floor.Count < rooms.Count)
			{
				this.floor.Add(0);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00002834 File Offset: 0x00000A34
		public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000C733 File Offset: 0x0000A933
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.isDarkOut())
			{
				Game1.ambientLight = new Color(180, 180, 0);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000C758 File Offset: 0x0000A958
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
	}
}
