using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200014D RID: 333
	public class Birdie : Critter
	{
		// Token: 0x060012E1 RID: 4833 RVA: 0x0017DE44 File Offset: 0x0017C044
		public Birdie(int tileX, int tileY, int startingIndex = 25) : base(startingIndex, new Vector2((float)(tileX * Game1.tileSize), (float)(tileY * Game1.tileSize)))
		{
			this.flip = (Game1.random.NextDouble() < 0.5);
			this.position.X = this.position.X + (float)(Game1.tileSize / 2);
			this.position.Y = this.position.Y + (float)(Game1.tileSize / 2);
			this.startingPosition = this.position;
			this.flightOffset = (float)Game1.random.NextDouble() - 0.5f;
			this.state = 0;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0017DEEB File Offset: 0x0017C0EB
		public void hop(Farmer who)
		{
			this.gravityAffectedDY = -2f;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0017DEF8 File Offset: 0x0017C0F8
		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			if (this.state == 1)
			{
				base.draw(b);
			}
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0017DF0A File Offset: 0x0017C10A
		public override void draw(SpriteBatch b)
		{
			if (this.state != 1)
			{
				base.draw(b);
			}
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0017DF1C File Offset: 0x0017C11C
		private void donePecking(Farmer who)
		{
			this.state = ((Game1.random.NextDouble() < 0.5) ? 0 : 3);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0017DF3D File Offset: 0x0017C13D
		private void playFlap(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("batFlap");
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0017DF5B File Offset: 0x0017C15B
		private void playPeck(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("shiny4");
			}
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0017DF7C File Offset: 0x0017C17C
		public override bool update(GameTime time, GameLocation environment)
		{
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
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer < 0)
			{
				Character f = Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 4, environment);
				this.characterCheckTimer = 200;
				if (f != null && this.state != 1)
				{
					if (Game1.random.NextDouble() < 0.85)
					{
						Game1.playSound("SpringBirds");
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
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 6)), 70),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 60, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 8)), 70),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 7)), 60)
					});
					this.sprite.loop = true;
				}
			}
			switch (this.state)
			{
			case 0:
				if (this.sprite.currentAnimation == null)
				{
					List<FarmerSprite.AnimationFrame> peckAnim = new List<FarmerSprite.AnimationFrame>();
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 480));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 170, false, this.flip, null, false));
					peckAnim.Add(new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 170, false, this.flip, null, false));
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
				this.yOffset -= 2f + this.flightOffset;
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
					switch (Game1.random.Next(6))
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
					case 5:
						this.state = 4;
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame((int)((short)this.baseFrame), 100),
							new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 1)), 100)
						});
						this.sprite.loop = true;
						if (this.position.X >= this.startingPosition.X)
						{
							this.flip = false;
						}
						else
						{
							this.flip = true;
						}
						this.walkTimer = Game1.random.Next(5, 15) * 100;
						break;
					}
				}
				else if (this.sprite.currentAnimation == null)
				{
					this.sprite.CurrentFrame = this.baseFrame;
				}
				break;
			case 4:
				if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-1, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X - 1f;
				}
				else if (this.flip && !environment.isCollidingPosition(this.getBoundingBox(1, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X + 1f;
				}
				this.walkTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.walkTimer < 0)
				{
					this.state = 3;
					this.sprite.loop = false;
					this.sprite.currentAnimation = null;
					this.sprite.CurrentFrame = this.baseFrame;
				}
				break;
			}
			return base.update(time, environment);
		}

		// Token: 0x04001352 RID: 4946
		public const int brownBird = 25;

		// Token: 0x04001353 RID: 4947
		public const int blueBird = 45;

		// Token: 0x04001354 RID: 4948
		public const int flyingSpeed = 6;

		// Token: 0x04001355 RID: 4949
		public const int walkingSpeed = 1;

		// Token: 0x04001356 RID: 4950
		public const int pecking = 0;

		// Token: 0x04001357 RID: 4951
		public const int flyingAway = 1;

		// Token: 0x04001358 RID: 4952
		public const int sleeping = 2;

		// Token: 0x04001359 RID: 4953
		public const int stopped = 3;

		// Token: 0x0400135A RID: 4954
		public const int walking = 4;

		// Token: 0x0400135B RID: 4955
		private int state;

		// Token: 0x0400135C RID: 4956
		private float flightOffset;

		// Token: 0x0400135D RID: 4957
		private int characterCheckTimer = 200;

		// Token: 0x0400135E RID: 4958
		private int walkTimer;
	}
}
