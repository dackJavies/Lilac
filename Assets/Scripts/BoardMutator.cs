using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMutator : MonoBehaviour {

	private Ray mouseRay;
	private RaycastHit hit;

	public GameObject DetachablePrefab;
	public GameObject BlankPrefab;

	public static GameObject dragging;
	private DetachableKey draggingKey;

	private static GameObject homeObject;
	private static Key homeKey;
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonUp(1)) {
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouseRay, out hit, 2.0f)) {
				if (hit.collider.tag == "DetachableKey") {
					if (PlayerInventory.HasTablet()) {
							GameObject keyObject = hit.collider.gameObject;
							DetachableKey key = keyObject.GetComponent<DetachableKey>();
						if (!key.IsPressed()) {
							if (dragging != null) {
								char temp = draggingKey.GetValue();
								draggingKey.SetValue(key.GetValue());
								key.SetValue(temp);
							} else {
								dragging = Object.Instantiate(DetachablePrefab, FindUtilities.TryFind("FirstPersonCharacter").transform);
								draggingKey = dragging.GetComponent<DetachableKey>();
								draggingKey.SetValue(key.GetValue());
								dragging.GetComponent<BoxCollider>().enabled = false;
								draggingKey.Drag();
								BlankKey bk = KeyUtilities.ConvertKey<DetachableKey, BlankKey>(
									ref keyObject, ColorUtilities.BLANK, "BlankKey"
								);
								bk.gameObject.transform.parent.gameObject.GetComponent<Puzzle>().ResetBoardAt(bk.GetRow(), bk.GetCol(), bk);
							}
						}
					} else {
						Debug.Log("COME BACK WITH A TABLET LOSER");
					}
				} else if (hit.collider.tag == "BlankKey") {
					if (dragging != null) {
						GameObject keyObject = hit.collider.gameObject;
						BlankKey key = keyObject.GetComponent<BlankKey>();
						char val = draggingKey.GetValue();
						Object.Destroy(draggingKey);
						Object.Destroy(dragging);
						dragging = null;
						draggingKey = null;
						DetachableKey placed = KeyUtilities.ConvertKey<BlankKey, DetachableKey>(
							ref keyObject, ColorUtilities.DETACHABLE, "DetachableKey"
						);
						placed.SetValue(val);
					}
				}
			}
		}*/
		if (Input.GetMouseButtonDown(1)) {
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouseRay, out hit, 2.0f)) {
				if (hit.collider.tag == "DetachableKey") {
					PickUpKey(hit.collider.gameObject);
				}
			}
		} else if (Input.GetMouseButtonUp(1)) {
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouseRay, out hit, 2.0f)) {
				if (hit.collider.tag == "DetachableKey") {
					SwapKeys(hit.collider.gameObject);
				} else if (hit.collider.tag == "BlankKey") {
					PutDownKey(hit.collider.gameObject);
				} else {
					DropKey();
				}
			} else {
				DropKey();
			}
		}
	}

	private void PickUpKey(GameObject target) {
		homeObject = target;
		homeKey = target.GetComponent<DetachableKey>();
		dragging = Object.Instantiate(DetachablePrefab, FindUtilities.TryFind("FirstPersonCharacter").transform);
		draggingKey = dragging.GetComponent<DetachableKey>();
		draggingKey.SetValue(homeKey.GetValue());
		dragging.GetComponent<BoxCollider>().enabled = false;
		draggingKey.Drag();
	}

	private void PutDownKey(GameObject target) {
		BlankKey bk = KeyUtilities.ConvertKey<DetachableKey, BlankKey>(
			ref homeObject, ColorUtilities.BLANK, "BlankKey"
		);
		bk.gameObject.transform.parent.gameObject.GetComponent<Puzzle>().ResetBoardAt(bk.GetRow(), bk.GetCol(), bk);
		BlankKey targetKey = target.GetComponent<BlankKey>();
		char val = draggingKey.GetValue();
		Object.Destroy(dragging);
		dragging = null;
		draggingKey = null;
		DetachableKey placed = KeyUtilities.ConvertKey<BlankKey, DetachableKey>(
			ref target, ColorUtilities.DETACHABLE, "DetachableKey"
		);
		placed.SetValue(val);
		placed.gameObject.transform.parent.GetComponent<Puzzle>().ResetBoardAt(placed.GetRow(), placed.GetCol(), placed);
	}

	private void SwapKeys(GameObject target) {
		DetachableKey targetKey = target.GetComponent<DetachableKey>();
		char temp = targetKey.GetValue();
		targetKey.SetValue(homeKey.GetValue());
		homeKey.SetValue(temp);
		target.transform.parent.GetComponent<Puzzle>().ResetBoardAt(targetKey.GetRow(), targetKey.GetCol(), targetKey);
		homeObject.transform.parent.GetComponent<Puzzle>().ResetBoardAt(homeKey.GetRow(), homeKey.GetCol(), homeKey);
		Object.Destroy(dragging);
		dragging = null;
		draggingKey = null;
	}

	private void DropKey() {
		Object.Destroy(dragging);
		dragging = null;
		draggingKey = null;
	}

}
