using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindUtilities : MonoBehaviour {

	public static GameObject TryFind(GameObject parent, string nameToFind) {
		Transform parentTransform = parent.transform;
		if (parent == null) {
			Debug.Log("TryFind: Cannot search for child of null parent.");
		}
		Transform attempt = parentTransform.Find(nameToFind);
		if (attempt == null) {
			Debug.Log("TryFind: Could not find " + parent.name + 
				"'s child of the name: " + nameToFind + ".");
		}
		return attempt.gameObject;
	}

	public static GameObject TryFind(string nameToFind) {
		GameObject attempt = GameObject.Find(nameToFind);
		if (attempt == null) {
			Debug.Log("TryFind: Could not find game object with name: " + nameToFind + ".");
		}
		return attempt;
	}



}
