using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	abstract public class SkillTreeBase : MonoBehaviour {
		[DisplayName("SkillTreeData")] public string displayName = "Skill Tree";

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

		// Records of the children's parents for collecitons
		public Dictionary<SkillCollectionBase, List<SkillCollectionBase>> childParents;

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

			childParents = GetParentData();
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

		public SkillCollectionGrid GetGrid (SkillCategoryBase category) {
			SkillCollectionBase[] collect = category.GetComponentsInChildren<SkillCollectionBase>();
			Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
			Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

			// Find the min x, min y, max x, and max y
			foreach (SkillCollectionBase col in collect) {
				if (col.windowRect.x < min.x) min.x = col.windowRect.x;
				if (col.windowRect.x > max.x) max.x = col.windowRect.x;
				if (col.windowRect.y < min.y) min.y = col.windowRect.y;
				if (col.windowRect.y > max.y) max.y = col.windowRect.y;
			}

			int x, y;
			int width = Mathf.CeilToInt(Mathf.Abs(min.x - max.x) / gridCellSize.x) + 1;
			int height = Mathf.CeilToInt(Mathf.Abs(min.y - max.y) / gridCellSize.y) + 1;
			SkillCollectionBase[,] grid = new SkillCollectionBase[width, height];
			foreach (SkillCollectionBase col in collect) {
				x = Mathf.RoundToInt((col.windowRect.x - min.x) / gridCellSize.x);
				y = Mathf.RoundToInt((col.windowRect.y - min.y) / gridCellSize.y);
				grid[x, y] = col;
			}

			return new SkillCollectionGrid(grid);
		}

		/// <summary>
		/// Determines if a parent collection is properly unlocked
		/// </summary>
		/// <returns><c>true</c> if this instance is parent unlocked the specified collection; otherwise, <c>false</c>.</returns>
		/// <param name="collection">Collection.</param>
		public virtual bool IsParentUnlocked (SkillCollectionBase collection) {
			// Check to see if it has no parent
			if (!childParents.ContainsKey(collection)) {
				return true;
			}

			// Check all parents to see if any are unlocked
			foreach (SkillCollectionBase parent in childParents[collection]) {
				if (parent.GetSkill(0).unlocked) return true;
			}

			// No matches
			return false;
		}

		/// <summary>
		/// Loops through all collections to determine parent elements. Warning, quite expensive.
		/// </summary>
		/// <returns>The parent data.</returns>
		Dictionary<SkillCollectionBase, List<SkillCollectionBase>> GetParentData () {
			Dictionary<SkillCollectionBase, List<SkillCollectionBase>> childParents = new Dictionary<SkillCollectionBase, List<SkillCollectionBase>>();

			foreach (SkillCategoryBase category in GetComponentsInChildren<SkillCategoryBase>()) {
				foreach (SkillCollectionBase parent in category.GetComponentsInChildren<SkillCollectionBase>()) {
					foreach (SkillCollectionBase child in parent.childSkills) {
						if (!childParents.ContainsKey(child)) {
							childParents[child] = new List<SkillCollectionBase>();
						}
						
						childParents[child].Add(parent);
					}
				}
			}

			return childParents;
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
