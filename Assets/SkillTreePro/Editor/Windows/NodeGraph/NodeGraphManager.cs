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
				_activeCat = null;

				if (_db != null) {
					EditorPrefs.SetFloat(DATABASE_ID_KEY, _db.GetInstanceID());
				}
			}
		}

		public static SkillCategoryDefinition _activeCat;
		public static SkillCategoryDefinition DbCat {
			get {
				return _activeCat;
			}

			set {
				_activeCat = value;
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
