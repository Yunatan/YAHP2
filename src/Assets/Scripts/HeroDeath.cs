using System.Collections;
using UnityEngine;

public class HeroDeath : MonoBehaviour
{
    public SpriteRenderer fadeOutSprite;
    public float fadeSpeed;
    public GameObject spawnPoint;

    private Animator playerAnimator;
    private int drawDepth = 5;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void Start()
    {
        playerAnimator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        fadeOutSprite.color = new Color(1, 1, 1, alpha);
    }

    void Update()
    {
        if (Input.GetButtonDown("Respawn"))
        {
            //CANCONTROL = FALSE;===================================================
            StartCoroutine(Fade());
            playerAnimator.Play("hero_death");
            StartCoroutine(Respawn());
        }
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    IEnumerator Fade()
    {
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
    }

    IEnumerator OutOfFade()
    {
        float fadeTime = BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = gameObject.transform;
        gameObject.transform.position = spawnPoint.transform.position;
        yield return new WaitForSeconds(1f);
        StartCoroutine(OutOfFade());
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = null;
        yield return new WaitForSeconds(0.8f);
        playerAnimator.Play("Idle");
        //CANCONTROL = TRUE;=========================================================
    }
}