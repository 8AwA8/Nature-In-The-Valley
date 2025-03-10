﻿using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;

namespace NatureInTheValley
{
	// Token: 0x02000008 RID: 8
	public interface ISpaceCoreAPI
	{
		// Token: 0x0600008E RID: 142
		string[] GetCustomSkills();

		// Token: 0x0600008F RID: 143
		int GetLevelForCustomSkill(Farmer farmer, string skill);

		// Token: 0x06000090 RID: 144
		int GetExperienceForCustomSkill(Farmer farmer, string skill);

		// Token: 0x06000091 RID: 145
		List<Tuple<string, int, int>> GetExperienceAndLevelsForCustomSkill(Farmer farmer);

		// Token: 0x06000092 RID: 146
		void AddExperienceForCustomSkill(Farmer farmer, string skill, int amt);

		// Token: 0x06000093 RID: 147
		int GetProfessionId(string skill, string profession);

		// Token: 0x06000094 RID: 148
		void RegisterSerializerType(Type type);

		// Token: 0x06000095 RID: 149
		void RegisterCustomProperty(Type declaringType, string name, Type propType, MethodInfo getter, MethodInfo setter);
	}
}
