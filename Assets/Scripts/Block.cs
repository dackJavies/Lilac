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
		//return (transform.localScale.x + LeftMargin) / puzzleWidth;
	}

	public float GetHeightSpacer(int puzzleHeight) {
		return GetAvailableHeight() / puzzleHeight;
		//return (transform.localScale.y + TopMargin) / puzzleHeight;
	}

	public float GetWidthDivider() {
		//return (transform.localScale.x - LeftMargin) / 2;
		return GetAvailableWidth() / 2;
	}

	public float GetHeightDivider() {
		//return (transform.localScale.y - TopMargin) / 2;
		return GetAvailableHeight() / 2;
	}
	
}
