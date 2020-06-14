using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class inputButtons : MonoBehaviourPunCallbacks
{
    [SerializeField] private float speed;
    [SerializeField] private syncBtn syncBtn;
    [SerializeField] private GameObject[] Players;
    private GameObject myPlatform;
    private Transform platformTransform;
    private bool holdButtonUp = false;
    private bool holdButtonDown = false;
    private int actNum;
    public void Start()
    {
        actNum = PhotonNetwork.LocalPlayer.ActorNumber - 2;
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (holdButtonUp)
        {
            MoveBall(1);
        }
        if (holdButtonDown)
        {
            MoveBall(-1);
        }
    }

    private void MoveBall(int direction)  //1 - вверх, -1 - вниз
    {
        platformTransform.Translate(Vector3.up * direction * speed);
        Debug.Log("local ball move");
    }

    public void down(bool isUp)
    {
        if (isUp) 
        {
            holdButtonUp = true;
        }
        else
        {
            holdButtonDown = true;
        }
    }

    public void up(bool isUp)
    {
        if (isUp)
        {
            holdButtonUp = false;
        }
        else
        {
            holdButtonDown = false;
        }
    }

    public void leaveBtn()
    {
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.Disconnect();
        SceneManager.LoadScene(2);
    }

    public void SetMyPlatform(GameObject platform)
    {
        myPlatform = platform;
        platformTransform = myPlatform.transform;
    }
}
