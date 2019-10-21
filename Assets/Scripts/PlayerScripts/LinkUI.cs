using UnityEngine;

public class LinkUI : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Inventory _inventory = default;
    [SerializeField] private KeySystem _keySystem = default;
    [SerializeField] private HeartSystem _heartSystem = default;
    [Header("Prefabs")]
    [SerializeField] private GameObject _pfbOpenPrompt = default;
    private GameObject _currentlyShownPrompt;

    public Inventory Inventory { get { return _inventory; } private set { _inventory = value; } }
    public KeySystem KeySystem { get { return _keySystem; } private set { _keySystem = value; } }
    public HeartSystem HeartSystem { get { return _heartSystem; } private set { _heartSystem = value; } }

    public void ShowPrompt(Transform player)
    {
        if (_currentlyShownPrompt == null)
        {
            Vector2 uiPosition = Camera.main.WorldToScreenPoint(player.position);
            _currentlyShownPrompt = Instantiate(_pfbOpenPrompt, uiPosition, Quaternion.identity);
        }
    }

    public void HidePrompt()
    {
        Destroy(_currentlyShownPrompt);
    }
}
