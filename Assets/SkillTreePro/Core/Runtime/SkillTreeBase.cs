using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public abstract class SkillTreeBase : MonoBehaviour {
		public abstract List<SkillTreeDataBase> Databases { get; }

		void Awake () {
			Debug.Log("Post process databases here for in-game use");
		}
	}
}
