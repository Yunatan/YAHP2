using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWalker : MonoBehaviour {
    public float aggroDistance;
    public float hitDistance;
    public LayerMask WhatIsGround;

    private GameObject player;
    private float playerDistance;
    private bool canSeek = true;
    private bool canSmash = true;
    private FightingMotor fightingMotor;

    public int Speed;

    public bool IsGrounded { get { return grounded; } }
    private bool grounded;

    private Rigidbody2D walkerRigidbody;
    private Animator walkerAnimator;
    private SpriteRenderer walkerSprite;
    private BoxCollider2D groundCheck;
    private PowerCore powerCore;

    private float currentMovementAxis;
    private bool jumpPending;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        walkerAnimator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
        fightingMotor = gameObject.transform.Find("FightingSystem").GetComponent<FightingMotor>();

        walkerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        walkerSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        groundCheck = gameObject.transform.Find("Colliders").Find("Ground").gameObject.GetComponent<BoxCollider2D>();
        powerCore = gameObject.GetComponent<PowerCore>();
    }
    void Update () {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (playerDistance < aggroDistance && canSeek)
        {
            StartCoroutine(Seek());
        }

        if (player.transform.position.x - transform.position.x > 0)
        {
            currentMovementAxis = 1;
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }else { 
            currentMovementAxis = -1;
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x) * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        if (playerDistance < hitDistance && canSmash)
        {
            walkerRigidbody.velocity = Vector2.zero;
            StartCoroutine(Smash());
        }
    }

    IEnumerator Seek()
    {
        canSeek = false;

        walkerRigidbody.velocity = new Vector2(currentMovementAxis * Speed, walkerRigidbody.velocity.y);
        walkerAnimator.SetFloat("Speed", Mathf.Abs(walkerRigidbody.velocity.x));

        yield return new WaitForSeconds(1f);

        //if(playerDistance < aggroDistance)

        canSeek = true;
    }

    IEnumerator Smash()
    {
        canSmash = false;
        walkerAnimator.Play(new[] { "hero_hit_hook", "hero_hit_upper" }[Random.Range(0, 2)]);
        StartCoroutine(fightingMotor.Attack(0.15f, 0.2f));
        yield return new WaitForSeconds(0.35f);
        canSmash = true;
    }

}
