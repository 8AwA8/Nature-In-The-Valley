using System;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000010 RID: 16
	internal class NatInValleyConfig
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000DC RID: 220 RVA: 0x0000277E File Offset: 0x0000097E
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00002786 File Offset: 0x00000986
		public float spawnRateMultiplier { get; set; } = 1f;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000DE RID: 222 RVA: 0x0000278F File Offset: 0x0000098F
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00002797 File Offset: 0x00000997
		public float creaturePriceMultiplier { get; set; } = 1f;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000027A0 File Offset: 0x000009A0
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000027A8 File Offset: 0x000009A8
		public float maxcreaturelLimitMultiplier { get; set; } = 1f;

		// Token: 0x060000E2 RID: 226 RVA: 0x00012B24 File Offset: 0x00010D24
		public NatInValleyConfig()
		{
			this.KeyForEncy = KeybindList.Parse("I + LeftShift");
			this.includeChargeSound = true;
			this.catchingDifficultyMultiplier = 1f;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000027B1 File Offset: 0x000009B1
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000027B9 File Offset: 0x000009B9
		public bool addCreaturesToShippingCollection { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000027C2 File Offset: 0x000009C2
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000027CA File Offset: 0x000009CA
		public float catchingDifficultyMultiplier { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000027D3 File Offset: 0x000009D3
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000027DB File Offset: 0x000009DB
		public KeybindList KeyForEncy { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000027E4 File Offset: 0x000009E4
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000027EC File Offset: 0x000009EC
		public bool useOnlyContentPacks { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000027F5 File Offset: 0x000009F5
		// (set) Token: 0x060000EC RID: 236 RVA: 0x000027FD File Offset: 0x000009FD
		public bool useTerrariumWallpapers { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00002806 File Offset: 0x00000A06
		// (set) Token: 0x060000EE RID: 238 RVA: 0x0000280E File Offset: 0x00000A0E
		public bool includeChargeSound { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00002817 File Offset: 0x00000A17
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000281F File Offset: 0x00000A1F
		public float creatureRangeMultiplier { get; set; } = 1f;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00002828 File Offset: 0x00000A28
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00002830 File Offset: 0x00000A30
		public float creatureSizeMultiplier { get; set; } = 1f;
	}
}
