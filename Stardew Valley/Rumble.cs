using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
	// Token: 0x0200004B RID: 75
	public static class Rumble
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x000A38DC File Offset: 0x000A1ADC
		public static void update(float milliseconds)
		{
			if (Rumble.isRumbling && Game1.options.gamepadControls)
			{
				Rumble.rumbleTimerCurrent += milliseconds;
				if (Rumble.fade)
				{
					if (Rumble.rumbleTimerCurrent > Rumble.rumbleTimerMax - 1000f)
					{
						Rumble.rumbleDuringFade = Rumble.maxRumbleDuringFade - Rumble.maxRumbleDuringFade * ((Rumble.rumbleTimerMax - Rumble.rumbleTimerCurrent) / 1000f);
					}
					GamePad.SetVibration(Game1.playerOneIndex, Rumble.rumbleDuringFade, Rumble.rumbleDuringFade);
				}
				if (Rumble.rumbleTimerCurrent > Rumble.rumbleTimerMax)
				{
					int tries = 500;
					while (!GamePad.SetVibration(Game1.playerOneIndex, 0f, 0f) && tries > 0)
					{
						tries--;
					}
					Rumble.isRumbling = false;
				}
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000A3998 File Offset: 0x000A1B98
		public static void stopRumbling()
		{
			int tries = 0;
			while (GamePad.GetState(PlayerIndex.One).IsConnected && !GamePad.SetVibration(Game1.playerOneIndex, 0f, 0f) && tries < 5)
			{
				tries++;
			}
			Rumble.isRumbling = false;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000A39E0 File Offset: 0x000A1BE0
		public static void rumble(float leftPower, float rightPower, float milliseconds)
		{
			if (!Rumble.isRumbling && Game1.options.gamepadControls && Game1.options.rumble)
			{
				Rumble.rumbleTimerCurrent = 0f;
				Rumble.fade = false;
				Rumble.rumbleTimerMax = milliseconds;
				Rumble.isRumbling = true;
				GamePad.SetVibration(Game1.playerOneIndex, leftPower, rightPower);
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000A3A38 File Offset: 0x000A1C38
		public static void rumble(float power, float milliseconds)
		{
			if (!Rumble.isRumbling && Game1.options.gamepadControls && Game1.options.rumble)
			{
				Rumble.fade = false;
				Rumble.rumbleTimerCurrent = 0f;
				Rumble.rumbleTimerMax = milliseconds;
				Rumble.isRumbling = true;
				GamePad.SetVibration(Game1.playerOneIndex, power, power);
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000A3A90 File Offset: 0x000A1C90
		public static void rumbleAndFade(float power, float milliseconds)
		{
			if (!Rumble.isRumbling && Game1.options.gamepadControls && Game1.options.rumble)
			{
				Rumble.rumbleTimerCurrent = 0f;
				Rumble.rumbleTimerMax = milliseconds;
				Rumble.isRumbling = true;
				GamePad.SetVibration(Game1.playerOneIndex, power, power);
				Rumble.fade = true;
				Rumble.rumbleDuringFade = power;
				Rumble.maxRumbleDuringFade = power;
			}
		}

		// Token: 0x040007B5 RID: 1973
		private static float rumbleTimerMax;

		// Token: 0x040007B6 RID: 1974
		private static float rumbleTimerCurrent;

		// Token: 0x040007B7 RID: 1975
		private static float rumbleDuringFade;

		// Token: 0x040007B8 RID: 1976
		private static float maxRumbleDuringFade;

		// Token: 0x040007B9 RID: 1977
		private static bool isRumbling;

		// Token: 0x040007BA RID: 1978
		private static bool fade;
	}
}
