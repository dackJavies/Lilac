using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {

	private Ray mouseRay;
	private RaycastHit hit;

	void Update () {
		if (Input.GetMouseButtonUp(2) || Input.GetKeyUp("e")) {
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouseRay, out hit)) {
				if (hit.collider.tag == "Door") {
					Object.Destroy(hit.collider.gameObject);
				}
			}
		}
	}
}
