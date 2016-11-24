using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000EF RID: 239
	public class Fish
	{
		// Token: 0x06000E5B RID: 3675 RVA: 0x001253DC File Offset: 0x001235DC
		public Fish(int whichFish)
		{
			this.whichFish = whichFish;
			this.fishingField = new Rectangle(0, 0, 1028, 612);
			Dictionary<int, string> data = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
			if (data.ContainsKey(whichFish))
			{
				string[] rawData = data[whichFish].Split(new char[]
				{
					'/'
				});
				this.fishName = rawData[0];
				this.chanceToDart = (float)Convert.ToInt32(rawData[1]);
				this.dartingRandomness = (float)Convert.ToInt32(rawData[2]);
				this.dartingIntensity = (float)Convert.ToInt32(rawData[3]);
				this.dartingDuration = (float)Convert.ToInt32(rawData[4]);
				this.turnFrequency = (float)Convert.ToInt32(rawData[5]);
				this.turnSpeed = (float)Convert.ToInt32(rawData[6]);
				this.turnIntensity = (float)Convert.ToInt32(rawData[7]);
				this.minSpeed = (float)Convert.ToInt32(rawData[8]);
				this.maxSpeed = (float)Convert.ToInt32(rawData[9]);
				this.speedChangeFrequency = (float)Convert.ToInt32(rawData[10]);
				this.bobberDifficulty = Convert.ToInt32(rawData[11]);
			}
			this.position = new Vector2(514f, 306f);
			this.targetSpeed = this.minSpeed / 50f;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0012552B File Offset: 0x0012372B
		public bool isWithinRectangle(Rectangle r, int xPositionOfFishingField, int yPositionOfFishingField)
		{
			return r.Contains((int)this.position.X + xPositionOfFishingField, (int)this.position.Y + yPositionOfFishingField);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00125558 File Offset: 0x00123758
		public void Update(GameTime time)
		{
			this.animationTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.animationTimer <= 0)
			{
				this.animationTimer = 65 - (int)(this.currentSpeed * 10f);
				this.indexOfAnimation = (this.indexOfAnimation + 1) % 8;
			}
			if (!this.isDarting && Game1.random.NextDouble() < (double)(this.chanceToDart / 10000f))
			{
				this.rotation += (float)((double)Game1.random.Next(-(int)this.dartingRandomness, (int)this.dartingRandomness) * 3.1415926535897931 / 100.0);
				this.targetSpeed = this.rotation;
				this.dartingExtraSpeed = this.dartingIntensity / 20f;
				this.dartingExtraSpeed *= 1f + (float)Game1.random.Next(-10, 10) / 100f;
				this.dartingTimer = this.dartingDuration * 10f + (float)Game1.random.Next(-(int)this.dartingDuration, (int)this.dartingDuration) * 0.1f;
				this.isDarting = true;
			}
			if (this.dartingTimer > 0f)
			{
				this.dartingTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.dartingTimer <= 0f && this.isDarting)
				{
					this.isDarting = false;
					this.dartingTimer = this.dartingDuration * 10f + (float)Game1.random.Next(-(int)this.dartingDuration, (int)this.dartingDuration) * 0.1f;
				}
				if (!this.isDarting)
				{
					this.dartingExtraSpeed -= this.dartingExtraSpeed * 0.0005f * (float)time.ElapsedGameTime.Milliseconds;
				}
			}
			if (Game1.random.NextDouble() < (double)(this.turnFrequency / 10000f))
			{
				this.targetRotation = (float)((double)((float)Game1.random.Next((int)(-(int)this.turnIntensity), (int)this.turnIntensity) / 100f) * 3.1415926535897931);
			}
			if (Game1.random.NextDouble() < (double)(this.speedChangeFrequency / 10000f))
			{
				this.targetSpeed = (float)((int)((float)Game1.random.Next((int)this.minSpeed, (int)this.maxSpeed) / 20f));
			}
			if (Math.Abs(this.rotation - this.targetRotation) > Math.Abs(this.targetRotation / (100f - this.turnSpeed)))
			{
				this.rotation += this.targetRotation / (100f - this.turnSpeed);
			}
			this.rotation %= 6.28318548f;
			this.currentSpeed += (this.targetSpeed - this.currentSpeed) / 10f;
			this.currentSpeed = Math.Min(this.maxSpeed / 20f, this.currentSpeed);
			this.currentSpeed = Math.Max(this.minSpeed / 20f, this.currentSpeed);
			this.position.X = this.position.X + (float)((double)this.currentSpeed * Math.Cos((double)this.rotation));
			int wallsHit = 0;
			if (!this.fishingField.Contains(new Rectangle((int)this.position.X - Game1.tileSize / 2, (int)this.position.Y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize)))
			{
				Vector2 cartesian = new Vector2(this.currentSpeed * (float)Math.Cos((double)this.rotation), this.currentSpeed * (float)Math.Sin((double)this.rotation));
				cartesian.X = -cartesian.X;
				this.rotation = (float)Math.Atan((double)(cartesian.Y / cartesian.X));
				if (cartesian.X < 0f)
				{
					this.rotation += 3.14159274f;
				}
				else if (cartesian.Y < 0f)
				{
					this.rotation += 1.57079637f;
				}
				this.position.X = this.position.X + (float)((double)this.currentSpeed * Math.Cos((double)this.rotation));
				wallsHit++;
			}
			this.position.Y = this.position.Y + (float)((double)this.currentSpeed * Math.Sin((double)this.rotation));
			if (!this.fishingField.Contains(new Rectangle((int)this.position.X - Game1.tileSize / 2, (int)this.position.Y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize)))
			{
				Vector2 cartesian2 = new Vector2(this.currentSpeed * (float)Math.Cos((double)this.rotation), this.currentSpeed * (float)Math.Sin((double)this.rotation));
				cartesian2.Y = -cartesian2.Y;
				this.rotation = (float)Math.Atan((double)(cartesian2.Y / cartesian2.X));
				if (cartesian2.X < 0f)
				{
					this.rotation += 3.14159274f;
				}
				else if (cartesian2.Y > 0f)
				{
					this.rotation += 1.57079637f;
				}
				this.position.Y = this.position.Y + (float)((double)this.currentSpeed * Math.Sin((double)this.rotation));
				wallsHit++;
			}
			if (wallsHit >= 2)
			{
				Vector2 targetLocation = Utility.getVelocityTowardPoint(new Point((int)this.position.X, (int)this.position.Y), new Vector2(514f, 306f), this.currentSpeed);
				this.rotation = (float)Math.Atan((double)(targetLocation.Y / targetLocation.X));
				if (targetLocation.X < 0f)
				{
					this.rotation += 3.14159274f;
				}
				else if (targetLocation.Y < 0f)
				{
					this.rotation += 1.57079637f;
				}
				this.position.X = this.position.X + (float)((double)this.currentSpeed * Math.Cos((double)this.rotation));
				this.position.Y = this.position.Y + (float)((double)this.currentSpeed * Math.Sin((double)this.rotation));
				return;
			}
			if (wallsHit == 1)
			{
				this.targetRotation = this.rotation;
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00125BD0 File Offset: 0x00123DD0
		public void draw(SpriteBatch b, Vector2 positionOfFishingField)
		{
			b.Draw(Game1.mouseCursors, this.position + positionOfFishingField, new Rectangle?(new Rectangle(561, 1846 + this.indexOfAnimation * 16, 16, 16)), Color.White, this.rotation + 1.57079637f, new Vector2(8f, 8f), 4f, SpriteEffects.None, 0.5f);
		}

		// Token: 0x04000F53 RID: 3923
		public const int widthOfTrack = 1020;

		// Token: 0x04000F54 RID: 3924
		public const int msPerFrame = 65;

		// Token: 0x04000F55 RID: 3925
		public const int fishingFieldWidth = 1028;

		// Token: 0x04000F56 RID: 3926
		public const int fishingFieldHeight = 612;

		// Token: 0x04000F57 RID: 3927
		public int whichFish;

		// Token: 0x04000F58 RID: 3928
		public int indexOfAnimation;

		// Token: 0x04000F59 RID: 3929
		public int animationTimer = 65;

		// Token: 0x04000F5A RID: 3930
		public float chanceToDart;

		// Token: 0x04000F5B RID: 3931
		public float dartingRandomness;

		// Token: 0x04000F5C RID: 3932
		public float dartingIntensity;

		// Token: 0x04000F5D RID: 3933
		public float dartingDuration;

		// Token: 0x04000F5E RID: 3934
		public float dartingTimer;

		// Token: 0x04000F5F RID: 3935
		public float dartingExtraSpeed;

		// Token: 0x04000F60 RID: 3936
		public float turnFrequency;

		// Token: 0x04000F61 RID: 3937
		public float turnSpeed;

		// Token: 0x04000F62 RID: 3938
		public float turnIntensity;

		// Token: 0x04000F63 RID: 3939
		public float minSpeed;

		// Token: 0x04000F64 RID: 3940
		public float maxSpeed;

		// Token: 0x04000F65 RID: 3941
		public float speedChangeFrequency;

		// Token: 0x04000F66 RID: 3942
		public float currentSpeed;

		// Token: 0x04000F67 RID: 3943
		public float targetSpeed;

		// Token: 0x04000F68 RID: 3944
		public float positionOnTrack = 510f;

		// Token: 0x04000F69 RID: 3945
		public Vector2 position;

		// Token: 0x04000F6A RID: 3946
		public float rotation;

		// Token: 0x04000F6B RID: 3947
		public float targetRotation;

		// Token: 0x04000F6C RID: 3948
		public bool isDarting;

		// Token: 0x04000F6D RID: 3949
		public Rectangle fishingField;

		// Token: 0x04000F6E RID: 3950
		private string fishName;

		// Token: 0x04000F6F RID: 3951
		public int bobberDifficulty;
	}
}
