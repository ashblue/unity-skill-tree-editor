using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree.Example.MultiCategory {
	public class SkillMenu : MonoBehaviour {
		[SerializeField] SkillTreeBase skillTree;

		[Header("Header")]
		[SerializeField] Transform categoryContainer;
		[SerializeField] GameObject categoryButtonPrefab;
		[SerializeField] Text skillOutput;
		[SerializeField] Text categoryName;

		[Header("Nodes")]
		[SerializeField] GameObject linePrefab;
		[SerializeField] Transform nodeContainer;
		[SerializeField] GameObject nodeRowPrefab;
		[SerializeField] GameObject nodePrefab;

		void Start () {
			SkillCategoryBase[] skillCategories = skillTree.GetCategories();

			// Clear out test categories
			foreach (Transform child in categoryContainer) {
				Destroy(child.gameObject);
			}

			// Populate categories
			foreach (SkillCategoryBase category in skillCategories) {
				GameObject go = Instantiate(categoryButtonPrefab);
				go.transform.SetParent(categoryContainer);
				go.transform.localScale = Vector3.one;
				
				Text txt = go.GetComponentInChildren<Text>();
				txt.text = category.displayName;

				// Dump in a tmp variable to force capture the variable by the event
				SkillCategoryBase tmpCat = category; 
				go.GetComponent<Button>().onClick.AddListener(() => {
					ShowCategory(tmpCat);
				});
			}

//			DrawLine(nodeContainer, new Vector3(50f, 50f), new Vector3(25f, 75f));
			if (skillCategories.Length > 0) {
				ShowCategory(skillCategories[0]);
			}

			Repaint();
		}

		void DrawLine (Transform container, Vector3 start, Vector3 end) {
			float angle = Vector2.Angle(start, end);

			GameObject go = Instantiate(linePrefab);
			go.transform.SetParent(container);
			go.transform.localScale = Vector3.one;
			go.transform.Rotate(new Vector3(0f, 0f, angle));
			go.GetComponent<RectTransform>().anchoredPosition = start;
		}

		void ShowCategory (SkillCategoryBase category) {
			categoryName.text = string.Format("{0}: Level {1}", category.displayName, category.skillLv);

			foreach (Transform child in nodeContainer) {
				Destroy(child.gameObject);
			}

			// Generate node row data
			List<List<SkillCollectionBase>> rows = new List<List<SkillCollectionBase>>();
			List<SkillCollectionBase> rootNodes = category.GetRootSkillCollections();
			rows.Add(rootNodes);
			RecursiveRowAdd(rows);

			// Output proper rows
			foreach (List<SkillCollectionBase> row in rows) {
				GameObject nodeRow = Instantiate(nodeRowPrefab);
				nodeRow.transform.SetParent(nodeContainer);
				nodeRow.transform.localScale = Vector3.one;
				
				foreach (SkillCollectionBase rowItem in row) {
					GameObject node = Instantiate(nodePrefab);
					node.transform.SetParent(nodeRow.transform);
					node.transform.localScale = Vector3.one;

					node.GetComponentInChildren<Text>().text = rowItem.displayName;
				}
			}

//			nodeContainer
					
					// Populate skill collections and groups procedurally with lines drawn
		}

		void RecursiveRowAdd (List<List<SkillCollectionBase>> rows) {
			Debug.Log("New row");
			List<SkillCollectionBase> row = new List<SkillCollectionBase>();
			foreach (SkillCollectionBase collection in rows[rows.Count - 1]) {
				foreach (SkillCollectionBase child in collection.childSkills) {
					// @TODO We need to remove any duplicate entries (keep a record of every node added for ref)
					if (!row.Contains(child)) {
						Debug.Log("Item added " + child.displayName);
						row.Add(child);
					}
				}
			}

			if (row.Count > 0) {
				rows.Add(row);
				RecursiveRowAdd(rows);
			}
		}

		void Repaint () {
			skillOutput.text = "Skill Points: " + skillTree.skillPoints;
		}
	}
}

