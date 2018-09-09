using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ZoomToKeypad : MonoBehaviour {

	private GameObject controller, character, clickCollider;
	private Transform playerHead;
	private Vector3 KEYPAD_CAM_POSITION = new Vector3(0f, 0f, -1.5f);
	private Vector3 GIFT_CAM_POSITION = new Vector3(0f, 0f, -2.5f);
	private Vector3	KEYPAD_CAM_LOCALEULER = new Vector3(0f, 0f, 0f);
	private Vector3	KEYPAD_CAM_LOCALSCALE = new Vector3(1f, 1f, 1f);

	private Ray mouseRay;
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
		this.controller = FindUtilities.TryFind("FPSController");
		this.character = FindUtilities.TryFind(controller, "FirstPersonCharacter");
		this.clickCollider = FindUtilities.TryFind(character, "ClickCollider");
	}

	void Update() {
		if (Input.GetMouseButtonUp(0)) {
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouseRay, out hit, 2.0f)) {
//				Debug.Log("hit something or other: " + hit.collider.tag);
				Interact(hit.collider);
			} else {
				if (this.character.transform.parent != this.controller.transform) {
					this.ExitPuzzle();
				}
			}
		} else if ((Input.GetKey("w") || Input.GetKey("a") ||
					Input.GetKey("s") || Input.GetKey("d")) &&
					this.character.transform.parent != this.controller.transform) {
			ExitPuzzle();
		}
	}

	private void SavePlayerHeadTransform() {
		this.playerHead = character.transform;
	}

	private void Interact(Collider other) {
		if (other.tag == "Keypad") {
			this.controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
			PlayerInventory.PuzzleView();
			controller.GetComponent<CharacterController>().enabled = false;
			GameObject keypad = other.gameObject.transform.parent.gameObject;
			SavePlayerHeadTransform();
			FindUtilities.TryFind(keypad.transform.parent.gameObject, "ExitButton").GetComponent<ExitButton>().SetPlayerHead(this.playerHead);
			FindUtilities.TryFind(keypad.transform.parent.gameObject, "SubmitButton")
				.GetComponent<BoxCollider>().enabled = true;
			this.controller.GetComponent<FirstPersonController>().enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.character.transform.parent = keypad.transform;
			this.character.transform.localPosition = KEYPAD_CAM_POSITION;
			this.character.transform.localEulerAngles = KEYPAD_CAM_LOCALEULER;
			Puzzle p = FindUtilities.TryFind(keypad, "Puzzle").GetComponent<Puzzle>();
			p.SetEnabledKeys(true);
			Hint.SetHint(p.buildUp, p.words);
			p.FillInHint();
			this.controller.GetComponent<ZoomToTablet>().enabled = false;
		} else if (other.tag == "Gift") {
			this.controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
			PlayerInventory.PuzzleView();
			controller.GetComponent<CharacterController>().enabled = false;
			GameObject keypad = other.gameObject.transform.parent.gameObject;
			SavePlayerHeadTransform();
			FindUtilities.TryFind(keypad.transform.parent.gameObject, "ExitButton").GetComponent<ExitButton>().SetPlayerHead(this.playerHead);
			this.controller.GetComponent<FirstPersonController>().enabled = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.character.transform.parent = keypad.transform;
			this.character.transform.localPosition = GIFT_CAM_POSITION;
			this.character.transform.localEulerAngles = KEYPAD_CAM_LOCALEULER;
			this.controller.GetComponent<ZoomToTablet>().enabled = false;		
			Puzzle p = FindUtilities.TryFind(keypad, "Puzzle").GetComponent<GiftPuzzle>();
			p.SetEnabledKeys(true);
		} else if (other.tag == "Dock" && PlayerInventory.HasTablet()) {
			this.controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
			PlayerInventory.PlaceTablet(other.gameObject.transform.parent);
			other.gameObject.GetComponent<Dock>().ReturnTablet();
		} else if (other.tag == "Tablet" && !PlayerInventory.HasTablet()) {
			this.controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
			if (!PlayerInventory.HasTablet()) {
				FindUtilities
					.TryFind(other.gameObject.transform.parent.parent.gameObject, "DockBlock")
					.GetComponent<Dock>().TakeTablet();
				PlayerInventory.PickUpTablet(other.transform.parent.gameObject);
			} else {
				Debug.Log("Already holding a tablet.");
			}
		} else if (other.tag != "Key" && other.tag != "DetachableKey" &&
					other.tag != "NormalKey" && other.tag != "BlankKey" &&
					other.tag != "Keypad" && other.tag != "SubmitButton") {
			if (this.character.transform.parent != this.controller.transform) {
				this.ExitPuzzle();
			}
		}
	}

	private void ExitPuzzle() {
		ExitButton eb = FindUtilities
			.TryFind(this.character.transform.parent.parent.gameObject, "ExitButton")
			.GetComponent<ExitButton>();
		eb.ExitPuzzle();
	}

}
