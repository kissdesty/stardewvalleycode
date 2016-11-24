using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000052 RID: 82
	public class TemporaryAnimatedSprite
	{
		// Token: 0x170000B0 RID: 176
		public Vector2 Position
		{
			// Token: 0x06000785 RID: 1925 RVA: 0x000A65D6 File Offset: 0x000A47D6
			get
			{
				return this.position;
			}
			// Token: 0x06000786 RID: 1926 RVA: 0x000A65DE File Offset: 0x000A47DE
			set
			{
				this.position = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		public Texture2D Texture
		{
			// Token: 0x06000787 RID: 1927 RVA: 0x000A65E7 File Offset: 0x000A47E7
			get
			{
				return this.texture;
			}
			// Token: 0x06000788 RID: 1928 RVA: 0x000A65EF File Offset: 0x000A47EF
			set
			{
				this.texture = value;
			}
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x000A65F8 File Offset: 0x000A47F8
		public TemporaryAnimatedSprite getClone()
		{
			return new TemporaryAnimatedSprite
			{
				texture = this.texture,
				interval = this.interval,
				currentParentTileIndex = this.currentParentTileIndex,
				oldCurrentParentTileIndex = this.oldCurrentParentTileIndex,
				initialParentTileIndex = this.initialParentTileIndex,
				totalNumberOfLoops = this.totalNumberOfLoops,
				currentNumberOfLoops = this.currentNumberOfLoops,
				xStopCoordinate = this.xStopCoordinate,
				yStopCoordinate = this.yStopCoordinate,
				animationLength = this.animationLength,
				bombRadius = this.bombRadius,
				pingPongMotion = this.pingPongMotion,
				flicker = this.flicker,
				timeBasedMotion = this.timeBasedMotion,
				overrideLocationDestroy = this.overrideLocationDestroy,
				pingPong = this.pingPong,
				holdLastFrame = this.holdLastFrame,
				extraInfoForEndBehavior = this.extraInfoForEndBehavior,
				lightID = this.lightID,
				acceleration = this.acceleration,
				accelerationChange = this.accelerationChange,
				alpha = this.alpha,
				alphaFade = this.alphaFade,
				attachedCharacter = this.attachedCharacter,
				bigCraftable = this.bigCraftable,
				color = this.color,
				delayBeforeAnimationStart = this.delayBeforeAnimationStart,
				destroyable = this.destroyable,
				endFunction = this.endFunction,
				endSound = this.endSound,
				flash = this.flash,
				flipped = this.flipped,
				hasLit = this.hasLit,
				id = this.id,
				initialPosition = this.initialPosition,
				light = this.light,
				local = this.local,
				motion = this.motion,
				owner = this.owner,
				parent = this.parent,
				parentSprite = this.parentSprite,
				position = this.position,
				rotation = this.rotation,
				rotationChange = this.rotationChange,
				scale = this.scale,
				scaleChange = this.scaleChange,
				scaleChangeChange = this.scaleChangeChange,
				shakeIntensity = this.shakeIntensity,
				shakeIntensityChange = this.shakeIntensityChange,
				sourceRect = this.sourceRect,
				sourceRectStartingPos = this.sourceRectStartingPos,
				startSound = this.startSound,
				timeBasedMotion = this.timeBasedMotion,
				verticalFlipped = this.verticalFlipped,
				xPeriodic = this.xPeriodic,
				xPeriodicLoopTime = this.xPeriodicLoopTime,
				xPeriodicRange = this.xPeriodicRange,
				yPeriodic = this.yPeriodic,
				yPeriodicLoopTime = this.yPeriodicLoopTime,
				yPeriodicRange = this.yPeriodicRange,
				yStopCoordinate = this.yStopCoordinate,
				totalNumberOfLoops = this.totalNumberOfLoops
			};
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000A68F2 File Offset: 0x000A4AF2
		public static void addMoneyToFarmerEndBehavior(int extraInfo)
		{
			Game1.player.money += extraInfo;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000A6908 File Offset: 0x000A4B08
		public TemporaryAnimatedSprite()
		{
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000A69A8 File Offset: 0x000A4BA8
		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped)
		{
			if (initialParentTileIndex == -1)
			{
				this.swordswipe = true;
				this.currentParentTileIndex = 0;
			}
			else
			{
				this.currentParentTileIndex = initialParentTileIndex;
			}
			this.initialParentTileIndex = initialParentTileIndex;
			this.interval = animationInterval;
			this.totalNumberOfLoops = numberOfLoops;
			this.position = position;
			this.animationLength = animationLength;
			this.flicker = flicker;
			this.flipped = flipped;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000A6A98 File Offset: 0x000A4C98
		public TemporaryAnimatedSprite(int rowInAnimationTexture, Vector2 position, Color color, int animationLength = 8, bool flipped = false, float animationInterval = 100f, int numberOfLoops = 0, int sourceRectWidth = -1, float layerDepth = -1f, int sourceRectHeight = -1, int delay = 0) : this(Game1.animations, new Rectangle(0, rowInAnimationTexture * Game1.tileSize, sourceRectWidth, sourceRectHeight), animationInterval, animationLength, numberOfLoops, position, false, flipped, layerDepth, 0f, color, 1f, 0f, 0f, 0f, false)
		{
			if (sourceRectWidth == -1)
			{
				sourceRectWidth = Game1.tileSize;
				this.sourceRect.Width = Game1.tileSize;
			}
			if (sourceRectHeight == -1)
			{
				sourceRectHeight = Game1.tileSize;
				this.sourceRect.Height = Game1.tileSize;
			}
			if (layerDepth == -1f)
			{
				layerDepth = (position.Y + (float)(Game1.tileSize / 2)) / 10000f;
			}
			this.delayBeforeAnimationStart = delay;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000A6B48 File Offset: 0x000A4D48
		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, bool verticalFlipped, float rotation) : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.rotation = rotation;
			this.verticalFlipped = verticalFlipped;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x000A6B6B File Offset: 0x000A4D6B
		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool bigCraftable, bool flipped) : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.bigCraftable = bigCraftable;
			if (bigCraftable)
			{
				this.position.Y = this.position.Y - (float)Game1.tileSize;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000A6BA0 File Offset: 0x000A4DA0
		public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped) : this(0, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.Texture = texture;
			this.sourceRect = sourceRect;
			this.sourceRectStartingPos = new Vector2((float)sourceRect.X, (float)sourceRect.Y);
			this.initialPosition = position;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000A6BF0 File Offset: 0x000A4DF0
		public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, float layerDepth, float alphaFade, Color color, float scale, float scaleChange, float rotation, float rotationChange, bool local = false) : this(0, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.Texture = texture;
			this.sourceRect = sourceRect;
			this.sourceRectStartingPos = new Vector2((float)sourceRect.X, (float)sourceRect.Y);
			this.layerDepth = layerDepth;
			this.alphaFade = Math.Max(0f, alphaFade);
			this.color = color;
			this.scale = scale;
			this.scaleChange = scaleChange;
			this.rotation = rotation;
			this.rotationChange = rotationChange;
			this.local = local;
			this.initialPosition = position;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000A6C88 File Offset: 0x000A4E88
		public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, bool flipped, float alphaFade, Color color) : this(0, 999999f, 1, 0, position, false, flipped)
		{
			this.Texture = texture;
			this.sourceRect = sourceRect;
			this.sourceRectStartingPos = new Vector2((float)sourceRect.X, (float)sourceRect.Y);
			this.initialPosition = position;
			this.alphaFade = Math.Max(0f, alphaFade);
			this.color = color;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000A6CF0 File Offset: 0x000A4EF0
		public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, GameLocation parent, Farmer owner) : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
		{
			this.position.X = (float)((int)this.position.X);
			this.position.Y = (float)((int)this.position.Y);
			this.parent = parent;
			switch (initialParentTileIndex)
			{
			case 286:
				this.bombRadius = 3;
				break;
			case 287:
				this.bombRadius = 5;
				break;
			case 288:
				this.bombRadius = 7;
				break;
			}
			this.owner = owner;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000A6D84 File Offset: 0x000A4F84
		public virtual void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			if (this.local)
			{
				localPosition = true;
			}
			if (this.currentParentTileIndex >= 0 && this.delayBeforeAnimationStart <= 0)
			{
				if (this.text != null)
				{
					spriteBatch.DrawString(Game1.dialogueFont, this.text, localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, this.Position), this.color * this.alpha, this.rotation, Vector2.Zero, this.scale, SpriteEffects.None, this.layerDepth);
					return;
				}
				if (this.Texture != null)
				{
					spriteBatch.Draw(this.Texture, (localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((int)this.Position.X + xOffset), (float)((int)this.Position.Y + yOffset)))) + new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)) * this.scale + new Vector2((float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0), (float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0)), new Rectangle?(this.sourceRect), this.color * this.alpha, this.rotation, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, this.flipped ? SpriteEffects.FlipHorizontally : (this.verticalFlipped ? SpriteEffects.FlipVertically : SpriteEffects.None), (this.layerDepth >= 0f) ? this.layerDepth : ((this.Position.Y + (float)this.sourceRect.Height) / 10000f));
					return;
				}
				if (this.bigCraftable)
				{
					spriteBatch.Draw(Game1.bigCraftableSpriteSheet, localPosition ? this.Position : (Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((int)this.Position.X + xOffset), (float)((int)this.Position.Y + yOffset))) + new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2))), new Rectangle?(Object.getSourceRectForBigCraftable(this.currentParentTileIndex)), Color.White, 0f, new Vector2((float)(this.sourceRect.Width / 2), (float)(this.sourceRect.Height / 2)), this.scale, SpriteEffects.None, (this.Position.Y + (float)(Game1.tileSize / 2)) / 10000f);
					return;
				}
				if (!this.swordswipe)
				{
					spriteBatch.Draw(Game1.objectSpriteSheet, localPosition ? this.Position : (Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((int)this.Position.X + xOffset), (float)((int)this.Position.Y + yOffset))) + new Vector2(8f, 8f) * (float)Game1.pixelZoom + new Vector2((float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0), (float)((this.shakeIntensity > 0f) ? Game1.random.Next(-(int)this.shakeIntensity, (int)this.shakeIntensity + 1) : 0))), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.currentParentTileIndex)), (this.flash ? (Color.LightBlue * 0.85f) : Color.White) * this.alpha, this.rotation, new Vector2(8f, 8f), (float)Game1.pixelZoom * this.scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (this.layerDepth >= 0f) ? this.layerDepth : ((this.Position.Y + (float)(Game1.tileSize / 2)) / 10000f));
				}
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000A71CC File Offset: 0x000A53CC
		public void unload()
		{
			if (this.endSound != null)
			{
				Game1.playSound(this.endSound);
			}
			if (this.endFunction != null)
			{
				this.endFunction(this.extraInfoForEndBehavior);
			}
			if (this.hasLit)
			{
				Utility.removeLightSource(this.lightID);
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x000A721C File Offset: 0x000A541C
		public void reset()
		{
			this.sourceRect.X = (int)this.sourceRectStartingPos.X;
			this.sourceRect.Y = (int)this.sourceRectStartingPos.Y;
			this.currentParentTileIndex = 0;
			this.oldCurrentParentTileIndex = 0;
			this.timer = 0f;
			this.totalTimer = 0f;
			this.currentNumberOfLoops = 0;
			this.pingPongMotion = 1;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x000A728C File Offset: 0x000A548C
		public virtual bool update(GameTime time)
		{
			if (this.paused)
			{
				return false;
			}
			if (this.bombRadius > 0 && Game1.activeClickableMenu != null)
			{
				return false;
			}
			if (this.delayBeforeAnimationStart > 0)
			{
				this.delayBeforeAnimationStart -= time.ElapsedGameTime.Milliseconds;
				if (this.delayBeforeAnimationStart <= 0 && this.startSound != null)
				{
					Game1.playSound(this.startSound);
				}
				if (this.delayBeforeAnimationStart <= 0 && this.parentSprite != null)
				{
					this.position = this.parentSprite.position + this.position;
				}
				return false;
			}
			this.timer += (float)time.ElapsedGameTime.Milliseconds;
			this.totalTimer += (float)time.ElapsedGameTime.Milliseconds;
			this.alpha -= this.alphaFade * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			if (this.alphaFade > 0f && this.light && this.alpha < 1f && this.alpha >= 0f)
			{
				try
				{
					Utility.getLightSource(this.lightID).color.A = (byte)(255f * this.alpha);
				}
				catch (Exception)
				{
				}
			}
			this.shakeIntensity += this.shakeIntensityChange * (float)time.ElapsedGameTime.Milliseconds;
			this.scale += this.scaleChange * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.scaleChange += this.scaleChangeChange * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.rotation += this.rotationChange;
			if (this.xPeriodic)
			{
				this.position.X = this.initialPosition.X + this.xPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.xPeriodicLoopTime * (double)this.totalTimer);
			}
			else
			{
				this.position.X = this.position.X + this.motion.X * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			}
			if (this.yPeriodic)
			{
				this.position.Y = this.initialPosition.Y + this.yPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.yPeriodicLoopTime * (double)(this.totalTimer + this.yPeriodicLoopTime / 2f));
			}
			else
			{
				this.position.Y = this.position.Y + this.motion.Y * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			}
			if (this.attachedCharacter != null)
			{
				if (this.xPeriodic)
				{
					this.attachedCharacter.position.X = this.initialPosition.X + this.xPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.xPeriodicLoopTime * (double)this.totalTimer);
				}
				else
				{
					Character expr_348_cp_0_cp_0 = this.attachedCharacter;
					expr_348_cp_0_cp_0.position.X = expr_348_cp_0_cp_0.position.X + this.motion.X * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
				}
				if (this.yPeriodic)
				{
					this.attachedCharacter.position.Y = this.initialPosition.Y + this.yPeriodicRange * (float)Math.Sin(6.2831853071795862 / (double)this.yPeriodicLoopTime * (double)this.totalTimer);
				}
				else
				{
					Character expr_3CF_cp_0_cp_0 = this.attachedCharacter;
					expr_3CF_cp_0_cp_0.position.Y = expr_3CF_cp_0_cp_0.position.Y + this.motion.Y * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
				}
			}
			this.motion.X = this.motion.X + this.acceleration.X * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.motion.Y = this.motion.Y + this.acceleration.Y * (float)(this.timeBasedMotion ? time.ElapsedGameTime.Milliseconds : 1);
			this.acceleration.X = this.acceleration.X + this.accelerationChange.X;
			this.acceleration.Y = this.acceleration.Y + this.accelerationChange.Y;
			if (this.xStopCoordinate != -1 || this.yStopCoordinate != -1)
			{
				if (this.xStopCoordinate != -1 && Math.Abs(this.position.X - (float)this.xStopCoordinate) <= Math.Abs(this.motion.X))
				{
					this.motion.X = 0f;
					this.acceleration.X = 0f;
					this.xStopCoordinate = -1;
				}
				if (this.yStopCoordinate != -1 && Math.Abs(this.position.Y - (float)this.yStopCoordinate) <= Math.Abs(this.motion.Y))
				{
					this.motion.Y = 0f;
					this.acceleration.Y = 0f;
					this.yStopCoordinate = -1;
				}
				if (this.xStopCoordinate == -1 && this.yStopCoordinate == -1)
				{
					this.rotationChange = 0f;
					if (this.reachedStopCoordinate != null)
					{
						this.reachedStopCoordinate(0);
					}
				}
			}
			if (!this.pingPong)
			{
				this.pingPongMotion = 1;
			}
			if (this.pulse)
			{
				this.pulseTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.originalScale == 0f)
				{
					this.originalScale = this.scale;
				}
				if (this.pulseTimer <= 0f)
				{
					this.pulseTimer = this.pulseTime;
					this.scale = this.originalScale * this.pulseAmount;
				}
				if (this.scale > this.originalScale)
				{
					this.scale -= this.pulseAmount / 100f * (float)time.ElapsedGameTime.Milliseconds;
				}
			}
			if (this.light)
			{
				if (!this.hasLit)
				{
					this.hasLit = true;
					this.lightID = Game1.random.Next(-2147483648, 2147483647);
					Game1.currentLightSources.Add(new LightSource(4, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.lightRadius, this.lightcolor.Equals(Color.White) ? new Color(0, 131, 255) : this.lightcolor, this.lightID));
				}
				else
				{
					Utility.repositionLightSource(this.lightID, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)));
				}
			}
			if (this.alpha <= 0f || (this.position.X < -2000f && !this.overrideLocationDestroy) || this.scale <= 0f)
			{
				this.unload();
				return this.destroyable;
			}
			if (this.timer > this.interval)
			{
				this.currentParentTileIndex += this.pingPongMotion;
				this.sourceRect.X = this.sourceRect.X + this.sourceRect.Width * this.pingPongMotion;
				if (this.Texture != null)
				{
					if (!this.pingPong && this.sourceRect.X >= this.Texture.Width)
					{
						this.sourceRect.Y = this.sourceRect.Y + this.sourceRect.Height;
					}
					if (!this.pingPong)
					{
						this.sourceRect.X = this.sourceRect.X % this.Texture.Width;
					}
					if (this.pingPong)
					{
						if ((float)this.sourceRect.X + ((float)this.sourceRect.Y - this.sourceRectStartingPos.Y) / (float)this.sourceRect.Height * (float)this.Texture.Width >= this.sourceRectStartingPos.X + (float)(this.sourceRect.Width * this.animationLength))
						{
							this.pingPongMotion = -1;
							this.sourceRect.X = this.sourceRect.X - this.sourceRect.Width * 2;
							this.currentParentTileIndex--;
							if (this.sourceRect.X < 0)
							{
								this.sourceRect.X = this.Texture.Width + this.sourceRect.X;
							}
						}
						else if ((float)this.sourceRect.X < this.sourceRectStartingPos.X && (float)this.sourceRect.Y == this.sourceRectStartingPos.Y)
						{
							this.pingPongMotion = 1;
							this.sourceRect.X = (int)this.sourceRectStartingPos.X + this.sourceRect.Width;
							this.currentParentTileIndex++;
							this.currentNumberOfLoops++;
							if (this.endFunction != null)
							{
								this.endFunction(this.extraInfoForEndBehavior);
								this.endFunction = null;
							}
							if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
							{
								this.unload();
								return this.destroyable;
							}
						}
					}
					else if (this.totalNumberOfLoops >= 1 && (float)this.sourceRect.X + ((float)this.sourceRect.Y - this.sourceRectStartingPos.Y) / (float)this.sourceRect.Height * (float)this.Texture.Width >= this.sourceRectStartingPos.X + (float)(this.sourceRect.Width * this.animationLength))
					{
						this.sourceRect.X = (int)this.sourceRectStartingPos.X;
						this.sourceRect.Y = (int)this.sourceRectStartingPos.Y;
					}
				}
				this.timer = 0f;
				if (this.flicker)
				{
					if (this.currentParentTileIndex < 0 || this.flash)
					{
						this.currentParentTileIndex = this.oldCurrentParentTileIndex;
						this.flash = false;
					}
					else
					{
						this.oldCurrentParentTileIndex = this.currentParentTileIndex;
						if (this.bombRadius > 0)
						{
							this.flash = true;
						}
						else
						{
							this.currentParentTileIndex = -100;
						}
					}
				}
				if (this.currentParentTileIndex - this.initialParentTileIndex >= this.animationLength)
				{
					this.currentNumberOfLoops++;
					if (this.holdLastFrame)
					{
						this.currentParentTileIndex = this.initialParentTileIndex + this.animationLength - 1;
						this.setSourceRectToCurrentTileIndex();
						if (this.endFunction != null)
						{
							this.endFunction(this.extraInfoForEndBehavior);
							this.endFunction = null;
						}
						return false;
					}
					this.currentParentTileIndex = this.initialParentTileIndex;
					if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
					{
						if (this.bombRadius > 0)
						{
							if (Game1.fuseSound != null)
							{
								Game1.fuseSound.Stop(AudioStopOptions.AsAuthored);
								Game1.fuseSound = Game1.soundBank.GetCue("fuse");
							}
							Game1.playSound("explosion");
							Game1.flashAlpha = 1f;
							this.parent.explode(new Vector2((float)((int)(this.position.X / (float)Game1.tileSize)), (float)((int)(this.position.Y / (float)Game1.tileSize))), this.bombRadius, this.owner);
						}
						this.unload();
						return this.destroyable;
					}
					if (this.bombRadius > 0 && this.currentNumberOfLoops == this.totalNumberOfLoops - 5)
					{
						this.interval -= this.interval / 3f;
					}
				}
			}
			return false;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x000A7E7C File Offset: 0x000A607C
		private void setSourceRectToCurrentTileIndex()
		{
			this.sourceRect.X = (int)(this.sourceRectStartingPos.X + (float)(this.currentParentTileIndex * this.sourceRect.Width)) % this.texture.Width;
			if (this.sourceRect.X < 0)
			{
				this.sourceRect.X = 0;
			}
			this.sourceRect.Y = (int)this.sourceRectStartingPos.Y;
		}

		// Token: 0x0400082A RID: 2090
		public float timer;

		// Token: 0x0400082B RID: 2091
		public float interval = 200f;

		// Token: 0x0400082C RID: 2092
		public int currentParentTileIndex;

		// Token: 0x0400082D RID: 2093
		public int oldCurrentParentTileIndex;

		// Token: 0x0400082E RID: 2094
		public int initialParentTileIndex;

		// Token: 0x0400082F RID: 2095
		public int totalNumberOfLoops;

		// Token: 0x04000830 RID: 2096
		public int currentNumberOfLoops;

		// Token: 0x04000831 RID: 2097
		public int xStopCoordinate = -1;

		// Token: 0x04000832 RID: 2098
		public int yStopCoordinate = -1;

		// Token: 0x04000833 RID: 2099
		public int animationLength;

		// Token: 0x04000834 RID: 2100
		public int bombRadius;

		// Token: 0x04000835 RID: 2101
		public int pingPongMotion = 1;

		// Token: 0x04000836 RID: 2102
		public bool flicker;

		// Token: 0x04000837 RID: 2103
		public bool timeBasedMotion;

		// Token: 0x04000838 RID: 2104
		public bool overrideLocationDestroy;

		// Token: 0x04000839 RID: 2105
		public bool pingPong;

		// Token: 0x0400083A RID: 2106
		public bool holdLastFrame;

		// Token: 0x0400083B RID: 2107
		public bool pulse;

		// Token: 0x0400083C RID: 2108
		public int extraInfoForEndBehavior;

		// Token: 0x0400083D RID: 2109
		public int lightID;

		// Token: 0x0400083E RID: 2110
		public bool bigCraftable;

		// Token: 0x0400083F RID: 2111
		public bool swordswipe;

		// Token: 0x04000840 RID: 2112
		public bool flash;

		// Token: 0x04000841 RID: 2113
		public bool flipped;

		// Token: 0x04000842 RID: 2114
		public bool verticalFlipped;

		// Token: 0x04000843 RID: 2115
		public bool local;

		// Token: 0x04000844 RID: 2116
		public bool light;

		// Token: 0x04000845 RID: 2117
		public bool hasLit;

		// Token: 0x04000846 RID: 2118
		public bool xPeriodic;

		// Token: 0x04000847 RID: 2119
		public bool yPeriodic;

		// Token: 0x04000848 RID: 2120
		public bool destroyable = true;

		// Token: 0x04000849 RID: 2121
		public bool paused;

		// Token: 0x0400084A RID: 2122
		public float rotation;

		// Token: 0x0400084B RID: 2123
		public float alpha = 1f;

		// Token: 0x0400084C RID: 2124
		public float alphaFade;

		// Token: 0x0400084D RID: 2125
		public float layerDepth = -1f;

		// Token: 0x0400084E RID: 2126
		public float scale = 1f;

		// Token: 0x0400084F RID: 2127
		public float scaleChange;

		// Token: 0x04000850 RID: 2128
		public float scaleChangeChange;

		// Token: 0x04000851 RID: 2129
		public float rotationChange;

		// Token: 0x04000852 RID: 2130
		public float id;

		// Token: 0x04000853 RID: 2131
		public float lightRadius;

		// Token: 0x04000854 RID: 2132
		public float xPeriodicRange;

		// Token: 0x04000855 RID: 2133
		public float yPeriodicRange;

		// Token: 0x04000856 RID: 2134
		public float xPeriodicLoopTime;

		// Token: 0x04000857 RID: 2135
		public float yPeriodicLoopTime;

		// Token: 0x04000858 RID: 2136
		public float shakeIntensityChange;

		// Token: 0x04000859 RID: 2137
		public float shakeIntensity;

		// Token: 0x0400085A RID: 2138
		public float pulseTime;

		// Token: 0x0400085B RID: 2139
		public float pulseAmount = 1.1f;

		// Token: 0x0400085C RID: 2140
		public Vector2 position;

		// Token: 0x0400085D RID: 2141
		public Vector2 sourceRectStartingPos;

		// Token: 0x0400085E RID: 2142
		protected GameLocation parent;

		// Token: 0x0400085F RID: 2143
		private Texture2D texture;

		// Token: 0x04000860 RID: 2144
		public Rectangle sourceRect;

		// Token: 0x04000861 RID: 2145
		public Color color = Color.White;

		// Token: 0x04000862 RID: 2146
		public Color lightcolor = Color.White;

		// Token: 0x04000863 RID: 2147
		protected Farmer owner;

		// Token: 0x04000864 RID: 2148
		public Vector2 motion = Vector2.Zero;

		// Token: 0x04000865 RID: 2149
		public Vector2 acceleration = Vector2.Zero;

		// Token: 0x04000866 RID: 2150
		public Vector2 accelerationChange = Vector2.Zero;

		// Token: 0x04000867 RID: 2151
		public Vector2 initialPosition;

		// Token: 0x04000868 RID: 2152
		public int delayBeforeAnimationStart;

		// Token: 0x04000869 RID: 2153
		public string startSound;

		// Token: 0x0400086A RID: 2154
		public string endSound;

		// Token: 0x0400086B RID: 2155
		public string text;

		// Token: 0x0400086C RID: 2156
		public TemporaryAnimatedSprite.endBehavior endFunction;

		// Token: 0x0400086D RID: 2157
		public TemporaryAnimatedSprite.endBehavior reachedStopCoordinate;

		// Token: 0x0400086E RID: 2158
		public TemporaryAnimatedSprite parentSprite;

		// Token: 0x0400086F RID: 2159
		public Character attachedCharacter;

		// Token: 0x04000870 RID: 2160
		private float pulseTimer;

		// Token: 0x04000871 RID: 2161
		private float originalScale;

		// Token: 0x04000872 RID: 2162
		private float totalTimer;

		// Token: 0x02000172 RID: 370
		// Token: 0x06001387 RID: 4999
		public delegate void endBehavior(int extraInfo);
	}
}
