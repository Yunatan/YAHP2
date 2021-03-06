﻿using System;
using System.Collections;
using UnityEngine;

public class HeroDeath : MonoBehaviour, IDeathScript
{
    public Sprite DeadBodySprite;
    public SpriteRenderer fadeOutSprite;
    public float fadeSpeed;
    public GameObject spawnPoint;
    private PowerCore powerCore;
    private Rigidbody2D rb;

    private FightingMotor fightMotor;
    private Animator playerAnimator;
    private int drawDepth = 5;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void Start()
    {
        playerAnimator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
        fightMotor = gameObject.transform.Find("FightingSystem").GetComponent<FightingMotor>();
        powerCore = gameObject.GetComponent<PowerCore>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        fadeOutSprite.color = new Color(1, 1, 1, alpha);
    }

    public void Die()
    {
        GameManager.EnableInput = false;
        gameObject.GetComponent<PlayerMotor>().currentMovementAxis = 0;
        fightMotor.invulnerable = true;
        StartCoroutine(Fade());
        StartCoroutine(Respawn());
    }

    private void LeaveABodyBehinde()
    {
        var body = Instantiate(gameObject);

        var visual = body.transform.Find("Sprite");
        var cols = body.transform.Find("Colliders");
        var brb = body.GetComponent<Rigidbody2D>();

        MonoBehaviour[] comps = body.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            if (c != brb)
            {
                Destroy(c);
            }
        }
        foreach (Transform c in body.transform)
        {
            if (c != visual && c != cols)
            {
                Destroy(c.gameObject);
            }
        }

        brb.velocity = Vector2.zero;
        visual.GetComponent<Animator>().enabled = false;
        visual.GetComponent<SpriteRenderer>().material = new Material(visual.GetComponent<SpriteRenderer>().material);
        visual.GetComponent<SpriteRenderer>().sprite = DeadBodySprite;
        visual.GetComponent<SpriteRenderer>().sortingOrder = 8;
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    IEnumerator Fade()
    {
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }

    IEnumerator OutOfFade()
    {
        float fadeTime = BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    IEnumerator Respawn()
	{
		gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		yield return new WaitForSeconds(.3f);
		playerAnimator.Play("hero_death");
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = gameObject.transform;

        //leave a body behind
        LeaveABodyBehinde();

        gameObject.transform.position = spawnPoint.transform.position;
        yield return new WaitForSeconds(1f);
        StartCoroutine(OutOfFade());

        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = null;
        yield return new WaitForSeconds(0.8f);
        playerAnimator.Play("Idle");

        powerCore.CurrentPower = powerCore.MaxPower;
        GameManager.EnableInput = true;
        fightMotor.invulnerable = false;
    }
}