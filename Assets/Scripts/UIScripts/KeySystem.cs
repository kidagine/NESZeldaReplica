using UnityEngine;
using UnityEngine.UI;

public class KeySystem : MonoBehaviour
{
    [SerializeField] private Image[] _keys = default;
    private readonly int _maximumKeys = 5;

    public void SetKeys(int keyAmount)
    {
        for (int i = 0; i < _keys.Length; i++)
        {
            if (i < keyAmount)
            {
                _keys[i].enabled = true;
            }
            else
            {
                _keys[i].enabled = false;
            }
        }
    }
}
