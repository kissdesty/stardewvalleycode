using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x020000EA RID: 234
	internal class DebugInputMenu : NamingMenu
	{
		// Token: 0x06000E26 RID: 3622 RVA: 0x001211B8 File Offset: 0x0011F3B8
		public DebugInputMenu(NamingMenu.doneNamingBehavior b) : base(b, "Debug Input:", "")
		{
			this.textBox.limitWidth = false;
			this.textBox.Width = Game1.tileSize * 8;
			this.textBox.X -= Game1.tileSize * 2;
			ClickableTextureComponent expr_58_cp_0_cp_0 = this.randomButton;
			expr_58_cp_0_cp_0.bounds.X = expr_58_cp_0_cp_0.bounds.X + Game1.tileSize * 2;
			ClickableTextureComponent expr_73_cp_0_cp_0 = this.doneNamingButton;
			expr_73_cp_0_cp_0.bounds.X = expr_73_cp_0_cp_0.bounds.X + Game1.tileSize * 2;
			this.minLength = 0;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0012124C File Offset: 0x0011F44C
		public override void update(GameTime time)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Game1.exitActiveMenu();
				Game1.lastDebugInput = this.textBox.Text;
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00121280 File Offset: 0x0011F480
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.randomButton.containsPoint(x, y))
			{
				this.textBox.Text = Game1.lastDebugInput;
				this.randomButton.scale = this.randomButton.baseScale;
				Game1.playSound("drumkit6");
				return;
			}
			base.receiveLeftClick(x, y, playSound);
		}
	}
}
