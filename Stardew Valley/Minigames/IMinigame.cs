using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
	// Token: 0x020000CE RID: 206
	public interface IMinigame
	{
		// Token: 0x06000D26 RID: 3366
		bool tick(GameTime time);

		// Token: 0x06000D27 RID: 3367
		void receiveLeftClick(int x, int y, bool playSound = true);

		// Token: 0x06000D28 RID: 3368
		void leftClickHeld(int x, int y);

		// Token: 0x06000D29 RID: 3369
		void receiveRightClick(int x, int y, bool playSound = true);

		// Token: 0x06000D2A RID: 3370
		void releaseLeftClick(int x, int y);

		// Token: 0x06000D2B RID: 3371
		void releaseRightClick(int x, int y);

		// Token: 0x06000D2C RID: 3372
		void receiveKeyPress(Keys k);

		// Token: 0x06000D2D RID: 3373
		void receiveKeyRelease(Keys k);

		// Token: 0x06000D2E RID: 3374
		void draw(SpriteBatch b);

		// Token: 0x06000D2F RID: 3375
		void changeScreenSize();

		// Token: 0x06000D30 RID: 3376
		void unload();

		// Token: 0x06000D31 RID: 3377
		void receiveEventPoke(int data);
	}
}
