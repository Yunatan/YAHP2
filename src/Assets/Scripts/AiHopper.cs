using System.Collections;
using UnityEngine;

public class AiHopper : MonoBehaviour
{
    public float aggroDistance;

    private GameObject player;
    private float playerDistance;
    private bool canAttack = true;
	private Animator animation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		animation = gameObject.GetComponent<Animator> ();
    }

    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (playerDistance < aggroDistance && canAttack)
        {
            StartCoroutine(Attack());
        }

		if (player.transform.position.x - transform.position.x < 0) {
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
		} else {
			gameObject.GetComponent<SpriteRenderer> ().flipX = false;
		}
    }

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag =="Ground") {
			canAttack = true;
			animation.Play ("hopper_idle");
		}
	}

    IEnumerator Attack()
    {
        canAttack = false;
        var rb = gameObject.GetComponent<Rigidbody2D>();
		yield return new WaitForSeconds(0.8f);
		animation.Play ("hopper_hop");
		yield return new WaitForSeconds(0.2f);
        if (player.transform.position.x - transform.position.x < 0)
        {
            rb.AddForce(new Vector2(-9000, 5000));
        }
        else
        {
            rb.AddForce(new Vector2(9000, 5000));
        }
        yield return new WaitForSeconds(1f);
    }
}
