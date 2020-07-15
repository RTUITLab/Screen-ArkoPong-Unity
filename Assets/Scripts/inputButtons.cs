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
    private syncPlatform SyncPlatform;
    private GameObject myPlatform;
    private bool holdButtonUp = false;
    private bool holdButtonDown = false;
    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (holdButtonUp)
        {
            SyncPlatform.PlatformMove(1);
        }
        if (holdButtonDown)
        {
            SyncPlatform.PlatformMove(-1);
        }
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
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1);
    }

    public void SetMyPlatform(GameObject platform)
    {
        myPlatform = platform;
        try
        {
            SyncPlatform = myPlatform.GetComponent<syncPlatform>();
        }
        catch
        {
            Debug.LogError("Cant get platorm sync script");
        }
    }
}
