using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startDial : MonoBehaviour {


	public GameObject label;
	public string text;

	bool isTyping = false;


	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			StopAllCoroutines ();
			StartCoroutine (writing ());
		}
	}


	IEnumerator writing(){
		label.GetComponent<TextMesh> ().text = "";
		StartCoroutine (typing(text));
		yield return new WaitForSeconds (6f);
		clearText ();
	}

	IEnumerator typing(string text){
		var shush = label.GetComponent<TextMesh> ().text;
		for (int i = 0; i < text.Length; i++){
			label.GetComponent<TextMesh> ().text += text [i];
			if ((i+1) % 20 == 0) {
				if (text[i] == ' '){
					
				}

			}
			yield return new WaitForSeconds (0.001f);
		}
	}

	void clearText(){
		label.GetComponent<TextMesh>().text = "";
	}
}


