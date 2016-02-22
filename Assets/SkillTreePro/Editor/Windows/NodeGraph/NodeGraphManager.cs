using UnityEngine;
using UnityEditor;

namespace Adnc.SkillTreePro {
	public static class Wm {
		const string DATABASE_ID_KEY = "CurrentSkillTreeDatabase";
		public static SkillTreeDatabase _db;
		public static SkillTreeDatabase Db {
			get {
				if (_db == null && EditorPrefs.HasKey(DATABASE_ID_KEY)) {
					_db = GetDatabaseFromStorage();
				}

				return _db;
			}

			set {
				_db = value;
				DbCat = null;

				if (_db != null) {
					EditorPrefs.SetFloat(DATABASE_ID_KEY, _db.GetInstanceID());
				}
			}
		}

		public static NodeGraphWindow _win;
		public static NodeGraphWindow Win {
			get {
				return _win;
			}

			set {
				_win = value;
			}
		}

		public static SkillCategoryDefinitionBase DbCat {
			get {
				return Wm.Db.ActiveCategory;
			}

			set {
				DbCol = null;
				Wm.Db.ActiveCategory = value;
			}
		}

		public static SkillCollectionDefinitionBase _dbCol;
		public static SkillCollectionDefinitionBase DbCol {
			get {
				return _dbCol;
			}

			set {
				_dbCol = value;
			}
		}

		public static void DestroyCategory (int index) {
			SkillCategoryDefinitionBase cat = Wm.Db.categories[index];
			Wm.Db.ActiveCategory = null;

			// Wipe all associated objects from serialization
			Object.DestroyImmediate(cat.start, true);
			cat.skillCollections.ForEach(sc => Object.DestroyImmediate(sc, true));
			cat.skillDefinitions.ForEach(sd => Object.DestroyImmediate(sd, true));
			Object.DestroyImmediate(cat, true);
			Wm.Db.categories.RemoveAt(index);

			EditorUtility.SetDirty(Wm.Db);
			AssetDatabase.SaveAssets();
		}

		public static void DestroyCollection (SkillCategoryDefinitionBase cat, SkillCollectionDefinitionBase col) {
			cat.skillCollections.Remove(col);

			foreach (SkillCollectionDefinitionBase c in cat.skillCollections) {
				c.childCollections.Remove(col);
			}

			Object.DestroyImmediate(col, true);

			EditorUtility.SetDirty(Wm.Db);
			AssetDatabase.SaveAssets();
		}

		public static void DestroySkill (SkillCategoryDefinitionBase cat, SkillCollectionDefinitionBase col, SkillDefinitionBase skill) {
			if (col.skills.IndexOf(skill) == col.skillIndex) {
				col.skillIndex = 0;
			}

			cat.skillDefinitions.Remove(skill);
			col.skills.Remove(skill);

			Object.DestroyImmediate(skill, true);

			EditorUtility.SetDirty(Wm.Db);
			AssetDatabase.SaveAssets();
		}

		static SkillTreeDatabase GetDatabaseFromStorage () {
			int instanceId = (int)EditorPrefs.GetFloat(DATABASE_ID_KEY);
			Object obj = EditorUtility.InstanceIDToObject(instanceId);

			if (obj is SkillTreeDatabase) {
				return (SkillTreeDatabase)obj;
			}

			return null;
		}

		[MenuItem("Window/Skill Tree Pro")]
		public static void ShowSkillTreeEditor () {
			EditorWindow.GetWindow<NodeGraphWindow>("Skill Tree Editor");
		}
	}
}
