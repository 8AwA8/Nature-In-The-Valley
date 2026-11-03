using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x02000009 RID: 9
	public interface ISpaceCoreAPI
	{
		// Token: 0x060000BA RID: 186
		string[] GetCustomSkills();

		// Token: 0x060000BB RID: 187
		int GetLevelForCustomSkill(Farmer farmer, string skill);

		// Token: 0x060000BC RID: 188
		int GetExperienceForCustomSkill(Farmer farmer, string skill);

		// Token: 0x060000BD RID: 189
		List<Tuple<string, int, int>> GetExperienceAndLevelsForCustomSkill(Farmer farmer);

		// Token: 0x060000BE RID: 190
		void AddExperienceForCustomSkill(Farmer farmer, string skill, int amt);

		// Token: 0x060000BF RID: 191
		int GetProfessionId(string skill, string profession);

		// Token: 0x060000C0 RID: 192
		void RegisterSerializerType(Type type);

		// Token: 0x060000C1 RID: 193
		void RegisterCustomProperty(Type declaringType, string name, Type propType, MethodInfo getter, MethodInfo setter);
	}
}
