using UnityEngine;

public class LinkUI : MonoBehaviour
{
    [SerializeField] private Inventory _inventory = default;
    [SerializeField] private KeySystem _keySystem = default;
    [SerializeField] private HeartSystem _heartSystem = default;


    public Inventory Inventory { get { return _inventory; } private set { _inventory = value; } }
    public KeySystem KeySystem { get { return _keySystem; } private set { _keySystem = value; } }
    public HeartSystem HeartSystem { get { return _heartSystem; } private set { _heartSystem = value; } }
}
