using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel : MonoBehaviour {

	private const float FADE_TRANSITION_DURATION = 2.0f;
	public Material one;
	public Material two;
	private MeshRenderer MR;


	void Start() {
		MR = GetComponent<MeshRenderer>();
		MR.material = one;
		StartCoroutine(FadeTo(two.color.r, two.color.a));
	}

	// ASSUMES THAT THE COMPONENT THAT OWNS THIS SCRIPT
	// IS ONLY ON GREYSCALE; R == G == B
	private IEnumerator FadeTo(float val, float alpha) {
		float colorDiff = val - MR.material.color.r;
		float alphaDiff = alpha - MR.material.color.a;
		float delta;
		Color temp;
		for(float i = 0; i < FADE_TRANSITION_DURATION; i += Time.deltaTime) {
			temp = MR.material.color;
			delta = colorDiff * (Time.deltaTime / FADE_TRANSITION_DURATION);
			temp.r += delta;
			temp.g += delta;
			temp.b += delta;
			delta = alphaDiff * (Time.deltaTime / FADE_TRANSITION_DURATION);
			temp.a += delta;
			MR.material.color = temp;
			yield return null;
		}
		MR.material.color = new Color(val, val, val, alpha);
		if (val == two.color.r) {
			StartCoroutine(FadeTo(one.color.r, one.color.a));
		} else {
			StartCoroutine(FadeTo(two.color.r, two.color.a));
		}
	}

}
