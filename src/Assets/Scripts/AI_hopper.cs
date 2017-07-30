using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_hopper : MonoBehaviour {


	public float aggroDistance;

	GameObject player;
	float playerDistance;
	bool canAttack = true;

	// Use this for initialization
	void Start () { 
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		playerDistance = Vector2.Distance (new Vector2 (player.transform.position.x, player.transform.position.y), new Vector2 (transform.position.x, transform.position.y));
		//Debug.Log (Vector2.Distance (new Vector2 (player.transform.position.x, player.transform.position.y), new Vector2 (transform.position.x, transform.position.y)));
		if (playerDistance < aggroDistance) {
			//gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100, 100));	
			if (canAttack){
				StartCoroutine("Attack");
			}
		}
	}

	IEnumerator Attack (){
		canAttack = false;
		yield return new WaitForSeconds (1f);
		if (player.transform.position.x - transform.position.x < 0) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-9000, 5000));
		} else {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (9000, 5000));
		}
		yield return new WaitForSeconds (2f);
		canAttack = true;
	}

}
