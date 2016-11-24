using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;

namespace StardewValley.Menus
{
	// Token: 0x020000EC RID: 236
	public class DiscreteColorPicker : IClickableMenu
	{
		// Token: 0x06000E48 RID: 3656 RVA: 0x0012392C File Offset: 0x00121B2C
		public DiscreteColorPicker(int xPosition, int yPosition, int startingColor = 0, Item itemToDrawColored = null)
		{
			this.totalColors = 21;
			this.xPositionOnScreen = xPosition;
			this.yPositionOnScreen = yPosition;
			this.width = this.totalColors * 9 * Game1.pixelZoom + IClickableMenu.borderWidth;
			this.height = 7 * Game1.pixelZoom + IClickableMenu.borderWidth;
			this.itemToDrawColored = itemToDrawColored;
			this.visible = Game1.player.showChestColorPicker;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x001239A4 File Offset: 0x00121BA4
		public int getSelectionFromColor(Color c)
		{
			for (int i = 0; i < this.totalColors; i++)
			{
				if (this.getColorFromSelection(i).Equals(c))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x001239D7 File Offset: 0x00121BD7
		public Color getCurrentColor()
		{
			return this.getColorFromSelection(this.colorSelection);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0011E4C9 File Offset: 0x0011C6C9
		public override void update(GameTime time)
		{
			base.update(time);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x001239E8 File Offset: 0x00121BE8
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.visible)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			Rectangle area = new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth / 2, this.yPositionOnScreen + IClickableMenu.borderWidth / 2, 9 * Game1.pixelZoom * this.totalColors, 7 * Game1.pixelZoom);
			if (area.Contains(x, y))
			{
				this.colorSelection = (x - area.X) / (9 * Game1.pixelZoom);
				Game1.soundBank.PlayCue("coin");
				if (this.itemToDrawColored is Chest)
				{
					(this.itemToDrawColored as Chest).playerChoiceColor = this.getColorFromSelection(this.colorSelection);
					(this.itemToDrawColored as Chest).currentLidFrame = 131;
				}
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00123AB0 File Offset: 0x00121CB0
		public Color getColorFromSelection(int selection)
		{
			switch (selection)
			{
			case 1:
				return new Color(85, 85, 255);
			case 2:
				return new Color(119, 191, 255);
			case 3:
				return new Color(0, 170, 170);
			case 4:
				return new Color(0, 234, 175);
			case 5:
				return new Color(0, 170, 0);
			case 6:
				return new Color(159, 236, 0);
			case 7:
				return new Color(255, 234, 18);
			case 8:
				return new Color(255, 167, 18);
			case 9:
				return new Color(255, 105, 18);
			case 10:
				return new Color(255, 0, 0);
			case 11:
				return new Color(135, 0, 35);
			case 12:
				return new Color(255, 173, 199);
			case 13:
				return new Color(255, 117, 195);
			case 14:
				return new Color(172, 0, 198);
			case 15:
				return new Color(143, 0, 255);
			case 16:
				return new Color(89, 11, 142);
			case 17:
				return new Color(64, 64, 64);
			case 18:
				return new Color(100, 100, 100);
			case 19:
				return new Color(200, 200, 200);
			case 20:
				return new Color(254, 254, 254);
			default:
				return Color.Black;
			}
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00123C68 File Offset: 0x00121E68
		public override void draw(SpriteBatch b)
		{
			if (this.visible)
			{
				IClickableMenu.drawTextureBox(b, this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.LightGray);
				for (int i = 0; i < this.totalColors; i++)
				{
					if (i == 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth / 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth / 2)), new Rectangle?(new Rectangle(295, 503, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
					}
					else
					{
						b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth / 2 + i * 9 * Game1.pixelZoom, this.yPositionOnScreen + IClickableMenu.borderWidth / 2, 7 * Game1.pixelZoom, 7 * Game1.pixelZoom), this.getColorFromSelection(i));
					}
					if (i == this.colorSelection)
					{
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), this.xPositionOnScreen + IClickableMenu.borderWidth / 2 - Game1.pixelZoom + i * 9 * Game1.pixelZoom, this.yPositionOnScreen + IClickableMenu.borderWidth / 2 - Game1.pixelZoom, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, Color.Black, (float)Game1.pixelZoom, false);
					}
				}
				if (this.itemToDrawColored != null && this.itemToDrawColored is Chest)
				{
					(this.itemToDrawColored as Chest).draw(b, this.xPositionOnScreen + this.width + IClickableMenu.borderWidth / 2, this.yPositionOnScreen + Game1.pixelZoom * 4, 1f, true);
				}
			}
		}

		// Token: 0x04000F3F RID: 3903
		public const int sizeOfEachSwatch = 7;

		// Token: 0x04000F40 RID: 3904
		public Item itemToDrawColored;

		// Token: 0x04000F41 RID: 3905
		public bool showExample;

		// Token: 0x04000F42 RID: 3906
		public bool visible = true;

		// Token: 0x04000F43 RID: 3907
		public int colorSelection;

		// Token: 0x04000F44 RID: 3908
		public int totalColors;
	}
}
