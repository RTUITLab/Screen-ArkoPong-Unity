using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicPlatform : MonoBehaviour
{
    private MoveDirection currDirection = MoveDirection.Stay;
    private Rigidbody2D rigidbody;
    private bool collisionDown;
    private bool collisionUp;

    private Vector2 upVector2Vel = new Vector2(0f, 10f);
    private Vector2 downVector2Vel = new Vector2(0f, -10f);
    private Vector2 stayVector2Vel = new Vector2(0,0);
    private Vector3 startPos;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startPos = gameObject.transform.position;
    }

    private void Update()
    {
        if (!Network.gameStart) { return; }
        rigidbody.velocity = GetCurrentVelocity();
    }

    private Vector2 GetCurrentVelocity()
    {
        if (currDirection == MoveDirection.Up && !collisionUp)
        {
            return upVector2Vel;
        }
        else if (currDirection == MoveDirection.Down && !collisionDown)
        {
            return downVector2Vel;
        }
        return stayVector2Vel;
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

    public void SetDirection(MoveDirection direction)
    {
        currDirection = direction;
    }

    public void Reset()
    {
        gameObject.transform.position = startPos;
    }
}
