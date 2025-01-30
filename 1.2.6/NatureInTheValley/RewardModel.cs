using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000012 RID: 18
	public class RewardModel
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000026E8 File Offset: 0x000008E8
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000026F0 File Offset: 0x000008F0
		public string ItemId { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000026F9 File Offset: 0x000008F9
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00002701 File Offset: 0x00000901
		public int ItemCount { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000270A File Offset: 0x0000090A
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00002712 File Offset: 0x00000912
		public int TotalDonated { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000271B File Offset: 0x0000091B
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00002723 File Offset: 0x00000923
		public List<string> creatureRequirements { get; set; }
	}
}
