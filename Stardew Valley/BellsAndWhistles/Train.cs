using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200015F RID: 351
	public class Train
	{
		// Token: 0x06001348 RID: 4936 RVA: 0x00183C00 File Offset: 0x00181E00
		public Train()
		{
			Random r = new Random();
			if (r.NextDouble() < 0.1)
			{
				this.type = 3;
			}
			else if (r.NextDouble() < 0.1)
			{
				this.type = 1;
			}
			else if (r.NextDouble() < 0.1)
			{
				this.type = 2;
			}
			else if (r.NextDouble() < 0.05)
			{
				this.type = 5;
			}
			else if (Game1.currentSeason.ToLower().Equals("winter") && r.NextDouble() < 0.2)
			{
				this.type = 6;
			}
			else
			{
				this.type = 0;
			}
			int numCars = r.Next(8, 25);
			if (r.NextDouble() < 0.1)
			{
				numCars *= 2;
			}
			this.speed = 0.2f;
			this.smokeTimer = this.speed * 2000f;
			Color color = Color.White;
			double chanceForPassengerCar = 1.0;
			double chanceForCoalCar = 1.0;
			switch (this.type)
			{
			case 0:
				chanceForPassengerCar = 0.2;
				chanceForCoalCar = 0.2;
				break;
			case 1:
				chanceForPassengerCar = 0.0;
				chanceForCoalCar = 0.0;
				color = Color.DimGray;
				break;
			case 2:
				chanceForPassengerCar = 0.0;
				chanceForCoalCar = 0.7;
				break;
			case 3:
				chanceForPassengerCar = 1.0;
				chanceForCoalCar = 0.0;
				this.speed = 0.4f;
				break;
			case 5:
				chanceForCoalCar = 0.0;
				chanceForPassengerCar = 0.0;
				color = Color.MediumBlue;
				this.speed = 0.4f;
				break;
			case 6:
				chanceForPassengerCar = 0.0;
				chanceForCoalCar = 1.0;
				color = Color.Red;
				break;
			}
			this.cars.Add(new TrainCar(r, 3, -1, Color.White, 0, 0));
			for (int i = 1; i < numCars; i++)
			{
				int whichCar = 0;
				if (r.NextDouble() < chanceForPassengerCar)
				{
					whichCar = 2;
				}
				else if (r.NextDouble() < chanceForCoalCar)
				{
					whichCar = 1;
				}
				Color carColor = color;
				if (color.Equals(Color.White))
				{
					bool redTint = false;
					bool greenTint = false;
					bool blueTint = false;
					switch (r.Next(3))
					{
					case 0:
						redTint = true;
						break;
					case 1:
						greenTint = true;
						break;
					case 2:
						blueTint = true;
						break;
					}
					carColor = new Color(r.Next(redTint ? 0 : 100, 250), r.Next(greenTint ? 0 : 100, 250), r.Next(blueTint ? 0 : 100, 250));
				}
				int frontDecal = -1;
				if (this.type == 1)
				{
					frontDecal = 2;
				}
				else if (this.type == 5)
				{
					frontDecal = 1;
				}
				else if (this.type == 6)
				{
					frontDecal = -1;
				}
				else if (r.NextDouble() < 0.3)
				{
					frontDecal = r.Next(35);
				}
				int resourceType = 0;
				if (whichCar == 1)
				{
					resourceType = r.Next(9);
					if (this.type == 6)
					{
						resourceType = 9;
					}
				}
				this.cars.Add(new TrainCar(r, whichCar, frontDecal, carColor, resourceType, r.Next(1, 3)));
			}
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00183F60 File Offset: 0x00182160
		public Rectangle getBoundingBox()
		{
			return new Rectangle(-this.cars.Count * 128 * 4 + (int)this.position, 45 * Game1.tileSize - Game1.tileSize * 2 - 32, this.cars.Count * 128 * 4, Game1.tileSize * 2);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00183FBC File Offset: 0x001821BC
		public bool Update(GameTime time, GameLocation location)
		{
			this.position += (float)time.ElapsedGameTime.Milliseconds * this.speed;
			this.wheelRotation += (float)time.ElapsedGameTime.Milliseconds * 0.0122718466f;
			this.wheelRotation %= 6.28318548f;
			foreach (Farmer f in location.getFarmers())
			{
				if (f.GetBoundingBox().Intersects(this.getBoundingBox()))
				{
					f.xVelocity = 8f;
					f.yVelocity = (float)(this.getBoundingBox().Center.Y - f.GetBoundingBox().Center.Y) / 4f;
					Game1.farmerTakeDamage(20, true, null);
					if (f.usingTool)
					{
						Game1.playSound("clank");
					}
				}
			}
			if (Game1.random.NextDouble() < 0.001 && location.Equals(Game1.currentLocation))
			{
				Game1.playSound("trainWhistle");
				this.whistleSteam = new TemporaryAnimatedSprite(27, new Vector2(this.position - 250f, (float)(45 * Game1.tileSize - Game1.tileSize * 4)), Color.White, 8, false, 100f, 0, Game1.tileSize, 1f, Game1.tileSize, 0);
			}
			if (this.whistleSteam != null)
			{
				this.whistleSteam.Position = new Vector2(this.position - 258f, (float)(45 * Game1.tileSize - Game1.tileSize * 4 - 32));
				if (this.whistleSteam.update(time))
				{
					this.whistleSteam = null;
				}
			}
			this.smokeTimer -= (float)time.ElapsedGameTime.Milliseconds;
			if (this.smokeTimer <= 0f)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2(this.position - 170f, (float)(45 * Game1.tileSize - Game1.tileSize * 6)), Color.White, 8, false, 100f, 0, Game1.tileSize, 1f, Game1.tileSize * 2, 0));
				this.smokeTimer = this.speed * 2000f;
			}
			return this.position > (float)(this.cars.Count * 128 * 4 + 70 * Game1.tileSize);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00184250 File Offset: 0x00182450
		public void draw(SpriteBatch b)
		{
			for (int i = 0; i < this.cars.Count; i++)
			{
				this.cars[i].draw(b, new Vector2(this.position - (float)((i + 1) * 512), (float)(45 * Game1.tileSize - Game1.tileSize * 4 - 32)), this.wheelRotation);
			}
			if (this.whistleSteam != null)
			{
				this.whistleSteam.draw(b, false, 0, 0);
			}
		}

		// Token: 0x040013EA RID: 5098
		public const int minCars = 8;

		// Token: 0x040013EB RID: 5099
		public const int maxCars = 24;

		// Token: 0x040013EC RID: 5100
		public const double chanceForLongTrain = 0.1;

		// Token: 0x040013ED RID: 5101
		public const int randomTrain = 0;

		// Token: 0x040013EE RID: 5102
		public const int jojaTrain = 1;

		// Token: 0x040013EF RID: 5103
		public const int coalTrain = 2;

		// Token: 0x040013F0 RID: 5104
		public const int passengerTrain = 3;

		// Token: 0x040013F1 RID: 5105
		public const int uniformColorPlainTrain = 4;

		// Token: 0x040013F2 RID: 5106
		public const int prisonTrain = 5;

		// Token: 0x040013F3 RID: 5107
		public const int christmasTrain = 6;

		// Token: 0x040013F4 RID: 5108
		public List<TrainCar> cars = new List<TrainCar>();

		// Token: 0x040013F5 RID: 5109
		public int type;

		// Token: 0x040013F6 RID: 5110
		public float position;

		// Token: 0x040013F7 RID: 5111
		public float speed;

		// Token: 0x040013F8 RID: 5112
		public float wheelRotation;

		// Token: 0x040013F9 RID: 5113
		public float smokeTimer;

		// Token: 0x040013FA RID: 5114
		private TemporaryAnimatedSprite whistleSteam;
	}
}
