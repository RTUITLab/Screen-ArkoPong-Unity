using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.SignalR.Client;

public class Network : MonoBehaviour
{
    private static HubConnection hubConnection;
    private string tvID = null;
    [SerializeField] private TextScore textScore;
    [SerializeField] private string URL;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private PhysicPlatform[] platforms;

    public static bool gameStart { private set; get; }

    private void Start()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(URL)
            .Build();

        Connect();

        hubConnection.On<string>("OutsideLog", (msg) => Debug.Log(msg));
        hubConnection.On<int, int>("SetDirection",(id, direction) => platforms[id].SetDirection((MoveDirection)direction));
        hubConnection.On("StartGame", () => gameStart = true);
        hubConnection.On("RestartGame", () => ResetGame());
        hubConnection.On<string>("SetID", id => tvID = id);

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
                gameStart = true;
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