﻿using System.Collections;
using UnityEngine;

public class HeroDeath : MonoBehaviour, IDeathScript
{
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
        powerCore = gameObject.transform.GetComponent<PowerCore>();
        rb = gameObject.transform.GetComponent<Rigidbody2D>();
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
        fightMotor.invulnerable = true;
        rb.bodyType = RigidbodyType2D.Static;

        StartCoroutine(Fade());
        playerAnimator.Play("hero_death");
        StartCoroutine(Respawn());
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
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = gameObject.transform;
        gameObject.transform.position = spawnPoint.transform.position;
        yield return new WaitForSeconds(1f);
        StartCoroutine(OutOfFade());
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = null;
        yield return new WaitForSeconds(0.8f);
        playerAnimator.Play("Idle");

        powerCore.CurrentPower = powerCore.MaxPower;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager.EnableInput = true;
        fightMotor.invulnerable = false;
    }
}