using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Xml;

public class Network : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool isTvBild = false;
    [SerializeField] private GameObject platformPlayer;
    [SerializeField] private string version = "1.0";
    [SerializeField] private string nameTV = "tv";
    [SerializeField] private Transform[] spawns;
    [SerializeField] private GameObject[] objTV;
    [SerializeField] private GameObject[] objPhone;
    [SerializeField] private Camera camera;
    public static bool gameStart { private set; get; }

    void Start()
    {
        if (!isTvBild)
        {
            camera.cullingMask = 0;
        }

        if (PhotonNetwork.IsConnected)
        {
            OnConnectedToMaster();
        }
        else
        {
            PhotonNetwork.GameVersion = version;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isTvBild)
        {
            PhotonNetwork.CreateRoom(nameTV, new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Ошибка при входе в комнату: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError($"Join room. Count players {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount < 3)
        {
            for (int i = 0; i < objTV.Length; ++i)
            {
                objTV[i].SetActive(true);
            }
            for (int i = 0; i < objPhone.Length; ++i)
            {
                objPhone[i].SetActive(false);
            }
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            GameObject platform = PhotonNetwork.Instantiate(platformPlayer.name, spawns[PhotonNetwork.CurrentRoom.PlayerCount - 2].position, Quaternion.identity);

            for (int i = 0; i < objTV.Length; ++i)
            {
                objTV[i].SetActive(false);
            }
            for (int i = 0; i < objPhone.Length; ++i)
            {
                objPhone[i].SetActive(true);
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                gameStart = true;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  //Вызываеться, когда кто то заходит, тут запускаем игру если 3 человека в руме
    {
        Debug.Log($"Count ppl in room {PhotonNetwork.CurrentRoom.PlayerCount}");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            gameStart = true;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            for (int i = 0; i < objTV.Length; ++i)
            {
                objTV[i].SetActive(false);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EndGame();
    }

    private void loadLevel(int num)
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void EndGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Вышел с румы");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(isTvBild ? 0 : 1);
    }
}
