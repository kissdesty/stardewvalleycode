using System;
using Galaxy.Api;

namespace StardewValley
{
	// Token: 0x02000005 RID: 5
	public class GOGHelper
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000027A8 File Offset: 0x000009A8
		public static void initialize()
		{
			if (!GOGHelper.gogEnabled)
			{
				return;
			}
			try
			{
				GalaxyInstance.Init("8767653913349277", "58be5c2e55d7f535cf8c4b6bbc09d185de90b152c8c42703cc13502465f0d04a");
				GalaxyInstance.User().SignIn();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027EC File Offset: 0x000009EC
		public static void getAchievement(string achieve)
		{
			if (GOGHelper.active)
			{
				if (achieve.Equals("0"))
				{
					achieve = "a0";
				}
				GalaxyInstance.Stats().SetAchievement(achieve);
				GalaxyInstance.Stats().StoreStatsAndAchievements();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000281E File Offset: 0x00000A1E
		public static void update()
		{
			if (GOGHelper.active)
			{
				GalaxyInstance.ProcessData();
			}
		}

		// Token: 0x04000012 RID: 18
		public static bool active;

		// Token: 0x04000013 RID: 19
		public static bool gogEnabled;
	}
}
