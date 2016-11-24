using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200015D RID: 349
	public class Seagull : Critter
	{
		// Token: 0x0600133F RID: 4927 RVA: 0x00182E1F File Offset: 0x0018101F
		public Seagull(Vector2 position, int startingState) : base(0, position)
		{
			this.moveLeft = (Game1.random.NextDouble() < 0.5);
			this.startingPosition = position;
			this.state = startingState;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00180816 File Offset: 0x0017EA16
		public void hop(Farmer who)
		{
			this.gravityAffectedDY = -4f;
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00182E60 File Offset: 0x00181060
		public override bool update(GameTime time, GameLocation environment)
		{
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer < 0)
			{
				Character f = Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 4, environment);
				this.characterCheckTimer = 200;
				if (f != null && this.state != 1)
				{
					if (Game1.random.NextDouble() < 0.25)
					{
						Game1.playSound("seagulls");
					}
					this.state = 1;
					if (f.position.X > this.position.X)
					{
						this.moveLeft = true;
					}
					else
					{
						this.moveLeft = false;
					}
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 10)), 80),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 11)), 80),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 12)), 80),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 13)), 100)
					});
					this.sprite.loop = true;
				}
			}
			switch (this.state)
			{
			case 0:
				if (this.moveLeft && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X - 2f;
				}
				else if (!this.moveLeft && !environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X + 2f;
				}
				if (Game1.random.NextDouble() < 0.005)
				{
					this.state = 3;
					this.sprite.loop = false;
					this.sprite.currentAnimation = null;
					this.sprite.CurrentFrame = 0;
				}
				break;
			case 1:
				if (this.moveLeft)
				{
					this.position.X = this.position.X - 4f;
				}
				else
				{
					this.position.X = this.position.X + 4f;
				}
				this.yOffset -= 2f;
				break;
			case 2:
			{
				this.sprite.CurrentFrame = this.baseFrame + 9;
				float tmpY = this.yOffset;
				if ((time.TotalGameTime.TotalMilliseconds + (double)((int)this.position.X * 4)) % 2000.0 < 1000.0)
				{
					this.yOffset = 2f;
				}
				else
				{
					this.yOffset = 0f;
				}
				if (this.yOffset > tmpY)
				{
					environment.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f, 8, 0, new Vector2(this.position.X - (float)(Game1.tileSize / 2), this.position.Y - (float)(Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 1f, 0.003f, 0f, 0f, false));
				}
				break;
			}
			case 3:
				if (Game1.random.NextDouble() < 0.003 && this.sprite.currentAnimation == null)
				{
					this.sprite.loop = false;
					switch (Game1.random.Next(4))
					{
					case 0:
					{
						List<FarmerSprite.AnimationFrame> frames = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 100),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 100),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 200),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 5)), 200)
						};
						int extra = Game1.random.Next(5);
						for (int i = 0; i < extra; i++)
						{
							frames.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 200));
							frames.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 5)), 200));
						}
						this.sprite.setCurrentAnimation(frames);
						break;
					}
					case 1:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(6, (int)((short)Game1.random.Next(500, 4000)))
						});
						break;
					case 2:
					{
						List<FarmerSprite.AnimationFrame> frames = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 500),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 100, false, false, new AnimatedSprite.endOfAnimationBehavior(this.hop), false),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 100)
						};
						int extra = Game1.random.Next(3);
						for (int j = 0; j < extra; j++)
						{
							frames.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 100));
							frames.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 100));
						}
						this.sprite.setCurrentAnimation(frames);
						break;
					}
					case 3:
						this.state = 0;
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 200),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 200)
						});
						this.sprite.loop = true;
						this.moveLeft = (Game1.random.NextDouble() < 0.5);
						if (Game1.random.NextDouble() < 0.33)
						{
							if (this.position.X > this.startingPosition.X)
							{
								this.moveLeft = true;
							}
							else
							{
								this.moveLeft = false;
							}
						}
						break;
					}
				}
				else if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame;
				}
				break;
			}
			this.flip = !this.moveLeft;
			return base.update(time, environment);
		}

		// Token: 0x040013CE RID: 5070
		public const int walkingSpeed = 2;

		// Token: 0x040013CF RID: 5071
		public const int flyingSpeed = 4;

		// Token: 0x040013D0 RID: 5072
		public const int walking = 0;

		// Token: 0x040013D1 RID: 5073
		public const int flyingAway = 1;

		// Token: 0x040013D2 RID: 5074
		public const int flyingToLand = 4;

		// Token: 0x040013D3 RID: 5075
		public const int swimming = 2;

		// Token: 0x040013D4 RID: 5076
		public const int stopped = 3;

		// Token: 0x040013D5 RID: 5077
		private int state;

		// Token: 0x040013D6 RID: 5078
		private int characterCheckTimer = 200;

		// Token: 0x040013D7 RID: 5079
		private bool moveLeft;
	}
}
