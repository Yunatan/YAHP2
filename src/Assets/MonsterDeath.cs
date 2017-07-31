using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeath : MonoBehaviour, IDeathScript
{
    public void Die()
    {
        //stop doimg everything
        var visual = gameObject.transform.Find("Sprite").gameObject;

        MonoBehaviour[] comps = gameObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour c in comps)
        {
            c.enabled = false;
        }

        visual.SetActive(true);

        //play death anim

        //leave a sprite behind
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
