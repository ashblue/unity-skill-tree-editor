using UnityEngine;
using System;
using System.Collections;

namespace Adnc.SkillTree {
	abstract public class SkillBase : MonoBehaviour {
		[Tooltip("Has this skill been unlocked by the player?")]
		public bool unlocked;

		[Tooltip("Used for internal purposes only usually")]
		public string displayName = "Skill";

		[Tooltip("Used for data lookup")]
		public string uniqueName;
		
		[TextArea(3, 5)]
		public string description;

		[Header("Requirements")]
		[Tooltip("Required tree level to unlock this skill. Leave at 0 to ignore this.")]
		public int requiredLevel;
		
		[Tooltip("List of additional requirements beyond unlocking the previous skill entry and skill collection")]
		public SkillBase[] requiredSkills;

		// @TODO Consider removing
		[HideInInspector ]public string uuid;

		/// <summary>
		/// Visual print out of requirements
		/// </summary>
		/// <returns>The requirements.</returns>
		virtual public string GetRequirements () {
			string requirements = "";
			SkillCategoryBase category = transform.parent.parent.GetComponent<SkillCategoryBase>();

			if (requiredLevel > 0) 
				requirements += string.Format("* {0} Skill Lv {1} \n", category.displayName, requiredLevel);

			foreach (SkillBase skill in requiredSkills) {
				SkillCollectionBase collection = skill.transform.parent.GetComponent<SkillCollectionBase>();
				requirements += string.Format("* {0} Lv {1} \n", collection.displayName, skill.transform.GetSiblingIndex() + 1);
			}

			return requirements;
		}

		/// <summary>
		/// Loops through all requirements to check if this skill is available for purchase
		/// </summary>
		/// <returns><c>true</c> if this instance is requirements; otherwise, <c>false</c>.</returns>
		virtual public bool IsRequirements () {
			SkillCategoryBase category = transform.parent.parent.GetComponent<SkillCategoryBase>();

			if (category.skillLv < requiredLevel) {
				return false;
			}

			foreach (SkillBase skill in requiredSkills) {
				if (!skill.unlocked) {
					return false;
				}
			}

			return true;
		}
	}
}
