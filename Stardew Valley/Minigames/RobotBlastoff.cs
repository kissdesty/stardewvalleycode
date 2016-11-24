using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
	// Token: 0x020000D1 RID: 209
	public class RobotBlastoff : IMinigame
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x0010D124 File Offset: 0x0010B324
		public bool tick(GameTime time)
		{
			this.millisecondsSinceStart += time.ElapsedGameTime.Milliseconds;
			float f = 1.35f - 0.85f * (5f / Math.Max(5f, this.robotPosition.Y / 20f));
			this.backgroundPosition += (int)(0.25f * (float)time.ElapsedGameTime.Milliseconds * f) / 2;
			this.robotPosition.Y = this.robotPosition.Y - 0.3f * (float)time.ElapsedGameTime.Milliseconds / 4f;
			this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.smokeTimer <= 0)
			{
				this.smokeTimer = 350;
				this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(143, 1828, 15, 20), 1500f, 4, 0, this.robotPosition + new Vector2(0f, (float)(18 * Game1.pixelZoom)), false, false)
				{
					motion = new Vector2(0f, -0.9f),
					acceleration = new Vector2(-0.001f, 0.006f),
					scale = (float)Game1.pixelZoom,
					scaleChange = 0.002f,
					alphaFade = 0.0025f
				});
			}
			for (int i = this.tempSprites.Count - 1; i >= 0; i--)
			{
				if (this.tempSprites[i].update(time))
				{
					this.tempSprites.RemoveAt(i);
				}
			}
			if (this.robotPosition.Y < 0f && Game1.random.NextDouble() < 0.005)
			{
				this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(256, 1680, 16, 16), 80f, 5, 0, new Vector2((float)Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Width), (float)Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Height / 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					motion = new Vector2(4f, 4f)
				});
			}
			if (this.robotPosition.Y < (float)(-(float)Game1.tileSize * 8) && !Game1.globalFade)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 0.006f);
			}
			return false;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0010D3E8 File Offset: 0x0010B5E8
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

		// Token: 0x06000D4A RID: 3402 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0010D43A File Offset: 0x0010B63A
		public void receiveKeyPress(Keys k)
		{
			if (k == Keys.Escape)
			{
				this.robotPosition.Y = -1000f;
				this.tempSprites.Clear();
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0010D45C File Offset: 0x0010B65C
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.mouseCursors, new Rectangle(0, this.backgroundPosition, Game1.graphics.GraphicsDevice.Viewport.Width, 2560), new Rectangle?(new Rectangle(264, 1858, 1, 84)), Color.White);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)this.backgroundPosition), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.5f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.backgroundPosition - 188 * Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.Draw(Game1.mouseCursors, this.robotPosition + new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)), new Rectangle?(new Rectangle(206 + this.millisecondsSinceStart / 50 % 4 * 15, 1827, 15, 27)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.tempSprites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b, true, 0, 0);
				}
			}
			b.End();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0010D710 File Offset: 0x0010B910
		public void changeScreenSize()
		{
			this.backgroundPosition = 2560 - Game1.graphics.GraphicsDevice.Viewport.Height;
			this.robotPosition = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)Game1.graphics.GraphicsDevice.Viewport.Height);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00002834 File Offset: 0x00000A34
		public void unload()
		{
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000A8788 File Offset: 0x000A6988
		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000DC8 RID: 3528
		public const float backGroundSpeed = 0.25f;

		// Token: 0x04000DC9 RID: 3529
		public const float robotSpeed = 0.3f;

		// Token: 0x04000DCA RID: 3530
		public const int skyLength = 2560;

		// Token: 0x04000DCB RID: 3531
		public int millisecondsSinceStart;

		// Token: 0x04000DCC RID: 3532
		public int backgroundPosition = -2560 + Game1.graphics.GraphicsDevice.Viewport.Height;

		// Token: 0x04000DCD RID: 3533
		public int smokeTimer = 500;

		// Token: 0x04000DCE RID: 3534
		public Vector2 robotPosition = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float)Game1.graphics.GraphicsDevice.Viewport.Height);

		// Token: 0x04000DCF RID: 3535
		public List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();
	}
}
