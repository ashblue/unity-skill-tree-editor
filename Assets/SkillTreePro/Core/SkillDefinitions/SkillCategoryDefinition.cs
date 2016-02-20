using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCategoryDefinition : SkillDefinitionBase {
		[Tooltip("Optional index used to determine the category placement")]
		public int sortIndex;

		[Tooltip("What is the starting skill level in this category?")]
		public int defaultSkillLv = 0;

		public SkillCollectionStartDefinition start = new SkillCollectionStartDefinition(null);
		public List<SkillCollectionDefinition> skillCollections = new List<SkillCollectionDefinition>();
		public List<SkillDefinition> skillDefinitions = new List<SkillDefinition>();

		public SkillDefinition GetSkill (string uuid) {
			return skillDefinitions.Find(s => s.uuid == uuid);
		}

		/// <summary>
		/// Wipe all skill references then clean up the collection
		/// </summary>
		/// <param name="col">Collection</param>
		public void DestroyCollection (SkillCollectionDefinition col) {
			col.skills.ForEach(uuid => DestorySkill(col, GetSkill(uuid)));
			skillCollections.Remove(col);
		}
			
		public void DestorySkill (SkillCollectionDefinition col, SkillDefinition skill) {
			col.skills.RemoveAll(uuid => uuid == skill.uuid);
			skillDefinitions.Remove(skill);
		}
	}
}
