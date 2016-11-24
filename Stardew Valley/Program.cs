using System;
using System.IO;

namespace StardewValley
{
	// Token: 0x02000048 RID: 72
	internal static class Program
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x0007F63C File Offset: 0x0007D83C
		private static void Main(string[] args)
		{
			Program.GameTesterMode = true;
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.handleException);
			using (Game1 game = new Game1())
			{
				Program.gamePtr = game;
				game.Run();
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0007F694 File Offset: 0x0007D894
		public static void handleException(object sender, UnhandledExceptionEventArgs args)
		{
			if (Program.handlingException || !Program.GameTesterMode)
			{
				return;
			}
			Game1.gameMode = 11;
			Program.handlingException = true;
			Exception e = (Exception)args.ExceptionObject;
			Game1.errorMessage = string.Concat(new object[]
			{
				"Message: ",
				e.Message,
				Environment.NewLine,
				"InnerException: ",
				e.InnerException,
				Environment.NewLine,
				"Stack Trace: ",
				e.StackTrace
			});
			long targetTime = DateTime.Now.Ticks / 10000L + 25000L;
			if (!Program.hasTriedToPrintLog)
			{
				Program.hasTriedToPrintLog = true;
				string filename = string.Concat(new object[]
				{
					(Game1.player != null) ? Game1.player.name : "NullPlayer",
					"_",
					Game1.uniqueIDForThisGame,
					"_",
					(int)((Game1.player != null) ? Game1.player.millisecondsPlayed : ((uint)Game1.random.Next(999999))),
					".txt"
				});
				int expr_130 = (Environment.OSVersion.Platform != PlatformID.Unix) ? 26 : 28;
				string fullFilePath = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder)expr_130), "StardewValley"), "ErrorLogs"), filename);
				FileInfo info = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder)expr_130), "StardewValley"), "ErrorLogs"), "asdfasdf"));
				if (!info.Directory.Exists)
				{
					info.Directory.Create();
				}
				if (File.Exists(fullFilePath))
				{
					File.Delete(fullFilePath);
				}
				try
				{
					File.WriteAllText(fullFilePath, Game1.errorMessage);
					Program.successfullyPrintedLog = true;
					Game1.errorMessage = string.Concat(new string[]
					{
						"(Error Report created at ",
						fullFilePath,
						")",
						Environment.NewLine,
						Game1.errorMessage
					});
				}
				catch (Exception)
				{
				}
			}
			Game1.gameMode = 3;
		}

		// Token: 0x0400064E RID: 1614
		public const int build_steam = 0;

		// Token: 0x0400064F RID: 1615
		public const int build_gog = 1;

		// Token: 0x04000650 RID: 1616
		public static bool GameTesterMode = false;

		// Token: 0x04000651 RID: 1617
		public static bool releaseBuild = true;

		// Token: 0x04000652 RID: 1618
		public static int buildType = 1;

		// Token: 0x04000653 RID: 1619
		public static Game1 gamePtr;

		// Token: 0x04000654 RID: 1620
		public static bool handlingException;

		// Token: 0x04000655 RID: 1621
		public static bool hasTriedToPrintLog;

		// Token: 0x04000656 RID: 1622
		public static bool successfullyPrintedLog;
	}
}
