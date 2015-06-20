using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class NodeEditor : EditorWindow {
	List<BaseNode> windows = new List<BaseNode>();
	Vector2 mousePos;
	BaseNode selectedNode;
	bool makeTransistionMode = false;

	[MenuItem("Window/Node Editor")]
	static void ShowEditor () {
		EditorWindow.GetWindow<NodeEditor>();
	}

	void OnGUI () {
		Event e = Event.current;
		mousePos = e.mousePosition;
		int selectIndex;

		if (e.button == 1 && !makeTransistionMode) {
			if (e.type == EventType.MouseDown) {
				bool clickedOnWindow = false;
				selectIndex = -1;

				for (int i = 0; i < windows.Count; i++) {
					if (windows[i].windowRect.Contains(mousePos)) {
						selectIndex = i;
						clickedOnWindow = true;
						break;
					}
				}

				if (!clickedOnWindow) {
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallback, "inputNode");
					menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallback, "outputNode");
					menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallback, "calcNode");
					menu.AddItem(new GUIContent("Add Comparison Node"), false, ContextCallback, "compNode");

					menu.ShowAsContext();
					e.Use();
				} else {
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Make Transistion"), false, ContextCallback, "makeTransistion");
					menu.AddSeparator("");
					menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

					menu.ShowAsContext();
					e.Use();
				}
			}

		} else if (e.button == 0 && e.type == EventType.MouseDown && makeTransistionMode) {
			bool clickedOnWindow = false;
			selectIndex = -1;
			
			for (int i = 0; i < windows.Count; i++) {
				if (windows[i].windowRect.Contains(mousePos)) {
					selectIndex = i;
					clickedOnWindow = true;
					break;
				}
			}

			if (clickedOnWindow && !windows[selectIndex].Equals(selectedNode)) {
				windows[selectIndex].SetInput((BaseInputNode)selectedNode, mousePos);
				makeTransistionMode = false;
				selectedNode = null;
			}

			// Disable transistion mode
			if (!clickedOnWindow) {
				makeTransistionMode = false;
				selectedNode = null;
			}

			e.Use();

		} else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransistionMode) {
			bool clickedOnWindow = false;
			selectIndex = -1;
			
			for (int i = 0; i < windows.Count; i++) {
				if (windows[i].windowRect.Contains(mousePos)) {
					selectIndex = i;
					clickedOnWindow = true;
					break;
				}
			}

			if (clickedOnWindow) {
				BaseInputNode nodeToChange = windows[selectIndex].ClickedOnInput(mousePos);
				if (nodeToChange != null) {
					selectedNode = nodeToChange;
					makeTransistionMode = true;
				}
			}
		}

		if (makeTransistionMode && selectedNode != null) {
			Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
			DrawNodeCurve(selectedNode.windowRect, mouseRect);

			Repaint();
		}

		foreach (BaseNode n in windows) {
			n.DrawCurves();
		}

		BeginWindows();
		for (int i = 0; i < windows.Count; i++) {
			windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
		}
		EndWindows();
	}

	void DrawNodeWindow (int id) {
		windows[id].DrawWindow();
		GUI.DragWindow();
	}

	void ContextCallback (object obj) {
		string clb = obj.ToString();
		if (clb.Equals("inputNode")) {
			InputNode inputNode = ScriptableObject.CreateInstance<InputNode>();
			inputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

			windows.Add(inputNode);

		} else if (clb.Equals("outputNode")) {
			OutputNode outputNode = ScriptableObject.CreateInstance<OutputNode>();
			outputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

			windows.Add(outputNode);

		} else if (clb.Equals("calcNode")) {
			CalcNode calcNode = ScriptableObject.CreateInstance<CalcNode>();
			calcNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

			windows.Add(calcNode);

		} else if (clb.Equals("compNode")) {
			ComparisonNode comparisonNode = ScriptableObject.CreateInstance<ComparisonNode>();
			comparisonNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

			windows.Add(comparisonNode);

		} else if (clb.Equals("makeTransistion")) {
			bool clickedOnWindow = false;
			int selectIndex = -1;
			
			for (int i = 0; i < windows.Count; i++) {
				if (windows[i].windowRect.Contains(mousePos)) {
					selectIndex = i;
					clickedOnWindow = true;
					break;
				}
			}

			if (clickedOnWindow) {
				Debug.Log("Transition being made");
				selectedNode = windows[selectIndex];
				makeTransistionMode = true;
			}

		} else if (clb.Equals("deleteNode")) {
			bool clickedOnWindow = false;
			int selectIndex = -1;
			
			for (int i = 0; i < windows.Count; i++) {
				if (windows[i].windowRect.Contains(mousePos)) {
					selectIndex = i;
					clickedOnWindow = true;
					break;
				}
			}

			if (clickedOnWindow) {
				BaseNode selNode = windows[selectIndex];
				windows.RemoveAt(selectIndex);

				foreach (BaseNode n in windows) {
					n.NodeDeleted(selNode);
				}
			}
		}
	}

	public static void DrawNodeCurve (Rect start, Rect end) {
		Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
		Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;
		Color shadowCol = new Color(0, 0, 0, 0.06f);

		for (int i = 0; i < 3; i++) {
			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
		}

		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
	}
}
