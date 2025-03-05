using System;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace NatureInTheValley
{
	// Token: 0x02000019 RID: 25
	public interface IInformant
	{
		// Token: 0x06000181 RID: 385
		void AddTerrainFeatureTooltipGenerator(string id, Func<string> displayName, Func<string> description, Func<TerrainFeature, string> generator);

		// Token: 0x06000182 RID: 386
		void AddObjectTooltipGenerator(string id, Func<string> displayName, Func<string> description, Func<StardewValley.Object, string> generator);

		// Token: 0x06000183 RID: 387
		void AddItemDecorator(string id, Func<string> displayName, Func<string> description, Func<Item, Texture2D> decorator);
	}
}
