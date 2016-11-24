using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200014E RID: 334
	public class Butterfly : Critter
	{
		// Token: 0x060012E9 RID: 4841 RVA: 0x0017E59C File Offset: 0x0017C79C
		public Butterfly(Vector2 position)
		{
			this.position = position * (float)Game1.tileSize;
			this.startingPosition = this.position;
			if (Game1.currentSeason.Equals("spring"))
			{
				this.baseFrame = ((Game1.random.NextDouble() < 0.5) ? (Game1.random.Next(3) * 3 + 160) : (Game1.random.Next(3) * 3 + 180));
			}
			else
			{
				this.baseFrame = ((Game1.random.NextDouble() < 0.5) ? (Game1.random.Next(3) * 4 + 128) : (Game1.random.Next(3) * 4 + 148));
				this.summerButterfly = true;
			}
			this.motion = new Vector2((float)(Game1.random.NextDouble() + 0.25) * 3f * (float)((Game1.random.NextDouble() < 0.5) ? -1 : 1) / 2f, (float)(Game1.random.NextDouble() + 0.5) * 3f * (float)((Game1.random.NextDouble() < 0.5) ? -1 : 1) / 2f);
			this.flapSpeed = Game1.random.Next(45, 80);
			this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 16, 16);
			this.sprite.loop = false;
			this.startingPosition = position;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0017E744 File Offset: 0x0017C944
		public void doneWithFlap(Farmer who)
		{
			this.flapTimer = 200 + Game1.random.Next(-5, 6);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0017E760 File Offset: 0x0017C960
		public override bool update(GameTime time, GameLocation environment)
		{
			this.flapTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.flapTimer <= 0 && this.sprite.currentAnimation == null)
			{
				this.motionMultiplier = 1f;
				this.motion.X = this.motion.X + (float)Game1.random.Next(-80, 81) / 100f;
				this.motion.Y = (float)(Game1.random.NextDouble() + 0.25) * -3f / 2f;
				if (Math.Abs(this.motion.X) > 1.5f)
				{
					this.motion.X = 3f * (float)Math.Sign(this.motion.X) / 2f;
				}
				if (Math.Abs(this.motion.Y) > 3f)
				{
					this.motion.Y = 3f * (float)Math.Sign(this.motion.Y);
				}
				if (this.summerButterfly)
				{
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 3, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame, this.flapSpeed, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneWithFlap), false)
					});
				}
				else
				{
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
						new FarmerSprite.AnimationFrame(this.baseFrame, this.flapSpeed, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneWithFlap), false)
					});
				}
			}
			this.position += this.motion * this.motionMultiplier;
			this.motion.Y = this.motion.Y + 0.005f * (float)time.ElapsedGameTime.Milliseconds;
			this.motionMultiplier -= 0.0005f * (float)time.ElapsedGameTime.Milliseconds;
			if (this.motionMultiplier <= 0f)
			{
				this.motionMultiplier = 0f;
			}
			return base.update(time, environment);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00002834 File Offset: 0x00000A34
		public override void draw(SpriteBatch b)
		{
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0017EA44 File Offset: 0x0017CC44
		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, -128f + this.yJumpOffset + this.yOffset)), this.position.Y / 10000f, 0, 0, Color.White, this.flip, 4f, 0f, false);
		}

		// Token: 0x0400135F RID: 4959
		public const float maxSpeed = 3f;

		// Token: 0x04001360 RID: 4960
		private int flapTimer;

		// Token: 0x04001361 RID: 4961
		private int checkForLandingSpotTimer;

		// Token: 0x04001362 RID: 4962
		private int landedTimer;

		// Token: 0x04001363 RID: 4963
		private int flapSpeed = 50;

		// Token: 0x04001364 RID: 4964
		private Vector2 motion;

		// Token: 0x04001365 RID: 4965
		private float motionMultiplier = 1f;

		// Token: 0x04001366 RID: 4966
		private bool summerButterfly;
	}
}
