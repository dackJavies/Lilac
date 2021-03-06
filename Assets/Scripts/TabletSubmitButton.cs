﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletSubmitButton : SubmitButton {

	private TabletPuzzle myTabletPuzzle;

	void Awake() {
		SetBoxColliderEnabled(false);
	}

	// Use this for initialization
	protected override void Start () {
		myTabletPuzzle = FindUtilities
			.TryFind(this.transform.parent.gameObject, "Puzzle")
			.GetComponent<TabletPuzzle>();
	}

	public void SetBoxColliderEnabled(bool setting) {
		GetComponent<BoxCollider>().enabled = setting;
	}

	protected override void OnMouseUp() {
		if (BoardMutator.dragging != null) {
			return;
		}
		if (myTabletPuzzle.TestSubmission()) {
			myTabletPuzzle.FoundWord();
			if (!myTabletPuzzle.IsComplete()) {
				this.myTabletPuzzle.ReleaseAllKeys();
			}
		} else {
			this.myTabletPuzzle.ReleaseAllKeys();
		}
		myTabletPuzzle.ClearCurrentString();
	}

}
