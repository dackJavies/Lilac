using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankKey : Key {

	protected override void Awake() {
		base.Awake();
	}

	protected override void Start() {
		base.Start();
		this.available = false;
		FindUtilities.TryFind(this.gameObject, "KeyVal")
			.GetComponent<TextMesh>().text = "";
		this.defaultColor = Color.black;
	}

	protected override void OnMouseUp() {}

	public override void SetAvailability(bool a) {
		// this.available = a;
		// if (a) {
		// 	StartCoroutine(MoveToLocalPoint(AVAIL_POSITION));
		// } else if (!this.pressed) {
		// 	StartCoroutine(MoveToLocalPoint(UNAVAIL_POSITION));
		// } 
	}

	public override void Release() {}

	public override void SetValue(char c) {
		this.val = c;
	}

	protected override IEnumerator MoveToLocalPoint(Vector3 dest) {
		yield return null;
	}

}
