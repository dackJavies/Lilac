using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftPuzzle : Puzzle {

	protected override void Start() {
		this.MyBlock = FindUtilities
			.TryFind(this.transform.parent.gameObject, "Block")
			.GetComponent<Block>();
		InitializeBoard();
		FillBoardTwo();
		SetEnabledKeys(false);
	}

	protected override void ShowBuildUp() {}

	public override void ClearCurrentString() {}

}
