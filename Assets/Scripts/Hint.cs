using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour {

	private static TextMesh myText;
	private static Text myCanvasText;
	private static GameObject myBackpane;
	private static string buildUp;
	private static string[] words;
	private static string[] display;

	// Use this for initialization
	void Start () {
//		myText = this.gameObject.GetComponent<TextMesh>();
		myCanvasText = GetComponent<Text>();
//		myBackpane = FindUtilities.TryFind(this.gameObject, "Backpane");
	}

	public static void SetHint(string buildUp, string[] words) {
		Hint.buildUp = buildUp;
		Hint.words = words;
		display = new string[words.Length];
		for(int i = 0; i < words.Length; i++) {
			display[i] += GenerateBlanks(words[i]);
		}
//		myBackpane.GetComponent<MeshRenderer>().enabled = true;
		myCanvasText.enabled = true;
		RefreshDisplay();
	}

	public static string GetHintText() {
//		return myText.text;
		return myCanvasText.text;
	}

	private static void RefreshDisplay() {
		string result = Hint.buildUp + "\n";
		for(int i = 0; i < display.Length; i++) {
			result += (display[i] + "\n");
		}
//		myText.text = result;
		myCanvasText.text = result;
	}

	public static void ClearHint() {
//		words = null;
//		display = null;
//		myText.text = "";
		myCanvasText.text = "";
		//myBackpane.GetComponent<MeshRenderer>().enabled = false;
		myCanvasText.enabled = false;
	}

	public static void FoundWord(string word) {
		for(int i = 0; i < words.Length; i++) {
			if (word == words[i]) {
				display[i] = words[i];
			}
		}
		RefreshDisplay();
	}

	private static string GenerateBlanks(string word) {
		string result = "";
		int numLetters = word.Length;
		for(int i = 0; i < numLetters; i++) {
			result += "_ ";
		}
		return result;
	}
	
}
