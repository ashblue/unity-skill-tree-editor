using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Adnc.SkillTree {
	abstract public class SkillCategoryBase : MonoBehaviour {
		[Tooltip("Name the user will see")]
		public string displayName = "Category";

		[Tooltip("Used for data retrieval purposes")]
		public string uniqueName;
		
		[TextArea(3, 5)]
		public string description;

		[Header("Menu Icons")]
		public Image imgRegular;
		public Image imgHighlight;
		public Image imgPress;

		public void GetRootSkillGroup () {
			// Retrieves all skill groups that don't have a parent
		}
	}
}
