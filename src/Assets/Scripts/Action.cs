﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Invoker(){
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		Destroy( gameObject.GetComponent<BoxCollider2D> ());
	}
}