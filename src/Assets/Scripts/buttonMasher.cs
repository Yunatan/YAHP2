using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMasher : MonoBehaviour {

	public Action[] objects;

	private bool buttonActive;

	void Start () {
		buttonActive = false;
	}

	void Update () {
		if (buttonActive && Input.GetButtonDown("Action")){
			Destroy(transform.GetChild(0).gameObject);
			foreach (var gm in objects) {
				gm.Invoker ();
			}
		}	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			buttonActive = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			buttonActive = false;
		}

	}
}
