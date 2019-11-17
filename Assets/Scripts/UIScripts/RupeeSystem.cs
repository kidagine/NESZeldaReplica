using UnityEngine;
using UnityEngine.UI;

public class RupeeSystem : MonoBehaviour
{
    [SerializeField] private Text _rupees = default;
    private int _totalRupees;


    public void SetRupees(int rupeeAmount)
    {
        if (_totalRupees < 1000)
        {
            _totalRupees += rupeeAmount;
            _rupees.text = _totalRupees.ToString();
        }
    }
}
