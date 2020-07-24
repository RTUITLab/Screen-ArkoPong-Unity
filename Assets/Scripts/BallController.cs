using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{


    // КУСОК КОСТЫЛЯ

    [SerializeField]
    private GameObject ball1Prefab;
    private GameObject ball1Instance;

    private GameObject[] instance = new GameObject[2];
    private int indx = 0;

    void Start()
    {
        ballDestroyed();
        ballDestroyed();
    }

    public void ballDestroyed()
    {
        instance[indx%instance.Length] = Instantiate(ball1Prefab, new Vector3(0, 0), Quaternion.Euler(0f, 0f, 0f));
        instance[indx % instance.Length].GetComponent<Ball>().addForce();
        instance[indx % instance.Length].GetComponent<Ball>().controller = this;
        indx++;
    }


}
