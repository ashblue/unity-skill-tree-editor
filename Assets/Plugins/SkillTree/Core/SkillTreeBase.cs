using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	abstract public class SkillTreeBase : MonoBehaviour {
		[Tooltip("Name of this skill tree displayed to the user")]
		public string title = "Skill Tree";

		[TextArea(3, 5)]
		public string description;

		// Used by the skill tree window to discover the currently active category for editing
		[HideInInspector] public SkillCategoryBase currentCategory;

		// Override these classes with custom classes that inherit from the base
		abstract public System.Type SkillCategory { get; }
		abstract public System.Type SkillCollection { get; }
		abstract public System.Type Skill { get; }

		[Tooltip("How many skill points are available")]
		public int skillPoints = 0; // Number of available skill points

		Dictionary<string, SkillCategoryBase> categoryLib = new Dictionary<string, SkillCategoryBase>();
		Dictionary<string, SkillCollectionBase> skillCollectionLib = new Dictionary<string, SkillCollectionBase>();
		Dictionary<string, SkillBase> skillLib = new Dictionary<string, SkillBase>();
		
		void Awake () {
			foreach (SkillCategoryBase cat in GetCategories()) {
				if (!string.IsNullOrEmpty(cat.id)) categoryLib[cat.id] = cat;
			}

			foreach (SkillCollectionBase col in GetSkillCollections()) {
				if (!string.IsNullOrEmpty(col.id)) skillCollectionLib[col.id] = col;
			}

			foreach (SkillBase skill in GetSkills()) {
				if (!string.IsNullOrEmpty(skill.id)) skillLib[skill.id] = skill;
			}
		}

		/// <summary>
		/// Retrieve all active categories. Warning, expensive
		/// </summary>
		/// <returns>The categories.</returns>
		public SkillCategoryBase[] GetCategories () {
			return GetComponentsInChildren<SkillCategoryBase>();
		}

		/// <summary>
		/// Returns all skill collecitons. Warning, expensive
		/// </summary>
		/// <returns>The skill collections.</returns>
		public SkillCollectionBase[] GetSkillCollections () {
			return GetComponentsInChildren<SkillCollectionBase>();
		}

		/// <summary>
		/// Returns all skils. Warning expensive
		/// </summary>
		/// <returns>The skills.</returns>
		public SkillBase[] GetSkills () {
			return GetComponentsInChildren<SkillBase>();
		}

		/// <summary>
		/// Returns a category from the user assigned ID
		/// </summary>
		/// <returns>The category.</returns>
		/// <param name="categoryId">Category identifier.</param>
		public SkillCategoryBase GetCategory (string categoryId) {
			return categoryLib[categoryId];
		}

		/// <summary>
		/// Returns a collection from the user assigned ID
		/// </summary>
		/// <returns>The collection.</returns>
		/// <param name="collectionId">Collection identifier.</param>
		public SkillCollectionBase GetCollection (string collectionId) {
			return skillCollectionLib[collectionId];
		}

		/// <summary>
		/// Returns a skill from the user assigned ID
		/// </summary>
		/// <returns>The skill.</returns>
		/// <param name="skillId">Skill identifier.</param>
		public SkillBase GetSkill (string skillId) {
			return skillLib[skillId];
		}

		/// <summary>
		/// Return the current skill from a collection
		/// </summary>
		/// <returns>The skill from category.</returns>
		/// <param name="collectionId">Collection identifier.</param>
		public SkillBase GetSkillFromCategory (string collectionId) {
			return GetCollection(collectionId).Skill;
		}

		/// <summary>
		/// Declare the current level of a specific category
		/// </summary>
		/// <param name="categoryId">Category identifier.</param>
		/// <param name="lv">Lv.</param>
		public void SetCategoryLv (string categoryId, int lv) {
			SkillCategoryBase cat = GetCategory(categoryId);
			cat.skillLv = lv;
		}

		/// <summary>
		/// Check if a specific skill has been unlocked
		/// </summary>
		/// <returns><c>true</c> if this instance is unlocked the specified skillId; otherwise, <c>false</c>.</returns>
		/// <param name="skillId">Skill identifier.</param>
		public bool IsUnlocked (string skillId) {
			return GetSkill(skillId).unlocked;
		}

		// @TODO Should spit back a giant array of data so the user can save it however they want
		// @TODO change to abstract when moving SkillTree to an abstract class
		virtual public void Save () {
//			Dictionary<string, bool> skillRecords = new Dictionary<string, bool>();
//
//			foreach (Transform collection in transform) {
//				foreach (Transform child in collection.transform) {
//					Skill skill = child.GetComponent<Skill>();
//					if (string.IsNullOrEmpty(skill.uuid)) {
//						Debug.LogErrorFormat("Skill {0} has no UUID", skill.uuid);
//					} else {
//						skillRecords.Add(skill.uuid, skill.unlocked);
//					}
//				}
//			}

			// Create a special save class that we can load or create
//			SaveData saveData = new SaveData {
//				name = title,
//				skills = skillRecords
//			};

			// Turn save class into JSON via JSON.net or your preferred JSON library
		}

		// @TODO change to abstract when moving SkillTree to an abstract class
		// @TODO Should swallow Save data format to update all active skills
		virtual public void Load () {
			// Loop through skill dictionary
			// For each UUID apply the unlocked skill value
			// If we hit a missing UUID fire a warning
		}
	}
}
