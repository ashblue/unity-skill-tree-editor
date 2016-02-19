using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCollectionStarDefinition : SkillCollectionDefinition {
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
	}
}
	