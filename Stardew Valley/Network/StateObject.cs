using System;
using System.Net.Sockets;
using System.Text;

namespace StardewValley.Network
{
	// Token: 0x020000A1 RID: 161
	public class StateObject
	{
		// Token: 0x04000B4F RID: 2895
		public Socket workSocket;

		// Token: 0x04000B50 RID: 2896
		public const int BufferSize = 1024;

		// Token: 0x04000B51 RID: 2897
		public byte[] buffer = new byte[1024];

		// Token: 0x04000B52 RID: 2898
		public StringBuilder sb = new StringBuilder();
	}
}
