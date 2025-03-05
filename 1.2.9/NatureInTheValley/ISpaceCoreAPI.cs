using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x02000009 RID: 9
	public interface ISpaceCoreAPI
	{
		// Token: 0x060000AB RID: 171
		string[] GetCustomSkills();

		// Token: 0x060000AC RID: 172
		int GetLevelForCustomSkill(Farmer farmer, string skill);

		// Token: 0x060000AD RID: 173
		int GetExperienceForCustomSkill(Farmer farmer, string skill);

		// Token: 0x060000AE RID: 174
		List<Tuple<string, int, int>> GetExperienceAndLevelsForCustomSkill(Farmer farmer);

		// Token: 0x060000AF RID: 175
		void AddExperienceForCustomSkill(Farmer farmer, string skill, int amt);

		// Token: 0x060000B0 RID: 176
		int GetProfessionId(string skill, string profession);

		// Token: 0x060000B1 RID: 177
		void RegisterSerializerType(Type type);

		// Token: 0x060000B2 RID: 178
		void RegisterCustomProperty(Type declaringType, string name, Type propType, MethodInfo getter, MethodInfo setter);
	}
}
