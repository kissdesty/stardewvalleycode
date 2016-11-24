using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000D4 RID: 212
	public class AddNewItemMenu : IClickableMenu
	{
		// Token: 0x06000D68 RID: 3432 RVA: 0x0010E250 File Offset: 0x0010C450
		public AddNewItemMenu() : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (300 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 300 + IClickableMenu.borderWidth * 2, false)
		{
			this.playerInventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, true, null, null, -1, 3, 0, 0, true);
			this.garbage = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + this.height - Game1.tileSize, Game1.tileSize, Game1.tileSize), "Garbage");
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00002834 File Offset: 0x00000A34
		public override void draw(SpriteBatch b)
		{
		}

		// Token: 0x04000DDA RID: 3546
		private InventoryMenu playerInventory;

		// Token: 0x04000DDB RID: 3547
		private ClickableComponent garbage;
	}
}
