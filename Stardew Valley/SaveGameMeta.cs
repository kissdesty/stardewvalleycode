using System;

namespace StardewValley
{
	// Token: 0x0200001D RID: 29
	internal struct SaveGameMeta : IComparable
	{
		// Token: 0x06000134 RID: 308 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
		public SaveGameMeta(string name, int year, string season, int day, int money, int level, int saveTime, string lastSaveDate, string lastSaveTime, string filename)
		{
			this.name = name;
			this.season = season;
			this.money = money;
			this.year = year;
			this.day = day;
			this.filename = filename;
			this.level = level;
			this.saveTime = saveTime;
			this.lastSaveDate = lastSaveDate;
			this.lastSaveTime = lastSaveTime;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000CC52 File Offset: 0x0000AE52
		public int CompareTo(object obj)
		{
			return ((SaveGameMeta)obj).saveTime - this.saveTime;
		}

		// Token: 0x04000143 RID: 323
		public string name;

		// Token: 0x04000144 RID: 324
		public string season;

		// Token: 0x04000145 RID: 325
		public string filename;

		// Token: 0x04000146 RID: 326
		public string lastSaveDate;

		// Token: 0x04000147 RID: 327
		public string lastSaveTime;

		// Token: 0x04000148 RID: 328
		public int money;

		// Token: 0x04000149 RID: 329
		public int year;

		// Token: 0x0400014A RID: 330
		public int day;

		// Token: 0x0400014B RID: 331
		public int level;

		// Token: 0x0400014C RID: 332
		public int saveTime;
	}
}
