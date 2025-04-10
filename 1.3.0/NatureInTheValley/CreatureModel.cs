using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000018 RID: 24
	public class CreatureModel
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00002B6B File Offset: 0x00000D6B
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00002B73 File Offset: 0x00000D73
		public int rarity { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00002B7C File Offset: 0x00000D7C
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00002B84 File Offset: 0x00000D84
		public bool grounded { get; set; } = true;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00002B8D File Offset: 0x00000D8D
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00002B95 File Offset: 0x00000D95
		public float speed { get; set; } = 0.007f;

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00002B9E File Offset: 0x00000D9E
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00002BA6 File Offset: 0x00000DA6
		public int pauseTime { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00002BAF File Offset: 0x00000DAF
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00002BB7 File Offset: 0x00000DB7
		public bool doesRun { get; set; } = true;

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00002BC0 File Offset: 0x00000DC0
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public bool isMover { get; set; } = true;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00002BD1 File Offset: 0x00000DD1
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00002BD9 File Offset: 0x00000DD9
		public int range { get; set; } = 3;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00002BE2 File Offset: 0x00000DE2
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00002BEA File Offset: 0x00000DEA
		public bool dangerous { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00002BF3 File Offset: 0x00000DF3
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00002BFB File Offset: 0x00000DFB
		public List<string> seasons { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00002C04 File Offset: 0x00000E04
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00002C0C File Offset: 0x00000E0C
		public string weatherCode { get; set; } = "0";

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00002C15 File Offset: 0x00000E15
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00002C1D File Offset: 0x00000E1D
		public List<string> locations { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00002C26 File Offset: 0x00000E26
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00002C2E File Offset: 0x00000E2E
		public int minTime { get; set; } = 600;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00002C37 File Offset: 0x00000E37
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00002C3F File Offset: 0x00000E3F
		public int maxTime { get; set; } = 2600;

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00002C48 File Offset: 0x00000E48
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00002C50 File Offset: 0x00000E50
		public int frames { get; set; } = 4;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00002C59 File Offset: 0x00000E59
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00002C61 File Offset: 0x00000E61
		public string spritePath { get; set; } = "Mods\\NatureInTheValley\\Creatures\\CommonButterFly";

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00002C6A File Offset: 0x00000E6A
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00002C72 File Offset: 0x00000E72
		public int xShadow { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00002C7B File Offset: 0x00000E7B
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00002C83 File Offset: 0x00000E83
		public string localSpawnCode { get; set; } = "0";

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00002C8C File Offset: 0x00000E8C
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00002C94 File Offset: 0x00000E94
		public int yShadow { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00002C9D File Offset: 0x00000E9D
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00002CA5 File Offset: 0x00000EA5
		public int spriteIndex { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00002CAE File Offset: 0x00000EAE
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00002CB6 File Offset: 0x00000EB6
		public string itemTexture { get; set; } = "Mods\\NatureInTheValley\\Creatures\\Items";

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00002CBF File Offset: 0x00000EBF
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00002CC7 File Offset: 0x00000EC7
		public string displayName { get; set; } = "Default Creature";

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00002CD0 File Offset: 0x00000ED0
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public string displayDescription { get; set; } = "Default Desc";

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00002CE1 File Offset: 0x00000EE1
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00002CE9 File Offset: 0x00000EE9
		public int xSpriteSize { get; set; } = 32;

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00002CF2 File Offset: 0x00000EF2
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00002CFA File Offset: 0x00000EFA
		public int ySpriteSize { get; set; } = 32;

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00002D03 File Offset: 0x00000F03
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00002D0B File Offset: 0x00000F0B
		public string GSQ { get; set; } = " ";

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00002D14 File Offset: 0x00000F14
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00002D1C File Offset: 0x00000F1C
		public bool compelxAnims { get; set; }

		// Token: 0x06000171 RID: 369 RVA: 0x00016098 File Offset: 0x00014298
		public CreatureModel()
		{
			this.variantGSQs = new List<string>();
			base..ctor();
			this.displayedLocation = "";
			this.displayedLocalLocation = "";
			this.isTerrariumable = true;
			this.isDonatable = true;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00002D25 File Offset: 0x00000F25
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00002D2D File Offset: 0x00000F2D
		public float scale { get; set; } = 1f;

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00002D36 File Offset: 0x00000F36
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00002D3E File Offset: 0x00000F3E
		public int price { get; set; } = 100;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00002D47 File Offset: 0x00000F47
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00002D4F File Offset: 0x00000F4F
		public float xDef { get; set; } = 60f;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00002D58 File Offset: 0x00000F58
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00002D60 File Offset: 0x00000F60
		public float yDef { get; set; } = 15f;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00002D69 File Offset: 0x00000F69
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00002D71 File Offset: 0x00000F71
		public int packSize { get; set; } = 1;

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00002D7A File Offset: 0x00000F7A
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00002D82 File Offset: 0x00000F82
		public bool rotaryAnims { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00002D8B File Offset: 0x00000F8B
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00002D93 File Offset: 0x00000F93
		public bool complexIdling { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00002D9C File Offset: 0x00000F9C
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public bool friendlyFollower { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00002DAD File Offset: 0x00000FAD
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00002DB5 File Offset: 0x00000FB5
		public int dangerDamage { get; set; } = 25;

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00002DBE File Offset: 0x00000FBE
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00002DC6 File Offset: 0x00000FC6
		public int health { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00002DCF File Offset: 0x00000FCF
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00002DD7 File Offset: 0x00000FD7
		public bool forceSword { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00002DE0 File Offset: 0x00000FE0
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public string cueName { get; set; } = "";

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00002DF1 File Offset: 0x00000FF1
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00002DF9 File Offset: 0x00000FF9
		public List<string> variantList { get; set; } = new List<string>();

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00002E02 File Offset: 0x00001002
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00002E0A File Offset: 0x0000100A
		public float variantChance { get; set; } = 0.1f;

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00002E13 File Offset: 0x00001013
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00002E1B File Offset: 0x0000101B
		public string alternativeDrop { get; set; } = "";

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00002E24 File Offset: 0x00001024
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00002E2C File Offset: 0x0000102C
		public bool onlyAlternativeDrop { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00002E35 File Offset: 0x00001035
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00002E3D File Offset: 0x0000103D
		public bool isTerrariumable { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00002E46 File Offset: 0x00001046
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00002E4E File Offset: 0x0000104E
		public bool isDonatable { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00002E57 File Offset: 0x00001057
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00002E5F File Offset: 0x0000105F
		public float alternativeDropChance { get; set; } = 1f;

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00002E68 File Offset: 0x00001068
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00002E70 File Offset: 0x00001070
		public bool semiAquatic { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00002E79 File Offset: 0x00001079
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00002E81 File Offset: 0x00001081
		public float soundRange { get; set; } = 500f;

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00002E8A File Offset: 0x0000108A
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00002E92 File Offset: 0x00001092
		public int soundFrequency { get; set; } = 1150;

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00002E9B File Offset: 0x0000109B
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00002EA3 File Offset: 0x000010A3
		public bool useBetterCreatureBounds { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00002EAC File Offset: 0x000010AC
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00002EB4 File Offset: 0x000010B4
		public string displayedLocation { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00002EBD File Offset: 0x000010BD
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00002EC5 File Offset: 0x000010C5
		public string displayedLocalLocation { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00002ECE File Offset: 0x000010CE
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00002ED6 File Offset: 0x000010D6
		public int secondStartTime { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00002EDF File Offset: 0x000010DF
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00002EE7 File Offset: 0x000010E7
		public int secondEndTime { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00002EF0 File Offset: 0x000010F0
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00002EF8 File Offset: 0x000010F8
		public List<string> variantGSQs { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00002F01 File Offset: 0x00001101
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00002F09 File Offset: 0x00001109
		public string extraDescription { get; set; } = "";
	}
}
