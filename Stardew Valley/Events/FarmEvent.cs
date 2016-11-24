using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Events
{
	// Token: 0x0200013D RID: 317
	public interface FarmEvent
	{
		// Token: 0x060011F5 RID: 4597
		bool setUp();

		// Token: 0x060011F6 RID: 4598
		bool tickUpdate(GameTime time);

		// Token: 0x060011F7 RID: 4599
		void draw(SpriteBatch b);

		// Token: 0x060011F8 RID: 4600
		void drawAboveEverything(SpriteBatch b);

		// Token: 0x060011F9 RID: 4601
		void makeChangesToLocation();
	}
}
