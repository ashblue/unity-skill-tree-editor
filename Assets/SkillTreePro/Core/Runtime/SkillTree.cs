using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	public class SkillTree : SkillTreeBase {
		public List<SkillTreeData> databases;

		public override List<SkillTreeDataBase> Databases {
			get {
				return databases.ConvertAll(d => (SkillTreeDataBase)d);
			}
		} 
	}
}
