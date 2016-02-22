using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Adnc.SkillTreePro {
	public class GridPrinter {
		Color lineColor = new Color(0f, 0f, 0f, 0.2f);

		public void Update (Vector2 size, Vector2 offset) {
			DrawGrid(size, offset, NodeData.CELL_SIZE);
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

			Color handleColor = Handles.color;
			Handles.color = lineColor;

			// horizontal lines
			for (int i = 0, l = cellHorizontalCount; i < l; i++) {
				Handles.DrawLine(
					new Vector3(offset.x, gridOffset.y + (i * cellSize), 0), 
					new Vector3(offset.x + size.x, gridOffset.y + (i * cellSize), 0));
			}

			// vertical lines
			for (int j = 0, l = cellVerticalCount; j < l; j++) {
				Handles.DrawLine(
					new Vector3(gridOffset.x + (j * cellSize), offset.y, 0), 
					new Vector3(gridOffset.x + (j * cellSize), offset.y + size.y, 0));
			}

			Handles.color = handleColor;
		}
	}
}
