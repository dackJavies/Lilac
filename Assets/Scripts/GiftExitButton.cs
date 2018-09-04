using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GiftExitButton : ExitButton {

	protected override void OnMouseUp() {
		PlayerInventory.BackToWalkingView();
		PlayerInventory.ClearCurrent();
		controller.GetComponent<CharacterController>().enabled = true;
		this.controller.GetComponent<FirstPersonController>().enabled = true;
		this.clickCollider.GetComponent<BoxCollider>().enabled = true;
		this.character.transform.parent = controller.transform;
		this.character.transform.localPosition = this.playerHead.localPosition;
		this.character.transform.localEulerAngles = this.playerHead.localEulerAngles;
		this.character.transform.localScale = this.playerHead.localScale;
	}

}
