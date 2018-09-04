using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airlock : MonoBehaviour {

	public Material openMaterial;
	public Material closedMaterial;

	public void Open() {
		this.gameObject.GetComponent<BoxCollider>().enabled = false;
		this.gameObject.GetComponent<MeshRenderer>().material = openMaterial;
	}

	public void Close() {
		this.gameObject.GetComponent<BoxCollider>().enabled = true;
		this.gameObject.GetComponent<MeshRenderer>().material = closedMaterial;
	}
	
}
