using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x0200000F RID: 15
	public class Model
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000025C1 File Offset: 0x000007C1
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000025C9 File Offset: 0x000007C9
		public List<string> locations { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000025D2 File Offset: 0x000007D2
		// (set) Token: 0x060000BC RID: 188 RVA: 0x000025DA File Offset: 0x000007DA
		public List<int> frames { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000025E3 File Offset: 0x000007E3
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000025EB File Offset: 0x000007EB
		public List<string> creatures { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000025F4 File Offset: 0x000007F4
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000025FC File Offset: 0x000007FC
		public List<Vector2> positions { get; set; }
	}
}
