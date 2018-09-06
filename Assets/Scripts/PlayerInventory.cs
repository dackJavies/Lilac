using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	private static char[] keys;
	private static GameObject parent;

	private static GameObject MyTablet;
	private static TabletPuzzle TabletPuzzle;

	private static Vector2 BACK_TO_WALKING_POSITION = new Vector2(0.3f, -0.3f);
	private static Vector2 SOLVING_POSITION = new Vector2(0f, 0f);
	private static Vector3 PUZZLE_POSITION = new Vector3(0f, 0.35f, 0.8f);


	void Start() {
		PlayerInventory.MyTablet = null;
		PlayerInventory.TabletPuzzle = null;
		PlayerInventory.parent = this.transform.parent.gameObject;
		PlayerInventory.keys = new char[0];
	}

	public static bool HasTablet() {
		return MyTablet != null;
	}

	public static void PickUpTablet(GameObject tablet) {
		PlayerInventory.MyTablet = tablet;
		PlayerInventory.TabletPuzzle = FindUtilities.TryFind(MyTablet, "Puzzle").GetComponent<TabletPuzzle>();
		TabletPuzzle.RemovedFromDock();
		MyTablet.transform.parent = parent.transform;
		WalkingView();
		keys = new char[TabletPuzzle.width * TabletPuzzle.height];
		int pseudoIndex = 0;
		for(int i = 0; i < TabletPuzzle.keys.Length; i++) {
			if (TabletPuzzle.keys[i] != '\'') {
				if (TabletPuzzle.keys[i] == '_') {
					keys[pseudoIndex++] = '\0';
				} else {
					keys[pseudoIndex++] = TabletPuzzle.keys[i];
				}
			}
		}
		TabletPuzzle.SetEnabledKeys(true);
	}

	public static void PlaceTablet(Transform dock) {
		MyTablet.transform.parent = dock;
		MyTablet.transform.localPosition = Vector3.zero;
		MyTablet.transform.localEulerAngles = Vector3.zero;
		MyTablet.transform.localScale = Vector3.one;
		TabletPuzzle.PlacedOnDock();
		MyTablet = null;
		TabletPuzzle = null;
		keys = new char[0];
	}

	public static void PuzzleView() {
		if (MyTablet != null) {
			MyTablet.transform.localPosition = PUZZLE_POSITION;
			TabletPuzzle.SetEnabledKeys(true);
			FindUtilities.TryFind(MyTablet, "SubmitButton")
				.GetComponent<TabletSubmitButton>()
				.SetBoxColliderEnabled(true);
		}
	}

	public static void WalkingView() {
		if (MyTablet != null) {
			MyTablet.transform.localPosition = new Vector3(0.3f, -0.3f, 0.8f);
			MyTablet.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			MyTablet.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
			TabletPuzzle.SetEnabledKeys(false);
			FindUtilities.TryFind(MyTablet, "SubmitButton")
				.GetComponent<TabletSubmitButton>()
				.SetBoxColliderEnabled(false);
		}
	}

	public static void BackToWalkingView() {
		if (MyTablet != null) {
			ToggleSolvingView(false);
		}
	}

	public static void SolvingView() {
		if (MyTablet != null) {
			ToggleSolvingView(true);
		}
	}

	private static void ToggleSolvingView(bool solving) {
			Vector3 copy = MyTablet.transform.localPosition;
			copy.x = solving ? SOLVING_POSITION.x : BACK_TO_WALKING_POSITION.x;
			copy.y = solving ? SOLVING_POSITION.y : BACK_TO_WALKING_POSITION.y;
			MyTablet.transform.localPosition = copy;
			FindUtilities.TryFind(MyTablet, "SubmitButton")
				.GetComponent<TabletSubmitButton>()
				.SetBoxColliderEnabled(true);
	}

	public static Puzzle GetPuzzle() {
		return PlayerInventory.TabletPuzzle;
	}

	public static GameObject GetTablet() {
		return PlayerInventory.MyTablet;
	}

	public static char[] GetKeys() {
		return PlayerInventory.keys;
	}

	public static char GetKey(int index) {
		return PlayerInventory.keys[index];
	}

	public static void ClearCurrent() {
		TabletPuzzle.ClearCurrentString();
		TabletPuzzle.ReleaseAllKeys();
	}

	public static int FindFirstBlank() {
		Key current;
		for(int i = 0; i < keys.Length; i++) {
			current = TabletPuzzle.GetKeyAt(i);
			if (current.gameObject.tag == "BlankKey") {
				return i;
			}
		}
		return -1;
	}

	public static bool AutoPickUp(char c) {
		int firstBlank = FindFirstBlank();
		if (firstBlank > -1) {
			GameObject target = TabletPuzzle.GetKeyAt(firstBlank).gameObject;
			DetachableKey dk = KeyUtilities.ConvertKey<BlankKey, DetachableKey>(
				ref target, ColorUtilities.DETACHABLE, "DetachableKey"
			);
			dk.SetValue(c);
			TabletPuzzle.ResetBoardAt(dk.GetRow(), dk.GetCol(), dk);
			return true;
		}
		return false;
	}

}
