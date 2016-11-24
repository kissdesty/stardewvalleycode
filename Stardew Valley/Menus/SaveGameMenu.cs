using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x0200010E RID: 270
	public class SaveGameMenu : IClickableMenu
	{
		// Token: 0x06000FA5 RID: 4005 RVA: 0x001419BC File Offset: 0x0013FBBC
		public SaveGameMenu()
		{
			this.saveText = new SparklingText(Game1.dialogueFont, "Your progress has been saved.", Color.LimeGreen, Color.Black * 0.001f, false, 0.1, 1500, Game1.tileSize / 2, 500);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00141A28 File Offset: 0x0013FC28
		public override void update(GameTime time)
		{
			if (this.quit)
			{
				return;
			}
			base.update(time);
			if (!Game1.saveOnNewDay)
			{
				this.quit = true;
				if (Game1.activeClickableMenu.Equals(this))
				{
					Game1.player.checkForLevelTenStatus();
					Game1.exitActiveMenu();
				}
				return;
			}
			if (this.loader != null)
			{
				this.loader.MoveNext();
				if (this.loader.Current >= 100)
				{
					this.margin -= time.ElapsedGameTime.Milliseconds;
					if (this.margin <= 0)
					{
						Game1.playSound("money");
						this.completePause = 1500;
						this.loader = null;
					}
				}
			}
			else if (this.hasDrawn && this.completePause == -1)
			{
				this.loader = SaveGame.Save();
			}
			if (this.completePause >= 0)
			{
				this.completePause -= time.ElapsedGameTime.Milliseconds;
				this.saveText.update(time);
				if (this.completePause < 0)
				{
					this.quit = true;
					this.completePause = -9999;
					if (Game1.activeClickableMenu.Equals(this))
					{
						Game1.player.checkForLevelTenStatus();
						Game1.exitActiveMenu();
					}
					Game1.currentLocation.resetForPlayerEntry();
				}
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00141B64 File Offset: 0x0013FD64
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.completePause >= 0)
			{
				this.saveText.draw(b, new Vector2((float)Game1.tileSize, (float)(Game1.viewport.Height - Game1.tileSize)));
			}
			else
			{
				b.DrawString(Game1.dialogueFont, "Saving...", new Vector2((float)Game1.tileSize, (float)(Game1.viewport.Height - Game1.tileSize)), Color.White);
			}
			this.hasDrawn = true;
		}

		// Token: 0x040010E5 RID: 4325
		private IEnumerator<int> loader;

		// Token: 0x040010E6 RID: 4326
		private int completePause = -1;

		// Token: 0x040010E7 RID: 4327
		public bool quit;

		// Token: 0x040010E8 RID: 4328
		public bool hasDrawn;

		// Token: 0x040010E9 RID: 4329
		private SparklingText saveText;

		// Token: 0x040010EA RID: 4330
		private int margin = 500;
	}
}
