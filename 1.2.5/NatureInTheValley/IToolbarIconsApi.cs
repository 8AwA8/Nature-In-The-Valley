using System;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x02000013 RID: 19
	public interface IToolbarIconsApi
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000E3 RID: 227
		// (remove) Token: 0x060000E4 RID: 228
		event EventHandler<string> ToolbarIconPressed;

		// Token: 0x060000E5 RID: 229
		void AddToolbarIcon(string id, string texturePath, Rectangle? sourceRect, string hoverText);

		// Token: 0x060000E6 RID: 230
		void RemoveToolbarIcon(string id);
	}
}
