using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _itemsInventory = default;
    public static Inventory Instance { get; private set; }
    private List<ItemDescriptor> _items = new List<ItemDescriptor>();
    private InventorySlot[] _inventorySlots;
    private int _inventorySpace = 12;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _inventorySlots = _itemsInventory.GetComponentsInChildren<InventorySlot>();
    }

    public void Add(ItemDescriptor item)
    {
        if (_items.Count >= _inventorySpace)
        {
            return;
        }
        _items.Add(item);
        AddToInventoryUI();
    }

    private void AddToInventoryUI()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (i < _items.Count)
            {
                _inventorySlots[i].AddItem(_items[i]);
            }
        }
    }
}
