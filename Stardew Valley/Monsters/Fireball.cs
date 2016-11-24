using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000BA RID: 186
	public class Fireball : Monster
	{
		// Token: 0x06000BFD RID: 3069 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public Fireball()
		{
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000EE4F4 File Offset: 0x000EC6F4
		public Fireball(Vector2 position) : base("Fireball", position)
		{
			this.scale = (float)Game1.pixelZoom + (float)Game1.random.Next(-20, 20) / 100f;
			base.IsWalkingTowardPlayer = false;
			this.homing = true;
			this.id = Game1.random.NextDouble();
			this.sprite.spriteWidth = 8;
			this.sprite.spriteHeight = 8;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000EE570 File Offset: 0x000EC770
		public Fireball(Vector2 position, int xVelocity, int yVelocity) : base("Fireball", position)
		{
			this.scale = (float)Game1.pixelZoom + (float)Game1.random.Next(-20, 20) / 100f;
			base.IsWalkingTowardPlayer = false;
			this.dx = (float)xVelocity;
			this.dy = (float)yVelocity;
			this.id = Game1.random.NextDouble();
			this.sprite.spriteWidth = 8;
			this.sprite.spriteHeight = 8;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000EE5F5 File Offset: 0x000EC7F5
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Fireball"), 0, 8, 8);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000EE614 File Offset: 0x000EC814
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			if (isBomb)
			{
				return 0;
			}
			this.health -= damage;
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(362, 100f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5));
			return 1;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x000EE692 File Offset: 0x000EC892
		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, this.sprite.getWidth(), this.sprite.getHeight());
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000EE6C8 File Offset: 0x000EC8C8
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			if (this.homing)
			{
				if (this.position.X < Game1.player.position.X - 12f)
				{
					this.dx = Math.Min(this.dx + 0.8f, 8f);
				}
				else if (this.position.X > Game1.player.position.X + 12f)
				{
					this.dx = Math.Max(this.dx - 0.8f, -8f);
				}
				if (this.position.Y + (float)(Game1.tileSize / 4) < (float)(Game1.player.getStandingY() - 6))
				{
					this.dy = Math.Max(this.dy - 0.8f, -8f);
				}
				else if (this.position.Y + (float)(Game1.tileSize / 4) > (float)(Game1.player.getStandingY() + 6))
				{
					this.dy = Math.Min(this.dy + 0.8f, 8f);
				}
			}
			this.position.X = this.position.X + this.dx;
			this.position.Y = this.position.Y - this.dy;
			if (Game1.currentLocation.isCollidingPosition(this.GetBoundingBox(), Game1.viewport, false, 8, true, this) || this.GetBoundingBox().Intersects(Game1.player.GetBoundingBox()))
			{
				for (int i = Game1.currentLocation.characters.Count - 1; i >= 0; i--)
				{
					if (Game1.currentLocation.characters[i].name.Equals("Fireball") && ((Fireball)Game1.currentLocation.characters[i]).id == this.id)
					{
						Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(362, 100f, 6, 1, this.position + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5));
						Game1.currentLocation.characters.RemoveAt(i);
						return;
					}
				}
			}
		}

		// Token: 0x04000BBE RID: 3006
		private bool homing;

		// Token: 0x04000BBF RID: 3007
		private new double id;

		// Token: 0x04000BC0 RID: 3008
		private float dx;

		// Token: 0x04000BC1 RID: 3009
		private float dy;
	}
}
