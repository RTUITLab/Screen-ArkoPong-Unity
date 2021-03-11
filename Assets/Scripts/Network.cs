using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine.Events;

public class Network : MonoBehaviour
{
    private static HubConnection hubConnection;
    [SerializeField] private string phoneUrl;
    [SerializeField] private string URL;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private PhysicPlatform[] platforms;
    public ConnectionEvent onConnection;
    public UnityEvent onGameStart;
    public UnityEvent onGameStop;

    public static bool gameStart { private set; get; }

    private void Awake()
    {

    }

    private void Start()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(URL)
            .Build();

        Connect();

        hubConnection.On<string>("OutsideLog", (msg) => Debug.Log(msg));
        hubConnection.On<int, int>("SetDirection",(id, direction) => platforms[id].SetDirection((MoveDirection)direction));
        hubConnection.On("StartGame", () => onGameStart.Invoke());
        hubConnection.On("StopGame", () => onGameStop.Invoke());
        hubConnection.On<string>("SetID", id => onConnection.Invoke($"{phoneUrl}#{id}"));

        hubConnection.SendAsync("ConnectTV");
    }

    private static async Task Connect() //Connect to server.
    {
        await hubConnection.StartAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Cant connect");
                //Application.Quit();
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