using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class NodeData {
		public Rect _rect;
		const float WIDTH = 100;
		const float HEIGHT = 100;

		/// <summary>
		/// Interact with a rectangle position while maitaining a consistent size
		/// </summary>
		/// <value>The rect position.</value>
		public Rect RectPos {
			get {
				return _rect;
			}

			set {
				_rect = new Rect(value.x, value.y, WIDTH, HEIGHT);
			}
		}
	}
}
