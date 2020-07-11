using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class syncPlatform : MonoBehaviour
{
    private PhotonView photonView;
    private Transform transform;
    private Vector3 moveTo;
    private float speed = 5f; 

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
}
