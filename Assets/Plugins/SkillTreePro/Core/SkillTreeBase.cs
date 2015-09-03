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

		[Header("Window Debug Data")]

		[Tooltip("How large is a grid cell? Will be used to build a visual array that turns into the version a user sees.")]
		public Vector2 gridCellSize = new Vector2(250f, 200f);

		Dictionary<string, SkillCategoryBase> categoryLib = new Dictionary<string, SkillCategoryBase>();
		Dictionary<string, SkillCollectionBase> skillCollectionLib = new Dictionary<string, SkillCollectionBase>();
		Dictionary<string, SkillBase> skillLib = new Dictionary<string, SkillBase>();

		Dictionary<string, SkillCategoryBase> categoryUuidLib = new Dictionary<string, SkillCategoryBase>();
		Dictionary<string, SkillCollectionBase> collectionUuidLib = new Dictionary<string, SkillCollectionBase>();
		Dictionary<string, SkillBase> skillUuidLib = new Dictionary<string, SkillBase>();
		
		void Awake () {
			foreach (SkillCategoryBase cat in GetCategories()) {
				if (!string.IsNullOrEmpty(cat.id)) categoryLib[cat.id] = cat;
				categoryUuidLib[cat.Uuid] = cat;
			}

			foreach (SkillCollectionBase col in GetSkillCollections()) {
				if (!string.IsNullOrEmpty(col.id)) skillCollectionLib[col.id] = col;
				collectionUuidLib[col.Uuid] = col;
			}

			foreach (SkillBase skill in GetSkills()) {
				if (!string.IsNullOrEmpty(skill.id)) skillLib[skill.id] = skill;
				skillUuidLib[skill.Uuid] = skill;
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

		/// <summary>
		/// Returns a snapshot of this skill tree's current state. It is recommended that you save this data to a file
		/// in a way it can be easily restored to the same structure.
		/// </summary>
		/// <returns>The snapshot.</returns>
		virtual public SaveSkillTree GetSnapshot () {
			List<SaveSkill> skills = new List<SaveSkill>();
			List<SaveSkillCollection> skillCollections = new List<SaveSkillCollection>();
			List<SaveSkillCategory> skillCategories = new List<SaveSkillCategory>();

			foreach (SkillBase s in GetSkills()) {
				skills.Add(new SaveSkill {
					uuid = s.Uuid,
					unlocked = s.unlocked
				});
			}

			foreach (SkillCollectionBase s in GetSkillCollections()) {
				skillCollections.Add(new SaveSkillCollection {
					uuid = s.Uuid,
					skillIndex = s.SkillIndex
				});
			}

			foreach (SkillCategoryBase s in GetCategories()) {
				skillCategories.Add(new SaveSkillCategory {
					uuid = s.Uuid,
					skillLv = s.skillLv
				});
			}

			return new SaveSkillTree {
				skillPoints = skillPoints,
				skills = skills,
				collections = skillCollections,
				categories = skillCategories
			};
		}

		/// <summary>
		/// Restores a snapshot and overwrites the current skill tree values with it
		/// </summary>
		/// <param name="snapshot">Snapshot.</param>
		virtual public void LoadSnapshot (SaveSkillTree snapshot) {
			skillPoints = snapshot.skillPoints;

			foreach (SaveSkill s in snapshot.skills) {
				skillUuidLib[s.uuid].unlocked = s.unlocked;
			}
			
			foreach (SaveSkillCollection c in snapshot.collections) {
				collectionUuidLib[c.uuid].SkillIndex = c.skillIndex;
			}

			foreach (SaveSkillCategory c in snapshot.categories) {
				categoryUuidLib[c.uuid].skillLv = c.skillLv;
			}
		}
	}
}
