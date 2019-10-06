using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemSlot _itemSlot = default;
    [SerializeField] private GameObject _itemsInventory = default;
    public static Inventory Instance { get; private set; }
    private List<ItemDescriptor> _items = new List<ItemDescriptor>();
    private InventorySlot[] _inventorySlots;


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
        if (item.isConsumamble)
        {
            item.consumambleUses++;
        }

        bool itemExists = CheckItemExists(item);
        if (!itemExists)
        {
            _items.Add(item);
        }
        UpdateInventoryUI(itemExists);
    }

    public void UseConsumamble(ItemDescriptor item)
    {
        item.consumambleUses--;
        _inventorySlots[0].IncrementItemCounter(item);
        if (item.consumambleUses == 0)
        {
            _itemSlot.EmptyItemSlot();
            _items.Remove(item);
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI(bool itemExists = false)
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (i < _items.Count)
            {
                if (!itemExists)
                {
                    _inventorySlots[i].AddItem(_items[i]);
                }
                else
                {
                    _inventorySlots[i].IncrementItemCounter(_items[i]);
                }
            }
            else
            {
                _inventorySlots[i].ClearSlot();
            }
        }
    }

    private bool CheckItemExists(ItemDescriptor itemToAdd)
    {
        foreach (ItemDescriptor item in _items)
        {
            if (item == itemToAdd)
            {
                return true;
            }
        }
        return false;
    }
}
