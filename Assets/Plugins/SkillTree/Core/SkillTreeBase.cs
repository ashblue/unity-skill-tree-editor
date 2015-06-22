using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	abstract public class SkillTreeBase : MonoBehaviour {
		public GraphSidebar[] test;

		public string title = "Skill Tree";
		[TextArea(3, 5)]
		public string description;

		// Used by the skill tree window to discover the currently active category for editing
		[HideInInspector] public SkillCategoryBase currentCategory;

		// Override these base classes with custom classes that inherit from the base
		abstract public System.Type SkillCategory { get; }
		abstract public System.Type SkillCollection { get; }
		abstract public System.Type Skill { get; }

		// @TODO Remove mock save pattern after implementing with ADNC
		// This is just an implementation example of a save, you must implement your own
		// @TODO change to abstract when moving SkillTree to an abstract class
		virtual public void Save () {
//			Dictionary<string, bool> skillRecords = new Dictionary<string, bool>();
//
//			foreach (Transform collection in transform) {
//				foreach (Transform child in collection.transform) {
//					Skill skill = child.GetComponent<Skill>();
//					if (string.IsNullOrEmpty(skill.uuid)) {
//						Debug.LogErrorFormat("Skill {0} has no UUID", skill.uuid);
//					} else {
//						skillRecords.Add(skill.uuid, skill.unlocked);
//					}
//				}
//			}

			// Create a special save class that we can load or create
//			SaveData saveData = new SaveData {
//				name = title,
//				skills = skillRecords
//			};

			// Turn save class into JSON via JSON.net or your preferred JSON library
		}

		// @TODO change to abstract when moving SkillTree to an abstract class
		virtual public void Load () {
			// Loop through skill dictionary
			// For each UUID apply the unlocked skill value
			// If we hit a missing UUID fire a warning
		}
	}
}
