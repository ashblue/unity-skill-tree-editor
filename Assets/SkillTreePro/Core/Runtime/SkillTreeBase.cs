using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public abstract class SkillTreeBase : MonoBehaviour {
		/// <summary>
		/// Reference for current running skill tree, will be auto-cleared when this GameObject is destroyed
		/// </summary>
		public static SkillTreeBase current;

//		[Tooltip("These databases will be post-processed at run-time into a usable format")]
		public abstract List<SkillTreeDataBase> Databases { get; }

		[Tooltip("Prints out a log of the skill tree at key events")]
		public bool debug;

		Dictionary<string, SkillTreeEntry> skillTrees = new Dictionary<string, SkillTreeEntry>();

		void Awake () {
			current = this;

			foreach (SkillTreeDataBase d in Databases) {
				if (d.database == null) {
					Debug.LogError("Skill tree database is misssing, skipping entry.");
					continue;
				}

				if (d.id == null) {
					Debug.LogError("Cannot use a skill tree database without assigning an ID to it.");
					continue;
				}
					
				BuildTree(d);
			}
		}

		void BuildTree (SkillTreeDataBase data) {
			skillTrees[data.id] = new SkillTreeEntry(data);
		}

		public SkillTreeEntry GetTree (string id) {
			return skillTrees[id];
		}

		public CategoryEntry GetCategory (string treeId, string categoryId) {
			return GetTree(treeId).categoriesById[categoryId];
		}

		public CategoryEntry GetCategoryByUuid (string treeId, string categoryUuid) {
			return GetTree(treeId).categoriesByUuid[categoryUuid];
		}

		public SkillEntry GetSkill (string treeId, string skillId) {
			return GetTree(treeId).skillsById[skillId];
		}

		public SkillEntry GetSkillByUuid (string treeId, string skillUuid) {
			return GetTree(treeId).skillsByUuid[skillUuid];
		}

		public bool IsSkillUnlocked (string treeId, string skillId) {
			SkillEntry skill = GetSkill(treeId, skillId);
			return skill.unlocked && skill.IsActive;
		}

		public void Save () {
			// @TODO Return JSON.net serialized data in a writable format
		}

		public void Load () {
			// @TODO Unpackage JSON from JSON.net and overwrite data
		}

		void OnDestroy () {
			if (current == this) {
				current = null;
			}
		}
	}
}
