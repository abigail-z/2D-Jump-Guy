using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;
    public uint jumpLenienceTicks;
    public float spinsPerSecond;
    public LayerMask groundLayers;
    public uint maxHealth;
    public float invulnerabilityTime;
    public float damageFlashPeriod;
    public float freezeFrameLength;
    public float freezeFrameTimescale;
    public uint initialHealth;

    // private vars
    private uint health;
    private Rigidbody2D rb;
    private bool isGrounded;
    private int spinDirection;
    private uint jumpWindow;
    private bool knockedBack;
    private bool hurtable;
    private Transform sprite;
    private SpriteRenderer spriteRenderer;
    private float widthFromCenter;
    private float heightFromCenter;
    private Color color;
    // input vars
    private float moveHorizontal;
    private bool jumpPressed;
    private bool jumpReleased;
    private bool alive;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

        sprite = transform.GetChild(0);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        isGrounded = GroundCheck();
        jumpWindow = isGrounded ? jumpLenienceTicks : 0;
        knockedBack = false;
        hurtable = true;
        widthFromCenter = GetComponent<Collider2D>().bounds.extents.x;
        heightFromCenter = GetComponent<Collider2D>().bounds.extents.y;

        spinDirection = 0;
        moveHorizontal = 0f;
        jumpPressed = false;
        jumpReleased = false;

        health = initialHealth;
        GameManager.instance.UpdateHealth(health);
        alive = true;
        color = spriteRenderer.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!knockedBack && alive)
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
                sprite.Rotate(new Vector3(0, 0, spinDirection * 360) * Time.deltaTime * spinsPerSecond);
            }
        }
        else
        {
            sprite.rotation = Quaternion.identity; // resets to no rotation
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
        if (rb.velocity.y > 0)
        {
            return false;
        }

        Vector2 leftCheckOrigin = new Vector2(transform.position.x - widthFromCenter - 0.05f, transform.position.y - heightFromCenter);
        Vector2 rightCheckOrigin = new Vector2(transform.position.x + widthFromCenter + 0.05f, transform.position.y - heightFromCenter);

#if UNITY_EDITOR
        Debug.DrawRay(leftCheckOrigin, Vector2.down, Color.blue);
        Debug.DrawRay(rightCheckOrigin, Vector2.down, Color.blue);
#endif

        RaycastHit2D leftCheck = Physics2D.Raycast(leftCheckOrigin, Vector2.down, 0.1f, groundLayers);
        RaycastHit2D rightCheck = Physics2D.Raycast(rightCheckOrigin, Vector2.down, 0.1f, groundLayers);

        return leftCheck || rightCheck;
    }

    public void TakeDamage(Vector2 enemyPos, float knockBackPower)
    {
        StartCoroutine(DamageCoroutine(enemyPos, knockBackPower));
    }

    internal IEnumerator DamageCoroutine(Vector2 enemyPos, float knockBackPower)
    {
        // only allow one collision to be handled
        if (!hurtable)
        {
            yield break;
        }
        hurtable = false;

        // disable collisions between player and enemy layer
        Physics2D.IgnoreLayerCollision(9, 10, true);

        // start with applying movement, this should happen even when invincible
        // set current velocity to 0 to prevent movement affecting knockback
        rb.velocity = Vector2.zero;

        // if grounded, send at a diagonal
        Vector2 knockBackDirection = rb.position - enemyPos;
        if (isGrounded)
        {
            knockBackDirection = new Vector2(knockBackDirection.x > 0 ? 1 : -1, 1);
        }

        // clamp the magnitude then apply knockback. also set knocked back state to ingore inputs
        Vector2.ClampMagnitude(knockBackDirection, 1);
        rb.AddForce(knockBackDirection * knockBackPower, ForceMode2D.Impulse);
        knockedBack = true;

#if UNITY_EDITOR
        Debug.DrawRay(rb.position, knockBackDirection, Color.magenta, 3);
#endif

        // damage and death
        if (health > 0)
        {
            health--;
            GameManager.instance.UpdateHealth(health);
        }
        else if (alive)
        {
            GameManager.instance.GameOver();
            health = 0;
            color = Color.grey;
            spriteRenderer.color = color;
            alive = false;
            moveHorizontal = 0;
            jumpPressed = false;
            jumpReleased = false;
        }

        // freezeframe
        Time.timeScale = freezeFrameTimescale;
        spriteRenderer.color = Color.white;
        yield return new WaitForSecondsRealtime(freezeFrameLength);
        Time.timeScale = 1;
        spriteRenderer.color = color;

        // start invulnerability timer
        StartCoroutine(InvulnTime());

        // invulerability flashing
        while (!hurtable)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(damageFlashPeriod);
        }
        spriteRenderer.enabled = true;
        knockedBack = false; // just in case, only matters is player is still falling somehow
        Physics2D.IgnoreLayerCollision(9, 10, false); // reenable enemy collisions
    }

    private IEnumerator InvulnTime()
    {
        yield return new WaitForSeconds(invulnerabilityTime);

        hurtable = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kill"))
        {
            GameManager.instance.GameOver();
            gameObject.SetActive(false);
            return;
        }

        if (other.CompareTag("Pickup") && alive)
        {
            PickupBehavior pickup = other.GetComponent<PickupBehavior>();
            pickup.GetPickedUp();
            GameManager.instance.IncreaseScore();
            return;
        }
    }
}
