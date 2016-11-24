using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000A5 RID: 165
	public class Bug : Monster
	{
		// Token: 0x06000B59 RID: 2905 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public Bug()
		{
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x000E48C4 File Offset: 0x000E2AC4
		public Bug(Vector2 position, int facingDirection) : base("Bug", position, facingDirection)
		{
			this.sprite.spriteHeight = 16;
			this.sprite.UpdateSourceRect();
			this.onCollision = new Monster.collisionBehavior(this.collide);
			this.yOffset = (float)(-(float)Game1.tileSize / 2);
			base.IsWalkingTowardPlayer = false;
			base.setMovingInFacingDirection();
			this.defaultAnimationInterval = 40;
			this.collidesWithOtherCharacters = false;
			if (Game1.mine.getMineArea(-1) == 121)
			{
				this.isArmoredBug = true;
				this.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\Monsters\\Armored Bug");
				this.damageToFarmer *= 2;
				this.slipperiness = -1;
				this.health = 150;
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool passThroughCharacters()
		{
			return true;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x000E4983 File Offset: 0x000E2B83
		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteHeight = 16;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000E49A4 File Offset: 0x000E2BA4
		private void collide(GameLocation location)
		{
			Rectangle bb = this.nextPosition(this.facingDirection);
			using (List<Farmer>.Enumerator enumerator = location.getFarmers().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetBoundingBox().Intersects(bb))
					{
						return;
					}
				}
			}
			this.facingDirection = (this.facingDirection + 2) % 4;
			base.setMovingInFacingDirection();
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000E4A24 File Offset: 0x000E2C24
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (this.isArmoredBug)
			{
				Game1.playSound("crafting");
				return 0;
			}
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				Game1.playSound("hitEnemy");
				base.setTrajectory(xTrajectory / 3, yTrajectory / 3);
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x000E4AA8 File Offset: 0x000E2CA8
		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				Vector2 offset = default(Vector2);
				if (this.facingDirection % 2 == 0)
				{
					offset.X = (float)(Math.Sin((double)((float)Game1.currentGameTime.TotalGameTime.Milliseconds / 1000f) * 6.2831853071795862) * 10.0);
				}
				else
				{
					offset.Y = (float)(Math.Sin((double)((float)Game1.currentGameTime.TotalGameTime.Milliseconds / 1000f) * 6.2831853071795862) * 10.0);
				}
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom) / 2f + offset.X, (float)(this.GetBoundingBox().Height * 5 / 2 - Game1.tileSize * 3 / 4)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)this.yJumpOffset / 40f) * this.scale, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f) - 1E-06f);
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)this.yJumpOffset) + offset, new Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000E4CE0 File Offset: 0x000E2EE0
		public override void deathAnimation()
		{
			base.deathAnimation();
			Game1.playSound("slimedead");
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), Color.Violet, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				holdLastFrame = true,
				alphaFade = 0.01f,
				interval = 70f
			}, Game1.currentLocation, 4, 64, 64);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000E4D64 File Offset: 0x000E2F64
		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, this.sprite.getHeight() * 4, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, (float)Game1.pixelZoom);
		}

		// Token: 0x04000B5B RID: 2907
		public bool isArmoredBug;
	}
}
