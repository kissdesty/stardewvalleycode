using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000038 RID: 56
	public class SeaMonsterTemporarySprite : TemporaryAnimatedSprite
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0003A9F5 File Offset: 0x00038BF5
		public SeaMonsterTemporarySprite(float animationInterval, int animationLength, int numberOfLoops, Vector2 position) : base(-666, animationInterval, animationLength, numberOfLoops, position, false, false)
		{
			this.texture = Game1.content.Load<Texture2D>("LooseSprites\\SeaMonster");
			Game1.playSound("pullItemFromWater");
			this.currentParentTileIndex = 0;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0003AA30 File Offset: 0x00038C30
		public override void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, base.Position), new Rectangle?(new Rectangle(this.currentParentTileIndex * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (base.Position.Y + (float)(Game1.tileSize / 2)) / 10000f);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0003AAAC File Offset: 0x00038CAC
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
					this.currentParentTileIndex = 2;
				}
			}
			if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
			{
				this.position.Y = this.position.Y + 2f;
				if (this.position.Y >= (float)Game1.currentLocation.Map.DisplayHeight)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000345 RID: 837
		public Texture2D texture;
	}
}
