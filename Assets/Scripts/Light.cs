using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour {

	private MeshRenderer myMeshRenderer;

	// Use this for initialization
	void Start () {
		myMeshRenderer = this.gameObject.GetComponent<MeshRenderer>();
	}

	public void TurnOn() {
		myMeshRenderer.material.color = ColorUtilities.LIGHT_ON;
	}

	public void TurnOff() {
		myMeshRenderer.material.color = ColorUtilities.LIGHT_OFF;
	}
	
}
