using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Network network;
    [SerializeField] private Image image;
    [Header("Max waiting time for player actions")]
    [SerializeField] [Range(1, 10)] private byte Minutes;

    private int MaxTimeSeconds;
    private int remainingTimeSeconds;

    private void Awake()
    {
        MaxTimeSeconds = Minutes * 60;
        remainingTimeSeconds = MaxTimeSeconds;
        network.onUserActions.AddListener(() => remainingTimeSeconds = MaxTimeSeconds);
        Network.instance.onGamePaused.AddListener(() =>
        {
            remainingTimeSeconds = 10;
        });
        Network.instance.onPlayerJoin.AddListener(() =>
        {
            remainingTimeSeconds = MaxTimeSeconds;
        });
    }

    private void Start()
    {
        StartCoroutine(TimerCorutine());
    }

    private IEnumerator TimerCorutine()
    {
        while (true)
        {
            image.fillAmount = (float) remainingTimeSeconds / MaxTimeSeconds;
            if (remainingTimeSeconds-- <= 0)
            {
                Application.Quit();
            }

            yield return new WaitForSeconds(1);
        }
    }
}
