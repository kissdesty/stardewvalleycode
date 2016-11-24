using System;
using System.Net;

namespace StardewValley.Network
{
	// Token: 0x020000A3 RID: 163
	public abstract class Client
	{
		// Token: 0x170000D1 RID: 209
		public abstract bool isConnected
		{
			// Token: 0x06000B4C RID: 2892
			get;
		}

		// Token: 0x170000D2 RID: 210
		public abstract float averageRoundtripTime
		{
			// Token: 0x06000B4D RID: 2893
			get;
		}

		// Token: 0x170000D3 RID: 211
		public abstract IPAddress serverAddress
		{
			// Token: 0x06000B4E RID: 2894
			get;
		}

		// Token: 0x06000B4F RID: 2895
		public abstract void initializeConnection(string address);

		// Token: 0x06000B50 RID: 2896
		public abstract void receiveMessages();

		// Token: 0x06000B51 RID: 2897
		public abstract void sendMessage(byte which, object[] data);

		// Token: 0x04000B55 RID: 2901
		public bool hasHandshaked;

		// Token: 0x04000B56 RID: 2902
		public string serverName = "???";
	}
}
