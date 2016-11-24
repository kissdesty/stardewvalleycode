using System;

namespace StardewValley
{
	// Token: 0x0200003D RID: 61
	public class Response
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0003C6CD File Offset: 0x0003A8CD
		public Response(string responseKey, string responseText)
		{
			this.responseKey = responseKey;
			this.responseText = responseText;
		}

		// Token: 0x0400035E RID: 862
		public string responseKey;

		// Token: 0x0400035F RID: 863
		public string responseText;
	}
}
