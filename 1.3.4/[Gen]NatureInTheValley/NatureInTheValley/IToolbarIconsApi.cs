using System;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x02000013 RID: 19
	public interface IToolbarIconsApi
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000114 RID: 276
		// (remove) Token: 0x06000115 RID: 277
		event EventHandler<string> ToolbarIconPressed;

		// Token: 0x06000116 RID: 278
		void AddToolbarIcon(string id, string texturePath, Rectangle? sourceRect, string hoverText);

		// Token: 0x06000117 RID: 279
		void RemoveToolbarIcon(string id);
	}
}
