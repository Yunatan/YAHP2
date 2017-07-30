using System.Collections;
using UnityEngine;

public class DropDownThruPlatformScript : MonoBehaviour
{
    private bool dropDownPending;
    private bool droppingDownThruPlatform;
    private PlayerMotor playerMotor;

    private void Start()
    {
        playerMotor = gameObject.GetComponent<PlayerMotor>();
    }

    private void FixedUpdate()
    {
        if (dropDownPending)
        {
            playerMotor.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 100);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryToDropDown(collision);
    }

    private void TryToDropDown(Collision2D collision)
    {
        if (dropDownPending)
        {
            if (collision.collider.usedByEffector)
            {
                var effector = collision.collider.gameObject.GetComponent<PlatformEffector2D>();
                droppingDownThruPlatform = true;
                StartCoroutine(DropDown(effector, collision));
            }

            dropDownPending = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.usedByEffector && droppingDownThruPlatform)
        {
            droppingDownThruPlatform = false;
        }
    }

    IEnumerator DropDown(PlatformEffector2D effector, Collision2D collision)
    {
        effector.rotationalOffset = 180;
        yield return new WaitWhile(() => droppingDownThruPlatform);
        effector.rotationalOffset = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && playerMotor.IsGrounded && GameManager.EnableInput)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                dropDownPending = true;
            }
        }
    }
}
