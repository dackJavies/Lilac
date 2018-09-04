using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {

	private Ray mouseRay;
	private RaycastHit hit;
	private BoxCollider bc;

	// Use this for initialization
	void Start () {
	//	bc = this.gameObject.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(2) || Input.GetKeyUp("e")) {
//			bc.enabled = false;
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouseRay, out hit)) {
				if (hit.collider.tag == "Door") {
					Object.Destroy(hit.collider.gameObject);
				}
			}
//			bc.enabled = true;
		}
	}
}
