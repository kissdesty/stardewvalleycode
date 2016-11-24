using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace StardewValley.Tools
{
	// Token: 0x0200005C RID: 92
	public class Lantern : Tool
	{
		// Token: 0x060008E8 RID: 2280 RVA: 0x000C086D File Offset: 0x000BEA6D
		public Lantern() : base("Lantern", 0, 74, 74, "Lights up dark places.", false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x000C08A0 File Offset: 0x000BEAA0
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			this.on = !this.on;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			if (this.on)
			{
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(who.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 2.5f + (float)this.fuelLeft / 100f * 10f * 0.75f, new Color(0, 131, 255), -85736));
				return;
			}
			for (int i = Game1.currentLightSources.Count - 1; i >= 0; i--)
			{
				if (Game1.currentLightSources.ElementAt(i).identifier == -85736)
				{
					Game1.currentLightSources.Remove(Game1.currentLightSources.ElementAt(i));
					return;
				}
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x000C099C File Offset: 0x000BEB9C
		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (this.on && this.fuelLeft > 0 && Game1.drawLighting)
			{
				this.fuelTimer += time.ElapsedGameTime.Milliseconds;
				if (this.fuelTimer > 6000)
				{
					this.fuelLeft--;
					this.fuelTimer = 0;
				}
				bool wasFound = false;
				foreach (LightSource i in Game1.currentLightSources)
				{
					if (i.identifier == -85736)
					{
						i.position = new Vector2(who.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize);
						wasFound = true;
						break;
					}
				}
				if (!wasFound)
				{
					Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(who.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 2.5f + (float)this.fuelLeft / 100f * 10f * 0.75f, new Color(0, 131, 255), -85736));
				}
			}
			if (this.on && this.fuelLeft <= 0)
			{
				Utility.removeLightSource(1);
			}
		}

		// Token: 0x040008F1 RID: 2289
		public const float baseRadius = 10f;

		// Token: 0x040008F2 RID: 2290
		public const int millisecondsPerFuelUnit = 6000;

		// Token: 0x040008F3 RID: 2291
		public const int maxFuel = 100;

		// Token: 0x040008F4 RID: 2292
		public int fuelLeft;

		// Token: 0x040008F5 RID: 2293
		private int fuelTimer;

		// Token: 0x040008F6 RID: 2294
		public bool on;
	}
}
