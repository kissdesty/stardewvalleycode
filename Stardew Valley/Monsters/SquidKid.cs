using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;

namespace StardewValley.Monsters
{
	// Token: 0x020000BB RID: 187
	public class SquidKid : Monster
	{
		// Token: 0x06000C04 RID: 3076 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public SquidKid()
		{
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000EE91C File Offset: 0x000ECB1C
		public SquidKid(Vector2 position) : base("Squid Kid", position)
		{
			this.sprite.spriteHeight = 16;
			base.IsWalkingTowardPlayer = false;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000EE949 File Offset: 0x000ECB49
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Squid Kid"));
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000EE968 File Offset: 0x000ECB68
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				Game1.playSound("hitEnemy");
				this.sprite.CurrentFrame = this.sprite.CurrentFrame - this.sprite.CurrentFrame % 4 + 3;
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000EE9FC File Offset: 0x000ECBFC
		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 64, 16, 16), 70f, 7, 0, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), false, false)
			{
				scale = (float)Game1.pixelZoom
			});
			Game1.playSound("fireball");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 100
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 200
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 300
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
			{
				delayBeforeAnimationStart = 400
			});
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x000EEC88 File Offset: 0x000ECE88
		public override void drawAboveAllLayers(SpriteBatch b)
		{
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + this.yOffset)), new Rectangle?(base.Sprite.SourceRect), Color.White, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + (float)this.yOffset / 20f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000EEDE4 File Offset: 0x000ECFE4
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			base.faceGeneralDirection(Game1.player.position, 0);
			this.yOffset = (int)(Math.Sin((double)((float)time.TotalGameTime.Milliseconds / 2000f) * 6.2831853071795862) * 15.0);
			if (this.sprite.CurrentFrame % 4 != 0 && Game1.random.NextDouble() < 0.1)
			{
				this.sprite.CurrentFrame -= this.sprite.CurrentFrame % 4;
			}
			if (Game1.random.NextDouble() < 0.01)
			{
				AnimatedSprite expr_AC = this.sprite;
				int currentFrame = expr_AC.CurrentFrame;
				expr_AC.CurrentFrame = currentFrame + 1;
			}
			this.lastFireball = Math.Max(0f, this.lastFireball - (float)time.ElapsedGameTime.Milliseconds);
			if (this.withinPlayerThreshold() && this.lastFireball == 0f && Game1.random.NextDouble() < 0.01)
			{
				base.IsWalkingTowardPlayer = false;
				Vector2 fireballLocation = new Vector2(this.position.X, this.position.Y + (float)Game1.tileSize);
				this.Halt();
				switch (this.facingDirection)
				{
				case 0:
					this.sprite.CurrentFrame = 3;
					break;
				case 1:
					this.sprite.CurrentFrame = 7;
					fireballLocation.X += (float)Game1.tileSize;
					break;
				case 2:
					this.sprite.CurrentFrame = 11;
					fireballLocation.Y += (float)(Game1.tileSize / 2);
					break;
				case 3:
					this.sprite.CurrentFrame = 15;
					fireballLocation.X -= (float)(Game1.tileSize / 2);
					break;
				}
				this.sprite.UpdateSourceRect();
				Vector2 trajectory = Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(base.getStandingPosition()), 8f, Game1.player);
				Game1.currentLocation.projectiles.Add(new BasicProjectile(15, 10, 3, 4, 0f, trajectory.X, trajectory.Y, base.getStandingPosition(), "", "", true, false, this, false, null));
				Game1.playSound("fireball");
				this.lastFireball = (float)Game1.random.Next(1200, 3500);
				return;
			}
			if (this.lastFireball != 0f && Game1.random.NextDouble() < 0.02)
			{
				this.Halt();
				if (this.withinPlayerThreshold())
				{
					this.slipperiness = 8;
					base.setTrajectory((int)Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(base.getStandingPosition()), 8f, Game1.player).X, (int)(-(int)Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(base.getStandingPosition()), 8f, Game1.player).Y));
				}
			}
		}

		// Token: 0x04000BC2 RID: 3010
		private float lastFireball;

		// Token: 0x04000BC3 RID: 3011
		private new int yOffset;
	}
}
