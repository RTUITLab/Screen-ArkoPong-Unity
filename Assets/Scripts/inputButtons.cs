using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class inputButtons : MonoBehaviourPunCallbacks
{
    public static MoveDirection moveDirection { private set; get; }


    private void Awake()
    {
        moveDirection = MoveDirection.Stay;
    }

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
    }

    public void down(bool isUp)
    {
        if (isUp)
        {
            moveDirection = MoveDirection.Down;
        }
        else
        {
            moveDirection = MoveDirection.Stay;
        }
    }

    public void up(bool isUp)
    {
        if (isUp)
        {
            moveDirection = MoveDirection.Up;
        }
        else
        {
            moveDirection = MoveDirection.Stay;
        }
    }

    public void leaveBtn()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1);
    }
}
