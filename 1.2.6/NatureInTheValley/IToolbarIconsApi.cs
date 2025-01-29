using System;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x02000013 RID: 19
	public interface IToolbarIconsApi
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000E6 RID: 230
		// (remove) Token: 0x060000E7 RID: 231
		event EventHandler<string> ToolbarIconPressed;

		// Token: 0x060000E8 RID: 232
		void AddToolbarIcon(string id, string texturePath, Rectangle? sourceRect, string hoverText);

		// Token: 0x060000E9 RID: 233
		void RemoveToolbarIcon(string id);
	}
}
