using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Adnc.SkillTreePro {
	public class GridPrinter {
		Color lineColor = new Color(0f, 0f, 0f, 0.2f);

		public float CellSize {
			get {
				return 110;
			}
		}

		public void Update (Vector2 size, Vector2 offset) {
			DrawGrid(size, offset, CellSize);
		}

		void DrawGrid (Vector2 size, Vector2 offset, float cellSize) {
			int cellHorizontalCount = Mathf.RoundToInt(size.y / cellSize) + 1;
			int cellVerticalCount = Mathf.RoundToInt(size.x / cellSize) + 1;

			// Calculate the offset remainder
			float offsetX = offset.x % cellSize;
			float offsetY = offset.y % cellSize;

			// On the opposite axis we have to flip the calculation to subtract instead of add
			if (offsetX > 0) {
				offsetX -= cellSize;
			}

			if (offsetY > 0) {
				offsetY -= cellSize;
			}

			// Slightly adjust lines to stay with their correct position
			Vector2 gridOffset = new Vector2(
				offset.x + Mathf.Abs(offsetX), 
				offset.y + Mathf.Abs(offsetY));

			// horizontal lines
			for (int i = 0, l = cellHorizontalCount; i < l; i++) {
				Drawing.DrawLine(
					new Vector2(offset.x, gridOffset.y + (i * cellSize)), 
					new Vector2(offset.x + size.x, gridOffset.y + (i * cellSize)), lineColor);
			}

			// vertical lines
			for (int j = 0, l = cellVerticalCount; j < l; j++) {
				Drawing.DrawLine(
					new Vector2(gridOffset.x + (j * cellSize), offset.y), 
					new Vector2(gridOffset.x + (j * cellSize), offset.y + size.y), lineColor);
			}
		}
	}
}
