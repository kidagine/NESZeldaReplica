using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _itemIcon = default;
    private ItemDescriptor _item;


    public void SetItemSlot(ItemDescriptor item)
    {
        _item = item;
        _itemIcon.sprite = item.icon;
        _itemIcon.enabled = true;
    }

    public ItemDescriptor GetItemSlot()
    {
        return _item;
    }

    public void EmptyItemSlot()
    {
        _item = null;
        _itemIcon.sprite = null;
        _itemIcon.enabled = false;
    }
}
