﻿using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine.Events;

public class Network : MonoBehaviour
{
    private static HubConnection hubConnection;
    [SerializeField] private string phoneURL;
    [SerializeField] private string serverURL;
    [SerializeField] private PhysicPlatform[] platforms;
    [HideInInspector] public ConnectionEvent onConnection;
    [HideInInspector] public UnityEvent onGameStart;
    [HideInInspector] public UnityEvent onGameStop;
    [HideInInspector] public UnityEvent onUserActions;
    [HideInInspector] public UnityEvent onPlayerJoin;
    private Settings settings;

    public static bool gameStart { private set; get; }
    public byte needPlayers { get; private set; }

    private void Awake()
    {
        settings = new Settings(phoneURL, serverURL);
        onGameStart.AddListener(() => gameStart = true);
        onGameStop.AddListener(() =>
        {
            gameStart = false;
            Application.Quit();
        });
        onConnection.AddListener((url) => Debug.Log(url));
        needPlayers = 2;
    }

    private void Start()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(settings.serverURL)
            .Build();

        Connect();

        hubConnection.On<string>("OutsideLog", (msg) => Debug.Log($"Outside Log: {msg}"));
        hubConnection.On<int, float>("SetDirection",(id, direction) =>
        {
            platforms[id].SetDirection(direction);
            onUserActions.Invoke();
        });
        hubConnection.On("StopGame", () => onGameStop.Invoke());
        hubConnection.On<string>("SetID", id => onConnection.Invoke($"{settings.phoneURL}#{id}"));
        hubConnection.On("PlayerJoin", () =>
        {
            if (--needPlayers == 0)
            {
                onGameStart.Invoke();
                return;
            }
            onPlayerJoin.Invoke();
        });

        hubConnection.SendAsync("ConnectTV");
    }

    private static async Task Connect() //Connect to server.
    {
        await hubConnection.StartAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Cant connect");
                Application.Quit();
            }
            else
            {
                Debug.Log("Connected");
            }
        });
    }

    private void ResetGame()
    {
        foreach (var platform in platforms)
        {
            platform.Reset();
        }
    }
}

[System.Serializable]
public class ConnectionEvent : UnityEvent<string> //link string
{

}