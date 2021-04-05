using UnityEngine;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine.Events;

public class Network : MonoBehaviour
{
    public static Network instance { private set; get; }
    private static HubConnection hubConnection;
    [SerializeField] private string phoneURL;
    [SerializeField] private string serverURL;
    [SerializeField] private PhysicPlatform[] platforms;
    [HideInInspector] public ConnectionEvent onConnection;
    [HideInInspector] public UnityEvent onGameStart;
    [HideInInspector] public UnityEvent onGamePaused;
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
        onGameStart.AddListener(() =>
        {
            Debug.Log("Game start");
            gameStart = true;
        });
        onGamePaused.AddListener(() =>
        {
            Debug.Log("Game paused");
        });
        onGameStop.AddListener(() =>
        {
            Debug.Log("Game stop");
            Application.Quit();
        });
        onPlayerJoin.AddListener((() =>
        {
            Debug.Log("Player join");
            --needPlayers;
        }));
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
        hubConnection.On("PauseGame", () => onGamePaused.Invoke());
        hubConnection.On("StartGame", () => onGameStart.Invoke());
        hubConnection.On<string>("SetID", id => onConnection.Invoke($"{settings.phoneURL}#{id}"));
        hubConnection.On("PlayerJoin", () => onPlayerJoin.Invoke());

        hubConnection.On("PlayerLeft", () =>
        {
            Debug.Log("Player left");
            ++needPlayers;
            if (needPlayers == 2)
            {
                onGameStop.Invoke();
            }
            else
            {
                onGamePaused.Invoke();
            }
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
}

[System.Serializable]
public class ConnectionEvent : UnityEvent<string> //link string
{

}