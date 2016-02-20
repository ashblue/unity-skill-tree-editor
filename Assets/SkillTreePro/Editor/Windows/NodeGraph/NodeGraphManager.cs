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

		public static SkillCategoryDefinition DbCat {
			get {
				return Wm.Db.ActiveCategory;
			}

			set {
				DbCol = null;
				Wm.Db.ActiveCategory = value;
			}
		}

		public static SkillCollectionDefinition _dbCol;
		public static SkillCollectionDefinition DbCol {
			get {
				return _dbCol;
			}

			set {
				_dbCol = value;
			}
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
