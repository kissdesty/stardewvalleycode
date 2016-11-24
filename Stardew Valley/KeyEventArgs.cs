using System;
using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
	// Token: 0x02000015 RID: 21
	public class KeyEventArgs : EventArgs
	{
		// Token: 0x06000102 RID: 258 RVA: 0x0000C27F File Offset: 0x0000A47F
		public KeyEventArgs(Keys keyCode)
		{
			this.keyCode = keyCode;
		}

		// Token: 0x17000016 RID: 22
		public Keys KeyCode
		{
			// Token: 0x06000103 RID: 259 RVA: 0x0000C28E File Offset: 0x0000A48E
			get
			{
				return this.keyCode;
			}
		}

		// Token: 0x0400012E RID: 302
		private Keys keyCode;
	}
}
