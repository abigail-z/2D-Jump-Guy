using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;
    public float spinSpeed;
    public GameObject sprite;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int spinDirection;
    // input vars
    private float moveHorizontal;
    private bool jumpPressed;
    private bool jumpReleased;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 groundCheckStart = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        Vector2 groundCheckEnd = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f);
        isGrounded = Physics2D.OverlapArea(groundCheckStart, groundCheckEnd, groundLayers);

        spinDirection = 0;
        moveHorizontal = 0f;
        jumpPressed = false;
        jumpReleased = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        } else if (Input.GetButtonUp("Jump"))
        {
            jumpReleased = true;
        }

        if (!isGrounded)
        {
            if (moveHorizontal != 0)
            {
                spinDirection = moveHorizontal > 0 ? -1 : 1;
            }
            
            sprite.transform.Rotate(new Vector3(0,0, spinDirection) * Time.deltaTime * spinSpeed);
        }
        else
        {
            sprite.transform.rotation = Quaternion.identity;
            spinDirection = 0;
        }
	}

    // FixedUpdate is called once per game tick
    void FixedUpdate()
    {
        Vector2 groundCheckStart = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        Vector2 groundCheckEnd = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f);
        isGrounded = Physics2D.OverlapArea(groundCheckStart, groundCheckEnd, groundLayers);

        // jump has to be handled first so the upward velocity is included in rb.velocity
        if (jumpPressed)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpPower);
            }
            jumpPressed = false;
        }

        Vector2 myVelocity = rb.velocity;
        myVelocity.x = moveHorizontal * moveSpeed;

        // jumpReleased handled after jump and movement to minimize rewriting rb.velocity
        // and to handle the case where both jumpPressed and jumpReleased are true on the same FixedUpdate
        if (jumpReleased)
        {
            if (!isGrounded && rb.velocity.y > 0)
            {
                myVelocity.y = 0; // stop upward movement
            }
            jumpReleased = false;
        }
        rb.velocity = myVelocity;
    }
}
