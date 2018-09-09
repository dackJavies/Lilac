using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftPuzzle : Puzzle {

	protected override void Awake() {
		this.MyBlock = FindUtilities
			.TryFind(this.transform.parent.gameObject, "Block")
			.GetComponent<Block>();
		InitializeBoard();
		FillBoard();
		SetEnabledKeys(false);
	}

	protected override void Start() {}

	protected override void ShowBuildUp() {}

	public override void ClearCurrentString() {}

}
