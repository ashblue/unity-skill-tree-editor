using UnityEngine;
using System.Collections;

public class BaseInputNode : BaseNode {
	public virtual string GetResult () {
		return "None";
	}

	public override void DrawCurves () {

	}
}
