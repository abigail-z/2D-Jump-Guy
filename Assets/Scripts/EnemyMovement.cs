using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private int direction;
    private float radius;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        radius = GetComponent<Collider2D>().bounds.extents.x;

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
	
	// Update is called once per frame
	void Update ()
    {
		
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
            return Physics2D.Raycast(rb.position, Vector2.left, radius + 0.1f, groundLayers);
        }
        else
        {
            return Physics2D.Raycast(rb.position, Vector2.right, radius + 0.1f, groundLayers);
        }
    }
}
