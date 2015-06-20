using UnityEngine;
using System.Collections;
using UnityEditor;

public class InputNode : BaseInputNode {
	InputType inputType;

	public enum InputType {
		Number,
		Randomization
	}

	string randomFrom = "";
	string randomTo = "";
	string inputValue = "";

	public InputNode () {
		windowTitle = "Input Node";
	}

	public override void DrawWindow () {
		base.DrawWindow();
		inputType = (InputType)EditorGUILayout.EnumPopup("Input type: ", inputType);

		if (inputType == InputType.Number) {
			inputValue = EditorGUILayout.TextField("Value", inputValue);
		} else if (inputType == InputType.Randomization) {
			randomFrom = EditorGUILayout.TextField("From", randomFrom);
			randomTo = EditorGUILayout.TextField("To", randomTo);

			if (GUILayout.Button("Calculate Random")) {
				CalculateRandom();
			}
		}
	}

	public override void DrawCurves () {

	}

	void CalculateRandom () {
		float rFrom = 0;
		float rTo = 0;

		float.TryParse(randomFrom, out rFrom);
		float.TryParse(randomTo, out rTo);

		int randFrom = (int)(rFrom * 10);
		int randTo = (int)(rTo * 10);

		int selected = UnityEngine.Random.Range(randFrom, randTo + 1);

		float selectedValue = selected / 10;

		inputValue = selectedValue.ToString();
	}

	public override string GetResult () {
		return inputValue.ToString();
	}
}
