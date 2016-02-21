using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public abstract class SkillCollectionDefinitionBase : SkillDefinitionBase {
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

		[Tooltip("Hide from skill node and all children from printed view")]
		public bool hidden;

		[HideInInspector] public NodeData node = new NodeData();
		[HideInInspector] public int skillIndex = 0;
		[HideInInspector] public List<string> childCollections = new List<string>();
		[HideInInspector] public List<string> skills = new List<string>();

		/// <summary>
		/// Determines if this object is in drag mode in the editor
		/// </summary>
		[System.NonSerialized] public bool _drag;

		public virtual void Setup (SkillCategoryDefinition cat) {
			Setup();

			if (cat == null) return;

			SkillDefinition def = new SkillDefinition(cat);
			skills.Add(def.uuid);

			cat.skillCollections.Add(this);
		}
	}
}
