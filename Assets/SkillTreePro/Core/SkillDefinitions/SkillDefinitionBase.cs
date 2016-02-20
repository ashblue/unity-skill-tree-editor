using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public abstract class SkillDefinitionBase {
		[Tooltip("UUID generated at creation")]
		public string uuid;

		[Tooltip("Used for data lookup purposes")]
		public string id;

		[Tooltip("Name the user will see when the node is printed")]
		[SerializeField] string displayName = "Untitled";
		virtual public string DisplayName {
			get {
				return displayName;
			}

			set {
				displayName = value;
			}
		}

		[SerializeField] string description;
		virtual public string Description {
			get {
				return description;
			}

			set {
				description = value;
			}
		}

		public string notes;

		public SkillDefinitionBase () {
			uuid = System.Guid.NewGuid().ToString();
		}
	}
}
