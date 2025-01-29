using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000012 RID: 18
	public class RewardModel
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000026DA File Offset: 0x000008DA
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000026E2 File Offset: 0x000008E2
		public string ItemId { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000026EB File Offset: 0x000008EB
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000026F3 File Offset: 0x000008F3
		public int ItemCount { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000026FC File Offset: 0x000008FC
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00002704 File Offset: 0x00000904
		public int TotalDonated { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000270D File Offset: 0x0000090D
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00002715 File Offset: 0x00000915
		public List<string> creatureRequirements { get; set; }
	}
}
