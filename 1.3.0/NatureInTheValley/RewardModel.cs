using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000012 RID: 18
	public class RewardModel
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000028EE File Offset: 0x00000AEE
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000028F6 File Offset: 0x00000AF6
		public string ItemId { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000028FF File Offset: 0x00000AFF
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00002907 File Offset: 0x00000B07
		public int ItemCount { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00002910 File Offset: 0x00000B10
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00002918 File Offset: 0x00000B18
		public int TotalDonated { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00002921 File Offset: 0x00000B21
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00002929 File Offset: 0x00000B29
		public List<string> creatureRequirements { get; set; }
	}
}
