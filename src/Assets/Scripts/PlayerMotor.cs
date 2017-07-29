using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public int Speed;
    public int JumpForce;

    public LayerMask WhatIsGround;

    private bool grounded;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D groundCheck;

    private float currentMovementAxis;
    private bool jumpPending;

    private void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        groundCheck = gameObject.transform.Find("Colliders").Find("Ground").gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpPending = true;
        }
        currentMovementAxis = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        HandleJumping();
        HandleMovement();
        PerformGroundCheck();
    }

    void HandleJumping()
    {
        if(jumpPending)
        {
            playerRigidbody.AddForce(new Vector2(0, JumpForce));
            jumpPending = false;
        }
    }

    void PerformGroundCheck()
    {
        grounded = groundCheck.IsTouchingLayers(WhatIsGround);
    }

    void HandleMovement()
    {
        playerRigidbody.velocity = new Vector2(currentMovementAxis * Speed, playerRigidbody.velocity.y);
    }
}
