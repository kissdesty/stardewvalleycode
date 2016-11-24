using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x0200011A RID: 282
	public class Toolbar : IClickableMenu
	{
		// Token: 0x06001030 RID: 4144 RVA: 0x0014F458 File Offset: 0x0014D658
		public Toolbar() : base(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize, Game1.viewport.Height, Game1.tileSize * 14, Game1.tileSize * 3 + Game1.tileSize / 4, false)
		{
			for (int i = 0; i < 12; i++)
			{
				this.buttons.Add(new ClickableComponent(new Rectangle(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + i * Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8, Game1.tileSize, Game1.tileSize), string.Concat(i)));
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0014F548 File Offset: 0x0014D748
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!Game1.player.usingTool)
			{
				foreach (ClickableComponent c in this.buttons)
				{
					if (c.containsPoint(x, y))
					{
						Game1.player.CurrentToolIndex = Convert.ToInt32(c.name);
						if (Game1.player.ActiveObject != null)
						{
							Game1.player.showCarrying();
							Game1.playSound("pickUpItem");
							break;
						}
						Game1.player.showNotCarrying();
						Game1.playSound("stoneStep");
						break;
					}
				}
			}
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0014F5FC File Offset: 0x0014D7FC
		public override void performHoverAction(int x, int y)
		{
			this.hoverItem = null;
			foreach (ClickableComponent c in this.buttons)
			{
				if (c.containsPoint(x, y))
				{
					int slotNumber = Convert.ToInt32(c.name);
					if (slotNumber < Game1.player.items.Count && Game1.player.items[slotNumber] != null)
					{
						c.scale = Math.Min(c.scale + 0.05f, 1.1f);
						this.hoverTitle = Game1.player.items[slotNumber].Name;
						this.hoverItem = Game1.player.items[slotNumber];
					}
				}
				else
				{
					c.scale = Math.Max(c.scale - 0.025f, 1f);
				}
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0014F6FC File Offset: 0x0014D8FC
		public void shifted(bool right)
		{
			if (right)
			{
				for (int i = 0; i < this.buttons.Count; i++)
				{
					this.buttons[i].scale = 1f + (float)i * 0.03f;
				}
				return;
			}
			for (int j = this.buttons.Count - 1; j >= 0; j--)
			{
				this.buttons[j].scale = 1f + (float)(11 - j) * 0.03f;
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00002834 File Offset: 0x00000A34
		public override void update(GameTime time)
		{
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0014F77C File Offset: 0x0014D97C
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			for (int i = 0; i < 12; i++)
			{
				this.buttons[i].bounds = new Rectangle(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + i * Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8, Game1.tileSize, Game1.tileSize);
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0014F7E8 File Offset: 0x0014D9E8
		public override bool isWithinBounds(int x, int y)
		{
			return new Rectangle(this.buttons.First<ClickableComponent>().bounds.X, this.buttons.First<ClickableComponent>().bounds.Y, this.buttons.Last<ClickableComponent>().bounds.X - this.buttons.First<ClickableComponent>().bounds.X + Game1.tileSize, Game1.tileSize).Contains(x, y);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0014F864 File Offset: 0x0014DA64
		public override void draw(SpriteBatch b)
		{
			if (Game1.activeClickableMenu != null)
			{
				return;
			}
			int arg_143_0 = this.yPositionOnScreen;
			if (Game1.options.pinToolbarToggle)
			{
				this.yPositionOnScreen = Game1.viewport.Height;
				this.transparency = Math.Min(1f, this.transparency + 0.075f);
				if (Game1.GlobalToLocal(Game1.viewport, new Vector2((float)Game1.player.GetBoundingBox().Center.X, (float)Game1.player.GetBoundingBox().Center.Y)).Y > (float)(Game1.viewport.Height - Game1.tileSize * 3))
				{
					this.transparency = Math.Max(0.33f, this.transparency - 0.15f);
				}
			}
			else
			{
				this.yPositionOnScreen = ((Game1.GlobalToLocal(Game1.viewport, new Vector2((float)Game1.player.GetBoundingBox().Center.X, (float)Game1.player.GetBoundingBox().Center.Y)).Y > (float)(Game1.viewport.Height / 2 + Game1.tileSize)) ? (Game1.tileSize + Game1.tileSize * 3 / 4) : Game1.viewport.Height);
			}
			if (arg_143_0 != this.yPositionOnScreen)
			{
				for (int i = 0; i < 12; i++)
				{
					this.buttons[i].bounds.Y = this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8;
				}
			}
			IClickableMenu.drawTextureBox(b, Game1.menuTexture, this.toolbarTextSource, Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.pixelZoom * 4, this.yPositionOnScreen - Game1.tileSize * 3 / 2 - Game1.pixelZoom * 2, Game1.tileSize * 12 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Color.White * this.transparency, 1f, false);
			for (int j = 0; j < 12; j++)
			{
				Vector2 toDraw = new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + j * Game1.tileSize), (float)(this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8));
				b.Draw(Game1.menuTexture, toDraw, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, (Game1.player.CurrentToolIndex == j) ? 56 : 10, -1, -1)), Color.White * this.transparency);
				string strToDraw = (j == 9) ? "0" : ((j == 10) ? "-" : ((j == 11) ? "=" : string.Concat(j + 1)));
				b.DrawString(Game1.tinyFont, strToDraw, toDraw + new Vector2(4f, -8f), Color.DimGray * this.transparency);
			}
			for (int k = 0; k < 12; k++)
			{
				this.buttons[k].scale = Math.Max(1f, this.buttons[k].scale - 0.025f);
				Vector2 toDraw2 = new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + k * Game1.tileSize), (float)(this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8));
				if (Game1.player.items.Count > k && Game1.player.items.ElementAt(k) != null)
				{
					Game1.player.items[k].drawInMenu(b, toDraw2, (Game1.player.CurrentToolIndex == k) ? 0.9f : (this.buttons.ElementAt(k).scale * 0.8f), this.transparency, 0.88f);
				}
			}
			if (this.hoverItem != null)
			{
				IClickableMenu.drawToolTip(b, this.hoverItem.getDescription(), this.hoverItem.Name, this.hoverItem, false, -1, 0, -1, -1, null, -1);
				this.hoverItem = null;
			}
		}

		// Token: 0x040011A8 RID: 4520
		private List<ClickableComponent> buttons = new List<ClickableComponent>();

		// Token: 0x040011A9 RID: 4521
		private new int yPositionOnScreen;

		// Token: 0x040011AA RID: 4522
		private string hoverTitle = "";

		// Token: 0x040011AB RID: 4523
		private Item hoverItem;

		// Token: 0x040011AC RID: 4524
		private float transparency = 1f;

		// Token: 0x040011AD RID: 4525
		public Rectangle toolbarTextSource = new Rectangle(0, 256, 60, 60);
	}
}
