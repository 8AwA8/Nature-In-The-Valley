using System;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x02000013 RID: 19
	public interface IToolbarIconsApi
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000101 RID: 257
		// (remove) Token: 0x06000102 RID: 258
		event EventHandler<string> ToolbarIconPressed;

		// Token: 0x06000103 RID: 259
		void AddToolbarIcon(string id, string texturePath, Rectangle? sourceRect, string hoverText);

		// Token: 0x06000104 RID: 260
		void RemoveToolbarIcon(string id);
	}
}
