using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	/// <summary>
	/// Contains a skill tree database with additional data that can be attached from the scene view or project.
	/// A great place to extend your database with additional data for extended classes.
	/// </summary>
	[System.Serializable]
	public abstract class SkillTreeDataBase {
		[Tooltip("The ID used to reference this database at run-time")]
		public string id;

		[Tooltip("The database to be loaded into the game")]
		public SkillTreeDatabase database;
	}
}
