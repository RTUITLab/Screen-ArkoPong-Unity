using UnityEngine;
using UnityEngine.UI;

public class TextScore : MonoBehaviour
{
    [SerializeField] private Text text;
    private byte _leftCount = 0;
    private byte _RightCount = 0;


    public byte leftCount
    {
        get
        {
            return _leftCount;
        }
        private set
        {
            _leftCount = value;
            updScore();
        }
    }

    public byte RightCount
    {
        get
        {
            return _RightCount;
        }
        private set
        {
            _RightCount = value;
            updScore();
        }
    }
    
    public void AddRight()
    {
        ++RightCount;
    }

    public void AddLeft()
    {
        ++leftCount;
    }

    private void updScore()
    {
        text.text = $"{leftCount}:{RightCount}";
    }
}
