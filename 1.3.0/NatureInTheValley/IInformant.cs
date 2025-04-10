using System;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace NatureInTheValley
{
	// Token: 0x02000019 RID: 25
	public interface IInformant
	{
		// Token: 0x060001AC RID: 428
		void AddTerrainFeatureTooltipGenerator(string id, Func<string> displayName, Func<string> description, Func<TerrainFeature, string> generator);

		// Token: 0x060001AD RID: 429
		void AddObjectTooltipGenerator(string id, Func<string> displayName, Func<string> description, Func<StardewValley.Object, string> generator);

		// Token: 0x060001AE RID: 430
		void AddItemDecorator(string id, Func<string> displayName, Func<string> description, Func<Item, Texture2D> decorator);
	}
}
