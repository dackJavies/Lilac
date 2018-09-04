using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletPuzzle : Puzzle {

	public GameObject LinkedDoor;

	/*public override FoundWord() {
		foundWords[foundIndex++] = currentString.text;
		if (foundIndex >= words.Length) {
			this.CompletePuzzle();
		}
	}*/

	protected override void ShowBuildUp() {
		FindUtilities
			.TryFind(this.LinkedDoor.transform.parent.gameObject, "BuildUp")
			.GetComponent<TextMesh>()
			.text = buildUp;
	}

	public override void CompletePuzzle() {
		this.complete = true;
		FindUtilities
			.TryFind(this.transform.parent.gameObject, "SubmitButton")
			.GetComponent<TabletSubmitButton>()
			.SetBoxColliderEnabled(false);
		this.RemoveDoor();
		FindUtilities
			.TryFind(this.transform.parent.gameObject, "Block")
			.GetComponent<MeshRenderer>().material.color = ColorUtilities.SOLVED;
	}

	public override void RemoveDoor() {
		Object.Destroy(LinkedDoor);
	}

	protected override void ChangeWireChain() {
		return;
	}
	
	public override void Exit() {

	}

	public void RemovedFromDock() {
		if (wireChain != null) {
			wireChain.GetComponent<WireChain>().RemoveTablet();
		}
	}

	public void PlacedOnDock() {
		if (wireChain != null) {
			wireChain.GetComponent<WireChain>().PlaceTablet();
		}
	}

}
