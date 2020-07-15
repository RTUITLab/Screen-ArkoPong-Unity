using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class reJoin : MonoBehaviourPunCallbacks
{
    public void reJoinBtn()
    {
        SceneManager.LoadScene(0);
    }
}
