using UnityEngine;
using UnityEngine.UI;

public class KeySystem : MonoBehaviour
{
    [SerializeField] private Image[] _keys = default;
    public int Keys { get; set; }

    public void SetKeys(int keyAmount)
    {
        Keys = keyAmount;
        for (int i = 0; i < _keys.Length; i++)
        {
            if (i < Keys)
            {
                _keys[i].enabled = true;
            }
            else
            {
                _keys[i].enabled = false;
            }
        }
    }

    public void UseKey()
    {
        Keys--;
        SetKeys(Keys);
    }

}
