using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemSlot _itemSlot = default;
    [SerializeField] private Image _itemIcon = default;
    private ItemDescriptor _item;


    public void AddItem(ItemDescriptor item)
    {
        _item = item;
        _itemIcon.sprite = _item.icon;
        _itemIcon.enabled = true;
    }

    public void SetCurrentlyUsedItem()
    {
        if (_item != null)
        {
            _itemSlot.SetItemSlot(_item);
        }
    }
}
