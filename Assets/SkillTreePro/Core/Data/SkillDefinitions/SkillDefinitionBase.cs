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

		public virtual void Activate (SkillTreeDataBase tree) {
			// Place activation logic here
		}

		public virtual void Deactivate (SkillTreeDataBase tree) {
			// Place teardown logic here
		}

		public virtual bool Requirements (SkillTreeDataBase tree) {
			// Place extra unlock requirements here such as key value pairs, story progression, inventory, ect.
			return true;
		}
	}
}
