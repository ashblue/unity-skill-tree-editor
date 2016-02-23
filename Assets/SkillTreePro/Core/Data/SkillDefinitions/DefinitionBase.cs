using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	/// <summary>
	/// Skill definition text in a collapsable display
	/// </summary>
	[System.Serializable]
	public class SkillDefinitionText {
		[TextArea] public string description;
		[TextArea] public string notes;
	}

	[System.Serializable]
	public abstract class DefinitionBase : ScriptableObject {
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

		public SkillDefinitionText text;

		public virtual void Setup () {
			hideFlags = HideFlags.HideInHierarchy;
		}
	}
}
