using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public abstract class SkillTreeBase : MonoBehaviour {
		public static SkillTreeBase current;

		public abstract List<SkillTreeDataBase> Databases { get; }

		Dictionary<string, SkillTreeEntry> skillTrees = new Dictionary<string, SkillTreeEntry>();

		void Awake () {
			current = this;
			Databases.ForEach(d => BuildTree(d));
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

		public SkillEntry GetSkill (string treeId, string categoryId, string skillId) {
			return GetCategory(treeId, categoryId).skillsById[skillId];
		}

		public SkillEntry GetSkillByUuid (string treeId, string categoryUuid, string skillUuid) {
			return GetCategory(treeId, categoryUuid).skillsByUuid[skillUuid];
		}

		public void Save () {
			// @TODO Return JSON.net serialized data in a writable format
		}

		public void Load () {
			// @TODO Unpackage JSON from JSON.net and overwrite data
		}

		void OnDestroy () {
			current = null;
		}
	}
}
