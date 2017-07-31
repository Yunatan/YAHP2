using UnityEngine;

public class MonsterDeath : MonoBehaviour, IDeathScript
{
    public void Die()
    {
        var visual = gameObject.transform.Find("Sprite");
        var cols = gameObject.transform.Find("Colliders");
        var rb = gameObject.GetComponent<Rigidbody2D>();

        gameObject.GetComponent<AiHopper>().StopAllCoroutines();

        MonoBehaviour[] comps = gameObject.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            if (c != rb)
            {
                Destroy(c);
            }
        }
        foreach (Transform c in gameObject.transform)
        {
           if (c != visual && c != cols)
            {
                Destroy(c.gameObject);
            }
        }
        visual.GetComponent<Animator>().Play("hopper_die");
        rb.velocity = Vector2.zero;
    }
}
