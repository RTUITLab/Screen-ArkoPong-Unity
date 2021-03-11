﻿using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine.Events;

public class Network : MonoBehaviour
{
    private static HubConnection hubConnection;
    [SerializeField] private string phoneUrl;
    [SerializeField] private string URL;
    [SerializeField] private PhysicPlatform[] platforms;
    [HideInInspector] public ConnectionEvent onConnection;
    [HideInInspector] public UnityEvent onGameStart;
    [HideInInspector] public UnityEvent onGameStop;
    private Settings settings = new Settings();

    public static bool gameStart { private set; get; }

    private void Awake()
    {
        Setup(ref settings);
        onGameStart.AddListener(() => gameStart = true);
        onGameStop.AddListener(() =>
        {
            gameStart = false;
            Application.Quit();
        });
        onConnection.AddListener((url) => Debug.Log(url));
    }

    private void Start()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(settings.serverURL)
            .Build();

        Connect();

        hubConnection.On<string>("OutsideLog", (msg) => Debug.Log($"Outside Log: {msg}"));
        hubConnection.On<int, float>("SetDirection",(id, direction) => platforms[id].SetDirection(direction));
        hubConnection.On("StartGame", () => onGameStart.Invoke());
        hubConnection.On("StopGame", () => onGameStop.Invoke());
        hubConnection.On<string>("SetID", id => onConnection.Invoke($"{settings.phoneURL}#{id}"));

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

    private void Setup(ref Settings settings)
    {
        string path = Directory.GetCurrentDirectory() + "/settings.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonUtility.FromJson<Settings>(json);
        }
        else
        {
            settings.phoneURL = phoneUrl;
            settings.serverURL = URL;
            File.WriteAllText(path, JsonUtility.ToJson(settings));
        }
    }
}

[System.Serializable]
public class ConnectionEvent : UnityEvent<string> //link string
{

}