using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Network network;
    [SerializeField] private qrCode qr;
    [SerializeField] private TextScore score;
    [SerializeField] private GameObject ConnectionUi;
    [SerializeField] private GameObject inGameUi;

    private void Awake()
    {
        network.onConnection.AddListener((link) => qr.GenerateQR(link));
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
