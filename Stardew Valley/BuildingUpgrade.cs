using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200000F RID: 15
	public class BuildingUpgrade
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00007F34 File Offset: 0x00006134
		public BuildingUpgrade(string whichBuilding, Vector2 positionOfCarpenter)
		{
			this.whichBuilding = whichBuilding;
			this.positionOfCarpenter = positionOfCarpenter;
			this.positionOfCarpenter.X = this.positionOfCarpenter.X * (float)Game1.tileSize;
			this.positionOfCarpenter.X = this.positionOfCarpenter.X - (float)(Game1.tileSize / 10);
			this.positionOfCarpenter.Y = this.positionOfCarpenter.Y * (float)Game1.tileSize;
			this.positionOfCarpenter.Y = this.positionOfCarpenter.Y - (float)(Game1.tileSize / 2);
			this.workerTexture = Game1.content.Load<Texture2D>("LooseSprites\\robinAtWork");
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007FE8 File Offset: 0x000061E8
		public BuildingUpgrade()
		{
			this.workerTexture = Game1.content.Load<Texture2D>("LooseSprites\\robinAtWork");
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00008034 File Offset: 0x00006234
		public Rectangle getSourceRectangle()
		{
			return new Rectangle(this.currentFrame * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00008058 File Offset: 0x00006258
		public void update(float milliseconds)
		{
			this.timeAccumulator += milliseconds;
			if (this.numberOfHammers > 0 && (this.timeAccumulator > (float)this.hammerPause || (this.timeAccumulator > (float)(this.hammerPause / 3) && this.currentFrame == 3)))
			{
				this.timeAccumulator = 0f;
				switch (this.currentFrame)
				{
				case 0:
					this.currentFrame = 1;
					Game1.playSound("woodyStep");
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					this.currentFrame = 0;
					break;
				}
				this.currentHammer++;
				if (this.currentHammer >= this.numberOfHammers && this.currentFrame == 0)
				{
					this.currentHammer = 0;
					this.numberOfHammers = 0;
					this.pauseTime = Game1.random.Next(800, 3000);
					this.currentFrame = ((Game1.random.NextDouble() < 0.2) ? 4 : 2);
					return;
				}
			}
			else if (this.timeAccumulator > (float)this.pauseTime)
			{
				this.timeAccumulator = 0f;
				this.numberOfHammers = Game1.random.Next(2, 14);
				this.currentFrame = 3;
				this.hammerPause = Game1.random.Next(400, 700);
			}
		}

		// Token: 0x040000AE RID: 174
		public int daysLeftTillUpgradeDone = 4;

		// Token: 0x040000AF RID: 175
		public string whichBuilding;

		// Token: 0x040000B0 RID: 176
		public Vector2 positionOfCarpenter;

		// Token: 0x040000B1 RID: 177
		[XmlIgnore]
		public Texture2D workerTexture;

		// Token: 0x040000B2 RID: 178
		private int currentFrame = 2;

		// Token: 0x040000B3 RID: 179
		private int numberOfHammers;

		// Token: 0x040000B4 RID: 180
		private int currentHammer;

		// Token: 0x040000B5 RID: 181
		private int pauseTime = 1000;

		// Token: 0x040000B6 RID: 182
		private int hammerPause = 500;

		// Token: 0x040000B7 RID: 183
		private float timeAccumulator;
	}
}
