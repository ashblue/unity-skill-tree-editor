using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	abstract public class SkillCategoryBase : MonoBehaviour {
		[DisplayName("Category")] public string displayName = "Category";

		[Tooltip("Used for data retrieval purposes")]
		public string id;
		
		[TextArea(3, 5)]
		public string description;

		[Tooltip("What is the current skill level in this category?")]
		public int skillLv = 0;

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
		/// Retreives skill collections without any parents
		/// </summary>
		/// <returns>The root skill collections.</returns>
		public List<SkillCollectionBase> GetRootSkillCollections () {
			List<SkillCollectionBase> skills = new List<SkillCollectionBase>();

			// Loop through and find all collection that are a child of something
			Dictionary<Transform, bool> blacklist = new Dictionary<Transform, bool>();
			foreach (Transform child in transform) {
				foreach (SkillCollectionBase childNode in child.GetComponent<SkillCollectionBase>().childSkills) {
					blacklist[childNode.transform] = true;
				}
			}

			// Anything not blacklisted as a child node is a root, return those
			foreach (Transform child in transform) {
				if (!blacklist.ContainsKey(child)) {
					SkillCollectionBase skill = child.GetComponent<SkillCollectionBase>();
					skills.Add(skill);
				}
			}
			
			return skills;
		}
	}
}
