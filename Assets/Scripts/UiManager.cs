using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Network network;
    [SerializeField] private qrCode qr;
    [SerializeField] private TextScore score;
    [SerializeField] private Text playerNotifyre;
    [SerializeField] private GameObject ConnectionUi;
    [SerializeField] private GameObject inGameUi;

    private void Awake()
    {
        network.onConnection.AddListener((link) => qr.GenerateQR(link));
        network.onPlayerJoin.AddListener(() =>
        {
            if (network.needPlayers == 1)
            {
                playerNotifyre.text = "Ожидание 1 игрока";
            }
        });
        network.onGameStart.AddListener(() =>
        {
            ConnectionUi.SetActive(false);
            inGameUi.SetActive(true);
        });
        network.onGameStop.AddListener(() =>
        {
            ConnectionUi.SetActive(true);
            inGameUi.SetActive(false);
        });
    }
}
