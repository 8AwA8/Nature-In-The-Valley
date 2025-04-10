using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x02000009 RID: 9
	public interface ISpaceCoreAPI
	{
		// Token: 0x060000B4 RID: 180
		string[] GetCustomSkills();

		// Token: 0x060000B5 RID: 181
		int GetLevelForCustomSkill(Farmer farmer, string skill);

		// Token: 0x060000B6 RID: 182
		int GetExperienceForCustomSkill(Farmer farmer, string skill);

		// Token: 0x060000B7 RID: 183
		List<Tuple<string, int, int>> GetExperienceAndLevelsForCustomSkill(Farmer farmer);

		// Token: 0x060000B8 RID: 184
		void AddExperienceForCustomSkill(Farmer farmer, string skill, int amt);

		// Token: 0x060000B9 RID: 185
		int GetProfessionId(string skill, string profession);

		// Token: 0x060000BA RID: 186
		void RegisterSerializerType(Type type);

		// Token: 0x060000BB RID: 187
		void RegisterCustomProperty(Type declaringType, string name, Type propType, MethodInfo getter, MethodInfo setter);
	}
}
