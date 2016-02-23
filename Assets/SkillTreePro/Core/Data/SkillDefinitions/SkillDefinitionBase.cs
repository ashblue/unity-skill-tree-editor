using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public abstract class SkillDefinitionBase : DefinitionBase {
		[Tooltip("Is this skill unlocked by default?")]
		public bool unlocked;

		[Tooltip("Required tree level to unlock this skill. Leave at 0 to ignore this.")]
		public int requiredLevel;

		public void Setup (SkillCategoryDefinitionBase cat, SkillCollectionDefinitionBase col) {
			base.Setup();
			cat.skillDefinitions.Add(this);
			col.skills.Add(this);
		}
	}
}
