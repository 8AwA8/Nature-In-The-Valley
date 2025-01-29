using System;
using System.Collections.Generic;

namespace NatureInTheValley
{
	// Token: 0x02000018 RID: 24
	public class CreatureModel
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000028FE File Offset: 0x00000AFE
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00002906 File Offset: 0x00000B06
		public int rarity { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000290F File Offset: 0x00000B0F
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00002917 File Offset: 0x00000B17
		public bool grounded { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00002920 File Offset: 0x00000B20
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00002928 File Offset: 0x00000B28
		public float speed { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00002931 File Offset: 0x00000B31
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00002939 File Offset: 0x00000B39
		public int pauseTime { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00002942 File Offset: 0x00000B42
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000294A File Offset: 0x00000B4A
		public bool doesRun { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00002953 File Offset: 0x00000B53
		// (set) Token: 0x0600011D RID: 285 RVA: 0x0000295B File Offset: 0x00000B5B
		public bool isMover { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00002964 File Offset: 0x00000B64
		// (set) Token: 0x0600011F RID: 287 RVA: 0x0000296C File Offset: 0x00000B6C
		public int range { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00002975 File Offset: 0x00000B75
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000297D File Offset: 0x00000B7D
		public bool dangerous { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00002986 File Offset: 0x00000B86
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000298E File Offset: 0x00000B8E
		public List<string> seasons { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00002997 File Offset: 0x00000B97
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000299F File Offset: 0x00000B9F
		public string weatherCode { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000029A8 File Offset: 0x00000BA8
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000029B0 File Offset: 0x00000BB0
		public List<string> locations { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000029B9 File Offset: 0x00000BB9
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000029C1 File Offset: 0x00000BC1
		public int minTime { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000029CA File Offset: 0x00000BCA
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000029D2 File Offset: 0x00000BD2
		public int maxTime { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000029DB File Offset: 0x00000BDB
		// (set) Token: 0x0600012D RID: 301 RVA: 0x000029E3 File Offset: 0x00000BE3
		public int frames { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600012E RID: 302 RVA: 0x000029EC File Offset: 0x00000BEC
		// (set) Token: 0x0600012F RID: 303 RVA: 0x000029F4 File Offset: 0x00000BF4
		public string spritePath { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000130 RID: 304 RVA: 0x000029FD File Offset: 0x00000BFD
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00002A05 File Offset: 0x00000C05
		public int xShadow { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00002A0E File Offset: 0x00000C0E
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00002A16 File Offset: 0x00000C16
		public string localSpawnCode { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00002A1F File Offset: 0x00000C1F
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00002A27 File Offset: 0x00000C27
		public int yShadow { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00002A30 File Offset: 0x00000C30
		// (set) Token: 0x06000137 RID: 311 RVA: 0x00002A38 File Offset: 0x00000C38
		public int spriteIndex { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00002A41 File Offset: 0x00000C41
		// (set) Token: 0x06000139 RID: 313 RVA: 0x00002A49 File Offset: 0x00000C49
		public string itemTexture { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00002A52 File Offset: 0x00000C52
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00002A5A File Offset: 0x00000C5A
		public string displayName { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00002A63 File Offset: 0x00000C63
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00002A6B File Offset: 0x00000C6B
		public string displayDescription { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00002A74 File Offset: 0x00000C74
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00002A7C File Offset: 0x00000C7C
		public int xSpriteSize { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00002A85 File Offset: 0x00000C85
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00002A8D File Offset: 0x00000C8D
		public int ySpriteSize { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00002A96 File Offset: 0x00000C96
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00002A9E File Offset: 0x00000C9E
		public string GSQ { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00002AA7 File Offset: 0x00000CA7
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00002AAF File Offset: 0x00000CAF
		public bool compelxAnims { get; set; }

		// Token: 0x06000146 RID: 326 RVA: 0x00002425 File Offset: 0x00000625
		public CreatureModel()
		{
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00002AB8 File Offset: 0x00000CB8
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00002AC0 File Offset: 0x00000CC0
		public float scale { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00002AC9 File Offset: 0x00000CC9
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public int price { get; set; }

		// Token: 0x0600014B RID: 331 RVA: 0x0000E350 File Offset: 0x0000C550
		public CreatureModel(int rarity, bool grounded, float speed, int pauseTime, float scale, bool doesRun, bool isMover, int range, bool dangerous, List<string> seasons, string weatherCode, List<string> locations, int minTime, int maxTime, int frames, string spritePath, int xShadow, string localSpawnCode, int yShadow, int price, int spriteIndex, float xDef, float yDef, string itemTexture, string displayName, string displayDescription, int xSpriteSize, int ySpriteSize, string GSQ, bool compelxAnims)
		{
			this.rarity = rarity;
			this.grounded = grounded;
			this.speed = speed;
			this.pauseTime = pauseTime;
			this.scale = scale;
			this.doesRun = doesRun;
			this.isMover = isMover;
			this.range = range;
			this.dangerous = dangerous;
			this.seasons = seasons;
			this.weatherCode = weatherCode;
			this.locations = locations;
			this.minTime = minTime;
			this.maxTime = maxTime;
			this.frames = frames;
			this.spritePath = spritePath;
			this.xShadow = xShadow;
			this.localSpawnCode = localSpawnCode;
			this.yShadow = yShadow;
			this.price = price;
			this.spriteIndex = spriteIndex;
			this.xDef = xDef;
			this.yDef = yDef;
			this.itemTexture = itemTexture;
			this.displayName = displayName;
			this.displayDescription = displayDescription;
			this.xSpriteSize = xSpriteSize;
			this.ySpriteSize = ySpriteSize;
			this.GSQ = GSQ;
			this.compelxAnims = compelxAnims;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00002ADA File Offset: 0x00000CDA
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00002AE2 File Offset: 0x00000CE2
		public float xDef { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00002AEB File Offset: 0x00000CEB
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00002AF3 File Offset: 0x00000CF3
		public float yDef { get; set; }
	}
}
