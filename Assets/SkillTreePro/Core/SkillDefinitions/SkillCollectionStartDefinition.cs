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

		public override string DisplayName {
			get {
				return "Start";
			}
		}

		public override string Description {
			get {
				return "All nodes must originate from this start node or they will not be included.";
			}
		}

		public SkillCollectionStartDefinition (SkillCategoryDefinition cat) : base (cat) {
			node.RectPos = new Rect(0, 0, 0, 0);
		}
	}
}
	