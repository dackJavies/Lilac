using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireChain : MonoBehaviour {

	public void PuzzleSolved() {
		ChangeColor(ColorUtilities.SOLVED);
	}

	private void ChangeColor(Color c) {
		GameObject wire;
		MeshRenderer currentMesh;
		foreach(Transform child in this.transform) {
			wire = child.gameObject;
			currentMesh = wire.GetComponent<MeshRenderer>();
			if (currentMesh != null) {
				currentMesh.material.color = c;
			}
		}
	}

	public void RemoveTablet() {
		ChangeColor(ColorUtilities.WRONG);
	}

	public void PlaceTablet() {
		ChangeColor(ColorUtilities.SOLVED);
	}

}
