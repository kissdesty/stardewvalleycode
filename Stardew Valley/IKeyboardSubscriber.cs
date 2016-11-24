using System;
using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
	// Token: 0x02000019 RID: 25
	public interface IKeyboardSubscriber
	{
		// Token: 0x06000118 RID: 280
		void RecieveTextInput(char inputChar);

		// Token: 0x06000119 RID: 281
		void RecieveTextInput(string text);

		// Token: 0x0600011A RID: 282
		void RecieveCommandInput(char command);

		// Token: 0x0600011B RID: 283
		void RecieveSpecialInput(Keys key);

		// Token: 0x17000017 RID: 23
		bool Selected
		{
			// Token: 0x0600011C RID: 284
			get;
			// Token: 0x0600011D RID: 285
			set;
		}
	}
}
