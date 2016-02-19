using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCollectionStartDefinition : SkillCollectionDefinition {
		public override bool AllowParents {
			get {
				return false;
			}
		}

		public override bool Editable {
			get {
				return false;
			}
		}

		public SkillCollectionStartDefinition (SkillCategoryDefinition cat) : base (cat) {
			// Do nothing so we override the base constructor
		}
	}
}
	