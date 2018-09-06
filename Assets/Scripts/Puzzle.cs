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
		FillBoard();
		this.complete = false;
	}

	protected virtual void ShowBuildUp() {
		FindUtilities
			.TryFind(this.transform.parent.parent.parent.gameObject, "BuildUp")
			.GetComponent<TextMesh>()
			.text = buildUp;
	}

	protected virtual void CompleteBuildUp() {
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
			.GetComponent<MeshRenderer>().material.color = ColorUtilities.SOLVED;
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
						cloneKey.AutoPickUp(k.transform.position);
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
		bool sameRow, sameCol;
		for(int i = 0; i < board.GetLength(0); i++) {
			sameRow = i == pressed.GetRow();
			for(int j = 0; j < board.GetLength(1); j++) {
				sameCol = j == pressed.GetCol();
				if (board[i, j].IsPressed()) {
					board[i, j].SetAvailability(false);
					continue;
				}
				if (j == pressed.GetCol() - 1 && sameRow && pressed.LeftAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (j == pressed.GetCol() + 1 && sameRow && pressed.RightAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (i == pressed.GetRow() - 1 && sameCol && pressed.UpAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (i == pressed.GetRow() + 1 && sameCol && pressed.DownAvailable()) {
					board[i, j].SetAvailability(true);
					continue;
				}
				if (board[i, j].GetAvailability()) {
					board[i, j].SetAvailability(false);
				}
			}
		}
	}

	protected void FillBoard() {

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

		int currentRow, currentCol;
		bool neighborToLeft, neighborToRight, neighborAbove, neighborBelow;
		int pseudoIndex = 0;
		bool nextDetachable = false;

		for(int i = 0; i < keys.Length; i++) {
			switch(keys[i]) {
				case '\'':
					nextDetachable = true;
					break;
				
				default:
					if (nextDetachable) {
						currentKey = Object.Instantiate(detachableKey, this.transform);
						nextDetachable = false;
					} else if(keys[i] == '_') {
						currentKey = Object.Instantiate(blankKey, this.transform);
					} else {
						currentKey = Object.Instantiate(key, this.transform);
					}

					currentRow = pseudoIndex / width;
					currentCol = pseudoIndex % width;
					board[currentRow, currentCol] = currentKey.GetComponent<Key>();
					board[currentRow, currentCol].SetRow(currentRow);
					board[currentRow, currentCol].SetCol(currentCol);
					board[currentRow, currentCol].SetValue(keys[i]);

					relativePosition.x = (leftMargin 
											+ (currentCol * placementSpacer.x)
											+ key.transform.localScale.x / 2)
										 - this.MyBlock.GetWidthDivider();
					relativePosition.y = ((0 - topMargin)
											- (currentRow * placementSpacer.y)
											- key.transform.localScale.y / 2)
										 + this.MyBlock.GetHeightDivider();
					relativePosition.z = 
						0 - (this.MyBlock.transform.localScale.z / 2) 
						  - (this.key.transform.localScale.z / 2);
					board[currentRow, currentCol].transform.localPosition = relativePosition;

					neighborToLeft = currentCol > 0;
					neighborToRight = currentCol < width - 1;
					neighborAbove = currentRow > 0;
					neighborBelow = currentRow < height - 1;
					board[currentRow, currentCol].SetNeighbors(neighborToLeft, neighborToRight, neighborAbove, neighborBelow);

					board[currentRow, currentCol].AssignAvailPosition();
					board[currentRow, currentCol].AssignUnavailPosition();

					pseudoIndex += 1;
					break;
			}
		}

	}

	public void ResetBoardAt(int row, int col, Key newKey) {
		this.board[row, col] = newKey;
	}

}
