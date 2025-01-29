using System;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000010 RID: 16
	internal class NatInValleyConfig
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00002605 File Offset: 0x00000805
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x0000260D File Offset: 0x0000080D
		public float spawnRateMultiplier { get; set; } = 1f;

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00002616 File Offset: 0x00000816
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x0000261E File Offset: 0x0000081E
		public float creaturePriceMultiplier { get; set; } = 1f;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00002627 File Offset: 0x00000827
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x0000262F File Offset: 0x0000082F
		public float maxcreaturelLimitMultiplier { get; set; } = 1f;

		// Token: 0x060000C8 RID: 200 RVA: 0x0000CECC File Offset: 0x0000B0CC
		public NatInValleyConfig()
		{
			this.catchingDifficultyMultiplier = 1f;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00002638 File Offset: 0x00000838
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00002640 File Offset: 0x00000840
		public bool addCreaturesToShippingCollection { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00002649 File Offset: 0x00000849
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00002651 File Offset: 0x00000851
		public float catchingDifficultyMultiplier { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000265A File Offset: 0x0000085A
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00002662 File Offset: 0x00000862
		public KeybindList KeyForEncy { get; set; } = KeybindList.Parse("I + LeftShift");

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000266B File Offset: 0x0000086B
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00002673 File Offset: 0x00000873
		public bool useOnlyContentPacks { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000267C File Offset: 0x0000087C
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00002684 File Offset: 0x00000884
		public bool useTerrariumWallpapers { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000268D File Offset: 0x0000088D
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00002695 File Offset: 0x00000895
		public bool includeChargeSound { get; set; } = true;
	}
}
