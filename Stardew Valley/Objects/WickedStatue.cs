using System;
using Microsoft.Xna.Framework;
using StardewValley.Projectiles;
using StardewValley.Tools;

namespace StardewValley.Objects
{
	// Token: 0x0200009B RID: 155
	public class WickedStatue : Object
	{
		// Token: 0x06000B26 RID: 2854 RVA: 0x000D9F75 File Offset: 0x000D8175
		public WickedStatue()
		{
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000E31E0 File Offset: 0x000E13E0
		public WickedStatue(Vector2 tileLocation) : base(tileLocation, 84, false)
		{
			this.fragility = 2;
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000E3230 File Offset: 0x000E1430
		public override bool performToolAction(Tool t)
		{
			if (t is Pickaxe)
			{
				Game1.createRadialDebris(Game1.currentLocation, 14, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(8, 16), false, -1, false, -1);
				Game1.createMultipleObjectDebris(390, (int)this.tileLocation.X, (int)this.tileLocation.Y, (int)this.tileLocation.X % 4 + 3, t.getLastFarmerToUse().uniqueMultiplayerID);
				if (Game1.currentLocation.objects.ContainsKey(this.tileLocation))
				{
					Game1.currentLocation.objects.Remove(this.tileLocation);
				}
				Game1.playSound("hammer");
				return false;
			}
			return false;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000E32F4 File Offset: 0x000E14F4
		public override void updateWhenCurrentLocation(GameTime time)
		{
			base.updateWhenCurrentLocation(time);
			if (Game1.random.NextDouble() < 0.001 && Utility.isThereAFarmerWithinDistance(this.tileLocation, 12) != null)
			{
				Farmer target = Utility.getNearestFarmerInCurrentLocation(this.tileLocation);
				Vector2 v = Utility.getVelocityTowardPlayer(new Point((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize), 12f, target);
				Game1.currentLocation.projectiles.Add(new BasicProjectile(8, 10, 2, 2, 0.196349546f, v.X, v.Y, this.tileLocation * (float)Game1.tileSize));
			}
		}
	}
}
