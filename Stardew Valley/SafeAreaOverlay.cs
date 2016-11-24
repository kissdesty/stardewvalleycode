using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200004C RID: 76
	public class SafeAreaOverlay : DrawableGameComponent
	{
		// Token: 0x060006E4 RID: 1764 RVA: 0x000A3AF1 File Offset: 0x000A1CF1
		public SafeAreaOverlay(Game game) : base(game)
		{
			base.DrawOrder = 1000;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000A3B08 File Offset: 0x000A1D08
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(Game1.graphics.GraphicsDevice);
			this.dummyTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
			this.dummyTexture.SetData<Color>(new Color[]
			{
				Color.White
			});
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000A3B60 File Offset: 0x000A1D60
		public override void Draw(GameTime gameTime)
		{
			Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
			Rectangle safeArea = viewport.TitleSafeArea;
			int viewportRight = viewport.X + viewport.Width;
			int viewportBottom = viewport.Y + viewport.Height;
			Rectangle leftBorder = new Rectangle(viewport.X, viewport.Y, safeArea.X - viewport.X, viewport.Height);
			Rectangle rightBorder = new Rectangle(safeArea.Right, viewport.Y, viewportRight - safeArea.Right, viewport.Height);
			Rectangle topBorder = new Rectangle(safeArea.Left, viewport.Y, safeArea.Width, safeArea.Top - viewport.Y);
			Rectangle bottomBorder = new Rectangle(safeArea.Left, safeArea.Bottom, safeArea.Width, viewportBottom - safeArea.Bottom);
			Color translucentRed = Color.Red;
			this.spriteBatch.Begin();
			this.spriteBatch.Draw(this.dummyTexture, leftBorder, translucentRed);
			this.spriteBatch.Draw(this.dummyTexture, rightBorder, translucentRed);
			this.spriteBatch.Draw(this.dummyTexture, topBorder, translucentRed);
			this.spriteBatch.Draw(this.dummyTexture, bottomBorder, translucentRed);
			this.spriteBatch.End();
		}

		// Token: 0x040007BB RID: 1979
		private SpriteBatch spriteBatch;

		// Token: 0x040007BC RID: 1980
		private Texture2D dummyTexture;
	}
}
