using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Adnc.SkillTree {
	public class SkillCategory : MonoBehaviour {
		public string displayName = "Category";

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
