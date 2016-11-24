using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x02000100 RID: 256
	public class MineElevatorMenu : IClickableMenu
	{
		// Token: 0x06000F47 RID: 3911 RVA: 0x0013AC1C File Offset: 0x00138E1C
		public MineElevatorMenu() : base(0, 0, 0, 0, true)
		{
			int numElevators = Math.Min(Game1.mine.lowestLevelReached, 120) / 5;
			this.width = ((numElevators > 50) ? ((Game1.tileSize * 3 / 4 - 4) * 11 + IClickableMenu.borderWidth * 2) : Math.Min((Game1.tileSize * 3 / 4 - 4) * 5 + IClickableMenu.borderWidth * 2, numElevators * (Game1.tileSize * 3 / 4 - 4) + IClickableMenu.borderWidth * 2));
			this.height = Math.Max(Game1.tileSize + IClickableMenu.borderWidth * 3, numElevators * (Game1.tileSize * 3 / 4 - 4) / (this.width - IClickableMenu.borderWidth) * (Game1.tileSize * 3 / 4 - 4) + Game1.tileSize + IClickableMenu.borderWidth * 3);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - this.width / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;
			Game1.playSound("crystal");
			int x = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
			int y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.borderWidth / 3;
			this.elevators.Add(new ClickableComponent(new Rectangle(x, y, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat(0)));
			x = x + Game1.tileSize - 20;
			if (x > this.xPositionOnScreen + this.width - IClickableMenu.borderWidth)
			{
				x = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
				y += Game1.tileSize - 20;
			}
			for (int i = 1; i <= numElevators; i++)
			{
				this.elevators.Add(new ClickableComponent(new Rectangle(x, y, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat(i * 5)));
				x = x + Game1.tileSize - 20;
				if (x > this.xPositionOnScreen + this.width - IClickableMenu.borderWidth)
				{
					x = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
					y += Game1.tileSize - 20;
				}
			}
			base.initializeUpperRightCloseButton();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0013AE70 File Offset: 0x00139070
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.isWithinBounds(x, y))
			{
				foreach (ClickableComponent c in this.elevators)
				{
					if (c.containsPoint(x, y))
					{
						Game1.playSound("smallSelect");
						if (Convert.ToInt32(c.name) == 0)
						{
							if (!Game1.currentLocation.Equals(Game1.mine))
							{
								return;
							}
							Game1.warpFarmer("Mine", 17, 4, true);
							Game1.exitActiveMenu();
							Game1.changeMusicTrack("none");
						}
						else
						{
							if (Game1.currentLocation.Equals(Game1.mine) && Convert.ToInt32(c.name) == Game1.mine.mineLevel)
							{
								return;
							}
							Game1.player.ridingMineElevator = true;
							Game1.enterMine(false, Convert.ToInt32(c.name), null);
							Game1.exitActiveMenu();
						}
					}
				}
				base.receiveLeftClick(x, y, true);
				return;
			}
			Game1.exitActiveMenu();
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0013AF84 File Offset: 0x00139184
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			foreach (ClickableComponent c in this.elevators)
			{
				if (c.containsPoint(x, y))
				{
					c.scale = 2f;
				}
				else
				{
					c.scale = 1f;
				}
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0013AFFC File Offset: 0x001391FC
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize + Game1.tileSize / 8, this.width + Game1.tileSize / 3, this.height + Game1.tileSize, false, true, null, false);
			foreach (ClickableComponent c in this.elevators)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(c.bounds.X - Game1.pixelZoom), (float)(c.bounds.Y + Game1.pixelZoom)), new Rectangle?(new Rectangle((c.scale > 1f) ? 267 : 256, 256, 10, 10)), Color.Black * 0.5f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.865f);
				b.Draw(Game1.mouseCursors, new Vector2((float)c.bounds.X, (float)c.bounds.Y), new Rectangle?(new Rectangle((c.scale > 1f) ? 267 : 256, 256, 10, 10)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.868f);
				Vector2 textPosition = new Vector2((float)(c.bounds.X + 16 + NumberSprite.numberOfDigits(Convert.ToInt32(c.name)) * 6), (float)(c.bounds.Y + Game1.pixelZoom * 6 - NumberSprite.getHeight() / 4));
				NumberSprite.draw(Convert.ToInt32(c.name), b, textPosition, ((Game1.mine.mineLevel == Convert.ToInt32(c.name) && Game1.currentLocation.Equals(Game1.mine)) || (Convert.ToInt32(c.name) == 0 && !Game1.currentLocation.Equals(Game1.mine))) ? (Color.Gray * 0.75f) : Color.Gold, 0.5f, 0.86f, 1f, 0, 0);
			}
			base.drawMouse(b);
			base.draw(b);
		}

		// Token: 0x04001070 RID: 4208
		public List<ClickableComponent> elevators = new List<ClickableComponent>();
	}
}
