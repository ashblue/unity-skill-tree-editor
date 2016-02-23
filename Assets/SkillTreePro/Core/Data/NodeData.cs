using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class NodeData {
		public Rect _rect;
		const float WIDTH = 100;
		const float HEIGHT = 100;
		public static float CELL_SIZE = 110;

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

		public void SetPosition (Vector2 pos, bool snap) {
			RectPos = new Rect(pos, RectPos.size);

			if (snap) {
				RectPos = SnapPosition(RectPos);
			}
		}

		public static Rect SnapPosition (Rect r) {
			return new Rect(
				RoundNumber(r.position.x, CELL_SIZE) - ((r.width - CELL_SIZE) / 2),
				RoundNumber(r.position.y, CELL_SIZE) - ((r.height - CELL_SIZE) / 2),
				r.width,
				r.height
			);
		}

		public static float RoundNumber (float num, float chunk) {
			return ((int)Mathf.Round(num / chunk)) * chunk;
		}
	}
}
