using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public int Speed;
    public int JumpForce;

    public LayerMask WhatIsGround;

    public bool IsGrounded { get { return grounded; } }
    private bool grounded;

	private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
	private SpriteRenderer playerSprite;
    private BoxCollider2D groundCheck;

    private float currentMovementAxis;
    private bool jumpPending;

    private void Start()
    {
		playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
		playerAnimator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
		playerSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        groundCheck = gameObject.transform.Find("Colliders").Find("Ground").gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
		HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && grounded && !(Input.GetAxisRaw("Vertical") < 0))
        {
            jumpPending = true;
        }
        currentMovementAxis = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
	{
		PerformGroundCheck();
		HandleMovement();
		HandleJumping();
		
    }

    void HandleJumping()
    {
        if(jumpPending)
        {
            playerRigidbody.AddForce(new Vector2(0, JumpForce));
			playerAnimator.Play ("jump");
            jumpPending = false;
        }
    }

    void PerformGroundCheck()
    {
        grounded = groundCheck.IsTouchingLayers(WhatIsGround);
		playerAnimator.SetBool ("Grounded", grounded);
    }

    void HandleMovement()
    {
		playerRigidbody.velocity = new Vector2(currentMovementAxis * Speed, playerRigidbody.velocity.y);
		playerAnimator.SetFloat ("Speed", Mathf.Abs(playerRigidbody.velocity.x));
		if (playerRigidbody.velocity.x > 0) {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		} 
		if (playerRigidbody.velocity.x < 0)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x) * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }
}
