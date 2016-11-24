using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000A6 RID: 166
	public class BigSlime : Monster
	{
		// Token: 0x06000B62 RID: 2914 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public BigSlime()
		{
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x000E4DDC File Offset: 0x000E2FDC
		public BigSlime(Vector2 position) : this(position, Game1.mine.getMineArea(-1))
		{
			this.sprite.ignoreStopAnimation = true;
			this.ignoreMovementAnimations = true;
			this.hideShadow = true;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000E4E0C File Offset: 0x000E300C
		public BigSlime(Vector2 position, int mineArea) : base("Big Slime", position)
		{
			this.ignoreMovementAnimations = true;
			this.sprite.ignoreStopAnimation = true;
			this.sprite.spriteWidth = 32;
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
			this.sprite.framesPerAnimation = 8;
			this.c = Color.White;
			int mineArea2 = Game1.mine.getMineArea(-1);
			if (mineArea2 <= 40)
			{
				if (mineArea2 != 0)
				{
					if (mineArea2 == 40)
					{
						this.c = Color.Turquoise;
						this.health *= 2;
						this.experienceGained *= 2;
					}
				}
				else
				{
					this.c = Color.Green;
				}
			}
			else if (mineArea2 != 80)
			{
				if (mineArea2 == 121)
				{
					this.c = Color.BlueViolet;
					this.health *= 4;
					this.damageToFarmer *= 3;
					this.experienceGained *= 3;
				}
			}
			else
			{
				this.c = Color.Red;
				this.health *= 3;
				this.damageToFarmer *= 2;
				this.experienceGained *= 3;
			}
			int r = (int)this.c.R;
			int g = (int)this.c.G;
			int b = (int)this.c.B;
			r += Game1.random.Next(-20, 21);
			g += Game1.random.Next(-20, 21);
			b += Game1.random.Next(-20, 21);
			this.c.R = (byte)Math.Max(Math.Min(255, r), 0);
			this.c.G = (byte)Math.Max(Math.Min(255, g), 0);
			this.c.B = (byte)Math.Max(Math.Min(255, b), 0);
			this.c *= (float)Game1.random.Next(7, 11) / 10f;
			this.sprite.interval = 300f;
			this.hideShadow = true;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000E5030 File Offset: 0x000E3230
		public override void reloadSprite()
		{
			base.reloadSprite();
			this.sprite.spriteWidth = 32;
			this.sprite.spriteHeight = 32;
			this.sprite.interval = 300f;
			this.sprite.ignoreStopAnimation = true;
			this.ignoreMovementAnimations = true;
			this.hideShadow = true;
			this.sprite.UpdateSourceRect();
			this.sprite.framesPerAnimation = 8;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000E50A0 File Offset: 0x000E32A0
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.slipperiness = 3;
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("hitEnemy");
				base.IsWalkingTowardPlayer = true;
				if (this.health <= 0)
				{
					this.deathAnimation();
					Stats expr_77 = Game1.stats;
					uint slimesKilled = expr_77.SlimesKilled;
					expr_77.SlimesKilled = slimesKilled + 1u;
					if (Game1.gameMode == 3 && Game1.random.NextDouble() < 0.75)
					{
						int toCreate = Game1.random.Next(2, 5);
						for (int i = 0; i < toCreate; i++)
						{
							Game1.currentLocation.characters.Add(new GreenSlime(this.position, Game1.mine.mineLevel));
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].setTrajectory(xTrajectory / 8 + Game1.random.Next(-2, 3), yTrajectory / 8 + Game1.random.Next(-2, 3));
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].willDestroyObjectsUnderfoot = false;
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].moveTowardPlayer(4);
							Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].scale = 0.75f + (float)Game1.random.Next(-5, 10) / 100f;
						}
					}
				}
			}
			return actualDamage;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000E5270 File Offset: 0x000E3470
		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, this.c, 10, false, 70f, 0, -1, -1f, -1, 0));
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), this.c, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 100
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)(Game1.tileSize / 2), 0f), this.c, 10, false, 70f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 200
			});
			Game1.playSound("slimedead");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), this.c, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 300
			});
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000E53B8 File Offset: 0x000E35B8
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			int currentIndex = this.sprite.CurrentFrame;
			this.sprite.AnimateDown(time, 0, "");
			if (this.isMoving())
			{
				this.sprite.interval = 100f;
			}
			else
			{
				this.sprite.interval = 200f;
			}
			if (Utility.isOnScreen(this.position, Game1.tileSize * 2) && this.sprite.CurrentFrame == 0 && currentIndex == 7)
			{
				Game1.playSound("slimeHit");
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000E5444 File Offset: 0x000E3644
		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize * 3 / 4 + Game1.pixelZoom * 2), (float)(Game1.tileSize / 4 + this.yJumpOffset)), new Rectangle?(base.Sprite.SourceRect), this.c, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize * 3 / 4 + Game1.pixelZoom * 2), (float)(Game1.tileSize / 4 + this.yJumpOffset)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0f, new Vector2(16f, 16f), (float)Game1.pixelZoom * Math.Max(0.2f, this.scale), this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x000E5604 File Offset: 0x000E3804
		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00002834 File Offset: 0x00000A34
		public override void shedChunks(int number, float scale)
		{
		}

		// Token: 0x04000B5C RID: 2908
		public Color c;
	}
}
