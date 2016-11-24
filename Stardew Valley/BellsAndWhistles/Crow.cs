using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000157 RID: 343
	public class Crow : Critter
	{
		// Token: 0x06001312 RID: 4882 RVA: 0x00180790 File Offset: 0x0017E990
		public Crow(int tileX, int tileY) : base(14, new Vector2((float)(tileX * Game1.tileSize), (float)(tileY * Game1.tileSize)))
		{
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.position.X = this.position.X + (float)(Game1.tileSize / 2);
			this.position.Y = this.position.Y + (float)(Game1.tileSize / 2);
			this.startingPosition = this.position;
			this.state = 0;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00180816 File Offset: 0x0017EA16
		public void hop(Farmer who)
		{
			this.gravityAffectedDY = -4f;
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00180823 File Offset: 0x0017EA23
		private void donePecking(Farmer who)
		{
			this.state = ((Game1.random.NextDouble() < 0.5) ? 0 : 3);
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0017DF3D File Offset: 0x0017C13D
		private void playFlap(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("batFlap");
			}
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0017DF5B File Offset: 0x0017C15B
		private void playPeck(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("shiny4");
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00180844 File Offset: 0x0017EA44
		public override bool update(GameTime time, GameLocation environment)
		{
			Farmer f = Utility.isThereAFarmerWithinDistance(this.position / (float)Game1.tileSize, 4);
			if (this.yJumpOffset < 0f && this.state != 1)
			{
				if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X - 2f;
				}
				else if (!environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X + 2f;
				}
			}
			if (f != null && this.state != 1)
			{
				if (Game1.random.NextDouble() < 0.85)
				{
					Game1.playSound("crow");
				}
				this.state = 1;
				if (f.position.X > this.position.X)
				{
					this.flip = false;
				}
				else
				{
					this.flip = true;
				}
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 10)), 40, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 40),
					new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 40)
				});
				this.sprite.loop = true;
			}
			switch (this.state)
			{
			case 0:
				if (this.sprite.currentAnimation == null)
				{
					List<FarmerSprite.AnimationFrame> peckAnim = new List<FarmerSprite.AnimationFrame>();
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 480));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 170, false, this.flip, null, false));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 170, false, this.flip, null, false));
					int pecks = Game1.random.Next(1, 5);
					for (int i = 0; i < pecks; i++)
					{
						peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 70));
						peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 100, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playPeck), false));
					}
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 100));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 70, false, this.flip, null, false));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 70, false, this.flip, null, false));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 500, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.donePecking), false));
					this.sprite.loop = false;
					this.sprite.setCurrentAnimation(peckAnim);
				}
				break;
			case 1:
				if (!this.flip)
				{
					this.position.X = this.position.X - 6f;
				}
				else
				{
					this.position.X = this.position.X + 6f;
				}
				this.yOffset -= 2f;
				break;
			case 2:
				if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame + 5;
				}
				if (Game1.random.NextDouble() < 0.003 && this.sprite.currentAnimation == null)
				{
					this.state = 3;
				}
				break;
			case 3:
				if (Game1.random.NextDouble() < 0.008 && this.sprite.currentAnimation == null && this.yJumpOffset >= 0f)
				{
					switch (Game1.random.Next(5))
					{
					case 0:
						this.state = 2;
						break;
					case 1:
						this.state = 0;
						break;
					case 2:
						this.hop(null);
						break;
					case 3:
						this.flip = !this.flip;
						this.hop(null);
						break;
					case 4:
						this.state = 1;
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 10)), 50, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 9)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 50),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 50)
						});
						this.sprite.loop = true;
						break;
					}
				}
				else if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame;
				}
				break;
			}
			return base.update(time, environment);
		}

		// Token: 0x0400138E RID: 5006
		public const int flyingSpeed = 6;

		// Token: 0x0400138F RID: 5007
		public const int pecking = 0;

		// Token: 0x04001390 RID: 5008
		public const int flyingAway = 1;

		// Token: 0x04001391 RID: 5009
		public const int sleeping = 2;

		// Token: 0x04001392 RID: 5010
		public const int stopped = 3;

		// Token: 0x04001393 RID: 5011
		private int state;
	}
}
