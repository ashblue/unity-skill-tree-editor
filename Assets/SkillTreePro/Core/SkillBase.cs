using UnityEngine;
using System;
using System.Collections;

namespace Adnc.SkillTree {
	abstract public class SkillBase : MonoBehaviour {
		[Tooltip("Has this skill been unlocked by the player?")]
		public bool unlocked;

		[Tooltip("Used for internal purposes only usually")]
		[DisplayName("Skill")] public string displayName = "Skill";

		[Tooltip("Used for data lookup")]
		public string id;
		
		[TextArea(3, 5)]
		public string description;

		[Header("Requirements")]
		[Tooltip("Required tree level to unlock this skill. Leave at 0 to ignore this.")]
		public int requiredLevel;

		SkillCategoryBase _category;
		public SkillCategoryBase Category { 
			get { 
				if (_category == null) _category = transform.parent.parent.GetComponent<SkillCategoryBase>();
				return _category;
			} 
		}

		SkillCollectionBase _collection;
		public SkillCollectionBase Collection { 
			get { 
				if (_collection == null) _collection = GetComponentInParent<SkillCollectionBase>(); 
				return _collection;
			} 
		}

		SkillTreeBase _tree;
		public SkillTreeBase Tree { 
			get { 
				if (_tree == null) _tree = Category.GetComponentInParent<SkillTreeBase>(); 
				return _tree;
			} 
		}

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

		/// <summary>
		/// Visual print out of requirements
		/// </summary>
		/// <returns>The requirements.</returns>
		virtual public string GetRequirements () {
			string requirements = "";
			SkillCategoryBase category = transform.parent.parent.GetComponent<SkillCategoryBase>();

			if (requiredLevel > 0) 
				requirements += string.Format("* {0} Skill Lv {1} \n", category.displayName, requiredLevel);

			return requirements;
		}

		/// <summary>
		/// Loops through all requirements to check if this skill is available for purchase
		/// </summary>
		/// <returns><c>true</c> if this instance is requirements; otherwise, <c>false</c>.</returns>
		virtual public bool IsRequirements () {
			if (!Tree.IsParentUnlocked(Collection)) {
				return false;
			}

			if (Category.skillLv < requiredLevel) {
				return false;
			}

			return true;
		}
	}
}
