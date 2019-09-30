using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    private ItemDescriptor _item;
    public void AddItem(ItemDescriptor newItem)
    {
        _item = newItem;

        _itemIcon.sprite = _item.icon;
        _itemIcon.enabled = true;
    }
}
