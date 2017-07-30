using System.Collections;
using UnityEngine;

public class FightingMotor : MonoBehaviour
{
    public int Health;
    public int Damage;

    private Collider2D damageArea;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private HeroDeath deathScript;

    private Vector2 knockbackEffectPending;
    private bool currentlyAttacking;

    public bool invulnerable;

    private void Start()
    {
        damageArea = gameObject.GetComponent<BoxCollider2D>();
        damageArea.enabled = false;
        animator = gameObject.transform.parent.Find("Sprite").GetComponent<Animator>();
        sprite = gameObject.transform.parent.Find("Sprite").GetComponent<SpriteRenderer>();
        deathScript = gameObject.transform.parent.GetComponent<HeroDeath>();
        rb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !currentlyAttacking && gameObject.transform.parent.tag == "Player" && GameManager.EnableInput)
        {
            animator.Play(new[] { "hero_hit_hook", "hero_hit_upper" }[Random.Range(0, 2)]);
            StartCoroutine(Attack(0.15f, 0.2f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var notFriendlyFire = (collider.gameObject.tag == "Enemy" && gameObject.transform.parent.tag == "Player") || (gameObject.transform.parent.tag == "Enemy" && collider.gameObject.tag == "Player");
        if (notFriendlyFire)
        {
            DealDamage(collider.transform.parent.parent.gameObject);
        }
    }

    public IEnumerator Attack(float secondsBeforeAttack, float secondsDuringAttack)
    {
        yield return new WaitForSeconds(secondsBeforeAttack);
        damageArea.enabled = true;
        currentlyAttacking = true;
        yield return new WaitForSeconds(secondsDuringAttack);
        currentlyAttacking = false;
        damageArea.enabled = false;
    }

    public void DealDamage(GameObject enemy)
    {
        var enemyFighingMotor = enemy.transform.Find("FightingSystem").GetComponent<FightingMotor>();
        enemyFighingMotor.GetDamage(Damage, gameObject.transform.position);
    }

    public void GetDamage(int damage, Vector2 attackerPos)
    {
        if (invulnerable) return;

        Health -= damage;
        var lethal = Health <= 0;

        //add knockback here
        knockbackEffectPending = gameObject.transform.position.x - attackerPos.x > 0 ? Vector2.right : Vector2.left;

        //add grace period
        if (gameObject.transform.parent.tag == "Player")
        {
            StartCoroutine(ReciveGracePeriod(lethal));
        }

        //add death check
        if(lethal)
        {
            deathScript.DieAndRespawn();
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

    private IEnumerator ReciveGracePeriod(bool lethal)
    {
        invulnerable = true;
        var mat = sprite.material;
        var dmgCol = new Color(211f, 80f, 57f);
        mat.SetColor("_Color", dmgCol);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_Color", dmgCol);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_Color", dmgCol);
        yield return new WaitForSeconds(0.1f);
        mat.SetColor("_Color", Color.white);
        invulnerable = lethal;
    }
}
