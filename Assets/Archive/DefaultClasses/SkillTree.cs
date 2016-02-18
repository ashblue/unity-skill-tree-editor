using UnityEngine;
using System.Collections;

namespace Adnc.SkillTree {
	public class SkillTree : SkillTreeBase {
		override public System.Type SkillCategory { get { return typeof(SkillCategory); } }
		override public System.Type SkillCollection { get { return typeof(SkillCollection); } }
		override public System.Type Skill { get { return typeof(Skill); } }
	}
}
