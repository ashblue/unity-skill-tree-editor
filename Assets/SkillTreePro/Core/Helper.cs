using UnityEngine;
using System.Collections;

namespace Adnc.SkillTree {
	static public class Helper {
		// @url http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
		static public Quaternion Rotate2D (Vector3 start, Vector3 end) {
			Vector3 diff = start - end;
			diff.Normalize();
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			return Quaternion.Euler(0f, 0f, rot_z - 90f);
		}
	}
}
