using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000007 RID: 7
	public class AnimatedSprite
	{
		// Token: 0x17000001 RID: 1
		public Texture2D Texture
		{
			// Token: 0x0600003E RID: 62 RVA: 0x000035D8 File Offset: 0x000017D8
			get
			{
				return this.spriteTexture;
			}
			// Token: 0x0600003F RID: 63 RVA: 0x000035E0 File Offset: 0x000017E0
			set
			{
				this.spriteTexture = value;
			}
		}

		// Token: 0x17000002 RID: 2
		public virtual int CurrentFrame
		{
			// Token: 0x06000040 RID: 64 RVA: 0x000035E9 File Offset: 0x000017E9
			get
			{
				return this.currentFrame;
			}
			// Token: 0x06000041 RID: 65 RVA: 0x000035F1 File Offset: 0x000017F1
			set
			{
				this.currentFrame = value;
				this.UpdateSourceRect();
			}
		}

		// Token: 0x17000003 RID: 3
		public List<FarmerSprite.AnimationFrame> CurrentAnimation
		{
			// Token: 0x06000042 RID: 66 RVA: 0x00003600 File Offset: 0x00001800
			get
			{
				return this.currentAnimation;
			}
			// Token: 0x06000043 RID: 67 RVA: 0x00003608 File Offset: 0x00001808
			set
			{
				this.currentAnimation = value;
			}
		}

		// Token: 0x17000004 RID: 4
		public Rectangle SourceRect
		{
			// Token: 0x06000044 RID: 68 RVA: 0x00003611 File Offset: 0x00001811
			get
			{
				return this.sourceRect;
			}
			// Token: 0x06000045 RID: 69 RVA: 0x00003619 File Offset: 0x00001819
			set
			{
				this.sourceRect = value;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003624 File Offset: 0x00001824
		public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
		{
			this.spriteTexture = texture;
			this.textureWidth = ((texture == null) ? 96 : texture.Width);
			this.textureHeight = ((texture == null) ? 128 : texture.Height);
			this.currentFrame = currentFrame;
			this.spriteWidth = spriteWidth;
			this.spriteHeight = spriteHeight;
			if (this.spriteTexture != null)
			{
				this.UpdateSourceRect();
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000036B4 File Offset: 0x000018B4
		public AnimatedSprite(Texture2D texture)
		{
			this.spriteTexture = texture;
			this.UpdateSourceRect();
			this.textureWidth = ((texture == null) ? 96 : texture.Width);
			this.textureHeight = ((texture == null) ? 128 : texture.Height);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003726 File Offset: 0x00001926
		public int getHeight()
		{
			return this.spriteHeight;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000372E File Offset: 0x0000192E
		public int getWidth()
		{
			return this.spriteWidth;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003738 File Offset: 0x00001938
		public virtual void StopAnimation()
		{
			if (this.ignoreStopAnimation)
			{
				return;
			}
			if (this.currentAnimation != null)
			{
				this.currentAnimation = null;
				this.CurrentFrame = this.oldFrame;
				return;
			}
			if (this is FarmerSprite && this.CurrentFrame >= 232)
			{
				this.CurrentFrame -= 8;
			}
			if (this.CurrentFrame >= 64 && this.CurrentFrame <= 155)
			{
				this.CurrentFrame = (this.CurrentFrame - this.CurrentFrame % (this.textureWidth / this.spriteWidth)) % 32 + 96;
			}
			else
			{
				this.CurrentFrame = (this.currentFrame - this.currentFrame % (this.textureWidth / this.spriteWidth)) % 32;
			}
			this.UpdateSourceRect();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000037F8 File Offset: 0x000019F8
		public virtual void standAndFaceDirection(int direction)
		{
			switch (direction)
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
			this.UpdateSourceRect();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003846 File Offset: 0x00001A46
		public void faceDirectionStandard(int direction)
		{
			if (direction == 0)
			{
				direction = 2;
			}
			else if (direction == 2)
			{
				direction = 0;
			}
			this.CurrentFrame = direction * 4;
			this.UpdateSourceRect();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003868 File Offset: 0x00001A68
		public virtual void faceDirection(int direction)
		{
			if (this.ignoreStopAnimation)
			{
				return;
			}
			try
			{
				switch (direction)
				{
				case 0:
					this.CurrentFrame = this.textureWidth / this.spriteWidth * 2 + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
					break;
				case 1:
					this.CurrentFrame = this.textureWidth / this.spriteWidth + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
					break;
				case 2:
					this.CurrentFrame %= this.textureWidth / this.spriteWidth;
					break;
				case 3:
					if (this.textureUsesFlippedRightForLeft)
					{
						this.CurrentFrame = this.textureWidth / this.spriteWidth + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
					}
					else
					{
						this.CurrentFrame = this.textureWidth / this.spriteWidth * 3 + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
					}
					break;
				}
			}
			catch (Exception)
			{
			}
			this.UpdateSourceRect();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003988 File Offset: 0x00001B88
		public void AnimateRight(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
		{
			if (this.CurrentFrame >= this.framesPerAnimation * 2 || this.CurrentFrame < this.framesPerAnimation)
			{
				this.CurrentFrame = this.framesPerAnimation + this.CurrentFrame % this.framesPerAnimation;
			}
			this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval + (float)intervalOffset)
			{
				int num = this.CurrentFrame;
				this.CurrentFrame = num + 1;
				this.timer = 0f;
				if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
				{
					Game1.playSound(soundForFootstep);
				}
				if (this.CurrentFrame >= this.framesPerAnimation * 2 && this.loop)
				{
					this.CurrentFrame = this.framesPerAnimation;
				}
			}
			this.UpdateSourceRect();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003A6C File Offset: 0x00001C6C
		public void AnimateUp(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
		{
			if (this.CurrentFrame >= this.framesPerAnimation * 3 || this.CurrentFrame < this.framesPerAnimation * 2)
			{
				this.CurrentFrame = this.framesPerAnimation * 2 + this.CurrentFrame % this.framesPerAnimation;
			}
			this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval + (float)intervalOffset)
			{
				int num = this.CurrentFrame;
				this.CurrentFrame = num + 1;
				this.timer = 0f;
				if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
				{
					Game1.playSound(soundForFootstep);
				}
				if (this.CurrentFrame >= this.framesPerAnimation * 3 && this.loop)
				{
					this.CurrentFrame = this.framesPerAnimation * 2;
				}
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003B58 File Offset: 0x00001D58
		public void AnimateDown(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
		{
			if (this.CurrentFrame >= this.framesPerAnimation || this.CurrentFrame < 0)
			{
				this.CurrentFrame %= this.framesPerAnimation;
			}
			this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval + (float)intervalOffset)
			{
				int num = this.CurrentFrame;
				this.CurrentFrame = num + 1;
				this.timer = 0f;
				if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
				{
					Game1.playSound(soundForFootstep);
				}
				if (this.CurrentFrame >= this.framesPerAnimation && this.loop)
				{
					this.CurrentFrame = 0;
				}
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003C28 File Offset: 0x00001E28
		public void AnimateLeft(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
		{
			if (this.CurrentFrame >= this.framesPerAnimation * 4 || this.CurrentFrame < this.framesPerAnimation * 3)
			{
				this.CurrentFrame = this.framesPerAnimation * 3 + this.CurrentFrame % this.framesPerAnimation;
			}
			this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > this.interval + (float)intervalOffset)
			{
				int num = this.CurrentFrame;
				this.CurrentFrame = num + 1;
				this.timer = 0f;
				if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
				{
					Game1.playSound(soundForFootstep);
				}
				if (this.CurrentFrame >= this.framesPerAnimation * 4 && this.loop)
				{
					this.CurrentFrame = this.framesPerAnimation * 3;
				}
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003D14 File Offset: 0x00001F14
		public bool Animate(GameTime gameTime, int startFrame, int numberOfFrames, float interval)
		{
			if (this.CurrentFrame >= startFrame + numberOfFrames || this.CurrentFrame < startFrame)
			{
				this.CurrentFrame = startFrame + this.CurrentFrame % numberOfFrames;
			}
			this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > interval)
			{
				int num = this.CurrentFrame;
				this.CurrentFrame = num + 1;
				this.timer = 0f;
				if (this.CurrentFrame >= startFrame + numberOfFrames)
				{
					if (this.loop)
					{
						this.CurrentFrame = startFrame;
					}
					this.UpdateSourceRect();
					return true;
				}
			}
			this.UpdateSourceRect();
			return false;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003DB0 File Offset: 0x00001FB0
		public bool AnimateBackwards(GameTime gameTime, int startFrame, int numberOfFrames, float interval)
		{
			if (this.CurrentFrame >= startFrame + numberOfFrames || this.CurrentFrame < startFrame)
			{
				this.CurrentFrame = startFrame + this.CurrentFrame % numberOfFrames;
			}
			this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (this.timer > interval)
			{
				int num = this.CurrentFrame;
				this.CurrentFrame = num - 1;
				this.timer = 0f;
				if (this.CurrentFrame <= startFrame - numberOfFrames)
				{
					if (this.loop)
					{
						this.CurrentFrame = startFrame;
					}
					this.UpdateSourceRect();
					return true;
				}
			}
			this.UpdateSourceRect();
			return false;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003E4C File Offset: 0x0000204C
		public virtual void setCurrentAnimation(List<FarmerSprite.AnimationFrame> animation)
		{
			this.currentAnimation = animation;
			this.oldFrame = this.currentFrame;
			this.currentAnimationIndex = 0;
			if (this.currentAnimation.Count > 0)
			{
				this.timer = (float)this.currentAnimation[0].milliseconds;
				this.CurrentFrame = this.currentAnimation[0].frame;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003EB0 File Offset: 0x000020B0
		public bool animateOnce(GameTime time)
		{
			if (this.currentAnimation != null)
			{
				this.timer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.timer <= 0f)
				{
					this.currentAnimationIndex++;
					if (this.currentAnimationIndex >= this.currentAnimation.Count)
					{
						if (!this.loop)
						{
							this.currentFrame = this.oldFrame;
							this.currentAnimation = null;
							return true;
						}
						this.currentAnimationIndex = 0;
					}
					if (this.currentAnimation[this.currentAnimationIndex].frameBehavior != null)
					{
						this.currentAnimation[this.currentAnimationIndex].frameBehavior(null);
					}
					if (this.currentAnimation != null)
					{
						this.timer = (float)this.currentAnimation[this.currentAnimationIndex].milliseconds;
						this.CurrentFrame = this.currentAnimation[this.currentAnimationIndex].frame;
						this.UpdateSourceRect();
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003FBC File Offset: 0x000021BC
		public virtual void UpdateSourceRect()
		{
			if (!this.ignoreSourceRectUpdates)
			{
				this.SourceRect = new Rectangle(this.CurrentFrame * this.spriteWidth % this.spriteTexture.Width, this.CurrentFrame * this.spriteWidth / this.spriteTexture.Width * this.spriteHeight, this.spriteWidth, this.spriteHeight);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004024 File Offset: 0x00002224
		public void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth)
		{
			b.Draw(this.spriteTexture, screenPosition, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, (this.currentAnimation != null && this.currentAnimation[this.currentAnimationIndex].flip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004084 File Offset: 0x00002284
		public void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth, int xOffset, int yOffset, Color c, bool flip = false, float scale = 1f, float rotation = 0f, bool characterSourceRectOffset = false)
		{
			b.Draw(this.spriteTexture, screenPosition, new Rectangle?(new Rectangle(this.sourceRect.X + xOffset, this.sourceRect.Y + yOffset, this.sourceRect.Width, this.sourceRect.Height)), c, rotation, characterSourceRectOffset ? new Vector2((float)(this.spriteWidth / 2), (float)this.spriteHeight * 3f / 4f) : Vector2.Zero, scale, (flip || (this.currentAnimation != null && this.currentAnimation[this.currentAnimationIndex].flip)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004138 File Offset: 0x00002338
		public void drawShadow(SpriteBatch b, Vector2 screenPosition, float scale = 4f)
		{
			b.Draw(Game1.shadowTexture, screenPosition + new Vector2((float)(this.spriteWidth / 2 * Game1.pixelZoom) - scale, (float)(this.spriteHeight * Game1.pixelZoom) - scale), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, Utility.PointToVector2(Game1.shadowTexture.Bounds.Center), scale, SpriteEffects.None, 1E-05f);
		}

		// Token: 0x04000019 RID: 25
		protected Texture2D spriteTexture;

		// Token: 0x0400001A RID: 26
		public float timer;

		// Token: 0x0400001B RID: 27
		public float interval = 175f;

		// Token: 0x0400001C RID: 28
		public int framesPerAnimation = 4;

		// Token: 0x0400001D RID: 29
		public int currentFrame;

		// Token: 0x0400001E RID: 30
		public int spriteWidth = 16;

		// Token: 0x0400001F RID: 31
		public int spriteHeight = 24;

		// Token: 0x04000020 RID: 32
		public Rectangle sourceRect;

		// Token: 0x04000021 RID: 33
		public bool loop = true;

		// Token: 0x04000022 RID: 34
		public bool ignoreStopAnimation;

		// Token: 0x04000023 RID: 35
		public bool textureUsesFlippedRightForLeft;

		// Token: 0x04000024 RID: 36
		protected AnimatedSprite.endOfAnimationBehavior endOfAnimationFunction;

		// Token: 0x04000025 RID: 37
		protected int textureWidth;

		// Token: 0x04000026 RID: 38
		protected int textureHeight;

		// Token: 0x04000027 RID: 39
		public List<FarmerSprite.AnimationFrame> currentAnimation;

		// Token: 0x04000028 RID: 40
		public int oldFrame;

		// Token: 0x04000029 RID: 41
		public int currentAnimationIndex;

		// Token: 0x0400002A RID: 42
		public bool ignoreSourceRectUpdates;

		// Token: 0x02000163 RID: 355
		// Token: 0x06001354 RID: 4948
		public delegate void endOfAnimationBehavior(Farmer who);
	}
}
