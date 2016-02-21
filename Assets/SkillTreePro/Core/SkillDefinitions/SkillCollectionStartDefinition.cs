using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCollectionStartDefinition : SkillCollectionDefinitionBase {
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

		public static bool hideInAddMenu = true;

		public override void Setup (SkillCategoryDefinition cat) {
			Setup();
		}

		public bool test;
	}
}
	