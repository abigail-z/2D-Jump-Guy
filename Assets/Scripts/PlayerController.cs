using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;
    public uint jumpLenienceTicks;
    public float spinsPerSecond;
    public GameObject sprite;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int spinDirection;
    private uint jumpWindow;
    // input vars
    private float moveHorizontal;
    private bool jumpPressed;
    private bool jumpReleased;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

        isGrounded = GroundCheck();
        jumpWindow = isGrounded ? jumpLenienceTicks : 0;

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
                spinDirection = moveHorizontal > 0 ? -1 : 1; // i hate these
            }
            
            if (spinDirection != 0)
            {
                sprite.transform.Rotate(new Vector3(0, 0, spinDirection * 360) * Time.deltaTime * spinsPerSecond);
            }
        }
        else
        {
            sprite.transform.rotation = Quaternion.identity; // resets to no rotation
            spinDirection = 0;
        }
	}

    // FixedUpdate is called once per game tick
    void FixedUpdate()
    {
        isGrounded = GroundCheck();
        if (isGrounded)
        {
            jumpWindow = jumpLenienceTicks;
        }
        else if(jumpWindow > 0)
        {
            jumpWindow--;
        }

        // horizontal movement
        Vector2 myVelocity = rb.velocity;
        myVelocity.x = moveHorizontal * moveSpeed;

        // jump has to be handled first so the upward velocity is included in rb.velocity
        if (jumpPressed)
        {
            if (isGrounded || jumpWindow > 0)
            {
                myVelocity.y = jumpPower;
                jumpWindow = 0;
            }
            jumpPressed = false;
        }

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
        // apply velocity to the RigidBody2D
        rb.velocity = myVelocity;
    }

    bool GroundCheck()
    {
        Vector2 groundCheckStart = new Vector2(transform.position.x - 0.49f, transform.position.y - 0.5f);
        Vector2 groundCheckEnd = new Vector2(transform.position.x + 0.49f, transform.position.y - 0.51f);
        return Physics2D.OverlapArea(groundCheckStart, groundCheckEnd, groundLayers);
    }
}
