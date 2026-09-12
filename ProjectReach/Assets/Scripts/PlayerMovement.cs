using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 7f;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float dashBoost = 50f;
    [SerializeField] float dashTime = 2f;


    public Rigidbody2D rb;

    InputAction inputMovement;
    InputAction inputDash;
    InputAction inputJump;
   
    Vector2 direction;
    bool canJump = true;
    bool canDash = true;
    bool isDashing = false;

    Coroutine coroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputMovement = InputSystem.actions.FindAction("Move");
        inputJump = InputSystem.actions.FindAction("Jump");
        inputDash = InputSystem.actions.FindAction("Sprint");

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

            //dash
            if (inputDash.WasPerformedThisFrame() && canDash)
            {
                canDash = false;
                direction = (direction != Vector2.zero) ? direction : Vector2.right;

                rb.linearVelocity = (dashBoost * direction);
                coroutine = StartCoroutine(dashTimer());
            }
        }

        //jump
        if (inputJump.WasPressedThisFrame() && canJump == true)
        {
            rb.linearVelocityY = (jumpHeight);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) //empeche le saut et dash a l'infini
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            canDash =  true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
    }

    private IEnumerator dashTimer()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        StopCoroutine(coroutine);
    }
}
