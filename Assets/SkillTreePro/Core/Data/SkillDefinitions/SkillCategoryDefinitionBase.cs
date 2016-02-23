using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public abstract class SkillCategoryDefinitionBase : DefinitionBase {
		[Header("Category Details")]

		[Tooltip("Hide this category from printed view?")]
		public bool hidden;

		[Tooltip("What is the starting skill level in this category?")]
		public int defaultSkillLv;

		[Tooltip("Optional category display icon")]
		public Sprite icon;

		[HideInInspector] public SkillCollectionStartDefinition start;
		[HideInInspector] public List<SkillCollectionDefinitionBase> skillCollections = new List<SkillCollectionDefinitionBase>();
		[HideInInspector] public List<SkillDefinitionBase> skillDefinitions = new List<SkillDefinitionBase>();

//		public SkillDefinition GetSkill (string uuid) {
//			return skillDefinitions.Find(s => s.uuid == uuid);
//		}

		/// <summary>
		/// Wipe all skill references then clean up the collection
		/// </summary>
		/// <param name="col">Collection</param>
		public void DestroyCollection (SkillCollectionDefinitionBase col) {
			col.skills.ForEach(s => DestorySkill(col, s));
			skillCollections.Remove(col);
		}

		public void DestorySkill (SkillCollectionDefinitionBase col, SkillDefinitionBase skill) {
			col.skills.Remove(skill);
			skillDefinitions.Remove(skill);
		}
	}
}
