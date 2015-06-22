using UnityEngine;
using System.Collections;

namespace Adnc.SkillTree {
	public class SkillTreeDefault : SkillTree {
		override public System.Type SkillCategory { get { return typeof(SkillCategoryDefault); } }
		override public System.Type SkillCollection { get { return typeof(SkillCollectionDefault); } }
		override public System.Type Skill { get { return typeof(SkillDefault); } }
	}
}
