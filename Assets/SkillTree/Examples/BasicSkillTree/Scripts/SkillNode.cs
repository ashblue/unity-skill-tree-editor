using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Adnc.SkillTree.Example.MultiCategory {
	public class SkillNode : MonoBehaviour {
		Button btn;
		[HideInInspector] public SkillMenu menu;

		[Tooltip("Simple reference to pre-existing skill node data. Used for updating skill nodes in real time.")]
		public SkillCollectionBase skillCollection;

		// [Tooltip("Current status of the node")]
		NodeStatus status;

		public List<SkillNode> parents = new List<SkillNode>();
		public List<SkillNode> children = new List<SkillNode>();

		void Awake () {
			btn = GetComponent<Button>();
		}

		public void ShowDetails () {
			menu.ShowNodeDetails(this);
		}

		public void SetStatus (NodeStatus status, Color color) {
			this.status = status;

			ColorBlock colorBlock = btn.colors;
			colorBlock.normalColor = color;
			colorBlock.highlightedColor = color;

			btn.colors = colorBlock;
		}

		public NodeStatus GetStatus () {
			return status;
		}
	}
}
