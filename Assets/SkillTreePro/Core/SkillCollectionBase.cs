using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	abstract public class SkillCollectionBase : MonoBehaviour {
//		[Tooltip("Name the user will see when the node is printed")]
		[DisplayName("SkillCollection")] public string displayName = "Skill Collection";

		[Tooltip("Used for data lookup purposes")]
		public string id;

		[TextArea(3, 5)]
		public string notes;
		
		[Header("Window Debug Data")]
		public Rect windowRect;
		[HideInInspector] public List<SkillCollectionBase> childSkills = new List<SkillCollectionBase>();

		string uuid;
		public string Uuid {
			get {
				if (string.IsNullOrEmpty(uuid)) {
					uuid = System.Guid.NewGuid().ToString();
				}
				
				return uuid;
			}
			
			set {
				uuid = value;
			}
		}

		// Determines what skill is currently active and returns it
		int skillIndex = 0;
		public int SkillIndex {
			get {
				return skillIndex;
			}

			set {
				if (value >= 0 && value < transform.childCount) {
					skillIndex = value;
				} 
			}
		}

		// Returns the current active skill
		public SkillBase Skill {
			get {
				return transform.GetChild(skillIndex).GetComponent<SkillBase>();
			}
		}

		// How many skills does this collection contain
		public int SkillCount {
			get {
				return transform.childCount;
			}
		}

		/// <summary>
		/// Unlock the currently active skill and set the pointer to the next unlocked if available
		/// </summary>
		public void Purchase () {
			SkillTree skillTree = transform.parent.parent.GetComponent<SkillTree>();

			if (skillTree.skillPoints <= 0) return;
			skillTree.skillPoints -= 1;

			Skill.unlocked = true;
			SkillIndex += 1;
		}

		/// <summary>
		/// Get the skill at a specific index
		/// </summary>
		/// <returns>The skill.</returns>
		/// <param name="index">Index.</param>
		public SkillBase GetSkill (int index) {
			return transform.GetChild(index).GetComponent<SkillBase>();
		}
	}
}
