using System;

namespace StardewValley
{
	// Token: 0x0200003E RID: 62
	public class NPCDialogueResponse : Response
	{
		// Token: 0x060002FC RID: 764 RVA: 0x0003C6E3 File Offset: 0x0003A8E3
		public NPCDialogueResponse(int id, int friendshipChange, string keyToNPCresponse, string responseText) : base(keyToNPCresponse, responseText)
		{
			this.friendshipChange = friendshipChange;
			this.id = id;
		}

		// Token: 0x04000360 RID: 864
		public int friendshipChange;

		// Token: 0x04000361 RID: 865
		public int id;
	}
}
