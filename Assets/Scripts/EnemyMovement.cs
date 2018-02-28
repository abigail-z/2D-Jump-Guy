using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    public float knockback;
    public LayerMask wallLayers;

    private Rigidbody2D rb;
    private int direction;
    private float widthFromCenter;

	// Use this for initialization
	void OnEnable ()
    {
        rb = GetComponent<Rigidbody2D>();
        widthFromCenter = GetComponent<Collider2D>().bounds.extents.x;

        // get movement direction
        if (Random.value >= 0.5)
        {
            direction = 1; // right
        }
        else
        {
            direction = -1; // left
        }
	}

    void FixedUpdate()
    {
        if (WallCheck())
        {
            direction *= -1;
        }

        Vector2 myVelocity = rb.velocity;
        myVelocity.x = moveSpeed * direction;
        rb.velocity = myVelocity;
    }

    private bool WallCheck()
    {
        if (direction < 0)
        {
            return Physics2D.Raycast(rb.position, Vector2.left, widthFromCenter + 0.1f, wallLayers);
        }
        else
        {
            return Physics2D.Raycast(rb.position, Vector2.right, widthFromCenter + 0.1f, wallLayers);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if (col.CompareTag("Player"))
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            StartCoroutine(pc.TakeDamage(transform.position, knockback));
        }
    }
}
