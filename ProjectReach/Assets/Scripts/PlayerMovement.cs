using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction inputMovement;
    private InputAction inputDash;
    private InputAction inputJump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        inputMovement = new InputAction("Move");
        inputJump = new InputAction("Jump");
        inputDash = new InputAction("Sprint");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
