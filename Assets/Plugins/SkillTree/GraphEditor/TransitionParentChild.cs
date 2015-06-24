using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	/// <summary>
	/// Used to represent a parent to child relationship for transitions in a single class
	/// </summary>
	public class TransitionParentChild {
		public SkillCollectionBase parent;
		public SkillCollectionBase child;
	}
}