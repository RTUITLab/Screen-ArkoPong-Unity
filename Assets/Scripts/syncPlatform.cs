using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PhotonView))]
public class syncPlatform : MonoBehaviour
{
    private PhotonView photonView;
    private Transform transform;
    private Vector3 moveTo;
    private float speed = 5f;
    private int collisionStatus = 0;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        transform = gameObject.transform;
        moveTo = transform.position;
    }
    void Update()
    {
        if (transform.position.y != moveTo.y && Vector3.Distance(transform.position, moveTo) > 0.1f) //Сглаживание передвижения
        {
            Vector3 direction = (moveTo - transform.position).normalized;
            transform.Translate(Time.deltaTime * direction * speed); 
        }
    }

    public void SendPositon()
    {
        photonView.RPC("SyncPos", RpcTarget.All, transform.position.x, transform.position.y);
    }

    [PunRPC] private void SyncPos(float x, float y)
    {
        moveTo = new Vector3(x, y, 0);
    }

    public void PlatformMove(int direction)
    {
        if (direction != collisionStatus)
        {
            transform.Translate(Vector3.up * direction * 0.1f);

            // Так как платформа должна иметь компонент RigidBody2D, то двигать ее с помощью Translate нельзя, иначе начнется будут траблы с физикой и отскоком мяча
            // Поэтому нужно использовать метод:
            // GetComponent<Rigidbody2D>().AddForce(Vector2.up * direction * 0.1f);
            // Потестить пока не успел, но должно работать

            SendPositon();
            return;
        }
        Debug.LogError("Незя так двигаться");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.collider.name == "TopWall")
        {
            collisionStatus = 1;
        }
        else if(collision.collider.name == "BottomWall")
        {
            collisionStatus = -1;
        }
        Debug.Log(collision.gameObject.tag);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "TopWall" || collision.collider.name == "BottomWall")
        {
            collisionStatus = 1;
        }
    }
}
