using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace StardewValley.Objects
{
	// Token: 0x0200009A RID: 154
	public class SwitchFloor : Object
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x000E2C5E File Offset: 0x000E0E5E
		public SwitchFloor()
		{
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x000E2C6D File Offset: 0x000E0E6D
		public SwitchFloor(Vector2 tileLocation, Color onColor, Color offColor, bool on)
		{
			this.tileLocation = tileLocation;
			this.onColor = onColor;
			this.offColor = offColor;
			this.isOn = on;
			this.fragility = 2;
			this.name = "Switch Floor";
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x000E2CAC File Offset: 0x000E0EAC
		public void flip(GameLocation environment)
		{
			this.isOn = !this.isOn;
			this.glow = 0.65f;
			foreach (Vector2 v in Utility.getAdjacentTileLocations(this.tileLocation))
			{
				if (environment.objects.ContainsKey(v) && environment.objects[v] is SwitchFloor)
				{
					environment.objects[v].isOn = !environment.objects[v].isOn;
					(environment.objects[v] as SwitchFloor).glow = 0.3f;
				}
			}
			Game1.playSound("shiny4");
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000E2D84 File Offset: 0x000E0F84
		public void setSuccessCountdown(int ticks)
		{
			this.ticksToSuccess = ticks;
			this.glow = 0.5f;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x000E2D98 File Offset: 0x000E0F98
		public void checkForCompleteness()
		{
			Queue<Vector2> openList = new Queue<Vector2>();
			HashSet<Vector2> closedList = new HashSet<Vector2>();
			openList.Enqueue(this.tileLocation);
			Vector2 current = default(Vector2);
			List<Vector2> adjacent = new List<Vector2>();
			while (openList.Count > 0)
			{
				current = openList.Dequeue();
				if (Game1.currentLocation.objects.ContainsKey(current) && Game1.currentLocation.objects[current] is SwitchFloor && (Game1.currentLocation.objects[current] as SwitchFloor).isOn != this.isOn)
				{
					return;
				}
				closedList.Add(current);
				adjacent = Utility.getAdjacentTileLocations(current);
				for (int i = 0; i < adjacent.Count; i++)
				{
					if (!closedList.Contains(adjacent[i]) && Game1.currentLocation.objects.ContainsKey(current) && Game1.currentLocation.objects[current] is SwitchFloor)
					{
						openList.Enqueue(adjacent[i]);
					}
				}
				adjacent.Clear();
			}
			int successTicks = 5;
			foreach (Vector2 v in closedList)
			{
				if (Game1.currentLocation.objects.ContainsKey(v) && Game1.currentLocation.objects[v] is SwitchFloor)
				{
					(Game1.currentLocation.objects[v] as SwitchFloor).setSuccessCountdown(successTicks);
				}
				successTicks += 2;
			}
			int coins = (int)Math.Sqrt((double)closedList.Count) * 2;
			Vector2 treasurePosition = closedList.Last<Vector2>();
			while (Game1.currentLocation.isTileOccupiedByFarmer(treasurePosition) != null)
			{
				closedList.Remove(treasurePosition);
				if (closedList.Count > 0)
				{
					treasurePosition = closedList.Last<Vector2>();
				}
			}
			Game1.currentLocation.objects[treasurePosition] = new Chest(coins, null, treasurePosition, false);
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, treasurePosition * (float)Game1.tileSize, false, false));
			Game1.playSound("coin");
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool isPassable()
		{
			return true;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x000E2FD4 File Offset: 0x000E11D4
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 1280, Game1.tileSize, Game1.tileSize)), this.finished ? SwitchFloor.successColor : (this.isOn ? this.onColor : this.offColor), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-08f);
			if (this.glow > 0f)
			{
				spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 1280, Game1.tileSize, Game1.tileSize)), Color.White * this.glow, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 2E-08f);
			}
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000E30D8 File Offset: 0x000E12D8
		public override void updateWhenCurrentLocation(GameTime time)
		{
			if (this.glow > 0f)
			{
				this.glow -= 0.04f;
			}
			if (this.ticksToSuccess > 0)
			{
				this.ticksToSuccess--;
				if (this.ticksToSuccess == 0)
				{
					this.finished = true;
					this.glow += 0.2f;
					Game1.playSound("boulderCrack");
					return;
				}
			}
			else if (!this.finished)
			{
				using (List<Farmer>.Enumerator enumerator = Game1.currentLocation.getFarmers().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getTileLocation().Equals(this.tileLocation))
						{
							if (this.readyToflip)
							{
								this.flip(Game1.currentLocation);
								this.checkForCompleteness();
							}
							this.readyToflip = false;
							return;
						}
					}
				}
				this.readyToflip = true;
			}
		}

		// Token: 0x04000B44 RID: 2884
		public static Color successColor = Color.LightBlue;

		// Token: 0x04000B45 RID: 2885
		public Color onColor;

		// Token: 0x04000B46 RID: 2886
		public Color offColor;

		// Token: 0x04000B47 RID: 2887
		private bool readyToflip;

		// Token: 0x04000B48 RID: 2888
		public bool finished;

		// Token: 0x04000B49 RID: 2889
		private int ticksToSuccess = -1;

		// Token: 0x04000B4A RID: 2890
		private float glow;
	}
}
