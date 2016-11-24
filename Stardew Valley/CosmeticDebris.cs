using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000053 RID: 83
	public class CosmeticDebris : TemporaryAnimatedSprite
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x000A7EF4 File Offset: 0x000A60F4
		public CosmeticDebris(Texture2D texture, Vector2 startingPosition, float rotationSpeed, float xVelocity, float yVelocity, int groundYLevel, Rectangle sourceRect, Color color, Cue tapSound, LightSource light, int lightTailLength, int disappearTime)
		{
			this.timeToDisappearAfterReachingGround = disappearTime;
			this.disappearTimer = this.timeToDisappearAfterReachingGround;
			this.texture = texture;
			this.position = startingPosition;
			this.rotationSpeed = rotationSpeed;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.sourceRect = sourceRect;
			this.groundYLevel = groundYLevel;
			this.color = color;
			this.tapSound = tapSound;
			this.light = light;
			if (lightTailLength > 0)
			{
				this.lightTail = new Queue<Vector2>();
				this.lightTailLength = lightTailLength;
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000A7F80 File Offset: 0x000A6180
		public override bool update(GameTime time)
		{
			this.yVelocity += 0.3f;
			this.position += new Vector2(this.xVelocity, this.yVelocity);
			this.rotation += this.rotationSpeed;
			if (this.position.Y >= (float)this.groundYLevel)
			{
				this.position.Y = (float)(this.groundYLevel - 1);
				this.yVelocity = -this.yVelocity;
				this.yVelocity *= 0.45f;
				this.xVelocity *= 0.45f;
				this.rotationSpeed *= 0.225f;
				if (Game1.soundBank != null && !this.tapSound.IsPlaying)
				{
					this.tapSound = Game1.soundBank.GetCue(this.tapSound.Name);
					this.tapSound.Play();
				}
				this.disappearTimer--;
			}
			if (this.disappearTimer < this.timeToDisappearAfterReachingGround)
			{
				this.disappearTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.disappearTimer <= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000A80C0 File Offset: 0x000A62C0
		public override void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, this.position), new Rectangle?(this.sourceRect), this.color, this.rotation, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.groundYLevel + 1) / 10000f);
		}

		// Token: 0x04000873 RID: 2163
		public const float gravity = 0.3f;

		// Token: 0x04000874 RID: 2164
		public const float bounciness = 0.45f;

		// Token: 0x04000875 RID: 2165
		private new Vector2 position;

		// Token: 0x04000876 RID: 2166
		private new float rotation;

		// Token: 0x04000877 RID: 2167
		private float rotationSpeed;

		// Token: 0x04000878 RID: 2168
		private float xVelocity;

		// Token: 0x04000879 RID: 2169
		private float yVelocity;

		// Token: 0x0400087A RID: 2170
		private new Rectangle sourceRect;

		// Token: 0x0400087B RID: 2171
		private int groundYLevel;

		// Token: 0x0400087C RID: 2172
		private int disappearTimer;

		// Token: 0x0400087D RID: 2173
		private int lightTailLength;

		// Token: 0x0400087E RID: 2174
		private int timeToDisappearAfterReachingGround;

		// Token: 0x0400087F RID: 2175
		private new Color color;

		// Token: 0x04000880 RID: 2176
		private Cue tapSound;

		// Token: 0x04000881 RID: 2177
		private new LightSource light;

		// Token: 0x04000882 RID: 2178
		private Queue<Vector2> lightTail;

		// Token: 0x04000883 RID: 2179
		private Texture2D texture;
	}
}
