using System;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x02000013 RID: 19
	public interface IToolbarIconsApi
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600010E RID: 270
		// (remove) Token: 0x0600010F RID: 271
		event EventHandler<string> ToolbarIconPressed;

		// Token: 0x06000110 RID: 272
		void AddToolbarIcon(string id, string texturePath, Rectangle? sourceRect, string hoverText);

		// Token: 0x06000111 RID: 273
		void RemoveToolbarIcon(string id);
	}
}
