using UnityEngine;
using System.Collections;

namespace Adnc.SkillTree.Example.MultiCategory {
	public class SkillTreeSnapshot : MonoBehaviour {
		SaveSkillTree snapshot;
		SkillMenu menu;

		void Awake () {
			menu = GetComponent<SkillMenu>();
		}

		public void SaveSnapshot () {
			snapshot = menu.skillTree.GetSnapshot();
		}

		public void LoadSnapshot () {
			if (snapshot != null) {
				menu.skillTree.LoadSnapshot(snapshot);
				SkillCategoryBase[] categories = menu.skillTree.GetCategories();
				menu.ShowCategory(categories[0]);
			}
		}
	}
}
