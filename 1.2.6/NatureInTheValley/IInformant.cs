using System;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace NatureInTheValley
{
	// Token: 0x02000019 RID: 25
	public interface IInformant
	{
		// Token: 0x06000157 RID: 343
		void AddTerrainFeatureTooltipGenerator(string id, Func<string> displayName, Func<string> description, Func<TerrainFeature, string> generator);

		// Token: 0x06000158 RID: 344
		void AddObjectTooltipGenerator(string id, Func<string> displayName, Func<string> description, Func<StardewValley.Object, string> generator);

		// Token: 0x06000159 RID: 345
		void AddItemDecorator(string id, Func<string> displayName, Func<string> description, Func<Item, Texture2D> decorator);
	}
}
