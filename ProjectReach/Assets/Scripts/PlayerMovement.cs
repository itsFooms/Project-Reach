using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 7f;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float dashBoost = 50f;
    [SerializeField] float slideSpeed = 2f;
    [SerializeField] float climbSpeed = 4f;

    [SerializeField] float dashTime = 2f;
    [SerializeField] float climbTime = 4f;
    
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float jumpMultiplier = 2f;
    [SerializeField] private TrailRenderer trail;


    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask groundTile;
    [SerializeField] LayerMask wallTile;
    [SerializeField] float checkZone = 0.2f;



    private Animator animator;
    public Rigidbody2D rb;
    private Collider2D colliderino;

    InputAction inputMovement;
    InputAction inputDash;
    InputAction inputJump;
    InputAction inputClimb;

    Vector2 direction;
    bool canJump = true;
    bool canDash = true;
    bool isDashing = false;
    bool isGrounded;
    bool canClimb = false;
    bool onWall;
    bool isRight = true;
    bool finished = true;

    Coroutine timerDash;
    Coroutine timerHold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        colliderino = GetComponent<Collider2D>();
        inputMovement = InputSystem.actions.FindAction("Move");
        inputJump = InputSystem.actions.FindAction("Jump");
        inputDash = InputSystem.actions.FindAction("Sprint");
        inputClimb = InputSystem.actions.FindAction("Grab");

        inputMovement.Enable();
        inputJump.Enable();
        inputDash.Enable();
        inputClimb.Enable();

    }

    // Update is called once per frame
    void Update()
    {

        CheckEnvironnement();

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
                rb.linearVelocity = (Vector2.up * jumpHeight);
            }

            //Variable jump 
            if (!onWall)
            {
                if (rb.linearVelocity.y < 0)
                {
                    rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                else if (rb.linearVelocity.y > 0 && !inputJump.IsPressed())
                {
                    rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
                }
            }

            //dash
            if (inputDash.WasPressedThisFrame() && canDash)
            {
                canDash = false;
                direction = (direction != Vector2.zero) ? direction : Vector2.right;

                rb.linearVelocity = (dashBoost * direction);
                timerDash = StartCoroutine(dashTimer());
            }


            //Sliding and climbing 
            if (onWall && inputClimb.IsPressed())
            {
                    if (finished) { timerHold = StartCoroutine(holdTimer()); } 

                    if (canClimb)
                    {
                        rb.linearVelocity = new Vector2(0, climbSpeed * inputMovement.ReadValue<Vector2>().y);
                    }
                    else { rb.linearVelocity = new Vector2(rb.linearVelocityX, -slideSpeed); }
                    
            }
        }

        //animation
        setAnimation(direction);

    }
    private void CheckEnvironnement()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkZone, groundTile);
        onWall = Physics2D.OverlapCircle(wallCheck.position, checkZone, wallTile) && !isGrounded;

        if (isGrounded)
        {
            canJump = true;
            canDash = true;
        }
        else
        {
            canJump = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) //empeche le saut et dash a l'infini
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            canDash = true;
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            onWall = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
            canClimb = true;
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            onWall = false;
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
        finished = false;
        yield return new WaitForSecondsRealtime(climbTime);

        finished = true;
        canClimb = false;

        StopCoroutine(timerHold);
    }



    //---------------------------ANIMATIONs (made by the goat)-------------------------------------------- type shit man
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
        else if (onWall && !isGrounded)
        {
            if(canClimb && direction.y > 0)
            {
                animator.Play("Player_climb");
            }
            else
            {
                animator.Play("Player_slidewall");
            }
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
