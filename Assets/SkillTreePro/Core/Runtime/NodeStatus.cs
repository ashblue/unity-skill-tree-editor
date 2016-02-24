using UnityEngine;
using System.Collections;

namespace Adnc.SkillTreePro {
	public enum NodeStatus {
		Hidden, // Not visible, cascades to parent nodes
		Locked, // Locked and unable to be interacted with
		Purchasable // All requirements have been met to purchase 
	}
}

