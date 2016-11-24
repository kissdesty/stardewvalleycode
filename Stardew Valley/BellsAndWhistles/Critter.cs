using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200015C RID: 348
	public abstract class Critter
	{
		// Token: 0x06001338 RID: 4920 RVA: 0x0000282C File Offset: 0x00000A2C
		public Critter()
		{
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00182B92 File Offset: 0x00180D92
		public Critter(int baseFrame, Vector2 position)
		{
			this.baseFrame = baseFrame;
			this.position = position;
			this.sprite = new AnimatedSprite(Critter.critterTexture, baseFrame, 32, 32);
			this.startingPosition = position;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00182BC4 File Offset: 0x00180DC4
		public virtual Rectangle getBoundingBox(int xOffset, int yOffset)
		{
			return new Rectangle((int)this.position.X - Game1.tileSize / 2 + xOffset, (int)this.position.Y - Game1.tileSize / 4 + yOffset, Game1.tileSize, Game1.tileSize / 2);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00182C04 File Offset: 0x00180E04
		public virtual bool update(GameTime time, GameLocation environment)
		{
			this.sprite.animateOnce(time);
			if (this.gravityAffectedDY < 0f || this.yJumpOffset < 0f)
			{
				this.yJumpOffset += this.gravityAffectedDY;
				this.gravityAffectedDY += 0.25f;
			}
			return this.position.X < (float)(-(float)Game1.tileSize * 2) || this.position.Y < (float)(-(float)Game1.tileSize * 2) || this.position.X > (float)environment.map.DisplayWidth || this.position.Y > (float)environment.map.DisplayHeight;
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00182CC0 File Offset: 0x00180EC0
		public virtual void draw(SpriteBatch b)
		{
			if (this.sprite != null)
			{
				this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, -128f + this.yJumpOffset + this.yOffset)), this.position.Y / 10000f + this.position.X / 100000f, 0, 0, Color.White, this.flip, 4f, 0f, false);
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, -4f)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + Math.Max(-3f, (this.yJumpOffset + this.yOffset) / 64f), SpriteEffects.None, (this.position.Y - 1f) / 10000f);
			}
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void drawAboveFrontLayer(SpriteBatch b)
		{
		}

		// Token: 0x040013C2 RID: 5058
		public const int spriteWidth = 32;

		// Token: 0x040013C3 RID: 5059
		public const int spriteHeight = 32;

		// Token: 0x040013C4 RID: 5060
		public const float gravity = 0.25f;

		// Token: 0x040013C5 RID: 5061
		public static Texture2D critterTexture = Game1.content.Load<Texture2D>("TileSheets\\critters");

		// Token: 0x040013C6 RID: 5062
		public Vector2 position;

		// Token: 0x040013C7 RID: 5063
		public Vector2 startingPosition;

		// Token: 0x040013C8 RID: 5064
		public int baseFrame;

		// Token: 0x040013C9 RID: 5065
		public AnimatedSprite sprite;

		// Token: 0x040013CA RID: 5066
		public bool flip;

		// Token: 0x040013CB RID: 5067
		public float gravityAffectedDY;

		// Token: 0x040013CC RID: 5068
		public float yOffset;

		// Token: 0x040013CD RID: 5069
		public float yJumpOffset;
	}
}
