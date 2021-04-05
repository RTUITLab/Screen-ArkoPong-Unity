using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private qrCode qr;
    [SerializeField] private TextScore score;
    [SerializeField] private Text playerNotifyre;
    [SerializeField] private GameObject ConnectionUi;
    [SerializeField] private GameObject inGameUi;

    private void Awake()
    {
        Network.instance.onConnection.AddListener((link) => qr.GenerateQR(link));
        Network.instance.onPlayerJoin.AddListener(() =>
        {
            if (Network.instance.needPlayers == 1)
            {
                playerNotifyre.text = "Ожидание 1 игрока";
            }
        });
        Network.instance.onGameStart.AddListener(() =>
        {
            ConnectionUi.SetActive(false);
            inGameUi.SetActive(true);
        });
        Network.instance.onGamePaused.AddListener(() =>
        {
            playerNotifyre.text = "Игра приостановлена";
            ConnectionUi.SetActive(true);
            inGameUi.SetActive(false);
        });
        Network.instance.onGameStop.AddListener(() =>
        {
            playerNotifyre.text = "Выход из игры";
        });
    }
}