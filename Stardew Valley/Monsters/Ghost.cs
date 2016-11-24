using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	// Token: 0x020000BF RID: 191
	public class Ghost : Monster
	{
		// Token: 0x06000C1F RID: 3103 RVA: 0x000F006C File Offset: 0x000EE26C
		public Ghost()
		{
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000F008E File Offset: 0x000EE28E
		public Ghost(Vector2 position) : base("Ghost", position)
		{
			this.slipperiness = 8;
			this.isGlider = true;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x000F00C4 File Offset: 0x000EE2C4
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Ghost"));
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000F00E0 File Offset: 0x000EE2E0
		public override void drawAboveAllLayers(SpriteBatch b)
		{
			b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + this.yOffset)), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White, 0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
			b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + (float)this.yOffset / 20f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000F023C File Offset: 0x000EE43C
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			this.slipperiness = 8;
			Utility.addSprinklesToLocation(Game1.currentLocation, base.getTileX(), base.getTileY(), 2, 2, 101, 50, Color.LightBlue, null, false);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				this.health -= actualDamage;
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
				base.setTrajectory(xTrajectory, yTrajectory);
			}
			this.addedSpeed = -1;
			Utility.removeLightSource(this.identifier);
			return actualDamage;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x000F0364 File Offset: 0x000EE564
		public override void deathAnimation()
		{
			Game1.playSound("ghost");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 24), 100f, 4, 0, this.position, false, false, 0.9f, 0.001f, Color.White, (float)Game1.pixelZoom, 0.01f, 0f, 0.0490873866f, false));
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x000F03DC File Offset: 0x000EE5DC
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			this.yOffset = (int)(Math.Sin((double)((float)time.TotalGameTime.Milliseconds / 1000f) * 6.2831853071795862) * 20.0) - this.yOffsetExtra;
			bool wasFound = false;
			foreach (LightSource i in Game1.currentLightSources)
			{
				if (i.identifier == this.identifier)
				{
					i.position = new Vector2(this.position.X + (float)(Game1.tileSize / 2), this.position.Y + (float)Game1.tileSize + (float)this.yOffset);
					wasFound = true;
				}
			}
			if (!wasFound)
			{
				Game1.currentLightSources.Add(new LightSource(5, new Vector2(this.position.X + 8f, this.position.Y + (float)Game1.tileSize), 1f, Color.White * 0.7f, this.identifier));
			}
			float xSlope = (float)(-(float)(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X));
			float ySlope = (float)(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
			float t = 400f;
			xSlope /= t;
			ySlope /= t;
			if (this.wasHitCounter <= 0)
			{
				this.targetRotation = (float)Math.Atan2((double)(-(double)ySlope), (double)xSlope) - 1.57079637f;
				if ((double)(Math.Abs(this.targetRotation) - Math.Abs(this.rotation)) > 2.748893571891069 && Game1.random.NextDouble() < 0.5)
				{
					this.turningRight = true;
				}
				else if ((double)(Math.Abs(this.targetRotation) - Math.Abs(this.rotation)) < 0.39269908169872414)
				{
					this.turningRight = false;
				}
				if (this.turningRight)
				{
					this.rotation -= (float)Math.Sign(this.targetRotation - this.rotation) * 0.0490873866f;
				}
				else
				{
					this.rotation += (float)Math.Sign(this.targetRotation - this.rotation) * 0.0490873866f;
				}
				this.rotation %= 6.28318548f;
				this.wasHitCounter = 0;
			}
			float maxAccel = Math.Min(4f, Math.Max(1f, 5f - t / (float)Game1.tileSize / 2f));
			xSlope = (float)Math.Cos((double)this.rotation + 1.5707963267948966);
			ySlope = -(float)Math.Sin((double)this.rotation + 1.5707963267948966);
			this.xVelocity += -xSlope * maxAccel / 6f + (float)Game1.random.Next(-10, 10) / 100f;
			this.yVelocity += -ySlope * maxAccel / 6f + (float)Game1.random.Next(-10, 10) / 100f;
			if (Math.Abs(this.xVelocity) > Math.Abs(-xSlope * 5f))
			{
				this.xVelocity -= -xSlope * maxAccel / 6f;
			}
			if (Math.Abs(this.yVelocity) > Math.Abs(-ySlope * 5f))
			{
				this.yVelocity -= -ySlope * maxAccel / 6f;
			}
			base.faceGeneralDirection(Game1.player.getStandingPosition(), 0);
			if (this.GetBoundingBox().Intersects(Game1.player.GetBoundingBox()))
			{
				int attempts = 0;
				Vector2 attemptedPosition = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12)));
				while (attempts < 3 && (attemptedPosition.X >= (float)Game1.currentLocation.map.GetLayer("Back").LayerWidth || attemptedPosition.Y >= (float)Game1.currentLocation.map.GetLayer("Back").LayerHeight || attemptedPosition.X < 0f || attemptedPosition.Y < 0f || Game1.currentLocation.map.GetLayer("Back").Tiles[(int)attemptedPosition.X, (int)attemptedPosition.Y] == null || !Game1.currentLocation.isTilePassable(new Location((int)attemptedPosition.X, (int)attemptedPosition.Y), Game1.viewport) || attemptedPosition.Equals(new Vector2((float)(Game1.player.getStandingX() / Game1.tileSize), (float)(Game1.player.getStandingY() / Game1.tileSize)))))
				{
					attemptedPosition = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12)));
					attempts++;
				}
				if (attempts < 3)
				{
					this.position = new Vector2(attemptedPosition.X * (float)Game1.tileSize, attemptedPosition.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2));
					this.Halt();
				}
			}
		}

		// Token: 0x04000BCB RID: 3019
		public const float rotationIncrement = 0.0490873866f;

		// Token: 0x04000BCC RID: 3020
		private int wasHitCounter;

		// Token: 0x04000BCD RID: 3021
		private float targetRotation;

		// Token: 0x04000BCE RID: 3022
		private bool turningRight;

		// Token: 0x04000BCF RID: 3023
		private bool seenPlayer;

		// Token: 0x04000BD0 RID: 3024
		private int identifier = Game1.random.Next(-99999, 99999);

		// Token: 0x04000BD1 RID: 3025
		private new int yOffset;

		// Token: 0x04000BD2 RID: 3026
		private int yOffsetExtra;
	}
}
