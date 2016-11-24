using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000A9 RID: 169
	public class MetalHead : Monster
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public MetalHead()
		{
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x000E65CA File Offset: 0x000E47CA
		public MetalHead(Vector2 tileLocation) : this(tileLocation, Game1.mine.getMineArea(-1))
		{
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000E65E0 File Offset: 0x000E47E0
		public MetalHead(Vector2 tileLocation, int mineArea) : base("Metal Head", tileLocation)
		{
			this.sprite.spriteHeight = 16;
			this.sprite.UpdateSourceRect();
			this.c = Color.White;
			base.IsWalkingTowardPlayer = true;
			int mineArea2 = Game1.mine.getMineArea(-1);
			if (mineArea2 == 0)
			{
				this.c = Color.White;
				return;
			}
			if (mineArea2 == 40)
			{
				this.c = Color.Turquoise;
				this.health *= 2;
				return;
			}
			if (mineArea2 != 80)
			{
				return;
			}
			this.c = Color.White;
			this.health *= 3;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000E667C File Offset: 0x000E487C
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision, "clank");
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000E6690 File Offset: 0x000E4890
		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.DarkGray, 10, false, 70f, 0, -1, -1f, -1, 0));
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), Color.DarkGray, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 300
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float)(Game1.tileSize / 2), 0f), Color.DarkGray, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 600
			});
			Game1.playSound("monsterdead");
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.MediumPurple, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				holdLastFrame = true,
				alphaFade = 0.01f,
				interval = 70f
			}, Game1.currentLocation, 4, 64, 64);
			base.deathAnimation();
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x000E67D8 File Offset: 0x000E49D8
		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 2 / 3) + this.yOffset), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3.5f + this.scale + this.yOffset / 30f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 3 / 4 + this.yJumpOffset)), new Rectangle?(base.Sprite.SourceRect), this.c, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000E696C File Offset: 0x000E4B6C
		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, this.sprite.getHeight() * 4, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, scale * (float)Game1.pixelZoom);
		}

		// Token: 0x04000B63 RID: 2915
		public Color c;
	}
}
