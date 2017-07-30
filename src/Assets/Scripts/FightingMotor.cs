using System.Collections;
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
        damageArea.enabled = false;
        animator = gameObject.transform.parent.Find("Sprite").GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && !currentlyAttacking && gameObject.transform.parent.tag == "Player")
        {
           StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider);
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
        //add death check
    }
}
