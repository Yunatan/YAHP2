using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonMasher : MonoBehaviour {

	public Action[] objects;
	public Sprite newSprite;
	public int powerNeeded;

	private bool buttonActive;
	private GameObject player;

	void Start () {
		buttonActive = false;
		player = GameObject.Find("Player");
	}

	void Update () {
		if (buttonActive && Input.GetButtonDown("Action") && GameManager.EnableInput){
			Destroy(transform.GetChild(0).gameObject);
			gameObject.GetComponent<SpriteRenderer> ().sprite = newSprite;
			player.GetComponent<PowerCore> ().CurrentPower -= powerNeeded;
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
