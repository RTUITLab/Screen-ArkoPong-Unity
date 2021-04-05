using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine.Events;

public class Network : MonoBehaviour
{
    public static Network instance { private set; get; }
    public static HubConnection hubConnection { private set; get; }
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
        instance = this;
        settings = new Settings(phoneURL, serverURL);
        onGameStart.AddListener(() => gameStart = true);
        onGameStop.AddListener(() =>
        {
            Application.Quit();
        });
        onConnection.AddListener((url) => Debug.Log(url));
        needPlayers = 2;

        hubConnection = new HubConnectionBuilder()
            .WithUrl(settings.serverURL)
            .Build();
    }

    private void Start()
    {
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