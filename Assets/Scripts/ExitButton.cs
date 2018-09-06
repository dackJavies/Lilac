using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class ExitButton : MonoBehaviour {

	// Hierarchy of player's game objects
	protected GameObject controller;
	protected GameObject character;
	protected GameObject clickCollider;

	protected Ray mouseRay;
	protected RaycastHit hit;

	protected Transform playerHead;

	// Use this for initialization
	void Start () {
		controller = FindUtilities.TryFind("FPSController");
		character = FindUtilities.TryFind(controller, "FirstPersonCharacter");
		clickCollider = FindUtilities.TryFind(character, "ClickCollider");
	}

	protected virtual void OnMouseUp() {
		ExitPuzzle();
	}

	public void ExitPuzzle() {
		if (controller != null && BoardMutator.dragging == null) {
			PlayerInventory.BackToWalkingView();
			if (PlayerInventory.HasTablet()) {
				PlayerInventory.ClearCurrent();
			}
			controller.GetComponent<CharacterController>().enabled = true;
			Puzzle p = FindUtilities.TryFind(
				FindUtilities.TryFind(this.transform.parent.gameObject, "Pad"),
				"Puzzle")
				.GetComponent<Puzzle>();
			if (!p.IsComplete()) {
				p.ReleaseAllKeys();
				p.ClearCurrentString();
			}
			p.SetEnabledKeys(false);
			if (transform.parent.tag != "Gift") {
				Light l = FindUtilities
					.TryFind(this.transform.parent.gameObject, "Light")
					.GetComponent<Light>();
				if (p.HasLeftovers()) {
					l.TurnOn();
				} else {
					l.TurnOff();
				}
			}
			this.controller.GetComponent<FirstPersonController>().enabled = true;
			this.character.transform.parent = controller.transform;
			this.character.transform.localPosition = this.playerHead.localPosition;
			this.character.transform.localEulerAngles = this.playerHead.localEulerAngles;
			this.character.transform.localScale = this.playerHead.localScale;
			Hint.ClearHint();
			this.controller.GetComponent<ZoomToTablet>().enabled = true;
			FindUtilities.TryFind(transform.parent.gameObject, "SubmitButton")
				.GetComponent<BoxCollider>().enabled = false;
		}
	}

	public void SetPlayerHead(Transform pHead) {
		this.playerHead = pHead;
	}

}
