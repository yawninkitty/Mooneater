using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    public bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Joystick joystick;
    private bool isKnockbacked = false;

    public Animator animator;
    public SoundManager soundManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundManager = GetComponent<SoundManager>();

    }

    private void FixedUpdate()
    {
        if (isKnockbacked) return;
        moveInput = joystick.Horizontal;
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        animator.SetFloat("horizontalMove", Mathf.Abs(moveInput));
        animator.SetFloat("verticalMove", rb.velocity.y);
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    public void PlayFootstepsSound()
    {
        soundManager.PlaySound(0);
    }

    private void Update()
    {
        if (isKnockbacked) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        animator.SetBool("jumping", !isGrounded);


    }

    public void OnJumpButtonDown()
    {
        if (isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            soundManager.PlaySound(1);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void Knockback(Vector2 direction, float force)
    {
        isKnockbacked = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        Invoke(nameof(ResetKnockback), 0.3f); // Возвращаем управление через 0.3 сек
    }

    private void ResetKnockback()
    {
        isKnockbacked = false;
    }
}

