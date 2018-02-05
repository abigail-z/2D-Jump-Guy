﻿#define DEBUG

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
    public uint maxHealth;
    public uint invincibilityTime;

    // private vars
    private uint health;
    private Rigidbody2D rb;
    public bool isGrounded;
    private int spinDirection;
    private uint jumpWindow;
    public bool knockedBack;
    public bool hurtable;
    private SpriteRenderer spriteRenderer;
    // input vars
    private float moveHorizontal;
    private bool jumpPressed;
    private bool jumpReleased;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();

        isGrounded = GroundCheck();
        jumpWindow = isGrounded ? jumpLenienceTicks : 0;
        knockedBack = false;
        hurtable = true;

        spinDirection = 0;
        moveHorizontal = 0f;
        jumpPressed = false;
        jumpReleased = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!knockedBack)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                jumpPressed = true;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                jumpReleased = true;
            }
        }

        if (!isGrounded)
        {
            if (rb.velocity.x != 0)
            {
                spinDirection = rb.velocity.x > 0 ? -1 : 1; // i hate these
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

        // don't allow input during knockback
        if (knockedBack)
        {
            if (!isGrounded)
            {
                return;
            }
            knockedBack = false;
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
        Vector2 leftCheckOrigin = new Vector2(transform.position.x - 0.5f, transform.position.y);
        Vector2 rightCheckOrigin = new Vector2(transform.position.x + 0.5f, transform.position.y);

        #if DEBUG
        Debug.DrawRay(leftCheckOrigin, Vector2.down, Color.blue);
        Debug.DrawRay(rightCheckOrigin, Vector2.down, Color.blue);
        #endif

        bool leftCheck = Physics2D.Raycast(leftCheckOrigin, Vector2.down, 0.52f, groundLayers);
        bool rightCheck = Physics2D.Raycast(rightCheckOrigin, Vector2.down, 0.52f, groundLayers);
        return leftCheck || rightCheck;
    }

    internal IEnumerator TakeDamage(Vector2 enemyPos, float knockBackPower)
    {
        // start with applying movement, this should happen even when invincible
        // set current velocity to 0 to prevent movement affecting knockback
        rb.velocity = Vector2.zero;

        Vector2 knockBackDirection = (Vector2)rb.position - enemyPos;
        if (isGrounded)
        {
            knockBackDirection += Vector2.up;
        }

        Vector2.ClampMagnitude(knockBackDirection, 1);
        rb.AddForce(knockBackDirection * knockBackPower, ForceMode2D.Impulse);
        Debug.DrawRay(rb.position, knockBackDirection, Color.magenta, 3);

        // if currently invincible from recent damage, exit here
        if (!hurtable)
        {
            yield break;
        }

        // freezeframe
        Time.timeScale = 0.1f;
        spriteRenderer.color = Color.white;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
        spriteRenderer.color = Color.green;

        // set up the state of knocked back
        knockedBack = true;
        hurtable = false;

        // damage and death
        health--;
        if (health <= 0)
        {
            // TODO: do death stuff
            // don't continue with rest of stuff so body bounces around without doing invuln flash
            yield break;
        }
        
        // code executed along with each FixedUpdate
        for (uint i = 0; i < invincibilityTime; i++)
        {
            // maybe this should be framerate dependent instead of tick dependent?
            if (spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }

            yield return new WaitForFixedUpdate();
        }
        knockedBack = false;
        hurtable = true;
    }
}
