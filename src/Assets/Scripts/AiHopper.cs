using System.Collections;
using UnityEngine;

public class AiHopper : MonoBehaviour
{
    public float aggroDistance;

    private GameObject player;
    private float playerDistance;
    private bool canAttack = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (playerDistance < aggroDistance && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        var rb = gameObject.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(1f);
        if (player.transform.position.x - transform.position.x < 0)
        {
            rb.AddForce(new Vector2(-9000, 5000));
        }
        else
        {
            rb.AddForce(new Vector2(9000, 5000));
        }
        yield return new WaitForSeconds(.55f);
        canAttack = true;
    }
}
