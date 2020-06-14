using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class syncBtn : MonoBehaviourPunCallbacks
{ 
    [SerializeField] private Transform[] Balls;
    private int actNum = 0;
    private PhotonView photonView;
    private Transform lastTransform;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        actNum = PhotonNetwork.LocalPlayer.ActorNumber - 2;
        if (!PhotonNetwork.IsMasterClient)
        {
            lastTransform = Balls[actNum];
        }
    }
}
