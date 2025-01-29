using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x02000008 RID: 8
	public interface ISpaceCoreAPI
	{
		// Token: 0x06000088 RID: 136
		string[] GetCustomSkills();

		// Token: 0x06000089 RID: 137
		int GetLevelForCustomSkill(Farmer farmer, string skill);

		// Token: 0x0600008A RID: 138
		int GetExperienceForCustomSkill(Farmer farmer, string skill);

		// Token: 0x0600008B RID: 139
		List<Tuple<string, int, int>> GetExperienceAndLevelsForCustomSkill(Farmer farmer);

		// Token: 0x0600008C RID: 140
		void AddExperienceForCustomSkill(Farmer farmer, string skill, int amt);

		// Token: 0x0600008D RID: 141
		int GetProfessionId(string skill, string profession);

		// Token: 0x0600008E RID: 142
		void RegisterSerializerType(Type type);

		// Token: 0x0600008F RID: 143
		void RegisterCustomProperty(Type declaringType, string name, Type propType, MethodInfo getter, MethodInfo setter);
	}
}
