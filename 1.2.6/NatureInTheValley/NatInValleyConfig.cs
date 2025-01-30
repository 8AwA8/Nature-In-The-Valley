using System;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000010 RID: 16
	internal class NatInValleyConfig
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000264F File Offset: 0x0000084F
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00002657 File Offset: 0x00000857
		public float spawnRateMultiplier { get; set; } = 1f;

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00002660 File Offset: 0x00000860
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00002668 File Offset: 0x00000868
		public float creaturePriceMultiplier { get; set; } = 1f;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00002671 File Offset: 0x00000871
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00002679 File Offset: 0x00000879
		public float maxcreaturelLimitMultiplier { get; set; } = 1f;

		// Token: 0x060000CE RID: 206 RVA: 0x0000D0F4 File Offset: 0x0000B2F4
		public NatInValleyConfig()
		{
			this.catchingDifficultyMultiplier = 1f;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00002682 File Offset: 0x00000882
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000268A File Offset: 0x0000088A
		public bool addCreaturesToShippingCollection { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00002693 File Offset: 0x00000893
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x0000269B File Offset: 0x0000089B
		public float catchingDifficultyMultiplier { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000026A4 File Offset: 0x000008A4
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000026AC File Offset: 0x000008AC
		public KeybindList KeyForEncy { get; set; } = KeybindList.Parse("I + LeftShift");

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000026B5 File Offset: 0x000008B5
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x000026BD File Offset: 0x000008BD
		public bool useOnlyContentPacks { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000026C6 File Offset: 0x000008C6
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x000026CE File Offset: 0x000008CE
		public bool useTerrariumWallpapers { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000026D7 File Offset: 0x000008D7
		// (set) Token: 0x060000DA RID: 218 RVA: 0x000026DF File Offset: 0x000008DF
		public bool includeChargeSound { get; set; } = true;
	}
}
