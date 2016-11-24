using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
	// Token: 0x020000CA RID: 202
	public class PlaneFlyBy : IMinigame
	{
		// Token: 0x06000CE8 RID: 3304 RVA: 0x00107668 File Offset: 0x00105868
		public bool tick(GameTime time)
		{
			this.millisecondsSinceStart += time.ElapsedGameTime.Milliseconds;
			this.robotPosition.X = this.robotPosition.X - 1f * (float)time.ElapsedGameTime.Milliseconds / 4f;
			this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.smokeTimer <= 0)
			{
				this.smokeTimer = 100;
				this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(173, 1828, 15, 20), 1500f, 2, 0, this.robotPosition + new Vector2((float)(17 * Game1.pixelZoom), (float)(-6 * Game1.pixelZoom)), false, false)
				{
					motion = new Vector2(0f, 0.1f),
					scale = (float)Game1.pixelZoom,
					scaleChange = 0.002f,
					alphaFade = 0.0025f,
					rotation = -1.57079637f
				});
			}
			for (int i = this.tempSprites.Count - 1; i >= 0; i--)
			{
				if (this.tempSprites[i].update(time))
				{
					this.tempSprites.RemoveAt(i);
				}
			}
			if (this.robotPosition.X < (float)(-(float)Game1.tileSize * 2) && !Game1.globalFade)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 0.006f);
			}
			return false;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x001077EC File Offset: 0x001059EC
		public void afterFade()
		{
			Game1.currentMinigame = null;
			Game1.globalFadeToClear(null, 0.02f);
			if (Game1.currentLocation.currentEvent != null)
			{
				Event expr_27 = Game1.currentLocation.currentEvent;
				int currentCommand = expr_27.CurrentCommand;
				expr_27.CurrentCommand = currentCommand + 1;
				Game1.currentLocation.temporarySprites.Clear();
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0010783E File Offset: 0x00105A3E
		public void receiveKeyPress(Keys k)
		{
			if (k == Keys.Escape)
			{
				this.robotPosition.X = -1000f;
				this.tempSprites.Clear();
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00107860 File Offset: 0x00105A60
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.mouseCursors, new Rectangle(0, this.backgroundPosition, Game1.graphics.GraphicsDevice.Viewport.Width, 2560), new Rectangle?(new Rectangle(264, 1858, 1, 84)), Color.White);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)this.backgroundPosition), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.5f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, this.robotPosition, new Rectangle?(new Rectangle(222 + this.millisecondsSinceStart / 50 % 2 * 20, 1890, 20, 9)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.tempSprites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, true, 0, 0);
				}
			}
			b.End();
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00107AF0 File Offset: 0x00105CF0
		public void changeScreenSize()
		{
			this.backgroundPosition = 2560 - Game1.graphics.GraphicsDevice.Viewport.Height;
			this.robotPosition = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)Game1.graphics.GraphicsDevice.Viewport.Height);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00002834 File Offset: 0x00000A34
		public void unload()
		{
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000A8788 File Offset: 0x000A6988
		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D1D RID: 3357
		public const float backGroundSpeed = 0.25f;

		// Token: 0x04000D1E RID: 3358
		public const float robotSpeed = 1f;

		// Token: 0x04000D1F RID: 3359
		public const int skyLength = 2560;

		// Token: 0x04000D20 RID: 3360
		public int millisecondsSinceStart;

		// Token: 0x04000D21 RID: 3361
		public int backgroundPosition = -2560 + Game1.graphics.GraphicsDevice.Viewport.Height;

		// Token: 0x04000D22 RID: 3362
		public int smokeTimer = 500;

		// Token: 0x04000D23 RID: 3363
		public Vector2 robotPosition = new Vector2((float)Game1.graphics.GraphicsDevice.Viewport.Width, (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2));

		// Token: 0x04000D24 RID: 3364
		public List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();
	}
}
