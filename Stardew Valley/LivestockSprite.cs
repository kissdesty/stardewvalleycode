using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000031 RID: 49
	public class LivestockSprite : AnimatedSprite
	{
		// Token: 0x06000297 RID: 663 RVA: 0x00035FEB File Offset: 0x000341EB
		public LivestockSprite(Texture2D texture, int currentFrame) : base(texture, 0, Game1.tileSize, Game1.tileSize * 3 / 2)
		{
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00036004 File Offset: 0x00034204
		public override void faceDirection(int direction)
		{
			switch (direction)
			{
			case 0:
				this.CurrentFrame = this.spriteTexture.Width / Game1.tileSize * 2 + this.CurrentFrame % (this.spriteTexture.Width / Game1.tileSize);
				break;
			case 1:
				this.CurrentFrame = this.spriteTexture.Width / (Game1.tileSize * 2) + this.CurrentFrame % (this.spriteTexture.Width / (Game1.tileSize * 2));
				break;
			case 2:
				this.CurrentFrame %= this.spriteTexture.Width / Game1.tileSize;
				break;
			case 3:
				this.CurrentFrame = this.spriteTexture.Width / (Game1.tileSize * 2) * 3 + this.CurrentFrame % (this.spriteTexture.Width / (Game1.tileSize * 2));
				break;
			}
			this.UpdateSourceRect();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000360F8 File Offset: 0x000342F8
		public override void UpdateSourceRect()
		{
			switch (this.currentFrame)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				base.SourceRect = new Rectangle(this.currentFrame * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
				return;
			case 4:
			case 5:
			case 6:
			case 7:
				base.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize * 2, Game1.tileSize * 3 / 2, Game1.tileSize * 2, Game1.tileSize * 3 / 2);
				return;
			case 8:
			case 9:
			case 10:
			case 11:
				base.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize, Game1.tileSize * 3 / 2);
				return;
			case 12:
			case 13:
			case 14:
			case 15:
				base.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize * 2, Game1.tileSize * 9 / 2, Game1.tileSize * 2, Game1.tileSize * 3 / 2);
				return;
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
				break;
			case 24:
			case 25:
			case 26:
			case 27:
				base.SourceRect = new Rectangle((this.currentFrame - 20) * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize, Game1.tileSize * 3 / 2);
				break;
			default:
				return;
			}
		}
	}
}
