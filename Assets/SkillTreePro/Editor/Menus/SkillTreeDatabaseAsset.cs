using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Adnc.SkillTreePro {
	public class DecisionDatabaseAsset {  
		[MenuItem("Assets/Create/Skill Tree/Database")]
		public static void CreateStatsDatabase () {
			SkillTreeDatabase asset = ScriptableObject.CreateInstance("SkillTreeDatabase") as SkillTreeDatabase;

			// @TODO Make sure an asset of the same name doesn't already exist
			AssetDatabase.CreateAsset(asset, GetPath() + "/SkillTreeDatabase.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;        
		}

		public static string GetPath () {
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
				path = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(path)) {
					path = Path.GetDirectoryName(path);
				}
				break;
			}

			return path;
		}
	}
}
