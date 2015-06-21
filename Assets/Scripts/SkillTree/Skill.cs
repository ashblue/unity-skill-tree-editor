using UnityEngine;
using System.Collections;

namespace Adnc.SkillTree {
	public class Skill : MonoBehaviour {
		[Tooltip("Has this skill been unlocked by the player?")]
		public bool unlocked;

		public string displayName = "Skill";
		public string uniqueName;
		
		[TextArea(3, 5)]
		public string description;
	}
}
