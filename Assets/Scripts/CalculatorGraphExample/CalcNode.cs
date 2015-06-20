using UnityEngine;
using System.Collections;
using UnityEditor;

public class CalcNode : BaseInputNode {
	BaseInputNode input1;
	Rect input1Rect;

	BaseInputNode input2;
	Rect input2Rect;

	CalculationType calculationType;
	public enum CalculationType {
		Addition,
		Subtraction,
		Multiplication,
		Division
	}

	public CalcNode () {
		windowTitle = "Calculation Node";
		hasInputs = true;
	}

	public override void DrawWindow () {
		base.DrawCurves();

		Event e  = Event.current;

		calculationType = (CalculationType)EditorGUILayout.EnumPopup("Calculation Type", calculationType);
		string input1Title = "None";

		if (input1) {
			input1Title = input1.GetResult();
		}

		GUILayout.Label("Input 1: " + input1Title);

		if (e.type == EventType.Repaint) {
			input1Rect = GUILayoutUtility.GetLastRect();
		}


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
		clickPos.x -= windowRect.x;
		clickPos.y -= windowRect.y;

		if (input1Rect.Contains(clickPos)) {
			input1 = input;
		} else if (input2Rect.Contains(clickPos)) {
			input2 = input;
		}
	}

	public override void DrawCurves () {
		if (input1) {
			Rect rect = windowRect;
			rect.x += input1Rect.x;
			rect.y += input1Rect.y + input2Rect.height / 2;
			rect.width = 1;
			rect.height = 1;

			NodeEditor.DrawNodeCurve(input1.windowRect, rect);
		}

		if (input2) {
			Rect rect = windowRect;
			rect.x += input2Rect.x;
			rect.y += input2Rect.y + input2Rect.height / 2;
			rect.width = 1;
			rect.height = 1;
			
			NodeEditor.DrawNodeCurve(input2.windowRect, rect);		}
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

		switch (calculationType) {
		case CalculationType.Addition:
			result = (input1Value + input2Value).ToString();
			break;
		case CalculationType.Division:
			result = (input1Value / input2Value).ToString();
			break;
		case CalculationType.Multiplication:
			result = (input1Value * input2Value).ToString();
			break;
		case CalculationType.Subtraction:
			result = (input1Value - input2Value).ToString();
			break;
		}

		return result;
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

	public override void NodeDeleted (BaseNode node) {
		if (node.Equals(input1)) {
			input1 = null;
		}

		if (node.Equals(input2)) {
			input2 = null;
		}
	}
}
