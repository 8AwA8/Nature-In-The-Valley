using System;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000011 RID: 17
	public interface IGenericModConfigMenuApi
	{
		// Token: 0x060000D8 RID: 216
		void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);

		// Token: 0x060000D9 RID: 217
		void Unregister(IManifest mod);

		// Token: 0x060000DA RID: 218
		void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, Func<float, string> formatValue = null, string fieldId = null);

		// Token: 0x060000DB RID: 219
		void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);

		// Token: 0x060000DC RID: 220
		void AddKeybindList(IManifest mod, Func<KeybindList> getValue, Action<KeybindList> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
	}
}
