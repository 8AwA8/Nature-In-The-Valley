using System;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000011 RID: 17
	public interface IGenericModConfigMenuApi
	{
		// Token: 0x06000100 RID: 256
		void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);

		// Token: 0x06000101 RID: 257
		void Unregister(IManifest mod);

		// Token: 0x06000102 RID: 258
		void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, Func<float, string> formatValue = null, string fieldId = null);

		// Token: 0x06000103 RID: 259
		void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);

		// Token: 0x06000104 RID: 260
		void AddKeybindList(IManifest mod, Func<KeybindList> getValue, Action<KeybindList> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
	}
}
