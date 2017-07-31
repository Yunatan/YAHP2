using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class push : MonoBehaviour {


	public GameObject gg;



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
			
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.collider.tag == "Ground") {
			Vector2 vc = col.contacts [0].normal;
			GameObject go = (GameObject) Instantiate(gg, gameObject.transform.position,Quaternion.identity); 
			if (vc.x != 0) {
				Debug.Log (vc);
				go.transform.Rotate (0, 0, 90);
			}
			Destroy (this.gameObject);
		}
	}


}
