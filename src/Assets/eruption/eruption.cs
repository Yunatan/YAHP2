using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eruption : MonoBehaviour {

	public GameObject blood;
	public GameObject spark;

	int x;
	int y;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {		
	}

	public void StartEruption(GameObject enemy)
	{
		x = Random.Range (-1700, 1700);
		y = Random.Range (-1700, 1700);
		StartCoroutine (Erupt (enemy));
	}

	IEnumerator Erupt(GameObject enemy) {
		GameObject spakly = (GameObject)Instantiate (spark, enemy.transform.position, Quaternion.identity);
		var bloodChance = Random.Range (0, 4);
		if(bloodChance == 0)
		{
			for (int i = 0; i < 7; i++) 
			{
				x = x + Random.Range (-500, 500);
				y = y + Random.Range (-500, 500);
				GameObject go = (GameObject)Instantiate (blood, enemy.transform.position, Quaternion.identity);
				go.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (x, y));
				yield return new WaitForSeconds (.002f);
			}
		}

	}
}


