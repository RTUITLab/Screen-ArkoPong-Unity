﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Xml;

public class Network : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject platformPlayer;
    [SerializeField] private string version = "1.0";
    [SerializeField] private string nameTV = "tv";
    [SerializeField] private Transform[] spawns;
    [SerializeField] private GameObject[] objTV;
    [SerializeField] private GameObject[] objPhone;
    [SerializeField] private inputButtons buttons;

    void Start()
    {
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(nameTV, new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
        Debug.LogError("Create room");
    }
    public override void OnJoinedRoom()
    {
        Debug.LogError($"Join room. Count players {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount < 3)
        {
            for(int i =  0; i < objTV.Length; ++i)
            {
                objTV[i].SetActive(true);
            }
            for(int i = 0; i < objPhone.Length; ++i)
            {
                objPhone[i].SetActive(false);
            }
        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < objTV.Length; ++i)
            {
                objTV[i].SetActive(false);
            }
            for (int i = 0; i < objPhone.Length; ++i)
            {
                objPhone[i].SetActive(true);
            }
        }

        if(PhotonNetwork.LocalPlayer.ActorNumber != 1)
        {
            GameObject platform = PhotonNetwork.Instantiate(platformPlayer.name, spawns[3 - PhotonNetwork.LocalPlayer.ActorNumber].position, Quaternion.identity);
            buttons.SetMyPlatform(platform);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  //Вызываеться, когда кто то заходит, тут запускаем игру если 3 человека в руме
    {
        Debug.Log($"Count ppl in room {PhotonNetwork.CurrentRoom.PlayerCount}");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            for (int i = 0; i < objTV.Length; ++i)
            {
                objTV[i].SetActive(false);
            }
        }
    }

    private void loadLevel(int num)
    {
        PhotonNetwork.LoadLevel(1);
    }
}