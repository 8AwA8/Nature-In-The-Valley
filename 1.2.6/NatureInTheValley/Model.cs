using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x0200000F RID: 15
	public class Model
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000025FD File Offset: 0x000007FD
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00002605 File Offset: 0x00000805
		public List<string> locations { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000260E File Offset: 0x0000080E
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00002616 File Offset: 0x00000816
		public List<int> frames { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000261F File Offset: 0x0000081F
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00002627 File Offset: 0x00000827
		public List<string> creatures { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00002630 File Offset: 0x00000830
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00002638 File Offset: 0x00000838
		public List<Vector2> positions { get; set; }
	}
}
