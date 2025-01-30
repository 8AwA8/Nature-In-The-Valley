using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000018 RID: 24
	public class CreatureModel
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00002948 File Offset: 0x00000B48
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00002950 File Offset: 0x00000B50
		public int rarity { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00002959 File Offset: 0x00000B59
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00002961 File Offset: 0x00000B61
		public bool grounded { get; set; } = true;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000296A File Offset: 0x00000B6A
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00002972 File Offset: 0x00000B72
		public float speed { get; set; } = 0.007f;

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000297B File Offset: 0x00000B7B
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00002983 File Offset: 0x00000B83
		public int pauseTime { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000298C File Offset: 0x00000B8C
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00002994 File Offset: 0x00000B94
		public bool doesRun { get; set; } = true;

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000299D File Offset: 0x00000B9D
		// (set) Token: 0x06000123 RID: 291 RVA: 0x000029A5 File Offset: 0x00000BA5
		public bool isMover { get; set; } = true;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000029AE File Offset: 0x00000BAE
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000029B6 File Offset: 0x00000BB6
		public int range { get; set; } = 3;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000029BF File Offset: 0x00000BBF
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000029C7 File Offset: 0x00000BC7
		public bool dangerous { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000029D0 File Offset: 0x00000BD0
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000029D8 File Offset: 0x00000BD8
		public List<string> seasons { get; set; } = new List<string>
		{
			"spring",
			"summer",
			"fall",
			"winter"
		};

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000029E1 File Offset: 0x00000BE1
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000029E9 File Offset: 0x00000BE9
		public string weatherCode { get; set; } = "0";

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000029F2 File Offset: 0x00000BF2
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000029FA File Offset: 0x00000BFA
		public List<string> locations { get; set; } = new List<string>
		{
			"0",
			"0"
		};

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00002A03 File Offset: 0x00000C03
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00002A0B File Offset: 0x00000C0B
		public int minTime { get; set; } = 600;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00002A14 File Offset: 0x00000C14
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00002A1C File Offset: 0x00000C1C
		public int maxTime { get; set; } = 2600;

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00002A25 File Offset: 0x00000C25
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00002A2D File Offset: 0x00000C2D
		public int frames { get; set; } = 4;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00002A36 File Offset: 0x00000C36
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00002A3E File Offset: 0x00000C3E
		public string spritePath { get; set; } = "Mods\\NatureInTheValley\\Creatures\\CommonButterFly";

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00002A47 File Offset: 0x00000C47
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00002A4F File Offset: 0x00000C4F
		public int xShadow { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00002A58 File Offset: 0x00000C58
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00002A60 File Offset: 0x00000C60
		public string localSpawnCode { get; set; } = "0";

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00002A69 File Offset: 0x00000C69
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00002A71 File Offset: 0x00000C71
		public int yShadow { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00002A7A File Offset: 0x00000C7A
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00002A82 File Offset: 0x00000C82
		public int spriteIndex { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00002A8B File Offset: 0x00000C8B
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00002A93 File Offset: 0x00000C93
		public string itemTexture { get; set; } = "Mods\\NatureInTheValley\\Creatures\\Items";

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00002A9C File Offset: 0x00000C9C
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00002AA4 File Offset: 0x00000CA4
		public string displayName { get; set; } = "Default Creature";

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00002AAD File Offset: 0x00000CAD
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00002AB5 File Offset: 0x00000CB5
		public string displayDescription { get; set; } = "Default Desc";

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00002ABE File Offset: 0x00000CBE
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00002AC6 File Offset: 0x00000CC6
		public int xSpriteSize { get; set; } = 32;

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00002ACF File Offset: 0x00000CCF
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00002AD7 File Offset: 0x00000CD7
		public int ySpriteSize { get; set; } = 32;

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00002AE0 File Offset: 0x00000CE0
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public string GSQ { get; set; } = " ";

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00002AF1 File Offset: 0x00000CF1
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00002AF9 File Offset: 0x00000CF9
		public bool compelxAnims { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00002B02 File Offset: 0x00000D02
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00002B0A File Offset: 0x00000D0A
		public float scale { get; set; } = 1f;

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00002B13 File Offset: 0x00000D13
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00002B1B File Offset: 0x00000D1B
		public int price { get; set; } = 100;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00002B24 File Offset: 0x00000D24
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00002B2C File Offset: 0x00000D2C
		public float xDef { get; set; } = 60f;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00002B35 File Offset: 0x00000D35
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00002B3D File Offset: 0x00000D3D
		public float yDef { get; set; } = 15f;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00002B46 File Offset: 0x00000D46
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00002B4E File Offset: 0x00000D4E
		public int packSize { get; set; } = 1;
	}
}
