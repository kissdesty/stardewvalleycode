using System;
using Microsoft.Xna.Framework;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000150 RID: 336
	public class EmilysParrot : TemporaryAnimatedSprite
	{
		// Token: 0x060012F4 RID: 4852 RVA: 0x0017EC98 File Offset: 0x0017CE98
		public EmilysParrot(Vector2 location)
		{
			base.Texture = Game1.mouseCursors;
			this.sourceRect = new Rectangle(92, 148, 9, 16);
			this.sourceRectStartingPos = new Vector2(92f, 149f);
			this.position = location;
			this.initialPosition = this.position;
			this.scale = (float)Game1.pixelZoom;
			this.id = 5858585f;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0017ED0B File Offset: 0x0017CF0B
		public void doAction()
		{
			Game1.soundBank.PlayCue("parrot");
			this.shakeTimer = 800;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0017ED28 File Offset: 0x0017CF28
		public override bool update(GameTime time)
		{
			this.currentPhaseTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.currentPhaseTimer <= 0)
			{
				this.currentPhase = Game1.random.Next(5);
				this.currentPhaseTimer = Game1.random.Next(4000, 16000);
				if (this.currentPhase == 1)
				{
					this.currentPhaseTimer /= 2;
					if (this.currentFrame == 0)
					{
						this.position.X = this.initialPosition.X;
					}
					else
					{
						this.position.X = this.initialPosition.X - (float)(Game1.pixelZoom * 2);
					}
				}
				else
				{
					this.position = this.initialPosition;
				}
			}
			if (this.shakeTimer > 0)
			{
				this.shakeIntensity = 1f;
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.shakeIntensity = 0f;
			}
			this.currentFrameTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.currentFrameTimer <= 0)
			{
				switch (this.currentPhase)
				{
				case 0:
					if (this.currentFrame == 7)
					{
						this.currentFrame = 0;
						this.currentFrameTimer = 600;
					}
					else if (Game1.random.NextDouble() < 0.5)
					{
						this.currentFrame = 7;
						this.currentFrameTimer = 300;
					}
					break;
				case 1:
					this.currentFrame = 6 - this.currentPhaseTimer % 1000 / 166;
					this.currentFrame = 3 - Math.Abs(this.currentFrame - 3);
					this.currentFrameTimer = 0;
					this.position.Y = this.initialPosition.Y - (float)(Game1.pixelZoom * (3 - this.currentFrame));
					if (this.currentFrame == 0)
					{
						this.position.X = this.initialPosition.X;
					}
					else
					{
						this.position.X = this.initialPosition.X - (float)(Game1.pixelZoom * 2);
					}
					break;
				case 2:
					this.currentFrame = Game1.random.Next(3, 5);
					this.currentFrameTimer = 1000;
					break;
				case 3:
					if (this.currentFrame == 5)
					{
						this.currentFrame = 6;
					}
					else
					{
						this.currentFrame = 5;
					}
					this.currentFrameTimer = 1000;
					break;
				case 4:
					if (this.currentFrame == 1 && Game1.random.NextDouble() < 0.1)
					{
						this.currentFrame = 2;
					}
					else if (this.currentFrame == 2)
					{
						this.currentFrame = 1;
					}
					else
					{
						this.currentFrame = Game1.random.Next(2);
					}
					this.currentFrameTimer = 500;
					break;
				}
			}
			if (this.currentPhase == 1 && this.currentFrame != 0)
			{
				this.sourceRect.X = 38 + this.currentFrame * 13;
				this.sourceRect.Width = 13;
			}
			else
			{
				this.sourceRect.X = 92 + this.currentFrame * 9;
				this.sourceRect.Width = 9;
			}
			return false;
		}

		// Token: 0x0400136C RID: 4972
		public const int flappingPhase = 1;

		// Token: 0x0400136D RID: 4973
		public const int hoppingPhase = 0;

		// Token: 0x0400136E RID: 4974
		public const int lookingSidewaysPhase = 2;

		// Token: 0x0400136F RID: 4975
		public const int nappingPhase = 3;

		// Token: 0x04001370 RID: 4976
		public const int headBobbingPhase = 4;

		// Token: 0x04001371 RID: 4977
		private int currentFrame;

		// Token: 0x04001372 RID: 4978
		private int currentFrameTimer;

		// Token: 0x04001373 RID: 4979
		private int currentPhaseTimer;

		// Token: 0x04001374 RID: 4980
		private int currentPhase;

		// Token: 0x04001375 RID: 4981
		private int shakeTimer;
	}
}
