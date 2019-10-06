using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemSlot _itemSlot = default;
    [SerializeField] private Text _itemCounter = default;
    [SerializeField] private Image _itemIcon = default;
    private ItemDescriptor _item;


    public void AddItem(ItemDescriptor item)
    {
        _item = item;
        _itemIcon.sprite = _item.icon;
        _itemIcon.enabled = true;
        if (item.isConsumamble)
        {
            _itemCounter.enabled = true;
            _itemCounter.text = item.consumambleUses.ToString();
        }
    }

    public ItemDescriptor GetItem()
    {
        return _item;
    }

    public void IncrementItemCounter(ItemDescriptor item)
    {
        _itemCounter.text = item.consumambleUses.ToString();
    }

    public void ClearSlot()
    {
        _item = null;
        _itemIcon.sprite = null;
        _itemIcon.enabled = false;
        _itemCounter.enabled = false;
    }

    public void SetCurrentlyUsedItem()
    {
        if (_item != null)
        {
            _itemSlot.SetItemSlot(_item);
        }
    }
}
