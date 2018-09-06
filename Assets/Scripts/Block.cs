using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public float LeftMargin;
	public float RightMargin;
	public float TopMargin;
	public float BottomMargin;

	public float GetAvailableWidth() {
		return transform.localScale.x - LeftMargin - RightMargin;
	}

	public float GetAvailableHeight() {
		return transform.localScale.y - TopMargin - BottomMargin;
	}

	public float GetWidthSpacer(int puzzleWidth) {
		return GetAvailableWidth() / puzzleWidth;
	}

	public float GetHeightSpacer(int puzzleHeight) {
		return GetAvailableHeight() / puzzleHeight;
	}

	public float GetWidthDivider() {
		return GetAvailableWidth() / 2;
	}

	public float GetHeightDivider() {
		return GetAvailableHeight() / 2;
	}
	
}
