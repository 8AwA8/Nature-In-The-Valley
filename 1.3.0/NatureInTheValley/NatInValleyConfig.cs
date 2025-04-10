using System;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000010 RID: 16
	internal class NatInValleyConfig
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00002811 File Offset: 0x00000A11
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00002819 File Offset: 0x00000A19
		public float spawnRateMultiplier { get; set; } = 1f;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00002822 File Offset: 0x00000A22
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000282A File Offset: 0x00000A2A
		public float creaturePriceMultiplier { get; set; } = 1f;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00002833 File Offset: 0x00000A33
		// (set) Token: 0x060000EA RID: 234 RVA: 0x0000283B File Offset: 0x00000A3B
		public float maxcreaturelLimitMultiplier { get; set; } = 1f;

		// Token: 0x060000EB RID: 235 RVA: 0x00014B1C File Offset: 0x00012D1C
		public NatInValleyConfig()
		{
			this.KeyForEncy = KeybindList.Parse("I + LeftShift");
			this.includeChargeSound = true;
			this.catchingDifficultyMultiplier = 1f;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00002844 File Offset: 0x00000A44
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000284C File Offset: 0x00000A4C
		public bool addCreaturesToShippingCollection { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00002855 File Offset: 0x00000A55
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000285D File Offset: 0x00000A5D
		public float catchingDifficultyMultiplier { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00002866 File Offset: 0x00000A66
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x0000286E File Offset: 0x00000A6E
		public KeybindList KeyForEncy { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00002877 File Offset: 0x00000A77
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x0000287F File Offset: 0x00000A7F
		public bool useOnlyContentPacks { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00002888 File Offset: 0x00000A88
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00002890 File Offset: 0x00000A90
		public bool useTerrariumWallpapers { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00002899 File Offset: 0x00000A99
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000028A1 File Offset: 0x00000AA1
		public bool includeChargeSound { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000028AA File Offset: 0x00000AAA
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000028B2 File Offset: 0x00000AB2
		public float creatureRangeMultiplier { get; set; } = 1f;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000028BB File Offset: 0x00000ABB
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000028C3 File Offset: 0x00000AC3
		public float creatureSizeMultiplier { get; set; } = 1f;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000028CC File Offset: 0x00000ACC
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000028D4 File Offset: 0x00000AD4
		public bool boolDisableShop { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000028DD File Offset: 0x00000ADD
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000028E5 File Offset: 0x00000AE5
		public float creatureDamageModifier { get; set; } = 1f;
	}
}
