using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000012 RID: 18
	public class RewardModel
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00002839 File Offset: 0x00000A39
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00002841 File Offset: 0x00000A41
		public string ItemId { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000284A File Offset: 0x00000A4A
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00002852 File Offset: 0x00000A52
		public int ItemCount { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000285B File Offset: 0x00000A5B
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00002863 File Offset: 0x00000A63
		public int TotalDonated { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000286C File Offset: 0x00000A6C
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00002874 File Offset: 0x00000A74
		public List<string> creatureRequirements { get; set; }
	}
}
