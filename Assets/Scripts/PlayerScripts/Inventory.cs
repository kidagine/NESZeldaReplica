using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemSlot _itemSlot = default;
    [SerializeField] private GameObject _itemsInventory = default;
    [SerializeField] private GameObject _sword = default;
    private readonly List<ItemDescriptor> _items = new List<ItemDescriptor>();
    private InventorySlot[] _inventorySlots;


    private void Start()
    {
        _inventorySlots = _itemsInventory.GetComponentsInChildren<InventorySlot>();
    }

    public void Add(ItemDescriptor item)
    {
        if (item.isPassive)
        {
            if (item.itemType == ItemType.WhiteSword || item.itemType == ItemType.MagicalSword)
            {
                SpriteRenderer swordSpriteRenderer = _sword.GetComponent<SpriteRenderer>();
                swordSpriteRenderer.sprite = item.icon;
                bool itemExists = CheckItemExists(item);
                if (!itemExists)
                {
                    _items.Add(item);
                }
                UpdateInventoryUI(itemExists);
            }
            else if (item.itemType == ItemType.BlueRing || item.itemType == ItemType.RedRing)
            {
                if (item.itemType == ItemType.RedRing)
                {
                    Camera.main.GetComponent<LinkPaletteSwap>().SetArmor(ArmorType.Red);
                }
                else if (!CheckPassiveItem(ItemType.RedRing))
                {
                    Camera.main.GetComponent<LinkPaletteSwap>().SetArmor(ArmorType.Blue);
                }
                bool itemExists = CheckItemExists(item);
                if (!itemExists)
                {
                    _items.Add(item);
                }
                UpdateInventoryUI(itemExists);
            }
        }
        else
        {
            if (item.isConsumamble)
            {
                item.consumambleUses++;
            }

            else
            {
                bool itemExists = CheckItemExists(item);
                if (!itemExists)
                {
                    _items.Add(item);
                }
                UpdateInventoryUI(itemExists);
            }
        }
    }

    public void UseConsumamble(ItemDescriptor item)
    {
        item.consumambleUses--;
        _inventorySlots[0].IncrementItemCounter(item);
        if (item.consumambleUses == 0)
        {
            _itemSlot.EmptyItemSlot();
            _items.Remove(item);
        }
        UpdateInventoryUI();
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

    public bool CheckPassiveItem(ItemType itemType)
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (i < _items.Count)
            {
                if (_inventorySlots[i].GetItem().isPassive)
                {
                    if (_inventorySlots[i].GetItem().itemType == itemType)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
