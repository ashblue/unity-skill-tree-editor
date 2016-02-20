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
			node.RectPos = new Rect(0, 0, 0, 0);
		}
	}
}
	