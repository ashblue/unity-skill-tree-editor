using UnityEngine;
using System.Collections;
using UnityEditor;

public class ComparisonNode : BaseInputNode {

	ComparisonType comparisonType;
	public enum ComparisonType {
		Greater,
		Less,
		Equal
	}
	
	BaseInputNode input1;
	Rect input1Rect;

	BaseInputNode input2;
	Rect input2Rect;

	string compareText = "";

	public ComparisonNode () {
		windowTitle = "Comparison Node";
		hasInputs = true;
	}

	public override void DrawWindow () {
		base.DrawWindow();

		Event e = Event.current;
		comparisonType = (ComparisonType)EditorGUILayout.EnumPopup("Comparison Type", comparisonType);

		// Inpute 1
		string input1Title = "None";
		if (input1) {
			input1Title = input1.GetResult();
		}
		
		GUILayout.Label("Input 1: " + input1Title);
		
		if (e.type == EventType.Repaint) {
			input1Rect = GUILayoutUtility.GetLastRect();
		}

		// Input 2
		string input2Title = "None";
		if (input2) {
			input2Title = input2.GetResult();
		}
		
		GUILayout.Label("Input 2: " + input2Title);
		
		if (e.type == EventType.Repaint) {
			input2Rect = GUILayoutUtility.GetLastRect();
		}
	}

	public override void SetInput (BaseInputNode input, Vector2 clickPos) {
		// Position normalizer for click
		clickPos.x -= windowRect.x;
		clickPos.y -= windowRect.y;
		
		if (input1Rect.Contains(clickPos)) {
			input1 = input;
		} else if (input2Rect.Contains(clickPos)) {
			input2 = input;
		}
	}

	public override string GetResult () {
		float input1Value = 0;
		float input2Value = 0;
		string input1Raw = "";
		string input2Raw = "";
		
		if (input1) {
			input1Raw = input1.GetResult();
			float.TryParse(input1Raw, out input1Value);
		}
		
		if (input2) {
			input2Raw = input2.GetResult();
			float.TryParse(input2Raw, out input2Value);
		}
		
		string result = "false";
		
		switch (comparisonType) {
		case ComparisonType.Equal:
			if (input1Value == input2Value) result = "true";
			break;
		case ComparisonType.Greater:
			if (input1Value > input2Value) result = "true";
			break;
		case ComparisonType.Less:
			if (input1Value < input2Value) result = "true";
			break;
		}
		
		return result;
	}

	public override void NodeDeleted (BaseNode node) {
		if (node.Equals(input1)) {
			input1 = null;
		}
		
		if (node.Equals(input2)) {
			input2 = null;
		}
	}

	public override BaseInputNode ClickedOnInput (Vector2 pos) {
		BaseInputNode retVal = null;
		
		pos.x -= windowRect.x;
		pos.y -= windowRect.y;
		
		if (input1Rect.Contains(pos)) {
			retVal = input1;
			input1 = null;
		} else if (input2Rect.Contains(pos)) {
			retVal = input2;
			input2 = null;
		}
		
		return retVal;
	}
}
