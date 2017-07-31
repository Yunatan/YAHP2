using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour {

	public float time;

	// Use this for initialization
	void Start () {
		StartCoroutine ("Death");
	}


	IEnumerator Death() {
		yield return new WaitForSeconds (time);
		Destroy (this.gameObject);
	}
}
