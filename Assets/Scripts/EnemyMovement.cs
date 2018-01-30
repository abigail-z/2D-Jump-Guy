using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float moveSpeed;

    private Rigidbody2D rb;
    private int direction;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

        // get movement direction
        if (Random.value >= 0.5)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        Vector2 myVelocity = rb.velocity;
        myVelocity.x = moveSpeed * direction;
        rb.velocity = myVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            direction *= -1;
        }
    }
}
