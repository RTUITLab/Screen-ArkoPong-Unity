using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TextScore : MonoBehaviour
{
    [SerializeField] private Text text;
    private byte leftCount = 0;
    private byte RightCount = 0;
    public void AddRight()
    {
        ++RightCount;
        updScore();
    }

    public void AddLeft()
    {
        ++leftCount;
        updScore();
    }

    private void updScore()
    {
        text.text = $"{leftCount}:{RightCount}";
    }
}
