using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public class SkillTree : MonoBehaviour {
		[SerializeField] List<SkillTreeDatabase> databases;

		void Awake () {
			Debug.Log("Post process databases here for in-game use");
		}
	}
}
