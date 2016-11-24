using System;
using System.ServiceModel;

namespace StardewValley.Network
{
	// Token: 0x0200009E RID: 158
	[ServiceContract]
	public interface IGetMapService
	{
		// Token: 0x06000B2F RID: 2863
		[OperationContract]
		GameLocation getMapFromName(string name);
	}
}
