using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x0200000F RID: 15
	public class Model
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000260B File Offset: 0x0000080B
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00002613 File Offset: 0x00000813
		public List<string> locations { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000261C File Offset: 0x0000081C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00002624 File Offset: 0x00000824
		public List<int> frames { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000262D File Offset: 0x0000082D
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00002635 File Offset: 0x00000835
		public List<string> creatures { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000263E File Offset: 0x0000083E
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00002646 File Offset: 0x00000846
		public List<Vector2> positions { get; set; }
	}
}
