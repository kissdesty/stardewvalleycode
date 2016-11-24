using System;
using System.Runtime.InteropServices;
using System.Text;

namespace StardewValley
{
	// Token: 0x02000013 RID: 19
	public class KeyboardLayout
	{
		// Token: 0x060000F6 RID: 246
		[DllImport("user32.dll")]
		private static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);

		// Token: 0x060000F7 RID: 247
		[DllImport("user32.dll")]
		private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);

		// Token: 0x060000F8 RID: 248 RVA: 0x0000C1F2 File Offset: 0x0000A3F2
		public static string getName()
		{
			StringBuilder expr_07 = new StringBuilder(9);
			KeyboardLayout.GetKeyboardLayoutName(expr_07);
			return expr_07.ToString();
		}

		// Token: 0x04000128 RID: 296
		private const uint KLF_ACTIVATE = 1u;

		// Token: 0x04000129 RID: 297
		private const int KL_NAMELENGTH = 9;

		// Token: 0x0400012A RID: 298
		private const string LANG_EN_US = "00000409";

		// Token: 0x0400012B RID: 299
		private const string LANG_HE_IL = "0001101A";
	}
}
