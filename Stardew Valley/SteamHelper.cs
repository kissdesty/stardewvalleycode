using System;
using Steamworks;

namespace StardewValley
{
	// Token: 0x02000051 RID: 81
	public class SteamHelper
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x000A64F8 File Offset: 0x000A46F8
		public static void initialize()
		{
			if (!SteamHelper.steamworksEnabled)
			{
				return;
			}
			try
			{
				SteamHelper.active = SteamAPI.Init();
			}
			catch (Exception)
			{
			}
			if (SteamHelper.active)
			{
				SteamHelper.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(SteamHelper.OnGameOverlayActivated));
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000A654C File Offset: 0x000A474C
		public static void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
		{
			if (SteamHelper.active)
			{
				if (pCallback.m_bActive != 0)
				{
					Game1.paused = !Game1.IsMultiplayer;
					return;
				}
				Game1.paused = false;
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000A6574 File Offset: 0x000A4774
		public static void getAchievement(string achieve)
		{
			if (SteamHelper.active && SteamAPI.IsSteamRunning())
			{
				if (achieve.Equals("0"))
				{
					achieve = "a0";
				}
				try
				{
					SteamUserStats.SetAchievement(achieve);
					SteamUserStats.StoreStats();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000A65C8 File Offset: 0x000A47C8
		public static void update()
		{
			if (SteamHelper.active)
			{
				SteamAPI.RunCallbacks();
			}
		}

		// Token: 0x04000827 RID: 2087
		public static Callback<GameOverlayActivated_t> m_GameOverlayActivated;

		// Token: 0x04000828 RID: 2088
		public static bool active;

		// Token: 0x04000829 RID: 2089
		public static bool steamworksEnabled;
	}
}
