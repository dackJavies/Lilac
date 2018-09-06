using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	protected char val;
	public bool pressed;
	public bool available;

	protected int row;
	protected int col;

	protected bool leftNeighbor;
	protected bool rightNeighbor;
	protected bool upNeighbor;
	protected bool downNeighbor;

	protected MeshRenderer myMeshRenderer;
	public Color defaultColor;

	protected float MOVE_TIME = 0.2f;
	protected Vector3 AVAIL_POSITION;
	protected Vector3 UNAVAIL_POSITION;

	protected virtual void Awake () {
		this.myMeshRenderer = this.gameObject.GetComponent<MeshRenderer>();
		this.available = true;
	}

	protected virtual void Start() {}

	public void AssignAvailPosition() {
		this.AVAIL_POSITION = transform.localPosition;
	}

	public void AssignUnavailPosition() {
		this.UNAVAIL_POSITION = transform.localPosition;
		this.UNAVAIL_POSITION.z += 0.03f;
	}

	public Vector3 GetAVAIL() {
		return AVAIL_POSITION;
	}

	public void SetAVAIL(Vector3 newAvail) {
		AVAIL_POSITION = newAvail;
	}

	public Vector3 GetUNAVAIL() {
		return UNAVAIL_POSITION;
	}

	public void SetUNAVAIL(Vector3 newUnavail) {
		UNAVAIL_POSITION = newUnavail;
	}

	public void SetDefaultColor(Color newDefault) {
		defaultColor = newDefault;
		ReturnToDefaultColor();
	}

	public void ReturnToDefaultColor() {
		this.myMeshRenderer.material.color = this.defaultColor;
	}

	public void SelectOnTablet() {
		this.myMeshRenderer.material.color = Color.magenta;
	}

	public bool IsPressed() {
		return this.pressed;
	}

	public char GetValue() {
		return this.val;
	}

	public bool GetAvailability() {
		return this.available;
	}

	public int GetRow() {
		return this.row;
	}

	public int GetCol() {
		return this.col;
	}

	public bool LeftAvailable() {
		return this.leftNeighbor;
	}
	
	public bool RightAvailable() {
		return this.rightNeighbor;
	}

	public bool UpAvailable() {
		return this.upNeighbor;
	}

	public bool DownAvailable() {
		return this.downNeighbor;
	}

	public virtual void SetValue(char c) {
		this.val = c;
		FindUtilities.TryFind(this.gameObject, "KeyVal")
			.GetComponent<TextMesh>().text = this.val.ToString();
	}

	public void SetRow(int r) {
		this.row = r;
	}

	public void SetCol(int c) {
		this.col = c;
	}

	public virtual void SetAvailability(bool a) {
		this.available = a;
		if (a) {
			SendKeyToLocal(AVAIL_POSITION);
			this.myMeshRenderer.material.color = ColorUtilities.AVAILABLE;
		} else if (!this.pressed) {
			this.myMeshRenderer.material.color = Color.black;
			SendKeyToLocal(UNAVAIL_POSITION);
		} 
	}

	public void SetNeighbors(bool left, bool right, bool up, bool down) {
		leftNeighbor = left;
		rightNeighbor = right;
		upNeighbor = up;
		downNeighbor = down;
	}

	public virtual void Release() {
		this.pressed = false;
	}

	public void Lock() {
		this.enabled = false;
		this.myMeshRenderer.material.color = Color.black;
	}

	protected virtual void OnMouseUp() {
		if (this.available && BoardMutator.dragging == null) {
			Puzzle p = transform.parent.gameObject.GetComponent<Puzzle>();
			p.AddToCurrentString(this.val);
			this.pressed = true;
			this.myMeshRenderer.material.color = ColorUtilities.PRESSED; //Color.magenta;
			SendKeyToLocal(UNAVAIL_POSITION);
			p.DetermineAvailability(this);
		}
	}

	public void AutoPickUp(Vector3 dest) {
		StartCoroutine(MoveToTabletPoint(dest));
		StartCoroutine(MorphToScale(this.transform.localScale * 0.4f));
	}

	public void SendKeyToLocal(Vector3 dest) {
		StartCoroutine(MoveToLocalPoint(dest));
	}

	public void ResetPressed() {
		this.transform.localPosition = AVAIL_POSITION;
	}

	protected IEnumerator MoveToTabletPoint(Vector3 destination) {
		float timeRemaining = MOVE_TIME;
		for(float i = 0; i < MOVE_TIME; i += Time.deltaTime) {
			this.transform.position +=
				CalculateMovementDelta(destination, timeRemaining);
			timeRemaining -= Time.deltaTime;
			yield return null;
		}
		this.transform.position = destination;
		if (PlayerInventory.AutoPickUp(this.val)) {
			this.myMeshRenderer.enabled = false;
			Puzzle p = transform.parent.gameObject.GetComponent<Puzzle>();
			this.tag = "BlankKey";
			p.CollectUnusedDetachables();
		}
		Object.Destroy(this.gameObject);
	}

	protected virtual IEnumerator MoveToLocalPoint(Vector3 destination) {
		float timeRemaining = MOVE_TIME;
		for(float i = 0; i < MOVE_TIME; i += Time.deltaTime) {
			this.transform.localPosition +=
				CalculateLocalMovementDelta(destination, timeRemaining);
			timeRemaining -= Time.deltaTime;
			yield return null;
		}
		this.transform.localPosition = destination;
	}

	protected IEnumerator MorphToScale(Vector3 scale) {
		float timeRemaining = MOVE_TIME;
		for(float i = 0; i < MOVE_TIME; i += Time.deltaTime) {
			this.transform.localScale +=
				CalculateLocalScaleDelta(scale, timeRemaining);
			timeRemaining -= Time.deltaTime;
			yield return null;
		}
		this.transform.localScale = scale;
	}

	protected Vector3 CalculateLocalMovementDelta(Vector3 destination, float timeRemaining) {
		float timeUsed = Time.deltaTime / timeRemaining;
		Vector3 diff = destination - transform.localPosition;
		return timeUsed * diff;
	}

	protected Vector3 CalculateLocalScaleDelta(Vector3 destination, float timeRemaining) {
		float timeUsed = Time.deltaTime / timeRemaining;
		Vector3 diff = destination - transform.localScale;
		return timeUsed * diff;
	}

	protected Vector3 CalculateMovementDelta(Vector3 destination, float timeRemaining) {
		float timeUsed = Time.deltaTime / timeRemaining;
		Vector3 diff = destination - transform.position;
		return timeUsed * diff;
	}


}
