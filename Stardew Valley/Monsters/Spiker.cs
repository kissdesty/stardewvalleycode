using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000AE RID: 174
	public class Spiker : Monster
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public Spiker()
		{
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000E8BC8 File Offset: 0x000E6DC8
		public Spiker(Vector2 position, bool mover, Rectangle room, int startingDirection) : base("Spiker", position)
		{
			this.mover = mover;
			this.position = position;
			this.room = new Rectangle(room.X * Game1.tileSize - Game1.tileSize / 4, room.Y * Game1.tileSize - Game1.tileSize / 4, room.Width * Game1.tileSize + Game1.tileSize / 2, room.Height * Game1.tileSize + Game1.tileSize / 2);
			int mineArea = Game1.mine.getMineArea(-1);
			if (mineArea != 0)
			{
				if (mineArea != 40)
				{
					if (mineArea == 80)
					{
						this.offset = 1;
					}
				}
				else
				{
					this.offset = 2;
				}
			}
			else
			{
				this.offset = 0;
			}
			this.movementSpeed = Game1.random.Next(5, 9);
			if (mover)
			{
				this.offset = 4;
			}
			this.sprite.spriteHeight = Game1.tileSize;
			this.sprite.CurrentFrame = this.offset;
			this.movementDirection = startingDirection;
			this.pauseAtCollision = Game1.random.Next(500, 2000);
			if (mover)
			{
				Vector2 v = Utility.getVelocityTowardPoint(new Point((int)position.X, (int)position.Y), new Vector2(position.X + 32f, position.Y + 32f), (float)this.movementSpeed);
				this.xVelocity = v.X;
				this.yVelocity = v.Y;
				this.slipperiness = 9999;
				this.isGlider = true;
			}
			else
			{
				this.facingDirection = this.movementDirection;
				base.setMovingInFacingDirection();
			}
			base.IsWalkingTowardPlayer = false;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000E8D63 File Offset: 0x000E6F63
		public override void reloadSprite()
		{
			this.sprite.spriteHeight = Game1.tileSize;
			base.reloadSprite();
			this.sprite.CurrentFrame = this.offset;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x000E8D8C File Offset: 0x000E6F8C
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			Game1.playSound("parry");
			return 0;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000E8D99 File Offset: 0x000E6F99
		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X + 2, (int)this.position.Y + 2, Game1.tileSize - 4, Game1.tileSize - 4);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x000E8DCC File Offset: 0x000E6FCC
		public override void behaviorAtGameTick(GameTime time)
		{
			if (this.mover)
			{
				this.rotation = (this.rotation + (float)time.ElapsedGameTime.Milliseconds * 0.00613592332f) % 6.28318548f;
				if (!this.room.Contains((int)this.position.X, (int)this.position.Y) || ((int)this.previousPosition.X == (int)this.position.X && (int)this.previousPosition.Y == (int)this.position.Y))
				{
					Vector2 v = Utility.getVelocityTowardPoint(new Point((int)this.position.X, (int)this.position.Y), new Vector2((float)this.room.Center.X, (float)this.room.Center.Y), (float)this.movementSpeed);
					if ((int)v.X == 0)
					{
						v.X = 1f;
					}
					if ((int)v.Y == 0)
					{
						v.Y = 1f;
					}
					this.xVelocity = v.X;
					this.yVelocity = -v.Y;
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
				}
			}
			else
			{
				if (this.movementStartTimer > 0)
				{
					this.movementStartTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.movementStartTimer <= 0)
					{
						this.addedSpeed = Math.Min(this.movementSpeed, this.addedSpeed + 1);
						if (this.addedSpeed < this.movementSpeed)
						{
							this.movementStartTimer = 50;
						}
					}
				}
				if (this.pauseAtCollisionTimer > 0)
				{
					this.pauseAtCollisionTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.pauseAtCollisionTimer <= 0)
					{
						this.facingDirection = this.movementDirection;
						base.setMovingInFacingDirection();
						this.addedSpeed = 0;
						this.movementStartTimer = 50;
					}
				}
				else if (Game1.currentLocation.isCollidingPosition(this.nextPosition(this.movementDirection), Game1.viewport, false, 5, false, this) || !this.room.Contains(this.nextPosition(this.movementDirection)))
				{
					this.movementDirection = (this.movementDirection + 2) % 4;
					this.pauseAtCollisionTimer = this.pauseAtCollision;
					if (Utility.isOnScreen(this.position, 0))
					{
						Game1.playSound("parry");
					}
				}
			}
			this.previousPosition = new Vector2(this.position.X, this.position.Y);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool passThroughCharacters()
		{
			return true;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000E906C File Offset: 0x000E726C
		public override void updateMovement(GameLocation location, GameTime time)
		{
			base.updateMovement(location, time);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x000E9078 File Offset: 0x000E7278
		public override void draw(SpriteBatch b)
		{
			if (this.mover)
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(new Rectangle(64, 64, 64, 64)), Color.White, this.rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + Game1.tileSize) / 10000f)));
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(new Rectangle(88, 84, 16, 24)), Color.White, 0f, new Vector2(8f, 12f), 1f, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.992f : ((float)(base.getStandingY() + Game1.tileSize + 1) / 10000f)));
				return;
			}
			this.sprite.CurrentFrame = this.offset;
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(this.sprite.SourceRect), Color.White, this.rotation, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)(base.getStandingY() + Game1.tileSize / 2) / 10000f)));
		}

		// Token: 0x04000B75 RID: 2933
		public const int speedIncrement = 50;

		// Token: 0x04000B76 RID: 2934
		public int offset;

		// Token: 0x04000B77 RID: 2935
		public int movementDirection;

		// Token: 0x04000B78 RID: 2936
		public int pauseAtCollision;

		// Token: 0x04000B79 RID: 2937
		public int movementSpeed;

		// Token: 0x04000B7A RID: 2938
		private int pauseAtCollisionTimer;

		// Token: 0x04000B7B RID: 2939
		private int movementStartTimer;

		// Token: 0x04000B7C RID: 2940
		public bool mover;

		// Token: 0x04000B7D RID: 2941
		private new float rotation;

		// Token: 0x04000B7E RID: 2942
		public Rectangle room;

		// Token: 0x04000B7F RID: 2943
		private Vector2 previousPosition;
	}
}
