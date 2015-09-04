using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree.Example.MultiCategory {
	public class SkillMenu : MonoBehaviour {
		Dictionary<SkillCollectionBase, SkillNode> nodeRef;
		List<SkillNode> skillNodes;

		public SkillTreeBase skillTree;

		[Header("Header")]
		[SerializeField] Transform categoryContainer;
		[SerializeField] GameObject categoryButtonPrefab;
		[SerializeField] Text skillOutput;
		[SerializeField] Text categoryName;

		[Header("Nodes")]
		[SerializeField] Transform nodeContainer;
		[SerializeField] RectTransform nodeInnerContainer;
		[SerializeField] GameObject nodeRowPrefab;
		[SerializeField] GameObject nodePrefab;
		[SerializeField] Color colorUnlock;
		[SerializeField] Color colorPurchase;
		[SerializeField] Color colorLock;

		[Tooltip("How large of a space each node takes up")]
		[SerializeField] Vector2 cellSize;

		[Header("Node Lines")]
		[SerializeField] Transform lineContainer;
		[SerializeField] GameObject linePrefab;
		[SerializeField] Color lineColor;

		[Header("Context Sidebar")]
		[SerializeField] RectTransform sidebarContainer;
		[SerializeField] Text sidebarTitle;
		[SerializeField] Text sidebarBody;
		[SerializeField] Text sidebarRequirements;
		[SerializeField] Text sidebarPurchasedMessage;
		[SerializeField] Button sidebarPurchase;

		void Start () {
			// Clear out test categories
			foreach (Transform child in categoryContainer) {
				Destroy(child.gameObject);
			}

			// Populate categories
			SkillCategoryBase[] skillCategories = skillTree.GetCategories();
			foreach (SkillCategoryBase category in skillCategories) {
				GameObject go = Instantiate(categoryButtonPrefab);
				go.transform.SetParent(categoryContainer);
				go.transform.localScale = Vector3.one;
				
				Text txt = go.GetComponentInChildren<Text>();
				txt.text = category.displayName;

				// Dump in a tmp variable to force capture the variable by the event
				SkillCategoryBase tmpCat = category; 
				go.GetComponent<Button>().onClick.RemoveAllListeners();
				go.GetComponent<Button>().onClick.AddListener(() => {
					ShowCategory(tmpCat);
				});
			}

			if (skillCategories.Length > 0) {
				ShowCategory(skillCategories[0]);
			}
		}

		public void ShowCategory (SkillCategoryBase category) {
			skillNodes = new List<SkillNode>();
			nodeRef = new Dictionary<SkillCollectionBase, SkillNode>();
			categoryName.text = string.Format("{0}: Level {1}", category.displayName, category.skillLv);
			ClearDetails();

			CreateGrid(category, cellSize);

			StartCoroutine(ConnectNodes());
		}

		void CreateGrid (SkillCategoryBase category, Vector2 cellSize) {
			// Clean up pre-existing data
			foreach (Transform child in nodeContainer) {
				Destroy(child.gameObject);
			}
			
			foreach (Transform child in lineContainer) {
				Destroy(child.gameObject);
			}

			SkillCollectionGrid grid = category.GetComponentInParent<SkillTreeBase>().GetGrid(category);

			// Generate container with width and height based on cellSize
			nodeInnerContainer.sizeDelta = new Vector2(grid.Width * cellSize.x, grid.Height * cellSize.y);

			// Adjust the container position based on padding, resulting in perfectly aligned grid items
			RectTransform nodeRect = nodePrefab.GetComponent<RectTransform>();
			Vector2 cellPadding = new Vector2((cellSize.x - nodeRect.sizeDelta.x) / 2f, (cellSize.y - nodeRect.sizeDelta.y) / 2f);

			nodeRef = new Dictionary<SkillCollectionBase, SkillNode>();

			// Place all grid items
			foreach (SkillCollectionGridItem gridItem in grid.GetAllCollections()) {
				GameObject node = Instantiate(nodePrefab);
				node.transform.SetParent(nodeContainer);
				node.transform.localScale = Vector3.one;
				node.GetComponentInChildren<Text>().text = gridItem.collection.displayName;
				node.GetComponent<RectTransform>().anchoredPosition = new Vector2((gridItem.x * cellSize.x) + cellPadding.x, (gridItem.y * cellSize.y * -1f) - cellPadding.y);

				SkillNode skillNode = node.GetComponent<SkillNode>();
				skillNode.menu = this;
				skillNode.skillCollection = gridItem.collection;
				skillNodes.Add(skillNode);

				nodeRef.Add(gridItem.collection, skillNode);
			}
		}

		void UpdateNodes () {
			foreach (SkillNode node in skillNodes) {
				node.SetStatus(NodeStatus.Locked, colorLock);

				if (node.skillCollection.Skill.unlocked) {
					node.SetStatus(NodeStatus.Unlocked, colorUnlock); // Fully purchased
				} else if (skillTree.skillPoints > 0 && node.skillCollection.Skill.IsRequirements()) {
					node.SetStatus(NodeStatus.Purchasable, colorPurchase); // Avaialable for purchase
				}
			}
		}

		// Done after a frame skip so they nodes are sorted properly into position
		IEnumerator ConnectNodes () {
			// We have to skip a frame so the Unity GUI has a chance to properly recalculate all the positioning
			yield return null;

			foreach (SkillNode node in nodeContainer.GetComponentsInChildren<SkillNode>()) {
				foreach (SkillCollectionBase child in node.skillCollection.childSkills) {
					// @NOTE We must translate a center point on the node into a transform position for accurary of the line
					Vector3 lineStart = node.transform.GetChild(0).position;
					Vector3 lineEnd = nodeRef[child].transform.GetChild(0).position;
					DrawLine(lineContainer, lineStart, lineEnd, lineColor);
				}
			}

			Repaint();
		}

		void DrawLine (Transform container, Vector3 start, Vector3 end, Color color) {
			GameObject go = Instantiate(linePrefab);
			go.GetComponent<Image>().color = color;

			// Adjust the layering so it appears underneath
			go.transform.SetParent(container);
			go.transform.localScale = Vector3.one;
			go.transform.SetSiblingIndex(0);

			// Adjust height to proper sizing
			RectTransform rectTrans = go.GetComponent<RectTransform>();
			Rect rect = rectTrans.rect;
			rect.height = Vector3.Distance(start, end);
			rectTrans.sizeDelta = new Vector2(rect.width, rect.height);

			// Adjust rotation and placement
			go.transform.rotation = Helper.Rotate2D(start, end);
			go.transform.position = start;
		}

		/// <summary>
		/// Recursively adds rows.
		/// </summary>
		/// <param name="rows">Rows.</param>
		/// <param name="history">History of all added items so we don't accidentally add two of the same thing</param>
		void RecursiveRowAdd (List<List<SkillCollectionBase>> rows, Dictionary<SkillCollectionBase, bool> history) {
			List<SkillCollectionBase> row = new List<SkillCollectionBase>();
			foreach (SkillCollectionBase collection in rows[rows.Count - 1]) {
				foreach (SkillCollectionBase child in collection.childSkills) {
					if (!row.Contains(child) && !history.ContainsKey(child)) {
						row.Add(child);
						history[child] = true;
					}
				}
			}

			if (row.Count > 0) {
				rows.Add(row);
				RecursiveRowAdd(rows, history);
			}
		}

		public void ShowNodeDetails (SkillNode node) {
			SkillCollectionBase skillCollection = node.skillCollection;
			NodeStatus status = node.GetStatus();

			sidebarTitle.text = string.Format("{0}: Lv {1}", skillCollection.displayName, skillCollection.SkillIndex + 1);
			sidebarBody.text = skillCollection.Skill.description;

			string requirements = skillCollection.Skill.GetRequirements();
			if (string.IsNullOrEmpty(requirements)) {
				sidebarRequirements.gameObject.SetActive(false);
			} else {
				sidebarRequirements.text = "<b>Requirements:</b> \n" + skillCollection.Skill.GetRequirements();
				sidebarRequirements.gameObject.SetActive(true);
			}

			if (status == NodeStatus.Purchasable) {
				sidebarPurchasedMessage.gameObject.SetActive(false);
				sidebarPurchase.gameObject.SetActive(true);
				sidebarPurchase.onClick.RemoveAllListeners();
				sidebarPurchase.onClick.AddListener(() => {
					skillCollection.Purchase();
					UpdateNodes();
					ShowNodeDetails(node);
					UpdateSkillPoints();
				});
			} else if (status == NodeStatus.Unlocked) {
				sidebarPurchasedMessage.gameObject.SetActive(true);
				sidebarPurchase.gameObject.SetActive(false);
			} else {
				sidebarPurchasedMessage.gameObject.SetActive(false);
				sidebarPurchase.gameObject.SetActive(false);
			}

			sidebarContainer.gameObject.SetActive(true);
		}

		void ClearDetails () {
			sidebarContainer.gameObject.SetActive(false);
		}

		void UpdateSkillPoints () {
			skillOutput.text = "Skill Points: " + skillTree.skillPoints;
		}

		void Repaint () {
			UpdateSkillPoints();
			UpdateNodes();
		}
	}
}

