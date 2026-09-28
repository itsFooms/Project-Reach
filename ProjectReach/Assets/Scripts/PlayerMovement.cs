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
    [SerializeField] private TrailRenderer trail;

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
    bool isRight = true;

    Coroutine timerDash;
    Coroutine timerHold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        inputMovement = InputSystem.actions.FindAction("Move");
        inputJump = InputSystem.actions.FindAction("Jump");
        inputDash = InputSystem.actions.FindAction("Sprint");
        inputClimb = InputSystem.actions.FindAction("Grab");

        inputMovement.Enable();
        inputJump.Enable();
        inputDash.Enable();
        inputClimb.Enable(); //i felt like thhis could help

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
            else
            {
                rb.linearVelocityX = 0;
                direction = Vector2.zero;
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

            if (isClimbing && inputMovement.IsPressed() && inputClimb.IsPressed())
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
        setAnimation(direction);

    }

    private void OnCollisionEnter2D(Collision2D collision) //empeche le saut et dash a l'infini
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            canDash = true;
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
        trail.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trail.emitting = false;
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



    //---------------------------ANIMATIONs (made by the goat)--------------------------------------------
    private void SwitchAnim()
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private void setAnimation(Vector2 direction)
    {

        if (direction.x > 0 && !isRight)
        {
            SwitchAnim();
        }
        else if (direction.x < 0 && isRight)
        {
            SwitchAnim();
        }

        if (isDashing)
        {
            animator.Play("Player_dash");

        }
        else if (isClimbing)
        {
            animator.Play("Player_climb");
        }

        else if (!canJump)
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
        else
        {
            if (direction.x == 0)
            {

                animator.Play("Player_idle");
            }
            else
            {
                animator.Play("Player_run");
            }
        }
        
    }
}
