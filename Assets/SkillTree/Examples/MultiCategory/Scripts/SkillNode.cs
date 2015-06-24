using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTree.Example.MultiCategory {
	public class SkillNode : MonoBehaviour {
		[Tooltip("Simple reference to pre-existing skill node data. Used for updating skill nodes in real time.")]
		public SkillCollectionBase skillCollection;

		public List<SkillNode> parents = new List<SkillNode>();
		public List<SkillNode> children = new List<SkillNode>();

		public void ShowDetails () {
			SkillMenu.current.ShowNodeDetails(skillCollection);
		}
	}
}
