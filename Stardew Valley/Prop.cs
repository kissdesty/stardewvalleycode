using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000024 RID: 36
	public class Prop
	{
		// Token: 0x060001DA RID: 474 RVA: 0x0002B09C File Offset: 0x0002929C
		public Prop(Texture2D texture, int index, int tilesWideSolid, int tilesHighSolid, int tilesHighDraw, int tileX, int tileY, bool solid = true)
		{
			this.texture = texture;
			this.sourceRect = Game1.getSourceRectForStandardTileSheet(texture, index, 16, 16);
			this.sourceRect.Width = tilesWideSolid * 16;
			this.sourceRect.Height = tilesHighDraw * 16;
			this.drawRect = new Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize + (tilesHighSolid - tilesHighDraw) * Game1.tileSize, tilesWideSolid * Game1.tileSize, tilesHighDraw * Game1.tileSize);
			this.boundingRect = new Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize, tilesWideSolid * Game1.tileSize, tilesHighSolid * Game1.tileSize);
			this.solid = solid;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0002B14E File Offset: 0x0002934E
		public bool isColliding(Rectangle r)
		{
			return this.solid && r.Intersects(this.boundingRect);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0002B168 File Offset: 0x00029368
		public void draw(SpriteBatch b)
		{
			this.drawRect.X = this.boundingRect.X - Game1.viewport.X;
			this.drawRect.Y = this.boundingRect.Y + (this.boundingRect.Height - this.drawRect.Height) - Game1.viewport.Y;
			b.Draw(this.texture, this.drawRect, new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.solid ? ((float)this.boundingRect.Y / 10000f) : 0f);
		}

		// Token: 0x04000211 RID: 529
		private Texture2D texture;

		// Token: 0x04000212 RID: 530
		private Rectangle sourceRect;

		// Token: 0x04000213 RID: 531
		private Rectangle drawRect;

		// Token: 0x04000214 RID: 532
		private Rectangle boundingRect;

		// Token: 0x04000215 RID: 533
		private bool solid;
	}
}
