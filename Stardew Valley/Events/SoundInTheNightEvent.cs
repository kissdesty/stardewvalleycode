using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;

namespace StardewValley.Events
{
	// Token: 0x0200013E RID: 318
	public class SoundInTheNightEvent : FarmEvent
	{
		// Token: 0x060011FA RID: 4602 RVA: 0x0016FEED File Offset: 0x0016E0ED
		public SoundInTheNightEvent(int which)
		{
			this.behavior = which;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0016FEFC File Offset: 0x0016E0FC
		public bool setUp()
		{
			Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			Farm f = Game1.getLocationFromName("Farm") as Farm;
			switch (this.behavior)
			{
			case 0:
				this.soundName = "UFO";
				this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_UFO", new object[0]);
				for (int attempts = 50; attempts > 0; attempts--)
				{
					this.targetLocation = new Vector2((float)r.Next(5, f.map.GetLayer("Back").TileWidth - 4), (float)r.Next(5, f.map.GetLayer("Back").TileHeight - 4));
					if (!f.isTileLocationTotallyClearAndPlaceable(this.targetLocation))
					{
						return true;
					}
				}
				break;
			case 1:
			{
				this.soundName = "Meteorite";
				this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_Meteorite", new object[0]);
				this.targetLocation = new Vector2((float)r.Next(5, f.map.GetLayer("Back").TileWidth - 20), (float)r.Next(5, f.map.GetLayer("Back").TileHeight - 4));
				int i = (int)this.targetLocation.X;
				while ((float)i <= this.targetLocation.X + 1f)
				{
					int j = (int)this.targetLocation.Y;
					while ((float)j <= this.targetLocation.Y + 1f)
					{
						Vector2 v = new Vector2((float)i, (float)j);
						if (!f.isTileOpenBesidesTerrainFeatures(v) || !f.isTileOpenBesidesTerrainFeatures(new Vector2(v.X + 1f, v.Y)) || !f.isTileOpenBesidesTerrainFeatures(new Vector2(v.X + 1f, v.Y - 1f)) || !f.isTileOpenBesidesTerrainFeatures(new Vector2(v.X, v.Y - 1f)) || f.doesTileHaveProperty((int)v.X, (int)v.Y, "Water", "Back") != null || f.doesTileHaveProperty((int)v.X + 1, (int)v.Y, "Water", "Back") != null)
						{
							return true;
						}
						j++;
					}
					i++;
				}
				break;
			}
			case 2:
				this.soundName = "dogs";
				if (r.NextDouble() < 0.5)
				{
					return true;
				}
				foreach (Building b in f.buildings)
				{
					if (b.indoors != null && b.indoors is AnimalHouse && !b.animalDoorOpen && (b.indoors as AnimalHouse).animalsThatLiveHere.Count > (b.indoors as AnimalHouse).animals.Count && r.NextDouble() < (double)(1f / (float)f.buildings.Count))
					{
						this.targetBuilding = b;
						break;
					}
				}
				return this.targetBuilding == null;
			case 3:
				this.soundName = "owl";
				for (int attempts = 50; attempts > 0; attempts--)
				{
					this.targetLocation = new Vector2((float)r.Next(5, f.map.GetLayer("Back").TileWidth - 4), (float)r.Next(5, f.map.GetLayer("Back").TileHeight - 4));
					if (!f.isTileLocationTotallyClearAndPlaceable(this.targetLocation))
					{
						return true;
					}
				}
				break;
			case 4:
				this.soundName = "thunder_small";
				this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_Earthquake", new object[0]);
				break;
			}
			Game1.freezeControls = true;
			return false;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00170310 File Offset: 0x0016E510
		public bool tickUpdate(GameTime time)
		{
			this.timer += time.ElapsedGameTime.Milliseconds;
			if (this.timer > 1500 && !this.playedSound)
			{
				if (this.soundName != null && !this.soundName.Equals(""))
				{
					Game1.playSound(this.soundName);
					this.playedSound = true;
				}
				if (!this.playedSound && this.message != null)
				{
					Game1.drawObjectDialogue(this.message);
					Game1.globalFadeToClear(null, 0.02f);
					this.showedMessage = true;
				}
			}
			if (this.timer > 7000 && !this.showedMessage)
			{
				Game1.pauseThenMessage(10, this.message, false);
				this.showedMessage = true;
			}
			if (this.showedMessage && this.playedSound)
			{
				Game1.freezeControls = false;
				return true;
			}
			return false;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x001703EC File Offset: 0x0016E5EC
		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Black);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00170440 File Offset: 0x0016E640
		public void makeChangesToLocation()
		{
			Farm f = Game1.getLocationFromName("Farm") as Farm;
			switch (this.behavior)
			{
			case 0:
			{
				Object o = new Object(this.targetLocation, 96, false);
				o.minutesUntilReady = 24000 - Game1.timeOfDay;
				f.objects.Add(this.targetLocation, o);
				return;
			}
			case 1:
				if (f.terrainFeatures.ContainsKey(this.targetLocation))
				{
					f.terrainFeatures.Remove(this.targetLocation);
				}
				if (f.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(1f, 0f)))
				{
					f.terrainFeatures.Remove(this.targetLocation + new Vector2(1f, 0f));
				}
				if (f.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(1f, 1f)))
				{
					f.terrainFeatures.Remove(this.targetLocation + new Vector2(1f, 1f));
				}
				if (f.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(0f, 1f)))
				{
					f.terrainFeatures.Remove(this.targetLocation + new Vector2(0f, 1f));
				}
				f.resourceClumps.Add(new ResourceClump(622, 2, 2, this.targetLocation));
				return;
			case 2:
			{
				AnimalHouse indoors = this.targetBuilding.indoors as AnimalHouse;
				long idOfRemove = 0L;
				foreach (long a in indoors.animalsThatLiveHere)
				{
					if (!indoors.animals.ContainsKey(a))
					{
						idOfRemove = a;
						break;
					}
				}
				if (!Game1.getFarm().animals.ContainsKey(idOfRemove))
				{
					return;
				}
				Game1.getFarm().animals.Remove(idOfRemove);
				indoors.animalsThatLiveHere.Remove(idOfRemove);
				using (Dictionary<long, FarmAnimal>.Enumerator enumerator2 = Game1.getFarm().animals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<long, FarmAnimal> a2 = enumerator2.Current;
						a2.Value.moodMessage = 5;
					}
					return;
				}
				break;
			}
			case 3:
				break;
			default:
				return;
			}
			f.objects.Add(this.targetLocation, new Object(this.targetLocation, 95, false));
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00002834 File Offset: 0x00000A34
		public void drawAboveEverything(SpriteBatch b)
		{
		}

		// Token: 0x040012CF RID: 4815
		public const int cropCircle = 0;

		// Token: 0x040012D0 RID: 4816
		public const int meteorite = 1;

		// Token: 0x040012D1 RID: 4817
		public const int dogs = 2;

		// Token: 0x040012D2 RID: 4818
		public const int owl = 3;

		// Token: 0x040012D3 RID: 4819
		public const int earthquake = 4;

		// Token: 0x040012D4 RID: 4820
		private int behavior;

		// Token: 0x040012D5 RID: 4821
		private int timer;

		// Token: 0x040012D6 RID: 4822
		private string soundName;

		// Token: 0x040012D7 RID: 4823
		private string message;

		// Token: 0x040012D8 RID: 4824
		private bool playedSound;

		// Token: 0x040012D9 RID: 4825
		private bool showedMessage;

		// Token: 0x040012DA RID: 4826
		private Vector2 targetLocation;

		// Token: 0x040012DB RID: 4827
		private Building targetBuilding;
	}
}
