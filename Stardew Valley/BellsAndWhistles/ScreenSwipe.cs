using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200015A RID: 346
	public class ScreenSwipe
	{
		// Token: 0x06001323 RID: 4899 RVA: 0x0018174C File Offset: 0x0017F94C
		public ScreenSwipe(int which, float swipeVelocity = -1f, int durationAfterSwipe = -1)
		{
			Game1.playSound("throw");
			if (swipeVelocity == -1f)
			{
				swipeVelocity = 5f;
			}
			if (durationAfterSwipe == -1)
			{
				durationAfterSwipe = 2700;
			}
			this.swipeVelocity = swipeVelocity;
			this.durationAfterSwipe = durationAfterSwipe;
			Vector2 screenCenter = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2));
			if (which == 0)
			{
				this.messageSource = new Rectangle(128, 1367, 110, 14);
			}
			if (which == 0)
			{
				this.bgSource = new Rectangle(128, 1296, 1, 71);
				this.flairSource = new Rectangle(144, 1303, 144, 58);
				this.movingFlairSource = new Rectangle(643, 768, 8, 13);
				this.originalBGSourceXLimit = this.bgSource.X + this.bgSource.Width;
				this.yPosition = (int)screenCenter.Y - this.bgSource.Height * Game1.pixelZoom / 2;
				this.messagePosition = new Vector2(screenCenter.X - (float)(this.messageSource.Width * Game1.pixelZoom / 2), screenCenter.Y - (float)(this.messageSource.Height * Game1.pixelZoom / 2));
				this.flairPositions.Add(new Vector2(this.messagePosition.X - (float)(this.flairSource.Width * Game1.pixelZoom) - (float)Game1.tileSize, (float)(this.yPosition + 7 * Game1.pixelZoom)));
				this.flairPositions.Add(new Vector2(this.messagePosition.X + (float)(this.messageSource.Width * Game1.pixelZoom) + (float)Game1.tileSize, (float)(this.yPosition + 7 * Game1.pixelZoom)));
				this.movingFlairPosition = new Vector2(this.messagePosition.X + (float)(this.messageSource.Width * Game1.pixelZoom) + (float)(Game1.tileSize * 3), screenCenter.Y + (float)(Game1.tileSize / 2));
				this.movingFlairMotion = new Vector2(0f, -0.5f);
			}
			this.bgDest = new Rectangle(0, this.yPosition, this.bgSource.Width * Game1.pixelZoom, this.bgSource.Height * Game1.pixelZoom);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x001819D8 File Offset: 0x0017FBD8
		public bool update(GameTime time)
		{
			if (this.durationAfterSwipe > 0 && this.bgDest.Width <= Game1.viewport.Width)
			{
				this.bgDest.Width = this.bgDest.Width + (int)((double)this.swipeVelocity * time.ElapsedGameTime.TotalMilliseconds);
				if (this.bgDest.Width > Game1.viewport.Width)
				{
					Game1.playSound("newRecord");
				}
			}
			else if (this.durationAfterSwipe <= 0)
			{
				this.bgDest.X = this.bgDest.X + (int)((double)this.swipeVelocity * time.ElapsedGameTime.TotalMilliseconds);
				for (int i = 0; i < this.flairPositions.Count; i++)
				{
					if ((float)this.bgDest.X > this.flairPositions[i].X)
					{
						this.flairPositions[i] = new Vector2((float)this.bgDest.X, this.flairPositions[i].Y);
					}
				}
				if ((float)this.bgDest.X > this.messagePosition.X)
				{
					this.messagePosition = new Vector2((float)this.bgDest.X, this.messagePosition.Y);
				}
				if ((float)this.bgDest.X > this.movingFlairPosition.X)
				{
					this.movingFlairPosition = new Vector2((float)this.bgDest.X, this.movingFlairPosition.Y);
				}
			}
			if (this.bgDest.Width > Game1.viewport.Width && this.durationAfterSwipe > 0)
			{
				if (Game1.oldMouseState.LeftButton == ButtonState.Pressed)
				{
					this.durationAfterSwipe = 0;
				}
				this.durationAfterSwipe -= (int)time.ElapsedGameTime.TotalMilliseconds;
				if (this.durationAfterSwipe <= 0)
				{
					Game1.playSound("tinyWhip");
				}
			}
			this.movingFlairPosition += this.movingFlairMotion;
			return this.bgDest.X > Game1.viewport.Width;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00181BF0 File Offset: 0x0017FDF0
		public Rectangle getAdjustedSourceRect(Rectangle sourceRect, float xStartPosition)
		{
			if (xStartPosition > (float)this.bgDest.Width || xStartPosition + (float)(sourceRect.Width * Game1.pixelZoom) < (float)this.bgDest.X)
			{
				return Rectangle.Empty;
			}
			int x = (int)Math.Max((float)sourceRect.X, (float)sourceRect.X + ((float)this.bgDest.X - xStartPosition) / (float)Game1.pixelZoom);
			return new Rectangle(x, sourceRect.Y, (int)Math.Min((float)(sourceRect.Width - (x - sourceRect.X) / Game1.pixelZoom), ((float)this.bgDest.Width - xStartPosition) / (float)Game1.pixelZoom), sourceRect.Height);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00181CA0 File Offset: 0x0017FEA0
		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, this.bgDest, new Rectangle?(this.bgSource), Color.White);
			foreach (Vector2 v in this.flairPositions)
			{
				Rectangle r = this.getAdjustedSourceRect(this.flairSource, v.X);
				if (r.Right >= this.originalBGSourceXLimit)
				{
					r.Width = this.originalBGSourceXLimit - r.X;
				}
				b.Draw(Game1.mouseCursors, v, new Rectangle?(r), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			b.Draw(Game1.mouseCursors, this.movingFlairPosition, new Rectangle?(this.getAdjustedSourceRect(this.movingFlairSource, this.movingFlairPosition.X)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, this.messagePosition, new Rectangle?(this.getAdjustedSourceRect(this.messageSource, this.messagePosition.X)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
		}

		// Token: 0x0400139E RID: 5022
		public const int swipe_bundleComplete = 0;

		// Token: 0x0400139F RID: 5023
		public const int borderPixelWidth = 7;

		// Token: 0x040013A0 RID: 5024
		private Rectangle bgSource;

		// Token: 0x040013A1 RID: 5025
		private Rectangle flairSource;

		// Token: 0x040013A2 RID: 5026
		private Rectangle messageSource;

		// Token: 0x040013A3 RID: 5027
		private Rectangle movingFlairSource;

		// Token: 0x040013A4 RID: 5028
		private Rectangle bgDest;

		// Token: 0x040013A5 RID: 5029
		private int yPosition;

		// Token: 0x040013A6 RID: 5030
		private int durationAfterSwipe;

		// Token: 0x040013A7 RID: 5031
		private int originalBGSourceXLimit;

		// Token: 0x040013A8 RID: 5032
		private List<Vector2> flairPositions = new List<Vector2>();

		// Token: 0x040013A9 RID: 5033
		private Vector2 messagePosition;

		// Token: 0x040013AA RID: 5034
		private Vector2 movingFlairPosition;

		// Token: 0x040013AB RID: 5035
		private Vector2 movingFlairMotion;

		// Token: 0x040013AC RID: 5036
		private float swipeVelocity;
	}
}
