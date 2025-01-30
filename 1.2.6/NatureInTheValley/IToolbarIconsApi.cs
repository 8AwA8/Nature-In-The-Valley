using System;
using Microsoft.Xna.Framework;

namespace NatureInTheValley
{
	// Token: 0x02000013 RID: 19
	public interface IToolbarIconsApi
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000E9 RID: 233
		// (remove) Token: 0x060000EA RID: 234
		event EventHandler<string> ToolbarIconPressed;

		// Token: 0x060000EB RID: 235
		void AddToolbarIcon(string id, string texturePath, Rectangle? sourceRect, string hoverText);

		// Token: 0x060000EC RID: 236
		void RemoveToolbarIcon(string id);
	}
}
