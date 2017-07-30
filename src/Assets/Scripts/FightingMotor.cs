﻿using System.Collections;
using UnityEngine;

public class FightingMotor : MonoBehaviour
{
    public int Health;
    public int Damage;

    private Collider2D damageArea;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private Vector2 knockbackEffectPending;
    private bool currentlyAttacking;

    public bool invulnerable;

    private void Start()
    {
        damageArea = gameObject.GetComponent<BoxCollider2D>();
        damageArea.enabled = false;
        animator = gameObject.transform.parent.Find("Sprite").GetComponent<Animator>();
        sprite = gameObject.transform.parent.Find("Sprite").GetComponent<SpriteRenderer>();
        rb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !currentlyAttacking && gameObject.transform.parent.tag == "Player")
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var isEnemy = collider.gameObject.tag == "Enemy";
        if (isEnemy)
        {
            DealDamage(collider.transform.parent.parent.gameObject);
        }
    }

    private IEnumerator Attack()
    {
        animator.Play(new[] { "hero_hit_hook", "hero_hit_upper" }[UnityEngine.Random.Range(0, 2)]);
        yield return new WaitForSeconds(0.15f);
        damageArea.enabled = true;
        currentlyAttacking = true;
        yield return new WaitForSeconds(0.2f);
        currentlyAttacking = false;
        damageArea.enabled = false;
    }

    private void DealDamage(GameObject enemy)
    {
        var enemyFighingMotor = enemy.transform.Find("FightingSystem").GetComponent<FightingMotor>();
        enemyFighingMotor.GetDamage(Damage, gameObject.transform.position);
    }

    public void GetDamage(int damage, Vector2 attackerPos)
    {
        if (invulnerable) return;

        Health -= damage;

        //add knockback here
        knockbackEffectPending = gameObject.transform.position.x - attackerPos.x > 0 ? Vector2.right : Vector2.left;

        //add grace period
        if (gameObject.transform.parent.tag == "Player")
        {
            StartCoroutine(ReciveGracePeriod());
        }

        //add death check
        if(Health <= 0)
        {

        }
    }

    private void FixedUpdate()
    {
        HandleKnockBack();
    }

    private void HandleKnockBack()
    {
        if (knockbackEffectPending != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(knockbackEffectPending.x * 3000, 3000));
            knockbackEffectPending = Vector2.zero;
        }
    }

    private IEnumerator ReciveGracePeriod()
    {
        invulnerable = true;
        var initMat = sprite.material;
        var blinkMat = new Material("UI/Default");
        sprite.material = blinkMat;
        yield return new WaitForSeconds(0.1f);
        sprite.material = initMat;
        yield return new WaitForSeconds(0.1f);
        sprite.material = blinkMat;
        yield return new WaitForSeconds(0.1f);
        sprite.material = initMat;
        yield return new WaitForSeconds(0.1f);
        sprite.material = blinkMat;
        yield return new WaitForSeconds(0.1f);
        sprite.material = initMat;
        invulnerable = false;
    }
}
