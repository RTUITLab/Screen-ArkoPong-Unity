using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicPlatform : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Vector2 velocity = new Vector2(0f, 0f);
    bool collisionDown;
    bool collisionUp;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!Network.gameStart) { return; }

        if (inputButtons.moveDirection == MoveDirection.Up && !collisionUp)
        {
            rigidbody.velocity = new Vector2(0f,10f);
        }
        else if (inputButtons.moveDirection == MoveDirection.Down && !collisionDown)
        {
            rigidbody.velocity = new Vector2(0f, -10f);
        }
        else
        {
            rigidbody.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"CollisionEnter {collision.gameObject.name}");
        if(collision.gameObject.name == "BottomWall")
        {
            collisionDown = true;
        }
        else if (collision.gameObject.name == "TopWall")
        {
            collisionUp = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"CollisionExit {collision.gameObject.name}");
        if (collision.gameObject.name == "BottomWall")
        {
            collisionDown = false;
        }
        else if (collision.gameObject.name == "TopWall")
        {
            collisionUp = false;
        }
    }
}
