using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public abstract class SkillDefinitionBase : ScriptableObject {
		[HideInInspector] public string uuid = System.Guid.NewGuid().ToString();

		[Tooltip("Used for data lookup purposes")]
		public string id;

		[Tooltip("Name the user will see when the node is printed")]
		public string _displayName = "Untitled";
		virtual public string DisplayName {
			get {
				return _displayName;
			}

			set {
				_displayName = value;
			}
		}

		[TextArea] public string description;
		[TextArea] public string notes;

		public virtual void Setup () {
			hideFlags = HideFlags.HideInHierarchy;
		}
	}
}
