using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public static bool canMove = true;

    private float horizontal;
    private bool isFacingRight = true;

    // uses [SerializeField] to save the runtime game state and tweak variables in the editor 

    [Header("Movement")]

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpingPower = 8f;
    private bool isJumping;

    [SerializeField] private bool canDoubleJump;
    [SerializeField] private float doubleJumpingPower = 8f;
    private bool doubleJump;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteCounter;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioSource footSteps;

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        audioManager = AudioManager.instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!canMove)
        {
            animator.SetFloat("Speed", 0f);
            rb.velocity = new Vector2(0f, 0f);
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("isJumping", isJumping);

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            isJumping = false;
            doubleJump = false;
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteCounter > 0f || (doubleJump && canDoubleJump))
            {
                audioManager.PlaySFX(audioManager.jump);
                coyoteCounter = 0;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);

                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && IsGrounded())
        {
            footSteps.enabled = true;
        }
        else
        {
            footSteps.enabled = false;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
    }

    private void Flip() // flips the sprite scale depending on direction
    {
        if (!canMove)
        {
            return;
        }

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}