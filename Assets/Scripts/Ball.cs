using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviourPunCallbacks
{
    [SerializeField] private int speed = 0;
    [SerializeField] private TextScore textScore;
    private Rigidbody rigidbody;
    private Vector2 direction;
    void Start()
    {
        direction = new Vector2(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f));
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (other.collider.name == "TopWall" || other.collider.name == "BottomWall")
            {
                direction.y *= -1;
            }
            else if (other.collider.name == "LeftWall" || other.collider.name == "RightWall")
            {
                direction.x *= -1;
                if (other.collider.name == "LeftWall")
                {
                    textScore.AddRight();
                }
                else if (other.collider.name == "RightWall")
                {
                    textScore.AddLeft();
                }
                transform.position = new Vector2(0, 0);
                direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, -1f));
            }
            else if (other.collider.tag == "Player")
            {
                Debug.Log("Удар о игрока");
                direction.x *= -1;
            }
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        rigidbody.velocity = direction.normalized * speed;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 3 && PhotonNetwork.IsMasterClient)
        {
            gameObject.transform.rotation = new Quaternion(0, 0, Random.Range(0, 1f), Quaternion.identity.w);
            rigidbody.AddForce(Vector3.up * speed);
            ChangeDirection();
        }
    }
}
