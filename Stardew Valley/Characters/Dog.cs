using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Characters
{
	// Token: 0x02000141 RID: 321
	public class Dog : Pet
	{
		// Token: 0x0600121E RID: 4638 RVA: 0x00172CC0 File Offset: 0x00170EC0
		public Dog()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\dog"), 0, 32, 32);
			this.hideShadow = true;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00172CFC File Offset: 0x00170EFC
		public Dog(int xTile, int yTile)
		{
			this.name = "Dog";
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\dog"), 0, 32, 32);
			this.position = new Vector2((float)xTile, (float)yTile) * (float)Game1.tileSize;
			this.breather = false;
			this.willDestroyObjectsUnderfoot = false;
			this.currentLocation = Game1.currentLocation;
			this.hideShadow = true;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00172D73 File Offset: 0x00170F73
		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			this.sprintTimer = 0;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00172D84 File Offset: 0x00170F84
		public override void update(GameTime time, GameLocation location)
		{
			base.update(time, location);
			if (this.currentLocation == null)
			{
				this.currentLocation = location;
			}
			if (Game1.eventUp)
			{
				return;
			}
			if (this.sprintTimer > 0)
			{
				this.sprite.loop = true;
				this.sprintTimer -= time.ElapsedGameTime.Milliseconds;
				this.speed = 6;
				base.tryToMoveInDirection(this.facingDirection, false, -1, false);
				if (this.sprintTimer <= 0)
				{
					this.sprite.currentAnimation = null;
					this.Halt();
					this.faceDirection(this.facingDirection);
					this.speed = 2;
					base.CurrentBehavior = 0;
				}
				return;
			}
			if (Game1.timeOfDay > 2000 && this.sprite.currentAnimation == null && this.xVelocity == 0f && this.yVelocity == 0f)
			{
				base.CurrentBehavior = 1;
			}
			int currentBehavior = base.CurrentBehavior;
			switch (currentBehavior)
			{
			case 0:
				if (this.sprite.currentAnimation == null && Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(7 + ((this.currentLocation is Farm) ? 1 : 0)))
					{
					case 0:
					case 1:
					case 2:
					case 3:
						base.CurrentBehavior = 0;
						break;
					case 4:
					case 5:
						switch (this.facingDirection)
						{
						case 0:
						case 1:
						case 3:
							this.Halt();
							if (this.facingDirection == 0)
							{
								this.facingDirection = ((Game1.random.NextDouble() < 0.5) ? 3 : 1);
							}
							this.faceDirection(this.facingDirection);
							this.sprite.loop = false;
							base.CurrentBehavior = 50;
							break;
						case 2:
							this.Halt();
							this.faceDirection(2);
							this.sprite.loop = false;
							base.CurrentBehavior = 2;
							break;
						}
						break;
					case 6:
					case 7:
						base.CurrentBehavior = 51;
						break;
					}
				}
				break;
			case 1:
				if (Game1.timeOfDay < 2000 && Game1.random.NextDouble() < 0.001)
				{
					base.CurrentBehavior = 0;
					return;
				}
				if (Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
				}
				return;
			case 2:
				if (base.Sprite.currentFrame != 18 && this.sprite.currentAnimation == null)
				{
					base.CurrentBehavior = 2;
				}
				else if (base.Sprite.currentFrame == 18 && Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(4))
					{
					case 0:
						base.CurrentBehavior = 0;
						this.Halt();
						this.faceDirection(2);
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(17, 200),
							new FarmerSprite.AnimationFrame(16, 200),
							new FarmerSprite.AnimationFrame(0, 200)
						});
						this.sprite.loop = false;
						break;
					case 1:
					{
						List<FarmerSprite.AnimationFrame> pant = new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(18, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false),
							new FarmerSprite.AnimationFrame(19, 200)
						};
						int pants = Game1.random.Next(7, 20);
						for (int i = 0; i < pants; i++)
						{
							pant.Add(new FarmerSprite.AnimationFrame(18, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false));
							pant.Add(new FarmerSprite.AnimationFrame(19, 200));
						}
						this.sprite.setCurrentAnimation(pant);
						break;
					}
					case 2:
					case 3:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(27, (Game1.random.NextDouble() < 0.3) ? 500 : Game1.random.Next(2000, 15000)),
							new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
						});
						this.sprite.loop = false;
						break;
					}
				}
				break;
			default:
				if (currentBehavior == 50)
				{
					if (base.withinPlayerThreshold(2))
					{
						if (!this.wagging)
						{
							this.wag(this.facingDirection == 3);
						}
					}
					else if (base.Sprite.currentFrame != 23 && this.sprite.currentAnimation == null)
					{
						this.sprite.CurrentFrame = 23;
					}
					else if (this.sprite.currentFrame == 23 && Game1.random.NextDouble() < 0.01)
					{
						bool localFlip = this.facingDirection == 3;
						switch (Game1.random.Next(7))
						{
						case 0:
							base.CurrentBehavior = 0;
							this.Halt();
							this.faceDirection(localFlip ? 3 : 1);
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(23, 100, false, localFlip, null, false),
								new FarmerSprite.AnimationFrame(22, 100, false, localFlip, null, false),
								new FarmerSprite.AnimationFrame(21, 100, false, localFlip, null, false),
								new FarmerSprite.AnimationFrame(20, 100, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
							});
							this.sprite.loop = false;
							break;
						case 1:
							if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 10, this.currentLocation))
							{
								Game1.playSound("dog_bark");
								base.shake(500);
							}
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(26, 500, false, localFlip, null, false),
								new FarmerSprite.AnimationFrame(23, 1, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
							});
							break;
						case 2:
							this.wag(localFlip);
							break;
						case 3:
						case 4:
							this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(23, Game1.random.Next(2000, 6000), false, localFlip, null, false),
								new FarmerSprite.AnimationFrame(23, 1, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
							});
							break;
						default:
						{
							this.sprite.loop = false;
							List<FarmerSprite.AnimationFrame> panting = new List<FarmerSprite.AnimationFrame>
							{
								new FarmerSprite.AnimationFrame(24, 200, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false),
								new FarmerSprite.AnimationFrame(25, 200, false, localFlip, null, false)
							};
							int pantings = Game1.random.Next(5, 15);
							for (int j = 0; j < pantings; j++)
							{
								panting.Add(new FarmerSprite.AnimationFrame(24, 200, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false));
								panting.Add(new FarmerSprite.AnimationFrame(25, 200, false, localFlip, null, false));
							}
							this.sprite.setCurrentAnimation(panting);
							break;
						}
						}
					}
				}
				break;
			}
			if (this.sprite.currentAnimation != null)
			{
				this.sprite.loop = false;
			}
			else
			{
				this.wagging = false;
			}
			if (this.sprite.currentAnimation == null)
			{
				this.MovePosition(time, Game1.viewport, location);
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00173504 File Offset: 0x00171704
		public void wag(bool localFlip)
		{
			int delay = base.withinPlayerThreshold(2) ? 120 : 200;
			this.wagging = true;
			this.sprite.loop = false;
			List<FarmerSprite.AnimationFrame> wag = new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(31, delay, false, localFlip, null, false),
				new FarmerSprite.AnimationFrame(23, delay, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false)
			};
			int wags = Game1.random.Next(2, 6);
			for (int i = 0; i < wags; i++)
			{
				wag.Add(new FarmerSprite.AnimationFrame(31, delay, false, localFlip, null, false));
				wag.Add(new FarmerSprite.AnimationFrame(23, delay, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false));
			}
			wag.Add(new FarmerSprite.AnimationFrame(23, 2, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.doneWagging), false));
			this.sprite.setCurrentAnimation(wag);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x001735DD File Offset: 0x001717DD
		public void doneWagging(Farmer who)
		{
			this.wagging = false;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x001735E8 File Offset: 0x001717E8
		public override void initiateCurrentBehavior()
		{
			this.sprintTimer = 0;
			base.initiateCurrentBehavior();
			bool localflip = this.facingDirection == 3;
			int currentBehavior = base.CurrentBehavior;
			if (currentBehavior == 50)
			{
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(20, 100, false, localflip, null, false),
					new FarmerSprite.AnimationFrame(21, 100, false, localflip, null, false),
					new FarmerSprite.AnimationFrame(22, 100, false, localflip, null, false),
					new FarmerSprite.AnimationFrame(23, 100, false, localflip, new AnimatedSprite.endOfAnimationBehavior(base.hold), false)
				});
				return;
			}
			if (currentBehavior != 51)
			{
				return;
			}
			this.faceDirection((Game1.random.NextDouble() < 0.5) ? 3 : 1);
			localflip = (this.facingDirection == 3);
			this.sprintTimer = Game1.random.Next(1000, 3500);
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize, this.currentLocation))
			{
				Game1.playSound("dog_bark");
			}
			this.sprite.loop = true;
			this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(32, 100, false, localflip, null, false),
				new FarmerSprite.AnimationFrame(33, 100, false, localflip, null, false),
				new FarmerSprite.AnimationFrame(34, 100, false, localflip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false),
				new FarmerSprite.AnimationFrame(33, 100, false, localflip, null, false)
			});
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0017375E File Offset: 0x0017195E
		public void hitGround(Farmer who)
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), 2 * Game1.tileSize, this.currentLocation))
			{
				this.currentLocation.playTerrainSound(base.getTileLocation(), this, false);
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0017378D File Offset: 0x0017198D
		public void pantSound(Farmer who)
		{
			if (base.withinPlayerThreshold(5))
			{
				Game1.playSound("dog_pant");
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x001737A2 File Offset: 0x001719A2
		public void thumpSound(Farmer who)
		{
			if (base.withinPlayerThreshold(4))
			{
				Game1.playSound("thudStep");
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x001737B7 File Offset: 0x001719B7
		public override void playContentSound()
		{
			if (Utility.isOnScreen(base.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
			{
				Game1.playSound("dog_pant");
				DelayedAction.playSoundAfterDelay("dog_pant", 400);
			}
		}

		// Token: 0x040012E5 RID: 4837
		public const int behavior_sit_right = 50;

		// Token: 0x040012E6 RID: 4838
		public const int behavior_sprint = 51;

		// Token: 0x040012E7 RID: 4839
		private int sprintTimer;

		// Token: 0x040012E8 RID: 4840
		private bool wagging;
	}
}
