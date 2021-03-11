using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Network network;
    [Header("Max waiting time for player actions")]
    [SerializeField] [Range(1, 10)] private byte Minutes;

    private int remainingTimeSeconds;

    private void Awake()
    {
        remainingTimeSeconds = Minutes;
        network.onUserActions.AddListener(() => remainingTimeSeconds = Minutes);
    }

    private void Start()
    {
        StartCoroutine(TimerCorutine());
    }

    private IEnumerator TimerCorutine()
    {
        while (true)
        {
            if (remainingTimeSeconds-- <= 0)
            {
                Application.Quit();
            }

            yield return new WaitForSeconds(60);
        }
    }
}
