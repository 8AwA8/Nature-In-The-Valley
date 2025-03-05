using System;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace NatureInTheValley
{
	// Token: 0x02000011 RID: 17
	public interface IGenericModConfigMenuApi
	{
		// Token: 0x060000F3 RID: 243
		void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);

		// Token: 0x060000F4 RID: 244
		void Unregister(IManifest mod);

		// Token: 0x060000F5 RID: 245
		void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, Func<float, string> formatValue = null, string fieldId = null);

		// Token: 0x060000F6 RID: 246
		void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);

		// Token: 0x060000F7 RID: 247
		void AddKeybindList(IManifest mod, Func<KeybindList> getValue, Action<KeybindList> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
	}
}
