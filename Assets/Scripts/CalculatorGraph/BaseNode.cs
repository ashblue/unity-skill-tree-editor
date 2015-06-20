using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class BaseNode : ScriptableObject {
	public Rect windowRect; // Rectangle for the current window
	public bool hasInputs = false; // Does it have inputs
	public string windowTitle = ""; // Title displayed for the module

	public virtual void DrawWindow () {
		windowTitle = EditorGUILayout.TextField("Title", windowTitle);
	}

	// Output of curve data
	public abstract void DrawCurves();

	public virtual void SetInput (BaseInputNode input, Vector2 clickPos) {

	}

	public virtual void NodeDeleted (BaseNode node) {

	}

	public virtual BaseInputNode ClickedOnInput (Vector2 pos) {
		return null;
	}
}
