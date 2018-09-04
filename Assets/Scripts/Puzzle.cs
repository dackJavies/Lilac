using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {

	public int width = 0;
	public int height = 0;
	public string keys;
	public GameObject key;
	public GameObject detachableKey;
	public GameObject blankKey;
	public string[] words;
	public string buildUp;
	public GameObject wireChain;

	protected Key[,] board;
	protected TextMesh currentString;
	protected string[] foundWords;
	protected int foundIndex;
	protected bool complete;

	protected GameObject MyKeypad;
	protected Block MyBlock;

	// Use this for initialization
	protected virtual void Start () {

		this.MyBlock = FindUtilities.TryFind(this.transform.parent.gameObject, "Block")
						.GetComponent<Block>();
		this.MyKeypad = this.MyBlock.transform.parent.gameObject;

		ShowBuildUp();

		currentString = FindUtilities.TryFind(this.gameObject, "Current String")
							.GetComponent<TextMesh>();
		foundWords = new string[words.Length];
		foundIndex = 0;
		ClearCurrentString();
		InitializeBoard();
		//FillBoard();
		FillBoardTwo();
		SetEnabledKeys(false);
		this.complete = false;
	}

	protected virtual void ShowBuildUp() {
		FindUtilities
			.TryFind(this.transform.parent.parent.parent.gameObject, "BuildUp")
			.GetComponent<TextMesh>()
			.text = buildUp;
	}

	protected virtual void CompleteBuildUp() {
//		Debug.Log(Hint.GetHintText());
		FindUtilities
			.TryFind(this.transform.parent.parent.parent.gameObject, "BuildUp")
			.GetComponent<TextMesh>()
			.text = Hint.GetHintText();
	}

	public Key GetKeyAt(int index) {
		int row = index / width;
		int col = index % width;
		return board[row, col];
	}

	public bool IsComplete() {
		return this.complete;
	}
	
	public void SetComplete(bool c) {
		this.complete = c;
	}

	public bool HasLeftovers() {
		if (!complete) {
			return false;
		}
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				if (board[i, j].tag == "DetachableKey") {
					return true;
				}
			}
		}
		return false;
	}

	public bool TestSubmission() {
		if (Contains(words, currentString.text)) {
			if (Contains(foundWords, currentString.text)) {
				Debug.Log("already submitted");
				return false;
			}
			Hint.FoundWord(currentString.text);
			return true;
		}
		return false;
	}

	public void ConvertPressedDetachablesToVanilla() {
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				if (board[i, j].tag == "DetachableKey" && board[i, j].pressed) {
					DetachableKey dk = (DetachableKey)board[i, j];
					GameObject keyObject = dk.gameObject;
					NormalKey nk = KeyUtilities.ConvertKey<DetachableKey, NormalKey>(
						ref keyObject, ColorUtilities.NORMAL, "NormalKey"
					);
					ResetBoardAt(nk.GetRow(), nk.GetCol(), nk);
				}
			}
		}
	}

	protected bool Contains(string[] array, string word) {
		for(int i = 0; i < array.Length; i++) {
			if (array[i] == word) {
				return true;
			}
		}
		return false;
	}

	public void FoundWord() {
		foundWords[foundIndex++] = currentString.text;
		if (foundIndex >= words.Length) {
			CompletePuzzle();
		}
	}

	public virtual void CompletePuzzle() {
		this.complete = true;
		Debug.Log("found all words");
		ChangeWireChain();
		CompleteBuildUp();
		if (PlayerInventory.HasTablet()) {
			CollectUnusedDetachables();
		} else {
			LockPuzzle();
		}
	}

	public virtual void LockPuzzle() {
		LockKeys();
		FindUtilities.TryFind(this.transform.parent.parent.gameObject, "SubmitButton").
			GetComponent<SubmitButton>().enabled = false;
		RemoveDoor();
		FindUtilities
			.TryFind(transform.parent.gameObject, "Block")
			.GetComponent<MeshRenderer>().material.color = ColorUtilities.SOLVED; //Color.green;
		Exit();
	}

	public virtual void CollectUnusedDetachables() {
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				if (board[i, j].gameObject.tag == "DetachableKey") {
					int firstBlank = PlayerInventory.FindFirstBlank();
					if (firstBlank > -1) {
						GameObject original = board[i, j].gameObject;
						GameObject clone = Object.Instantiate(this.detachableKey, this.transform);
						DetachableKey cloneKey = clone.GetComponent<DetachableKey>();
						clone.transform.localPosition = original.transform.localPosition;
						cloneKey.SetValue(board[i, j].GetValue());
						BlankKey blankOriginal = KeyUtilities.ConvertKey<DetachableKey, BlankKey>(
							ref original, ColorUtilities.BLANK, "BlankKey"
						);
						ResetBoardAt(blankOriginal.GetRow(), blankOriginal.GetCol(), blankOriginal);
						Key k = PlayerInventory.GetPuzzle().GetKeyAt(firstBlank);
						Vector3 dest = k.transform.position;
						cloneKey.AutoPickUp(dest);
						return;
					}
				}
			}
		}
		LockPuzzle();
	}

	protected virtual void ChangeWireChain() {
		if (wireChain != null) {
			wireChain.GetComponent<WireChain>().PuzzleSolved();
		}
	}

	public virtual void RemoveDoor() {
		Object.Destroy(FindUtilities.TryFind(this.transform.parent.parent.parent.gameObject, "Door"));
	}

	protected void InitializeBoard() {
		board = new Key[height, width];
	}

	public virtual void ClearCurrentString() {
		currentString.text = "";
	}

	public virtual void Exit() {
		ExitButton eb = FindUtilities
			.TryFind(this.transform.parent.parent.gameObject, "ExitButton")
			.GetComponent<ExitButton>();
		eb.ExitPuzzle();
	}

	public void SetEnabledKeys(bool setting) {
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				if (board[i, j] == null) {
					Debug.Log("nope: " + i.ToString() + ", " + j.ToString());
				}
				if (!complete) {
					board[i, j].enabled = setting;
				}
				board[i, j].gameObject.GetComponent<BoxCollider>().enabled = setting;
			}
		}
	}

	public void FillInHint() {
		for(int i = 0; i < foundIndex; i++) {
			Hint.FoundWord(foundWords[i]);
		}
	}

	public void AddToCurrentString(char c) {
		currentString.text += c;
	}

	public void ReleaseAllKeys() {
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				board[i, j].Release();
				board[i, j].SetAvailability(true);
				board[i, j].ReturnToDefaultColor();
			}
		}
	}

	public void LockKeys() { // LOOK HERE FOR BEING ABLE TO CLICC DETACHABLES AFTER SOLVING
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				if (board[i, j].tag != "DetachableKey") {
					board[i, j].Lock();
					board[i, j].SetAvailability(false);
				} else {
					board[i, j].Release();
					board[i, j].ReturnToDefaultColor();
				}
			}
		}
	}

	public void DetermineAvailability(Key pressed) {
		for(int i = 0; i < board.GetLength(0); i++) {
			for(int j = 0; j < board.GetLength(1); j++) {
				if (board[i, j].IsPressed()) {
					board[i, j].SetAvailability(false);
					continue;
				}
				if (j == pressed.GetCol() - 1 && i == pressed.GetRow() && pressed.LeftAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (j == pressed.GetCol() + 1 && i == pressed.GetRow() && pressed.RightAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (i == pressed.GetRow() - 1 && j == pressed.GetCol() && pressed.UpAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (i == pressed.GetRow() + 1 && j == pressed.GetCol() && pressed.DownAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (board[i, j].GetAvailability()) {
					board[i, j].SetAvailability(false);
				}
			}
		}
	}

	protected void FillBoardTwo() {

		float leftMargin = MyBlock.LeftMargin;
		float rightMargin = MyBlock.RightMargin;
		float topMargin = MyBlock.TopMargin;
		float bottomMargin = MyBlock.BottomMargin;

		Vector2 availableBoard = 
			new Vector2(this.MyBlock.GetAvailableWidth(),
						this.MyBlock.GetAvailableHeight());
		Vector2 placementSpacer =
			new Vector2(this.MyBlock.GetWidthSpacer(this.width),
						this.MyBlock.GetHeightSpacer(this.height));

		GameObject currentKey;
		Vector3 relativePosition = new Vector3(0f, 0f, 0f);

		int row, col;
		bool left, right, up, down;
		int pseudoIndex = 0;
		bool nextDetachable = false;
		bool currentIsBlank = false;

		for(int i = 0; i < keys.Length; i++) {
			switch(keys[i]) {
				case '\'':
					nextDetachable = true;
					break;
				
				default:
					if (keys[i] == '_') {
						currentIsBlank = true;
					}
					row = pseudoIndex / width;
					col = pseudoIndex % width;
					if (nextDetachable) {
						currentKey = Object.Instantiate(detachableKey, this.transform);
						nextDetachable = false;
					} else if(currentIsBlank) {
						currentKey = Object.Instantiate(blankKey, this.transform);
					} else {
						currentKey = Object.Instantiate(key, this.transform);
					}
					relativePosition.x = (leftMargin 
											+ (col * placementSpacer.x)
											+ key.transform.localScale.x / 2)
										 - this.MyBlock.GetWidthDivider();
					relativePosition.y = ((0 - topMargin)
											- (row * placementSpacer.y)
											- key.transform.localScale.y / 2)
										 + this.MyBlock.GetHeightDivider();
					relativePosition.z = 
						0 - (this.MyBlock.transform.localScale.z / 2) 
						  - (this.key.transform.localScale.z / 2);
					currentKey.transform.localPosition = relativePosition;
					if (keys[i] != '_') {
						currentKey.GetComponent<Key>().SetValue(keys[i]);
					}
					board[row, col] = currentKey.GetComponent<Key>();
					left = col > 0;
					right = col < width - 1;
					up = row > 0;
					down = row < height - 1;
					board[row, col].SetNeighbors(left, right, up, down);
					board[row, col].SetRow(row);
					board[row, col].SetCol(col);
					board[row, col].AssignAvailPosition();
					board[row, col].AssignUnavailPosition();
					pseudoIndex += 1;
					currentIsBlank = false;
					break;
			}
		}

	}

	protected void FillBoard() {
		GameObject current = null;
		Vector3 relativePosition = new Vector3(0f, 0f, 0f);
		int row, col;
		bool left, right, up, down;
		int pseudoIndex = 0;
		bool nextDetachable = false;
		for(int i = 0; i < keys.Length; i++) {
			if (keys[i] == '\'') {
				nextDetachable = true;
				continue;
			} else {
				row = pseudoIndex / width;
				col = pseudoIndex % width;
				if (keys[i] == '_') {
					current = Object.Instantiate(blankKey, this.transform);
				} else {
					current = nextDetachable ? 
								Object.Instantiate(detachableKey, this.transform) :
								Object.Instantiate(key, this.transform);
				}
				relativePosition.z = col * -0.2f;
				relativePosition.y = row * -0.2f; 
				current.transform.localPosition = relativePosition;
				if (keys[i] != '_') {
					current.GetComponent<Key>().SetValue(keys[i]);
				}
				board[row, col] = current.GetComponent<Key>();
				left = col > 0;
				right = col < width - 1;
				up = row > 0;
				down = row < height - 1;
				board[row, col].SetNeighbors(left, right, up, down);
				board[row, col].SetRow(row);
				board[row, col].SetCol(col);
				nextDetachable = false;
				pseudoIndex += 1;
			}
		}
	}

	public void ResetBoardAt(int row, int col, Key newKey) {
		Debug.Log("RESETTING: (" + row.ToString() + ", " + col.ToString() + ")");
		this.board[row, col] = newKey;
	}

}
