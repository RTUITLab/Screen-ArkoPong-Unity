using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Network network;
    [SerializeField] private TextScore TextScore;
    // КУСОК КОСТЫЛЯ

    [SerializeField]
    private GameObject ball1Prefab;
    private GameObject ball1Instance;

    private GameObject[] instance = new GameObject[2];
    private int indx = 0;
    private bool spawned = false;

    private void Awake()
    {
        network.onGameStart.AddListener(() =>
        {
            if (!spawned)
            {
                ballDestroyed();
                ballDestroyed();
                spawned = true;
            }
        });
    }

    public void ballDestroyed()
    {
        int index = indx % instance.Length;
        instance[index] = Instantiate(ball1Prefab, new Vector3(0, 0), Quaternion.Euler(0f, 0f, 0f));
        instance[index].GetComponent<Ball>().OnSpawn(TextScore, this);
        indx++;
    }


}
