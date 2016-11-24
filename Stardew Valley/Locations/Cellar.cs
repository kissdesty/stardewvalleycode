using System;
using Microsoft.Xna.Framework;
using StardewValley.Objects;
using xTile;

namespace StardewValley.Locations
{
	// Token: 0x02000122 RID: 290
	public class Cellar : GameLocation
	{
		// Token: 0x0600107A RID: 4218 RVA: 0x00151B90 File Offset: 0x0014FD90
		public Cellar()
		{
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x001539E5 File Offset: 0x00151BE5
		public Cellar(Map map, string name) : base(map, name)
		{
			this.setUpAgingBoards();
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x001539F5 File Offset: 0x00151BF5
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00153A00 File Offset: 0x00151C00
		public void setUpAgingBoards()
		{
			for (int i = 6; i < 17; i++)
			{
				Vector2 v = new Vector2((float)i, 8f);
				if (!this.objects.ContainsKey(v))
				{
					this.objects.Add(v, new Cask(v));
				}
				v = new Vector2((float)i, 10f);
				if (!this.objects.ContainsKey(v))
				{
					this.objects.Add(v, new Cask(v));
				}
				v = new Vector2((float)i, 12f);
				if (!this.objects.ContainsKey(v))
				{
					this.objects.Add(v, new Cask(v));
				}
			}
		}
	}
}
