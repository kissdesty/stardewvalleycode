using System;

namespace StardewValley
{
	// Token: 0x02000014 RID: 20
	public class CharacterEventArgs : EventArgs
	{
		// Token: 0x060000FA RID: 250 RVA: 0x0000C207 File Offset: 0x0000A407
		public CharacterEventArgs(char character, int lParam)
		{
			this.character = character;
			this.lParam = lParam;
		}

		// Token: 0x1700000F RID: 15
		public char Character
		{
			// Token: 0x060000FB RID: 251 RVA: 0x0000C21D File Offset: 0x0000A41D
			get
			{
				return this.character;
			}
		}

		// Token: 0x17000010 RID: 16
		public int Param
		{
			// Token: 0x060000FC RID: 252 RVA: 0x0000C225 File Offset: 0x0000A425
			get
			{
				return this.lParam;
			}
		}

		// Token: 0x17000011 RID: 17
		public int RepeatCount
		{
			// Token: 0x060000FD RID: 253 RVA: 0x0000C22D File Offset: 0x0000A42D
			get
			{
				return this.lParam & 65535;
			}
		}

		// Token: 0x17000012 RID: 18
		public bool ExtendedKey
		{
			// Token: 0x060000FE RID: 254 RVA: 0x0000C23B File Offset: 0x0000A43B
			get
			{
				return (this.lParam & 16777216) > 0;
			}
		}

		// Token: 0x17000013 RID: 19
		public bool AltPressed
		{
			// Token: 0x060000FF RID: 255 RVA: 0x0000C24C File Offset: 0x0000A44C
			get
			{
				return (this.lParam & 536870912) > 0;
			}
		}

		// Token: 0x17000014 RID: 20
		public bool PreviousState
		{
			// Token: 0x06000100 RID: 256 RVA: 0x0000C25D File Offset: 0x0000A45D
			get
			{
				return (this.lParam & 1073741824) > 0;
			}
		}

		// Token: 0x17000015 RID: 21
		public bool TransitionState
		{
			// Token: 0x06000101 RID: 257 RVA: 0x0000C26E File Offset: 0x0000A46E
			get
			{
				return (this.lParam & -2147483648) > 0;
			}
		}

		// Token: 0x0400012C RID: 300
		private readonly char character;

		// Token: 0x0400012D RID: 301
		private readonly int lParam;
	}
}
