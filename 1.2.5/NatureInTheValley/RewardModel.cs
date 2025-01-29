using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000012 RID: 18
	public class RewardModel
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000269E File Offset: 0x0000089E
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000026A6 File Offset: 0x000008A6
		public string ItemId { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000026AF File Offset: 0x000008AF
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000026B7 File Offset: 0x000008B7
		public int ItemCount { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000026C0 File Offset: 0x000008C0
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000026C8 File Offset: 0x000008C8
		public int TotalDonated { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000026D1 File Offset: 0x000008D1
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000026D9 File Offset: 0x000008D9
		public List<string> creatureRequirements { get; set; }
	}
}
