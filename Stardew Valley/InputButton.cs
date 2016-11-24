using System;
using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
	// Token: 0x02000035 RID: 53
	public struct InputButton
	{
		// Token: 0x060002AE RID: 686 RVA: 0x000376FB File Offset: 0x000358FB
		public InputButton(Keys key)
		{
			this.key = key;
			this.mouseLeft = false;
			this.mouseRight = false;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00037712 File Offset: 0x00035912
		public InputButton(bool mouseLeft)
		{
			this.key = Keys.None;
			this.mouseLeft = mouseLeft;
			this.mouseRight = !mouseLeft;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0003772C File Offset: 0x0003592C
		public override string ToString()
		{
			if (this.mouseLeft)
			{
				return "Left-Click";
			}
			if (this.mouseRight)
			{
				return "Right-Click";
			}
			switch (this.key)
			{
			case Keys.D0:
				return "0";
			case Keys.D1:
				return "1";
			case Keys.D2:
				return "2";
			case Keys.D3:
				return "3";
			case Keys.D4:
				return "4";
			case Keys.D5:
				return "5";
			case Keys.D6:
				return "6";
			case Keys.D7:
				return "7";
			case Keys.D8:
				return "8";
			case Keys.D9:
				return "9";
			default:
				return this.key.ToString().Replace("Oem", "");
			}
		}

		// Token: 0x040002CC RID: 716
		public Keys key;

		// Token: 0x040002CD RID: 717
		public bool mouseLeft;

		// Token: 0x040002CE RID: 718
		public bool mouseRight;
	}
}
