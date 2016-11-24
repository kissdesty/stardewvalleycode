using System;

namespace StardewValley
{
	// Token: 0x0200003B RID: 59
	public class PathNode
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0003C49C File Offset: 0x0003A69C
		public PathNode(int x, int y, PathNode parent)
		{
			this.x = x;
			this.y = y;
			this.parent = parent;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0003C4B9 File Offset: 0x0003A6B9
		public PathNode(int x, int y, byte g, PathNode parent)
		{
			this.x = x;
			this.y = y;
			this.g = g;
			this.parent = parent;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0003C4DE File Offset: 0x0003A6DE
		public override bool Equals(object obj)
		{
			return this.x == ((PathNode)obj).x && this.y == ((PathNode)obj).y;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0003C508 File Offset: 0x0003A708
		public override int GetHashCode()
		{
			return 100000 * this.x + this.y;
		}

		// Token: 0x04000358 RID: 856
		public int x;

		// Token: 0x04000359 RID: 857
		public int y;

		// Token: 0x0400035A RID: 858
		public byte g;

		// Token: 0x0400035B RID: 859
		public PathNode parent;
	}
}
