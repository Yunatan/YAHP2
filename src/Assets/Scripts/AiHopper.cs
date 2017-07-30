using System.Collections;
using UnityEngine;

public class AiHopper : MonoBehaviour
{
    public float aggroDistance;
    public LayerMask WhatIsGround;

    private GameObject player;
    private float playerDistance;
    private bool canAttack = true;
	private Animator hopperAnimation;
	private FightingMotor fightingMotor;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		hopperAnimation = gameObject.transform.Find("Sprite").GetComponent<Animator> ();
        fightingMotor = gameObject.transform.Find("FightingSystem").GetComponent<FightingMotor> ();
    }

    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (playerDistance < aggroDistance && canAttack)
        {
            StartCoroutine(Attack());
        }

		if (player.transform.position.x - transform.position.x < 0) {
			gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer> ().flipX = true;
		} else {
			gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer> ().flipX = false;
		}
    }

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.layer == 8) {
            StopAllCoroutines();
			canAttack = true;
			hopperAnimation.Play ("hopper_idle");
		}
	}

    IEnumerator Attack()
    {
        canAttack = false;
        StartCoroutine(fightingMotor.Attack(1.7f, .4f));
        var rb = gameObject.GetComponent<Rigidbody2D>();
		yield return new WaitForSeconds(1.5f);
		hopperAnimation.Play ("hopper_hop");
		yield return new WaitForSeconds(0.2f);
        if (player.transform.position.x - transform.position.x < 0)
        {
            rb.AddForce(new Vector2(-9000, 5000));
        }
        else
        {
            rb.AddForce(new Vector2(9000, 5000));
        }
        yield return new WaitForSeconds(.4f);
    }
}
