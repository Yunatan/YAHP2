using UnityEngine;

public class MonsterDeath : MonoBehaviour, IDeathScript
{
    public void Die()
    {
        var visual = gameObject.transform.Find("Sprite").gameObject;

        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        MonoBehaviour[] comps = gameObject.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            c.enabled = false;
        }
        foreach (Transform c in gameObject.transform)
        {
            c.gameObject.SetActive(false);
        }

        visual.SetActive(true);

        //TODO: play death anim
    }
}
