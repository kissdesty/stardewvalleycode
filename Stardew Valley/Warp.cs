using System;

namespace StardewValley
{
	// Token: 0x02000056 RID: 86
	public class Warp
	{
		// Token: 0x170000B7 RID: 183
		public int X
		{
			// Token: 0x060008AF RID: 2223 RVA: 0x000BA5EE File Offset: 0x000B87EE
			get
			{
				return this.x;
			}
		}

		// Token: 0x170000B8 RID: 184
		public int Y
		{
			// Token: 0x060008B0 RID: 2224 RVA: 0x000BA5F6 File Offset: 0x000B87F6
			get
			{
				return this.y;
			}
		}

		// Token: 0x170000B9 RID: 185
		public int TargetX
		{
			// Token: 0x060008B1 RID: 2225 RVA: 0x000BA5FE File Offset: 0x000B87FE
			get
			{
				return this.targetX;
			}
			// Token: 0x060008B2 RID: 2226 RVA: 0x000BA606 File Offset: 0x000B8806
			set
			{
				this.targetX = value;
			}
		}

		// Token: 0x170000BA RID: 186
		public int TargetY
		{
			// Token: 0x060008B3 RID: 2227 RVA: 0x000BA60F File Offset: 0x000B880F
			get
			{
				return this.targetY;
			}
			// Token: 0x060008B4 RID: 2228 RVA: 0x000BA617 File Offset: 0x000B8817
			set
			{
				this.targetY = value;
			}
		}

		// Token: 0x170000BB RID: 187
		public string TargetName
		{
			// Token: 0x060008B5 RID: 2229 RVA: 0x000BA620 File Offset: 0x000B8820
			get
			{
				return this.targetName;
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000BA628 File Offset: 0x000B8828
		public Warp(int x, int y, string targetName, int targetX, int targetY, bool flipFarmer)
		{
			this.x = x;
			this.y = y;
			this.targetX = targetX;
			this.targetY = targetY;
			this.targetName = targetName;
			this.flipFarmer = flipFarmer;
		}

		// Token: 0x040008A8 RID: 2216
		private int x;

		// Token: 0x040008A9 RID: 2217
		private int y;

		// Token: 0x040008AA RID: 2218
		private int targetX;

		// Token: 0x040008AB RID: 2219
		private int targetY;

		// Token: 0x040008AC RID: 2220
		public bool flipFarmer;

		// Token: 0x040008AD RID: 2221
		private string targetName;
	}
}
