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
		public string displayName = "Untitled";

		public string description;

		public string notes;

		public SkillDefinitionBase () {
			uuid = System.Guid.NewGuid().ToString();
		}
	}
}
