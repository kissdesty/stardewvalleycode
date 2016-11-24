using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000DA RID: 218
	public class ButtonTutorialMenu : IClickableMenu
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x00114648 File Offset: 0x00112848
		public ButtonTutorialMenu(int which) : base(-42 * Game1.pixelZoom, Game1.viewport.Height / 2 - 109 * Game1.pixelZoom / 2, 42 * Game1.pixelZoom, 109 * Game1.pixelZoom, false)
		{
			this.sourceRect = new Rectangle(275 + which * 42, 0, 42, 109);
			ButtonTutorialMenu.current++;
			this.myID = ButtonTutorialMenu.current;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x001146C8 File Offset: 0x001128C8
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.myID != ButtonTutorialMenu.current)
			{
				this.destroy = true;
			}
			if (this.xPositionOnScreen < 0 && this.timerToclose > 0)
			{
				this.xPositionOnScreen += (int)((float)time.ElapsedGameTime.Milliseconds * 0.2f);
				if (this.xPositionOnScreen >= 0)
				{
					this.xPositionOnScreen = 0;
					return;
				}
			}
			else
			{
				this.timerToclose -= time.ElapsedGameTime.Milliseconds;
				if (this.timerToclose <= 0)
				{
					if (this.xPositionOnScreen >= -42 * Game1.pixelZoom - Game1.tileSize)
					{
						this.xPositionOnScreen -= (int)((float)time.ElapsedGameTime.Milliseconds * 0.2f);
						return;
					}
					this.destroy = true;
				}
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0011479C File Offset: 0x0011299C
		public override void draw(SpriteBatch b)
		{
			if (!this.destroy)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(this.sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.82f);
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04000E4F RID: 3663
		public const int move_run_check = 0;

		// Token: 0x04000E50 RID: 3664
		public const int useTool_menu = 1;

		// Token: 0x04000E51 RID: 3665
		public const float movementSpeed = 0.2f;

		// Token: 0x04000E52 RID: 3666
		public new const int width = 42;

		// Token: 0x04000E53 RID: 3667
		public new const int height = 109;

		// Token: 0x04000E54 RID: 3668
		private int timerToclose = 15000;

		// Token: 0x04000E55 RID: 3669
		private Rectangle sourceRect;

		// Token: 0x04000E56 RID: 3670
		private static int current;

		// Token: 0x04000E57 RID: 3671
		private int myID;
	}
}
