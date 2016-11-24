using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200000A RID: 10
	public class BatTemporarySprite : TemporaryAnimatedSprite
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00005494 File Offset: 0x00003694
		public BatTemporarySprite(Vector2 position) : base(-666, 100f, 4, 99999, position, false, false)
		{
			this.texture = Game1.content.Load<Texture2D>("LooseSprites\\Bat");
			this.currentParentTileIndex = 0;
			if (position.X > (float)(Game1.currentLocation.Map.DisplayWidth / 2))
			{
				this.moveLeft = true;
			}
			this.horizontalSpeed = Game1.random.Next(1, 8);
			this.verticalSpeed = (float)Game1.random.Next(3, 7);
			this.interval = 160f - ((float)this.horizontalSpeed + this.verticalSpeed) * 10f;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000553C File Offset: 0x0000373C
		public override void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, base.Position), new Rectangle?(new Rectangle(this.currentParentTileIndex * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (base.Position.Y + (float)(Game1.tileSize / 2)) / 10000f);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000055B8 File Offset: 0x000037B8
		public override bool update(GameTime time)
		{
			this.timer += (float)time.ElapsedGameTime.Milliseconds;
			if (this.timer > this.interval)
			{
				this.currentParentTileIndex++;
				this.timer = 0f;
				if (this.currentParentTileIndex >= this.animationLength)
				{
					this.currentNumberOfLoops++;
					this.currentParentTileIndex = 0;
				}
			}
			if (this.moveLeft)
			{
				this.position.X = this.position.X - (float)this.horizontalSpeed;
			}
			else
			{
				this.position.X = this.position.X + (float)this.horizontalSpeed;
			}
			this.position.Y = this.position.Y + this.verticalSpeed;
			this.verticalSpeed -= 0.1f;
			return this.position.Y >= (float)Game1.currentLocation.Map.DisplayHeight || this.position.Y < 0f || this.position.X < 0f || this.position.X >= (float)Game1.currentLocation.Map.DisplayWidth;
		}

		// Token: 0x04000049 RID: 73
		public Texture2D texture;

		// Token: 0x0400004A RID: 74
		private bool moveLeft;

		// Token: 0x0400004B RID: 75
		private int horizontalSpeed;

		// Token: 0x0400004C RID: 76
		private float verticalSpeed;
	}
}
