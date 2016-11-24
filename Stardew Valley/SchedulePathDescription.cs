using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000046 RID: 70
	public class SchedulePathDescription
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x00074D82 File Offset: 0x00072F82
		public SchedulePathDescription(Stack<Point> route, int facingDirection, string endBehavior, string endMessage)
		{
			this.endOfRouteMessage = endMessage;
			this.route = route;
			this.facingDirection = facingDirection;
			this.endOfRouteBehavior = endBehavior;
		}

		// Token: 0x040005BA RID: 1466
		public Stack<Point> route;

		// Token: 0x040005BB RID: 1467
		public int facingDirection;

		// Token: 0x040005BC RID: 1468
		public string endOfRouteBehavior;

		// Token: 0x040005BD RID: 1469
		public string endOfRouteMessage;
	}
}
