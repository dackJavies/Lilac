using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour {

	private Airlock airlock;
	
	void Start() {
		this.airlock = FindUtilities
			.TryFind(transform.parent.parent.gameObject, "Airlock")
			.GetComponent<Airlock>();
	}

	public void TakeTablet() {
		this.airlock.Close();
	}

	public void ReturnTablet() {
		this.airlock.Open();
	}

}
