using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class SkillCollectionGrid {
		public int Width { get { return _grid.GetLength(0); } }
		public int Height { get { return _grid.GetLength(1); } }

		/// <summary>
		/// The grid organized as grid[y, x].
		/// </summary>
		public SkillCollectionBase[,] _grid;

		public SkillCollectionGrid (SkillCollectionBase[,] grid) {
			_grid = grid;
		}

		/// <summary>
		/// Gets the collection.
		/// </summary>
		/// <returns>The collection.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public SkillCollectionBase GetCollection (int x, int y) {
			return _grid[x, y];
		}

		/// <summary>
		/// Gets all collections available.
		/// </summary>
		/// <returns>All collections stored in the grid.</returns>
		public List<SkillCollectionGridItem> GetAllCollections () {
			List<SkillCollectionGridItem> collections = new List<SkillCollectionGridItem>();

			for (int x = 0; x < _grid.GetLength(0); x++) {
				for (int y = 0; y < _grid.GetLength(1); y++) {
					if (_grid[x, y] != null) collections.Add(new SkillCollectionGridItem {
						x = x,
						y = y,
						collection = _grid[x, y]
					});
				}
			}

			return collections;
		}
	}
}
