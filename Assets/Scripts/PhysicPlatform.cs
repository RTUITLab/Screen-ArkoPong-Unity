using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicPlatform : MonoBehaviour
{
    private float currDirectionSpeed = 0;   //from -1 to 1
    private Rigidbody2D rigidbody;
    private bool collisionDown;
    private bool collisionUp;

    private Vector2 Vector2Vel = new Vector2(0f, 10f);
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
        if (currDirectionSpeed > 0 && !collisionUp)
        {
            return Vector2Vel * currDirectionSpeed;
        }
        else if (currDirectionSpeed < 0 && !collisionDown)
        {
            return Vector2Vel * currDirectionSpeed;
        }
        return Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"CollisionEnter {collision.gameObject.name}");
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
        //Debug.Log($"CollisionExit {collision.gameObject.name}");
        if (collision.gameObject.name == "BottomWall")
        {
            collisionDown = false;
        }
        else if (collision.gameObject.name == "TopWall")
        {
            collisionUp = false;
        }
    }

    public void SetDirection(float directionSpeed)
    {
        currDirectionSpeed = directionSpeed;
    }

    public void Reset()
    {
        gameObject.transform.position = startPos;
    }
}
