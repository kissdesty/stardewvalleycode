using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
	// Token: 0x020000CC RID: 204
	public class HaleyCowPictures : IMinigame
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x001081D4 File Offset: 0x001063D4
		public HaleyCowPictures()
		{
			this.content = Game1.content.CreateTemporary();
			this.pictures = (Game1.currentSeason.Equals("winter") ? this.content.Load<Texture2D>("LooseSprites\\cowPhotosWinter") : this.content.Load<Texture2D>("LooseSprites\\cowPhotos"));
			this.centerOfScreen = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2));
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0010827C File Offset: 0x0010647C
		public bool tick(GameTime time)
		{
			this.betweenPhotoTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.betweenPhotoTimer <= 0)
			{
				this.betweenPhotoTimer = 5000;
				this.numberOfPhotosSoFar++;
				if (this.numberOfPhotosSoFar < 5)
				{
					Game1.playSound("cameraNoise");
				}
				if (this.numberOfPhotosSoFar >= 6)
				{
					Event expr_63 = Game1.currentLocation.currentEvent;
					int currentCommand = expr_63.CurrentCommand;
					expr_63.CurrentCommand = currentCommand + 1;
					return true;
				}
			}
			if (this.numberOfPhotosSoFar >= 5)
			{
				this.fadeAlpha = Math.Min(1f, this.fadeAlpha += 0.007f);
			}
			return false;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyPress(Keys k)
		{
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0010832C File Offset: 0x0010652C
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
			if (this.numberOfPhotosSoFar > 0)
			{
				b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f), new Rectangle?(new Rectangle(0, 0, 104, 124)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				Game1.player.faceDirection(2);
				Game1.player.FarmerRenderer.draw(b, Game1.player, 0, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float)Game1.pixelZoom, 0.01f, false);
				b.Draw(Game1.shadowTexture, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float)Game1.pixelZoom + new Vector2(32f, (float)(24 + Game1.tileSize * 3 / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 0.005f);
			}
			if (this.numberOfPhotosSoFar > 1)
			{
				Game1.player.faceDirection(3);
				b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2((float)(Game1.pixelZoom * 4), (float)(Game1.pixelZoom * 4)), new Rectangle?(new Rectangle(104, 0, 104, 124)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
				Game1.player.FarmerRenderer.draw(b, Game1.player, 6, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2((float)(Game1.pixelZoom * 4), (float)(Game1.pixelZoom * 4)) + new Vector2(64f, 66f) * (float)Game1.pixelZoom, 0.11f, true);
				b.Draw(Game1.shadowTexture, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2((float)(Game1.pixelZoom * 4), (float)(Game1.pixelZoom * 4)) + new Vector2(64f, 66f) * (float)Game1.pixelZoom + new Vector2(32f, (float)(24 + Game1.tileSize * 3 / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 0.105f);
			}
			if (this.numberOfPhotosSoFar > 2)
			{
				Game1.player.faceDirection(3);
				b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f) - new Vector2((float)(Game1.pixelZoom * 6), (float)(Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(0, 124, 104, 124)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.2f);
				Game1.player.FarmerRenderer.draw(b, Game1.player, 89, this.centerOfScreen + new Vector2(-208f, -248f) - new Vector2((float)(Game1.pixelZoom * 6), (float)(Game1.pixelZoom * 2)) + new Vector2(55f, 66f) * (float)Game1.pixelZoom, 0.21f, true);
				b.Draw(Game1.shadowTexture, this.centerOfScreen + new Vector2(-208f, -248f) - new Vector2((float)(Game1.pixelZoom * 6), (float)(Game1.pixelZoom * 2)) + new Vector2(55f, 66f) * (float)Game1.pixelZoom + new Vector2(32f, (float)(24 + Game1.tileSize * 3 / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 0.205f);
			}
			if (this.numberOfPhotosSoFar > 3)
			{
				Game1.player.faceDirection(2);
				b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f), new Rectangle?(new Rectangle(104, 124, 104, 124)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.3f);
				Game1.player.FarmerRenderer.draw(b, Game1.player, 94, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float)Game1.pixelZoom, 0.31f, false);
				b.Draw(Game1.shadowTexture, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float)Game1.pixelZoom + new Vector2(32f, (float)(24 + Game1.tileSize * 3 / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, 0.305f);
			}
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * this.fadeAlpha, 0f, Vector2.Zero, SpriteEffects.None, 1f);
			b.End();
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00108A3C File Offset: 0x00106C3C
		public void changeScreenSize()
		{
			this.centerOfScreen = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2));
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00108A88 File Offset: 0x00106C88
		public void unload()
		{
			this.content.Unload();
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000A8788 File Offset: 0x000A6988
		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D30 RID: 3376
		private const int pictureWidth = 416;

		// Token: 0x04000D31 RID: 3377
		private const int pictureHeight = 496;

		// Token: 0x04000D32 RID: 3378
		private const int sourceWidth = 104;

		// Token: 0x04000D33 RID: 3379
		private const int sourceHeight = 124;

		// Token: 0x04000D34 RID: 3380
		private int numberOfPhotosSoFar;

		// Token: 0x04000D35 RID: 3381
		private int betweenPhotoTimer = 1000;

		// Token: 0x04000D36 RID: 3382
		private LocalizedContentManager content;

		// Token: 0x04000D37 RID: 3383
		private Vector2 centerOfScreen;

		// Token: 0x04000D38 RID: 3384
		private Texture2D pictures;

		// Token: 0x04000D39 RID: 3385
		private float fadeAlpha;
	}
}
