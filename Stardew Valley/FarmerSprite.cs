using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace StardewValley
{
	// Token: 0x02000043 RID: 67
	public class FarmerSprite : AnimatedSprite
	{
		// Token: 0x17000052 RID: 82
		public FarmerSprite.AnimationFrame CurrentAnimationFrame
		{
			// Token: 0x0600041A RID: 1050 RVA: 0x0004B45E File Offset: 0x0004965E
			get
			{
				if (this.currentAnimation == null)
				{
					return new FarmerSprite.AnimationFrame(0, 100, 0, false, false, null, false, 0);
				}
				return this.currentAnimation[this.indexInCurrentAnimation % this.currentAnimation.Count];
			}
		}

		// Token: 0x17000053 RID: 83
		public int CurrentSingleAnimation
		{
			// Token: 0x0600041B RID: 1051 RVA: 0x0004B494 File Offset: 0x00049694
			get
			{
				if (this.currentAnimation != null)
				{
					return this.currentAnimation[0].frame;
				}
				return -1;
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0004B4B1 File Offset: 0x000496B1
		public void setOwner(Farmer owner)
		{
			this.owner = owner;
		}

		// Token: 0x17000054 RID: 84
		public override int CurrentFrame
		{
			// Token: 0x0600041D RID: 1053 RVA: 0x000035E9 File Offset: 0x000017E9
			get
			{
				return this.currentFrame;
			}
			// Token: 0x0600041E RID: 1054 RVA: 0x0004B4BA File Offset: 0x000496BA
			set
			{
				if (this.currentFrame != value && !this.freezeUntilDialogueIsOver)
				{
					this.currentFrame = value;
					this.UpdateSourceRect();
				}
				if (value > FarmerRenderer.featureYOffsetPerFrame.Length - 1)
				{
					this.currentFrame = 0;
				}
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0004B4F0 File Offset: 0x000496F0
		public void setCurrentAnimation(FarmerSprite.AnimationFrame[] animation)
		{
			base.CurrentAnimation = animation.ToList<FarmerSprite.AnimationFrame>();
			this.oldFrame = this.currentFrame;
			this.currentAnimationIndex = 0;
			if (this.currentAnimation.Count > 0)
			{
				this.interval = (float)this.currentAnimation[0].milliseconds;
				this.CurrentFrame = this.currentAnimation[0].frame;
				this.currentAnimationFrames = this.currentAnimation.Count;
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0004B56C File Offset: 0x0004976C
		public override void faceDirection(int direction)
		{
			switch (direction)
			{
			case 0:
				this.setCurrentFrame(12);
				break;
			case 1:
				this.setCurrentFrame(6);
				break;
			case 2:
				this.setCurrentFrame(0);
				break;
			case 3:
				this.setCurrentFrame(6, 0, 100, 1, true, false);
				break;
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0004B5C0 File Offset: 0x000497C0
		public void setCurrentSingleFrame(int which, short interval = 32000, bool secondaryArm = false, bool flip = false)
		{
			this.loopThisAnimation = false;
			base.CurrentAnimation = new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame((int)((short)which), (int)interval, secondaryArm, flip, null, false)
			}.ToList<FarmerSprite.AnimationFrame>();
			this.CurrentFrame = base.CurrentAnimation[0].frame;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0004B612 File Offset: 0x00049812
		public void setCurrentFrame(int which)
		{
			this.setCurrentFrame(which, 0);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0004B61C File Offset: 0x0004981C
		public void setCurrentFrame(int which, int offset)
		{
			this.setCurrentFrame(which, offset, 100, 1, false, false);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0004B62C File Offset: 0x0004982C
		public void setCurrentFrameBackwards(int which, int offset, int interval, int numFrames, bool secondaryArm, bool flip)
		{
			base.CurrentAnimation = FarmerSprite.getAnimationFromIndex(which, this, interval, numFrames, secondaryArm, flip).ToList<FarmerSprite.AnimationFrame>();
			base.CurrentAnimation.Reverse();
			this.CurrentFrame = base.CurrentAnimation[Math.Min(base.CurrentAnimation.Count - 1, offset)].frame;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0004B688 File Offset: 0x00049888
		public void setCurrentFrame(int which, int offset, int interval, int numFrames, bool flip, bool secondaryArm)
		{
			if (this.nextOffset != 0)
			{
				offset = this.nextOffset;
				this.nextOffset = 0;
			}
			base.CurrentAnimation = FarmerSprite.getAnimationFromIndex(which, this, interval, numFrames, flip, secondaryArm).ToList<FarmerSprite.AnimationFrame>();
			this.CurrentFrame = base.CurrentAnimation[Math.Min(base.CurrentAnimation.Count - 1, offset)].frame;
			this.interval = (float)this.CurrentAnimationFrame.milliseconds;
			this.timer = 0f;
		}

		// Token: 0x17000055 RID: 85
		public bool PauseForSingleAnimation
		{
			// Token: 0x06000426 RID: 1062 RVA: 0x0004B70B File Offset: 0x0004990B
			get
			{
				return this.pauseForSingleAnimation;
			}
			// Token: 0x06000427 RID: 1063 RVA: 0x0004B713 File Offset: 0x00049913
			set
			{
				this.pauseForSingleAnimation = value;
			}
		}

		// Token: 0x17000056 RID: 86
		public int CurrentToolIndex
		{
			// Token: 0x06000428 RID: 1064 RVA: 0x0004B71C File Offset: 0x0004991C
			get
			{
				return this.currentToolIndex;
			}
			// Token: 0x06000429 RID: 1065 RVA: 0x0004B724 File Offset: 0x00049924
			set
			{
				this.currentToolIndex = value;
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0004B730 File Offset: 0x00049930
		public FarmerSprite(Texture2D texture) : base(texture)
		{
			this.interval /= 2f;
			this.spriteWidth = 16;
			this.spriteHeight = 32;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0004B788 File Offset: 0x00049988
		public void animate(int whichAnimation, GameTime time)
		{
			this.animate(whichAnimation, time.ElapsedGameTime.Milliseconds);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0004B7AC File Offset: 0x000499AC
		public void animate(int whichAnimation, int milliseconds)
		{
			if (!this.PauseForSingleAnimation)
			{
				if (whichAnimation != this.currentSingleAnimation || base.CurrentAnimation == null || base.CurrentAnimation.Count <= 1)
				{
					float oldtimer = this.timer;
					int oldIndex = this.indexInCurrentAnimation;
					this.currentSingleAnimation = whichAnimation;
					this.setCurrentFrame(whichAnimation);
					this.timer = oldtimer;
					this.CurrentFrame = base.CurrentAnimation[Math.Min(oldIndex, base.CurrentAnimation.Count - 1)].frame;
					this.indexInCurrentAnimation = oldIndex;
					this.currentAnimationIndex = oldIndex;
					this.UpdateSourceRect();
				}
				this.animate(milliseconds);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0004B84A File Offset: 0x00049A4A
		public void checkForSingleAnimation(GameTime time)
		{
			if (this.PauseForSingleAnimation)
			{
				if (!this.animateBackwards)
				{
					this.animateOnce(time);
					return;
				}
				this.animateBackwardsOnce(time);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0004B86B File Offset: 0x00049A6B
		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames)
		{
			this.animateOnce(whichAnimation, animationInterval, numberOfFrames, null);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0004B877 File Offset: 0x00049A77
		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames, AnimatedSprite.endOfAnimationBehavior endOfBehaviorFunction)
		{
			this.animateOnce(whichAnimation, animationInterval, numberOfFrames, endOfBehaviorFunction, false, false);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0004B886 File Offset: 0x00049A86
		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames, AnimatedSprite.endOfAnimationBehavior endOfBehaviorFunction, bool flip, bool secondaryArm)
		{
			this.animateOnce(whichAnimation, animationInterval, numberOfFrames, endOfBehaviorFunction, flip, secondaryArm, false);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0004B898 File Offset: 0x00049A98
		public void animateOnce(FarmerSprite.AnimationFrame[] animation)
		{
			if (!this.PauseForSingleAnimation)
			{
				this.currentSingleAnimation = 0;
				this.CurrentFrame = this.currentSingleAnimation;
				this.PauseForSingleAnimation = true;
				this.oldFrame = this.CurrentFrame;
				this.oldInterval = this.interval;
				this.currentSingleAnimationInterval = 100f;
				this.timer = 0f;
				base.CurrentAnimation = animation.ToList<FarmerSprite.AnimationFrame>();
				this.CurrentFrame = base.CurrentAnimation[0].frame;
				this.currentAnimationFrames = base.CurrentAnimation.Count;
				this.indexInCurrentAnimation = 0;
				this.interval = (float)this.CurrentAnimationFrame.milliseconds;
				this.loopThisAnimation = false;
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0004B94C File Offset: 0x00049B4C
		public void showFrameUntilDialogueOver(int whichFrame)
		{
			this.freezeUntilDialogueIsOver = true;
			this.setCurrentFrame(whichFrame);
			this.UpdateSourceRect();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0004B964 File Offset: 0x00049B64
		public void animateOnce(int whichAnimation, float animationInterval, int numberOfFrames, AnimatedSprite.endOfAnimationBehavior endOfBehaviorFunction, bool flip, bool secondaryArm, bool backwards)
		{
			if (!this.PauseForSingleAnimation && !this.freezeUntilDialogueIsOver)
			{
				if (!this.owner.IsMainPlayer)
				{
					if (whichAnimation <= 240)
					{
						if (whichAnimation == 232)
						{
							this.owner.faceDirection(2);
							goto IL_A9;
						}
						if (whichAnimation == 240)
						{
							this.owner.faceDirection(1);
							goto IL_A9;
						}
					}
					else
					{
						if (whichAnimation == 248)
						{
							this.owner.faceDirection(0);
							goto IL_A9;
						}
						if (whichAnimation == 256)
						{
							this.owner.faceDirection(3);
							goto IL_A9;
						}
					}
					int faceDirection = whichAnimation / 8 % 4;
					if (faceDirection == 0)
					{
						faceDirection = 2;
					}
					else if (faceDirection == 2)
					{
						faceDirection = 0;
					}
					this.owner.faceDirection(faceDirection);
				}
				IL_A9:
				this.currentSingleAnimation = whichAnimation;
				this.CurrentFrame = this.currentSingleAnimation;
				this.PauseForSingleAnimation = true;
				this.oldFrame = this.CurrentFrame;
				this.oldInterval = this.interval;
				this.currentSingleAnimationInterval = animationInterval;
				this.endOfAnimationFunction = endOfBehaviorFunction;
				this.timer = 0f;
				this.animatingBackwards = false;
				if (backwards)
				{
					this.animatingBackwards = true;
					this.setCurrentFrameBackwards(this.currentSingleAnimation, 0, (int)animationInterval, numberOfFrames, secondaryArm, flip);
				}
				else
				{
					this.setCurrentFrame(this.currentSingleAnimation, 0, (int)animationInterval, numberOfFrames, secondaryArm, flip);
				}
				if (base.CurrentAnimation[0].frameBehavior != null && !base.CurrentAnimation[0].behaviorAtEndOfFrame)
				{
					base.CurrentAnimation[0].frameBehavior(this.owner);
				}
				if (this.owner.Stamina <= 0f && this.owner.usingTool)
				{
					for (int i = 0; i < base.CurrentAnimation.Count; i++)
					{
						base.CurrentAnimation[i] = new FarmerSprite.AnimationFrame(base.CurrentAnimation[i].frame, base.CurrentAnimation[i].milliseconds * 2, base.CurrentAnimation[i].secondaryArm, base.CurrentAnimation[i].flip, base.CurrentAnimation[i].frameBehavior, base.CurrentAnimation[i].behaviorAtEndOfFrame);
					}
				}
				this.currentAnimationFrames = base.CurrentAnimation.Count;
				this.indexInCurrentAnimation = 0;
				this.interval = (float)this.CurrentAnimationFrame.milliseconds;
				if (Game1.IsClient && this.owner.uniqueMultiplayerID == Game1.player.uniqueMultiplayerID)
				{
					this.currentToolIndex = -1;
					if (this.owner.UsingTool)
					{
						this.currentToolIndex = this.owner.CurrentTool.CurrentParentTileIndex;
						if (this.owner.CurrentTool is FishingRod)
						{
							if (this.owner.facingDirection == 3 || this.owner.facingDirection == 1)
							{
								this.currentToolIndex = 55;
							}
							else
							{
								this.currentToolIndex = 48;
							}
						}
					}
					MultiplayerUtility.sendAnimationMessageToServer(whichAnimation, numberOfFrames, animationInterval, false, this.currentToolIndex);
				}
				else if (Game1.IsServer)
				{
					if (this.owner.IsMainPlayer && this.owner.UsingTool)
					{
						this.currentToolIndex = this.owner.CurrentTool.CurrentParentTileIndex;
						if (this.owner.CurrentTool is FishingRod)
						{
							if (this.owner.facingDirection == 3 || this.owner.facingDirection == 1)
							{
								this.currentToolIndex = 55;
							}
							else
							{
								this.currentToolIndex = 48;
							}
						}
					}
					MultiplayerUtility.broadcastFarmerAnimation(this.owner.uniqueMultiplayerID, whichAnimation, numberOfFrames, animationInterval, false, this.owner.currentLocation.name, this.currentToolIndex);
				}
				if (Game1.IsMultiplayer && this.getWeaponTypeFromAnimation() == 3)
				{
					MeleeWeapon.doSwipe(this.getWeaponTypeFromAnimation(), this.owner.position, this.owner.facingDirection, animationInterval, this.owner);
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0004BD40 File Offset: 0x00049F40
		public void animateBackwardsOnce(int whichAnimation, float animationInterval)
		{
			this.animateOnce(whichAnimation, animationInterval, 6, null, false, false, true);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0004BD5C File Offset: 0x00049F5C
		public bool isUsingWeapon()
		{
			return this.PauseForSingleAnimation && ((this.currentSingleAnimation >= 232 && this.currentSingleAnimation < 264) || (this.currentSingleAnimation >= 272 && this.currentSingleAnimation < 280));
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0004BDAB File Offset: 0x00049FAB
		public int getWeaponTypeFromAnimation()
		{
			if (this.currentSingleAnimation >= 272 && this.currentSingleAnimation < 280)
			{
				return 1;
			}
			if (this.currentSingleAnimation >= 232 && this.currentSingleAnimation < 264)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0004BDE8 File Offset: 0x00049FE8
		public bool isOnToolAnimation()
		{
			return (this.PauseForSingleAnimation || this.owner.UsingTool) && ((this.currentSingleAnimation >= 160 && this.currentSingleAnimation < 192) || (this.currentSingleAnimation >= 232 && this.currentSingleAnimation < 264) || (this.currentSingleAnimation >= 272 && this.currentSingleAnimation < 280));
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0004BE60 File Offset: 0x0004A060
		private void doneWithAnimation()
		{
			if (this.CurrentFrame < 64 || this.CurrentFrame > 96)
			{
				this.CurrentFrame = this.oldFrame;
			}
			else
			{
				int currentFrame = this.CurrentFrame;
				this.CurrentFrame = currentFrame - 1;
			}
			this.interval = this.oldInterval;
			if (!Game1.eventUp)
			{
				this.owner.CanMove = true;
			}
			this.owner.Halt();
			this.PauseForSingleAnimation = false;
			this.animatingBackwards = false;
			this.ignoreDefaultActionThisTime = false;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0004BEE0 File Offset: 0x0004A0E0
		private void currentAnimationTick()
		{
			if (this.indexInCurrentAnimation >= base.CurrentAnimation.Count)
			{
				return;
			}
			if (base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior != null && base.CurrentAnimation[this.indexInCurrentAnimation].behaviorAtEndOfFrame)
			{
				base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior(this.owner);
			}
			this.indexInCurrentAnimation++;
			if (this.loopThisAnimation)
			{
				this.indexInCurrentAnimation %= base.CurrentAnimation.Count;
			}
			else if (this.indexInCurrentAnimation >= base.CurrentAnimation.Count)
			{
				this.loopThisAnimation = false;
				return;
			}
			if (base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior != null && !base.CurrentAnimation[this.indexInCurrentAnimation].behaviorAtEndOfFrame)
			{
				base.CurrentAnimation[this.indexInCurrentAnimation].frameBehavior(this.owner);
			}
			if (base.CurrentAnimation != null && this.indexInCurrentAnimation < base.CurrentAnimation.Count)
			{
				this.currentSingleAnimationInterval = (float)base.CurrentAnimation[this.indexInCurrentAnimation].milliseconds;
				this.CurrentFrame = base.CurrentAnimation[this.indexInCurrentAnimation].frame;
				this.interval = (float)base.CurrentAnimation[this.indexInCurrentAnimation].milliseconds;
				return;
			}
			this.owner.completelyStopAnimatingOrDoingAction();
			this.owner.forceCanMove();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0004C076 File Offset: 0x0004A276
		public override void UpdateSourceRect()
		{
			base.SourceRect = new Rectangle(this.CurrentFrame * this.spriteWidth % 96, this.CurrentFrame * this.spriteWidth / 96 * this.spriteHeight, this.spriteWidth, this.spriteHeight);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0004C0B8 File Offset: 0x0004A2B8
		private new void animateOnce(GameTime time)
		{
			if (this.freezeUntilDialogueIsOver)
			{
				return;
			}
			this.timer += (float)time.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval * this.intervalModifier)
			{
				this.currentAnimationTick();
				this.timer = 0f;
				int currentFrame;
				if (this.indexInCurrentAnimation > this.currentAnimationFrames - 1)
				{
					if (this.owner.IsMainPlayer)
					{
						FarmerSprite.AnimationFrame arg_77_0 = this.CurrentAnimationFrame;
						if (this.CurrentAnimationFrame.behaviorAtEndOfFrame)
						{
							this.CurrentAnimationFrame.frameBehavior(this.owner);
						}
						if (this.endOfAnimationFunction != null && !this.ignoreDefaultActionThisTime)
						{
							AnimatedSprite.endOfAnimationBehavior arg_C4_0 = this.endOfAnimationFunction;
							this.endOfAnimationFunction = null;
							arg_C4_0(this.owner);
							if (this.owner.UsingTool && this.owner.CurrentTool.Name.Equals("Fishing Rod"))
							{
								this.PauseForSingleAnimation = false;
								this.interval = this.oldInterval;
								if (this.owner.IsMainPlayer)
								{
									this.owner.CanMove = false;
									return;
								}
							}
							else if (!(this.owner.CurrentTool is MeleeWeapon) || (this.owner.CurrentTool as MeleeWeapon).type != 1)
							{
								this.doneWithAnimation();
							}
							return;
						}
						if (!this.ignoreDefaultActionThisTime && (this.currentSingleAnimation < 160 || this.currentSingleAnimation >= 192) && (this.currentSingleAnimation < 200 || this.currentSingleAnimation >= 216) && (this.currentSingleAnimation < 232 || this.currentSingleAnimation >= 264) && this.currentSingleAnimation >= 272)
						{
							int arg_1BF_0 = this.currentSingleAnimation;
						}
						this.doneWithAnimation();
						if (Game1.isEating)
						{
							Game1.doneEating();
						}
					}
					else if ((this.owner.UsingTool && this.owner.CurrentTool is FishingRod) || (this.currentToolIndex >= 48 && this.currentToolIndex <= 55))
					{
						this.PauseForSingleAnimation = false;
						this.interval = this.oldInterval;
						currentFrame = this.CurrentFrame;
						this.CurrentFrame = currentFrame - 1;
					}
					else
					{
						this.doneWithAnimation();
					}
				}
				currentFrame = this.currentSingleAnimation;
				if (currentFrame <= 168)
				{
					if (currentFrame <= 161)
					{
						if (currentFrame != 160 && currentFrame != 161)
						{
							goto IL_32B;
						}
					}
					else if (currentFrame != 165)
					{
						if (currentFrame != 168)
						{
							goto IL_32B;
						}
						goto IL_32B;
					}
					if (this.owner.CurrentTool != null)
					{
						this.owner.CurrentTool.Update(2, this.indexInCurrentAnimation, this.owner);
					}
				}
				else if (currentFrame <= 184)
				{
					switch (currentFrame)
					{
					case 172:
					case 173:
					case 174:
					case 175:
						goto IL_32B;
					case 176:
						break;
					default:
						switch (currentFrame)
						{
						case 180:
						case 181:
							break;
						case 182:
						case 183:
						case 184:
							goto IL_32B;
						default:
							goto IL_32B;
						}
						break;
					}
					if (this.owner.CurrentTool != null)
					{
						this.owner.CurrentTool.Update(0, this.indexInCurrentAnimation, this.owner);
					}
				}
				else if (currentFrame != 188 && currentFrame != 189)
				{
				}
				IL_32B:
				if (this.CurrentFrame == 109)
				{
					Game1.playSound("eat");
				}
				if (!this.owner.IsMainPlayer && this.isOnToolAnimation() && !this.isUsingWeapon() && this.indexInCurrentAnimation == 4 && this.currentToolIndex % 2 == 0 && !(this.owner.CurrentTool is FishingRod))
				{
					this.currentToolIndex++;
				}
			}
			this.UpdateSourceRect();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0004C45C File Offset: 0x0004A65C
		private void checkForFootstep()
		{
			if (Game1.player.isRidingHorse())
			{
				return;
			}
			if (((this.currentSingleAnimation >= 32 && this.currentSingleAnimation <= 56) || (this.currentSingleAnimation >= 128 && this.currentSingleAnimation <= 152)) && this.indexInCurrentAnimation % 4 == 0)
			{
				Vector2 tileLocationOfPlayer = this.owner.getTileLocation();
				if (Game1.currentLocation.IsOutdoors || Game1.currentLocation.Name.ToLower().Contains("mine") || Game1.currentLocation.Name.ToLower().Contains("cave") || Game1.currentLocation.Name.Equals("Greenhouse"))
				{
					string stepType = Game1.currentLocation.doesTileHaveProperty((int)tileLocationOfPlayer.X, (int)tileLocationOfPlayer.Y, "Type", "Buildings");
					if (stepType == null || stepType.Length < 1)
					{
						stepType = Game1.currentLocation.doesTileHaveProperty((int)tileLocationOfPlayer.X, (int)tileLocationOfPlayer.Y, "Type", "Back");
					}
					if (stepType != null)
					{
						if (!(stepType == "Dirt"))
						{
							if (!(stepType == "Stone"))
							{
								if (!(stepType == "Grass"))
								{
									if (stepType == "Wood")
									{
										this.currentStep = "woodyStep";
									}
								}
								else
								{
									this.currentStep = (Game1.currentSeason.Equals("winter") ? "snowyStep" : "grassyStep");
								}
							}
							else
							{
								this.currentStep = "stoneStep";
							}
						}
						else
						{
							this.currentStep = "sandyStep";
						}
					}
				}
				else
				{
					this.currentStep = "thudStep";
				}
				if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocationOfPlayer) && Game1.currentLocation.terrainFeatures[tileLocationOfPlayer].GetType() == typeof(Flooring))
				{
					this.currentStep = ((Flooring)Game1.currentLocation.terrainFeatures[tileLocationOfPlayer]).getFootstepSound();
				}
				if (this.currentStep.Equals("sandyStep"))
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(128, 2948, 64, 64), 80f, 8, 0, new Vector2(this.owner.position.X + (float)(Game1.tileSize / 4) + (float)Game1.random.Next(-8, 8), this.owner.position.Y + (float)(Game1.random.Next(-3, -1) * Game1.pixelZoom)), false, Game1.random.NextDouble() < 0.5, this.owner.position.Y / 10000f, 0.03f, Color.Khaki * 0.45f, 0.75f + (float)Game1.random.Next(-3, 4) * 0.05f, 0f, 0f, 0f, false));
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(128, 2948, 64, 64), 80f, 8, 0, new Vector2(this.owner.position.X + (float)(Game1.tileSize / 4) + (float)Game1.random.Next(-4, 4), this.owner.position.Y + (float)(Game1.random.Next(-3, -1) * Game1.pixelZoom)), false, Game1.random.NextDouble() < 0.5, this.owner.position.Y / 10000f, 0.03f, Color.Khaki * 0.45f, 0.55f + (float)Game1.random.Next(-3, 4) * 0.05f, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = 20
					});
				}
				else if (this.currentStep.Equals("snowyStep"))
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(247, 407, 6, 6), 2000f, 1, 10000, new Vector2(this.owner.position.X + (float)(Game1.pixelZoom * 6) + (float)(Game1.random.Next(-4, 4) * Game1.pixelZoom), this.owner.position.Y + (float)(Game1.pixelZoom * 2) + (float)(Game1.random.Next(-4, 4) * Game1.pixelZoom)), false, false, this.owner.position.Y / 1E+07f, 0.01f, Color.White, (float)(Game1.pixelZoom * 3) / 4f + (float)Game1.random.NextDouble(), 0f, (this.owner.facingDirection == 1 || this.owner.facingDirection == 3) ? -0.7853982f : 0f, 0f, false));
				}
				Game1.playSound(this.currentStep);
				Stats expr_51C = Game1.stats;
				uint stepsTaken = expr_51C.StepsTaken;
				expr_51C.StepsTaken = stepsTaken + 1u;
				return;
			}
			if (this.CurrentFrame == 4 || this.CurrentFrame == 12 || this.CurrentFrame == 20 || this.CurrentFrame == 28 || this.CurrentFrame == 100 || this.CurrentFrame == 108 || this.CurrentFrame == 116 || this.CurrentFrame == 124)
			{
				Stats expr_580 = Game1.stats;
				uint stepsTaken = expr_580.StepsTaken;
				expr_580.StepsTaken = stepsTaken + 1u;
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0004C9F8 File Offset: 0x0004ABF8
		public static void checkForFootstep(Character who)
		{
			if (who != null)
			{
				Game1.currentLocation.playTerrainSound(who.getTileLocation(), who, true);
				return;
			}
			string stepType = Game1.currentLocation.doesTileHaveProperty((int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y, "Type", "Back");
			if (stepType == "Stone")
			{
				Game1.playSound("stoneStep");
				Rumble.rumble(0.1f, 50f);
				return;
			}
			if (!(stepType == "Wood"))
			{
				Game1.playSound("thudStep");
				Rumble.rumble(0.3f, 50f);
				return;
			}
			Game1.playSound("woodyStep");
			Rumble.rumble(0.1f, 50f);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0004CABC File Offset: 0x0004ACBC
		private void animateBackwardsOnce(GameTime time)
		{
			this.timer += (float)time.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.currentSingleAnimationInterval)
			{
				int currentFrame = this.CurrentFrame;
				this.CurrentFrame = currentFrame - 1;
				this.timer = 0f;
				if (this.indexInCurrentAnimation > this.currentAnimationFrames - 1)
				{
					if (this.CurrentFrame < 63 || this.CurrentFrame > 96)
					{
						this.CurrentFrame = this.oldFrame;
					}
					else
					{
						this.CurrentFrame = this.CurrentFrame % 16 + 8;
					}
					this.interval = this.oldInterval;
					this.PauseForSingleAnimation = false;
					this.animatingBackwards = false;
					if (!Game1.eventUp)
					{
						this.owner.CanMove = true;
					}
					if (this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Fishing Rod"))
					{
						this.owner.usingTool = false;
					}
					this.owner.Halt();
					if (((this.CurrentSingleAnimation >= 160 && this.CurrentSingleAnimation < 192) || (this.CurrentSingleAnimation >= 200 && this.CurrentSingleAnimation < 216) || (this.CurrentSingleAnimation >= 232 && this.CurrentSingleAnimation < 264)) && this.owner.IsMainPlayer)
					{
						Game1.toolAnimationDone();
					}
				}
				if (this.owner.UsingTool && this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Fishing Rod"))
				{
					currentFrame = this.CurrentFrame;
					if (currentFrame <= 168)
					{
						if (currentFrame != 164)
						{
							if (currentFrame == 168)
							{
								this.owner.CurrentTool.Update(1, 0);
							}
						}
						else
						{
							this.owner.CurrentTool.Update(2, 0);
						}
					}
					else if (currentFrame != 180)
					{
						if (currentFrame == 184)
						{
							this.owner.CurrentTool.Update(3, 0);
						}
					}
					else
					{
						this.owner.CurrentTool.Update(0, 0);
					}
				}
			}
			this.UpdateSourceRect();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0004CCEC File Offset: 0x0004AEEC
		public int frameOfCurrentSingleAnimation()
		{
			if (this.PauseForSingleAnimation)
			{
				return this.CurrentFrame - (this.currentSingleAnimation - this.currentSingleAnimation % 8);
			}
			if (!Game1.pickingTool && this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Watering Can"))
			{
				return 4;
			}
			if (!Game1.pickingTool && this.owner.UsingTool && ((this.currentToolIndex >= 48 && this.currentToolIndex <= 55) || (this.owner.CurrentTool != null && this.owner.CurrentTool.Name.Equals("Fishing Rod"))))
			{
				return 6;
			}
			return 0;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0004CDA0 File Offset: 0x0004AFA0
		public void setCurrentSingleAnimation(int which)
		{
			this.CurrentFrame = which;
			this.currentSingleAnimation = which;
			base.CurrentAnimation = FarmerSprite.getAnimationFromIndex(which, this, 100, 1, false, false).ToList<FarmerSprite.AnimationFrame>();
			if (base.CurrentAnimation != null && base.CurrentAnimation.Count > 0)
			{
				this.currentAnimationFrames = base.CurrentAnimation.Count;
				this.interval = (float)base.CurrentAnimation.First<FarmerSprite.AnimationFrame>().milliseconds;
				this.CurrentFrame = base.CurrentAnimation.First<FarmerSprite.AnimationFrame>().frame;
			}
			if (this.interval <= 50f)
			{
				this.interval = 800f;
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0004CE44 File Offset: 0x0004B044
		private void animate(int Milliseconds, int firstFrame, int lastFrame)
		{
			if (this.CurrentFrame > lastFrame || this.CurrentFrame < firstFrame)
			{
				this.CurrentFrame = firstFrame;
			}
			this.timer += (float)Milliseconds;
			if (this.timer > this.interval * this.intervalModifier)
			{
				int currentFrame = this.CurrentFrame;
				this.CurrentFrame = currentFrame + 1;
				this.timer = 0f;
				if (this.CurrentFrame > lastFrame)
				{
					this.CurrentFrame = firstFrame;
				}
				this.checkForFootstep();
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0004CEC8 File Offset: 0x0004B0C8
		private void animate(int Milliseconds)
		{
			this.timer += (float)Milliseconds;
			if (this.timer > this.interval * this.intervalModifier)
			{
				this.currentAnimationTick();
				this.timer = 0f;
				this.checkForFootstep();
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0004CF18 File Offset: 0x0004B118
		public override void StopAnimation()
		{
			if (!this.pauseForSingleAnimation)
			{
				this.interval = 0f;
				if (this.CurrentFrame >= 64 && this.CurrentFrame <= 155 && this.owner != null && !this.owner.bathingClothes)
				{
					switch (this.owner.facingDirection)
					{
					case 0:
						this.CurrentFrame = 12;
						break;
					case 1:
						this.CurrentFrame = 6;
						break;
					case 2:
						this.CurrentFrame = 0;
						break;
					case 3:
						this.CurrentFrame = 6;
						break;
					}
				}
				else if (!Game1.pickingTool)
				{
					if (this.owner != null)
					{
						switch (this.owner.facingDirection)
						{
						case 0:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(112, 1);
							}
							else
							{
								this.setCurrentFrame(16, 1);
							}
							break;
						case 1:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(104, 1);
							}
							else
							{
								this.setCurrentFrame(8, 1);
							}
							break;
						case 2:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(96, 1);
							}
							else
							{
								this.setCurrentFrame(0, 1);
							}
							break;
						case 3:
							if (this.owner.ActiveObject != null && !Game1.eventUp)
							{
								this.setCurrentFrame(120, 1);
							}
							else
							{
								this.setCurrentFrame(24, 1);
							}
							break;
						}
					}
					this.currentSingleAnimation = -1;
				}
				this.indexInCurrentAnimation = 0;
				this.UpdateSourceRect();
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0004D0B8 File Offset: 0x0004B2B8
		public static FarmerSprite.AnimationFrame[] getAnimationFromIndex(int index, FarmerSprite requester, int interval, int numberOfFrames, bool flip, bool secondaryArm)
		{
			bool showCarryingArm = (index >= 96 && index < 160) || index == 232 || index == 248;
			if (requester.owner != null && requester.owner.ActiveObject != null && requester.owner.ActiveObject is Furniture)
			{
				showCarryingArm = false;
			}
			requester.loopThisAnimation = true;
			int frameOffset = 0;
			if (requester.owner.bathingClothes)
			{
				frameOffset += 108;
			}
			if (index <= 88)
			{
				if (index <= 43)
				{
					if (index <= 16)
					{
						if (index <= 0)
						{
							if (index == -1)
							{
								return new FarmerSprite.AnimationFrame[]
								{
									new FarmerSprite.AnimationFrame(0, 100, showCarryingArm, false, null, false)
								};
							}
							if (index != 0)
							{
								goto IL_241D;
							}
						}
						else
						{
							if (index == 8)
							{
								goto IL_481;
							}
							if (index != 16)
							{
								goto IL_241D;
							}
							goto IL_4E8;
						}
					}
					else if (index <= 32)
					{
						if (index == 24)
						{
							goto IL_553;
						}
						if (index != 32)
						{
							goto IL_241D;
						}
						goto IL_5BA;
					}
					else
					{
						if (index == 40)
						{
							goto IL_66D;
						}
						if (index != 43)
						{
							goto IL_241D;
						}
						flip = (requester.owner.FacingDirection == 3);
						goto IL_241D;
					}
				}
				else
				{
					if (index > 71)
					{
						if (index <= 83)
						{
							if (index != 72)
							{
								switch (index)
								{
								case 79:
									break;
								case 80:
									goto IL_15FE;
								case 81:
								case 82:
									goto IL_241D;
								case 83:
									requester.loopThisAnimation = false;
									return new FarmerSprite.AnimationFrame[]
									{
										new FarmerSprite.AnimationFrame(0, 0, false, false, null, false)
									};
								default:
									goto IL_241D;
								}
							}
							requester.loopThisAnimation = false;
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(6, 0, false, false, null, false)
							};
						}
						if (index != 87)
						{
							if (index != 88)
							{
								goto IL_241D;
							}
							goto IL_165F;
						}
						IL_15FE:
						requester.loopThisAnimation = false;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(12, 0, false, false, null, false)
						};
					}
					if (index <= 56)
					{
						if (index == 48)
						{
							goto IL_724;
						}
						if (index != 56)
						{
							goto IL_241D;
						}
						goto IL_7DD;
					}
					else
					{
						if (index != 64 && index != 71)
						{
							goto IL_241D;
						}
						requester.loopThisAnimation = false;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(0, 0, false, false, null, false)
						};
					}
				}
			}
			else if (index <= 152)
			{
				if (index <= 120)
				{
					if (index <= 104)
					{
						switch (index)
						{
						case 95:
							goto IL_165F;
						case 96:
							break;
						case 97:
							requester.loopThisAnimation = false;
							flip = (requester.owner.FacingDirection == 3);
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(97, 800, false, flip, null, false)
							};
						default:
							if (index != 104)
							{
								goto IL_241D;
							}
							goto IL_481;
						}
					}
					else
					{
						if (index == 112)
						{
							goto IL_4E8;
						}
						if (index != 120)
						{
							goto IL_241D;
						}
						goto IL_553;
					}
				}
				else if (index <= 136)
				{
					if (index == 128)
					{
						goto IL_5BA;
					}
					if (index != 136)
					{
						goto IL_241D;
					}
					goto IL_66D;
				}
				else
				{
					if (index == 144)
					{
						goto IL_724;
					}
					if (index != 152)
					{
						goto IL_241D;
					}
					goto IL_7DD;
				}
			}
			else if (index <= 184)
			{
				if (index <= 168)
				{
					if (index == 160)
					{
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(66, 150, false, false, null, false),
							new FarmerSprite.AnimationFrame(67, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(68, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
							new FarmerSprite.AnimationFrame(69, (int)((short)(170 + requester.owner.toolPower * 30)), false, false, null, false),
							new FarmerSprite.AnimationFrame(70, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					}
					switch (index)
					{
					case 164:
					case 166:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(54, 0, false, false, null, false),
							new FarmerSprite.AnimationFrame(54, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(55, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
							new FarmerSprite.AnimationFrame(25, 500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					case 165:
					case 167:
						goto IL_241D;
					case 168:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(48, 100, false, false, null, false),
							new FarmerSprite.AnimationFrame(49, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(50, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
							new FarmerSprite.AnimationFrame(51, (int)((short)(220 + requester.owner.toolPower * 30)), false, false, null, false),
							new FarmerSprite.AnimationFrame(52, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					default:
						goto IL_241D;
					}
				}
				else
				{
					switch (index)
					{
					case 172:
					case 174:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(58, 0, false, false, null, false),
							new FarmerSprite.AnimationFrame(58, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(59, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
							new FarmerSprite.AnimationFrame(45, 500, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					case 173:
					case 175:
						goto IL_241D;
					case 176:
						requester.loopThisAnimation = false;
						requester.ignoreDefaultActionThisTime = true;
						return new FarmerSprite.AnimationFrame[]
						{
							new FarmerSprite.AnimationFrame(36, 100, false, false, null, false),
							new FarmerSprite.AnimationFrame(37, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
							new FarmerSprite.AnimationFrame(38, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
							new FarmerSprite.AnimationFrame(63, (int)((short)(220 + requester.owner.toolPower * 30)), false, false, null, false),
							new FarmerSprite.AnimationFrame(62, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
						};
					default:
						switch (index)
						{
						case 180:
						case 182:
							requester.loopThisAnimation = false;
							requester.ignoreDefaultActionThisTime = true;
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(62, 0, false, false, null, false),
								new FarmerSprite.AnimationFrame(62, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
								new FarmerSprite.AnimationFrame(63, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
								new FarmerSprite.AnimationFrame(46, 500, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
							};
						case 181:
						case 183:
							goto IL_241D;
						case 184:
							requester.loopThisAnimation = false;
							requester.ignoreDefaultActionThisTime = true;
							return new FarmerSprite.AnimationFrame[]
							{
								new FarmerSprite.AnimationFrame(48, 100, false, true, null, false),
								new FarmerSprite.AnimationFrame(49, 40, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
								new FarmerSprite.AnimationFrame(50, 40, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), false),
								new FarmerSprite.AnimationFrame(51, (int)((short)(220 + requester.owner.toolPower * 30)), false, true, null, false),
								new FarmerSprite.AnimationFrame(52, 75, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
							};
						default:
							goto IL_241D;
						}
						break;
					}
				}
			}
			else if (index <= 216)
			{
				switch (index)
				{
				case 188:
				case 190:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 0, false, true, null, false),
						new FarmerSprite.AnimationFrame(58, 75, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(59, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
						new FarmerSprite.AnimationFrame(45, 500, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 189:
				case 191:
				case 193:
				case 195:
				case 197:
					goto IL_241D;
				case 192:
					index = 3;
					interval = 500;
					goto IL_241D;
				case 194:
					index = 9;
					interval = 500;
					goto IL_241D;
				case 196:
					index = 15;
					interval = 500;
					goto IL_241D;
				case 198:
					index = 9;
					flip = true;
					interval = 500;
					goto IL_241D;
				default:
					if (index != 216)
					{
						goto IL_241D;
					}
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(0, 0),
						new FarmerSprite.AnimationFrame(84, (requester.owner.mostRecentlyGrabbedItem != null && requester.owner.mostRecentlyGrabbedItem is Object && (requester.owner.mostRecentlyGrabbedItem as Object).ParentSheetIndex == 434) ? 1000 : 250, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false),
						new FarmerSprite.AnimationFrame(85, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false),
						new FarmerSprite.AnimationFrame(86, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), true),
						new FarmerSprite.AnimationFrame(86, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), true),
						new FarmerSprite.AnimationFrame(87, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(88, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(87, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(88, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(87, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(0, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false)
					};
				}
			}
			else
			{
				switch (index)
				{
				case 224:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(104, 350, false, false, null, false),
						new FarmerSprite.AnimationFrame(105, 350, false, false, null, false)
					};
				case 225:
				case 226:
				case 227:
				case 228:
				case 229:
				case 230:
				case 231:
				case 233:
				case 235:
				case 236:
				case 237:
				case 238:
				case 239:
				case 241:
				case 244:
				case 245:
				case 246:
				case 247:
				case 249:
				case 250:
				case 251:
				case 253:
				case 254:
				case 255:
				case 257:
				case 260:
				case 261:
				case 262:
				case 263:
				case 264:
				case 265:
				case 266:
				case 267:
				case 268:
				case 269:
				case 270:
				case 271:
				case 273:
				case 275:
				case 277:
					goto IL_241D;
				case 232:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(24, 55, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(25, 45, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(26, 25, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(27, 25, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(28, 25, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(29, (int)((short)interval * 2), showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(29, 0, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 234:
					index = 28;
					secondaryArm = true;
					goto IL_241D;
				case 240:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(30, 55, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(31, 45, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(32, 25, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 25, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(34, 25, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, (int)((short)interval * 2), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, 0, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 242:
				case 243:
					index = 34;
					goto IL_241D;
				case 248:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(36, 55, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(37, 45, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(38, 25, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(39, 25, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(40, 25, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(41, (int)((short)interval * 2), showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(41, 0, showCarryingArm, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 252:
					index = 40;
					secondaryArm = true;
					goto IL_241D;
				case 256:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(30, 55, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(31, 45, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(32, 25, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 25, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(34, 25, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, (int)((short)interval * 2), true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(35, 0, true, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 258:
				case 259:
					index = 34;
					flip = true;
					goto IL_241D;
				case 272:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(25, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(27, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(27, 0, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 274:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(34, (int)((short)interval), false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, (int)((short)interval), false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 276:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(40, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(38, (int)((short)interval), true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(38, 0, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 278:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(34, (int)((short)interval), false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, (int)((short)interval), false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showSwordSwipe), false),
						new FarmerSprite.AnimationFrame(33, 0, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
					};
				case 279:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(62, 0, false, false, null, false),
						new FarmerSprite.AnimationFrame(62, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(63, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(64, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(65, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(65, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 280:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 0, false, false, null, false),
						new FarmerSprite.AnimationFrame(58, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(59, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(60, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 281:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(54, 0, false, false, null, false),
						new FarmerSprite.AnimationFrame(54, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(55, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(56, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(57, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(57, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 282:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 0, false, true, null, false),
						new FarmerSprite.AnimationFrame(58, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(59, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(60, 100, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 200, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false),
						new FarmerSprite.AnimationFrame(61, 0, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showItemIntake), false)
					};
				case 283:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(82, 400),
						new FarmerSprite.AnimationFrame(83, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(82, 400),
						new FarmerSprite.AnimationFrame(83, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 284:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(80, 400),
						new FarmerSprite.AnimationFrame(81, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(80, 400),
						new FarmerSprite.AnimationFrame(81, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 285:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(78, 400),
						new FarmerSprite.AnimationFrame(79, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(78, 400),
						new FarmerSprite.AnimationFrame(79, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 286:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(80, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(81, 400, false, true, new AnimatedSprite.endOfAnimationBehavior(Shears.playSnip), false),
						new FarmerSprite.AnimationFrame(80, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(81, 400, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 287:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(62, 400),
						new FarmerSprite.AnimationFrame(63, 400),
						new FarmerSprite.AnimationFrame(62, 400),
						new FarmerSprite.AnimationFrame(63, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 288:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 400),
						new FarmerSprite.AnimationFrame(59, 400),
						new FarmerSprite.AnimationFrame(58, 400),
						new FarmerSprite.AnimationFrame(59, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 289:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(54, 400),
						new FarmerSprite.AnimationFrame(55, 400),
						new FarmerSprite.AnimationFrame(54, 400),
						new FarmerSprite.AnimationFrame(55, 400, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 290:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(58, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(59, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(58, 400, false, true, null, false),
						new FarmerSprite.AnimationFrame(59, 400, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 291:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(16, 1500),
						new FarmerSprite.AnimationFrame(16, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
					};
				case 292:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(16, 500),
						new FarmerSprite.AnimationFrame(0, 500),
						new FarmerSprite.AnimationFrame(16, 500),
						new FarmerSprite.AnimationFrame(0, 500),
						new FarmerSprite.AnimationFrame(0, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.completelyStopAnimating), false)
					};
				case 293:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(16, 1000),
						new FarmerSprite.AnimationFrame(0, 500),
						new FarmerSprite.AnimationFrame(16, 1000),
						new FarmerSprite.AnimationFrame(4, 200),
						new FarmerSprite.AnimationFrame(5, 2000, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.doSleepEmote), false),
						new FarmerSprite.AnimationFrame(5, 2000, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.passOutFromTired), false),
						new FarmerSprite.AnimationFrame(5, 2000)
					};
				case 294:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(0, 1),
						new FarmerSprite.AnimationFrame(90, 250),
						new FarmerSprite.AnimationFrame(91, 150),
						new FarmerSprite.AnimationFrame(92, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(93, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.drinkGlug), false),
						new FarmerSprite.AnimationFrame(92, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(93, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.drinkGlug), false),
						new FarmerSprite.AnimationFrame(92, 250, false, false, null, false),
						new FarmerSprite.AnimationFrame(93, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.drinkGlug), false),
						new FarmerSprite.AnimationFrame(91, 250),
						new FarmerSprite.AnimationFrame(90, 50)
					};
				case 295:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(76, 100, false, false, null, false),
						new FarmerSprite.AnimationFrame(38, 40, false, false, null, false),
						new FarmerSprite.AnimationFrame(63, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(62, 80, false, false, null, false),
						new FarmerSprite.AnimationFrame(63, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 296:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(48, 100, false, false, null, false),
						new FarmerSprite.AnimationFrame(49, 40, false, false, null, false),
						new FarmerSprite.AnimationFrame(50, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(51, 80, false, false, null, false),
						new FarmerSprite.AnimationFrame(52, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 297:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(66, 100, false, false, null, false),
						new FarmerSprite.AnimationFrame(67, 40, false, false, null, false),
						new FarmerSprite.AnimationFrame(68, 40, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(69, 80, false, false, null, false),
						new FarmerSprite.AnimationFrame(70, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 298:
					requester.loopThisAnimation = false;
					requester.ignoreDefaultActionThisTime = true;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(48, 100, false, true, null, false),
						new FarmerSprite.AnimationFrame(49, 40, false, true, null, false),
						new FarmerSprite.AnimationFrame(50, 40, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
						new FarmerSprite.AnimationFrame(51, 80, false, true, null, false),
						new FarmerSprite.AnimationFrame(52, 200, false, true, new AnimatedSprite.endOfAnimationBehavior(FishingRod.doneWithCastingAnimation), true)
					};
				case 299:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(76, 5000, false, false, null, false)
					};
				case 300:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(72, 5000, false, false, null, false)
					};
				case 301:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(74, 5000, false, false, null, false)
					};
				case 302:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(72, 5000, false, true, null, false)
					};
				case 303:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(125, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(125, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(125, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(123, 150, false, true, null, false),
						new FarmerSprite.AnimationFrame(124, 150, false, true, new AnimatedSprite.endOfAnimationBehavior(Pan.playSlosh), false),
						new FarmerSprite.AnimationFrame(123, 500, false, true, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true)
					};
				case 304:
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(84, 99999999, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showEatingItem), false)
					};
				default:
					if (index != 999996)
					{
						goto IL_241D;
					}
					requester.loopThisAnimation = false;
					return new FarmerSprite.AnimationFrame[]
					{
						new FarmerSprite.AnimationFrame(96, 800, false, false, null, false)
					};
				}
			}
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(1 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(2 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(frameOffset, 200, showCarryingArm, false, null, false)
			};
			IL_481:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(7 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(6 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(8 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(6 + frameOffset, 200, showCarryingArm, false, null, false)
			};
			IL_4E8:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(13 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(12 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(14 + frameOffset, 200, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(12 + frameOffset, 200, showCarryingArm, false, null, false)
			};
			IL_553:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(7 + frameOffset, 200, showCarryingArm, true, null, false),
				new FarmerSprite.AnimationFrame(6 + frameOffset, 200, showCarryingArm, true, null, false),
				new FarmerSprite.AnimationFrame(8 + frameOffset, 200, showCarryingArm, true, null, false),
				new FarmerSprite.AnimationFrame(6 + frameOffset, 200, showCarryingArm, true, null, false)
			};
			IL_5BA:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(0, 90, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(1, 60, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(18, 120, -4, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(1, 60, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(0, 90, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(2, 60, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(19, 120, -4, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(2, 60, -2, showCarryingArm, false, null, false, 0)
			};
			IL_66D:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(6, 80, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(20, 140, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(11, 100, 0, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(6, 80, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(21, 140, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(17, 100, 0, showCarryingArm, false, null, false, 0)
			};
			IL_724:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(12, 90, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(13, 60, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(22, 120, -3, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(13, 60, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(12, 90, showCarryingArm, false, null, false),
				new FarmerSprite.AnimationFrame(14, 60, -2, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(23, 120, -3, showCarryingArm, false, null, false, 0),
				new FarmerSprite.AnimationFrame(14, 60, -2, showCarryingArm, false, null, false, 0)
			};
			IL_7DD:
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(6, 80, showCarryingArm, true, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, showCarryingArm, true, null, false, 0),
				new FarmerSprite.AnimationFrame(20, 140, -2, showCarryingArm, true, null, false, 0),
				new FarmerSprite.AnimationFrame(11, 100, 0, showCarryingArm, true, null, false, 0),
				new FarmerSprite.AnimationFrame(6, 80, showCarryingArm, true, null, false),
				new FarmerSprite.AnimationFrame(6, 10, -1, showCarryingArm, true, null, false, 0),
				new FarmerSprite.AnimationFrame(21, 140, -2, showCarryingArm, true, null, false, 0),
				new FarmerSprite.AnimationFrame(17, 100, 0, showCarryingArm, true, null, false, 0)
			};
			IL_165F:
			requester.loopThisAnimation = false;
			return new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(6, 0, false, true, null, false)
			};
			IL_241D:
			if (index > FarmerRenderer.featureYOffsetPerFrame.Length - 1)
			{
				index = 0;
			}
			requester.loopThisAnimation = false;
			FarmerSprite.AnimationFrame[] frames = new FarmerSprite.AnimationFrame[numberOfFrames];
			for (int i = 0; i < numberOfFrames; i++)
			{
				frames[i] = new FarmerSprite.AnimationFrame((int)((short)(i + index)), (int)((short)interval), secondaryArm, flip, null, false);
			}
			return frames;
		}

		// Token: 0x040004B1 RID: 1201
		public const int walkDown = 0;

		// Token: 0x040004B2 RID: 1202
		public const int walkRight = 8;

		// Token: 0x040004B3 RID: 1203
		public const int walkUp = 16;

		// Token: 0x040004B4 RID: 1204
		public const int walkLeft = 24;

		// Token: 0x040004B5 RID: 1205
		public const int runDown = 32;

		// Token: 0x040004B6 RID: 1206
		public const int runRight = 40;

		// Token: 0x040004B7 RID: 1207
		public const int runUp = 48;

		// Token: 0x040004B8 RID: 1208
		public const int runLeft = 56;

		// Token: 0x040004B9 RID: 1209
		public const int grabDown = 64;

		// Token: 0x040004BA RID: 1210
		public const int grabRight = 72;

		// Token: 0x040004BB RID: 1211
		public const int grabUp = 80;

		// Token: 0x040004BC RID: 1212
		public const int grabLeft = 88;

		// Token: 0x040004BD RID: 1213
		public const int carryWalkDown = 96;

		// Token: 0x040004BE RID: 1214
		public const int carryWalkRight = 104;

		// Token: 0x040004BF RID: 1215
		public const int carryWalkUp = 112;

		// Token: 0x040004C0 RID: 1216
		public const int carryWalkLeft = 120;

		// Token: 0x040004C1 RID: 1217
		public const int carryRunDown = 128;

		// Token: 0x040004C2 RID: 1218
		public const int carryRunRight = 136;

		// Token: 0x040004C3 RID: 1219
		public const int carryRunUp = 144;

		// Token: 0x040004C4 RID: 1220
		public const int carryRunLeft = 152;

		// Token: 0x040004C5 RID: 1221
		public const int toolDown = 160;

		// Token: 0x040004C6 RID: 1222
		public const int toolRight = 168;

		// Token: 0x040004C7 RID: 1223
		public const int toolUp = 176;

		// Token: 0x040004C8 RID: 1224
		public const int toolLeft = 184;

		// Token: 0x040004C9 RID: 1225
		public const int toolChooseDown = 192;

		// Token: 0x040004CA RID: 1226
		public const int toolChooseRight = 194;

		// Token: 0x040004CB RID: 1227
		public const int toolChooseUp = 196;

		// Token: 0x040004CC RID: 1228
		public const int toolChooseLeft = 198;

		// Token: 0x040004CD RID: 1229
		public const int seedThrowDown = 200;

		// Token: 0x040004CE RID: 1230
		public const int seedThrowRight = 204;

		// Token: 0x040004CF RID: 1231
		public const int seedThrowUp = 208;

		// Token: 0x040004D0 RID: 1232
		public const int seedThrowLeft = 212;

		// Token: 0x040004D1 RID: 1233
		public const int eat = 216;

		// Token: 0x040004D2 RID: 1234
		public const int sick = 224;

		// Token: 0x040004D3 RID: 1235
		public const int swordswipeDown = 232;

		// Token: 0x040004D4 RID: 1236
		public const int swordswipeRight = 240;

		// Token: 0x040004D5 RID: 1237
		public const int swordswipeUp = 248;

		// Token: 0x040004D6 RID: 1238
		public const int swordswipeLeft = 256;

		// Token: 0x040004D7 RID: 1239
		public const int punchDown = 272;

		// Token: 0x040004D8 RID: 1240
		public const int punchRight = 274;

		// Token: 0x040004D9 RID: 1241
		public const int punchUp = 276;

		// Token: 0x040004DA RID: 1242
		public const int punchLeft = 278;

		// Token: 0x040004DB RID: 1243
		public const int harvestItemUp = 279;

		// Token: 0x040004DC RID: 1244
		public const int harvestItemRight = 280;

		// Token: 0x040004DD RID: 1245
		public const int harvestItemDown = 281;

		// Token: 0x040004DE RID: 1246
		public const int harvestItemLeft = 282;

		// Token: 0x040004DF RID: 1247
		public const int shearUp = 283;

		// Token: 0x040004E0 RID: 1248
		public const int shearRight = 284;

		// Token: 0x040004E1 RID: 1249
		public const int shearDown = 285;

		// Token: 0x040004E2 RID: 1250
		public const int shearLeft = 286;

		// Token: 0x040004E3 RID: 1251
		public const int milkUp = 287;

		// Token: 0x040004E4 RID: 1252
		public const int milkRight = 288;

		// Token: 0x040004E5 RID: 1253
		public const int milkDown = 289;

		// Token: 0x040004E6 RID: 1254
		public const int milkLeft = 290;

		// Token: 0x040004E7 RID: 1255
		public const int tired = 291;

		// Token: 0x040004E8 RID: 1256
		public const int tired2 = 292;

		// Token: 0x040004E9 RID: 1257
		public const int passOutTired = 293;

		// Token: 0x040004EA RID: 1258
		public const int drink = 294;

		// Token: 0x040004EB RID: 1259
		public const int fishingUp = 295;

		// Token: 0x040004EC RID: 1260
		public const int fishingRight = 296;

		// Token: 0x040004ED RID: 1261
		public const int fishingDown = 297;

		// Token: 0x040004EE RID: 1262
		public const int fishingLeft = 298;

		// Token: 0x040004EF RID: 1263
		public const int fishingDoneUp = 299;

		// Token: 0x040004F0 RID: 1264
		public const int fishingDoneRight = 300;

		// Token: 0x040004F1 RID: 1265
		public const int fishingDoneDown = 301;

		// Token: 0x040004F2 RID: 1266
		public const int fishingDoneLeft = 302;

		// Token: 0x040004F3 RID: 1267
		public const int pan = 303;

		// Token: 0x040004F4 RID: 1268
		public const int showHoldingEdible = 304;

		// Token: 0x040004F5 RID: 1269
		private int currentToolIndex;

		// Token: 0x040004F6 RID: 1270
		private float oldInterval;

		// Token: 0x040004F7 RID: 1271
		public bool pauseForSingleAnimation;

		// Token: 0x040004F8 RID: 1272
		public bool animateBackwards;

		// Token: 0x040004F9 RID: 1273
		public bool loopThisAnimation;

		// Token: 0x040004FA RID: 1274
		public bool ignoreDefaultActionThisTime;

		// Token: 0x040004FB RID: 1275
		public bool freezeUntilDialogueIsOver;

		// Token: 0x040004FC RID: 1276
		private int currentSingleAnimation;

		// Token: 0x040004FD RID: 1277
		private int currentAnimationFrames;

		// Token: 0x040004FE RID: 1278
		public int indexInCurrentAnimation;

		// Token: 0x040004FF RID: 1279
		public float currentSingleAnimationInterval = 200f;

		// Token: 0x04000500 RID: 1280
		public float intervalModifier = 1f;

		// Token: 0x04000501 RID: 1281
		public string currentStep = "sandyStep";

		// Token: 0x04000502 RID: 1282
		private Farmer owner;

		// Token: 0x04000503 RID: 1283
		public int nextOffset;

		// Token: 0x04000504 RID: 1284
		public bool animatingBackwards;

		// Token: 0x04000505 RID: 1285
		public const int cheer = 97;

		// Token: 0x0200016D RID: 365
		public struct AnimationFrame
		{
			// Token: 0x0600136F RID: 4975 RVA: 0x00184BB5 File Offset: 0x00182DB5
			public AnimationFrame(int frame, int milliseconds, int positionOffset, bool secondaryArm, bool flip, AnimatedSprite.endOfAnimationBehavior frameBehavior = null, bool behaviorAtEndOfFrame = false, int xOffset = 0)
			{
				this.frame = frame;
				this.milliseconds = milliseconds;
				this.positionOffset = positionOffset;
				this.secondaryArm = secondaryArm;
				this.flip = flip;
				this.frameBehavior = frameBehavior;
				this.behaviorAtEndOfFrame = behaviorAtEndOfFrame;
				this.xOffset = xOffset;
			}

			// Token: 0x06001370 RID: 4976 RVA: 0x00184BF4 File Offset: 0x00182DF4
			public AnimationFrame(int frame, int milliseconds, bool secondaryArm, bool flip, AnimatedSprite.endOfAnimationBehavior frameBehavior = null, bool behaviorAtEndOfFrame = false)
			{
				this.frame = frame;
				this.milliseconds = milliseconds;
				this.positionOffset = 0;
				this.secondaryArm = secondaryArm;
				this.flip = flip;
				this.frameBehavior = frameBehavior;
				this.behaviorAtEndOfFrame = behaviorAtEndOfFrame;
				this.xOffset = 0;
			}

			// Token: 0x06001371 RID: 4977 RVA: 0x00184C31 File Offset: 0x00182E31
			public AnimationFrame(int frame, int milliseconds)
			{
				this = new FarmerSprite.AnimationFrame(frame, milliseconds, false, false, null, false);
			}

			// Token: 0x04001444 RID: 5188
			public int frame;

			// Token: 0x04001445 RID: 5189
			public int milliseconds;

			// Token: 0x04001446 RID: 5190
			public int positionOffset;

			// Token: 0x04001447 RID: 5191
			public int xOffset;

			// Token: 0x04001448 RID: 5192
			public bool secondaryArm;

			// Token: 0x04001449 RID: 5193
			public bool flip;

			// Token: 0x0400144A RID: 5194
			public bool behaviorAtEndOfFrame;

			// Token: 0x0400144B RID: 5195
			public AnimatedSprite.endOfAnimationBehavior frameBehavior;
		}
	}
}
