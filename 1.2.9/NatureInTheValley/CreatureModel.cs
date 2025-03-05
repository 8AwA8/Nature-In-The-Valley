using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000018 RID: 24
	public class CreatureModel
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00002AB6 File Offset: 0x00000CB6
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00002ABE File Offset: 0x00000CBE
		public int rarity { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00002AC7 File Offset: 0x00000CC7
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00002ACF File Offset: 0x00000CCF
		public bool grounded { get; set; } = true;

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00002AD8 File Offset: 0x00000CD8
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public float speed { get; set; } = 0.007f;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00002AE9 File Offset: 0x00000CE9
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00002AF1 File Offset: 0x00000CF1
		public int pauseTime { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00002AFA File Offset: 0x00000CFA
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00002B02 File Offset: 0x00000D02
		public bool doesRun { get; set; } = true;

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00002B0B File Offset: 0x00000D0B
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00002B13 File Offset: 0x00000D13
		public bool isMover { get; set; } = true;

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00002B1C File Offset: 0x00000D1C
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00002B24 File Offset: 0x00000D24
		public int range { get; set; } = 3;

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00002B2D File Offset: 0x00000D2D
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00002B35 File Offset: 0x00000D35
		public bool dangerous { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00002B3E File Offset: 0x00000D3E
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00002B46 File Offset: 0x00000D46
		public List<string> seasons { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00002B4F File Offset: 0x00000D4F
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00002B57 File Offset: 0x00000D57
		public string weatherCode { get; set; } = "0";

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00002B60 File Offset: 0x00000D60
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00002B68 File Offset: 0x00000D68
		public List<string> locations { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00002B71 File Offset: 0x00000D71
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00002B79 File Offset: 0x00000D79
		public int minTime { get; set; } = 600;

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00002B82 File Offset: 0x00000D82
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00002B8A File Offset: 0x00000D8A
		public int maxTime { get; set; } = 2600;

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00002B93 File Offset: 0x00000D93
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00002B9B File Offset: 0x00000D9B
		public int frames { get; set; } = 4;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00002BA4 File Offset: 0x00000DA4
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00002BAC File Offset: 0x00000DAC
		public string spritePath { get; set; } = "Mods\\NatureInTheValley\\Creatures\\CommonButterFly";

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00002BB5 File Offset: 0x00000DB5
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00002BBD File Offset: 0x00000DBD
		public int xShadow { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00002BC6 File Offset: 0x00000DC6
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00002BCE File Offset: 0x00000DCE
		public string localSpawnCode { get; set; } = "0";

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00002BD7 File Offset: 0x00000DD7
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00002BDF File Offset: 0x00000DDF
		public int yShadow { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00002BE8 File Offset: 0x00000DE8
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public int spriteIndex { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00002BF9 File Offset: 0x00000DF9
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00002C01 File Offset: 0x00000E01
		public string itemTexture { get; set; } = "Mods\\NatureInTheValley\\Creatures\\Items";

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00002C0A File Offset: 0x00000E0A
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00002C12 File Offset: 0x00000E12
		public string displayName { get; set; } = "Default Creature";

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00002C1B File Offset: 0x00000E1B
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00002C23 File Offset: 0x00000E23
		public string displayDescription { get; set; } = "Default Desc";

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00002C2C File Offset: 0x00000E2C
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00002C34 File Offset: 0x00000E34
		public int xSpriteSize { get; set; } = 32;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00002C3D File Offset: 0x00000E3D
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00002C45 File Offset: 0x00000E45
		public int ySpriteSize { get; set; } = 32;

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00002C4E File Offset: 0x00000E4E
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00002C56 File Offset: 0x00000E56
		public string GSQ { get; set; } = " ";

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00002C5F File Offset: 0x00000E5F
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00002C67 File Offset: 0x00000E67
		public bool compelxAnims { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00002C70 File Offset: 0x00000E70
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00002C78 File Offset: 0x00000E78
		public float scale { get; set; } = 1f;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00002C81 File Offset: 0x00000E81
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00002C89 File Offset: 0x00000E89
		public int price { get; set; } = 100;

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00002C92 File Offset: 0x00000E92
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00002C9A File Offset: 0x00000E9A
		public float xDef { get; set; } = 60f;

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00002CA3 File Offset: 0x00000EA3
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00002CAB File Offset: 0x00000EAB
		public float yDef { get; set; } = 15f;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00002CB4 File Offset: 0x00000EB4
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00002CBC File Offset: 0x00000EBC
		public int packSize { get; set; } = 1;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00002CC5 File Offset: 0x00000EC5
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00002CCD File Offset: 0x00000ECD
		public bool rotaryAnims { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00002CD6 File Offset: 0x00000ED6
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00002CDE File Offset: 0x00000EDE
		public bool complexIdling { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00002CE7 File Offset: 0x00000EE7
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00002CEF File Offset: 0x00000EEF
		public bool friendlyFollower { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00002CF8 File Offset: 0x00000EF8
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00002D00 File Offset: 0x00000F00
		public int dangerDamage { get; set; } = 25;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00002D09 File Offset: 0x00000F09
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00002D11 File Offset: 0x00000F11
		public int health { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00002D1A File Offset: 0x00000F1A
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00002D22 File Offset: 0x00000F22
		public bool forceSword { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00002D2B File Offset: 0x00000F2B
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00002D33 File Offset: 0x00000F33
		public string cueName { get; set; } = "";

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00002D3C File Offset: 0x00000F3C
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00002D44 File Offset: 0x00000F44
		public List<string> variantList { get; set; } = new List<string>();

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00002D4D File Offset: 0x00000F4D
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00002D55 File Offset: 0x00000F55
		public float variantChance { get; set; } = 0.1f;
	}
}
