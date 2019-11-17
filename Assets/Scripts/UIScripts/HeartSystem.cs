using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    [SerializeField] private Image[] _hearts = default;
    [SerializeField] private Sprite _fullHeart = default;
    [SerializeField] private Sprite _halfHeart = default;
    [SerializeField] private Sprite _emptyHeart = default;


    public void SetHearts(int heartContainers, int currentHearts)
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            if (i < heartContainers)
            {
                _hearts[i].enabled = true;
                _hearts[i].sprite = _emptyHeart;
            }
            else
            {
                _hearts[i].enabled = false;
            }
        }

        for (int i = 0; i < currentHearts; i++)
        {
            int heartIndex = i;
            if (heartIndex % 2 == 0)
            {
                heartIndex -= heartIndex / 2;
                _hearts[heartIndex].sprite = _halfHeart;
            }
            else
            {
                heartIndex -= (heartIndex + 1) / 2;
                _hearts[heartIndex].sprite = _fullHeart;
            }
        }
    }
}
