using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitButton : MonoBehaviour {

	private Puzzle myPuzzle;

	// Use this for initialization
	protected virtual void Start () {
		myPuzzle = FindUtilities.TryFind(
			FindUtilities.TryFind(this.transform.parent.gameObject, "Pad"),
			"Puzzle").GetComponent<Puzzle>();
		GetComponent<BoxCollider>().enabled = false;
	}
	
	protected virtual void OnMouseUp() {
		if (BoardMutator.dragging != null) {
			return;
		}
		if (myPuzzle.TestSubmission()) {
			myPuzzle.ConvertPressedDetachablesToVanilla();
			myPuzzle.FoundWord();
			if (!myPuzzle.IsComplete()) {
				this.myPuzzle.ReleaseAllKeys();
			}
		} else {
			this.myPuzzle.ReleaseAllKeys();
		}
		myPuzzle.ClearCurrentString();
	}

}
