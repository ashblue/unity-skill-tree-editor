using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillCollectionDefinition : SkillDefinitionBase {
		/// <summary>
		/// Determines if an editing interace is shown in the sidebar
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
		virtual public bool Editable {
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Adnc.SkillTreePro.SkillCollectionDefinition"/> allows parent nodes.
		/// </summary>
		/// <value><c>true</c> if allow parents; otherwise, <c>false</c>.</value>
		virtual public bool AllowParents {
			get { return true; }
		}

		[Tooltip("Hide from skill tree print view")]
		public bool hidden;

		public NodeData node = new NodeData();

		[Tooltip("Determines what skill is currently active and returns it")]
		public int skillIndex = 0;

		[Tooltip("Connected collections")]
		public List<string> childCollections = new List<string>();

		[Tooltip("Tiers of skills")]
		public List<string> skills = new List<string>();

		public SkillCollectionDefinition (SkillCategoryDefinition cat) {
			if (cat == null) return;

			SkillDefinition def = new SkillDefinition(cat);
			skills.Add(def.uuid);

			cat.skillCollections.Add(this);
		}
	}
}
