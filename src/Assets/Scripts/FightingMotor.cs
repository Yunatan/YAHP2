using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingMotor : MonoBehaviour
{
    public int Health;
    public int Damage;

    private Collider2D damageArea;
    private Animator animator;

    private bool currentlyAttacking;

    private void Start()
    {
        damageArea = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.transform.parent.Find("Sprite").GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && !currentlyAttacking && gameObject.transform.parent.tag == "Player")
        {
           StartCoroutine(Attack());
        }
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        if (currentlyAttacking)
        {
            Debug.Log(collider);
            var isEnemy = collider.gameObject.tag == "Enemy";
            if (isEnemy)
            {
                DealDamage(collider.gameObject);
            }
        }
    }

    private IEnumerator Attack()
    {
        animator.Play(new[] { "hero_hit_hook", "hero_hit_upper" }[UnityEngine.Random.Range(0, 2)]);
        yield return new WaitForSeconds(0.15f);
        currentlyAttacking = true;
        //var animState = animator.GetCurrentAnimatorStateInfo(0);
        //yield return new WaitWhile(() => (animState.IsName("hero_hit_hook") || animState.IsName("hero_hit_upper")));
        yield return new WaitForSeconds(0.3f);
        currentlyAttacking = false;
    }

    private void DealDamage(GameObject gameObject)
    {
        var enemyFighingMotor = gameObject.transform.Find("FightingSystem").GetComponent<FightingMotor>();
        enemyFighingMotor.GetDamage(Damage);
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
        //add knockback here
        //add grace period
    }
}
