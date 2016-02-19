using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCategoryDefinition : SkillDefinitionBase {
		[Tooltip("Optional index used to determine the category placement")]
		public int sortIndex;

		[Tooltip("What is the starting skill level in this category?")]
		public int defaultSkillLv = 0;

		public SkillCollectionStarDefinition start = new SkillCollectionStarDefinition();
	}
}
