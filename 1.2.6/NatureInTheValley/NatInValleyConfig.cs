using System;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000010 RID: 16
	internal class NatInValleyConfig
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00002641 File Offset: 0x00000841
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00002649 File Offset: 0x00000849
		public float spawnRateMultiplier { get; set; } = 1f;

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00002652 File Offset: 0x00000852
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000265A File Offset: 0x0000085A
		public float creaturePriceMultiplier { get; set; } = 1f;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00002663 File Offset: 0x00000863
		// (set) Token: 0x060000CA RID: 202 RVA: 0x0000266B File Offset: 0x0000086B
		public float maxcreaturelLimitMultiplier { get; set; } = 1f;

		// Token: 0x060000CB RID: 203 RVA: 0x0000C388 File Offset: 0x0000A588
		public NatInValleyConfig()
		{
			this.catchingDifficultyMultiplier = 1f;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00002674 File Offset: 0x00000874
		// (set) Token: 0x060000CD RID: 205 RVA: 0x0000267C File Offset: 0x0000087C
		public bool addCreaturesToShippingCollection { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00002685 File Offset: 0x00000885
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000268D File Offset: 0x0000088D
		public float catchingDifficultyMultiplier { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00002696 File Offset: 0x00000896
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x0000269E File Offset: 0x0000089E
		public KeybindList KeyForEncy { get; set; } = KeybindList.Parse("I + LeftShift");

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000026A7 File Offset: 0x000008A7
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000026AF File Offset: 0x000008AF
		public bool useOnlyContentPacks { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000026B8 File Offset: 0x000008B8
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x000026C0 File Offset: 0x000008C0
		public bool useTerrariumWallpapers { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000026C9 File Offset: 0x000008C9
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x000026D1 File Offset: 0x000008D1
		public bool includeChargeSound { get; set; } = true;
	}
}
