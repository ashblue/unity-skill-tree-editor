using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class SkillCollection : MonoBehaviour {
		public string displayName = "Skill Collection";
		public string uniqueName;

		[TextArea(3, 5)]
		public string description;

		[Header("Menu Icons")]
		public Image imgRegular;
		public Image imgHighlight;
		public Image imgSelect;
		public Image imgPurchase;

		[Header("Window Debug Data")]
		public Rect windowRect;
		public List<SkillCollection> childSkills = new List<SkillCollection>();
	}
}
