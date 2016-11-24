using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;

namespace StardewValley.Minigames
{
	// Token: 0x020000C5 RID: 197
	public class Test : IMinigame
	{
		// Token: 0x06000C99 RID: 3225 RVA: 0x0010151C File Offset: 0x000FF71C
		public Test()
		{
			for (int i = 0; i < 40; i++)
			{
				this.wallpaper.Add(new Wallpaper(i, true));
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0000821B File Offset: 0x0000641B
		public bool tick(GameTime time)
		{
			return false;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00002834 File Offset: 0x00000A34
		public void afterFade()
		{
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00101559 File Offset: 0x000FF759
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			Game1.currentMinigame = null;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyPress(Keys k)
		{
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00101564 File Offset: 0x000FF764
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, 2000, 2000), Color.White);
			Vector2 draw = new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4));
			for (int i = 0; i < this.wallpaper.Count; i++)
			{
				this.wallpaper[i].drawInMenu(b, draw, 1f);
				draw.X += (float)(Game1.tileSize * 2);
				if (draw.X >= (float)(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2))
				{
					draw.X = (float)(Game1.tileSize / 4);
					draw.Y += (float)(Game1.tileSize * 2);
				}
			}
			b.End();
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00002834 File Offset: 0x00000A34
		public void changeScreenSize()
		{
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00002834 File Offset: 0x00000A34
		public void unload()
		{
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveEventPoke(int data)
		{
		}

		// Token: 0x04000CCF RID: 3279
		public List<Wallpaper> wallpaper = new List<Wallpaper>();
	}
}
