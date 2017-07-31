using System;
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

        LeaveABodyBehinde();

        StartCoroutine(Fade());
        playerAnimator.Play("hero_death");
        StartCoroutine(Respawn());
    }

    private void LeaveABodyBehinde()
    {
        var body = Instantiate(gameObject);
        var visual = body.transform.Find("Sprite").gameObject;

        body.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        MonoBehaviour[] comps = body.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            c.enabled = false;
        }
        foreach (Transform c in body.transform)
        {
            c.gameObject.SetActive(false);
        }

        visual.SetActive(true);
        visual.GetComponent<Animator>().enabled = false;
        visual.GetComponent<SpriteRenderer>().sprite = DeadBodySprite;
        visual.GetComponent<SpriteRenderer>().material = new Material(visual.GetComponent<SpriteRenderer>().material);
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
		Debug.Log ("DEd");
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
        rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager.EnableInput = true;
        fightMotor.invulnerable = false;
    }
}