using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class reJoin : MonoBehaviourPunCallbacks
{
    public void reJoinBtn()
    {
        //SceneManager.LoadScene(0);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join rand room failed");
    }
}
