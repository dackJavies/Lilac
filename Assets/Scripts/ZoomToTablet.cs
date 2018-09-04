using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ZoomToTablet : MonoBehaviour {

	private GameObject controller, character, clickCollider;
	private bool solving;

	void Start() {
		this.controller = this.gameObject;
		this.character = FindUtilities.TryFind(this.controller, "FirstPersonCharacter");
		this.clickCollider = FindUtilities.TryFind(this.character, "ClickCollider");
		solving = false;
	}

	void Update() {
		if (Input.GetKeyUp("q")) {
			if (PlayerInventory.HasTablet()) {
				if (this.solving) {
					Debug.Log("now trying to unsolve");
					this.controller.GetComponent<CharacterController>().enabled = true;
					this.controller
						.GetComponent<FirstPersonController>()
						.enabled = true;
					this.solving = false;
					if (!PlayerInventory.GetPuzzle().IsComplete()) {
						PlayerInventory.GetPuzzle().SetEnabledKeys(false);
						PlayerInventory.GetPuzzle().ReleaseAllKeys();
						PlayerInventory.GetPuzzle().ClearCurrentString();
					}
					Hint.ClearHint();
					this.character.transform.parent = this.controller.transform;
					PlayerInventory.GetTablet().transform.parent = this.character.transform;
					PlayerInventory.BackToWalkingView();
				} else {
					this.controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
					Debug.Log("now trying to solve");
					this.controller.GetComponent<CharacterController>().enabled = false;
					this.controller
						.GetComponent<FirstPersonController>()
						.enabled = false;
					Cursor.lockState = CursorLockMode.None;
					this.solving = true;
					if (!PlayerInventory.GetPuzzle().IsComplete()) {
						PlayerInventory.GetPuzzle().SetEnabledKeys(true);
					}
					Hint.SetHint(PlayerInventory.GetPuzzle().buildUp, PlayerInventory.GetPuzzle().words);
					PlayerInventory.GetPuzzle().FillInHint();
					PlayerInventory.SolvingView();
					PlayerInventory.GetTablet().transform.parent = null;
					this.character.transform.parent = 
						FindUtilities.TryFind(
							PlayerInventory.GetPuzzle().gameObject.transform.parent.gameObject,
							"Block").transform;
				}
			}
		}
	}

}
