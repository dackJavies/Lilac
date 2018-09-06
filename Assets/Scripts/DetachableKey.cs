using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachableKey : Key {

    private bool dragging;
    private Vector3 mouse;
    
    void Update() {
        if (this.dragging) {
            mouse = Input.mousePosition;
            mouse.z = 0.5f;
            transform.position = Camera.main.ScreenToWorldPoint(mouse);
        }
    }

    protected override void OnMouseUp() {
        if (!KeyUtilities.DetectIfOnGift(this) && !transform.parent.GetComponent<Puzzle>().IsComplete()) {
            base.OnMouseUp();
        }
    }

    public void Drag() {
        transform.localScale *= 0.5f;
        this.dragging = true;
    }

}
