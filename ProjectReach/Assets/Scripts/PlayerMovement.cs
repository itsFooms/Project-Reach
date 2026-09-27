using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 7f;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float dashBoost = 50f;
    [SerializeField] float dashTime = 2f;
    [SerializeField] float climbTime = 4f;

    private Animator animator;
    public Rigidbody2D rb;

    InputAction inputMovement;
    InputAction inputDash;
    InputAction inputJump;
    InputAction inputClimb;
   
    Vector2 direction;
    bool canJump = true;
    bool canDash = true;
    bool isDashing = false;
    bool isGrounded;
    bool isClimbing;

    Coroutine timerDash;
    Coroutine timerHold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        inputMovement = InputSystem.actions.FindAction("Move");
        inputJump = InputSystem.actions.FindAction("Jump");
        inputDash = InputSystem.actions.FindAction("Sprint");
        inputClimb = InputSystem.actions.FindAction("Hold");

        inputMovement.Enable();
        inputJump.Enable();
        inputDash.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            //deplacement
            if (inputMovement.IsPressed())
            {
                direction = inputMovement.ReadValue<Vector2>().normalized;
                rb.linearVelocityX = (direction.x * movementSpeed);
            }
            else if (!inputMovement.IsPressed())
            {
                rb.linearVelocityX = 0;
            }

            //jump
            if (inputJump.WasPressedThisFrame() && canJump == true)
            {
                rb.linearVelocityY = (jumpHeight);
            }

            //dash
            if (inputDash.WasPressedThisFrame() && canDash)
            {
                canDash = false;
                direction = (direction != Vector2.zero) ? direction : Vector2.right;

                rb.linearVelocity = (dashBoost * direction);
                timerDash = StartCoroutine(dashTimer());
            }

            if(isClimbing && inputMovement.IsPressed() && inputClimb.IsPressed())
            {
                direction = inputMovement.ReadValue<Vector2>().normalized;
                if (!isGrounded)
                {
                    rb.linearVelocityY = (direction.y * movementSpeed);
                }
                else
                {
                    rb.linearVelocityY = (direction.y * movementSpeed) * 2;
                }
                
            }

        }

        //animation
        setAnimation(direction.x);

    }

    private void OnCollisionEnter2D(Collision2D collision) //empeche le saut et dash a l'infini
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            canDash =  true;
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Walls") && inputClimb.WasPressedThisFrame() && isGrounded == false)
        {
            rb.gravityScale = 0;
            isClimbing = true;

            if (timerHold != null)
            {
                timerHold = StartCoroutine(holdTimer());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            rb.gravityScale = 1;
        }
    }

    private IEnumerator dashTimer()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        StopCoroutine(timerDash);
    }

    private IEnumerator holdTimer()
    {
        yield return new WaitForSecondsRealtime(climbTime);
        rb.gravityScale = 1;
        timerHold = null;

        StopCoroutine(timerHold);
    }

    private void setAnimation(float moveX)
    {
        if (!canJump)
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Player_jump");
            }
            else
            {
                animator.Play("Player_fall");

            }
        }
        else if (moveX==0) {

            animator.Play("Player_idle");
        }
        else if (moveX > 0)
        {
            animator.Play("Player_run");
        }
    }
}
