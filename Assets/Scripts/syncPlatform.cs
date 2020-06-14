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
    private Vector3 lastTransform;
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        transform = gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lastTransform != transform.position)
        {
            photonView.RPC("SyncPos", RpcTarget.All, transform.position.x, transform.position.y);
        }
    }

    [PunRPC] private void SyncPos(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
        lastTransform = transform.position;
    }
}
