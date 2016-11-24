using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Network
{
	// Token: 0x020000A4 RID: 164
	public abstract class Server
	{
		// Token: 0x06000B53 RID: 2899 RVA: 0x000E48AA File Offset: 0x000E2AAA
		public Server(string name)
		{
			this.serverName = name;
		}

		// Token: 0x170000D4 RID: 212
		public abstract int connectionsCount
		{
			// Token: 0x06000B54 RID: 2900
			get;
		}

		// Token: 0x06000B55 RID: 2901
		public abstract void initializeConnection();

		// Token: 0x06000B56 RID: 2902
		public abstract void stopServer();

		// Token: 0x06000B57 RID: 2903
		public abstract void receiveMessages();

		// Token: 0x06000B58 RID: 2904
		public abstract void sendMessages(GameTime time);

		// Token: 0x04000B57 RID: 2903
		public const int messageSendDelay = 50;

		// Token: 0x04000B58 RID: 2904
		public const int defaultPort = 24642;

		// Token: 0x04000B59 RID: 2905
		public const int defaultMapServerPort = 24643;

		// Token: 0x04000B5A RID: 2906
		protected string serverName;
	}
}
